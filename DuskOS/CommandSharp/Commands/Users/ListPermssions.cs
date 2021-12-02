using System;

using DuskOS.System.Security.Permissions;

namespace CommandSharp.Commands.Users
{
    public class ListPermssions : Command
    {
        private static readonly CommandData data = new CommandData("@listperms", "Lists permissions on the system.");

        public ListPermssions() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //Display all permissions.
            var perms = PermissionRegistry.INSTANCE;
            foreach (Permission p in perms.GetAllPermissions())
            {
                string s = $"{p.GetName()} {(string)p.GetValue()}";
                Console.WriteLine(s);
            }
            return true;
        }
    }
}
