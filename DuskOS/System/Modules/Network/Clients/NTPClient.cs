using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP;

namespace DuskOS.System.Modules.Network.Clients
{
    public class NTPClient
    {
        /// <summary>
        /// Show debug messages?
        /// </summary>
        public bool Debug { get; set; } = false;
        public Address NTPServer = new Address(40, 119, 6, 228);

        public DateTime GetNetworkTime()
        {
            //IP address of time.windows.com

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B;  // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            using (var xClient = new UdpClient(123))
            {
                if (Debug)
                    Console.WriteLine("Connecting to " + NTPServer.ToString());

                xClient.Connect(NTPServer, 123);

                if (Debug)
                    Console.WriteLine("Sending NTP packet");

                // Send Data
                xClient.Send(ntpData);

                if (Debug)
                    Console.WriteLine("Receiving NTP packet");

                // Receive data
                var endpoint = new EndPoint(NTPServer, 123);
                ntpData = xClient.Receive(ref endpoint);

                if (Debug)
                    Console.WriteLine("Closing connection");

                xClient.Close();
            }

            if (Debug)
                Console.WriteLine("Parsing data...");

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            var x = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var networkDateTime = x.AddMilliseconds(milliseconds);

            return networkDateTime;
        }

        // stackoverflow.com/a/3294698/162671
        private uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                          ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) +
                          ((x & 0xff000000) >> 24));
        }
    }
}
