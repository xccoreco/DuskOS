using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;

namespace DuskOS.System.Modules.Network.Clients
{
    public class HTTPClient
    {
        private TcpClient client;
        private Address address;
        private string HostName;
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36";
        public HTTPClient(string hostName)
        {
            this.address = Utils.GetAddressFromName(hostName);
            this.HostName = hostName;
            client = new TcpClient(80);
        }

        public string GET(string url = "/")
        {
            Console.WriteLine("Connecting to: " + this.address.ToString());
            client.Connect(this.address, 80);
            string httpget = $"GET {url} HTTP/1.1\r\n" +
                             $"User-Agent: Wget {UserAgent}\r\n" +
                             "Accept: */*\r\n" +
                             "Accept-Encoding: identity\r\n" +
                             "Host: " + HostName + "\r\n" +
                             "Connection: Keep-Alive\r\n\r\n";

            client.Send(Encoding.ASCII.GetBytes(httpget));

            var ep = new EndPoint(Address.Zero, 0);
            var data = client.Receive(ref ep);
            try
            {
                client.Close();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

            string httpresponse = Encoding.ASCII.GetString(data);
            return httpresponse;
        }
    }
}
