using System;
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
using ANT_Managed_Library;
using AntPlus.Profiles.BikeCadence;
using AntPlus.Profiles.BikePower;
using AntPlus.Profiles.BikeSpeed;
using AntPlus.Profiles.Components;
using AntPlus.Types;

namespace GarminSensorsDisplayGUI
{
    public partial class Form1 : Form
    {
        static readonly byte CHANNEL_TYPE_INVALID = 2;

        static readonly byte USER_ANT_CHANNEL_CADENZA = 0;  // ANT Channel to use
        static readonly byte USER_ANT_CHANNEL_VELOCITA = 1;
        static readonly ushort USER_DEVICENUM = 0;        // Device number    
        static readonly byte USER_DEVICETYPE_CADENZA = 122;
        static readonly byte USER_DEVICETYPE_VELOCITA = 123;// Device type     ---- 122 per cadenza, 123 per velocità
        static readonly byte USER_TRANSTYPE = 0;           // Transmission type    ----- lasciare sempre 0 per ricerca dei sensori

        static readonly byte USER_RADIOFREQ = 57;          // RF Frequency + 2400 MHz
        static readonly ushort USER_CHANNELPERIOD_CADENZA = 8102;   // Channel Period (8192/32768)s period = 4Hz
        static readonly ushort USER_CHANNELPERIOD_VELOCITA = 8118;

        static readonly byte[] USER_NETWORK_KEY = { 0xB9, 0xA5, 0x21, 0xFB, 0xBD, 0x72, 0xC3, 0x45 };
        static readonly byte USER_NETWORK_NUM_CADENZA = 0;         // The network key is assigned to this network number-----aggiunto _CADENZA per inizializzare singolo canale
        static readonly byte USER_NETWORK_NUM_VELOCITA = 1;         //idem

        static BikeCadenceSensor sensoreCadenza;

        static Network network = new Network(0, USER_NETWORK_KEY, USER_RADIOFREQ);


        static ANT_Device device0;
        static ANT_Channel channel0;
        static ANT_Channel channel1;
        static ANT_ReferenceLibrary.ChannelType channelTypeCadenza;
        static ANT_ReferenceLibrary.ChannelType channelTypeVelocita;
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

        BackgroundWorker worker;
        BackgroundWorker workerDebug;
        delegate void BindTextBoxControlValue(double output, int selector);
        delegate void BindTextBoxControlError(string message,int selector);

        public Form1()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            workerDebug = new BackgroundWorker();

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            
            
            workerDebug.DoWork += workerDebug_DoWork;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main();
           
            
        }
        
