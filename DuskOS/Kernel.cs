/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          Kernel
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *                  WinMister332/Chris Emberley <cemberley@nerdhub.net>
 */

using System;
using System.Net.NetworkInformation;
using Sys = Cosmos.System;
using Debug = Cosmos.Debug.Kernel;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using CommandSharp;
using CommandSharp.Commands;
using CommandSharp.Commands.Users;
using CommandSharp.Commands.System;
using Cosmos.HAL.Network;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using DuskOS.CommandSharp.Commands.Network;
using DuskOS.System.Modules.Network;
using DuskOS.CommandSharp.Commands;
using DuskOS.CommandSharp.Commands.Filesystem;
using DuskOS.CommandSharp.Commands.System;
using DuskOS.System;
using DuskOS.System.Registry;
using DuskOS.System.Security.Permissions;

//using DuskResearchKenrel.System.Users;

namespace DuskOS
{
    public class Kernel : Sys.Kernel
    {
        public static Debug.Debugger debugger = new Debug.Debugger("DuskOS", "Kernel");
        public static CosmosVFS fs = new CosmosVFS();
        public static string currentdir = @"0:\";
        public static System.Users.User currentuser = null;
        public static string computername = "DuskOS";
        public static string currentvol = @"0:\";
        public static string version = "1.0-RK";
        public static OSEdition edition = OSEdition.BASIC;
        public static readonly string os = "DuskOS";
        public static string osString = $"{os} {GetEditionName(edition)} {version}";

        public static CommandPrompt prompt = null;
        private static Registry registry = null;

        protected override void BeforeRun()
        {
            VFSManager.RegisterVFS(fs);
            //fs.Format("0", "FAT32", false);
            //DuskFS.InitFileSystem();
           // Console.WriteLine("Done!");
            //Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
            try
            {
                if (registry == null)
                    registry = new Registry();
                registry.Add("OSName", os);
                registry.Add("OSVer", version);
                registry.Add("OSVerStr", osString);
                registry.Add("OSEdition", GetEditionName(edition));
                
                foreach (var x in registry)
                {
                    string val = $"{x.GetKey()}: {x.GetValue()}";
                    Console.WriteLine(val);
                }

                //foreach (RegistryItem item in registry.GetItems())
                //{
                //    var key = item.GetKey();
                //    var value = item.GetValue().GetValue();
                //    var str = (string)value;
                //    Console.WriteLine($"{key}: {str}");
                //}

                #region Init FileSystem

                if (!DuskFS.DuskFSExists())
                {
                    DuskFS.InitFileSystem(fs);
                }

                #endregion

                #region CommandSharp Init

                prompt = new CommandPrompt();
                prompt.MachineName = "DuskOS";
                prompt.CurrentDirectory = @"0:\";
                //TODO: Bring back formatted EchoMessage class.
                //Do not change prompt.Message until EchoMessage class is reimplemented.

                //Register commands.

                #region Native CommandSharp

                GetInvoker().Register(new DummyCommand());

                #endregion

                #region FileSystem

                GetInvoker().Register(new CatCommand());
                GetInvoker().Register(new CDCommand());
                GetInvoker().Register(new EditorCommand());
                //GetInvoker().Register(new FormatCommand());
                GetInvoker().Register(new ListDirectoryCommand());
                GetInvoker().Register(new MkdirCommand());
                GetInvoker().Register(new MkfileCommand());
                GetInvoker().Register(new RmdirCommand());
                GetInvoker().Register(new RmfileCommand());
                GetInvoker().Register(new TreeCommand());

                #endregion

                #region Network
                // GetInvoker().Register(new IPConfigCommand());
                // GetInvoker().Register(new PingCommand());
                #endregion

                #region System

                GetInvoker().Register(new HashCommand());
                GetInvoker().Register(new ListDataCommand());
                GetInvoker().Register(new RebootCommand());
                GetInvoker().Register(new RegistryCommand());
                GetInvoker().Register(new ShutdownCommand());
                GetInvoker().Register(new TimeCommand());

                #endregion

                #region Terminal

                #endregion

                #region Users

                GetInvoker().Register(new ListPermssions());
                //login
                GetInvoker().Register(new LogoutCommand());
                GetInvoker().Register(new UsersCommand());

                #endregion

                #region Test Command

                GetInvoker().Register(new TestSwitch());

                #endregion

                #endregion

                #region Network init
                //currently disabled
                //NetworkManager.Initialize();
                #endregion

                //TODO: Move these elsewhere.
                //Load Permissions.
                PermissionLoader.LoadConfigurationPermssions();
                PermissionLoader.LoadFilePermssions();
                PermissionLoader.LoadSpecialPermssions();
                 //Display all permissions.
                var perms = PermissionRegistry.INSTANCE;
               
                foreach (Permission p in perms.GetAllPermissions())
                {
                    string s = $"{p.GetName()} {(string)p.GetValue()}";
                    Console.WriteLine(s);
                }

                //TODO: Should be false by default for all editions except Desktop, Professional, and Ultimate!
                System.Users.LoginManager.AllowAutoLogin = true;
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.Clear();
        }

        bool skipAutoLogin = false;
        protected override void Run()
        {
            // Console.Write("Input: ");
            // var input = Console.ReadLine();
            // Console.Write("Text typed: ");
            //Console.WriteLine(input);
            // Console.WriteLine(LanguageFiles.test());

            if (currentuser != null)
            {
                //Show command prompt.
                prompt.Prompt(false); //Have COSMOS run this for us. Be sure loop is off.
            }
            else
            {
                if (System.Users.LoginManager.AllowAutoLogin && !skipAutoLogin)
                {
                    System.Users.LoginManager.SupressMessages = true;
                    var usrs = System.Users.User.GetTemporaryUserRegistry();
                    foreach (System.Users.User user in usrs)
                    {
                        //The currentuser value is populated automatically if a login is valid.
                        var x = System.Users.LoginManager.Login(user.GetUsername(), "", out System.Users.User u);
                        //The above is for optional checking.
                    }
                    System.Users.LoginManager.SupressMessages = false;
                    skipAutoLogin = true;
                }
                else
                {
                    //Show login prompt.
                    System.Users.LoginManager.Login(out System.Users.User user, out System.Users.AccountAccessStatus accessStatus);
                }
            }
        }

        private static string GetEditionName(OSEdition edition)
            => GetEditionName((int)edition);

        public static string GetEditionName(int editionID)
        {
            switch (editionID)
            {
                case (1):
                    return "Desktop Basic";
                case (2):
                    return "Desktop";
                case (3):
                    return "Desktop Professional";
                case (4):
                    return "Desktop Ultimate";
                case (5):
                    return "Datacenter Server";
                default: return "Basic";
            }
        }

        public static CommandPrompt GetPrompt() => prompt;
        public static CommandInvoker GetInvoker() => GetPrompt().GetInvoker();
        internal static Registry GetRegistry() => registry;
    }
}


