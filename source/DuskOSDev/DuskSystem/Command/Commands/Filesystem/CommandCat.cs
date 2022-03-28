using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandCat : Command
    {
        private static readonly CommandData data = new CommandData("cat", "Displays a text file");

        public CommandCat() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);
                if (File.Exists(Kernel.CurrentDirectory + file))
                {
                    foreach (var line in File.ReadAllLines(Kernel.CurrentDirectory + file))
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine($"'{file}' does not exist!");
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
