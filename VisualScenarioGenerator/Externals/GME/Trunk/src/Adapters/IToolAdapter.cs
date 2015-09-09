using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Adapter
{
    public interface IToolAdapter
    {
        Int32 RootId { get; }
        Boolean Process(IXPathNavigable component);
    }
}
