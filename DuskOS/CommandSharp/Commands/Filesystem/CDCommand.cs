/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Filesystem/CDCommand.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using System.IO;
using CommandSharp.Commands;

namespace DuskOS.CommandSharp.Commands.Filesystem
{
    public sealed class CDCommand : Command
    {
        private static readonly CommandData data = new CommandData("cd", "Change directory");

        public CDCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);
                try
                {
                    if (directory == "..")
                    {
                        Directory.SetCurrentDirectory(Kernel.currentdir);
                        var root = Kernel.fs.GetDirectory(Kernel.currentdir);
                        if (Kernel.currentdir != Kernel.currentvol)
                        {
                            Kernel.currentdir = root.mParent.mFullPath;
                            Kernel.prompt.CurrentDirectory = Kernel.currentdir;
                        }
                    }
                    else if (directory == Kernel.currentvol)
                    {
                        Kernel.currentdir = Kernel.currentvol;
                        Kernel.prompt.CurrentDirectory = Kernel.currentdir;
                    }
                    else
                    {
                        if (Directory.Exists(Kernel.currentdir + directory))
                        {
                            Directory.SetCurrentDirectory(Kernel.currentdir);
                            Kernel.currentdir = Kernel.currentdir + directory + @"\";
                            Kernel.prompt.CurrentDirectory = Kernel.currentdir;
                        }
                        else if (File.Exists(Kernel.currentdir + directory))
                        {
                            Console.WriteLine("Error this is a file");
                        }
                        else
                        {
                            Console.WriteLine("Directory doesn't exist");
                        }
                    }

                    // return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return true;
        }
    }
}
