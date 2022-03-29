using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Terminal
{
    public sealed class CommandClear : Command
    {
        private static readonly CommandData data = new CommandData("clear", "Clears the console.", new string[] { "clr", "cls" });

        public CommandClear() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.Clear();

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
