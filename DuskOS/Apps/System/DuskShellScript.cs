/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          Apps/System/DuskShellScript.cs | Dusk Shell Script
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.IO;

namespace DuskOS.Apps.System
{
    public class DuskShellScript
    {
        public static void Execute(string filename)
        {
            try
            {
                if (filename.EndsWith(".dsh"))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        if (!line.StartsWith("#")) //don't read the line if it starts with '#' for comment
                        {
                            //DuskCommander.CommandManager.Prompt(line);
                            //for <statement> <in?> <object>?
                        }
                        /*  --- Loops ---
                         *  if line contains a number note and pass
                         *  if a loop statement is detected loop back to that number X times
                         *
                         *  --- IF ---
                         * if <condition> then
                         *  <block>
                         * endif
                         */
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid script!");
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