        void Main()
        {
            byte ucChannelType = CHANNEL_TYPE_INVALID;

            

            textBoxInitDebug.Text = "main";

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
            textBoxInitDebug.Text = "init";
            try
            {

                Console.WriteLine("Attempting to connect to an ANT USB device...");
                device0 = new ANT_Device();   // Create a device instance using the automatic constructor (automatic detection of USB device number and baud rate)
                device0.deviceResponse += new ANT_Device.dDeviceResponseHandler(DeviceResponse);    // Add device response function to receive protocol event messages


                channel1 = device0.getChannel(USER_ANT_CHANNEL_VELOCITA);    // Get channel from ANT device
                channel1.channelResponse += new dChannelResponseHandler(ChannelResponseVelocita);  // Add channel response function to receive channel event messages

                channel0 = device0.getChannel(USER_ANT_CHANNEL_CADENZA);
                channel0.channelResponse += new dChannelResponseHandler(ChannelResponseCadenza);


                textBoxInitDebug.Text = "cunfigurazione completata";

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


            textBoxInitDebug.Text = "start";
            // If a channel type has not been set at the command line,
            // prompt the user to specify one now
            /* do
             {
                 if (ucChannelTypeCadenza == CHANNEL_TYPE_INVALID) //inserire tipo canale rer sensore di candenza
                 {
                     Console.WriteLine("Channel Type(cadenza)? (Master = 0, Slave = 1)");
                     try
                     {
                         ucChannelTypeCadenza = byte.Parse(Console.ReadLine());
                     }
                     catch (Exception)
                     {
                         ucChannelTypeCadenza = CHANNEL_TYPE_INVALID;
                     }
                 }

                 if (ucChannelTypeVelocita == CHANNEL_TYPE_INVALID)      //inserire tipo canale rer sensore di velocità
                 {
                     Console.WriteLine("Channel Type(velocita)? (Master = 0, Slave = 1)");
                     try
                     {
                         ucChannelTypeVelocita = byte.Parse(Console.ReadLine());
                     }
                     catch (Exception)
                     {
                         ucChannelTypeVelocita = CHANNEL_TYPE_INVALID;
                     }
                 }

                 if (ucChannelTypeCadenza == 0)      //assegnazione indirizzo di ricezione/trasmissione sensore cadenza
                 {
                     channelTypeCadenza = ANT_ReferenceLibrary.ChannelType.BASE_Master_Transmit_0x10;
                 }
                 else if (ucChannelTypeCadenza == 1)
                 {
                     channelTypeCadenza = ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00;
                 }
                 else
                 {
                     ucChannelTypeCadenza = CHANNEL_TYPE_INVALID;
                     Console.WriteLine("Error: Invalid channel type");
                 }

                 if (ucChannelTypeVelocita == 0)     //assegnazione indirizzo di ricezione/trasmissione sensore velocità
                 {
                     channelTypeVelocita = ANT_ReferenceLibrary.ChannelType.BASE_Master_Transmit_0x10;
                 }
                 else if (ucChannelTypeVelocita == 1)
                 {
                     channelTypeVelocita = ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00;
                 }
                 else
                 {
                     ucChannelTypeVelocita = CHANNEL_TYPE_INVALID;
                     Console.WriteLine("Error: Invalid channel type");
                 }

             } while (ucChannelTypeCadenza == CHANNEL_TYPE_INVALID || ucChannelTypeVelocita == CHANNEL_TYPE_INVALID);  //fa un po schifo ma vabbe, cambiare in futuro
             */ //tolto per evitare di fare input per tipo canale da gui

            ucChannelTypeCadenza = byte.Parse(textBoxChannelTypeCadenza.Text);
            ucChannelTypeVelocita = byte.Parse(textBoxChannelTypeVelocita.Text);
            try
            {
                ConfigureANT(USER_NETWORK_NUM_CADENZA, channelTypeCadenza, USER_DEVICETYPE_CADENZA, USER_CHANNELPERIOD_CADENZA, channel0);
                ConfigureANT(USER_NETWORK_NUM_VELOCITA, channelTypeVelocita, USER_DEVICETYPE_VELOCITA, USER_CHANNELPERIOD_VELOCITA, channel1);
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
            textBoxInitDebug.Text = "configure";

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

            if (USER_DEVICETYPE == 122)
                sensoreCadenza = new BikeCadenceSensor(channel, network);  //lasciare stare, crea sensore virtuale

           
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

                                //Console.Write(BitConverter.ToString(response.getDataPayload()) + Environment.NewLine);  // Display data payload
                                velocita = calcolaVelocita(response.getDataPayload());
      
                                worker.RunWorkerAsync(argument: new object[] { velocita, 1 });

                                workerDebug.RunWorkerAsync(argument: new object[] { "RX Success", 1 });
                            }
                            else
                            {
                                string[] ac = { "|", "/", "_", "\\" };
                                Console.Write("Rx: " + ac[iIndex++] + "\r");
                                iIndex &= 3;
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
                                case ANT_ReferenceLibrary.ANTEventID.EVENT_TRANSFER_RX_FAILED_0x04:
                                    {
                                        textBoxDebugCadenza.Text = "Burst receive has failed";
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
                                        Console.WriteLine("Unhandled Channel Event " + response.getChannelEventCode());
                                        break;
                                    }
                            }
                            break;
                        }
                    case ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E:
                        {
                            if (true)
                            {
                                

                                 cadenza = calcolaCadenza(response.getDataPayload());
                                //Console.Write(BitConverter.ToString(response.getDataPayload()) + " cadenza:  " + cadenza + " inmovinento: " + inMovimentoCadenza + Environment.NewLine);  // Display data payload

 
                                worker.RunWorkerAsync(argument: new object[] { cadenza, 0 });
                                
                                workerDebug.RunWorkerAsync(argument: new object[] {"RX Success",0 }); //i made this to sent ant integer used to select the ui element to change without creating other backgrouworkers
                                                                                                      //pattern {message,selector} (selecort: 0=cadence; 1=speed; 2=hr; 3=power)
                                
                            }
                            else
                            {
                                string[] ac = { "|", "/", "_", "\\" };
                                textBoxDebugCadenza.Text = ("Rx: " + ac[iIndex++] + "\r");
                                iIndex &= 3;
                            }
                            break;
                        }
                    default:
                        {
                            textBoxDebugCadenza.Text= ("Unknown Message " + response.responseID);
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

            if (velocita >= 0)
                ultimaVelocitaValida = velocita;

            if (pagineVelocitaRicevute >= 4)
            {
                VelocitaUltimoGruppo = velocita;
                inMovimentoCadenza = false;
                pagineVelocitaRicevute = 0;
                velocita = ultimaVelocitaValida;
            }

            if (velocita == VelocitaUltimoGruppo && inMovimentoCadenza == false && VelocitaUltimoGruppo != 0)
                velocita = 0;
            else
            {
                velocita = ultimaVelocitaValida;
                inMovimentoCadenza = true;
            }


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
                    textBoxDebugCadenza.Text = message;
                    break;
                case 1:
                    textBoxDebugVelocita.Text = message;
                    break;
            }
            
        }
        private void button3_Click(object sender, EventArgs e) //pulsante fine
        {
             MessageBox.Show("SESSIONE INTERROTTA");
           
            ANT_Device.shutdownDeviceInstance(ref device0);  // Close down the device completely and completely shut down all communication
            
        }

        private void buttonExit_Click(object sender, EventArgs e) //pulsante uscita
        {
            Application.Exit();
        }

        
    }
}



