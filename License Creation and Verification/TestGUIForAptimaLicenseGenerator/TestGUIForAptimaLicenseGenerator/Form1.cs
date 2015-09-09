using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aptima.CommonComponent.AptimaLicenseVerifier;

namespace TestGUIForAptimaLicenseGenerator
{
    public partial class Form1 : Form
    {
        public class Key
        {
            private string licenseKey = string.Empty;

            public Key(string s)
            {
                licenseKey = s;
            }

            public string ToString()
            {
                if (licenseKey.Length != 32)
                    return string.Empty;

                return String.Format("{0}  {1}  {2}  {3}", licenseKey.Substring(0, 8), licenseKey.Substring(8, 8), licenseKey.Substring(16, 8), licenseKey.Substring(24, 8)); 
            }
            public string ToActualString()
            {
                return licenseKey;
            }
        }

        private static Dictionary<string, string> productKeyMapping = new Dictionary<string, string>(); //[full prod name,prodID]
        public static Key _licenseKey;

        public Form1()
        {
            productKeyMapping.Add("DDD 4.0", "_DDD");
            productKeyMapping.Add("MOST", "MOST");
            productKeyMapping.Add("Performance Workbench", "_PWB");
            productKeyMapping.Add("PM Engine", "_PME");
            productKeyMapping.Add("Spotlite", "SPOT");
            productKeyMapping.Add("VSG", "_VSG");
            productKeyMapping.Add("After Action Review", "_AAR");

            InitializeComponent();
            foreach (string s in productKeyMapping.Keys)
            {
                cbProductName.Items.Add(s);
            }
            cbProductName.SelectedIndex = 0;
            cbLicenseType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long rem;
            Math.DivRem(DateTime.Now.Ticks, 32000, out rem);
            int ran = Convert.ToInt32(rem);
            Random r = new Random(ran);
            StringBuilder list = new StringBuilder();
            int yeah;
            for (int x = 0; x < 4; x++)
            {
                yeah = r.Next(16);
                switch (yeah)
                {
                    case 0:
                        list.Append("0");
                        break;
                    case 1:
                        list.Append("1");
                        break;
                    case 2:
                        list.Append("2");
                        break;
                    case 3:
                        list.Append("3");
                        break;
                    case 4:
                        list.Append("4");
                        break;
                    case 5:
                        list.Append("5");
                        break;
                    case 6:
                        list.Append("6");
                        break;
                    case 7:
                        list.Append("7");
                        break;
                    case 8:
                        list.Append("8");
                        break;
                    case 9:
                        list.Append("9");
                        break;
                    case 10:
                        list.Append("A");
                        break;
                    case 11:
                        list.Append("B");
                        break;
                    case 12:
                        list.Append("C");
                        break;
                    case 13:
                        list.Append("D");
                        break;
                    case 14:
                        list.Append("E");
                        break;
                    case 15:
                        list.Append("F");
                        break;
                    default:
                        throw new Exception("Apparently random doesn't work, go find Adam");
                        break;
                }
            }
            tbRandomNumber.Text = list.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool toReturn = false;
            if (tbRandomNumber.Text == string.Empty)
                toReturn = true;
            if (tbNumOfUsers.Text == string.Empty)
                toReturn = true;
            if (tbMinorVersion.Text == string.Empty)
                toReturn = true;
            if (tbMajorVersion.Text == string.Empty)
                toReturn = true;
            if (toReturn)
            {
                MessageBox.Show("Missing required field.");
                return;
            }

            if (Convert.ToInt32(tbNumOfUsers.Text) > 255)
            {
                MessageBox.Show("Too many bits (255 Max)");
                return;
            }
            StringBuilder inputString = new StringBuilder();
            //[_DDD][04][01][2007][10][10][2][A8][RAND]

            try
            {
                inputString.Append(productKeyMapping[Convert.ToString(cbProductName.SelectedItem)]);
            }
            catch
            {
                MessageBox.Show("Error trying to handle product code.");
                return;
            }
            try
            {
                
                if (tbMajorVersion.Text.Length == 1)
                    inputString.Append("0");
                inputString.Append(tbMajorVersion.Text);
                if (tbMinorVersion.Text.Length == 1)
                    inputString.Append("0");
                inputString.Append(tbMinorVersion.Text);
                inputString.AppendFormat("{0:yyyyMMdd}", dtExpirationDate.Value);
                inputString.Append(cbLicenseType.SelectedIndex);
                inputString.Append(AptimaLicenseVerifier.ConvertNumberStringToHex(tbNumOfUsers.Text));
                inputString.Append(tbRandomNumber.Text);
            }
            catch
            {
                throw new Exception("Error trying to create input string");
            }

            labelInputString.Text = inputString.ToString();
            labelLength2.Text = String.Format("Length: {0}", labelInputString.Text.Length);
            //tbLicenseKey.Text = AptimaLicenseVerifier.GenerateLicenseKey(labelInputString.Text);
            _licenseKey = new Key(AptimaLicenseVerifier.GenerateLicenseKey(labelInputString.Text));
            tbLicenseKey.Text = _licenseKey.ToString();
            labelLength.Text = String.Format("Length: {0}", _licenseKey.ToActualString().Length);

        }

