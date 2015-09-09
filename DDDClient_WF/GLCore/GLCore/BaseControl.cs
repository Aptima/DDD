using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Gui.Common.GameLib.Gui
{
    public abstract class BaseControl: IRenderable
    {

        public bool Sticky
        {
            get
            {
                return _Sticky;
            }
            set
            {
                _Sticky = value;
            }
        }
        private bool _selected = false;
        public virtual bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        public bool Hide = false;
        public Rectangle ClientArea
        {
            get
            {
                return this._client_area;
            }
            set
            {
                this._client_area = value;
            }

        }

        #region Protected Members


        protected Rectangle _client_area = Rectangle.Empty;

        protected bool _Sticky = false;

        protected MouseEventHandler _MouseClick = null;
        protected MouseEventHandler _MouseDoubleClick = null;
        protected MouseEventHandler _MouseDown = null;
        protected MouseEventHandler _MouseMove = null;
        protected MouseEventHandler _MouseUp = null;
        protected MouseEventHandler _MouseWheel = null;

        protected KeyEventHandler _KeyDown = null;
        protected KeyEventHandler _KeyUp = null;

        protected KeyPressEventHandler _KeyPress = null;
        #endregion

        #region Private Members
        private Rectangle _mouse_click_area = Rectangle.Empty;
        #endregion



        public bool HitTest(Point pt)
        {
                Selected = _client_area.Contains(pt);
                return Selected;
        }

        public void FitToTextureDimensions(Canvas c, float TextureWidth, float TextureHeight)
        {
            _client_area.X = (int)(c.Size.Width * _client_area.X / TextureWidth);
            _client_area.Y = (int)(c.Size.Height * _client_area.Y / TextureHeight);
            _client_area.Width = (int)(c.Size.Width * _client_area.Width / TextureWidth);
            _client_area.Height = (int)(c.Size.Height * _client_area.Height / TextureHeight);
        }

        public void DrawBoundingBox(Canvas c)
        {
            c.DrawRect(_client_area, Color.Yellow);
        }



        #region Events
        public virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (_MouseClick != null)
            {
                _MouseClick(sender, e);
            }
        }
        public virtual void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_MouseDoubleClick != null)
            {
                _MouseDoubleClick(sender, e);
            }
        }

        public virtual void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (_MouseWheel != null)
            {
                _MouseWheel(sender, e);
            }
        }

        public virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_MouseUp != null)
            {
                _MouseUp(sender, e);
            }
        }
        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_MouseDown != null)
            {
                _MouseDown(sender, e);
            }
        }


        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_MouseMove != null)
            {
                _MouseMove(sender, e);
            }
        }


        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_KeyDown != null)
            {
                _KeyDown(sender, e);
            }
        }

        public virtual void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (_KeyUp != null)
            {
                _KeyUp(sender, e);
            }
        }

        public virtual void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_KeyPress != null)
            {
                _KeyPress(sender, e);
            }
        }
        #endregion


        #region IRenderable Members

        public virtual void OnRender(Canvas c)
        {
            if ((Selected || Sticky) && !Hide)
            {
                DrawBoundingBox(c);
            }
        }
        #endregion
    }

    public class PanelControl : BaseControl
    {
        public bool Moving = false;
        public bool Resizing = false;
        public bool HasDocument = false;

        public Panel Parent;
        protected Microsoft.DirectX.Direct3D.Font _font;

        public Microsoft.DirectX.Direct3D.Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        public PanelControl()
        {
            Parent = null;
        }
        public virtual float DocumentWidth
        {
            get
            {
                return 0f;
            }
        }
        public virtual float DocumentHeight
        {
            get
            {
                return 0f;
            }
        }
        public virtual float DocumentPositionX
        {
            get
            {
                return 0f;
            }
            set
            {
                ;
            }
        }
        public virtual float DocumentPositionY
        {
            get
            {
                return 0f;
            }
            set
            {
                ;
            }
        }


        public void SetClientArea(int x1, int y1, int x2, int y2)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
        }

    }


}
