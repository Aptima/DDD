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
    public partial class Ctl_Engram : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<EngramDataStruct> Engrams = new ObjectTypeLists<EngramDataStruct>();
  
        private EngramDataStruct _datastore = EngramDataStruct.Empty();

          private Dictionary<string, string> _engram_list = new Dictionary<string, string>();
        public Dictionary<string, string> EngramList
        {
            get
            {
                return _engram_list;
            }
        } 


        public Ctl_Engram()
        {
            InitializeComponent();
            ResetForNewEntry();
        }

  
        
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                EngramDataStruct data = (EngramDataStruct)object_data;
                this.txtID.Text = data.ID;
               this.txtInitialValue.Text= data.Value;
       
            }
        }

        public void ResetForNewEntry()
        {
            this.txtID.Text = "";
            this.txtInitialValue.Text = "";
            _datastore = new EngramDataStruct("");
            _datastore.Value = "";
            _datastore.globallyUnique = false;
            _datastore.CheckUniquenessOnSave = true; // Tests within other Engrams
        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtID.Text != string.Empty)
            {
                _datastore.ID = txtID.Text;
                _datastore.Value = txtInitialValue.Text;
                Notify((object)_datastore);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (this.txtID.Text != string.Empty)
            {
                if(!Engrams.GetNames().Contains(txtID.Text))
                ask = true;
            }
            else if (!_datastore.Equals(Engrams.GetFromList(txtID.Text)))
            {
                ask = true;
            }
            if (!ask)
            {
ResetForNewEntry();
            return;
            } 
            DialogResult answer = MessageBox.Show("Do you want to save this Engram definition?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
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
