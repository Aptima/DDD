using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace Aptima.Visualization
{
    /// <summary>
    /// This class extends shape and can draw a wedge or an arc (or a full circle, if you're so inclined).
    /// Note that WPF uses DEGREES for all angles, while the trigonemtric functions in Math.* all use RADIANS.
    /// This class assumes all angles are given in DEGREES.
    /// </summary>
    class Wedge : Shape
    {
        /// <summary>
        /// Gets or sets the inner radius of the wedge (0 to start at the circle's center).
        /// </summary>
        /// <value>The inner radius of the wedge.</value>
        public double innerRadius { get; set;}
        /// <summary>
        /// Gets or sets the outer radius of the wedge.
        /// </summary>
        /// <value>The outer radius of the wedge.</value>
        public double outerRadius { get; set; }
        /// <summary>
        /// Gets or sets the sweep of the wedge (in degrees) 360.0 for a full circle.
        /// </summary>
        /// <value>The sweep of the wedge.</value>
        public double sweep { get; set; }
        /// <summary>
        /// Gets or sets the center of the wedge (where in the canvas it will be displayed).
        /// </summary>
        /// <value>The center.</value>
        public Point  center { get; set; }
        /// <summary>
        /// Gets or sets the rotation angle of the wedge (where within the circle will the wedge be displayed).
        /// </summary>
        /// <value>The rotation angle of the wedge.</value>
        public double rotationAngle { get; set; }
        
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

        /// <summary>
        /// Draws the primitive geometric components of the wedge.
        /// </summary>
        /// <param name="context">The context.</param>
        public void InternalDrawGeometry(StreamGeometryContext context)
        {
            Point startPoint = center;

            Point innerArcStartPoint = ComputeCartesianCoordinate(rotationAngle, innerRadius);
            innerArcStartPoint.Offset(center.X, center.Y);

            Point innerArcEndPoint = ComputeCartesianCoordinate(rotationAngle + sweep, innerRadius);
            innerArcEndPoint.Offset(center.X, center.Y);

            Point outerArcStartPoint = ComputeCartesianCoordinate(rotationAngle, /*innerRadius +*/ outerRadius);
            outerArcStartPoint.Offset(center.X, center.Y);

            Point outerArcEndPoint = ComputeCartesianCoordinate(rotationAngle + sweep, /*innerRadius +*/ outerRadius);
            outerArcEndPoint.Offset(center.X, center.Y);

            bool largeArc = sweep > 180.0;

           /*
                Point offset = ComputeCartesianCoordinate(rotationAngle + sweep / 2, PushOut);
                innerArcStartPoint.Offset(offset.X, offset.Y);
                innerArcEndPoint.Offset(offset.X, offset.Y);
                outerArcStartPoint.Offset(offset.X, offset.Y);
                outerArcEndPoint.Offset(offset.X, offset.Y);
            */

            Size outerArcSize = new Size(/*innerRadius +*/ outerRadius, /*innerRadius +*/ outerRadius);
            Size innerArcSize = new Size(innerRadius, innerRadius);

            context.BeginFigure(innerArcStartPoint, true, true);
            context.LineTo(outerArcStartPoint, true, true);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
            context.LineTo(innerArcEndPoint, true, true);
            context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
        }

        /// <summary>
        /// Helper method that computes the a cartesian coordinate based on an angle and radius of the wedge.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="radius">The radius.</param>
        /// <returns></returns>
        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}
