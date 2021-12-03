/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/MkfileCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.IO;
using DuskOS;

namespace CommandSharp.Commands.Filesystem
{
    public sealed class MkfileCommand : Command
    {
        public static readonly CommandData data = new CommandData("mkfile", "Makes a new file");

        public MkfileCommand() : base (data) { }
        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);

                if (!File.Exists(Kernel.currentdir + file))
                {
                    File.Create(Kernel.currentdir + file);
                }
                else
                {
                    Console.WriteLine($"File '{file}' already exists!");
                }
            }

            return true;
        }
    }
}
