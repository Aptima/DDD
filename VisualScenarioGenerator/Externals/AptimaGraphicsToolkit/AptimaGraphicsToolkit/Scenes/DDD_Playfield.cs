using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;
using AGT.Motion;
using AGT.Forms;
using AGT.Mapping;
using AGT.GameToolkit;
using AGT.GameFramework;

namespace AGT.Scenes
{
    public enum ScrollStateEnum : int { ScrollUp, ScrollDown, ScrollLeft, ScrollRight, NoScroll };
    public enum PlayfieldModeType : int { Select, Move, Engage, Undock, Waypoint, Zone};

    public delegate void MouseSelectionHandler(string id);

    public class DDD_Playfield: AGT_Scene, IAGT_SplashDialog, IAGT_SceneLoadDialog
    {
        #region Scene Options
        public bool UnitTestMode= false;
        public string ImageLibraryPath = string.Empty;
        public System.Drawing.Bitmap Map = null;
        public string MapFile = string.Empty;

        public MouseSelectionHandler OnMouseSelection = null;

        public bool SimulateMotion = true;
        public bool UserControl = true;

        private bool _draw_waypoints = false;
        public bool DrawWaypoints
        {
            get
            {
                lock (this)
                {
                    return _draw_waypoints;
                }
            }
            set
            {
                lock (this)
                {
                    _draw_waypoints = value;
                }
            }
        }
        
        private AGT_Heading _heading;
        public HeadingStyle Style
        {
            get
            {
                lock (this)
                {
                    return _heading.Style;
                }
            }
            set
            {
                lock (this)
                {
                    _heading.Style = value;
                }
            }
        }

        private bool _mini_map = true;
        public bool MiniMap
        {
            get
            {
                lock (this)
                {
                    return _mini_map;
                }
            }
            set
            {
                lock (this)
                {
                    _mini_map = value;
                }
            }
        }

        private bool _pause = false;
        public bool Pause
        {
            get
            {
                lock (this)
                {
                    return _pause;
                }
            }
            set
            {
                lock (this)
                {
                    _pause = value;
                }
            }
        }

        private AGT_Pawn _primary_pawn_selection;
        private List<string> _target_selection;
        public AGT_Pawn CurrentPawnSelection
        {
            set
            {
                lock (this)
                {
                    _primary_pawn_selection = value;
                }
            }
        }

        private AGT_PointList _active_zone_selection = AGT_PointList.Empty;
        public AGT_PointList CurrentActiveZone
        {
            set
            {
                lock (this)
                {
                    _active_zone_selection = value;
                }
            }
        }

        private AGT_PointList _waypoint_group_selection = AGT_PointList.Empty;
        public AGT_PointList CurrentWaypointGroup
        {
            set
            {
                lock (this)
                {
                    _waypoint_group_selection = value;
                }
            }
        }

        public int Scroll_Increment = 10;
        public ScrollStateEnum ScrollState = ScrollStateEnum.NoScroll;
        
        public float PlayfieldScale = 1.0f;
        public int MiniMap_Width = 150;

        private bool _show_minimap = true;
        private Rectangle _minmap_rect = Rectangle.Empty;
        private Rectangle _minmap_thumb = Rectangle.Empty;
        private Rectangle _minmap_blip = Rectangle.Empty;
        #endregion


        private PlayfieldModeType Mode = PlayfieldModeType.Select;
        private Rectangle _collision_rect = Rectangle.Empty;

        private Microsoft.DirectX.Direct3D.Font _pawn_font = null;

        private AGT_SpriteManager _map_sprites = null;
        private AGT_SpriteManager _pawn_sprites = null;
        private AGT_SpriteManager _active_zone_sprites = null;
        private AGT_SpriteManager _waypoint_group_sprites = null;

        private AGT_Label _label = null;

        private LinkedList<AGT_Pawn> _pawn_stack = new LinkedList<AGT_Pawn>();
        private LinkedList<AGT_PointList> _active_zone_stack = new LinkedList<AGT_PointList>();
        private LinkedList<AGT_PointList> _waypoint_group_stack = new LinkedList<AGT_PointList>();
        
        private Dictionary<string, AGT_Text> _label_dictionary = new Dictionary<string, AGT_Text>();
        private Dictionary<string, AGT_LinearMotion> _motion_calculators = new Dictionary<string, AGT_LinearMotion>();

        private Rectangle _playfield_bounds = Rectangle.Empty;
        private AGT_SpriteId _playfield_image_id = AGT_SpriteId.Empty;

        private List<Point> _mouse_points = new List<Point>();
        private List<Vector2> _mouse_points_v2 = new List<Vector2>();

        private bool _dragging_minmap = false;


        public DDD_Playfield()
        {
            _primary_pawn_selection = new AGT_Pawn();
            _minmap_blip.Width = _minmap_blip.Height = 4;
            ShowMouseCursor = true;
        }


        public void ChangeMode()
        {
            if (CursorImage == GameFramework.SystemImages.Cursor_Select)
            {
                Mode = PlayfieldModeType.Move;
                CursorImage = GameFramework.SystemImages.Cursor_Move;
                return;
            }

            if (CursorImage == GameFramework.SystemImages.Cursor_Move)
            {
                Mode = PlayfieldModeType.Engage;
                CursorImage = GameFramework.SystemImages.Cursor_Engage;
                return;
            }

            if (CursorImage == GameFramework.SystemImages.Cursor_Engage)
            {
                Mode = PlayfieldModeType.Undock;
                CursorImage = GameFramework.SystemImages.Cursor_Undock;
                return;
            }

            if (CursorImage == GameFramework.SystemImages.Cursor_Undock)
            {
                Mode = PlayfieldModeType.Select;
                CursorImage = GameFramework.SystemImages.Cursor_Select;
                return;
            }
        }

