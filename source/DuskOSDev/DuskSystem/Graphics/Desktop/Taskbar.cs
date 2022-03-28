using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Cosmos.System.Graphics;

namespace DuskOSDev.DuskSystem.Graphics.Desktop
{
    public class Taskbar
    {
        public Color TaskbarColor { get; set; } = Color.Blue;
        public int TaskbarHeight { get; set; } = 30;

        public void Render()
        {
            var rc = OSDesktop.RenderCanvas;
            var sz = OSDesktop.ScreenSize;
            var w = sz.Width;
            var h = sz.Height;

            rc.DrawRectangle(new Pen(TaskbarColor), 0, h - TaskbarHeight, w, TaskbarHeight);
        }
    }
}
