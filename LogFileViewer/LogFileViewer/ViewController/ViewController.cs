using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using System.Windows.Forms;

namespace LogFileViewer.ViewController
{
    public class ReportData
    {
        public ReportData(String baseName,Boolean hasCSV, Boolean hasCSVHeader)
        {
            m_baseName = baseName;
            m_hasCSV = hasCSV;
            m_hasCSVHeader = hasCSVHeader;
        }
        private String m_baseName;
        private Boolean m_hasCSV;
        private Boolean m_hasCSVHeader;

        public String BaseName
        {
            get { return m_baseName; }
        }
        public String ReportName
        {
            get { return m_baseName + "RPT.xslt"; }
        }
        public String CSVName
        {
            get { return m_baseName + "CSV.xslt"; }
        }
        public String CSVHeaderName
        {
            get { return m_baseName + "CSV_Header.xslt"; }
        }
        public Boolean HasCSV
        {
            get { return m_hasCSV; }
        }
        public Boolean HasCSVHeader
        {
            get { return m_hasCSVHeader; }
        }

    }



    public class ViewController: IDisposable
    {
        public Dictionary<String, ReportData> reportTransforms;
        private string  xmlsource_file;
        public string CurrentFile
        {
            get
            {
                return transforms["Summary"];
            }
        }
        Dictionary<string, string> transforms = new Dictionary<string, string>();


        public bool SaveWithColumnHeaders = true;

        public ViewController()
        {            
            xmlsource_file = Path.GetTempFileName();
            reportTransforms = new Dictionary<string, ReportData>();
        }

        public void LoadReportTransforms()
        {
            reportTransforms["Summary"] = new ReportData("Summary", false, false);
            DirectoryInfo di= new DirectoryInfo("Xsl");
            FileInfo[] reportFiles = di.GetFiles("*RPT.xslt");
            FileInfo[] csvFiles = di.GetFiles("*CSV.xslt");
            FileInfo[] csvHeaderFiles = di.GetFiles("*CSV_Header.xslt");

            foreach (FileInfo fi in reportFiles)
            {
                String baseName = fi.Name.Replace("RPT.xslt", "");
                String csvName = baseName + "CSV.xslt";
                String csvHeaderName = baseName + "CSV_Header.xslt";
                Boolean hasCSV = false;
                Boolean hasCSVHeader = false;
                foreach (FileInfo fi2 in csvFiles)
                {
                    if (fi2.Name == csvName)
                    {
                        hasCSV = true;
                    }
                }
                foreach (FileInfo fi2 in csvHeaderFiles)
                {
                    if (fi2.Name == csvHeaderName)
                    {
                        hasCSVHeader = true;
                    }
                }
                reportTransforms[baseName] = new ReportData(baseName, hasCSV, hasCSVHeader);
            }
        }

        public void DeleteTempFile()
        {
            File.Delete(xmlsource_file);
        }
        
        public string LoadReplayLog(string sourcefile)
        {
            if (FormatLogFileToXmlFile(sourcefile))
            {

                XslCompiledTransform trans = new XslCompiledTransform();
                
                XmlReader stylesheet = XmlReader.Create(Application.StartupPath+"\\Xsl\\" + reportTransforms["Summary"].ReportName);
                if (transforms.ContainsKey("Summary"))
                {
                    if (File.Exists(transforms["Summary"]))
                    {
                        foreach (string filename in transforms.Values)
                        {
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }
                        }
                    }
                }
                
                transforms["Summary"] = Path.GetTempFileName();

                trans.Load(stylesheet);
                trans.Transform(xmlsource_file, transforms["Summary"]);

                return transforms["Summary"];
            }
            return string.Empty;
        }

        public string DisplayReport(String report)
        {

            XslCompiledTransform trans = new XslCompiledTransform();
            XmlReader stylesheet = XmlReader.Create(Application.StartupPath + "\\Xsl\\"+report+"RPT.xslt");

            if (transforms.ContainsKey(report))
            {
                if (File.Exists(transforms[report]))
                {
                    return transforms[report];
                }
            }
            transforms[report] = Path.ChangeExtension(Path.GetTempFileName(),"htm");

            trans.Load(stylesheet);
            trans.Transform(xmlsource_file, transforms[report]);

            return transforms[report];
        }

        public void SaveAsCSV(String choice, string outputfile)
        {
            string stylesheet_resource = FindCSVStylesheet(choice);
            if (stylesheet_resource != string.Empty)
            {
                XslCompiledTransform trans = new XslCompiledTransform();
                XmlReader stylesheet = XmlReader.Create(Application.StartupPath + "\\Xsl\\" + stylesheet_resource);
                trans.Load(stylesheet);
                trans.Transform(xmlsource_file, outputfile);
            }                    
        }

        private string FindCSVStylesheet(String choice)
        {
            if (reportTransforms[choice].HasCSVHeader && SaveWithColumnHeaders)
            {
                return reportTransforms[choice].CSVHeaderName;
            }
            else
            {
                return reportTransforms[choice].CSVName;
            }
        }

        private bool FormatLogFileToXmlFile(string filename)
        {
            StreamReader r = new StreamReader(filename);
            string data = r.ReadToEnd();
            r.Close();
            if (data.Contains("ExternalApp_SimStart"))
            {
                string xml_doc = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>" +
                    "<Scenario name=\"" + Path.GetFileNameWithoutExtension(filename) + "\">"
                    + data + @"</Scenario>";

                StreamWriter w = new StreamWriter(xmlsource_file);
                w.Write(xml_doc);
                w.Close();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            DeleteTempFile();
            foreach (string filename in transforms.Values)
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }

        #endregion
    }
}
