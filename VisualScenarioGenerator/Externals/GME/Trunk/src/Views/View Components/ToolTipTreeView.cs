using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AME.Views.View_Components
{
    public class ToolTipTreeView : TreeView
    {
        public ToolTipTreeView()
            : base()
        {

        }

        // turn off automatic tool tips
        // from http://social.msdn.microsoft.com/forums/en-US/winforms/thread/00a91481-c931-4efd-b289-ab1f7c79c96f/
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parms = base.CreateParams;
                parms.Style |= 0x80;  // Turn on TVS_NOTOOLTIPS 
                return parms;
            }
        }
    }
}
