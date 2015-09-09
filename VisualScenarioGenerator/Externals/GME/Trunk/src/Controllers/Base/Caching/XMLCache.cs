using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using AME.Controllers.Base.Data_Structures;
using AME.Model;
using System.Xml.Xsl;
using System.Data;

namespace AME.Controllers.Base.Caching
{
    public class XMLCache
    {
        // cache of rootID + LT to XmlDocument, per model
        private static Dictionary<AME.Model.Model, Dictionary<String, XmlDocument>> documentCache = new Dictionary<AME.Model.Model, Dictionary<string, XmlDocument>>();
      
        // component and link id xpaths
        private static Dictionary<AME.Model.Model, Dictionary<int, XPathExpression>> componentXPaths = new Dictionary<AME.Model.Model, Dictionary<int, XPathExpression>>();
        private static Dictionary<AME.Model.Model, Dictionary<int, XPathExpression>> linkXPaths = new Dictionary<AME.Model.Model, Dictionary<int, XPathExpression>>();
       
        // parameter xpaths, mapping is first id, then the combined parameter name
        private static Dictionary<AME.Model.Model, Dictionary<int, Dictionary<string, XPathExpression>>> componentParameterXPaths = new Dictionary<AME.Model.Model, Dictionary<int, Dictionary<string, XPathExpression>>>();
        private static Dictionary<AME.Model.Model, Dictionary<int, Dictionary<string, XPathExpression>>> linkParameterXPaths = new Dictionary<AME.Model.Model, Dictionary<int, Dictionary<string, XPathExpression>>>();
        private String delimeter = "#"; // internal delimeter for keys
        private XslCompiledTransform linkIDXsl;

        private Dictionary<String, List<String>> ltToKeyMap;

        public XMLCache(Controller c) 
        {
            linkIDXsl = new XslCompiledTransform();
            XmlReader xslreader = c.GetXSL("LinkID.xsl");
            linkIDXsl.Load(xslreader);
            xslreader.Close();
        }

        public String CreateCacheKey(int rootID, int displayID, String linkType)
        {
            return "" + rootID + delimeter + displayID + delimeter + linkType;
        }

        public String CreateCacheKey(int compID, String linkType)
        {
            return "" + compID + delimeter + linkType;
        }

        public void AddModel(AME.Model.Model model)
        {
            if (!documentCache.ContainsKey(model))
            {
                documentCache.Add(model, new Dictionary<string, XmlDocument>());
                componentXPaths.Add(model, new Dictionary<int,XPathExpression>());
                linkXPaths.Add(model, new Dictionary<int,XPathExpression>());
                componentParameterXPaths.Add(model, new Dictionary<int,Dictionary<string,XPathExpression>>());
                linkParameterXPaths.Add(model, new Dictionary<int,Dictionary<string,XPathExpression>>());
            }
        }

        public bool Contains(AME.Model.Model model, String key)
        {
            return documentCache[model].ContainsKey(key);
        }

        public XmlDocument GetDocumentFromCache(AME.Model.Model model, String key)
        {
            return documentCache[model][key]; 
        }

        public void AddCacheDocument(AME.Model.Model model, String key, XmlDocument doc)
        {
            documentCache[model][key] = doc;
        }

        public void ClearCache(AME.Model.Model model)
        {
            documentCache[model].Clear();
            componentParameterXPaths[model].Clear();
            componentXPaths[model].Clear();
            linkParameterXPaths[model].Clear();
            linkXPaths[model].Clear();
        }

        public void IndexLinkTypes(AME.Model.Model model)
        {
            Dictionary<String, XmlDocument> keysToDocs = documentCache[model];

            ltToKeyMap = new Dictionary<String, List<String>>();
            foreach (String key in keysToDocs.Keys)
            {
                String parsedLinkType = parseLinkType(key);
                if (!ltToKeyMap.ContainsKey(parsedLinkType))
                {
                    ltToKeyMap.Add(parsedLinkType, new List<String>());
                }

                ltToKeyMap[parsedLinkType].Add(key);
            }
        }

