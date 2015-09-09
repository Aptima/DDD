/*
 * Interface        : IRootController
 * File             : IRootController.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Interface for RootController class.
 * It inherits from IController interface.
 * Contains the signature of the methods implemented in RootController class.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

#endregion  //Namespaces

namespace AME.Controllers
{
    public interface IRootController : IController
    {
        int CreateRootComponent(string name, string description);
        void UpdateRootName(int id, string value);
        IXPathNavigable GetRootComponents(ComponentOptions compOptions);
    }//IRootController
}//namespace AME.Controllers