        public void ChangeMode(PlayfieldModeType mode)
        {
            switch (mode)
            {
                case PlayfieldModeType.Engage:
                    Mode = PlayfieldModeType.Engage;
                    CursorImage = GameFramework.SystemImages.Cursor_Engage;
                    break;
                case PlayfieldModeType.Move:
                    Mode = PlayfieldModeType.Move;
                    CursorImage = GameFramework.SystemImages.Cursor_Move;
                    break;
                case PlayfieldModeType.Undock:
                    Mode = PlayfieldModeType.Undock;
                    CursorImage = GameFramework.SystemImages.Cursor_Undock;
                    break;
                case PlayfieldModeType.Waypoint:
                    Mode = PlayfieldModeType.Waypoint;
                    CursorImage = GameFramework.SystemImages.Waypoint_Add;
                    ClearPawnSelection();
                    _mouse_points.Clear();
                    _mouse_points_v2.Clear();
                    break;
                case PlayfieldModeType.Zone:
                    Mode = PlayfieldModeType.Zone;
                    CursorImage = GameFramework.SystemImages.Waypoint_Add;
                    ClearPawnSelection();
                    _mouse_points.Clear();
                    _mouse_points_v2.Clear();
                    break;
                default:
                    Mode = PlayfieldModeType.Select;
                    CursorImage = GameFramework.SystemImages.Cursor_Select;
                    break;

            }
        }

        #region Game Interaction Functions

        public bool IsMoving(string pawn_id)
        {
            if (_motion_calculators.ContainsKey(pawn_id))
            {
                return _motion_calculators[pawn_id].IsRunning;
            }
            return false;
        }

