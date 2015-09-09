using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AGT.GameToolkit
{
    public class AGT_Label
    {

        private Microsoft.DirectX.Direct3D.Line _line = null;
        private Vector2[] _line_points;

        public AGT_Label(Microsoft.DirectX.Direct3D.Device d)
        {
            _line_points = new Vector2[2];

            _line = new Line(d);
        }
        

        public void Begin(int width)
        {
            _line.Antialias = true;
            _line.Width = width;
            
            _line.Begin();
        }

        public void End()
        {
            _line.End();
        }


        public void Draw(Color color, Color border_color, AGT_Text label)
        {
            _line_points[0].X = (label.X );
            _line_points[0].Y = (float)(label.Y - 1);
            _line_points[1].X = (float)(_line_points[0].X + label.Width) ;
            _line_points[1].Y = (float)(_line_points[0].Y);

            _line.Draw(_line_points, border_color);

            _line_points[0].X = (float)(label.X) ;
            _line_points[0].Y = (float)(label.Y + 1);
            _line_points[1].X = (float)(_line_points[0].X + label.Width);
            _line_points[1].Y = (float)(_line_points[0].Y);

            _line.Draw(_line_points, border_color);


            _line_points[0].X = (float)(label.X + 1);
            _line_points[0].Y = (float)(label.Y);
            _line_points[1].X = (float)(label.X + label.Width - 1) ;
            _line_points[1].Y = (float)(label.Y);

            _line.Draw(_line_points, label.Background);


            label.DrawMessage(color);
        }

        public void DrawRelative(Color color, Color border_color, AGT_Text label, float x_offset, float y_offset)
        {
            _line_points[0].X = (float)(label.X)+ x_offset ;
            _line_points[0].Y = (float)((label.Y - 1))+ y_offset;
            _line_points[1].X = (float)(_line_points[0].X + label.Width);
            _line_points[1].Y = (float)(_line_points[0].Y);

            _line.Draw(_line_points, border_color);

            _line_points[0].X = (float)(label.X)+x_offset;
            _line_points[0].Y = (float)((label.Y + 1)) + y_offset;
            _line_points[1].X = (float)(_line_points[0].X + label.Width);
            _line_points[1].Y = (float)(_line_points[0].Y);

            _line.Draw(_line_points, border_color);


            _line_points[0].X = (float)((label.X + 1))+ x_offset ;
            _line_points[0].Y = (float)(label.Y) + y_offset;
            _line_points[1].X = (float)((label.X + label.Width - 1) )+ x_offset;
            _line_points[1].Y = (float)(label.Y) + y_offset;

            _line.Draw(_line_points, label.Background);


            label.DrawMessageRelative(color, x_offset, y_offset);
        }


    }

}
