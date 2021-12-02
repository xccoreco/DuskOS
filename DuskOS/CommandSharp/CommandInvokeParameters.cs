using System;
using System.Collections;
using System.Collections.Generic;
namespace CommandSharp.Commands
{
    public class CommandInvokeParameters
    {
        public CommandArguments Arguments { get; internal set; }
        public CommandInvoker Invoker { get; internal set; }
        public CommandPrompt Prompt { get; internal set; }
    }
}