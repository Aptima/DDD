using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.PathController;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public class MappableObject
    {
        public string ID;
        public bool DrawWithRotation = true;
        public bool DrawBoundingBox;
        public bool DrawObjectName;
        public bool CanSelect = true;

        protected Material _text_color_material;
        private Color _text_color;
        public Color TextColor
        {
            get
            {
                return _text_color;
            }
            set
            {
                _text_color = value;
                _text_color_material = new Material();
                _text_color_material.Diffuse = value;
            }
        }

        protected Material _diffuse_color_material;
        private Color _diffuse_color;
        public Color DiffuseColor
        {
            get
            {
                return _diffuse_color;
            }
            set
            {
                _diffuse_color = value;
                _diffuse_color_material = new Material();
                _diffuse_color_material.Diffuse = value;
            }
        }

        protected Material _heading_color_material;
        private Color _heading_color;
        public Color HeadingColor
        {
            get
            {
                return _heading_color;
            }
            set
            {
                _heading_color = value;
                _heading_color_material = new Material();
                _heading_color_material.Diffuse = value;
            }
        }

        protected Material _textbox_color_material;
        private Color _textbox_color;
        public Color TextBoxColor
        {
            get
            {
                return _textbox_color;
            }
            set
            {
                _textbox_color = value;
                _textbox_color_material = new Material();
                _textbox_color_material.Diffuse = value;
            }
        }
        public bool Hide = false;

        #region Range Ring Info

        protected Obj_Sprite _sensorRing = null;
        protected Bitmap _sensorBitmap = null;
        protected Color _sensorColor = Color.FromArgb(51, 0, 255, 0);
        protected SolidBrush _sensorBrush = null;
        protected Graphics _sensorGraphic = null;
        protected float _sensorRadius = 0.0F;
        protected float _widthPixels = 0.0F;
        protected float _heightPixels = 0.0F;
        protected Texture _sensorTexture;

        protected Obj_Sprite _capabilityRing = null;
        protected Bitmap _capabilityBitmap = null;
        protected Color _capabilityColor = Color.FromArgb(51, 255, 0, 0);
        protected SolidBrush _capabilityBrush = null;
        protected Graphics _capabilityGraphic = null;
        protected float _capabilityRadius = 0.0F;
        protected float _capabilityWidthPixels = 0.0F;
        protected float _capabilityHeightPixels = 0.0F;
        protected Texture _capabilityTexture;

        protected Obj_Sprite _vulnerabilityRing = null;
        protected Bitmap _vulnerabilityBitmap = null;
        protected Color _vulnerabilityColor = Color.FromArgb(51, 0, 0, 255);
        protected SolidBrush _vulnerabilityBrush = null;
        protected Graphics _vulnerabilityGraphic = null;
        protected float _vulnerabilityRadius = 0.0F;
        protected float _vulnerabilityWidthPixels = 0.0F;
        protected float _vulnerabilityHeightPixels = 0.0F;
        protected Texture _vulnerabilityTexture;

        private const float DefaultDisplayRadius = 200f;

        public float GetRatioScale(float actualDisplayPx, float originalScale)
        {
            return actualDisplayPx / DefaultDisplayRadius * originalScale;
        }

    #region SENSORS
        public bool IsSensorRingSet()
        {
            return (_sensorRing != null);
        }

        public void SetSensorRingValues(float sensorRadius, Color ringColor, Canvas canvas, float h_metersPerPixel, float v_metersPerPixel)
        {
            float height, width;
            int t_height, t_width;
            try
            {
                TransformMetersToPixels(sensorRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _widthPixels = width;
                _heightPixels = height;
                _sensorRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _sensorRadius = sensorRadius;

                _sensorBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _sensorColor = ringColor; //Color.FromArgb(33, 0, 255, 0);
                _sensorBrush = new SolidBrush(_sensorColor);
                _sensorGraphic = Graphics.FromImage(_sensorBitmap);
                _sensorRing.Initialize(canvas);
                _sensorGraphic.FillEllipse(_sensorBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _sensorTexture = canvas.CreateTexture(_sensorBitmap, out t_width, out t_height);
                _sensorRing.Texture(_sensorTexture, t_width, t_height);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Setting Sensor Ring Value: {0};\r\n{1}", e.Message, e.StackTrace);
            }
        }

        public void ClearSensorRingValues()
        {
            _sensorRing = null;
            _sensorBitmap = null;
            _sensorColor = Color.FromArgb(51, 0, 255, 0);
            _sensorBrush = null;
            _sensorGraphic = null;
            _sensorRadius = 0.0F;
            _widthPixels = 0.0F;
            _heightPixels = 0.0F;
            _sensorTexture = null;
        }

        //if color changes, or new ring radius is selected
        public void ModifySensorRing(float sensorRadius, Color sensorColor, float h_metersPerPixel, float v_metersPerPixel, Canvas canvas)
        {
            bool hasChanged = false;
            if (_sensorRadius != sensorRadius)
            {
                _sensorRadius = sensorRadius;
                hasChanged = true;
            }
            if (_sensorColor.Name != sensorColor.Name)
            {
                _sensorColor = sensorColor;
                hasChanged = true;
            }

            if (hasChanged)
            {
                float height, width;
                int t_height, t_width;
                TransformMetersToPixels(sensorRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _widthPixels = width;
                _heightPixels = height;
                _sensorRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _sensorRadius = sensorRadius;

                _sensorBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _sensorColor = sensorColor;
                _sensorBrush = new SolidBrush(_sensorColor);
                _sensorGraphic = Graphics.FromImage(_sensorBitmap);
                _sensorRing.Initialize(canvas);
                _sensorGraphic.FillEllipse(_sensorBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _sensorTexture = canvas.CreateTexture(_sensorBitmap, out t_width, out t_height);
                _sensorRing.Texture(_sensorTexture, t_width, t_height);
                
            }

        }

        public void DrawSensorRangeRing(float xpos, float ypos, float zpos, Canvas canvas)
        {
            float height = _sensorRing.TextureHeight;
            float width = _sensorRing.TextureWidth;
            float modifiedScale = GetRatioScale(_widthPixels, Scale);

            _sensorRing.SetScale(modifiedScale, modifiedScale, 0);

            xpos -= ((width / 2) * modifiedScale);
            ypos -= (((height / 2)) * modifiedScale) - (45 * Scale);

            _sensorRing.DrawIcon(canvas, xpos, ypos);

        }
    #endregion SENSORS

    #region CAPABILITIES
        public bool IsCapabilityRingSet()
        {
            return (_capabilityRing != null);
        }

        public void SetCapabilityRingValues(float capabilityRadius, Color ringColor, Canvas canvas, float h_metersPerPixel, float v_metersPerPixel)
        {
            float height, width;
            int t_height, t_width;
            try
            {
                TransformMetersToPixels(capabilityRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _capabilityWidthPixels = width;
                _capabilityHeightPixels = height;
                _capabilityRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _capabilityRadius = capabilityRadius;

                _capabilityBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _capabilityColor = ringColor; //Color.FromArgb(33, 0, 255, 0);
                _capabilityBrush = new SolidBrush(_capabilityColor);
                _capabilityGraphic = Graphics.FromImage(_capabilityBitmap);
                _capabilityRing.Initialize(canvas);
                _capabilityGraphic.FillEllipse(_capabilityBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _capabilityTexture = canvas.CreateTexture(_capabilityBitmap, out t_width, out t_height);
                _capabilityRing.Texture(_capabilityTexture, t_width, t_height);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Setting Capability Ring Value: {0};\r\n{1}", e.Message, e.StackTrace);
            }
        }

        public void ClearCapabilityRingValues()
        {
            _capabilityRing = null;
            _capabilityBitmap = null;
            _capabilityColor = Color.FromArgb(51, 0, 255, 0);
            _capabilityBrush = null;
            _capabilityGraphic = null;
            _capabilityRadius = 0.0F;
            _capabilityWidthPixels = 0.0F;
            _capabilityHeightPixels = 0.0F;
            _capabilityTexture = null;
        }

        //if color changes, or new ring radius is selected
        public void ModifyCapabilityRing(float capabilityRadius, Color capabilityColor, float h_metersPerPixel, float v_metersPerPixel, Canvas canvas)
        {
            bool hasChanged = false;
            if (_capabilityRadius != capabilityRadius)
            {
                _capabilityRadius = capabilityRadius;
                hasChanged = true;
            }
            if (_capabilityColor.Name != capabilityColor.Name)
            {
                _capabilityColor = capabilityColor;
                hasChanged = true;
            }

            if (hasChanged)
            {
                float height, width;
                int t_height, t_width;
                TransformMetersToPixels(capabilityRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _capabilityWidthPixels = width;
                _capabilityHeightPixels = height;
                _capabilityRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _capabilityRadius = capabilityRadius;

                _capabilityBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _capabilityColor = capabilityColor;
                _capabilityBrush = new SolidBrush(_capabilityColor);
                _capabilityGraphic = Graphics.FromImage(_capabilityBitmap);
                _capabilityRing.Initialize(canvas);
                _capabilityGraphic.FillEllipse(_capabilityBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _capabilityTexture = canvas.CreateTexture(_capabilityBitmap, out t_width, out t_height);
                _capabilityRing.Texture(_capabilityTexture, t_width, t_height);

            }

        }

        public void DrawCapabilityRangeRing(float xpos, float ypos, float zpos, Canvas canvas)
        {
            float height = _capabilityRing.TextureHeight;
            float width = _capabilityRing.TextureWidth;
            float modifiedScale = GetRatioScale(_capabilityWidthPixels, Scale);

            _capabilityRing.SetScale(modifiedScale, modifiedScale, 0);

            xpos -= ((width / 2) * modifiedScale);
            ypos -= (((height / 2)) * modifiedScale) - (45 * Scale);

            _capabilityRing.DrawIcon(canvas, xpos, ypos);

        }
        #endregion CAPABILITIES

        #region VULNERABILITIES
        public bool IsVulnerabilityRingSet()
        {
            return (_vulnerabilityRing != null);
        }

        public void SetVulnerabilityRingValues(float vulnerabilityRadius, Color ringColor, Canvas canvas, float h_metersPerPixel, float v_metersPerPixel)
        {
            float height, width;
            int t_height, t_width;
            try
            {
                TransformMetersToPixels(vulnerabilityRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _vulnerabilityWidthPixels = width;
                _vulnerabilityHeightPixels = height;
                _vulnerabilityRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _vulnerabilityRadius = vulnerabilityRadius;

                _vulnerabilityBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _vulnerabilityColor = ringColor; //Color.FromArgb(33, 0, 255, 0);
                _vulnerabilityBrush = new SolidBrush(_vulnerabilityColor);
                _vulnerabilityGraphic = Graphics.FromImage(_vulnerabilityBitmap);
                _vulnerabilityRing.Initialize(canvas);
                _vulnerabilityGraphic.FillEllipse(_vulnerabilityBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _vulnerabilityTexture = canvas.CreateTexture(_vulnerabilityBitmap, out t_width, out t_height);
                _vulnerabilityRing.Texture(_vulnerabilityTexture, t_width, t_height);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Setting Vulnerability Ring Value: {0};\r\n{1}", e.Message, e.StackTrace);
            }
        }

        public void ClearVulnerabilityRingValues()
        {
            _vulnerabilityRing = null;
            _vulnerabilityBitmap = null;
            _vulnerabilityColor = Color.FromArgb(51, 0, 255, 0);
            _vulnerabilityBrush = null;
            _vulnerabilityGraphic = null;
            _vulnerabilityRadius = 0.0F;
            _vulnerabilityWidthPixels = 0.0F;
            _vulnerabilityHeightPixels = 0.0F;
            _vulnerabilityTexture = null;
        }

        //if color changes, or new ring radius is selected
        public void ModifyVulnerabilityRing(float vulnerabilityRadius, Color vulnerabilityColor, float h_metersPerPixel, float v_metersPerPixel, Canvas canvas)
        {
            bool hasChanged = false;
            if (_vulnerabilityRadius != vulnerabilityRadius)
            {
                _vulnerabilityRadius = vulnerabilityRadius;
                hasChanged = true;
            }
            if (_vulnerabilityColor.Name != vulnerabilityColor.Name)
            {
                _vulnerabilityColor = vulnerabilityColor;
                hasChanged = true;
            }

            if (hasChanged)
            {
                float height, width;
                int t_height, t_width;
                TransformMetersToPixels(vulnerabilityRadius, h_metersPerPixel, v_metersPerPixel, out width, out height);
                _vulnerabilityWidthPixels = width;
                _vulnerabilityHeightPixels = height;
                _vulnerabilityRing = new Obj_Sprite(SpriteFlags.AlphaBlend); //allows transparency
                _vulnerabilityRadius = vulnerabilityRadius;

                _vulnerabilityBitmap = new Bitmap(Convert.ToInt32(DefaultDisplayRadius), Convert.ToInt32(DefaultDisplayRadius));
                _vulnerabilityColor = vulnerabilityColor;
                _vulnerabilityBrush = new SolidBrush(_vulnerabilityColor);
                _vulnerabilityGraphic = Graphics.FromImage(_vulnerabilityBitmap);
                _vulnerabilityRing.Initialize(canvas);
                _vulnerabilityGraphic.FillEllipse(_vulnerabilityBrush, 0, 0, DefaultDisplayRadius, DefaultDisplayRadius); //just fill ellipse once, or else you get a solid color.
                _vulnerabilityTexture = canvas.CreateTexture(_vulnerabilityBitmap, out t_width, out t_height);
                _vulnerabilityRing.Texture(_vulnerabilityTexture, t_width, t_height);

            }

        }

        public void DrawVulnerabilityRangeRing(float xpos, float ypos, float zpos, Canvas canvas)
        {
            float height = _vulnerabilityRing.TextureHeight;
            float width = _vulnerabilityRing.TextureWidth;
            float modifiedScale = GetRatioScale(_vulnerabilityWidthPixels, Scale);

            _vulnerabilityRing.SetScale(modifiedScale, modifiedScale, 0);

            xpos -= ((width / 2) * modifiedScale);
            ypos -= (((height / 2)) * modifiedScale) - (45 * Scale);

            _vulnerabilityRing.DrawIcon(canvas, xpos, ypos);

        }
        #endregion VULNERABILITIES

        /// <summary>
        /// Width and Height can be at most 4096 each, due to Texture limitations
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ScaleDownWidthHeight(ref float width, ref float height)
        {
            //AD: Great for showing an "estimated" ring, should display message to user.
            if (width > height)
            {
                if (width > 4096)
                {
                    double ratio = width / 4096;
                    width = (float)Math.Floor(width / ratio);
                    height = (float)Math.Floor(height / ratio);
                    Console.WriteLine("Width was above 4096 pixels, size reduced to {0} x {1}", width, height);
                }
            }
            else
            {
                if (height > 4096)
                {
                    double ratio = height / 4096;
                    width = (float)Math.Floor(width / ratio);
                    height = (float)Math.Floor(height / ratio);
                    Console.WriteLine("Width was above 4096 pixels, size reduced to {0} x {1}", width, height);
                }
            }
        }

        public void TransformMetersToPixels(float radius, float h_metersPerPixel, float v_metersPerPixel, out float width, out float height)
        { 
            //radius is initially in meters.  
            width = radius;
            height = radius;

            //change to pixels
            width /= h_metersPerPixel;
            height /= v_metersPerPixel;

            //mult by 2 for diameters.
            width *= 2;
            height *= 2;

            ScaleDownWidthHeight(ref width, ref height);
        }

        #endregion


        public RectangleF SpriteAreaF
        {
            get
            {
                return _sprite_area;
            }
        }
        public Rectangle SpriteArea
        {
            get
            {
                return Rectangle.Truncate(SpriteAreaF);
            }
        }

        public Vector3 Position
        {
            get
            {
                //return ToScreenCoordinates(_position.X, _position.Y);
                return _position;

            }
        }

        public Vector2 ScreenCoordinates
        {
            get
            {
                return ToScreenCoordinates(_position.X, _position.Y);
            }
        }

        
        public static float Scale = 1;
        public static float MinScale = .5f;

        public Vector3 Destination
        {
            get
            {
                //return ToScreenCoordinates(_destination.X, _destination.Y);
                return _destination;
            }
        }

        public string TTDStr
        {
            get
            {
                if (this._path_calculator != null)
                {
                    return _path_calculator.TTDStr;
                }
                return string.Empty;
            }
        }
        public string FullTTDStr
        {
            get
            {
                if (this._path_calculator != null)
                {
                    return _path_calculator.FullTTDStr;
                }
                return string.Empty;
            }
        }
        #region Private Members
        private Vector3 _position;
        private Vector3 _destination;


        private Obj_Sprite _sprite;
        private Path_Linear _path_calculator;
        private RectangleF _sprite_area;
        private RectangleF _destination_area;
        private RectangleF _overlay_rect;
        private Rectangle _text_rect;
       
        private Vector2 _converted_coordinates;

        
        private static Vector2 _map_position = Vector2.Empty;

        #endregion


        public MappableObject()
        {
            ID = "Unknown";
            DiffuseColor = Color.White;

            DrawBoundingBox = false;
            DrawObjectName = false;
            TextBoxColor = Color.Black;
            HeadingColor = Color.DodgerBlue;

            TextColor = Color.Yellow;

            _path_calculator = new Path_Linear();
            _sprite = null;
            _sprite_area = RectangleF.Empty;
            _destination_area = RectangleF.Empty;
            _overlay_rect = Rectangle.Empty;
            _position = Vector3.Empty;
            _destination = Vector3.Empty;
            _converted_coordinates = Vector2.Empty;

        }



        public void SetPosition(float xpos, float ypos, float zpos)
        {
            _position.X = xpos;
            _position.Y = ypos;
            _position.Z = zpos;
            PositionUpdate();
        }
        protected void InitSprite(Obj_Sprite sprite)
        {
            _sprite = sprite;
            _sprite_area = sprite.ToRectangleF();
        }

        public static void MapPosition(float xpos, float ypos)
        {
            _map_position.X = xpos;
            _map_position.Y = ypos;
        }

        public void DrawHeading(Canvas canvas)
        {
            if (IsPathCalculatorRunning())
            {
                Vector2 destination = ToScreenCoordinates(_destination.X, _destination.Y);

                float center_x = _overlay_rect.X + (_overlay_rect.Width / 2);
                float center_y = _overlay_rect.Y + (_overlay_rect.Height / 2);

                canvas.DrawLine(HeadingColor, 1, center_x, center_y, destination.X, destination.Y);
                _destination_area.X = destination.X - 3;
                _destination_area.Y = destination.Y - 3;
                _destination_area.Height = 6;
                _destination_area.Width = 6;
                canvas.DrawFillRect(this._destination_area, _heading_color_material);
            }
        }

        public void DrawObjectID(Canvas canvas, Microsoft.DirectX.Direct3D.Font font)
        {
            if (ID != string.Empty)
            {
                _text_rect = font.MeasureString(null, ID, DrawTextFormat.Center, TextColor);
                _text_rect.X = (SpriteArea.X + ((SpriteArea.Width - _text_rect.Width) / 2)) - 5;
                _text_rect.Y = SpriteArea.Bottom;
                _text_rect.Width += 10;
                canvas.DrawFillRect(_text_rect, _textbox_color_material);
           
                font.DrawText(null, ID, _text_rect, DrawTextFormat.Center, TextColor);
            }
        }


        private static SizeF _size = Size.Empty;
        public SizeF DrawIcon(Canvas canvas, float x, float y, float width, float height)
        {
            _size = SizeF.Empty;
            if (_sprite != null)
            {
                float scale = 1;
                if (width > height)
                {
                    scale = width / _sprite.TextureWidth;
                }
                else
                {
                    scale = height / _sprite.TextureHeight;
                }
                _sprite.SetScale(scale, scale, 0);
                _size.Width = scale * _sprite.TextureWidth;
                _size.Height = scale * _sprite.TextureHeight;
                _sprite.DrawIcon(canvas, x, y);
            }
            return _size;
        }

        public void Draw(Canvas canvas, Microsoft.DirectX.Direct3D.Font font)
        {
            if (_sprite != null)
            {
                if (!Hide)
                {

                    if (IsPathCalculatorRunning())
                    {
                        CalculateNewPosition();
                    }
                    else
                    {
                        PositionUpdate();
                    }

                    Vector2 screen = ToScreenCoordinates(_position.X, _position.Y);


                    _sprite.Diffuse = DiffuseColor;
                    if (Scale <= MinScale)
                    {
                        _sprite.SetScale(MinScale, MinScale, 0);
                    }
                    else
                    {
                        _sprite.SetScale(Scale, Scale, 0);
                    }

                    if (DrawWithRotation)
                    {
                        _sprite.SetRotation(_path_calculator.AngleOfAttack, _path_calculator.AngleOfAttack, _path_calculator.AngleOfAttack);
                    }
                    _sprite.SetPosition(screen.X, screen.Y, 0);
                    _sprite_area = _sprite.ToRectangleF();

                    _sprite.Draw(canvas);

                    // *** Overlay Controls and Display *** 
                    _overlay_rect = SpriteAreaF;

                }
            }

        }

        public void DrawSpecial(Canvas canvas, Microsoft.DirectX.Direct3D.Font font)
        {
            DrawSpecial(font, _overlay_rect, _text_rect, canvas);
        }

        public virtual void DrawSpecial(Microsoft.DirectX.Direct3D.Font font, RectangleF overlay_rect, Rectangle text_rect, Canvas canvas)
        {
        }

        public virtual void PositionUpdate()
        {
        }
        public virtual void DestinationUpdate()
        {
        }

        public virtual bool HitTest(float xpos, float ypos)
        {
            return _sprite_area.Contains(xpos, ypos);
        }

        public Vector2 ToScreenCoordinates(float xpos, float ypos)
        {
            _converted_coordinates.X = (xpos * Scale) + _map_position.X;
            _converted_coordinates.Y = (ypos * Scale) + _map_position.Y;
            return _converted_coordinates;
        }


        public bool IsPathCalculatorRunning()
        {
            if (_path_calculator != null)
            {
                return _path_calculator.IsRunning;
            }
            return false;
        }

        public void StartPathCalculator(float xpos, float ypos, float zpos, float velocity)
        {
            if (_path_calculator != null)
            {
                _destination.X = xpos;
                _destination.Y = ypos;
                _destination.Z = zpos;
                
                _path_calculator.InitializeLineFormula(
                                _position.X, 
                                _position.Y,
                                _position.Z, 
                                _destination.X, 
                                _destination.Y, 
                                _destination.Z, 
                                velocity
                                );
                DestinationUpdate();
            }
        }

        private void CalculateNewPosition()
        {
            if (_path_calculator != null)
            {
                _path_calculator.CalculateNewPosition(ref _position);
            }
        }
        public void StopPathCalculator()
        {
            if (_path_calculator.IsRunning)
            {
                _path_calculator.StopPathCalculator();
            }
        }

        public double TimeToDestination()
        {
            if (IsPathCalculatorRunning())
            {
                return _path_calculator.TTD;
            }
            return 0f;
        }
    }
}


