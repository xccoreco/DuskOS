/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Terminal/EchoCommand.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 *
 */

using System;

namespace CommandSharp.Commands.Terminal
{
    public class EchoCommand : Command
    {
        private static readonly CommandData data = new CommandData("echo", "Returns data to the console.");

        public EchoCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (!args.IsEmpty)
            {
                if (args.StartsWith("@off") || args.StartsWithSwitch("off"))
                {
                    //Turn the echo off.
                    e.Prompt.DisplayEcho = false;
                }
                else if (args.StartsWith("@on") || args.StartsWithSwitch("on"))
                {
                    //Turn the echo on.
                    e.Prompt.DisplayEcho = true;
                }
                else
                {
                    Console.WriteLine(args.GetArgumentAtPosition(0));
                }    
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return $"{e.CommandNamePassed} [message | [@on | @off]]";
        }
    }
}
