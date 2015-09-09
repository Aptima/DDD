using System;
using System.Text;
using AME.Model;
using AME.MeasureModels;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Reflection;
using log4net;

namespace AME.Controllers
{

	public class MeasuresController : Controller {

		private static readonly ILog logger = LogManager.GetLogger(typeof(MeasuresController));
        private MeasureGraphConverter _measureGraphConverter = null;

		public enum GraphCategory {
			SINGLEMEASURE = 0,
			RUNTORUN = 1
		}

        #region Constructors

        public MeasuresController(AME.Model.Model model, String configType)
            : base(model, configType)
        {
        }//constructor

        #endregion  //constructors


        #region Methods

        #region Private methods

        private void GetEngineModelTypes(
            int engineId,
            out string engineModelType,
            out string engineModelBaseType)
        {
            DataRow drEngine = base._GetComponent(engineId);
            engineModelType = (string)drEngine["type"];
            engineModelBaseType = base._GetBaseComponentType(engineModelType);
        }//GetEngineModelBaseType

        #endregion  //private methods

        #region Public Methods

        public void Run(int engineId, int runId)
        {
            try
            {
                string sEngineModelType = null;
                string sEngineModelBaseType = null;
                string sInputFileName = null;
                string sOutputFileName = null;
                string sGraphInputFileName = null;
                int iMeasureId = -1;
                string sRunCategory = null;
                Dictionary<string, IXPathNavigable> cMeasureOutputs = new Dictionary<string, IXPathNavigable>();

                //Getting Run type
                string sRunType = base._GetType(runId);

                //Getting Engine Model & Base type
                this.GetEngineModelTypes(engineId, out sEngineModelType, out sEngineModelBaseType);

                #region A. Reading measure's class names

                //[className, measureInfo]
                Dictionary<string, MeasureInfo> lMeasureInfo = new Dictionary<string, MeasureInfo>();
                IXPathNavigable componentRun = base.m_model.GetComponent(sRunType);
                if (componentRun == null)
                    return;

                XPathNavigator navComponentRun = componentRun.CreateNavigator();

                XPathNodeIterator measureClassIterator = navComponentRun.Select(String.Format("SubComponents/Component[@type='{0}']", "Measure"));
                foreach (XPathNavigator measureClass in measureClassIterator)
                {
                    String sMeasureClassName = measureClass.GetAttribute("name", navComponentRun.NamespaceURI);

                    MeasureInfo measureInfo = new MeasureInfo(sMeasureClassName);
                    measureInfo.DisplayName = measureClass.GetAttribute("displayName", navComponentRun.NamespaceURI);
                    measureInfo.GraphType = measureClass.GetAttribute("graphType", navComponentRun.NamespaceURI); ;

                    lMeasureInfo.Add(measureInfo.ClassName, measureInfo);
                }

                #endregion

                #region B. Instantiating all measure classes

                Dictionary<string, IMeasureModel> lMeasureModels = new Dictionary<string, IMeasureModel>();
                // Get the entry assembly which should always be the project name
                string projectAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
                string typeString = ".";
                switch (sEngineModelBaseType)
                {
                    case ("Simulation"):
                        projectAssemblyName = sEngineModelType; // Need the namespace which happens to be the same as the ModelType...
                        typeString = string.Format(".Measures.");
                        Assembly.Load(projectAssemblyName);
                        break;

                    case ("Optimization"):
                        typeString = ".Optimizations.Measures.";
                        break;
                }
                foreach (KeyValuePair<string, MeasureInfo> pair in lMeasureInfo)
                {
                    MeasureInfo measureInfo = pair.Value;
                    string measureModelTypeName = projectAssemblyName + typeString + measureInfo.ClassName;

                    ObjectHandle objMeasureModel = Activator.CreateInstance(projectAssemblyName, measureModelTypeName);
                    IMeasureModel measureModel = (IMeasureModel)objMeasureModel.Unwrap();
                    measureModel.DisplayName = measureInfo.DisplayName;
                    measureModel.GraphType = measureInfo.GraphType;
                    lMeasureModels[measureInfo.ClassName] = measureModel;
                }

                #endregion

                #region C. Reading measure's input file name

                DataTable dtMeasures = base._GetChildComponents(runId, "Measure", true);

                if (dtMeasures.Rows.Count > 0)
                {
                    DataRow drMeasure = dtMeasures.Rows[0];
                    iMeasureId = base._GetID(drMeasure).Value;
                    sInputFileName = base._GetParameterValue(iMeasureId, "Measure Parameters.Input Filename");
                }

                if (iMeasureId == -1)
                {
                    return;
                }

                sOutputFileName = sInputFileName.Replace("Input", "Output");
                sGraphInputFileName = sInputFileName.Replace("Input", "GraphInput");
                string sFolderPath = this.OutputPath;
                string pInputFilePath = Path.Combine(sFolderPath, sInputFileName + ".xml");
                string pOutputFilePath = Path.Combine(sFolderPath, sOutputFileName + ".xml");
                string pGraphInputFilePath = Path.Combine(sFolderPath, sGraphInputFileName + ".xml");

                #endregion

                #region D. Reading measure's Input File

                FileInfo xmlInputFile = new FileInfo(pInputFilePath);
                XmlDocument xmlInputDoc = new XmlDocument();
                xmlInputDoc.Load(xmlInputFile.OpenText());

                //Reading Run Category
                XmlNode component = xmlInputDoc.SelectSingleNode("/Component");
                sRunCategory = component.Attributes["name"].Value;

                IXPathNavigable navComponent = component.CreateNavigator();

                #endregion

                #region E. Calling Measure Engines' run by passing parameters

                foreach (KeyValuePair<string, IMeasureModel> pair in lMeasureModels)
                {
                    IMeasureModel measureModel = pair.Value;
                    measureModel.MeasureInputXml = navComponent;
                    bool bSuccess = measureModel.Start();
                    IXPathNavigable outParameter = measureModel.MeasureOutputXml;
                    cMeasureOutputs.Add(pair.Key, outParameter);
                }

                #endregion

                #region F. Creating Measure Output File, Storing it & Updating Parameter

                XmlDocument measureOutputXmlFile = new XmlDocument();
                XmlDeclaration mDeclaration = measureOutputXmlFile.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                measureOutputXmlFile.AppendChild(mDeclaration);

                XmlElement xeComponent = measureOutputXmlFile.CreateElement("Component");
                measureOutputXmlFile.AppendChild(xeComponent);

                // Added by CK for Report Card Threshhold Values
                IXPathNavigable iNavSimulationParameters = this.GetParametersForComponent(engineId);
                XPathNavigator navSimulationParameters = iNavSimulationParameters.CreateNavigator();
                XmlNode nodeSimulation = ((IHasXmlNode)navSimulationParameters).GetNode();
                nodeSimulation = measureOutputXmlFile.ImportNode(nodeSimulation, true);
                xeComponent.AppendChild(nodeSimulation);  
                /////////////////////////////////////////////////

                XmlAttribute xaComp_type = measureOutputXmlFile.CreateAttribute("type");
                xaComp_type.Value = "Measure";
                xeComponent.Attributes.Append(xaComp_type);

                XmlAttribute xaComp_name = measureOutputXmlFile.CreateAttribute("name");
                xaComp_name.Value = sRunCategory;
                xeComponent.Attributes.Append(xaComp_name);

                XmlElement xeMeasures = measureOutputXmlFile.CreateElement("Measures");
                xeComponent.AppendChild(xeMeasures);

                //Preparing XML file with all of the measure outputs.
                foreach (KeyValuePair<string, IXPathNavigable> pair in cMeasureOutputs)
                {
                    string measureName = pair.Key;
                    IXPathNavigable measureOut = pair.Value;
                    if (measureOut != null)
                    {
                        XPathNavigator navMeasureOut = measureOut.CreateNavigator();

                        XPathNodeIterator measureIterator = navMeasureOut.Select("/Measure");
                        measureIterator.MoveNext();
                        XmlNode origNode = ((IHasXmlNode)measureIterator.Current).GetNode();
                        xeMeasures.InnerXml += string.Copy(origNode.OuterXml);
                    }
                }//foreach measure output

                measureOutputXmlFile.Save(pOutputFilePath);
                //storing measure output file's name.
                this.UpdateParameters(iMeasureId, "Measure Parameters.Output Filename", sOutputFileName, eParamParentType.Component);

                #endregion

                #region G. Creating Measure Graph Input File, Storing it & Updating Parameter

                XPathNavigator navMeasureOutput = measureOutputXmlFile.CreateNavigator();
                this._measureGraphConverter = new MeasureGraphConverter(navMeasureOutput);

                IXPathNavigable navGraphInput = this._measureGraphConverter.Transform();

                if (navGraphInput != null)
                {
                    XmlDocument measureGraphInXmlFile = (XmlDocument)navGraphInput;
                    measureGraphInXmlFile.Save(pGraphInputFilePath);
                    //storing measure Graph Input file's name.
                    this.UpdateParameters(iMeasureId, "Measure Parameters.Graph Input Filename", sGraphInputFileName, eParamParentType.Component);
                }

                //setting it back to null, as we need a new instance of it for new input.
                this._measureGraphConverter = null;

                #endregion

            }//try
            catch 
            {
            }
        }//Run

