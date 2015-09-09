using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;


namespace AGT.GameToolkit
{
    public class AGT_Waypoint
    {
        public int Width
        {
            get
            {
                lock (this)
                {
                    return _width;
                }
            }
            set
            {
                lock (this)
                {
                    _width = value;
                }
            }
        }
        public int Height
        {
            get
            {
                lock (this)
                {
                    return _height;
                }
            }
            set
            {
                lock (this)
                {
                    _height = value;
                }
            }
        }

        public Color Diffuse = Color.White;
        public float X
        {
            get
            {
                lock (this)
                {
                    return _point.X;
                }
            }
        }
        public float Y
        {
            get
            {
                lock (this)
                {
                    return _point.Y;
                }
            }
        }
        public float Z
        {
            get
            {
                lock (this)
                {
                    return _point.Z;
                }
            }
        }



        private static int _width = 25;
        private static int _height = 25;
        private string _id = string.Empty;
        private Vector3 _point = Vector3.Empty;
        private Rectangle _collision_rect = Rectangle.Empty;
        


        public AGT_Waypoint(string id, Vector3 point)
        {
            _id = id;
            SetPoint(point);
        }


        public AGT_Waypoint(string id, float x, float y, float z)
        {
            _id = id;
            SetPoint(x, y, z);
        }


        public void SetPoint(float x, float y, float z)
        {
            lock (this)
            {
                _point.X = x;
                _point.Y = y;
                _point.Z = z;
                _collision_rect.X = (int)(x - (Width * .5f));
                _collision_rect.Y = (int)(y - (Height * .5f));
                _collision_rect.Width = Width;
                _collision_rect.Height = Height; 
            }
        }


        public void SetPoint(Vector3 point)
        {
            SetPoint(point.X, point.Y, point.Z);
        }

        public bool HitTest(int x, int y)
        {
            lock (this)
            {
                return (_collision_rect.Contains(x, y));
            }
        }

    }

}
