using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Command = DuskOSDev.DuskSystem.Command;

namespace DuskOSDev.DuskSystem.Command
{
    public class CommandInvoker
    {
        private bool processQuotes = true;
        private bool ignoreInnerQuotes = false;
        private List<Command> commands;

        //public string UnauthorizedCommandAccessMessage { get; set; } = "You do not have access to this command!";

        public CommandInvoker(bool processQuotes = true, bool ignoreInnerQuotes = false)
        {
            commands = new List<Command>();

            this.processQuotes = processQuotes;
            this.ignoreInnerQuotes = ignoreInnerQuotes;

            RegisterInternalCommands();
        }

        private void RegisterInternalCommands()
        {
            //only put certain commands here like HELP, ECHO, CLEAR, CD, ECT, all others must be placed elsewhere.
            //For example, DummyCommand does not go here. And neither does Logout or hello.

        }

        public void Register(Command command)
        {
            //if (GetPrompt().Debug)
                Console.WriteLine("Registered command: \"" + command.GetName() + "\"");

            if (!CommandExists(command) && (GetCommand(command.GetName()) == null))
                //Register Command.
                commands.Add(command);
        }

        public void Register(Command[] commands)
        {
            foreach (Command cmd in commands)
                Register(cmd);
        }

        public void Register(List<Command> commands)
            => Register(commands.ToArray());

        public void Unregister(Command command)
        {
            if (CommandExists(command))
                commands.Remove(command);
        }

        public void Unregister(Command[] commands)
        {
            foreach (Command cmd in commands)
                Unregister(cmd);
        }

        public void Unregister(List<Command> commands)
            => Unregister(commands.ToArray());

        public void Override(string oldCommand, Command newCommand)
        {
            Command cmd = GetCommand(oldCommand);
            if (CommandExists(cmd))
                Override(cmd, newCommand);
        }

        public void Override(Command oldCommand, Command newCommand)
        {
            var index = IndexOf(oldCommand);
            if (index <= -1)
                throw new Exception("Could not continue, index is outside the bounds of the command registry.");
            if (CommandExists(oldCommand) && !CommandExists(newCommand)) //We don't want a duplicate value of newCommand.
                commands[index] = newCommand; //Inject the newCommand at the index of oldCommand.
        }

        public bool CommandExists(Command command)
        {
            foreach (Command c in GetCommands())
            {
                if (c == command)
                    return true;
                else
                    continue;
            }
            return false;
        }

        public int IndexOf(Command command)
        {
            for (int i = 0; i < GetCommands().Length; i++)
            {
                Command cmd = commands[i];
                if (CommandExists(command) && (cmd == command))
                    return i;
                else
                    continue;
            }
            return -1;
        }

        public Command[] GetCommands() => commands.ToArray();

        public Command GetCommandByName(string name)
        {
            //if (GetPrompt().Debug)
                Console.WriteLine("Attempting to get a command with name: \"" + name + "\"...");
            foreach (Command c in GetCommands())
            {
                //if (GetPrompt().Debug)
                    Console.WriteLine("Checking command: \"" + c.GetName().ToLower() + "\n");
                if (c.GetName().ToLower().Equals(name.ToLower()))
                {
                    //if (GetPrompt().Debug)
                        Console.WriteLine("Command Found!");
                    return c;
                }
                else
                    continue;
            }
            return null;
        }

        public Command GetCommandByAlias(string alias)
        {
            Command c = null;
            //if (GetPrompt().Debug)
                Console.WriteLine("Attempting to get a command with alias: \"" + alias + "\"...");
            foreach (Command cmd in GetCommands())
            {
                var aliases = cmd.GetAliases();
                if ((aliases != null) && aliases.Length > 0)
                {
                    foreach (string a in aliases)
                    {
                        bool isNull = a.IsNullWhiteSpaceOrEmpty();
                        if ((!isNull) && a.ToLower().Equals(alias.ToLower()))
                        {
                            c = cmd;
                            break;
                        }
                        else continue;
                    }
                }
                else continue;
            }
            return c;
        }

        public Command GetCommand(string value)
        {
            Command cname = GetCommandByName(value);
            Command calias = GetCommandByAlias(value);

            bool nameNull = cname == null;
            bool aliasNull = calias == null;

            /*if (GetPrompt().Debug)
            {
                var nA = (nameNull) ? "No Command With Name Found!" : "Name Found!";
                var aA = (aliasNull) ? "No Command With Alias Found!" : "Alias Found!";
                var n = $"{nA}\n{aA}";
                Console.WriteLine(n);
            }*/

            if (!nameNull && aliasNull)
                return cname;
            else if (nameNull && !aliasNull)
                return calias;
            else if (!nameNull && !aliasNull)
                return cname;
            else
                return null;
        }

        public void Invoke(string input)
        {
            string name = "";
            string[] rawArgs = null;

            if (GetPrompt().Debug)
                Console.WriteLine("Parsing command name from input.");

            //Split for only the name and drop the rest.
            var x = input.Split(" ");
            name = x[0];
            x = null;

            if (GetPrompt().Debug)
                Console.WriteLine("Obtained name: \"" + name + "\"...");

            if (GetPrompt().Debug)
                Console.WriteLine("Parsing the arguments...");

            //Get the arguments.
            rawArgs = ParseArguments(input);

            //Skip the first value since the first value will always be the name.
            rawArgs = rawArgs.Skip(1);

            if (GetPrompt().Debug)
                Console.WriteLine("Getting an instance of a command based on the name.");
            //Initialize an instance of the called command.
            Command cmd = GetCommand(name);



            if (GetPrompt().Debug)
                Console.WriteLine("Creating an instance of CommandArgs class...");
            //Initialize an instance of CommandArgs.
            CommandArguments args = new CommandArguments(rawArgs);

            if (GetPrompt().Debug)
                Console.WriteLine("Handling invokation...");
            //Pass to the ManualInvoker for Invokation.
            ManualInvoke(cmd, args, name);
        }

