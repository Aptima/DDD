using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class MoveWithAvoidanceBehavior : BehaviorInterface
    {
        private Waypoint m_holdWP;
        public Waypoint HoldWP
        {
            get { return m_holdWP; }
            set { m_holdWP = value; }
        }
        private String m_thisID;
        //private String m_destID;
        private LocationValue m_destLocation;
        private double m_avoidRange;
        private double m_avoidPathWidth;
        private DMView m_dmView;
        private double m_throttle;

        private Boolean m_isBlocked;
        private Boolean m_wasBlocked;

        private Boolean m_done = false;

        private Boolean m_paused = false;
        public Boolean Paused
        {
            set { m_paused = value; }
            get { return m_paused; }
        }

        private Boolean m_shouldAvoid = true;
        public Boolean ShouldAvoid
        {
            set { m_shouldAvoid = value; }
            get { return m_shouldAvoid; }
        }

        private String m_blockedBy;

        private List<String> m_objectClassesToAvoid = null;
        public MoveWithAvoidanceBehavior(String thisID, LocationValue destLoc, double avoidRange,double avoidPathWidth, List<String> objectClassesToAvoid, double throttle)
        {
            m_thisID = thisID;
            m_destLocation = destLoc;
            m_avoidRange = avoidRange;
            m_avoidPathWidth = avoidPathWidth;
            m_dmView = null;
            m_objectClassesToAvoid = objectClassesToAvoid;
            m_throttle = throttle;
            m_holdWP = null;
            m_blockedBy = String.Empty;

            
        }
        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;
            //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.Start(){0}",m_thisID));
            LogWriter.Write(m_dmView.DecisionMakerID + "_" + m_thisID, dmView.SimTime, String.Format("MoveWithAvoidanceBehavior.Start,{0},{1}", m_destLocation.ToXML(), m_throttle));
            m_wasBlocked = false;
            
            
            SimObject me = dmView.AllObjects[m_thisID];
            //SimObject dest = dmView.AllObjects[m_destID];

            LocationValue myLocation = me.Location;
            //LocationValue destLocation = dest.Location;


            if (BehaviorHelper.LocationIsEqual(myLocation, m_destLocation))
            {
                m_done = true;
                return;
            }

            if (!IsBlocked2())
            {
                m_wasBlocked = false;
                serverConnection.SendMoveObjectRequest(m_thisID, me.Owner, m_destLocation, m_throttle);
            }
            else
            {
                m_wasBlocked = true;
            }

        }

        //Boolean IsBlocked()
        //{

        //    if (Paused)
        //    {
        //        m_blockedBy = "PAUSED";
        //        m_isBlocked = true;
        //        return m_isBlocked;
        //    }

        //    SimObject me = m_dmView.AllObjects[m_thisID];

            
        //    //SimObject dest = m_dmView.AllObjects[m_destID];

        //    LocationValue myLoc = me.Location;
        //    //LocationValue destLoc = dest.Location;
        //    LocationValue otherLocation = null;
        //    Vec2D myVec = new Vec2D(myLoc);
        //    Vec2D otherVec = null;
        //    Vec2D destVec = new Vec2D(m_destLocation);

        //    Polygon2D poly = GetPoly(myVec, destVec);

        //    foreach (String id in m_dmView.AllObjects.Keys)
        //    {
        //        if (id == m_thisID)
        //        {
        //            continue;
        //        }

        //        if (!m_objectClassesToAvoid.Contains(m_dmView.AllObjects[id].ClassName))
        //        {
        //            continue;
        //        }
        //        otherLocation = m_dmView.AllObjects[id].Location;
        //        otherVec = new Vec2D(otherLocation);

        //        if (Polygon2D.IsPointInside(poly, otherVec))
        //        {
        //            if (myVec.ScalerDistanceTo(otherVec) < m_avoidRange)
        //            {
        //                m_blockedBy = id;
        //                m_isBlocked = true;
        //                return m_isBlocked;
        //            }
        //        }
        //    }
            

        //    if (m_holdWP != null)
        //    {
        //        otherVec = new Vec2D(m_holdWP.Location);
        //        if (Polygon2D.IsPointInside(poly, otherVec))
        //        {

        //            if (myVec.ScalerDistanceTo(otherVec) < m_avoidRange)
        //            {
        //                //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.IsBlocked() {0} true", m_thisID));
        //                m_blockedBy = "HOLD";
        //                m_isBlocked = true;
        //                return m_isBlocked;
        //            }
        //        }
        //    }

        //    //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.IsBlocked() {0} false", m_thisID));
        //    m_blockedBy = String.Empty;
        //    m_isBlocked = false;
        //    return m_isBlocked;
        //}


        Boolean IsBlocked2()
        {

            if (Paused)
            {
                m_blockedBy = "PAUSED";
                m_isBlocked = true;
                return m_isBlocked;
            }

            if (!ShouldAvoid)
            {
                m_blockedBy = String.Empty; ;
                m_isBlocked = false;
                return m_isBlocked;
            }

            SimObject me = m_dmView.AllObjects[m_thisID];
            LocationValue myLoc = me.Location;
            LocationValue otherLocation = null;
            Vec2D myVec = new Vec2D(myLoc);
            Vec2D otherVec = null;
            Vec2D destVec = new Vec2D(m_destLocation);


            Vec2D farPoint = ProjectPointOnLine(myVec, destVec, m_avoidRange);

            //Polygon2D poly = GetPoly(myVec, destVec);

            foreach (String id in m_dmView.AllObjects.Keys)
            {
                if (id == m_thisID)
                {
                    continue;
                }

                if (!m_objectClassesToAvoid.Contains("ALL"))
                {
                    if (!m_objectClassesToAvoid.Contains(m_dmView.AllObjects[id].ClassName))
                    {
                        continue;
                    }
                }
                otherLocation = m_dmView.AllObjects[id].Location;
                otherVec = new Vec2D(otherLocation);

                double d = BehaviorHelper.DistanceFromPointToLineSegment(otherVec, myVec, farPoint);

                if (d < (m_avoidPathWidth / 2))
                {
                    m_blockedBy = id;
                    m_isBlocked = true;
                    return m_isBlocked;
                }
            }


            if (m_holdWP != null)
            {
                otherVec = new Vec2D(m_holdWP.Location);
                double d2 = BehaviorHelper.DistanceFromPointToLineSegment(otherVec, myVec, farPoint);

                if (d2 < (m_avoidPathWidth / 2))
                {
                    m_blockedBy = "HOLD";
                    m_isBlocked = true;
                    return m_isBlocked;
                }
            }

            //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.IsBlocked() {0} false", m_thisID));
            m_blockedBy = String.Empty;
            m_isBlocked = false;
            return m_isBlocked;
        }

        Vec2D ProjectPointOnLine(Vec2D p1, Vec2D p2, Double d)
        {
            Vec2D r = new Vec2D(0,0);
            Double d1, x1, y1, x2, y2, x3, y3;
            x1 = p1.X;
            y1 = p1.Y;
            x2 = p2.X;
            y2 = p2.Y;
            d1 = Math.Sqrt(Math.Pow(x2 -x1,2) + Math.Pow(y2 - y1,2));

            if ((y2 == y1) && (x2 == x1)) // p1 == p2 is not allowed
            {
                return null;
            }
            else
            {
                x3 = x1 + ((d * (x2 - x1)) / d1);
                y3 = y1 + ((d * (y2 - y1)) / d1);
                r.X = x3;
                r.Y = y3;

                return r;
            }

            

            
        }

        //Vec2D SmallestX(List<Vec2D> l)
        //{
        //    Vec2D r = l[0];
        //    for (int i = 1; i < l.Count; i++)
        //    {
        //        if (l[i].X < r.X)
        //        {
        //            r = l[i];
        //        }
        //    }
        //    return r;
        //}
        //Vec2D SmallestY(List<Vec2D> l)
        //{
        //    Vec2D r = l[0];
        //    for (int i = 1; i < l.Count; i++)
        //    {
        //        if (l[i].Y < r.Y)
        //        {
        //            r = l[i];
        //        }
        //    }
        //    return r;
        //}
        //Vec2D BiggestY(List<Vec2D> l)
        //{
        //    Vec2D r = l[0];
        //    for (int i = 1; i < l.Count; i++)
        //    {
        //        if (l[i].Y > r.Y)
        //        {
        //            r = l[i];
        //        }
        //    }
        //    return r;
        //}

        //Polygon2D GetPoly2(Vec2D p1, Vec2D p2)
        //{
        //    double halfwidth = m_avoidPathWidth / 2;
        //    Polygon2D poly = new Polygon2D();

        //    List<Vec2D> vertices = new List<Vec2D>();
            
        //    Vec2D v1 = GetPerpendicularPoint(p1, p2, -1* halfwidth);
        //    Vec2D v2 = GetPerpendicularPoint(p1, p2, 1 * halfwidth);
        //    Vec2D v3 = GetPerpendicularPoint(p2, p1, 1 * halfwidth);
        //    Vec2D v4 = GetPerpendicularPoint(p2, p1, -1 * halfwidth);

        //    if (v1 == null || v2 == null || v3 == null || v4 == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        vertices.Add(v1);
        //        vertices.Add(v2);
        //        vertices.Add(v3);
        //        vertices.Add(v4);

        //        Vec2D first = SmallestX(vertices);
        //        vertices.Remove(first);
        //        Vec2D second = SmallestX(vertices);
        //        vertices.Remove(second);
        //        Vec2D third;

        //        if (second.Y > first.Y)
        //        {
        //            third = BiggestY(vertices);
        //            vertices.Remove(third);
        //        }
        //        else
        //        {
        //            third = SmallestY(vertices);
        //            vertices.Remove(third);
        //        }
        //        Vec2D fourth = vertices[0];

        //        poly.AddVertex(first);
        //        poly.AddVertex(second);
        //        poly.AddVertex(third);
        //        poly.AddVertex(fourth);
        //        return poly;
        //    }

        //}

        //Vec2D GetPerpendicularPoint(Vec2D p1, Vec2D p2, double d)
        //{
        //    Vec2D p3 = new Vec2D(0, 0);

        //    p3.X = p2.X + 1;
        //    double m = -1 * ((p2.Y - p1.Y) / (p2.X - p1.X));
        //    double b = p2.Y + ((p2.Y - p1.Y) / (p2.X - p1.X));
        //    p3.Y = (m * p3.X) + b;

        //    return ProjectPointOnLine(p2, p3, d);
        //}


        Polygon2D GetPoly(Vec2D p1, Vec2D p2)
        {
            double halfwidth = m_avoidPathWidth/2;
            Polygon2D poly = new Polygon2D();

            double dx = p2.X - p1.X >= 0 ? halfwidth : halfwidth * -1;
            double dy = p2.Y - p1.Y >= 0 ? halfwidth : halfwidth * -1;

            if (Math.Abs(p2.X - p1.X) > Math.Abs(p2.Y - p1.Y))
            {
                poly.AddVertex(new Vec2D(p1.X, p1.Y - halfwidth));
                poly.AddVertex(new Vec2D(p1.X, p1.Y + halfwidth));
                poly.AddVertex(new Vec2D(p2.X+dx, p2.Y + halfwidth));
                poly.AddVertex(new Vec2D(p2.X+dx, p2.Y - halfwidth));
                return poly;
            }
            else
            {
                poly.AddVertex(new Vec2D(p1.X - halfwidth, p1.Y));
                poly.AddVertex(new Vec2D(p1.X + halfwidth, p1.Y));
                poly.AddVertex(new Vec2D(p2.X + halfwidth, p2.Y+dy));
                poly.AddVertex(new Vec2D(p2.X - halfwidth, p2.Y+dy));
                return poly;
            }
        }

        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.Update() {0}", m_thisID));
            if (m_done)
            {
                return;
            }
            SimObject me = dmView.AllObjects[m_thisID];
            //SimObject dest = dmView.AllObjects[m_destID];

            LocationValue myLocation = me.Location;
            
            VelocityValue myVelocity = me.Velocity;


            if (ObjectMath.IsWithinRange(0.1, myLocation, m_destLocation))
            {
                m_done = true;
                return;
            }

            if (!IsBlocked2())
            {
                if (m_wasBlocked)
                {
                    m_wasBlocked = false;
                    serverConnection.SendMoveObjectRequest(m_thisID, me.Owner, m_destLocation, m_throttle);
                }
                else if (myVelocity.VX == 0 && myVelocity.VY == 0 && myVelocity.VZ == 0)
                {
                    serverConnection.SendMoveObjectRequest(m_thisID, me.Owner, m_destLocation, m_throttle);
                }
            }
            else
            {
                m_wasBlocked = true;
                serverConnection.SendMoveObjectRequest(m_thisID, me.Owner,myLocation, 0);
            }

            
        }

        public bool IsDone(DDDServerConnection serverConnection, DMView dmView)
        {
            //Console.Out.WriteLine(String.Format("MoveWithAvoidanceBehavior.IsDone() {0} {1}", m_thisID,m_done));
            return m_done;
        }

        
        public void ViewProInitializeObject(SimulationEvent ev) { }
        public void ViewProAttributeUpdate(SimulationEvent ev) { }
        public void ViewProMotionUpdate(SimulationEvent ev) { }
        public void ViewProAttackUpdate(SimulationEvent ev) { }
        public void ViewProStopObjectUpdate(SimulationEvent ev) { }
        public void ViewProActiveRegionUpdate(SimulationEvent ev) { }
        public void AttackSucceeded(SimulationEvent ev) { }
        public String GetName()
        {
            return "MoveWithAvoidanceBehavior";
        }
        public String GetState()
        {
            if (m_isBlocked == true)
            {
                return "BLOCKED:" + m_blockedBy;
            }
            else
            {
                return "MOVING";
            }
        }

        public BehaviorStatus GetBehaviorStatus()
        {
            BehaviorStatus s = new BehaviorStatus();
            s.Name = GetName();
            s.Status = GetState();
            return s;
        }
    }
}
