using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Globalization;
using Aptima.Visualization.Utility;

namespace Aptima.Visualization
{
    /// <summary>
    /// This class builds a classical heatmap based on a 2-D array of data points.
    /// Call 'generate' before displaying.
    /// </summary>
    public class HeatMap : Visualization
    {
        private static string name = "heatmap";

        /// <summary>
        /// Gets or sets the data used to display the heatmap.
        /// </summary>
        /// <value>The data to display in the heatmpa.</value>
        public double[][] data {get;set;}
        /// <summary>
        /// list of strings used as column headers.  Must match dimension in the data set.
        /// </summary>
        public List<string> columnHeaders;
        /// <summary>
        /// list of strings used as row headers.  Must match dimension in the data set.
        /// </summary>
        public List<string> rowHeaders;

        /// <summary>
        /// Gets or sets the number of color buckets used in the heatmap.
        /// </summary>
        /// <value>The number of color buckets.</value>
        public int numBuckets { get; set; }

        /// <summary>
        /// Padding (in WPF pixels) between heatmap rectangles
        /// </summary>
        private static double padding = 2.0;

        private static double horizonalMargin = 50;
        private static double verticalMargin = 50;

        private static double yLabelWidth = 75;
        private static double xLabelWidth = 75;

        /// <summary>
        /// Bucket values
        /// </summary>
        private List<double> buckets = new List<double>();
        /// <summary>
        /// Colors to go with the bucket values
        /// </summary>
        private List<SolidColorBrush> colors = new List<SolidColorBrush>();

        public HeatMap() : base()
        { 
        }
        public override string getName()
        {
            return name;
        }

