using DuskOSDev.DuskSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DuskOSDev.DuskSystem.Command.Commands
{
    public class CommandHelp : Command
    {
        private static readonly CommandData data = new CommandData("help", "help command");

        public CommandHelp() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.WriteLine("IT WORKS");
            DuskFS.DisplayInfomation(0);

            return true;
        }
    }
}
