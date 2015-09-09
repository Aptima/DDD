using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using AME.Model;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Reflection;

namespace AME.Controllers
{
    public partial class Controller
    {
        private String noParent = "None", currentFileName;
        protected String Root = "Root";
        private Boolean firstProcessed = false, needToSave = false;
        private XmlElement linkRoot, functionsRoot, linkElement, functionsElement, functionsLT;

        private void ValidateConnect(String linkType, String fromType, String toType) {
            try {
                // process linktype, if necessary
                if (this.IsLinkTypeDynamic(linkType)) {
                    linkType = this.GetBaseLinkType(linkType);
                }

                //adjust for base type
                toType = this._GetUltimateBaseComponentType(toType);
                fromType = this._GetUltimateBaseComponentType(fromType);

                // check in configuration file for a matching connect entry
                IXPathNavigable checkLink = this.m_model.GetLink(linkType, fromType, toType);

                if (checkLink == null) {
                    throw new Exception("Connection between types: " + fromType + " and " + toType + " for linktype " + linkType + " is undefined.  Check configuration file");
                }
            }
            catch (Exception) {
                throw;
            }
        }

        public void ValidateAllSchemaLinks()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReader reader = GetConfigurationReader();
            xmlDoc.Load(reader);
            XPathNavigator docNav = xmlDoc.CreateNavigator();
            reader.Close();

            XPathNodeIterator links = docNav.Select("GME/Global/Links/Link/Connect");

            foreach (XPathNavigator link in links)
            {
                String from = link.GetAttribute("from", "");
                String to = link.GetAttribute("to", "");

                XPathNodeIterator checkFrom = docNav.Select("GME/Global/Components/Component[@name='" + from + "']");
                if (checkFrom.Count != 1)
                {
                    Console.WriteLine("Warning:  Found connect from entry for: " + from + " but no matching global component");
                }

                XPathNodeIterator checkTo = docNav.Select("GME/Global/Components/Component[@name='" + to + "']");
                if (checkTo.Count != 1)
                {
                    Console.WriteLine("Warning:  Found connect to entry for: " + to + " but no matching global component");
                }
            }

            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                String[] names = entryAssembly.GetManifestResourceNames();
       
                XmlDocument linksOutput = new XmlDocument();
                linkRoot = linksOutput.CreateElement(Root);
                linksOutput.AppendChild(linkRoot);

                XmlDocument functionsOutput = new XmlDocument();
                functionsRoot = functionsOutput.CreateElement(Root);
                functionsOutput.AppendChild(functionsRoot);

                foreach (String name in names)
                {
                    if (name.Contains(".xsd"))
                    {
                        Console.WriteLine(name);

                        reader = GetXSD(name);
                        XmlSchema schema = XmlSchema.Read(reader, null);
                        reader.Close();

                        XmlSchemaSet set = new XmlSchemaSet();
                        set.Add(schema);
                        set.Compile();

                        currentFileName = name;

                        foreach (XmlSchema compiledSchema in set.Schemas())
                        {
                            firstProcessed = false;
                            needToSave = false;
                            WalkSchema(compiledSchema);

                            if (firstProcessed && needToSave)
                            {
                                XmlWriterSettings settings = new XmlWriterSettings();
                                //settings.Indent = true;
                                Stream writeXSD = GetXSDStream(name);
                                StreamWriter sWriter = new StreamWriter(writeXSD);
                                XmlWriter write = XmlWriter.Create(sWriter, settings);
                                schema.Write(write);
                                write.Close();
                                sWriter.Close();
                                writeXSD.Close();
                            }
                        }

                        Console.WriteLine("");
                    }
                }

                linksOutput.Save("UnprocessedSchemaLinks.xml");

                functionsOutput.Save("Functions.xml");

                PostProcess(linksOutput);

