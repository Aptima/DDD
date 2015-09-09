using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public enum PanelLayout:int  { HorizontalFree = 0, VerticalFree = 1, Horizontal = 2, Vertical = 3};

    public class Panel : BaseControl
    {
        public bool AlwaysRender = false;
        public PanelLayout Layout = PanelLayout.Horizontal;
        public Color Background = Color.Transparent;
        public bool BoundingBox = false;

        private List<PanelControl> Ctl_List = null;
        public List<PanelControl> Controls
        {
            get
            {
                return Ctl_List;
            }
        }

        private bool _moving;
        public bool Moving
        {
            set
            {
                _moving = value;
                foreach (PanelControl c in Ctl_List)
                {
                    c.Moving = value;
                }
            }
            get
            {
                return _moving;
            }
        }
        private bool _resizing;
        public bool Resizing
        {
            set
            {
                _resizing = value;
                foreach (PanelControl c in Ctl_List)
                {
                    c.Resizing = value;
                }
            }
            get
            {
                return _resizing;
            }
        }
        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                if (Ctl_List != null)
                {
                    if (Ctl_List.Count > 0)
                    {
                        switch (Selected)
                        {
                            case true:
                                Ctl_List[0].Selected = true;
                                break;
                            case false:
                                foreach (PanelControl p in Ctl_List)
                                {
                                    p.Selected = false;
                                }
                                break;
                        }
                    }
                }
            }
        }
        public Panel()
        {
            _client_area = Rectangle.Empty;
            base.Selected = false;
            Ctl_List = new List<PanelControl>();
        }

        public Panel(int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            
            base.Selected = false;

            Ctl_List = new List<PanelControl>();
        }


        public PanelControl BindPanelControl(PanelControl control)
        {
            int index = Ctl_List.Count;
            control.Parent = this;

            
            switch(Layout) {
                case PanelLayout.Horizontal:
                    control.ClientArea = _client_area;
                    Ctl_List.Add(control);

                    Layout_Horizontal();
                    break;
                case PanelLayout.Vertical:
                    control.ClientArea = _client_area;
                    Ctl_List.Add(control);

                    Layout_Vertical();
                    break;
                case PanelLayout.VerticalFree:
                    Ctl_List.Add(control);

                    Layout_VerticalFree();
                    break;
                case PanelLayout.HorizontalFree:
                    Ctl_List.Add(control);

                    Layout_HorzontalFree();
                    break;
            }

            return Ctl_List[index];
        }

        public void UnbindControl(PanelControl c)
        {
            if (Ctl_List.Contains(c))
            {
                Ctl_List.Remove(c);
            }
        }

        public void MoveTo(int x, int y)
        {
            _client_area.X = x;
            _client_area.Y = y;
            foreach (PanelControl c in Ctl_List)
            {
                c.ClientArea = _client_area;
            }

        }
        public void SetWidth(int width)
        {
            _client_area.Width = width;
        }
        public void SetHeight(int height)
        {
            _client_area.Height = height;
        }

        public void ResizeAll(Rectangle position)
        {
            _client_area = position;
            foreach (PanelControl c in Ctl_List)
            {
                c.ClientArea = position;
            }
        }

        public void Layout_Horizontal()
        {
            if (Ctl_List.Count > 1)
            {
                int width = _client_area.Width / Ctl_List.Count;
                for (int i = 0; i < Ctl_List.Count; i++)
                {
                    Ctl_List[i].ClientArea = new Rectangle(_client_area.X + (i * width), _client_area.Y, width - 2, _client_area.Height);
                }
            }
        }
        public void Layout_Vertical()
        {
            if (Ctl_List.Count > 1)
            {
                int height = _client_area.Height / Ctl_List.Count;
                for (int i = 0; i < Ctl_List.Count; i++)
                {
                    Ctl_List[i].ClientArea = new Rectangle(_client_area.X, _client_area.Y + (i * height), _client_area.Width, height - 2);
                }
            }
        }
        public void Layout_VerticalFree()
        {
            if (Ctl_List.Count > 0)
            {
                Ctl_List[0].SetPosition(_client_area.X, _client_area.Y);
                _client_area.Width = Ctl_List[0].ClientArea.Width;
                _client_area.Height = Ctl_List[0].ClientArea.Height;
                if (Ctl_List.Count > 1)
                {
                    for (int i = 1; i < Ctl_List.Count; i++)
                    {
                        Ctl_List[i].SetPosition(_client_area.X, Ctl_List[i - 1].ClientArea.Bottom);
                        _client_area.Height += Ctl_List[i].ClientArea.Height;
                    }
                }
            }
        }
        public void Layout_HorzontalFree()
        {
            if (Ctl_List.Count > 0)
            {
                Ctl_List[0].SetPosition(_client_area.X, _client_area.Y);
                _client_area.Width = Ctl_List[0].ClientArea.Width;
                _client_area.Height = Ctl_List[0].ClientArea.Height;
                if (Ctl_List.Count > 1)
                {
                    for (int i = 1; i < Ctl_List.Count; i++)
                    {
                        Ctl_List[i].SetPosition(Ctl_List[i - 1].ClientArea.Right, _client_area.Y);
                        _client_area.Width += Ctl_List[i].ClientArea.Width;
                    }
                }
            }
        }

        public new void FitToTextureDimensions(Canvas c, float TextureWidth, float TextureHeight)
        {
            _client_area.X = (int)(c.Size.Width * _client_area.X / TextureWidth);
            _client_area.Y = (int)(c.Size.Height * _client_area.Y / TextureHeight);
            _client_area.Width = (int)(c.Size.Width * _client_area.Width / TextureWidth);
            _client_area.Height = (int)(c.Size.Height * _client_area.Height / TextureHeight);

            foreach (PanelControl p in Ctl_List)
            {
                p.FitToTextureDimensions(c, TextureWidth, TextureHeight);
            }
        }


        public override void OnRender(Canvas c)
        {
            if (!Hide)
            {
                if ((Ctl_List.Count > 0) || AlwaysRender)
                {
                    foreach (PanelControl ctl in Ctl_List)
                    {
                        ctl.OnRender(c);
                    }
                }

            }
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.Selected)
                {
                    ctl.OnKeyDown(sender, e);
                    return;
                }
            }

        }
        public override void OnKeyPress(object sender, KeyPressEventArgs e)
        {

            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.Selected)
                {
                    ctl.OnKeyPress(sender, e);
                    return;
                }
            }
        }
        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.Selected)
                {
                    ctl.OnKeyUp(sender, e);
                    return;
                }
            }
        }
        public override void OnMouseClick(object sender, MouseEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.HitTest(e.Location))
                {
                    ctl.OnMouseClick(sender, e);
                }
            }
        }


        public override void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //foreach (PanelControl ctl in Ctl_List)
            //{
            //    switch (ctl.Sticky)
            //    {
            //        case false:
            //            if (ctl.HitTest(e.Location))
            //            {
            //                ctl.OnMouseDoubleClick(sender, e);
            //                return;
            //            }
            //            break;
            //        case true:
            //            if (ctl.Selected)
            //            {
            //                ctl.OnMouseDoubleClick(sender, e);
            //                return;
            //            }
            //            break;
            //    }
            //}

        }
        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.HitTest(e.Location))
                {
                    ctl.OnMouseDown(sender, e);
                } 
            }
        }
        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                switch (ctl.Sticky)
                {
                    case false:
                        if (ctl.HitTest(e.Location))
                        {
                            ctl.OnMouseMove(sender, e);
                        }
                        break;
                    case true:
                        if (ctl.Selected)
                        {
                            ctl.OnMouseMove(sender, e);
                        }
                        break;
                }
            }

        }


        public override void OnMouseUp(object sender, MouseEventArgs e)
        {

            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.HitTest(e.Location))
                {
                   ctl.OnMouseUp(sender, e);
                }
            }

        }

        public override void OnMouseWheel(object sender, MouseEventArgs e)
        {
            foreach (PanelControl ctl in Ctl_List)
            {
                if (ctl.HitTest(e.Location))
                {
                    ctl.OnMouseWheel(sender, e);
                }
            }
        }

    }
}