        public override void generate()
        {

            //remove any old child graphical elements
            Children.Clear();

            //determine the minumum and maxium value in the dataset
            double min, max;
            min = max = data[0][0];

            //Perhaps this should have been a 1-dimensional array with width and height properties...
            for (int x = 0; x < data.Count(); x++)
            {
                var temp = data[x].Min();
                if (temp < min)
                {
                    min = temp;
                }

                temp = data[x].Max();
                if (temp > max)
                {
                    max = temp;
                }
            }

            //create a maximum value for each bin
            buckets.Clear();
            for (double d = min; d < max; d += (max - min) / numBuckets)
            {
                buckets.Add(d);
            }

            /*
            if ((colors.Count == 0) && (numBuckets > 0))
            {
                //figure out coloring!
                var h = 240.0;
                var hIncrement = 360.0 / numBuckets;
                var s = 1.0;
                var sIncrement = 1.0 - (1.0 / numBuckets);

                //determine a unique color for each bin (walk around the color wheel, and vary saturation at the same time)
                buckets.ForEach(x => colors.Add(new SolidColorBrush(ColorUtils.computeFromHSV(h, s += sIncrement, 0.75))));
            }*/

            if ((colors.Count == 0) && (numBuckets > 0))
            {
                //figure out coloring!
                double h = 240.0;
                double s = 0;
                double sIncrement = 1.0 / numBuckets;
                int r,g,b;

                //determine a unique color for each bin (walk around the color wheel, and vary saturation at the same time)
                foreach (double bucket in buckets)
	            {
                    s += sIncrement;
                    ColorUtils.HsvToRgb(h, s, 0.75, out r, out g, out b);
                    colors.Add(new SolidColorBrush(Color.FromRgb((byte)r,(byte)g,(byte)b)));            		
	            }
            }

            /*
            // Show colors for debugging
            {
                int xPos = 0;
                foreach (SolidColorBrush color in colors)
                {
                    Rectangle r = new Rectangle();

                    r.Width = 20;
                    r.Height = 20;
                    r.Fill = color;
                    r.Stroke = System.Windows.Media.Brushes.Black;
                    r.StrokeThickness = 0.5;
                    r.RenderTransform = new TranslateTransform(xPos, 0);
                    Children.Add(r);
                    xPos += 50;
                }
            }
            */

            double rectHeight = (Height - xLabelWidth - verticalMargin * 2) / data[0].Count() - padding;
            double rectWidth = (Width - yLabelWidth - horizonalMargin * 2) / data.Count() - padding;

            //build rectangles for each wedge
            for (int x = 0; x < data.Count(); x++)
            {
                for (int y = 0; y < data[x].Count(); y++)
                {
                    Rectangle r = new Rectangle();
                    int bucketNum = 0;

                    r.Width = (Width - yLabelWidth - horizonalMargin * 2) / data.Count() - padding;
                    r.Height = (Height - xLabelWidth - verticalMargin * 2) / data[x].Count() - padding;
                    if (buckets.Count > 0)
                    {
                        r.Fill = getColor(data[x][y]);
                        bucketNum = getBucketNum(data[x][y]);
                    }
                    else
                    {
                        r.Fill = new SolidColorBrush(Colors.White);
                    }
                    r.Stroke = System.Windows.Media.Brushes.Black;
                    r.StrokeThickness = 0.5;
                    r.RenderTransform = new TranslateTransform(horizonalMargin + yLabelWidth + x * (r.Width + padding), verticalMargin + y * (r.Height + padding));
                    Children.Add(r);

                    /*
                    Label l = new Label();
                    Typeface myTypeface = new Typeface(l.FontFamily.ToString());
                    FormattedText ft = new FormattedText(rowHeaders[y],
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        myTypeface, l.FontSize, Brushes.Black);
                    l.Content = bucketNum.ToString();
                    l.HorizontalContentAlignment = HorizontalAlignment.Left;
                    l.VerticalAlignment = VerticalAlignment.Top;
                    l.RenderTransform = new TranslateTransform(horizonalMargin + yLabelWidth + x * (r.Width + padding), verticalMargin + y * (r.Height + padding));
                    Children.Add(l);
                    */
                }
            }

            // Row Headings
            for (int y = 0; y < rowHeaders.Count(); y++)
            {
                Label l = new Label();
                Typeface myTypeface = new Typeface(l.FontFamily.ToString());
                FormattedText ft = new FormattedText(rowHeaders[y],
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    myTypeface, l.FontSize, Brushes.Black);
                l.Content = rowHeaders[y];
                l.HorizontalContentAlignment = HorizontalAlignment.Left;
                l.VerticalAlignment = VerticalAlignment.Top;
                l.RenderTransform = new TranslateTransform(horizonalMargin, verticalMargin + y * (rectHeight + padding) + (rectHeight - ft.Height) / 2);

                Children.Add(l);
            }

            // Column Headings
            for (int x = 0; x < columnHeaders.Count(); x++)
            {
                Label l = new Label();
                l.Content = columnHeaders[x];
                l.Width = rectWidth;
                l.HorizontalContentAlignment = HorizontalAlignment.Center;
                l.VerticalAlignment = VerticalAlignment.Top;
                l.RenderTransform = new TranslateTransform(horizonalMargin + xLabelWidth + x * (rectWidth + padding), verticalMargin +
                    rowHeaders.Count() * (rectHeight + padding));

                Children.Add(l);
            }

        }

        /// <summary>
        /// Gets the color from the color bucket based on the heatmap value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private SolidColorBrush getColor(double value)
        {
            SolidColorBrush result = colors.Last();

            if (value == 0.0)
            {
                return new SolidColorBrush(Colors.White);
            }

            for (int x = 0; x < buckets.Count(); x++)
            {
                if (buckets[x] > value)
                {
                    return colors[x - 1];
                }
            }

            return result;
        }

        private int getBucketNum(double value)
        {
            SolidColorBrush result = colors.Last();
            for (int x = 0; x < buckets.Count(); x++)
            {
                if (buckets[x] > value)
                {
                    return x - 1;
                }
            }

            return buckets.Count - 1;
        }

    }
}
