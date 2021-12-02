/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Users/IUserBase.cs | Users.cs | System User
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 */

using System;
using System.Collections.Generic;

namespace DuskOS.System.Users
{
    public enum AccountType
    {
        GUEST = 0,
        CHILD = 1,
        USER = 2,
        ADMIN = 3,
        SYSTEM = 4, //This account has all permissions.
        RECOVERY = 5 //This account has no permissions since they're ignored anyway if an account with this type is loaded.
    }

    public enum NameFormat
    {
        First_Last = 1,
        Last_First = 0
    }

    public enum AccountAccessStatus
    {
        /// <summary>
        /// The user is not permitted to login either because the account is disabled, or the account is of a type that cannot login.
        /// </summary>
        DISABLED = 0,
        NO_USERNAME_PROVIDED = -5,
        /// <summary>
        /// The child's time limits have expired.
        /// </summary>
        TIME_LIMIT_EXPIRED = -4,
        /// <summary>
        /// The child's curfew was reached.
        /// </summary>
        CURFEW_REACHED = -3,
        /// <summary>
        /// The account requires a password to login, but no password was provided.
        /// </summary>
        NO_PASSWORD_PROVIDED = -2,
        /// <summary>
        /// The login is incorrect.
        /// </summary>
        ACCESS_DENIED = -1,
        /// <summary>
        /// The account does not require a password to login, so any provided passwords were ignored. And access was granted.
        /// </summary>
        NO_PASSWORD_REQUIRED = 1,
        /// <summary>
        /// The password was correct and the user was able to login.
        /// </summary>
        ACCESS_GRANTED = 2
    }

    public sealed class User
    {
        private string username, firstName, lastName, hashedPassword;
        //Temporarally restrict permissions.
        //private Perms.PermissionCollection permissions;
        private AccountType accountType = AccountType.USER; //User is the defaut type.
        private bool loggedIn = false, enabled = true, canLogin = true;
        private readonly DateTime timeCreated;

        public User(string username, string rawPassword, string firstName = "", string lastName = "", AccountType accountType = AccountType.USER)
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            //Load the permissions based on the type.
            //permissions = Perms.PermissionLoader.GetPermissionSet(this.accountType);
            //Set account properties.
            if (accountType == AccountType.ADMIN || accountType == AccountType.CHILD || accountType == AccountType.GUEST || accountType == AccountType.USER)
                CanLogin = true;
            else
                CanLogin = false;
            this.accountType = accountType;
            //Load hash.
            timeCreated = DateTime.Now;
            hashedPassword = (!(Utilities.Utilities.IsNullWhiteSpaceOrEmpty(rawPassword)) ? Hash(FormatForHash(rawPassword)) : "");
        }

