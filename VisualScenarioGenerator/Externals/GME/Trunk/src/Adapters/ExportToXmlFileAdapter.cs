using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AME.Adapters
{
    public class ExportToXmlFileAdapter : IExportDeviceAdapter<IXPathNavigable>
    {
        private ExportToXmlFileDeviceAdapterSettings settings;
        private XmlWriter writer;

        public ExportToXmlFileAdapter(ExportToXmlFileDeviceAdapterSettings settings)
        {
            this.settings = settings;
        }

        public ExportToXmlFileAdapter(String outputFilePath)
        {
            this.settings = new ExportToXmlFileDeviceAdapterSettings(outputFilePath);
        }

        #region IExportDeviceAdapter<IXPathNavigable> Members

        void IExportDeviceAdapter<IXPathNavigable>.InitializeDevice()
        {
            writer = XmlWriter.Create(this.settings.outputFilePath, this.settings.settings);
        }

        void IExportDeviceAdapter<IXPathNavigable>.Write(IXPathNavigable data)
        {
            ((XmlDocument)data).WriteTo(this.writer);
        }

        void IExportDeviceAdapter<IXPathNavigable>.FinalizeDevice()
        {
            this.writer.Close();
        }

        #endregion
    }

    public class ExportToXmlFileDeviceAdapterSettings : IExportDeviceSettings
    {
        public String outputFilePath;
        public XmlWriterSettings settings;
       

        public ExportToXmlFileDeviceAdapterSettings(String outputFilePath, Encoding encoding, bool indent)
        {
            this.settings = new XmlWriterSettings();
            this.outputFilePath = outputFilePath;
            this.settings.Encoding = encoding;
            this.settings.Indent = indent;
        }

        public ExportToXmlFileDeviceAdapterSettings(String outputFilePath)
        {
            this.settings = new XmlWriterSettings();
            this.outputFilePath = outputFilePath;
            this.settings.Encoding = Encoding.UTF8;
            this.settings.Indent = true;
        }
    }
}
