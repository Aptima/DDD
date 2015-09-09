using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Adapter
{
    public interface IInputAdapter
    {
        Boolean Process(IXPathNavigable component, IXPathNavigable source);
    }
}
