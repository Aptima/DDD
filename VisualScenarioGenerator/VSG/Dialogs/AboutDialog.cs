using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VSG.Dialogs
{
    public partial class AboutDialog : Form
    {
        private string _urlToNoticeFile = string.Empty;

        public AboutDialog(string urlToNoticeFile)
        {
            InitializeComponent();
            this.Text = String.Format("About VSG {0}.{1}", Program._productMajorVersion, Program._productMinorVersion);
            this.labelProductInfo.Text = String.Format("{0} Ver {1}.{2}", Program._productDisplayName, Program._productMajorVersion, Program._productMinorVersion);
            this.labelCompileDate.Text = String.Format("Compiled on: {0}", Program._buildDate);
            this.linkLabelAsimSite.Text = Program._moreInfo;
            this.linkLabelNoticeFile.Text = "View AME Notice File";
            _urlToNoticeFile = urlToNoticeFile;
        }

        private void linkLabelNoticeFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (System.IO.File.Exists(_urlToNoticeFile))
            {
                try
                {
                    System.Diagnostics.Process.Start(_urlToNoticeFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error attempting to load AME Notice File");
                }
            }
            else
            {
                MessageBox.Show("AME Notice File not found at location: " + _urlToNoticeFile, "Error attempting to load AME Notice File");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelAsimSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //if (System.IO.File.Exists(urlToPDF))
            //{
                try
                {
                    System.Diagnostics.Process.Start(Program._moreInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error attempting to load Product Web Site");
                }
        //    }
        //    else
        //    {
        //        MessageBox.Show("User Guide not found at location: " + urlToPDF, "Error attempting to load User Guide");
        //    }
            }
    }
}