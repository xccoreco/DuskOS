using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuskOSDev.DuskSystem.Common;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandCD : Command
    {
        private static readonly CommandData data = new CommandData("cd", "Changes a directory");

        public CommandCD() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                try
                {
                    if (directory == "..")
                    {
                        Directory.SetCurrentDirectory(Kernel.CurrentDirectory);
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return true;
        }
    }
}
