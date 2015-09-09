using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AME.Views.View_Components.Helpers
{
    public class ColorHelper
    {
        public static Icon IconFromColor(Color c)
        {
            Bitmap bm = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage((Image)bm);
            Brush forGraphics = new SolidBrush(c);
            Pen black = new Pen(Color.Black);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(black, new Rectangle(0, 0, bm.Width - 1, bm.Height - 1));
            g.FillRectangle(forGraphics, new Rectangle(0, 0, bm.Width - 1, bm.Height - 1));
            Icon icon = Icon.FromHandle(bm.GetHicon());
            black.Dispose();
            forGraphics.Dispose();
            g.Dispose();
            bm.Dispose();

            return icon;
        }
    }
}
