using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Aptima.Asim.DDD.Client.Controller
{
    public static class UTM_Mapping
    {
        public static string Path;
        public static int ImageWidth = 0;
        public static int ImageHeight = 0;

        public static float HorizonalMetersPerPixel;  // Per Pixel Width in Meters.
        public static float VerticalMetersPerPixel;   // Per Pixel Height in Meters.

        
        public static float HorizontalPixelsToMeters(float pixel_position)
        {
            return pixel_position * HorizonalMetersPerPixel;
        }
        public static int HorizontalMetersToPixels(float meters)
        {
            return (int)(meters / HorizonalMetersPerPixel);
        }
        public static float VerticalPixelsToMeters(float pixel_position)
        {
            return (ImageHeight - pixel_position) * VerticalMetersPerPixel;
            //return pixel_position * VerticalMetersPerPixel;

        }
        public static int VerticalMetersToPixels(float meters)
        {
            return (ImageHeight - (int)(meters / VerticalMetersPerPixel));
            //return (int)(meters / VerticalMetersPerPixel);
        }
        public static float VelocityToPixels(float meters_per_second)
        {
            return (meters_per_second / ((HorizonalMetersPerPixel + VerticalMetersPerPixel) / 2) );
        }
    }
}
