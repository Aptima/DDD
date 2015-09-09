using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace Aptima.Visualization
{
    class Star : Shape
    {
        public Point center { get; set; }
        public int numPoints { get; set; }
        public double radius { get; set; }
        public List<Point> tips { get; set; }
        public List<Point> points { get; set; }

        public List<double> radii { get; set; }

        protected override Geometry DefiningGeometry
        {

            get
            {
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void InternalDrawGeometry(StreamGeometryContext context)
        {
            generateGeometry();

            context.BeginFigure(tips[0], true, true);
            for (int x = 1; x < points.Count(); x++)
            {
                context.LineTo(points[x], true, true);
            }
        }

        public void generateGeometry()
        {
            double theta = (2.0 * Math.PI / numPoints);
            //double r = (radius / numPoints) / (2 * Math.Sin(theta / 2));

            tips = new List<Point>();
            points = new List<Point>();

            //generate the tips of the stars
            for (int i = 0; i < numPoints; i++)
            {
                Point p = new Point(center.X + radii[i] * Math.Sin(theta * i), center.Y + radii[i] * Math.Cos(theta * i));
                points.Add(p);
                tips.Add(p);

                //halfway step (inner)
                double phi = theta * i + theta / 2.0;
                p = new Point(center.X + 10.0 * Math.Sin(phi), center.Y + 10.0 * Math.Cos(phi));
                points.Add(p);
            }
        }
    }
}
