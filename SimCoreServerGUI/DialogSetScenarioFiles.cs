using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class DialogSetScenarioFiles : Form
    {
        private static string OFDXMLFilter = "XML File(*.xml)|*.xml";
        private static string OFDXSDFilter = "Schema File(*.xsd)|*.xsd";
        private static OpenFileDialog ofd = new OpenFileDialog();

        public DialogSetScenarioFiles()
        {
            InitializeComponent();
            ofd.InitialDirectory = Form1.ApplicationPath;
            SetBoxesUp();
        }

        //private void buttonSchemaBrowse_Click(object sender, EventArgs e)
        //{
        //    ofd.Filter = OFDXSDFilter;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        textBoxSchemaFile.Text = ofd.FileName;
        //    }
        //}

        private void buttonScenarioBrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = OFDXMLFilter;
            if (File.Exists(textBoxScenario.Text))
            {
                ofd.InitialDirectory = textBoxScenario.Text.Remove(textBoxScenario.Text.LastIndexOf('\\'));
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

        private void buttonAcceptFiles_Click(object sender, EventArgs e)
        {
            if (//File.Exists(textBoxSchemaFile.Text) &&
                File.Exists(textBoxScenario.Text))
            {
               // Form1.SchemaFile = textBoxSchemaFile.Text;
                Form1.ScenarioFile = textBoxScenario.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("That file does not exist.");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SetBoxesUp();
            this.Close();
        }
        public void SetBoxesUp()
        {
            if (Form1.ScenarioFile != string.Empty)
                textBoxScenario.Text = Form1.ScenarioFile;
            //if (Form1.SchemaFile != string.Empty)
            //    textBoxSchemaFile.Text = Form1.SchemaFile;
        }
    }
}