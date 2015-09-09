using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace AME.Views.Assessment
{
    public class DefaultViewHelper : IAssessmentViewHelper
    {
        private List<XmlDocument> dataRuns = new List<XmlDocument>();
        private List<string> xpaths;
        private Dictionary<String, String> doNotLoadTheseElements = new Dictionary<String, String>();

        public void SetDataRuns(List<XmlDocument> p_dataRuns)
        {
            dataRuns = p_dataRuns;
        }

        public void SetFilter(List<string> p_xpaths)
        {
            xpaths = p_xpaths;
        }

        private List<XmlNode> TrimSets(List<List<XmlNode>> sets)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                List<XmlNode> baseSet = sets[i];

                for (int j = 0; j < baseSet.Count; j++)
                {
                    XmlNode element = baseSet[j];

                    if (element != null)
                    {
                        for (int k = i + 1; k < sets.Count; k++)
                        {
                            List<XmlNode> compareSet = sets[k];

                            CheckContainsAndRemove(element, baseSet, compareSet);
                        }
                    }
                }
            }

            if (sets.Count > 0)
            {
                List<XmlNode> forReturn = new List<XmlNode>();
                List<XmlNode> buildFromList = sets[0];
                for (int i = 0; i < buildFromList.Count; i++)
                {
                    XmlNode element = buildFromList[i];
                    String name = element.Attributes["name"].Value;

                    if (!doNotLoadTheseElements.ContainsKey(name))
                    {
                        forReturn.Add(element);
                    }
                }

                return forReturn;
            }
            else
            {
                return null;
            }
        }

        public List<XmlNode> GetData()
        {
            doNotLoadTheseElements.Clear();
            List<List<XmlNode>> sets = new List<List<XmlNode>>();

            if (xpaths != null && xpaths.Count > 0)
            {
                foreach (XmlDocument dataNav in dataRuns)
                {

                    for (int ixp = 0; ixp < xpaths.Count; ixp++)
                    {
                        XmlNodeList set = dataNav.SelectNodes(xpaths[ixp]);

                        List<XmlNode> forSet = new List<XmlNode>();
                        for (int i = 0; i < set.Count; i++)
                        {
                            forSet.Add(set[i]);
                        }

                        sets.Add(forSet);
                    }

                    if (xpaths.Count > 1)
                    {
                        List<XmlNode> trimmed = TrimSets(sets);

                        for (int j = 0; j < sets.Count; j++)
                        {
                            sets[j] = trimmed;
                        }
                    }
                }
            }
            return TrimSets(sets);
        }

        public void CheckContainsAndRemove(XmlNode element, List<XmlNode> baseSet, List<XmlNode> compareSet)
        {
            String name1 = element.Attributes["name"].Value;

            bool found = false;

            for (int i = 0; i < compareSet.Count; i++)
            {
                XmlNode check = compareSet[i];

                if (check != null)
                {
                    String name2 = check.Attributes["name"].Value;

                    if (name1.Equals(name2))
                    {
                        found = true; 
                        break;
                    }
                }
            }

            if (!found)
            {
                String name = element.Attributes["name"].Value;
                if (!doNotLoadTheseElements.ContainsKey(name))
                {
                    doNotLoadTheseElements.Add(name, name);
                }
            }
        }
    }
}
