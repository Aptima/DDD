using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Adapter
{
    public interface IOutputAdapter
    {
        Boolean Process(IXPathNavigable component, IXPathNavigable simulation, ref String simRunFilename);
    }
}