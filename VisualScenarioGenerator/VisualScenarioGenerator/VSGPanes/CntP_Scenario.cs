using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator.VSGPanes
{
    public partial class CntP_Scenario : Ctl_ContentPane

    {
        public CntP_Scenario()
        {
            InitializeComponent();
            this.ctl_ScenarioImages1.ContentPane = this;
        }


        public override void Update(object object_data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void ctl_ScenarioImages1_Load(object sender, EventArgs e)
        {

        }
    }
}
