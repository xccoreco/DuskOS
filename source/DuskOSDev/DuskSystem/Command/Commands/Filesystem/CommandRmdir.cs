using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Dev note: I know 5 name spaces is long, but I don't feel like refactor right now, or restructure for that matter. 
 */
namespace DuskOSDev.DuskSystem.Command.Commands.Filesystem
{
    public class CommandRmdir : Command
    {
        private static readonly CommandData data = new CommandData("rmdir", "Remove a directory");

        public CommandRmdir() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                string directory = e.Arguments.GetArgumentAtPosition(0);

                if (Directory.Exists(Kernel.CurrentDirectory + directory))
                {
                    Directory.Delete(Kernel.CurrentDirectory + directory, true);
                }
                else
                {
                    Console.WriteLine($"Directory '{directory}' doesn't exist!");
                }    
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return base.OnSyntaxError(e);
        }

        /*
         * In development we don't want them to delete 0:\DuskOS\ directory but we can let them delete whats inside (maybe)
         * If they delete that directory the file system automatically reformats.
         * --- IN PROGRESS ---
         */
        public bool IsBlackListed(string directory)
        {
            switch (directory)
            {
                case @"0:\DuskOS\":
                    Console.WriteLine("Can't delete this directory!");
                    break;
            }

            return false;
        }
    }
}
