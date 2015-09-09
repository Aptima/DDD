using System;
using System.Collections.Generic;


namespace VSG.CoordinateTransform
{
    /// <summary>
    /// IMplements a test for convexity of a polygonal region. Reference
    /// http://arxiv.org/PS_cache/cs/pdf/0609/0609141.pdf
    /// POLYGON CONVEXITY: A MINIMAL O(n) TEST
    /// IOSIF PINELIS
    /// </summary>
    public class GeometricPointType
    {
        private double x;
        public double X
        {
            get { return x; }
        }
        private double y;
        public double Y
        {
            get { return y; }
        }
        public double SlopeTo(GeometricPointType p)
        {
            if (this.x == p.X)
            {
                throw new ApplicationException("Cannot compute vertical slope from "
                    + "(" + this.x.ToString() + this.y.ToString() + ") to "
                    + "(" + p.X.ToString() + p.Y.ToString() + ") in GeometricGeometricPointType.SlopeTo");
            }

            return (p.Y - this.y) / (p.X - this.x);
        }

        public GeometricPointType(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public static class ConvexTest
    {
        /// <summary>
        /// Removes the middle of three collinear points from a polygon
        /// </summary>
        /// <param name="inLine">List of points</param>
        /// <returns>reduced list of points</returns>
        public static List<GeometricPointType> RemoveCollinear(List<GeometricPointType> inLine)
        {
            List<GeometricPointType> outLine = new List<GeometricPointType>();
            int N = inLine.Count;
            outLine.Add(inLine[0]);
            int nextPoint = 1;
            int first = 0; // represents the first of three successive points
            while (nextPoint < N)
            {
                GeometricPointType start = inLine[first];
                GeometricPointType middle = inLine[(nextPoint) % N];
                GeometricPointType endpoint = inLine[(nextPoint + 1) % N];
                if ((middle.X == endpoint.X) && (middle.X == start.X))
                {
                    /* do nothing*/
                    ;
                }
                else if (middle.X == endpoint.X || middle.X == start.X)
                {
                    if (N != nextPoint)
                    {
                        outLine.Add(inLine[nextPoint]);
                        first = nextPoint;
                    }
                }
                else if (middle.SlopeTo(start) == middle.SlopeTo(endpoint))
                {
                    /* do nothing*/
                    ;
                }

                else
                {
                    if (N != nextPoint)
                    {
                        outLine.Add(inLine[nextPoint]);
                        first = nextPoint;
                    }


                }

                nextPoint = nextPoint + 1;

            }//end while
            return outLine;

        }

        /// <summary>
        /// Calculates the sign of the 3x3 determinant whose first column 1 all 1, and
        /// whose second and third columns consist of the three points whose indices are given
        /// stacked one atop the next
        /// </summary>
        /// <param name="inLine"></param>
        /// <param name="alpha"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static int DetSign(List<GeometricPointType> inLine, int alpha, int i, int j)
        {
            double detValue = inLine[i].X * inLine[j].Y
                         + inLine[alpha].X * inLine[i].Y
                         + inLine[j].X * inLine[alpha].Y
                     - (inLine[alpha].Y * inLine[i].X
                         + inLine[i].Y * inLine[j].X
                         + inLine[j].Y * inLine[alpha].X
                        );
            if (detValue > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
            // in this convexity test  zero is not possible

        }
        public static Boolean IsConvex(List<GeometricPointType> inLine)
        {
            Dictionary<int, int> a = new Dictionary<int, int>();
            Dictionary<int, int> b = new Dictionary<int, int>();
            Dictionary<int, int> c = new Dictionary<int, int>();
            List<GeometricPointType> trimmed = RemoveCollinear(inLine);
            int N = trimmed.Count;
            for (int i = 2; i < N; i++)
            {
                a.Add(i, DetSign(trimmed, (i + 1) % N, i - 1, i));
                // Use of modulus not explicit in paper, but implied.
                b.Add(i, DetSign(trimmed, 0, i - 1, i));
                c.Add(i, DetSign(trimmed, i, 0, 1));
            }
            for (int i = 2; i < N - 1; i++) // test only involves these, but need b[i+1] and c[i+1]
            {
                if ((a[i] * b[i] < 0)
                    || (a[i] * b[i + 1] < 0)
                    || (c[i] * c[i + 1] < 0)
                    )
                    return false;
            }
            return true;
        }
    }

}
