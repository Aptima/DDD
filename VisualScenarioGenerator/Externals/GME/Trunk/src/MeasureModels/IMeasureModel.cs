using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AME.MeasureModels
{
    public interface IMeasureModel : IMeasureInfo
    {
        IXPathNavigable MeasureInputXml { get; set; }
        IXPathNavigable MeasureOutputXml { get; }

        bool Start(); // Start the measure model
    }//IMeasureModel
}//namespace GME.MeasureModels
