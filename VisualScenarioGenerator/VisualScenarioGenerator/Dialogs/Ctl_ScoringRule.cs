using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_ScoringRule : Ctl_ContentPaneControl
    {
        private ScoringRuleDataStruct _datastore = ScoringRuleDataStruct.Empty;

        public Ctl_ScoringRule()
        {
            InitializeComponent();
        }

        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                ScoringRuleDataStruct data = (ScoringRuleDataStruct)object_data;
                txtName.Text = data.ID;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                Notify((object)_datastore);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void cboUnitID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)cboUnitID.SelectedItem == "Any object")
            {
                cboSpeciesOrUnit.Enabled = false;
            }
            else if ((string)cboUnitID.SelectedItem == "Any object of species:")
            {
                cboSpeciesOrUnit.Enabled = true;
            }
            else if ((string)cboUnitID.SelectedItem == "Object with unit id:")
            {
                cboSpeciesOrUnit.Enabled = true;
            }
        }

        private void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if ((string)cboRegion.SelectedItem == "Anywhere")
            {
                cboActiveRegions.Enabled = false;
            }
            else if ((string)cboRegion.SelectedItem == "In region:")
            {
                cboActiveRegions.Enabled = true;
            }
            else if ((string)cboRegion.SelectedItem == "Not in region:")
            {
                cboActiveRegions.Enabled = true;
            }
        }

        private void cboUnitID2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)cboUnitID2.SelectedItem == "Any object")
            {
                cboSpeciesOrUnit2.Enabled = false;
            }
            else if ((string)cboUnitID2.SelectedItem == "Any object of species:")
            {
                cboSpeciesOrUnit2.Enabled = true;
            }
            else if ((string)cboUnitID2.SelectedItem == "Object with unit id:")
            {
                cboSpeciesOrUnit2.Enabled = true;
            }
        }

        private void cboRegion2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)cboRegion2.SelectedItem == "Anywhere")
            {
                cboActiveRegions2.Enabled = false;
            }
            else if ((string)cboRegion2.SelectedItem == "In region:")
            {
                cboActiveRegions2.Enabled = true;
            }
            else if ((string)cboRegion2.SelectedItem == "Not in region:")
            {
                cboActiveRegions2.Enabled = true;
            }
        }

        private void cboRuleTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)cboRuleTypeBox.SelectedItem == "Object 1 exists")
            {
                cboUnitID2.Enabled = false;
                cboOwner2.Enabled = false;
                cboRegion2.Enabled = false;
                cboActiveRegions2.Enabled = false;
                cboSpeciesOrUnit2.Enabled = false;
                cboStateEntered.Enabled = false;
                cboStateFrom.Enabled = false;
                groupBox2.Enabled = false;
            }
            else if ((string)cboRuleTypeBox.SelectedItem == "Object 1 causes object 2 state change")
            {
                cboUnitID2.Enabled = true;
                cboUnitID2.SelectedItem = null;
                cboOwner2.Enabled = true;
                cboOwner2.SelectedItem = null;
                cboRegion2.Enabled = true;
                cboRegion2.SelectedItem = null;
                cboActiveRegions2.Enabled = false;
                cboSpeciesOrUnit2.Enabled = false;
                cboStateEntered.Enabled = true;
                cboStateFrom.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                Notify((object)_datastore);
            }
        }
    }
}