        public void ClearCache(AME.Model.Model model, String linkType)
        {
            if (ltToKeyMap.ContainsKey(linkType))
            {
                List<String> keys = ltToKeyMap[linkType];
                foreach (String key in keys)
                {
                    documentCache[model].Remove(key);
                }
                ltToKeyMap.Remove(linkType);
            }
        }

        private String parseLinkType(String key)
        {
            int delimIndex = key.LastIndexOf(delimeter) + 1; // pull out the lt
            String parseLinkType = key.Substring(delimIndex, key.Length - delimIndex);

            return parseLinkType;
        }

        private void addToOtherLinkTypes(String key, Controller c, AME.Model.Model model, XmlElement createForCache, int id, int linkID, string linkType, string lastValidateAddParentXPath)
        {
            XmlDocument doc;
            // find other documents with the same linktype = e.g. display ID based
            foreach (String documentKey in documentCache[model].Keys)
            {
                String parsedLinkType = parseLinkType(documentKey);
                if (parsedLinkType.Equals(linkType) && documentKey != key)
                {
                    doc = documentCache[model][documentKey];
                    try
                    {
                        createForCache = (XmlElement)doc.ImportNode(createForCache, true);
                        c.AddChildAtXPath(doc, lastValidateAddParentXPath, createForCache, false);
                    }
                    catch (Exception) { } // parent didn't exist - ok to skip
                }
            }

            if (!componentXPaths[model].ContainsKey(id))
            {
                componentXPaths[model].Add(id, XPathExpression.Compile(createComponentXPath(id)));
            }

            if (!linkXPaths[model].ContainsKey(linkID))
            {
                linkXPaths[model].Add(linkID, XPathExpression.Compile(createLinkXPath(linkID)));
            }
        }

        public void AddComponentToCache(Controller c, AME.Model.Model model, int topID, int parentID, int id, string type, string baseType, int linkID, string name, string linkType, string desc, Component.eComponentType eType, String lastValidateAddParentXPath, List<ComponentFunction> lastSchemaValuesValidateAdd)
        {
            String key = CreateCacheKey(topID, topID, linkType);
            if (!this.Contains(model, key))
            {
                ComponentOptions full = new ComponentOptions();
                full.CompParams = true;
                full.ClassInstanceInfo = true;
                full.SubclassInstanceInfo = true;
                full.LinkParams = true;
                XmlDocument doc2 = c._GetComponentsXmlDoc(topID, topID, linkType, full);
                AddCacheDocument(model, key, doc2);
                // This occurs at the -end- of a connect that would normally cause a cache add.
                // However, we haven't seen the document before, and the connect has already gone through to the DB.
                // This means the document we just retrieved above to initially populate the cache (c_.GetCompnentsXmlDoc)
                // already contains the item we're looking to add with this call.  So we should actually just return.
                // If we continue as below we will see doubles for the first items added under a linktype.
                // Because the cache is similarly populated (outside of this call) when a linktype is fetched, this side effect usually occurs with programmatic creation
                // where no data has been fetched / used yet, but links are being created and items are being added to the cache.

                // grab the element we're interested in (including children)
                XmlElement alreadyPresent = (XmlElement)doc2.SelectSingleNode(lastValidateAddParentXPath + createComponentXPath(id).Substring(1)); // remove the first slash in the //Component

                // bug fix - add to other link types as well
                addToOtherLinkTypes(key, c, model, alreadyPresent, id, linkID, linkType, lastValidateAddParentXPath);

                return;
            }
            XmlDocument doc = documentCache[model][key];
            XmlElement createForCache;
            DataTable childCheck = model.GetChildComponentLinks(id, linkType);
            if (childCheck.Rows.Count > 0)
            {
                ComponentOptions full = new ComponentOptions();
                full.CompParams = true;
                full.ClassInstanceInfo = true;
                full.SubclassInstanceInfo = true;
                full.LinkParams = true;
                XmlDocument thisCompDoc = (XmlDocument)c._GetComponentsXmlDoc(topID, id, linkType, full);
                XmlNode findChild = thisCompDoc.SelectSingleNode("/Components/Component");
                // set the link ID - the get call doesn't have enough information to do this for the top component
                int lID = c.GetLinkID(parentID, id, linkType);
                XmlAttribute attrLinkID = thisCompDoc.CreateAttribute(XmlSchemaConstants.Display.Component.LinkID);
                attrLinkID.Value = ""+lID;
                findChild.Attributes.Append(attrLinkID);

                // link parameters?
                IXPathNavigable linkParameters = c.GetParametersForLink(lID);

                if (linkParameters != null)
                {
                    XmlNode insert = ((XmlNode)linkParameters).SelectSingleNode(XmlSchemaConstants.Display.sLinkParameters);
                    insert = thisCompDoc.ImportNode(insert, true);
                    findChild.AppendChild(insert);
                }
   
                findChild = doc.ImportNode(findChild, true);
                createForCache = (XmlElement)findChild;
            }
            else
            {
                createForCache = CreateCacheElement(c, doc, id, linkID, type, baseType, name, desc, eType, lastSchemaValuesValidateAdd);
            }
 
            c.AddChildAtXPath(doc, lastValidateAddParentXPath, createForCache, false);
            
            // find other documents with the same linktype = e.g. display ID based
            addToOtherLinkTypes(key, c, model, createForCache, id, linkID, linkType, lastValidateAddParentXPath);
        }

