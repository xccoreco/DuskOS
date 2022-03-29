using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandMkdir : Command
    {
        private static readonly CommandData data = new CommandData("mkdir", "Makes a new directory");

        public CommandMkdir() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                if (directory.Contains("."))
                {
                    Console.WriteLine("mkdir does not support dots!");
                }
                else
                {
                    if (!Directory.Exists(Kernel.CurrentDirectory + directory))
                    {
                        Directory.CreateDirectory(Kernel.CurrentDirectory + directory);
                    }
                    else
                    {
                        Console.WriteLine("Directory already exists!");
                    }
                }
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
