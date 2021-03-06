using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using AME.EngineModels;
using AME.Model.Configuration;

namespace AME.Adapters
{
    public class InternalToolAdapter : IModelingAdapter
    {
        IXPathNavigable m_component;
        private Configuration modelConfiguration;

        #region IModelingAdapter Members

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

        public Configuration ModelConfiguration
        {
            get
            {
                return modelConfiguration;
            }
            set
            {
                modelConfiguration = value;
            }
        }

        public Boolean Process(IEngineModel engineModel)
        {
            // Do all necessary prerequisite steps

            // Call Start on the model if model is not an external tool
            if (engineModel.Start())
                return true;
            else
                return false;
        }

        #endregion
    }
}
