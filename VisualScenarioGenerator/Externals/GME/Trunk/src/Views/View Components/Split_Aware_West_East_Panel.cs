using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AME.Views.View_Components
{
    /**
    * Convenience class for quickly adding custom split containers to a designer
    * window
    */
    public class Split_Aware_West_East_Panel : Split_Aware_Panel
    {
        public Split_Aware_West_East_Panel()
            : base()
        {
            this.Orientation = Orientation.Vertical;
        }
    }
}
