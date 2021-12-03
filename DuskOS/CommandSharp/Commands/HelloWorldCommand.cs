/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/HelloWorldCommand.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*
*/

using System;

namespace CommandSharp.Commands
{
    public class HelloWorldCommand : Command
    {
        private static readonly CommandData data = new CommandData("hello", "Says hello to the world.", new string[] { "helloworld" });

        public HelloWorldCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.WriteLine("Hello World! This is a command invoked from CommandSharp in Oracle VM VirtualBox!");
            return true; //True means the command invoked successfully.
        }
    }
}
