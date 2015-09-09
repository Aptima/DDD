using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using AME.EngineModels;
using AME.Model.Configuration;

namespace AME.Adapters
{
    public interface IModelingAdapter
    {
        IXPathNavigable Component { get; set; }
        Boolean Process(IEngineModel engineModel);
        Configuration ModelConfiguration { get; set; }
    }
}
