using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using VSG.Controllers;

namespace VSG.ViewComponents
{
    public partial class Classification : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 classificationId = -1;
        private VSGController controller;
        private String classificationName="";

        public Classification()
        {
            myHelper = new ViewComponentHelper(this);
            InitializeComponent();
            label.Text = classificationName;
        }
        public Int32 ClassificationId
        {
            get
            {
                return classificationId;
            }
            set
            {
                classificationId = value;
                if (classificationId >= 0)
                {
                    reset();
                    UpdateViewComponent();
                }
            }
        }

        public String ClassificationName
        {
            get
            {
                return classificationName;
            }
            set
            {
                classificationName = value;
                label.Text = classificationName;
                //customTabPage1.Description = classificationName;
            }
        }

        private void reset()
        {

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
            }
        }

        public void UpdateViewComponent()
        {
            label.Text = classificationName;
        }

        #endregion
    }
}
