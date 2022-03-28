using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Applications
{
    public class DuskShellScript
    {
        //private static readonly string version = "0.1";
        private static List<string> instructions = new();
        private static int instCount;
        public static void Execute(string filename)
        {
            try
            {
                if (filename.EndsWith(".dsh"))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        instructions.Add(line);
                        /*
                        else if (line.StartsWith("!#"))
                        {
                            /*
                             * !# Dusk Version 1.0
                             * !@ Dusk Version 1.0
                             *

                            string[] split = line.Split(" ");
                            if (split.Contains(version))
                            {
                                Console.WriteLine("Using Dusk Interpreter 0.1");
                            }
                        }*/
                    }
                    
                    instCount = instructions.Count;

                    foreach (string line in instructions)
                    {
                        if (!line.StartsWith("#"))
                        {
                            Kernel.GetInvoker().Invoke(line);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("This file is not a valid script");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
