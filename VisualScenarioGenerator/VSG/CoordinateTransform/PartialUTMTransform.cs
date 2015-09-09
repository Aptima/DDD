using System;
using System.Collections.Generic;
using System.Text;
using AME.Views.View_Components.CoordinateTransform;
using AGT.Mapping;
using System.Drawing;
using System.Windows.Forms;

namespace VSG.CoordinateTransform
{
    // uses horizontal and vertical meters per pixel
    public class PartialUTMTransform : ICoordinateTransform
    {
        public PartialUTMTransform(int imageWidth, int imageHeight, float horizontalMPP, float verticalMPP)
        {
            UTM_Mapping.ImageWidth = imageWidth;
            UTM_Mapping.ImageHeight = imageHeight;
            UTM_Mapping.HorizonalMetersPerPixel = horizontalMPP;
            UTM_Mapping.VerticalMetersPerPixel = verticalMPP;
        }

        #region ICoordinateTransform Members

        public float StoreX(float x) 
        {
            return UTM_Mapping.HorizontalPixelsToMeters(x);
        }

        public float StoreY(float y)
        {
            return UTM_Mapping.VerticalPixelsToMeters(y);
        }

        public float RetrieveX(float x) 
        {
            return UTM_Mapping.HorizontalMetersToPixels(x);
        }

        public float RetrieveY(float y)
        {
            return UTM_Mapping.VerticalMetersToPixels(y);
        }

        public void PostProcessPolygon(List<PointF> points, String componentType)
        {
            if (componentType == "ActiveRegion") // only check Active Zones
            // this string match will need to change if the type changes...
            {
                List<GeometricPointType> forConvexTest = new List<GeometricPointType>();

                foreach (PointF point in points)
                {
                    forConvexTest.Add(new GeometricPointType(point.X, point.Y));
                }

                if (!ConvexTest.IsConvex(forConvexTest))
                {
                    MessageBox.Show("Active Region polygons must be convex.  Please ensure that no section of the polygon points inward");
                }
            }
        }

        #endregion
    }
}
