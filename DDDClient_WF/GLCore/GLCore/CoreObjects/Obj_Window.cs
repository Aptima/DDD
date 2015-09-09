using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Gui.Common.GameLib.GameObjects
{
    public class Obj_Window
    {
        private Sprite _background;
        private Texture _window_background;
        private int _window_texture_width;
        private int _window_texture_height;

        private Rectangle _text_rectangle = Rectangle.Empty;
        public DrawTextFormat Format = DrawTextFormat.Left;
        public Color Foreground = Color.Yellow;
        public int Padding = 2;

        public bool Activated = false;



        public Obj_Window(Device device)
        {
            _background = new Sprite(device);

            Assembly assem = this.GetType().Assembly;
            Stream strm = assem.GetManifestResourceStream("GameLib.images.window.png");
            _window_background = Texture.FromStream(device, strm, Usage.None, Pool.Managed);

            using (Surface s = _window_background.GetSurfaceLevel(0))
            {
                _window_texture_height = s.Description.Height;
                _window_texture_width = s.Description.Width;
            }
            strm.Close();

        }

        
        public void Draw(string text, float x, float y, float width, float height, Microsoft.DirectX.Direct3D.Font font)
        {

            _background.Begin(SpriteFlags.AlphaBlend);
            _background.Transform = Matrix.Scaling(width / _window_texture_width, height / _window_texture_height, 0) * Matrix.Translation(x, y, 0);

            _background.Draw(_window_background, Vector3.Empty, Vector3.Empty, Color.White.ToArgb());

            _text_rectangle.X = (int)x + Padding;
            _text_rectangle.Y = (int)y + Padding;
            _text_rectangle.Width = (int)width - (2 * Padding);
            _text_rectangle.Height = (int)height - (2 * Padding);

            _background.Transform = Matrix.Scaling(1, 1, 1);
            font.DrawText(_background, text, _text_rectangle, Format, Foreground);
            if (Activated)
            {
                Activated = false;
            }
            _background.End();
        }
        public void Draw(string text, Rectangle rect, Microsoft.DirectX.Direct3D.Font font)
        {
            Draw(text, rect.X, rect.Y, rect.Width, rect.Height, font);
        }



    }
}
