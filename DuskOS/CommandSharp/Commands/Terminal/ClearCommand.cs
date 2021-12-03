/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Terminal/ClearCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;

namespace CommandSharp.Commands.Terminal
{
    public class ClearCommand : Command
    {
        private static readonly CommandData data = new CommandData("clear", "Clears the console.", new string[] { "clr", "cls" });

        public ClearCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.Clear();
            return true;
        }
    }
}
