using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AME.Views.Assessment
{
    public interface IAssessmentViewHelper
    {
        void SetDataRuns(List<XmlDocument> dataruns);
        void SetFilter(List<string> xpaths);
        List<XmlNode> GetData();
    }
}
