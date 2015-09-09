using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.CommonComponents.SimMathTools
{
    public class Polygon3D
    {
        private Polygon2D footprint;
        private double bottom;
        private double top;

        public Polygon2D Footprint
        {
            get
            {
                return footprint;
            }
        }

        public Polygon3D(double z1,double z2)
        {
            footprint = new Polygon2D();
            bottom = z1;
            top = z2;
        }

        public void AddVertex(Vec2D p)
        {
            footprint.AddVertex(p);
        }

        public Double BottomZ
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
        }
        public Double TopZ
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }
        public static Vec3D FindIntersect(Polygon3D poly, Vec3D fromP, Vec3D toP)
        {
            Vec2D intersect = Polygon2D.FindIntersect(poly.footprint,
                                                      new Vec2D(fromP.X, fromP.Y),
                                                      new Vec2D(toP.X, toP.Y));

            if (intersect != null)
            {
                return new Vec3D(intersect.X, intersect.Y, fromP.Z);
            }
            else
            {
                return null;
            }
        }

        public static bool IsPointInside(Polygon3D poly, Vec3D p)
        {
            if (p.Z < poly.bottom || p.Z > poly.top)
            {
                return false;
            }

            return Polygon2D.IsPointInside(poly.footprint, new Vec2D(p.X, p.Y));
        }

        
        public static bool DoesLineCross(Polygon3D poly, Vec3D fromP, Vec3D toP)
        {
            if (fromP.Z < poly.bottom || fromP.Z > poly.top)
            {
                return false;
            }
            if (toP.Z < poly.bottom || toP.Z > poly.top)
            {
                return false;
            }
            return Polygon2D.DoesLineCross(poly.footprint, new Vec2D(fromP.X, fromP.Y), new Vec2D(toP.X, toP.Y));
        }
        public static bool SensorDoesLineCross(Polygon3D poly, Vec3D fromP, Vec3D toP)
        {
            if (fromP.Z < poly.bottom || fromP.Z > poly.top)
            {
                return false;
            }
            if (toP.Z < poly.bottom || toP.Z > poly.top)
            {
                return false;
            }
            return Polygon2D.SensorDoesLineCross(poly.footprint, new Vec2D(fromP.X, fromP.Y), new Vec2D(toP.X, toP.Y));
        }
    }
}
