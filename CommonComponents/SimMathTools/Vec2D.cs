using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimMathTools
{
    public class Vec2D
    {
        public double X, Y;

        override public string ToString()
        {
            return "[" + X.ToString() + "," + Y.ToString() + "," + "]";
        }
        public Vec2D(LocationValue v)
        {
            X = v.X;
            Y = v.Y;
        }
        public Vec2D(Vec2D v)
        {
            X = v.X;
            Y = v.Y;
        }
        public Vec2D(VelocityValue v)
        {
            X = v.VX;
            Y = v.VY;
        }

        public Vec2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Set(LocationValue v)
        {
            X = v.X;
            Y = v.Y;
        }
        public void Set(Vec2D v)
        {
            X = v.X;
            Y = v.Y;
        }
        public void Set(VelocityValue v)
        {
            X = v.VX;
            Y = v.VY;
        }

        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        public LocationValue ToLocationValue()
        {
            LocationValue v = new LocationValue();
            v.X = X;
            v.Y = Y;
            v.Z = 0;
            v.exists = true;
            return v;
        }

        public VelocityValue ToVelocityValue()
        {
            VelocityValue v = new VelocityValue();
            v.VX = X;
            v.VY = Y;
            v.VZ = 0;
            return v;
        }

        public Vec2D Multiply(double s)
        {
            Vec2D r = new Vec2D(this);
            r.X *= s;
            r.Y *= s;
            return r;
        }



        public double ScalerDistanceTo(Vec2D v2)
        {
            return Math.Sqrt(Math.Pow(v2.X - X, 2)
                             + Math.Pow(v2.Y - Y, 2));
        }

        public Vec2D VectorDistanceTo(Vec2D v2)
        {
            Vec2D r = new Vec2D(v2);
            r.X -= X;
            r.Y -= Y;
            return r;
        }

        public Vec2D Add(Vec2D v2)
        {
            Vec2D r = new Vec2D(this);
            r.X += v2.X;
            r.Y += v2.Y;
            return r;
        }

        public void Normalize()
        {
            double t = Math.Sqrt(X * X + Y * Y);
            X /= t;
            Y /= t;
            //return this;
        }
    }
}
