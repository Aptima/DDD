using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Data;
using System.Xml.XPath;
using System.Xml;
using System.Runtime.Remoting;
using System.Reflection;
using GME.Adapters;
using GME.EngineModels;
using System.Threading;
using System.IO;
using GME;

namespace GME.Controllers
{
    public class SimulationController : Controller, ISimulationController
    {   
        #region Constructors

        public SimulationController(IModel model, String configType)
            : base(model, configType)
        {
        }

        //public List<String> GetSimulations()
        //{
        //    return m_model.GetComponentTypes(this.Configuration, this.RootComponentType);
        //}

        public List<String> GetInputs(String name, String type)
        {
            List<String> inputs = new List<String>();
            IXPathNavigable inav = m_model.GetComponent(this.Configuration, name, type);
            XPathNavigator nav = inav.CreateNavigator();
            XPathNodeIterator itInputs = nav.Select(String.Format("/Inputs/Input"));

            while (itInputs.MoveNext())
            {
                String input = itInputs.Current.GetAttribute("name", String.Empty);
                inputs.Add(input);
            }
            return inputs;
        }

        public void Run(Int32 simulationId, List<Int32> simRunInputs, String simRunName)
        {
            ////////////////////////////////////////////////////////////////////////////////
            // 1. Build the source xml file                                               //
            //    This contains all the inputs and simulation information needed to       //
            //    run a simulation                                                        //
            ////////////////////////////////////////////////////////////////////////////////

            // Create source xml file
            XmlDocument iSource = buildSourceXml(simulationId, simRunInputs);

            // Get the sim model type from config using simulationId
            String engineModelType = (String)_GetComponent(simulationId)["name"]; // Think about this, why reversed in DB
            String engineModelName = (String)_GetComponent(simulationId)["type"];

            // Create a engineModel  
            // Get the executing assembly which should always be the gme name
            String gmeAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            // Get the entry assembly which should always be the project name
            String projectAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            String engineModelTypeName = projectAssemblyName + ".Simulations." + engineModelType;
            ObjectHandle objengineModel = Activator.CreateInstance(projectAssemblyName, engineModelTypeName);
            IEngineModel engineModel = (IEngineModel)objengineModel.Unwrap();
            engineModel.Name = engineModelType;
            engineModel.SourceXml = iSource; // Reference the source xml (this could be rather large so cloning it might be time consuming)
            engineModel.LogFilename = "SimRun_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");

            // Create a node that represents the Simulation selected 
            IXPathNavigable iComponentSimType = m_model.GetComponent(Configuration, engineModelName, engineModelType);
            XPathNavigator navComponentSimType = iComponentSimType.CreateNavigator();

            // Adapters

