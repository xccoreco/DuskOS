/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          Security/Permissions/Permissions.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley <cemberley@nerdhub.net>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Security.Permissions
{
    public interface IReadOnlyPermission
    {
        object GetValue();
    }
    public interface IReadOnlyPermission<T> : IReadOnlyPermission
    {
        new T GetValue();
    }

    public interface IPermissionNode<T> : IReadOnlyPermission<T>
    {
        Permission SetValue(T value);
        IReadOnlyPermission AsReadOnly();
    }

    public class Permission<V> : Permission, IPermissionNode<V>
    {
        public Permission(string permissionName)
            : base(permissionName, default) { }

        public Permission(string permissionName, V defaultValue) : base(permissionName, defaultValue) { }

        public Permission SetValue(V value)
            => base.SetValue(value);

        public new V GetValue() => (V)base.GetValue();

        public new IReadOnlyPermission AsReadOnly()
            => this;
    }

    public class Permission : IPermissionNode<object>
    {
        private string permissionName = "";
        private object permissionValue = null;

        public Permission(string permissionName)
        {
            this.permissionName = permissionName;
        }

        public Permission(string permissionName, object defaultValue)
        {
            this.permissionName = permissionName;
            SetValue(defaultValue);
        }

        public IReadOnlyPermission AsReadOnly()
            => this;

        public string GetName() => permissionName;

        public object GetValue() => permissionValue;

        public Permission SetValue(object value)
        {
            permissionValue = value;
            return this;
        }
    }

    public interface IReadOnlyPermissionCollection
    {
        int Count { get; }
        Permission this[int index] { get; }
        Permission this[string key] { get; }
        bool ContainsPermission(string key);
        bool Contains(Permission permission);
        int IndexOf(Permission permission);
        int IndexOfPermission(string key);
        Permission GetPermission(string permission);
        Permission<V> GetPermission<V>(string permission);
        IReadOnlyPermission[] GetAllPermissions();
    }

    public interface IPermissionCollection : IReadOnlyPermissionCollection
    {
        new Permission this[int index] { get; set; }
        new Permission this[string key] { get; set; }
        void AddPermission(Permission p);
        void AddPermission<V>(string name);
        void AddPermission<V>(string name, V defaultValue);
        void AddPermissions(Permission[] permissions);
        void Remove(Permission p);
        void RemoveAt(int index);
        new Permission[] GetAllPermissions();
        IReadOnlyPermissionCollection AsReadOnly();
    }

    public class PermissionCollection : IPermissionCollection
    {
        private List<Permission> permissions = new List<Permission>();

        public int Count => permissions.Count;

        public PermissionCollection() { }

        public void AddPermission(Permission p)
            => permissions.Add(p);

        public void AddPermission<V>(string name)
            => AddPermission(new Permission<V>(name));

        public void AddPermission<V>(string name, V defaultValue)
        {
            AddPermission(new Permission<V>(name, defaultValue));
        }

        public void AddPermissions(Permission[] permissions)
        {
            foreach (Permission p in permissions)
                AddPermission(p);
        }

        public bool ContainsPermission(string name)
        {
            foreach (Permission p in permissions)
            {
                if (p.GetName().Equals(name))
                    return true;
                else continue;
            }
            return false;
        }

        public Permission GetPermission(string name)
        {
            foreach (Permission p in permissions)
            {
                if (p.GetName().Equals(name))
                    return p;
                else continue;
            }
            return null;
        }

        public Permission<V> GetPermission<V>(string name)
            => (Permission<V>)GetPermission(name);

        public int IndexOfPermission(string name)
        {
            for (int i = 0; i < permissions.Count; i++)
            {
                var permission = this[i];
                if (permission.GetName().Equals(name))
                    return i;
                else continue;
            }
            return -1;
        }

        public Permission this[int index]
        {
            get => permissions[index];
            set => permissions[index] = value;
        }

        public Permission this[string name]
        {
            get => GetPermission(name);
            set
            {
                if (ContainsPermission(name))
                {
                    //Override existing.
                    var index = IndexOfPermission(name);
                    this[index] = value;
                }
                else
                {
                    //Add new.
                    AddPermission(value);
                }
            }
        }

        public Permission[] GetAllPermissions()
            => permissions.ToArray();

        public void Remove(Permission p)
            => permissions.Remove(p);

        public void RemoveAt(int index)
            => permissions.RemoveAt(index);

        public IReadOnlyPermissionCollection AsReadOnly()
            => this;

        public bool Contains(Permission permission)
        {
            foreach (var p in GetAllPermissions())
            {
                if (p == permission)
                    return true;
                else continue;
            }
            return false;
        }

        public int IndexOf(Permission permission)
        {
            for (int i = 0; i < Count; i++)
            {
                var x = this[i];
                if (x == permission)
                    return i;
                else continue;
            }
            return -1;
        }

        IReadOnlyPermission[] IReadOnlyPermissionCollection.GetAllPermissions()
            => GetAllPermissions();
    }

    public sealed class PermissionRegistry : PermissionCollection
    {
        PermissionRegistry() { }

        public static PermissionRegistry INSTANCE = new PermissionRegistry();
    }

    public static class PermissionLoader
    {
        private static readonly PermissionRegistry reg = PermissionRegistry.INSTANCE;
        public static void LoadConfigurationPermssions()
        {
            reg.AddPermissions(new Permission[]
            {
                new Permission<string>("CanChangeFonts"),
                new Permission<string>("CanChangeDesktopBG"),
                new Permission<string>("CanChangeUserBG"),
                new Permission<string>("CanChangeCursors"),
                new Permission<string>("CanChangeSounds"),
                new Permission<string>("CanChangeDevices"),
                new Permission<string>("CanChangeNetworks"),
                new Permission<string>("CanChangePwrSettings"),
                new Permission<string>("CanChangeUsrPerms"),
                new Permission<string>("CanChangeGrpPerms"),
                new Permission<string>("CanUpdateUsrs"),
                new Permission<string>("CanUpdateGrps"),
                new Permission<string>("CanInstallApps"),
                new Permission<string>("CanInstallSrvcs"),
                new Permission<string>("CanInstallDrvrs"),
                new Permission<string>("CanInstallFonts"),
                new Permission<string>("CanInstallThemes"),
                new Permission<string>("CanInstallUIs")
            });
        }
        public static void LoadFilePermssions()
        {
            reg.AddPermissions(new Permission[]
            {
                //For System files.
                new Permission<string>("CanReadSystemFiles"),
                new Permission<string>("CanWriteSystemFiles"),
                new Permission<string>("CanCreateSystemFiles"),
                new Permission<string>("CanDeleteSystemFiles"),
                new Permission<string>("CanCopySystemFiles"),
                new Permission<string>("CanMoveSystemFiles"),
                new Permission<string>("CanViewSystemFiles"),
                //For the files of other users.
                new Permission<string>("CanReadGlobalFiles"),
                new Permission<string>("CanWriteGlobalFiles"),
                new Permission<string>("CanCreateGlobalFiles"),
                new Permission<string>("CanDeleteGlobalFiles"),
                new Permission<string>("CanCopyGlobalFiles"),
                new Permission<string>("CanMoveGlobalFiles"),
                new Permission<string>("CanViewGlobalFiles"),
                //For the files of a specific user.
                new Permission<string>("CanReadLocalFiles"),
                new Permission<string>("CanWriteLocalFiles"),
                new Permission<string>("CanCreateLocalFiles"),
                new Permission<string>("CanDeleteLocalFiles"),
                new Permission<string>("CanCopyLocalFiles"),
                new Permission<string>("CanMoveLocalFiles"),
                new Permission<string>("CanViewLocalFiles"),
                //For disks.
                new Permission<string>("CanViewInstallDisk"),
                new Permission<string>("CanViewAdditionalDisks"),
                new Permission<string>("CanViewRemovableDisks"),
                new Permission<string>("CanModifyInstallDisk"),
                new Permission<string>("CanModifyAdditionalDisks"),
                //Additionals
                new Permission<string[]>("PermittedDirectories"),
                new Permission<string[]>("PermittedFiles"),
                new Permission<string[]>("PermittedAltDisks"),
                new Permission<string[]>("PermittedRemovableDisks"),
            });
        }
        public static void LoadSpecialPermssions()
        {
            reg.AddPermissions(new Permission[]
            {
                new Permission<bool>("CanHasTimeLimits"),
                new Permission<bool>("CanHasCerfew"),
                new Permission<bool>("CanHasAppBlacklist"),
                new Permission<bool>("CanHasAppWhitelist"),
                new Permission<bool>("CanHasWebBlacklist"),
                new Permission<bool>("CanHasWebWhitelist"),
                new Permission<bool>("AdminCanForceUI"),
                new Permission<bool>("AdminCanForceFont"),
                new Permission<bool>("AdminCanForceDesktopBG"),
                new Permission<bool>("AdminCanForceUserBG"),
                new Permission<bool>("AdminCanForceCurors"),
                new Permission<bool>("AdminCanForceSounds"),
                new Permission<bool>("ForcedFont"),
                new Permission<bool>("ForcedUI"),
                new Permission<bool>("ForcedDesktopBG"),
                new Permission<bool>("ForcedUserBG"),
                new Permission<bool>("ForcedCursors"),
                new Permission<bool>("ForcedSounds"),
            });
        }

        public static PermissionCollection GetPermissionSet(Users.AccountType accountType)
        {
            if (accountType == Users.AccountType.GUEST)
            {
                //Load Guest permissions.
                var collection = new PermissionCollection();
                collection.AddPermission(reg.GetPermission("CanChangeUsrPerms").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanChangeGrpPerms").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanUpdateUsrs").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanUpdateGrps").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanInstallApps").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanInstallSrvcs").SetValue(false));

                collection.AddPermission(reg.GetPermission("CanReadSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanWriteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopySystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewSystemFiles").SetValue(false));

                collection.AddPermission(reg.GetPermission("CanReadGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanWriteGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopyGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewGlobalFiles").SetValue(false));

                collection.AddPermission(reg.GetPermission("CanReadLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanWriteLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopyLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveLocalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewLocalFiles").SetValue(false));

                collection.AddPermission(reg.GetPermission("CanViewInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewAdditionalDisks").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewRemovableDisks").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanModifyInstallDisk").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanModifyAdditionalDisks").SetValue(false));

                collection.AddPermission(reg.GetPermission("PermittedDirectories").SetValue(new string[]
                {
                    "Desktop",
                    "Documents",
                    "Pictures",
                    "Videos"
                }));

                return collection;
            }
            else if (accountType == Users.AccountType.CHILD)
                throw new Exception("Since some features of the child account are not setup, it will have NO permissions.");
            else if (accountType == Users.AccountType.ADMIN)
            {
                //Load Admin permissions.
                var collection = new PermissionCollection();
                collection.AddPermission(reg.GetPermission("CanChangeUsrPerms").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanChangeGrpPerms").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanUpdateUsrs").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanUpdateGrps").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanInstallApps").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanInstallSrvcs").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopySystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewSystemFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopyGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewGlobalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopyLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewLocalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanViewInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewAdditionalDisks").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewRemovableDisks").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanModifyInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanModifyAdditionalDisks").SetValue(true));

                //collection.AddPermission(reg.GetPermission("PermittedDirectories").SetValue(new string[0]));

                return collection;
            }
            else if (accountType == Users.AccountType.SYSTEM)
            {
                //Load System permissions.
                var collection = new PermissionCollection();
                collection.AddPermission(reg.GetPermission("CanChangeUsrPerms").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanChangeGrpPerms").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanUpdateUsrs").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanUpdateGrps").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanInstallApps").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanInstallSrvcs").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopySystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewSystemFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopyGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveGlobalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewGlobalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopyLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewLocalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanViewInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewAdditionalDisks").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewRemovableDisks").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanModifyInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanModifyAdditionalDisks").SetValue(true));

                //collection.AddPermission(reg.GetPermission("PermittedDirectories").SetValue(new string[0]));

                return collection;
            }
            else if (accountType == Users.AccountType.RECOVERY)
                return null;
            else
            {
                //Load Admin permissions.
                var collection = new PermissionCollection();
                collection.AddPermission(reg.GetPermission("CanChangeUsrPerms").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanChangeGrpPerms").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanUpdateUsrs").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanUpdateGrps").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanInstallApps").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanInstallSrvcs").SetValue(false));

                collection.AddPermission(reg.GetPermission("CanReadSystemFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopySystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveSystemFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewSystemFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanWriteGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCreateGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanDeleteGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanCopyGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanMoveGlobalFiles").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanViewGlobalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanReadLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanWriteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCreateLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanDeleteLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanCopyLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanMoveLocalFiles").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewLocalFiles").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanViewInstallDisk").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewAdditionalDisks").SetValue(true));
                collection.AddPermission(reg.GetPermission("CanViewRemovableDisks").SetValue(true));

                collection.AddPermission(reg.GetPermission("CanModifyInstallDisk").SetValue(false));
                collection.AddPermission(reg.GetPermission("CanModifyAdditionalDisks").SetValue(true));

                //collection.AddPermission(reg.GetPermission("PermittedDirectories").SetValue(new string[0]));

                return collection;
            }
        }
    }
}
