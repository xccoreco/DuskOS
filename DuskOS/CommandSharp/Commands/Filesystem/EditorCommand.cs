/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/EditorCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using CommandSharp.Commands;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public sealed class EditorCommand : Command
    {
        private static readonly CommandData data = new CommandData("editor", "Opens a new instance of kate editor");

        public EditorCommand() : base(data) { }
        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);

                Apps.User.Kate.Startkate(Kernel.currentdir + file);
            }
            return true;
        }
    }
}
