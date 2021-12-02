using System;
using CommandSharp.Commands;
using DuskOS.System.Security;

namespace DuskOS.CommandSharp.Commands.System
{
    //add hash command
    /*
     * hash <string> -t/--type <type> -f/--file <file>
     *
     */
    public class HashCommand : Command
    {
        private static readonly CommandData data = new CommandData("hash", "hashes a string");

        public HashCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            if (!e.Arguments.IsEmpty)
            {
                // Can be a file or a string depending on the switch
                string kind = e.Arguments.GetArgumentAtPosition(0);
                string type = e.Arguments.GetArgumentAtPosition(1);
                /*
                //this might need some work file system isn't in yet.
                if (e.Arguments.GetArgumentAtPosition(2) != null)
                {
                    string file = e.Arguments.GetArgumentAtPosition(2);
                    byte[] bytes = File.ReadAllBytes(file);

                    string result = Encoding.UTF8.GetString(bytes);

                    if (type == "md5" || type == "MD5")
                    {
                        string hash = MD5.hash(result);
                        Console.WriteLine("Hashed Result of MD5 - " + hash);
                    }
                    else if (type == "sha256" || type == "SHA256")
                    {
                        string hash = SHA256.hash(result);
                        Console.WriteLine("Hashed Result of SHA256 - " + hash);
                    }
                    else
                    {
                        Console.WriteLine("Type not found!");
                    }

                }
                else
                {*/

                if (kind != null)
                {
                    if (type == "md5" || type == "MD5")
                    {
                        string hash = MD5.hash(kind);
                        Console.WriteLine("Hashed Result of MD5 - " + hash);
                    }
                    else if (type == "sha256" || type == "SHA256")
                    {
                        string hash = SHA256.hash(kind);
                        Console.WriteLine("Hashed Result of SHA256 - " + hash);
                    }
                    else
                    {
                        Console.WriteLine("Type not found!");
                    }
                }
                else
                {
                    Console.WriteLine("The string is null.");
                }
            }
            //}

            return true;
        }
    }
}
