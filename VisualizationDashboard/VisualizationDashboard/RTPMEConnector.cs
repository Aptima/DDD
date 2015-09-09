using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using ChartDirector;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Timers;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Resources;

using DashboardDataAccess;
using VisualizationDashboard.RTPMEServiceRef;
using VisualizationDashboard.Visualizations;
using Aptima.PMEngine.ServiceHosts;

namespace VisualizationDashboard
{
    public struct InstCurValueList
    {
        public List<string> instanceIDs;
        public List<double> dataValues;
    }

    public class RTPMEConnector
    {
        private PMEServiceClient rtpmeClient = null;
        private Thread workerThread;
        private MeasureUpdater measureUpdater;
        public PieChart testChart;
        public WinChartViewer testChart2;
        public Dictionary<int, DashboardVisualization> dashboardVisualizationMap;
        private ConfigDataModel configDataModel;
        const int UPDATEDELAY = 1000;
        private Dictionary<string, XmlWriter> measureInstXmlMap = new Dictionary<string, XmlWriter>();
        private Dictionary<string, StringBuilder> measureInstXmlStringMap = new Dictionary<string, StringBuilder>();
        private List<ThreadedServiceHost> Services;
        private Boolean rtPMERunning = false;

        private List<string> rtPMEMeasureFileNames = new List<string>() {"/Resources/CoVE Communication Measures.xml",
                                                                         "/Resources/CoVE Outcome Definitions.xml",
                                                                         "/Resources/CoVE Execution Measure Definitions.xml",
                                                                         "/Resources/CoVE Workload Trigger Definitions.xml"};

        private List<string> rtPMEMeasureFileIDs = new List<string>() {"CoVE_Communication",
                                                                       "CoVE_Outcome",
                                                                       "CoVE_Execution",
                                                                       "CoVE_Workload_Triggers"};

        private class MeasureUpdater
        {
            private PMEServiceClient rTPMEclient;
            private bool working = true;
            private RTPMEConnector rtPMEConnector;
            private DisplayControl displayControl;
            ManualResetEvent mExitEvent = new ManualResetEvent(false);
            System.Timers.Timer mTimer = new System.Timers.Timer(UPDATEDELAY);

            public MeasureUpdater(DisplayControl displayControl, RTPMEConnector rtPMEConnector, PMEServiceClient rTPMEclient)
            {
                this.rTPMEclient = rTPMEclient;
                this.rtPMEConnector = rtPMEConnector;
                this.displayControl = displayControl;
                mTimer.Elapsed += TimerEvent;
            }

            public void kill()
            {
                working = false;
            }

            void TimerEvent(object source, ElapsedEventArgs e)
            {
//                mTimer.Stop();
                mExitEvent.Set();
            }

            public void Update()
            {
                // Setup a timer
                mTimer.Start();

                while (working)
                {
                    // Obtain data from RT PME
                    Dictionary<string, VisualizationDashboard.RTPMEServiceRef.MeasureResult[]> measureResults = this.rTPMEclient.GetResults();

                    if (measureResults != null && measureResults.Count > 0)
                    {
                        // Store measure results
                        rtPMEConnector.CopyResultToDataMap(measureResults);

                        // Update Visualizations
                        displayControl.Dispatcher.Invoke(DispatcherPriority.Normal, displayControl.measureUpdateCallBack, null);
                    }

                    // Update Visualizations
                    //displayControl.Dispatcher.Invoke(DispatcherPriority.Normal, displayControl.measureUpdateCallBack, null);

                    // Wait for timer to fire
                    mExitEvent.Reset();
                    mExitEvent.WaitOne();
                }
            }
        }

        public RTPMEConnector()
        {
            // Create the config display data map
            dashboardVisualizationMap = new Dictionary<int, DashboardVisualization>();

            // Create Measurement Instance XML Object
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            foreach (string measureInstID in rtPMEMeasureFileIDs)
            {
                StringBuilder measureInstXmlString = new StringBuilder();
                XmlWriter measureInstXml = XmlWriter.Create(measureInstXmlString, settings);
                measureInstXml.WriteStartDocument();
                measureInstXml.WriteStartElement("HPML", "http://schemas.aptima.com/hpml");
                measureInstXml.WriteAttributeString("ID", measureInstID);
                measureInstXml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                measureInstXml.WriteAttributeString("xmlns", "xi", null, "http://www.w3.org/2001/XInclude");
                measureInstXml.WriteAttributeString("xsi", "NamespaceSchemaLocation", XmlSchema.InstanceNamespace, "HPML.xsd");
                measureInstXml.WriteStartElement("MeasurementInstances");
                measureInstXmlStringMap[measureInstID] = measureInstXmlString;
                measureInstXmlMap[measureInstID] = measureInstXml;
            }
            Services = new List<ThreadedServiceHost>();

        }

