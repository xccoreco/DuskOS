using Cosmos.Core.Memory;
using Cosmos.Debug.Kernel;
using Cosmos.HAL;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using DuskOSDev.DuskSystem.Command;
using DuskOSDev.DuskSystem.Command.Commands;
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
        #endregion

        #region Graphics

        //public static VGAScreen VGAScreen = new VGAScreen();

        #endregion

        #region Resources

        #endregion

        #region Processes
        private static User user;
        private static CommandPrompt prompt;
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

            //VGAScreen = new VGAScreen();
            //VGAScreen.SetGraphicsMode(VGADriver.ScreenSize.Size720x480, ColorDepth.ColorDepth16);
            //VGAScreen.SetTextMode(VGADriver.TextSize.Size90x60);

            #endregion

            #region CommandUtils Init

            GetInvoker().Register(new CommandHelp());

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
