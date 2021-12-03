/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Network/IPConfigCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 * NOTES:
 *                  Original offer didn't have copyright but it was Misha
 */

using DuskOS.System.Modules.Network;

namespace CommandSharp.Commands.Network
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
