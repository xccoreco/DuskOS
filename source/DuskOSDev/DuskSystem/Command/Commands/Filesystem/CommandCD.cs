using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuskOSDev.DuskSystem.Common;

namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public sealed class CommandCD : Command
    {
        private static readonly CommandData data = new CommandData("cd", "Changes a directory");

        public CommandCD() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                try
                {
                    if (directory == "..")
                    {
                        Directory.SetCurrentDirectory(Kernel.CurrentDirectory);
                        var root = DuskFS.GetDirectory(Kernel.CurrentDirectory);
                        if (Kernel.CurrentDirectory != Kernel.CurrentPartition.ToString() + @":\")
                        {
                            Kernel.CurrentDirectory = root.mParent.mFullPath;
                            Kernel.prompt.CurrentDirectory = Kernel.CurrentDirectory;
                        }
                    }
                    else if (directory == Kernel.CurrentPartition.ToString())
                    {
                        Kernel.CurrentDirectory = Kernel.CurrentPartition.ToString() + @":\";
                        Kernel.prompt.CurrentDirectory = Kernel.CurrentDirectory;
                    }
                    else
                    {
                        if (Directory.Exists(Kernel.CurrentDirectory + directory))
                        {
                            Directory.SetCurrentDirectory(Kernel.CurrentDirectory);
                            Kernel.CurrentDirectory = Kernel.CurrentDirectory + directory + @"\";
                            Kernel.prompt.CurrentDirectory = Kernel.CurrentDirectory;
                        }
                        else if (File.Exists(Kernel.CurrentDirectory + directory))
                        {
                            Console.WriteLine("Error this is a file");
                        }
                        else
                        {
                            Console.WriteLine("Directory doesn't exist");
                        }
                    }
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
