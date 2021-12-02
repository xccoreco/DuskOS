using DuskOS.System.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandSharp
{
    public class CommandPrompt
    {
        private CommandInvoker invoker;

        public bool DisplayEcho { get; set; } = true;
        public string Message { get; set; } = "";
        public string CurrentUser { get; set; } = "Admin";
        public string MachineName { get; set; } = "CommandSharp";
        public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor CurrentForegroundColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor CurrentBackgroundColor { get; set; } = Console.BackgroundColor;
        public bool Debug { get; set; } = false;
        public string CurrentDirectory { get; set; } = "";
        
        private string defaultMsg = $"";
        private readonly string rawMsg = $"[$%currUsr%@%sys%]: %dir% > ";

        public CommandPrompt(CommandInvoker invoker = null)
        {

            if (invoker == null)
                this.invoker = new CommandInvoker();
            else
                this.invoker = invoker;

            //Set the default colors.
            CurrentForegroundColor = DefaultForegroundColor;
            CurrentBackgroundColor = CurrentBackgroundColor;
            Debug = false;
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentUser))
                CurrentUser = "Admin";
            if (Utilities.IsNullWhiteSpaceOrEmpty(MachineName))
                MachineName = "CommandSharp";
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentDirectory))
                CurrentDirectory = @"C:\";
            if (Utilities.IsNullWhiteSpaceOrEmpty(Message))
            {
                defaultMsg = rawMsg;
                if (defaultMsg.Contains("%sys%"))
                {
                    defaultMsg = defaultMsg.Replace("%currUsr%", CurrentUser);
                    defaultMsg = defaultMsg.Replace("%sys%", MachineName);
                    defaultMsg = defaultMsg.Replace("%dir%", CurrentDirectory);
                    Message = defaultMsg;
                }
                else
                    Message = defaultMsg;
            }

            this.invoker.SetCommandPrompt(this);
        }

        public CommandInvoker GetInvoker() => invoker;

        private bool exitLoop = false;

        public void Prompt(bool loop = true)
        {
            try
            {
                if (loop)
                {
                    do
                        DisplayPrompt();
                    while (loop && !exitLoop);
                }
                else
                    DisplayPrompt();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void DisplayPrompt()
        {
            //Check whether to update the message.
            if (Message == defaultMsg)
            {
                defaultMsg = rawMsg;
                if (defaultMsg.Contains("%sys%"))
                {
                    defaultMsg = defaultMsg.Replace("%currUsr%", CurrentUser);
                    defaultMsg = defaultMsg.Replace("%sys%", MachineName);
                    defaultMsg = defaultMsg.Replace("%dir%", CurrentDirectory);
                    Message = defaultMsg;
                }
            }

            if (DisplayEcho)
                Console.Write(Message);

            var input = Console.ReadLine();
            if (!Utilities.IsNullWhiteSpaceOrEmpty(input))
            {
                if (Debug)
                    Console.WriteLine("Sending \"" + input + "\" to the command invoker...");

                invoker.Invoke(input);
            }
            else
                return;
        }

        public bool ExitLoop
        {
            get => exitLoop;
            set => exitLoop = value;
        }
    }
}
