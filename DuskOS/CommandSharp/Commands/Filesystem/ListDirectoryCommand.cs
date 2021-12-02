/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/ListDirectoryCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using CommandSharp.Commands;
using DuskOS.System.Shell;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public class ListDirectoryCommand : Command
    {
        private static readonly CommandData data = new CommandData("dir", "List directories and files",
            cmdAliases: new string[] { "ls" });

        public ListDirectoryCommand() : base (data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                //display directory pointed at or display hidden?
            }
            else
            {
                DirectoryListing.DisplayDirectories(Kernel.currentdir);
                DirectoryListing.DisplayFiles(Kernel.currentdir);
                Console.WriteLine();
            }
            return true;
        }
    }
}