        public string GetUsername() => username;
        public string GetFirstName() => firstName;
        public string GetLastName() => lastName;
        public string GetName(NameFormat nameFormat = NameFormat.Last_First)
        {
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(GetFirstName()) || Utilities.Utilities.IsNullWhiteSpaceOrEmpty(GetLastName()))
                return null;
            if (nameFormat == NameFormat.Last_First)
                return $"{GetLastName()}, {GetFirstName()}";
            else
                return $"{GetFirstName()} {GetLastName()}";
        }

        public AccountType GetAccountType() => accountType;

        public bool ChangePassword(string currentPassword, string newPassword)
        {
            //TODO: Check for permissions.
            //Check if the account has the ability to change password.
            if (!(GetAccountType() == AccountType.ADMIN || GetAccountType() == AccountType.USER || GetAccountType() == AccountType.CHILD))
                return false;
            else
            {
                //For now, the user can change their own password. Admins should be able to change reguardless, and child and standard accounts must have the permission to change. User accounts have the ability by default. (This permission is not yet implemented.)
                //Check the validity of the password.
                var x = FormatForHash(currentPassword);
                var hash = Hash(x);
                if (hashedPassword == hash)
                {
                    //Change the password.
                    var newX = FormatForHash(newPassword);
                    var newHash = Hash(newX);
                    hashedPassword = newHash;
                    return true;
                }
                else
                    return false;
            }
        }
        public bool ChangePassword(User user, string newPassword)
        {
            //Check if the current user has either the admin type or the modify users permission.
            if (accountType == AccountType.ADMIN || accountType == AccountType.SYSTEM)
            {
                var x = FormatForHash(newPassword);
                var hash = Hash(x);
                user.hashedPassword = hash;
                return true;
            }
            else
            {
                /* //Check for permission.
                var perm = permissions.GetPermission<bool>("CanUpdateUsrs");
                if ((perm != null) && perm.GetValue() == true)
                {
                    //Change the password of the specified user.
                    var x = FormatForHash(newPassword);
                    var hash = Hash(x);
                    user.hashedPassword = hash;
                    return true;
                }
                else
                    return false; */
                return false;
            }
        }

        //Checks the password provided to login.
        public AccountAccessStatus Login(string password)
        {
            if (LoginAllowed)
            {
                if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(hashedPassword) || GetAccountType() == AccountType.GUEST)
                    return AccountAccessStatus.NO_PASSWORD_REQUIRED;
                else
                {
                    //"Hashed" string is not empty and account is not guest.
                    if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(password))
                        return AccountAccessStatus.NO_PASSWORD_PROVIDED;
                    else
                    {
                        var x = FormatForHash(password);
                        var hash = Hash(x);
                        //Check password.
                        if (hashedPassword == hash)
                            return AccountAccessStatus.ACCESS_GRANTED;
                        else
                            return AccountAccessStatus.ACCESS_DENIED;
                    }
                }
            }
            else
                return AccountAccessStatus.DISABLED;
        }

        public void Logout()
        {
            if (IsLoggedIn)
                IsLoggedIn = false;
        }

        public bool IsLoggedIn
        {
            get => loggedIn;
            internal set => loggedIn = value;
        }

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public bool CanLogin
        {
            get => canLogin;
            internal set => canLogin = value;
        }

        /// <summary>
        /// Setting this to true will allow an account to be logged in even if prohibited so use with caution.
        /// </summary>
        public bool LoginAllowed
        {
            get => Enabled && CanLogin;
            internal set
            {
                Enabled = value;
                CanLogin = value;
            }
        }

        public DateTime GetTimeCreated() => timeCreated;

        public bool ChangeAccountType(User user, AccountType accountType)
        {
            //var perm = GetPermissions().GetPermission<bool>("CanUpdateUsrs");
            /*(perm != null && perm.GetValue() == true ) */
            if ((GetAccountType() == AccountType.ADMIN || GetAccountType() == AccountType.SYSTEM))
            {
                //Change the type of the user, but make sure the user is not a system or recovery account or is not this.
                if (!(user == this && (user.GetAccountType() == AccountType.SYSTEM || user.GetAccountType() == AccountType.RECOVERY)))
                {
                    //Load new permission set.
                    //permissions = Perms.PermissionLoader.GetPermissionSet(accountType);
                    if (!CanLogin)
                        CanLogin = true;
                    //Change type.
                    user.accountType = accountType;
                    return true;
                }
                else
                    //Prohibit changing the type, return false.
                    return false;
            }
            else
                return false; //Is not a system or admin account or the account does not have the required permission.
        }

        //public Perms.IReadOnlyPermissionCollection GetPermissions() => permissions;

        private string FormatForHash(string pass)
        {
            //var dtString = timeCreated.ToString("HH_mm_ss-yyyy_MM_dd");
            var hasher = "__" + pass + "__";
            return hasher;
        }

        internal string Hash(string stringToHash)
        {
            return Security.SHA256.hash(stringToHash);
        }

        public static User Guest = new User("Guest", "", accountType: AccountType.GUEST)
        {
            Enabled = false
        };
        //Have OOBE set pasword if the user wishes, otherwise keep this empty.
        public static User Admin = new User("Admin", "", accountType: AccountType.ADMIN);
        //This account is only for running system-based actions, such as changing passwords, and running system-based commands.
        public static User System = new User("System", "", accountType: AccountType.SYSTEM);
        //Use LoginAllowed to force enable this user.
        public static User Recovery = new User("Recovery", "", accountType: AccountType.RECOVERY);

        internal static User[] GetTemporaryUserRegistry() => new User[]
        {
            Guest,
            Admin,
            System,
            Recovery
        };

        //Temporary
        public static User GetUserInternal(string username)
        {
            foreach (User usr in GetTemporaryUserRegistry())
            {
                if (usr.GetUsername().ToLower().Equals(username.ToLower()))
                    return usr;
                else continue;
            }
            return null;
        }

        //Temporary
        public static AccountAccessStatus Login(string username, string password, out User user)
        {
            //TODO: Perform this step in the UserRegistry instead.
            //For now verify login with only the username and password.
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(username))
            {
                user = null;
                return AccountAccessStatus.NO_USERNAME_PROVIDED;
            }
            else
            {
                foreach (User usr in GetTemporaryUserRegistry())
                {
                    if (usr.GetUsername().ToLower().Equals(username.ToLower()))
                    {
                        //Username is valid.
                        user = usr;
                        return usr.Login(password);
                    }
                    else continue;
                }
                user = null;
                return AccountAccessStatus.ACCESS_DENIED;
            }
        }
    }

    public sealed class UserRegistry
    {
        private List<User> users;

        public UserRegistry()
        {

        }
    }
}
