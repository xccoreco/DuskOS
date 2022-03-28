using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cosmos.System;
using Cosmos.System.Graphics;

namespace DuskOSDev.DuskSystem.Process
{
    public class Window : Process
    {
        private Bitmap icon;
        private string windowName;

        public Window(string name)
        {
            windowName = name;
        }

        public string Title
        {
            get => windowName;
            set
            {
                
            }
        }
    }
}