        ~RTPMEConnector()
        {
            if (measureUpdater != null)
            {
                measureUpdater.kill();
            }

            if (rtpmeClient != null)
            {
                rtpmeClient.TerminateSession();
                rtpmeClient = null;
            }
        }

        public bool ConnectToServer(DisplayControl displayControl)
        {
            foreach (string measureInstID in rtPMEMeasureFileIDs)
            {
                try
                {
                    StringBuilder measureInstXmlString = measureInstXmlStringMap[measureInstID];
                    XmlWriter measureInstXml = measureInstXmlMap[measureInstID];

                    // Complete Measurement Instances Data
                    measureInstXml.WriteEndElement();
                    measureInstXml.WriteEndElement();
                    measureInstXml.WriteEndDocument();
                    measureInstXml.Close();

                    string test = measureInstXmlString.ToString();

                    // Output list of undefined measures to debug stream
                    OutputUndefinedMeasureList(test);
                }
                catch (Exception)
                {
                }
            }

            // Write the string to a file.
            /*
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\AptimaProjects\\Cove\\out.xml");
                foreach (string xmlFileID in rtPMEMeasureFileIDs)
                {
                    StringBuilder measureInstXmlString = measureInstXmlStringMap[xmlFileID];
                    file.WriteLine(measureInstXmlString);
                    file.WriteLine("");
                }
                file.Close();
            }
            */

            // Start the RT PME Service
            StartServices();

            while (!rtPMERunning)
            {
                Thread.Sleep(2000);
            }

            rtpmeClient = new PMEServiceClient("net.tcp_IRTPMEService");

            rtpmeClient.InitializeSession();
            //client.InitializeRTI();

            // Load measurement definitions
            foreach (string resourceName in rtPMEMeasureFileNames)
            {
                try
                {
                    Uri uri = new Uri(resourceName, UriKind.Relative);
                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    StreamReader reader = new StreamReader(info.Stream);
                    string measureStr = reader.ReadToEnd();
                    rtpmeClient.LoadMeasurementFile(measureStr);
                }
                catch (Exception)
                {
                }
            }

            // Load measurement instances
            foreach (string measureInstID in rtPMEMeasureFileIDs)
            {
                try
                {
                    StringBuilder measureInstXmlString = measureInstXmlStringMap[measureInstID];

                    rtpmeClient.LoadMeasureInstance(measureInstXmlString.ToString());

                }
                catch (Exception)
                {
                }
            }

            measureUpdater = new MeasureUpdater(displayControl, this, rtpmeClient);

            // Start Updating thread
            ThreadStart threadStart = new ThreadStart(measureUpdater.Update);
            workerThread = new Thread(threadStart);
            workerThread.Start();

            return true;
        }

        public bool DisconnectFromServer()
        {
            if (measureUpdater != null)
            {
                measureUpdater.kill();
            }

            if (rtpmeClient != null)
            {
                rtpmeClient.TerminateSession();
                rtpmeClient = null;
            }

            // Stop the RT PME Service
            StopServices();

            return true;

        }

