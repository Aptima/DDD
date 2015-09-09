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

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;




namespace Aptima.Asim.DDD.Client
{

    public enum MapModes : int { MOVE = 0, ATTACK = 1, SUBPLATFORM = 2, DRAW = 3 }
    

    partial class MapPlayfield: MapScene
    {

        public MapModes Mode = MapModes.MOVE;
        public bool WinForm_Mode = false;

        #region Private Members
        
        private ICommand Command;
        private bool _pause;

        private Point LineStartLocation;
        private Point LineEndLocation;
        private bool DrawDistanceLine = false;

        private Point MouseDownLocation;


        private Microsoft.DirectX.Direct3D.Font font_large;
        private Microsoft.DirectX.Direct3D.Font font_small;

        private Vector2 _lastScreenPosition = new Vector2(-1, -1);
        private Vector2 _lastScreenSize = new Vector2(-1, -1);
        private Vector2 _metersPerPixelValues = new Vector2();
        private float _lastZoom = 0.0F;

        #endregion


        public MapPlayfield(Canvas canvas, Obj_Sprite map, Rectangle client_area, ICommand command)
        {
            ViewCamera = new Camera();
            Map = map;
            _pause = false;

            _client_area = client_area;
            Command = command;

            EnterMoveMode();
            MinimumScale();
        }
        public MapPlayfield(GameTexture map_texture, Rectangle client_area, ICommand command)
        {
            int width = 0;
            int height = 0;
            Map = new Obj_Sprite(SpriteFlags.None);
            Map.Texture(map_texture.texture, map_texture.width, map_texture.height);
            _client_area = client_area;
            Command = command;

            EnterMoveMode();
            MinimumScale();


        }

        public void SetScale(float scale) {
            Map.SetScale(scale, scale, 0);
        }





        #region ModeHandlers
        public void EnterAttackMode()
        {
            Mode = MapModes.ATTACK;
        }
        public void EnterMoveMode()
        {
            Mode = MapModes.MOVE;
            if (SelectionCount() > 1)
            {
                RemoveSelection(1, SelectionCount() - 1);
            }
        }
        public void EnterSubPlatformMode()
        {
            Mode = MapModes.SUBPLATFORM;
        }
        public void EnterDrawMode()
        {
            Mode = MapModes.DRAW;
        }
        #endregion


        #region Map Logic

        public void WinForm_AttackSelected()
        {
            if (!_pause)
            {
                DDDObjects obj = GetSelectedObject();
                DDDObjects target = GetSelectedTarget();
                bool attackSent = false;

                if (obj != null && target != null)
                {
                    if (obj.CapabilityAndWeapons.Length > 0)
                    {
                        if ((obj.CurrentCapabilityAndWeapon >= 0) &&
                            (obj.CurrentCapabilityAndWeapon < obj.CapabilityAndWeapons.Length))
                        {
                            if (!obj.IsWeapon)
                            {
                                if (!DDD_Global.Instance.IsObserver && !DDD_Global.Instance.IsForkReplay)
                                {
                                    Command.DoAttack(DDD_Global.Instance.PlayerID,
                                        obj.ObjectID,
                                        target.ObjectID,
                                        obj.CapabilityAndWeapons[obj.CurrentCapabilityAndWeapon]);
                                    attackSent = true;
                                }
                            }
                        }

                        _select_buffer.Remove(target.ObjectID);
                    }
                }
                if (!attackSent)
                {
                    EnterMoveMode();
                }
                Command.SelectionUpdate();
            }
        }


        public void WinForm_DockToSelected()
        {
            if (!_pause)
            {
                DDDObjects obj = GetSelectedObject();
                DDDObjects target = GetSelectedTarget();

                if (obj != null && target != null)
                {
                    if (!DDD_Global.Instance.IsObserver && !DDD_Global.Instance.IsForkReplay)
                    {
                        Command.SubPlatformDock(obj.ObjectID, target.ObjectID);
                    }
                    //if (obj.CapabilityAndWeapons.Length > 0)
                    //{
                    //    if ((obj.CurrentCapabilityAndWeapon >= 0) &&
                    //        (obj.CurrentCapabilityAndWeapon < obj.CapabilityAndWeapons.Length))
                    //    {
                    //        if (!obj.IsWeapon)
                    //        {
                    //            Command.DoAttack(DDD_Global.Instance.PlayerID,
                    //                obj.ObjectID,
                    //                target.ObjectID,
                    //                obj.CapabilityAndWeapons[obj.CurrentCapabilityAndWeapon]);
                    //        }
                    //    }

                    //    _select_buffer.Remove(target.ObjectID);
                    //}
                }

                EnterMoveMode();
                Command.SelectionUpdate();
            }
        }

