using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AME.Controllers;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace VSG.Adapters
{
    public class ValidationHelper
    {
        protected IController validatingController;

        public ValidationHelper(IController pValidatingController)
        {
            validatingController = pValidatingController;
        }
    }
}
