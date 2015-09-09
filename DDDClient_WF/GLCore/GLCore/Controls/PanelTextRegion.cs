using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;



namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{

    public class PanelTextRegion : PanelViewportRegion
    {

        public int BufferSize = 0;
        public bool HasInput = false;
        public TextChatEventHandler OnTextChat;

        public static int Alpha = 127;
        private static int _transparency_80pct = 204;

        private static Color _border_color = Color.FromArgb(_transparency_80pct, 150, 150, 160);
        public static Color BorderColor
        {
            get
            {
                return _border_color;
            }
            set
            {
                _border_color = Color.FromArgb(255, value);
            }
        }

  
        private static Color _background_color = Color.FromArgb(Alpha, 63, 63, 63);
        public static Color BackgroundColor
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = Color.FromArgb(Alpha, value);
            }
        }

        private static Color _foreground_color = Color.FromArgb(207, 207, 141);
        public static Color ForegroundColor
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

  
        private Rectangle _text_area;
        private Rectangle _border_rect;

        private static int _DEFAULT_MAX_BUFFER_SIZE = 2000;

        private int _max_buffer_size;
        public int MaxBufferSize
        {
            get
            {
                return _max_buffer_size;
            }
            set
            {
                _max_buffer_size = value;
            }
        }



        private int _buffer_size;


        private float _viewport_offsetx = 0;
        private float _viewport_offsety = 0;
        private float _pixels_x = 0;
        private float _pixels_y = 0;

        private StringBuilder _input_buffer;
        
        private String _input_buffer_str;

        private Rectangle _input_buffer_rect;
        private bool _input_buffer_hasdata;

        private Stack<TextElement> _buffer;

        public override float DocumentPositionX
        {
            get
            {
                return _viewport_offsetx;
            }
            set
            {
                _viewport_offsetx = value;
            }
        }
        public override float DocumentPositionY
        {
            get
            {
                return _viewport_offsety;
            }
            set
            {
                _viewport_offsety = value;
            }
        }

        
        private Viewport _editbox_viewport;
        private Rectangle _editbox_rect;
        
        public override float DocumentWidth
        {
            get
            {
                return _pixels_x;
            }
        }
        public override float DocumentHeight
        {
            get
            {
                return _pixels_y;
            }
        }


        public PanelTextRegion()
        {
            _client_area = Rectangle.Empty;
            HasDocument = true;

            _buffer = new Stack<TextElement>();
            _font = null;
            _Sticky = true;
            _text_area = Rectangle.Empty;

            _max_buffer_size = _DEFAULT_MAX_BUFFER_SIZE;

                _editbox_viewport = new Viewport();
                _input_buffer = new StringBuilder();
                ClearInputBuffer();
            
        }


        public PanelTextRegion(Microsoft.DirectX.Direct3D.Font font, int x1, int y1, int x2, int y2)
        {
            _buffer = new Stack<TextElement>();
            HasDocument = true;

            _client_area.X = x1;
            _client_area.Y = y1;
            _client_area.Width = x2 - x1;
            _client_area.Height = y2 - y1;
            InitWindow();

            _max_buffer_size = _DEFAULT_MAX_BUFFER_SIZE;

            _font = font;

            _Sticky = true;

                _editbox_viewport = new Viewport();
                _input_buffer = new StringBuilder();

                ClearInputBuffer();
        }

        public void InitWindow()
        {

            if (HasInput)
            {
                _editbox_rect.X = _client_area.X + 1;
                _editbox_rect.Y = _client_area.Y ;
                _editbox_rect.Height = _input_buffer_rect.Height;
                _editbox_rect.Width = (int)(_client_area.Width - 1);

                _text_area.X = _client_area.X + 1;
                _text_area.Y = _editbox_rect.Y + _editbox_viewport.Height + 1;
                _text_area.Height = (int)(_client_area.Bottom - _editbox_rect.Bottom) - 2;

                _text_area.Width = (int)(_client_area.Width - 1);
            }
            else
            {
                _text_area = _client_area;
                _text_area.X ++;
                _text_area.Height -= 2;

                _text_area.Width -= 2;

            }


            _border_rect = _client_area;
            _border_rect.Width--;
            _border_rect.Height--;

        }




        public void AddText(Color color,  string text)
        {
            lock (this)
            {
                if (_buffer_size > _max_buffer_size)
                {
                    _buffer.Clear();
                    _buffer_size = 0;
                    _pixels_y = 0;
                }
                Rectangle dimensions = _font.MeasureString(null, text, DrawTextFormat.None, color);
                //_buffer.Enqueue(new TextElement(color, text, dimensions));
                _buffer.Push(new TextElement(color, text, dimensions));
                _buffer_size += text.Length;
                _pixels_y += dimensions.Height;
            }

        }
 
        public void AddText(string text)
        {
            AddText(ForegroundColor, text);
        }


        public void ClearTextWindow()
        {
            lock (this)
            {
                _buffer.Clear();
            }
        }




        protected void ClearInputBuffer()
        {
            lock (this)
            {
                if (_input_buffer.Length > 0)
                {
                    _input_buffer.Remove(0, _input_buffer.Length);
                }
                _input_buffer.Append(">_");
                _input_buffer_hasdata = false;
                _input_buffer_str = _input_buffer.ToString();
            }
        }

        protected void InsertInputBuffer(char c)
        {
            lock (this)
            {
                if (_input_buffer.Length == 1)
                {
                    _input_buffer.Insert(0, c);
                }
                else
                {
                    _input_buffer.Insert(_input_buffer.Length - 1, c);
                }
                _input_buffer_hasdata = true;
                _input_buffer_str = _input_buffer.ToString();
            }
        }

        protected void DeleteInputBuffer()
        {
            lock (this)
            {
                if (_input_buffer.Length > 3)
                {
                    _input_buffer.Remove(_input_buffer.Length - 2, 1);
                    _input_buffer_hasdata = true;
                }
                else
                {
                    _input_buffer_hasdata = false;
                }
                _input_buffer_str = _input_buffer.ToString();

            }
        }
        
        protected void CommitInputBuffer()
        {
            lock (this)
            {
                if (_input_buffer_hasdata)
                {
                    string text = _input_buffer.ToString(1, _input_buffer.Length - 2);
                    if (OnTextChat != null)
                    {
                        OnTextChat(null, new TextChatEventArgs(text));
                    }
                    ClearInputBuffer();
                }
            }
        }

        protected void DrawInputBuffer(Rectangle client_area)
        {
            lock (this)
            {
                if (_input_buffer.Length > 0)
                {
                    //string text = _input_buffer.ToString();
                    _input_buffer_rect = _font.MeasureString(null, _input_buffer_str, DrawTextFormat.None, ForegroundColor);
                    if (_input_buffer_rect.Width > client_area.Width)
                    {
                        _font.DrawText(null, _input_buffer_str, client_area.X - (_input_buffer_rect.Width - client_area.Width), client_area.Y, ForegroundColor);
                    }
                    else
                    {
                        _font.DrawText(null, _input_buffer_str, client_area.X, client_area.Y, ForegroundColor);
                    }
                }
            }
        }



        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (HasInput)
            {
                switch (e.KeyCode)
                {
                    case Keys.Return:
                        CommitInputBuffer();
                        ClearInputBuffer();
                        break;
                    case Keys.Back:
                        DeleteInputBuffer();
                        break;
                }
            }
        }
        
        public override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (HasInput)
            {
                if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsPunctuation(e.KeyChar) ||
                    Char.IsWhiteSpace(e.KeyChar))
                {
                    if (e.KeyChar != '\r')
                    {
                        InsertInputBuffer(e.KeyChar);
                    }
                }
            }
        }


        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {

                case MouseButtons.Left:
                    if (Moving) {
                        if (HasInput)
                        {
                            CommitInputBuffer();
                        }
                    }

                    break;

            }
        }



        private void DrawEditBox(Canvas c)
        {
            _editbox_viewport.X = _client_area.X;
            //_editbox_viewport.Y = _caption_area.Bottom + 1;
            _editbox_viewport.Y = _editbox_rect.Y;

            _editbox_viewport.Width = _text_area.Width;
            _editbox_viewport.Height = _input_buffer_rect.Height;

            Viewport v = c.Viewport;
            c.Viewport = _editbox_viewport;
            DrawInputBuffer(_editbox_rect);
            c.DrawLine(BorderColor, 1, _editbox_rect.X,  _editbox_rect.Bottom-1, _editbox_rect.Right, _editbox_rect.Bottom-1);
            c.Viewport = v;

        }

        private void DrawViewport(Canvas c)
        {
            if (HasInput)
            {
                DrawEditBox(c);
            }

            StartViewport(c, _text_area);
            _pixels_x = 1;
            int posy = _text_area.Y - (int)_viewport_offsety;

            lock (this)
            {
                foreach (TextElement t in _buffer)
                {
                    if (!t.Draw(_font, _text_area, (int)_viewport_offsetx, posy))
                    {
                        break;
                    }

                    posy += (int)t.Height;
                    if (_pixels_x < t.Width)
                    {
                        _pixels_x = t.Width;
                    }
                }
            }
            EndViewport(c);
        }





        public override void OnRender(Canvas c)
        {

            InitWindow();


            if (!Moving)
            {
                lock (this)
                {
                    DrawViewport(c);
                }

            }

        }
               
    }





    struct TextElement
    {
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
        }
        public Color Foreground;
        public float Height
        {
            get
            {
                return draw_rect.Height;
            }
        }
        public float Width
        {
            get
            {
                return draw_rect.Width;
            }
        }

        public int Length
        {
            get
            {
                return Text.Length;
            }
        }
        private Rectangle draw_rect;
        private Rectangle temp;
        

        public TextElement(Color foreground, string text, Rectangle measure)
        {
            _text = text;
            Foreground = foreground;
            draw_rect.Height =measure.Height;
            draw_rect = measure;
            temp = Rectangle.Empty;
        }

        public void Append(string text)
        {
            _text = _text + text;
        }

        public bool Draw(Microsoft.DirectX.Direct3D.Font font, Rectangle client_rect, int offsetx, int offsety)
        {

            temp.X = client_rect.X - offsetx;
            temp.Y = client_rect.Y - offsety;
            
            if (offsety < client_rect.Bottom)
            {
                font.DrawText(null, _text, client_rect.X - (int)offsetx, offsety, Foreground);
                return true;
            }
            return false;

        }
    }

    public delegate void TextChatEventHandler (object obj, TextChatEventArgs e);

    public class TextChatEventArgs : EventArgs
    {
        public string Message;
        public TextChatEventArgs(string message)
        {
            Message = message;
        }
    }
}
