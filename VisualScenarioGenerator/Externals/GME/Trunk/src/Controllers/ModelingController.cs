using System;
using System.Collections.Generic;
using System.Text;
using AME.Model;
using System.Data;
using System.Xml.XPath;
using System.Xml;
using System.Runtime.Remoting;
using System.Reflection;
using AME.Adapters;
using AME.EngineModels;
using System.Threading;
using System.IO;
using AME.Controllers;
using System.Web;
using AME.Controllers.Base.DataStructures;

namespace AME.Controllers
{
    public class ModelingController : Controller, IModelingController
    {
        private String dateTime;

        public ModelingController(AME.Model.Model model, String configType)
            : base(model, configType)
        {
        }

        #region IModelingController Members

        public Dictionary<String, String> GetInputNames(Int32 componentId, Boolean selectable)
        {
            DataRow component = base._GetComponent(componentId);
            String compType = (String)component[SchemaConstants.Type];

            return this.GetInputNames(compType, selectable);
        }//GetInputNames

        private Dictionary<String, String> GetInputNames(String name, Boolean selectable)
        {
            Dictionary<String, String> inputs = new Dictionary<String, String>();
            IXPathNavigable inav = m_model.GetComponent(name);
            XPathNavigator nav = inav.CreateNavigator();
            XPathNodeIterator itInputs = nav.Select(String.Format("EngineModel/Inputs/Input"));

            while (itInputs.MoveNext())
            {
                String isSelectable = itInputs.Current.GetAttribute("selectable", String.Empty);
                if (selectable)
                {
                    if (XmlConvert.ToBoolean(isSelectable))
                    {
                        String input = itInputs.Current.GetAttribute("name", String.Empty);
                        String link = itInputs.Current.GetAttribute("link", String.Empty);
                        inputs.Add(input, link);
                    }
                }
                else
                {
                    if (!XmlConvert.ToBoolean(isSelectable))
                    {
                        String input = itInputs.Current.GetAttribute("name", String.Empty);
                        String link = itInputs.Current.GetAttribute("link", String.Empty);
                        inputs.Add(input, link);
                    }
                }
            }
            return inputs;
        }

