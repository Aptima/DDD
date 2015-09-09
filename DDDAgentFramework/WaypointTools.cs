using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class Waypoint
    {
        private String m_name;
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private LocationValue m_location;
        public LocationValue Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        public Waypoint()
        {
            m_name = String.Empty;
            m_location = new LocationValue();
        }

        public Waypoint(String name)
        {
            m_name = name;
            m_location = new LocationValue();
        }

        public Waypoint(String name, LocationValue loc)
        {
            m_name = name;
            m_location = new LocationValue();
            m_location.X = loc.X;
            m_location.Y = loc.Y;
            m_location.Z = loc.Z;
            m_location.exists = loc.exists;
        }
        public Waypoint(String name, Double x,Double y,Double z)
        {
            m_name = name;
            m_location = new LocationValue();
            m_location.X = x;
            m_location.Y = y;
            m_location.Z = z;
            m_location.exists = true;


        }

        public Waypoint(Waypoint wp)
        {
            m_name = wp.Name;
            m_location = new LocationValue();
            m_location.X = wp.Location.X;
            m_location.Y = wp.Location.Y;
            m_location.Z = wp.Location.Z;
            m_location.exists = wp.Location.exists;
        }

        public String ToXML()
        {
            StringBuilder s = new StringBuilder(String.Empty);
            s.Append("<Waypoint>");
            if (Location.exists)
            {
                s.Append(String.Format("<X>{0:0.00}</X><Y>{1:0.00}</Y><Z>{2:0.00}</Z>", Location.X, Location.Y, Location.Z));
            }
            else
            {
                s.Append(String.Format("<X>NULL</X><Y>NULL</Y><Z>NULL</Z>"));
            }
            s.Append("</Waypoint>");
            return s.ToString();
        }

    }
    public class ActiveRegionWaypointSorter : IComparer<ActiveRegionWaypoint>
    {
        private Waypoint m_refPoint;
        public ActiveRegionWaypointSorter(Waypoint refPoint)
        {
            m_refPoint = refPoint;
        }

        public int Compare(ActiveRegionWaypoint obj1, ActiveRegionWaypoint obj2)
        {
            double dis1 = BehaviorHelper.Distance(obj1.Location, m_refPoint.Location);
            double dis2 = BehaviorHelper.Distance(obj2.Location, m_refPoint.Location);

            if (dis1 < dis2)
            {
                return -1;
            }
            else if (dis1 > dis2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public class ActiveRegionWaypoint : Waypoint
    {
        private String m_activeRegion;
        public String ActiveRegion
        {
            get { return m_activeRegion; }
            set { m_activeRegion = value; }
        }
        public ActiveRegionWaypoint()
            : base()
        {
            m_activeRegion = String.Empty;
        }
        public ActiveRegionWaypoint(String name)
            : base(name)
        {
            m_activeRegion = String.Empty;
        }
        public ActiveRegionWaypoint(ActiveRegionWaypoint wp)
            : base(wp)
        {
            m_activeRegion = String.Empty;
        }
        public ActiveRegionWaypoint(String name, LocationValue loc)
            : base(name, loc)
        {
            m_activeRegion = String.Empty;
        }
        public ActiveRegionWaypoint(String name, double x, double y, double z)
            : base(name, x, y, z)
        {
            m_activeRegion = String.Empty;
        }
        public ActiveRegionWaypoint(String name, String activeRegion, double x, double y, double z)
            : base(name, x,y,z)
        {
            m_activeRegion = activeRegion;
        }
    }

    public class LineSegment
    {
        Waypoint first;

        public Waypoint First
        {
            get { return first; }
            set { first = value; }
        }
        Waypoint second;

        public Waypoint Second
        {
            get { return second; }
            set { second = value; }
        }

        public LineSegment()
        {
            first = null;
            second = null;
        }
        public LineSegment(Waypoint first, Waypoint second)
        {
            this.first = first;
            this.second = second;
        }

    }

    public class WaypointSequence : List<Waypoint>
    {
        private int m_nextIndex = 0;
        private int m_currentIndex = 0;
        public int NextWaypointIndex
        {
            get { return m_nextIndex; }
            set { m_nextIndex = value; }
        }
        public int CurrentWaypointIndex
        {
            get { return m_currentIndex; }
            set { m_currentIndex = value; }
        }
        public Waypoint GetNextWaypoint()
        {
            //int index = m_nextIndex;
            //m_nextIndex += 1;
            m_currentIndex = m_nextIndex;
            m_nextIndex += 1;
            if (m_nextIndex >= this.Count)
            {
                m_nextIndex = 0;
            }

            return this[m_currentIndex];
        }

        public List<LineSegment> ToLineSegments()
        {
            List<LineSegment> segments = new List<LineSegment>();
            if (Count > 1)
            {
                for (int i = 1; i < Count; i++)
                {
                    LineSegment ls = new LineSegment(this[i - 1], this[i]);
                    segments.Add(ls);
                }
            }
            return segments;
        }

        public Waypoint GetWaypointClosestTo(LocationValue loc)
        {
            int closestIndex = 0;
            
            Vec3D refVec = new Vec3D(loc);
            Vec3D wpVec = new Vec3D(this[0].Location);
            Double closestDistance = refVec.ScalerDistanceTo(wpVec);
            Double dis;
            for (int i = 1; i < this.Count; i++)
            {
                wpVec = new Vec3D(this[i].Location);
                dis = refVec.ScalerDistanceTo(wpVec);
                if (dis < closestDistance)
                {
                    closestDistance = dis;
                    closestIndex = i;
                }
            }

            m_nextIndex = closestIndex;
            return this.GetNextWaypoint();
        }

        public WaypointSequence ToAbsolute(LocationValue referenceLoc)
        {
            WaypointSequence abs = new WaypointSequence();
            Waypoint newWP;
            foreach (Waypoint wp in this)
            {
                newWP = new Waypoint(wp);

                newWP.Location.X = referenceLoc.X + wp.Location.X;
                newWP.Location.Y = referenceLoc.Y + wp.Location.Y;
                newWP.Location.Z = referenceLoc.Z + wp.Location.Z;

                abs.Add(newWP);
            }
            abs.m_nextIndex = this.m_nextIndex;
            return abs;
        }
        public WaypointSequence ToRelative(LocationValue referenceLoc)
        {
            WaypointSequence rel = new WaypointSequence();

            Waypoint newWP;
            foreach (Waypoint wp in this)
            {
                newWP = new Waypoint(wp);

                newWP.Location.X = wp.Location.X - referenceLoc.X;
                newWP.Location.Y = wp.Location.Y - referenceLoc.Y;
                newWP.Location.Z = wp.Location.Z - referenceLoc.Z;

                rel.Add(newWP);
            }
            rel.m_nextIndex = this.m_nextIndex;
            return rel;
        }
    }

    public class WaypointRoute : WaypointSequence
    {
        private String m_name;
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public WaypointRoute():this(String.Empty)
        {
            
        }
        public WaypointRoute(String name)
        {
            Name = name;
        }
    }

    public class WaypointRouteTable : Dictionary<String, WaypointRoute>
    {
        public WaypointRouteTable()
        {

        }



        void ComputeIntersections(WaypointRoute route1, WaypointRoute route2)
        {
            List<LineSegment> segments1 = route1.ToLineSegments();
            List<LineSegment> segments2 = route2.ToLineSegments();

            Double THRESHOLD = 1.0;

            foreach (LineSegment ls1 in segments1)
            {
                foreach (LineSegment ls2 in segments2)
                {
                    LocationValue intersect = BehaviorHelper.LineIntersect(ls1.First.Location, ls1.Second.Location, ls2.First.Location, ls2.Second.Location);

                    if (intersect != null)
                    {
                        // if the intersect is really close to one of the route verticies, than don't do anything.
                        // otherwise, insert the intersect into the routes
                        if (BehaviorHelper.Distance(intersect, ls1.First.Location) > THRESHOLD && BehaviorHelper.Distance(intersect, ls1.Second.Location) > THRESHOLD)
                        {
                            int pos = route1.IndexOf(ls1.Second);
                            route1.Insert(pos, new Waypoint(String.Format("between-{0}-{1}",ls1.First.Name,ls1.Second.Name), intersect));
                        }

                        if (BehaviorHelper.Distance(intersect, ls2.First.Location) > THRESHOLD && BehaviorHelper.Distance(intersect, ls2.Second.Location) > THRESHOLD)
                        {
                            int pos = route2.IndexOf(ls2.Second);
                            route2.Insert(pos, new Waypoint(String.Format("between-{0}-{1}", ls2.First.Name, ls2.Second.Name), intersect));
                        }

                        return; // we only allow for one intersection between the two routes.
                    }
                }
            }
        }
        public void ComputeIntersections()
        {
            List<String> routes = new List<string>(Keys);
            while (routes.Count >= 2)
            {
                String rt1 = routes[0];
                routes.RemoveAt(0);

                foreach (String rt2 in routes)
                {
                    WaypointRoute route1 = this[rt1];
                    WaypointRoute route2 = this[rt2];

                    ComputeIntersections(route1, route2);
                }
            }

            
            
        }

        //public LocationGraph GenerateGraph()
        //{
        //    LocationGraph result = new LocationGraph();

        //    List<String> routeNames = new List<string>(this.Keys);
        //    String name;
        //    for (int i = 0; i < routeNames.Count; i++)
        //    {
        //        name = routeNames[i];
        //        WaypointRoute route = this[name];
        //        LocationGraph.LocationNode lastNode = null;
        //        foreach (Waypoint wp in route)
        //        {
        //            LocationGraph.LocationNode node = new LocationGraph.LocationNode(wp.Name, wp.Location);
        //            if (lastNode != null)
        //            {
        //                result.BiConnect(lastNode, node);
        //            }
        //        }
        //        if (i > 0)
        //        {
        //            String lastName = routeNames[i - 1];
        //            String currName = routeNames[i];
        //            // figure out intersection of two routes
                    
        //        }
        //    }





        //    return result;
        //}

        
    }
}
