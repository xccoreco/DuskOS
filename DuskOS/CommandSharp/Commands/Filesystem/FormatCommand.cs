/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/FormatCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using CommandSharp.Commands;
using DuskOS.System;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public class FormatCommand : Command
    {
        private static readonly CommandData data = new CommandData("format", "Format's disk drive");

        public FormatCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string drive = e.Arguments.GetArgumentAtPosition(0); //0

                if (e.Arguments.GetArgumentAtPosition(1) != null)
                {
                    string agree = e.Arguments.GetArgumentAtPosition(1);
                    Console.WriteLine("Are you sure? y or n");

                    if (agree == "y" || agree == "yes")
                    {
                        //do shit
                        DuskFS.FormatFileSystem(Kernel.fs, drive, "FAT32", true);
                    } else if (agree == "n" || agree == "no")
                    {
                        //cancel
                    }
                    else
                    {
                        Console.WriteLine("Agreement must be yes or no.");
                    }
                }
                else
                {
                    //you need to agree
                }
            }
            return true;
        }
    }
}
