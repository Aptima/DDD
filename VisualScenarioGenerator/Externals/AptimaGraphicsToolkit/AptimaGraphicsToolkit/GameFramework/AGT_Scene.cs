using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

// Necessary for DllImport and Performance Counter stuff.
using System.Runtime.InteropServices;

using AGT.Sprites;
using AGT.Forms;

namespace AGT.GameFramework
{
    public enum SceneState : int { INIT = 0, RENDER = 1, END = 2, REINIT = 3, ERROR = 4 }
    public delegate void RenderableLayer(Device d, float frame_rate);

    public abstract class AGT_Scene: IAGT_SceneLoadDialog, IAGT_SplashDialog
    {
        private Point _cursor_location = new Point();
        public int CursorX
        {
            get
            {
                return _cursor_location.X;
            }
        }
        public int CursorY
        {
            get
            {
                return _cursor_location.Y;
            }
        }
        public bool ShowMouseCursor = false;
        public bool ShowSplashScreen = false;

        public SceneState _state = SceneState.INIT;
        public SceneState State
        {
            get
            {
                lock (this)
                {
                    return _state;
                }
            }
            set
            {
                lock (this)
                {
                    _state = value;
                }
            }
        }
        public Matrix _Projection_ = Matrix.Identity;
        public Matrix _View_ = Matrix.Identity;

        public AGT_SpriteId CursorImage
        {
            set
            {
                if (GameFramework != null)
                {
                    GameFramework.Cursor = value;
                }
            }
            get
            {
                if (GameFramework != null)
                {
                    return GameFramework.Cursor;
                }
                return AGT_SpriteId.Empty;
            }
        }
        protected Microsoft.DirectX.Direct3D.Line _Line_ = null;
        private Vector2[] _line_pts = null;

        public AGT_GameFramework GameFramework = null;


        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceCounter(ref long PerformanceCount); 


        public System.Drawing.Rectangle TargetControlRect = System.Drawing.Rectangle.Empty;

        protected List<RenderableLayer> RenderLayers;





        public void SetCursorPosition(int x, int y)
        {
            _cursor_location.X = x;
            _cursor_location.Y = y;
        }
        public Point GetCursorPosition()
        {
            return _cursor_location;
        }

        protected void InitializeLine(Device d)
        {
            _Line_ = new Line(d);
        }
        protected void SetLineOptions(bool antialias, float width)
        {
            if (_Line_ != null)
            {
                _Line_.Antialias = antialias;
                _Line_.Width = width;
                _line_pts = new Vector2[2];
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }
        }
        protected void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            if (_Line_ != null)
            {
                _line_pts[0].X = x1;
                _line_pts[1].Y = y1;
                _line_pts[0].X = x2;
                _line_pts[1].Y = y2;

                _Line_.Begin();
                _Line_.Draw(_line_pts, color);
                _Line_.End();                
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }
        }
        protected void DrawLines(Vector2[] points, Color color)
        {
            if (_Line_ != null)
            {
                _Line_.Begin();
                _Line_.Draw(points, color);
                _Line_.End();
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }
        }
        protected void DrawRectangle(Rectangle rect, Color color)
        {
            if (_Line_ != null)
            {
                _Line_.Begin();

                _line_pts[0].X = rect.Left;
                _line_pts[0].Y = rect.Top;
                _line_pts[1].X = rect.Right;
                _line_pts[1].Y = rect.Top;
                _Line_.Draw(_line_pts, color);

                _line_pts[0].X = rect.Left;
                _line_pts[0].Y = rect.Top;
                _line_pts[1].X = rect.Left;
                _line_pts[1].Y = rect.Bottom;
                _Line_.Draw(_line_pts, color);

                _line_pts[0].X = rect.Right;
                _line_pts[0].Y = rect.Bottom;
                _line_pts[1].X = rect.Right;
                _line_pts[1].Y = rect.Top;
                _Line_.Draw(_line_pts, color);

                _line_pts[0].X = rect.Right;
                _line_pts[0].Y = rect.Bottom;
                _line_pts[1].X = rect.Left;
                _line_pts[1].Y = rect.Bottom;
                _Line_.Draw(_line_pts, color);

                _Line_.End();
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }

        }

        protected void DrawTriangle(Rectangle rect, Color color)
        {
            if (_Line_ != null)
            {
                _Line_.Begin();

                _line_pts[0].X = rect.Left + (int)(rect.Width * .5f);
                _line_pts[0].Y = rect.Top;
                _line_pts[1].X = rect.Right;
                _line_pts[1].Y = rect.Bottom;
                _Line_.Draw(_line_pts, color);

                _line_pts[0].X = _line_pts[0].X;
                _line_pts[0].Y = rect.Top;
                _line_pts[1].X = rect.Left;
                _line_pts[1].Y = rect.Bottom;
                _Line_.Draw(_line_pts, color);

                _line_pts[0].X = rect.Right;
                _line_pts[0].Y = rect.Bottom;
                _line_pts[1].X = rect.Left;
                _line_pts[1].Y = rect.Bottom;
                _Line_.Draw(_line_pts, color);

                _Line_.End();
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }

        }

        protected void DrawFillRectangle(Rectangle rect, Color color)
        {
            if (_Line_ != null)
            {
                SetLineOptions(true, rect.Height);
                _Line_.Begin();

                _line_pts[0].X = rect.Left;
                _line_pts[0].Y = rect.Top + (int)(rect.Height * .5f);
                _line_pts[1].X = rect.Right;
                _line_pts[1].Y = _line_pts[0].Y;

                _Line_.Draw(_line_pts, color);
                
                _Line_.End();
            }
            else
            {
                throw new NullReferenceException("Line resource must be initialized with call to \"InitializeLine(Device d)\"");
            }
        }
        public void RenderScene(Device d, float frame_rate)
        {
            if (d == null)
            {
                throw new ArgumentException("Device is null");
            }
            switch (State)
            {
                case SceneState.INIT:
                    RenderLayers = new List<RenderableLayer>();
                    TargetControlRect = System.Drawing.Rectangle.FromLTRB(0, 0, d.Viewport.Width, d.Viewport.Height);
                    OnInitialize(d);
                    return;
                case SceneState.REINIT:
                    TargetControlRect = System.Drawing.Rectangle.FromLTRB(0, 0, d.Viewport.Width, d.Viewport.Height);
                    OnReInitialize(d);
                    return;
                case SceneState.RENDER:
                    foreach (RenderableLayer layer in RenderLayers)
                    {
                        layer(d, frame_rate);
                    }
                    break;
                default:
                    return;
            }
        }

        public static float ToRadians(float degrees)
        {
            return degrees * 0.0174532925f;
        }
        public static float ToDegrees(float radians)
        {
            return radians * 57.2957795f;
        }


        public abstract void OnInitialize(Device d);
        public abstract void OnReInitialize(Device d);

        public virtual void OnKeyUp(object sender, KeyEventArgs e)
        {
        }

        public virtual void OnKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        public virtual void OnMouseWheel(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
        }




        #region IAGT_SceneLoadDialog Members

        public void DialogInitialize(AGT_SceneLoadDialog dialog_instance)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LoadResources(AGT_SceneLoadDialog dialog_instance, Device device)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IAGT_SplashDialog Members

        public void DialogInitialize(AGT_SplashDialog dialog_instance)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void LoadResources(AGT_SplashDialog dialog_instance, Device device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
