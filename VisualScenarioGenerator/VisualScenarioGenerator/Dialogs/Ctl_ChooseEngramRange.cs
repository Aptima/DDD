using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_ChooseEngramRange : UserControl, ICtl_ContentPane__OutboundUpdate
    {
        public string EngramName = "";
        public List<string> Range = new List<string>();
        public Boolean Included = true;

        public Ctl_ChooseEngramRange()
        {
            InitializeComponent();
        }

        public Ctl_ChooseEngramRange(string engramName, List<string> range, Boolean included)
        {
            InitializeComponent();

            //Fill the cboSelectEngram with all known engram names
            List<string> allEngrams = Ctl_Engram.Engrams.GetNames();
            allEngrams.Sort();
            int selectedIndex = 0;
            cboSelectEngram.BeginUpdate();
            cboSelectEngram.Items.Clear();
            for (int i = 0; i < allEngrams.Count; i++)
            {
                cboSelectEngram.Items.Add(allEngrams[i]);
                if (allEngrams[i] == engramName)
                    selectedIndex = i;

            }
            cboSelectEngram.EndUpdate();
            //and, Fill the current values box
            cboEngramRange.BeginUpdate();
            cboEngramRange.Items.Clear();
            for (int i = 0; i < range.Count; i++)
            {
                cboEngramRange.Items.Add(range[i]);
            }
            cboEngramRange.EndUpdate();
            rbIncluded.Checked = included;
        }


        #region IVSG_ControlStateOutboundUpdate Members

        public void Update(Control control, object object_data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void rbIncludedExcluded_Click(object sender, EventArgs e)
        {
            Included = rbIncluded.Checked;
        }

        private void cboSelectEngram_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EngramName = (cboSelectEngram.SelectedItem).ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (("" != cboEngramRange.Text)
                && (cboEngramRange.Text == cboEngramRange.SelectedItem.ToString())
                )
            {
                Boolean addItToList = true;
                //can't use FindString(Exact) to see if texty already in list
                // because those routines are not case-sensitive
                for (int i = 0; i < cboEngramRange.Items.Count; i++)
                {
                    if (cboEngramRange.Text == cboEngramRange.Items[i].ToString())
                    {
                        addItToList = false;
                        break;
                    }
                }
                if (addItToList)
                {
                    cboEngramRange.Items.Insert(0, cboEngramRange.Text);
                    cboEngramRange.Text = "";
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cboEngramRange.Text == cboEngramRange.SelectedValue.ToString())
            {
                cboEngramRange.Text = "";
            }
            else
            {
                cboEngramRange.Items.RemoveAt(cboEngramRange.SelectedIndex);
            }
                
        }
    }
}