            if (InputAdapter(navComponentSimType, gmeAssemblyName, projectAssemblyName, engineModel))
            {
                if (ToolAdapter(navComponentSimType, gmeAssemblyName, projectAssemblyName, engineModel))
                {
                    if (OutputAdapter(navComponentSimType, gmeAssemblyName, projectAssemblyName, engineModel))
                    {
                        // A. Create SimRun File.

                        // B. Create Measures File

                        // C.

                        ////////////////////////////////////////////////////////////////////////////////
                        // 2. Create a SimRun file to store on disk and add info to the database      //
                        //    This contains all the outputs and simulation information needed to      //
                        //    run assessment                                                          //
                        ////////////////////////////////////////////////////////////////////////////////
                        XmlDocument simRun = (XmlDocument)engineModel.LogXml;
                        simRun.Save(Path.Combine(GMEManager.Instance.ConfigurationPath, @"output\" + engineModel.LogFilename + ".xml"));

                        // Create component in DB as well as paramters and links
                        // Add component and link
                        Int32 cId = AddComponent(simulationId, simulationId, "SimRun", simRunName, "Simulation", "SimRun for " + engineModel.Name + ": " + simulationId);
                        // Add parameters
                        IXPathNavigable iNav = engineModel.SourceXml;
                        XPathNavigator nav = iNav.CreateNavigator();
                        XPathNodeIterator itInputs = nav.Select("//Inputs/Component");
                        while (itInputs.MoveNext())
                        {
                            String name = itInputs.Current.GetAttribute("Type", String.Empty);  // Case on attribute doesnt match other xml sources (see 'type')
                            String value = itInputs.Current.GetAttribute("Name", String.Empty);  // Case on attribute doesnt match other xml sources (see 'name')

                            this.UpdateParameters(cId, name, value, eParamParentType.Component);
                        }
                        this.UpdateParameters(cId, "Filename", engineModel.LogFilename, eParamParentType.Component); // Assume filename exists as a param?   
                    }
                }
            }
        }

        public Boolean InputAdapter(XPathNavigator navComponentSimType, String gmeAssemblyName, String projectAssemblyName, IEngineModel engineModel)
        {
            ////////////////////////////////////////
            // A. InputAdapter                    //
            ////////////////////////////////////////

            // Get the name of the input adapter from config and create an InputSimAdapter       
            XPathNavigator navSubcomponentInputAdapter = navComponentSimType.SelectSingleNode(String.Format("SubComponents/Component[@type='{0}']", "Input"));
            String inputAdapterName = navSubcomponentInputAdapter.GetAttribute("name", navSubcomponentInputAdapter.NamespaceURI);
            IXPathNavigable iComponentInputAdapter = m_model.GetComponent(Configuration, inputAdapterName);

            String inputAdapterTypeName = projectAssemblyName + ".Adapters." + inputAdapterName;
            ObjectHandle objInputSimAdapter;
            try
            {
                objInputSimAdapter = Activator.CreateInstance(projectAssemblyName, inputAdapterTypeName);
            }
            catch (TypeLoadException)
            {
                inputAdapterTypeName = inputAdapterTypeName.Replace(projectAssemblyName, gmeAssemblyName); // This needs to be changed in case project name is in the string twice
                objInputSimAdapter = Activator.CreateInstance(gmeAssemblyName, inputAdapterTypeName);
            }
            IModelingAdapter inputAdapter = (IModelingAdapter)objInputSimAdapter.Unwrap();
            inputAdapter.Component = iComponentInputAdapter;
            Boolean inputSuccess = inputAdapter.Process(engineModel);

            return inputSuccess;
        }

        public Boolean ToolAdapter(XPathNavigator navComponentSimType, String gmeAssemblyName, String projectAssemblyName, IEngineModel engineModel)
        {
            ////////////////////////////////////////
            // B. ToolAdapter                    //
            ////////////////////////////////////////

            // Get the name of the tool adapter from config and create a ToolSimAdapter          
            XPathNavigator navSubcomponentToolAdapter = navComponentSimType.SelectSingleNode(String.Format("SubComponents/Component[@type='{0}']", "Tool"));
            String toolAdapterName = navSubcomponentToolAdapter.GetAttribute("name", navSubcomponentToolAdapter.NamespaceURI);
            IXPathNavigable iComponentToolAdapter = m_model.GetComponent(Configuration, toolAdapterName);
            String toolAdapterTypeName = projectAssemblyName + ".Adapters." + toolAdapterName;
            ObjectHandle objToolSimAdapter;
            try
            {
                objToolSimAdapter = Activator.CreateInstance(Assembly.GetEntryAssembly().GetName().Name, toolAdapterTypeName);
            }
            catch (TypeLoadException)
            {
                toolAdapterTypeName = toolAdapterTypeName.Replace(projectAssemblyName, gmeAssemblyName); // This needs to be changed in case project name is in the string twice
                objToolSimAdapter = Activator.CreateInstance(gmeAssemblyName, toolAdapterTypeName);
            }
            IModelingAdapter toolAdapter = (IModelingAdapter)objToolSimAdapter.Unwrap();
            toolAdapter.Component = iComponentToolAdapter;
            Boolean toolSuccess = toolAdapter.Process(engineModel);

            return toolSuccess;
        }

        public Boolean OutputAdapter(XPathNavigator navComponentSimType, String gmeAssemblyName, String projectAssemblyName, IEngineModel engineModel)
        {
            ////////////////////////////////////////
            // C. OutputAdapter                    //
            ////////////////////////////////////////

            // Get the name of the output adapter from config and create an OutputSimAdapter          
            XPathNavigator navSubcomponentOutputAdapter = navComponentSimType.SelectSingleNode(String.Format("SubComponents/Component[@type='{0}']", "Output"));
            String outputAdapterName = navSubcomponentOutputAdapter.GetAttribute("name", navSubcomponentOutputAdapter.NamespaceURI);
            IXPathNavigable iComponentOutputAdapter = m_model.GetComponent(Configuration, outputAdapterName);
            String outputAdapterTypeName = projectAssemblyName + ".Adapters." + outputAdapterName;
            ObjectHandle objOutputSimAdapter;
            try
            {
                objOutputSimAdapter = Activator.CreateInstance(Assembly.GetEntryAssembly().GetName().Name, outputAdapterTypeName);
            }
            catch (TypeLoadException)
            {
                outputAdapterTypeName = outputAdapterTypeName.Replace(projectAssemblyName, gmeAssemblyName); // This needs to be changed in case project name is in the string twice
                objOutputSimAdapter = Activator.CreateInstance(gmeAssemblyName, outputAdapterTypeName);
            }
            IModelingAdapter outputAdapter = (IModelingAdapter)objOutputSimAdapter.Unwrap();
            outputAdapter.Component = iComponentOutputAdapter;
            Boolean outputSuccess = outputAdapter.Process(engineModel);

            return outputSuccess;
        }

        private XmlDocument buildSourceXml(Int32 simulationId, List<Int32> simRunInputs)
        {
            // Build the source xml file
            XmlDocument source = new XmlDocument();
            XmlDeclaration declaration = source.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            source.AppendChild(declaration);
            XmlElement root = source.CreateElement("Inputs");

            XmlNode nodeSimulation = _GetXmlTree(source, simulationId, true, false, true);
            root.AppendChild(nodeSimulation);

            foreach (Int32 input in simRunInputs)
            {
                XmlNode nodeComponent = _GetXmlTree(source, input, true, false, true);
                String component = nodeComponent.Attributes["Type"].Value;

                String simulationType = (String)_GetComponent(simulationId)["name"]; // Think about this, why reversed in DB
                String simulationName = (String)_GetComponent(simulationId)["type"];
                IXPathNavigable iComponentSimType = m_model.GetComponent(Configuration, simulationName, simulationType);
                XPathNavigator navComponentSimType = iComponentSimType.CreateNavigator();

                XPathNodeIterator itLinkTypes = navComponentSimType.Select(String.Format("/Inputs/Input[@name='{0}']/LinkTypes/Link", component));
                while (itLinkTypes.MoveNext())
                {
                    String linkType = itLinkTypes.Current.GetAttribute("type", String.Empty);
                    XmlNode nodeLinkType = source.CreateElement("Link");
                    XmlAttribute attributeLinkType = source.CreateAttribute("Name");
                    attributeLinkType.Value = linkType;
                    nodeLinkType.Attributes.Append(attributeLinkType);

                    IXPathNavigable inav = this.GetComponentAndChildren(input, linkType, 2, true, false, true);
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
                root.AppendChild(nodeComponent);
            }
            source.AppendChild(root);
            source.Save(@"C:\source.xml");
            return source;
        }

        //private List<String> getInputLinkTypes(XPathNavigator navSubcomponentInputAdapter, String component)
        //{
        //    List<String> list = new List<String>();
        //    String inputAdapterName = navSubcomponentInputAdapter.GetAttribute("name", navSubcomponentInputAdapter.NamespaceURI);
        //    IXPathNavigable iComponentInputAdapter = m_model.GetComponent(Configuration, inputAdapterName);
        //    XPathNavigator navComponentInputAdapter = iComponentInputAdapter.CreateNavigator();
        //    XPathNodeIterator itLinkTypesList = navComponentInputAdapter.Select(String.Format("SubComponents/Component[@name='{0}']/LinkTypes/Link", component));
        //    while (itLinkTypesList.MoveNext())
        //    {
        //        list.Add(itLinkTypesList.Current.GetAttribute("name", itLinkTypesList.Current.NamespaceURI));
        //    }
        //    return list;
        //}
        #endregion
    }
}