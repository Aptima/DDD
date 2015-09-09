using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public class PanelProgressLabel : PanelControl
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
        public double Progress = 0;


        private Microsoft.DirectX.Direct3D.Font _font = null;
        private string _text;
        private int _text_size;

        private Material _border_color_material;
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
                _border_color_material = new Material();
                _border_color_material.Diffuse = value;
            }
        }

        private Material _background_color_material;
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
                _background_color_material = new Material();
                _background_color_material.Diffuse = value;
            }
        }

        private Material _foreground_color_material;
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
                _foreground_color_material = new Material();
                _foreground_color_material.Diffuse = value;
            }
        }

        private Material _progress_color_material;
        private Color _progress_color = Color.FromArgb(122, 123, 143);
        public Color ProgressColor
        {
            get
            {
                return _progress_color;
            }
            set
            {
                _progress_color = value;
                _progress_color_material = new Material();
                _progress_color_material.Diffuse = value;
            }
        }




        public PanelProgressLabel()
        {
            _client_area = Rectangle.Empty;
            _Sticky = true;
        }
        public PanelProgressLabel(int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            _text = string.Empty;
            _Sticky = true;
        }
        public PanelProgressLabel(string text, int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            _text = text;
            _Sticky = true;
        }

        public override void OnRender(Canvas c)
        {

            c.DrawProgressBar(_client_area, _background_color_material, _progress_color_material, Progress);
            _font.DrawText(null, _text, ClientArea, DrawTextFormat.VerticalCenter | DrawTextFormat.Center, ForegroundColor);

            c.DrawRect(_client_area, BorderColor);
        }
    
    }
}
