using DuskOSDev.DuskSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuskOSDev.DuskSystem.Utilities;
using System.Threading.Tasks;
namespace DuskOSDev.DuskSystem.Command.Commands
{
    public sealed class CommandHelp : Command
    {
        private static readonly CommandData data = new CommandData("help", "Displays all commands, or displays information on a command depending on the context provided.", new string[] { "?", "cmds" });

        public CommandHelp() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //Check if arguments is NOT null.
            var args = e.Arguments;
            if (!args.IsEmpty)
            {
                if (args.StartsWithSwitch('w') || args.StartsWithSwitch("wide"))
                {
                    //Show wide view.
                    StringBuilder b = new StringBuilder();
                    string s = "";
                    int i = 0;
                    foreach (Command cmd in e.Invoker.GetCommands())
                    {
                        if (!cmd.IsCommandHidden)
                        {
                            i++;
                            if (i == 5)
                            {
                                if (b.Length > 0)
                                {
                                    b.Append($"\n{s}");
                                    s = "";
                                    i = 0;
                                    s = cmd.GetName();
                                }
                                else
                                {
                                    b.Append(s);
                                    s = "";
                                    i = 0;
                                    s = cmd.GetName();
                                }
                            }
                            else
                            {
                                //Append s.
                                if (s.Length > 0)
                                    s += $", {cmd.GetName()}";
                                else
                                    s = cmd.GetName();
                            }
                        }
                        else continue;
                    }

                    if (s.Length > 0)
                    {
                        if (b.Length > 0)
                            b.Append($"\n{s}");
                        else
                            b.Append(s);
                    }
                    //Return the value.
                    Console.WriteLine(b.ToString());
                }
                else
                {
                    //Get the command from the argument at position 0.
                    var c = args.GetArgumentAtPosition(0);
                    var cmd = e.Invoker.GetCommand(c);
                    if (cmd != null)
                    {
                        StringBuilder b = new StringBuilder();
                        string s = $"-----| {cmd.GetName()} |-----";
                        bool printDesc = !Utilities.Utilities.IsNullWhiteSpaceOrEmpty(cmd.GetDescription());
                        var a = cmd.GetAliases();
                        bool printAliases = !(a.Length < 1 || a == null);
                        var synt = e.Invoker.RequestSyntax(cmd);
                        b.AppendLine(s);
                        if (printDesc)
                            b.AppendLine($"Description: {cmd.GetDescription()}");
                        if (printAliases)
                            b.AppendLine($"Aliases: {GetStringFromArray(cmd.GetAliases())}");
                        if (synt != null)
                            b.AppendLine($"Syntax:\n{synt}");
                        b.Append(new string('-', s.Length));
                        Console.WriteLine(b.ToString());
                    }
                    else
                        Console.WriteLine("No command with such name...");
                }
            }
            else
            {
                //Return normal help.
                StringBuilder b = new StringBuilder();
                foreach (Command c in e.Invoker.GetCommands())
                {
                    if (!c.IsCommandHidden)
                    {
                        if (b.Length > 0)
                            b.Append($"\n{c.GetName()}{(!Utilities.Utilities.IsNullWhiteSpaceOrEmpty(c.GetDescription()) ? $": {c.GetDescription()}" : "")}");
                        else
                            b.Append($"{c.GetName()}{(!Utilities.Utilities.IsNullWhiteSpaceOrEmpty(c.GetDescription()) ? $": {c.GetDescription()}" : "")}");
                    }
                    else continue;
                }
                Console.WriteLine(b.ToString());
            }
            return true;
        }

        private string GetStringFromArray(string[] array)
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in array)
            {
                if (b.Length > 0)
                    b.Append($", {s}");
                else
                    b.Append(s);
            }
            return b.ToString();
        }
    }
}
