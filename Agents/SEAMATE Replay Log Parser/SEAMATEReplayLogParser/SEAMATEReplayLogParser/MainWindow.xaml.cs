using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.DDDAgentFramework;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SEAMATEReplayLogParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double MaxProgressValue = 100.0;
        private delegate void UpdateButtonDelegate(System.Windows.Controls.Button b, bool isEnabled, String text);
        private delegate void UpdateProgressDelegate(double val, String text);
        private delegate void UpdateOutputDelegate(String text);
        private bool _isParsing = false;
        private String _selectedDirectory = "";
        private Thread _parseThread;
        public MainWindow()
        {
            InitializeComponent();
            progressBar.Maximum = MaxProgressValue;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Locate the directory which houses your .ddd replay files";
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _selectedDirectory = dlg.SelectedPath;
                lblDirectory.Content = _selectedDirectory;
            }
        }

        private void buttonStartParsing_Click(object sender, RoutedEventArgs e)
        {
            if (_isParsing)
            {
                //stop
                if (_parseThread != null)
                {
                    if (_parseThread.IsAlive)
                    {
                        _parseThread.Abort();
                    }
                }
                _isParsing = false;
            }
            else
            {
                //start
                _isParsing = true;
                _parseThread = new Thread(new ThreadStart(ParseDirectory));
                _parseThread.Start();
            }
        }

        private void ParseDirectory()
        {
            _isParsing = true;
            String dir = _selectedDirectory;
            String[] files = Directory.GetFiles(dir, "*.ddd", SearchOption.TopDirectoryOnly);
            int countFiles = files.Length;
            int counter = 1;
            UpdateButton(buttonStartParsing, false, "Parsing...");
            foreach (String s in files)
            {
                AppendOutput("Parsing file" + s);
                UpdateProgressBar(counter++ / (double)countFiles * MaxProgressValue, "Processing file '" + s + "'");
                //Thread.Sleep(500);
                ParseFileToOutput(s, GetMeasureResultFilePath(s));
                AppendOutput("Parsing complete");
            }
            _isParsing = false;
            AppendOutput("******ALL PARSING COMPLETED******");
            UpdateButton(buttonStartParsing, true, "Parse Directory");
        }

        private void UpdateButton(System.Windows.Controls.Button b, bool isEnabled, string text)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                b.IsEnabled = isEnabled;
                b.Content = text;
            }
            else
            {
                Dispatcher.BeginInvoke(new UpdateButtonDelegate(UpdateButton), b, isEnabled, text);
            }
        }

        private void AppendOutput(string text)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                textBoxOutput.Text = String.Format("{0}\r\n{1}", textBoxOutput.Text,text);
            }
            else
            {
                Dispatcher.BeginInvoke(new UpdateOutputDelegate(AppendOutput), text);
            }
        }//

        private string GetMeasureResultFilePath(String inputFilePath)
        {
            //File f = new File(inputFilePath);
            String[] parts = inputFilePath.Split('\\');
            String fileName = parts[parts.Length - 1];
            fileName = fileName.Replace(".ddd", ".csv");
            return String.Format("{0}\\Measure Results\\{1}", Directory.GetParent(inputFilePath).FullName, fileName);
        }

        private void UpdateProgressBar(double val, String text)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                labelCurrentProgress.Content = text;
                progressBar.Value = val;
            }
            else
            {
                Dispatcher.BeginInvoke(new UpdateProgressDelegate(UpdateProgressBar), val, text);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_parseThread != null)
            {
                if (_parseThread.IsAlive)
                {
                    _parseThread.Abort();
                }
            }
            _isParsing = false;
        }

        private int GetFindTime(String objectId, String dmId, int startTime, int endTime)
        {
            if (findTimes.ContainsKey(objectId))
                foreach(int i in findTimes[objectId])
                {
                    if (i >= startTime && i <= endTime)
                        return i;
                }
               //TODO make sure it was THIS DM who found it

            return -100;
        }
        private int GetFixTime(String objectId, String dmId, int startTime, int endTime)
        {
            if (fixTimes.ContainsKey(objectId))
                foreach (int i in fixTimes[objectId])
                {
                    if (i >= startTime && i <= endTime)
                        return i;
                }//TODO make sure it was THIS DM who found it

            return -100;
        }
        private int GetTrackTime(String objectId, String dmId, int startTime, int endTime)
        {
            if (trackTimes.ContainsKey(objectId))
                foreach (int i in trackTimes[objectId])
                {
                    if (i >= startTime && i <= endTime)
                        return i;
                } //TODO make sure it was THIS DM who found it

            return -100;
        }
        private int GetTargetTime(String objectId, String dmId, int startTime, int endTime)
        {
            if (targetTimes.ContainsKey(objectId))
                foreach (int i in targetTimes[objectId])
                {
                    if (i >= startTime && i <= endTime)
                        return i;
                } //TODO make sure it was THIS DM who found it

            return -100;

        }
        private String GetAssignedDM(String stimObjId)
        {
            if (objectsAssignedTo.ContainsKey(stimObjId))
            {
                foreach (String s in objectsAssignedTo[stimObjId])
                {
                    if (s != String.Empty)
                        return s;
                }
            }

            return "";
        }

        private void UpdateFindTime(String objectID, int time)
        {
            if (findTimes.ContainsKey(objectID))
                findTimes[objectID].Add(time);
            else
            {
                findTimes.Add(objectID, new List<int>());
                findTimes[objectID].Add(time);
            }

        }
        private void UpdateFixTime(String objectID, int time, String dm, String classification)
        {
            if (!fixTimes.ContainsKey(objectID))
                fixTimes.Add(objectID, new List<int>());
            
                
            fixTimes[objectID].Add(time);
            if (!fixedAs.ContainsKey(objectID))
                fixedAs.Add(objectID, new Dictionary<string, string>());
            if (!fixedAs[objectID].ContainsKey(dm))
            {
                fixedAs[objectID].Add(dm, classification);
            }
            else
            {
                fixedAs[objectID][dm] = classification;
            }
        }
        private String GetFixedAs(String objectId, String dmId)
        {
            if (fixedAs.ContainsKey(objectId) && fixedAs[objectId].ContainsKey(dmId))
                return fixedAs[objectId][dmId];

            return "";
        }
        private void UpdateTrackTime(String objectID, int time)
        {
            if (!trackTimes.ContainsKey(objectID))
                trackTimes.Add(objectID, new List<int>());


            trackTimes[objectID].Add(time);

        }
        private void UpdateTargetTime(String objectID, int time)
        {
            if (!targetTimes.ContainsKey(objectID))
                targetTimes.Add(objectID, new List<int>());


            targetTimes[objectID].Add(time);
        }
        private bool ObjectAssignedToDM(String objectId, String dmId)
        {
            if (objectsAssignedTo != null && objectsAssignedTo.ContainsKey(objectId))
            {
                return objectsAssignedTo[objectId].Contains(dmId);
            }

            return false;
        }
        //adds event to in order, as most are in order, will add from the rear
        private void AddToList(List<SimulationEvent> eventList, SimulationEvent eventToAdd)
        {
            for (int i = eventList.Count - 1; i > 0; i--)
            {
                if (((IntegerValue)eventList[i].parameters["Time"]).value <= ((IntegerValue)eventToAdd.parameters["Time"]).value)
                {
                    eventList.Add(eventToAdd);
                    return;
                }
                if (((IntegerValue)eventList[i].parameters["Time"]).value >= ((IntegerValue)eventToAdd.parameters["Time"]).value &&
                    ((IntegerValue)eventToAdd.parameters["Time"]).value >= ((IntegerValue)eventList[i - 1].parameters["Time"]).value)
                {
                    eventList.Insert(i, eventToAdd);
                    return;
                }
            }
            eventList.Insert(0, eventToAdd);
        }
        private int currentTime = 0;
        private int lastItemTime = -1;
        private Dictionary<String, SimObject> objects;
        private Dictionary<String, List<int>> findTimes;
        private Dictionary<String, List<int>> fixTimes;
        private Dictionary<String, List<int>> trackTimes;
        private Dictionary<String, List<int>> targetTimes;
        private Dictionary<String, Dictionary<String, String>> fixedAs; //object, dm, fixed as
        private Dictionary<String, List<String>> objectsAssignedTo;
        private Dictionary<String, Dictionary<Int32, ItemDefinition>> dmAssignedItems; //DM, [time,item info]
        private Dictionary<Int32, List<ItemDefinition>> itemsByTime; //item start time, item info; shares same reference to item definition
        private Dictionary<String, String> personMap;
        private Dictionary<String, String> teamMap;
        private List<RowOutput> rows;
        private int bamsAvailableTracks = 6;
        private int firescoutAvailableTracks = 6;
        String scenDate, scenarioStart, scenarioEnd;
        private Dictionary<String, List<int>> selfDefenses;
        private Dictionary<String, String> iffValues;
        private List<String> suspectObjects;
        private Dictionary<String, double> objectSpeeds;
        private Dictionary<String, double> objectMaxSpeeds;
        private Dictionary<String, String> objectLastAttackItems;

        private void ParseFileToOutput(String replayFilePath, String outputFilePath)
        {
            currentTime = 0;
            lastItemTime = -1;
            objects = new Dictionary<string, SimObject>();
            findTimes = new Dictionary<string, List<int>>();
            fixTimes = new Dictionary<string, List<int>>();
            trackTimes = new Dictionary<string, List<int>>();
            targetTimes = new Dictionary<string, List<int>>();
            objectsAssignedTo = new Dictionary<string, List<string>>();
            dmAssignedItems = new Dictionary<string, Dictionary<int, ItemDefinition>>();
            itemsByTime = new Dictionary<Int32, List<ItemDefinition>>();
            objectLastAttackItems = new Dictionary<string, String>();
            objectSpeeds = new Dictionary<string,double>();
            objectMaxSpeeds = new Dictionary<string, double>();
            rows = new List<RowOutput>();
            personMap = new Dictionary<string, string>();
            teamMap = new Dictionary<string, string>();
            scenDate = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
            scenarioStart = String.Format("{0:hh:mm tt}", DateTime.Now);
            personMap.Add("BAMS DM", ".");
            personMap.Add("Firescout DM", ".");
            teamMap.Add("BAMS DM", ".");
            teamMap.Add("Firescout DM", ".");
            selfDefenses = new Dictionary<string, List<int>>();
            fixedAs = new Dictionary<string, Dictionary<string, string>>();
            iffValues = new Dictionary<string, string>();
            suspectObjects = new List<string>();

            bool hasViewProEvents = false;//unfortunately not all replays will have the VPAttUpdate events :-\
            //get scenario name from file name

            bool setBackItemTimes = false;
            bool hasReceivedStimuli = false;

            //parse scenario line by line
            int counter = 0;
            ItemDefinition def;
            string line;
            String id;
            SimObject o;
            String dm, itemId, action;
            int t;
            List<String> outputRows = new List<string>();
            outputRows.Add(RowOutput.ToOutputHeader());
            // Read the file and display it line by line.
            StreamReader file = new StreamReader(replayFilePath);
            List<SimulationEvent> inOrderEvents = new List<SimulationEvent>();
            while ((line = file.ReadLine()) != null)
            {
                SimulationEvent ev = null;
                try
                {
                    ev = SimulationEventFactory.XMLDeserialize(line);
                    if (ev.eventType == "SEAMATE_StimulusSent")
                    {
                        if (!hasReceivedStimuli)
                        {
                            if (((IntegerValue)ev.parameters["Time"]).value > 50000)
                            {
                                ((IntegerValue)ev.parameters["Time"]).value = Math.Max(1000, ((IntegerValue)ev.parameters["Time"]).value - 60000); //prevent items that start at min=1
                                setBackItemTimes = true;
                            }
                            hasReceivedStimuli = true;
                        }
                        else
                        {
                            if (setBackItemTimes && ((StringValue)ev["StimulusType"]).value != "Attack")
                                ((IntegerValue)ev.parameters["Time"]).value = Math.Max(1000, ((IntegerValue)ev.parameters["Time"]).value - 60000); //prevent items that start at min=1
                        }
                    }
                    AddToList(inOrderEvents, ev);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }

            file.Close();
            foreach (SimulationEvent ev in inOrderEvents)
            {
                switch (ev.eventType)
                {
                    case "NewObject":
                        /*
                         * <NewObject><Parameter><Name>ID</Name><Value><StringType>7738</StringType></Value></Parameter><Parameter><Name>ObjectType</Name><Value><StringType>SeaObject</StringType></Value></Parameter><Parameter><Name>StateTable</Name><Value><StateTableType><StateName><Name>FullyFunctional</Name><Value><AttributeCollectionType><Attribute><Name>Capability</Name><Value><CapabilityType><Effect><Name>Surface to air</Name><Range>500</Range><Intensity>1</Intensity><Probability>0.100000001490116</Probability></Effect><Effect><Name>Surface to surface</Name><Range>4000</Range><Intensity>1</Intensity><Probability>0.899999976158142</Probability></Effect></CapabilityType></Value></Attribute><Attribute><Name>Vulnerability</Name><Value><VulnerabilityType><Transition><State>Dead</State><Conditions><Condition><Capability>Allied shelling</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Under Siege</State><Conditions><Condition><Capability>Surface to surface</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Coordinate Strike</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Laser</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition></VulnerabilityType></Value></Attribute><Attribute><Name>Sensors</Name><Value><SensorArrayType></SensorArrayType></Value></Attribute><Attribute><Name>Emitters</Name><Value><EmitterType><Emitter><AttributeName>ID</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ClassName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>State</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Location</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ObjectName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Velocity</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Throttle</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>IconName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter></EmitterType></Value></Attribute><Attribute><Name>IsWeapon</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>75</DoubleType></Value></Attribute><Attribute><Name>Size</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>LaunchDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>AttackDuration</Name><Value><IntegerType>50000</IntegerType></Value></Attribute><Attribute><Name>DockingDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>FuelCapacity</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>FuelAmount</Name><Value><DoubleType>3000</DoubleType></Value></Attribute><Attribute><Name>FuelConsumptionRate</Name><Value><DoubleType>1</DoubleType></Value></Attribute><Attribute><Name>FuelDepletionState</Name><Value><StringType>Dead</StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.big.png</StringType></Value></Attribute><Attribute><Name>RemoveOnDestruction</Name><Value><BooleanType>True</BooleanType></Value></Attribute><Attribute><Name>LaunchedByOwner</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>CanOwn</Name><Value><StringListType></StringListType></Value></Attribute><Attribute><Name>DefaultClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>ClassificationDisplayRules</Name><Value><ClassificationDisplayRulesType><Rule><State>FullyFunctional</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule></ClassificationDisplayRulesType></Value></Attribute></AttributeCollectionType></Value></StateName><StateName><Name>Hostile</Name><Value><AttributeCollectionType><Attribute><Name>Capability</Name><Value><CapabilityType><Effect><Name>Surface to air</Name><Range>500</Range><Intensity>1</Intensity><Probability>0.100000001490116</Probability></Effect><Effect><Name>Surface to surface</Name><Range>4000</Range><Intensity>1</Intensity><Probability>0.899999976158142</Probability></Effect></CapabilityType></Value></Attribute><Attribute><Name>Vulnerability</Name><Value><VulnerabilityType><Transition><State>Dead</State><Conditions><Condition><Capability>Allied shelling</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Under Siege</State><Conditions><Condition><Capability>Surface to surface</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Coordinate Strike</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Laser</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition></VulnerabilityType></Value></Attribute><Attribute><Name>Sensors</Name><Value><SensorArrayType></SensorArrayType></Value></Attribute><Attribute><Name>Emitters</Name><Value><EmitterType><Emitter><AttributeName>ID</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ClassName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>State</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Location</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ObjectName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Velocity</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Throttle</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>IconName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter></EmitterType></Value></Attribute><Attribute><Name>IsWeapon</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>75</DoubleType></Value></Attribute><Attribute><Name>Size</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>LaunchDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>AttackDuration</Name><Value><IntegerType>50000</IntegerType></Value></Attribute><Attribute><Name>DockingDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>FuelCapacity</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>FuelAmount</Name><Value><DoubleType>3000</DoubleType></Value></Attribute><Attribute><Name>FuelConsumptionRate</Name><Value><DoubleType>1</DoubleType></Value></Attribute><Attribute><Name>FuelDepletionState</Name><Value><StringType>Dead</StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.big.png</StringType></Value></Attribute><Attribute><Name>RemoveOnDestruction</Name><Value><BooleanType>True</BooleanType></Value></Attribute><Attribute><Name>LaunchedByOwner</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>CanOwn</Name><Value><StringListType></StringListType></Value></Attribute><Attribute><Name>DefaultClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>ClassificationDisplayRules</Name><Value><ClassificationDisplayRulesType><Rule><State>FullyFunctional</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule></ClassificationDisplayRulesType></Value></Attribute></AttributeCollectionType></Value></StateName><StateName><Name>Dead</Name><Value><AttributeCollectionType><Attribute><Name>Capability</Name><Value><CapabilityType><Effect><Name>Surface to air</Name><Range>500</Range><Intensity>1</Intensity><Probability>0.100000001490116</Probability></Effect><Effect><Name>Surface to surface</Name><Range>4000</Range><Intensity>1</Intensity><Probability>0.899999976158142</Probability></Effect></CapabilityType></Value></Attribute><Attribute><Name>Vulnerability</Name><Value><VulnerabilityType><Transition><State>Dead</State><Conditions><Condition><Capability>Allied shelling</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Under Siege</State><Conditions><Condition><Capability>Surface to surface</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Coordinate Strike</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Laser</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition></VulnerabilityType></Value></Attribute><Attribute><Name>Sensors</Name><Value><SensorArrayType></SensorArrayType></Value></Attribute><Attribute><Name>Emitters</Name><Value><EmitterType><Emitter><AttributeName>ID</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ClassName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>State</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Location</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ObjectName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Velocity</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Throttle</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>IconName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter></EmitterType></Value></Attribute><Attribute><Name>IsWeapon</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>Size</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>LaunchDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>AttackDuration</Name><Value><IntegerType>50000</IntegerType></Value></Attribute><Attribute><Name>DockingDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>FuelCapacity</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>FuelAmount</Name><Value><DoubleType>3000</DoubleType></Value></Attribute><Attribute><Name>FuelConsumptionRate</Name><Value><DoubleType>1</DoubleType></Value></Attribute><Attribute><Name>FuelDepletionState</Name><Value><StringType>Dead</StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.big.png</StringType></Value></Attribute><Attribute><Name>RemoveOnDestruction</Name><Value><BooleanType>True</BooleanType></Value></Attribute><Attribute><Name>LaunchedByOwner</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>CanOwn</Name><Value><StringListType></StringListType></Value></Attribute><Attribute><Name>DefaultClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>ClassificationDisplayRules</Name><Value><ClassificationDisplayRulesType><Rule><State>FullyFunctional</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule></ClassificationDisplayRulesType></Value></Attribute></AttributeCollectionType></Value></StateName><StateName><Name>Under Siege</Name><Value><AttributeCollectionType><Attribute><Name>Capability</Name><Value><CapabilityType><Effect><Name>Surface to air</Name><Range>500</Range><Intensity>1</Intensity><Probability>0.100000001490116</Probability></Effect><Effect><Name>Surface to surface</Name><Range>4000</Range><Intensity>1</Intensity><Probability>0.899999976158142</Probability></Effect></CapabilityType></Value></Attribute><Attribute><Name>Vulnerability</Name><Value><VulnerabilityType><Transition><State>Dead</State><Conditions><Condition><Capability>Allied shelling</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Allied shelling</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Surface to surface</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Coordinate Strike</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition><Transition><State>Dead</State><Conditions><Condition><Capability>Laser</Capability><Effect>1</Effect><Range>0</Range><Probability>100</Probability></Condition></Conditions></Transition></VulnerabilityType></Value></Attribute><Attribute><Name>Sensors</Name><Value><SensorArrayType></SensorArrayType></Value></Attribute><Attribute><Name>Emitters</Name><Value><EmitterType><Emitter><AttributeName>ID</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ClassName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>State</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Location</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>ObjectName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Velocity</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>Throttle</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter><Emitter><AttributeName>IconName</AttributeName><IsEngram>False</IsEngram><Levels><Level>A</Level><Variance>0</Variance></Levels></Emitter></EmitterType></Value></Attribute><Attribute><Name>IsWeapon</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>75</DoubleType></Value></Attribute><Attribute><Name>Size</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>LaunchDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>AttackDuration</Name><Value><IntegerType>50000</IntegerType></Value></Attribute><Attribute><Name>EngagementDuration</Name><Value><IntegerType>10000</IntegerType></Value></Attribute><Attribute><Name>DockingDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>FuelCapacity</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>FuelAmount</Name><Value><DoubleType>3000</DoubleType></Value></Attribute><Attribute><Name>FuelConsumptionRate</Name><Value><DoubleType>1</DoubleType></Value></Attribute><Attribute><Name>FuelDepletionState</Name><Value><StringType>Dead</StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.siege.png</StringType></Value></Attribute><Attribute><Name>RemoveOnDestruction</Name><Value><BooleanType>True</BooleanType></Value></Attribute><Attribute><Name>LaunchedByOwner</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>CanOwn</Name><Value><StringListType></StringListType></Value></Attribute><Attribute><Name>DefaultClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>ClassificationDisplayRules</Name><Value><ClassificationDisplayRulesType><Rule><State>FullyFunctional</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule><Rule><State>FullyFunctional</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Hostile</Classification><DisplayIcon>ImageLib.hostile.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Threat</Classification><DisplayIcon>ImageLib.threat.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Suspect</Classification><DisplayIcon>ImageLib.suspect.png</DisplayIcon></Rule><Rule><State>Hostile</State><Classification>Friendly</Classification><DisplayIcon>ImageLib.friendly.png</DisplayIcon></Rule></ClassificationDisplayRulesType></Value></Attribute></AttributeCollectionType></Value></StateName></StateTableType></Value></Parameter><Parameter><Name>Attributes</Name><Value><AttributeCollectionType><Attribute><Name>OwnerID</Name><Value><StringType>Merchant DM</StringType></Value></Attribute><Attribute><Name>ClassName</Name><Value><StringType>Cargo ship</StringType></Value></Attribute></AttributeCollectionType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>0</IntegerType></Value></Parameter></NewObject>
                         */
                        id = ((StringValue)ev["ID"]).value;
                        o = new SimObject(id);
                        String genus = ((StringValue)ev["ObjectType"]).value;
                        if (genus == "SeaObject" || genus == "AirObject")
                        {
                            //TODO: attrs
                            //-owner
                            //-classname
                            o.Owner = ((StringValue)((AttributeCollectionValue)ev["Attributes"])["OwnerID"]).value;
                            o.ClassName = ((StringValue)((AttributeCollectionValue)ev["Attributes"])["ClassName"]).value;

                            objects.Add(id, o);
                        }
                        else
                        {
                            Console.WriteLine("Didn't store new object data for " + id + ", type = " + genus);
                        }
                        break;
                    case "RevealObject":
                        /*
                         * <RevealObject><Parameter><Name>ObjectID</Name><Value><StringType>7738</StringType></Value></Parameter><Parameter><Name>Attributes</Name><Value><AttributeCollectionType><Attribute><Name>IsWeapon</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>75</DoubleType></Value></Attribute><Attribute><Name>Size</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>LaunchDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>AttackDuration</Name><Value><IntegerType>50000</IntegerType></Value></Attribute><Attribute><Name>DockingDuration</Name><Value><IntegerType>0</IntegerType></Value></Attribute><Attribute><Name>FuelCapacity</Name><Value><DoubleType>0</DoubleType></Value></Attribute><Attribute><Name>FuelAmount</Name><Value><DoubleType>3000</DoubleType></Value></Attribute><Attribute><Name>FuelConsumptionRate</Name><Value><DoubleType>1</DoubleType></Value></Attribute><Attribute><Name>FuelDepletionState</Name><Value><StringType>Dead</StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.big.png</StringType></Value></Attribute><Attribute><Name>RemoveOnDestruction</Name><Value><BooleanType>True</BooleanType></Value></Attribute><Attribute><Name>InitialTag</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>LaunchedByOwner</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>DefaultClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>DockedToParent</Name><Value><BooleanType>False</BooleanType></Value></Attribute><Attribute><Name>Location</Name><Value><LocationType><X>42200</X><Y>66000</Y><Z>0</Z></LocationType></Value></Attribute><Attribute><Name>State</Name><Value><StringType>FullyFunctional</StringType></Value></Attribute></AttributeCollectionType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>0</IntegerType></Value></Parameter></RevealObject>
                         */
                        id = ((StringValue)ev["ObjectID"]).value;
                        o = objects[id];
                        //TODO: attrs
                        AttributeCollectionValue acv = ev["Attributes"] as AttributeCollectionValue;
                        o.State = ((StringValue)acv["State"]).value;
                        objectMaxSpeeds.Add(id, ((DoubleValue)acv["MaximumSpeed"]).value);
                        if (acv.attributes.ContainsKey("ObjectName"))
                        {
                            String iff = ((StringValue)acv["ObjectName"]).value;
                            if (!iffValues.ContainsKey(id))
                                iffValues.Add(id, iff);
                            else //random edge case, but this did happen
                                iffValues[id] = iff;
                        }
                        else
                        {
                            //no object name is close to suspect
                            suspectObjects.Add(id);
                        }
                        //get max speed
                        break;
                    case "MoveObject":
                        /*
                         * <MoveObject><Parameter><Name>ObjectID</Name><Value><StringType>7738</StringType></Value></Parameter><Parameter><Name>DestinationLocation</Name><Value><LocationType><X>78600</X><Y>104200</Y><Z>0</Z></LocationType></Value></Parameter><Parameter><Name>Throttle</Name><Value><DoubleType>1</DoubleType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>1000</IntegerType></Value></Parameter></MoveObject>
                         */

                        //get throttle, set current speed
                        id = ((StringValue)ev["ObjectID"]).value;
                        double throttle = ((DoubleValue)ev["Throttle"]).value;
                        double maxSpeed = objectMaxSpeeds[id];
                        double speed = throttle * maxSpeed;

                            if (objectSpeeds.ContainsKey(id))
                            {
                                objectSpeeds[id] = speed;
                            }
                            else
                            {
                                objectSpeeds.Add(id, speed);
                            }
                        
                        break;
                    case "ViewProAttributeUpdate":
                        /*
                         * <ViewProAttributeUpdate><Parameter><Name>TargetPlayer</Name><Value><StringType>BAMS DM</StringType></Value></Parameter><Parameter><Name>ObjectID</Name><Value><StringType>7738</StringType></Value></Parameter><Parameter><Name>OwnerID</Name><Value><StringType>Merchant DM</StringType></Value></Parameter><Parameter><Name>Attributes</Name><Value><AttributeCollectionType><Attribute><Name>Location</Name><Value><LocationType><X>38230.7221663048</X><Y>83085.8679033485</Y><Z>0</Z></LocationType></Value></Attribute><Attribute><Name>ID</Name><Value><StringType>7738</StringType></Value></Attribute><Attribute><Name>OwnerID</Name><Value><StringType>Merchant DM</StringType></Value></Attribute><Attribute><Name>CapabilitiesList</Name><Value><StringListType><Value>Surface to air</Value><Value>Surface to surface</Value></StringListType></Value></Attribute><Attribute><Name>DockedObjects</Name><Value><StringListType></StringListType></Value></Attribute><Attribute><Name>MaximumSpeed</Name><Value><DoubleType>75</DoubleType></Value></Attribute><Attribute><Name>CurrentClassification</Name><Value><StringType></StringType></Value></Attribute><Attribute><Name>IconName</Name><Value><StringType>ImageLib.big.png</StringType></Value></Attribute></AttributeCollectionType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>311000</IntegerType></Value></Parameter></ViewProAttributeUpdate>
                         */
                        //hasViewProEvents = true;
                        //look for classifications, or attributes with times
                        id = ((StringValue)ev["ObjectID"]).value;
                        o = objects[id];
                        //TODO: attrs
                        AttributeCollectionValue VPAUacv = (AttributeCollectionValue)ev["Attributes"];
                        
                        break;
                    case "TimeTick":
                        currentTime = ((IntegerValue)ev["Time"]).value; //millis
                        int intTime = currentTime - 60000;
                        if (!itemsByTime.ContainsKey(intTime))
                            continue;
                        List<ItemDefinition> itemsWhoseTimeIsUp = itemsByTime[intTime];
                        if (itemsByTime == null)
                            continue;
                        foreach (ItemDefinition d in itemsWhoseTimeIsUp)
                        {
                            if (d.DM_ID == "")
                            {
                                //need to find DMs assigned to this attack
                                ItemDefinition.ActionDefinition ad = null;
                                int count = 0;
                                while (ad == null && count < d.Actions.Count)
                                {
                                    if (d.Actions[count].Action.Contains("Attack"))
                                        ad = d.Actions[count];
                                    count++;
                                }
                                if (ad != null)
                                {
                                    String stimObj = ad.ObjectID;
                                    if (objectsAssignedTo.ContainsKey(stimObj))
                                    {
                                        foreach (String dmid in objectsAssignedTo[stimObj])
                                        {
                                            if (dmid != String.Empty)
                                            {
                                                if (dmid == "FireScout")
                                                    d.DM_ID = "Firescout DM";
                                                else if (dmid == "BAMS")
                                                    d.DM_ID = "BAMS DM";
                                                else d.DM_ID = dmid;
                                                //itemsToOutput(d);
                                                //                                                outputRows.AddRange(ProcessItem(d, personMap[d.DM_ID], teamMap[d.DM_ID], scenDate, scenarioStart, scenarioEnd));
                                            }
                                            else
                                            {
                                                //without a DM nothing we can do
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //itemsToOutput(d);
                                //                                outputRows.AddRange(ProcessItem(d, personMap[d.DM_ID], teamMap[d.DM_ID], scenDate, scenarioStart, scenarioEnd));
                            }
                        }
                        break;
                    case "SEAMATE_ExperimenterLogin":
                        /*
                         * <SEAMATE_ExperimenterLogin>
                             * <Parameter><Name>IndividualID</Name><Value><StringType>291B</StringType></Value></Parameter>
                             * <Parameter><Name>TeamID</Name><Value><StringType>291</StringType></Value></Parameter>
                             * <Parameter><Name>DM_ID</Name><Value><StringType>BAMS DM</StringType></Value></Parameter>
                             * <Parameter><Name>Time</Name><Value><IntegerType>0</IntegerType></Value></Parameter>
                         * </SEAMATE_ExperimenterLogin>
                         */
                        dm = FixDM(((StringValue)ev["DM_ID"]).value);
                        String person = ((StringValue)ev["IndividualID"]).value;
                        String team = ((StringValue)ev["TeamID"]).value;
                        personMap[dm] = person;
                        teamMap[dm] = team;
                        break;
                    case "SEAMATE_StimulusSent":
                        /*
                         * <SEAMATE_StimulusSent>
                             * <Parameter><Name>ItemID</Name><Value><StringType>1</StringType></Value></Parameter>
                             * <Parameter><Name>DM_ID</Name><Value><StringType>BAMS DM</StringType></Value></Parameter>
                             * <Parameter><Name>ObjectID</Name><Value><StringType>2469</StringType></Value></Parameter>
                             * <Parameter><Name>StimulusType</Name><Value><StringType>Reveal</StringType></Value></Parameter>
                             * <Parameter><Name>Time</Name><Value><IntegerType>1000</IntegerType></Value></Parameter>
                         * </SEAMATE_StimulusSent>
                         */
                        dm = FixDM(((StringValue)ev["DM_ID"]).value);
                        itemId = ((StringValue)ev["ItemID"]).value;
                        id = ((StringValue)ev["ObjectID"]).value;
                        action = ((StringValue)ev["StimulusType"]).value;
                        t = ((IntegerValue)ev["Time"]).value;
                        
                        // def = dmAssignedItems[dm];
                        if (dm == String.Empty)
                        {
                            dm = GetAssignedDM(id);
                        }
                        if (action.ToLower().StartsWith("attack"))
                        {//add to previous item for DM 
                            def = FindLastItemForDm(itemId, dm, t);
                            if (objectLastAttackItems.ContainsKey(id))
                            { 
                                
                            }
                        }
                        else
                        {
                            if (!dmAssignedItems.ContainsKey(dm))
                            {
                                def = new ItemDefinition(itemId, dm, t);
                                
                                AddItem(def);
                            }
                            else
                            {
                                if (dmAssignedItems[dm].ContainsKey(t))
                                {
                                    def = dmAssignedItems[dm][t];
                                }
                                else
                                {
                                    def = new ItemDefinition(itemId, dm, t);
                                    dmAssignedItems[dm].Add(t, def);
                                    AddItem(def);
                                }

                            }
                        }
                        if (t != def.Time)
                        {
                            //we have a new item, process old one, unless we do that on TimeTicks.
                        }
                        if (!objectsAssignedTo.ContainsKey(id))
                        {
                            objectsAssignedTo.Add(id, new List<string>());
                        }
                        if (!objectsAssignedTo[id].Contains(dm))
                            objectsAssignedTo[id].Add(dm);
                        
                        if (action.ToLower().StartsWith("attack"))
                        {
                            if (!objectLastAttackItems.ContainsKey(id))
                            {
                                objectLastAttackItems.Add(id, itemId);
                            }
                            else
                            {
                                objectLastAttackItems[id]=itemId;
                            }
                            try
                            {
                                def.Actions.Add(new ItemDefinition.AttackActionDefinition(id, action, t, objectSpeeds[id]));
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            try
                            {
                                def.Actions.Add(new ItemDefinition.ActionDefinition(id, action, objectSpeeds[id]));
                            }
                            catch (Exception ex) { }
                        }
                        break;
                    case "StateChange":

                        break;
                    case "SelfDefenseAttackStarted":
                        dm = ((StringValue)ev["AttackerObjectID"]).value;
                        t = ((IntegerValue)ev["Time"]).value;
                        if (!selfDefenses.ContainsKey(dm))
                        {
                            selfDefenses.Add(dm, new List<int>());
                        }
                        selfDefenses[dm].Add(t);
                        break;

                    case "History_AttackedObjectReport":

                        break;
                    case "AttackSucceeded":

                        break;

                    case "ClientMeasure_ObjectSelected":
                        /*
                         * <ClientMeasure_ObjectSelected><Parameter><Name>ObjectID</Name><Value><StringType>5069</StringType></Value></Parameter><Parameter><Name>UserID</Name><Value><StringType>Firescout DM</StringType></Value></Parameter><Parameter><Name>OwnerID</Name><Value><StringType>Merchant DM</StringType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>3000</IntegerType></Value></Parameter></ClientMeasure_ObjectSelected>
                         */
                        //FIND
                        //hopefully not needed
                        if (hasViewProEvents)
                            continue;
                        //else we need to keep track here
                        id = ((StringValue)ev["ObjectID"]).value;
                        dm = ((StringValue)ev["UserID"]).value;
                        t = ((IntegerValue)ev["Time"]).value;
                        if (ObjectAssignedToDM(id, dm))
                        {
                            UpdateFindTime(id, t);
                        }
                        break;
                    case "ObjectClassificationRequest":
                        /*
                         * <ObjectClassificationRequest>
                             * <Parameter><Name>UserID</Name><Value><StringType>Firescout DM</StringType></Value></Parameter>
                             * <Parameter><Name>ObjectID</Name><Value><StringType>5069</StringType></Value></Parameter>
                             * <Parameter><Name>ClassificationName</Name><Value><StringType>Friendly</StringType></Value></Parameter>
                             * <Parameter><Name>Time</Name><Value><IntegerType>8000</IntegerType></Value></Parameter>
                         * </ObjectClassificationRequest>
                         */
                        //FIX
                        //hopefully not needed
                        if (hasViewProEvents)
                            continue;
                        //else we need to keep track here
                        id = ((StringValue)ev["ObjectID"]).value;
                        dm = ((StringValue)ev["UserID"]).value;
                        t = ((IntegerValue)ev["Time"]).value;
                        String classification = ((StringValue)ev["ClassificationName"]).value;
                        if (ObjectAssignedToDM(id, dm))
                        {
                            UpdateFixTime(id, t, dm, classification);//TODO Set classified as somewhere so we can check correctness
                        }
                        break;
                    case "SEAMATE_TrackAdded":
                        /*
                         * <SEAMATE_TrackAdded>
                             * <Parameter><Name>UserID</Name><Value><StringType>BAMS DM</StringType></Value></Parameter>
                             * <Parameter><Name>ObjectID</Name><Value><StringType>1604</StringType></Value></Parameter>
                             * <Parameter><Name>Time</Name><Value><IntegerType>24000</IntegerType></Value></Parameter>
                         * </SEAMATE_TrackAdded>
                         */
                        //TRACK
                        if (hasViewProEvents)
                            continue;
                        //else we need to keep track here
                        id = ((StringValue)ev["ObjectID"]).value;
                        dm = FixDM(((StringValue)ev["UserID"]).value);
                        t = ((IntegerValue)ev["Time"]).value;
                        if (ObjectAssignedToDM(id, dm))
                        {
                            UpdateTrackTime(id, t);
                        }
                        if (dm.ToLower().Contains("bams"))
                        {
                            bamsAvailableTracks--;
                        }
                        else
                        {
                            firescoutAvailableTracks--;
                        }
                        break;
                    case "AttackObject":
                        /*
                         * <AttackObject>
                             * <Parameter><Name>ObjectID</Name><Value><StringType>BAMS</StringType></Value></Parameter>
                             * <Parameter><Name>TargetObjectID</Name><Value><StringType>5553</StringType></Value></Parameter>
                             * <Parameter><Name>CapabilityName</Name><Value><StringType>Coordinate Strike</StringType></Value></Parameter>
                             * <Parameter><Name>PercentageApplied</Name><Value><IntegerType>0</IntegerType></Value></Parameter>
                             * <Parameter><Name>Time</Name><Value><IntegerType>210000</IntegerType></Value></Parameter>
                         * </AttackObject>
                         */
                        //TARGET
                        if (hasViewProEvents)
                            continue;
                        //else we need to keep track here
                        id = ((StringValue)ev["TargetObjectID"]).value;
                        // dm = id = ((StringValue)ev["UserID"]).value;
                        String attacker = ((StringValue)ev["ObjectID"]).value;
                        dm = FixDM(objects[attacker].Owner);
                        t = ((IntegerValue)ev["Time"]).value;
                        if (ObjectAssignedToDM(id, dm))
                        {
                            UpdateTargetTime(id, t);
                        }
                        break;
                    case "SEAMATE_TrackRemoved":
                        //<SEAMATE_TrackRemoved><Parameter><Name>UserID</Name><Value><StringType>BAMS DM</StringType></Value></Parameter><Parameter><Name>ObjectID</Name><Value><StringType>4257</StringType></Value></Parameter><Parameter><Name>Time</Name><Value><IntegerType>268000</IntegerType></Value></Parameter></SEAMATE_TrackRemoved>
                        id = ((StringValue)ev["ObjectID"]).value;
                        dm = FixDM(((StringValue)ev["UserID"]).value);

                        if (dm.ToLower().Contains("bams"))
                        {
                            bamsAvailableTracks++;
                        }
                        else
                        {
                            firescoutAvailableTracks++;
                        }
                        break;
                }
            }

            List<int> itemTimes = new List<int>();
            foreach (String dmName in dmAssignedItems.Keys)
            {
                foreach (int i in dmAssignedItems[dmName].Keys)
                {
                    if (!itemTimes.Contains(i))
                        itemTimes.Add(i);
                }
            }
            itemTimes.Sort();
            ItemDefinition itemDef;
            foreach (int i in itemTimes)
            {
                foreach (String dmName in dmAssignedItems.Keys)
                {
                    if (dmAssignedItems[dmName].ContainsKey(i))
                    {
                        itemDef = dmAssignedItems[dmName][i];
                        outputRows.AddRange(ProcessItem(itemDef, personMap[itemDef.DM_ID], teamMap[itemDef.DM_ID], scenDate, scenarioStart, scenarioEnd));                        
                    }
                }
            }

            FileStream fs = new FileStream(outputFilePath, FileMode.Create);
            byte[] b;
            ASCIIEncoding encoding = new ASCIIEncoding();
            foreach (String ss in outputRows)
            {
                b = encoding.GetBytes(ss);
                fs.Write(b, 0, b.Length);
            }
            fs.Close();
        }
        private String FixDM(String dmid)
        {
            if (dmid == "FireScout")
                return "Firescout DM";
            else if (dmid == "BAMS")
                return "BAMS DM";

            return dmid;
        }
        private ItemDefinition FindLastItemForDm(string itemId, string dm, int t)
        {
            ItemDefinition def;
            //step through past items backwards.
            if (dmAssignedItems.ContainsKey(dm))
            {
                int highest = -1;
                foreach (int k in dmAssignedItems[dm].Keys)
                {
                    if (dmAssignedItems[dm][k].ID == itemId && k > highest)
                        highest = k;
                }
                if (highest > -1 && currentTime - highest <= 60000) //added second half of check in case last item was more than a minute ago
                {
                    return dmAssignedItems[dm][highest];
                }
            }

            //if none found do something like this to add a new item
            if (!dmAssignedItems.ContainsKey(dm))
            {
                def = new ItemDefinition(itemId, dm, t);
                AddItem(def);
            }
            else
            {
                if (dmAssignedItems[dm].ContainsKey(t))
                {
                    def = dmAssignedItems[dm][t];
                }
                else
                {
                    def = new ItemDefinition(itemId, dm, t);
                    dmAssignedItems[dm].Add(t, def);
                    AddItem(def);
                }
            }

            return def;
        }

        private void AddItem(ItemDefinition item)
        {
            if (item.DM_ID == "FireScout")
                item.DM_ID = "Firescout DM";
            if (item.DM_ID == "BAMS")
                item.DM_ID = "BAMS DM";
            if (item.DM_ID == String.Empty)
            {
                //automated attack, needs DM and Item ID 
                ItemDefinition.ActionDefinition ad = null;
                int count = 0;
                while (ad == null && count < item.Actions.Count)
                {
                    if (item.Actions[count].Action.Contains("Attack"))
                        ad = item.Actions[count];
                    count++;
                }
                if (ad != null)
                {
                    String stimObj = ad.ObjectID;
                    if (objectsAssignedTo.ContainsKey(stimObj))
                    {
                        foreach (String dmid in objectsAssignedTo[stimObj])
                        {
                            if (dmid != String.Empty)
                            {
                                if (dmid == "FireScout")
                                    item.DM_ID = "Firescout DM";
                                else if (dmid == "BAMS")
                                    item.DM_ID = "BAMS DM";
                                else item.DM_ID = dmid;
                                //ProcessItem(d, personMap[d.DM_ID], teamMap[d.DM_ID], scenDate, scenarioStart, scenarioEnd);
                                dmAssignedItems[item.DM_ID][item.Time].Actions.Add(ad);
                            }
                            else
                            {
                                //without a DM nothing we can do
                            }
                        }
                    }
                }
                return; //steps already done above
            }
            if (!dmAssignedItems.ContainsKey(item.DM_ID))
            {
                dmAssignedItems.Add(item.DM_ID, new Dictionary<int, ItemDefinition>());

                dmAssignedItems[item.DM_ID].Add(item.Time, item);
            }
            else
            {
                if (dmAssignedItems[item.DM_ID].ContainsKey(item.Time))
                {
                    dmAssignedItems[item.DM_ID][item.Time] = item;
                }
                else
                {
                    dmAssignedItems[item.DM_ID].Add(item.Time, item);
                }

            }

            if (!itemsByTime.ContainsKey(item.Time))
            {
                itemsByTime.Add(item.Time, new List<ItemDefinition>());
            }
            itemsByTime[item.Time].Add(item);
        }
        private bool IsObjectHostile(String objectId)
        {//it has attacked
            if (selfDefenses.ContainsKey(objectId))
                return true;
            return false;
        }
        private bool DoesDMHaveAvailableTracks(string dmID)
        {
            return true; //TODO: Fix this
        }
        private bool IsObjectSuspect(String objectId)
        {//it is off sea lane, heading away from port, squawking pirate

            if (suspectObjects.Contains(objectId))
                return true;

            return false;
        }
        private bool IsObjectFriendly(String objectId)
        {//squawking friendly
            if (iffValues.ContainsKey(objectId))
            {
                if (iffValues[objectId].ToLower() == "friendly")
                    return true;
                if (iffValues[objectId].ToLower() == "pirate" || iffValues[objectId].ToLower() == "hostile")
                    return false;

                return true;
            }
            return false;
        }

        private String GetFixCorrectness(String objId, String dmId)
        {
            //it was fixed, but was it right
            if (fixedAs.ContainsKey(objId) && fixedAs[objId].ContainsKey(dmId))
            {
                String fAs = fixedAs[objId][dmId];
                if (fAs.ToLower() == "hostile")
                {
                    if (IsObjectHostile(objId))
                        return "1";
                    else
                        return "0";
                }
                else if (fAs.ToLower() == "suspect")
                {
                    if (IsObjectSuspect(objId))
                        return "1";
                    else
                        return "0";
                }
                else if (fAs.ToLower() == "friendly")
                {
                    if (IsObjectFriendly(objId))
                        return "1";
                    else
                        return "0";
                }
            }
            //it wasn't fixed, was it ok?
            if (IsObjectHostile(objId))
                return "0";
            else
                if (IsObjectSuspect(objId) && DoesDMHaveAvailableTracks(dmId))
                    return "0";

            return ".";
        }
        private List<String> ProcessItem(ItemDefinition item, String person, String team, String date, String scenarioStart, String scenarioEnd)
        {
            //takes item, actions; creates row output for each action and adds it to rows.
            List<String> outputRows = new List<string>();
            int count = 1;
            String fixBehavior, fixCorrectness, trackCorrectness, targetCorrectness;
            int findTime, fixTime, trackTime, targetTime;
            foreach (ItemDefinition.ActionDefinition a in item.Actions)
            {
                fixBehavior = fixCorrectness = trackCorrectness = targetCorrectness = ".";
                #region REVEAL
                if (a.Action.Contains("Reveal"))
                {//requires Find,Fix,Track if suspect
                    fixTime = GetFixTime(a.ObjectID, item.DM_ID, item.Time, item.Time+60000);
                    trackCorrectness = ".";
                    if (fixTime > 0)
                    {
                        fixBehavior = "User identified the entity as "+GetFixedAs(a.ObjectID, item.DM_ID)+" in the time limit.";
                        fixCorrectness = GetFixCorrectness(a.ObjectID, item.DM_ID);
                        if (fixCorrectness == "0")
                            fixBehavior = "User INCORRECTLY identified the entity";
                    }
                    else
                    {
                        fixCorrectness = "0";
                        fixBehavior = "User didn't identify the entity in the time limit.";
                    }
                    if (IsObjectSuspect(a.ObjectID))
                    {
                        if (GetFixedAs(a.ObjectID, item.DM_ID).ToLower() == "suspect")
                            trackCorrectness = "1";
                        else if (GetFixedAs(a.ObjectID, item.DM_ID).ToLower() == "hostile")
                        {
                            if (IsObjectHostile(a.ObjectID))
                                trackCorrectness = "1";
                        }
                        else
                            trackCorrectness = "0";
                    }
                }
                #endregion
                #region ATTACK
                else if (a.Action.Contains("Attack"))
                {//requires Fix as hostile, Target
                    targetCorrectness = "0"; //default
                    fixCorrectness = "0";
                    String fx = GetFixedAs(a.ObjectID, item.DM_ID);
                    if (fx.ToLower() == "hostile")
                        fixCorrectness = "1";
                    if (selfDefenses.ContainsKey(a.ObjectID))
                    {
                        foreach (int i in selfDefenses[a.ObjectID])
                        {
                            if (((SEAMATEReplayLogParser.MainWindow.ItemDefinition.AttackActionDefinition)a).AttackTime - i < 60000)
                                targetCorrectness = "1"; //attack occurred within 60 secs
                        }
                    }

                }
                #endregion
                #region MOVE
                else if (a.Action.Contains("Mov")) //covers "moving" and "move"
                { //requires Fix, Track if suspect
                    trackCorrectness = "."; //default

                    fixCorrectness = GetFixCorrectness(a.ObjectID, item.DM_ID);
                    //TODO Track
                    if (IsObjectSuspect(a.ObjectID))
                    {
                        if (GetFixedAs(a.ObjectID, item.DM_ID).ToLower() == "suspect")
                            trackCorrectness = "1";
                        else if (GetFixedAs(a.ObjectID, item.DM_ID).ToLower() == "hostile")
                        {
                            if (IsObjectHostile(a.ObjectID))
                                trackCorrectness = "1";
                        }
                        else
                            trackCorrectness = "0";
                    }
                }
                #endregion
                outputRows.Add(new RowOutput(person, team, date, scenarioStart, scenarioEnd, item.DM_ID, item.ID, String.Format("{0}", count++), String.Format("{0}", item.Time / 1000),
                    String.Format("{0}", item.Time / 1000 + 60), String.Format("{0}", item.Time / 1000), a.ObjectID, a.Action, "EAST", "NORTH", a.SpeedAtTime.ToString(), "HEADING", GetFindTime(a.ObjectID, item.DM_ID, item.Time, item.Time + 60000).ToString(),
                    fixBehavior, GetFixTime(a.ObjectID, item.DM_ID, item.Time, item.Time+60000).ToString(), fixCorrectness, GetTrackTime(a.ObjectID, item.DM_ID, item.Time, item.Time+60000).ToString(), trackCorrectness,
                    GetTargetTime(a.ObjectID, item.DM_ID, item.Time, item.Time+60000).ToString(), targetCorrectness, GetFixedAs(a.ObjectID, item.DM_ID)).ToOutput());
            }
            //TODO

            return outputRows;
        }

        public class RowOutput
        {
            public String Person = "";
            public String Team = "";
            public String Scenario = "N/A"; //always N/A
            public String Date = ""; //day/month/year
            public String ScenarioStart = ""; //time: 4:28 PM
            public String ScenarioEnd = ""; //time: 4:28 PM
            public String Role = "";
            public String Item = "";
            public String StimEvent = "";
            public String ItemStartTime = "";
            public String ItemEndTime = "";
            public String StimulusStart = "";
            public String StimulusObject = "";
            public String StimulusBehavior = "";
            public String StimLocEasting = "";
            public String StimLocNorthing = "";
            public String StimSpeed = "";
            public String StimHeading = "";
            public String FindYesNo = "";
            public String FindTime = "";
            public String FindRightWrong = "";
            public String FixBehavior = "";
            public String FixTime = "";
            public String FixRightWrong = "";
            public String TrackBehavior = "";
            public String TrackTime = "";
            public String TrackRightWrong = "";
            public String TargetBehavior = "";
            public String TargetTime = "";
            public String TargetRightWrong = "";
            public RowOutput()
            { }
            public RowOutput(String person, String team, String date, String scenStart, String scenEnd, String role, String itemId, String stimCounter, String itemStartTime, String itemEndTime,
                String stimStart, String stimObject, String stimBehavior, String locEasting, String locNorthing, String stimSpeed, String stimHeading, String findTime, String fixBehavior,
                String fixTime, String fixCorrectness, String trackTime, String trackCorrectness, String targetTime, String targetCorrectness, String fixedAs)
            {
                Person = person;
                Team = team;
                Date = date;
                ScenarioStart = scenStart;
                ScenarioEnd = scenEnd;
                Role = role;
                Item = itemId;
                StimEvent = stimCounter;
                ItemStartTime = itemStartTime;
                ItemEndTime = itemEndTime;
                StimulusStart = stimStart;
                StimulusObject = stimObject;
                StimulusBehavior = stimBehavior;
                StimLocEasting = locEasting;
                StimLocNorthing = locNorthing;
                StimSpeed = stimSpeed;
                StimHeading = stimHeading;
                FindTime = findTime;
                if (FindTime == "-100")
                    FindTime = ".";
                if (StimulusBehavior.Contains("Reveal"))
                {
                    //was supposed to click object
                    if (FindTime == "." || FindTime == "0" || Int32.Parse(FindTime)/1000 < Int32.Parse(itemStartTime) || Int32.Parse(FindTime)/1000 > Int32.Parse(itemEndTime))
                    {
                        FindYesNo = String.Format("User didn't click on object {0} in time.", StimulusObject);
                        FindRightWrong = "0";
                    }
                    else
                    {
                        FindRightWrong = "1";
                        FindYesNo = String.Format("User Clicked on object {0}", StimulusObject);
                    }
                }
                else
                {
                    //wasn't supposed to
                    FindRightWrong = ".";
                    FindTime = ".";
                    FindYesNo = ".";
                }
                if (FindTime != "." && FindTime != "-100")
                    FindTime = (Int32.Parse(FindTime) / 1000).ToString();

                FixTime = fixTime;
                if (FixTime == "-100")
                    FixTime = ".";
                if(FixTime != "." && FixTime != "-100")
                    FixTime = (Int32.Parse(FixTime) / 1000).ToString();
                FixRightWrong = fixCorrectness;
                FixBehavior = fixBehavior;

                TrackTime = trackTime;
                if (TrackTime == "-100")
                    TrackTime = ".";
                TrackRightWrong = trackCorrectness;
                if (TrackTime == "." || TrackTime == "0")
                {
                    TrackBehavior = "User did not track entity " + stimObject + " in the specified time limit.";
                }
                else
                {
                    TrackBehavior = "User tracked object " + stimObject + " which was identified as " + fixedAs;
                    TrackTime = (Int32.Parse(TrackTime) / 1000).ToString();
                }
                if (StimulusBehavior.Contains("Attack"))
                {
                    TargetTime = targetTime;
                    if (TargetTime == "-100")
                        TargetTime = ".";
                    TargetRightWrong = targetCorrectness;
                    if (TargetTime == "." || TargetTime == "0")
                    {
                        TargetBehavior = "User did not destroy the hostile target in the specified time.";
                        TargetRightWrong = "0";
                    }
                    else
                    {
                        if (Int32.Parse(TargetTime) - Int32.Parse(StimulusStart) * 1000 <= 60000)
                        {
                            //success
                            if (TargetRightWrong == "1")
                            {
                                TargetBehavior = String.Format("User attacked object {0} which was classified as Hostile", StimulusObject);
                            }
                            else if (TargetRightWrong == "0")
                            {
                                TargetBehavior = String.Format("User attacked object {0} which was NOT classified as Hostile", StimulusObject);
                            }
                            else
                            {
                                TargetBehavior = ".";
                            }
                        }
                        else
                        {
                            TargetRightWrong = "0";
                            TargetBehavior = "User did not destroy the hostile target in the specified time.";
                        }

                    }
                }
                else
                {//not an attack, so no output
                    TargetBehavior = ".";
                    TargetTime = ".";
                    TargetRightWrong = ".";
                }
                if (TargetTime != "." && TargetTime != "-100")
                    TargetTime = (Int32.Parse(TargetTime) / 1000).ToString();
            }

            public String ToOutput()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Person + ",");
                sb.Append(Team + ",");
                sb.Append(Scenario + ",");
                sb.Append(Date + ",");
                sb.Append(ScenarioStart + ",");
                sb.Append(ScenarioEnd + ",");
                sb.Append(Role + ",");
                sb.Append(Item + ",");
                sb.Append(StimEvent + ",");
                sb.Append(ItemStartTime + ",");
                sb.Append(ItemEndTime + ",");
                sb.Append(StimulusStart + ",");
                sb.Append(StimulusObject + ",");
                sb.Append(StimulusBehavior + ",");
                sb.Append(StimLocEasting + ",");
                sb.Append(StimLocNorthing + ",");
                sb.Append(StimSpeed + ",");
                sb.Append(StimHeading + ",");
                sb.Append(FindYesNo + ",");
                sb.Append(FindTime + ",");
                sb.Append(FindRightWrong + ",");
                sb.Append(FixBehavior + ",");
                sb.Append(FixTime + ",");
                sb.Append(FixRightWrong + ",");
                sb.Append(TrackBehavior + ",");
                sb.Append(TrackTime + ",");
                sb.Append(TrackRightWrong + ",");
                sb.Append(TargetBehavior + ",");
                sb.Append(TargetTime + ",");
                sb.Append(TargetRightWrong);
                sb.Append("\r\n");

                return sb.ToString();
            }
            public static String ToOutputHeader()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Person,");
                sb.Append("Team,");
                sb.Append("Scen,");
                sb.Append("Date,");
                sb.Append("ScenStart,");
                sb.Append("ScenEnd,");
                sb.Append("Role,");
                sb.Append("Item,");
                sb.Append("StimE,");
                sb.Append("ItemStart,");
                sb.Append("ItemEnd,");
                sb.Append("StimStart,");
                sb.Append("StimObject,");
                sb.Append("StimBeh,");
                sb.Append("StimLocEasting,");
                sb.Append("StimLocNorthing,");
                sb.Append("StimSpeed,");
                sb.Append("StimHead,");
                sb.Append("FindY/N,");
                sb.Append("FindTime,");
                sb.Append("FindR/W,");
                sb.Append("FixBehavior,");
                sb.Append("FixTime,");
                sb.Append("FixR/W,");
                sb.Append("TrackBehavior,");
                sb.Append("TrackTime,");
                sb.Append("TrackR/W,");
                sb.Append("TargY/N,");
                sb.Append("TargTime,");
                sb.Append("TargR/W");
                sb.Append("\r\n");
                return sb.ToString();
            }


        }
        public class ItemDefinition
        {
            public class ActionDefinition
            {
                public String ObjectID = "";
                public String Action = "";
                public double SpeedAtTime = 0.0;
                public ActionDefinition()
                { }
                public ActionDefinition(String objectId, String action, double objSpeed)
                {
                    ObjectID = objectId;
                    Action = action;
                    SpeedAtTime = objSpeed;
                }
            }
            public class AttackActionDefinition : ActionDefinition
            {
                public int AttackTime = 0;
                public AttackActionDefinition(String objectId, String action, int time, double objSpeed)
                : base(objectId, action, objSpeed)
                {
                    
                    AttackTime = time;
                }
            }
            public String ID = "";
            public String DM_ID = "";
            public int Time = -1;
            public List<ActionDefinition> Actions = new List<ActionDefinition>();
            public ItemDefinition()
            { }
            public ItemDefinition(String id, String dmId, int time)
            {
                ID = id;
                DM_ID = dmId;
                Time = time;
            }
            public ItemDefinition(String id)
                : this(id, "", -1)
            {
            }

        }
    }
}
