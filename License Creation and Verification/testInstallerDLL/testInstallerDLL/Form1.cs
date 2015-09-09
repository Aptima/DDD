using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace testInstallerDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static AptimaLicenseInfo licenseInfo;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 32)
            {
                MessageBox.Show("Invalid License Key Length.  Make sure the key was entered correctly.", "Error validating license key");
                return;
            }
            try
            {
                licenseInfo = AptimaInstallerApp.Main(textBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Validating license key.  " + ex.Message, "Error validating license key");
                return;
            }
            if (licenseInfo.IsValid)
            {
                label1.Text = "SUCCESS";
            }
            else
            {
                label1.Text = "FAILED:";
                MessageBox.Show(licenseInfo.ErrorMessage);
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (((Label)sender).Text == "SUCCESS")
            {
                KeyWasValid();
            }
            else
            {
                buttonContinue.Enabled = false;
            }
        }
        private void KeyWasValid()
        {
            buttonContinue.Enabled = true;
            button1.Enabled = false;
            textBox1.Enabled = false;
            if (licenseInfo == null)
                return;

            //populate labels that describe features of the key.
            labelProduct.Text = licenseInfo.ProductName;
            labelVersion.Text = string.Format("{0}.{1}", licenseInfo.MajorVersion, licenseInfo.MinorVersion);
            labelUserID.Text = licenseInfo.RandomUniqueID;
            labelSeats.Text = licenseInfo.NumberOfSeats.ToString();
            labelLicenseType.Text = licenseInfo.LicenseKeyTypeString;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //somehow tell install shield it exited fine
            this.Close();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            //somehow tell install shield that it exited poorly.
            this.Close();
        }
    }
}