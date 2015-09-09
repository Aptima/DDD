using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects
{
    public class Obj_Polygon2D:BaseGameObject
    {
        private CustomVertex.PositionColored[] polygon;
        private Material diffuse;
        private float Area;
        private Vector2 Centroid;


        public Obj_Polygon2D(Vector3[] points, Color fill)
        {
            Centroid = new Vector2();

            diffuse = new Material();
            diffuse.Diffuse = fill;
            ColorValue c = ColorValue.FromArgb(fill.ToArgb());
            c.Alpha = 255 / 2;
            
            CalculateProperties(points);
            Console.WriteLine("Area = {0}", Area);
            Console.WriteLine("Centroid = {0}, {1}", Centroid.X, Centroid.Y);

            polygon = new CustomVertex.PositionColored[points.Length+1];
            polygon[0] = new CustomVertex.PositionColored(new Vector3(Centroid.X, Centroid.Y, 0), c.ToArgb());
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine("{0} = {1}", i, points[i].ToString());
                polygon[i+1] =
                    new CustomVertex.PositionColored(points[i], c.ToArgb());

            }

        }

        // Area of Polygon:  A = .5 * SUM of all (x[i]y[i+1] - x[i+1]y[i])
        private void CalculateProperties(Vector3[] points)
        {
            float PointDiff = 0;
            int N = points.Length;
            int j = 0;

            Centroid.X = 0;
            Centroid.Y = 0;

            for (int i = 0; i < N; i++)
            {
                j = (i + 1) % N;
                PointDiff = (points[i].X * points[j].Y)
                    - (points[j].X * points[i].Y);
                Area += PointDiff;
                Centroid.X += (points[i].X + points[j].X) * PointDiff;
                Centroid.Y += (points[i].Y + points[j].Y) * PointDiff;
            }
            Area *= .5f;

            float CentroidConst = 1 / (6 * Area);
            Centroid.X *= CentroidConst;
            Centroid.Y *= CentroidConst;
            
        }


        public override void Draw(Canvas canvas)
        {
            if (polygon.Length >= 3)
            {
                canvas.Material = diffuse;
                canvas.DrawUserPrimitives(PrimitiveType.TriangleFan,
                    polygon.Length - 2,
                    CustomVertex.PositionColored.Format,
                    polygon);
            }
            else
            {
                Console.WriteLine("Warning: not enough vertices");
            }
        }

    }
}
