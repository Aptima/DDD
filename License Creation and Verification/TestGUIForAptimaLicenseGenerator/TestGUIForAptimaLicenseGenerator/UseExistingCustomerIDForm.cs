using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestGUIForAptimaLicenseGenerator
{
    public partial class UseExistingCustomerIDForm : Form
    {
        private static string custID = string.Empty;
        public static string GetCustomerID()
        {
            return custID;
        }
        public UseExistingCustomerIDForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbCustomerID.Text.Length == 4)
            {
                custID = tbCustomerID.Text.ToUpper();
                this.Close();
                return;
            }

            if (tbCustomerID.Text.Length > 4)
            {
                custID = tbCustomerID.Text.Substring(0, 4).ToUpper();
                this.Close();
                return;
            }

            int x = tbCustomerID.Text.Length;
            StringBuilder st = new StringBuilder(tbCustomerID.Text);
            while (st.Length < 4)
            {
                st.Append("0");
            }
            custID = st.ToString().ToUpper();
            this.Close();
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            custID = string.Empty;
            this.Close();
        }
    }
}