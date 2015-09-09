using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public class MapScene:Scene
    {
        protected Obj_Sprite Map;
        private static int MAX_SELECTIONS = 2;

        protected List<string> _select_buffer;
        protected LinkedList<string> _stacking_order;

        protected Rectangle _client_area = Rectangle.Empty;
        public Rectangle ClientArea
        {
            get
            {
                return _client_area;
            }
            set
            {
                _client_area = value;
            }
        }

        protected class ActiveZone
        {
            public int Color;
            public bool Visible = false;
            public List<CustomVertex.TransformedColored> Vertices;

            public ActiveZone(int color, bool visible, List<CustomVertex.TransformedColored> vertices)
            {
                Color = color;
                Visible = visible;
                Vertices = vertices;
            }
        }
        protected Dictionary<string, ActiveZone> _active_zones;

        protected Dictionary<string, MappableObject> _playfield_objects;
        public List<string> PlayfieldObjects
        {
            get
            {
                lock (this)
                {
                    return new List<string>(_playfield_objects.Keys);
                }
            }
        }
        public List<string> OrderedPlayfieldObjects
        {
            get
            {
                lock (this)
                {
                    return new List<string>(_stacking_order);
                }
            }
        }

        public Color PlayfieldColor
        {
            set
            {
                Map.Diffuse = value;
            }
            get
            {
                return Map.Diffuse;
            }
        }


        public MapScene()
        {
            _select_buffer = new List<string>();
            _stacking_order = new LinkedList<string>();

            _playfield_objects = new Dictionary<string, MappableObject>();
            _active_zones = new Dictionary<string, ActiveZone>();

        }

        #region Selection
        public void Select(string item)
        {
            lock (this)
            {
                if (!_select_buffer.Contains(item))
                {
                    _select_buffer.Add(item);
                }
            }
        }
        public bool RightClickSelect(float xpos, float ypos)
        {
            lock (this)
            {
                LinkedListNode<string> current_node = _stacking_order.Last;

                while (current_node != null)
                {
                    MappableObject p = _playfield_objects[current_node.Value];
                    if (p.HitTest(xpos, ypos) && p.CanSelect)
                    {
                        switch (_select_buffer.Contains(p.ID))
                        {
                            case true:
                                // If we already have a p.id and its not the primary selection, then remove it.
                                if ((_select_buffer.Count > 1) && (_select_buffer[0] != p.ID))
                                {
                                    _select_buffer.Remove(p.ID);
                                }
                                break;

                            case false:
                                // If we have 2 selections overwrite the second one, otherwise assume 1 already selected, add another.
                                if (_select_buffer.Count == MAX_SELECTIONS)
                                {
                                    _select_buffer[1] = p.ID;
                                }
                                else
                                {
                                    _select_buffer.Add(p.ID);
                                }
                                break;
                        }

                        return true;

                    }

                    current_node = current_node.Previous;
                }

            }

            return false;
        }

        public bool SelectionExists(float xpos, float ypos)
        {
            lock (this)
            {
                LinkedListNode<string> current_node = _stacking_order.Last;

                while (current_node != null)
                {
                    MappableObject p = _playfield_objects[current_node.Value];

                    if (p.HitTest(xpos, ypos) && p.CanSelect)
                    {
                        return true;
                    }

                    current_node = current_node.Previous;
                }


            }
            return false;
        }

        public bool SelectSingle(float xpos, float ypos)
        {
            lock (this)
            {
                // If more than one selection exists, remove the last selection so only
                // one exists.
                if (_select_buffer.Count > 1)
                {
                    _select_buffer.RemoveRange(1, _select_buffer.Count - 1);

                    //return true;
                }


                LinkedListNode<string> current_node = _stacking_order.Last;

                while (current_node != null)
                {
                    MappableObject p = _playfield_objects[current_node.Value];

                    if (p.HitTest(xpos, ypos) && p.CanSelect)
                    {

                        switch (_select_buffer.Contains(p.ID))
                        {
                            case true:
                                _select_buffer.Clear();
                                _stacking_order.Remove(current_node);
                                _stacking_order.AddFirst(current_node);
                                return SelectSingle(xpos, ypos);
                            case false:
                                _select_buffer.Clear();
                                _select_buffer.Add(p.ID);
                                _stacking_order.Remove(current_node);
                                _stacking_order.AddLast(current_node);
                                return true;
                        }
                    }

                    current_node = current_node.Previous;
                }


            }
            return false;
        }

        public int SelectionCount()
        {
            lock (this)
            {
                if (_select_buffer != null)
                {
                    return _select_buffer.Count;
                }
                return -1;
            }
        }
        public MappableObject GetSelection(int index)
        {
            lock (this)
            {
                try
                {
                    if ((index < _select_buffer.Count) && (index >= 0))
                    {
                        return _playfield_objects[_select_buffer[index]];
                    }
                }
                catch (NullReferenceException)
                {
                }
                return null;           
            }
        }
        public void RemoveSelection(int start, int count)
        {
            lock (this)
            {
                if (_select_buffer != null)
                {
                    _select_buffer.RemoveRange(start, count);
                }
            }
        }
        public void DeselectAll()
        {
            lock (this)
            {
                if (_select_buffer != null)
                {
                    _select_buffer.Clear();
                }
            }
        }
        #endregion

        #region Map Objects
        public void AddActiveZone(string id, int color, bool visible, List<CustomVertex.TransformedColored> vertices)
        {
            lock (this)
            {
                if (!_active_zones.ContainsKey(id))
                {
                    _active_zones.Add(id, new ActiveZone(color, visible, vertices));
                }
                else
                {
                    _active_zones[id] = new ActiveZone(color, visible, vertices);
                }
            }
        }
        public void ClearActiveZones()
        {
            lock (this)
            {
                _active_zones.Clear();
            }
        }
        public void DrawActiveZones(Canvas c)
        {
            lock (this)
            {
                foreach (ActiveZone zone in _active_zones.Values)
                {
                    if (zone.Visible)
                    {
                        c.DrawShape(zone.Vertices, this.Scale, Map.Position.X, Map.Position.Y, true);
                    }
                }
            }
        }

        public void AddMappableObject(string id, MappableObject obj)
        {
            lock (this)
            {
                if (!_playfield_objects.ContainsKey(id))
                {
                    _playfield_objects.Add(id, obj);
                    _stacking_order.AddFirst(id);
                    ObjectAddedUpdate(obj);
                }
            }
        }
        public virtual void ObjectAddedUpdate(MappableObject obj)
        {
        }
        public void RemoveMapObject(string id)
        {
            lock (this)
            {
                if (_playfield_objects.ContainsKey(id))
                {
                    _playfield_objects.Remove(id);
                }
                if (_select_buffer.Contains(id))
                {
                    _select_buffer.Remove(id);
                }
                if (_stacking_order.Contains(id))
                {
                    _stacking_order.Remove(id);
                }
                ObjectRemovedUpdate(id);
            }
        }
        public virtual void ObjectRemovedUpdate(string object_id)
        {
        }

        public bool ContainsMapObject(string id)
        {
            lock (this)
            {
                return _playfield_objects.ContainsKey(id);
            }
        }
        public bool IsMapObjectMoving(string id)
        {
            lock (this)
            {
                return _playfield_objects[id].IsPathCalculatorRunning();
            }
        }
        public void MoveMapObject(string id, float xpos, float ypos, float zpos, float velocity)
        {
            lock (this)
            {
                _playfield_objects[id].StartPathCalculator(xpos, ypos, zpos, velocity);
            }
        }
        public void MoveMapObject(string id, float startx, float starty, float startz, float endx, float endy, float endz, float velocity)
        {
            lock (this)
            {
                _playfield_objects[id].SetPosition(startx, starty, startz);
                if ((startx != endx) || (starty != endy) || (startz != endz))
                {
                    _playfield_objects[id].StartPathCalculator(endx, endy, endz, velocity);
                }
            }
        }
        public MappableObject GetMappableObject(string id)
        {
            try
            {
                lock (this)
                {
                    if (_playfield_objects.ContainsKey(id))
                    {
                        return _playfield_objects[id];
                    }
                    return null;
                }
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Key not found: " + id);
            }
            return null;
        }

        public List<string> GetListOfObjects()
        {
            List<string> objectList = new List<string>();

            lock (this)
            {
                foreach (string s in _playfield_objects.Keys)
                {
                    objectList.Add(s);
                }
            }

            return objectList;
        }

        #endregion



    }
}
