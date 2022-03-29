using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;

namespace DuskOSDev.DuskSystem.Common
{
    public static class DuskFS
    {
        public static CosmosVFS FileSystem = new CosmosVFS();
        public static List<Disk> Disks;

        private static string _currentPath = "";
        private static int _currentPartition = 0;
        private static int _currentDisk = 0;
        public static void Initialize()
        {
            VFSManager.RegisterVFS(FileSystem);
            Disks = FileSystem.GetDisks();
            InitializeFS();
        }

        #region Filesystem Initialization & Users

        private static void InitializeFS()
        {
            if (!IsDuskFSInitialized())
            {
                Format(0);

                #region Applications
                Directory.CreateDirectory(@"0:\Applications");
                Directory.CreateDirectory(@"0:\Applications\x86");
                Directory.CreateDirectory(@"0:\Applications\Startup");
                #endregion

                #region Users
                CreateNewUser("default");
                #endregion

                #region DuskOS
                Directory.CreateDirectory(@"0:\DuskOS\System");
                Directory.CreateDirectory(@"0:\DuskOS\System\Drivers");
                Directory.CreateDirectory(@"0:\DuskOS\System\Languages");
                Directory.CreateDirectory(@"0:\DuskOS\System\Configuration");
                Directory.CreateDirectory(@"0:\DuskOS\System\Recovery");
                Directory.CreateDirectory(@"0:\DuskOS\System\Recovery\Recovery");

                Directory.CreateDirectory(@"0:\DuskOS\Logs");
                Directory.CreateDirectory(@"0:\DuskOS\Shell");
                Directory.CreateDirectory(@"0:\DuskOS\Media");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Ringtones");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Images");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Cursors");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Fonts");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Icons");
                Directory.CreateDirectory(@"0:\DuskOS\Media\Icons\UserIcons");
                #endregion
            }
        }

        private static bool IsDuskFSInitialized()
        {
            if (Directory.Exists(@"0:\DuskOS\"))
                return true;

            return false;
        }

        internal static void CreateNewUser(string username)
        {
            #region users
            Directory.CreateDirectory(@"0:\Users\" + username); //0:\username
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Userdata"); //0:\username\Userdata
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Userdata\Temp");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Userdata\Applications");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Userdata\Applications\x86");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Userdata\Applications\Startup");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Desktop");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Documents");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Downloads");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Pictures");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Music");
            Directory.CreateDirectory(@"0:\Users\" + username + @"\Videos");
            #endregion
        }

        #endregion

        #region Getters and Setters

        public static string GetCurrentPath => _currentPath;

        public static int CurrentPartition
        {
            get
            {
                return _currentPartition;
            }
            set
            {
                _currentPartition = value;
            }
        }

        public static int CurrentDisk
        {
            get
            {
                return _currentDisk;
            }
            set
            {
                _currentDisk = value;
            }
        }

        public static string GetFullPath => $@"{_currentPartition}:\{_currentPath}";

        internal static void SetPath(string currentPath)
        {
            if (Directory.Exists($@"{_currentPartition}:\{_currentPath}"))
            {
                _currentPath = currentPath;
            }

            Console.WriteLine($"Could not navigate to: {currentPath}");
        }

        public static string GetCurrentDirectory()
        {
            var path = _currentPath.Split(@"\");
            return path[path.Length];
        }    

        public static void SetDirectory(string directory)
        {
            if (Directory.Exists($@"{_currentPath}\{directory}"))
            {
                _currentPath = $@"{_currentPath}\{directory}";
            }
            Console.WriteLine($"{directory} does not exist");
        }

        #endregion

        #region Functions

        #region Informative

        public static void DisplayInfomation()
        {
            Disks[_currentDisk].DisplayInformation();
        }

        public static void DisplayInfomation(int disk)
        {
            Disks[disk].DisplayInformation();
        }

        #endregion

        #region Partition Commons

        public static void Mount()
        {
            Disks[_currentDisk].Mount();
        }

        public static void Mount(int disk)
        {
            Disks[disk].Mount();
        }

        public static void MountPartition(int index)
        {
            Disks[_currentDisk].MountPartition(index);
        }

        public static void MountPartition(int disk, int index)
        {
            Disks[disk].MountPartition(index);
        }

        public static void CreatePartition(int size)
        {
            Disks[_currentDisk].CreatePartition(size);
        }

        public static void CreatePartition(int disk, int size)
        {
            Disks[disk].CreatePartition(size);
        }

        #region Formatting

        /*Untested*/
        public static void Format(int currentPartition)
        {
            Disks[_currentDisk].FormatPartition(currentPartition, "FAT32");
        }

        /*Untested*/
        public static void Format(int currentDisk, int currentPartition)
        {
            Disks[currentDisk].FormatPartition(currentPartition, "FAT32");
        }

        #endregion

        #region DangerZone

        public static void DeletePartition(int index)
        {
            Disks[_currentDisk].DeletePartition(index);
        }

        public static void DeletePartition(int disk, int index)
        {
            Disks[disk].DeletePartition(index);
        }

        public static void DeleteAllPartitions()
        {
            Disks[_currentDisk].Clear();
        }

        public static void DeleteAllPartitions(int disk)
        {
            Disks[disk].Clear();
        }

        #endregion

        #endregion

        public static void CreateFolder(string folderName)
        {
            if (FileSystem.GetDirectory($@"{GetFullPath}\{folderName}") != null)
            {
                Console.WriteLine($"{folderName} already exists");
            }
            else
            {
                FileSystem.CreateDirectory($@"{GetFullPath}\{folderName}");
                _currentPath += $@"\{folderName}";
            }
        }

        public static void CreateFile(string fileName)
        {
            if (FileSystem.GetFile($@"{GetFullPath}\{fileName}") != null)
            {
                Console.WriteLine($"{fileName} already exists");
            }
            else
            {
                FileSystem.CreateFile($@"{GetFullPath}\{fileName}");
            }
        }

        public static string[] GetFileContents(string fileName)
        {
            if (FileSystem.GetFile($@"{GetFullPath}\{fileName}") == null)
            {
                Console.WriteLine($"{fileName} does not exist"); return new string[] { };
            }

            string[] content = File.ReadAllLines($@"{GetFullPath}\{fileName}");

            if (content.Length == 0)
            {
                Console.WriteLine($"{fileName} does not have any content"); return new string[] { };
            }

            return content;
        }

        public static void RemoveFolder(string folderName)
        {
            if (FileSystem.GetDirectory($@"{GetFullPath}\{folderName}") == null)
            {
                Console.WriteLine($"{folderName} does not exist");
            }
            else
            {
                Directory.Delete($@"{GetFullPath}\{folderName}");
            }
        }

        public static void RemoveFile(string fileName)
        {
            if (FileSystem.GetFile($@"{GetFullPath}\{fileName}") == null)
            {
                Console.WriteLine($"{fileName} does not exist");
            }
            else
            {
                File.Delete($@"{GetFullPath}\{fileName}");
            }
        }

        public static bool FileExists(string fileName)
        {
            return FileSystem.GetFile($@"{GetFullPath}\{fileName}") != null;
        }

        public static bool FolderExists(string folderName)
        {
            return FileSystem.GetDirectory($@"{GetFullPath}\{folderName}") != null;
        }

        public static DirectoryEntry GetDirectory(string directory)
        {
            return FileSystem.GetDirectory(directory);
        }

        public static void WriteAllText(string fileName, string text)
        {
            File.WriteAllText($@"{GetFullPath}\{fileName}", text);
        }

        public static void MoveFile(string fileName, string destination)
        {
            if (!FileExists(fileName))
            {
                Console.WriteLine($"File '{fileName}' cannot be found!"); return;
            }

            if (!FolderExists(destination))
            {
                Console.WriteLine($"Folder '{fileName}' cannot be found!"); return;
            }

            string[] fileContents = GetFileContents(fileName);
            RemoveFile(fileName);
            CreateFile($@"{destination}\{fileName}");
            WriteAllText($@"{destination}\{fileName}", string.Join("\n", fileContents));
        }

        public static void CopyFile(string fileName, string destination)
        {
            if (!FileExists(fileName))
            {
                Console.WriteLine($"File '{fileName}' cannot be found!"); return;
            }

            if (!FolderExists(destination))
            {
                Console.WriteLine($"Folder '{fileName}' cannot be found!"); return;
            }

            string[] fileContents = GetFileContents(fileName);
            CreateFile($@"{destination}\{fileName}");
            WriteAllText($@"{destination}\{fileName}", string.Join("\n", fileContents));
        }

        #endregion

        #region tests

        public static void test()
        {
            var root = GetDirectory(Kernel.CurrentDirectory);

        }

        #endregion

    }
}
