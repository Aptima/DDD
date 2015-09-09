using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public class PanelCheckbox : PanelControl
    {
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

        private Rectangle _box = Rectangle.Empty;
        

        private string _text = string.Empty;
        public string Label
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public PanelCheckbox()
        {
            _client_area = Rectangle.Empty;
            _text = string.Empty;
            _Sticky = false;

        }
        public PanelCheckbox(string text, int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;


            _text = text;
            _Sticky = false;
        }

        public override void OnRender(Canvas c)
        {
            _box.X = _client_area.X;
            _box.Y = _client_area.Y;
            _box.Width = _client_area.Width / 4;
            _box.Height = _client_area.Height;

            c.DrawRect(_box, Color.Yellow);
            c.DrawLine(Color.Yellow, 1, _box.X, _box.Y, _box.Right, _box.Bottom);
            c.DrawLine(Color.Yellow, 1, _box.Right, _box.Top, _box.Left, _box.Bottom);       
        }

    }
}
