using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Xml.Schema;

namespace AME.Views.Forms
{
    public partial class SqlServerExpressSetupForm : Form
    {
        private XmlDocument xmlDatabaseConfiguration;

        public String ServerHost
        {
            get
            {
                return tbServerHost.Text;
            }
            set
            {
                tbServerHost.Text = value;
            }
        }

        public String Database
        {
            get
            {
                return tbDatabase.Text;
            }
            set
            {
                tbDatabase.Text = value;
            }
        }

        public AME.AMEManager.AuthenticationMode AuthenticationMode
        {
            get
            {
                try
                {
                    Enum eMode = comboBox1.SelectedValue as Enum;
                    AME.AMEManager.AuthenticationMode mode = (AME.AMEManager.AuthenticationMode)eMode;
                    return mode;
                }
                catch (System.Exception)
                {
                    return AME.AMEManager.AuthenticationMode.WindowsAuthentication;
                }
            }
            set
            {
                if (Enum.IsDefined(typeof(AME.AMEManager.AuthenticationMode), value))
                {
                    comboBox1.SelectedValue = value;
                }
            }
        }

        public String Username
        {
            get
            {
                return tbUsername.Text;
            }
            set
            {
                tbUsername.Text = value;
            }
        }
        
        public String Password
        {
            get
            {
                return tbPassword.Text;
            }
        }

        public SqlServerExpressSetupForm()
        {
            InitializeComponent();

            comboBox1.DataSource = AME.AMEManager.EnumDescription.GetValuesAndDescription(typeof(AME.AMEManager.AuthenticationMode));
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SqlServerExpressSetupForm_Activated(object sender, EventArgs e)
        {
            tbPassword.Focus();
        }

        private void SqlServerExpressSetupForm_Load(object sender, EventArgs e)
        {
            Boolean valid = false;
            xmlDatabaseConfiguration = new XmlDocument();

            try
            {
                Assembly exeAssembly = Assembly.GetExecutingAssembly();
                String currentPath = Directory.GetParent(exeAssembly.Location).FullName;
                FileInfo fileDatabaseConfiguration = new FileInfo(Path.Combine(currentPath, "databaseConfiguration.xml"));

                if (fileDatabaseConfiguration.Exists)
                {
                    StreamReader streamReader = null;
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    String[] names = entryAssembly.GetManifestResourceNames();
                    foreach (String name in names)
                    {
                        if (name.Contains("databaseConfig.xsd"))
                        {
                            try
                            {
                                streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(name));
                            }
                            catch (System.Exception)
                            {
                                valid = false;
                                xmlDatabaseConfiguration = new XmlDocument();
                            }
                        }                    
                    }
                    //StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(entryAssembly.GetName().Name + ".Config.ConfigXSD.databaseConfig.xsd"));
                    if (streamReader != null)
                    {
                        XmlTextReader xsdXmlTextReader = new XmlTextReader(streamReader);

                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.ValidationType = ValidationType.Schema;
                        settings.Schemas.Add(null, xsdXmlTextReader);
                        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                        settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

                        //Create reader with settings
                        XmlReader xmlReader = XmlReader.Create(fileDatabaseConfiguration.FullName, settings);
                        try
                        {
                            xmlDatabaseConfiguration.Load(xmlReader);
                            valid = true;
                        }
                        catch (System.Exception)
                        {
                            valid = false;
                            xmlDatabaseConfiguration = new XmlDocument();
                        }
                        finally
                        {
                            xsdXmlTextReader.Close();
                            xmlReader.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not find databaseConfig.xsd - is it embedded as a project resource?", "Database Configuration");
                    }
                }

                if (valid)
                {
                    XmlNode nodeDatabase = xmlDatabaseConfiguration.SelectSingleNode("Database");
                    String server = nodeDatabase.Attributes["server"].Value;
                    String database = nodeDatabase.Attributes["database"].Value;
                    String authenticationMode = nodeDatabase.Attributes["authenticationMode"].Value;
                    String username = nodeDatabase.Attributes["username"].Value;

                    tbServerHost.Text = server;
                    tbDatabase.Text = database;
                    if (Enum.IsDefined(typeof(AME.AMEManager.AuthenticationMode), authenticationMode))
                    {
                        comboBox1.SelectedValue = Enum.Parse(typeof(AME.AMEManager.AuthenticationMode), authenticationMode);
                    }
                    tbUsername.Text = username;
                }
                else
                {
                    tbServerHost.Text = "localhost";
                    tbDatabase.Text = "database";
                    comboBox1.SelectedValue = AME.AMEManager.AuthenticationMode.WindowsAuthentication;
                    tbUsername.Text = "sa";
                }
            }
            catch (Exception)
            {
                tbServerHost.Text = "localhost";
                tbDatabase.Text = "database";
                comboBox1.SelectedValue = AME.AMEManager.AuthenticationMode.WindowsAuthentication;
                tbUsername.Text = "sa";
            }
        }

        private void validationEventHandler(object sender, ValidationEventArgs args)
        {
            String message = String.Empty;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    message = "Import Error: " + args.Message;
                    break;
                case XmlSeverityType.Warning:
                    message = "Import Warning: " + args.Message;
                    break;
            }

            throw new System.Xml.Schema.XmlSchemaValidationException(args.Message);
        }

