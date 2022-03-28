using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Users
{
    public class User
    {
        public enum AccountType
        {
            GUEST = -2,
            CHILD = -1,
            USER = 0,
            ADMIN = 1,
            SYSTEM = 2,
            RECOVERY = 3
        }

        public enum AccessStatus
        {
            /// <summary>
            /// The account is disabled.
            /// </summary>
            DISABLED = 0,
            NO_USERNAME_PROVIDED = -5,
            TIME_LIMIT_EXPIRED = -4,
            CURFEW_REACHED = -3,
            NO_PASSWORD_PROVIDED = -2,
            ACCESS_DENIED = -1,
            NO_PASSWORD_REQUIRED = 1,
            ACCESS_GRANTED = 2
        }

        private string username, passwordHash, email, firstName, lastName;
        private DateTime timeCreated;
        private DateTime? lastLogin;
        private bool isLoggedIn = false, enabled = true;
        private UserID uid;
        private AccountType accountType = AccountType.USER;

        public User(string username, string password, AccountType accountType = AccountType.USER, string email = "", string firstName = "", string lastName = "")
        {
            uid = new UserID();
            if (username.IsNullWhiteSpaceOrEmpty())
                throw new ArgumentNullException("username", "You must provide a username to any user account.");

            passwordHash = GetHash(password);
            this.accountType = accountType;
            timeCreated = DateTime.Now;

            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;

            Users.INSTANCE.Register(this);

            Console.WriteLine("Created User with ID: " + uid.ToString());

        }

        internal User(string username, string passHash, AccountType accountType, string email, string firstName, string lastName, DateTime timeCreated, DateTime? lastLogin, UserID uid)
        {
            if (username.IsNullOrEmpty())
                throw new ArgumentNullException("username", "The username cannot be null.");
            if (passHash.IsNullOrEmpty())
                throw new ArgumentNullException("passHash", "A password hash cannot be empty or null.");
            this.email = email ?? "";
            this.firstName = firstName ?? "";
            this.lastName = lastName ?? "";
            this.timeCreated = timeCreated;
            this.lastLogin = lastLogin;
            this.uid = uid ?? new UserID();
        }

        public string GetUsername() => username;
        public string GetPasswordHash() => passwordHash;
        public string GetEmailAddress() => email;
        public string GetFirstName() => firstName;
        public string GetLastName() => lastName;

        public DateTime GetCreationDate() => timeCreated;
        public DateTime? GetLastLogin() => lastLogin;

        public AccountType GetAccountType() => accountType;

        public UserID GetUserID() => uid;

        public bool IsLoggedIn
        {
            get => isLoggedIn;
            private set => isLoggedIn = value;
        }

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public bool CanLogin
        {
            get => !(GetAccountType() == AccountType.SYSTEM && GetAccountType() == AccountType.RECOVERY) && Enabled;
        }

        public bool CanSetPassword()
            => !(GetAccountType() == AccountType.SYSTEM || GetAccountType() == AccountType.RECOVERY || GetAccountType() == AccountType.GUEST);

        public bool HasPassword()
            => !(HashEquals("") || HashEquals(" ") || HashEquals(null));

        public AccessStatus UserLogin()
        {
            var x = Input.InputHandler.HandleInput("Password: ", true);
            if (x.IsNullWhiteSpaceOrEmpty() && (!(GetAccountType() == AccountType.GUEST) || !HasPassword()))
                return AccessStatus.NO_PASSWORD_PROVIDED;
            else
                return (AccessStatus.NO_PASSWORD_REQUIRED | AccessStatus.ACCESS_GRANTED);
            x = GetHash(x);
            if (GetPasswordHash() == x)
                return AccessStatus.ACCESS_GRANTED;
            return AccessStatus.ACCESS_DENIED;
        }

        //Used to format the pre-hashed string in a way where there will always be a hash even if the password is null or empty.
        private string GetPreHash(string password)
            => $"[{GetUserID()}]__{password}__({GetCreationDate().ToString("yyyy/MM/dd @ HH:mm:ss K")})";

        private string GetHash(string password)
            => Security.SHA256.ComputeHash(Encoding.UTF8.GetBytes(GetPreHash(password)));

        public void Delete()
            => Users.INSTANCE.Destroy(this);

        public void ChangeUsername(string username)
            => this.username = username;

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (HashEquals(oldPassword))
            {
                passwordHash = GetHash(newPassword);
                return true;
            }
            return false;
        }

        public bool ChangePassword(User user, string newPassword)
        {
            if (GetAccountType() == AccountType.SYSTEM || GetAccountType() == AccountType.ADMIN || GetAccountType() == AccountType.RECOVERY && !(user.GetAccountType() == AccountType.SYSTEM || user.GetAccountType() == AccountType.RECOVERY))
            {
                user.passwordHash = GetHash(newPassword);
                return true;
            }
            return false;
        }

        internal bool HashEquals(string rawValue)
            => GetPasswordHash() == GetHash(rawValue);

        public static User GetUserByUsername(string username)
            => Users.INSTANCE.GetUserByUsername(username);

        public static User GetUserByUserID(UserID uid)
            => Users.INSTANCE.GetUserByUID(uid);

        public static User GetUserByUserID(string uid)
            => GetUserByUserID(uid);

        public static User GetUser(string value)
            => Users.INSTANCE.GetUser(value);

        public static User[] GetAllUsers()
            => Users.INSTANCE.GetUsers();

        public static User[] GetAdminUsers()
            => Users.INSTANCE.GetAdminUsers();

        public static User[] GetChildUsers()
            => Users.INSTANCE.GetChildUsers();

        public static bool UserExists(string value)
            => GetUser(value) != null;

        public static User.AccessStatus Login()
        {
            var x = Input.InputHandler.HandleInput("Username: ");
            if (x.IsNullWhiteSpaceOrEmpty())
                return AccessStatus.NO_USERNAME_PROVIDED;
            else
            {
                var uuname = Users.INSTANCE.GetUserByUsername(x);
                var uemail = Users.INSTANCE.GetUserByEmail(x);

                var u = uuname ?? uemail;
                if (u == null)
                    return AccessStatus.ACCESS_DENIED;
                else
                    return u.UserLogin();
            }
        }

        public static void CreateStandardUser(string username, string password, string email = "", string firstName = "", string lastName = "")
            => new User(username, password, AccountType.USER, email, firstName, lastName);
        public static void CreateAdminUser(string username, string password, string email = "", string firstName = "", string lastName = "")
            => new User(username, password, AccountType.ADMIN, email, firstName, lastName);
        //It's currently not setup like this, but there needs to 
        public static void CreateChildUser(string username, string password, string email = "", string firstName = "", string lastName = "")
            => new User(username, password, AccountType.CHILD, email, firstName, lastName);
    }

    public sealed class UserID
    {
        private string[] uid = null;
        private static readonly short factor = 4;
        private static readonly short size = 32;

        public UserID()
        {
            uid = new string[factor];
            Generate();
        }

        UserID(string[] data)
            => uid = data;

        private void Generate()
        {
            var nLength = 32 / factor;
            for (int i = 0; i < uid.Length; i++)
                uid[i] = GenerateSegment(nLength);
        }

        private string GenerateSegment(int length)
        {
            var alpha = new AlphaNumericGenerator(length).ToString();
            return (!uid.Contains(alpha)) ? alpha : GenerateSegment(length);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string s in uid)
            {
                if (builder.Length > 0)
                    builder.Append($"-{s}");
                else
                    builder.Append(s);
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is UserID)
                return ((UserID)obj).ToString() == ToString();
            else return false;
        }

        public static UserID Parse(string s)
        {
            if (s.Length != 32)
                throw new ArgumentOutOfRangeException("s", "The size of the UID to parse is invalid, it must be a length of 32");
            if ((s.StartsWith("{") && s.EndsWith("}")) || (s.StartsWith("[") && s.EndsWith("]")) || (s.StartsWith("(") && s.EndsWith(")")))
            {
                s = s.Remove(0, 1);
                s = s.Remove(s.Length - 1, 1);
            }

            if (s.Contains("-"))
            {
                var x = s.Split(spl: "-");
                var n = new UserID(x);
                return n;
            }
            else
                throw new Exception("The input string is of an invalid format.");
        }
    }
}
