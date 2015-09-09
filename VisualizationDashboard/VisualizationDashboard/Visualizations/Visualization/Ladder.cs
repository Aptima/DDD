using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Aptima.Visualization
{
    /// <summary>
    /// THIS CLASS IS A WORK IN PROGRESS. NOT FULLY IMPLEMENTED!
    /// </summary>
    class Ladder : Canvas
    {
        class Node : Canvas
        {
            //public String label {get;set;}
            public Point  center {get;set;}

            private double CIRCLE_RADIUS = 35.0;

            public void setup( string label, Point center, bool leftHand )
            {
                //this.label = label;
                this.center = new Point( center.X, center.Y);

                Ellipse ellipse = new Ellipse();
                ellipse.Width = CIRCLE_RADIUS;
                ellipse.Height = CIRCLE_RADIUS;
                ellipse.Stroke = System.Windows.Media.Brushes.Black;
                ellipse.StrokeThickness = 0.75;
                ellipse.RenderTransform = new TranslateTransform(center.X + CIRCLE_RADIUS / 2.0, center.Y + CIRCLE_RADIUS/2.0);
                Children.Add(ellipse);

                TextBlock display = new TextBlock();
                display.Text = label;
                display.Width = 50;
                display.TextWrapping = TextWrapping.Wrap;

                if (leftHand)
                {
                    display.RenderTransform = new TranslateTransform(center.X - CIRCLE_RADIUS, center.Y + CIRCLE_RADIUS / 2.0);
                }
                else 
                {
                    display.RenderTransform = new TranslateTransform(center.X + 2 * CIRCLE_RADIUS, center.Y + CIRCLE_RADIUS / 2.0);
                }
                Children.Add( display );
            }
        }

        public double[][] graph {get;set;}
        public List<string> dependantLabels {get;set;}
        public List<string> independantLabels { get; set; }

        public void getComponent()
        {
            
            Point p = new Point(50,50);

            //draw two columns of labels for the circles...
            foreach(string x in dependantLabels )
            {
                Node n = new Node();
                n.setup(x, p, true);
                p.Y += 75.0;

                Children.Add(n);
            }

            p = new Point(250, 50);
            foreach (string x in independantLabels)
            {
                Node n = new Node();
                n.setup(x, p, false);
                p.Y += 75.0;
                Children.Add(n);
            }

            p = new Point(50, 50);
            //then draw lines between them...
            for (int x = 0; x < graph.Count(); x++)
            {
                Point p2 = new Point(250, 50);

                for (int y = 0; y < graph[x].Count(); y++)
                {
                    if (graph[x][y] > 0.0)
                    {
                        Line l = new Line();
                        l.X1 = p.X+35;
                        l.Y1 = p.Y+35;
                        l.X2 = p2.X+35;
                        l.Y2 = p2.Y+35;
                        l.Stroke = System.Windows.Media.Brushes.Black;
                        l.StrokeThickness = graph[x][y];
                        Children.Add(l);
                    }
                    p2.Y += 75.0;
                }
                p.Y += 75.0;
            }
            
            /*
            System.Windows.Controls.Grid grid = new Grid();

            ColumnDefinition firstLabel = new ColumnDefinition();
            ColumnDefinition firstCircle = new ColumnDefinition();
            ColumnDefinition secondCircle = new ColumnDefinition();
            ColumnDefinition secondLabel = new ColumnDefinition();

            

            int maxEntries = independantLabels.Count() > dependantLabels.Count() ? independantLabels.Count() : dependantLabels.Count();

            for (int x = 0; x < maxEntries; x++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            grid.ColumnDefinitions.Add(firstLabel);
            grid.ColumnDefinitions.Add(firstCircle);
            grid.ColumnDefinitions.Add(secondCircle);
            grid.ColumnDefinitions.Add(secondLabel);

            List<UIElement> elements = new List<UIElement>();

            for (int x = 0; x < independantLabels.Count();x++ )
            {
                Label l = new Label();
                l.Content = independantLabels[x];
                l.HorizontalAlignment = HorizontalAlignment.Right;
                l.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(l, x);
                Grid.SetColumn(l, 0);
                elements.Add(l);

                Ellipse ellipse = new Ellipse();
                ellipse.Width = 35.0;
                ellipse.Height = 35.0;
                ellipse.Stroke = System.Windows.Media.Brushes.Black;
                ellipse.StrokeThickness = 0.75;
                Grid.SetRow(ellipse, x);
                Grid.SetColumn(ellipse, 1);
                elements.Add(ellipse);
            }

            for (int x = 0; x < dependantLabels.Count(); x++)
            {
                Label l = new Label();
                l.Content = dependantLabels[x];
                l.HorizontalAlignment = HorizontalAlignment.Left;
                l.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(l, x);
                Grid.SetColumn(l, 3);
                elements.Add(l);

                Ellipse ellipse = new Ellipse();
                ellipse.Width = 35.0;
                ellipse.Height = 35.0;
                ellipse.Stroke = System.Windows.Media.Brushes.Black;
                ellipse.StrokeThickness = 0.75;
                Grid.SetRow(ellipse, x);
                Grid.SetColumn(ellipse, 2);
                elements.Add(ellipse);
            }

            elements.ForEach(x => grid.Children.Add(x));
            
            //can't draw lines with this grid...
            return grid;
             * */
        }
    }
}
