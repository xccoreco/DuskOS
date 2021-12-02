/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/RmdirCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using System.IO;
using CommandSharp.Commands;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    //possible joined function with rmfile
    public class RmdirCommand : Command
    {
        private static readonly CommandData data = new CommandData("rmdir", "Remove a directory");

        public RmdirCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                if (Directory.Exists(Kernel.currentdir + directory))
                {
                    Directory.Delete(Kernel.currentdir + directory, true);
                }
                else
                {
                    Console.WriteLine($"Directory '{directory}' doesn't exist");
                }
            }

            return true;
        }
    }
}
