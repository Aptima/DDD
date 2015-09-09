using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aptima.Visualization.Data
{
    /// <summary>
    /// Tree Structure that contains tags, as well as some simple calculations for
    /// determining the number of children and tree depth (with just a root node, depth is 1)
    /// </summary>
    public class HierarchicalValue : Value
    {
        public List<HierarchicalValue> children { get; set; }
        public HierarchicalValue parent { get; set; }
        public string tag = "";

        public HierarchicalValue() : base()
        {
        }

        public double totalValues()
        {
            double result = value;
            if (children != null)
            {
                children.ForEach(x => result += x.totalValues());
            }
            return result;
        }

        /// <summary>
        /// Determines the depth of this node - if the node is a leaf, the depth is 1.
        /// </summary>
        /// <returns></returns>
        public int depth()
        {
            List<int> childDepth = new List<int>();

            if (children == null || children.Count() == 0)
                return 1; //for the purposes of visualization, return 1 as a minimum depth

            children.ForEach(x => childDepth.Add(x.depth()));
            return childDepth.Max() + 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalValue"/> class.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag associated to the value.</param>
        public HierarchicalValue(string name, double value, string tag)
        {
            this.name = name;
            this.value = value;
            this.tag = tag;

            children = new List<HierarchicalValue>();//empty list
        }

        /// <summary>
        /// Gets the unique tags in the tree.
        /// </summary>
        /// <returns></returns>
        public List<string> getUniqueTags()
        {
            List<string> result = new List<string>();

            result.Add(tag);

            foreach (HierarchicalValue child in children)
            {
                var temp = child.getUniqueTags();
                temp.ForEach(x => { if (!result.Contains(x)) { result.Add(x); } });
            }

            return result;
        }
    }

    /// <summary>
    /// Name/Value Tuple
    /// </summary>
    public class Value
    {
        public string name { get; set; }
        public double value;
    }
}
