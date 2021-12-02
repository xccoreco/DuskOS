using System;
using CommandSharp.Commands;

namespace DuskOS.CommandSharp.Commands.System
{
    public class TimeCommand : Command
    {
        private static readonly CommandData data = new CommandData("time", "Gets time");

        public TimeCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.Write("Local Time: "); Console.Write(DateTime.Now);

            return true;
        }
    }
}
