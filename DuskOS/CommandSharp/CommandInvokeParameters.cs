/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/CommandInvokeParameters.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*
*/

namespace CommandSharp.Commands
{
    public class CommandInvokeParameters
    {
        public CommandArguments Arguments { get; internal set; }
        public CommandInvoker Invoker { get; internal set; }
        public CommandPrompt Prompt { get; internal set; }
    }
}