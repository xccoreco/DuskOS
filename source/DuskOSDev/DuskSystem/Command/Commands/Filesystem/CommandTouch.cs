using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandTouch : Command
    {
        private static readonly CommandData data = new CommandData("touch", "Makes a new file", new string[] { "mkfile" });

        public CommandTouch() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);

                if (!File.Exists(Kernel.CurrentDirectory + file))
                {
                    File.Create(Kernel.CurrentDirectory + file);
                }
                else
                {
                    Console.WriteLine($"File '{file}' already exists!");
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
