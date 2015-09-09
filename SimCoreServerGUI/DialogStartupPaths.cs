using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class DialogStartupPaths : Form
    {
        private static string XMLFilter = "XML File(*.xml)|*.xml";
        private static string TXTFilter = "TXT File(*.txt)|*.txt";
        private static string XSDFilter = "XSD File(*.xsd)|*.xsd";
        private static OpenFileDialog ofd = new OpenFileDialog();
        private static SaveFileDialog sfd = new SaveFileDialog();
        private static FolderBrowserDialog fbd = new FolderBrowserDialog();

        public DialogStartupPaths()
        {
            InitializeComponent();
            ofd.InitialDirectory = Form1.ApplicationPath;
            sfd.InitialDirectory = Form1.ApplicationPath;
            LoadPreviousFileInfo();
        }

        private void buttonSimModel_Click(object sender, EventArgs e)
        {
            ofd.Filter = XMLFilter;
            if (File.Exists(textBoxSimModel.Text))
            {
                ofd.InitialDirectory = textBoxSimModel.Text.Remove(textBoxSimModel.Text.LastIndexOf('\\'));
                ofd.FileName = textBoxSimModel.Text;
            }
            else
            {
                ofd.InitialDirectory = Form1.ApplicationPath;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxSimModel.Text = ofd.FileName;
            }
        }

        private void buttonSchema_Click(object sender, EventArgs e)
        {
            ofd.Filter = XSDFilter;
            if (File.Exists(textBoxSchema.Text))
            {
                ofd.InitialDirectory = textBoxSchema.Text.Remove(textBoxSchema.Text.LastIndexOf('\\'));
                ofd.FileName = textBoxSchema.Text;
            }
            else
            {
                ofd.InitialDirectory = Form1.ApplicationPath;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxSchema.Text = ofd.FileName;
            }
        }

        private void buttonScenario_Click(object sender, EventArgs e)
        {
            ofd.Filter = XMLFilter;
            if (File.Exists(textBoxScenario.Text))
            {
                ofd.InitialDirectory = textBoxScenario.Text.Remove(textBoxScenario.Text.LastIndexOf('\\'));
                ofd.FileName = textBoxScenario.Text;
            }
            else
            {
                ofd.InitialDirectory = Form1.ApplicationPath;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxScenario.Text = ofd.FileName;
            }
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxLog.Text))
            {
                fbd.SelectedPath = textBoxLog.Text;
            }
            else
            {
                fbd.SelectedPath = Form1.ApplicationPath;
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxLog.Text = fbd.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//save defaults
            if (SaveDefaultsToFile())
            {
                this.Close();
            }
        }
        private void LoadPreviousFileInfo()
        {
            string filePath = System.Windows.Forms.Application.StartupPath + "\\default.txt";
            string[] delimiters = { ";" };

            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string currPath;

                currPath = sr.ReadLine();
                string[] parts;
                while (currPath != null)
                {
                    parts = currPath.Split(delimiters, StringSplitOptions.None);
                    switch (parts[0])
                    {
                        case "SimulationModel":
                            textBoxSimModel.Text = parts[1].Trim();
                            break;
                        case "SchemaFile":
                            textBoxSchema.Text = parts[1].Trim();
                            break;
                        case "ScenarioFile":
                            textBoxScenario.Text = parts[1].Trim();
                            break;
                        case "HostName":
                            textBoxHostName.Text = parts[1].Trim();
                            break;
                        case "Port":
                            textBoxPortNumber.Text = parts[1].Trim();
                            break;
                        case "StrongPassword":
                            strongPasswordCheckbox.Checked = Boolean.Parse(parts[1].Trim());
                            break;
                        case "DebugFile":
                            //do nothing.
                            break;
                        case "LogPath":
                            textBoxLog.Text = parts[1].Trim();
                            string logType = parts[2];
                            //radioButtonNoLog.Checked = true;
                            if (logType.Trim() == "DETAILED")
                            {
                                Form1.LoggingType = Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.DETAILED;
                            }
                                //radioButtonDetailed.Checked = true;
                            if (logType.Trim() == "LIMITED")
                            {
                                Form1.LoggingType = Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.LIMITED;
                            }
                                //radioButtonLimited.Checked = true;
                            break;
                        case "UseObjectLog":
                            break;
                        default:
                            throw new Exception("Incorrect data in default settings file!");
                    }
                    currPath = sr.ReadLine();
                }
                sr.Close();
            }
            if (textBoxHostName.Text == string.Empty)
            {
                textBoxHostName.Text = System.Net.Dns.GetHostName();
            }
        }

        private bool SaveDefaultsToFile()
        {
            string filePath = System.Windows.Forms.Application.StartupPath + "\\default.txt";
            bool savedCorrectly = false;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (textBoxHostName.Text == string.Empty)
            {
                MessageBox.Show("Missing hostname.");
                return false;
            }
            if (textBoxPortNumber.Text == string.Empty)
            {
                MessageBox.Show("Missing port number.");
                return false;
            }
            if (!File.Exists(textBoxSimModel.Text))
            {
                MessageBox.Show("Invalid Simulation Model File.");
                return false;
            }
            if (!File.Exists(textBoxSchema.Text))
            {
                MessageBox.Show("Invalid Schema File.");
                return false;
            }
            if (!File.Exists(textBoxScenario.Text))
            {
                MessageBox.Show("Invalid Scenario File.");
                return false;
            }
            if (!Directory.Exists(textBoxLog.Text))
            {
                MessageBox.Show("Invalid Event Log Path.");
                return false;
            }


            StreamWriter sw = new StreamWriter(filePath);

            if (textBoxHostName.Text != string.Empty)
            {
                sw.WriteLine(string.Format("HostName; {0}", textBoxHostName.Text));
                Form1.Hostname = textBoxHostName.Text;
            }
            if (textBoxPortNumber.Text != string.Empty)
            {
                sw.WriteLine(string.Format("Port; {0}", textBoxPortNumber.Text));
                Form1.PortNumber = Convert.ToInt32(textBoxPortNumber.Text);
            }
            if (textBoxSimModel.Text != string.Empty)
            {
                sw.WriteLine(string.Format("SimulationModel; {0}", textBoxSimModel.Text));
                Form1.SimulationModelFile = textBoxSimModel.Text;
            }
            if (textBoxSchema.Text != string.Empty)
            {
                sw.WriteLine(string.Format("SchemaFile; {0}", textBoxSchema.Text));
                Form1.SchemaFile = textBoxSchema.Text;
            }
            if (textBoxScenario.Text != string.Empty)
            {
                sw.WriteLine(string.Format("ScenarioFile; {0}", textBoxScenario.Text));
                Form1.ScenarioFile = textBoxScenario.Text;
            }
            string logType = "NOLOG";


            if (textBoxLog.Text != string.Empty)
            {
                //if (radioButtonDetailed.Checked)
                //    logType = "DETAILED";
                //if (radioButtonLimited.Checked)
                //    logType = "LIMITED";
                if (Form1.LoggingType == Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.DETAILED)
                {
                    logType = "DETAILED";
                }
                if (Form1.LoggingType == Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.LIMITED)
                {
                    logType = "LIMITED";
                }

                sw.WriteLine(string.Format("LogPath; {0}; {1}", textBoxLog.Text, logType));
                //Form1.LogPath = textBoxLog.Text;
                //if (logType == "DETAILED")
                //{
                //    Form1.LoggingType = Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.DETAILED;
                //}
                //else
                //{
                //    Form1.LoggingType = Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.LIMITED;
                //}
            }

            sw.WriteLine(string.Format("StrongPassword; {0}", strongPasswordCheckbox.Checked));
            Form1.StrongPasswords = strongPasswordCheckbox.Checked;

            sw.WriteLine(string.Format("UseObjectLog; {0}", Form1.UseObjectLog));

            sw.Close();
            savedCorrectly = true;

            return savedCorrectly;
        }

        private void button2_Click(object sender, EventArgs e)
        {//clear boxes
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = string.Empty;
                }
                if (c is GroupBox)
                {
                    foreach (Control cc in ((GroupBox)c).Controls)
                    {
                        if (cc is TextBox)
                        {
                            ((TextBox)cc).Text = string.Empty;
                        }
                    }
                }
            }
            //radioButtonLimited.Checked = true;
            textBoxHostName.Text = System.Net.Dns.GetHostName();
        }

        private void button3_Click(object sender, EventArgs e)
        {//reset boxes, close form
            LoadPreviousFileInfo();

            this.Close();
        }


        private string RemoveFileNameFromPath(string path)
        {
            if (path == null)
                return string.Empty;
            if (path == string.Empty)
                return path;
            if (!path.Contains("\\"))
                return path;

            string newString;
            int fileNameIndex = path.LastIndexOf("\\");
            newString = path.Remove(fileNameIndex);

            return newString;
        }
    }
}