/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Users/UsersCommand.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 *
 */

using System;
using System.Text;
using DuskOS.System.Users;

namespace CommandSharp.Commands.Users
{
    public class UsersCommand : Command
    {
        //TODO: Program support for verifying whether the current user has the ability to perform this command via CommandInvoker.
        private static readonly CommandData data = new CommandData("users", "Performs actions for users.", authorizedAccountTypes: new AccountType[] 
        { 
            AccountType.SYSTEM,
            AccountType.RECOVERY,
            AccountType.ADMIN
        });

        public UsersCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //The user can perform the command.
            if (e.Arguments.IsEmpty)
            {
                //List all the users on the system.
                StringBuilder users = new StringBuilder();
                foreach (DuskOS.System.Users.User user in DuskOS.System.Users.User.GetTemporaryUserRegistry())
                {
                    if (users.Length > 0)
                        users.Append($"\n{user.GetUsername()}");
                    else
                        users.Append(user.GetUsername());
                }
            }
            else
            {
                if (e.Arguments.StartsWith("add"))
                {
                    //Add a user.

                }
                else if (e.Arguments.StartsWith("remove"))
                {
                    //Remove a user.

                }
                else if (e.Arguments.StartsWith("edit"))
                {
                    //Edit a users information.

                }
                else if (e.Arguments.StartsWithSwitch('v'))
                {
                    //Check if a user exists.
                    var uArg = e.Arguments.GetArgumentAfterSwitch('v');
                    var u = DuskOS.System.Users.User.GetUserInternal(uArg);
                    if (u != null)
                        Console.WriteLine("The user: \"" + u.GetUsername() + "\" exists.");
                    else
                        Console.WriteLine("The user: \"" + u.GetUsername() + "\" does not exist.");
                }
                else if (e.Arguments.StartsWith("perms"))
                {
                    //Handle the permissions of the user. (Forward to the 'perms' command.)
                    Console.WriteLine("User permissions cannot be set at this time.");
                }
                else if (e.Arguments.StartsWithSwitch('e'))
                {
                    //Get the username.
                    var uArg = e.Arguments.GetArgumentAfterSwitch('e');
                    var u = DuskOS.System.Users.User.GetUserInternal(uArg);
                    if (u == null)
                        Console.WriteLine("Cannot enable a user that does not exist!");
                    else
                    {
                        if (!u.Enabled)
                        {
                            u.Enabled = true;
                            Console.Write("The account: \"" + u.GetUsername() + "\" is ");
                            var col = e.Prompt.CurrentForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("ENABLED");
                            Console.ForegroundColor = col;
                            Console.WriteLine(".");
                        }
                    }
                }
                else if (e.Arguments.StartsWithSwitch('d'))
                {
                    //Get the username.
                    var uArg = e.Arguments.GetArgumentAfterSwitch('e');
                    var u = DuskOS.System.Users.User.GetUserInternal(uArg);
                    if (u == null)
                        Console.WriteLine("Cannot disable a user that does not exist!");
                    else
                    {
                        if (u.Enabled)
                        {
                            u.Enabled = false;
                            Console.Write("The account: \"" + u.GetUsername() + "\" is ");
                            var col = e.Prompt.CurrentForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("DISABLED");
                            Console.ForegroundColor = col;
                            Console.WriteLine(".");
                        }
                    }
                }
                else
                    return false;
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            Console.WriteLine("Syntax Reached!");
            return base.OnSyntaxError(e);
        }
    }
}
