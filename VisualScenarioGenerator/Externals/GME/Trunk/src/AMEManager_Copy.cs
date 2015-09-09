using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using System.IO;
using System.Xml;
using AME.Model;
using System.Xml.Schema;
using System.Windows.Forms;
using AME.Controllers.Base;
using System.Xml.XPath;
using System.Data;

namespace AME
{
    public partial class AMEManager
    {
        public class CopyParameters
        {
            private IController readingController = null;
            private IController writingController = null;
            private String startingLinkType = null;
            private String newName = null;
            private Int32 startingRootID = -1;
            private Boolean clearWriteDB = false;
            private Boolean showDialog = true;
            private Boolean copyOutput = true;
            private Boolean updateAfterCopy = true;
            private List<String> limitToLinkTypes = null;
            private List<String> onlyLinkTheseTypes = null;
            private List<String> skipParameters = null;

            public IController ReadingController { get { return readingController; } set { readingController = value; } }
            public IController WritingController { get { return writingController; } set { writingController = value; } }
            public String StartingLinkType { get { return startingLinkType; } set { startingLinkType = value; } }
            public Int32 StartingRootID { get { return startingRootID; } set { startingRootID = value; } }
            public String NewName { get { return newName; } set { newName = value; } }
            public Boolean ClearWriteDB { get { return clearWriteDB; } set { clearWriteDB = value; } }
            public Boolean ShowDialog { get { return showDialog; } set { showDialog = value; } }
            public Boolean CopyOutput { get { return copyOutput; } set { copyOutput = value; } }
            public Boolean UpdateAfterCopy { get { return updateAfterCopy; } set { updateAfterCopy = value; } }
            public List<String> LimitToLinkTypes { get { return limitToLinkTypes; } set { limitToLinkTypes = value; } }
            public List<String> OnlyLinkTypes { get { return onlyLinkTheseTypes; } set { onlyLinkTheseTypes = value; } }
            public List<String> SkipParameters { get { return skipParameters; } set { skipParameters = value; } }
        }

        private Dictionary<String, List<String>> schemaRootToLinkType = new Dictionary<String, List<String>>();
        private Dictionary<String, Boolean> isDynamicRoot = new Dictionary<String, Boolean>();
        private Dictionary<String, String> dynamicBridge = new Dictionary<String, String>();
        private Dictionary<String, String> appCodeObjects = new Dictionary<String, String>();
        private Dictionary<String, String> refLinkTypes = new Dictionary<String, String>();
        private Dictionary<String, String> alreadyRead = new Dictionary<String, String>();
        private IController readingController, writingController;
        private BulkHelper helper;
        private Dictionary<Int32, String> seenComponentIDs = new Dictionary<Int32, String>();
        private Dictionary<String, String> limitToLinkTypes = new Dictionary<String, String>();
        private Dictionary<String, String> onlyLinkTheseTypesMap = new Dictionary<String, String>();
        private Dictionary<String, String> skipParametersMap = new Dictionary<String, String>();
        private String copiedRootName;
        private Boolean copyOutput;