        private void cbLicenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newIndex = ((ComboBox)sender).SelectedIndex;
            switch (newIndex)
            { 
                case 0:
                    dtExpirationDate.Value = DateTime.Now.AddDays(30);
                    //dtExpirationDate.Enabled = false;
                    break;
                case 1:
                    dtExpirationDate.Value = DateTime.Now.AddDays(90);
                    //dtExpirationDate.Enabled = false;
                    break;
                case 2:
                    dtExpirationDate.Value = new DateTime(2099, 12, 31, 23, 59, 59);
                    //dtExpirationDate.Enabled = true;
                    break;
                default:
                    return;
            }
        }

        private void tbLicenseKey_TextChanged(object sender, EventArgs e)
        {
            if (tbLicenseKey.Text == string.Empty)
            {
                labelValidity.Text = "";
                button4.Enabled = false;
                return;
            }

            //AptimaLicenseInfo li = AptimaLicenseVerifier.VerifyLicenseKey(tbLicenseKey.Text);
            AptimaLicenseInfo li = AptimaLicenseVerifier.VerifyLicenseKey(_licenseKey.ToActualString());
            if (li.IsValid)
            {
                labelValidity.Text = "It's Valid!";
                button4.Enabled = true;
            }
        }
        private string ConvertFromNumberToHexString(string numString)
        {
            bool keepGoing = true;
            int numInt = Convert.ToInt32(numString);
            int remainder;
            int quotient;
            StringBuilder sb = new StringBuilder();
            //if ((numInt / 16) > 0)
            //{
                quotient = Math.DivRem(numInt, 16, out remainder);
                if (quotient < 10)
                {
                    sb.Append(quotient);
                }
                else
                {
                    switch (quotient)
                    { 
                        case 10:
                            sb.Append("A");
                            break;
                        case 11:
                            sb.Append("B");
                            break;
                        case 12:
                            sb.Append("C");
                            break;
                        case 13:
                            sb.Append("D");
                            break;
                        case 14:
                            sb.Append("E");
                            break;
                        case 15:
                            sb.Append("F");
                            break;
                    }
                }
                if (remainder < 10)
                {
                    sb.Append(remainder);
                }
                else
                {
                    switch (remainder)
                    {
                        case 10:
                            sb.Append("A");
                            break;
                        case 11:
                            sb.Append("B");
                            break;
                        case 12:
                            sb.Append("C");
                            break;
                        case 13:
                            sb.Append("D");
                            break;
                        case 14:
                            sb.Append("E");
                            break;
                        case 15:
                            sb.Append("F");
                            break;
                    }
                }
            //}
            //else
            //{
            //    sb.Append("0");
            //}
            return sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UseExistingCustomerIDForm form = new UseExistingCustomerIDForm();
            form.ShowDialog(this);
            string s = UseExistingCustomerIDForm.GetCustomerID();
            if (s != string.Empty)
            {
                tbRandomNumber.Text = s;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(tbLicenseKey.Text, true);
            //copy to clipboard
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will eventually have a dynamic dialog that, depending on the selected product, will display that product's specific use for the custom bits");
        }

        private void cbProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem.ToString() == "DDD 4.0")
            {
                label6.Text = "Number of seats (DDD)";
            }
            else
            {
                label6.Text = "Product Specific Custom Bits";
            }
        }
    }
}