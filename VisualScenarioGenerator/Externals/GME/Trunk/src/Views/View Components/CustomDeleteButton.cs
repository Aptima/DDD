using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using Forms;
using System.Drawing;

namespace AME.Views.View_Components
{
    public partial class CustomDeleteButton : Button, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController myController;

        public CustomDeleteButton()
            : base()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

            this.AutoSize = true;
        }

        private int m_DeleteID;

        public int DeleteID
        {
            get { return m_DeleteID; }
            set { m_DeleteID = value; }
        }

        #region ViewComponentUpdate Members

        public IController Controller
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
            //
        }

        #endregion
    }
}
