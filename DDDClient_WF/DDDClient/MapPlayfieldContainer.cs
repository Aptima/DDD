using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using System.Drawing;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.Controls;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.PathController;
using Aptima.Asim.DDD.Client.Whiteboard;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;


namespace Aptima.Asim.DDD.Client
{

    public enum MapPlayfieldScrollState : int { NONE = 0, UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4 }
    

    class MapPlayfieldContainer : WindowManager, IMapUpdate
    {
        public bool MiniMapOverride = false;
        public bool ShowPosition = true;
        public bool ShowMiniMap = true;
        public bool ShowUnitColorOnMiniMap = false;
        public bool PauseRendering = false;
        public float MapTextureWidth = 0;
        public float MapTextureHeight = 0;

        public MapPlayfieldScrollState MapScrollState = MapPlayfieldScrollState.NONE;
        public Vector3 Position
        {
            get
            {
                if (Map != null)
                {
                    return Map.Position;
                }
                return Vector3.Empty;
            }
        }
        public Rectangle MapRect
        {
            get
            {
                if (Map != null)
                {
                    return Map.ToRectangle();
                }
                return Rectangle.Empty;
            }
        }
        #region Private Members
        private bool IsPaused = false;

        private bool _mini_map_scroll = false;

        private int ColorStep = 8;
        private new float MinScale = 0;

        private long AnimationTimer = 0;
        private double BlinkCycle = .25;
        private double AnimationCycle = .5;

        private Rectangle background = Rectangle.Empty;
        private Rectangle _bottom_status_bar = Rectangle.Empty;
        private Rectangle _mini_map_track = Rectangle.FromLTRB(0, 0, 4, 4);
        private Rectangle _mini_map = Rectangle.Empty;
        private Rectangle _mini_map_thumb = Rectangle.Empty;

        private Rectangle _mouseoverRangeFinderDisplay = Rectangle.Empty;
        private Microsoft.DirectX.Direct3D.Font _rangeFinderFont = null;
      //  private Material _rangeFinderMaterial = null;

        private ICommand _commands;
        private IGameControl _game_control;

        private MapPlayfield _playfield;
        
        private Obj_Sprite Map = null;

        private Canvas _Canvas_ = null;

        private string _position_text = string.Empty;
        private string Time = "00:00:00";

        private Material _mini_map_background = new Material();
        private Material _mini_map_track_material = new Material();

        private WhiteboardRoom wbRoom = null;

        private Point LineStartLocation;
        private Point LineEndLocation;
        private bool DrawDistanceLine = false;
        private bool WhiteboardDrawing = false;
        static private int MaxFontSize = 1000;
        private Microsoft.DirectX.Direct3D.Font[] wbFonts = new Microsoft.DirectX.Direct3D.Font[MaxFontSize + 1]; 
        static private int ShortenLineBy = 20;
        static private int LineTextSize = 8;
        private Dictionary<string, int> DMColorMap = new Dictionary<string, int>();

        public Color MiniMapColor
        {
            set
            {
                _mini_map_background.Diffuse = Color.FromArgb(Window.BackgroundColor.A, value);
            }
            get
            {
                return _mini_map_background.Diffuse;
            }
        }

        public WhiteboardRoom WBRoom
        {
            set
            {
                wbRoom = value;
            }
            get
            {
                return wbRoom;
            }
        }

        private Sprite _current_unit_sprite = null;
        #endregion

        public MapModes MapState
        {
            get
            {
                if (_playfield != null)
                {
                    return _playfield.Mode;
                }
                return MapModes.MOVE;
            }
        }

        private bool DefaultUnmanagedUnitLabelValue = true;
        public bool DrawUnmanagedUnitLabels
        {
            get
            {
                if (_playfield != null)
                {
                    return _playfield.DrawUnmanagedAssetLabels;
                }
                return false;
            }
            set
            {
                if (_playfield != null)
                {
                    _playfield.DrawUnmanagedAssetLabels = value;
                }
                else
                {
                    DefaultUnmanagedUnitLabelValue = value;
                }
            }
        }

        public bool DrawCenteredText(Microsoft.DirectX.Direct3D.Font font, string text, int x, int y, int color)
        {
            int centeredX = 0;
            int centeredY = 0;
            Rectangle rect;

            if ((font == null) || (text == null) || (text.Length <= 0))
            {
                return false;
            }

            rect = font.MeasureString(null, text, DrawTextFormat.None, color);

            centeredX = x - rect.Width / 2;
            centeredY = y - rect.Height / 2;
            if (font.DrawText(null, text, centeredX, centeredY, color) == 0)
            {
                return false;
            }

            return true;
        }

