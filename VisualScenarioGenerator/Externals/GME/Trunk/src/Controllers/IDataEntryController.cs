/*
 * Interface        : IDataEntryController
 * File             : IDataEntryController.cs
 * Description      : 
 * Interface for DataEntryController class.
 * It inherits from IController interface.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using GME.Views.View_Components;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace GME.Controllers
{
    public interface IDataEntryController : IController
    {
        XmlNodeList GetSubComponentsOfType(String componentType);
        IXPathNavigable GetComponentOfType(String componentType);
        IXPathNavigable GetConfiguration();
    } //IDataEntryController
} // Controllers namespace
