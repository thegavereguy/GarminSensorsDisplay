﻿ /*
This software is subject to the license described in the License.txt file
included with this software distribution. You may not use this file except
in compliance with this license.

Copyright (c) Dynastream Innovations Inc. 2016
All rights reserved.
*/

//////////////////////////////////////////////////////////////////////////
// To use the managed library, you must:
// 1. Import ANT_NET.dll as a reference
// 2. Reference the ANT_Managed_Library namespace
// 3. Include the following files in the working directory of your application:
//  - DSI_CP310xManufacturing_3_1.dll
//  - DSI_SiUSBXp_3_1.dll
//  - ANT_WrappedLib.dll
//  - ANT_NET.dll
//////////////////////////////////////////////////////////////////////////

#define ENABLE_EXTENDED_MESSAGES // Un - coment to enable extended messages

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANT_Managed_Library;
using AntPlus.Profiles.BikeCadence;
using AntPlus.Profiles.BikeSpeed;
using AntPlus.Profiles.Components;
using AntPlus.Types;

namespace ANT_Console_Demo
{
    class demo
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
        static byte[] txBuffer = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static bool bDone;
        static bool bDisplay;
        static bool bBroadcasting;
        static int iIndex = 0;

        static int ultimoEventoCadenza=0;
        static int ultimoNumeroGiriCadenza=0;
        static double ultimaCadenzaValida=0;
        static int pagineCadenzaRicevute = 0;
        static double cadenzaUltimoGruppo = 0;
        static bool inMovimentoCadenza=false;

        ////////////////////////////////////////////////////////////////////////////////
        // Main
        //
        // Usage:
        //
        // c:\demo_net.exe [channel_type]
        //
        // ... where
        // channel_type:  Master = 0, Slave = 1
        //
        // ... example
        //
        // c:\demo_net.exe 0
        // 
        // will connect to an ANT USB stick open a Master channel
        //
        // If optional arguements are not supplied, user will 
        // be prompted to enter these after the program starts
        //
        ////////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            byte ucChannelType = CHANNEL_TYPE_INVALID;

            if (args.Length > 0)
            {
                ucChannelType = byte.Parse(args[0]);
            }
            Console.WriteLine("ciao");

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



