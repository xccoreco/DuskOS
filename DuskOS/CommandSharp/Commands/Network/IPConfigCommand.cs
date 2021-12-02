using System;
using System.Collections.Generic;
using System.Text;
using CommandSharp.Commands;
using DuskOS.System.Modules.Network;

namespace DuskOS.CommandSharp.Commands.Network
{
    public class IPConfigCommand : Command
    {
        private static readonly CommandData data = new CommandData("ipconfig", "Displays the IP Config");

        public IPConfigCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            NetworkManager.IPConfigNM();
            return true;
        }
    }
}
