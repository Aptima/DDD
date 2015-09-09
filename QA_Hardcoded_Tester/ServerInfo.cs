using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace QA_Hardcoded_Tester
{
    public partial class ServerInfo : Form
    {
        public void GetHostnameAndPort(out String hostname, out int port)
        {
            hostname = textBox1.Text;
            port = Int32.Parse(textBox2.Text);
        }
        public ServerInfo()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isValid = false;

            try
            {
                if (textBox1.Text.Trim() == string.Empty)
                    throw new Exception();
                if (textBox2.Text.Trim() == string.Empty)
                    throw new Exception();
                int test = Int32.Parse(textBox2.Text);
                if (test < 1)
                    throw new Exception();

                isValid = true;
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            if (isValid)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Failed hostname/port validation");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
