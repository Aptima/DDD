using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using GameLib;
using GameLib.GameObjects;
using GameLib.PathController;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GameLib
{
    public class HotSpot
    {
        private Rectangle _IntersectRect = Rectangle.Empty;
        private Rectangle _HotSpotArea = Rectangle.Empty;
        private MouseEventHandler _HotSpotCallBack = null;

        public Rectangle AreaRect
        {
            get
            {
                return _HotSpotArea;
            }
            set
            {
                _HotSpotArea = value;
            }
        }

        public int X
        {
            get
            {
                return _HotSpotArea.X;
            }
            set
            {
                _HotSpotArea.X = value;
            }
        }
        public int Y
        {
            get
            {
                return _HotSpotArea.Y;
            }
            set
            {
                _HotSpotArea.Y = value;
            }
        }
        public int Width
        {
            get
            {
                return _HotSpotArea.Width;
            }
            set
            {
                _HotSpotArea.Width = value;
            }
        }
        public int Height
        {
            get
            {
                return _HotSpotArea.Height;
            }
            set
            {
                _HotSpotArea.Height = value;
            }
        }

        public HotSpot(Rectangle area, MouseEventHandler callback)
        {
            _HotSpotArea = area;
            _HotSpotCallBack = callback;
        }
        public HotSpot(int x1, int y1, int x2, int y2, MouseEventHandler callback)
        {
            _HotSpotArea.X = x1;
            _HotSpotArea.Y = y1;
            _HotSpotArea.Width = x2 - x1;
            _HotSpotArea.Height = y2 - y1;
            _HotSpotCallBack = callback;
        }

        public bool HotSpotIntersect(int x, int y)
        {
            _IntersectRect.X = x;
            _IntersectRect.Y = y;
            _IntersectRect.Width = 1;
            _IntersectRect.Height = 1;
            return _HotSpotArea.IntersectsWith(_IntersectRect);
        }
        public void HotSpotNotify(object sender, MouseEventArgs event_arg)
        {
            if (HotSpotIntersect(event_arg.X, event_arg.Y))
            {
                if (_HotSpotCallBack != null)
                {
                    _HotSpotCallBack(sender, event_arg);
                }
            }
        }
    }
}
