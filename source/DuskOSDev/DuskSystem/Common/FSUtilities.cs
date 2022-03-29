using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Common
{
    public class FSUtilities
    {
        public static void ListDirectories(string currentDirectory)
        {
            foreach (string directory in Directory.GetDirectories(currentDirectory))
            {
                if (!directory.StartsWith("."))
                {
                    Console.Write(directory + "\t");
                }
                else
                {
                    Console.WriteLine("Directories cannot start with periods!");
                }
            }
        }

        public static void ListFiles(string currentDirectory)
        {
            foreach (string file in Directory.GetFiles(currentDirectory))
            {
                if (!file.StartsWith("."))
                {
                    Console.Write(file + "\t");
                }
                else
                {
                    Console.WriteLine("Files cannot start with periods!");
                }
            }
        }
    }
}
