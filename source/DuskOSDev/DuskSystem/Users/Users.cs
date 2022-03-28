using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Users
{
    public class Users
    {
        internal static Users INSTANCE = new Users();

        private List<User> users;

        Users()
        {
            users = new List<User>();
            new User("Recovery", "", User.AccountType.RECOVERY);
            new User("System", "", User.AccountType.SYSTEM);
            new User("Guest", "", User.AccountType.GUEST);
        }

        public void Register(User user)
        {
            //Make sure the user does not exist and make sure there isn't already an existing RECOVERY or SYSTEM account.
            if (!GetUsers().Contains(user) && user.GetAccountType() == User.AccountType.RECOVERY && !HasRecoveryAccount() && user.GetAccountType() == User.AccountType.SYSTEM && !HasSystemAccount())
                users.Add(user);
        }

        public void Unregister(User user)
        {
            users.Remove(user);
        }

        public User[] GetUsers() => users.ToArray();

        public User GetUserByUsername(string username)
        {
            foreach (User user in GetUsers())
            {
                if (user.GetUsername().EqualsLowerCase(username))
                    return user;
                else continue;
            }
            return null;
        }

        public User GetUserByUID(string uid)
            => GetUserByUID(UserID.Parse(uid));

        public User GetUserByUID(UserID uid)
        {
            foreach (User user in GetUsers())
            {
                if (user.GetUserID().Equals(uid))
                    return user;
                else continue;
            }
            return null;
        }

        public User GetUserByEmail(string email)
        {
            foreach (User user in GetUsers())
            {
                if (user.GetEmailAddress().EqualsLowerCase(email))
                    return user;
                else continue;
            }
            return null;
        }

        public User GetUser(string value)
        {
            var uuname = GetUserByUsername(value);
            var uuid = GetUserByUID(value);

            return uuname ?? uuid ?? null;
        }

        public User[] GetAdminUsers()
        {
            List<User> admins = new List<User>();
            foreach (User user in GetUsers())
            {
                if (user.GetAccountType() == User.AccountType.ADMIN)
                    admins.Add(user);
                else continue;
            }
            return admins.ToArray();
        }

        public User GetGuestUser()
        {
            foreach (User user in GetUsers())
            {
                if (user.GetAccountType() == User.AccountType.GUEST)
                    return user;
                else continue;
            }
            return null;
        }

        public User[] GetChildUsers()
        {
            List<User> children = new List<User>();
            foreach (User user in GetUsers())
            {
                if (user.GetAccountType() == User.AccountType.CHILD)
                    children.Add(user);
                else continue;
            }
            return null;
        }

        public bool HasAdminWithPassword()
        {
            foreach (User user in GetAdminUsers())
            {
                if (user.HasPassword())
                    return true;
                else continue;
            }
            return false;
        }

        public bool HasSystemAccount()
        {
            foreach (User user in GetUsers())
            {
                if (user.GetAccountType() == User.AccountType.SYSTEM)
                    return true;
                else continue;
            }
            return false;
        }

        public bool HasRecoveryAccount()
        {
            foreach (User user in GetUsers())
            {
                if (user.GetAccountType() == User.AccountType.RECOVERY)
                    return true;
                else continue;
            }
            return false;
        }

        public void Destroy(User user)
        {
            Unregister(user);
            user = null;
        }
    }
}
