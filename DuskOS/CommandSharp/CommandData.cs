/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/CommandData.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*
*/

using DuskOS.System.Utilities;
using DuskOS.System.Users;
using System;

namespace CommandSharp.Commands
{
    public sealed class CommandData
    {
        private string name, description;
        private string[] aliases;
        private bool hideCommand, canHide;
        private AccountType[] authorizedAccounts;

        //This defines all the types have access.
        private static readonly AccountType[] defaultAccountUsage = new AccountType[]
        { 
            AccountType.RECOVERY,
            AccountType.SYSTEM,
            AccountType.ADMIN,
            AccountType.USER,
            AccountType.CHILD,
            AccountType.GUEST
        };     

        public CommandData(string name, string description = "", string[] cmdAliases = null, bool hideCommand = false, AccountType[] authorizedAccountTypes = null)
        {
            if (Utilities.IsNullWhiteSpaceOrEmpty(name))
                throw new ArgumentNullException("The paramter with the name: \"name\" cannot be null, whitespace, or empty.");
            this.description = description ?? "";
            aliases = cmdAliases ?? new string[0];
            this.hideCommand = hideCommand; //Can only be true or false, so it doesn't matter what the value is.

            if (authorizedAccountTypes == null)
                authorizedAccountTypes = defaultAccountUsage;

            authorizedAccounts = authorizedAccountTypes;

            if (name.StartsWith("#") || name.StartsWith("@") || name.StartsWith(".") || name.StartsWith("!"))
            {
                string s = "";
                for (int i = 0; i < name.Length; i++)
                {
                    if (i > 0)
                        s += name[i];
                    else continue;
                }
                name = s;
                canHide = false;
                SetCommandHidden(true);
            }
            else
            {
                canHide = true;
                SetCommandHidden(hideCommand);
            }
            this.name = name;
        }

        public string GetName() => name;
        public string GetDescription() => description;
        public string[] GetAliases() => aliases;

        public AccountType[] GetAuthorizedAccountTypes() => authorizedAccounts;

        public bool IsCommandHidden
        {
            get => hideCommand;
            set
            {
                if (canHide)
                    hideCommand = value;
            }
        }

        //For setting hidden status reguradless of canSet.
        internal void SetCommandHidden(bool value)
            => hideCommand = value;

        internal void ChangeName(string newName)
        {
            if (newName.StartsWith("#") || newName.StartsWith("@") || newName.StartsWith(".") || newName.StartsWith("!"))
            {
                string s = "";
                for (int i = 0; i < newName.Length; i++)
                {
                    if (i > 1)
                        s += newName[i];
                    else continue;
                }
                name = s;
                canHide = false;
                IsCommandHidden = true;
            }
            else
                name = newName;
        }
    }
}