using Cosmos.System;

namespace DuskOSDev.DuskSystem.Command.Commands.System
{
    public sealed class CommandShutdown : Command
    {
        private static readonly CommandData data = new CommandData("shutdown", "Shutdown the OS");

        public CommandShutdown() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (e.Arguments.IsEmpty)
            {
                /* ACPI shutdown */
                Power.Shutdown();
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