		public List<MeasureInfo> getAllGraphableMeasures(int runId) {

			List<MeasureInfo> measures = new List<MeasureInfo>();

				//string sEngineModelType = null;
				//string sEngineModelBaseType = null;

                //Getting Run type
                string runType = base._GetType(runId);

                //Getting Engine Model & Base type
                //this.GetEngineModelTypes(engineId, out sEngineModelType, out sEngineModelBaseType);

                IXPathNavigable componentRun = base.m_model.GetComponent(runType);
                if (componentRun == null)
                    return measures;

                XPathNavigator navComponentRun = componentRun.CreateNavigator();

                XPathNodeIterator measureClassIterator = navComponentRun.Select(String.Format("SubComponents/Component[@type='{0}' and @graphType!='']", "Measure"));
				foreach (XPathNavigator measureClass in measureClassIterator) {
					String measureClassName = measureClass.GetAttribute("name", navComponentRun.NamespaceURI);

					MeasureInfo measureInfo = new MeasureInfo(measureClassName);
					measureInfo.DisplayName = measureClass.GetAttribute("displayName", navComponentRun.NamespaceURI);
					measureInfo.GraphType = measureClass.GetAttribute("graphType", navComponentRun.NamespaceURI); ;

					measures.Add(measureInfo);
				}

				return measures;
		}

