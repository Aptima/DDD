using System;
using System.Collections.Generic;
using System.Text;
using AME.Views.View_Component_Packages;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Windows.Forms;
using Forms;
using AME.Model;
using log4net;
using AME;
using AME.Views.User_Controls;
using AME.EngineModels;

namespace AME.Controllers {

    public class AssessmentController : Controller {

        private static readonly ILog logger = LogManager.GetLogger(typeof(AssessmentController));

        //private const String SIMULATIONLINK = "Project";

        #region Constructors

        public AssessmentController(AME.Model.Model model, String configType)
            : base(model, configType) {
        }
        #endregion


        #region public Assessment Methods

		public bool ComponentExists(int componentId) {
			return this._ComponentExists(componentId);
		}

        public IXPathNavigable GetSimRunData(int id) {

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.CompParams = true;

            IXPathNavigable iNavigator = base._GetComponentAndChildren(id, "Raw", true, compOptions);
            XPathNavigator navigator = iNavigator.CreateNavigator();

            XPathNavigator navFilename = navigator.SelectSingleNode("/Components/Component/Component/ComponentParameters/Parameter/Parameter[@displayedName='Filename']");
            String filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);

            try
            {
                return this.m_model.GetOutputXml(filename);
            }
            catch (FileNotFoundException e)
            {
                logger.Debug("The data for the Run ID " + id + " is not found.  FileNotFoundException: " + e.Message);
                return null;
            }
        }

        public Int32 GetVisualizationId(int runId)
        {
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.CompParams = true;

            IXPathNavigable iNavigator = base._GetComponentAndChildren(runId, "Visualization", true, compOptions);// Doesnt return base type visualization!?
            XPathNavigator navigator = iNavigator.CreateNavigator();
            String visId = String.Empty;
            Int32 id = -1;
            if (navigator != null)
            {
                XPathNavigator navVisComponent = navigator.SelectSingleNode("Components/Component/Component[@BaseType='Visualization']");
                if (navVisComponent != null)
                {
                    visId = navVisComponent.GetAttribute("ID", navigator.NamespaceURI);
                    id = Int32.Parse("0" + visId);
                }
            }
            return id;
        }

        public IXPathNavigable GetAllProcessRuns(int processId, ProcessType processType) {

            DataTable subComponents = this.m_model.GetChildComponents(processId);

            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            String rootName = String.Empty;
            String runName = String.Empty;
            switch (processType) {
                case ProcessType.OPTIMIZATION:
                    rootName = "Optimization";
                    runName = "OptRun";
                    break;
                case ProcessType.SIMULATION:
                    rootName = "Simulation";
                    runName = "SimRun";
                    break;
                default:
                    throw new Exception("The Process Type is not defined in the ProcessControllType enum");
            }

            XmlElement root = doc.CreateElement(rootName);
            root.SetAttribute("id", processId.ToString());
            doc.AppendChild(root);

            int id;
            String name;
            DataTable parameters;
            XmlElement runNode;
            foreach (DataRow simRunRows in subComponents.Rows) {
                id = (int)simRunRows[SchemaConstants.Id];
                runNode = doc.CreateElement(runName);
                runNode.SetAttribute("id", id.ToString());

                name = simRunRows[SchemaConstants.Name].ToString();
                runNode.SetAttribute("name", name);
                root.AppendChild(runNode);

                parameters = this.m_model.GetParameterTable(id, eParamParentType.Component.ToString());
                XmlElement parameterNode;
                String parameterId, parameterName, parameterValue;
                foreach (DataRow parameterRows in parameters.Rows) {
                    parameterId = parameterRows[SchemaConstants.Id].ToString();
                    parameterName = parameterRows[SchemaConstants.Name].ToString();
                    parameterValue = parameterRows[SchemaConstants.Value].ToString();
                    parameterNode = doc.CreateElement("Parameter");
                    parameterNode.SetAttribute("name", parameterName);
                    parameterNode.SetAttribute("value", parameterValue);
                    runNode.AppendChild(parameterNode);
                }
            }

            return doc.Clone();
        }

        public IXPathNavigable GetMeasureData(int simRunId)
        {
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.CompParams = true;

            IXPathNavigable iNavigator = base._GetComponentAndChildren(simRunId, "Measure", true, compOptions);
            XPathNavigator navigator = iNavigator.CreateNavigator();

            return navigator;
        }

