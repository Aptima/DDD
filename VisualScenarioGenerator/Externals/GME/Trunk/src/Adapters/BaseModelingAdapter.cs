using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using AME.Adapters;
using AME.EngineModels;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using AME.Model.Configuration;

namespace AME.Adapters
{
    public class BaseModelingAdapter : IModelingAdapter
    {
        private IXPathNavigable m_component;
        private Configuration modelConfiguration;

        public IXPathNavigable Component
        {
            get
            {
                return m_component;
            }
            set
            {
                m_component = value;
            }
        }

        public Configuration ModelConfiguration { set { modelConfiguration = value; } get { return modelConfiguration; } }

        public virtual Boolean Process(IEngineModel engineModel)
        {
            return true;
        }
    }
}
