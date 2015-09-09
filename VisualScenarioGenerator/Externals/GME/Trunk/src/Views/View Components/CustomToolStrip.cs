using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;

namespace AME.Views.View_Components
{
    public class CustomToolStrip : ToolStrip, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController myController;

        public CustomToolStrip()
            : base()
        {
            myHelper = new ViewComponentHelper(this);
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        public void UpdateViewComponent()
        {
            foreach(ToolStripItem item in this.Items)
            {
                if (item is IViewComponent)
                {
                    ((IViewComponent)item).UpdateViewComponent();
                }
            }
        }

        #endregion
    }
}
