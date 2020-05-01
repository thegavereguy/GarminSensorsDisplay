using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANT_Managed_Library;

namespace GARMIN_SENSORS_DISPLAY
{
    class Program
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


        static void Main(string[] args)
        {
            Console.WriteLine("Inizio programma");



        }
    }
}
