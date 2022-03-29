using DuskOSDev.DuskSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandListDirectory : Command
    {
        private static readonly CommandData data = new CommandData("dir", "List directories and files", new string[] { "ls" });

        public CommandListDirectory() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (e.Arguments.IsEmpty)
            {
                FSUtilities.ListDirectories(Kernel.CurrentDirectory);
                FSUtilities.ListFiles(Kernel.CurrentDirectory);
                Console.WriteLine();
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
