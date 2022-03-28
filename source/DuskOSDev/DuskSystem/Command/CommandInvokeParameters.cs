using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command
{
    public class CommandInvokeParameters
    {
        public CommandArguments Arguments { get; internal set; }
        public CommandInvoker Invoker { get; internal set; }
        public CommandPrompt Prompt { get; internal set; }
    }
}
