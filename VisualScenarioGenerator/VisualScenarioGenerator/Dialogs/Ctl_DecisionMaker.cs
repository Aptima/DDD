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
    
    public partial class Ctl_DecisionMaker : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<DecisionMakerDataStruct> DecisionMakers = new ObjectTypeLists<DecisionMakerDataStruct>();

        private DecisionMakerDataStruct _datastore = DecisionMakerDataStruct.Empty();
        private NamedColorDialog colorDialog1 = new NamedColorDialog();
        public Ctl_DecisionMaker()
        {
            InitializeComponent();

        }

        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                DecisionMakerDataStruct data = (DecisionMakerDataStruct)object_data;
                txtID.Text = data.ID;
                rtbBriefing.Rtf = data.Briefing;
                txtRole.Text = data.Role;
                lblSwath.BackColor = data.DMColor;
                cboTeam.Items.Clear();
                List<string> allTeams = Ctl_Teams.Teams.GetNames();
                if (allTeams.Count > 0)
                {
                    allTeams.Sort();
                    allTeams.Insert(0, "");
                    for (int i = 0; i < allTeams.Count; i++)
                    {
                        cboTeam.Items.Add(allTeams[i]);
                    }
                    // FindExact on cboBox is not case sensitive
                    cboTeam.SelectedIndex = 0;
                    for (int i = 0; i < allTeams.Count; i++)
                    {
                        if (allTeams[i] == data.Team)
                        {
                            cboTeam.SelectedIndex = i;
                            break;
                        }
                    }

                }
            }
        }
        public void ResetForNewEntry()
        {
            this.txtID.Text = "";
            this.txtRole.Text = "";
            /* teams handling moved to Layout handler
            this.cboTeam.Items.Clear();
            List<string> allTeams = Ctl_Teams.Teams.GetNames();
            allTeams.Sort();
            for (int i = 0; i < allTeams.Count; i++)
            {
                cboTeam.Items.Add(allTeams[i]);
            }
            */
            this.cboTeam.Text = "";
            this.rtbBriefing.Text = "";
            this.lblSwath.BackColor = Color.Black;
            _datastore = new DecisionMakerDataStruct("");
            _datastore.CheckUniquenessOnSave = true;
        }



     

      

   

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnChooseColor_Click(object sender, EventArgs e)
        {

     
  
            
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                lblSwath.BackColor = colorDialog1.ChosenColor;
            }
/*
 * Why? Just do on Accept, no?
            if (txtID.Text != string.Empty)
            {
                _datastore.ID = txtID.Text;
                Notify((object)_datastore);
            }
            */
        }

      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtID.Text != string.Empty)
            {
                if (!(DecisionMakers.GetNames().Contains(txtID.Text)))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(DecisionMakers.GetFromList(txtID.Text)))
                {
                    ask = true;
                }
            }
            if (!ask)
            {
                ResetForNewEntry();
                return;
            }
            DialogResult answer = MessageBox.Show("Do you want to save this Decision Maker?", "Visual Scenario Generator", MessageBoxButtons.YesNoCancel);
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

            // Don't allow user to erase the object name.
            if (txtID.Text != string.Empty)
            {
                _datastore.ID = txtID.Text;
                _datastore.Briefing = rtbBriefing.Rtf;
                _datastore.Role = txtRole.Text;
                _datastore.DMColor = lblSwath.BackColor;
                _datastore.Team = "";
                if (null != cboTeam.SelectedItem)
                    _datastore.Team = cboTeam.SelectedItem.ToString();


                Notify((object)_datastore);
            }
           
        }

 /*       private void Ctl_DecisionMaker_Layout(object sender, LayoutEventArgs e)
        {
            this.cboTeam.Items.Clear();
            List<string> allTeams = Ctl_Teams.Teams.GetNames();
            if(allTeams.Count>0)
            {
            allTeams.Sort();
            for (int i = 0; i < allTeams.Count; i++)
            {
                cboTeam.Items.Add(allTeams[i]);
            }
           // FindExact on cboBox is not case sensitive
            cboTeam.SelectedIndex = 0;
            for (int i = 0; i < allTeams.Count; i++)
            {
     //           if(allTeams[i]==
            }
            }
        }
        */




    }

    public class DecisionMakerDataStruct : VisualScenarioGenerator.VSGPanes.ObjectTypeStructure
    {
        public Color DMColor=System.Drawing.Color.Black;
        public string Role = "";
        public string Briefing="";
        public string Team="";
        public static DecisionMakerDataStruct Empty()
        {
            return new DecisionMakerDataStruct("");
        }

        public DecisionMakerDataStruct(string name):base(name,true)
        {
   
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is DecisionMakerDataStruct)
            {
                return ((DecisionMakerDataStruct)obj).ID == ID;
            }
            return false;
        }
        #region ICloneable Members

        public override object Clone()
        {
            DecisionMakerDataStruct obj = new DecisionMakerDataStruct(ID);
            obj.Team = Team;
            obj.Role = Role;
            obj.Briefing = Briefing;
            obj.DMColor = DMColor;
             return obj;
        }

        #endregion

    }

}
