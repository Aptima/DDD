/*
 * Class            : DataEntryController
 * File             : DataEntryController.cs
 * Description      : 
 * Inherits from Controller class, IDataEntryController interface.
 * Class for Data entry components who have a parent.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using GME.Model;
using GME.Views.View_Components;
using System.Xml;
using System.Xml.XPath;
using System.Data;

#endregion

namespace GME.Controllers
{
    public class DataEntryController : Controller, IDataEntryController
    {
        #region Constructors

        public DataEntryController(IModel model, String configType)
            : base(model, configType)
        {
        }

        #endregion

        #region Public Members

        public XmlNodeList GetSubComponentsOfType(String componentType)
        {
            XmlNodeList collection = this.m_model.GetSubComponents(this.Configuration, componentType);

            return collection;
        }

        public IXPathNavigable GetComponentOfType(String componentType)
        {
            IXPathNavigable iComponent = this.m_model.GetComponent(this.Configuration, componentType);

            return iComponent;
        }

        public IXPathNavigable GetConfiguration()
        {
            IXPathNavigable component = this.m_model.GetConfiguration(this.Configuration);
            if (component != null)
            {
                return component;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Members


        #endregion

    }//ProjectController class
}//namespace GME.Controllers 
