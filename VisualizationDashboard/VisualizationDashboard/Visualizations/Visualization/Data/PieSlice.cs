using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aptima.Visualization.Data
{
    public class PieSlice
    {
        public string name { get; set; }
        public List<Rings> rings { get; set; }

        public PieSlice()
        {
            rings = new List<Rings>();
        }

        public class Rings
        {
            public List<double> values { get; set; }

            public Rings()
            {
                values = new List<double>();
            }
        }
    }
}
