using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;


namespace AGT.Sprites
{
    public static class AGT_GDIBridge
    {
        public static Bitmap CreateCircle(Device d, int radius, Brush brush)
        {
            int width = radius*2;
            int height = width;

            AGT_TextureResource gdi = new AGT_TextureResource(width, height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                if (g != null)
                {
                    g.FillEllipse(brush, 0, 0, width, height);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }

        public static Bitmap CreateRectangle(Device d, Rectangle rect, Brush brush)
        {
            AGT_TextureResource gdi = new AGT_TextureResource(rect.Width, rect.Height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                if (g != null)
                {
                    g.FillRectangle(brush, rect);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }

        public static Bitmap CreateRectangle(Device d, RectangleF rectF, Brush brush)
        {
            AGT_TextureResource gdi = new AGT_TextureResource(rectF.Width, rectF.Height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                if (g != null)
                {
                    g.FillRectangle(brush, rectF);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }

        public static Bitmap CreateFillPolygon(Device d, GraphicsPath path, Brush brush)
        {
            RectangleF rectF = path.GetBounds();
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            m.Translate(-rectF.X, -rectF.Y);
            path.Transform(m);

            AGT_TextureResource gdi = new AGT_TextureResource(rectF.Width, rectF.Height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                if (g != null)
                {
                    g.FillPath(brush, path);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }

        public static Bitmap CreatePolygon(Device d, GraphicsPath path, Pen pen)
        {
            RectangleF rectF = path.GetBounds();
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            m.Translate(-rectF.X, -rectF.Y);
            path.Transform(m);
            
            AGT_TextureResource gdi = new AGT_TextureResource(rectF.Width, rectF.Height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (g != null)
                {
                    g.DrawPath(pen, path);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }


        public static Bitmap CreateLine(Device d, GraphicsPath path, Pen pen, bool show_joints)
        {
            RectangleF rectF = path.GetBounds();
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            m.Translate(-rectF.X, -rectF.Y);
            path.Transform(m);

            AGT_TextureResource gdi = new AGT_TextureResource(rectF.Width, rectF.Height);
            if (gdi != null)
            {
                Graphics g = gdi.GetGraphics();
                if (g != null)
                {
                    g.DrawPath(pen, path);
                    return gdi.ToBitmap();
                }
            }
            return null;
        }

               
    }
}