        public int Copy(CopyParameters parameters, out String newName)
        {
            if (parameters.ReadingController == null && parameters.WritingController == null)
            {
                MessageBox.Show("No reading or writing controller provided!");
                newName = "-1";
                return -1;
            }

            if (parameters.ReadingController != null && parameters.WritingController == null)
            {
                parameters.WritingController = parameters.ReadingController; // use read to write if no write is supplied
                if (parameters.ClearWriteDB)
                {
                    MessageBox.Show("Clearing with no write controller is not recommended!");
                    newName = "-1";
                    return -1;
                }
            }

            if (parameters.StartingRootID == -1)
            {
                MessageBox.Show("No starting root ID provided!");
                newName = "-1";
                return -1;
            }

            this.readingController = parameters.ReadingController;
            this.writingController = parameters.WritingController;

            limitToLinkTypes = new Dictionary<String, String>();

            if (parameters.LimitToLinkTypes != null)
            {
                Dictionary<String, String> buildLLFromParameters = new Dictionary<String, String>();
                foreach (String llt in parameters.LimitToLinkTypes)
                {
                    buildLLFromParameters.Add(llt, llt);
                }
                limitToLinkTypes = buildLLFromParameters;
            }

            if (parameters.OnlyLinkTypes != null)
            {
                onlyLinkTheseTypesMap.Clear();

                foreach (String type in parameters.OnlyLinkTypes)
                {
                    onlyLinkTheseTypesMap.Add(type, type);
                }
            }

            if (parameters.SkipParameters != null)
            {
                skipParametersMap.Clear();

                foreach (String parameter in parameters.SkipParameters)
                {
                    skipParametersMap.Add(parameter, parameter);
                }
            }

            copyOutput = parameters.CopyOutput;

            schemaRootToLinkType.Clear();
            seenComponentIDs.Clear();
            isDynamicRoot.Clear();
            dynamicBridge.Clear();
            appCodeObjects.Clear();
            refLinkTypes.Clear();
            alreadyRead.Clear();

            // Read all of the XSD schemas associated with the configuration.xml in the specified directory
            XmlReader configFileReader = this.readingController.GetConfigurationReader();
            XmlDocument configFile = new XmlDocument();
            configFile.Load(configFileReader);
            configFileReader.Close();

            // .xsds are referenced in the Links
            XmlNodeList links = configFile.SelectNodes("GME/Global/Links/Link");
            String schemaFileName;
            XmlAttribute schemaFileAttribute;
            XmlReader schemaReader;
            XmlSchema schema;
            XmlSchemaSet schemaSet;
            Boolean foundRootAndLT;

            foreach (XmlNode link in links)
            {
                //(use the filename if provided otherwise, use the linktype + .xsd
                schemaFileAttribute = link.Attributes[ConfigFileConstants.schemaFilename];
                if (schemaFileAttribute != null)
                {
                    schemaFileName = schemaFileAttribute.Value;
                }
                else
                {
                    schemaFileName = link.Attributes[ConfigFileConstants.Type].Value + ".xsd";
                }

                XmlNode dynamic = link.SelectSingleNode("Dynamic");
                if (dynamic != null)
                {
                    String linkTypeValue = link.Attributes[ConfigFileConstants.Type].Value;
                    Boolean isRoot = Boolean.Parse(dynamic.Attributes[ConfigFileConstants.isRoot].Value);
                    isDynamicRoot.Add(linkTypeValue, isRoot);

                    if (!isRoot) // non root dynamic, use app code class and ref link type to find it later 
                    {
                        String appCode = dynamic.Attributes[ConfigFileConstants.appCode].Value;
                        appCodeObjects.Add(linkTypeValue, appCode);

                        String refLinkType = dynamic.Attributes[ConfigFileConstants.refLinkType].Value;
                        refLinkTypes.Add(linkTypeValue, refLinkType);
                    }
                }

                XmlNode bridgeDynamic = link.SelectSingleNode("BridgeDynamic");
                if (bridgeDynamic != null)
                {
                    String linkTypeValue = link.Attributes[ConfigFileConstants.Type].Value;
                    String value = bridgeDynamic.Attributes[ConfigFileConstants.component].Value;
                    dynamicBridge.Add(linkTypeValue, value);
                }

                schemaReader = this.readingController.GetXSD(schemaFileName);
                schema = XmlSchema.Read(schemaReader, null);
                schemaReader.Close();
                schemaSet = new XmlSchemaSet();
                schemaSet.Add(schema);
                schemaSet.Compile();

                foreach (XmlSchema compiledSchema in schemaSet.Schemas())
                {
                    foundRootAndLT = false;
                    foreach (XmlSchemaElement element in compiledSchema.Elements.Values)
                    {
                        foundRootAndLT |= FindRootAndLinkType(element);
                    }
                    if (!foundRootAndLT)
                    {
                        MessageBox.Show("Root and link type not found for schema: " + schemaFileName);
                    }
                }
            }

            // Schemas processed, start copying!
            if (!parameters.UpdateAfterCopy)
            {
                writingController.TurnViewUpdateOff();
            }

            readingController.EnableLoadingCache();

            helper = new BulkHelper();
            helper.BeginBulkOperations(writingController.Configuration);
            XPathNavigator self = readingController.GetComponent(parameters.StartingRootID).CreateNavigator().SelectSingleNode("Components/Component");
            
            copiedRootName = parameters.NewName;

            // ensure no invalid filename characters
            for (int i = 0; i < copiedRootName.Length; i++)
            {
                char c = copiedRootName[i];
                for (int j = 0; j < Path.GetInvalidFileNameChars().Length; j++)
                {
                    if (c == Path.GetInvalidFileNameChars()[j])
                    {
                        MessageBox.Show("Invalid character in new name: " + c);
                        newName = "-1";
                        return -1;
                    }
                }
            }

            newName = copiedRootName;

            String componentType = self.GetAttribute(XmlSchemaConstants.Display.Component.Type, String.Empty);
            String description = self.GetAttribute(XmlSchemaConstants.Display.Component.Description, String.Empty);

            if (!schemaRootToLinkType.ContainsKey(componentType))
            {
                schemaRootToLinkType.Add(componentType, new List<String>());
            }

            if (parameters.StartingLinkType != null && !schemaRootToLinkType[componentType].Contains(parameters.StartingLinkType))
            {
                schemaRootToLinkType[componentType].Add(parameters.StartingLinkType);
            }

            String copyID = helper.CreateRootComponent(componentType, copiedRootName, description);

            CopyIDType(parameters.StartingRootID, copyID, componentType, null, null);

            readingController.DisableLoadingCache();

            int rootID = helper.EndBulkOperations(parameters.ClearWriteDB, parameters.ShowDialog, "Copying " + componentType, "Copying...");

            if (!parameters.UpdateAfterCopy)
            {
                writingController.TurnViewUpdateOn(false, false);
            }

            helper = null;

            return rootID;
        }