        private XPathExpression getComponentXPath(AME.Model.Model model, int compID)
        {
            if (!componentXPaths[model].ContainsKey(compID))
            {
                componentXPaths[model].Add(compID, XPathExpression.Compile(createComponentXPath(compID)));
            }
            return componentXPaths[model][compID];
        }

        private XPathExpression getParameterXPath(AME.Model.Model model, int id, string category, string name, string childField, eParamParentType parent)
        {
            bool component = true;
            String key = category + name + childField;
            Dictionary<AME.Model.Model, Dictionary<int, Dictionary<string, XPathExpression>>> lookup;
           
            if (parent.ToString() == eParamParentType.Component.ToString())
            {
                lookup = componentParameterXPaths;
            }
            else
            {
                lookup = linkParameterXPaths;
                component = false;
            }

            if (!lookup[model].ContainsKey(id))
            {
                lookup[model][id] = new Dictionary<string, XPathExpression>();
            }

            if (!lookup[model][id].ContainsKey(key))
            {
                lookup[model][id].Add(key, XPathExpression.Compile(createParameterXPath(id, category, name, childField, component)));
            }
            return lookup[model][id][key];
        }

        private XPathExpression getLinkXPath(AME.Model.Model model, int linkID)
        {
            if (!linkXPaths[model].ContainsKey(linkID))
            {
                linkXPaths[model].Add(linkID, XPathExpression.Compile(createLinkXPath(linkID)));
            }
            return linkXPaths[model][linkID];
        }

        private String createComponentXPath(int id)
        {
            return "//Component[@ID='" + id + "']";
        }

        private String createLinkXPath(int id)
        {
            return "//Component[@LinkID='" + id + "']";
        }

        private String createParameterXPath(int id, String category, String name, String childField, bool component)
        {
            String common;
            if (childField.Length == 0)
            {
                common = String.Format("Parameter[@type='{0}']/Parameter[@displayedName='{1}'][@category='{2}']", ConfigFileConstants.complexType, name, category);
            }
            else
            {
                common = String.Format("Parameter[@type='{0}']/Parameter[@displayedName='{1}'][@category='{2}']/Parameters/Parameter[@name='{3}']", ConfigFileConstants.complexType, name, category, childField);
            }
            if (component)
            {
                return createComponentXPath(id) + "/ComponentParameters/" + common;
            }
            else
            {
                return createLinkXPath(id) + "/LinkParameters/" + common;
            }
        }

