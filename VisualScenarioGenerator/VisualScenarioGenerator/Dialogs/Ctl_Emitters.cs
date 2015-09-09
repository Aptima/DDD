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
    public partial class Ctl_Emitters : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<EmitterDataStruct> Emitters = new ObjectTypeLists<EmitterDataStruct>();

        private EmitterDataStruct _datastore = EmitterDataStruct.Empty();

        public Ctl_Emitters()
        {
            InitializeComponent();
        }
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                EmitterDataStruct data = (EmitterDataStruct)object_data;
                txtName.Text = data.ID;
                rbAll.Checked = data.AllAttributes; ;
                txLevel_1.Text = "";
                txLevel_2.Text = "";
                nndVariance_1.Value = 0;
                nndVariance_2.Value = 0;
                ckbUnlimited.Checked = data.Unlimited;

                if (!data.AllAttributes)
                {
                    if ("" == data.Attribute)
                    {
                        rbInvisible.Checked = true;
                        rbSingle.Checked = false;
                    }
                    else
                    {
                        rbInvisible.Checked = false;
                        rbSingle.Checked = true;
                        lbxAttribute.SelectedItem = data.Attribute;
                        try
                        {
                            txLevel_1.Text = data.Levels[0];
                                                nndVariance_1.Value = data.Variances[0];
               
                        }
                        catch { 
                            txLevel_1.Text = "";
                        nndVariance_1.Value = 0;
                    }
                    try
                    {
                        txLevel_2.Text = data.Levels[1];
                        nndVariance_2.Value = data.Variances[1];
                    }
                    catch
                    {
                        txLevel_2.Text = "";
                        nndVariance_2.Value = 0;
                    }
                    }
                }
                if (rbInvisible.Checked || rbAll.Checked)
                {
                    lbxAttribute.Enabled = false;
                    ckbUnlimited.Enabled = false;
                    lblLevel_1.Enabled = false;
                    lblLevel_2.Enabled = false;
                    txLevel_1.Enabled = false;
                    txLevel_2.Enabled = false;
                    lblVariance_1.Enabled = false;
                    lblVariance_2.Enabled = false;
                    nndVariance_1.Enabled = false;
                    nndVariance_2.Enabled = false;
                    lbxAttribute.Enabled = false;
                }
                else
                {
                    lbxAttribute.Enabled = true;
                    ckbUnlimited.Enabled = true;
                    if (ckbUnlimited.Checked)
                    {
                        lblLevel_1.Enabled = false;
                        lblLevel_2.Enabled = false;
                        txLevel_1.Enabled = false;
                        txLevel_2.Enabled = false;
                        lblVariance_1.Enabled = false;
                        lblVariance_2.Enabled = false;
                        nndVariance_1.Enabled = false;
                        nndVariance_2.Enabled = false;
                    
  
   

                    }
                    else
                    {
                        lblLevel_1.Enabled = true;
                        lblLevel_2.Enabled = true;
                        lblVariance_1.Enabled = true;
                        lblVariance_2.Enabled = true;
                        txLevel_1.Enabled = true;
                        txLevel_2.Enabled = true;
                        nndVariance_1.Enabled = true;
                        nndVariance_2.Enabled = true;
         
  
   
                    }
                }

            }

        }
        public void ResetForNewEntry()
        {
            lbxAttribute.Enabled = false;
            ckbUnlimited.Enabled = false;
            lblLevel_1.Enabled = false;
            lblLevel_2.Enabled = false;
            lblVariance_1.Enabled = false;
            lblVariance_2.Enabled = false;
            txLevel_1.Enabled = false;
            txLevel_2.Enabled = false;
            nndVariance_1.Enabled = false;
            nndVariance_2.Enabled = false;
            lbxAttribute.Enabled = false;
  
   
            rbAll.Checked = true;
            rbInvisible.Checked = false;
            rbSingle.Checked = false;
            txtName.Text = "";
            _datastore = new EmitterDataStruct("");
            _datastore.CheckUniquenessOnSave = true;



        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Boolean ask = false;
            if (txtName.Text != string.Empty)
            {
                if (!(Emitters.GetNames().Contains(txtName.Text)))
                {
                    ask = true;
                }
                else if (!_datastore.Equals(Emitters.GetFromList(txtName.Text)))
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

        private void btnAccept_Click(object sender, EventArgs e)
        {

            if (txtName.Text != string.Empty)
            {
                _datastore.Attribute = "";
                _datastore.ID = txtName.Text;
                if (rbAll.Checked)
                {
                    _datastore.AllAttributes = true;
                    _datastore.Levels.Clear();
                    _datastore.Variances.Clear();
                    _datastore.Unlimited = true;
                    lbxAttribute.SelectedItem = null;
                }
                else if (rbInvisible.Checked)
                {
                    _datastore.AllAttributes = false;
                    _datastore.Levels.Clear();
                    _datastore.Variances.Clear();
                    _datastore.Unlimited = true;
                    lbxAttribute.SelectedItem = null;

                }
                else // single
                {
                    _datastore.AllAttributes = false;
                    if(lbxAttribute.SelectedItem!=null)
                                    _datastore.Attribute = lbxAttribute.SelectedItem.ToString();
                    if (ckbUnlimited.Checked)
                    {
                        _datastore.Levels.Clear();
                        _datastore.Variances.Clear();
                        _datastore.Unlimited = true;
                    }
                    else
                    {
                        _datastore.Levels=new List<string>(new string[]{txLevel_1.Text,txLevel_2.Text});
                        _datastore.Variances=new List<double>(new double[]{nndVariance_1.Value,nndVariance_2.Value});            
                        _datastore.Unlimited = false;
                    }
 
                }

                Notify((object)_datastore);
            }

        }

        private void rbAll_Click(object sender, EventArgs e)
        {
            lbxAttribute.Enabled = false;
            ckbUnlimited.Enabled = false;
            lblLevel_1.Enabled = false;
            lblLevel_2.Enabled = false;
            lblVariance_1.Enabled = false;
            lblVariance_2.Enabled = false;
            txLevel_1.Enabled = false;
            txLevel_2.Enabled = false;
            nndVariance_1.Enabled = false;
            nndVariance_2.Enabled = false;
            lbxAttribute.Enabled = false;
  
   
        }

        private void rbInvisible_Click(object sender, EventArgs e)
        {
            lbxAttribute.Enabled = false;
            ckbUnlimited.Enabled = false;
            lblLevel_1.Enabled = false;
            lblLevel_2.Enabled = false;
            lblVariance_1.Enabled = false;
            lblVariance_2.Enabled = false;
            txLevel_1.Enabled = false;
            txLevel_2.Enabled = false;
            nndVariance_1.Enabled = false;
            nndVariance_2.Enabled = false;
            lbxAttribute.Enabled = false;
  
   
        }

        private void rbSingle_Click(object sender, EventArgs e)
        {
            lbxAttribute.Enabled = true;
            ckbUnlimited.Enabled = true;
            if (ckbUnlimited.Checked)
            {
                lblLevel_1.Enabled = false;
                lblLevel_2.Enabled = false;
                lblVariance_1.Enabled = false;
                lblVariance_2.Enabled = false;
                txLevel_1.Enabled = false;
                txLevel_2.Enabled = false;
                nndVariance_1.Enabled = false;
                      nndVariance_2.Enabled = false;
                  
  
   
            }
            else
            {
                lblLevel_1.Enabled = true;
                lblLevel_2.Enabled = true;
                lblVariance_1.Enabled = true;
                lblVariance_2.Enabled = true;
                txLevel_1.Enabled = true;
                txLevel_2.Enabled = true;
 
                nndVariance_1.Enabled = true;
                nndVariance_2.Enabled = true;
        
  
   
            }
         
        }

        private void ckbUnlimited_Click(object sender, EventArgs e)
        {
            // Value is the result of this click operation
            if (rbSingle.Checked && ckbUnlimited.Checked)
            {
                lblLevel_1.Enabled = false;
                lblLevel_2.Enabled = false;
                lblVariance_1.Enabled = false;
                lblVariance_2.Enabled = false;
                nndVariance_1.Enabled = false;
                nndVariance_2.Enabled = false;
                txLevel_1.Enabled = false;
                txLevel_2.Enabled = false;
 
                lbxAttribute.Enabled = true;
  
            }
            else if (rbSingle.Checked && !ckbUnlimited.Checked)
            {
                lblLevel_1.Enabled = true;
                lblLevel_2.Enabled = true;
                lblVariance_1.Enabled = true;
                lblVariance_2.Enabled = true;
                txLevel_1.Enabled = true;
                txLevel_2.Enabled = true;
 
                nndVariance_1.Enabled = true;
                nndVariance_2.Enabled = true;
                lbxAttribute.Enabled = true;
  
            }
        }









    }
}