        private void CopyIDType(Int32 componentID, String copyID, String componentType, XPathNavigator callingNavigator, String callingLinkType)
        {
            CheckForTransitionFromComponentType(componentID, copyID, componentType, callingNavigator, callingLinkType);
            
            String baseComponentType = readingController.GetBaseComponentType(componentType);
            if (baseComponentType != null)
            {
                CheckForTransitionFromComponentType(componentID, copyID, baseComponentType, callingNavigator, callingLinkType);
            }
        }

        private void CheckForTransitionFromComponentType(Int32 componentID, String copyID, String componentType, XPathNavigator callingNavigator, String callingLinkType)
        {
            if (schemaRootToLinkType.ContainsKey(componentType))
            {
                IXPathNavigable iNav;
                XPathNavigator nav;
                foreach (String linkType in schemaRootToLinkType[componentType])
                {
                    List<String> ltReads = new List<String>();
                    List<String> ltWrites = new List<String>();
                    ltReads.Add(linkType);
                    ltWrites.Add(linkType);
      
                    if (isDynamicRoot.ContainsKey(linkType))
                    {
                        ltReads.Clear();
                        ltWrites.Clear();
                        if (isDynamicRoot[linkType])
                        {
                            ltReads.Add(writingController.GetDynamicLinkType(linkType, "" + componentID));
                            ltWrites.Add(writingController.GetDynamicLinkType(linkType, copyID, true));
                        }
                        else
                        {
                            String refLT = refLinkTypes[linkType];
                            if (refLT.Equals(callingLinkType)) // non-dynamic and the calling linktype matches the ref
                            {
                                String appCode = appCodeObjects[linkType]; // call app code builder to get the dynamic links

                                Object obj = AMEManager.CreateObject(appCode, new object[] { });
                                if (obj != null)
                                {
                                    IDynamicLinkBuilder builder = (IDynamicLinkBuilder)obj;
                                    builder.GetDynamicLink(callingNavigator, writingController, linkType, seenComponentIDs, out ltReads, out ltWrites);
                                }
                            }
                        }
                    }

                    if (dynamicBridge.ContainsKey(linkType))
                    {
                        if (componentType.Equals(dynamicBridge[linkType]))
                        {
                            ltReads.Clear();
                            ltWrites.Clear();
                            ltReads.Add(writingController.GetDynamicLinkType(linkType, "" + componentID));
                            ltWrites.Add(writingController.GetDynamicLinkType(linkType, copyID, true));
                        }
                    }

                    for (int i = 0; i < ltReads.Count; i++)
                    {
                        String ltRead = ltReads[i];
                        String ltWrite = ltWrites[i];

                        String key = "" + componentID + ltRead;
                        if (!alreadyRead.ContainsKey(key))
                        {

                            iNav = readingController.GetComponentAndChildren(componentID, ltRead, new ComponentOptions());
                            nav = iNav.CreateNavigator();

                            XPathNavigator self = nav.SelectSingleNode("Components/Component");
                            XPathNodeIterator children = self.Select("Component");
                            foreach (XPathNavigator child in children)
                            {
                                CopyChild(copyID, copyID, linkType, ltWrite, child);
                            }
                            alreadyRead.Add(key, key);
                        }
                    }
                }
            }
        }

