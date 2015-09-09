using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class BehaviorStatus
    {
        String m_name;
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        String m_status;
        public String Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
    }

    public interface BehaviorInterface
    {
        void Start(DDDServerConnection serverConnection, DMView dmView);
        void Update(DDDServerConnection serverConnection, DMView dmView);
        bool IsDone(DDDServerConnection serverConnection, DMView dmView);
        void ViewProInitializeObject(SimulationEvent ev);
        void ViewProAttributeUpdate(SimulationEvent ev);
        void ViewProMotionUpdate(SimulationEvent ev);
        void ViewProAttackUpdate(SimulationEvent ev);
        void ViewProStopObjectUpdate(SimulationEvent ev);
        void ViewProActiveRegionUpdate(SimulationEvent ev);
        void AttackSucceeded(SimulationEvent ev);
        String GetName();
        String GetState();
        BehaviorStatus GetBehaviorStatus();
    }

    public class BehaviorHelper
    {
        static public bool LocationIsEqual(LocationValue loc1, LocationValue loc2)
        {
            return LocationIsEqual(loc1, loc2, 0.1);
        }
        static public bool LocationIsEqual(LocationValue loc1, LocationValue loc2,Double threshhold)
        {
            return ObjectMath.IsWithinRange(threshhold, loc1, loc2);
        }

        static public bool VelocityIsEqual(VelocityValue vel1, VelocityValue vel2, Double threshhold)
        {
            if (Math.Abs(vel1.VX - vel2.VX) > threshhold)
            {
                return false;
            }
            else if (Math.Abs(vel1.VY - vel2.VY) > threshhold)
            {
                return false;
            }
            else if (Math.Abs(vel1.VZ - vel2.VZ) > threshhold)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        static public bool VelocityIsEqual(VelocityValue vel1, VelocityValue vel2)
        {
            return VelocityIsEqual(vel1, vel2, 0.1);
        }

        static public bool StringListIsEqual(List<string> l1,List<String> l2)
        {
            if (l1.Count != l2.Count)
            {
                return false;
            }
            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                {
                    return false;
                }
            }
            return true;

        }

        static public Double Distance(LocationValue l1, LocationValue l2)
        {
            Vec3D v1 = new Vec3D(l1);
            Vec3D v2 = new Vec3D(l2);
            return v1.ScalerDistanceTo(v2);
        }



        static public VelocityValue ComputeVelocityVector(LocationValue start, LocationValue dest, Double maxSpeed, Double throttle)
        {
            Vec3D startVec = new Vec3D(start);
            Vec3D destVec = new Vec3D(dest);
            Vec3D velVec;
            if (LocationIsEqual(start, dest))
            {
                velVec = new Vec3D(0, 0, 0);
                return velVec.ToVelocityValue();
            }
            else
            {
                velVec = startVec.VectorDistanceTo(destVec);
            }

            
            velVec.Normalize();
            velVec = velVec.Multiply(maxSpeed * throttle);

            //if (System.Double.IsNaN(velVec.X))
            //{
            //    int i = 0;
            //}

            return velVec.ToVelocityValue();
        }
        static public LocationValue LineIntersect(LocationValue p1, LocationValue p2, LocationValue p3, LocationValue p4)
        {
            Double x1, x2, x3, x4, y1, y2, y3, y4;
            x1 = p1.X;
            y1 = p1.Y;
            x2 = p2.X;
            y2 = p2.Y;
            x3 = p3.X;
            y3 = p3.Y;
            x4 = p4.X;
            y4 = p4.Y;

            Double uanum = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
            Double ubnum = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
            Double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            if (uanum == 0 && ubnum == 0 && denom == 0) // lines are coincident
            {
                return null;
            }
            else if (denom == 0) // lines are parallel
            {
                return null;
            }
            Double ua = uanum / denom;
            Double ub = uanum / denom;
            Double x = x1 + ua * (x2 - x1);
            Double y = y1 + ua * (y2 - y1);

            if (x < Math.Min(x1, x2)
                || x > Math.Max(x1, x2)
                || x < Math.Min(x3, x4)
                || x > Math.Max(x3, x4))
            {
                return null;
            }
            if (y < Math.Min(y1, y2)
                || y > Math.Max(y1, y2)
                || y < Math.Min(y3, y4)
                || y > Math.Max(y3, y4))
            {
                return null;
            }

            LocationValue l = new LocationValue();
            l.exists = true;
            l.X = x;
            l.Y = y;
            return l;

        }
        static public LocationValue ClosestPointOnLine(LocationValue point, LocationValue line1, LocationValue line2)
        {
            Double x0=0, y0=0;

            Double x1 = line1.X;
            Double y1 = line1.Y;
            Double x2 = line2.X;
            Double y2 = line2.Y;
            Double x3 = point.X;
            Double y3 = point.Y;

            Double dx = x2 - x1;
            Double dy = y2 - y1;

            Double t;

            if ((dx == 0) && (dy == 0))
            {
                x0 = x1;
                y0 = y1;
            }
            else
            {
                t = ((x3 - x1) * dx + (y3 - y1) * dy) / (dx * dx + dy * dy);

                t = Math.Min(Math.Max(0, t), 1.0);
                x0 = x1 + t * dx;
                y0 = y1 + t * dy;
            }

            LocationValue r = new LocationValue();
            r.exists = true;
            r.X = x0;
            r.Y = y0;
            r.Z = 0;
            return r;
            
            //Vec3D p = new Vec3D(point);
            //Vec3D l1 = new Vec3D(line1);
            //Vec3D l2 = new Vec3D(line2);

            //Vec3D u = l2.Subtract(l1);
            //Vec3D v = p.Subtract(l1);

            //u.Normalize();

            //Vec3D r = u.Multiply((u.X*v.X)+(u.Y*v.Y)+(u.Z*v.Z));
            //return r.ToLocationValue();


        }

        static double Vec2DLength(Vec2D v)
        {
            return Math.Sqrt(v.X*v.X + v.Y*v.Y);
        }
        static double DotProduct(Vec2D v1, Vec2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        static public double DistanceFromPointToLineSegment(Vec2D point, Vec2D anchor, Vec2D end)
        {
            Vec2D d = end.Add(anchor.Multiply(-1));
            
            double length = Vec2DLength(d);
            if (length == 0) return Vec2DLength(point.Add(anchor.Multiply(-1)));
            d.Normalize();
            double intersect = DotProduct(point.Add(anchor.Multiply(-1)), d);
            if (intersect < 0) return Vec2DLength(point.Add(anchor.Multiply(-1)));
            if (intersect > length) return Vec2DLength(point.Add(end.Multiply(-1)));
            return Vec2DLength(point.Add(anchor.Add(d.Multiply(intersect)).Multiply(-1)));
        }
    }
}