        public void UpdateParameters(AME.Model.Model model, int parentID, string category, string name, string childField, string paramValue, eParamParentType paramParType)
        {
            XPathExpression exp = getParameterXPath(model, parentID, category, name, childField, paramParType);

            foreach (XmlDocument doc in documentCache[model].Values)
            {
                XPathNodeIterator nodes = doc.CreateNavigator().Select(exp);
                foreach (XPathNavigator node in nodes)
                {
                    node.MoveToAttribute(ConfigFileConstants.Value, "");
                    node.SetValue(paramValue);
                }
            }
        }

        public void UpdateParameters(AME.Model.Model model, int parentID, string category, string name, string childField, List<XmlElement> newChildren, eParamParentType paramParType)
        {
            XPathExpression exp = getParameterXPath(model, parentID, category, name, childField, paramParType);

            foreach (XmlDocument doc in documentCache[model].Values)
            {
                XPathNodeIterator nodes = doc.CreateNavigator().Select(exp);
                foreach (XPathNavigator node in nodes)
                {
                    XPathNodeIterator oldChildren = node.SelectChildren(XPathNodeType.Element);
                    for (int i = oldChildren.Count-1; i >= 0; i--)
                    {
                        oldChildren.MoveNext();
                        oldChildren.Current.DeleteSelf();
                    }

                    foreach (XmlNode newChild in newChildren)
                    {
                        node.AppendChild(newChild.CreateNavigator());
                    }
                }
            }
        }

        public bool DeleteComponent(AME.Model.Model model, int id)
        {
            String path = createComponentXPath(id);
            foreach (XmlDocument doc in documentCache[model].Values)
            {
                XmlNodeList nodes = doc.SelectNodes(path);
                for (int i = (nodes.Count - 1); i >= 0; i--)
                {
                    nodes[i].ParentNode.RemoveChild(nodes[i]);
                }
            }
            componentXPaths[model].Remove(id);
            componentParameterXPaths[model].Remove(id);
            return true;
        }

        public bool DeleteLink(AME.Model.Model model, int id, String lt)
        {
            String path = createLinkXPath(id);
            XmlDocument doc;
            String parsedLT;
            foreach (String key in documentCache[model].Keys)
            {
                parsedLT = this.parseLinkType(key);
                if (parsedLT.Equals(lt))
                {
                    doc = documentCache[model][key];
                    XmlNodeList nodes = doc.SelectNodes(path);
                    for (int i = (nodes.Count - 1); i >= 0; i--)
                    {
                        nodes[i].ParentNode.RemoveChild(nodes[i]);
                    }
                }
            }
            linkXPaths[model].Remove(id);
            linkParameterXPaths[model].Remove(id);
            return true;
        }

