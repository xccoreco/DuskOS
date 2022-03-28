using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev
{
    public class Resources
    {
        //1920x1024
        [ManifestResourceStream(ResourceName = "DuskOSDev.DuskSystem.Resources.EveryMacWallpaper1920.bmp")]
        public static byte[] Wallpaper;

        [ManifestResourceStream(ResourceName = "DuskOSDev.DuskSystem.Resources.desert.bmp")]
        public static byte[] DefaultWallpaper;

        [ManifestResourceStream(ResourceName = "DuskOSDev.DuskSystem.Resources.Cursor.bmp")]
        public static byte[] DefaultCursor;
    }
}
