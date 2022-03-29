using Cosmos.Core.Memory;
using Cosmos.Debug.Kernel;
using Cosmos.HAL;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using DuskOSDev.DuskSystem.Command;
using DuskOSDev.DuskSystem.Command.Commands;
using DuskOSDev.DuskSystem.Command.Commands.DuskShell;
using DuskOSDev.DuskSystem.Command.Commands.Filesystem;
using DuskOSDev.DuskSystem.Command.Commands.System;
using DuskOSDev.DuskSystem.Command.Commands.Terminal;
using DuskOSDev.DuskSystem.Process;
using DuskOSDev.DuskSystem.Users;
using System;
using System.Collections.Generic;
using System.Text;
using DuskOSDev.DuskSystem.Common;
using Sys = Cosmos.System;
namespace DuskOSDev
{
    public class Kernel
    {
        #region Applications
        /*empty for now*/
        #endregion

        #region Graphics
        /*empty for now*/
        #endregion

        #region Resources
        /*empty for now*/
        #endregion

        #region Processes

        public static User user;
        public static CommandPrompt prompt;

        #endregion

        #region Properties

        public static string ComputerName = "DuskOS";
        public static string CurrentDirectory = @"0:\";
        public static int CurrentPartition = 0;

        #endregion

        public static void BeforeRun()
        {
            #region Initialization

            DuskFS.Initialize();

            prompt = new CommandPrompt();
            prompt.DefaultBackgroundColor = ConsoleColor.Blue;
            prompt.DefaultForegroundColor = ConsoleColor.White;
            prompt.CurrentUser = "Admin";
            prompt.Debug = false;

            #endregion

            #region CommandUtils Init

            #region Default

            GetInvoker().Register(new CommandHelp());

            #endregion

            #region DuskShell
            /*empty for now*/
            #endregion

            #region Filesystem

            GetInvoker().Register(new CommandCat());
            GetInvoker().Register(new CommandCD());
            GetInvoker().Register(new CommandListDirectory());
            GetInvoker().Register(new CommandMkdir());
            GetInvoker().Register(new CommandRmdir());
            GetInvoker().Register(new CommandRmfile());
            GetInvoker().Register(new CommandTouch());

            #endregion

            #region System

            GetInvoker().Register(new CommandReboot());
            GetInvoker().Register(new CommandShutdown());
            GetInvoker().Register(new CommandSystemInfo());

            #endregion

            #region Terminal

            GetInvoker().Register(new CommandClear());
            GetInvoker().Register(new CommandEcho());

            #endregion

            #endregion

        }

        public static void Run()
        {
            prompt.Prompt();
        }

        public static CommandPrompt GetPrompt() => prompt;
        public static CommandInvoker GetInvoker() => GetPrompt().GetInvoker();
    }
}
