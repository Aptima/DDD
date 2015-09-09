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
    public enum WindowState : int { MINIMIZED = 0, MAXIMIZED = 1, SHOW = 3, HIDE = 4 };
    public class Window: IRenderable, IDeviceInput
    {
        private bool CanHScroll = true;
        private bool CanVScroll = true;

        public int ScrollWidth = 18;
        public int ScrollChange = 10;

        public bool AllowResize;
        public bool AllowMove;
        public bool AllowShade;
        public bool HasScrollBars;

        public bool Selected
        {
            get
            {
                return _main_panel.Selected;
            }
            set
            {
                _main_panel.Selected = value;
            }
        }
        public bool Moving
        {
            get
            {
                return _main_panel.Moving;
            }
            set
            {
                _main_panel.Moving = value;
            }
        }

        public bool AlwaysRender
        {
            get
            {
                return _main_panel.AlwaysRender;
            }
            set
            {
                _main_panel.AlwaysRender = value;
            }
        }
        public bool Resizing
        {
            get
            {
                return _main_panel.Resizing;
            }
            set
            {
                _main_panel.Resizing = value;
            }
        }

        
        public int CaptionHeight
        {
            set
            {
                _caption_bar.Height = value;
            }
            get
            {
                return _caption_bar.Height;
            }
        }
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
        public WindowState State;
        public string CaptionText = string.Empty;

        private static Color _scroll_background = Color.FromArgb(204, 122, 123, 143);
        private static Material _scroll_background_material = new Material();

        private static Color _scroll_color = Color.FromArgb(204, 221, 222, 200);
        private static Material _scroll_color_material = new Material();

        private static Color _border_color = Color.FromArgb(204, 150, 150, 160);
        private static Material _border_color_material = new Material();

        private static Color _caption_color = Color.FromArgb(204, 122, 123, 143);
        private static Material _caption_color_material = new Material();

        private static Color _caption_foreground = Color.FromArgb(204, 221, 222, 200);
        private static Material _caption_foreground_material = new Material();

        private static Color _background_color = Color.FromArgb(204, 63, 63, 63);
        private static Material _background_color_material = new Material();

        public Rectangle DefaultLocation;
        
        public Rectangle ResizeButton
        {
            get
            {
                return _resize_btn;
            }
        }
        
        public Rectangle CaptionBar
        {
            get
            {
                return _caption_bar;
            }
        }
        
        public Rectangle ShadeButton
        {
            get
            {
                return _shade_btn;
            }
        }
        
        public static Color BorderColor
        {
            get
            {
                return _border_color;
            }
            set
            {
                _border_color = value;
                _border_color_material.Diffuse = value;
            }
        }

        public static Color CaptionColor
        {
            get
            {
                return _caption_color;
            }
            set
            {
                _caption_color = value;
                _caption_color_material.Diffuse = value;
            }
        }
        
        public static Color CaptionForeground
        {
            get
            {
                return _caption_foreground;
            }
            set
            {
                _caption_foreground = value;
                _caption_foreground_material.Diffuse = value;
            }
        }
 
        public static Color BackgroundColor
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = value;
                _background_color_material.Diffuse = _background_color;
            }
        }

        public static Color ScrollColor
        {
            get
            {
                return _scroll_color;
            }
        }

        public static Color ScrollBackground
        {
            get
            {
                return _scroll_background;
            }
        }

        private Rectangle _client_area;
        private Rectangle _last_area;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel _main_panel;
        public Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelControl MainControl
        {
            get
            {
                return _main_panel.Controls[0];
            }
        }
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelControl _control;

        public Rectangle ClientArea
        {
            get
            {
                return _client_area;
            }
            set
            {
                _client_area = value;
                _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
            }
        }


        #region Private Members
        protected Microsoft.DirectX.Direct3D.Font _font;

        private static int _DEFAULT_CAPTION_HEIGHT_ = 28;

        private Rectangle _resize_btn;
        private Rectangle _shade_btn;
        private Rectangle _caption_bar;


        private RectangleF _v_scroll_rect;
        private RectangleF _v_scroll_thumb;
        private RectangleF _h_scroll_rect;
        private RectangleF _h_scroll_thumb;

        private bool _ShowVerticalScroll = false;
        private bool _ShowHorizontalScroll = false;

        #endregion

        public Window(string caption, Rectangle location, WindowState state)
        {
            AllowShade = true;
            HasScrollBars = true;
            State = state;
            CaptionText = caption;
            DefaultLocation = location;
            _last_area = _client_area = DefaultLocation;

            _main_panel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(location.Left, location.Top, location.Right, location.Bottom);
            _main_panel.AlwaysRender = true;

            _caption_bar = _client_area;
            _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

            _scroll_background_material.Diffuse = ScrollBackground;
            _scroll_color_material.Diffuse = ScrollColor;
            _border_color_material.Diffuse = BorderColor;
            _caption_color_material.Diffuse = CaptionColor;
            _caption_foreground_material.Diffuse = CaptionForeground;
            _background_color_material.Diffuse = BackgroundColor;


        }
        public Window(string caption, int left, int top, int right, int bottom, WindowState state)
        {
            AllowShade = true;
            HasScrollBars = true;

            State = state;
            CaptionText = caption;

            DefaultLocation = Rectangle.FromLTRB(left, top, right, bottom);
            _last_area =  _client_area = DefaultLocation;

            _caption_bar = DefaultLocation;
            _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

            _main_panel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(left, _caption_bar.Bottom, right, bottom);
            _main_panel.AlwaysRender = true;

            _scroll_background_material.Diffuse = ScrollBackground;
            _scroll_color_material.Diffuse = ScrollColor;
            _border_color_material.Diffuse = BorderColor;
            _caption_color_material.Diffuse = CaptionColor;
            _caption_foreground_material.Diffuse = CaptionForeground;
            _background_color_material.Diffuse = BackgroundColor;

        }

        public Controls.PanelControl BindPanelControl(Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelControl control)
        {
            _control = control;
            return _main_panel.BindPanelControl(control);
        }
        public void FitToTextureDimensions(Canvas c, float TextureWidth, float TextureHeight)
        {
            _main_panel.FitToTextureDimensions(c, TextureWidth, TextureHeight);
        }

        
        public void Maximize()
        {
            if (AllowResize)
            {
                State = WindowState.MAXIMIZED;
                _client_area = _last_area;
                _caption_bar = _client_area;
                _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

                _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
            }
        }
        public void Minimize()
        {
            if (AllowResize)
            {
                State = WindowState.MINIMIZED;
                _last_area = _client_area;
                _client_area = DefaultLocation;
                _caption_bar = _client_area;
                _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

                _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
            }
        }

        public void ToggleShade()
        {
            if (AllowShade)
            {
                switch (State)
                {
                    case WindowState.MAXIMIZED:
                        State = WindowState.MINIMIZED;
                        _last_area = _client_area;
                        _client_area = DefaultLocation;
                        _caption_bar = _client_area;
                        _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

                        _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
                        break;
                    case WindowState.MINIMIZED:
                        State = WindowState.MAXIMIZED;
                        _last_area = _client_area;
                        _client_area = DefaultLocation;
                        _client_area.Height = DefaultLocation.Height * 2;
                        _client_area.Y -= DefaultLocation.Height;

                        _caption_bar = _client_area;
                        _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

                        _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
                        break;
                }
            }
        }
        public void Shade()
        {
            if (AllowShade)
            {
                Minimize();
            }
        }
        public void Unshade()
        {
            if (AllowShade)
            {
                State = WindowState.MAXIMIZED;
                _last_area = _client_area;
                _client_area = DefaultLocation;
                _client_area.Height = DefaultLocation.Height * 2;
                _client_area.Y -= DefaultLocation.Height;

                _caption_bar = _client_area;
                _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

                _main_panel.ResizeAll(_client_area);
            }
        }

        public void Show()
        {
            State = WindowState.SHOW;
            Selected = true;
            //_main_panel.Hide = false;
        }
        public void Hide()
        {
            State = WindowState.HIDE;
            Selected = false;
            //_main_panel.Hide = true;
        }

        public void Move(int xpos, int ypos)
        {
            _client_area.X = xpos;
            _client_area.Y = ypos;
            _caption_bar = _client_area;
            _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;

            _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
        }
        public void Resize(int width, int height)
        {
            _client_area.Width = width;
            _client_area.Height = height + _DEFAULT_CAPTION_HEIGHT_;
            _caption_bar = _client_area;
            _caption_bar.Height = _DEFAULT_CAPTION_HEIGHT_;
            //_main_panel.ClientArea = Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom);        

            _main_panel.ResizeAll(Rectangle.FromLTRB(_client_area.Left, _caption_bar.Bottom, _client_area.Right, _client_area.Bottom));
        }

        public bool HitTest(Point pt)
        {
            _main_panel.Selected = _client_area.Contains(pt);
            return _main_panel.Selected;
        }

        private void DrawVerticalScroll(Canvas c)
        {
            float pct = (float)this._v_scroll_rect.Height / (float) _control.DocumentHeight;

            _v_scroll_thumb = _v_scroll_rect;
            _v_scroll_thumb.Width -= 3;
            _v_scroll_thumb.X += 2;
            if (pct < 1)
            {
                _v_scroll_thumb.Height = pct * _v_scroll_rect.Height;
                _v_scroll_thumb.Y += Math.Abs(_control.DocumentPositionY) * pct;
            }


            if (_v_scroll_thumb.Bottom <= _v_scroll_rect.Bottom)
            {
                CanVScroll = true;
            }
            else
            {
                CanVScroll = false;
                _v_scroll_thumb.Height = _v_scroll_rect.Bottom - _v_scroll_thumb.Top;
            }

            // Scrollbar background
            c.DrawFillRect(_v_scroll_rect, _scroll_background_material);

            // Scrollbar thumb
            c.DrawFillRect(_v_scroll_thumb, _scroll_color_material);
            c.DrawRect(_v_scroll_rect, BorderColor);

        }

        private void DrawHorizontalScroll(Canvas c)
        {
            float pct = (float)this._h_scroll_rect.Width / (float)_control.DocumentWidth;

            _h_scroll_thumb = _h_scroll_rect;
            _h_scroll_thumb.Height -= 3;
            _h_scroll_thumb.Y += 2;
            if (pct < 1)
            {
                _h_scroll_thumb.Width = pct * _h_scroll_rect.Width;
                _h_scroll_thumb.X += _control.DocumentPositionX * pct;
            }
            if (_h_scroll_thumb.Right < _h_scroll_rect.Right)
            {
                CanHScroll = true;
            }
            else
            {
                CanHScroll = false;
                _h_scroll_thumb.Width = _h_scroll_rect.Right - _h_scroll_thumb.Left;
            }

            // Scrollbar background
            c.DrawFillRect(_h_scroll_rect, _scroll_background_material);

            // Scrollbar thumb
            c.DrawFillRect(_h_scroll_thumb, _scroll_color_material);
            c.DrawRect(_h_scroll_rect, BorderColor);

        }



        private bool IsClientArea(Point p)
        {
            return _main_panel.ClientArea.Contains(p);
        }
        private bool IsShadeButton(Point p)
        {
            return _shade_btn.Contains(p);
        }
        private bool IsCaptionBar(Point p)
        {
            return _caption_bar.Contains(p);
        }
        private bool IsResizeButton(Point p)
        {
            return _resize_btn.Contains(p);
        }
        private bool IsVerticalScrollBar(Point p)
        {
            return _v_scroll_rect.Contains(p);
        }
        private bool IsHorizontalScrollBar(Point p)
        {
            return _h_scroll_rect.Contains(p);
        }

        #region IRenderable Members

        public void OnRender(Canvas c)
        {
            if (State != WindowState.HIDE)
            {

                _shade_btn = _caption_bar;
                _shade_btn.Width = 12;
                _shade_btn.Height = 12;
                _shade_btn.X = _caption_bar.Right - 28;
                _shade_btn.Y += 7;

                _v_scroll_rect.X = _client_area.Right - ScrollWidth;
                _v_scroll_rect.Y = _caption_bar.Bottom;
                _v_scroll_rect.Height = _client_area.Height - (ScrollWidth + _caption_bar.Height);
                _v_scroll_rect.Width = ScrollWidth - 1;

                _h_scroll_rect.X = _client_area.X;
                _h_scroll_rect.Y = _client_area.Bottom - ScrollWidth;
                _h_scroll_rect.Width = _client_area.Width - ScrollWidth;
                _h_scroll_rect.Height = ScrollWidth - 1;

                _v_scroll_thumb = _v_scroll_rect;
                _h_scroll_thumb = _h_scroll_rect;


                c.DrawFillRect(_main_panel.ClientArea, _background_color_material);

                _main_panel.OnRender(c);

                if (HasScrollBars)
                {
                    if (this._ShowHorizontalScroll)
                    {
                        DrawHorizontalScroll(c);
                    }
                    if (this._ShowVerticalScroll)
                    {
                        DrawVerticalScroll(c);
                    }
                }

                c.DrawRect(_client_area, BorderColor);


                if (CaptionText != string.Empty)
                {
                    c.DrawFillRect(_caption_bar, _caption_color_material);
                    if (Font != null)
                    {
                        Font.DrawText(null, CaptionText, _caption_bar, DrawTextFormat.VerticalCenter | DrawTextFormat.Center, CaptionForeground);
                    }

                    if (AllowShade)
                    {
                        switch (State)
                        {
                            case WindowState.MINIMIZED:
                                c.DrawFillTri(_shade_btn, _caption_foreground_material, DIRECTION.UP);
                                c.DrawTri(_shade_btn, BorderColor, DIRECTION.UP);
                                break;
                            case WindowState.MAXIMIZED:
                                c.DrawFillTri(_shade_btn, _caption_foreground_material, DIRECTION.DOWN);
                                c.DrawTri(_shade_btn, BorderColor, DIRECTION.DOWN);
                                break;
                        }
                    }
                    
                    if (Selected)
                    {
                        c.DrawRect(_caption_bar, Color.White);
                    }
                    else
                    {
                        c.DrawRect(_caption_bar, BorderColor);
                    }
                }


                if (AllowResize)
                {
                    _resize_btn.X = _client_area.Right - 15;
                    _resize_btn.Y = _client_area.Bottom - 15;
                    _resize_btn.Width = 10;
                    _resize_btn.Height = 10;

                    c.DrawFillRect(_resize_btn, _caption_color_material);
                    c.DrawRect(_resize_btn, BorderColor);
                }


            }
        }

        #endregion


        #region IDeviceInput Members

        public void OnMouseClick(object sender, MouseEventArgs e)
        {

            if (_main_panel != null)
            {
                if (IsShadeButton(e.Location) && AllowShade)
                {
                    ToggleShade();
                    return;
                }

                if (IsClientArea(e.Location))
                {
                    _main_panel.OnMouseClick(sender, e);
                }
            }
        }

        public void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((_main_panel != null))
            {
                if (IsCaptionBar(e.Location) && (e.Button == MouseButtons.Left))
                {
                    switch (State)
                    {
                        case WindowState.MINIMIZED:
                            Maximize();
                            break;
                        case WindowState.MAXIMIZED:
                            Minimize();
                            break;
                    }
                    return;
                }
                if (IsClientArea(e.Location))
                {
                    _main_panel.OnMouseDoubleClick(sender, e);
                }
            }
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if ((_main_panel != null) && IsClientArea(e.Location))
            {
                _main_panel.OnMouseDown(sender, e);
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_main_panel != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (IsCaptionBar(e.Location) && AllowMove)
                    {
                        Moving = true;
                    }
                    if (IsResizeButton(e.Location) && AllowResize)
                    {
                        Resizing = true;
                    }
                    if (Moving)
                    {
                        Move(e.X - (ClientArea.Width / 2), e.Y);
                        return;
                    }
                    if (Resizing)
                    {
                        Resize(e.X - ClientArea.X, e.Y - ClientArea.Y);
                        return;
                    }
                }

                if (IsVerticalScrollBar(e.Location))
                {
                    _ShowVerticalScroll = true;
                    return;
                }
                else
                {
                    _ShowVerticalScroll = false;
                }

                if (IsHorizontalScrollBar(e.Location))
                {
                    _ShowHorizontalScroll = true;
                    return;
                }
                else
                {
                    _ShowHorizontalScroll = false;
                }

                if (IsClientArea(e.Location))
                {
                    _main_panel.OnMouseMove(sender, e);
                }
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_main_panel != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Moving)
                    {
                        Moving = false;
                    }
                    if (Resizing)
                    {
                        Resizing = false;
                        if (ClientArea.Width < DefaultLocation.Width)
                        {
                            Minimize();
                        }
                        if (ClientArea.Height < DefaultLocation.Height)
                        {
                            Minimize();
                        }
                    }
                }
                if (IsClientArea(e.Location))
                {
                    _main_panel.OnMouseUp(sender, e);
                }
            }
        }

        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (_main_panel != null)
            {
                lock (this)
                {
                    if (e.Delta < 0)
                    {
                        if (_ShowVerticalScroll)
                        {
                            if (CanVScroll)
                            {
                                _control.DocumentPositionY += ScrollChange;
                            }

                        }
                        else if (_ShowHorizontalScroll)
                        {
                            if (CanHScroll)
                            {
                                _control.DocumentPositionX += ScrollChange;
                            }
                        }
                    }
                    else
                    {
                        if (_ShowVerticalScroll)
                        {
                            if (_control.DocumentPositionY > 0)
                            {
                                _control.DocumentPositionY -= ScrollChange;
                            }
                        }
                        else if (_ShowHorizontalScroll)
                        {
                            if (_control.DocumentPositionX > 0)
                            {
                                _control.DocumentPositionX -= ScrollChange;
                            }

                        }

                    }
                }

                if (IsClientArea(e.Location))
                {
                    _main_panel.OnMouseWheel(sender, e);
                }
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_main_panel != null)
            {
                _main_panel.OnKeyDown(sender, e);
            }
        }

        public void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_main_panel != null)
            {
                _main_panel.OnKeyPress(sender, e);
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (_main_panel != null)
            {
                _main_panel.OnKeyUp(sender, e);
            }
        }

        #endregion
    }
}