        public void WinForm_LaunchSubPlatform(float xpos, float ypos)
        {
            if (!_pause)
            {
                DDDObjects obj = GetSelectedObject();
                if (obj != null)
                {
                    xpos = (xpos - Map.Position.X) / Scale;
                    ypos = (ypos - Map.Position.Y) / Scale;

                    if (obj.SubPlatforms != null)
                    {
                        if (obj.SubPlatforms.Length > 0)
                        {
                            if ((obj.CurrentSubplatform >= 0) && (obj.CurrentSubplatform < obj.SubPlatforms.Length))
                            {
                                if (!obj.IsWeapon)
                                {
                                    if (!DDD_Global.Instance.IsObserver && !DDD_Global.Instance.IsForkReplay)
                                    {
                                        double parentAltitude = 0.0;
                                        parentAltitude = obj.Altitude;
                                        Command.SubPlatformLaunch(obj.SubPlatforms[obj.CurrentSubplatform], obj.ObjectID, xpos, ypos, parentAltitude);
                                    }
                                }
                            }
                        }
                    }
                }

                
                Mode = MapModes.MOVE;
                Command.SelectionUpdate();
            }
        }


        public void MoveSelected(float xpos, float ypos)
        {
            if (!_pause)
            {
                DDDObjects obj = GetSelectedObject();

                if (obj != null)
                {
                    xpos = (xpos - Map.Position.X) / Scale;
                    ypos = (ypos - Map.Position.Y) / Scale;

                    try
                    {
                        if (!obj.IsWeapon)
                        {
                            double altitude = obj.Altitude;
                            if (!DDD_Global.Instance.IsObserver && !DDD_Global.Instance.IsForkReplay)
                            {
                                Command.DoMove(DDD_Global.Instance.PlayerID, obj.ObjectID, obj.ThrottleSlider, xpos, ypos, altitude);
                            }
                            
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        public void WinForm_StartWBDrawMode()
        {
            if (!_pause)
            {
                EnterDrawMode();
                Command.SelectionUpdate();
            }
        }

        public void WinForm_EndWBDrawMode()
        {
            if (!_pause)
            {
                EnterMoveMode();
                Command.SelectionUpdate();
            }
        }

        #endregion

        public DDDObjects GetSelectedObject()
        {
            return (DDDObjects)GetSelection(0);
        }
        public DDDObjects GetSelectedTarget()
        {
            return (DDDObjects)GetSelection(1);
        }

        public void SetMapPosition(int x, int y)
        {
                Map.SetPosition(x, y, 0);
        }

        public void Pause()
        {
            _pause = true;
            lock (this)
            {
                foreach (MappableObject obj in _playfield_objects.Values)
                {
                    if (obj.IsPathCalculatorRunning())
                    {
                        obj.StopPathCalculator();
                    }
                }
            }
        }
        public void Resume()
        {
            _pause = false;
        }
        
        #region Scene Events
        public float GetMinimumScale()
        {
            //Rectangle q = Rectangle.Empty;
            //bool do_loop = true;

            //if ((Map.TextureWidth <= ClientArea.Width) || (Map.TextureHeight <= ClientArea.Height))
            //{
            //    return Scale;
            //}

            //for (int i = 0; i < _zoom_levels.Length; i++)
            //{
            //    q.X = ClientArea.X;
            //    q.Y = ClientArea.Y;
            //    q.Width = (int)(Map.TextureWidth * _zoom_levels[i]);
            //    q.Height = (int)(Map.TextureHeight * _zoom_levels[i]);

            //    if (q.Contains(ClientArea))
            //    {
            //        if (i > 0)
            //        {
            //            return _zoom_levels[i];
            //        }
            //    }
            //}
            //return _zoom_levels[0];     
            float f = ((ClientArea.Width) / (Map.TextureWidth));
            float q = ((ClientArea.Height) / (Map.TextureHeight));
            float ret = 0;
            if (f < q)
            {
                Console.WriteLine("return f = {0}, {1}", ClientArea.Width, ClientArea.Height);
                ret = f - .001f;
                if (ret < 1)
                {
                    return ret;
                }
                
            }
            Console.WriteLine("return q = {0}, {1}", ClientArea.Width, ClientArea.Height);
            ret = q - .001f;
            if (ret < 1)
            {
                return ret;
            }
            return 1;
        }


        public void MinimumScale()
        {
            Rectangle q = Rectangle.Empty;
            bool do_loop = true;

            if ((Map.TextureWidth <= ClientArea.Width) || (Map.TextureHeight <= ClientArea.Height))
            {
                Zoom(_zoom_levels.Length - 1);
                Map.SetScale(Scale, Scale, 0);
                return;
            }

            for (int i = 0; i < _zoom_levels.Length; i++)
            {
                q.X = ClientArea.X;
                q.Y = ClientArea.Y;
                q.Width = (int)(Map.TextureWidth * _zoom_levels[i]);
                q.Height = (int)(Map.TextureHeight * _zoom_levels[i]);

                if (q.Contains(ClientArea))
                {
                    if (i > 0)
                    {
                        Zoom(i);
                        Map.SetScale(Scale, Scale, 0);
                        return;
                    }
                }
                Zoom(0);
                Map.SetScale(Scale, Scale, 0);
            }
        }

        public override bool OnZoom()
        {
            //DeselectAll();
            if ((Map.TextureWidth <= ClientArea.Width) || (Map.TextureHeight <= ClientArea.Height))
            {
                // Map is contained, return true.  Return false only when not contained.
                return true;
            }

            Rectangle q = Rectangle.Empty;

            q.X = ClientArea.X;
            q.Y = ClientArea.Y;
            q.Width = (int)(Map.TextureWidth * Scale);
            q.Height = (int)(Map.TextureHeight *Scale);

            if (q.Contains(ClientArea))
            {
                Map.SetScale(Scale, Scale, 0);

                if (!Map.ToRectangle().Contains(ClientArea))
                {
                    // Adjust position so not to fall off edge of map
                    if (Map.ToRectangle().Right < ClientArea.Right)
                    {
                        Pan(ClientArea.Right - Map.ToRectangle().Right, 0);
                    }
                    if (Map.ToRectangle().Bottom < ClientArea.Bottom)
                    {
                        Pan(0, ClientArea.Bottom - Map.ToRectangle().Bottom);
                    }
                }
                return true;
            }

            return false;
        }
        protected override void OnPan(float xpos, float ypos)
        {
            Map.SetPosition(Map.Position.X + xpos,  Map.Position.Y + ypos, 0);
        }
        
        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!ClientArea.Contains(e.X, e.Y))
            {
                return;
            }
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!DrawDistanceLine)
                    {
                        Mode = MapModes.MOVE;
                        DDDObjects previous = this.GetSelectedObject();
                        if (!SelectSingle(e.X, e.Y))
                        {
                            DeselectAll();
                            DDD_Global.Instance.RangeFinderDistanceString = "";
                        }
                        DDDObjects current = GetSelectedObject();

                        if ((current == null) && (previous == null))
                        {
                            return;
                        }
                        if ((current != null) && (previous != null))
                        {
                            if (previous.ObjectID.CompareTo(current.ObjectID) == 0)
                            {
                                return;
                            }
                        }
                        DDD_Global.Instance.RangeFinderDistanceString = " ";
                        Command.SelectionUpdate();
                    }
                    break;
                case MouseButtons.Right:
                    switch (Mode)
                    {
                        case MapModes.MOVE:
                               MoveSelected(e.X, e.Y);
                            break;
                        case MapModes.ATTACK:
                            if (RightClickSelect(e.X, e.Y))
                            {
                                WinForm_AttackSelected();
                            }
                            break;
                        case MapModes.SUBPLATFORM:
                            if (!RightClickSelect(e.X, e.Y))
                            {
                                WinForm_LaunchSubPlatform(e.X, e.Y);
                            }
                            else
                            {
                                // Dock to selected subplatform
                                WinForm_DockToSelected();
                            }
                            break;
                    }

                    break;
            }
        }
        public override void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
               ZoomOut();
            }
            else
            {
                ZoomIn();
            }
        }
        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    LineStartLocation = LineEndLocation = e.Location;
                    break;

