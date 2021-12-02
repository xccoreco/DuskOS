using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuskOS.System.Utilities
{
    public class SystemUtils
    {
        //add aura copyright
        public static void DoTree(string directory, int depth)
        {
            var directories = Directory.GetDirectories(directory);
            string dir;
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (string file in Directory.GetFiles(directory))
            {
                for (int i = 0; i < depth; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(file);
            }
            Console.ForegroundColor = ConsoleColor.White;

            for (int j = 0; j < directories.Length; j++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < depth; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(directories[j]);
                DoTree(directory + "/" + directories[j], depth + 4);
            }
        }
        /*
         * count++;
                    if (count == Console.WindowHeight)
                    {
                        Console.ReadKey();
                        count = 0;
                    }
         */
    }
}

