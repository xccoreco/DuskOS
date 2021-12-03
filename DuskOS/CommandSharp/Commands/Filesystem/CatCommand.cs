/*
 * SOURCE:          Aura Operating System Development
 *
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/CatCommand.cs
 * PROGRAMMERS:
 *                  Valentin Charbonnier <valentinbreiz@gmail.com>
 * EDITORS: 
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.IO;
using DuskOS;

namespace CommandSharp.Commands.Filesystem
{
    public class CatCommand : Command
    {
        private static readonly CommandData data = new CommandData("cat", "displays a text file");

        public CatCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string file = e.Arguments.GetArgumentAtPosition(0);
                if (File.Exists(Kernel.currentdir + file))
                {
                    foreach (var line in File.ReadAllLines(Kernel.currentdir + file))
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine($"{file} doesn't exist!");
                }
            }
            return true;
        }
    }
}
