using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command.Commands.Terminal
{
    public sealed class CommandEcho : Command
    {
        private static readonly CommandData data = new CommandData("echo", "Returns data to the console");

        public CommandEcho() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                if (e.Arguments.StartsWith("@off") || e.Arguments.StartsWithSwitch("off"))
                {
                    //Turn the echo off.
                    e.Prompt.DisplayEcho = false;
                }
                else if (e.Arguments.StartsWith("@on") || e.Arguments.StartsWithSwitch("on"))
                {
                    e.Prompt.DisplayEcho = true;
                }
                else
                {
                    /* Loop for each addition argument? */
                    Console.WriteLine(e.Arguments.GetArgumentAtPosition(0));
                }
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return $"{e.CommandNamePassed} [message | @on | @off]";
        }
    }
}
