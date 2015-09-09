using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public delegate void PanelMenuSelectHandler(int item_index, string item_str);

    public class PanelMenu : PanelViewportRegion
    {
        public string[] Items
        {
            get
            {
                return MenuOptions;
            }
        }
        public int ScrollButtonWidth = 15;
        public bool ScrollButtons = true;
        
        public DrawTextFormat DrawFormat = DrawTextFormat.VerticalCenter | DrawTextFormat.Center;

        private int StartX
        {
            get
            {
                return _start_x + _panel_area.X;
            }
        }
        private int StartY
        {
            get
            {
                return _start_y + _panel_area.Y;
            }
        }


        private int _menu_item_height;
        private Material _button_material = new Material();
        private Material _arrow_material = new Material();
        private Material _arrow_selected = new Material();

        private bool HideUpScrollBtn = true;
        private bool HideLeftScrollBtn = true;
        private bool HideRightScrollBtn = false;
        private bool HideDownScrollBtn = false;

        private PanelLayout _layout;
        private String[] MenuOptions;
        private Rectangle[] MenuLocations;
        private Microsoft.DirectX.Direct3D.Font _font;
        private PanelMenuSelectHandler _handler = null;
        private int _selected_index = -1;
        private int _start_x = 0;
        private int _start_y = 0;
        private Rectangle _up_button_area;
        private Rectangle _up_arrow;
        private Rectangle _down_button_area;
        private Rectangle _down_arrow;
        private Rectangle _left_button_area;
        private Rectangle _left_arrow;
        private Rectangle _right_button_area;
        private Rectangle _right_arrow;
        private Rectangle _panel_area;

        private Color _border_color = Color.FromArgb(150, 150, 160);
        public Color BorderColor
        {
            get
            {
                return _border_color;
            }
            set
            {
                _border_color = value;
            }
        }
        private Material _background_color_material = new Material();
        private Color _background_color = Color.FromArgb(122, 123, 143);
        public Color BackgroundColor
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = value;
                _background_color_material.Diffuse = value;
            }
        }
        private Color _foreground_color = Color.FromArgb(221, 222, 200);
        public Color ForegroundColor
        {
            get
            {
                return _foreground_color;
            }
            set
            {
                _foreground_color = value;
            }
        }

        private enum BoundingBox : int { NONE = 0, UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4 };
        private BoundingBox _draw_bounding_box = BoundingBox.NONE;


        public PanelMenuSelectHandler Handler
        {
            set
            {
                _handler = value;
            }
        }

        public PanelMenu(Microsoft.DirectX.Direct3D.Font font, PanelMenuSelectHandler handler)
        {
            _client_area = Rectangle.Empty;
            _Sticky = false;
            _font = font;
            _handler = handler;
            _layout = PanelLayout.Horizontal;
        }
        

        public PanelMenu(Microsoft.DirectX.Direct3D.Font font, string text, int x1, int y1, int x2, int y2, PanelMenuSelectHandler handler)
        {
            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;

            _font = font;
            _handler = handler; 
            _layout = PanelLayout.Horizontal;

            _Sticky = false;
        }

        public void ClearSelection()
        {
            _selected_index = -1;
        }
        public void Reset()
        {
            _selected_index = -1;
            _start_x = _start_y = 0;
            if ((_layout == PanelLayout.VerticalFree) || (_layout == PanelLayout.Vertical))
            {
                HideDownScrollBtn = false;
                HideUpScrollBtn = true;
            }
            if ((_layout == PanelLayout.HorizontalFree) || (_layout == PanelLayout.Horizontal))
            {
                HideLeftScrollBtn = true;
                HideRightScrollBtn = false;
            }
        }
        public void ResetToInitialPosition()
        {
            Console.WriteLine("Current Position {0}, {1} to {2}, {3}", _start_x, _start_y, PanelViewport.X, PanelViewport.Y);
            _start_x = this.PanelViewport.X;
            _start_y = this.PanelViewport.Y;
        }

        public void ScrollDown(int pixels)
        {
            // Get the Viewports screen position and subtract the full menu height minus the height of the last menu item.
            //  (We don't want to scroll past the last menu item, rather keep it visible.
            if (StartY > (PanelViewport.Y - (_menu_item_height - MenuLocations[MenuLocations.Length - 1].Height)))
            {
                HideUpScrollBtn = false;
                _start_y -= pixels;
            }
            if (!(StartY > (PanelViewport.Y - (_menu_item_height - MenuLocations[MenuLocations.Length - 1].Height))))
            {
                HideDownScrollBtn = true;
                HideUpScrollBtn = false;
            }
        }
        public void ScrollUp(int pixels)
        {
            if (StartY < (PanelViewport.Y))
            {
                HideDownScrollBtn = false;
                _start_y += pixels;
            }
            if (!(StartY < PanelViewport.Y))
            {
                HideUpScrollBtn = true;
                HideDownScrollBtn = false;
            }
        }


        public void ScrollLeft(int pixels)
        {
            if (StartX < (PanelViewport.X))
            {
                HideRightScrollBtn = false;
                _start_x += pixels;
            }
            if (!(StartX < PanelViewport.X))
            {
                HideLeftScrollBtn = true;
                HideRightScrollBtn = false;
            }
        }
        public void ScrollRight(int pixels)
        {
            if (StartX > (PanelViewport.X - (_menu_item_height - MenuLocations[MenuLocations.Length - 1].Width)))
            {
                HideLeftScrollBtn = false;
                _start_x -= pixels;
            }
            if (!(StartX > (PanelViewport.X - (_menu_item_height - MenuLocations[MenuLocations.Length - 1].Width))))
            {
                HideRightScrollBtn = true;
                HideLeftScrollBtn = false;
            }
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_up_button_area.Contains(e.Location))
            {
                _draw_bounding_box = BoundingBox.UP;
                return;
            }
            if (_down_button_area.Contains(e.Location))
            {
                _draw_bounding_box = BoundingBox.DOWN;
                return;
            }
            if (_left_button_area.Contains(e.Location))
            {
                _draw_bounding_box = BoundingBox.LEFT;
                return;
            }
            if (_right_button_area.Contains(e.Location))
            {
                _draw_bounding_box = BoundingBox.RIGHT;
                return;
            }
            _draw_bounding_box = BoundingBox.NONE;

        }
        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (MenuLocations == null)
            {
                return;
            }
            if (_up_button_area.Contains(e.Location))
            {
                if (MenuLocations.Length > 0)
                {
                    ScrollUp(MenuLocations[0].Height);
                }
                _draw_bounding_box = BoundingBox.NONE;
                return;
            }
            if (_down_button_area.Contains(e.Location))
            {
                if (MenuLocations.Length > 0)
                {
                    ScrollDown(MenuLocations[0].Height);
                }
                _draw_bounding_box = BoundingBox.NONE;
                return;
            }
            if (_left_button_area.Contains(e.Location))
            {
                if (MenuLocations.Length > 0)
                {
                    ScrollLeft(MenuLocations[0].Width);
                }
                _draw_bounding_box = BoundingBox.NONE;
                return;
            }
            if (_right_button_area.Contains(e.Location))
            {
                if (MenuLocations.Length > 0)
                {
                    ScrollRight(MenuLocations[0].Width);
                }
                _draw_bounding_box = BoundingBox.NONE;
                return;
            }
            _draw_bounding_box = BoundingBox.NONE;

            if (_handler != null)
            {

                for (int i = 0; i < MenuLocations.Length; i++)
                {
                    if (MenuLocations[i].Contains(e.Location))
                    {
                        _selected_index = i;
                        _handler(i, MenuOptions[i]);
                    }
                }
            }

        }

        protected override void ClientAreaChanged()
        {
            LayoutButtons(_layout);
        }

        public void LayoutButtons(PanelLayout type)
        {
            _button_material.Diffuse = Window.CaptionColor;
            _arrow_material.Diffuse = Window.CaptionForeground;
            _arrow_selected.Diffuse = Color.Yellow;

            int arrow_size = ScrollButtonWidth / 2;


            switch (type)
            {
                case PanelLayout.Vertical:
                    goto case PanelLayout.VerticalFree;
                case PanelLayout.VerticalFree:
                    if (_panel_area.Height < _menu_item_height)
                    {
                        _panel_area.X += 1;
                        _panel_area.Width -= 2;
                        _panel_area.Height -= ScrollButtonWidth * 2;
                        _panel_area.Y += ScrollButtonWidth;
                        _up_button_area = new Rectangle(_client_area.X, _client_area.Y, _panel_area.Width, ScrollButtonWidth);
                        _up_arrow = new Rectangle(_client_area.X + (int)(.5 * (_client_area.Width - arrow_size)), _client_area.Y + (int)(.5 * arrow_size), arrow_size, arrow_size);
                        _down_button_area = new Rectangle(_client_area.X, _panel_area.Bottom, _panel_area.Width, ScrollButtonWidth);
                        _down_arrow = new Rectangle(_up_arrow.X, _panel_area.Bottom + (int)(.5 * arrow_size), arrow_size, arrow_size);
                    }
                    break;
                case PanelLayout.Horizontal:
                    goto case PanelLayout.HorizontalFree;
                case PanelLayout.HorizontalFree:
                    if (_panel_area.Width < _menu_item_height)
                    {
                        _panel_area.Width -= (ScrollButtonWidth * 2) + 2;
                        _panel_area.X += ScrollButtonWidth + 1;
                        _left_button_area = new Rectangle(_client_area.X, _client_area.Y, ScrollButtonWidth, _client_area.Height);
                        _left_arrow = new Rectangle(_client_area.X + (int)(.5 * arrow_size), _client_area.Y + (int)(.5 * (_client_area.Height - arrow_size)), arrow_size, arrow_size);
                        _right_button_area = new Rectangle(_panel_area.Right, _client_area.Y, ScrollButtonWidth, _client_area.Height);
                        _right_arrow = new Rectangle(_panel_area.Right + (int)(.5 * arrow_size), _left_arrow.Y, arrow_size, arrow_size);
                    }
                    break;
                default:
                    throw new ArgumentException("LayoutMenuOptions: Unsupported Layout type.");

            }

        }

        public int LayoutMenuOptions(string[] options, PanelLayout type)
        {
            _panel_area = _client_area;

            lock (this)
            {
                MenuOptions = options;

                if (MenuOptions != null)
                {
                    // Step1; if no MenuLocations or MenuLocations Length not the same as MenuOptions Length
                    //   then create/recreate MenuLocations with the right size.
                    if (MenuLocations == null)
                    {
                        MenuLocations = new Rectangle[MenuOptions.Length];
                    }
                    else
                        if (MenuLocations.Length != MenuOptions.Length)
                        {
                            MenuLocations = new Rectangle[MenuOptions.Length];
                        }

                    //Step2; Initialize all MenuLocations to the empty rectangle.
                    //Step3; Set the layout type and return the total height of the MenuOptions, (useful for resizing)
                    _layout = type;
                    _menu_item_height = 0;
                    for (int i = 0; i < MenuOptions.Length; i++)
                    {
                        MenuLocations[i] = _font.MeasureString(null, MenuOptions[i], DrawTextFormat.None, Color.Yellow);

                        switch (type)
                        {
                            case PanelLayout.Vertical:
                                goto case PanelLayout.VerticalFree;
                            case PanelLayout.VerticalFree:
                                MenuLocations[i].Height = (int)(MenuLocations[i].Height * 1.5);
                                _menu_item_height += MenuLocations[i].Height;
                                break;
                            case PanelLayout.Horizontal:
                                goto case PanelLayout.HorizontalFree;
                            case PanelLayout.HorizontalFree:
                                MenuLocations[i].Width = (int)(MenuLocations[i].Width * 1.5);
                                MenuLocations[i].Height = _panel_area.Height;
                                _menu_item_height += MenuLocations[i].Width;
                                break;
                            default:
                                throw new ArgumentException("LayoutMenuOptions: Unsupported Layout type.");

                        }
                    }

                    LayoutButtons(type);
                    return _menu_item_height;
                }
            }
            return 0;
            
        }

        public override void OnRender(Canvas c)
        {
                SetViewportDimensions(_panel_area);
                StartViewport(c);
            lock(this) {
                if (MenuOptions != null)
                {
                    c.DrawFillRect(_client_area, _background_color_material);
                    if (MenuOptions.Length > 0)
                    {
                        int height = 0;
                        int width = 0;

                        if ((_layout == PanelLayout.Vertical) || (_layout == PanelLayout.Horizontal))
                        {
                            height = _client_area.Height / MenuOptions.Length;
                            width = _client_area.Width / MenuOptions.Length;
                        }

                        for (int i = 0; i < MenuOptions.Length; i++)
                        {
                            switch (_layout)
                            {
                                case PanelLayout.Vertical:
                                    MenuLocations[i].X = StartX;
                                    MenuLocations[i].Y = StartY + (i * height);
                                    MenuLocations[i].Width = _client_area.Width;
                                    MenuLocations[i].Height = height - 2;
                                    break;
                                case PanelLayout.Horizontal:
                                    MenuLocations[i].X = StartX + (i * width);
                                    MenuLocations[i].Y = StartY;
                                    MenuLocations[i].Width = width - 2;
                                    MenuLocations[i].Height = _client_area.Height;
                                    break;
                                case PanelLayout.HorizontalFree:
                                    MenuLocations[i].X = StartX + width;
                                    MenuLocations[i].Y = StartY;
                                    MenuLocations[i].Height = _client_area.Height;
                                    width += MenuLocations[i].Width;
                                    break;
                                case PanelLayout.VerticalFree:
                                    MenuLocations[i].X = StartX;
                                    MenuLocations[i].Y = StartY + height;
                                    MenuLocations[i].Width = _client_area.Width;
                                    height += MenuLocations[i].Height;
                                    break;
                                default:
                                    throw new ArgumentException("Unsupported Layout type.");
                            }

                            if (i != _selected_index)
                            {
                                _font.DrawText(null, MenuOptions[i], MenuLocations[i], DrawFormat, ForegroundColor);
                            }
                            else
                            {
                                _font.DrawText(null, MenuOptions[i], MenuLocations[i], DrawFormat, Color.Yellow);
                            }
                        }
                    }
                }
                    EndViewport(c);

                    switch (_layout)
                    {
                        case PanelLayout.Horizontal:
                            goto case PanelLayout.HorizontalFree;
                        case PanelLayout.HorizontalFree:
                            if (_panel_area.Width < _menu_item_height)
                            {
                                    c.DrawFillRect(_left_button_area, _button_material);
                                if (!HideLeftScrollBtn)
                                {
                                    if ((_draw_bounding_box == BoundingBox.LEFT))
                                    {
                                        c.DrawFillTri(_left_arrow, _arrow_selected, DIRECTION.LEFT);
                                    }
                                    else
                                    {
                                        c.DrawFillTri(_left_arrow, _arrow_material, DIRECTION.LEFT);
                                    }
                                }
                                    c.DrawFillRect(_right_button_area, _button_material);
                                if (!HideRightScrollBtn)
                                {
                                    if ((_draw_bounding_box == BoundingBox.RIGHT))
                                    {
                                        c.DrawFillTri(_right_arrow, _arrow_selected, DIRECTION.RIGHT);
                                    }
                                    else
                                    {
                                        c.DrawFillTri(_right_arrow, _arrow_material, DIRECTION.RIGHT);
                                    }
                                }
                            }
                            break;
                        case PanelLayout.Vertical:
                            goto case PanelLayout.VerticalFree;
                        case PanelLayout.VerticalFree:
                            if (_panel_area.Height < _menu_item_height)
                            {
                                    c.DrawFillRect(_up_button_area, _button_material);
                                if (!HideUpScrollBtn)
                                {
                                    if ((_draw_bounding_box == BoundingBox.UP))
                                    {
                                        c.DrawFillTri(_up_arrow, _arrow_selected, DIRECTION.UP);
                                    }
                                    else
                                    {
                                        c.DrawFillTri(_up_arrow, _arrow_material, DIRECTION.UP);
                                    }
                                }
                                    c.DrawFillRect(_down_button_area, _button_material);
                                if (!HideDownScrollBtn)
                                {
                                    if ((_draw_bounding_box == BoundingBox.DOWN))
                                    {
                                        c.DrawFillTri(_down_arrow, _arrow_selected, DIRECTION.DOWN);
                                    }
                                    else
                                    {
                                        c.DrawFillTri(_down_arrow, _arrow_material, DIRECTION.DOWN);
                                    }
                                }
                            }
                            break;
                        default:
                            throw new ArgumentException("PanelMenuRender: Unsupported Layout type.");

                    }
                    c.DrawRect(_client_area, BorderColor);
                }

            }
        

    }
}
