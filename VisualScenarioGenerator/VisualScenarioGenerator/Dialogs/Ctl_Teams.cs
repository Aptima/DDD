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
    public partial class Ctl_Teams : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<TeamDataStruct> Teams = new ObjectTypeLists<TeamDataStruct>();
        private TeamDataStruct _datastore = TeamDataStruct.Empty();

        public Ctl_Teams()
        {
            InitializeComponent();
            bbOpponents.AllowMultipleSelection(true);
            bbOpponents.SortLists(true);
            bbOpponents.SetLabels("Available Teams", "Opponents");
        }
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                TeamDataStruct data = (TeamDataStruct)object_data;
                txtName.Text = data.ID;
                _datastore.Opponents.Clear();
                for (int i = 0; i < data.Opponents.Count; i++)
                    _datastore.Opponents.Add(data.Opponents[i]);

                bbOpponents.DisplayLists(MakeNeutrals(), _datastore.Opponents);
            }


        }

        public void ResetForNewEntry()
        {
            this.txtName.Text = "";
            _datastore = new TeamDataStruct("");
            _datastore.CheckUniquenessOnSave = true;
            bbOpponents.DisplayLists(MakeNeutrals(), _datastore.Opponents);
        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                List<string> newOpponents = bbOpponents.GetCurrent();
                for (int i = 0; i < newOpponents.Count; i++)
                    _datastore.AddOpponent(newOpponents[i].ToString());
                Notify((object)_datastore);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtName.Text != string.Empty)
            {
                if (!Teams.GetNames().Contains(txtName.Text))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(Teams.GetFromList(txtName.Text)))
                {
                    ask = true;
                }
            }
            if (!ask)
            {
                ResetForNewEntry();
                return;
            }
            DialogResult answer = MessageBox.Show("Do you want to save this Team?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
            if (DialogResult.Yes == answer)
            {
                btnAccept_Click(sender, e);
            }
            else if (DialogResult.No == answer)
            {
                ResetForNewEntry();
            }
        }


        private List<string> MakeNeutrals()
        {
            List<string> returnValue = new List<string>();
            List<string> all = Teams.GetNames();
            for (int i = 0; i < all.Count; i++)
            {
                if (!_datastore.Opponents.Contains(all[i]))
                    returnValue.Add(all[i]);
            }
            /*            if ((addCurrent) && (!returnValue.Contains(txtName.Text)) &&
                            (!_datastore.Opponents.Contains(txtName.Text)))
                            returnValue.Add(txtName.Text);*/
            return returnValue;

        }





        private void txtName_Leave(object sender, EventArgs e)
        {
            List<string> opponents = bbOpponents.GetCurrent();
            List<string> neutrals = bbOpponents.GetAvailable();
            if (("" != txtName.Text))
            {
                if ((!opponents.Contains(txtName.Text)) && (!neutrals.Contains(txtName.Text)))
                    neutrals.Add(txtName.Text);
            }
            bbOpponents.DisplayLists(neutrals, opponents);

        }

        private void txtName_MouseLeave(object sender, EventArgs e)
        {
            txtName_Leave(sender, e);
        }




    }
}
