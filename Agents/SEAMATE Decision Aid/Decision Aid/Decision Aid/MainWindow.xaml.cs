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
using System.Collections.ObjectModel;
using Aptima.Asim.DDD.DDDAgentFramework;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using System.Windows.Threading;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Decision_Aid.Test;
using System.Media;
using System.Data;
using System.ComponentModel;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Decision_Aid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Score : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private String name, value;
            public String Name
            {
                get {
                    return this.name;
                }
                set
                {
                this.name = value;
                OnPropertyChanged("Name");
            } }
            public String Value
            {
                get {
                    return this.value;
                }
                set
                {
                this.value = value;
                OnPropertyChanged("Value");
            } }
            public Score(String name, String value)
            {
                Name = name;
                Value = value;
            }
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                { 
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public enum Urgency { 
        NONE,LOW,MODERATE,HIGH
        }
        private delegate void TwoStringParams(String name, String val);
        private delegate void AlertParams(String msg, Urgency urgency);
        private delegate bool bNoParams();
        private delegate void NoParams();
        private delegate void StringParams(String val);

        private static SoundPlayer _blip;
        private static SoundPlayer _ding;
        private static SoundPlayer _alarm;

        private String _dddHostname = Environment.MachineName;
        private int _dddPort = 9999;
        private string _dddSharePath = "DDDClient";
        private int time = -1;
        private Thread _connectingThread = null;
        private Thread _dddLoopThread = null;
        private String _dddConnectionStatus = "not"; //connecting, connected
        private int MaxTracks = 3;
        private String MyDM = "";
        private String UserID = "";
        private String TeamID = "";
        private String _mostRecentlyClickedObject = "";
        public ObservableCollection<DDDObj> Objects;
        public ObservableCollection<DDDObj> AllObjects;
        //protected DataSet Scores;
        //private List<Score> _scores;

        public ThreadsafeObservableCollection<Score> Scores
        {get;set;}
        
        private DDDServerConnection _conn;
        public DDDServerConnection DDDConnection
        {
            set { _conn = value; }
            get { if (_conn == null) _conn = new DDDServerConnection(); return _conn; }
        }
        public MainWindow()
        {
            Scores = new ThreadsafeObservableCollection<Score>();
            Objects = new ObservableCollection<DDDObj>();
            AllObjects = new ObservableCollection<DDDObj>();
            InitializeComponent();
            _blip = new SoundPlayer(Properties.Resources.blip);
            _blip.Load();
            _ding = new SoundPlayer(Properties.Resources.ding);
            _ding.Load();
            _alarm = new SoundPlayer(Properties.Resources.alarm);
            _alarm.Load();
            dataGrid1.ItemsSource = Objects;
            dataGrid2.ItemsSource = Scores;
            LoadCommandLineArgs();
        }
        private void LoadCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                String hostname, port, shareFolder;
                try
                {
                    hostname = Environment.GetCommandLineArgs()[1];
                    _dddHostname = hostname;
                    port = Environment.GetCommandLineArgs()[2];
                    _dddPort = Int32.Parse(port);
                    shareFolder = Environment.GetCommandLineArgs()[3];
                    _dddSharePath = shareFolder;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading command line args");
                }
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //ADD New Track
            List<String> ids;
            List<String> names;
            AllObjectIDs(out ids, out names);
            SelectObjectDialog dlg = new SelectObjectDialog();
            dlg.Owner = this;
            dlg.SetList(ids, names, _mostRecentlyClickedObject);
            if (dlg.ShowDialog().Value == false)
            {
                return;
            }
            if (IsBeingTracked(dlg.SelectedID))
            {
                MessageBox.Show(this,"That asset is already being tracked");
                return;
            }
            if (MaxTracks <= Objects.Count)
            {
                MessageBox.Show(this,String.Format("Adding this track would exceed your maximum number of tracks ({0}); Try removing a Track before adding a new track.", MaxTracks));
                return;
            }
            DDDObj d = GetFromAllObjects(dlg.SelectedID);
            Objects.Add(d);

            AddAlert("A new object (" + d.ID + ") was added", Urgency.NONE);
            SendTrackAddedEvent(MyDM, d.ID);
        }

        private void AddAlert(String msg, Urgency urgency)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                stackPanelUpdates.Children.Insert(0, CreateAlertBlock(msg));
                /*switch (urgency)
                { 
                    case Urgency.LOW:
                        _blip.Play();
                        break;
                    case Urgency.MODERATE:
                        _ding.Play();
                        break;
                    case Urgency.HIGH:
                        _alarm.Play();
                        break;
                    default:
                        break;
                }*/
                PlayThreadedBeep(urgency);
            }
            else
            {
                this.Dispatcher.Invoke(new AlertParams(AddAlert), msg, urgency);
            }
        }

        private void PlayThreadedBeep(Urgency howUrgent)
        {
            int hertz = 500; //For "LOW" Urgency by default
            int msPlayedFor = 500;
            int repeats = 1;
            if (howUrgent == Urgency.HIGH)
            {
                hertz = 1000; 
                msPlayedFor = 400;
                repeats = 3;
            }
            else if (howUrgent == Urgency.MODERATE)
            {
                hertz = 1000;
                msPlayedFor = 1000;
            }
            else if (howUrgent == Urgency.NONE)
                return;

            Thread t = new Thread(new ParameterizedThreadStart(PlayBeep));
            t.Start(new int[] { hertz, msPlayedFor, repeats });
        }
        private void PlayBeep(object param)
        {
            int[] ar = (int[])param;
            int freq = ar[0];
            int time = ar[1];
            int repeats = 1;
            if (ar.Length == 3)
            {
                repeats = ar[2];
            }
            for(int x = 0; x < repeats; x++)
                Console.Beep(freq, time); 
        }

        private bool IsInAllObjects(String id)
        {
            foreach (DDDObj d in AllObjects)
            {
                if (d.ID == id)
                    return true;
            }
            return false;
        }

        private bool IsBeingTracked(String id)
        {
            foreach (DDDObj d in Objects)
            {
                if (d.ID == id)
                    return true;
            }
            return false;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int row = dataGrid1.SelectedIndex;
            if (row < 0)
                return;
            String ID = ((DDDObj)dataGrid1.Items[row]).ID;
            RemoveTrack(ID);
            SendTrackRemovedEvent(MyDM, ID);
        }
        private void RemoveTrack(String id)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].ID == id)
                    {
                        Objects.RemoveAt(i);
                        return;
                    }
                }
            }
            else
            {
                this.Dispatcher.Invoke(new StringParams(RemoveTrack), id);
            }
        }
        private void RemoveFromAllObjects(String id)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
            for (int i = 0; i < AllObjects.Count; i++)
            {
                if (AllObjects[i].ID == id)
                {
                    AllObjects.RemoveAt(i);
                    return;
                }
            }
            }
            else
            {
                this.Dispatcher.Invoke(new StringParams(RemoveFromAllObjects), id);
            }
        }

        private TextBlock CreateAlertBlock(String msg)
        {
            TextBlock tb = new TextBlock();
            tb.Text = msg;
            tb.MouseUp += textBlockAlertMouseUp;
            tb.Style = (Style)FindResource("NewAlertStyle");

            return tb;
        }

        private void textBlockAlertMouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tb.Style = (Style)FindResource("AlertStyle");
        }
        private DDDObj GetFromAllObjects(String objID)
        {
            for (int i = 0; i < AllObjects.Count; i++)
            {
                if (AllObjects[i].ID == objID)
                    return AllObjects[i];
            }
            return null;
        }
        private DDDObj GetFromTrackedObjects(String objID)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].ID == objID)
                    return Objects[i];
            }
            return null;
        }
        private void AllObjectIDs(out List<String> ids, out List<String> names)
        {
            ids = new List<string>();
            names = new List<string>();

            foreach (DDDObj d in AllObjects)
            {
                ids.Add(d.ID);
                names.Add(d.Name);
            }
        }

        private void UpdateDDDConnectionStatus(String newStatus)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                _dddConnectionStatus = newStatus;
                switch (_dddConnectionStatus)
                {
                    case "not":
                        statusEllipse.Fill = new SolidColorBrush(Colors.Red);
                        btnConnectToDDD.Content = "Connect to DDD";
                        break;
                    case "connecting":
                        statusEllipse.Fill = new SolidColorBrush(Colors.Goldenrod);
                        btnConnectToDDD.Content = "Connecting to DDD";
                        break;
                    case "connected":
                        statusEllipse.Fill = new SolidColorBrush(Colors.Green);
                        btnConnectToDDD.Content = "Disconnect from DDD";
                        break;
                }
            }
            else
            {
                this.Dispatcher.Invoke(new StringParams(UpdateDDDConnectionStatus), newStatus);
            }
        }

        private void StartDDDLoop()
        {
            bool isConnected = true;
            lock (DDDConnection)
            {//lock so you can disconnect on the main thread
                isConnected = DDDConnection.IsConnected();
            }

            while (isConnected)
            {
                DDDConnection.ProcessEvents();
                Thread.Sleep(500);
                try
                {
                    lock (DDDConnection)
                    {
                        isConnected = DDDConnection.IsConnected();
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                }
            }
            UpdateDDDConnectionStatus("not");
        }
        private bool PromptForDDDDetails()
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                DDDLoginDialog dlg = new DDDLoginDialog(_dddHostname, _dddPort.ToString(), _dddSharePath);
                dlg.Owner = this;

                if (!dlg.ShowDialog().Value)
                    return false;
                string hostname = dlg.SelectedHostname;
                string port = dlg.SelectedPort;
                _dddHostname = hostname;
                _dddPort = Int32.Parse(port);
                _dddSharePath = dlg.SharePath;
                UserID = dlg.SelectedUserId;
                TeamID = dlg.SelectedTeamId;
                return true;
            }
            else
            {
                return (bool)this.Dispatcher.Invoke(new bNoParams(PromptForDDDDetails), DispatcherPriority.Normal);
            }
        }
        private void LoginToDDD()
        {
            if (!PromptForDDDDetails())
                return;
            string remoteSimulationModel = String.Format(@"\\{0}\{1}\SimulationModel.xml", _dddHostname, _dddSharePath);
            bool simModelResult = DDDConnection.ReadSimModel(remoteSimulationModel);

            if (!simModelResult)
            {
                MessageBox.Show(String.Format("Error in DDD Connection: Failed to read the simulation model at '{0}', please try again.", remoteSimulationModel), "DDD Connection Error");

                //AD: Here we could also ask the user to point to a local copy of the sim model, then re-ReadSimModel.

                return;
            }

            if (DDDConnection.IsConnected())
            {
                DDDConnection.Disconnect();
            }
            bool result = DDDConnection.ConnectToServer(_dddHostname, _dddPort);
            if (!result)
            {
                MessageBox.Show("Error connecting to DDD Server");
                return;
            }

            DDDConnection.SubscribeToEvent("SEAMATE_ResponseDecisionMaker"); //need this for custom event
             //get from dialog 
            MyDM = "";
            PromptForDM();
            if (MyDM == "")
                return;
            if (MyDM.ToLower().Contains("firescout"))
            {
                MaxTracks = 3;
            }
            else if (MyDM.ToLower().Contains("bams"))
            {
                MaxTracks = 6;// 5;
            }
            else if (MyDM.ToLower().Contains("individ"))
            {
                MaxTracks = 9;
            }
            UpdateTitle("SEAMATE Decision Aid - Logged in as " + MyDM);
            DDDConnection.LoginPlayer(MyDM, "OBSERVER");
            DDDConnection.GetDMView(MyDM); //initialize view...
            //

            DDDConnection.AddEventCallback("ViewProAttributeUpdate", new DDDServerConnection.ProcessSimulationEvent(ViewProAttributeUpdate));
            DDDConnection.AddEventCallback("ViewProInitializeObject", new DDDServerConnection.ProcessSimulationEvent(ViewProInitializeObject));
            DDDConnection.AddEventCallback("ClientRemoveObject", new DDDServerConnection.ProcessSimulationEvent(ClientRemoveObject));
            DDDConnection.AddEventCallback("TimeTick", new DDDServerConnection.ProcessSimulationEvent(TimeTick));
            DDDConnection.AddEventCallback("ViewProMotionUpdate", new DDDServerConnection.ProcessSimulationEvent(ViewProMotionUpdate));
            DDDConnection.AddEventCallback("AttackObject", new DDDServerConnection.ProcessSimulationEvent(AttackObject));
            DDDConnection.AddEventCallback("StateChange", new DDDServerConnection.ProcessSimulationEvent(StateChange));
            DDDConnection.AddEventCallback("ClientMeasure_ObjectSelected", new DDDServerConnection.ProcessSimulationEvent(ObjectSelected));
            DDDConnection.AddEventCallback("ScoreUpdate", new DDDServerConnection.ProcessSimulationEvent(ScoreUpdate));
            DDDConnection.AddEventCallback("SelfDefenseAttackStarted", new DDDServerConnection.ProcessSimulationEvent(SelfDefenseAttackStarted));
            
            _dddLoopThread = new Thread(new ThreadStart(StartDDDLoop));
            _dddLoopThread.Start();
            UpdateDDDConnectionStatus("connected");
            SendLoginEvent(UserID, MyDM, TeamID);
        }
        
        private void UpdateTitle(String title)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.Title = title;
            }
            else
            {
                this.Dispatcher.Invoke(new StringParams(UpdateTitle), title);
            }
        }
        private void PromptForDM()
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                string selectedDM = "";
                SimulationModelInfo simModel = DDDConnection.GetSimModel();
                SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "SEAMATE_RequestMyDecisionMaker");
                ((StringValue)ev["TerminalID"]).value = DDDConnection.TerminalID;
                ((StringValue)ev["ComputerName"]).value = System.Environment.MachineName;

                int attempts = 0;
                while (selectedDM == "" && attempts < 10)
                {
                    DDDConnection.SendSimEvent(ev);
                   // DDDConnection.ProcessEvents();
                    Thread.Sleep(1000);
                    DDDConnection.ProcessEvents();
                    Thread.Sleep(100);
                    List<SimulationEvent> events = DDDConnection.GetEvents();
                    foreach (SimulationEvent e in events)
                    {
                        if (e.eventType == "SEAMATE_ResponseDecisionMaker")
                        {
                            if (((StringValue)e["TerminalID"]).value == DDDConnection.TerminalID)
                            {
                                selectedDM = ((StringValue)e["DM_ID"]).value;
                                break;
                            }
                        }
                    }
                    attempts++;
                }
                if (selectedDM == "")
                {
                    MessageBox.Show("Unable to retrieve your Decision Maker info from the server in 10 seconds.  Make sure you've connected with a DDD Client, and then try re-connecting with the decision aid");
                    return;
                }

                //DDDConnection.RequestPlayers();
                //List<string> decisionMakers = new List<string>();
                //while (decisionMakers.Count == 0)
                //{
                //    decisionMakers = DDDConnection.Players;
                //    DDDConnection.ProcessEvents();
                //}
                //DMSelectorDialog dmDlg = new DMSelectorDialog(decisionMakers);
                //dmDlg.Owner = this;
                //if (!dmDlg.ShowDialog().Value)
                //    return;
                //string selectedDM = dmDlg.SelectedDecisionMaker;
                MyDM = selectedDM;
            }
            else
            {
                this.Dispatcher.Invoke(new NoParams(PromptForDM), DispatcherPriority.Normal); 
            }
        }
        private void RefreshTrackInfo()
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                dataGrid1.Items.Refresh();// = Objects;//.InvalidateVisual();
            }
            else
            {
                this.Dispatcher.Invoke(new NoParams(RefreshTrackInfo));
            }
        }

        #region DDD Event Handlers
        private void SendLoginEvent(String userID, String dmID, String teamID)
        {
            SimulationEvent ev = new SimulationEvent();
            ev.eventType = "SEAMATE_ExperimenterLogin";
            ev.parameters.Add("IndividualID", DataValueFactory.BuildString(userID));
            ev.parameters.Add("TeamID", DataValueFactory.BuildString(teamID));
            ev.parameters.Add("DM_ID", DataValueFactory.BuildString(dmID));
            ev.parameters.Add("Time", DataValueFactory.BuildInteger(time));

            DDDConnection.SendSimEvent(ev);
        }
        private void SendTrackAddedEvent(String dmID, String objectID)
        {
            SimulationEvent ev = new SimulationEvent();// SimulationEventFactory.BuildEvent(ref simModel, "SEAMATE_TrackAdded");
            ev.eventType = "SEAMATE_TrackAdded";
            ev.parameters.Add("UserID", DataValueFactory.BuildString(dmID));
            ev.parameters.Add("ObjectID", DataValueFactory.BuildString(objectID));
            ev.parameters.Add("Time", DataValueFactory.BuildInteger(time));

            DDDConnection.SendSimEvent(ev);
        }
        private void SendTrackRemovedEvent(String dmID, String objectID)
        {
            SimulationEvent ev = new SimulationEvent();
            ev.eventType = "SEAMATE_TrackRemoved";
            ev.parameters.Add("UserID", DataValueFactory.BuildString(dmID));
            ev.parameters.Add("ObjectID", DataValueFactory.BuildString(objectID));
            ev.parameters.Add("Time", DataValueFactory.BuildInteger(time));

            DDDConnection.SendSimEvent(ev);
        }
        private void TimeTick(SimulationEvent ev)
        {
            String simTime = ((StringValue)ev["SimulationTime"]).value;
            int seconds = ((IntegerValue)ev["Time"]).value;
            time = seconds;
            if (seconds % 1000 == 0)
            {
                seconds = seconds / 1000;
            }

            Console.WriteLine(simTime);
        }
        private void ObjectSelected(SimulationEvent ev)
        {
            if (((StringValue)ev["UserID"]).value != MyDM)
                return;
            String objectID = ((StringValue)ev["ObjectID"]).value;
            _mostRecentlyClickedObject = objectID;
            DDDObj d = GetFromAllObjects(objectID);
            if (d == null)
                return;
            String tag = "unknown";
            if (d.Name != String.Empty)
                tag = d.Name;
            SimulationEvent evnt = new SimulationEvent();
            evnt.eventType = "UpdateTag";
            evnt.parameters.Add("Time", DataValueFactory.BuildInteger(time));
            evnt.parameters.Add("UnitID", DataValueFactory.BuildString(objectID));
            evnt.parameters.Add("Tag", DataValueFactory.BuildString(tag));
            List<String> dms = new List<string>();
            if (MyDM.Contains("Individ"))
            {
                dms.Add(MyDM);
            }
            else
            {
                dms.Add("BAMS DM");
                dms.Add("Firescout DM");
                
            }
            evnt.parameters.Add("TeamMembers", DataValueFactory.BuildStringList(dms));
            DDDConnection.SendSimEvent(evnt);//.SendObjectAttributeUpdateEvent(objectID, "InitialTag", DataValueFactory.BuildString(tag));
        }
        private void ViewProAttributeUpdate(SimulationEvent ev)
        {//TargetPlayer, ObjectID, Attributes
            //if Tracked asset, update attributes
            if (((StringValue)ev["TargetPlayer"]).value != MyDM)
                return;
            String objectID = ((StringValue)ev["ObjectID"]).value;
            //if (!IsBeingTracked(objectID))
            //    return;

            DDDObj d = GetFromAllObjects(objectID);
            if (d == null)
                return;
            AttributeCollectionValue acv = ev["Attributes"] as AttributeCollectionValue;
            if (acv.attributes.ContainsKey("ObjectName"))
            {
                if (((StringValue)acv["ObjectName"]).value != "")
                {
                    d.Name = ((StringValue)acv["ObjectName"]).value;
                }
            }
            if (acv.attributes.ContainsKey("ClassName"))
            {
                d.Type = ((StringValue)acv["ClassName"]).value;
            }
            if (acv.attributes.ContainsKey("CurrentClassification"))
            {
                d.Classification = ((StringValue)acv["CurrentClassification"]).value;
            }
            if (acv.attributes.ContainsKey("GroundTruthIFF"))
            {
                d.IFF = ((StringValue)acv["GroundTruthIFF"]).value;
            }
            if (IsBeingTracked(objectID))
            {
                RefreshTrackInfo();
            }
        }
        private static int UnknownObjectCount = 0;
        private void ViewProInitializeObject(SimulationEvent ev)
        {//TargetPlayer, ObjectID, CurrentClassification
            if (((StringValue)ev["TargetPlayer"]).value != MyDM)
                return;
            //add object to AllObjects
            String objectID = ((StringValue)ev["ObjectID"]).value;
            if (IsInAllObjects(objectID))
                return;
            String objName = String.Format("unknown{0}", UnknownObjectCount++);
            DDDObj d = new DDDObj(objectID, objName, "", "", "");
            AllObjects.Add(d);
        }
        private void ClientRemoveObject(SimulationEvent ev)
        {//TargetPlayer, ObjectID
            if (((StringValue)ev["TargetPlayer"]).value != MyDM)
                return;
            String objectID = ((StringValue)ev["ObjectID"]).value;
            //if Tracked asset, stop track
            //if (IsBeingTracked(objectID))
            //{
            //    RemoveTrack(objectID);
            //}
            //remove from AllObjects
            //if (!IsInAllObjects(objectID))
            //    return;
            //RemoveFromAllObjects(objectID);
        }
        private Dictionary<String, double?> _objectHeadings = new Dictionary<string, double?>();
        private string _APPLICATION_NAME = "SEAMATE Decision Aid";
        private string _APPLICATION_COMPILE_DATE = "01-26-2012";
        private void ViewProMotionUpdate(SimulationEvent ev)
        {// TargetPlayer, ObjectID, Location, DestinationLocation, MaxSpeed, Throttle
            //if (((StringValue)ev["TargetPlayer"]).value != MyDM)
            //    return;
            //if Tracked asset, notify user
            String objectID = ((StringValue)ev["ObjectID"]).value;
            if (!IsBeingTracked(objectID))
                return;
            LocationValue curLoc = ev["Location"] as LocationValue;
            LocationValue destLoc = ev["DestinationLocation"] as LocationValue;
            if (curLoc.X == destLoc.X && curLoc.Y == destLoc.Y && curLoc.Z == destLoc.Z)
            {
                _objectHeadings[objectID] = null;
                return;
            }
            if (!_objectHeadings.ContainsKey(objectID))
            {
                _objectHeadings.Add(objectID, null);
            }
            double heading = Math.Round(CalculateHeading(curLoc, destLoc),2);
            if (_objectHeadings[objectID] != null)
            {
                if (Math.Round(_objectHeadings[objectID].Value,2) == heading)
                    return;
            }
            _objectHeadings[objectID] = heading;
            double speed = ((DoubleValue)ev["MaximumSpeed"]).value * ((DoubleValue)ev["Throttle"]).value;

            String msg = String.Format("ALERT: Tracked object '{0}' has changed its heading, is currently moving with heading = {1} deg, speed = {2} m/s.", objectID, heading, speed);

            AddAlert(msg, Urgency.LOW); //AD if this motion intersects another path, call it HIGH?
        }
        private void AttackStarted(String attacker, String defender)
        {
            if (!IsBeingTracked(attacker))
                return;
            //if target is MV, alert user
            try
            {
                DDDObj d = GetFromAllObjects(defender);
                if (d == null)
                    return;

                String msg = String.Format("HOSTILE: Tracked object '{0}' has engaged the vessel '{1}'", attacker, defender);

                AddAlert(msg, Urgency.HIGH);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void SelfDefenseAttackStarted(SimulationEvent ev)
        {
            String objectID = ((StringValue)ev["AttackerObjectID"]).value;
            String targetID = ((StringValue)ev["TargetObjectID"]).value;

            AttackStarted(objectID, targetID);
        }
        private void AttackObject(SimulationEvent ev)
        {//ObjectID, TargetObjectID, CapabilityName
            String objectID = ((StringValue)ev["ObjectID"]).value;
            String targetID = ((StringValue)ev["TargetObjectID"]).value;

            AttackStarted(objectID, targetID);
        }
        private void StateChange(SimulationEvent ev)
        {//ObjectID, NewState
            //if Tracked asset, alert user
            String objectID = ((StringValue)ev["ObjectID"]).value;
            if (!IsBeingTracked(objectID))
                return;
            if (((StringValue)ev["NewState"]).value == "Dead")
            {
                RemoveTrack(objectID);
                SendTrackRemovedEvent(MyDM, objectID);
            }
        }
        private void ScoreUpdate(SimulationEvent ev)
        {
            String dmID = ((StringValue)ev["DecisionMakerID"]).value;
            if (dmID != MyDM)
                return;

            String name = ((StringValue)ev["ScoreName"]).value;
            String value = ((DoubleValue)ev["ScoreValue"]).value.ToString();

            AddOrUpdateScore(name, value);
        }
        private void AddOrUpdateScore(String name, String value)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                bool handled = false;

                foreach (Score s in Scores)
                {
                    if (s.Name == name)
                    {
                        s.Value = value; //this should trigger notify.
                        return;
                    }

                }
                if (!handled)
                {
                    Scores.Add(new Score(name, value));
                }
            }
            else
            {
                this.Dispatcher.Invoke(new TwoStringParams(AddOrUpdateScore), name, value);
            }
        }
        #endregion
        private double CalculateHeading(LocationValue current, LocationValue destination)
        {
            if (!(current.exists && destination.exists))
                return 0;
            Vec3D c = new Vec3D(current);
            Vec3D d = new Vec3D(destination);
            Vec3D headingVector = c.VectorDistanceTo(d);
            headingVector.Normalize();
            return 180 / Math.PI * Math.Atan2(headingVector.X, headingVector.Y);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (_dddConnectionStatus)
            {
                case "not":
                    //connect to DDD
                    UpdateDDDConnectionStatus("connecting");
                    if (_connectingThread != null)
                    {
                        if (_connectingThread.IsAlive)
                        {
                            _connectingThread.Abort();
                            _connectingThread = null;
                        }
                    }
                    _connectingThread = new Thread(new ThreadStart(LoginToDDD));
                    _connectingThread.Start();
                    break;
                case "connecting":
                    //confirm cancel with user.
                    if (MessageBox.Show(this,"Do you want to cancel your pending connection to the DDD?", "Cancel Connection", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        //cancel connecting command
                        if (_connectingThread != null)
                        {
                            if (_connectingThread.IsAlive)
                            {
                                _connectingThread.Abort();
                                _connectingThread = null;
                            }
                        }
                        UpdateDDDConnectionStatus("not");
                    }
                    break;
                case "connected":
                    //confirm disconnection
                    if (MessageBox.Show(this,"Do you want to disconnect from the DDD?", "Disconnect from DDD", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        //disconnect
                        DDDConnection.Disconnect();
                        if (_dddLoopThread != null)
                        {
                            if (_dddLoopThread.IsAlive)
                            {
                                _dddLoopThread.Abort();
                                _dddLoopThread = null;
                            }
                        }
                        DDDConnection.ClearEventCallbacks();
                        DDDConnection.ResetForNewSession();
                        UpdateDDDConnectionStatus("not");
                    }
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DDDConnection.IsConnected())
            { 
                DDDConnection.Disconnect();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //AudioTests testWindow = new AudioTests();
            //testWindow.Show();
            //Scores.Add(new Score(DateTime.Now.ToString(), "0"));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, this.ToAboutString(), "About " + _APPLICATION_NAME);
        }
        private String ToAboutString()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            StringBuilder sb = new StringBuilder();
            sb.Append(_APPLICATION_NAME + "\r\n");
            sb.Append("Version: " + assembly.GetName().Version.ToString());
            sb.Append("\r\nCompiled on: " + _APPLICATION_COMPILE_DATE);
            return sb.ToString();
        }
    }

    public class DDDObj
    {
        public String ID { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String Classification { get; set; }
        public String IFF { get; set; }

        public DDDObj()
        {
            ID = "unknown";
            Name = "unknown";
            Type = "unknown";
            Classification = "unknown";
            IFF = "unknown";
        }
        public DDDObj(String id, String name, String type, String classification, String iff)
        {
            ID = id;
            Name = name;
            Type = type;
            Classification = classification;
            IFF = iff;
        }
    }
}