        private void CopyChild(String root, String parent, String baseLinkType, String linkTypeWrite, XPathNavigator child)
        {
            Int32 childID = Int32.Parse(child.GetAttribute(XmlSchemaConstants.Display.Component.Id, String.Empty));
            String name = child.GetAttribute(XmlSchemaConstants.Display.Component.Name, String.Empty);
            String componentType = child.GetAttribute(XmlSchemaConstants.Display.Component.Type, String.Empty);
            String description = child.GetAttribute(XmlSchemaConstants.Display.Component.Description, String.Empty);

            String copyChildID;
            Boolean newComponentCreate = true;
            Boolean newLinkCreate = true;

            if (!seenComponentIDs.ContainsKey(childID))
            {
                if (onlyLinkTheseTypesMap.ContainsKey(componentType))
                {
                    copyChildID = ""+childID; // use the same ID as the copied id - just link as an existing comp
                    seenComponentIDs.Add(childID, copyChildID);
                    helper.AddExistingComponent(childID, componentType, copyChildID, "");
                    newComponentCreate = false;
                }
                else
                {
                    copyChildID = helper.CreateComponent(componentType, name, description);
                    seenComponentIDs.Add(childID, copyChildID);
                }
            }
            else
            {
                copyChildID = seenComponentIDs[childID];
                newComponentCreate = false;
            }

            String copyChildLinkID = helper.Connect(root, parent, copyChildID, linkTypeWrite);
            if (copyChildLinkID == "-1")
            {
                newLinkCreate = false;
            }

            if (newComponentCreate) // only update parameters once
            {
                XPathNodeIterator parameterIterator = child.Select("ComponentParameters/Parameter/Parameter");
                foreach (XPathNavigator parameter in parameterIterator)
                {
                    writeParameter(copyChildID, parameter, eParamParentType.Component);
                }
            }
            if (newLinkCreate)
            {
                XPathNodeIterator parameterIterator = child.Select("LinkParameters/Parameter/Parameter");
                foreach (XPathNavigator parameter in parameterIterator)
                {
                    writeParameter(copyChildLinkID, parameter, eParamParentType.Link);
                }
            }

            CopyIDType(childID, copyChildID, componentType, child, baseLinkType); // recursively check for transitions from this type

            XPathNodeIterator children = child.Select("Component");
            foreach (XPathNavigator newChild in children)
            {
                CopyChild(root, copyChildID, baseLinkType, linkTypeWrite, newChild); // recursively copy children
            }
        }