        public XmlDocument GetMeasurementLibrary(String libraryName) { //tested <ml>

            XmlDocument xmlDoc = null;
            String libName = "_measurement_library.xml";

            if (libName != null)
                libName = libraryName + libName;

            try {
                String file = this.m_model.DataPath + @"\" + libName;
                FileInfo xmlFile = new FileInfo(file);
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile.OpenText());
            }
            catch (Exception ex) {
                logger.Warn(ex.Message);
                logger.Debug(String.Format("Failed to load for '{0}'!",
                    libName));
            }
            return xmlDoc;
        }

        ////////////////////////////////////////////////
        //                                            //
        //   Retreave, Add or Delete CUSTOM GRAPHS    //
        //                                            //
        ////////////////////////////////////////////////

        public IXPathNavigable GetCustomGraphs(int simRunId) {
            return this.getGraphs(simRunId);
        }

        public bool AddCustomGraphToFile(int simRunId, XmlElement element) {

            XmlDocument doc = (XmlDocument)this.getGraphs(simRunId);

            if (doc.DocumentElement == null) { //empty document
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                doc.AppendChild(declaration);

                XmlElement root = doc.CreateElement("CustomGraphs");
                doc.AppendChild(root);
            }

            doc.DocumentElement.AppendChild(doc.ImportNode(element, true));

            bool writeToDb = false;
            String fileName = this.getGraphsFileName(simRunId);
            if (fileName == null || fileName.CompareTo(String.Empty) == 0) { //file doesn't exist
                writeToDb = true;
                fileName = "CustomGraph_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            }

            try {
                doc.Save(Path.Combine(this.OutputPath, fileName + ".xml"));

            }
            catch (Exception) {
                return false;
            }

            //If the Exception was not thrown add the 'GraphFilename' parameter to this SimRun component
            if(writeToDb)
                this.UpdateParameters(simRunId, "GraphFilename", fileName, eParamParentType.Component);
            
            return true;
        }

        public bool DeleteCustomGraphFromFile(int simRunId, String graphName) {
            return this.deleteGraphFromFile(simRunId, graphName);            
        }

        ////////////////////////////////////////////////
        //                                            //
        // Retreave, Add or Delete RUN-TO-RUN GRAPHS  //
        //                                            //
        ////////////////////////////////////////////////

        public IXPathNavigable GetRunToRunGraphs(int simulationId) {
            return this.getGraphs(simulationId);
        }

        public bool AddRunToRunGraphToFile(int simulationId, XmlElement element) {

            XmlDocument doc = (XmlDocument)this.getGraphs(simulationId);

            if (doc.DocumentElement == null) { //empty document
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                doc.AppendChild(declaration);

                XmlElement root = doc.CreateElement("RunToRunGraphs");
                doc.AppendChild(root);
            }

            doc.DocumentElement.AppendChild(doc.ImportNode(element, true));

            bool writeToDb = false;
            String fileName = this.getGraphsFileName(simulationId);
            if (fileName == null || fileName.CompareTo(String.Empty) == 0) { //file doesn't exist
                writeToDb = true;
                fileName = "RunToRunGraph_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            }

            try {
                doc.Save(Path.Combine(this.OutputPath, fileName + ".xml"));

            }
            catch (Exception) {
                return false;
            }

            //If the Exception was not thrown add the 'GraphFilename' parameter to this SimRun component
            if (writeToDb)
                this.UpdateParameters(simulationId, "GraphFilename", fileName, eParamParentType.Component);

            return true;
        }

        public bool DeleteRunToRunGraphFromFile(int simulationId, String graphName) {
            return this.deleteGraphFromFile(simulationId, graphName);
        }

        #endregion

        #region Private Assessment Methods

        private String getGraphsFileName(int componentId) {

            //ComponentOptions compOptions = new ComponentOptions();
            //compOptions.CompParams = true;

            //IXPathNavigable iNavigator = base._GetXmlTree(new XmlDocument(), componentId, compOptions, null);

            //if (iNavigator == null)
            //    return null;
            
            //XPathNavigator navigator = iNavigator.CreateNavigator();
            //XPathNodeIterator filenameParam = navigator.SelectDescendants("Parameter", navigator.NamespaceURI, true);
            
            //String fileName = null;
            //foreach (XPathNavigator parameter in filenameParam) {
            //    String name = parameter.GetAttribute("Name", parameter.NamespaceURI);
            //    if (name.CompareTo("GraphFilename") == 0) {
            //        fileName = parameter.GetAttribute("Value", parameter.NamespaceURI);
            //        break;
            //    }
            //}

            //return fileName;
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.CompParams = true;

            IXPathNavigable iNavigator = base._GetComponentAndChildren(componentId, "Raw", true, compOptions);
            XPathNavigator navigator = iNavigator.CreateNavigator();

            String filename = null;
            XPathNavigator navFilename = navigator.SelectSingleNode("/Components/Component/Component/ComponentParameters/Parameter/Parameter[@displayedName='GraphFilename']");
            if (navFilename != null)
            {
                filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);
            }

            return filename;
        }

        private IXPathNavigable getGraphs(int componentId) {

            String fileName = this.getGraphsFileName(componentId);
            IXPathNavigable doc = null;

            try {
                doc = this.m_model.GetOutputXml(fileName);
            }
            catch (NullReferenceException) {
                logger.Debug("Graph output file for the Component ID " + componentId + " is NULL");
                doc = new XmlDocument();
            }
            catch (FileNotFoundException) {
                logger.Debug("Graph output file '" + fileName + "' for the Component ID " + 
                    componentId + " cannot be found");
                doc = new XmlDocument();
            }

            return doc;
        }


        private bool deleteGraphFromFile(int componentId, string graphName) {

            try {
                XmlDocument doc = (XmlDocument)this.getGraphs(componentId);
                if (doc == null) {
                    logger.Debug("Graph File for the component ID " + componentId + " doesn't exist");
                    return false;
                }

                XmlNode node = doc.SelectSingleNode(
                    String.Format(doc.DocumentElement.Name + @"/Graph[@name='{0}']", graphName));

                node.ParentNode.RemoveChild(node);
                doc.Save(Path.Combine(this.OutputPath, this.getGraphsFileName(componentId) + ".xml"));

                return true;

            }
            catch (Exception) {
                logger.Debug("Exception is caught trying to delete a graph " + graphName +
                    "from a Graph File for component ID " + componentId);
                throw;
            }
        }

        #endregion
    }
}
