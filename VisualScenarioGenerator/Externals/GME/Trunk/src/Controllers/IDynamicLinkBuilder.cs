using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace AME.Controllers
{
    public interface IDynamicLinkBuilder
    {
        void GetDynamicLink(XPathNavigator callingNavigator, IController c, String inLinkType, Dictionary<Int32, String> seenComponentIDs, out List<String> readLinkTypes, out List<String> writeLinkTypes);
    }
}
