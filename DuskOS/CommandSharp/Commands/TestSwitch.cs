using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CommandSharp.Commands;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;

namespace DuskOS.CommandSharp.Commands
{
    public class TestSwitch : Command
    {
        private static readonly CommandData data = new CommandData("test", "example");

        public TestSwitch() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
           // Console.Clear();

            

            if (!e.Arguments.IsEmpty)
            {
               /* string ip = e.Arguments.GetArgumentAtPosition(0);
                var args = e.Arguments;
                if (args.ContainsSwitch("test"))
                {
                    Console.WriteLine($"IP = {ip}");
                    Console.WriteLine("I've gotten the arg");
                }
                else
                {
                    Console.WriteLine($"IP = {ip}");
                }*/
            }

            return true;
        }
    }
}
