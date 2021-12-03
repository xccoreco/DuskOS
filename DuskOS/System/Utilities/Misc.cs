/*
 * SOURCE:          Aura Operating System Development
 *
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Utilities/Misc.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Utilities
{
    public class Misc
    {
        //https://stackoverflow.com/questions/59638467/parsing-command-line-args-with-quotes
        public static List<string> ParseCommandLine(string cmdLine)
        {
            var args = new List<string>();
            if (string.IsNullOrWhiteSpace(cmdLine)) return args;

            var currentArg = new StringBuilder();
            bool inQuotedArg = false;

            for (int i = 0; i < cmdLine.Length; i++)
            {
                if (cmdLine[i] == '"')
                {
                    if (inQuotedArg)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new StringBuilder();
                        inQuotedArg = false;
                    }
                    else
                    {
                        inQuotedArg = true;
                    }
                }
                else if (cmdLine[i] == ' ')
                {
                    if (inQuotedArg)
                    {
                        currentArg.Append(cmdLine[i]);
                    }
                    else if (currentArg.Length > 0)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new StringBuilder();
                    }
                }
                else
                {
                    currentArg.Append(cmdLine[i]);
                }
            }

            if (currentArg.Length > 0) args.Add(currentArg.ToString());

            return args;
        }

        
    }
}

