using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.Client.Controls;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            modifiableAttributePanel1.SetLabelName("Attributes:");
            modifiableAttributePanel1.SetLabelFont(new Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            modifiableAttributePanel1.SetLabelBackgroundColor(System.Drawing.SystemColors.ControlDark);
            modifiableAttributePanel1.SetCallbackFunction(CallbackFunction);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModifiableAttributePair att = new ModifiableAttributePair();
            att.Attribute = textBoxAttributeName.Text;
            att.Value = textBoxAttributeValue.Text;
            att.Modifiable = checkBoxModify.Checked;

            modifiableAttributePanel1.AddAttribute(att);

            textBoxAttributeName.Text = string.Empty;
            textBoxAttributeValue.Text = string.Empty;
            checkBoxModify.Checked = false;
        }

        private void CallbackFunction(object sender, EventArgs e)
        {
            //try
            //{
            //    if (e.KeyChar == 13) //enter key
            //    { 
            //    //send update tag function
                    string att = ((TextBox)sender).Name.Replace("_val_", "");
                    string value = ((TextBox)sender).Text;
                    Console.WriteLine(String.Format("Attribute Update: {0} -> {1}", att, value));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}