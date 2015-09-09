using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AME.Views.View_Components.CoordinateTransform
{
    /// <summary>
    /// Default CoordinateTransform for the AME Diagram.  Returns coordinates sent to it
    /// without modifying them.
    /// </summary>
    public class IdentityCoordinateTransform : ICoordinateTransform
    {
        public IdentityCoordinateTransform() { }

        #region ICoordinateTransform Members

        public float StoreX(float x)
        {
            return x;
        }

        public float StoreY(float y)
        {
            return y;
        }

        public float RetrieveX(float x)
        {
            return x;
        }

        public float RetrieveY(float y)
        {
            return y;
        }

        public void PostProcessPolygon(List<PointF> points, String componentType) {}

        #endregion
    }
}
