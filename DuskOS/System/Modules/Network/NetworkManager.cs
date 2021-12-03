/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Modules/Network/NetworkManager.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 * NOTES:
 *                  This person didn't have a copyright notice but it was Misha
 */

using System;
using Cosmos.HAL;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using DuskOS.System.Modules.Network.Clients;
using DuskOS.System.Modules.Network.Drivers;

namespace DuskOS.System.Modules.Network
{
    public class NetworkManager
    {
        public static void Initialize()
        {
            #region Register additional network cards

            int i = 1;
            foreach (var device in PCI.Devices)
            {
                if ((device.ClassCode == 0x02) && (device.Subclass == 0x00) // is Ethernet Controller
                                               && device == PCI.GetDevice(device.bus, device.slot, device.function))
                {
                    Console.WriteLine("Found " + PCIDevice.DeviceClass.GetDeviceString(device) + " on PCI " +
                                      device.bus + ":" + device.slot + ":" + device.function);


                    if (device.VendorID == 0x10EC && device.DeviceID == 0x8168)
                    {
                        Console.WriteLine("RTL81 NIC IRQ: " + device.InterruptLine);

                        var RTL8168Device = new RTL8168(device);

                        RTL8168Device.NameID = "eth" + i;

                        Console.WriteLine("Registered at " + RTL8168Device.NameID + " (" +
                                          RTL8168Device.MACAddress.ToString() + ")");

                        RTL8168Device.Enable();
                        i++;
                    }
                }
            }

            foreach (var item in Intel8254X.FindAll())
            {
                item.NameID = "eth" + i;
                item.Enable();
                Console.WriteLine("Registered at " + item.NameID + " (" + item.MACAddress.ToString() + ")");
                i++;
            }

            #endregion

            try
            {
                using (var xClient = new DHCPClient())
                {
                    /** Send a DHCP Discover packet **/
                    //This will automatically set the IP config after DHCP response
                    NetworkStack.Update();
                    int r = xClient.SendDiscoverPacket();

                    if (r == -1)
                    {
                        Console.WriteLine("Failure while configuring DHCP: timeout");
                        xClient.Close();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("DHCP Configure result: " + r);
                    }
                    xClient.Close();
                }

                IPConfigNM();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Err: " + ex.Message);
            }

            NTPClient client = new NTPClient();
            var t = client.GetNetworkTime();
            Console.WriteLine("Current time: " + t);

            HTTPClient http = new HTTPClient("youtube.com");
            Console.WriteLine(http.GET("/"));
        }

        public static void IPConfigNM()
        {
            if (NetworkStack.ConfigEmpty())
            {
                Console.WriteLine("No network configuration detected!");
            }
            foreach (NetworkDevice device in NetworkConfig.Keys)
            {
                switch (device.CardType)
                {
                    case CardType.Ethernet:
                        Console.Write("Ethernet Card : " + device.NameID + " - " + device.Name);
                        break;
                    case CardType.Wireless:
                        Console.Write("Wireless Card : " + device.NameID + " - " + device.Name);
                        break;
                }
                if (NetworkConfig.CurrentConfig.Key == device)
                {
                    Console.WriteLine(" (current)");
                }
                else
                {
                    Console.WriteLine();
                }

                Console.WriteLine("MAC Address          : " + device.MACAddress.ToString());
                Console.WriteLine("IP Address           : " + NetworkConfig.Get(device).IPAddress.ToString());
                Console.WriteLine("Subnet mask          : " + NetworkConfig.Get(device).SubnetMask.ToString());
                Console.WriteLine("Default Gateway      : " + NetworkConfig.Get(device).DefaultGateway.ToString());
                Console.WriteLine("DNS Nameservers      : ");
                foreach (Address dnsnameserver in DNSConfig.DNSNameservers)
                {
                    Console.WriteLine("                       " + dnsnameserver.ToString());
                }
            }
        }
    }
}
