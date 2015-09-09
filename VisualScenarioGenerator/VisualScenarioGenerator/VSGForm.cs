using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.CommonComponents.LicenseVerifier;
using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    public partial class VSGForm : Form, IVSGForm
    {

        public VSGForm()
        {

            InitializeComponent();
        }

        #region IVSGForm Members

        public void VSG_ViewChange(ViewType view)
        {
            Console.WriteLine("View Changed: {0}", view.ToString());
            this.toolStripStatusLabel1.Text = view.ToString();
        }
        //If you are closing this form, and don't want to prompt the user with "Are you sure you
        // want to exit?", then set this to true before calling this.Close()
        private bool _onFormClosingOverride = false;  

        #region Used with License
        private const string _productName = "VSG";
        private const string _productCode = "_VSG";
        private const string _productFamily = "Asim";
        private const string _productDisplayName = "Visual Scenario Generator";
        #endregion

        #endregion


        private void vsG_Panel1_Load(object sender, EventArgs e)
        {
            this.Show();
            vsG_Panel1.Hide();

            bool loop = true;

            SplashScreen splash = new SplashScreen();
            DatabaseDialog db = new DatabaseDialog();

            try
            {
                while (loop)
                {
                    db.ShowDialog(this);

                    if (db.DialogResult == DialogResult.Cancel)
                    {
                        if (MessageBox.Show("Are you sure you want to quit?", "Quitting the Visual Scenario Generator", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            loop = false;
                            Application.Exit();
                            return;
                        }
                    }
                    else
                    {
                        splash.ShowDialog(this);
                        if (splash.DialogResult == DialogResult.Cancel)
                        {
                            if (MessageBox.Show("Are you sure you want to quit?", "Quitting the Visual Scenario Generator", MessageBoxButtons.YesNo) ==
        DialogResult.Yes)
                            {
                                loop = false;
                                Application.Exit();
                                return;
                            }
                        }
                        else
                        {
                            loop = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
            vsG_Panel1.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AD: This if statement is added so if the application wants to shut down without giving the 
            //user the option of Yes/No, then the form will not prompt them.  This is set in the license
            //verifying routine if there is no valid license and the form is closing.
            if (_onFormClosingOverride)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to quit?", "Quitting the Visual Scenario Generator", MessageBoxButtons.YesNo) ==
    DialogResult.No)
            {
                e.Cancel = true;
            }

        }

        private void VSGForm_Load(object sender, EventArgs e)
        {
#if !DEBUG
            string licenseKey = string.Empty;
            string keyType = string.Empty;
            string expDate = string.Empty;
            DateTime expirationDate = new DateTime(1200,1,1);
            AptimaLicenseVerifierForm verifier;

            if (AptimaLicenseVerifierForm.GetLicenseKeyInfo(_productFamily, _productName, _productCode, out licenseKey, out expDate, out keyType) == false)
            {
                if (MessageBox.Show("Unable to locate your license key information.  Would you like to enter it now?",
                    "Error verifying license information", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    verifier = new AptimaLicenseVerifierForm(_productFamily, _productName, _productCode, _productDisplayName);
                    if (verifier.ShowDialog() == true)
                    { //returns true if key is entered and valid
                        AptimaLicenseVerifierForm.GetLicenseKeyInfo(_productFamily, _productName, _productCode, out licenseKey, out expDate, out keyType);
                    }
                    else
                    {
                        HardClose("No valid key entered.  Exiting Visual Scenario Generator.", "");
                        return;
                    }
                }
                else
                {
                    HardClose("Exiting Visual Scenario Generator.", "");
                    return;
                }
            }

            try
            {
                expirationDate = new DateTime(Convert.ToInt32(expDate.Substring(0, 4)), Convert.ToInt32(expDate.Substring(4, 2)), Convert.ToInt32(expDate.Substring(6, 2)));
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format("Error in License Key Verification: {0}", ex.Message), ex);
            }

            if (DateTime.Now.CompareTo(expirationDate) > 0) 
                //-1 if exp is at a later date than DT.Now
                //0 if equal
                //1 if exp date is before DT.Now (already expired
            { //key is expired, give user chance to re-new key
                if (MessageBox.Show("Your license key has expired.  Would you like to enter an updated key?",
                                    "License Key Expired", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    verifier = new AptimaLicenseVerifierForm(_productFamily, _productName, _productCode, _productDisplayName);
                    if (verifier.ShowDialog() == true)
                    { //returns true if key is entered and valid
                        AptimaLicenseVerifierForm.GetLicenseKeyInfo(_productFamily, _productName, _productCode, out licenseKey, out expDate, out keyType);
                    }
                    else
                    {
                        HardClose("No valid key entered.  Exiting Visual Scenario Generator.", "");
                        return;
                    }
                }
                else
                {
                    HardClose("Exiting Visual Scenario Generator.", "");
                    return;
                }
            }
#endif
        }

        private void HardClose(string dialogText, string dialogCaption)
        {
            MessageBox.Show(dialogText, dialogCaption, MessageBoxButtons.OK);
            _onFormClosingOverride = true;
            this.Close();
        }
    }
}