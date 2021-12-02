/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Users/LoginManager.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley <cemberley@nerdhub.net>
 *                  John Welsh <djlw78@gmail.com>
 */
using System;

namespace DuskOS.System.Users
{
    public class LoginManager
    {
        internal static bool AllowAutoLogin { get; set; } = false;
        internal static bool SupressMessages { get; set; } = false;
        internal static string DisabledAccountMessage { get; set; } = "That account is disabled.";
        internal static string AccessDeniedMessage { get; set; } = "The username or password is incorrect.";
        internal static string NoUsernameMessage { get; set; } = "You must provide a username inorder to login. Please enter a username.";
        internal static string NoPasswordMessage { get; set; } = "That account requires a password!";

        public static void Login(out User user, out AccountAccessStatus accessStatus)
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            accessStatus = Login(username, password, out user);
        }

        public static AccountAccessStatus Login(string username, string password, out User user)
        {
            var status = User.Login(username, password, out user);
            if (status == AccountAccessStatus.ACCESS_GRANTED || status == AccountAccessStatus.NO_PASSWORD_REQUIRED)
            {
                Kernel.currentuser = user;
                Kernel.GetPrompt().CurrentUser = username;
                user.IsLoggedIn = true;
            }
            SendMessage(status, user.GetUsername());
            return status;
        }

        public static void Logout()
        {
            if (!(Kernel.currentuser == null))
            {
                Kernel.currentuser.Logout();
                Kernel.currentuser = null;
            }
        }

        public static void SendMessage(AccountAccessStatus status, string loggedUserName)
        {
            if (status == AccountAccessStatus.ACCESS_GRANTED || status == AccountAccessStatus.NO_PASSWORD_REQUIRED)
            {
                Console.Clear();
                Console.WriteLine("Welcome " + loggedUserName + ". :)");
            }
            else
            {
                if (!SupressMessages)
                {
                    if (status == AccountAccessStatus.NO_USERNAME_PROVIDED)
                        Console.WriteLine(NoUsernameMessage);
                    else if (status == AccountAccessStatus.NO_PASSWORD_PROVIDED)
                        Console.WriteLine(NoPasswordMessage);
                    else if (status == AccountAccessStatus.DISABLED)
                        Console.WriteLine(DisabledAccountMessage);
                    else
                        Console.WriteLine(AccessDeniedMessage);
                }
            }
        }

        /*internal static bool Login(string username, string password)
        {
            if (username == "demo")
            {
                if (!CheckPassword(password))
                {
                    Console.WriteLine("Passwords Don't Match!");
                }
                Console.WriteLine("Logged in as " + username);
                return true;
            }
            //Debugging
           
            return false;
        }*/

        /*
         * check password by comparing hashes
         * Hashed password SHA256
         * Enter password -> than hash SHA256
         * if Hashed = Password Hashed
         * Login 
         */
        /*internal static bool CheckPassword(string password)
        {
            var tmp = SHA256.hash(password);
            var emp = "test";
            var emp2 = SHA256.hash(emp);

            if (tmp == emp2)
            {
                Console.WriteLine("Password Match!");
                return true;
            }
            return false;
        }*/
        /*internal static bool UserExists(string username)
        {
            if (username == "demo")
            {
                return true;
            }

            return false;
        }

        internal static bool IsAdmin(string username)
        {
            //check if user is admin from users file in system directory.
            return false;
        }

        internal static bool CreateUser(string username, string password, bool admin)
        {
            //create user, and init new user (username) with user files
            return false;
        } */

        internal static bool IsAdmin(User user)
            => user.GetAccountType() == AccountType.ADMIN;

        internal static bool IsCurrentUserAdmin()
            => IsAdmin(Kernel.currentuser);

        //TODO: Do once UserRegistry is setup.
        internal static bool UserExists(string user)
        {
            return false;
        }
    }
}
