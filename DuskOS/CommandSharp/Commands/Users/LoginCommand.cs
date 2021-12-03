/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/Users/LoginCommand.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*                  ProfessorDJ/John Welsh <djlw78@gmail.com>
*
*/

using DuskOS.System.Users;
using System.Text;

namespace CommandSharp.Commands.Users
{
    public class LoginCommand : Command
    {
        //Create the field that holds the data on the command.
        private static readonly CommandData data = new CommandData("login", "Login as selected user.");

        public LoginCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (!args.IsEmpty)
            {
                var username = args.GetArgumentAtPosition(0); //johndoe
                var password = args.GetArgumentAtPosition(1); //password1

                var x = LoginManager.Login(username, password, out User user);
                //get registry?

                /*
                 * Argument to logout user?
                 * 
                 */

                /* 
                    To logout a currently logged user call LoginManager.Logout();
                    it will logout any user that's currently logged into the system.
                 */
            }
            else
            {
                //Show the login prompt.
                LoginManager.Login(out User user, out AccountAccessStatus status);
                //Nothing further needs to be done. Everthing for the most part is done for you.
            }

            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{e.CommandNamePassed} [<username> <password>]\n");
            builder.AppendLine("user: Represents the Username of the user to logout.");
            return builder.ToString();
        }
    }
}
