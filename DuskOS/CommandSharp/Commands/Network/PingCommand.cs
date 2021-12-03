/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Network/PingCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 * NOTES:
 *                  Original offer didn't have copyright but it was Misha
 */

using System;
using Cosmos.System.Network.IPv4;

namespace CommandSharp.Commands.Network
{
    public class PingCommand : Command
    {
        private static readonly CommandData data = new CommandData("ping", "pings a remote server");

        public PingCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string ip = e.Arguments.GetArgumentAtPosition(0);

                Address dest = Address.Parse(ip);

                if (dest == null)
                {
                    Console.WriteLine("Error: Cannot find " + ip);
                }

                int PacketSent = 0;
                int PacketReceived = 0;
                int PacketLost = 0;
                int PercentLoss;
                try
                {
                    Console.WriteLine("Sending ping to " + dest.ToString());

                    var xClient = new ICMPClient();
                    xClient.Connect(dest);

                    for (int i = 0; i < 4; i++)
                    {
                        xClient.SendEcho();

                        PacketSent++;

                        var endpoint = new EndPoint(Address.Zero, 0);

                        int second = xClient.Receive(ref endpoint, 4000);

                        if (second == -1)
                        {
                            Console.WriteLine("Destination host unreachable.");
                            PacketLost++;
                        }
                        else
                        {
                            if (second < 1)
                            {
                                Console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time < 1s");
                            }
                            else if (second >= 1)
                            {
                                Console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time " + second + "s");
                            }

                            PacketReceived++;
                        }
                    }

                    xClient.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ping error: " + ex.Message);
                }

                PercentLoss = 25 * PacketLost;

                Console.WriteLine();
                Console.WriteLine("Ping statistics for " + dest.ToString() + ":");
                Console.WriteLine("    Packets: Sent = " + PacketSent + ", Received = " + PacketReceived + ", Lost = " + PacketLost + " (" + PercentLoss + "% loss)");
            }

            return true;
        }
    }
}