        public void DrawShortenedLine(Canvas canvas, int color, int pointSize,
            float x1, float y1, float x2, float y2, float scale)
        {
            float lineLength = (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            int shortenLineBy = (int) (ShortenLineBy * scale);

            if (lineLength < shortenLineBy)
            {
                canvas.DrawLine(color, pointSize, x1, y1, x2, y2);
            }
            else
            {
                float angle = (float) Math.Atan2(y2 - y1, x2 - x1);
                float newEndX = x1 + (lineLength - shortenLineBy) * (float)Math.Cos(angle);
                float newEndY = y1 + (lineLength - shortenLineBy) * (float)Math.Sin(angle);

                canvas.DrawLine(color, pointSize, x1, y1, newEndX, newEndY);
            }
        }

        public void DrawWhiteboardRoomObjects(WhiteboardRoom wbRoom, Canvas canvas)
        {
            if (wbRoom == null)
            {
                return;
            }

            // Draw all whiteboard commands
            Array drawingCommands = wbRoom.GetCurrentDrawingCommands();

            foreach (object obj in drawingCommands)
            {
                if (obj is LineObject)
                {
                    LineObject line = (LineObject)obj;
                    LocationValue start = new LocationValue();
                    LocationValue end = new LocationValue();
                    int drawPointSize = (int)(line.Width * _playfield.Scale);

                    if ((drawPointSize == 0) && (line.Width > 0))
                    {
                        drawPointSize = 1;
                    }

                    if ((line.Mode == DrawModes.Line) || (line.Mode == DrawModes.Circle) ||
                        (line.Mode == DrawModes.Arrow))
                    {
                        start.X = line.Start.X * _playfield.Scale + Map.Position.X;
                        start.Y = line.Start.Y * _playfield.Scale + Map.Position.Y;

                        end.X = line.End.X * _playfield.Scale + Map.Position.X;
                        end.Y = line.End.Y * _playfield.Scale + Map.Position.Y;

                        if (line.Mode == DrawModes.Line)
                        {
                            if ((line.Text != null) && (line.Text.Length > 0))
                            {
                                int lineTextSize = (int) ((LineTextSize / line.OriginalScale) * _playfield.Scale);

                                DrawShortenedLine(canvas, line.Color, drawPointSize, (float)start.X, (float)start.Y,
                                    (float)end.X, (float)end.Y, (float) (_playfield.Scale / line.OriginalScale));

                                if (lineTextSize > MaxFontSize)
                                {
                                    lineTextSize = MaxFontSize;
                                }
                                if (lineTextSize < 1)
                                {
                                    lineTextSize = 1;
                                }
                                if (wbFonts[lineTextSize] == null)
                                {
                                    wbFonts[lineTextSize] = canvas.CreateFont(new System.Drawing.Font("Arial", lineTextSize, FontStyle.Bold, GraphicsUnit.Pixel));
                                }
                                DrawCenteredText(wbFonts[lineTextSize], line.Text, (int)end.X, (int)end.Y, line.Color);
                            }
                            else
                            {
                                canvas.DrawLine(line.Color, drawPointSize, (float)start.X, (float)start.Y,
                                    (float)end.X, (float)end.Y);
                            }
                        }
                        else if (line.Mode == DrawModes.Circle)
                        {
                            canvas.DrawCircle(line.Color, drawPointSize, (float)start.X, (float)start.Y,
                                (float)end.X, (float)end.Y);
                        }
                        else if (line.Mode == DrawModes.Arrow)
                        {
                            canvas.DrawArrow(line.Color, drawPointSize, (float)start.X, (float)start.Y,
                                (float)end.X, (float)end.Y);
                        }
                    }
                    else if ((line.Mode == DrawModes.Text) && (line.Text != null) &&
                        (line.Text.Length > 0))
                    {
                        start.X = line.Start.X * _playfield.Scale + Map.Position.X;
                        start.Y = line.Start.Y * _playfield.Scale + Map.Position.Y;

                        if (drawPointSize > MaxFontSize)
                        {
                            drawPointSize = MaxFontSize;
                        }
                        if (wbFonts[drawPointSize] == null)
                        {
                            wbFonts[drawPointSize] = canvas.CreateFont(new System.Drawing.Font("Arial", drawPointSize, FontStyle.Bold, GraphicsUnit.Pixel));
                        }
                        DrawCenteredText(wbFonts[drawPointSize], line.Text, (int)start.X, (int)start.Y, line.Color);

                        // Update the bounding box if needed
                        if ((line.BoundingPolygon == null) || (wbRoom == WBRoom))
                        {
                            RectangleF textRect;
                            Polygon textPoly;
                            float width;
                            float height;
                            PointF boundStart = new PointF();

                            textRect = wbFonts[drawPointSize].MeasureString(null, line.Text, DrawTextFormat.None, line.Color);

                            width = textRect.Right / _playfield.Scale;
                            if ((width * line.OriginalScale) < 5)
                            {
                                width = 5 / (float) line.OriginalScale;
                            }
                            height = textRect.Bottom / _playfield.Scale;
                            if ((height * line.OriginalScale) < 5)
                            {
                                height = 5 / (float) line.OriginalScale;
                            }
                            boundStart.X = (float)line.Start.X - (width / 2);
                            boundStart.Y = (float)line.Start.Y - (height / 2);

                            textPoly = new Polygon();
                            textPoly.Points.Add(new Vector(boundStart.X, boundStart.Y));
                            textPoly.Points.Add(new Vector(boundStart.X + width, boundStart.Y));
                            textPoly.Points.Add(new Vector(boundStart.X + width, boundStart.Y + height));
                            textPoly.Points.Add(new Vector(boundStart.X, boundStart.Y + height));
                            textPoly.Offset(0, 0);
                            textPoly.BuildEdges();

                            line.BoundingPolygon = textPoly;
                        }
                    }

                    // Draw Bounding rectable if object is selected
                    if ((line.ObjectSelected) && (line.BoundingPolygon != null) && (line.BoundingPolygon.Points.Count >= 2))
                    {
                        LocationValue boundStart = new LocationValue();
                        LocationValue boundEnd = new LocationValue();
                        Vector prevPoint = line.BoundingPolygon.Points[line.BoundingPolygon.Points.Count - 1]; 
                        foreach (Vector curPoint in line.BoundingPolygon.Points)
                        {
                            boundStart.X = prevPoint.X * _playfield.Scale + Map.Position.X;
                            boundStart.Y = prevPoint.Y * _playfield.Scale + Map.Position.Y;

                            boundEnd.X = curPoint.X * _playfield.Scale + Map.Position.X;
                            boundEnd.Y = curPoint.Y * _playfield.Scale + Map.Position.Y;

                            canvas.DrawLine(Color.Black.ToArgb(), 1, (float)boundStart.X, (float)boundStart.Y,
                                (float)boundEnd.X, (float)boundEnd.Y);

                            prevPoint = curPoint;
                        }
                    }
                }
            }
        }

        public bool ScaleUnitWithMap
        {
            get
            {
                if (_playfield != null)
                {
                    return _playfield.ScaleUnitWithMap;
                }
                return false;
            }
            set
            {
                if (_playfield != null)
                {
                    _playfield.ScaleUnitWithMap = value;
                }
            }
        }

        public MapPlayfieldContainer(IGameControl game_control, ICommand commands)
            : base(game_control)
        {
            _commands = commands;
            _game_control = game_control;
            _mini_map_background.Diffuse = Color.FromArgb(180, 63, 63, 63);
            _mini_map_track_material.Diffuse = Color.Red;
        }

        public bool MapFits_To_MDXHostControl(Rectangle hostcontrol)
        {
            if (Map != null)
            {
                Rectangle MapRect = Map.ToRectangle();
                return MapRect.Contains(hostcontrol);
            }
            return false;
        }

        public bool Map_IsContained_Horizontal(Rectangle hostcontrol)
        {
            if (Map != null)
            {
                Rectangle MapRect = Map.ToRectangle();
                if (MapRect.Right >= hostcontrol.Right)
                {
                    return true;
                }
            }
            return false;
        }


        public bool Map_IsContained_Vertical(Rectangle hostcontrol)
        {
            if (Map != null)
            {
                Rectangle MapRect = Map.ToRectangle();
                if (MapRect.Bottom >= hostcontrol.Bottom)
                {
                    return true;
                }
            }
            return false;
        }

        public void ActiveRegionUpdate(string object_id, bool visible, int color, List<CustomVertex.TransformedColored> points)
        {
            if (_playfield != null)
            {
                points.Add(new CustomVertex.TransformedColored(points[0].X, points[0].Y, points[0].Z, points[0].Rhw, color));
                _playfield.AddActiveZone(object_id, color, visible, points);
            }
        }


        public void SetMapScale(float scale)
        {
            if (_playfield != null)
            {
                this.Scale = scale;
                _playfield.Scale = scale;
                Map.SetScale(scale, scale, 0);
            }
        }

        public void ClearMapSelection(bool send_update)
        {
            if (_playfield != null)
            {
                _playfield.DeselectAll();
                if (send_update)
                {
                    _commands.SelectionUpdate();
                }
            }
        }
        public void ClearMapSelection()
        {
            ClearMapSelection(true);
        }
        public DDDObjects GetSelectedObject()
        {
            if (_playfield != null)
            {
                return (DDDObjects)_playfield.GetSelectedObject();
            }
            return null;
        }
        public DDDObjects GetObjectAttributes(string objectname)
        {
            if (_playfield != null)
            {
                return (DDDObjects)_playfield.GetMappableObject(objectname);
            }
            return null;
        }

        public void EnterMoveMode()
        {
            if (_playfield != null)
            {
                _playfield.EnterMoveMode();
            }
        }
        public void EnterAttackMode()
        {
            if (_playfield != null)
            {
                _playfield.EnterAttackMode();
            }
        }
        public void EnterSubplatformMode()
        {
            if (_playfield != null)
            {
                _playfield.EnterSubPlatformMode();
            }
        }
        public void EnterDrawMode()
        {
            if (_playfield != null)
            {
                _playfield.EnterDrawMode();
            }
        }

        public DDDObjects SelectObject(string objectname)
        {
            if (_playfield != null)
            {
                if (_playfield.ContainsMapObject(objectname))
                {
                    _playfield.Select(objectname);
                    return (DDDObjects)_playfield.GetMappableObject(objectname);
                }
            }
            return null;
        }
        public void EngageTarget()
        {
            if (_playfield != null)
            {
                if (_playfield.SelectionCount() > 1)
                {
                    _playfield.WinForm_AttackSelected();
                }
            }
        }



#region IMapUpdate Members

        void IMapUpdate.PositionChange(DDDObjects obj)
        {
            DDDObjects selected = GetSelectedObject();
            if (selected != null)
            {
                if (selected == obj)
                {
                    _commands.SelectionUpdate();
                }
            }
        }

        #endregion

        public void RecalculateMinumum(Rectangle hostcontrol)
        {
            if (_playfield != null)
            {
                _playfield.ClientArea = hostcontrol;
                this.MinScale = _playfield.GetMinimumScale();
            }
        }
        public void SetMapPosition(int x, int y)
        {
            if (Map != null)
            {
                Map.SetPosition(x, y, 0);
            }
        }
        public void ResetVunerabilities()
        {
        }
        protected override void OnPan(float xpos, float ypos)
        {
            if (_playfield != null)
            {
                this._playfield.Pan(xpos, ypos);
            }
        }
        #region Scene Events
        public override void OnSceneLoading(GameFramework g)
        {
            SceneMode = MODE.SCENE_RENDER;
        }

        public void InitializeObjects(ViewProMotionUpdate update)
        {
            try
            {
                if (update.Icon == null)
                {
                    update.Icon = string.Format("{0}.Unknown.png", DDD_Global.Instance.ImageLibrary);
                }
                if (!Sprites.ContainsKey(update.Icon))
                {
                    update.Icon = string.Format("{0}.Unknown.png", DDD_Global.Instance.ImageLibrary); 
                }
                DDDObjects obj = new DDDObjects(this);
                obj.ObjectID = update.ObjectId;
                obj.PlayerID = update.PlayerId;
                obj.OwnerID = update.OwnerID;
                obj.CapabilityAndWeapons = null;
                obj.TextBoxColor = Color.FromArgb(update.PlayerColor);
                obj.SetPosition(update.StartX, update.StartY, update.StartZ);
                obj.Throttle = update.Throttle;
                obj.MaxSpeed = update.MaxSpeed;
                obj.FuelAmount = 0;
                obj.FuelCapacity = 0;
                obj.IsAttacking = update.IsWeapon;
                obj.Altitude = update.StartZ;
                obj.CurrentIcon = update.Icon;
                obj.IsWeapon = update.IsWeapon;

                // Saved the DM Color for use on the mini map
                if (!DMColorMap.ContainsKey(update.OwnerID))
                {
                    DMColorMap.Add(update.OwnerID, update.PlayerColor);
                }

                if (!DDD_Global.Instance.IsConnected)
                {
                    switch (update.ObjectId)
                    {
                        case "object1":
                            obj.PlayerID = "Demo Player";
                            obj.OwnerID = "Demo Player";
                            obj.CapabilityAndWeapons = new string[] { "Null Capability - object1", "Null Weapon1", "Null Weapon2", "Null Weapon3" };
                            obj.SubPlatforms = new string[] { "Pilot 1", "Navigator 1", "Black Box" };
                            obj.Vulnerabilities = new string[] { "Superman", "Batman", "The Flash" };
                            obj.Sensors = new string[] { "Sensor1", "Sensor2" };
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 100;
                            obj.FuelCapacity = 200;
                            obj.Altitude = 12;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;

                        case "object2":
                            obj.PlayerID = "Demo Player";
                            obj.OwnerID = "Demo Player";
                            obj.CapabilityAndWeapons = new string[] { "Null Capability - object2", "Null Weapon" };
                            obj.Vulnerabilities = new string[] { "Aquaman", "Green Lantern", "Robin" };
                            obj.Sensors = new string[] { "Sensor3", "Sensor4" };
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 275;
                            obj.FuelCapacity = 300;
                            obj.Altitude = 0;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;

                        case "object3":
                            obj.PlayerID = "Red";
                            obj.OwnerID = "Red";
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 48;
                            obj.FuelCapacity = 50;
                            obj.Altitude = 0;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;

                        case "object4":
                            obj.PlayerID = "Red";
                            obj.OwnerID = "Red";
                            obj.CapabilityAndWeapons = new string[] { "Null Capability - object4", "Null Weapon" };
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 49;
                            obj.FuelCapacity = 50;
                            obj.Altitude = 0;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;

                        case "object5":
                            obj.PlayerID = "Red";
                            obj.OwnerID = "Red";
                            obj.CapabilityAndWeapons = new string[] { "Null Capability - object5", "Null Weapon" };
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 50;
                            obj.FuelCapacity = 50;
                            obj.Altitude = 0;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;

                        default:
                            obj.PlayerID = "Demo Player";
                            obj.OwnerID = "Demo Player";
                            obj.CapabilityAndWeapons = new string[] { "Null Capability - Default", "Null Weapon1", "Null Weapon2", "Null Weapon3" };
                            obj.MaxSpeed = 24;
                            obj.FuelAmount = 100;
                            obj.FuelCapacity = 200;
                            obj.Altitude = 12;
                            obj.SetSprite(Sprites[update.Icon]);
                            obj.DrawWithRotation = false;
                            _playfield.AddMappableObject(update.ObjectId, obj);
                            break;
                    }
                }
                else
                {
                    if (!_playfield.ContainsMapObject(update.ObjectId))
                    {
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = GameFramework.Instance().GetTexture(update.Icon).rotate;
                        _playfield.AddMappableObject(update.ObjectId, obj);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Initialize: " + e.Message + ":" + e.StackTrace);
                return;
            }      

        }

        public List<string> GetListOfPlayfieldObjects()
        {
            return _playfield.GetListOfObjects();
        }


        public override void OnInitializeScene(GameFramework g)
        {
            Size DisplaySize = g.CANVAS.Size;

            BindGameController();

            this.Fonts.Add("Medium", g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 12)));
            this.Fonts.Add("Small", g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 8)));
            this.Fonts.Add("Arial10B", g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 10, FontStyle.Bold)));
            this.Fonts.Add("Arial8B", g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 8, FontStyle.Bold)));
            this.Fonts.Add("RangeRing", g.CANVAS.CreateFont(new System.Drawing.Font("Times New Roman", 8)));

            _current_unit_sprite = g.CANVAS.CreateSprite();

            background.X = 0;
            background.Y = 0;
            background.Height = DisplaySize.Height;
            background.Width = DisplaySize.Width;
            SetRootWindow(background);


            foreach (string str in g.Textures.Keys)
            {
                if (str.CompareTo("MAP") != 0)
                {
                    Obj_Sprite s = CreateSprite(str, SpriteFlags.AlphaBlend);
                    s.Initialize(g.CANVAS);
                    s.Texture(g.Textures[str]);
                }
            }
                
            Map = CreateSprite("MAP", SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            _mini_map.Width = 150;
            Map.Initialize(g.CANVAS);
            Map.Texture(g.GetTexture("MAP"));
            //Map.Diffuse = Color.Gray;
            MapTextureHeight = Map.TextureHeight;
            MapTextureWidth = Map.TextureWidth;



            _playfield = new MapPlayfield(g.CANVAS, Map, background, _commands);
            _playfield.WinForm_Mode = true;

            MinScale = _playfield.GetMinimumScale();
            Map.SetPosition(background.X, background.Y, 0);
            SetMapScale(MinScale);
            _playfield.OnInitializeScene(g);
            _playfield.SendMapPosition(true); //attempt to resolve bug 4539, send an initial screen map position

            _playfield.DrawUnmanagedAssetLabels = DefaultUnmanagedUnitLabelValue;

            if (!DDD_Global.Instance.IsConnected)
            {
                ViewProMotionUpdate update = new ViewProMotionUpdate();

                update.ObjectId = "object1";
                update.Throttle = 1;
                update.StartX = 10;
                update.StartY = 10;
                update.StartZ = 0;
                update.PlayerColor = Color.White.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.BTTR.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);

                update.ObjectId = "object2";
                update.Throttle = 1;
                update.StartX = 30;
                update.StartY = 30;
                update.StartZ = 0;
                update.PlayerColor = Color.Green.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CA.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);

                update.ObjectId = "object3";
                update.Throttle = 1;
                update.StartX = 60;
                update.StartY = 60;
                update.StartZ = 0;
                update.PlayerColor = Color.Yellow.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CH47.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);

                update.ObjectId = "object4";
                update.Throttle = 1;
                update.StartX = 90;
                update.StartY = 10;
                update.StartZ = 0;
                update.PlayerColor = Color.Blue.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CMBT-ENGR-CO.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);

                update.ObjectId = "object5";
                update.Throttle = 1;
                update.StartX = 10;
                update.StartY = 60;
                update.StartZ = 0;
                update.PlayerColor = Color.Black.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.DSM-TRP.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);

                update.ObjectId = "object1e";
                update.Throttle = 1;
                update.StartX = 10;
                update.StartY = 30;
                update.StartZ = 0;
                update.PlayerColor = Color.Red.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.FA-BTTR.png";
                ((IController)_game_control).ViewProInitializeUpdate(update);
            }

            AnimationTimer = GameFramework.QueryPerformanceTimer();
            _commands.HandshakeInitializeGUIDone(DDD_Global.Instance.PlayerID);


        }
             
        public override void OnRender(Canvas canvas)
        {
            if (!PauseRendering)
            {
                Rectangle PlayfieldRect = canvas.TargetControl.ClientRectangle;
                _playfield.ClientArea = PlayfieldRect;

                int bottom_left = (PlayfieldRect.Y + PlayfieldRect.Height);

                Rectangle SpriteRect = Rectangle.Empty;

                if (Map != null)
                {
                    SpriteRect = Map.ToRectangle();

                    // Prevent black from appearing on sides of map.
                    if (!SpriteRect.Contains(PlayfieldRect))
                    {
                        int right = (int)(Map.Position.X + (Map.TextureWidth * _playfield.Scale));
                        int bottom = (int)(Map.Position.Y + (Map.TextureHeight * _playfield.Scale));
                        if (right < PlayfieldRect.Width)
                        {
                            Pan(PlayfieldRect.Width - right, 0);
                        }
                        if (bottom < PlayfieldRect.Height)
                        {
                            Pan(0, PlayfieldRect.Height - bottom);
                        }
                        if (Map.Position.Y > PlayfieldRect.Y)
                        {
                            Pan(0, -(Map.Position.Y));
                        }
                        if (Map.Position.X > PlayfieldRect.X)
                        {
                            Pan(-(Map.Position.X), 0);
                        }
                    }
                }

                switch (MapScrollState)
                {
                    case MapPlayfieldScrollState.UP:
                        Pan(0, -10);
                        break;
                    case MapPlayfieldScrollState.DOWN:
                        Pan(0, 10);
                        break;
                    case MapPlayfieldScrollState.LEFT:
                        Pan(-10, 0);
                        break;
                    case MapPlayfieldScrollState.RIGHT:
                        Pan(10, 0);
                        break;
                }

                _playfield.OnRender(canvas);

                if (WBRoom != null)
                {
                    // Draw Whiteboard information from other selected rooms
                    List<string> otherWBNames = WBRoom.GetOtherWBRoomNames();
                    if (otherWBNames != null)
                    {
                        foreach (string otherWBName in otherWBNames)
                        {
                            DrawWhiteboardRoomObjects(WBRoom.GetOtherWBRoom(otherWBName), canvas);
                        }
                    }

                    // Draw Whiteboard information from current room
                    DrawWhiteboardRoomObjects(WBRoom, canvas);

                    // Draw any commands in progress
                    if ((WBRoom.DrawMode == DrawModes.Line) &&
                        (DrawDistanceLine == true) && (WhiteboardDrawing == true))
                    {
                        if ((WBRoom.DrawText != null) && (WBRoom.DrawText.Length > 0))
                        {
                            int lineTextSize = LineTextSize;

                            DrawShortenedLine(canvas, WBRoom.DrawColor, WBRoom.DrawPointSize, LineStartLocation.X, LineStartLocation.Y,
                                LineEndLocation.X, LineEndLocation.Y, 1f);

                            if (lineTextSize > MaxFontSize)
                            {
                                lineTextSize = MaxFontSize;
                            }
                            if (lineTextSize < 1)
                            {
                                lineTextSize = 1;
                            }
                            if (wbFonts[lineTextSize] == null)
                            {
                                wbFonts[lineTextSize] = canvas.CreateFont(new System.Drawing.Font("Arial", lineTextSize, FontStyle.Bold, GraphicsUnit.Pixel));
                            }
                            DrawCenteredText(wbFonts[lineTextSize], WBRoom.DrawText, LineEndLocation.X, LineEndLocation.Y, WBRoom.DrawColor);
                        }
                        else
                        {
                            canvas.DrawLine(WBRoom.DrawColor, WBRoom.DrawPointSize, LineStartLocation.X, LineStartLocation.Y,
                                LineEndLocation.X, LineEndLocation.Y);
                        }
                    }
                    else if ((WBRoom.DrawMode == DrawModes.Circle) &&
                        (DrawDistanceLine == true) && (WhiteboardDrawing == true))
                    {
                        canvas.DrawCircle(WBRoom.DrawColor, WBRoom.DrawPointSize, LineStartLocation.X, LineStartLocation.Y,
                            LineEndLocation.X, LineEndLocation.Y);
                    }
                    else if ((WBRoom.DrawMode == DrawModes.Arrow) &&
                        (DrawDistanceLine == true) && (WhiteboardDrawing == true))
                    {
                        canvas.DrawArrow(WBRoom.DrawColor, WBRoom.DrawPointSize, LineStartLocation.X, LineStartLocation.Y,
                            LineEndLocation.X, LineEndLocation.Y);
                    }
                    else if ((WBRoom.DrawMode == DrawModes.Text) &&
                        (DrawDistanceLine == true) && (WhiteboardDrawing == true))
                    {
                        if ((WBRoom.DrawText != null) && (WBRoom.DrawText.Length > 0))
                        {
                            int drawPointSize = WBRoom.DrawPointSize + 7;

                            if (drawPointSize > MaxFontSize)
                            {
                                drawPointSize = MaxFontSize;
                            }
                            if (wbFonts[drawPointSize] == null)
                            {
                                wbFonts[drawPointSize] = canvas.CreateFont(new System.Drawing.Font("Arial", drawPointSize, FontStyle.Bold, GraphicsUnit.Pixel));
                            }
                            DrawCenteredText(wbFonts[drawPointSize], WBRoom.DrawText, LineEndLocation.X, LineEndLocation.Y, WBRoom.DrawColor);
                        }
                    }
                    else if ((WBRoom.DrawMode == DrawModes.Selection) &&
                        (DrawDistanceLine == true) && (WhiteboardDrawing == true))
                    {
                        RectangleF selectionRect;

                        int selectionX = Math.Min(LineStartLocation.X, LineEndLocation.X);
                        int selectionY = Math.Min(LineStartLocation.Y, LineEndLocation.Y);

                        selectionRect = new RectangleF((float)selectionX, (float)selectionY,
                            (float)Math.Abs(LineStartLocation.X - LineEndLocation.X),
                            (float)Math.Abs(LineStartLocation.Y - LineEndLocation.Y));
                        canvas.DrawRect(selectionRect, Color.Orange);
                    }
                }

                // Draw Mini map
                _bottom_status_bar.Height = 35;
                _bottom_status_bar.Width = PlayfieldRect.Width;
                _bottom_status_bar.X = PlayfieldRect.X;
                _bottom_status_bar.Y = bottom_left - _bottom_status_bar.Height;
                canvas.DrawFillRect(_bottom_status_bar, _mini_map_background);
                
                if (true) //check DDD_Global or playfield
                {
                    if (_rangeFinderFont == null && this.Fonts.ContainsKey("RangeRing"))
                    {
                        _rangeFinderFont = this.Fonts["RangeRing"];
                    }
                    string rangeText = DDD_Global.Instance.GetRangeFinderDisplayString();

                    if (_rangeFinderFont != null && rangeText.Trim() != string.Empty)
                    {
                        _mouseoverRangeFinderDisplay = _rangeFinderFont.MeasureString(null, rangeText, DrawTextFormat.Center, Color.White);
                        _mouseoverRangeFinderDisplay.X = Convert.ToInt32(DDD_Global.Instance.RangeFinderXDisplay);// 500;
                        _mouseoverRangeFinderDisplay.Y = Convert.ToInt32(DDD_Global.Instance.RangeFinderYDisplay);// 400;
                        _mouseoverRangeFinderDisplay.Width += 10;
                        canvas.DrawFillRect(_mouseoverRangeFinderDisplay, _mini_map_background);//_rangeFinderMaterial);
                        _rangeFinderFont.DrawText(null, rangeText, _mouseoverRangeFinderDisplay, DrawTextFormat.Center, Color.White);

                        //DRAW LINE FROM OBJECT TO CURSOR

                        float objX = 0f;
                        float objY = 0f;
                        DDDObjects obj = _playfield.GetSelectedObject();
                        if (obj != null)
                        {
                            objX = obj.ScreenCoordinates.X;
                            objY = obj.ScreenCoordinates.Y;

                            canvas.DrawLine(Color.Black, 3, _mouseoverRangeFinderDisplay.X - 10, _mouseoverRangeFinderDisplay.Y - 15, objX, objY);
                        }
                    }
                }

                if (!MiniMapOverride)
                {
                    if ((!Map_IsContained_Horizontal(PlayfieldRect)) && (!Map_IsContained_Vertical(PlayfieldRect)))
                    {
                        ShowMiniMap = false;
                    }
                    else
                    {
                        ShowMiniMap = true;
                    }

                    if (WBRoom != null)
                    {
                        ShowMiniMap = true;
                    }
     
                    if (ShowMiniMap)
                    {
                        _mini_map.X = (PlayfieldRect.X + PlayfieldRect.Width) - _mini_map.Width;
                        _mini_map.Y = 0;
                        _mini_map.Height = (int)((Map.TextureHeight / Map.TextureWidth) * (float)_mini_map.Width);

                        _mini_map_thumb.X = (int)(-(Map.Position.X / (Map.TextureWidth * _playfield.Scale)) * (float)_mini_map.Width) + _mini_map.X;
                        _mini_map_thumb.Y = (int)(-(Map.Position.Y / (Map.TextureHeight * _playfield.Scale)) * (float)_mini_map.Height) + _mini_map.Y;
                        _mini_map_thumb.Width = (int)(_mini_map.Width * ((float)PlayfieldRect.Width / (Map.TextureWidth * _playfield.Scale)));
                        _mini_map_thumb.Height = (int)(_mini_map.Height * ((float)PlayfieldRect.Height / (Map.TextureHeight * _playfield.Scale)));

                        canvas.DrawFillRect(_mini_map, _mini_map_background);

                        // Draw rect for other users in a whiteboard room
                        if (wbRoom != null)
                        {
                            int originX = 0;
                            int originY = 0;
                            int screenSizeWidth = 0;
                            int screenSizeHeight = 0;
                            double screenZoom = 0.0;

                            foreach (string wbUser in wbRoom.MembershipList)
                            {
                                if (string.Compare(wbUser, DDD_Global.Instance.PlayerID) == 0)
                                {
                                    continue;
                                }

                                if (wbRoom.GetScreenViewInfo(wbUser, ref originX, ref originY, ref screenSizeWidth, ref screenSizeHeight,
                                        ref screenZoom))
                                {
                                    Rectangle userMiniMapRect = Rectangle.Empty;
                                    Vector2 _metersPerPixelValues = new Vector2();
                                    Vector2 userPosition = new Vector2();
                                    _metersPerPixelValues.X = UTM_Mapping.HorizonalMetersPerPixel;
                                    _metersPerPixelValues.Y = UTM_Mapping.VerticalMetersPerPixel;

                                    userPosition.X = (float) originX / _metersPerPixelValues.X;
                                    userPosition.Y = (float)originY  / _metersPerPixelValues.Y;
                                    userMiniMapRect.X = (int)((userPosition.X / (Map.TextureWidth)) * (float)_mini_map.Width) + _mini_map.X;
                                    userMiniMapRect.Y = (int)((userPosition.Y / (Map.TextureHeight)) * (float)_mini_map.Height) + _mini_map.Y;
                                    userMiniMapRect.Width = (int)((float)screenSizeWidth / _metersPerPixelValues.X /
                                        Map.TextureWidth * (float)_mini_map.Width);
                                    userMiniMapRect.Height = (int)((float)screenSizeHeight / _metersPerPixelValues.Y /
                                        Map.TextureHeight * (float)_mini_map.Height);

                                    // Draw rect for this a whiteboard user
                                    if (DMColorMap.ContainsKey(wbUser))
                                    {
                                        canvas.DrawRect(userMiniMapRect, Color.FromArgb(DMColorMap[wbUser]));
                                    }
                                    else
                                    {
                                        canvas.DrawRect(userMiniMapRect, Color.DarkBlue);
                                    }
                                }

                            }
                        }

                        // Draw rect for this user
                        if ((wbRoom != null) && DMColorMap.ContainsKey(DDD_Global.Instance.PlayerID))
                        {
                            canvas.DrawRect(_mini_map_thumb, Color.FromArgb(DMColorMap[DDD_Global.Instance.PlayerID]));
                        }
                        else
                        {
                            canvas.DrawRect(_mini_map_thumb, Color.Red);
                        }

                        lock (this)
                        {
                            DDDObjects selected = GetSelectedObject();
                            foreach (string obj_name in _playfield.PlayfieldObjects)
                            {
                                DDDObjects obj = (DDDObjects)_playfield.GetMappableObject(obj_name);
                                if (obj != null)
                                {
                                    _mini_map_track.X = (int)(((_mini_map.Width / Map.TextureWidth) * obj.Position.X) + _mini_map.X) - (int)(.5 * _mini_map_track.Width);
                                    _mini_map_track.Y = (int)(((_mini_map.Height / Map.TextureHeight) * obj.Position.Y) + _mini_map.Y) - (int)(.5 * _mini_map_track.Height);
                                    if (selected != null)
                                    {
                                        if ((selected.ID == obj.ID) && !ShowUnitColorOnMiniMap)
                                        {
                                            canvas.DrawTri(_mini_map_track, Color.Yellow, DIRECTION.UP);
                                            continue;
                                        }
                                    }

                                    if (!ShowUnitColorOnMiniMap)
                                    {
                                        canvas.DrawTri(_mini_map_track, Color.DodgerBlue, DIRECTION.UP);
                                    }
                                    else
                                    {
                                        canvas.DrawTri(_mini_map_track, obj.TextBoxColor, DIRECTION.UP);
                                    }

                                }
                            }


                            foreach (string obj_name in _playfield.PlayfieldObjects)
                            {
                                DDDObjects obj = (DDDObjects)_playfield.GetMappableObject(obj_name);
                                if (obj != null)
                                {
                                    _mini_map_track.X = (int)(((_mini_map.Width / Map.TextureWidth) * obj.Position.X) + _mini_map.X) - (int)(.5 * _mini_map_track.Width);
                                    _mini_map_track.Y = (int)(((_mini_map.Height / Map.TextureHeight) * obj.Position.Y) + _mini_map.Y) - (int)(.5 * _mini_map_track.Height);

                                    if (obj.IsBeingAttacked)
                                    {
                                        if (GameFramework.ElapsedSeconds(AnimationTimer, AnimationCycle))
                                        {
                                            AnimationTimer = GameFramework.QueryPerformanceTimer();
                                        }
                                        if (GameFramework.ElapsedSeconds(AnimationTimer, BlinkCycle))
                                        {
                                            canvas.DrawTri(_mini_map_track, Color.White, DIRECTION.UP);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }



                if (ShowPosition)
                {

                    DDDObjects obj = _playfield.GetSelectedObject();
                    if (obj != null)
                    {
                        if (obj.IsPathCalculatorRunning())
                        {
                            canvas.DrawFillRect(_bottom_status_bar, _mini_map_background);
                            SizeF size = obj.DrawIcon(canvas, PlayfieldRect.X, _bottom_status_bar.Y, 30, 30);

                            int font_height = (Fonts["Arial8B"].MeasureString(null, obj.FullVelocityStr, DrawTextFormat.None, Color.White)).Height;

                            Fonts["Arial8B"].DrawText(null, obj.FullVelocityStr, PlayfieldRect.X + (int)size.Width + 2, _bottom_status_bar.Y, Color.White);
                            Fonts["Arial8B"].DrawText(null, obj.FullTTDStr, PlayfieldRect.X + (int)size.Width + 2, (_bottom_status_bar.Y + font_height), Color.White);
                            Fonts["Arial8B"].DrawText(null, obj.FullDestinationStr, PlayfieldRect.X + (int)size.Width + 2, (_bottom_status_bar.Y + (2 * font_height)), Color.White);
                        }
                        else
                        {
                            SizeF size = obj.DrawIcon(canvas, PlayfieldRect.X, _bottom_status_bar.Y, 30, 30);

                            int font_height = (Fonts["Arial10B"].MeasureString(null, obj.ObjectID, DrawTextFormat.None, Color.White)).Height;
                            Fonts["Arial10B"].DrawText(null, obj.ObjectID, PlayfieldRect.X + (int)size.Width, (_bottom_status_bar.Bottom - font_height), Color.White);
                        }
                    }
                }


                int width;
                String str = String.Empty;
                if (!IsPaused)
                {
                    if (DDD_Global.Instance.IsForkReplay)
                    {
                        str = String.Format("Fork Replay {0}", Time);
                    }
                    else
                    {
                        str = Time;
                    }
                }
                else
                {
                    str = "Paused";
                }
                width = (Fonts["Arial10B"].MeasureString(null, str, DrawTextFormat.None, Color.Yellow)).Width + 5;
                Fonts["Arial10B"].DrawText(null, str, _bottom_status_bar.X + (_bottom_status_bar.Width - width), _bottom_status_bar.Y + 5, Color.Yellow);
                //if (!IsPaused)
                //{
                //    if (DDD_Global.Instance.IsForkReplay)
                //    {
                //        width = (Fonts["Arial10B"].MeasureString(null, Time, DrawTextFormat.None, Color.Yellow)).Width + 5;
                //        Fonts["Arial10B"].DrawText(null, String.Format("Fork Replay {0}",Time), _bottom_status_bar.X + (_bottom_status_bar.Width - width), _bottom_status_bar.Y + 5, Color.Yellow);
                //    }
                //    else
                //    {
                //        width = (Fonts["Arial10B"].MeasureString(null, Time, DrawTextFormat.None, Color.Yellow)).Width + 5;
                //        Fonts["Arial10B"].DrawText(null, Time, _bottom_status_bar.X + (_bottom_status_bar.Width - width), _bottom_status_bar.Y + 5, Color.Yellow);
                //    }
                //}
                //else
                //{
                //    width = (Fonts["Arial10B"].MeasureString(null, Time, DrawTextFormat.None, Color.Yellow)).Width + 5;
                //    Fonts["Arial10B"].DrawText(null, "Paused", _bottom_status_bar.X + (_bottom_status_bar.Width - width), _bottom_status_bar.Y + 5, Color.Yellow);
                //}

            }

        }

        
        public override void OnSceneCleanup(GameFramework g)
        {
            UnbindGameController();
        }
        #endregion


        public float GetMinScale()
        {
            return MinScale;
        }

        #region DDD Update Handler


        public void RemoveObject(string object_id)
        {
            if (_playfield != null)
            {
                if (_playfield.ContainsMapObject(object_id))
                {
                    //MsgWindowPanel.AddText(string.Format("RemoveObject: id = {0}", object_id));
                    _playfield.RemoveMapObject(object_id);

                }
            }
           
        }

        public void TimeTick(string time)
        {
            Time = time;
        }


        public void MoveUpdateObjects(ViewProMotionUpdate update)
        {
            
            lock (this)
            {
                if (_playfield != null)
                {
                    if (_playfield.ContainsMapObject(update.ObjectId))
                    {
                        DDDObjects obj = (DDDObjects)_playfield.GetMappableObject(update.ObjectId);

                        obj.Hide = update.HideObject;

                        if (!DDD_Global.Instance.IsConnected)
                        {
                            obj.Throttle = update.Throttle;
                            obj.MaxSpeed = 24;
                            _playfield.MoveMapObject(update.ObjectId, update.DestinationX, update.DestinationY, update.DestinationZ, ((float)(update.Throttle * obj.MaxSpeed * DDD_Global.Instance.GameSpeed)));
                        }
                        else
                        {
                            
                            obj.MaxSpeed = update.MaxSpeed;
                            obj.Throttle = update.Throttle;

                            _playfield.MoveMapObject(
                                update.ObjectId,
                                update.StartX,
                                update.StartY,
                                UTM_Mapping.VelocityToPixels(update.StartZ),
                                update.DestinationX,
                                update.DestinationY,
                                UTM_Mapping.VelocityToPixels(update.DestinationZ),
                                UTM_Mapping.VelocityToPixels((float)(update.Throttle * update.MaxSpeed * DDD_Global.Instance.GameSpeed)));

                        }
                    }
                }
            }
        }

        public void AttributeUpdateObjects(ViewProAttributeUpdate update)
        {
            lock (this)
            {
                if (_playfield != null)
                {
                    if (_playfield.ContainsMapObject(update.ObjectId))
                    {
                        DDDObjects obj = (DDDObjects)_playfield.GetMappableObject(update.ObjectId);
                        if (obj != null)
                        {
                            obj.CurrentIcon = update.IconName;
                            if (update.IconName.Length > 0)
                            {
                                if (Sprites.ContainsKey(update.IconName))
                                {
                                    obj.SetSprite(Sprites[update.IconName]);
                                    obj.DrawWithRotation = GameFramework.Instance().GetTexture(update.IconName).rotate;
                                }
                                else
                                {
                                    string unknown = string.Format("{0}.Unknown.png", DDD_Global.Instance.ImageLibrary);
                                    obj.SetSprite(Sprites[unknown]);
                                    obj.DrawWithRotation = GameFramework.Instance().GetTexture(unknown).rotate;
                                }

                            }
                            if (update.Throttle > 0)
                            {
                                obj.Throttle = update.Throttle;
                            }
                            if (update.CapabilityAndWeapons != null)
                            {
                                obj.CapabilityAndWeapons = update.CapabilityAndWeapons;
                            }
                            if (update.SubPlatforms != null)
                            {
                                obj.SubPlatforms = update.SubPlatforms;
                            }
                            if (update.Vulnerabilities != null)
                            {
                                obj.Vulnerabilities = update.Vulnerabilities;
                            }
                            if (update.FuelAmount >= 0)
                            {
                                obj.FuelAmount = (float)update.FuelAmount;
                            }
                            if (update.FuelCapacity >= 0)
                            {
                                obj.FuelCapacity = (float)update.FuelCapacity;
                            }
                            obj.PlayerID = update.PlayerId;
                            obj.MaxSpeed = update.MaxSpeed;

                            if (update.ClassName != null)
                            {
                                if (update.ClassName.Length > 0)
                                {
                                    obj.ClassName = update.ClassName;
                                }
                            }
                            if (update.Classification != null)
                            {
                                //if (update.Classification.Length > 0)
                                //{
                                    obj.Classification = update.Classification;
                                //}
                            }
                            if (update.State.Length > 0)
                            {
                                obj.State = update.State;
                            }
                            if (update.ParentId != null)
                            {
                                obj.ParentID = update.ParentId;
                            }
                            else
                            {
                                obj.ParentID = string.Empty;
                            }

                            if (obj.OwnerID != update.OwnerId)
                            {
                                obj.OwnerID = update.OwnerId;
                                _playfield.SwitchOwnership(obj.ObjectID);
                            }

                            if (update.CustomAttributes != null)
                            {
                                obj.UpdateCustomAttributes(update.CustomAttributes);
                            }

                            if (update.LocationX != 0 && update.LocationY != 0 && update.LocationZ != 0)
                            {
                                //obj.SetPosition(float.Parse(update.LocationX.ToString()), float.Parse(update.LocationY.ToString()), float.Parse(update.LocationZ.ToString()));
                                obj.Altitude = (float)update.LocationZ;
                                obj.SetPosition(obj.Position.X, obj.Position.Y, (float)update.LocationZ);//Might be issues with setting position > 0
                            }

                            if (update.Tag != null)
                            {
                                obj.Tag = update.Tag;
                            }

                            if (update.Sensors != null)
                            {
                                obj.Sensors = update.Sensors;
                            }

                            bool hasAnyRings = false;
                            if (update.SensorRangeRings != null)
                            {
                                hasAnyRings = true;
                                try
                                {
                                    obj.SetSensorRangeRings(update.SensorRangeRings);
                                }
                                catch (Exception ex)
                                { }
                            }
                            if (update.CapabilityRangeRings != null)
                            {
                                hasAnyRings = true;
                                try
                                {
                                    obj.SetCapabilityRangeRings(update.CapabilityRangeRings);
                                }
                                catch (Exception ex)
                                { }
                                
                            }
                            if (update.VulnerabilityRangeRings != null)
                            {
                                hasAnyRings = true;
                                try
                                {
                                    obj.SetVulnerabilityRangeRings(update.VulnerabilityRangeRings);
                                    Console.WriteLine("Vulnerability rings received: {0}", update.VulnerabilityRangeRings.ToString());
                                }
                                catch (Exception ex)
                                { }
                               
                            }
                            else
                            {
                                Console.WriteLine("No vulnerability rings received");
                            }                            
                        }
                    }
                }
            }
        }

        public void ViewProStopObjectUpdate(string objectID)
        {
            if (_playfield != null)
            {
                if (_playfield.ContainsMapObject(objectID))
                {
                    _playfield.GetMappableObject(objectID).StopPathCalculator();
                }
            }
        }

        public void TextChatRequest(string user_id, string message, string target_id)
        {
            if (!DDD_Global.Instance.IsForkReplay)
            {
                _commands.TextChatRequest(DDD_Global.Instance.PlayerID, message, string.Empty, string.Empty);
            }
        }

        public void PauseGame()
        {
            if (_playfield != null)
            {
                IsPaused = true;
                _playfield.Pause();
            }
        }

        public void ResumeGame()
        {
            if (_playfield != null)
            {
                IsPaused = false;
                _playfield.Resume();
            }
        }

        public void AttackUpdate(string attacker, string target, int start_time, int end_time)
        {
            try
            {
                lock (this)
                {
                    if (_playfield != null)
                    {
                        DDDObjects att = (DDDObjects)_playfield.GetMappableObject(attacker);
                        DDDObjects obj = (DDDObjects)_playfield.GetMappableObject(target);
                        if (obj != null)
                        {
                            if ((end_time - start_time) > 0)
                            {
                                obj.IsBeingAttacked = true;
                                obj.EngagementTimer = (end_time - start_time) / 1000;
                                if (att != null)
                                {
                                    att.EngagementTimer = obj.EngagementTimer;
                                    att.IsAttacking = true;
                                    obj.Attackers.Add(att);
                                }
                            }
                            else
                            {
                                obj.IsBeingAttacked = false;
                                obj.EngagementTimer = 0;
                                obj.Attackers.Clear();
                                if (att != null)
                                {
                                    att.IsAttacking = false;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("AttackUpdate" + e.Message);
            }
        }

        public void TransferObjectRequest(string playerID, string objectID, string newOwnerID, string objectState)
        {
            if (!DDD_Global.Instance.IsObserver || !DDD_Global.Instance.IsForkReplay)
            {
                _commands.TransferObjectRequest(playerID, objectID, newOwnerID, objectState);
            }
        }

        public void DockObjectRequest(string playerID, string objectID, string parentObjectID, bool dockingToOther)
        {
            if (!DDD_Global.Instance.IsObserver || !DDD_Global.Instance.IsForkReplay)
            {
                _commands.DockObjectRequest(playerID, objectID, parentObjectID, dockingToOther);
            }
        }

        public void ChangeTagRequest(string player_id, string objectID, string tag)
        {
            if (!DDD_Global.Instance.IsObserver || !DDD_Global.Instance.IsForkReplay)
            {
                _commands.ChangeTagRequest(player_id, objectID, tag);
            }
        }


        #endregion

        public void DarkenMap()
        {
            if (Map != null)
            {
                if ((Map.Diffuse.R - ColorStep) >= 0)
                {
                    Map.Diffuse = Color.FromArgb(Map.Diffuse.R - ColorStep, Map.Diffuse.G - ColorStep, Map.Diffuse.B - ColorStep);
                }
            }
        }
        public void LightenMap()
        {
            if (Map != null)
            {
                if ((Map.Diffuse.R + ColorStep) <= 255)
                {
                    Map.Diffuse = Color.FromArgb(Map.Diffuse.R + ColorStep, Map.Diffuse.G + ColorStep, Map.Diffuse.B + ColorStep);
                }
            }
        }
        #region Keyboard And Mouse Handlers



        public override void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((Map != null) && (_playfield != null))
            {
                if (_mini_map.Contains(e.Location) && (e.Button == MouseButtons.Left))
                {
                    // Handle Mouse click for mini-map
                    int posx = e.X - _mini_map.X;
                    int posy = e.Y - _mini_map.Y;
                    int scaledx = (int)(((float)posx / (float)_mini_map.Width) * ((float)Map.TextureWidth));
                    int scaledy = (int)(((float)posy / (float)_mini_map.Height) * ((float)Map.TextureHeight));
                    CenterMapToUnit(scaledx * _playfield.Scale, scaledy * _playfield.Scale);
                }
            }
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_playfield != null)
            {
                if (!_mini_map_thumb.Contains(e.Location) ||
                    (_mini_map_thumb.Contains(e.Location) && (e.Button == MouseButtons.Right)))
                {
                    if ((WBRoom != null) && (WhiteboardDrawing))
                    {
                        // Handle whiteboard mouse event
                        if (DrawDistanceLine)
                        {
                            if (((WBRoom.DrawMode == DrawModes.Line) || (WBRoom.DrawMode == DrawModes.Circle) ||
                                (WBRoom.DrawMode == DrawModes.Arrow))
                                && (WhiteboardDrawing == true))
                            {
                                LocationValue startLocation = new LocationValue();
                                LocationValue endLocation = new LocationValue();
                                double drawPointSize = WBRoom.DrawPointSize / _playfield.Scale;

                                startLocation.X = (LineStartLocation.X - Map.Position.X) / _playfield.Scale;
                                startLocation.Y = (LineStartLocation.Y - Map.Position.Y) / _playfield.Scale;
                                startLocation.exists = true;

                                endLocation.X = (LineEndLocation.X - Map.Position.X) / _playfield.Scale;
                                endLocation.Y = (LineEndLocation.Y - Map.Position.Y) / _playfield.Scale;
                                endLocation.exists = true;

                                if ((WBRoom.DrawMode == DrawModes.Line) && (WBRoom.DrawText != null) &&
                                    (WBRoom.DrawText.Length > 0))
                                {
                                    WBRoom.Controller.WhiteboardLineRequest(DDD_Global.Instance.PlayerID, (int)WBRoom.DrawMode,
                                        startLocation, endLocation, drawPointSize, _playfield.Scale, WBRoom.DrawColor, WBRoom.DrawText, WBRoom.Name);
                                }
                                else
                                {
                                    WBRoom.Controller.WhiteboardLineRequest(DDD_Global.Instance.PlayerID, (int)WBRoom.DrawMode,
                                        startLocation, endLocation, drawPointSize, _playfield.Scale, WBRoom.DrawColor, "", WBRoom.Name);
                                }

                                WhiteboardDrawing = false;
                            }
                            else if ((WBRoom.DrawMode == DrawModes.Text) && (WBRoom.DrawText != null) &&
                                (WBRoom.DrawText.Length > 0) && (WhiteboardDrawing == true))
                            {
                                LocationValue startLocation = new LocationValue();
                                LocationValue endLocation = new LocationValue();
                                double drawPointSize = (WBRoom.DrawPointSize + 7) / _playfield.Scale;

                                endLocation.X = (LineEndLocation.X - Map.Position.X) / _playfield.Scale;
                                endLocation.Y = (LineEndLocation.Y - Map.Position.Y) / _playfield.Scale;
                                endLocation.exists = true;

                                WBRoom.Controller.WhiteboardLineRequest(DDD_Global.Instance.PlayerID, (int)WBRoom.DrawMode,
                                    endLocation, endLocation, drawPointSize, _playfield.Scale, WBRoom.DrawColor, WBRoom.DrawText, WBRoom.Name);

                                WhiteboardDrawing = false;
                            }
                            else if ((WBRoom.DrawMode == DrawModes.Selection) && (WhiteboardDrawing == true))
                            {
                                RectangleF selectionRect;
                                float selectionX = (float)(Math.Min(LineStartLocation.X - Map.Position.X, LineEndLocation.X - Map.Position.X)) / _playfield.Scale;
                                float selectionY = (float)(Math.Min(LineStartLocation.Y - Map.Position.Y, LineEndLocation.Y - Map.Position.Y)) / _playfield.Scale;

                                selectionRect = new RectangleF(selectionX, selectionY,
                                    ((float)Math.Abs(LineStartLocation.X - LineEndLocation.X)) / _playfield.Scale,
                                    ((float)Math.Abs(LineStartLocation.Y - LineEndLocation.Y)) / _playfield.Scale);

                                // Clear the selection list
                                WBRoom.ClearSelectionList();

                                // Add contained objects to selection list
                                WBRoom.AddSelection(selectionRect, true);

                                if (!WBRoom.ObjectsSelected())
                                {
                                    _playfield.OnMouseUp(sender, e);
                                }

                                WhiteboardDrawing = false;
                            }
                            DrawDistanceLine = false;
                        }
                        if ((WBRoom.DrawMode == DrawModes.Selection) && (WhiteboardDrawing == true))
                        {
                            RectangleF selectionRect;

                            float selectionX = (float)(LineStartLocation.X - Map.Position.X) / _playfield.Scale;
                            float selectionY = (float)(LineStartLocation.Y - Map.Position.Y) / _playfield.Scale;

                            selectionRect = new RectangleF(selectionX, selectionY, 1.0f, 1.0f);

                            // Add contained objects to selection list
                            WBRoom.AddSelection(selectionRect, false);

                            if (!WBRoom.ObjectsSelected())
                            {
                                _playfield.OnMouseUp(sender, e);
                            }

                            WhiteboardDrawing = false;
                        }
                        WhiteboardDrawing = false;
                    }
                    else
                    {
                        _playfield.OnMouseUp(sender, e);
                    }
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        _mini_map_scroll = false;
                    }
                }
            }
        }
        

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            WhiteboardDrawing = false;
            if (_playfield != null)
            {
                // Give this control focus
                _game_control.GetTargetControl().Focus();

                if (!_mini_map_thumb.Contains(e.Location))
                {
                    if ((WBRoom != null) && ((WBRoom.DrawMode != DrawModes.Selection) ||
                        !(_playfield.SelectionExists(e.X, e.Y))))
                    {
                        // Handle whiteboard mouse event
                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                WhiteboardDrawing = true;
                                LineStartLocation = LineEndLocation = e.Location;
                                WBRoom.Controller.BeginWhiteboardLineRequest(DDD_Global.Instance.PlayerID, WBRoom.Name);
                                break;
                        }

                    }
                    else
                    {
                        _playfield.OnMouseDown(sender, e);
                    }
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        _mini_map_scroll = true;
                    }
                }
            }
        }
        
        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if ((Map != null) && (_playfield != null))
            {
                if (!_mini_map.Contains(e.Location))
                {
                    if ((WBRoom != null) && (WhiteboardDrawing))
                    {
                        // Handle whiteboard mouse event
                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                LineEndLocation = e.Location;
                                if ((Math.Abs(LineEndLocation.X - LineStartLocation.X) > 5) || (Math.Abs(LineEndLocation.Y - LineStartLocation.Y) > 5))
                                {
                                    DrawDistanceLine = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        _mini_map_scroll = false;
                        _playfield.OnMouseMove(sender, e);
                    }
                }
                else
                {
                    if (_mini_map_scroll && (e.Button == MouseButtons.Left))
                    {
                        int posx = e.X - _mini_map.X;
                        int posy = e.Y - _mini_map.Y;
                        int scaledx = (int)(((float)posx / (float)_mini_map.Width) * ((float)Map.TextureWidth));
                        int scaledy = (int)(((float)posy / (float)_mini_map.Height) * ((float)Map.TextureHeight));
                        CenterMapToUnit(scaledx * _playfield.Scale, scaledy * _playfield.Scale);
                    }
                }
            }
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (WBRoom != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    WBRoom.DeleteSelectionObjects();
                }
            }

            base.OnKeyDown(sender, e);
        }

        #endregion



        #region HeadsUpDisplay Support 
        /// <summary>
        /// Centers the currently displayed map around an X,Y position.
        /// Used when selecting assets from the Asset Menu ... Map recenters
        /// itself to the asset's location.
        /// </summary>
        /// <param name="obj_x"></param>
        /// <param name="obj_y"></param>
        public void CenterMapToUnit(float obj_x, float obj_y)
        {
            if (_playfield != null)
            {
                Rectangle PlayfieldRect = _playfield.ClientArea;

                float new_x = (float)((obj_x - (PlayfieldRect.Width * .5)));
                float new_y = (float)((obj_y - (PlayfieldRect.Height * .5)));

                float max_extent_Y = (Map.TextureHeight * _playfield.Scale) - PlayfieldRect.Height;
                float max_extent_X = (Map.TextureWidth * _playfield.Scale) - PlayfieldRect.Width;
                float min_extent_Y = PlayfieldRect.Y;
                float min_extent_X = PlayfieldRect.X;

                if (new_x >= max_extent_X)
                {
                    new_x = max_extent_X;
                }
                if (new_x <= min_extent_X)
                {
                    new_x = 0;
                }
                if (new_y >= max_extent_Y)
                {
                    new_y = max_extent_Y;
                }
                if (new_y <= min_extent_Y)
                {
                    new_y = 0;
                }

                _playfield.SetMapPosition(PlayfieldRect.X, PlayfieldRect.Y);
                _playfield.Pan(-new_x, -new_y);
            }
        }
       
        #endregion

        public void SendMapUpdate(bool sendNoMatterWhat)
        {
            if (_playfield != null)
            {
                _playfield.SendMapPosition(sendNoMatterWhat);
            }
        }

        public void SendWeaponSelectedUpdate(string userID, string parentObjectID, string weaponName, bool isWeapon)
        {
            _commands.DoSendWeaponSelectedUpdate(userID, parentObjectID, weaponName, isWeapon);
        }

        public void SendObjectSelectedUpdate(string userID, string objectID, string ownerID)
        {
            _commands.DoSendObjectSelectedUpdate(userID, objectID, ownerID);
        }

        public void SendTabSelectionUpdate(string userId, string objectID, string tabName)
        {
            _commands.DoSendTabSelectionUpdate(userId, objectID, tabName);
        }

    }
}
