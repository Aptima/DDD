using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Aptima.Visualization.Utility
{
    /// <summary>
    /// Class used to provide helper color methods:
    ///    computing RGB color values from HSV color values
    /// </summary>
    public class ColorUtils
    {
        /// <summary>
        /// Computes Color object from HSV values, using the standard HSV color transformation.
        /// </summary>
        /// <param name="h">The hue.</param>
        /// <param name="s">The saturation.</param>
        /// <param name="v">The value.</param>
        /// <returns></returns>
        public static Color computeFromHSV(double h, double s, double v)
        {
            double f, p, q, t;
            int hi;

            hi = (int)(Math.Floor(h / 60.0)) % 6;
            f = h / 60.0 - Math.Floor(h / 60.0);
            p = v * (1.0 - s);
            q = v * (1.0 - f * s);
            t = v * (1.0 - (1.0 - f) * s);

            v *= 256;
            q *= 256;
            t *= 256;
            p *= 256;

            switch (hi)
            {
                case 0: return Color.FromRgb((byte)v, (byte)t, (byte)p);
                case 1: return Color.FromRgb((byte)q, (byte)v, (byte)p);
                case 2: return Color.FromRgb((byte)p, (byte)v, (byte)t);
                case 3: return Color.FromRgb((byte)p, (byte)q, (byte)v);
                case 4: return Color.FromRgb((byte)t, (byte)p, (byte)v);
                case 5: return Color.FromRgb((byte)v, (byte)p, (byte)q);
            }

            //Return the color black if everything has gone awry in the switch statement above
            //this would be considered an error condition and in a sane world would throw an exception -
            //but I don't want to deal with C#'s 'interesting' exception handling mechanism.
            return Color.FromRgb(0, 0, 0);
        }

        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        static public void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static private int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
