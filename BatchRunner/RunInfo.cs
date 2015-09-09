using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
namespace BatchRunner
{
    public class RunInfo : ListViewItem
    {
        String m_runName;
        public string RunName
        {
            get { return m_runName; }
            set { m_runName = value; }
        }
        String m_runScenarioPath;
        public string RunScenarioPath
        {
            get { return m_runScenarioPath; }
            set { m_runScenarioPath = value; }
        }
        String m_runLogDirectoryPath;
        public string RunLogDirectoryPath
        {
            get { return m_runLogDirectoryPath; }
            set { m_runLogDirectoryPath = value; }
        }

        String m_runDataTag;
        public string RunDataTag
        {
            get { return m_runDataTag; }
            set { m_runDataTag = value; }
        }
        int m_runDuration;
        public int RunDuration
        {
            get { return m_runDuration; }
            set { m_runDuration = value; }
        }
        String m_externalSetupCommand;
        public string ExternalSetupCommand
        {
            get { return m_externalSetupCommand; }
            set { m_externalSetupCommand = value; }
        }

        String m_externalSetupArguments;
        public string ExternalSetupArguments
        {
            get { return m_externalSetupArguments; }
            set { m_externalSetupArguments = value; }
        }

        String m_externalSetupWorkingDirectory;
        public string ExternalSetupWorkingDirectory
        {
            get { return m_externalSetupWorkingDirectory; }
            set { m_externalSetupWorkingDirectory = value; }
        }

        int m_externalSetupDelay;
        public int ExternalSetupDelay
        {
            get { return m_externalSetupDelay; }
            set { m_externalSetupDelay = value; }
        }

        String m_externalTeardownCommand;
        public string ExternalTeardownCommand
        {
            get { return m_externalTeardownCommand; }
            set { m_externalTeardownCommand = value; }
        }

        String m_externalTeardownArguments;
        public string ExternalTeardownArguments
        {
            get { return m_externalTeardownArguments; }
            set { m_externalTeardownArguments = value; }
        }

        String m_externalTeardownWorkingDirectory;
        public string ExternalTeardownWorkingDirectory
        {
            get { return m_externalTeardownWorkingDirectory; }
            set { m_externalTeardownWorkingDirectory = value; }
        }

        int m_externalTeardownDelay;
        public int ExternalTeardownDelay
        {
            get { return m_externalTeardownDelay; }
            set { m_externalTeardownDelay = value; }
        }

        public void UpdateListViewItem()
        {
            this.SubItems.Clear();
            this.Text = RunName;
            this.SubItems.Add(this.RunDuration.ToString());
            this.SubItems.Add(this.RunScenarioPath);
        }

        public static void SaveBatchFile(List<RunInfo> runs, String path)
        {
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("BatchRunner");
                

                foreach (RunInfo ri in runs)
                {
                    writer.WriteStartElement("RunInfo");

                    writer.WriteElementString("RunName", ri.RunName);
                    writer.WriteElementString("RunScenarioPath", ri.RunScenarioPath);
                    writer.WriteElementString("RunLogDirectoryPath", ri.RunLogDirectoryPath);
                    writer.WriteElementString("RunDataTag", ri.RunDataTag);
                    writer.WriteElementString("RunDuration", ri.RunDuration.ToString());
                    writer.WriteElementString("ExternalSetupCommand", ri.ExternalSetupCommand);
                    writer.WriteElementString("ExternalSetupArguments", ri.ExternalSetupArguments);
                    writer.WriteElementString("ExternalSetupWorkingDirectory", ri.ExternalSetupWorkingDirectory);
                    writer.WriteElementString("ExternalSetupDelay", ri.ExternalSetupDelay.ToString());
                    writer.WriteElementString("ExternalTeardownCommand", ri.ExternalTeardownCommand);
                    writer.WriteElementString("ExternalTeardownArguments", ri.ExternalTeardownArguments);
                    writer.WriteElementString("ExternalTeardownWorkingDirectory", ri.ExternalTeardownWorkingDirectory);
                    writer.WriteElementString("ExternalTeardownDelay", ri.ExternalTeardownDelay.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        public static List<RunInfo> LoadBatchFile(String path)
        {
            List<RunInfo> batch = new List<RunInfo>();

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                MessageBox.Show("The file specified doesn't exist!", "Error Loading Batch File");
                return null;
            }

            XPathNavigator nav;
            XPathDocument docNav;
            XPathNodeIterator nodeIter;

            docNav = new XPathDocument(path);
            nav = docNav.CreateNavigator();

            nodeIter = nav.Select("BatchRunner/RunInfo");
            while (nodeIter.MoveNext())
            {
                RunInfo run = new RunInfo();
                run.RunName = nodeIter.Current.SelectSingleNode("RunName").Value;
                run.RunScenarioPath = nodeIter.Current.SelectSingleNode("RunScenarioPath").Value;
                run.RunLogDirectoryPath = nodeIter.Current.SelectSingleNode("RunLogDirectoryPath").Value;
                run.RunDataTag = nodeIter.Current.SelectSingleNode("RunDataTag").Value;
                run.RunDuration = nodeIter.Current.SelectSingleNode("RunDuration").ValueAsInt;
                run.ExternalSetupCommand = nodeIter.Current.SelectSingleNode("ExternalSetupCommand").Value;
                run.ExternalSetupArguments = nodeIter.Current.SelectSingleNode("ExternalSetupArguments").Value;
                run.ExternalSetupWorkingDirectory = nodeIter.Current.SelectSingleNode("ExternalSetupWorkingDirectory").Value;
                run.ExternalSetupDelay = nodeIter.Current.SelectSingleNode("ExternalSetupDelay").ValueAsInt;
                run.ExternalTeardownCommand = nodeIter.Current.SelectSingleNode("ExternalTeardownCommand").Value;
                run.ExternalTeardownArguments = nodeIter.Current.SelectSingleNode("ExternalTeardownArguments").Value;
                run.ExternalTeardownWorkingDirectory = nodeIter.Current.SelectSingleNode("ExternalTeardownWorkingDirectory").Value;
                run.ExternalTeardownDelay = nodeIter.Current.SelectSingleNode("ExternalTeardownDelay").ValueAsInt;
                batch.Add(run);
            }
            return batch;
        }
    }
}
