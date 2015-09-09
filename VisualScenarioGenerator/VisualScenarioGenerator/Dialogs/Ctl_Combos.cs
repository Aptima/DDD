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

    public partial class Ctl_Combos : Ctl_ContentPaneControl
    {

        public ListBox LbxCapability_1;
        public ListBox LbxCapability_2;
        public NonNegDecimal NndRange_1;
        public NonNegDecimal NndRange_2;
        public NumericUpDown NudEffect_1;
        public NumericUpDown NudEffect_2;
        public NumericUpDown NudProbability_1;
        public NumericUpDown NudProbability_2;
        public ListBox LbxNewState;

        public Ctl_Combos()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            lbxCapability_1.SelectedValue = "";
            lbxCapability_2.SelectedValue = "";
            nndRange_1.Value = 0;
            nndRange_2.Value = 0;
            nudEffect_1.Value = 0;
            nudEffect_2.Value = 0;
            nudProbability_1.Value = 100;
            nudProbability_2.Value = 100;
            lbxNewState.SelectedValue = "";
        }

        private void nndRange_1_Load(object sender, EventArgs e)
        {

        }

     







    }
}
