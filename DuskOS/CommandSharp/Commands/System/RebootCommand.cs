using System;
using DuskOS.System.Config;

namespace CommandSharp.Commands.System
{
    public class RebootCommand : Command
    {
        private static readonly CommandData data = new CommandData("reboot", "to do a CPU reboot", new string[] { "restart" });
        
        public RebootCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;

            if (args.IsEmpty)
            {
                /* Save Config */
                Console.WriteLine("Saving config!");
                ConfigurationManager.SaveConfig();
                Cosmos.System.Power.Reboot();
            }
            else
            {
                if (args.StartsWithSwitch('f') || args.StartsWithSwitch("force"))
                {
                    /* force a shutdown no config saving */
                    Console.WriteLine("Shutting down without saving!");
                    
                    Cosmos.System.Power.Reboot();
                } else if (args.StartsWithSwitch('d') || args.StartsWithSwitch("discard"))
                {
                    Console.WriteLine("Discarding Config!");
                    ConfigurationManager.DiscardConfig();
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
