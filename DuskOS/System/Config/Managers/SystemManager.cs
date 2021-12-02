using System;
using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Config.Managers
{
    public class SystemManager
    {
        public static ConfigStatus SaveConfig()
        {
            return ConfigStatus.Unknown;
        }

        public static ConfigStatus LoadConfig()
        {
            return ConfigStatus.Unknown;
        }

        public static ConfigStatus DiscardConfig()
        {
            return ConfigStatus.Unknown;
        }
    }
}
