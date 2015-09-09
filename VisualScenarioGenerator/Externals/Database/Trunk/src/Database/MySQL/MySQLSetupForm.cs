using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Database
{
    public partial class MySQLSetupForm : Form
    {
        public String ServerHost
        {
            get
            {
                return tbServerHost.Text;
            }
        }

        public Int32 Port
        {
            get
            {
                return Convert.ToInt32(tbPort.Text);
            }
        }

        public String Database
        {
            set
            {
                tbDatabase.Text = value;
            }
            get
            {
                return tbDatabase.Text;
            }
        }

        public String Username
        {
            get
            {
                return tbUsername.Text;
            }
        }
        
        public String Password
        {
            get
            {
                return tbPassword.Text;
            }
        }

        public Boolean Import
        {
            get
            {
                return cbImportSampleData.Checked;
            }
        }

        public String Data
        {
            get
            {
                return tbData.Text;
            }
        }

        public MySQLSetupForm()
        {
            InitializeComponent();

            // Initialize default values from database.xml
            String exePath = Directory.GetCurrentDirectory();
            String configurationPath = exePath + @"\Config";

            String file = configurationPath + @"\defaultdatabase.xml";

            String serverAttribute = "server";
            String databaseAttribute = "database";
            String userAttribute = "user";
            String portAttribute = "port";

            try
            {
                FileInfo xmlFile = new FileInfo(file);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile.OpenText());

                XmlNode dbNode = xmlDoc.SelectSingleNode("Database");

                if (dbNode != null && dbNode.Attributes.Count > 0)
                {
                    if (dbNode.Attributes[serverAttribute] != null)
                    {
                        tbServerHost.Text = dbNode.Attributes[serverAttribute].Value;
                    }

                    if (dbNode.Attributes[databaseAttribute] != null)
                    {
                        tbDatabase.Text = dbNode.Attributes[databaseAttribute].Value;
                    }

                    if (dbNode.Attributes[userAttribute] != null)
                    {
                        tbUsername.Text = dbNode.Attributes[userAttribute].Value;
                    }

                    if (dbNode.Attributes[portAttribute] != null)
                    {
                        tbPort.Text = dbNode.Attributes[portAttribute].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to read from database.xml at path '{0}'!", configurationPath));
            }
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            if (Database.Equals(String.Empty))
            {
                MessageBox.Show("Enter a database name to use!", "Database Name Needed", MessageBoxButtons.OK);
            }
            else
            {
                if (Data.Equals(String.Empty) && cbImportSampleData.Checked)
                {
                    MessageBox.Show("Import Sample Data is selected and SQL file has not been chosen!", "Import File Needed", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult result = MessageBox.Show("This will restore the DB to the default installation state!", "Initialize Database", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        MySqlDB db = new MySqlDB(ServerHost, Port, Username, Password, Database, Import);
                        db.CreateDB(Data);
                    }
                    this.Close();
                }
            }
        }

        private void SetupForm_Activated(object sender, EventArgs e)
        {
            tbPassword.Focus(); // password field focused on startup
        }

        private void bBrowseData_Click(object sender, EventArgs e)
        {
            String currentDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = @"SQL files|*.sql";
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"config\data");

            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                FileInfo file = new FileInfo(openFileDialog1.FileName);
                tbData.Text = file.Name;
            }
            Directory.SetCurrentDirectory(currentDirectory);
        }
    }
}