using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Aptima.Visualization.Utility;
using Aptima.Visualization.Data;

namespace Aptima.Visualization
{
    public static class ColorExtensions
    { 
        static public Color ToColor(this uint argb)
        {
          return Color.FromArgb(0xff,
                                (byte)((argb & 0xff0000) >> 0x10),
                                (byte)((argb & 0xff00) >> 8),
                                (byte)(argb & 0xff));
        } 
    }

    /// <summary>
    /// This class builds a multi-level bowtie visualization.
    /// The 'generate' method must be run in order to initialize the display
    /// based on the data.
    /// </summary>
    public class MultilevelBowTie : Visualization
    {
        private static string name = "multilevelbowtie";

        public uint[] bowTieColors = { 0x3333FF, 0x006600, 0x00CC33, 0x00FF66, 0x99FF99, 0xFF3333 };

        /// <summary>
        /// Maximum radius of the bow tie visualization
        /// </summary>
        private double maxRadius = 100.0;

        public double MaxRadius
        {
            get { return maxRadius; }
            set { maxRadius = value; }
        }

        public Point center {get;set;}


        private double leftTotal = 0.0;

        public double LeftTotal
        {
            get { return leftTotal; }
            set { leftTotal = value; }
        }

        private double rightTotal = 0.0;

        public double RightTotal
        {
            get { return rightTotal; }
            set { rightTotal = value; }
        }

        /// <summary>
        /// Maximum sweep of the arc-segments - 90.0 degrees
        /// </summary>
        private static double maximumArcSweep = 90.0;


        /// <summary>
        /// Starting angle for the left-side of the bowtie
        /// </summary>
        private double startingLeftAngle = 270.0 - maximumArcSweep / 2.0;
        /// <summary>
        /// Starting angle for the right-side of the bowtie
        /// </summary>
        private double startingRightAngle = 90.0 - maximumArcSweep / 2.0;

        /// <summary>
        /// Gets or sets the hiearchical data for the left-hand side of the bowtie
        /// </summary>
        /// <value>The root of the left-hand tree</value>
        public HierarchicalValue left { get; set; }
        /// <summary>
        /// Gets or sets the hiearchical data for the right-hand side of the bowtie
        /// </summary>
        /// <value>The root of the right-hand tree</value>
        public HierarchicalValue right { get; set; }

        /// <summary>
        /// Internal class that contains all of the wedges built while generating the display
        /// </summary>
        List<Wedge> wedges = new List<Wedge>();
        /// <summary>
        /// Internal container that maps data colors to tag values (so values with similar tags have similar colors)
        /// </summary>
        private Dictionary<string, SolidColorBrush> brushes = new Dictionary<string, SolidColorBrush>();

        public Dictionary<string, SolidColorBrush> Brushes
        {
            get { return brushes; }
            set { brushes = value; }
        }

        public MultilevelBowTie() : base()
        { 
        }

        public override string getName()
        {
            return name;
        }

        /// <summary>
        /// Generates the visualization - adds all of the display components to the canvas as children.
        /// </summary>
        public override void generate()
        {
            //remove any old child graphical elements
            Children.Clear();

            //Determine colors used by Hierarchical Values
            //need to take into account matching child colors.
            //this should be performed for both left and right hand sides...
            var tags = left.getUniqueTags();
            var rightTags = right.getUniqueTags();

            //merge the right tags and left tags together
            rightTags.ForEach(x => { if (!tags.Contains(x)) { tags.Add(x); } });

            //create a unique SolidColorBrush for each tag and store them in a map
            //use HSV to walk around a circle, varying H and S (S for the colorblind)
            double h = 0.0;
            double angularIncrement = 360.0 / tags.Count();
            double s = 0.5;
            double sIncrement = .5 / tags.Count();
            int pos = 0;
            tags.ForEach( x => brushes.Add(x, new SolidColorBrush(ColorExtensions.ToColor(bowTieColors[pos++]))));

            double leftRadius = 0.0;
            double rightRadius = 0.0;

            if ((leftTotal == 0) && (rightTotal == 0))
            {
                leftRadius = 0;
                rightRadius = 0;
            }
            else if (leftTotal > rightTotal)
            {
                leftRadius = maxRadius;
                rightRadius = (rightTotal / leftTotal) * maxRadius;
            }
            else
            {
                rightRadius = maxRadius;
                leftRadius = (leftTotal / rightTotal) * maxRadius;
            }

            //determine the radii at each concentric circle
            if ((leftTotal != 0) || (rightTotal != 0))
            {
                double radius = leftRadius / left.depth();
                generate(left, 0.0, radius, maximumArcSweep, startingLeftAngle);

                radius = rightRadius / right.depth();
                generate(right, 0.0, radius, maximumArcSweep, startingRightAngle);
            }

            //add each wedge into the canvas
            wedges.ForEach(x => Children.Add(x));
        }

        /// <summary>
        /// Recursively generates the wedges for each node in the hierarchical value tree.
        /// </summary>
        /// <param name="node">The node from which to create a wedge.</param>
        /// <param name="innerRadius">The inner radius of the wedge.</param>
        /// <param name="radius">The radius length (poor naming!).</param>
        /// <param name="arc">The arc of the wedge.</param>
        /// <param name="rotationalAngle">The rotational angle of the wedge (rotational transform, essentially).</param>
        private void generate(HierarchicalValue node, double innerRadius, double radius, double arc, double rotationalAngle)
        {
            Wedge wedge = new Wedge();

            wedge.center = center;
            wedge.innerRadius = innerRadius;
            wedge.outerRadius = innerRadius + radius;
            wedge.sweep = arc;
            wedge.rotationAngle = rotationalAngle;

            //use a default black stroke around each wedge
            wedge.Stroke = System.Windows.Media.Brushes.Black;
            wedge.StrokeThickness = 0.5;
            wedge.Fill = brushes[node.tag]; //lookup the unique color based on the tag for this node
            wedge.ToolTip = node.name;

            wedges.Add(wedge);

            //arc percentage for children...
            var h = rotationalAngle;

            if (rotationalAngle < 180.0)
            {
                //create wedges for all of the children of this node
                foreach (HierarchicalValue child in node.children)
                {
                    //sweep is related to the value the child contributes to the parent value
                    var angle = arc * (child.totalValues() / (node.totalValues() - node.value));
                    generate(child, wedge.outerRadius, radius, angle, h);
                    h += angle; //advance around the parent's arc
                }
            }
            else {
                // Process in reverse order
                //create wedges for all of the children of this node
                for(int i = node.children.Count - 1;i >= 0;i--)
                {
                    HierarchicalValue child = node.children[i];

                    //sweep is related to the value the child contributes to the parent value
                    var angle = arc * (child.totalValues() / (node.totalValues() - node.value));
                    generate(child, wedge.outerRadius, radius, angle, h);
                    h += angle; //advance around the parent's arc
                }
            }
        }

    }
}
