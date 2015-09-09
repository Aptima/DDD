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
    public partial class Level : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _vsgController;
        private int _emitterID = -1;
        private int _levelID = -1;
        private string _levelName = string.Empty;

        public int EmitterID
        {
            get
            {
                return _emitterID;
            }
            set
            {
                _emitterID = value;
            }
        }
        public int LevelID
        {
            get
            {
                return _levelID;
            }
            set
            {
                _levelID = value;
                this.customRadioButton1.ComponentId = _levelID;
                this.customRadioButton2.ComponentId = _levelID;
                this.customNonnegativeDouble1.ComponentId = _levelID;
                UpdateViewComponent();

            }
        }
        public string LevelName
        {
            get
            {
                return _levelName;
            }
            set
            {
                _levelName = value;
                customTabPage1.Description = _levelName;
            }
        }
        public Level()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return _vsgController;
            }
            set
            {
                _vsgController = (VSGController)value;
                this.customNonnegativeDouble1.Controller = value;
                this.customRadioButton1.Controller = value;
                this.customRadioButton2.Controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            this.customRadioButton1.UpdateViewComponent();
            this.customRadioButton2.UpdateViewComponent();
            this.customNonnegativeDouble1.UpdateViewComponent();            
        }

        #endregion

        private void radioButtons_CheckChanged(object sender, EventArgs e)
        {
            string radioText = ((RadioButton)sender).Text;

            switch (radioText)
            { 
                case "Variance":
                    label1.Text = "Variance (double)";
                    
                    break;
                case "Probability":
                    label1.Text = "Magnitude (%)";
                    break;
                default:
                    break;
            }
        }
    }
}
