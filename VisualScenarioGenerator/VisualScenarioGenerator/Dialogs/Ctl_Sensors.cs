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
    public partial class Ctl_Sensors : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<SensorDataStruct> Sensors = new ObjectTypeLists<SensorDataStruct>();
  
        private SensorDataStruct _datastore = SensorDataStruct.Empty();

        private void GreyOptions(Boolean makeGrey)
        {
            if (makeGrey)
            {
                lblAttribute.Enabled = false;
                lbxAttribute.Enabled = false;
                lblSpread.Enabled = false;
                nndSpread.Enabled = false;
                lblLevel.Enabled = false;
                txtLevel.Enabled = false;
                lblDirection.Enabled = false;
                xyzDirection.Enabled = false;
            }
            else {
                lblAttribute.Enabled = true;
            lbxAttribute.Enabled = true;
            lblSpread.Enabled = true;
            nndSpread.Enabled = true;
            lblLevel.Enabled = true;
            txtLevel.Enabled = true;
            lblDirection.Enabled = true;
            xyzDirection.Enabled = true;
           

            }

        }

        public Ctl_Sensors()
        {
            InitializeComponent();
        }
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                SensorDataStruct data = (SensorDataStruct)object_data;
                txtID.Text = data.ID;
                rbAll.Checked = data.SingleSelect;
                rbOneAttribute.Checked = !rbAll.Checked;
                nndRange.Value = data.Range;
                if (data.SingleSelect)
                {
                    nndSpread.Value = 360;
                    lbxAttribute.SelectedItems.Clear();
                    txtLevel.Text = "";
                    xyzDirection.DecX.Value = 0;
                    xyzDirection.DecY.Value= 0;
                    xyzDirection.DecZ.Value = 0;
                    GreyOptions(true);

                }
                else
                {
                    nndSpread.Value = data.Spread;
                    txtLevel.Text = data.Level;
                    lbxAttribute.SelectedItem = data.Attribute;
                    xyzDirection.DecX.Value = data.X;
                    xyzDirection.DecY.Value = data.Y;
                    xyzDirection.DecZ.Value= data.Z;
                    GreyOptions(false);
                }
            }
        }


        public void ResetForNewEntry()
        {
            nndRange.Value = 0.0;
            nndSpread.Value = 360.0;
            txtID.Text = "";
            rbAll.Checked = true;
            lbxAttribute.SelectedItems.Clear();
            txtLevel.Text = "";
            xyzDirection.DecX.Value = 0;
            xyzDirection.DecY.Value = 0;
            xyzDirection.DecZ.Value = 0;
            GreyOptions(true);
        }







        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                double temp =xyzDirection.DecX.Value;
        
            }
           catch 
            {
                MessageBox.Show("The value for X is invalid. Please correct it before saving.");
                return;
           }
           try
           {
               double temp =xyzDirection.DecY.Value;
           }
           catch
           {
               MessageBox.Show("The value for Y is invalid. Please correct it before saving.");
               return;
           }
           try
           {
               double temp =xyzDirection.DecZ.Value;
           }
           catch
           {
               MessageBox.Show("The value for Z is invalid. Please correct it before saving.");
               return;
           }
           try
           {
              double temp =nndRange.Value;
           }
           catch
           {
               MessageBox.Show("The value for Range is invalid. Please correct it before saving.");
               return;
           }
           try
           {
               double temp =nndSpread.Value;
           }
           catch
           {
               MessageBox.Show("The value for Spread is invalid. Please correct it before saving.");
               return;
           }

            _datastore.ID = txtID.Text;
            _datastore.SingleSelect = rbAll.Checked;
            _datastore.Range = nndRange.Value;
            _datastore.Spread = nndSpread.Value;
            _datastore.Level = txtLevel.Text;
            if (lbxAttribute.SelectedItems.Count > 0)
                _datastore.Attribute = lbxAttribute.SelectedItem.ToString();
            while (xyzDirection.DecX.Value > 360)
                xyzDirection.DecX.Value -= 360;
            while (xyzDirection.DecX.Value < -360)
                xyzDirection.DecX.Value += 360;
            _datastore.X = xyzDirection.DecX.Value;
            while (xyzDirection.DecY.Value > 360)
                xyzDirection.DecY.Value -= 360;
            while (xyzDirection.DecY.Value < -360)
                xyzDirection.DecY.Value += 360;
            _datastore.Y = xyzDirection.DecY.Value;
            while (xyzDirection.DecZ.Value > 360)
                xyzDirection.DecZ.Value -= 360;
            while (xyzDirection.DecZ.Value < -360)
                xyzDirection.DecZ.Value += 360;
            _datastore.Z = xyzDirection.DecZ.Value;
            Notify((object)_datastore);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtID.Text != string.Empty)
            {
                if (!Sensors.GetNames().Contains(txtID.Text))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(Sensors.GetFromList(txtID.Text)))
                {
                    ask = true;
                }
            }
            if (!ask)
            {
                ResetForNewEntry();
                return;
            }
            DialogResult answer = MessageBox.Show("Do you want to save this Sensor?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
            if (DialogResult.Yes == answer)
            {
                btnAccept_Click(sender, e);
            }
            else if (DialogResult.No == answer)
            {
                ResetForNewEntry();
            }
        }

        private void nndSpread_MouseLeave(object sender, EventArgs e)
        {
            // reduce to smaller than 360 (but allow 360 -- which is not he same as 0)
           while (nndSpread.Value > 360)
                nndSpread.Value -= 360;
        }

        private void rb_Click(object sender, EventArgs e)
        {
            if (rbAll.Checked)
            {
                GreyOptions(true);
            }
            else
            {
                GreyOptions(false);
            }
        }

        private void xyzDirection_Load(object sender, EventArgs e)
        {

        }








    }
}
