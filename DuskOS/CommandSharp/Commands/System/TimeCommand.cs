/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/System/TimeCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;

namespace CommandSharp.Commands.System
{
    public class TimeCommand : Command
    {
        private static readonly CommandData data = new CommandData("time", "Gets time");

        public TimeCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.Write("Local Time: "); Console.Write(DateTime.Now);

            return true;
        }
    }
}
