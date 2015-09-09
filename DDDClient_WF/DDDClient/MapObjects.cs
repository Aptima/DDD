using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Aptima.Asim.DDD.Client.Controller;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.PathController;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client
{
    public enum TagPositionEnum : int { ABOVE, CENTER, BELOW, INVISIBLE };

    public class DDDObjects : MappableObject
    {
        public string CurrentlySelectedSensor = String.Empty;
        public int CurrentlySelectedSensorRange = 0;

        public string CurrentlySelectedCapability = String.Empty;
        public int CurrentlySelectedCapabilityRange = 0;

        public string CurrentlySelectedVulnerability = String.Empty;
        public int CurrentlySelectedVulnerabilityRange = 0;


        //sensor range rings
        private Dictionary<string, RangeRingInfo> _sensorRangeRings;
        //capability range rings
        private Dictionary<string, RangeRingInfo> _capabilityRangeRings;
        //vulnerability range rings
        private Dictionary<string, RangeRingInfo> _vulnerabilityRangeRings;

        //These functions should also remove any currently-displayed rings that are no longer there.
        //There might also be a need to MERGE the existing dictionary with the incoming, but we'll see.
        public void SetSensorRangeRings(Dictionary<string, RangeRingInfo> incoming)
        {
            this._sensorRangeRings = new Dictionary<string, RangeRingInfo>(incoming);
        }
        public void SetCapabilityRangeRings(Dictionary<string, RangeRingInfo> incoming)
        {
            this._capabilityRangeRings = new Dictionary<string, RangeRingInfo>(incoming);
        }
        public void SetVulnerabilityRangeRings(Dictionary<string, RangeRingInfo> incoming)
        {
            this._vulnerabilityRangeRings = new Dictionary<string, RangeRingInfo>(incoming);
        }

        public int GetCapabilityRangeRingIntensity(string rangeName, bool isWeapon, int range)
        {
            if (this._capabilityRangeRings == null)
                return -1;
            if (isWeapon)
            {//gets rid of quantified weapons, i think
                if (rangeName.LastIndexOf('(') > 0)
                {
                    rangeName = rangeName.Remove(rangeName.LastIndexOf('('));
                    rangeName = rangeName.Trim();
                }
            }
            if (_capabilityRangeRings.ContainsKey(rangeName))
                return _capabilityRangeRings[rangeName].GetIntensityForRange(range);

            return -1;
        }

        public int GetSensorLongestRangeRingRadius(string rangeName)
        {
            if (this._sensorRangeRings == null)
                return -1;

            int longest = -1;
            if (_sensorRangeRings.ContainsKey(rangeName))
            {
                
                foreach (KeyValuePair<int, int> kvp in _sensorRangeRings[rangeName].rangeIntensities)
                {
                    if (longest < kvp.Key)
                    {
                        longest = kvp.Key;
                    }
                }
            }

            return longest;
        }
        public int GetVulnerabilityLongestRangeRingRadius(string rangeName)
        {
            if (this._vulnerabilityRangeRings == null)
                return -1;

            int longest = -1;
            if (_vulnerabilityRangeRings.ContainsKey(rangeName))
            {

                foreach (KeyValuePair<int, int> kvp in _vulnerabilityRangeRings[rangeName].rangeIntensities)
                {
                    if (longest < kvp.Key)
                    {
                        longest = kvp.Key;
                    }
                }
            }

            return longest;
        }
        public int GetCapabilityLongestRangeRingRadius(string rangeName, bool isWeapon)
        {
            if (this._capabilityRangeRings == null)
                return -1;
            if (isWeapon && rangeName.LastIndexOf('(') > 0)
            {//gets rid of quantified weapons, i think
                rangeName = rangeName.Remove(rangeName.LastIndexOf('('));
                rangeName = rangeName.Trim();
            }
            int longest = -1;
            if (_capabilityRangeRings.ContainsKey(rangeName))
            {

                foreach (KeyValuePair<int, int> kvp in _capabilityRangeRings[rangeName].rangeIntensities)
                {
                    if (longest < kvp.Key)
                    {
                        longest = kvp.Key;
                    }
                }
            }

            return longest;
        }


        private static int _ProgressBarHeight_ = 8;
        private static int _ProgressBarWidth_ = 40;
        public static Size _ProgressBarSize_ = Size.Empty;

        public int _engagementTimer = 0;
        private string _engagementTimeStr = string.Empty;
        public int EngagementTimer
        {
            set
            {
                if (value != _engagementTimer)
                {
                    _engagementTimer = value;
                    _engagementTimeStr = _engagementTimer.ToString();
                }
            }
            get
            {
                return _engagementTimer;
            }
        }

        public string ObjectID
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }
        private String _classification = "";
        public string Classification
        {
            get
            {
                return _classification;
            }
            set
            {
                _classification = value;
            }
        }
        public string PlayerID = "Unassigned";
        public string OwnerID = "Unassigned";
        public string ClassName = "Unknown";
        public string ParentID = "Unassigned";
        public string Tag = string.Empty;// "Unassigned Tag";

        public bool DrawUnmanagedAssetLabel = true;
        public bool DrawUnitTags = false;
        public bool IsWeapon = false;

        private Dictionary<string, DataValue> _customAttributes = new Dictionary<string, DataValue>();
        public Dictionary<string, DataValue> CustomAttributes
        {
            get
            {
                return _customAttributes;
            }
        }

        private double _throttle = 0;
        private double _throttleSlider = 1;
        private String _throttleString = "";
        public string ThrottleStr
        {
            get { return String.Format("{0} %", (_throttle*100)); }
            set { _throttleString = value; }
        }
        public string VelocityStr = string.Empty;
        public string FullVelocityStr = string.Empty;
        public double ThrottleSlider
        {
            get
            {
                return _throttleSlider;
            }
            set
            {
                _throttleSlider = value;
            }
        }
        public double Throttle
        {
            get
            {
                return _throttle;
            }
            set
            {
                _throttle = value;
                VelocityStr = string.Format("{0:0.0} km/hr", (float)(value * _max_speed) * 3.6f);
                FullVelocityStr = string.Format("Speed: {0:0.0} km/hr", (float)(value * _max_speed) * 3.6f);
            }
        }

        public string MaxSpeedStr = string.Empty;
        private double _max_speed = 0;
        public double MaxSpeed
        {
            get
            {
                return _max_speed;
            }
            set
            {
                _max_speed = value;
                MaxSpeedStr = string.Format("{0:0.0} km/hr", value * 3.6f);
                VelocityStr = string.Format("{0:0.0} km/hr", (float)(value * _throttle) * 3.6f);
                FullVelocityStr = string.Format("Speed: {0:0.0} km/hr", (float)(value * _throttle) * 3.6f);
            }
        }

        public string FuelAmountStr = string.Empty;
        private float _fuel_amount = 0;
        public float FuelAmount
        {
            get
            {
                return _fuel_amount;
            }
            set
            {
                _fuel_amount = value;
                FuelAmountStr = string.Format("{0:0.00} lbs", _fuel_amount);
            }
        }

        public string FuelCapacityStr = string.Empty;
        private float _fuel_capacity = 1;
        public float FuelCapacity
        {
            get
            {
                return _fuel_capacity;
            }
            set
            {
                _fuel_capacity = value;
                FuelCapacityStr = string.Format("{0:0.00} lbs", FuelCapacity);
                FuelAmountStr = string.Format("{0:0.00} lbs", _fuel_amount);
            }
        }

        public int CurrentCapabilityAndWeapon = -1;
        private string[] _capability_and_weapons = null;
        public string[] CapabilityAndWeapons
        {
            get
            {
                //if (OwnerID != DDD_Global.Instance.PlayerID)
                //{
                //    return null;
                //}
                return _capability_and_weapons;
            }
            set
            {
                _capability_and_weapons = value;
            }
        }

        public int CurrentSubplatform = -1;
        private string[] _subplatforms = null;
        public string[] SubPlatforms
        {
            get
            {
                //if (OwnerID != DDD_Global.Instance.PlayerID)
                //{
                //    return null;
                //}
                return _subplatforms;
            }
            set
            {
                _subplatforms = value;
            }
        }

        public string[] Vulnerabilities = null;

        public string[] Sensors = null;

        public string State = string.Empty;


        public bool IsAttacking = false;
        public bool IsBeingAttacked = false;
        public List<DDDObjects> Attackers = new List<DDDObjects>();

        public bool DrawProgressBar = true;

        private float _altitude = 0.0F;
        public float Altitude
        {
            set
            {
                _altitude = value;
                AltitudeStr = string.Format("Alt: {0} m", _altitude);
            }
            get
            {
                //float alt = 0.0F;
                //string altString = this.AltitudeStr;
                //altString = altString.Remove(0, 5);
                //altString = altString.Remove(altString.Length - 2, 2);
                ////MessageBox.Show(altString);
                //alt = float.Parse(altString);

                return _altitude;
            }
        }

        public string AltitudeStr = string.Empty;
        public string PositionStr = string.Empty;
        public string DestinationStr = string.Empty;
        public string FullDestinationStr = string.Empty;


        public Color BorderColor = Color.Black;

        private RectangleF _text_rect = RectangleF.Empty;
        private RectangleF _overlay_rect = RectangleF.Empty;
        private RectangleF _progressbar_rect = RectangleF.Empty;
        private RectangleF _border_rect = RectangleF.Empty;
        private Rectangle engagment_rect = Rectangle.Empty;

        private float ChangeX = 0;
        private float ChangeY = 0;
        private float ChangeZ = 0;
        private float DestChangeX = 0;
        private float DestChangeY = 0;
        private float DestChangeZ = 0;

        private Material progress_bar_foreground_material = new Material();
        private Material progress_bar_background_material = new Material();
        public Material red_material = new Material();
        public Material black_material = new Material();

        public Material tag_material = new Material();

        private IMapUpdate _callback_class = null;

        public string CurrentIcon = string.Empty;

        private List<float> _sensorRings = new List<float>();
        public List<float> SensorRangeRings
        {
            get
            {
                return _sensorRings;
            }
            set
            {
                _sensorRings = value;
            }
        }
        private Dictionary<string, float> _capabilityRings = new Dictionary<string, float>(); //{[CapabilityName],[Range]}
        public Dictionary<string, float> CapabilityRangeRings
        {
            get
            {
                return _capabilityRings;
            }
            set
            {
                _capabilityRings = value;
            }
        }
        private Dictionary<string, float> _weaponRings = new Dictionary<string, float>();//{[WeaponName],[Range]}; WeaponName doesn't contain quantities
        public Dictionary<string, float> WeaponRangeRings
        {
            get
            {
                return _weaponRings;
            }
            set
            {
                _weaponRings = value;
            }
        }

        public DDDObjects()
        {
            DiffuseColor = Color.White;
            progress_bar_background_material.Diffuse = Color.Black;
            progress_bar_foreground_material.Diffuse = Color.Lime;
            _ProgressBarSize_.Width = _ProgressBarWidth_;
            _ProgressBarSize_.Height = _ProgressBarHeight_;
            red_material.Diffuse = Color.Red;
            black_material.Diffuse = Color.Black;
            tag_material.Diffuse = Color.FromArgb(180, Color.DarkGoldenrod);
        }

        public DDDObjects(IMapUpdate callback_class)
        {
            DiffuseColor = Color.White;
            progress_bar_background_material.Diffuse = Color.Black;
            progress_bar_foreground_material.Diffuse = Color.Lime;
            _ProgressBarSize_.Width = _ProgressBarWidth_;
            _ProgressBarSize_.Height = _ProgressBarHeight_;
            red_material.Diffuse = Color.Red;
            black_material.Diffuse = Color.Black;
            tag_material.Diffuse = Color.FromArgb(180, Color.DarkGoldenrod);

            _callback_class = callback_class;
        }

        public void UpdateCustomAttributes(Dictionary<string, DataValue> newAttributes)
        {
            List<string> updatedAttributes = new List<string>(); //to avoid collection modification in foreach loop
            foreach (string k in _customAttributes.Keys)
            {
                if (newAttributes.ContainsKey(k))
                {
                    updatedAttributes.Add(k);
                }
            }

            foreach (string k in updatedAttributes)
            {
                _customAttributes[k] = newAttributes[k];
                newAttributes.Remove(k);
            }

            foreach (string att in newAttributes.Keys)
            {
                _customAttributes.Add(att, newAttributes[att]);
            }
        }

        public void SetSprite(Obj_Sprite sprite)
        {
            sprite.Centered = true;
            InitSprite(sprite);
        }

        public override void PositionUpdate()
        {
            if ((ChangeX != Position.X) || (ChangeY != Position.Y) || (ChangeZ != Position.Z))
            {
                ChangeX = Position.X;
                ChangeY = Position.Y;
                ChangeZ = Position.Z;
                PositionStr = string.Format("E: {0}, N: {1}", Math.Round(UTM_Mapping.HorizontalPixelsToMeters(Position.X)),
                    Math.Round(UTM_Mapping.VerticalPixelsToMeters(Position.Y)));
                if (_callback_class != null)
                {
                    _callback_class.PositionChange(this);
                }
                PositionStr = string.Format("E: {0}, N: {1}", Math.Round(UTM_Mapping.HorizontalPixelsToMeters(Position.X)),
                    Math.Round(UTM_Mapping.VerticalPixelsToMeters(Position.Y)));
            }
        }



        public override void DestinationUpdate()
        {
            if ((DestChangeX != Destination.X) || (DestChangeY != Destination.Y) || (DestChangeZ != Destination.Z))
            {
                DestChangeX = Destination.X;
                DestChangeY = Destination.Y;
                DestChangeZ = Destination.Z;
                DestinationStr = string.Format("E: {0}, N: {1}", Math.Round(UTM_Mapping.HorizontalPixelsToMeters(Destination.X)),
                    Math.Round(UTM_Mapping.VerticalPixelsToMeters(Destination.Y))
                   );
                FullDestinationStr = string.Format("Destination:  E: {0}, N: {1}", Math.Round(UTM_Mapping.HorizontalPixelsToMeters(Destination.X)),
    Math.Round(UTM_Mapping.VerticalPixelsToMeters(Destination.Y))
   );
            }
        }

        public void SetDiffuseColor()
        {
            if (IsBeingAttacked)
            {
                DiffuseColor = Color.Red;
            }
            else
            {
                DiffuseColor = Color.White;
            }
        }
        public override bool HitTest(float xpos, float ypos)
        {
            //return (_overlay_rect.Contains(xpos, ypos) || _text_rect.Contains(xpos, ypos) || _progressbar_rect.Contains(xpos, ypos));
            return (_overlay_rect.Contains(xpos, ypos) || _border_rect.Contains(xpos, ypos));

        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private void DrawRangeRing(int color, float radius, float origin_x, float origin_y, float origin_z, int numberOfVertices, Canvas canvas, float h_metersPerPixel, float v_metersPerPixel)
        {
            //if (!IsSensorRingSet())
            //{
            //    SetSensorRingValues(5000, Color.Green, 33, canvas, h_metersPerPixel, v_metersPerPixel);
            //}

            DrawSensorRangeRing(origin_x, origin_y, origin_z, canvas);

            /*
            //Sprite sp_ring = canvas.CreateSprite();
            Obj_Sprite ring = new Obj_Sprite(SpriteFlags.None);
            ring.Initialize(canvas);
            Bitmap b = new Bitmap(Convert.ToInt32(radius), Convert.ToInt32(radius));
            / *
            List<Point> points = new List<Point>();
            points.Add(new Point(0,0));
            points.Add(new Point(25,0));
            points.Add(new Point(50,0));
            points.Add(new Point(75,0));
            points.Add(new Point(100,0));

            Brush brush = new System.Drawing.Drawing2D.PathGradientBrush(points.ToArray()); //((Point[]){new Point(0,0), new Point(25, 0), new Point(50,0), new Point(75,0), new Point(100,0)});
             * /
            Color semiTransRed = Color.FromArgb(33, 255, 0, 0);
            //semiTransRed.A = 100;
            Brush brush = new System.Drawing.SolidBrush(semiTransRed);
            
            Graphics g = Graphics.FromImage(b);
            int width, height;
            //SizeF ringSize = SizeF.Empty;
            float scale = 1;
            //scale logic
    //g.
            g.FillEllipse(brush, 0, 0, 100, 100);
            
            ring.Texture(canvas.CreateTexture(b, out width, out height), Convert.ToInt32(radius), Convert.ToInt32(radius));

            ring.SetScale(scale, scale, 0);
            //ringSize.Width = ring.TextureWidth * scale;
            //ringSize.Height = ring.TextureHeight * scale;
            //ring.Draw(canvas);
            ring.Flags = SpriteFlags.AlphaBlend;
            ring.DrawIcon(canvas, origin_x, origin_y);
            
            //sp_ring.Draw2D(
             */
            return;


            //AD: This could have performance issues.  Might explore saving list of vertices in memory once and retrieving them when drawing.
            List<CustomVertex.TransformedColored> vertexList = new List<CustomVertex.TransformedColored>();
            for (int x = 0; x <= 360; x += (360/numberOfVertices))
            {
                vertexList.Add(new CustomVertex.TransformedColored((float)Math.Cos(DegreeToRadian(x)) * radius, (float)Math.Sin(DegreeToRadian(x)) * radius + 50, 0, 1, color));
            }
            //AD: If you don't convert degree to radian, then you get cool spirograph-like shapes.
            canvas.DrawShape(vertexList, Scale, origin_x, origin_y, true);
        }

        public void DrawRangeRings(Canvas canvas)
        { 
             /* RANGE RING LOGIC */
            float xpos = (SpriteArea.X + (SpriteArea.Width / 2));
            float ypos = (SpriteArea.Y + (SpriteArea.Height / 2) - (Scale * 50)); 
            float zpos = 0.0F;
            
            if (CurrentlySelectedSensor != string.Empty && CurrentlySelectedSensorRange > 0)
            {
                if (!IsSensorRingSet())
                {
                    SetSensorRingValues(CurrentlySelectedSensorRange, Color.FromArgb(DDD_RangeRings.GetInstance().SensorRangeRings.OpaqueRingColor), canvas, UTM_Mapping.HorizonalMetersPerPixel, UTM_Mapping.VerticalMetersPerPixel);
                }
                DrawSensorRangeRing(xpos, ypos, zpos, canvas);
            }
            if (CurrentlySelectedCapability != string.Empty && CurrentlySelectedCapabilityRange > 0)
            {
                if (!IsCapabilityRingSet())
                {
                    SetCapabilityRingValues(CurrentlySelectedCapabilityRange, Color.FromArgb(DDD_RangeRings.GetInstance().CapabilityRangeRings.OpaqueRingColor), canvas, UTM_Mapping.HorizonalMetersPerPixel, UTM_Mapping.VerticalMetersPerPixel);
                }
                DrawCapabilityRangeRing(xpos, ypos, zpos, canvas);
            }

            if (CurrentlySelectedVulnerability != string.Empty && CurrentlySelectedVulnerabilityRange > 0)
            {
                if (!IsVulnerabilityRingSet())
                {
                    SetVulnerabilityRingValues(CurrentlySelectedVulnerabilityRange, Color.FromArgb(DDD_RangeRings.GetInstance().VulnerabilityRangeRings.OpaqueRingColor), canvas, UTM_Mapping.HorizonalMetersPerPixel, UTM_Mapping.VerticalMetersPerPixel);
                }
                DrawVulnerabilityRangeRing(xpos, ypos, zpos, canvas);
            }

            /*       END        */
        }

        public override void DrawSpecial(Microsoft.DirectX.Direct3D.Font font, RectangleF overlay_rect, Rectangle text_rect, Canvas canvas)
        {

            _overlay_rect = overlay_rect;
            _text_rect = text_rect;
            _progressbar_rect = text_rect;
            _border_rect = text_rect;

           

            if ((OwnerID == DDD_Global.Instance.PlayerID) && (DrawProgressBar))
            {

                _progressbar_rect.Y += text_rect.Height;
                _progressbar_rect.Height = _ProgressBarHeight_;
                _progressbar_rect.Width = _ProgressBarWidth_;
                _progressbar_rect.X += ((text_rect.Width - _progressbar_rect.Width) / 2);

                _border_rect.Height = _ProgressBarHeight_ + _text_rect.Height + 2;

                canvas.DrawFillRect(_border_rect, _textbox_color_material);

                DrawObjectID(canvas, font);
                canvas.DrawRect(_border_rect, BorderColor);

                if (FuelCapacity > 0)
                {
                    canvas.DrawProgressBar(_progressbar_rect,
                        progress_bar_background_material,
                        progress_bar_foreground_material,
                        FuelAmount / FuelCapacity);
                }
                else
                {
                    canvas.DrawProgressBar(_progressbar_rect,
                        progress_bar_background_material,
                        progress_bar_foreground_material,
                        0);
                }
            }
            else
            {
                if (DrawUnmanagedAssetLabel)
                {
                    DrawObjectID(canvas, font);
                    canvas.DrawRect(_border_rect, BorderColor);
                }

            }



            foreach (DDDObjects attacker in Attackers)
            {
                float _destinationX = attacker.SpriteArea.X + (attacker.SpriteArea.Width / 2);
                float _destinationY = attacker.SpriteArea.Y + (attacker.SpriteArea.Height / 2);
                canvas.DrawLine(Color.Red, 1,
                    SpriteArea.X + (SpriteArea.Width / 2),
                    SpriteArea.Y + (SpriteArea.Height / 2),
                    _destinationX,
                    _destinationY
                    );
                //engagment_rect.X = (int)(_destinationX - 3);
                //engagment_rect.Y = (int)(_destinationY - 3);
                //engagment_rect.Height = 6;
                //engagment_rect.Width = 6;
                //canvas.DrawFillRect(engagment_rect, red_material);
            }

            if (IsBeingAttacked)
            {
                engagment_rect = font.MeasureString(null, _engagementTimeStr, DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.Red);
                engagment_rect.Width += 10;
                engagment_rect.X = (int)(_overlay_rect.X + (_overlay_rect.Width - engagment_rect.Width) / 2);
                engagment_rect.Y = (int)(_overlay_rect.Y + (_overlay_rect.Height - engagment_rect.Height) / 2);
                canvas.DrawFillRect(engagment_rect, black_material);
                canvas.DrawRect(engagment_rect, Color.Red);
                font.DrawText(null, _engagementTimeStr, engagment_rect, DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.Red);
            }


            // Draw object "Tags"
            if (Tag != string.Empty && (DDD_Global.Instance.TagPosition != TagPositionEnum.INVISIBLE))
            {
                Rectangle tag_rect = font.MeasureString(null, Tag, DrawTextFormat.Center, Color.Black);

                switch (DDD_Global.Instance.TagPosition)
                {
                    case TagPositionEnum.ABOVE:
                        tag_rect.Y = (int)(overlay_rect.Y - tag_rect.Height);
                        tag_rect.X = (int)(overlay_rect.X + ((overlay_rect.Width - tag_rect.Width) * .5f));
                        break;
                    case TagPositionEnum.BELOW:
                        if (!DrawUnmanagedAssetLabel && (OwnerID != DDD_Global.Instance.PlayerID))
                        {
                            tag_rect.Y = (int)(overlay_rect.Bottom + 1);
                            tag_rect.X = (int)(overlay_rect.X + ((overlay_rect.Width - tag_rect.Width) * .5f));
                        }
                        else
                        {
                            tag_rect.Y = (int)(_border_rect.Bottom + 2);
                            tag_rect.X = (int)(overlay_rect.X + ((overlay_rect.Width - tag_rect.Width) * .5f));
                        }
                        break;
                    case TagPositionEnum.CENTER:
                        //tag_rect.Y = (int)(overlay_rect.Y + ((overlay_rect.Height - tag_rect.Height) * .5f));
                        tag_rect.Y = (int)(overlay_rect.Bottom - tag_rect.Height - 2);
                        tag_rect.X = (int)(overlay_rect.X + ((overlay_rect.Width - tag_rect.Width) * .5f));
                        break;
                }

                canvas.DrawFillRect(tag_rect, tag_material);
                font.DrawText(null, Tag, tag_rect, DrawTextFormat.VerticalCenter | DrawTextFormat.Center, Color.Black);
            }


        }


    }
}
