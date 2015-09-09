using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using AME.Model.Configuration;

namespace AME.EngineModels
{
    public interface IEngineModel
    {
        String Name { get; set; }

        IXPathNavigable SourceXml { get; set; }
        IXPathNavigable LogXml { get; set; }
        IXPathNavigable RawXml { get; set; }
        IXPathNavigable MeasureInputXml { get; set; }

        // List of files created by the adapter that will require cleanup.
        //List<String> OutputFiles { get; set; }

        Dictionary<String, IXPathNavigable> GetVisualizationXml();
        void AddVisualizationXml(String fileNameParameter, IXPathNavigable xml);

        Configuration ModelConfiguration { get; set; }

        Boolean Start(); // Start the simulation model
    }
}