        private void writeParameter(String copyChildID, XPathNavigator parameter, eParamParentType parent)
        {
            String category, pname, full, value, isOutput;
            category = parameter.GetAttribute(ConfigFileConstants.category, String.Empty);
            pname = parameter.GetAttribute(ConfigFileConstants.displayedName, String.Empty);
            full = category + SchemaConstants.ParameterDelimiter + pname;

            if (skipParametersMap.ContainsKey(full))
            {
                return;
            }
            
            value = parameter.GetAttribute(ConfigFileConstants.Value, String.Empty);

            isOutput = parameter.GetAttribute(ConfigFileConstants.isOutput, String.Empty);
            if (copyOutput)
            {
                if (isOutput != null && isOutput.Length > 0 && Boolean.Parse(isOutput))
                {
                    try
                    {
                        IXPathNavigable existingFile = readingController.GetOutputXml(value);
                        value = value + " (" + copiedRootName + ")";
                        readingController.WriteOutputXml(value, (XmlDocument)existingFile);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Warning: Trying to copy referenced output file: " + value + " but it does not exist in the output directory.  Skipping");
                    }
                }
            }

            helper.UpdateParameters(copyChildID, full, value, parent);
        }
   
        private bool FindRootAndLinkType(XmlSchemaElement element)
        {
            XmlSchemaComplexType ct;
            XmlSchemaAttribute ltAttribute;
            XmlSchemaSequence elementSequence;
            XmlSchemaChoice elementChoice;
            String ltAttributeValue;

            if (element.SchemaType is XmlSchemaComplexType) // look for LinkType attribute, Root element type is below this
            {
                ct = (XmlSchemaComplexType)element.SchemaType;
                if (ct.Attributes.Count > 0)
                {
                    ltAttribute = (XmlSchemaAttribute)ct.Attributes[0];
                    if (ltAttribute.Name.Equals("LinkType"))
                    {
                        ltAttributeValue = ltAttribute.FixedValue;

                        if (ct.ContentTypeParticle is XmlSchemaSequence)
                        {
                            elementSequence = (XmlSchemaSequence)ct.ContentTypeParticle; // root is sequence after LinkType attr
                            if (elementSequence.Items.Count == 1)
                            {
                                ProcessPotentialRoot(elementSequence.Items[0], ltAttributeValue);
                                return true;
                            }
                        }
                        else if (ct.ContentTypeParticle is XmlSchemaChoice) // root is choice after LinkType attr
                        {
                            elementChoice = (XmlSchemaChoice)ct.ContentTypeParticle;
                            if (elementChoice.Items.Count > 0)
                            {
                                foreach (XmlSchemaObject potentialRoot in elementChoice.Items)
                                {
                                    ProcessPotentialRoot(potentialRoot, ltAttributeValue);
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void ProcessPotentialRoot(XmlSchemaObject potentialRoot, String ltAttributeValue)
        {
            String rootName;
            XmlSchemaElement rootElement;
            if (potentialRoot is XmlSchemaElement)
            {
                rootElement = (XmlSchemaElement)potentialRoot;

                if (rootElement.Name == null)
                {
                    rootName = rootElement.QualifiedName.Name;
                }
                else
                {
                    rootName = rootElement.Name;
                }

                if (!schemaRootToLinkType.ContainsKey(rootName))
                {
                    schemaRootToLinkType.Add(rootName, new List<String>());
                }
                if (limitToLinkTypes.Count == 0 || limitToLinkTypes.ContainsKey(ltAttributeValue))
                {
                    schemaRootToLinkType[rootName].Add(ltAttributeValue);
                }
            }
        }
    }
}



        

                                











       

               
               
