/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/DuskFS.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using Cosmos.System.FileSystem;
using System.IO;
using System.Runtime.InteropServices;

namespace DuskOS.System
{
    public static class DuskFS
    {
        private static readonly string default_path = @"0:\";
        public static void InitFileSystem(CosmosVFS fs/*CosmosVFS fs, bool format*/)
        {
            //if (format) FormatFileSystem(fs);
            if (!DuskFSExists())
            {
                FormatFileSystem(fs);
            }

            #region Applications
            Directory.CreateDirectory(default_path + "Applications");
            Directory.CreateDirectory(default_path + @"Applications\x86");
            Directory.CreateDirectory(default_path + @"Applications\Startup");
            #endregion

            #region Users
            /*
             * Shared or Default | Guest
             * 
             * 
             */
            CreateNewUser("default");
            #endregion

            #region DuskOS
            Directory.CreateDirectory(default_path + @"DuskOS\System");
            Directory.CreateDirectory(default_path + @"DuskOS\System\Drivers");
            Directory.CreateDirectory(default_path + @"DuskOS\System\Languages");
            Directory.CreateDirectory(default_path + @"DuskOS\System\Configuration");
            Directory.CreateDirectory(default_path + @"DuskOS\System\Recovery");
            Directory.CreateDirectory(default_path + @"DuskOS\System\Recovery\Recovery");

            Directory.CreateDirectory(default_path + @"DuskOS\Logs");
            Directory.CreateDirectory(default_path + @"DuskOS\Shell");
            Directory.CreateDirectory(default_path + @"DuskOS\Media");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Ringtones");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Images");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Cursors");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Fonts");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Icons");
            Directory.CreateDirectory(default_path + @"DuskOS\Media\Icons\UserIcons");
            #endregion

        }
        public static bool DuskFSExists()
        {
            if (Directory.Exists(@"0:\DuskOS\"))
            {
                return true;
            }

            return false;
        }

        internal static void CreateNewUser(string username)
        {
            #region users
            Directory.CreateDirectory(default_path + @"Users\" + username); //0:\username
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Userdata"); //0:\username\Userdata
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Userdata\Temp");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Userdata\Applications");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Userdata\Applications\x86");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Userdata\Applications\Startup");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Desktop");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Documents");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Downloads");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Pictures");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Music");
            Directory.CreateDirectory(default_path + @"Users\" + username + @"\Videos");
            #endregion
        }

        public static void FormatFileSystem(CosmosVFS fs)
        {
            fs.Format("0", "FAT32", true);
        }

        public static void FormatFileSystem(CosmosVFS fs, string driveId, string driveFormat, bool quick)
        {
            fs.Format(driveId, driveFormat, quick);
        }
    }
}
