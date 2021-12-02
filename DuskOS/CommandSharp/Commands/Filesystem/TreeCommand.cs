/*
 * SOURCE:          Aura Operating System Development
 *
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/TreeCommand.cs
 * PROGRAMMERS:
 *                  Valentin Charbonnier <valentinbreiz@gmail.com>
 * EDITORS: 
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using CommandSharp.Commands;
using DuskOS.System.Utilities;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public class TreeCommand : Command
    {
        private static readonly CommandData data = new CommandData("tree", "displays tree of directories and files");

        public TreeCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            SystemUtils.DoTree(Kernel.currentdir, 0);
            return true;
        }
    }
}
