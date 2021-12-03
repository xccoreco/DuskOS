/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/Command.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*
*/

using System;

namespace CommandSharp.Commands
{
    public abstract class Command
    {
        private CommandData data;

        protected Command(CommandData data)
        {
            if (data != null)
                this.data = data;
            else
                throw new ArgumentNullException("The parameter \"data\" cannot be null, a value MUST be provided.");
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