		public IXPathNavigable getMeasuredData(int id) {

			ComponentOptions compOptions = new ComponentOptions();
			compOptions.CompParams = true;

			IXPathNavigable iNavigator = base._GetComponentAndChildren(id, "Measure", true, compOptions);
			XPathNavigator navigator = iNavigator.CreateNavigator();

			XPathNavigator navFilename = navigator.SelectSingleNode("/Components/Component/Component/ComponentParameters/Parameter/Parameter[@displayedName='Graph Input Filename']");
			String filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);

			try {
				return this.m_model.GetOutputXml(filename);
			}
			catch (FileNotFoundException e) {
				logger.Debug("The measured data for the Run ID " + id + " is not found.  FileNotFoundException: " + e.Message);
				return null;
			}
		}

		private int getMeasureId(int runId) {

			int measureId = -1;

			DataTable measureTable = base._GetChildComponents(runId, "Measure", true);
			if (measureTable.Rows.Count > 0) {
				DataRow row = measureTable.Rows[0];
				measureId = base._GetID(row).Value;
			}
			return measureId;
		}

		private String getMeasureGraphFilename(int runId, GraphCategory category) {
			
			//Get Measures graph filename:
			ComponentOptions compOptions = new ComponentOptions();
			compOptions.CompParams = true;
			compOptions.LevelDown = 2;

			String filename = null;
			if (category == GraphCategory.SINGLEMEASURE) {

				IXPathNavigable iNavigator = base._GetComponentAndChildren(runId, "Measure", true, compOptions);
				XPathNavigator navigator = iNavigator.CreateNavigator();

				XPathNavigator navFilename = navigator.SelectSingleNode("/Components/Component/Component[@BaseType='Measure']/ComponentParameters/Parameter/Parameter[@displayedName='Graph Output Filename']");
				if (navFilename != null) {
					filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);
				}
			}
			else if (category == GraphCategory.RUNTORUN) {
				//TODO: add the measure Run-To-Run functionality
			}
			return filename;
		}

		public IXPathNavigable getGraphs(int runId, GraphCategory category) {

			String fileName = this.getMeasureGraphFilename(runId, category);

			IXPathNavigable doc = null;
			try {
				doc = this.m_model.GetOutputXml(fileName);
			}
			catch (NullReferenceException) {
				logger.Debug("Measure Graph output file for the Component ID " + runId + 
					", Graph Category " + category.ToString() + " is NULL");
				doc = new XmlDocument();
			}
			catch (FileNotFoundException) {
				logger.Debug("Measure Graph output file '" + fileName + "' for the Component ID " + 
		            runId + " cannot be found");
				doc = new XmlDocument();
			}

			return doc;
		}

		public bool addMeasureGraphToFile(int runId, XmlElement element, GraphCategory category) {

			XmlDocument doc = (XmlDocument)this.getGraphs(runId, category);

			if (doc.DocumentElement == null) { //empty document
				XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
				doc.AppendChild(declaration);

				XmlElement root = null;
				if (category == GraphCategory.SINGLEMEASURE) {
					root = doc.CreateElement("MeasureGraphs");
				}
				else if (category == GraphCategory.RUNTORUN) {
					//TODO: measure Run-To-Run implementation
					return false;
				}

				doc.AppendChild(root);
			}

			doc.DocumentElement.AppendChild(doc.ImportNode(element, true));

			bool writeToDb = false;
			String fileName = this.getMeasureGraphFilename(runId, category);
			if (fileName == null || fileName.CompareTo(String.Empty) == 0) { //file doesn't exist
				writeToDb = true;

				if (category == GraphCategory.SINGLEMEASURE) {
					fileName = "MeasureGraph_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
				}
				else if (category == GraphCategory.RUNTORUN) {
					//TODO: measure Run-To-Run implementation
					return false;
				}
			}

			try {
				doc.Save(Path.Combine(this.OutputPath, fileName + ".xml"));
			}
			catch (Exception e) {
				logger.Debug(e.Message);
				return false;
			}

			//If the Exception was not thrown add the 'GraphFilename' parameter to this SimRun component
			if (writeToDb)  {
				int measureId = this.getMeasureId(runId);

				if (category == GraphCategory.SINGLEMEASURE) {
					this.UpdateParameters(measureId, "Measure Parameters.Graph Output Filename", 
						fileName, eParamParentType.Component);
				}
				else if (category == GraphCategory.RUNTORUN) {
					//TODO: measure Run-To-Run implementation
					return false;
				}
			}
			return true;
		}

		public bool deleteGraphFromFile(int runId, String graphName, GraphCategory category) {

			try {
				XmlDocument doc = (XmlDocument)this.getGraphs(runId, category);
				if (doc == null) {
					logger.Debug("Measure Graph File for the Run ID " + runId + " doesn't exist");
					return false;
				}

				XmlNode node = doc.SelectSingleNode(
					String.Format(doc.DocumentElement.Name + @"/Graph[@name='{0}']", graphName));

				node.ParentNode.RemoveChild(node);
				doc.Save(Path.Combine(this.OutputPath, this.getMeasureGraphFilename(runId, category) + ".xml"));

				return true;

			}
			catch (Exception) {
				logger.Debug("Exception is caught trying to delete a measure graph " + graphName +
                    "from a Graph Output File for Run ID " + runId);
				return false;
			}
		}

        #endregion  //public methods

        #endregion  //methods


    }//MeasuresController class
}//namespace AME.Controllers
