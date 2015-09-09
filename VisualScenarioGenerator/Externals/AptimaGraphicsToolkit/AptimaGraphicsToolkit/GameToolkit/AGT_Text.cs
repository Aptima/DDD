using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AGT.GameToolkit
{
    public class AGT_Text
    {
        private Rectangle _message_rect = Rectangle.Empty;
        private Rectangle _label_rect = Rectangle.Empty;
        private Rectangle _collision_rect = Rectangle.Empty;

        private string _label_message;
        private Color _background_color = Color.DodgerBlue;

        public string Message
        {
            get
            {
                lock (this)
                {
                    return _label_message;
                }
            }
            set
            {
                lock (this)
                {
                    _label_message = value;
                }
            }
        }
        public Color Background
        {
            get
            {
                lock (this)
                {
                    return _background_color;
                }
            }
            set
            {
                lock (this)
                {
                    _background_color = value;
                }
            }
        }

        public int X
        {
            get
            {
                return _label_rect.X;
            }
        }
        public int Y
        {
            get
            {
                return _label_rect.Y;
            }
        }
        public int Width
        {
            get
            {
                return _label_rect.Width;
            }
        }
        public int Height
        {
            get
            {
                return _label_rect.Height;
            }
        }

        private Microsoft.DirectX.Direct3D.Font _font = null;
        public Microsoft.DirectX.Direct3D.Font Font
        {
            get
            {
                return _font;
            }
        }


        public AGT_Text(string message, Microsoft.DirectX.Direct3D.Font label_font)
        {
            _font = label_font;
            _label_message = message;
            _message_rect = label_font.MeasureString(null, message, DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.White);
            _message_rect.X = 0;
            _message_rect.Y = 0;
            _label_rect.X = 0;
            _label_rect.Y = 0;
            _label_rect.Height = _message_rect.Height;
            _label_rect.Width = (int)(_message_rect.Width + (_message_rect.Width * .2));
        }

        public bool HitTest(int x, int y)
        {
            _collision_rect.X = _label_rect.X - 1;
            _collision_rect.Y = _message_rect.Y - 1;
            _collision_rect.Width = _label_rect.Width + 1;
            _collision_rect.Height = _label_rect.Height + 1;
            return _collision_rect.Contains(x, y);
        }

        public bool RelativeHitTest(int mouse_x, int mouse_y, int x_offset, int y_offset)
        {
            _collision_rect.X = (_label_rect.X - 1)+x_offset;
            _collision_rect.Y = (_message_rect.Y - 1)+y_offset;
            _collision_rect.Width = _label_rect.Width + 1;
            _collision_rect.Height = _label_rect.Height + 1;
            return _collision_rect.Contains(mouse_x, mouse_y);
        }

        public void SetPosition(int x, int y)
        {
            _label_rect.X = x;
            _label_rect.Y = (int)(y + (_message_rect.Height * .5));
            _label_rect.Height = _message_rect.Height;
            _label_rect.Width = (int)(_message_rect.Width + (_message_rect.Width * .2));

            _message_rect.X = (int)(x - ((_message_rect.Width - _label_rect.Width) * .5)) + 1;
            _message_rect.Y = y;
        }

        public void DrawMessage(Color foreground)
        {
            _font.DrawText(null, _label_message, _message_rect.X , _message_rect.Y, foreground);
        }

        public void DrawMessageRelative(Color foreground, float x_offset, float y_offset)
        {
            _font.DrawText(null, _label_message, (int)(_message_rect.X + x_offset), (int)(_message_rect.Y + y_offset), foreground);
        }

    }

}
