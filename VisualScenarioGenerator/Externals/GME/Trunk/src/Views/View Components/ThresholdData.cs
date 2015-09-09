using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AME.Views.View_Components
{
    public class ThresholdData
    {
        public ThresholdData(String name)
        {
            this.Name = name;
        }

        public String Name { get; set; }

        public Double LowerValue { get; set; }
        public Double MiddleValue1 { get; set; }
        public Double MiddleValue2 { get; set; }
        public Double UpperValue { get; set; }

        public Color LowerColor { get; set; }
        public Color MiddleColor { get; set; }
        public Color UpperColor { get; set; }

        public Boolean LowerCheck { get; set; }
        public Boolean MiddleCheck1 { get; set; }
        public Boolean MiddleCheck2 { get; set; }
        public Boolean UpperCheck { get; set; }
    }
}
