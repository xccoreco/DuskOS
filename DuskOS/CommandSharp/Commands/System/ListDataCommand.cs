/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/System/ListDataCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using Cosmos.HAL;
using DuskOS.System.Utilities;

namespace CommandSharp.Commands.System
{
    public class ListDataCommand : Command
    {
        private static readonly CommandData data = new CommandData("list", "list System Data");

        public ListDataCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //list <devices|authors> <type>
            if (!e.Arguments.IsEmpty)
            {
                string type = e.Arguments.GetArgumentAtPosition(0);
                string kind = e.Arguments.GetArgumentAtPosition(1);

                if (type == "devices")
                {
                    if (kind == "pci")
                    {
                        ListKind("pci");
                    }
                } else if (type == "authors")
                {
                    Console.WriteLine($"Kind: {kind} not found for this type!");
                }
                else
                {
                    //Type not found
                    Console.WriteLine("The type isn't found");
                    Console.WriteLine("Available Types: "); //make automatic
                    Console.WriteLine("- devices");
                    Console.WriteLine("- authors");
                }
            }
            return true;
        }

        public void ListKind(string kind)
        {
            if (kind == "pci")
            {
                int count = 0;
                foreach (Cosmos.HAL.PCIDevice device in Cosmos.HAL.PCI.Devices)
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
    }
}
