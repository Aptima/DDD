using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace AME.Adapters
{
    public interface IImportAdapter
    {
        IXPathNavigable Process(String uriSource);
    }
}