        ////////////////////////////////////////////////////////////////////////////////
        // Init
        //
        // Initialize demo parameters.
        //
        ////////////////////////////////////////////////////////////////////////////////
        static void Init()
        {
            try
            {
                Console.WriteLine("Attempting to connect to an ANT USB device...");
                device0 = new ANT_Device();   // Create a device instance using the automatic constructor (automatic detection of USB device number and baud rate)
                device0.deviceResponse += new ANT_Device.dDeviceResponseHandler(DeviceResponse);    // Add device response function to receive protocol event messages
                
                channel1 = device0.getChannel(USER_ANT_CHANNEL_VELOCITA);    // Get channel from ANT device
                channel1.channelResponse += new dChannelResponseHandler(ChannelResponseVelocita);  // Add channel response function to receive channel event messages
                
                channel0 = device0.getChannel(USER_ANT_CHANNEL_CADENZA);    
                channel0.channelResponse += new dChannelResponseHandler(ChannelResponseCadenza);

                
             

                Console.WriteLine("Initialization was successful!");
            }
            catch (Exception ex)
            {
                if (device0 == null)    // Unable to connect to ANT
                {
                    throw new Exception("Could not connect to any device.\n" + 
                    "Details: \n   " + ex.Message);
                }
                else
                {
                    throw new Exception("Error connecting to ANT: " + ex.Message);
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
        static void Start(byte ucChannelType_)
        {
            byte ucChannelTypeCadenza = ucChannelType_;
            byte ucChannelTypeVelocita = ucChannelType_;
            bDone = false;
            bDisplay = true;
            bBroadcasting = false;

            PrintMenu();

            // If a channel type has not been set at the command line,
            // prompt the user to specify one now
            do
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

                if (ucChannelTypeVelocita  == 0)     //assegnazione indirizzo di ricezione/trasmissione sensore velocità
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

            try
            {   
                ConfigureANT(USER_NETWORK_NUM_CADENZA, channelTypeCadenza,USER_DEVICETYPE_CADENZA, USER_CHANNELPERIOD_CADENZA,channel0);
                ConfigureANT(USER_NETWORK_NUM_VELOCITA, channelTypeVelocita, USER_DEVICETYPE_VELOCITA, USER_CHANNELPERIOD_VELOCITA,channel1);
                
               
                while (!bDone)
                {
                    
                    string command = Console.ReadLine();
                    switch (command)
                    {
                        case "M":
                        case "m":
                        {
                            PrintMenu();
                            break;
                        }
                        case "Q":
                        case "q":
                        {
                            // Quit
                            Console.WriteLine("Closing Channel");
                            bBroadcasting = false;
                            channel0.closeChannel();
                            break;
                        }
                        case "A":
                        case "a":
                        {
                            // Send Acknowledged Data
                            byte[] myTxBuffer = { 1, 2, 3, 4, 5, 6, 7, 8 };
                            channel0.sendAcknowledgedData(myTxBuffer);
                            break;
                        }
                        case "B":
                        case "b":
                        {
                            // Send Burst Data (10 packets)
                            byte[] myTxBuffer = new byte[8 * 10];
                            for (byte i = 0; i < 8 * 10; i++)
                                myTxBuffer[i] = i;
                            channel0.sendBurstTransfer(myTxBuffer);
                            break;
                        }
                        /*          non serve al momento
                        case "R":
                        case "r":
                        {
                            // Reset the system and start over the test
                            ConfigureANT();
                            break;
                        }
                        */
                        case "C":
                        case "c":
                        {
                            // Request capabilities
                            ANT_DeviceCapabilities devCapab = device0.getDeviceCapabilities(500);
                            Console.Write(devCapab.printCapabilities() + Environment.NewLine);
                            break;
                        }
                        case "V":
                        case "v":
                        {
                            // Request version
                            // As this is not available in all ANT parts, we should not wait for a response, so
                            // we do not specify a timeout
                            // The response - if available - will be processed in DeviceResponse
                            device0.requestMessage(ANT_ReferenceLibrary.RequestMessageID.VERSION_0x3E);                    
                            break;
                        }
                        case "S":
                        case "s":
                        {
                            // Request channel status
                            ANT_ChannelStatus chStatus = channel0.requestStatus(500);

                            string[] allStatus = { "STATUS_UNASSIGNED_CHANNEL",
                                                    "STATUS_ASSIGNED_CHANNEL",
                                                    "STATUS_SEARCHING_CHANNEL",
                                                    "STATUS_TRACKING_CHANNEL"};
                            Console.WriteLine("STATUS: " + allStatus[(int)chStatus.BasicStatus]);
                            break;
                        }
                        case "I":
                        case "i":
                        {
                            // Request channel ID
                            ANT_Response respChID = device0.requestMessageAndResponse(ANT_ReferenceLibrary.RequestMessageID.CHANNEL_ID_0x51, 500);
                            ushort usDeviceNumber = (ushort) ((respChID.messageContents[2] << 8) + respChID.messageContents[1]);
                            byte ucDeviceType = respChID.messageContents[3];
                            byte ucTransmissionType = respChID.messageContents[4];
                            Console.WriteLine("CHANNEL ID: (" + usDeviceNumber.ToString() + "," + ucDeviceType.ToString() + "," + ucTransmissionType.ToString() + ")");
                            break;
                        }
                        case "D":
                        case "d":
                        {
                            bDisplay = !bDisplay;
                            break;
                        }
			            case "U":
			            case "u":
		                {
				            // Print out information about the device we are connected to
				            Console.WriteLine("USB Device Description");
				            
                            // Retrieve info
                            Console.WriteLine(String.Format("   VID: 0x{0:x}", device0.getDeviceUSBVID()));
                            Console.WriteLine(String.Format("   PID: 0x{0:x}", device0.getDeviceUSBPID()));
                            Console.WriteLine(String.Format("   Product Description: {0}", device0.getDeviceUSBInfo().printProductDescription()));
                            Console.WriteLine(String.Format("   Serial String: {0}", device0.getDeviceUSBInfo().printSerialString()));	
				            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(0);
                }
                // Clean up ANT
                Console.WriteLine("Disconnecting module...");
                ANT_Device.shutdownDeviceInstance(ref device0);  // Close down the device completely and completely shut down all communication
                Console.WriteLine("Demo has completed successfully!");
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
        private static void ConfigureANT(byte USER_NETWORK_NUM, ANT_ReferenceLibrary.ChannelType channelType, byte USER_DEVICETYPE, ushort USER_CHANNELPERIOD, ANT_Channel channel)
        {
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

            if(USER_DEVICETYPE==122)
                sensoreCadenza = new BikeCadenceSensor(channel,network);

/*#if (ENABLE_EXTENDED_MESSAGES)
            // Extended messages are not supported in all ANT devices, so
            // we will not wait for the response here, and instead will monitor 
            // the protocol events
            Console.WriteLine("Enabling extended messages...");
            device0.enableRxExtendedMessages(true);
#endif*/
        }

        ////////////////////////////////////////////////////////////////////////////////
        // ChannelResponse
        //
        // Called whenever a channel event is recieved. 
        // 
        // response: ANT message
        ////////////////////////////////////////////////////////////////////////////////
        static void ChannelResponseVelocita(ANT_Response response)
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
                                txBuffer[0]++;  // Increment the first byte of the buffer

                                // Broadcast data will be sent over the air on
                                // the next message period
                                if (bBroadcasting)
                                {
                                    channel1.sendBroadcastData(txBuffer);
                                    
                                    if (bDisplay)
                                    {
                                        // Echo what the data will be over the air on the next message period
                                        Console.WriteLine("Tx: (" + response.antChannel.ToString() + ")" + BitConverter.ToString(txBuffer) + " - " + BitConverter.ToUInt16(txBuffer,0));
                                    }
                                }
                                else
                                {
                                    string[] ac = { "|", "/", "_", "\\" };
                                    Console.Write("Tx: " + ac[iIndex++] + "\r");
                                    iIndex &= 3;
                                }
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
                                if (channel1.unassignChannel(500))
                                {
                                    Console.WriteLine("Unassigned Channel");
                                    Console.WriteLine("Press enter to exit");
                                    bDone = true;
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
                        if (bDisplay)
                        {
                            if (response.isExtended()) // Check if we are dealing with an extended message
                            {   
                                ANT_ChannelID chID = response.getDeviceIDfromExt();    // Channel ID of the device we just received a message from
                                Console.Write("Chan ID(" + chID.deviceNumber.ToString() + "," + chID.deviceTypeID.ToString() + "," + chID.transmissionTypeID.ToString() + ") - ");
                            }
                            if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.BROADCAST_DATA_0x4E 
                                || response.responseID == (byte) ANT_ReferenceLibrary.ANTMessageID.EXT_BROADCAST_DATA_0x5D)
                                Console.Write("Rx:(" + response.antChannel.ToString() + "): ");    //questo è quello importante
                            else if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.ACKNOWLEDGED_DATA_0x4F
                                || response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.EXT_ACKNOWLEDGED_DATA_0x5E)
                                Console.Write("Acked Rx:(" + response.antChannel.ToString() + "): ");
                            else
                                Console.Write("Burst(" + response.getBurstSequenceNumber().ToString("X2") + ") Rx:(" + response.antChannel.ToString() + "): ");

                          //  Console.Write(BitConverter.ToString(response.getDataPayload()) + Environment.NewLine);  // Display data payload
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

        static void ChannelResponseCadenza(ANT_Response response)
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
                                        txBuffer[0]++;  // Increment the first byte of the buffer

                                        // Broadcast data will be sent over the air on
                                        // the next message period
                                        if (bBroadcasting)
                                        {
                                            channel0.sendBroadcastData(txBuffer);

                                            if (bDisplay)
                                            {
                                                // Echo what the data will be over the air on the next message period
                                                Console.WriteLine("Tx: (" + response.antChannel.ToString() + ")" + BitConverter.ToString(txBuffer));
                                            }
                                        }
                                        else
                                        {
                                            string[] ac = { "|", "/", "_", "\\" };
                                            Console.Write("Tx: " + ac[iIndex++] + "\r");
                                            iIndex &= 3;
                                        }
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
                                        pagineCadenzaRicevute++;
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
                                        if (channel0.unassignChannel(500))
                                        {
                                            Console.WriteLine("Unassigned Channel");
                                            Console.WriteLine("Press enter to exit");
                                            bDone = true;
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
                                        pagineCadenzaRicevute++;
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
                            if (bDisplay)
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

                                //byte[] CadenzaRaw = response.getDataPayload();

                                Console.Write(BitConverter.ToString(response.getDataPayload()) + " cadenza:  " + calcolaCadenza(response.getDataPayload()) + " inmovinento: " + inMovimentoCadenza +  Environment.NewLine);  // Display data payload
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

        ////////////////////////////////////////////////////////////////////////////////
        // DeviceResponse
        //
        // Called whenever a message is received from ANT unless that message is a 
        // channel event message. 
        // 
        // response: ANT message
        ////////////////////////////////////////////////////////////////////////////////
        static void DeviceResponse(ANT_Response response)
        {
            switch ((ANT_ReferenceLibrary.ANTMessageID) response.responseID)
            {
                case ANT_ReferenceLibrary.ANTMessageID.STARTUP_MESG_0x6F:
                {
                    Console.Write("RESET Complete, reason: ");

                    byte ucReason = response.messageContents[0];

                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_POR_0x00)
                        Console.WriteLine("RESET_POR");
                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_RST_0x01)
                        Console.WriteLine("RESET_RST");
                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_WDT_0x02)
                        Console.WriteLine("RESET_WDT");
                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_CMD_0x20)
                        Console.WriteLine("RESET_CMD");
                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_SYNC_0x40)
                        Console.WriteLine("RESET_SYNC");
                    if(ucReason == (byte) ANT_ReferenceLibrary.StartupMessage.RESET_SUSPEND_0x80)
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
                                    bDone = true;
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
                            else if(response.getChannelEventCode() != ANT_ReferenceLibrary.ANTEventID.RESPONSE_NO_ERROR_0x00)
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
                            Console.WriteLine("Unhandled response " + response.getChannelEventCode() + " to message " + response.getMessageID());                            break;
                        }
                    }
                    break;
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        // PrintMenu
        //
        // Display demo menu
        // 
        ////////////////////////////////////////////////////////////////////////////////
        static void PrintMenu()
        {
            // Print out options  
            Console.WriteLine("M - Print this menu");
            Console.WriteLine("A - Send Acknowledged message");
            Console.WriteLine("B - Send Burst message");
            Console.WriteLine("R - Reset");
            Console.WriteLine("C - Request Capabilites");
            Console.WriteLine("V - Request Version");
            Console.WriteLine("I - Request Channel ID");
            Console.WriteLine("S - Request Status");
	        Console.WriteLine("U - Request USB Descriptor");
            Console.WriteLine("D - Toggle Display");
            Console.WriteLine("Q - Quit");
        }

        static double calcolaCadenza(byte[] response)
        {
            int cadenzaCount, cadenzaTimeEvent;
            double cadenza=-1;

            pagineCadenzaRicevute++;

            cadenzaCount = response[7] * 256 + response[6];
            cadenzaTimeEvent = response[5] * 256 + response[4];

            Console.Write("cadenzaCount: " + ( cadenzaCount)+ " tempoEvento: " + cadenzaTimeEvent + "ultimoNumerogiri: " + ultimoNumeroGiriCadenza + "ultimo evento: " + ultimoEventoCadenza + "  ");
            if (ultimoEventoCadenza - cadenzaTimeEvent != 0 )
            {
                cadenza = (double)(60 * (cadenzaCount - ultimoNumeroGiriCadenza) * 1024) / (cadenzaTimeEvent - ultimoEventoCadenza);
                inMovimentoCadenza = true;
            }
            
            if(cadenza>=0)
            ultimaCadenzaValida = cadenza;

            if (pagineCadenzaRicevute >= 4)
            {
                cadenzaUltimoGruppo = cadenza;
                inMovimentoCadenza = false;
                pagineCadenzaRicevute = 0;
                cadenza = ultimaCadenzaValida;
            }

            if (cadenza == cadenzaUltimoGruppo && inMovimentoCadenza == false && cadenzaUltimoGruppo!=0)
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

    }
}