        public void UpdateLinkIDs(AME.Model.Model model, Dictionary<int, int> changedLinkIDs)
        {
            Dictionary<string, XmlDocument> toUpdate = new Dictionary<string, XmlDocument>();
            foreach (String key in documentCache[model].Keys)
            {
                XmlDocument doc = documentCache[model][key];
                bool docChanged = false;

                foreach (KeyValuePair<int, int> pair in changedLinkIDs)
                {
                    XPathExpression exp = getLinkXPath(model, pair.Key);
                    XPathNodeIterator nodes = doc.CreateNavigator().Select(exp);
                    foreach (XPathNavigator node in nodes)
                    {
                        if (!docChanged)
                        {
                            docChanged = true;
                        }
      
                        node.MoveToAttribute(XmlSchemaConstants.Display.Component.LinkID, "");
                        node.SetValue(pair.Value.ToString()); // update linkID
                    }
                }
                if (docChanged)
                {
                    // apply a linkID xsl to sort on the new linkid values
                    XmlDocument newDocument = new XmlDocument();
                    using (XmlWriter writer = newDocument.CreateNavigator().AppendChild())
                    {
                        linkIDXsl.Transform(doc, (XsltArgumentList)null, writer);
                    }
                    newDocument.CreateNavigator();
                    
                    // save the key and new document to update the enumeration later
                    toUpdate.Add(key, newDocument);
                }
            }

            // update xpaths
            foreach (KeyValuePair<int, int> pair in changedLinkIDs)
            {
                linkXPaths[model].Remove(pair.Key);
                getLinkXPath(model, pair.Value); // will add
                linkParameterXPaths[model].Remove(pair.Key);
            }
            
            // update cache with saved keys / documents
            foreach (String newKey in toUpdate.Keys)
            {
                XmlDocument newDocument = toUpdate[newKey];
                documentCache[model][newKey] = newDocument;
            }
        }

        public bool UpdateComponentDescription(AME.Model.Model model, int compID, string value)
        {
            XPathExpression exp = getComponentXPath(model, compID);
            foreach (XmlDocument doc in documentCache[model].Values)
            {
                XPathNodeIterator nodes = doc.CreateNavigator().Select(exp);
                foreach (XPathNavigator node in nodes)
                {
                    node.MoveToAttribute(XmlSchemaConstants.Display.Component.Description, "");
                    node.SetValue(value);
                }
            }
            return true;
        }

        public void UpdateComponentName(AME.Model.Model model, int compID, string newName)
        {
            XPathExpression exp = getComponentXPath(model, compID);
            foreach (XmlDocument doc in documentCache[model].Values)
            {
                XPathNodeIterator nodes = doc.CreateNavigator().Select(exp);
                foreach (XPathNavigator node in nodes)
                {
                    node.MoveToAttribute(XmlSchemaConstants.Display.Component.Name, "");
                    node.SetValue(newName);
                }
            }
        }

        // used by cache 
        protected XmlElement CreateCacheElement(Controller c, XmlDocument doc, int id, int linkID, String type, String baseType, String name, String desc, Component.eComponentType eType, List<ComponentFunction> lastSchemaValuesValidateAdd)
        {
            // shared with controller_xml
            XmlElement newCacheElement = c.CommonElementCreate(doc, id, linkID, baseType, type, name, desc, eType);

            // parameters take an id instead of a component_row - a little different
            IXPathNavigable compClone = c.GetParametersForComponent(id);
            if (compClone != null)
            {
                XmlNode insert = ((XmlNode)compClone).SelectSingleNode(XmlSchemaConstants.Display.sComponentParameters);
                insert = doc.ImportNode(insert, true);
                newCacheElement.AppendChild(insert);
            }

            IXPathNavigable linkClone = c.GetParametersForLink(linkID);

            if (linkClone != null)
            {
                XmlNode insert = ((XmlNode)linkClone).SelectSingleNode(XmlSchemaConstants.Display.sLinkParameters);
                insert = doc.ImportNode(insert, true);
                newCacheElement.AppendChild(insert);
            }

            // Functions also slightly different, come from the last schema values from the ValidateAdd call
            if (lastSchemaValuesValidateAdd != null && lastSchemaValuesValidateAdd.Count > 0)
            {
                //creating "Functions" element
                XmlElement funcsElement = doc.CreateElement(XmlSchemaConstants.Display.sFunction + "s");    //Functions
                foreach (ComponentFunction func in lastSchemaValuesValidateAdd)
                {
                    //adding "Function" element to "Functions" element
                    XmlElement xmlElementFunc = c.CreateXmlFunction(doc, func.Name, func.Action, "" + func.Visible);
                    funcsElement.AppendChild(xmlElementFunc);
                }
                newCacheElement.AppendChild(funcsElement);
            }//if functions
            return newCacheElement;
        }
    }
}
