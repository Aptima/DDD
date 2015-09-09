using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Aptima.Visualization.Data;
using Aptima.Visualization.Utility;

namespace Aptima.Visualization
{
    /// <summary>
    /// This builds a highly modified version of a radar chart
    /// It uses a hierarchical tree to build pie-slices, which in turn each contain rings,
    /// which in turn contain values (which are displayed as wedges in each ring, in each pie slice.
    /// </summary>
    public class Radar : Visualization
    {
        private static string name = "radar";
        private static double maxRadius = 100.0;
        private static double minRadius = 20.0;
        public Point center { get; set; }

        private List<Wedge> wedges = new List<Wedge>();

        public List<PieSlice> slices { get; set; }

        public Radar() : base()
        {
            slices = new List<PieSlice>();
            wedges = new List<Wedge>();
            center = new Point();
        }

        public override string getName()
        {
            return name;
        }

        public override void generate()
        {
            //remove any old child graphical elements
            Children.Clear();

            //figure out the number of pie slices we will have
            int sliceCount = slices.Count();

            double angle = 0.0;

            double sliceIncrement = 360.0 / sliceCount;

            for (int s = 0; s < sliceCount; s++)
            {

                //determine the number of rings that we will have
                int ringCount = slices[s].rings.Count();

                //determine the ring increment
                double ringIncrement = (maxRadius - minRadius) / ringCount;
                double innerRadius = minRadius;

                for (int r = 0; r < ringCount; r++)
                {

                    //determine the number of wedges per ring
                    int wedgeCount = slices[s].rings[r].values.Count();

                    //determine the sweep increment
                    double wedgeIncrement = sliceIncrement / wedgeCount;

                    double wedgeAngle = angle;

                    double max = slices[s].rings[r].values.Max();

                    for (int w = 0; w < wedgeCount; w++)
                    {
                        //create a wedge...
                        Wedge wedge = new Wedge();
                        wedge.center = center;
                        wedge.innerRadius = innerRadius;
                        wedge.outerRadius = innerRadius + ((slices[s].rings[r].values[w] / max) * ringIncrement); //this needs to be adjusted to compare amongst the multiple data values at this level - max = 100%, then scale down from there...
                        wedge.sweep = wedgeIncrement;
                        wedge.rotationAngle = wedgeAngle;

                        wedge.Fill = new SolidColorBrush(ColorUtils.computeFromHSV(220.0, (double)w / (double)(wedgeCount), 0.85));
                        wedge.Stroke = System.Windows.Media.Brushes.Black;
                        wedge.StrokeThickness = 0.5;

                        Children.Add(wedge);

                        wedgeAngle += wedgeIncrement;
                    }

                    innerRadius += ringIncrement;
                }

                angle += sliceIncrement;
            }

            //draw all the circles...
            //draw outer circle
            Ellipse e = new Ellipse();
            e.Width = maxRadius;
            e.Height = maxRadius;
            e.Stroke = System.Windows.Media.Brushes.Black;
            e.StrokeThickness = 1.0;
            e.RenderTransform = new TranslateTransform(center.X, center.Y);

            //Children.Add(e);

            //draw inner circle
            e = new Ellipse();
            e.Width = minRadius;
            e.Height = minRadius;
            e.Stroke = System.Windows.Media.Brushes.Black;
            e.StrokeThickness = 1.0;
            e.RenderTransform = new TranslateTransform(center.X, center.Y);

            //Children.Add(e);

            for (int x = 0; x < sliceCount; x++)
            {
                Line l = new Line();
                l.X1 = minRadius * Math.Cos((x * (2 * Math.PI / sliceCount)));
                l.Y1 = minRadius * Math.Sin(x * (2 * Math.PI / sliceCount));
                l.X2 = maxRadius * Math.Cos(x * (2 * Math.PI / sliceCount));
                l.Y2 = maxRadius * Math.Sin(x * (2 * Math.PI / sliceCount));
                l.Stroke = System.Windows.Media.Brushes.Purple;

                l.StrokeThickness = 1.0;
                l.RenderTransform = new TranslateTransform(center.X, center.Y);
                //Children.Add(l);
            }

        }
    }
}
