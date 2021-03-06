﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using ANT_Managed_Library;
using AntPlus.Profiles.BikeCadence;
using AntPlus.Profiles.BikePower;
using AntPlus.Profiles.BikeSpeed;
using AntPlus.Profiles.Components;
using AntPlus.Types;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace GarminSensorsDisplayGUI
{
    public partial class Form1 : Form
    {
        static readonly byte CHANNEL_TYPE_INVALID = 2;

        static readonly byte USER_ANT_CHANNEL_CADENZA = 0;  // ANT Channel to use
        static readonly byte USER_ANT_CHANNEL_VELOCITA = 1;
        static readonly ushort USER_DEVICENUM = 0;        // Device number    
        static readonly byte USER_DEVICETYPE_CADENZA = 122;
        static readonly byte USER_DEVICETYPE_VELOCITA = 123;
        static readonly byte USER_DEVICETYPE_POWER = 11;  // Device type     ---- 122 per cadenza, 123 per velocità, 11 per potenza
        static readonly byte USER_TRANSTYPE = 0;              // ----- lasciare sempre 0 per ricerca dei sensori
        static readonly byte USER_ANT_CHANNEL_POWER = 3;         // ANT Channel to use 
                

        static readonly byte USER_RADIOFREQ = 57;          // RF Frequency + 2400 MHz
        static readonly ushort USER_CHANNELPERIOD_CADENZA = 8102;   
        static readonly ushort USER_CHANNELPERIOD_VELOCITA = 8118;
        static readonly ushort USER_CHANNELPERIOD_POWER = 8182;

        static readonly byte[] USER_NETWORK_KEY = { 0xB9, 0xA5, 0x21, 0xFB, 0xBD, 0x72, 0xC3, 0x45 };
        static readonly byte USER_NETWORK_NUM_CADENZA = 0;         // The network key is assigned to this network number-----aggiunto _CADENZA per inizializzare singolo canale
        static readonly byte USER_NETWORK_NUM_VELOCITA = 1;         //idem
        static readonly byte USER_NETWORK_NUM_POWER = 0;

      
        static BikePowerOnlySensor virtualPowerSensor;

        static Network network = new Network(0, USER_NETWORK_KEY, USER_RADIOFREQ);


        static ANT_Device device0;
        static ANT_Channel channel0;
        static ANT_Channel channel1;
        static ANT_Channel channel2;
        static ANT_ReferenceLibrary.ChannelType channelTypeCadenza;
        static ANT_ReferenceLibrary.ChannelType channelTypeVelocita;
        static ANT_ReferenceLibrary.ChannelType channelTypePower;
        static bool bDisplay;
        static bool bBroadcasting;
        static int iIndex = 0;

        static int ultimoEventoCadenza = 0;
        static int ultimoNumeroGiriCadenza = 0;
        static double ultimaCadenzaValida = 0;
        static int pagineCadenzaRicevute = 0;
        static double cadenzaUltimoGruppo = 0;
        static bool inMovimentoCadenza = false;
        static double cadenza;

        static double velocita;
        static int ultimoEventoVelocita = 0;
        static int ultimoNumeroGiriVelocita = 0;
        static double ultimaVelocitaValida = 0;
        static int pagineVelocitaRicevute = 0;
        static double VelocitaUltimoGruppo = 0;

        static double WhellCircumference = (double)28 * 2.54 * 10 * Math.PI;  //circumference of a 28 inch whell, used to calulate speed;

        static BackgroundWorker worker;
        static BackgroundWorker workerDebug;
        delegate void BindTextBoxControlValue(double output, int selector);
        delegate void BindTextBoxControlError(string message,int selector);

        bool isMoving;

        double resistanceValue;
        public Form1()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            workerDebug = new BackgroundWorker();

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            
            
            workerDebug.DoWork += workerDebug_DoWork;

            loadTrainers();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main();

            if (listViewTrainers.SelectedItems.Count > 0)
            {
                button2.Enabled = false;
                button3.Enabled = true;
                loadTrainersSpeeds();
                listViewTrainers.Enabled = false;

                trainerSpeeds.Value++;  //faking a number change to update of the resistance level textbox
                trainerSpeeds.Value--;
            }
            else
            {
                MessageBox.Show("SELECT A TRAINER!!!");
            }

            

            

        }
        
        void Main()
        {
            byte ucChannelType = CHANNEL_TYPE_INVALID;

            

            textBoxGeneralLog.Text = "main";

            try
            {
                Init();
                Start(ucChannelType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Demo failed with exception: \n" + ex.Message);
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            
        }
        ////////////////////////////////////////////////////////////////////////////////
        // Init
        //
        // Initialize demo parameters.
        //
        ////////////////////////////////////////////////////////////////////////////////
        void Init()
        {
            textBoxGeneralLog.Text = "init";
            try
            {

                Console.WriteLine("Attempting to connect to an ANT USB device...");
                device0 = new ANT_Device();   // Create a device instance using the automatic constructor (automatic detection of USB device number and baud rate)
                device0.deviceResponse += new ANT_Device.dDeviceResponseHandler(DeviceResponse);    // Add device response function to receive protocol event messages


                channel1 = device0.getChannel(USER_ANT_CHANNEL_VELOCITA);    // Get channel from ANT device
                channel1.channelResponse += new dChannelResponseHandler(ChannelResponseVelocita);  // Add channel response function to receive channel event messages

                channel0 = device0.getChannel(USER_ANT_CHANNEL_CADENZA);
                channel0.channelResponse += new dChannelResponseHandler(ChannelResponseCadenza);

                channel2 = device0.getChannel(USER_ANT_CHANNEL_POWER);    // Get channel from ANT device
                channel2.channelResponse += new dChannelResponseHandler(ChannelResponsePower);  // Add channel response function to receive channel event messages

                textBoxGeneralLog.Text = "cunfigurazione completata";

            }
            catch (Exception ex)
            {
                if (device0 == null)    // Unable to connect to ANT
                {
                    throw new Exception("Could not connect to any device.\n" +
                    "Details: \n   " + ex.Message);
                    Console.WriteLine("cunfigurazione  non completata");
                }
                else
                {
                    throw new Exception("Error connecting to ANT: " + ex.Message);
                    Console.WriteLine("cunfigurazione  non completata");
                }

            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        // Start
        //
        // Start the demo program.
        // 
        // ucChannelType_:  ANT Channel Type. 0 = Master, 1 = Slave
        //                  If not specified, 2 is passed in as invalid.
        ////////////////////////////////////////////////////////////////////////////////
        void Start(byte ucChannelType_)
        {
            byte ucChannelTypeCadenza = ucChannelType_;
            byte ucChannelTypeVelocita = ucChannelType_;
            
            bDisplay = true;
            bBroadcasting = false;


            textBoxGeneralLog.Text = "start";
            
            /*
            ucChannelTypeCadenza = byte.Parse(textBoxChannelTypeCadenza.Text);
            ucChannelTypeVelocita = byte.Parse(textBoxChannelTypeVelocita.Text);*/
            try
            {
                ConfigureANT(USER_NETWORK_NUM_VELOCITA, channelTypeVelocita, USER_DEVICETYPE_VELOCITA, USER_CHANNELPERIOD_VELOCITA, channel1);
                ConfigureANT(USER_NETWORK_NUM_CADENZA, channelTypeCadenza, USER_DEVICETYPE_CADENZA, USER_CHANNELPERIOD_CADENZA, channel0);
                
                ConfigureVirtualPowerSensor();

                Console.WriteLine("configurazione completata");

                
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Demo failed: " + ex.Message + Environment.NewLine);
            }
           
        }

        ////////////////////////////////////////////////////////////////////////////////
        // ConfigureANT
        //
        // Resets the system, configures the ANT channel and starts the demo
        ////////////////////////////////////////////////////////////////////////////////
        void ConfigureANT(byte USER_NETWORK_NUM, ANT_ReferenceLibrary.ChannelType channelType, byte USER_DEVICETYPE, ushort USER_CHANNELPERIOD, ANT_Channel channel)
        {
            textBoxGeneralLog.Text = "configure";

            Console.WriteLine("Resetting module...");
            //  device0.ResetSystem();     // Soft reset
            System.Threading.Thread.Sleep(500);    // Delay 500ms after a reset

            // If you call the setup functions specifying a wait time, you can check the return value for success or failure of the command
            // This function is blocking - the thread will be blocked while waiting for a response.
            // 500ms is usually a safe value to ensure you wait long enough for any response
            // If you do not specify a wait time, the command is simply sent, and you have to monitor the protocol events for the response,
            Console.WriteLine("Setting network key...");
            if (device0.setNetworkKey(USER_NETWORK_NUM, USER_NETWORK_KEY, 500))
                Console.WriteLine("Network key set");
            else
                throw new Exception("Error configuring network key");

            Console.WriteLine("Assigning channel...");
            if (channel.assignChannel(channelType, USER_NETWORK_NUM, 500))
                Console.WriteLine("Channel assigned");
            else
                throw new Exception("Error assigning channel");

            Console.WriteLine("Setting Channel ID...");
            if (channel.setChannelID(USER_DEVICENUM, false, USER_DEVICETYPE, USER_TRANSTYPE, 500))  // Not using pairing bit
                Console.WriteLine("Channel ID set");
            else
                throw new Exception("Error configuring Channel ID");

            Console.WriteLine("Setting Radio Frequency...");
            if (channel.setChannelFreq(USER_RADIOFREQ, 500))
                Console.WriteLine("Radio Frequency set");
            else
                throw new Exception("Error configuring Radio Frequency");

            Console.WriteLine("Setting Channel Period...");
            if (channel.setChannelPeriod(USER_CHANNELPERIOD, 500))
               Console.WriteLine("Channel Period set");
            else
                throw new Exception("Error configuring Channel Period");

            Console.WriteLine("Opening channel...");
            bBroadcasting = true;
            if (channel.openChannel(500))
            {
                Console.WriteLine("Channel opened");
            }
            else
            {
                bBroadcasting = false;
                throw new Exception("Error opening channel");
            }


           
        }

        private static void ConfigureVirtualPowerSensor()
        {

           
            System.Threading.Thread.Sleep(500);    // Delay 500ms after a reset

            Console.WriteLine("Setting network key...");
            if (device0.setNetworkKey(USER_NETWORK_NUM_POWER, USER_NETWORK_KEY, 500))
                Console.WriteLine("Network key set");
            else
                throw new Exception("Error configuring network key");

            Console.WriteLine("Setting Channel ID...");
            if (channel2.setChannelID(USER_DEVICENUM, false, USER_DEVICETYPE_POWER, USER_TRANSTYPE, 500))  // Not using pairing bit
                Console.WriteLine("Channel ID set");
            else
                throw new Exception("Error configuring Channel ID");

            virtualPowerSensor = new BikePowerOnlySensor(channel2, network);
            virtualPowerSensor.TurnOn();
            
        }
        void ChannelResponseVelocita(ANT_Response response)
        {
            try
            {
                switch ((ANT_ReferenceLibrary.ANTMessageID)response.responseID)
                {
                    case ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40:
                        {
                            switch (response.getChannelEventCode())
                            {
   

                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_SEARCH_TIMEOUT_0x01:
                                    {
                                        Console.WriteLine("Search Timeout");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_0x02:
                                    {
                                        workerDebug.RunWorkerAsync(argument: new object[] { "RX Failed", 1 });
                                        break;
                                    }

                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_CLOSED_0x07:
                                    {
                                        // This event should be used to determine that the channel is closed.
                                        Console.WriteLine("Channel Closed");
                                        Console.WriteLine("Unassigning Channel...");
                                        if (channel1.unassignChannel(500))
                                        {
                                            Console.WriteLine("Unassigned Channel");
                                            Console.WriteLine("Press enter to exit");
                                            
                                        }
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_GO_TO_SEARCH_0x08:
                                    {
                                        Console.WriteLine("Go to Search");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_COLLISION_0x09:
                                    {
                                        Console.WriteLine("Channel Collision");
                                        break;
                                    }

                                default:
                                    {
                                        Console.WriteLine("Unhandled Channel Event " + response.getChannelEventCode());
                                        break;
                                    }
                            }
                            break;
                        }
                    case ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E:
                    case ANT_ReferenceLibrary.ANTMessageID.ACKNOWLEDGED_DATA_0x4F:
                    case ANT_ReferenceLibrary.ANTMessageID.BURST_DATA_0x50:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_BROADCAST_DATA_0x5D:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_ACKNOWLEDGED_DATA_0x5E:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_BURST_DATA_0x5F:
                        {
                            if (true)
                            {
                                if (response.isExtended()) // Check if we are dealing with an extended message
                                {
                                    ANT_ChannelID chID = response.getDeviceIDfromExt();    // Channel ID of the device we just received a message from
                                    Console.Write("Chan ID(" + chID.deviceNumber.ToString() + "," + chID.deviceTypeID.ToString() + "," + chID.transmissionTypeID.ToString() + ") - ");
                                }
                                if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E
                                    || response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.EXT_BROADCAST_DATA_0x5D)
                                    Console.Write("Rx:(" + response.antChannel.ToString() + "): ");    //questo è quello importante
                                else if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.ACKNOWLEDGED_DATA_0x4F
                                    || response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.EXT_ACKNOWLEDGED_DATA_0x5E)
                                    Console.Write("Acked Rx:(" + response.antChannel.ToString() + "): ");
                                else
                                    Console.Write("Burst(" + response.getBurstSequenceNumber().ToString("X2") + ") Rx:(" + response.antChannel.ToString() + "): ");

                                isMoving = checkMovement(response.getDataPayload());
                               
                                velocita = calcolaVelocita(response.getDataPayload());

                                if (!worker.IsBusy)
                                {
                                    worker.RunWorkerAsync(argument: new object[] { velocita, 1 });
                                    Thread.Sleep(100);  //added this to avoid calling the worker while its busy. yeah, i know, its not an elegant solution but this is what i can do at the moment :)
                                }
                                    
                                
                                workerDebug.RunWorkerAsync(argument: new object[] { "RX Success", 1 });

                                setPower(calcolaVelocita(response.getDataPayload()));
                                
                            }

                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Unknown Message " + response.responseID);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Channel response processing failed with exception: " + ex.Message);
            }
        }

        void ChannelResponseCadenza(ANT_Response response)
        {
            try
            {
                switch ((ANT_ReferenceLibrary.ANTMessageID)response.responseID)
                {
                    case ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40:
                        {
                            switch (response.getChannelEventCode()) //identifica che tipo di messaggio è stato ricevuto
                            {
                                  
                                
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_SEARCH_TIMEOUT_0x01:
                                    {
                                        workerDebug.RunWorkerAsync(argument: new object[] { "Cadence sensor timeout", 0 });
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_0x02:
                                    {
                                        workerDebug.RunWorkerAsync(argument: new object[] { "RX Failed", 0 });
                                        pagineCadenzaRicevute++;
                                        break;
                                    }
                                
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_CLOSED_0x07:
                                    {
                                       
                                        channel0.unassignChannel(500);
                                        workerDebug.RunWorkerAsync(argument: new object[] { "channel closed", 0 });
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_GO_TO_SEARCH_0x08:
                                    {
                                        workerDebug.RunWorkerAsync(argument: new object[] { "Serching", 0 });
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_COLLISION_0x09:
                                    {
                                        workerDebug.RunWorkerAsync(argument: new object[] { "Channel collision", 0 });
                                        pagineCadenzaRicevute++;
                                        break;
                                    }
                               
                                default:
                                    {
                                       // Console.WriteLine("Unhandled Channel Event " + response.getChannelEventCode());
                                        workerDebug.RunWorkerAsync(argument: new object[] { "Unhandled Channel Event " + response.getChannelEventCode(), 0 });
                                        break;
                                    }
                            }
                            break;
                        }
                    case ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E:
                        {

                            //Console.Write(BitConverter.ToString(response.getDataPayload()) + " cadenza:  " + cadenza + " inmovinento: " + inMovimentoCadenza + Environment.NewLine);  // Display data payload

                            if (!worker.IsBusy)
                            {
                                worker.RunWorkerAsync(argument: new object[] { calcolaCadenza(response.getDataPayload()), 0 });
                                workerDebug.RunWorkerAsync(argument: new object[] { "RX Success", 0 }); //i made this to sent ant integer used to select the ui element to change without creating other backgrouworkers
                                                                                                        //pattern {message,selector} (selecort: 0=cadence; 1=speed; 2=power; 3=hr);
                                Thread.Sleep(150);
                            }





                            break;
                        }
                    default:
                        {
                            textBoxLogCadence.Text= ("Unknown Message " + response.responseID);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Channel response processing failed with exception: " + ex.Message);
            }
        }

        static void ChannelResponsePower(ANT_Response response)
        {
            

            try
            {
                switch ((ANT_ReferenceLibrary.ANTMessageID)response.responseID)
                {
                    case ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40:
                        {
                            switch (response.getChannelEventCode())
                            {
                                // This event indicates that a message has just been
                                // sent over the air. We take advantage of this event to set
                                // up the data for the next message period.   
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TX_0x03:
                                    {
                                        
                                        workerDebug.RunWorkerAsync(argument: new object[] { "pacchetto potenza inviato con successo", 2 });
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_SEARCH_TIMEOUT_0x01:
                                    {
                                        Console.WriteLine("Search Timeout");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_0x02:
                                    {
                                        Console.WriteLine("Rx Fail");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TRANSFER_RX_FAILED_0x04:
                                    {
                                        Console.WriteLine("Burst receive has failed");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TRANSFER_TX_COMPLETED_0x05:
                                    {
                                        Console.WriteLine("Transfer Completed");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TRANSFER_TX_FAILED_0x06:
                                    {
                                        Console.WriteLine("Transfer Failed");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_CLOSED_0x07:
                                    {
                                        // This event should be used to determine that the channel is closed.
                                        Console.WriteLine("Channel Closed");
                                        Console.WriteLine("Unassigning Channel...");
                                        if (channel2.unassignChannel(500))
                                        {
                                            Console.WriteLine("Unassigned Channel");
                                            Console.WriteLine("Press enter to exit");
                                            
                                        }
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_GO_TO_SEARCH_0x08:
                                    {
                                        Console.WriteLine("Go to Search");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_CHANNEL_COLLISION_0x09:
                                    {
                                        Console.WriteLine("Channel Collision banana");
                                        break;
                                    }
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TRANSFER_TX_START_0x0A:
                                    {
                                        Console.WriteLine("Burst Started");
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Unhandled Channel Event " + response.getChannelEventCode());
                                        break;
                                    }
                            }
                            break;
                        }
                    case ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E:
                    case ANT_ReferenceLibrary.ANTMessageID.ACKNOWLEDGED_DATA_0x4F:
                    case ANT_ReferenceLibrary.ANTMessageID.BURST_DATA_0x50:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_BROADCAST_DATA_0x5D:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_ACKNOWLEDGED_DATA_0x5E:
                    case ANT_ReferenceLibrary.ANTMessageID.EXT_BURST_DATA_0x5F:
                        {
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Unknown Message " + response.responseID);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Channel response processing failed with exception: " + ex.Message);
            }
        }
        void DeviceResponse(ANT_Response response)
        {
            switch ((ANT_ReferenceLibrary.ANTMessageID)response.responseID)
            {
                case ANT_ReferenceLibrary.ANTMessageID.STARTUP_MESG_0x6F:
                    {
                        Console.Write("RESET Complete, reason: ");

                        byte ucReason = response.messageContents[0];

                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_POR_0x00)
                            Console.WriteLine("RESET_POR");
                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_RST_0x01)
                            Console.WriteLine("RESET_RST");
                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_WDT_0x02)
                            Console.WriteLine("RESET_WDT");
                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_CMD_0x20)
                            Console.WriteLine("RESET_CMD");
                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_SYNC_0x40)
                            Console.WriteLine("RESET_SYNC");
                        if (ucReason == (byte)ANT_ReferenceLibrary.StartupMessage.RESET_SUSPEND_0x80)
                            Console.WriteLine("RESET_SUSPEND");
                        break;
                    }
                case ANT_ReferenceLibrary.ANTMessageID.VERSION_0x3E:
                    {
                        Console.WriteLine("VERSION: " + new ASCIIEncoding().GetString(response.messageContents));
                        break;
                    }
                case ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40:
                    {
                        switch (response.getMessageID())
                        {
                            case ANT_ReferenceLibrary.ANTMessageID.CLOSE_CHANNEL_0x4C:
                                {
                                    if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.CHANNEL_IN_WRONG_STATE_0x15)
                                    {
                                        Console.WriteLine("Channel is already closed");
                                        Console.WriteLine("Unassigning Channel...");
                                        if (channel0.unassignChannel(500))
                                        {
                                            Console.WriteLine("Unassigned Channel");
                                            Console.WriteLine("Press enter to exit");
                                           // bDone = true;
                                        }
                                    }
                                    break;
                                }
                            case ANT_ReferenceLibrary.ANTMessageID.NETWORK_KEY_0x46:
                            case ANT_ReferenceLibrary.ANTMessageID.ASSIGN_CHANNEL_0x42:
                            case ANT_ReferenceLibrary.ANTMessageID.CHANNEL_ID_0x51:
                            case ANT_ReferenceLibrary.ANTMessageID.CHANNEL_RADIO_FREQ_0x45:
                            case ANT_ReferenceLibrary.ANTMessageID.CHANNEL_MESG_PERIOD_0x43:
                            case ANT_ReferenceLibrary.ANTMessageID.OPEN_CHANNEL_0x4B:
                            case ANT_ReferenceLibrary.ANTMessageID.UNASSIGN_CHANNEL_0x41:
                                {
                                    if (response.getChannelEventCode() != ANT_ReferenceLibrary.ANTEventID.RESPONSE_NO_ERROR_0x00)
                                    {
                                        Console.WriteLine(String.Format("Error {0} configuring {1}", response.getChannelEventCode(), response.getMessageID()));
                                    }
                                    break;
                                }
                            case ANT_ReferenceLibrary.ANTMessageID.RX_EXT_MESGS_ENABLE_0x66:
                                {
                                    if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.INVALID_MESSAGE_0x28)
                                    {
                                        Console.WriteLine("Extended messages not supported in this ANT product");
                                        break;
                                    }
                                    else if (response.getChannelEventCode() != ANT_ReferenceLibrary.ANTEventID.RESPONSE_NO_ERROR_0x00)
                                    {
                                        Console.WriteLine(String.Format("Error {0} configuring {1}", response.getChannelEventCode(), response.getMessageID()));
                                        break;
                                    }
                                    Console.WriteLine("Extended messages enabled");
                                    break;
                                }
                            case ANT_ReferenceLibrary.ANTMessageID.REQUEST_0x4D:
                                {
                                    if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.INVALID_MESSAGE_0x28)
                                    {
                                        Console.WriteLine("Requested message not supported in this ANT product");
                                        break;
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Unhandled response " + response.getChannelEventCode() + " to message " + response.getMessageID()); break;
                                }
                        }
                        break;
                    }
            }
        }


        double calcolaCadenza(byte[] response)
        {
            int cadenzaCount, cadenzaTimeEvent;
            double cadenza = -1;

            pagineCadenzaRicevute++;

            cadenzaCount = response[7] * 256 + response[6];
            cadenzaTimeEvent = response[5] * 256 + response[4];

            //Console.Write("cadenzaCount: " + (cadenzaCount) + " tempoEvento: " + cadenzaTimeEvent + "ultimoNumerogiri: " + ultimoNumeroGiriCadenza + "ultimo evento: " + ultimoEventoCadenza + "  ");
            if (ultimoEventoCadenza - cadenzaTimeEvent != 0)
            {
                cadenza = (double)(60 * (cadenzaCount - ultimoNumeroGiriCadenza) * 1024) / (cadenzaTimeEvent - ultimoEventoCadenza);
                inMovimentoCadenza = true;
            }

            if (cadenza >= 0)
                ultimaCadenzaValida = cadenza;

            if (pagineCadenzaRicevute >= 4)
            {
                cadenzaUltimoGruppo = cadenza;
                inMovimentoCadenza = false;
                pagineCadenzaRicevute = 0;
                cadenza = ultimaCadenzaValida;
            }

            if (cadenza == cadenzaUltimoGruppo && inMovimentoCadenza == false && cadenzaUltimoGruppo != 0)
                cadenza = 0;
            else
            {
                cadenza = ultimaCadenzaValida;
                inMovimentoCadenza = true;
            }


            ultimoEventoCadenza = cadenzaTimeEvent;

            ultimoNumeroGiriCadenza = cadenzaCount;

            
           

            if (cadenza >= 0)
                return cadenza;
            else
                return ultimaCadenzaValida;


        }

        double calcolaVelocita(byte[] response)
        {
            int velocitaCount, velocitaTimeEvent;
            double velocitaTmp = -1;

            pagineVelocitaRicevute++;

            velocitaCount = response[7] * 256 + response[6];
            velocitaTimeEvent = response[5] * 256 + response[4];

            
            if (ultimoEventoVelocita - velocitaTimeEvent != 0)
            {
                velocitaTmp = (double)(WhellCircumference * (velocitaCount - ultimoNumeroGiriVelocita) / (velocitaTimeEvent - ultimoEventoVelocita)) * 3.6; //calculates istantaneus speed in km/s
                inMovimentoCadenza = true;
            }

            if (velocitaTmp >= 0)
                ultimaVelocitaValida = velocitaTmp;

            if (!isMoving)
                velocitaTmp = 0;

            ultimoEventoVelocita = velocitaTimeEvent;

            ultimoNumeroGiriVelocita = velocitaCount;




            if (velocitaTmp >= 0)
                return velocitaTmp;
            else
                return ultimaVelocitaValida;
            

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] tmp = (object[])e.Argument;
            double output = (double)tmp[0];
            int selector = (int)tmp[1];

            e.Result = output;
            this.Invoke(new BindTextBoxControlValue(UpdateTextboxValue), new object[] { output, selector});
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

  
        }
        void UpdateTextboxValue(double output, int selector)
        {
            switch (selector)
            {
                case 0:
                    textBoxCadenza.Text = output.ToString();
                    break;
                case 1:
                    textBoxVelocita.Text = output.ToString();
                    break;
                case 2:
                    textBoxPower.Text = output.ToString();
                    break;
            }
        }
        private void workerDebug_DoWork(object sender, DoWorkEventArgs e)
        {
 
            object[] tmp = (object[])e.Argument;
            string message = (string)tmp[0];
            int selector = (int)tmp[1];
            this.Invoke(new BindTextBoxControlError(UpdateTextboxErrorMessage), new object[] { message,selector });
        }
        void UpdateTextboxErrorMessage(string message, int selector)
        {

            switch (selector)
            {
                case 0:
                    textBoxLogCadence.Text = message;
                    break;
                case 1:
                    textBoxLogSpeed.Text = message;
                    break;
                case 2:
                    textBoxLogPower.Text = message;
                    break;
            }
            
        }
        void setPower(double newSpeed)
        {
            ushort power = Convert.ToUInt16(newSpeed * resistanceValue);
            virtualPowerSensor.InstantaneousPower = Convert.ToUInt16(newSpeed * resistanceValue);

            if (!worker.IsBusy)
                worker.RunWorkerAsync(argument: new object[] { Convert.ToDouble(power), 2 });
        }
        bool checkMovement(byte[] response)
        {
            if (response[1] == 0)
                return true;
            else
                return false;
        }

        void loadTrainers()     //this just load all the trainers in the listview
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("trainersList.xml");

            foreach(XmlNode node in doc.DocumentElement)        //still experimenting with better ways to check values in xml nodes (i'm a noob :) )
            {
                listViewTrainers.Items.Add(node.Attributes[0].InnerText + " " + node.Attributes[1].InnerText);
            }

        }

        void loadTrainersSpeeds()       //this changes the maximun number in the resistence level selector
        {

                string tmp = listViewTrainers.SelectedItems[0].Text;

                XmlDocument doc = new XmlDocument();
                doc.Load("trainersList.xml");

                foreach (XmlNode node in doc.DocumentElement)
                {
                    if (tmp == node.Attributes[0].InnerText + " " + node.Attributes[1].InnerText)
                    {
                        foreach(XmlNode childNode in node.ChildNodes)
                        {
                            
                            trainerSpeeds.Maximum = Int16.Parse(childNode.Attributes[0].Value);
                        }
                    }
                }



        }
        private void button3_Click(object sender, EventArgs e) //pulsante fine
        {
            button2.Enabled = true;
            button3.Enabled = false;
            MessageBox.Show("SESSIONE INTERROTTA");
           
            ANT_Device.shutdownDeviceInstance(ref device0);  // Close down the device completely and completely shut down all communication
            trainerSpeeds.Maximum = 1;
            
            listViewTrainers.Enabled = true;
        }

        private void buttonExit_Click(object sender, EventArgs e) //exit button
        {
            Application.Exit();
        }

        private void trainerSpeeds_ValueChanged(object sender, EventArgs e)     //this changes the resistance level whenever the value is changed, this can be changed whitout stopping the activity
        {
            string tmp = listViewTrainers.SelectedItems[0].Text;

            XmlDocument doc = new XmlDocument();
            doc.Load("trainersList.xml");

            foreach (XmlNode node in doc.DocumentElement)
            {
                if (tmp == node.Attributes[0].InnerText + " " + node.Attributes[1].InnerText)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {

                        foreach(XmlNode res in childNode.ChildNodes)
                        {
                            if (Int16.Parse(res.Attributes[0].Value) == trainerSpeeds.Value - 1)
                            {
                                textBoxResistanceLevel.Text = res.InnerText;

                                resistanceValue = Convert.ToDouble(textBoxResistanceLevel.Text);
                              
                            }
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            cartesianChart1.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<ObservablePoint>
                        {
                            new ObservablePoint(0,velocita),      //First Point of First Line
                            new ObservablePoint(4,7),       //2nd POint
                            new ObservablePoint(5,3),     //------
                            new ObservablePoint(7,6),
                            new ObservablePoint(10,8)
                        },
                        PointGeometrySize = 0.4
                    },
                    new LineSeries
                    {
                        Title="prova",
                        Values = new ChartValues<ObservablePoint>
                        {
                            new ObservablePoint(0,virtualPowerSensor.InstantaneousPower),      //First Point of First Line
                            new ObservablePoint(1,7),       //2nd POint
                            new ObservablePoint(2,3),     //------
                            new ObservablePoint(3,6),
                            new ObservablePoint(6,8)
                        },
                        PointGeometrySize = 0
                    },



                };

           

        }
        
    }
}



