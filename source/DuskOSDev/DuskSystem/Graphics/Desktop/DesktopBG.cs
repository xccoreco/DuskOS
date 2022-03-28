using Cosmos.System.Graphics;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Graphics.Desktop
{
    public class DesktopBG
    {
        public Bitmap BackgroundImage { get; set; }
        public Color BackgroundColor { get; set; } = Color.Teal;

        public DesktopBG()
        {
            if (BackgroundImage == null)
                BackgroundImage = new Bitmap(Resources.DefaultWallpaper);
        }

        public void Render()
        {
            if (BackgroundImage == null)
            {
                var w = OSDesktop.ScreenSize.Width;
                var h = OSDesktop.ScreenSize.Height;
                OSDesktop.RenderCanvas.DrawFilledRectangle(new Pen(BackgroundColor), new Cosmos.System.Graphics.Point(0, 0), w, h);
            }
            else
            {
                var w = OSDesktop.ScreenSize.Width;
                var h = OSDesktop.ScreenSize.Height;
                OSDesktop.RenderCanvas.DrawImage(BackgroundImage, 0, 0, w, h);
            }
        }
    }
}
