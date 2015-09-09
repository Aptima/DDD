using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls;

namespace AME.Views.View_Components
{
    /// <summary>
    /// A dummy view component helper - use to bind to non visible controls 
    /// The main use of this is in the Bulk Helper which threads an import then waits 
    /// for an update to come back through the controllers
    /// 
    /// When processing the update the bulk helper doesn't need to do any actual UpdateViewComponent work, so 
    /// we override visibility and invoke (as they can't actually be determined if we had used
    /// a normal view component helper)
    public class NonControlViewComponentHelper : ViewComponentHelper
    {
        public NonControlViewComponentHelper(IViewComponent iv) : base(iv) {}

        public NonControlViewComponentHelper(IViewComponent iv, UpdateType updateType) : base(iv, updateType) {}

        public override bool IViewHelperVisible
        {
            get
            {
                return true;
            }
        }

        public override Boolean InvokeRequired
        {
            get
            {
                return false;
            }
        }
    }
}
