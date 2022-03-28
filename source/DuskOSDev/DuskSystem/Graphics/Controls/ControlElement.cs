using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Graphics.Controls
{
    public abstract class ControlElement
    {
        private Color backColor;
        private Color foreColor;

        private Size size;
        private Point location;

        public Action BackgroundColorChanged;
        public Action ForegroundColorChanged;
        public Action SizeChanged;
        public Action LocationChanged;
        public Action Paint;

        public virtual void OnBackColorChanged() { }
        public virtual void OnForeColorChanged() { }
        public virtual void OnSizeChanged() { }
        public virtual void OnLocationChanged() { }
        public virtual void OnPaint() { }

        public Size Size
        {
            get => size;
        }

        public ControlElement() { /* set defaults. */ }
    }

    public sealed class ControlPaint
    {

    }
}
