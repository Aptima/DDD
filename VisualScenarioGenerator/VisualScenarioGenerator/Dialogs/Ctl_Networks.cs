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
    public partial class Ctl_Networks : Ctl_ContentPaneControl
    {

        public static ObjectTypeLists<NetworkDataStruct> Networks = new ObjectTypeLists<NetworkDataStruct>();

        private NetworkDataStruct _datastore = NetworkDataStruct.Empty();

        public Ctl_Networks()
        {
            InitializeComponent();
            bbMembers.AllowMultipleSelection(true);
            bbMembers.SortLists(true);
            bbMembers.SetLabels("Available DMs", "Network Members");

        }
        public void ResetForNewEntry()
        {
            this.txtName.Text = "";
            _datastore = new NetworkDataStruct("");
            _datastore.CheckUniquenessOnSave = true;
            bbMembers.DisplayLists(MakeNonmembers(), _datastore.Members);

        }

        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                NetworkDataStruct data = (NetworkDataStruct)object_data;
                txtName.Text = data.ID;
                _datastore.Members.Clear();
                for (int i = 0; i < data.Members.Count; i++)
                    _datastore.Members.Add(data.Members[i]);

                bbMembers.DisplayLists(MakeNonmembers(), _datastore.Members);


            }

        }

        private List<string> MakeNonmembers()
        {
            List<string> returnValue = new List<string>();
            List<string> all = Ctl_DecisionMaker.DecisionMakers.GetNames();
            for (int i = 0; i < all.Count; i++)
            {
                if (!_datastore.Members.Contains(all[i]))
                    returnValue.Add(all[i]);

            }
            return returnValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                Notify((object)_datastore);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtName.Text != string.Empty)
            {
                if (!Networks.GetNames().Contains(txtName.Text))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(Networks.GetFromList(txtName.Text)))
                {
                    ask = true;
                }
            }
            if (!ask)
            {
                ResetForNewEntry();
                return;
            }
            DialogResult answer = MessageBox.Show("Do you want to save this Network?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
            if (DialogResult.Yes == answer)
            {
                btnAccept_Click(sender, e);
            }
            else if (DialogResult.No == answer)
            {
                ResetForNewEntry();
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
  
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                List<string> members = bbMembers.GetCurrent();
                for (int i = 0; i < members.Count; i++)
                    _datastore.AddMember(members[i]);
                Notify((object)_datastore);
            }

        
        }

        private void Ctl_Networks_Load(object sender, EventArgs e)
        {
            // just for debugging
        }



   


    }
}