        public void DetermineVisDataReq(ConfigDataModel configDataModel)
        {
            ObservableCollection<ConfigDisplay> configDisplays = null;

            this.configDataModel = configDataModel;

            // Clear the dashboard visualization map
            dashboardVisualizationMap.Clear();

            // Load the config displays
            configDisplays = configDataModel.LoadConfigDisplays();

            // Loop through all of the configDisplays
            foreach (ConfigDisplay configDisplay in configDisplays)
            {
                DashboardVisualization dashboardVisualization = null;

                // Fill determine the data needed based on the type of visualization
                if (configDisplay.Display.Name.CompareTo("Multi Pie Chart") == 0)
                {
                    // Allocate a new MutiPie Chart Visualization
                    dashboardVisualization = new MultiPieChart(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Stacked Histogram") == 0)
                {
                    // Allocate a new Stacked Histogram Visualization
                    dashboardVisualization = new StackedHistogram(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Bubbles") == 0)
                {
                    // Allocate a new Bubbles Visualization
                    dashboardVisualization = new Bubbles(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Barcode") == 0)
                {
                    // Allocate a new Barcode Visualization
                    dashboardVisualization = new Barcode(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Color Wheel") == 0)
                {
                    // Allocate a new ColorWheel Visualization
                    dashboardVisualization = new ColorWheel(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Multi Level Bow Tie") == 0)
                {
                    // Allocate a new ColorWheel Visualization
                    dashboardVisualization = new MultiLevelBowTie(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Heatmap") == 0)
                {
                    // Allocate a new ColorWheel Visualization
                    dashboardVisualization = new Heatmap(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Radar") == 0)
                {
                    // Allocate a new ColorWheel Visualization
                    dashboardVisualization = new Radar(configDisplay, configDataModel, measureInstXmlMap);
                }
                else if (configDisplay.Display.Name.CompareTo("Star Spider") == 0)
                {
                    // Allocate a new ColorWheel Visualization
                    dashboardVisualization = new StarSpider(configDisplay, configDataModel, measureInstXmlMap);
                }
                else
                {
                    continue;
                }

                // Add this list to the configDisplay data map
                dashboardVisualizationMap.Add(configDisplay.ConfigDisplayID, dashboardVisualization);
            }
        }

        public void CopyResultToDataMap(Dictionary<string,VisualizationDashboard.RTPMEServiceRef.MeasureResult[]> measureResults)
        {
            foreach (DashboardVisualization dashboardVisualization in dashboardVisualizationMap.Values)
            {
                foreach (object rtPMEData in dashboardVisualization.rtPMEData)
                {
                    if (rtPMEData is InstCurValueList)
                    {
                        InstCurValueList instCurValueList = (InstCurValueList) rtPMEData;

                        // Obtain data value from measure results dictionary
                        for (int i = 0; i < instCurValueList.dataValues.Count; i++)
                        {
                            if ((instCurValueList.instanceIDs[i] != null) && (measureResults.ContainsKey(instCurValueList.instanceIDs[i])))
                            {
                                if (measureResults[instCurValueList.instanceIDs[i]].Length == 1)
                                {
                                    string resultStr = measureResults[instCurValueList.instanceIDs[i]][0].MeasurementValue;

                                    if (resultStr == null)
                                    {
                                        instCurValueList.dataValues[i] = 0.0;
                                        continue;
                                    }

                                    if ((resultStr.CompareTo("True") == 0) || (resultStr.CompareTo("true") == 0))
                                    {
                                        instCurValueList.dataValues[i] = 1.0;
                                    }
                                    else if ((resultStr.CompareTo("False") == 0) || (resultStr.CompareTo("false") == 0))
                                    {
                                        instCurValueList.dataValues[i] = 0.0;
                                    }
                                    else if (resultStr.Length > 0)
                                    {
                                        instCurValueList.dataValues[i] = double.Parse(resultStr);
                                    }
                                    else
                                    {
                                        instCurValueList.dataValues[i] = 0.0;
                                    }
                                }
                                else
                                {
                                    instCurValueList.dataValues[i] = 0.0;
                                }
                            }
                            else
                            {
                                instCurValueList.dataValues[i] = 0.0;
                            }
                        }
                    }
                }
            }

        }

        private void OutputUndefinedMeasureList(string xmlString)
        {
            // Get list of defined measures from Cove measures resource file
            List<string> measureIDs = new List<string>();
            List<string> missingIDs = new List<string>();

            foreach (string resourceName in rtPMEMeasureFileNames)
            {
                try
                {
                    Uri uri = new Uri(resourceName, UriKind.Relative);
                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    using (XmlReader reader = XmlReader.Create(info.Stream))
                    {
                        if (reader.ReadToFollowing("Measurement"))
                        {
                            do
                            {
                                string id = reader.GetAttribute("ID");
                                if (id != null)
                                {
                                    measureIDs.Add(id);
                                }
                            } while (reader.ReadToNextSibling("Measurement"));
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            // Output a list of undefine measures to the debug stream
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                if (reader.ReadToFollowing("MeasurementInstance"))
                {
                    do
                    {
                        string id = reader.GetAttribute("InstanceOf");
                        if (id != null)
                        {
                            // Check to see if measurement is defined in library
                            if ((!measureIDs.Contains(id)) && (!missingIDs.Contains(id)))
                            {
                                System.Diagnostics.Trace.WriteLine(id + " is not contained in Cove measurement library");
                                missingIDs.Add(id);
                            }
                        }
                    } while (reader.ReadToNextSibling("MeasurementInstance"));
                }
            }
        }

        private void StartServices()
        {
            try
            {
                Services.Add(new PMEngineHost());
                Services[0].StatusChanged += EngineServiceStatusChanged;
                Services[0].StartService();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "StarServices Exception");
                //this.Dispatcher.Invoke(DispatcherPriority.Normal, closeCallback);
            }
        }

        private void StopServices()
        {
            foreach (ThreadedServiceHost service in Services)
            {
                service.StopService();
            }
        }

        private void EngineServiceStatusChanged(Object sender, EventArgs eventArgs)
        {
            ThreadedServiceHost serviceHost = (ThreadedServiceHost)sender;

            switch (serviceHost.Status)
            {
                case ServiceStatus.Stopped:
                    //this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, string.Empty, EngineStatusElipst, Brushes.Red);
                    rtPMERunning = false;
                    break;
                case ServiceStatus.Starting:
                    //this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, "Starting PM Engine Service", EngineStatusElipst, Brushes.Yellow);
                    rtPMERunning = false;
                    break;
                case ServiceStatus.Running:
                    //this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, string.Empty, EngineStatusElipst, Brushes.Green);
                    try
                    {
                        Services.Add(new PMEngineDiagnosticsHost());
                        Services[1].StatusChanged += DiagnosticsServiceStatusChanged;
                        Services[1].StartService();
                        rtPMERunning = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "StatusChanged Exception");
                        //this.Dispatcher.Invoke(DispatcherPriority.Normal, closeCallback);
                    }
                    break;
                default:
                    break;
            }
        }

        private void DiagnosticsServiceStatusChanged(Object sender, EventArgs eventArgs)
        {
            ThreadedServiceHost serviceHost = (ThreadedServiceHost)sender;

            switch (serviceHost.Status)
            {
                case ServiceStatus.Stopped:
                    //this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, string.Empty, DiagnosicsStatusEllipse, Brushes.Red);
                    break;
                case ServiceStatus.Starting:
                    //this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, "Starting Engine Diagnostics Service", DiagnosicsStatusEllipse, Brushes.Yellow);
                    break;
                case ServiceStatus.Running:
                    /*
                    rtpmeClient = new PMEServiceClient("net.tcp_IRTPMEService");
                    diagnosticsClient = new DiagnosticsServiceClient("net.tcp_IDiagnosticsService");
                    rtpmeClient.InitializeSession();
                    measureUpdateCallBack = new UpdateCallback(this.UpdateMeasures);
                    payloadProcInfoCallback = new PayloadProcInfoCallback(this.UpdatePayloadProcessorInfo);
                    connectorInfoCallback = new ConnectorInfoCallback(this.UpdateConnectorInfo);
                    displayUpdater = new DisplayUpdater(this, rtpmeClient, diagnosticsClient);
                    displayUpdater.StartWorking();
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, updateServiceStatusCallback, string.Empty, DiagnosicsStatusEllipse, Brushes.Green);*/
                    break;
                default:
                    break;
            }
        }

        static string GetXmlString(string strFile)
        {
            // Load the xml file into XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Now create StringWriter object to get data from xml document.
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xw);
            return sw.ToString();
        }

    }

    public class FactorInfo
    {
        private string factorName = null;

        public string FactorName
        {
            get { return factorName; }
            set { factorName = value; }
        }
        private string levelName = null;

        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        private RTPMEType rtPMEType = RTPMEType.Measure;

        public RTPMEType RtPMEType
        {
            get { return rtPMEType; }
            set { rtPMEType = value; }
        }

        private int measureID = 0;

        public int MeasureID
        {
            get { return measureID; }
            set { measureID = value; }
        }

    }
}
