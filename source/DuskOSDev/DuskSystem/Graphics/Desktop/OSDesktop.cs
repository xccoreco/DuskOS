using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Graphics.Desktop
{
    public class OSDesktop
    {
        public static Canvas RenderCanvas { get; private set; }

        public static Size ScreenSize { get; private set; }

        public static DesktopBG DesktopBackground { get; set; }

        public static Taskbar TaskBar { get; private set; }

        public static void Initialize(Size screenSize, ColorDepth depth = ColorDepth.ColorDepth32)
        {
            var w = screenSize.Width;
            var h = screenSize.Height;
            RenderCanvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(w, h, depth));
            ScreenSize = screenSize;
            DesktopBackground = new DesktopBG();
            TaskBar = new Taskbar();
        }

        public static void RenderDesktop()
        {
            DesktopBackground.Render();
            TaskBar.Render();
        }
    }
}
