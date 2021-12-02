/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/MkdirCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using System.IO;
using CommandSharp.Commands;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public sealed class MkdirCommand : Command
    {
        private static readonly CommandData data = new CommandData("mkdir", "Makes a new directory");

        public MkdirCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                if (directory.Contains("."))
                {
                    Console.WriteLine("mkdir does not support dot!");
                }
                else
                {
                    if (!Directory.Exists(Kernel.currentdir + directory))
                    {
                        Directory.CreateDirectory(Kernel.currentdir + directory);
                    }
                    else if (Directory.Exists(Kernel.currentdir + directory))
                    {
                        Console.WriteLine("Directory already exits!");
                    }
                }
            }

            return true;
        }
    }
}
//TODO: might have to change '0' to '1'