using Cosmos.HAL;
using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.System
{
    public sealed class CommandSystemInfo : Command
    {
        private static readonly CommandData data = new CommandData("sysinfo", "lists system info");

        public CommandSystemInfo() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string type = e.Arguments.GetArgumentAtPosition(0);
                string kind = e.Arguments.GetArgumentAtPosition(1);

                if (type == "devices")
                {
                    if (kind == null)
                    {
                        Console.WriteLine("The kind was not found!");
                        Console.WriteLine("available kinds: ");
                        Console.WriteLine("- pci");
                    }
                    else
                    {
                        if (kind == "pci")
                        {
                            ListKind("pci");
                        }
                    }
                }
                else
                {
                    //Type was not found
                    Console.WriteLine("The type was not found!");
                    Console.WriteLine("Available types: ");
                    Console.WriteLine("- devices");
                }
            }
            else
            {
                DisplaySysInfo();
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }

        public void ListKind(string kind)
        {
            if (kind == "pci")
            {
                int count = 0;
                foreach (PCIDevice device in PCI.Devices)
                {
                    Console.WriteLine(Conversion.D2(device.bus) + ":" + Conversion.D2(device.slot) + ":" + Conversion.D2(device.function) + " - " + "0x" + Conversion.D4(Conversion.DecToHex(device.VendorID)) + ":0x" + Conversion.D4(Conversion.DecToHex(device.DeviceID)) + " : " + PCIDevice.DeviceClass.GetTypeString(device) + ": " + PCIDevice.DeviceClass.GetDeviceString(device));
                    count++;
                    if (count == Console.WindowHeight)
                    {
                        Console.ReadKey();
                        count = 0;
                    }
                }
            }
        }

        public void DisplaySysInfo()
        {
            /* Console Write Line Basic Sys Info */
        }
    }
}
