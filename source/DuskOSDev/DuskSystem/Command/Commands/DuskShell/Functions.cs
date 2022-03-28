using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.DuskShell
{
    public class Functions
    {

    }

    public class CommandTimeout : Command
    {
        //You want these hidden because they are suppose to be shell execusive.
        private static readonly CommandData data = new CommandData("help", "help command", hideCommand: true);

        public CommandTimeout() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Cosmos.HAL.PIT pit = new Cosmos.HAL.PIT();
            Console.WriteLine("NOT DONE!");

            return true;
        }
    }
}
