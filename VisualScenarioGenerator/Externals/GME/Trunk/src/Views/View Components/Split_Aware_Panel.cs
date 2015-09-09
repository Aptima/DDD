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
    public class Split_Aware_Panel : SplitContainer
    {
        protected Split_Aware_Panel()
            : base()
        {
            this.Dock = DockStyle.Fill;
            //this.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
