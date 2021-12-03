/*
* SOURCE:          Aura Operating System Development
*
* PROJECT:         Dusk Operating System Development
* CONTENT          System/Shell/DirectoryListing.cs
* PROGRAMMERS:
*                  Valentin Charbonnier <valentinbreiz@gmail.com>
* EDITORS: 
*                  ProfessorDJ/John Welsh <djlw78@gmail.com>
*
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuskOS.System.Shell
{
    public class DirectoryListing
    {
        public static void DisplayDirectories(string currentDirectory) //bool displayHidden?
        {
            foreach (string directory in Directory.GetDirectories(currentDirectory))
            {
                if (!directory.StartsWith("."))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(directory + "\t");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                /*
                 * else we don't support directories with periods
                 *
                 *
                 */
            }
        }

        public static void DisplayFiles(string currentDirectory) //bool displayHidden?
        {
            foreach (string file in Directory.GetFiles(currentDirectory))
            {
                string lastExtension = GetLastExtension(file);

                if (!file.StartsWith("."))
                {
                    if (lastExtension == "sys")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(file + "\t");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(file + "\t");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static string GetLastExtension(string file, char formatSeparator = '.')
        {
            string[] extension = file.Split(formatSeparator);
            return extension[extension.Length - 1];
        }
        
    }
}
