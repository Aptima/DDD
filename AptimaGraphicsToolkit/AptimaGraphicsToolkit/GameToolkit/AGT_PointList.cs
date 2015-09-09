using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using AGT.Sprites;

namespace AGT.GameToolkit
{
    public enum FillModeType : int { Outline, Fill, None };

    public class AGT_PointList
    {
        public static AGT_PointList Empty = new AGT_PointList(string.Empty, FillModeType.None);
        public bool ClosedShape = false;
        public bool DrawJoints = false;

        public string Id = string.Empty;
        public PointF Location = PointF.Empty;

        public Brush Brush = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(120, Color.Lime));
        public Pen Pen = new Pen(Color.Lime, 2);
        
        public AGT_SpriteId TypeId = AGT_SpriteId.Empty;
        public FillModeType FillMode = FillModeType.Outline;

        private GraphicsPath _graphics_path;
        private PointF _offset = PointF.Empty;
        private Bitmap _image = null;
        private Microsoft.DirectX.Vector3 _location = Microsoft.DirectX.Vector3.Empty;




        public AGT_PointList(string id, FillModeType fill_type)
        {
            Id = id;
            TypeId.Id = id;
            _graphics_path = new GraphicsPath();
            FillMode = fill_type;
        }
        

        public void AddLine(float x1, float y1, float x2, float y2)
        {
            _graphics_path.AddLine(x1, y1, x2, y2);
            RectangleF rect = _graphics_path.GetBounds();
            Location.X = rect.X;
            Location.Y = rect.Y;
        }


        public void AddLines(Point[] points)
        {
            if (ClosedShape)
            {
                points[points.Length - 1].X = points[0].X;
                points[points.Length - 1].Y = points[0].Y;
            }

            _graphics_path.AddLines(points);

            if (DrawJoints)
            {
                foreach (Point p in points)
                {
                    _graphics_path.AddEllipse(p.X - 2, p.Y - 2, 4, 4);
                }
            }

            RectangleF rect = RectangleF.Empty;
            rect = _graphics_path.GetBounds();
            Location.X = rect.X;
            Location.Y = rect.Y;

        }

        public void CreateTestShape(Microsoft.DirectX.Direct3D.Device video_device)
        {
            _graphics_path.AddEllipse(46, 4, 28, 28);
            _graphics_path.AddLine(36, 32, 84, 32);
            _graphics_path.AddLine(100, 80, 88, 84);
            _graphics_path.AddLine(76, 50, 74, 84);
            _graphics_path.AddLine(90, 150, 74, 150);
            _graphics_path.AddLine(60, 100, 46, 150);
            _graphics_path.AddLine(32, 150, 46, 84);
            _graphics_path.AddLine(44, 50, 32, 84);
            _graphics_path.AddLine(20, 80, 36, 32);
            _image = AGT_GDIBridge.CreatePolygon(video_device, _graphics_path, Pen);

        }

        public Bitmap ToBitmap(Microsoft.DirectX.Direct3D.Device video_device)
        {
            switch (FillMode)
            {
                case FillModeType.Fill:
                    _image = AGT_GDIBridge.CreateFillPolygon(video_device, _graphics_path, Brush);
                    break;
                case FillModeType.Outline:
                case FillModeType.None:
                    _image = AGT_GDIBridge.CreatePolygon(video_device, _graphics_path, Pen);
                    break;
            }
            return _image;
        }

        public void MoveTo(float x, float y)
        {
            Location.X = x;
            Location.Y = y;
        }

        public bool HitTest(int mouse_x, int mouse_y)
        {
            RectangleF _collision_rect = _graphics_path.GetBounds();
            _collision_rect.X = Location.X;
            _collision_rect.Y = Location.Y;
            // Need to handle Scaling as well
            return _collision_rect.Contains(mouse_x, mouse_y);
        }


    }
}
