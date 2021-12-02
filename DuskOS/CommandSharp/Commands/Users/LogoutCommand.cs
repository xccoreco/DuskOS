/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          CommandSharp/Commands/Users/LogoutCommand.cs
 * PROGRAMMERS:
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 */
using System;
using System.Collections.Generic;
using System.Text;
using DuskOS;
using DuskOS.System;
using DuskOS.System.Users;
using DuskOS.System.Utilities;

namespace CommandSharp.Commands.Users
{
    public class LogoutCommand : Command
    {
        //Create the field that holds the data on the command.
        private static readonly CommandData data = new CommandData("logout", "Logs out of the currently logged user, or logs out any specified user.");

        public LogoutCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //Check if there's any arguments.
            var args = e.Arguments;
            if (args.IsEmpty)
            {
                //No arguments were passed.
                //Logout the current user.
                var user = Kernel.currentuser;
                if (user != null)
                {
                    //A user is logged in. Perform logout via LoginManager.
                    LoginManager.Logout();
                }
            }
            else
            {
                //Get the first argument.
                var arg1 = args.GetArgumentAtPosition(0);
                if (Utilities.IsNullWhiteSpaceOrEmpty(arg1))
                    return false;
                //TODO: Get the user from the user registry rather then iterating manually.
                var x = User.GetTemporaryUserRegistry();
                User ux = null;
                foreach (User u in x)
                {
                    if (u.GetUsername().ToLower().Equals(arg1.ToLower()))
                    {
                        ux = u;
                        break;
                    }
                    else continue;
                }
                if (ux == null)
                    Console.WriteLine("No user found with that name.");
                else
                {
                    //Check the status of the user.
                    if (ux.IsLoggedIn)
                    {
                        if (Kernel.currentuser == ux)
                            //Logout via login manager.
                            LoginManager.Logout();
                        else
                        {
                            //Logout manually.
                            ux.Logout();
                        }
                    }
                    else
                        Console.WriteLine("That user is not logged in.");
                }
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{e.CommandNamePassed} <user>\n");
            builder.AppendLine("user: Represents the Username of the user to logout.");
            return builder.ToString();
        }
    }
}
