using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Gui.Common.GameLib.GameObjects;

namespace Aptima.Asim.DDD.Gui.Common.GameLib
{
    public class HeadsUpDisplay : WindowManager
    {
        protected List<Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel> PANELS;


        public HeadsUpDisplay(IGameControl game_control)
        {
            PANELS = new List<Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel>();

            game_control.GetTargetControl().MouseClick += new MouseEventHandler(MouseClick);
            game_control.GetTargetControl().MouseDoubleClick += new MouseEventHandler(MouseDoubleClick);
            game_control.GetTargetControl().MouseDown += new MouseEventHandler(MouseDown);
            game_control.GetTargetControl().MouseMove += new MouseEventHandler(MouseMove);
            game_control.GetTargetControl().MouseUp += new MouseEventHandler(MouseUp);
            game_control.GetTargetControl().MouseWheel += new MouseEventHandler(MouseWheel);
            game_control.GetTargetControl().KeyDown += new KeyEventHandler(KeyDown);
            game_control.GetTargetControl().KeyPress += new KeyPressEventHandler(KeyPress);
            game_control.GetTargetControl().KeyUp += new KeyEventHandler(KeyUp);
        }

        public override void OnRender(Canvas canvas)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                p.OnRender(canvas);
            }

            base.OnRender(canvas);
        }

        public Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel CreatePanel(int left, int top, int right, int bottom)
        {
            Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p = new Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel(left, top, right, bottom);
            int index = PANELS.Count;

            PANELS.Add(p);

            return PANELS[index];
        }

        public void ScaleHeadsUpDisplay(Canvas c, float TextureWidth, float TextureHeight)
        {
            ScaleWindows(c, TextureWidth, TextureHeight);
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                p.FitToTextureDimensions(c, TextureWidth, TextureHeight);
            }
        }



        #region Events
        public override void KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    w.Panel.OnKeyUp(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyUp(sender, e);
                    return;
                }
            }
        }



        public override void KeyPress(object sender, KeyPressEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    w.Panel.OnKeyPress(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyPress(sender, e);
                    return;
                }
            }
        }



        public override void KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    w.Panel.OnKeyDown(sender, e);
                    return;
                }
            }
            
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnKeyDown(sender, e);
                    return;
                }
            }
        }




        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    w.Panel.OnMouseWheel(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseWheel(sender, e);
                    return;
                }
            }
        }




        public override void MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (w.Panel.Moving)
                        {
                            w.Panel.Moving = false;
                        }
                        if (w.Panel.ReSizing)
                        {
                            w.Panel.ReSizing = false;
                            if (w.Panel.ClientArea.Width < w.DefaultLocation.Width)
                            {
                                w.Location = w.Panel.ClientArea;
                                w.Location.Width = w.DefaultLocation.Width;
                                w.Panel.ResetDimensions(w.Location);
                            }
                            if (w.Panel.ClientArea.Height < w.DefaultLocation.Height)
                            {
                                w.Location = w.Panel.ClientArea;
                                w.Location.Height = w.DefaultLocation.Height;
                                w.Panel.ResetDimensions(w.Location);
                            }
                        }
                    }

                    w.Panel.OnMouseUp(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseUp(sender, e);
                    return;
                }
            }
        }





        public override void MouseMove(object sender, MouseEventArgs e)
        {
            //foreach (GameLib.Window w in WINDOWS)
            //{
            //    if (w.Panel.Selected)
            //    {
            //        w.Panel.OnMouseMove(sender, e);
            //        return;
            //    }
            //}
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.Selected)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (w.CaptionArea.Contains(e.Location))
                        {
                            w.Panel.Moving = true;
                            w.Panel.ReSizing = false;
                        }
                        if (w.ResizeArea.Contains(e.Location))
                        {
                            w.Panel.ReSizing = true;
                            w.Panel.Moving = false;
                        }
                        if (w.Panel.Moving)
                        {
                            w.Location = w.Panel.ClientArea;
                            w.Location.X = e.X - (w.Location.Width / 2);
                            w.Location.Y = e.Y;
                            w.Panel.ResetDimensions(w.Location);
                        }
                        if (w.Panel.ReSizing)
                        {
                            w.Location = w.Panel.ClientArea;
                            w.Location.Width = (e.X - w.Location.X) + (w.ResizeArea.Width / 2);
                            w.Location.Height = e.Y - w.Location.Y + (w.ResizeArea.Height / 2);
                            w.Panel.ResetDimensions(w.Location);
                        }
                    }
                    w.Panel.OnMouseMove(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.Selected)
                {
                    p.OnMouseMove(sender, e);
                    return;
                }
            }
        }





        public override void MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.HitTest(e.Location))
                {
                    w.Panel.OnMouseDown(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.HitTest(e.Location))
                {
                    p.OnMouseDown(sender, e);
                    return;
                }
            }
        }





        public override void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.CaptionArea.Contains(e.Location) && (e.Button == MouseButtons.Left))
                {
                    switch (w.State)
                    {
                        case WindowState.MINIMIZED:
                            w.Maximize();
                            break;
                        case WindowState.MAXIMIZED:
                            w.Minimize();
                            break;
                    }
                }
                else
                if (w.Panel.HitTest(e.Location))
                {
                    w.Panel.OnMouseDoubleClick(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
            {
                if (p.HitTest(e.Location))
                {
                    p.OnMouseDoubleClick(sender, e);
                    return;
                }
            }
        }





        public override void MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Window w in WINDOWS)
            {
                if (w.Panel.HitTest(e.Location))
                {
                    w.Panel.OnMouseClick(sender, e);
                    return;
                }
            }

            foreach (Aptima.Asim.DDD.Gui.Common.GameLib.Gui.Panel p in PANELS)
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