                linksOutput.Save("ProcessedLinks.xml");
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read xsd / config directory", e.Message);
            }
        }

        // adapted from 
        // http://blogs.msdn.com/stan_kitsis/archive/2005/08/06/448572.aspx
        private void WalkSchema(XmlSchema schema)
        {
            foreach (XmlSchemaType type in schema.SchemaTypes.Values)
            {
                if (type is XmlSchemaComplexType)
                {
                    XmlSchemaComplexType ct = (XmlSchemaComplexType)type;

                    WalkTheParticle(ct.ContentTypeParticle, noParent);
                }
            }

            foreach (XmlSchemaElement el in schema.Elements.Values)
            {
                WalkTheParticle(el, noParent);
            }

            foreach (XmlSchemaObject xsa in schema.Items)
            {
                if (xsa is XmlSchemaGroup)
                {
                    XmlSchemaGroup xsg = (XmlSchemaGroup)xsa;

                    WalkTheParticle(xsg.Particle, noParent);
                }
            }
        }

        private void WalkTheParticle(XmlSchemaParticle particle, String parentName)
        {
            if (particle is XmlSchemaElement)
            {
                XmlSchemaElement elem = particle as XmlSchemaElement;

                // do processing here
                String elementName = "";
                if (elem.Name == null)
                {
                    elementName = elem.QualifiedName.Name;
                }
                else
                {
                    elementName = elem.Name;
                }

                XmlDocument owner;
                XmlSchemaComplexType ct;

                if (firstProcessed == false)
                {
                    if (elem.SchemaType is XmlSchemaComplexType)
                    {
                        ct = (XmlSchemaComplexType)elem.SchemaType;

                        if (ct.Attributes.Count > 0)
                        {
                            XmlSchemaAttribute firstAttr = (XmlSchemaAttribute)ct.Attributes[0];
                            if (firstAttr.Name.Equals("LinkType"))
                            {
                                firstProcessed = true;

                                // rename element to 'Root'
                                if (elem.Name != Root)
                                {
                                    elem.Name = Root;
                                    needToSave = true;
                                }

                                owner = linkRoot.OwnerDocument;

                                linkElement = owner.CreateElement("Link");

                                XmlAttribute type = owner.CreateAttribute(ConfigFileConstants.Type);
                                type.Value = firstAttr.FixedValue;

                                XmlAttribute schemaFilename = owner.CreateAttribute(ConfigFileConstants.schemaFilename);
                                schemaFilename.Value = currentFileName;

                                linkElement.SetAttributeNode(type);
                                linkElement.SetAttributeNode(schemaFilename);

                                linkRoot.AppendChild(linkElement);

                                BuildConnectElement(parentName, elementName);

                                owner = functionsRoot.OwnerDocument;
                                functionsLT = owner.CreateElement("LinkType");
                                functionsLT.InnerText = firstAttr.FixedValue;
                                functionsRoot.AppendChild(functionsLT);

                            }
                        } // attr
                    } // ct
                } // fp
                else
                {
                    BuildConnectElement(parentName, elementName);
                }

                if (elem.SchemaType is XmlSchemaComplexType && functionsLT != null)
                {
                    ct = (XmlSchemaComplexType)elem.SchemaType;

                    owner = functionsRoot.OwnerDocument;
                    functionsElement = owner.CreateElement(elementName);
                    functionsLT.AppendChild(functionsElement);
                    XmlElement functions = owner.CreateElement("Functions");

                    foreach (XmlSchemaAttribute attribute in ct.Attributes)
                    {
                        if (attribute.Name.Equals("VisibleFunctions") || attribute.Name.Equals("InvisibleFunctions"))
                        {
                            if (attribute.FixedValue != null)
                            {
                                String[] splitSemi = attribute.FixedValue.Split(new char[] { ';' });

                                StringBuilder forFunctions = new StringBuilder(splitSemi.Length);
                                for (int i = 0; i < splitSemi.Length; i++)
                                {
                                    String test = splitSemi[i];
                                    test = test.Trim();

                                    XmlElement function = owner.CreateElement("Function");

                                    String[] splitEquals = test.Split(new char[] { '=' });

                                    XmlAttribute fName = owner.CreateAttribute("Name");
                                    fName.Value = splitEquals[0].Trim();
                                    XmlAttribute fAction = owner.CreateAttribute("Action");
                                    fAction.Value = splitEquals[1].Trim();
                                    XmlAttribute fVisible = owner.CreateAttribute("Visible");
                                    if (attribute.Name.Equals("VisibleFunctions"))
                                    {
                                        fVisible.Value = "true";
                                    }
                                    else
                                    {
                                        fVisible.Value = "false";
                                    }

                                    function.SetAttributeNode(fName);
                                    function.SetAttributeNode(fAction);
                                    function.SetAttributeNode(fVisible);

                                    functions.AppendChild(function);
                                }
                            }
                        }
                    }
                    functionsElement.AppendChild(functions);
                } // ct

                if (elem.RefName.IsEmpty) // skip recursing into references, they will be visited as global elements
                {
                    XmlSchemaType type = (XmlSchemaType)elem.ElementSchemaType;

                    if (type is XmlSchemaComplexType)
                    {
                        ct = (XmlSchemaComplexType)type;

                        if (ct.QualifiedName.IsEmpty) // name that a ref attribute refers to, similar to above
                        {   // recurse using this element as the parent
                            WalkTheParticle(ct.ContentTypeParticle, elementName);
                        }
                    }
                }
            }
            else if (particle is XmlSchemaGroupBase)
            //follow xs:all, xs:choice, xs:sequence
            {
                XmlSchemaGroupBase baseParticle = (XmlSchemaGroupBase)particle;

                foreach (XmlSchemaParticle subParticle in baseParticle.Items)
                {
                    WalkTheParticle(subParticle, parentName);
                }
            }
        }

        private void BuildConnectElement(String fromName, String toName)
        {
            if (fromName != noParent)
            {
                XmlElement connect = linkElement.OwnerDocument.CreateElement("Connect");

                XmlAttribute from = linkElement.OwnerDocument.CreateAttribute(ConfigFileConstants.from);
                from.Value = fromName;

                XmlAttribute to = linkElement.OwnerDocument.CreateAttribute(ConfigFileConstants.to);
                to.Value = toName;

                connect.SetAttributeNode(from);
                connect.SetAttributeNode(to);

                linkElement.AppendChild(connect);
            }
        }

        // unify the links in the config file with these links
        // (duplicates in the config file are likely!)
        private void PostProcess(XmlDocument document)
        {
            String dynamicNodeString = "Dynamic";
            String parametersNodeString = "ComplexParameters";

            String[] linkAttributes = new String[] { ConfigFileConstants.Name, ConfigFileConstants.description, 
                ConfigFileConstants.dynamicType, ConfigFileConstants.deepCopy };
                                                    
            IXPathNavigable sourceLinks = this.m_model.GetLinks();
            XPathNavigator sourceLinksNav = sourceLinks.CreateNavigator();

            XmlNodeList newLinkList = document.SelectNodes("/Root/Link");
            foreach (XmlNode newLink in newLinkList)
            {
                String type = newLink.Attributes[ConfigFileConstants.Type].Value;
                XPathNodeIterator matchingSourceLinks = sourceLinksNav.Select("Link[@type='" + type + "']");
                foreach(XPathNavigator matchingSourceLink in matchingSourceLinks)
                {
                    for (int i = 0; i < linkAttributes.Length; i++)
                    {
                        String attribute = linkAttributes[i];

                        String checkAttribute = matchingSourceLink.GetAttribute(attribute, String.Empty);
                        if (checkAttribute != null && checkAttribute.Length > 0 && newLink.Attributes[attribute] == null)
                        {

                            XmlAttribute appendLinkAttribute = newLink.OwnerDocument.CreateAttribute(attribute);
                            appendLinkAttribute.Value = checkAttribute;

                            newLink.Attributes.Append(appendLinkAttribute);
                            
                        }
                    }

                    XPathNavigator checkDynamic = matchingSourceLink.SelectSingleNode(dynamicNodeString);
                    if (checkDynamic != null && newLink.SelectSingleNode(dynamicNodeString) == null)
                    {
                        XmlNode importDynamic = ((IHasXmlNode)checkDynamic).GetNode();
                        importDynamic = newLink.OwnerDocument.ImportNode(importDynamic, true);
                            newLink.AppendChild(importDynamic);   
                    }

                    XPathNodeIterator connects = matchingSourceLink.SelectChildren("Connect", String.Empty);
                    foreach (XPathNavigator connect in connects)
                    {
                        String from = connect.GetAttribute(ConfigFileConstants.from, String.Empty);
                        String to = connect.GetAttribute(ConfigFileConstants.to, String.Empty);

                        if (from != null && to != null && from.Length > 0 && to.Length > 0)
                        {
                            XmlNode newConnectNode = newLink.SelectSingleNode("Connect[@from='" + from + "'][@to='" + to + "']");
                            if (newConnectNode != null)
                            {
                                XPathNavigator checkParameters = connect.SelectSingleNode(parametersNodeString);
                                if (checkParameters != null && newConnectNode.SelectSingleNode(parametersNodeString) == null)
                                {
                                    XmlNode importParameters = ((IHasXmlNode)checkParameters).GetNode();
                                    importParameters = newLink.OwnerDocument.ImportNode(importParameters, true);
                                    newConnectNode.AppendChild(importParameters);   
                                }
                            }
                            else
                            {
                                Console.WriteLine("Cannot find connect: from: " + from + " to: " + to);
                            }
                        }
                    }
                } // matching

                // double check required attributes, fill in with temporary data to validate
                // developer should fill in appropriate values later
                if (newLink.Attributes[linkAttributes[0]] == null) // name
                {
                    XmlAttribute appendLinkAttribute = newLink.OwnerDocument.CreateAttribute(linkAttributes[0]);
                    appendLinkAttribute.Value = "None given";
                    newLink.Attributes.Append(appendLinkAttribute);
                }

                if (newLink.Attributes[linkAttributes[1]] == null) // description
                {
                    XmlAttribute appendLinkAttribute = newLink.OwnerDocument.CreateAttribute(linkAttributes[1]);
                    appendLinkAttribute.Value = "None given";
                    newLink.Attributes.Append(appendLinkAttribute);
                }

            }
        }
    }
}
