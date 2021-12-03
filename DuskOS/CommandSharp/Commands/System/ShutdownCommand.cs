/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/System/ShutdownCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

namespace CommandSharp.Commands.System
{
    public class ShutdownCommand : Command
    {
        private static readonly CommandData data = new CommandData("shutdown", "to do an ACPI shutdown");

        public ShutdownCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;

            if (args.IsEmpty)
            {
                /* Save Config */
                Cosmos.System.Power.Shutdown();
            }
            else
            {
                if (args.StartsWithSwitch('f') || args.StartsWithSwitch("force"))
                {
                    /* force a shutdown no config saving */
                    Cosmos.System.Power.Shutdown();
                }
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }
    }
}
