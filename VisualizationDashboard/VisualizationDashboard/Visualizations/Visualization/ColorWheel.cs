using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using Aptima.Visualization.Utility;

namespace Aptima.Visualization
{
    class ColorWheel : Visualization
    {
        public Point center { get; set; }

        //collection of value pairs (tuples)
        //really needs to be a data type - see generate method at bottom...
        public List<double> innerValues { get; set; }
        public List<double> outerValues { get; set; }

        //used to generate SmartLabels
        public List<string> innerValueLabels { get; set; }
        //used to generate SmartLabels
        public List<string> outerValueLabels { get; set; }

        public override string getName()
        {
            return "ColorWheel";
        }

        public ColorWheel() : base()
        {
        }

        override public void generate()
        {
            //remove any old child graphical elements
            Children.Clear();

            //compute total volume of pairs
            double totalVolume = innerValues.Sum<double>(x=>x);
            totalVolume += outerValues.Sum<double>(x => x);

            //compute pair volumes
            List<double> pairVolumes = new List<double>();
            for (int x = 0; x < innerValues.Count(); x++)
            {
                pairVolumes.Add(innerValues[x] + outerValues[x]);
            }

            //determine rotational angles per pair
            List<double> pairRotationalAngles = new List<double>();
            pairVolumes.ForEach(x => pairRotationalAngles.Add((x / totalVolume)*360.0)); //in degrees!

            double h = 0.0; //hue
            double s = 0.0; //saturation

            for (int x = 0; x < innerValues.Count(); x++)
            {
                //Draw the outer ring
                Wedge w = new Wedge();
                w.center = center;
                w.sweep = pairRotationalAngles[x];
                w.innerRadius = (innerValues[x] / pairVolumes[x]) * 100.0;
                w.outerRadius = (outerValues[x] / pairVolumes[x]) * 100.0;
                w.rotationAngle = h;

                //vary the color by both hue and saturation (for the colorblind)
                Color c = ColorUtils.computeFromHSV(h, 1.0-s, 0.9);
                w.Fill = new SolidColorBrush(c);
                w.Stroke = System.Windows.Media.Brushes.Black;

                //use a slightly darker version of the color as the stroke on the wedge
                w.Stroke = new SolidColorBrush(scaled(c, .8));
                w.StrokeThickness = 2;

                //temporary labelling!
                w.ToolTip = outerValueLabels[x];
                this.Children.Add(w);

                //draw the inner ring
                w = new Wedge();
                w.center = center;
                w.sweep = pairRotationalAngles[x];
                w.innerRadius = 0.0;
                w.outerRadius = (innerValues[x] / pairVolumes[x]) * 100.0;
                w.rotationAngle = h;

                c = ColorUtils.computeFromHSV(h, 1.0-s, 0.75);
                w.Fill = new SolidColorBrush(c);
                w.Fill.Opacity = 1.0;
                w.Stroke = new SolidColorBrush(scaled(c, .8));
                w.StrokeThickness = 2;

                //temporary labelling!
                w.ToolTip = innerValueLabels[x];
                this.Children.Add(w);

                h += pairRotationalAngles[x];

                s += pairVolumes[x] / totalVolume;
            }
        }

        private Color scaled(Color c, double p)
        {
            return Color.FromRgb((byte)(c.R * p), (byte)(c.G * p), (byte)(c.B * p));
        }
    }
}
