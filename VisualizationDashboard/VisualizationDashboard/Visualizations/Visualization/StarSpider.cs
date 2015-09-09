using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Aptima.Visualization.Utility;

namespace Aptima.Visualization
{
    class StarSpider : Visualization
    {
        public double[][] data { get; set; }
        public Point center { get; set; }

        //column index used to identify which column
        //is used to build the star polygon
        public int starIndex { get; set; }

        public StarSpider() : base()
        { 
        }

        public override string getName()
        {
            return "Star Spider";
        }

        public override void generate()
        {
            //remove any old child graphical elements
            Children.Clear();

            //draw a scale underneath the other images
            for (double x = 10.0; x < 100.0; x+=10.0)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = x*2.0;
                ellipse.Height = x*2.0;
                ellipse.Stroke = System.Windows.Media.Brushes.LightBlue;
                ellipse.StrokeThickness = 0.5;
                ellipse.RenderTransform = new TranslateTransform(center.X - x, center.Y-x);
                this.Children.Add(ellipse);
            }

            int polyCount = data.Count(); //number of stars

            Star star = new Star();
            star.center = center;
            star.numPoints = polyCount;
            star.radii = new List<double>();

            double maxStarValue = 0.0;
            for (int x = 0; x < polyCount; x++)
            {
                if (data[x][starIndex] > maxStarValue)
                {
                    maxStarValue = data[x][starIndex];
                }
            }

            for (int x = 0; x < polyCount; x++)
            {
                star.radii.Add((data[x][starIndex]/maxStarValue)*100.0);
            }

            star.Fill = System.Windows.Media.Brushes.Khaki;
            star.Stroke = System.Windows.Media.Brushes.Brown;
            star.StrokeThickness = 0.5;

            //generate the points used to locate the pie charts, etc.
            star.generateGeometry();
            this.Children.Add(star);

            //build the wedges at each star tip
            for (int x = 0; x < polyCount; x++)
            {
                //compute information for wedges
                double sum = data[x].Sum<double>(z => z);
                sum -= data[x][starIndex]; //remove the value used to calculate the star arms from the pie chart total

                double angle = 0.0;

                for (int y = 0; y < data[x].Count(); y++)
                {
                    if (y != starIndex) //skip this index if it's the one used to calculate the star arms
                    {
                        //Draw the outer ring
                        Wedge w = new Wedge();
                        w.center = star.tips[x];
                        w.sweep = data[x][y] / sum * 90.0;
                        w.innerRadius = 0.0;
                        w.outerRadius = 50.0;
                        w.rotationAngle = angle;

                        //running total of value to aid with the colorblind!
                        Color c = ColorUtils.computeFromHSV(angle * 4.0, 1.0, 0.9);
                        w.Fill = new SolidColorBrush(c);
                        w.Stroke = System.Windows.Media.Brushes.Black;
                        w.Stroke = new SolidColorBrush(scaled(c, .8));
                        w.StrokeThickness = 0.5;
                        RotateTransform rotation = new RotateTransform(180 - 45 - (360.0 / polyCount) * x);
                        rotation.CenterX = star.tips[x].X;
                        rotation.CenterY = star.tips[x].Y;
                        w.RenderTransform = rotation;
                        this.Children.Add(w);

                        angle += w.sweep;
                    }
                }
            }
        }

        private Color scaled(Color c, double p)
        {
            return Color.FromRgb((byte)(c.R * p), (byte)(c.G * p), (byte)(c.B * p));
        }
    }
}
