using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace VSG.Helpers
{
    class PolygonHelper
    {
        public static bool DeriveAbsoluteVerticesFromRelative(String currentReferencePoint, String relativeVertices, out String absoluteVertexList)
        {
            StringBuilder vertices = new StringBuilder();
            char[] cS = {' '};
            String[] rpSplit = currentReferencePoint.Split(cS, StringSplitOptions.RemoveEmptyEntries);
            if (rpSplit.Length != 2)
            {
                absoluteVertexList = "";
                return false;
            }

            Vec2D refPt = new Vec2D(double.Parse(rpSplit[0]),double.Parse(rpSplit[1]));
            String[] sS = { ")," };
            String[] vertSplit = relativeVertices.Split(sS, StringSplitOptions.RemoveEmptyEntries);
            foreach (String s in vertSplit)
            { 
                String[] pS = {","};
                String[] pt = s.Split(pS, StringSplitOptions.RemoveEmptyEntries);
                Vec2D v = new Vec2D(double.Parse(pt[0].Replace(',', ' ').Replace('(', ' ').Replace(')', ' ').Trim()), double.Parse(pt[1].Replace(',', ' ').Replace('(', ' ').Replace(')', ' ').Trim()));
                v.X += refPt.X;
                v.Y += refPt.Y;
                vertices.AppendFormat("({0}, {1}), ", v.X, v.Y);
            }
            if (vertices.Length > 2)
            {
                vertices.Remove(vertices.Length - 2, 1);
            }

            absoluteVertexList = vertices.ToString();
            return true;
        }

        public static bool DeriveReferenceFromVertexList(String currentReferencePoint, String vertexList, out String referencePoint, out String updatedVertexList)
        {
            referencePoint = "0 0";
            updatedVertexList = vertexList;

            if (vertexList.Trim().Length == 0)
                return false;
            //list is formed as (x, y), (x2, y2), etc
            char[] del = { ' ' };
            string[] pairDel = { ")," };
            String[] refPt = currentReferencePoint.Split(del);
            Vec2D curRefPt = new Vec2D(0, 0);
            if (refPt.Length == 2)
            {
                curRefPt = new Vec2D(Double.Parse(refPt[0].Trim()), Double.Parse(refPt[1].Trim()));
            }
            String[] pair = vertexList.Split(pairDel, StringSplitOptions.RemoveEmptyEntries);
            PolygonValue poly = new PolygonValue();

            double minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool set = false;

            foreach (String p in pair)
            {
                String[] pp = p.Replace('(', ' ').Replace(')', ' ').Split(del, StringSplitOptions.RemoveEmptyEntries);
                double x = Double.Parse(pp[0].Replace(",","").Replace("\r\n","").Trim());
                double y = Double.Parse(pp[1].Replace("\r\n", "").Trim());
                if (pp.Length == 2)
                {
                    if (!set)
                    {
                        minX = maxX = x;
                        minY = maxY = y;
                        set = true;
                    }
                    else
                    {
                        if (x < minX)
                        {
                            minX = x;
                        }
                        else if (x > maxX)
                        {
                            maxX = x;
                        }
                        if (y < minY)
                        {
                            minY = y;
                        }
                        else if (y > maxY)
                        {
                            maxY = y;
                        }
                    }
                    poly.points.Add(new PolygonValue.PolygonPoint(x, y));
                }
                else
                {
                    Console.WriteLine("Something's wrong!");
                }
            }
            Polygon2D p2d = new Polygon2D();
            p2d.FromPolygon(poly);
            if (!Polygon2D.IsPointInside(p2d, curRefPt))
            {
                //find new ref point
                Vec2D tempPt = new Vec2D((maxX + minX) / 2, (maxY + minY) / 2);
                Random r = new Random(DateTime.Now.Millisecond);
                while (!Polygon2D.IsPointInside(p2d, tempPt))
                {
                    double newX = r.NextDouble() * (maxX - minX) + minX;
                    double newY = r.NextDouble() * (maxY - minY) + minY;
                    tempPt = new Vec2D(newX, newY);
                }
                curRefPt = tempPt;
            }
            referencePoint = String.Format("{0} {1}", curRefPt.X, curRefPt.Y);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < poly.points.Count - 1; i++)
            {
                sb.AppendFormat("({0}, {1}), ", poly.points[i].X - curRefPt.X, poly.points[i].Y - curRefPt.Y);
            }
            //then once more for count -1 
            sb.AppendFormat("({0}, {1})", poly.points[poly.points.Count - 1].X - curRefPt.X, poly.points[poly.points.Count - 1].Y - curRefPt.Y);
            updatedVertexList = sb.ToString();

            return true;
        }
    }
}
