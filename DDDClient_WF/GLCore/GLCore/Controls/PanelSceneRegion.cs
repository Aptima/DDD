using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{

    public class PanelSceneRegion : PanelControl
    {
        #region Mouse and Keyboard Handlers
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
        #endregion



        private Scene _scene;
        private Vector3 _Scaling;
        private Microsoft.DirectX.Direct3D.Viewport view;



        public PanelSceneRegion()
        {
            _client_area = Rectangle.Empty;
            _Sticky = true;
        }


        public PanelSceneRegion(int x1, int y1, int x2, int y2, Scene scene)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;

            _scene = scene;

            view = new Viewport();
            view.X = _client_area.X;
            view.Y = _client_area.Y;
            view.Height = _client_area.Height;
            view.Width = _client_area.Width;

            _Sticky = true;
        }


        public override void OnRender(Canvas c)
        {
            Microsoft.DirectX.Direct3D.Viewport backup = c.Viewport;

            view.X = _client_area.X;
            view.Y = _client_area.Y;
            view.Height = _client_area.Height;
            view.Width = _client_area.Width;

            c.Viewport = this.view;
            c.ClearDevice(Microsoft.DirectX.Direct3D.ClearFlags.Target, Color.Black, 0, 0);

            _scene.OnRender(c);

            c.Viewport = backup;
            if (Selected)
            {
                DrawBoundingBox(c);
            }

        }
    }

}
