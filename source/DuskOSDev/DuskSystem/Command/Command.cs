using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command
{
    public abstract class Command
    {
        private CommandData data;

        public Command(CommandData data)
        {
            if (data == null)
                throw new ArgumentNullException("The parameter \"data\" cannot be null, a value MUST be provided.");
            this.data = data;
        }

        public CommandData GetData() => data;
        public string GetName() => GetData().GetName();
        public string GetDescription() => GetData().GetDescription();
        public string[] GetAliases() => GetData().GetAliases();
        public bool IsCommandHidden => GetData().IsCommandHidden;

        //If true, command ran normally. If false, throw syntax.
        /// <summary>
        /// This function is the entrypoint of the command. Any code placed in this function will run when the command is called.
        /// </summary>
        /// <param name="e">The parameters containing information on the command.</param>
        /// <returns>Return <see langword="false"/> to return a syntax error.</see></returns>
        public abstract bool OnInvoke(CommandInvokeParameters e);
        /// <summary>
        /// This function handles the processing and creation of a syntax message.
        /// </summary>
        /// <param name="e">The parameters containing information vital for creating a syntax message.</param>
        /// <returns>The syntax message.</returns>
        public virtual string OnSyntaxError(SyntaxErrorParameters e) { return ""; }
    }
}
