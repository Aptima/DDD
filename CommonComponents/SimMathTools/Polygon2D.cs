using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimMathTools
{
    public class Polygon2D
    {
        class Line
        {
            public Vec2D p1;
            public Vec2D p2;

            public Line(Vec2D p1, Vec2D p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
            public Line()
            {
                this.p1 = null;
                this.p2 = null;
            }
        }
        private List<double> xPoints = new List<double>();
        private List<double> yPoints = new List<double>();
        private double bboxXMin;
        private double bboxXMax;
        private double bboxYMin;
        private double bboxYMax;

        private static System.Random random = new Random();
        public Polygon2D()
        {
            //xPoints = new List<double>();
            //yPoints = new List<double>();
        }

        public Polygon2D(PolygonValue value)
        {
            this.FromPolygon(value);
        }
        

        public List<Vec2D> getVertices()
        {
            List<Vec2D> points = new List<Vec2D>();
            for (int i = 0; i < xPoints.Count; i++)
            {
                points.Add(new Vec2D(xPoints[i], yPoints[i]));
            }
            return points;
        }

        public PolygonValue GetPolygonValue()
        {
            PolygonValue p = new PolygonValue();

            for (int i = 0; i < xPoints.Count; i++)
            {
                p.points.Add(new PolygonValue.PolygonPoint(xPoints[i], yPoints[i]));
            }

            return p;
        }

        public void AddVertex(Vec2D p)
        {
            if (xPoints.Count == 0)
            {
                bboxXMax = bboxXMin = p.X;
                bboxYMax = bboxYMin = p.Y;
            }
            xPoints.Add(p.X);
            yPoints.Add(p.Y);

            if (p.X < bboxXMin)
            {
                bboxXMin = p.X;
            }
            if (p.X > bboxXMax)
            {
                bboxXMax = p.X;
            }
            if (p.Y < bboxYMin)
            {
                bboxYMin = p.Y;
            }
            if (p.Y > bboxYMax)
            {
                bboxYMax = p.Y;
            }
        }

        public void FromPolygon(PolygonValue p)
        {
            this.xPoints.Clear();
            this.yPoints.Clear();
            foreach (PolygonValue.PolygonPoint pp in p.points)
            {
                this.AddVertex(new Vec2D(pp.X, pp.Y));
            }
        }

        public static bool DoLinesIntersect(Vec2D p1, Vec2D p2, Vec2D p3, Vec2D p4)
        {
            double x1 = p1.X;
            double x2 = p2.X;
            double x3 = p3.X;
            double x4 = p4.X;
            double y1 = p1.Y;
            double y2 = p2.Y;
            double y3 = p3.Y;
            double y4 = p4.Y;

            double ua = (((x4 - x3) * (y1 - y3)) - ((y4 - y3) * (x1 - x3))) / (((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1)));
            double ub = (((x2 - x1) * (y1 - y3)) - ((y2 - y1) * (x1 - x3))) / (((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1)));

            double x = x1 + (ua * (x2 - x1));
            double y = y1 + (ub * (y2 - y1));




            bool xr = Math.Min(x1, x2) <= x && x <= Math.Max(x1, x2);
            bool yr = Math.Min(y1, y2) <= y && y <= Math.Max(y1, y2);


            return xr && yr;
        }

        public static Vec2D FindIntersect(Polygon2D poly1, Polygon2D poly2)
        {
            List<Vec2D> intersections = new List<Vec2D>(); //with polys, we might have 2 intersections, should we find the middle of the two?
            List<Vec2D> p2Points = poly2.getVertices();
            Vec2D intersect = null;
            for (int i = 0; i < p2Points.Count - 1; i++)
            {
                intersect = FindIntersect(poly1, p2Points[i], p2Points[i + 1]);
                if (intersect != null)
                    intersections.Add(intersect);
            }
            //then once more from len-1 to 0
            intersect = FindIntersect(poly1, p2Points[p2Points.Count - 1], p2Points[0]);
            if (intersect != null)
                intersections.Add(intersect);

            if (intersections.Count > 0)
                return AverageVectors(intersections);

            return null;
        }
        public static Vec2D FindIntersect(PolygonValue poly1, PolygonValue poly2)
        {
            List<Vec2D> intersections = new List<Vec2D>(); //with polys, we might have 2 intersections, should we find the middle of the two?
            List<PolygonValue.PolygonPoint> p2Points = poly2.points;
            Vec2D intersect = null;
            Polygon2D p2d = new Polygon2D();
            PolygonValue.PolygonPoint p0;
            PolygonValue.PolygonPoint p1;
            p2d.FromPolygon(poly1);
            for (int i = 0; i < p2Points.Count - 1; i++)
            {

                p0 = p2Points[i];
                p1 = p2Points[i + 1];
                intersect = FindIntersect(p2d, new Vec2D(p0.X, p0.Y), new Vec2D(p1.X, p1.Y));
                if (intersect != null)
                    intersections.Add(intersect);
            }
            //then once more from len-1 to 0
            p0 = p2Points[p2Points.Count - 1];
            p1 = p2Points[0];
            intersect = FindIntersect(p2d, new Vec2D(p0.X, p0.Y), new Vec2D(p1.X, p1.Y));
            //intersect = FindIntersect(poly1, p2Points[p2Points.Count - 1], p2Points[0]);
            if (intersect != null)
                intersections.Add(intersect);

            if (intersections.Count > 0)
                return AverageVectors(intersections);

            return null;
        }
        private static Vec2D AverageVectors(List<Vec2D> listOfVectors)
        {
            Vec2D returnValue = new Vec2D(0, 0);
            double totalX = 0.0;
            double totalY = 0.0;
            int count = 0;
            foreach (Vec2D v in listOfVectors)
            {
                totalX += v.X;
                totalY += v.Y;
                count++;
            }

            returnValue.X = totalX / (double)count;
            returnValue.Y = totalY / (double)count;

            return returnValue;
        }
        public static Vec2D FindIntersect(Polygon2D poly, Vec2D fromP, Vec2D toP)
        {
            Line line = new Line(fromP, toP);

            int k, j;
            Line line2 = new Line();

            int count = 0;

            Vec2D intersect = null;

            for (k = 0; k < poly.xPoints.Count; k++)
            {
                j = k - 1;
                if (j < 0)
                {
                    j = poly.xPoints.Count - 1;
                }

                line2.p1 = new Vec2D(poly.xPoints[k], poly.yPoints[k]);
                line2.p2 = new Vec2D(poly.xPoints[j], poly.yPoints[j]);


                intersect = FindIntersect2(fromP, toP, line2.p1, line2.p2); //AD: Changed to new method for now.
                if (intersect != null)
                {
                    return intersect;
                }

            }

            return null;

        }

        public static Vec2D FindIntersect2(Vec2D from1, Vec2D to1, Vec2D from2, Vec2D to2)
        {
            Vec2D E = new Vec2D(to1.X - from1.X, to1.Y - from1.Y);
            Vec2D F = new Vec2D(to2.X - from2.X, to2.Y - from2.Y);
            Vec2D P = new Vec2D(-1 * E.Y, E.X);
            double h = (VectorDotProduct(new Vec2D(from1.X - from2.X, from1.Y - from2.Y), P) / VectorDotProduct(F, P));
            double FxP = VectorDotProduct(F, P);
            if ((h < 0 || h > 1) && FxP != 0.0)
            {
                return null;
            }
            else if (FxP == 0.0)
            {
                //only equal if both line segments are overlapping
            }

            double x = from2.X + F.X * h;//Cx + Dx*h;
            double y = from2.Y + F.Y * h;//Cy + Dy*h
            bool lineOneIsVertical = to1.X == from1.X;
            bool lineTwoIsVertical = to2.X == from2.X;

            // if (lineOneIsVertical || lineTwoIsVertical)
            //{
            //Because line one is vertical we want to check that the intersection point (x,y) crosses both y values for the segment
            //this is important because vertical lines don't have a slope, and will trick the above calculation to returning TRUE when the segment
            //doesn't cross the second segment
            if ((Math.Min(from2.Y, to2.Y) <= y && y <= Math.Max(from2.Y, to2.Y) && Math.Min(from1.Y, to1.Y) <= y && y <= Math.Max(from1.Y, to1.Y)) &&
        (Math.Min(from2.X, to2.X) <= x && x <= Math.Max(from2.X, to2.X) && Math.Min(from1.X, to1.X) <= x && x <= Math.Max(from1.X, to1.X)))
            {
                //we're good
            }
            else
            {
                return null;
            }
            // }

            return new Vec2D(x, y);
        }
        public static Vec2D FindIntersect(Vec2D from1, Vec2D to1, Vec2D from2, Vec2D to2)
        {
            double x1 = from1.X;
            double y1 = from1.Y;

            double x2 = to1.X;
            double y2 = to1.Y;

            double x3 = from2.X;
            double y3 = from2.Y;

            double x4 = to2.X;
            double y4 = to2.Y;

            double ua = (((x4 - x3) * (y1 - y3)) - ((y4 - y3) * (x1 - x3))) / (((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1)));
            double ub = (((x2 - x1) * (y1 - y3)) - ((y2 - y1) * (x1 - x3))) / (((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1)));

            double x = x1 + (ua * (x2 - x1));
            double y = y1 + (ub * (y2 - y1));

            bool lineOneIsVertical = x2 == x1;
            bool lineTwoIsVertical = x3 == x4;


            bool xr = Math.Min(x1, x2) <= x && x <= Math.Max(x1, x2);
            bool yr = Math.Min(y1, y2) <= y && y <= Math.Max(y1, y2);

            if (lineOneIsVertical || lineTwoIsVertical)
            {
                //Because line one is vertical we want to check that the intersection point (x,y) crosses both y values for the segment
                //this is important because vertical lines don't have a slope, and will trick the above calculation to returning TRUE when the segment
                //doesn't cross the second segment
                if (Math.Min(y3, y4) <= y && y <= Math.Max(y3, y4))
                {
                    yr = yr && true;
                }
                else
                {
                    yr = yr && false;
                }
            }

            if (xr && yr)
            {
                Vec2D r = new Vec2D(x, y);
                return r;
            }
            else
            {
                return null;
            }

            //return xr && yr;
        }

        public Vec2D PointInside()
        {
            double x = random.Next((int)this.bboxXMin, (int)this.bboxXMax);
            double y = random.Next((int)this.bboxYMin, (int)this.bboxYMax);
            Vec2D vector = new Vec2D(x,y);
            if (IsPointInside(this, vector))
                return vector;
            else
                return PointInside();
        }


        public static bool IsPointInside(Polygon2D poly, Vec2D p)
        {

            // Check bounding box first

            if (p.X < poly.bboxXMin || p.X > poly.bboxXMax)
            {
                return false;
            }
            if (p.Y < poly.bboxYMin || p.Y > poly.bboxYMax)
            {
                return false;
            }

            // cast ray to random location to avoid casting through a vertex.
            Vec2D p2 = new Vec2D(p);
            p2.X = (random.NextDouble() * 1000) + poly.bboxXMax;
            p2.Y = (random.NextDouble() * 1000) + poly.bboxYMax;

            Line line = new Line(p, p2);

            int k, j;
            Line line2 = new Line();

            int count = 0;

            for (k = 0; k < poly.xPoints.Count; k++)
            {
                j = k - 1;
                if (j < 0)
                {
                    j = poly.xPoints.Count - 1;
                }

                line2.p1 = new Vec2D(poly.xPoints[k], poly.yPoints[k]);
                line2.p2 = new Vec2D(poly.xPoints[j], poly.yPoints[j]);

                if (DoLinesIntersect(line.p1, line.p2, line2.p1, line2.p2))
                {
                    count++;
                }
            }

            if ((count % 2) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        public static bool DoesLineCross(Polygon2D poly, Vec2D fromP, Vec2D toP)
        {

            // Check bounding box first
            /*
            if (toP.X < poly.bboxXMin || toP.X > poly.bboxXMax)
            {
                return false;
            }
            if (toP.Y < poly.bboxYMin || toP.Y > poly.bboxYMax)
            {
                return false;
            }
            */


            Line line = new Line(fromP, toP);

            int k, j;
            Line line2 = new Line();


            for (k = 0; k < poly.xPoints.Count; k++)
            {
                j = k - 1;
                if (j < 0)
                {
                    j = poly.xPoints.Count - 1;
                }

                line2.p1 = new Vec2D(poly.xPoints[k], poly.yPoints[k]);
                line2.p2 = new Vec2D(poly.xPoints[j], poly.yPoints[j]);

                if (DoLinesIntersect(line.p1, line.p2, line2.p1, line2.p2))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool SensorDoesLineCross(Polygon2D poly, Vec2D fromP, Vec2D toP)
        {

            Line line = new Line(fromP, toP);

            int k, j;
            Line line2 = new Line();


            for (k = 0; k < poly.xPoints.Count; k++)
            {
                j = k - 1;
                if (j < 0)
                {
                    j = poly.xPoints.Count - 1;
                }

                line2.p1 = new Vec2D(poly.xPoints[k], poly.yPoints[k]);
                line2.p2 = new Vec2D(poly.xPoints[j], poly.yPoints[j]);

                if (DoLinesIntersect(line.p1, line.p2, line2.p1, line2.p2))
                {
                    return true;
                }
            }

            return false;
        }
        public static double VectorDotProduct(Vec2D v1, Vec2D v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y);
        }
        public static Vec2D ClosestPointOnLineSegment(Vec2D p, Vec2D s0, Vec2D s1)
        {
            Vec2D v = s0.VectorDistanceTo(s1);
            Vec2D w = s0.VectorDistanceTo(p);

            double c1 = VectorDotProduct(w, v);
            if (c1 <= 0)
                return s0;

            double c2 = VectorDotProduct(v, v);
            if (c2 <= c1)
                return s1;

            double b = c1 / c2;
            Vec2D Pb = s0.Add(v.Multiply(b));

            return Pb;
        }
        public static double DistanceFromPointToSegment(Vec2D p, Vec2D s0, Vec2D s1)
        {/*
            Vec2D v = s0.VectorDistanceTo(s1);
            Vec2D w = s0.VectorDistanceTo(p);

            double c1 = VectorDotProduct(w, v);
            if (c1 <= 0)
                return p.ScalerDistanceTo(s0);// d(P, S.P0);

            double c2 = VectorDotProduct(v, v);
            if (c2 <= c1)
                return p.ScalerDistanceTo(s1);// d(P, S.P1);

            double b = c1 / c2;
            Vec2D Pb = s0.Add(v.Multiply(b));// S.P0 + b * v;
            */
            Vec2D Pb = ClosestPointOnLineSegment(p, s0, s1);
            return p.ScalerDistanceTo(Pb);// d(P, Pb);
        }
        /// <summary>
        /// Returns the distance to the nearest point of a polygon.  If the point is in the Poly, should return 0
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec2D ClosestPointOnPolygon(Polygon2D poly, Vec2D point)
        {
            double result = 999999;
            double curResult = 999999;
            Vec2D BestPoint = null, curPoint;
            Vec2D one, two, zero;
            zero = new Vec2D(point);
            for (int i = 0; i < poly.xPoints.Count - 1; i++)
            {
                //calculate [i]:[1+1]
                one = new Vec2D(poly.xPoints[i], poly.yPoints[i]);
                two = new Vec2D(poly.xPoints[i + 1], poly.yPoints[i + 1]);
                curPoint = ClosestPointOnLineSegment(zero, one, two);
                curResult = zero.ScalerDistanceTo(curPoint);
                if (result > curResult)
                {
                    result = curResult; //do we save the point where they intersect?
                    BestPoint = curPoint;
                }
            }
            //then [Count-1]:[0]
            one = new Vec2D(poly.xPoints[poly.xPoints.Count - 1], poly.yPoints[poly.xPoints.Count - 1]);
            two = new Vec2D(poly.xPoints[0], poly.yPoints[0]);
            curPoint = ClosestPointOnLineSegment(zero, one, two);
            curResult = zero.ScalerDistanceTo(curPoint);
            if (result > curResult)
            {
                result = curResult; //do we save the point where they intersect?
                BestPoint = curPoint;
            }

            return BestPoint;
        }
        /// <summary>
        /// Returns the distance to the nearest point of a polygon.  If the point is in the Poly, should return 0
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double ScalarDistanceToPolygon(Polygon2D poly, Vec2D point)
        {
            if (IsPointInside(poly, point))
            {
                return 0.0;
            }

            double result = 999999;
            double curResult = 999999;
            Vec2D one, two, zero;
            zero = new Vec2D(point);
            for (int i = 0; i < poly.xPoints.Count - 1; i++)
            {
                //calculate [i]:[1+1]
                one = new Vec2D(poly.xPoints[i], poly.yPoints[i]);
                two = new Vec2D(poly.xPoints[i + 1], poly.yPoints[i + 1]);
                //curResult = Math.Abs((two.X-one.X)*(one.Y-zero.Y)-(one.X-zero.X)*(two.Y-one.Y))/Math.Sqrt(Math.Pow(two.X-one.X,2)+Math.Pow(two.Y-one.Y,2));
                curResult = DistanceFromPointToSegment(zero, one, two);
                //curResult = (poly.yPoints[i + 1] - poly.yPoints[i]) * (point.X - poly.xPoints[i]) - (poly.xPoints[i + 1] - poly.xPoints[i]) * (point.Y - poly.yPoints[i]);
                //Math.Sqrt(Math.Pow((two.Y-one.Y)*(zero.X-one.X)+(two.X-one.X)*(zero.Y-one.Y),2)/(Math.Pow(two.X-one.X,2)+Math.Pow(two.Y-one.Y,2)));
                if (result > curResult)
                {
                    result = curResult; //do we save the point where they intersect?
                }
            }
            //then [Count-1]:[0]
            //curResult = (poly.yPoints[0] - poly.yPoints[poly.xPoints.Count - 1]) * (point.X - poly.xPoints[poly.xPoints.Count - 1]) - (poly.xPoints[0] - poly.xPoints[poly.xPoints.Count - 1]) * (point.Y - poly.yPoints[poly.xPoints.Count - 1]);
            one = new Vec2D(poly.xPoints[poly.xPoints.Count - 1], poly.yPoints[poly.xPoints.Count - 1]);
            two = new Vec2D(poly.xPoints[0], poly.yPoints[0]);
            curResult = DistanceFromPointToSegment(zero, one, two);
            if (result > curResult)
            {
                result = curResult; //do we save the point where they intersect?
            }

            return result;
        }
    }
}






