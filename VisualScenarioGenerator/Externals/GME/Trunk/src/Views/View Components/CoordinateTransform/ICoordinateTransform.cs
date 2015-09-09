using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace AME.Views.View_Components.CoordinateTransform
{
    /// <summary>
    /// The AME diagram will accept objects (CoordinateTransform property) that implement this 
    /// interface to allow them to perform transformations on XY coordinates coming out of and 
    /// into the AME Model.
    /// </summary>
    public interface ICoordinateTransform
    {
        float StoreX(float x);
        float StoreY(float y);

        float RetrieveX(float x);
        float RetrieveY(float y);

        void PostProcessPolygon(List<PointF> points, String componentType);
    }
}
