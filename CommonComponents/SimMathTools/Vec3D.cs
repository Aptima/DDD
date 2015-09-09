using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimMathTools
{
    public class Vec3D
    {
        public double X, Y, Z;

        override public string ToString()
        {
            return "[" + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + "]";
        }
        public Vec3D(LocationValue v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
        public Vec3D(Vec3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
        public Vec3D(VelocityValue v)
        {
            X = v.VX;
            Y = v.VY;
            Z = v.VZ;
        }

        public Vec3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(LocationValue v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
        public void Set(Vec3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
        public void Set(VelocityValue v)
        {
            X = v.VX;
            Y = v.VY;
            Z = v.VZ;
        }

        public void Set(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public LocationValue ToLocationValue()
        {
            LocationValue v = new LocationValue();
            v.X = X;
            v.Y = Y;
            v.Z = Z;
            v.exists = true;
            return v;
        }

        public VelocityValue ToVelocityValue()
        {
            VelocityValue v = new VelocityValue();
            v.VX = X;
            v.VY = Y;
            v.VZ = Z;
            return v;
        }

        public Vec3D Multiply(double s)
        {
            Vec3D r = new Vec3D(this);
            r.X *= s;
            r.Y *= s;
            r.Z *= s;
            return r;
        }
        
        

        public double ScalerDistanceTo(Vec3D v2)
        {
            return Math.Sqrt(Math.Pow(v2.X - X, 2)
                             + Math.Pow(v2.Y - Y, 2)
                             + Math.Pow(v2.Z - Z, 2));
        }

        public Vec3D VectorDistanceTo(Vec3D v2)
        {
            Vec3D r = new Vec3D(v2);
            r.X -= X;
            r.Y -= Y;
            r.Z -= Z;
            return r;
        }

        public Vec3D Add(Vec3D v2)
        {
            Vec3D r = new Vec3D(this);
            r.X += v2.X;
            r.Y += v2.Y;
            r.Z += v2.Z;
            return r;
        }
        public Vec3D Subtract(Vec3D v2)
        {
            Vec3D r = new Vec3D(this);
            r.X -= v2.X;
            r.Y -= v2.Y;
            r.Z -= v2.Z;
            return r;
        }

        public void Normalize()
        {
            double t = Math.Sqrt(X * X + Y * Y + Z * Z);
            X /= t;
            Y /= t;
            Z /= t;
            //return this;
        }
    }
}
