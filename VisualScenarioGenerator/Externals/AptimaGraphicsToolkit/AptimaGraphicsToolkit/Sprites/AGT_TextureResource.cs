using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AGT.Sprites
{
    public class AGT_TextureResource
    {
        private Bitmap _bitmap = null;



        public AGT_TextureResource(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
        }


        public AGT_TextureResource(float width, float height)
        {
            _bitmap = new Bitmap((int)(width + 1), (int)(height + 1));
        }


        public AGT_TextureResource(Stream strm)
        {
            if (strm != null)
            {
                _bitmap = new Bitmap(strm);
            }
            else
            {
                throw new ArgumentException("Null stream.");
            }
        }


        public Graphics GetGraphics()
        {
            if (_bitmap != null)
            {
                return Graphics.FromImage(_bitmap);
            }
            return null;
        }


        public Bitmap ToBitmap()
        {
            return _bitmap;
        }


        public Texture ToTexture(Device d)
        {
            if ((_bitmap != null) && (d != null))
            {
                return new Texture(d, _bitmap, Usage.None, Pool.Managed);
            }
            throw new ArgumentNullException("Null parameter passed");
        }


        public static Texture BitmapToTexture(Device d, Bitmap b)
        {
            if ((b != null) && (d != null))
            {
                return new Texture(d, b, Usage.None, Pool.Managed);
            }
            throw new ArgumentNullException("Null parameter passed");
        }


        public static Texture BitmapToTexture(Device d, Stream str)
        {
            using (Bitmap b = new Bitmap(str))
            {
                return BitmapToTexture(d, b);
            }
        }
    }
}
