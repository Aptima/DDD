using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;


namespace AGT.GameToolkit
{
    public enum HeadingStyle : int { Aptima = 0, MilStd = 1 };

    public class AGT_Heading
    {
        private const string Endpoint = "AGT.images.Endpoint.png";
        private AGT_SpriteId EndpointId = AGT_SpriteId.Empty;

        public int Length = 25;
        public int Width = 2;
        public HeadingStyle Style = HeadingStyle.Aptima;

        private Microsoft.DirectX.Direct3D.Line _line = null;
        private Vector2[] _line_points;

        private Color _foreground_color = Color.DodgerBlue;
        private AGT_SpriteManager _sprites;

        public Color Foreground
        {
            get
            {
                lock (this)
                {
                    return _foreground_color;
                }
            }
            set
            {
                lock (this)
                {
                    _foreground_color = value;
                }
            }
        }

       
        public AGT_Heading(Microsoft.DirectX.Direct3D.Device d)
        {
            _line_points = new Vector2[2];
            _line = new Line(d);
            _sprites = new AGT_SpriteManager(d);

            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(Endpoint)))
            {
                EndpointId = _sprites.AddResource(Endpoint, b, 0, 0, 0);
                _sprites.SetCenter(EndpointId, (float)(b.Width * .5f), (float)(b.Height * .5f), 0);
            }

        }

        public void SetPosition(float x1, float y1, float x2, float y2)
        {
            float deltaX = x2 - x1;
            float deltaY = y2 - y1;

            float lx = Math.Abs(deltaX);
            float ly = Math.Abs(deltaY);

            float angle = 0;

            if (ly != 0)
            {
                angle = (float)Math.Atan(ly / lx);
            }

            if (Style == HeadingStyle.MilStd)
            {
                _line_points[0].X = x1;
                _line_points[0].Y = y1;

                switch (deltaX < 0)
                {
                    case true:
                        _line_points[1].X = x1 - SolveForX(Length, angle);
                        break;
                    case false:
                        _line_points[1].X = x1 + SolveForX(Length, angle);
                        break;
                }
                switch (deltaY < 0)
                {
                    case true:
                        _line_points[1].Y = y1 - SolveForY(Length, angle);
                        break;
                    case false:
                        _line_points[1].Y = y1 + SolveForY(Length, angle);
                        break;
                }

            }
            else
            {
                _line_points[0].X = x1;
                _line_points[0].Y = y1;
                _line_points[1].X = x2;
                _line_points[1].Y = y2;
            }

        }

        private float SolveForY(float length, float angle)
        {
            return length * (float)Math.Sin(angle);
        }

        private float SolveForX(float length, float angle)
        {
            return length * (float)Math.Cos(angle);
        }

        public void Begin()
        {
            _line.Width = Width;
            _line.Antialias = true;

            _line.Begin();
        }

        public void Draw()
        {
            _line.Draw(_line_points, _foreground_color);
            
            if (Style == HeadingStyle.Aptima)
            {
                _sprites.SetPosition(EndpointId, _line_points[1].X, _line_points[1].Y, 0);

                _sprites.Begin(SpriteFlags.AlphaBlend);
                _sprites.Draw(EndpointId, Color.White);
                _sprites.End();

            }
        }


        public void End()
        {
            _line.End();
        }

    }
}
