using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuskOSDev.DuskSystem.Utilities;
namespace DuskOSDev.DuskSystem.Command
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
        public ConsoleColor CurrentForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        public ConsoleColor CurrentBackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }
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

            Debug = false;
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(CurrentUser))
                CurrentUser = "Admin";
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(MachineName))
                MachineName = "CommandSharp";
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(CurrentDirectory))
                CurrentDirectory = @"C:\";
            if (Utilities.Utilities.IsNullWhiteSpaceOrEmpty(Message))
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

        private bool exitLoop = false, doOnce = true;

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
            if (doOnce)
            {
                CurrentBackgroundColor = DefaultBackgroundColor;
                CurrentForegroundColor = DefaultForegroundColor;
                Console.Clear();
                doOnce = false;
            }

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
            if (!Utilities.Utilities.IsNullWhiteSpaceOrEmpty(input))
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
