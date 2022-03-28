using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command
{
    public sealed class CommandData
    {
        private string commandName, commandDescription, commandAuthor;
        private string[] commandAliases;
        private bool hideCommand, canHide;

        public CommandData(string name, string description = "", string[] cmdAliases = null, bool hideCommand = false, string developer = "")
        {
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(name))
                throw new ArgumentNullException("The paramter with the name: \"name\" cannot be null, whitespace, or empty.");
            commandDescription = description;
            commandAuthor = developer;
            commandAliases = cmdAliases;
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
            this.commandName = name;
        }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        /// <returns></returns>
        public string GetName() => commandName;
        /// <summary>
        /// Gets the description of what the command does.
        /// </summary>
        /// <returns></returns>
        public string GetDescription() => commandDescription;
        /// <summary>
        /// Gets the alternate names of the command.
        /// </summary>
        /// <returns></returns>
        public string[] GetAliases() => commandAliases;

        /// <summary>
        /// Gets or sets whether this command is hidden (This property cannot be set if the command was hidden using a hide prefix.)
        /// </summary>
        public bool IsCommandHidden
        {
            get => hideCommand;
            set
            {
                if (canHide)
                    hideCommand = value;
            }
        }

        //For setting hidden status reguradless of canHide.
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
                commandName = s;
                canHide = false;
                IsCommandHidden = true;
            }
            else
                commandName = newName;
        }
    }
}
