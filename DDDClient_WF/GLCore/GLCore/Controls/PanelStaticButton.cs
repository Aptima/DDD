using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public class PanelStaticButton : PanelControl
    {

        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
        public Microsoft.DirectX.Direct3D.Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }


        private Microsoft.DirectX.Direct3D.Font _font = null;
        private string _text;
        private int _text_size;

        private Color _border_color = Color.FromArgb(150, 150, 160);
        public Color BorderColor
        {
            get
            {
                return _border_color;
            }
            set
            {
                _border_color = value;
            }
        }

        private Color _background_color = Color.FromArgb(122, 123, 143);
        public Color BackgroundColor
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = value;
            }
        }
        
        private Color _foreground_color = Color.FromArgb(221, 222, 200);
        public Color ForegroundColor
        {
            get
            {
                return _foreground_color;
            }
            set
            {
                _foreground_color = value;
            }
        }

        public bool Vertical = false;

        private Material BackgroundMaterial;

        public MouseEventHandler MouseClick
        {
            set
            {
                _MouseClick = value;
            }
            get
            {
                return _MouseClick;
            }
        }
        public MouseEventHandler MouseDoubleClick
        {
            set
            {
                _MouseDoubleClick = value;
            }
            get
            {
                return _MouseDoubleClick;
            }
        }
        public MouseEventHandler MouseDown
        {
            set
            {
                _MouseDown = value;
            }
            get
            {
                return _MouseDown;
            }
        }

        public MouseEventHandler MouseMove
        {
            set
            {
                _MouseMove = value;
            }
            get
            {
                return _MouseMove;
            }
        }
        public MouseEventHandler MouseUp
        {
            set
            {
                _MouseUp = value;
            }
            get
            {
                return _MouseUp;
            }
        }
        public MouseEventHandler MouseWheel
        {
            set
            {
                _MouseWheel = value;
            }
            get
            {
                return _MouseWheel;
            }
        }

        public KeyEventHandler KeyDown
        {
            set
            {
                _KeyDown = value;
            }
            get
            {
                return _KeyDown;
            }
        }
        public KeyEventHandler KeyUp
        {
            set
            {
                _KeyUp = value;
            }
            get
            {
                return _KeyUp;
            }
        }
        public KeyPressEventHandler KeyPress
        {
            set
            {
                _KeyPress = value;
            }
            get
            {
                return _KeyPress;
            }
        }

        public PanelStaticButton()
        {
            _client_area = Rectangle.Empty;
            _Sticky = true;
            BackgroundMaterial = new Material();
            BackgroundMaterial.Diffuse = BackgroundColor;
        }
        public PanelStaticButton(int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            _text = string.Empty;
            _Sticky = true;
            BackgroundMaterial = new Material();
            BackgroundMaterial.Diffuse = BackgroundColor;

        }
        public PanelStaticButton(string text, int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            _text = text;
            _Sticky = true;
            BackgroundMaterial = new Material();
            BackgroundMaterial.Diffuse = BackgroundColor;

        }

        public override void OnRender(Canvas c)
        {

            c.DrawFillRect(_client_area, BackgroundMaterial);
            _font.DrawText(null, _text, ClientArea, DrawTextFormat.VerticalCenter | DrawTextFormat.Center, ForegroundColor);

            if (Selected)
            {
                DrawBoundingBox(c);
            }
            else
            {
                c.DrawRect(_client_area, BorderColor);
            }
        }
    
    }
}
