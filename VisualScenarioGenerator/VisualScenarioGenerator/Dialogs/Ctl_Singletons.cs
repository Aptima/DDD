using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Singletons : Ctl_ContentPaneControl
    {

        public NumericUpDown NudIntensity_1
        {
            get { return nudIntensity_1; }
            set { nudIntensity_1 = value; }
        }
        public NonNegDecimal NndRange_1
        {
            get { return nndRange_1; }
            set { nndRange_1 = value; }
        }

        public NumericUpDown NudProbability_1
        {
            get { return nudProbability_1; }
            set { nudProbability_1 = value; }
        }

        public NumericUpDown NudIntensity_2
        {
            get { return nudIntensity_2; }
            set { nudIntensity_2 = value; }
        }

        public NonNegDecimal NndRange_2
        {
            get { return nndRange_2; }
            set { nndRange_2 = value; }
        }
        public NumericUpDown NudProbability_2
        {
            get { return nudProbability_2; }
            set { nudProbability_2 = value; }
        }

        public ListBox LbxCapability
        {
            get { return lbxCapability; }
            set { lbxCapability = value; }
        }
        public ListBox LbxState_1
        {
            get { return lbxState_1; }
            set { lbxState_1 = value; }
        }
        public ListBox LbxState_2
        {
            get { return lbxState_2; }
            set { lbxState_2 = value; }
        }


         public Ctl_Singletons()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            nudIntensity_1.Value = 0;
            nudProbability_1.Value = 100;
            nndRange_1.Value = 0;
            nudIntensity_2.Value = 0;
            nudProbability_2.Value = 100;
            nndRange_2.Value = 0;
            lbxCapability.SelectedValue = "";
             
        }
   








    }
}