        private string[] ParseArguments(string input)
        {
            if (processQuotes && (input.Contains("\"") || input.Contains("\\\"")))
            {
                List<string> tokens = new List<string>();
                string currentToken = "";
                bool isInQuotes = false;
                //bool isEscaped = false;

                for (int i = 0; i < input.Length; i++)
                {
                    var @char = input[i];
                    if (@char == '\"')
                    {
                        if (!isInQuotes)
                            isInQuotes = true;
                        else
                        {
                            if (currentToken.Length > 0)
                                tokens.Add(currentToken);
                            currentToken = "";
                            isInQuotes = false;
                        }
                    }
                    else if ((@char == '\\' && isInQuotes) && !ignoreInnerQuotes)
                    {
                        var nCharI = input.IndexOf(@char) + 1;
                        var nChar = input[nCharI];
                        if (nChar == '\"')
                        {
                            currentToken += "$[&quote];";
                            //Remove the next value.
                            input = input.Remove(i, 1);
                            continue;
                        }
                        else continue;
                    }
                    else
                    {
                        if (@char == ' ' && !isInQuotes)
                        {
                            if (currentToken.Length > 0)
                                tokens.Add(currentToken);
                            currentToken = "";
                        }
                        else
                            currentToken += @char;
                    }
                }

                isInQuotes = false;

                //Flush anything in the currentToken if not null.
                if (currentToken.Length > 0)
                    tokens.Add(currentToken);
                currentToken = "";

                if (!ignoreInnerQuotes)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i].Contains("$[&quote];"))
                            tokens[i] = tokens[i].Replace("$[&quote];", "\"");
                    }
                }

                return tokens.ToArray();
            }
            else
            {
                //Split spaces.
                var x = input.Split(" ");
                return x;
            }
        }

        public void ManualInvoke(Command command, CommandArguments args, string inputName = "")
        {
            //if (GetPrompt().Debug)
                Console.WriteLine("Checking if command exists...");

            if (CommandExists(command))
            {
                //if (GetPrompt().Debug)
                    Console.WriteLine("The Command Exists!");

                //if (GetPrompt().Debug)
                    Console.WriteLine("Checking if the current user has access to this command...");

                //Check if the currently logged user has the ability to run this command.
                //var usr = Kernel.currentuser;
                /* if (!(usr == null && (usr.GetAccountType() == AccountType.ADMIN || usr.GetAccountType() == AccountType.SYSTEM) && CanUserInvoke(command, usr)))
                {
                    if (GetPrompt().Debug)
                        Console.WriteLine("The user: \"" + usr.GetUsername() + "\" is logged in, but does not have access...");

                    var col = GetPrompt().CurrentForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(UnauthorizedCommandAccessMessage);
                    Console.ForegroundColor = col;
                    return; //Cancel invokation.
                } */
                //else
                //{
                //if (GetPrompt().Debug)
                    //Console.WriteLine("The user: \"" + usr.GetUsername() + "\" is logged in, and has access...");
                
                if ((!args.IsEmpty) && args.StartsWith("help") && !command.GetName().ToLower().Equals("help"))
                {
                    //Forward to help.
                    ManualInvoke(GetCommand("help"), new CommandArguments(new string[] { command.GetName() }), inputName);
                    return;
                }

                CommandInvokeParameters @params = new CommandInvokeParameters()
                {
                    Prompt = GetPrompt(),
                    Invoker = this,
                    Arguments = args
                };

                //if (GetPrompt().Debug)
                    Console.WriteLine("Invoking the command.");

                var x = command.OnInvoke(@params);

                //if (GetPrompt().Debug)
                    Console.WriteLine("Checking if syntax was requested.");

                if (!x)
                {
                    var synt = RequestSyntax(command, inputName);
                    if (!synt.IsNullWhiteSpaceOrEmpty())
                        Console.WriteLine(synt);
                }
                //}
            }
            else
            {
                Console.WriteLine("\"" + inputName + "\", is not a valid command!");
            }
        }

        public string RequestSyntax(Command command, string inputName = null)
        {
            SyntaxErrorParameters e = new SyntaxErrorParameters()
            {
                CommandNamePassed = inputName ?? command.GetName()
            };
            var synt = command.OnSyntaxError(e);
            if (!synt.IsNullWhiteSpaceOrEmpty())
                return synt;
            else
                return null;
        }

        /* internal bool CanUserInvoke(Command c, User user)
        {
            var xD = c.GetData().GetAuthorizedAccountTypes();
            var uType = user.GetAccountType();
            if (Utilities.ArrayContains(xD, uType))
                return true;
            else return false;
        } */

        internal CommandPrompt prompt = null;

        internal void SetCommandPrompt(CommandPrompt p)
        {
            prompt = p;
        }

        public CommandPrompt GetPrompt() => prompt;
    }
}
