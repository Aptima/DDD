using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public delegate void PanelSliderChangeHandler(double slider_value);

    public class PanelSlider : PanelControl
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

        private double _slider_position = .5;
        public double SliderPosition
        {
            set
            {
                _slider_position = value;
                _percent = string.Format("{0}%", Math.Round(value * 100));
            }
        }
        public double SliderIncrement = .1;
        private string _percent = string.Empty;

        public int Height = 12;
        
        private Rectangle _slider_area = Rectangle.Empty;
        private Rectangle _right_arrow = Rectangle.Empty;
        private Rectangle _left_arrow = Rectangle.Empty;
        private Rectangle _txt_area = Rectangle.Empty;

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

        private Material _arrow_material = new Material();

        private Material _background_material = new Material();
        private Color _background_color = Color.Black;
        public Color BackgroundColor
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = value;
                _background_material.Diffuse = BackgroundColor;
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


        private Material _slider_foreground_material = new Material();
        private Color _slider_foreground = Color.Lime;
        public Color SliderForeground
        {
            get
            {
                return _slider_foreground;
            }
            set
            {
                _slider_foreground = value;
                _slider_foreground_material.Diffuse = value;
            }
        }

        private Material _slider_background_material = new Material();
        private Color _slider_background = Color.Black;
        public Color SliderBackground
        {
            get
            {
                return _slider_background;
            }
            set
            {
                _slider_background = value;
                _slider_background_material.Diffuse = value;
            }
        }
        

        private PanelSliderChangeHandler _handler = null;
        public object Payload = null;


        public PanelSliderChangeHandler Handler
        {
            set
            {
                _handler = value;
            }
        }


        public PanelSlider()
        {
            _client_area = Rectangle.Empty;
            _percent = string.Format("{0}%", Math.Round(_slider_position * 100));

            _Sticky = false;

            _background_material.Diffuse = BackgroundColor;
            _slider_background_material.Diffuse = SliderBackground;
            _slider_foreground_material.Diffuse = SliderForeground;
            _arrow_material.Diffuse = Window.CaptionForeground;
           
        }
        public PanelSlider(int x, int y, int width, int height)
        {
            _client_area.X = x;
            _client_area.Y = y;
            _client_area.Width = width;
            _client_area.Height = height;
            _percent = string.Format("{0}%", Math.Round(_slider_position * 100));

            _Sticky = false;

            _background_material.Diffuse = BackgroundColor;
            _slider_background_material.Diffuse = SliderBackground;
            _slider_foreground_material.Diffuse = SliderForeground;
            _arrow_material.Diffuse = Window.CaptionForeground;


        }

       
        public override void OnRender(Canvas c)
        {
            if ((_font != null) && !Hide)
            {
                c.DrawFillRect(_client_area, _background_material);

                _slider_area.Width = _client_area.Width - 18;
                _slider_area.Height = Height;

                _txt_area = _font.MeasureString(null, _percent, DrawTextFormat.None, ForegroundColor);
                _font.DrawText(null, _percent, _client_area, DrawTextFormat.Center | DrawTextFormat.Top, ForegroundColor);

                _left_arrow.X = _client_area.Left;
                _left_arrow.Y = _client_area.Y + _txt_area.Height + 1;
                _left_arrow.Width = 8;
                _left_arrow.Height = _slider_area.Height;
                c.DrawFillTri(_left_arrow, _arrow_material, DIRECTION.LEFT);

                _slider_area.X = _left_arrow.Right + 1;
                _slider_area.Y = _client_area.Y + _txt_area.Height + 1;
                c.DrawProgressBar(_slider_area, _slider_background_material, _slider_foreground_material, _slider_position);

                _right_arrow.X = _slider_area.Right + 1;
                _right_arrow.Y = _client_area.Y + _txt_area.Height + 1;
                _right_arrow.Width = 8;
                _right_arrow.Height = _slider_area.Height;
                c.DrawFillTri(_right_arrow, _arrow_material, DIRECTION.RIGHT);

                c.DrawRect(_client_area, BorderColor);
            }
            
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_left_arrow.Contains(e.Location) && (_slider_position > 0.01f))
            {
                _slider_position -= SliderIncrement;
            }
            if (_right_arrow.Contains(e.Location) && (_slider_position < 1.0f))
            {
                _slider_position += SliderIncrement;
            }
            if (_handler != null)
            {
                _handler(_slider_position);
            }
        }

    }
}