                case MouseButtons.Middle:
                    MouseDownLocation = e.Location;
                    break;
            }
        }
        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                   
                    if ((Control.ModifierKeys & Keys.Alt) != 0)
                    {
                        LineEndLocation = e.Location;
                        if ((Math.Abs(LineEndLocation.X - LineStartLocation.X) > 5) || (Math.Abs(LineEndLocation.Y - LineStartLocation.Y) > 5))
                        {
                            DrawDistanceLine = true;
                            this.DeselectAll();
                        }
                    }
                    break;
                case MouseButtons.Middle:
                    Rectangle PlayfieldRect = _client_area;
                    Rectangle SpriteRect = Map.ToRectangle();

                    int distanceX = (e.X - MouseDownLocation.X);
                    SpriteRect.X += distanceX;

                    int distanceY = (e.Y - MouseDownLocation.Y);
                    SpriteRect.Y += distanceY;

                    if (SpriteRect.Contains(PlayfieldRect))
                    {
                        Pan(distanceX, distanceY);
                        MouseDownLocation = e.Location;
                    }

                    break;
            }

          
            
            float xMap = UTM_Mapping.HorizontalPixelsToMeters((e.X - Map.Position.X) / Scale);
            float yMap = UTM_Mapping.HorizontalPixelsToMeters(Map.TextureHeight - (e.Y - Map.Position.Y) / Scale);

            
            DDD_Global.Instance.RangeFinderX = xMap; 
            DDD_Global.Instance.RangeFinderY = yMap;
            DDD_Global.Instance.RangeFinderXDisplay = e.X + 10;
            DDD_Global.Instance.RangeFinderYDisplay = e.Y + 20;
        }

        /// <summary>
        /// Sends map's current position info onto network bus.  If this is passed True for overriden, this will ALWAYS
        /// send the map position.  Otherwise, it will only send it if the position has changed.
        /// </summary>
        /// <param name="overridden"></param>
        public void SendMapPosition(bool overridden)
        {
            bool valuesChanged = false;

            if (_lastScreenPosition.X == -1 || _lastScreenPosition.Y == -1)
            {
                _lastScreenPosition = new Vector2();
                _metersPerPixelValues = new Vector2();
                _lastZoom = 0.0F;
                valuesChanged = true;
                _metersPerPixelValues.X = UTM_Mapping.HorizonalMetersPerPixel;
                _metersPerPixelValues.Y = UTM_Mapping.VerticalMetersPerPixel;
            }

            if (Map.Position.X != _lastScreenPosition.X || Map.Position.Y != _lastScreenPosition.Y || _lastZoom != Scale ||
                ClientArea.Width != _lastScreenSize.X || ClientArea.Height != _lastScreenSize.Y)
            {
                valuesChanged = true;
            }

            if (valuesChanged || overridden)
            {
                _lastScreenPosition.X = Map.Position.X;
                _lastScreenPosition.Y = Map.Position.Y;
                _lastScreenSize.X = ClientArea.Width;
                _lastScreenSize.Y = ClientArea.Height;
                _lastZoom = Scale;

                int originX = Convert.ToInt32(Map.Position.X * _metersPerPixelValues.X / Scale);
                int originY = Convert.ToInt32(Map.Position.Y * _metersPerPixelValues.Y / Scale);
                int screenSizeX = Convert.ToInt32(ClientArea.Width * _metersPerPixelValues.X / Scale);
                int screenSizeY = Convert.ToInt32(ClientArea.Height * _metersPerPixelValues.Y / Scale);


                Command.DoClientScreenUpdate(Math.Abs(originX), Math.Abs(originY), screenSizeX, screenSizeY, _lastZoom, DDD_Global.Instance.PlayerID);
            }
        }
        #endregion



    }
}