        public Int32[] Run(Int32 selectedId, List<Int32> selectedInputIds)
        {

            //Return values collection
            List<Int32> runIds = new List<Int32>();

            ////////////////////////////////////////////////////////////////////////////////
            // 1. Build the source xml file                                               //
            //    This contains all the inputs and simulation information needed to       //
            //    run a simulation                                                        //
            ////////////////////////////////////////////////////////////////////////////////

            // Create source xml file
            this.EnableLoadingCache();

            XmlDocument iSource = buildSourceXml(selectedId, selectedInputIds);

            this.DisableLoadingCache();

            // Get the component type from database using selectedId
            String componentType = (String)_GetComponent(selectedId)["type"];
            // Use the component type to get engine model information from config file
            IXPathNavigable iNavComponent = this.m_model.GetComponent(componentType);
            XPathNavigator navComponent = iNavComponent.CreateNavigator();
            XPathNavigator navEngineModel = navComponent.SelectSingleNode("EngineModel");
            if (navEngineModel == null)
                return runIds.ToArray(); // Inform the user about bad config?
            // Get base type
            String componentBaseType = navComponent.GetAttribute("base", navComponent.NamespaceURI); ;

            // Get outputs (run, raw, visualization, measures)
            XPathNavigator navRun = navEngineModel.SelectSingleNode("Outputs/Output[@type='Run']");
            XPathNavigator navRaw = navEngineModel.SelectSingleNode("Outputs/Output[@type='Raw']");
            XPathNavigator navVisualization = navEngineModel.SelectSingleNode("Outputs/Output[@type='Visualization']");
            XPathNavigator navMeasure = navEngineModel.SelectSingleNode("Outputs/Output[@type='Measure']");

            String linkType = navEngineModel.GetAttribute("link", navComponent.NamespaceURI);
            String engineModelAssembly = navEngineModel.GetAttribute("assembly", navComponent.NamespaceURI);
            String engineModelName = navEngineModel.GetAttribute("name", navComponent.NamespaceURI);
            IEngineModel engineModel = null;
            try
            {
                Assembly assembly = Assembly.Load(engineModelAssembly);
                Object obj = assembly.CreateInstance(engineModelAssembly + "." + engineModelName);
                engineModel = (IEngineModel)obj;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Simulation Error");
                return runIds.ToArray();
            }

            engineModel.Name = componentType;
            engineModel.SourceXml = iSource; // Reference the source xml (this could be rather large so cloning it might be time consuming)
            engineModel.ModelConfiguration = this.m_model.ModelConfiguration;

            dateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");

            // Adapters
            if (processAdapter(navEngineModel, engineModel, "Input", selectedId, linkType))
            {
                if (processAdapter(navEngineModel, engineModel, "Tool", selectedId, linkType))
                {
                    if (processAdapter(navEngineModel, engineModel, "Output", selectedId, linkType))
                    {
                        // Create the parent Run component to attach to the Simulation.
                        // Raw, Measures and Visualization components are all children of Run.
                        String sRunType = String.Empty;
                        if (navRun != null)
                        {
                            sRunType = navRun.GetAttribute("name", navRun.NamespaceURI);
                            Int32 runId = AddComponent(selectedId, selectedId, sRunType, sRunType, linkType, "Run for " + engineModel.Name + " id: " + selectedId).ComponentID;
                            runIds.Add(runId);
                            // Parameters for the Run are the inputs.
                            IXPathNavigable iNavSource = engineModel.SourceXml;
                            XPathNavigator navSource = iNavSource.CreateNavigator();
                            XPathNodeIterator itInputs = navSource.Select("//Inputs/Component");
                            while (itInputs.MoveNext())
                            {
                                String name = itInputs.Current.GetAttribute("BaseType", itInputs.Current.NamespaceURI);
                                if (name.Equals(String.Empty))
                                    name = itInputs.Current.GetAttribute("Type", itInputs.Current.NamespaceURI);
                                String value = itInputs.Current.GetAttribute("Name", itInputs.Current.NamespaceURI);

                                try
                                {
                                    this.UpdateParameters(runId, "Inputs." + name, value, eParamParentType.Component);
                                }
                                catch (Exception)
                                {
                                    // Skip the ones we do not want!
                                }
                            }
                            IXPathNavigable iNavSelected = GetParametersForComponent(selectedId);
                            XPathNavigator navSelected = iNavSelected.CreateNavigator();
                            XPathNodeIterator itParameters = navSelected.Select(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter", componentBaseType));
                            foreach (XPathNavigator navParameter in itParameters)
                            {
                                String name = navParameter.GetAttribute("displayedName", navParameter.NamespaceURI);
                                String value = navParameter.GetAttribute("value", navParameter.NamespaceURI);

                                try
                                {
                                    this.UpdateParameters(runId, componentBaseType + "." + name, value, eParamParentType.Component);
                                }
                                catch (Exception)
                                {
                                    // Skip the ones we do not want!
                                }
                            }
                            ////////////////////////////////////////////////////////////////////////////////
                            // 2. Create files to store on disk and add info to the database              //
                            //    Run file, measure input file, and visualization file                    //
                            ////////////////////////////////////////////////////////////////////////////////

                            #region A. Create Raw File.
                            if ((engineModel.RawXml != null) && (navRaw != null))
                            {
                                String sRawType = navRaw.GetAttribute("name", navRun.NamespaceURI);

                                // Check for multiple Runs!
                                XPathNavigator rawNav = engineModel.RawXml.CreateNavigator();
                                String runFilename = "Raw_" + dateTime;
                                //String runFilename = String.Format("{0}_{1}}", sRunType, dateTime);

                                XmlTextWriter writer = new XmlTextWriter(Path.Combine(this.OutputPath, runFilename + ".xml"), System.Text.Encoding.UTF8);
                                rawNav.WriteSubtree(writer);
                                writer.Close();

                                // Create component in DB as well as paramters and links
                                // Add component and link
                                Int32 rawId = AddComponent(selectedId, runId, sRawType, sRunType, linkType, "Raw for " + engineModel.Name + " id: " + selectedId).ComponentID;
                                // Parameters
                                this.UpdateParameters(rawId, "Raw Parameters.Filename", runFilename, eParamParentType.Component); // Assume filename exists as a param?   
                            }
                            #endregion

                            #region B. Store Measures Input File
                            if ((engineModel.MeasureInputXml != null) && (navMeasure != null))
                            {
                                String sMeasureType = navMeasure.GetAttribute("name", navRun.NamespaceURI);

                                String measureInputFilename = "MeasureInput_" + componentType + "_" + dateTime;
                                XmlDocument measureInput = (XmlDocument)engineModel.MeasureInputXml;
                                measureInput.Save(Path.Combine(this.OutputPath, measureInputFilename + ".xml"));

                                // Create component in DB as well as paramters and links
                                // Add component and link
                                Int32 measureId = AddComponent(selectedId, runId, sMeasureType, sMeasureType, linkType, "Measure for " + engineModel.Name + " id: " + selectedId).ComponentID;
                                // Add parameters
                                this.UpdateParameters(measureId, "Measure Parameters.Input Filename", measureInputFilename, eParamParentType.Component);
                            }
                            #endregion

                            #region C. Create Visualization File
                            //if (engineModel.VisualizationXml != null && (navVisualization != null))
                            if (engineModel.GetVisualizationXml().Count > 0 && (navVisualization != null))
                            {
                                // Create component in DB as well as paramters and links
                                // Add component and link
                                String sVisType = navVisualization.GetAttribute("name", navRun.NamespaceURI);
                                Int32 visualizationId = AddComponent(selectedId, runId, sVisType, sVisType, linkType, "Visualization for " + engineModel.Name + " id: " + selectedId).ComponentID;

                                foreach (String parameter in engineModel.GetVisualizationXml().Keys)
                                {
                                    String visualizationFilename = "Visualization." + parameter + "_" +dateTime;
                                    XmlDocument visualization = (XmlDocument)engineModel.GetVisualizationXml()[parameter];
                                    if (!String.IsNullOrEmpty(visualization.InnerXml))
                                    {
                                        visualization.Save(Path.Combine(this.OutputPath, visualizationFilename + ".xml"));

                                        // Add parameters
                                        // Assume Visualization Parameters is category?   
                                        this.UpdateParameters(visualizationId, String.Format("Visualization Parameters.{0}", parameter), visualizationFilename, eParamParentType.Component);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            return runIds.ToArray();
        }

        #endregion

        private Boolean processAdapter(XPathNavigator navEngineModel, IEngineModel engineModel, String adapterType, Int32 id, String link)
        {
            ////////////////////////////////////////
            // Adapter                            //
            ////////////////////////////////////////

            String engineModelAssembly = navEngineModel.GetAttribute("assembly", navEngineModel.NamespaceURI);

            // Get the name of the input adapter from config and create an InputAdapter       
            XPathNavigator navAdapter = navEngineModel.SelectSingleNode(String.Format("Adapters/Adapter[@type='{0}']", adapterType));
            //if (navAdapter == null)
            //    return true; // for opt bug.
            String adapterName = navAdapter.GetAttribute("name", navAdapter.NamespaceURI);
            String adapterFullName = navAdapter.GetAttribute("fullname", navAdapter.NamespaceURI);
            IXPathNavigable iComponentAdapter = m_model.GetComponent(adapterName);

            ObjectHandle objAdapter;
            Boolean success = false;
            try
            {
                objAdapter = Activator.CreateInstance(engineModelAssembly, adapterFullName);

                IModelingAdapter adapter = (IModelingAdapter)objAdapter.Unwrap();

                IXPathNavigable iNavigator = this.GetComponentAndChildren(id, link, new ComponentOptions());
                XPathNavigator navigator = iNavigator.CreateNavigator();
                XPathNavigator navigatorAdapter = navigator.SelectSingleNode(String.Format("Components/Component/Component[@BaseType='{0}']", adapterType));
                if (navigatorAdapter != null)
                    adapter.Component = navigatorAdapter;
                else
                    adapter.Component = iComponentAdapter;
       
                adapter.ModelConfiguration = this.m_model.ModelConfiguration;
                success = adapter.Process(engineModel);
            }
            catch (TypeLoadException)
            {
                // What to do?
            }

            return success;
        }

        protected virtual XmlDocument buildSourceXml(Int32 selectedId, List<Int32> selectedInputIds)
        {
            // Build the source xml file
            XmlDocument source = new XmlDocument();
            XmlDeclaration declaration = source.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            source.AppendChild(declaration);
            XmlElement root = source.CreateElement("Inputs");

            ComponentOptions compOptionsSelNodes = new ComponentOptions();
            compOptionsSelNodes.CompParams = true;
            compOptionsSelNodes.ClassInstanceInfo = true;
            compOptionsSelNodes.SubclassInstanceInfo = true;

            Dictionary<String, String> typeToBaseCache = new Dictionary<String, String>();
            Dictionary<String, XmlNode> typeToParametersXMLCache = new Dictionary<String, XmlNode>();
            Dictionary<String, List<DataRow>> parameterTableCache = this.GetParameterTableCache();

            XmlNode nodeSelected = _GetXmlTree(source, this._GetComponent(selectedId), compOptionsSelNodes, null, typeToBaseCache,
                typeToParametersXMLCache, parameterTableCache);
            root.AppendChild(nodeSelected);

            foreach (Int32 input in selectedInputIds)
            {
                XmlNode nodeComponent = _GetXmlTree(source, this._GetComponent(input), compOptionsSelNodes, null, typeToBaseCache, typeToParametersXMLCache, parameterTableCache);
                String component = nodeComponent.Attributes["Type"].Value;

                String selectedType = (String)_GetComponent(selectedId)["name"]; // Think about this, why reversed in DB
                String selectedName = (String)_GetComponent(selectedId)["type"];
                IXPathNavigable iComponentSimType = m_model.GetComponent(selectedName);
                XPathNavigator navComponentSimType = iComponentSimType.CreateNavigator();

                XPathNodeIterator itLinkTypes = navComponentSimType.Select(String.Format("EngineModel/Inputs/Input[@name='{0}']/Links/Link", component));
                foreach (XPathNavigator navLinkType in itLinkTypes)
                {
                    String linkType = navLinkType.GetAttribute("type", String.Empty);
                    XmlNode nodeLinkType = source.CreateElement("Link");
                    XmlAttribute attributeLinkType = source.CreateAttribute("Name");
                    attributeLinkType.Value = linkType;
                    nodeLinkType.Attributes.Append(attributeLinkType);

                    // 3/11/09 mw
                    // Look for includes to pass to ComponentOptions in controller for bridging link types
                    List<Include> includes = new List<Include>();
                    XPathNodeIterator includesIterator = navLinkType.Select("Include");
                    foreach (XPathNavigator includeNavigator in includesIterator)
                    {
                        Boolean isDynamic = Boolean.Parse(includeNavigator.GetAttribute("isDynamic", String.Empty));
                        String linkTypeInclude = includeNavigator.GetAttribute("linkType", String.Empty);
                        String componentType = includeNavigator.GetAttribute("componentType", String.Empty);
                        uint? levelDown = null;
                        bool includeParameters = true;

                        String sLevelDown = includeNavigator.GetAttribute("level", String.Empty);
                        if (sLevelDown != null)
                        {
                            levelDown = uint.Parse(sLevelDown);
                        }

                        String sIncludeParameters = includeNavigator.GetAttribute("includeParameters", String.Empty);
                        if (sIncludeParameters != null)
                        {
                            includeParameters = Boolean.Parse(sIncludeParameters);
                        }

                        Include include = new Include(componentType, linkTypeInclude, isDynamic, levelDown, includeParameters);
                        includes.Add(include);
                    }

                    // 5/8/08 mw - configuration xml change. Input now looks like Inputs/Input/Links/Link[@type].  
                    // so take the type given there and go find the link under Global/Links/Link
                    IXPathNavigable linkIteratorIXP = m_model.GetLink(linkType);
                    if (linkIteratorIXP == null)
                    {
                        throw new Exception("Could not find linktype in Global/Links/Link: " + linkType);
                    }

                    XPathNavigator linkIterator = linkIteratorIXP.CreateNavigator();

                    // Is this a dynamic linktype?
                    XPathNavigator navDynamic = linkIterator.SelectSingleNode("Dynamic");
                    // Yes then get names.
                    if (navDynamic != null)
                    {
                        String dynamicComponent = navDynamic.GetAttribute("component", navDynamic.NamespaceURI);
                        String refLinkType = navDynamic.GetAttribute("refLinkType", navDynamic.NamespaceURI);
                        Boolean isRoot = Boolean.Parse(navDynamic.GetAttribute("isRoot", navDynamic.NamespaceURI));

                        ComponentOptions optionsRefLinkType = new ComponentOptions();
                        //optionsRefLinkType.LevelDown = 2; // Level needs to be removed when bug fix is completed!
                        optionsRefLinkType.CompParams = false;
                        optionsRefLinkType.LinkParams = false;
                        optionsRefLinkType.ClassInstanceInfo = false;
                        optionsRefLinkType.SubclassInstanceInfo = false;

                        // Should ref link always be dynamic? This could be fixed with always using dynamic links!
                        String refLinkTypeDynamic = navDynamic.GetAttribute("refLinkTypeDynamic", navDynamic.NamespaceURI);
                        Boolean isRefLinkTypeDynamic = true;
                        if (!refLinkTypeDynamic.Equals(String.Empty))
                            isRefLinkTypeDynamic = XmlConvert.ToBoolean(refLinkTypeDynamic);

                        // What if we dont want PROJECT as the root id for inputs?

                        IXPathNavigable iNavRefLinkType = isRefLinkTypeDynamic ? this.GetComponentAndChildren(input, this.GetDynamicLinkType(refLinkType, input.ToString()), optionsRefLinkType) :
                                                                               this.GetComponentAndChildren(input, refLinkType, optionsRefLinkType);
                        XPathNavigator navRefLinkType = iNavRefLinkType.CreateNavigator();

                        XPathNodeIterator itDynamicComponents = isRoot ? navRefLinkType.Select(String.Format("/Components/Component[@Type='{0}']", dynamicComponent)) :
                                                                         navRefLinkType.Select(String.Format("/Components/Component/Component[@Type='{0}']", dynamicComponent));

                        while (itDynamicComponents.MoveNext())
                        {
                            ComponentOptions optionsDynamicComponent = new ComponentOptions();
                            optionsDynamicComponent.LevelDown = 2; // Level needs to be removed when bug fix is completed!
                            optionsDynamicComponent.CompParams = true;
                            optionsDynamicComponent.LinkParams = true;
                            optionsDynamicComponent.ClassInstanceInfo = true;
                            optionsDynamicComponent.SubclassInstanceInfo = true;
                            optionsDynamicComponent.Includes = includes;

                            Int32 dynamicComponentId = Int32.Parse(itDynamicComponents.Current.GetAttribute("ID", itDynamicComponents.Current.NamespaceURI));
                            String dynamicLinkType = GetDynamicLinkType(linkType, dynamicComponentId.ToString());
                            IXPathNavigable iNavDynamicLink = this.GetComponentAndChildren(input, dynamicLinkType, optionsDynamicComponent);
                            XPathNavigator navDynamicLink = iNavDynamicLink.CreateNavigator();
                            navDynamicLink = navDynamicLink.SelectSingleNode("/Components/Component");

                            XmlNode nDynamicLink = source.CreateNode(XmlNodeType.Element, "Dynamic", navDynamicLink.NamespaceURI);
                            XmlAttribute aDynamicLinkName = source.CreateAttribute("Name");
                            aDynamicLinkName.Value = dynamicLinkType;
                            nDynamicLink.Attributes.Append(aDynamicLinkName);
                            nodeLinkType.AppendChild(nDynamicLink);

                            XPathNodeIterator itDynamicLinks = navDynamicLink.SelectChildren(XPathNodeType.All);
                            while (itDynamicLinks.MoveNext())
                            {
                                XmlNode nodeSource = ((IHasXmlNode)itDynamicLinks.Current).GetNode();
                                XmlNode nodeDestination = source.ImportNode(nodeSource, true);
                                nDynamicLink.AppendChild(nodeDestination);
                            }
                            nodeComponent.AppendChild(nodeLinkType);
                        }
                    }
                    // No, just get components.
                    else
                    {
                        ComponentOptions compOptions = new ComponentOptions();
                        compOptions.LevelDown = 2; // Level needs to be removed when bug fix is completed!
                        compOptions.CompParams = true;
                        compOptions.ClassInstanceInfo = true;
                        compOptions.SubclassInstanceInfo = true;
                        compOptions.Includes = includes;

                        IXPathNavigable inav = this.GetComponentAndChildren(input, linkType, compOptions);
                        XPathNavigator nav = inav.CreateNavigator();
                        nav = nav.SelectSingleNode(String.Format("/Components/Component[@Type='{0}' and @ID='{1}']", component, input));
                        XPathNodeIterator itNav = nav.SelectChildren(XPathNodeType.All);
                        while (itNav.MoveNext())
                        {
                            XmlNode nodeSource = ((IHasXmlNode)itNav.Current).GetNode();
                            XmlNode nodeDestination = source.ImportNode(nodeSource, true);
                            nodeLinkType.AppendChild(nodeDestination);
                        }
                        nodeComponent.AppendChild(nodeLinkType);
                    }
                }
                root.AppendChild(nodeComponent);
            }
            source.AppendChild(root);
            source.Save(Path.Combine(this.OutputPath, "source.xml"));
            return source;
        }

        public Int32[] getParents(Int32 componentId, String linkType)
        {
            DataTable dt = this.m_model.GetParentComponents(componentId, linkType);
            List<Int32> ids = this._GetIDs(dt);
            return ids.ToArray();
        }

        public Int32[] getChildern(Int32 componentId, String linkType)
        {
            DataTable dt = this.m_model.GetChildComponents(componentId, linkType);
            List<Int32> ids = this._GetIDs(dt);
            return ids.ToArray();
        }
    }
}
