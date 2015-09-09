using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SimModelDocGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void simModelBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML File(*.xml)|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                simModelTextBox.Text = ofd.FileName;
            }
        }

        private void outputBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HTML File(*.htm)|*.htm";
            ofd.CheckFileExists = false;
            ofd.CheckPathExists = true;
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ouputTextBox.Text = ofd.FileName;
            }
        }

        private void generateDocButton_Click(object sender, EventArgs e)
        {
            SimulationModelReader smr = new SimulationModelReader();
            SimulationModelInfo simModel;

            try
            {
                simModel = smr.readModel(simModelTextBox.Text);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString(),"Error reading simulation model!");
                return;
            }

            StreamWriter outFile;

            try
            {
                outFile = new StreamWriter(ouputTextBox.Text);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString(), "Error opening output file!");
                return;
            }

            string headerHtml = "<html><head><title>DDD Event Documentation</title></head><body><p><h2>DDD Event Documentation</h2></p>";
            string footerHtml = "</body></html>";

            outFile.WriteLine(headerHtml);


            outFile.WriteLine(String.Format("<a name=\"TOC\"><h2>Table of Contents</h2></a>"));
            outFile.WriteLine("<ul>");
            List<string> eventKeys = new List<string>(simModel.eventModel.events.Keys);
            eventKeys.Sort();
            foreach (string s in eventKeys)
            {
                outFile.WriteLine("<li><a href=\"#{0}\">{0}</a></li>",s);
            }
            outFile.WriteLine("</ul>");

            foreach (string key in eventKeys)
            {
                WriteEventHTML(ref outFile, simModel.eventModel.events[key]);
            }

            outFile.WriteLine(footerHtml);
            outFile.Flush();
            outFile.Close();
        }

        void WriteParameterHTML(ref StreamWriter outFile, ParameterInfo paramInfo)
        {
            outFile.WriteLine(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", 
                                            paramInfo.name, paramInfo.dataType,paramInfo.description));
        }

        void WriteEventHTML(ref StreamWriter outFile, EventInfo eventInfo)
        {
            //outFile.WriteLine(String.Format("<a name=\"{0}\"><h4>{0}</h4></a>", eventInfo.name));
            //outFile.WriteLine(String.Format("<p><h4>{0}</h4></p>",eventInfo.name));
            outFile.WriteLine(String.Format("<table border=\"1\">"));

            outFile.WriteLine("<tr>");
            outFile.WriteLine(String.Format("<td><b>Event Name</b></td>"));
            outFile.WriteLine(String.Format("<td><a name=\"{0}\"><b>{0}</b></a></td>", eventInfo.name));
            outFile.WriteLine("</tr>");

            outFile.WriteLine("<tr>");
            outFile.WriteLine(String.Format("<td>Description</td>"));
            outFile.WriteLine(String.Format("<td>{0}</td>", eventInfo.description));
            outFile.WriteLine("</tr>");

            outFile.WriteLine("<tr>");
            outFile.WriteLine(String.Format("<td>Written to DDD Log File</td>"));
            outFile.WriteLine(String.Format("<td>{0}</td>", eventInfo.shouldLog));
            outFile.WriteLine("</tr>");

            outFile.WriteLine("<tr>");
            outFile.WriteLine(String.Format("<td>Read for Replay</td>"));
            outFile.WriteLine(String.Format("<td>{0}</td>", eventInfo.shouldReplay));
            outFile.WriteLine("</tr>");

            outFile.WriteLine("<tr>");
            outFile.WriteLine(String.Format("<td>Parameters</td>"));
            outFile.WriteLine(String.Format("<td>"));
            outFile.WriteLine(String.Format("<table border=\"1\">"));
            outFile.WriteLine("<tr><th>Name</th><th>Data Type</th><th>Description</th></tr>");

            foreach (ParameterInfo pi in eventInfo.parameters.Values)
            {
                WriteParameterHTML(ref outFile, pi);
            }

            outFile.WriteLine(String.Format("</table>"));
            outFile.WriteLine(String.Format("</td>"));
            outFile.WriteLine("</tr>");

            outFile.WriteLine(String.Format("</table>"));
            outFile.WriteLine("<a href=\"#TOC\">Back to Table of Contents</a>");
            outFile.WriteLine(String.Format("<p></p>"));
        }
    }
}