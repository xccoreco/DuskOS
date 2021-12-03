/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/RmfileCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.IO;

namespace CommandSharp.Commands.Filesystem
{
    //possible joined function with rmdirectory
    class RmfileCommand : Command
    {
        private static readonly CommandData data = new CommandData("rmfile", "Remove a File");

        public RmfileCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);

                if (File.Exists(Kernel.currentdir + file))
                {
                    File.Delete(Kernel.currentdir + file);
                }
                else
                {
                    Console.WriteLine($"File '{file}' doesn't exist!");
                }
            }

            return true;
        }
    }
}
