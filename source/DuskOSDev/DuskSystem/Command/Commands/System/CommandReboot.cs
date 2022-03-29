using Cosmos.System;

namespace DuskOSDev.DuskSystem.Command.Commands.System
{
    public sealed class CommandReboot : Command
    {
        private static readonly CommandData data = new CommandData("reboot", "Reboots the OS", new string[] { "restart" });

        public CommandReboot() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (e.Arguments.IsEmpty)
            {
                /* CPU Reboot */
                Power.Reboot();
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