        private void SqlServerExpressSetupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason.Equals(CloseReason.UserClosing))
            {
                if (!xmlDatabaseConfiguration.InnerXml.Equals(String.Empty))
                {
                    XmlNode nodeDatabase = xmlDatabaseConfiguration.SelectSingleNode("Database");
                    nodeDatabase.Attributes["server"].Value = tbServerHost.Text;
                    nodeDatabase.Attributes["authenticationMode"].Value = comboBox1.SelectedValue.ToString();
                    nodeDatabase.Attributes["database"].Value = tbDatabase.Text;
                    nodeDatabase.Attributes["username"].Value = tbUsername.Text;
                }
                else
                {
                    xmlDatabaseConfiguration = new XmlDocument();
                    
                    XmlDeclaration declaration = xmlDatabaseConfiguration.CreateXmlDeclaration("1.0", System.Text.Encoding.UTF8.WebName, String.Empty);
                    xmlDatabaseConfiguration.AppendChild(declaration);

                    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDatabaseConfiguration.NameTable);
                    namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                    XmlElement root = xmlDatabaseConfiguration.CreateElement("Database");
                    xmlDatabaseConfiguration.AppendChild(root);

                    XmlAttribute server = xmlDatabaseConfiguration.CreateAttribute("server");
                    server.Value = tbServerHost.Text;
                    root.SetAttributeNode(server);

                    XmlAttribute database = xmlDatabaseConfiguration.CreateAttribute("database");
                    database.Value = tbDatabase.Text;
                    root.SetAttributeNode(database);

                    XmlAttribute authenticationMode = xmlDatabaseConfiguration.CreateAttribute("authenticationMode");
                    authenticationMode.Value = comboBox1.SelectedValue.ToString();
                    root.SetAttributeNode(authenticationMode);

                    XmlAttribute username = xmlDatabaseConfiguration.CreateAttribute("username");
                    username.Value = tbUsername.Text;
                    root.SetAttributeNode(username);

                    XmlAttribute schema = xmlDatabaseConfiguration.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    schema.Value = "databaseConfig.xsd";
                    root.SetAttributeNode(schema);
                }

                try
                {
                    Assembly exeAssembly = Assembly.GetExecutingAssembly();
                    String currentPath = Directory.GetParent(exeAssembly.Location).FullName;
                    FileInfo fileDatabaseConfiguration = new FileInfo(Path.Combine(currentPath, "databaseConfiguration.xml"));

                    xmlDatabaseConfiguration.Save(fileDatabaseConfiguration.FullName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Configuration");
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Enum eMode = comboBox1.SelectedValue as Enum;
            if (eMode == null)
                return;
            AME.AMEManager.AuthenticationMode mode = (AME.AMEManager.AuthenticationMode)eMode;

            switch (mode)
            {
                case AME.AMEManager.AuthenticationMode.WindowsAuthentication:
                    tbUsername.Enabled = false;
                    tbPassword.Enabled = false;
                    break;
                case AME.AMEManager.AuthenticationMode.SQLServerAuthentication:
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    break;
            }
        }
    }
}