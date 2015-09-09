using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;


namespace AGT.GameToolkit
{
    public class AGT_WaypointList
    {
        public int Count
        {
            get
            {
                lock (this)
                {
                    return _waypoints.Count;
                }
            }
        }


        private string _id = string.Empty;
        private Vector2[] _line_points;

        private LinkedList<AGT_Waypoint> _waypoints = new LinkedList<AGT_Waypoint>();
        private Microsoft.DirectX.Direct3D.Line _line = null;



        public AGT_WaypointList(Microsoft.DirectX.Direct3D.Device d)
        {
            _line = new Line(d);
            _line_points = new Vector2[2];
        }


        public AGT_Waypoint GetWaypointAt(int x, int y)
        {
            lock (this)
            {
                foreach (AGT_Waypoint point in _waypoints)
                {
                    if (point.HitTest(x, y))
                    {
                        return point;
                    }
                }
                return null;
            }
        }


        public int AddWaypoint(AGT_Waypoint waypoint)
        {
            lock (this)
            {
                _waypoints.AddLast(waypoint);
                return _waypoints.Count;
            }
        }


        public bool DeleteWaypoint(AGT_Waypoint waypoint)
        {
            lock (this)
            {
                LinkedListNode<AGT_Waypoint> node = _waypoints.Find(waypoint);
                if (node != null)
                {
                    _waypoints.Remove(node);
                    return true;
                }
                return false;
            }
        }


        public AGT_Waypoint InsertWaypointAt(int x, int y)
        {
            lock (this)
            {
                LinkedListNode<AGT_Waypoint> node = _waypoints.First;
                while (node.Next != null)
                {
                    if (LineHitTest(node.Value.X, node.Value.Y, node.Next.Value.X, node.Next.Value.Y, x, y))
                    {
                        if ( ((node.Value.X <= x) && (node.Next.Value.X >= x)) &&
                              ((node.Value.Y <= y) && (node.Next.Value.Y >= y)) )
                        {
                            float new_x = (node.Next.Value.X + node.Value.X) * .5f;
                            float new_y = (node.Next.Value.Y + node.Value.Y) * .5f;
                            AGT_Waypoint point = new AGT_Waypoint("Point " + _waypoints.Count, (float)new_x, (float)new_y, 0);
                            _waypoints.AddAfter(node, point);
                            return point;
                        }
                    }
                    node = node.Next;
                }
                return null;
            }
        }


        public bool InsertWaypoint(AGT_Waypoint after, AGT_Waypoint insert)
        {
            lock (this)
            {
                LinkedListNode<AGT_Waypoint> node = _waypoints.Find(after);
                if (node != null)
                {
                    _waypoints.AddAfter(_waypoints.Find(after), insert);
                    return true;
                }
                return false;
            }
        }


        public bool LineHitTest(float line_x1, float line_y1, float line_x2, float line_y2, int point_x, int point_y)
        {
            // Ax + Bx + C = 0
            float A = line_y2 - line_y1;
            float B = line_x1 - line_x2;
            float C = A * line_x1 + B * line_y1;

            float distance = (float)((A * point_x + B * point_y + C) / Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2)));
            return (distance <= 5);
        }


        public void Begin()
        {
            _line.Width = 2;
            _line.Antialias = true;

            _line.Begin();
        }


        public void Draw()
        {
            LinkedListNode<AGT_Waypoint> node = _waypoints.First;
            while (node.Next != null)
            {
                _line_points[0].X = node.Value.X;
                _line_points[0].Y = node.Value.Y;
                _line_points[1].X = node.Next.Value.X;
                _line_points[1].Y = node.Next.Value.Y;
                _line.Draw(_line_points, Color.Black);
                node = node.Next;
            }
        }


        public void End()
        {
            _line.End();

            AGT_SystemImages.Begin();
            foreach (AGT_Waypoint point in _waypoints)
            {
                AGT_SystemImages.Draw(AGT_SystemImages.Waypoint, point.X, point.Y, point.Z, point.Diffuse);
            }
            AGT_SystemImages.End();
        }


    }
}
