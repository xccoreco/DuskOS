/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/DummyCommand.cs
 * PROGRAMMERS:
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 *
 */

using System;
using System.Text;

namespace CommandSharp.Commands
{
    public sealed class DummyCommand : Command
    {
        private static readonly CommandData data = new CommandData("@dummy", "The dummy command is essentially an example command that can be used as a reference point for creating other commands.", new string[] { "example" });

        public DummyCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            string s = "This is the dummy command. It is a reference point for creating commands in CommandSharp. If any arguments were provided they'll appear below this message.";

            if (!e.Arguments.IsEmpty)
            {
                s += "\n";
                s += $"The argument length is: {e.Arguments.Count}\n";
                StringBuilder b = new StringBuilder();
                foreach (string a in e.Arguments.GetArguments())
                {
                    b.AppendLine($"[{a}]");
                }
                s += b.ToString();
            }

            Console.WriteLine($"{s}");
            return true; //True means the command was successful.
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
