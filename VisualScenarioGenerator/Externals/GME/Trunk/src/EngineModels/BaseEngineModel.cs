using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using AME.Model.Configuration;

namespace AME.EngineModels
{
    public class BaseEngineModel : IEngineModel
    {
        private String m_name;  
        private IXPathNavigable m_sourceXml;
        private IXPathNavigable m_logXml = new XmlDocument();
        private IXPathNavigable m_rawXml;
        private IXPathNavigable m_measureInputXml;
        private Dictionary<String, IXPathNavigable> m_visualizationXmls = new Dictionary<String, IXPathNavigable>();
        private Configuration m_modelConfiguration;

        public BaseEngineModel()
        {
        }

        public String Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public IXPathNavigable SourceXml
        {
            get
            {
                return m_sourceXml;
            }
            set
            {
                m_sourceXml = value;
            }
        }

        public IXPathNavigable LogXml
        {
            get
            {
                return m_logXml;
            }
            set
            {
                m_logXml = value;
            }
        }

        public IXPathNavigable RawXml
        {
            get
            {
                return m_rawXml;
            }
            set
            {
                m_rawXml = value;
            }
        }

        public IXPathNavigable MeasureInputXml
        {
            get
            {
                return m_measureInputXml;
            }
            set
            {
                m_measureInputXml = value;
            }
        }

        public Dictionary<String, IXPathNavigable> GetVisualizationXml()
        {
            return this.m_visualizationXmls;
        }

        public void AddVisualizationXml(String fileNameParameter, IXPathNavigable xml)
        {
            if (String.IsNullOrEmpty(fileNameParameter) || xml == null)
                return;
            m_visualizationXmls.Add(fileNameParameter, xml);
        }

        public Configuration ModelConfiguration
        {
            get
            {
                return m_modelConfiguration;
            }
            set
            {
                m_modelConfiguration = value;
            }
        }

        public virtual Boolean Start()
        {
            return true;
        }
    }
}
