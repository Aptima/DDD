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
    public partial class Ctl_Capabilities : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<CapabilityDataStruct> Capabilities = new ObjectTypeLists<CapabilityDataStruct>();

       private CapabilityDataStruct _datastore = CapabilityDataStruct.Empty();

        public Ctl_Capabilities()
        {
            InitializeComponent();
        }
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
               
                CapabilityDataStruct data = (CapabilityDataStruct)object_data;

                txtName.Text = data.ID;
                nndRange_1.Value = data.Proximities[0].Range;
                try // so as not to worry about indexing out of range
                {
                    nudIntensity_1_1.Value = data.Proximities[0].Effects[0].Intensity;
                    nudProbability_1_1.Value = data.Proximities[0].Effects[0].Probability;
                    nudIntensity_1_2.Value = data.Proximities[0].Effects[1].Intensity;
                    nudProbability_1_2.Value = data.Proximities[0].Effects[1].Probability;


                }
                catch { }
                nndRange_2.Value = data.Proximities[1].Range;
                try // so as not to worry about indexing out of range
                {
                    nudIntensity_2_1.Value = data.Proximities[1].Effects[0].Intensity;
                    nudProbability_2_1.Value = data.Proximities[1].Effects[0].Probability;
                    nudIntensity_2_2.Value = data.Proximities[1].Effects[1].Intensity;
                    nudProbability_2_2.Value = data.Proximities[1].Effects[1].Probability;
                }
                catch { }
 
            }

        }
        public void ResetForNewEntry()
        {
            txtName.Text = "";
            nndRange_1.Value = 0;
            nndRange_2.Value = 0;
            nudIntensity_1_1.Value = 0;
            nudProbability_1_1.Value = 100;
            nudIntensity_1_2.Value = 0;
            nudProbability_1_2.Value = 100;
            nudIntensity_2_1.Value = 0;
            nudProbability_2_1.Value = 100;
            nudIntensity_2_2.Value = 0;
            nudProbability_2_2.Value = 100;



        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                _datastore.Proximities.Clear();
                Proximity p1 = new Proximity();
                p1.Range = nndRange_1.Value;
                p1.Effects.Add(new Effect(nudIntensity_1_1.Value, nudProbability_1_1.Value));
                p1.Effects.Add(new Effect(nudIntensity_1_2.Value, nudProbability_1_2.Value));
                _datastore.Add(p1);
                Proximity p2 = new Proximity();
                p2.Range = nndRange_2.Value;
                p2.Effects.Add(new Effect(nudIntensity_2_1.Value, nudProbability_2_1.Value));
                p2.Effects.Add(new Effect(nudIntensity_2_2.Value, nudProbability_2_2.Value));
                _datastore.Add(p2);

   
                Notify((object)_datastore);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtName.Text != string.Empty)
            {
                if (!(Capabilities.GetNames().Contains(txtName.Text)))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(Capabilities.GetFromList(txtName.Text)))
                {
                    ask = true;
                }
            }
            if (!ask)
            {
                ResetForNewEntry();
                return;
            }
            DialogResult answer = MessageBox.Show("Do you want to save this Emitter?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
            if (DialogResult.Yes == answer)
            {
                btnAccept_Click(sender, e);
            }
            else if (DialogResult.No == answer)
            {
                ResetForNewEntry();
            }

        }



     


   


    }
}
