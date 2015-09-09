using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public class WindowManager : Scene
    {
        private Rectangle _visible_rect;

        protected List<Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel> PANELS;
        protected List<Aptima.Asim.DDD.Client.Common.GLCore.Window> WINDOWS;

        private MouseEventHandler _mouse_up;
        private MouseEventHandler _mouse_click;
        private MouseEventHandler _mouse_down;
        private MouseEventHandler _mouse_move;
        private MouseEventHandler _mouse_wheel;
        private MouseEventHandler _mouse_double_click;

        private KeyPressEventHandler _key_press;

        private KeyEventHandler _key_down;
        private KeyEventHandler _key_up;

        private IGameControl _game_control;

        public WindowManager(IGameControl game_control)
        {
            PANELS = new List<Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel>();
            WINDOWS = new List<Aptima.Asim.DDD.Client.Common.GLCore.Window>();
            _visible_rect = Rectangle.Empty;
            _game_control = game_control;
        }

        public void BindGameController()
        {

            _mouse_up = new MouseEventHandler(OnMouseUp);
            _game_control.GetTargetControl().MouseUp += _mouse_up;

            _mouse_click = new MouseEventHandler(OnMouseClick);
            _game_control.GetTargetControl().MouseClick += _mouse_click;

            _mouse_down = new MouseEventHandler(OnMouseDown);
            _game_control.GetTargetControl().MouseDown += _mouse_down;

            _mouse_move = new MouseEventHandler(OnMouseMove);
            _game_control.GetTargetControl().MouseMove += _mouse_move;

            _mouse_wheel = new MouseEventHandler(OnMouseWheel);
            _game_control.GetTargetControl().MouseWheel += _mouse_wheel;

            _mouse_double_click = new MouseEventHandler(OnMouseDoubleClick);
            _game_control.GetTargetControl().MouseDoubleClick += _mouse_double_click;

            _key_press = new KeyPressEventHandler(OnKeyPress);
            _game_control.GetTargetControl().KeyPress += _key_press;

            _key_down = new KeyEventHandler(OnKeyDown);
            _game_control.GetTargetControl().KeyDown += _key_down;

            _key_up = new KeyEventHandler(OnKeyUp);
            _game_control.GetTargetControl().KeyUp += _key_up;

        }


        public void UnbindGameController()
        {

            _game_control.GetTargetControl().MouseUp -= _mouse_up;
            _game_control.GetTargetControl().MouseClick -= _mouse_click;
            _game_control.GetTargetControl().MouseDown -= _mouse_down;
            _game_control.GetTargetControl().MouseMove -= _mouse_move;
            _game_control.GetTargetControl().MouseWheel -= _mouse_wheel;

            _game_control.GetTargetControl().MouseDoubleClick -= _mouse_double_click;

            _game_control.GetTargetControl().KeyPress -= _key_press;
            _game_control.GetTargetControl().KeyDown -= _key_down;
            _game_control.GetTargetControl().KeyUp -= _key_up;

        }

        public override void OnRender(Canvas canvas)
        {

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                p.OnRender(canvas);
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                w.OnRender(canvas);
            }

        }


        public Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel CreatePanel(int left, int top, int right, int bottom)
        {
            Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(left, top, right, bottom);
            int index = PANELS.Count;

            PANELS.Add(p);

            return PANELS[index];
        }


        public Aptima.Asim.DDD.Client.Common.GLCore.Window CreateWindow(Microsoft.DirectX.Direct3D.Font font, string caption, int left, int top, int right, int bottom)
        {
            Window win = new Window(caption, left, top, right, bottom, WindowState.MINIMIZED);
            win.Font = font;
            win.AllowMove = false;
            int index = WINDOWS.Count;

            WINDOWS.Add(win);

            return WINDOWS[index];
        }

        public void SetRootWindow(Rectangle dimension)
        {
            _visible_rect = dimension;
        }

        public void ScaleWindowManager(Canvas c, float Width, float Height)
        {
            _visible_rect.X = _visible_rect.Y = 0;
            _visible_rect.Height = (int)Height;
            _visible_rect.Width = (int)Width;
           
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                w.FitToTextureDimensions(c, Width, Height);
            }
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                p.FitToTextureDimensions(c, Width, Height);
            }

        }

        #region Events
        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected)
                {
                    w.OnKeyUp(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyUp(sender, e);
                    return;
                }
            }

        }

        public override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected)
                {
                    w.OnKeyPress(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyPress(sender, e);
                    return;
                }
            }
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected)
                {
                    w.OnKeyDown(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyDown(sender, e);
                    return;
                }
            }
        }

        public override void OnMouseWheel(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected)
                {
                    w.OnMouseWheel(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseWheel(sender, e);
                    return;
                }
            }
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected && w.State != WindowState.HIDE)
                {
                    w.OnMouseUp(sender, e);

                    // OnMouseUp can come as a result of a window drag, Check to make sure window
                    //     is still within our visible rectangle space.
                    if (!_visible_rect.Contains(w.ClientArea))
                    {
                        w.Minimize();
                    }

                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseUp(sender, e);
                    return;
                }
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.Selected)
                {
                    w.OnMouseMove(sender, e);
                    return;
                }
            }
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseMove(sender, e);
                    return;
                }
            }

        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.HitTest(e.Location) && (w.State != WindowState.HIDE))
                {
                    w.OnMouseDown(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.HitTest(e.Location))
                {
                    p.OnMouseDown(sender, e);
                    return;
                }
            }
        }

        public override void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.HitTest(e.Location) && (w.State != WindowState.HIDE))
                {
                    w.OnMouseDoubleClick(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.HitTest(e.Location))
                {
                    p.OnMouseDoubleClick(sender, e);
                    return;
                }
            }
        }

        public override void OnMouseClick(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Window w in WINDOWS)
            {
                if (w.HitTest(e.Location) && (w.State != WindowState.HIDE))
                {
                    w.OnMouseClick(sender, e);
                    return;
                }   

            }

            foreach (Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel p in PANELS)
            {
                if (p.HitTest(e.Location))
                {
                    p.OnMouseClick(sender, e);
                    return;
                }

            }

        }
        #endregion


    }
}
