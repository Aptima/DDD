using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class XYZ : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        public double NndX
        {
            get
            {
                return double.Parse(nndX.Text);
            }
            set
            {
                try
                {
                    nndX.Text = value.ToString();
                }
                catch { }
            }
        }
        public double NndY
        {
            get
            {
                return double.Parse(nndY.Text);
            }
            set
            {
                try
                {
                    nndY.Text = value.ToString();
                }
                catch { }
            }
        }
        public double NndZ
        {
            get
            {
                return double.Parse(nndZ.Text);
            }
            set
            {
                try
                {
                    nndZ.Text = value.ToString();
                }
                catch { }
            }
        }

        private Int32 xYZId = -1;
 public Int32 XYZId
        {
            get { return xYZId; }
            set
            {
                xYZId = value;
                if (xYZId > 0)
                {
                    nndX.ComponentId = xYZId;
                    nndY.ComponentId = xYZId;
                    nndZ.ComponentId = xYZId;
                }
            }
        }
        private VSGController controller;
   
       
        private Int32 componentId;

        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }

            set
            {
                componentId = value;
            }
        }

        public XYZ()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = (VSGController)value;  // Only give this a vsg controller.
                nndX.Controller = controller;
                nndY.Controller = controller;
                nndZ.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            nndX.UpdateViewComponent();
            nndY.UpdateViewComponent();
            nndZ.UpdateViewComponent();
     
        }

        #endregion
    }
}
