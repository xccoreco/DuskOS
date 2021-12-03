/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/System/RegistryCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 */

using System;
using DuskOS;

namespace CommandSharp.Commands.System
{
    public class RegistryCommand : Command
    {
        //The '@', '!', '.', and '#' prefixes hide a command from help.
        private static readonly CommandData data = new CommandData("@registry", "Modifies or views registry information.", new string[] { "reg", "regedit" });

        private static readonly DuskOS.System.Registry.Registry registry = Kernel.GetRegistry();

        public RegistryCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (args.IsEmpty)
            {
                //List all items.
                foreach (var x in registry)
                {
                    string val = $"{x.GetKey()}: {x.GetValue()}";
                    Console.WriteLine(val);
                }
            }
            else
            {
                Console.WriteLine("Registry modification is not supported yet!");
            }
            return true;
        }
    }
}