        public Vector3 GetPawnPosition(string pawn_id)
        {
            Vector3 retval = Vector3.Empty;
            lock (this)
            {
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        retval.X = p.Position.X;
                        retval.Y = p.Position.Y;
                        retval.Z = p.Position.Z;
                    }
                }
            }
            return retval;
        }
        public AGT_Pawn SelectPawnAt(int x, int y)
        {
            lock (this)
            {
                if (_primary_pawn_selection != AGT_Pawn.Empty)
                {
                    SendPawnToBack(_primary_pawn_selection.Id);
                    _primary_pawn_selection = AGT_Pawn.Empty;
                }

                LinkedListNode<AGT_Pawn> node = _pawn_stack.Last;
                while (node != null)
                {
                    AGT_Pawn p = (AGT_Pawn)node.Value;
                    bool pawn = p.HitTest((int)((float)(x - _map_sprites.X) / PlayfieldScale), (int)((float)(y - _map_sprites.Y) / PlayfieldScale));
                    bool label = _label_dictionary[p.Id].RelativeHitTest(x, y, (int)_map_sprites.X, (int)_map_sprites.Y);

                    if (pawn || label)
                    {
                        lock (this)
                        {
                            _primary_pawn_selection = p;
                        }
                        BringPawnToFront(p.Id);
                        //CursorImage = GameFramework.SystemImages.Cursor_Move;
                        return p;
                    }
                    node = node.Previous;
                }
                CursorImage = GameFramework.SystemImages.Cursor_Select;
                return AGT_Pawn.Empty;
            }
        }

        public AGT_Pawn SelectPawn(string pawn_id)
        {
            lock (this)
            {
                if (_primary_pawn_selection != AGT_Pawn.Empty)
                {
                    SendPawnToBack(_primary_pawn_selection.Id);
                    _primary_pawn_selection = AGT_Pawn.Empty;
                }
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        _primary_pawn_selection = p;

                        BringPawnToFront(p.Id);
                        //CursorImage = GameFramework.SystemImages.Cursor_Move;
                        return p;
                    }
                }

                //CursorImage = GameFramework.SystemImages.Cursor_Select;
                return AGT_Pawn.Empty;
            }
        }

        public void ClearPawnSelection()
        {
            _primary_pawn_selection = AGT_Pawn.Empty;
        }

        public AGT_PointList SelectActiveZoneAt(int x, int y)
        {
            lock (this)
            {
                if (_active_zone_selection != null)
                {
                    SendActiveZoneToBack(_active_zone_selection.Id);
                    _active_zone_selection = AGT_PointList.Empty;
                }

                LinkedListNode<AGT_PointList> node = _active_zone_stack.Last;
                while (node != null)
                {
                    AGT_PointList p = (AGT_PointList)node.Value;

                    if (p.HitTest((int)((float)(x - _map_sprites.X) / PlayfieldScale), (int)((float)(y - _map_sprites.Y) / PlayfieldScale)))
                    {
                        lock (this)
                        {
                            _active_zone_selection = p;
                        }
                        BringActiveZoneToFront(p.Id);
                        return p;
                    }
                    node = node.Previous;
                }
                return AGT_PointList.Empty;
            }
        }

        public AGT_PointList SelectWaypointGroupAt(int x, int y)
        {
            lock (this)
            {
                if (_waypoint_group_selection != null)
                {
                    SendActiveZoneToBack(_waypoint_group_selection.Id);
                    _waypoint_group_selection = AGT_PointList.Empty;
                }

                LinkedListNode<AGT_PointList> node = _waypoint_group_stack.Last;
                while (node != null)
                {
                    AGT_PointList p = (AGT_PointList)node.Value;

                    if (p.HitTest((int)((float)(x - _map_sprites.X) / PlayfieldScale), (int)((float)(y - _map_sprites.Y) / PlayfieldScale)))
                    {
                        lock (this)
                        {
                            _waypoint_group_selection = p;
                        }
                        BringActiveZoneToFront(p.Id);
                        return p;
                    }
                    node = node.Previous;
                }
                return AGT_PointList.Empty;
            }
        }

        public void MoveSelected(float x, float y, float pixels_per_second)
        {
                
            lock (this)
            {
                if (_primary_pawn_selection != AGT_Pawn.Empty)
                {
                    if (SimulateMotion)
                    {
                        if (!_motion_calculators.ContainsKey(_primary_pawn_selection.Id))
                        {
                            AddMotionCalculator(_primary_pawn_selection);
                        }

                        if (_motion_calculators[_primary_pawn_selection.Id].IsRunning)
                        {
                            _motion_calculators[_primary_pawn_selection.Id].Stop();
                        }

                        _motion_calculators[_primary_pawn_selection.Id].ResetPosition(_primary_pawn_selection.Position.X, _primary_pawn_selection.Position.Y, _primary_pawn_selection.Position.Z);
                        _motion_calculators[_primary_pawn_selection.Id].MoveTo(x, y, 0, pixels_per_second);
                    }
                }

            }
        }

        public void MoveSelected(float x, float y, float pixels_per_second, System.Threading.ThreadStart callback)
        {

            lock (this)
            {
                if (_primary_pawn_selection != AGT_Pawn.Empty)
                {
                    if (SimulateMotion)
                    {
                        if (!_motion_calculators.ContainsKey(_primary_pawn_selection.Id))
                        {
                            AddMotionCalculator(_primary_pawn_selection, callback);
                        }

                        if (_motion_calculators[_primary_pawn_selection.Id].IsRunning)
                        {
                            _motion_calculators[_primary_pawn_selection.Id].Stop();
                        }

                        _motion_calculators[_primary_pawn_selection.Id].OnMoveComplete = callback;
                        _motion_calculators[_primary_pawn_selection.Id].ResetPosition(_primary_pawn_selection.Position.X, _primary_pawn_selection.Position.Y, _primary_pawn_selection.Position.Z);
                        _motion_calculators[_primary_pawn_selection.Id].MoveTo(x, y, 0, pixels_per_second);
                    }
                }

            }
        }



        public void MoveSelected(float x, float y)
        {

            lock (this)
            {
                if (_primary_pawn_selection != AGT_Pawn.Empty)
                {
                    _primary_pawn_selection.SetPosition(x, y, 0);
                }
            }

        }
        #endregion

        #region Motion Calculator Managment
        protected void AddMotionCalculator(AGT_Pawn pawn)
        {
            lock (this)
            {
                AGT_LinearMotion calculator = new AGT_LinearMotion(pawn.Position.X, pawn.Position.Y, pawn.Position.Z);
                if (_motion_calculators.ContainsKey(pawn.Id))
                {
                    _motion_calculators.Remove(pawn.Id);
                }
                _motion_calculators.Add(pawn.Id, calculator);
            }
        }

        protected void AddMotionCalculator(AGT_Pawn pawn, System.Threading.ThreadStart callback)
        {
            lock (this)
            {
                AGT_LinearMotion calculator = new AGT_LinearMotion(pawn.Position.X, pawn.Position.Y, pawn.Position.Z, callback);
                if (_motion_calculators.ContainsKey(pawn.Id))
                {
                    _motion_calculators.Remove(pawn.Id);
                }
                _motion_calculators.Add(pawn.Id, calculator);
            }
        }

        protected void RemoveMotionCalculator(AGT_Pawn pawn)
        {
            lock (this)
            {
                if (_motion_calculators.ContainsKey(pawn.Id))
                {
                    _motion_calculators.Remove(pawn.Id);
                }
            }
        }

        protected void RemoveMotionCalculator(string pawn_id)
        {
            lock (this)
            {
                if (_motion_calculators.ContainsKey(pawn_id))
                {
                    _motion_calculators.Remove(pawn_id);
                }
            }
        }

        #endregion

        #region Pawn Management
        protected void AddPawn(AGT_Pawn pawn)
        {
            lock (this)
            {
                _pawn_stack.AddFirst(pawn);
            }
        }
        protected void RemovePawn(string pawn_id)
        {
            lock (this)
            {
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        _pawn_stack.Remove(p);
                        break;
                    }
                }
            }
        }


        public void ClearPawns()
        {
            lock (this)
            {
                _pawn_stack.Clear();
            }
        }
        protected void BringPawnToFront(string pawn_id)
        {
            lock (this)
            {
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        _pawn_stack.Remove(p);
                        _pawn_stack.AddLast(p);
                        break;
                    }
                }
            }
        }
        protected void SendPawnToBack(string pawn_id)
        {
            lock (this)
            {
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        _pawn_stack.Remove(p);
                        _pawn_stack.AddFirst(p);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Label Management
        protected void AddLabel(AGT_Pawn p, AGT_Text label_info)
        {
            lock (this)
            {
                if (!_label_dictionary.ContainsKey(p.Id))
                {
                    _label_dictionary.Add(p.Id, label_info);
                }
                else
                {
                    _label_dictionary[p.Id] = label_info;
                }
            }
        }
        protected void RemoveLabel(string label_id)
        {
            lock (this)
            {
                if (_label_dictionary.ContainsKey(label_id))
                {
                    _label_dictionary.Remove(label_id);
                }
            }
        }
        public void ClearLabels()
        {
            lock (this)
            {
                _label_dictionary.Clear();
            }
        }
        #endregion

        #region Active Zone Management
        public void AddActiveZone(string active_zone_id)
        {
            lock (this)
            {
                AGT_PointList az = new AGT_PointList(active_zone_id, FillModeType.Fill);
                az.ClosedShape = true;
                Point[] points = _mouse_points.ToArray();
                for (int i = 0; i < points.Length; i++ )
                {
                    points[i].X =(int)((float)points[i].X / PlayfieldScale);
                    points[i].Y = (int)((float)points[i].Y / PlayfieldScale);
                }
                az.AddLines(points);
                
                _active_zone_sprites.AddResource(active_zone_id, az);
                ChangeMode(PlayfieldModeType.Select);

                _active_zone_stack.AddFirst(az);
            }
        }

        public void RemoveActiveZone(string active_zone_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _active_zone_stack)
                {
                    if (p.Id == active_zone_id)
                    {
                        _active_zone_stack.Remove(p);
                    }
                }
            }
        }
        public void ClearActiveZones()
        {
            lock (this)
            {
                _active_zone_stack.Clear();
            }
        }
        public void ClearActiveZoneSelection()
        {
            lock (this)
            {
                _active_zone_selection = AGT_PointList.Empty;
            }
        }


        public void BringActiveZoneToFront(string active_zone_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _active_zone_stack)
                {
                    if (p.Id == active_zone_id)
                    {
                        _active_zone_stack.Remove(p);
                        _active_zone_stack.AddLast(p);
                        break;
                    }
                }
            }
        }
        public void SendActiveZoneToBack(string active_zone_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _active_zone_stack)
                {
                    if (p.Id == active_zone_id)
                    {
                        _active_zone_stack.Remove(p);
                        _active_zone_stack.AddFirst(p);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Waypoint Group Management
        public void AddWaypointGroup(string waypoint_group_id)
        {
            lock (this)
            {
                AGT_PointList az = new AGT_PointList(waypoint_group_id, FillModeType.None);
                az.ClosedShape = false;
                az.DrawJoints = true;
                Point[] points = _mouse_points.ToArray();
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].X = (int)((float)points[i].X / PlayfieldScale);
                    points[i].Y = (int)((float)points[i].Y / PlayfieldScale);
                }

                az.AddLines(points);
                _waypoint_group_sprites.AddResource(waypoint_group_id, az);
                ChangeMode(PlayfieldModeType.Select);

                this._waypoint_group_stack.AddFirst(az);
            }
        }

        public void RemoveWaypointGroup(string waypoint_group_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _waypoint_group_stack)
                {
                    if (p.Id == waypoint_group_id)
                    {
                        _waypoint_group_stack.Remove(p);
                    }
                }
            }
        }
        public void ClearWaypointGroups()
        {
            lock (this)
            {
                _waypoint_group_stack.Clear();
            }
        }
        public void ClearWaypointGroupSelection()
        {
            lock (this)
            {
                _waypoint_group_selection = AGT_PointList.Empty;
            }
        }


        public void BringWaypointGroupToFront(string waypoint_group_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _waypoint_group_stack)
                {
                    if (p.Id == waypoint_group_id)
                    {
                        _waypoint_group_stack.Remove(p);
                        _waypoint_group_stack.AddLast(p);
                        break;
                    }
                }
            }
        }
        public void SendWaypointGroupToBack(string waypoint_group_id)
        {
            lock (this)
            {
                foreach (AGT_PointList p in _waypoint_group_stack)
                {
                    if (p.Id == waypoint_group_id)
                    {
                        _waypoint_group_stack.Remove(p);
                        _waypoint_group_stack.AddFirst(p);
                        break;
                    }
                }
            }
        }

        #endregion


        #region Scene Overrides
        public override void OnInitialize(Device d)
        {
            InitializeLine(d);
            /* Initialize fonts
             * */
            _pawn_font = new Microsoft.DirectX.Direct3D.Font(d, new System.Drawing.Font("Arial", 9, FontStyle.Bold));

            _label = new AGT_Label(d);
            _heading = new AGT_Heading(d);

            if (UnitTestMode)
            {
                UnitTest(d);
            }

            RenderLayers.Add(new RenderableLayer(RenderMapBackground));
            //RenderLayers.Add(new RenderableLayer(RenderHeadings));
            RenderLayers.Add(new RenderableLayer(RenderMapInset));
                       
            State = SceneState.RENDER;
        }

        
        public override void OnReInitialize(Device d)
        {
            State = SceneState.RENDER;
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.I:
                    ScrollState = ScrollStateEnum.ScrollUp;
                    break;
                case Keys.M:
                    ScrollState = ScrollStateEnum.ScrollDown;
                    break;
                case Keys.J:
                    ScrollState = ScrollStateEnum.ScrollLeft;
                    break;
                case Keys.L:
                    ScrollState = ScrollStateEnum.ScrollRight;
                    break;
            }
        }
        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.I:
                case Keys.M:
                case Keys.J:
                case Keys.L:
                    ScrollState = ScrollStateEnum.NoScroll;
                    break;
                case Keys.Return:
                    switch (Mode)
                    {
                        case PlayfieldModeType.Zone:
                            if (_mouse_points.Count > 2)
                            {
                                AddActiveZone("ActiveZone#" + _active_zone_sprites.Count);
                            }
                            break;

                        case PlayfieldModeType.Waypoint:
                            if (_mouse_points.Count > 2)
                            {
                                AddWaypointGroup("Waypoint#" + _waypoint_group_sprites.Count);
                            }
                            break;
                    }
                    break;
            }
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            switch (Mode)
            {

                case PlayfieldModeType.Zone:
                case PlayfieldModeType.Waypoint:
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            _mouse_points.Add(e.Location);
                            _mouse_points_v2.Add(new Vector2(e.X, e.Y));
                            break;
                        case MouseButtons.Right:
                            _mouse_points.Clear();
                            _mouse_points_v2.Clear();
                            break;
                    }
                    break;
                case PlayfieldModeType.Engage:
                    break;
                case PlayfieldModeType.Undock:
                    break;
                case PlayfieldModeType.Select:
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            if (!_dragging_minmap)
                            {
                                if (UserControl)
                                {
                                    if (SelectPawnAt(e.X, e.Y) != AGT_Pawn.Empty)
                                    {
                                        // We selected a pawn, therefore clear all other selection types.
                                        ClearActiveZoneSelection();
                                        ClearWaypointGroupSelection();
                                        if (OnMouseSelection != null)
                                        {
                                            OnMouseSelection(_primary_pawn_selection.Id);
                                        }
                                        return;
                                    }
                                }
                                if (SelectActiveZoneAt(e.X, e.Y) != AGT_PointList.Empty)
                                {
                                    ClearPawnSelection();
                                    ClearWaypointGroupSelection();
                                    if (OnMouseSelection != null)
                                    {
                                        OnMouseSelection(_active_zone_selection.Id);
                                    }
                                    return;
                                }
                                if (SelectWaypointGroupAt(e.X, e.Y) != AGT_PointList.Empty)
                                {
                                    ClearPawnSelection();
                                    ClearActiveZoneSelection();
                                    if (OnMouseSelection != null)
                                    {
                                        OnMouseSelection(_waypoint_group_selection.Id);
                                    }
                                    return;
                                }
                                if (OnMouseSelection != null)
                                {
                                    OnMouseSelection(string.Empty);
                                }
                            }
                            _dragging_minmap = false;
                            break;
                        case MouseButtons.Right:
                            if (UserControl)
                            {
                                MoveSelected((float)e.X, (float)e.Y, 24);
                            }
                            break;
                    }
                    break;
            }
        }


        public override void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((_minmap_rect.Contains(e.Location)) && (e.Button == MouseButtons.Left))
            {
                CenterPlayfieldToMiniMapPosition(e.X - _minmap_rect.X, e.Y - _minmap_rect.Y);
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (((_minmap_rect.Contains(e.Location)) && (e.Button == MouseButtons.Left)))
            {
                CenterPlayfieldToMiniMapPosition(e.X - _minmap_rect.X, e.Y - _minmap_rect.Y);
                _dragging_minmap = true;
            }
        }


        #endregion



        #region RenderLayers       
        private void RenderMapBackground(Device d, float frame_rate)
        {
            try
            {
                lock (this)
                {

                    #region Map Bounds Checking
                    /* Check the Playfield Bounds
                         * */
                    _playfield_bounds.X = (int)_map_sprites.X;
                    _playfield_bounds.Y = (int)_map_sprites.Y;
                    _playfield_bounds.Width = (int)((float)UTM_Mapping.ImageWidth * PlayfieldScale);
                    _playfield_bounds.Height = (int)((float)UTM_Mapping.ImageHeight * PlayfieldScale);

                    if (!TargetControlRect.Contains(_playfield_bounds))
                    {
                        #region Scroll Playfield
                        switch (ScrollState)
                        {
                            case ScrollStateEnum.ScrollUp:
                                if (_playfield_bounds.Y < (TargetControlRect.Y))
                                {
                                    SetPlayfieldVertical(_map_sprites.Y + Scroll_Increment);
                                }
                                break;
                            case ScrollStateEnum.ScrollRight:
                                if (_playfield_bounds.Right > (TargetControlRect.Right))
                                {
                                    SetPlayfieldHorizontal(_map_sprites.X - Scroll_Increment);
                                }
                                break;
                            case ScrollStateEnum.ScrollLeft:
                                if (_playfield_bounds.X < (TargetControlRect.X))
                                {
                                    SetPlayfieldHorizontal(_map_sprites.X + Scroll_Increment);
                                }
                                break;
                            case ScrollStateEnum.ScrollDown:
                                if (_playfield_bounds.Bottom > (TargetControlRect.Bottom))
                                {
                                    SetPlayfieldVertical(_map_sprites.Y - Scroll_Increment);
                                }
                                break;
                            default:
                                break;
                        }
                        #endregion

                        #region Bound the Playfield
                        if ((_playfield_bounds.Right <= TargetControlRect.Right) && (_playfield_bounds.X != TargetControlRect.X))
                            {
                            SetPlayfieldHorizontal(_map_sprites.X + (TargetControlRect.Right - _playfield_bounds.Right));
                        }

                        if ((_playfield_bounds.Bottom <= TargetControlRect.Bottom) && (_playfield_bounds.Y != TargetControlRect.Y))
                            {
                            SetPlayfieldVertical(_map_sprites.Y + (TargetControlRect.Height - _playfield_bounds.Bottom));
                        }

                        if ((_playfield_bounds.Left > TargetControlRect.X))
                        {
                            SetPlayfieldHorizontal(TargetControlRect.X);
                        }
                        #endregion

                        _show_minimap = true;

                    }
                    else
                    {

                        #region Reset Minimap to TopLeft Corner, Don't show MinMap
                        SetPlayfieldPosition(TargetControlRect.X, TargetControlRect.Y);
                        _show_minimap = false;
                        #endregion

                    }
                    #endregion


                    #region Draw Map Background
                    _map_sprites.Begin(SpriteFlags.AlphaBlend);
                    _map_sprites.SetTextureScale(_playfield_image_id, PlayfieldScale, PlayfieldScale, PlayfieldScale);
                    _map_sprites.Draw(_playfield_image_id, Color.White);
                    _map_sprites.End();
                    #endregion

                    #region Update Pawn Positions
                    foreach (AGT_Pawn p in _pawn_stack)
                    {
                        if (!Pause)
                        {
                            if (_motion_calculators.ContainsKey(p.Id))
                            {
                                if (_motion_calculators[p.Id].IsRunning)
                                {
                                    p.SetPosition(_motion_calculators[p.Id].ReCalculatePosition());

                                    /* Aptima Headings are drawn here because we want the line to start
                                     *  underneath the Pawn's Icon.
                                     * */
                                    if (_motion_calculators[p.Id].IsRunning && (Style == HeadingStyle.Aptima))
                                    {
                                        _heading.Begin();
                                        _heading.SetPosition(
                                            (p.Position.X * PlayfieldScale) + _map_sprites.X,
                                            (p.Position.Y * PlayfieldScale) + _map_sprites.Y,
                                            (_motion_calculators[p.Id].Destination.X * PlayfieldScale) + _map_sprites.X,
                                            (_motion_calculators[p.Id].Destination.Y * PlayfieldScale) + _map_sprites.Y);
                                        _heading.Draw();
                                        _heading.End();
                                    }

                                }
                                else
                                {
                                    RemoveMotionCalculator(p);
                                }
                            }
                        }
                    }    
                        #endregion

                    #region Draw Pawns
                    _pawn_sprites.Scale = PlayfieldScale;
                    _pawn_sprites.Begin(SpriteFlags.AlphaBlend);
                    foreach (AGT_Pawn p in _pawn_stack)
                    {
                        if (_pawn_sprites.CanRotate(p.SpriteId))
                        {
                            if (_motion_calculators.ContainsKey(p.Id) && (Style == HeadingStyle.Aptima))
                            {
                                _pawn_sprites.SetRotation(p.SpriteId, 0, 0, _motion_calculators[p.Id].AngleOfAttack);
                            }
                            else
                            {
                                _pawn_sprites.SetRotation(p.SpriteId, 0, 0, 0);
                            }
                        }
                        _pawn_sprites.SetPosition(p.SpriteId, p.Position);
                        //_pawn_sprites.SetTextureScale(p.SpriteId, PlayfieldScale, PlayfieldScale, PlayfieldScale);
                        //if (PlayfieldScale >= .25f)
                        //{
                        //    _pawn_sprites.SetTextureScale(p.SpriteId, 1f, 1f, 1f);
                        //}
                        //else
                        //{
                        //    _pawn_sprites.SetTextureScale(p.SpriteId, .5f, .5f, .5f);
                        //}

                        if (_primary_pawn_selection == p)
                        {
                            _pawn_sprites.Draw(p.SpriteId, Color.Yellow);
                        }
                        else
                        {
                            _pawn_sprites.Draw(p.SpriteId, Color.White);
                        }

                    }
                    _pawn_sprites.End();


                    #endregion
                    foreach (AGT_Pawn p in _pawn_stack)
                    {
                        if (p.Id != _primary_pawn_selection.Id)
                        {
                            _label.Begin(_label_dictionary[_pawn_stack.First.Value.Id].Height);

                            _label_dictionary[p.Id].SetPosition(
                                (int)((p.Position.X * PlayfieldScale) - (_label_dictionary[p.Id].Width * .5)),
                                (int)((p.Position.Y * PlayfieldScale) + ((p.Height) * .5) + 2));

                            _label.DrawRelative(Color.White, Color.Black, _label_dictionary[p.Id], _map_sprites.X, _map_sprites.Y);
                            _label.End();
                        }
                    }


                    if (!Pause)
                    {
                        foreach (AGT_LinearMotion mc in _motion_calculators.Values)
                        {
                            if (mc.IsRunning && (Style == HeadingStyle.MilStd))
                            {
                                _heading.Begin();
                                _heading.SetPosition(
                                    (mc.CurrentPosition.X * PlayfieldScale) + _map_sprites.X,
                                    (mc.CurrentPosition.Y * PlayfieldScale) + _map_sprites.Y,
                                    (mc.Destination.X * PlayfieldScale) + _map_sprites.X,
                                    (mc.Destination.Y * PlayfieldScale) + _map_sprites.Y);
                                _heading.Draw();
                                _heading.End();
                            }
                        }
                    }

                    #region Draw Selected Pawn's Identifying Label
                    if (_pawn_stack.Count > 0)
                    {
                        _label.Begin(_label_dictionary[_pawn_stack.First.Value.Id].Height);
                        if (_primary_pawn_selection != AGT_Pawn.Empty)
                        {
                            if (PlayfieldScale >= .5f)
                            {
                                _label_dictionary[_primary_pawn_selection.Id].SetPosition(
                                    (int)((_primary_pawn_selection.Position.X * PlayfieldScale) - (_label_dictionary[_primary_pawn_selection.Id].Width * .5)),
                                    (int)((_primary_pawn_selection.Position.Y * PlayfieldScale) + ((_primary_pawn_selection.Height * PlayfieldScale) * .5) + 2));
                            }
                            else
                            {
                                _label_dictionary[_primary_pawn_selection.Id].SetPosition(
                                    (int)((_primary_pawn_selection.Position.X * PlayfieldScale) - (_label_dictionary[_primary_pawn_selection.Id].Width * .5)),
                                    (int)((_primary_pawn_selection.Position.Y * PlayfieldScale) + ((_primary_pawn_selection.ScaledHeight) * .5f) * PlayfieldScale + 2));
                            }

                            _label.DrawRelative(Color.Yellow, Color.Yellow, _label_dictionary[_primary_pawn_selection.Id], _map_sprites.X, _map_sprites.Y);
                        }
                        _label.End();
                    }
                    #endregion


                    if (!Pause)
                    {
                        #region Mouse Feedback - Active Zone and Waypoint Group "RubberBanding"
                        if ((Mode == PlayfieldModeType.Zone) || (Mode == PlayfieldModeType.Waypoint))
                        {
                            if (_mouse_points_v2.Count > 0)
                            {
                                Point pos = GetCursorPosition();
                                _mouse_points_v2.Add(new Vector2((pos.X), (pos.Y)));
                                SetLineOptions(true, 2);
                                DrawLines(_mouse_points_v2.ToArray(), Color.Red);
                                _mouse_points_v2.RemoveAt(_mouse_points_v2.Count - 1);
                            }
                        }
                        #endregion
                    }

                    #region Active Zone Rendering
                    _active_zone_sprites.Scale = PlayfieldScale;
                    _active_zone_sprites.Begin(SpriteFlags.AlphaBlend);
                    foreach (AGT_PointList p in _active_zone_stack)
                    {
                        _active_zone_sprites.SetTextureScale(p.TypeId, PlayfieldScale, PlayfieldScale, PlayfieldScale);
                        if (p.Id != _active_zone_selection.Id)
                        {
                            _active_zone_sprites.Draw(p.TypeId, Color.White);
                        }
                        else
                        {
                            _active_zone_sprites.Draw(p.TypeId, Color.Gray);
                        }
                    }
                    _active_zone_sprites.End();
                    #endregion


                    #region Render Waypoint Groups
                    _waypoint_group_sprites.Scale = PlayfieldScale;
                    _waypoint_group_sprites.Begin(SpriteFlags.AlphaBlend);

                    foreach (AGT_PointList p in _waypoint_group_stack)
                    {
                        _waypoint_group_sprites.SetTextureScale(p.TypeId, PlayfieldScale, PlayfieldScale, PlayfieldScale);
                        if (p.Id != _waypoint_group_selection.Id)
                        {
                            _waypoint_group_sprites.Draw(p.TypeId, Color.White);
                        }
                        else
                        {
                            _waypoint_group_sprites.Draw(p.TypeId, Color.Gray);
                        }
                    }
                    _waypoint_group_sprites.End();
                    #endregion

                }


            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.StackTrace, e.Message);
                Console.WriteLine(e.StackTrace + "\n\n" + e.Message);
                State = SceneState.ERROR;
            }
        }

        private void RenderMapInset(Device d, float frame_rate)
        {
            lock (this)
            {
                if (_show_minimap)
                {
                    _minmap_rect.X = (int)(TargetControlRect.Right - MiniMap_Width);
                    _minmap_rect.Y = 0;
                    _minmap_rect.Width = MiniMap_Width;
                    _minmap_rect.Height = (int)(((float)UTM_Mapping.ImageHeight / (float)UTM_Mapping.ImageWidth) * (float)MiniMap_Width);

                    _minmap_thumb.X = PlayfieldToMinMapHorizontal(-(int)_map_sprites.X);
                    _minmap_thumb.Y = PlayfieldToMinMapVertical(-(int)_map_sprites.Y);
                    _minmap_thumb.Width = (int)(_minmap_rect.Width * ((float)TargetControlRect.Width / (UTM_Mapping.ImageWidth * PlayfieldScale)));
                    _minmap_thumb.Height = (int)(_minmap_rect.Height * ((float)TargetControlRect.Height / (UTM_Mapping.ImageHeight * PlayfieldScale)));

                    SetLineOptions(true, _minmap_rect.Height);
                    DrawFillRectangle(_minmap_rect, Color.FromArgb(180, 63, 63, 63));
                    SetLineOptions(true, 1);
                    DrawRectangle(_minmap_thumb, Color.Red);

                    foreach (AGT_Pawn p in _pawn_stack)
                    {
                        _minmap_blip.X = PlayfieldToMinMapHorizontal((int)(p.Position.X * PlayfieldScale)) - (int)(_minmap_blip.Width * .5f);
                        _minmap_blip.Y = PlayfieldToMinMapVertical((int)(p.Position.Y * PlayfieldScale)) - (int)(_minmap_blip.Height * .5f);
                        SetLineOptions(true, 1);
                        if (_primary_pawn_selection == p)
                        {
                            DrawTriangle(_minmap_blip, Color.Yellow);
                        }
                        else
                        {
                            DrawTriangle(_minmap_blip, Color.DodgerBlue);
                        }

                    }
                }
            }

        }
        #endregion
        




        public void SetPlayfieldPosition(float x, float y)
        {
            SetPlayfieldHorizontal(x);
            SetPlayfieldVertical(y);
        }
        public void SetPlayfieldHorizontal(float x)
        {
            _map_sprites.X = x;
            _pawn_sprites.X = _map_sprites.X;
            _active_zone_sprites.X = _map_sprites.X;
            _waypoint_group_sprites.X = _map_sprites.X;
        }
        public void SetPlayfieldVertical(float y)
        {
            _map_sprites.Y = y;
            _pawn_sprites.Y = _map_sprites.Y;
            _active_zone_sprites.Y = _map_sprites.Y;
            _waypoint_group_sprites.Y = _map_sprites.Y;
        }

        public int PlayfieldToMinMapHorizontal(int x)
        {
            return (int)((x / (UTM_Mapping.ImageWidth * PlayfieldScale)) * (float)MiniMap_Width) + _minmap_rect.X;
        }
        public int PlayfieldToMinMapVertical(int y)
        {
            return (int)((y/ (UTM_Mapping.ImageHeight * PlayfieldScale)) * (float)_minmap_rect.Height) + _minmap_rect.Y;
        }

        public void CenterPlayfieldToMiniMapPosition(float x, float y)
        {
            int scaledx = (int)(((float)x / (float)_minmap_rect.Width) * ((float)UTM_Mapping.ImageWidth));
            int scaledy = (int)(((float)y / (float)_minmap_rect.Height) * ((float)UTM_Mapping.ImageHeight));
            CenterPlayfieldPosition((scaledx * PlayfieldScale), (scaledy * PlayfieldScale)); 
        }

        public void CenterPlayfieldPosition(float x, float y)
        {
            float new_x = (float)((x - (TargetControlRect.Width * .5)));
            float new_y = (float)((y - (TargetControlRect.Height * .5)));

            float max_extent_Y = (UTM_Mapping.ImageHeight * PlayfieldScale) - TargetControlRect.Height;
            float max_extent_X = (UTM_Mapping.ImageWidth * PlayfieldScale) - TargetControlRect.Width;
            float min_extent_Y = TargetControlRect.Y;
            float min_extent_X = TargetControlRect.X;

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

            SetPlayfieldPosition(-new_x,-new_y);
        }

        public void AddMapObject(string id, string texture_reference, float x, float y, float z, Color color)
        {
            try
            {
                Rectangle measure = Rectangle.Empty;

                AGT_SpriteId sprite_id;
                sprite_id.Id = texture_reference;

                AGT_SpriteResource t = _pawn_sprites.GetTextureDefinition(sprite_id);
                AGT_Pawn p = new AGT_Pawn(id, t);
                p.SetPosition(x, y, z);
                AddPawn(p);

                if (_pawn_font != null)
                {
                    measure = _pawn_font.MeasureString(null, p.Id, DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.White);
                    AGT_Text text = new AGT_Text(p.Id, _pawn_font);
                    text.Background = color;
                    AddLabel(p, text);

                    AddMotionCalculator(p);
                }
            }
            catch (Exception)
            {
            }
        }

        public void AddMapObject(string id, string texture_reference, float x, float y, float z, Color color, System.Threading.ThreadStart callback)
        {
            try
            {
                Rectangle measure = Rectangle.Empty;

                AGT_SpriteId sprite_id;
                sprite_id.Id = texture_reference;

                AGT_SpriteResource t = _pawn_sprites.GetTextureDefinition(sprite_id);
                AGT_Pawn p = new AGT_Pawn(id, t);
                p.SetPosition(x, y, z);
                AddPawn(p);

                measure = _pawn_font.MeasureString(null, p.Id, DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.White);
                AGT_Text text = new AGT_Text(p.Id, _pawn_font);
                text.Background = color;
                AddLabel(p, text);

                AddMotionCalculator(p, callback);
            }
            catch (Exception)
            {
            }
        }

        public bool HasPawn(string id)
        {
            bool result = false;
            lock (this)
            {
                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == id)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }


        public void RemoveMapObject(string id)
        {
            lock (this)
            {
                RemovePawn(id);
                RemoveLabel(id);
                RemoveMotionCalculator(id);
                _primary_pawn_selection = AGT_Pawn.Empty;
            }
        }

        public void RemovePawns()
        {
            lock (this)
            {
                _pawn_stack.Clear();
                _label_dictionary.Clear();
                _motion_calculators.Clear();
            }
        }

        public void ChangePawnSprite(string pawn_id, string sprite_reference)
        {
            lock (this)
            {
                AGT_SpriteId sprite_id;
                sprite_id.Id = sprite_reference;

                foreach (AGT_Pawn p in _pawn_stack)
                {
                    if (p.Id == pawn_id)
                    {
                        AGT_SpriteResource t = _pawn_sprites.GetTextureDefinition(sprite_id);
                        p.SetSpriteReference(t);
                        break;
                    }
                }
            }
            
        }

        #region UnitTesting Initialization Routine

        private void UnitTest(Device d)
        {

            AddMapObject("Object 1a", "ImageLib.f16_small.png", 20, 30, 0, Color.DodgerBlue);

            AddMapObject("Object 2a", "ImageLib.f16_small.png", 33, 12, 0, Color.DodgerBlue);
            
            AddMapObject("Object 3a", "ImageLib.f16_small.png", 36, 14, 0, Color.DodgerBlue);

            AddMapObject("Bad Image", "bad_image.png", 50, 100, 0, Color.Red);

            ChangePawnSprite("Object 1a", "ImageLib.frankie.png");

        }

        #endregion


        #region IAGT_SplashDialog Members
        public new void DialogInitialize(AGT_SplashDialog dialog_instance)
        {
        }

        public new void LoadResources(AGT_SplashDialog dialog_instance, Device device)
        {
            dialog_instance.UpdateStatusBar("Initializing Sprite Manager", 0, 3);
            if (_active_zone_sprites != null)
            {
                _active_zone_sprites.Dispose();
            }
            if (_waypoint_group_sprites != null)
            {
                _waypoint_group_sprites.Dispose();
            }
            if (_map_sprites != null)
            {
                _map_sprites.Dispose();
            }
            if (_pawn_sprites != null)
            {
                _pawn_sprites.Dispose();
            }

            _active_zone_sprites = new AGT_SpriteManager(device);
            _waypoint_group_sprites = new AGT_SpriteManager(device);
            _map_sprites = new AGT_SpriteManager(device);
            _pawn_sprites = AGT_ImageLibrary.Load(ImageLibraryPath, dialog_instance, device);

            dialog_instance.UpdateStatusBar("Loading " + MapFile, 2, 3);
            _playfield_image_id = _map_sprites.AddResource(MapFile, MapFile, 0, 0, 0, false);


            UTM_Mapping.ImageHeight = _map_sprites.GetTextureHeight(_playfield_image_id);
            UTM_Mapping.ImageWidth = _map_sprites.GetTextureWidth(_playfield_image_id);

            dialog_instance.UpdateStatusBar("Finished", 3, 3);
        }

        #endregion

        #region IAGT_SceneLoadDialog Members

        public new void DialogInitialize(AGT_SceneLoadDialog dialog_instance)
        {
        }

        public new void LoadResources(AGT_SceneLoadDialog dialog_instance, Device device)
        {
            lock (this)
            {
                dialog_instance.UpdateStatusBar("Initializing Sprite Manager", 0, 3);
                if (_active_zone_sprites != null)
                {
                    _active_zone_sprites.Dispose();
                }
                if (_waypoint_group_sprites != null)
                {
                    _waypoint_group_sprites.Dispose();
                }
                if (_map_sprites != null)
                {
                    _map_sprites.Dispose();
                }
                if (_pawn_sprites != null)
                {
                    _pawn_sprites.Dispose();
                }

                _active_zone_sprites = new AGT_SpriteManager(device);
                _waypoint_group_sprites = new AGT_SpriteManager(device);
                _map_sprites = new AGT_SpriteManager(device);
               _pawn_sprites = AGT_ImageLibrary.Load(ImageLibraryPath, dialog_instance, device);
                
                dialog_instance.UpdateStatusBar("Loading " + MapFile, 2, 3);
                _playfield_image_id = _map_sprites.AddResource(MapFile, MapFile, 0, 0, 0, false);


                UTM_Mapping.ImageHeight = _map_sprites.GetBitmapHeight(_playfield_image_id);
                UTM_Mapping.ImageWidth = _map_sprites.GetBitmapWidth(_playfield_image_id);


                dialog_instance.UpdateStatusBar("Finished", 3, 3);
            }
        }

        #endregion
    }
}
