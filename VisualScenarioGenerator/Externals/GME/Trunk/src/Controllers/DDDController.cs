//using System;
//using System.Collections.Generic;
//using System.Text;
//using Aptima.Connector;
//using Aptima.Asim.DDD;
//using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
//using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
//using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

//namespace AME.Controllers
//{
//    public class DDDController : Controller, IDDDController
//    {
//        private DDDCallBackDelegate _callback = null;
//        private SimulationModelInfo _simModel = null;
//        private string _dddHostname = string.Empty;
//        private int _dddPort = 0;
//        private int _runID = 0;
//        private Dictionary<string, bool> _dddEvents = new Dictionary<string, bool>();
//        private DDDConnector _dddConnector;
//        private bool _simulationComplete = false;
//        public bool SimulationComplete
//        {
//            get { return _simulationComplete; }
//            set { _simulationComplete = value; }
//        }

//        public Dictionary<string, bool> DDDEventSubscriptions
//        {
//            get { return _dddEvents; }
//        }

//        public DDDController(AME.Model.Model model, String configType)
//            : base(model, configType)
//        { }

//        public bool InitializeDDDConnection(string hostname, int port)
//        {
//            // reset state from any previous runs - mw
//            _simulationComplete = false;
//            _dddEvents.Clear();

//            _dddHostname = hostname;
//            _dddPort = port;

//            _dddConnector = new DDDConnector();
//            if (_dddConnector.initializeDDDConnection(hostname, port))
//            {
//                _callback = new DDDCallBackDelegate(CallbackMethod);
//                _dddConnector.setCallbackDelegate(_callback);
//                SimulationModelReader reader = new SimulationModelReader();
//                try
//                {
//                    _simModel = reader.readModel(String.Format("\\\\{0}\\DDDClient\\SimulationModel.xml", hostname));
//                }
//                catch
//                { 
//                    //get sim model from virtual directory?
//                    //this is how the DDD Client gets it
//                    System.IO.Stream str = null;
//                    SimulationModelReader s = new SimulationModelReader();
//                    try
//                    {

//                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
//                        str = assembly.GetManifestResourceStream(this.GetType(), "SimulationModel.xml");
//                        _simModel = s.readModel(str); 
//                    }
//                    catch (Exception)
//                    {
//                        Console.WriteLine("Unable to read the Simulation Model.");
//                    return false;
//                }
//                }

//                PopulateDDDEventList();
//            }
//            else
//            {
//                return false;
//            }

//            return true;
//        }

//        private void PopulateDDDEventList()
//        {
//            _dddEvents = new Dictionary<string, bool>();
//            if (_simModel == null)
//                return;

//            foreach (string eventName in _simModel.eventModel.events.Keys)
//            {
//                //three events that always need to be subscribed to are:
//                // ExternalApp_SimStart -> Sim is starting
//                // ExternalApp_SimStop  -> Sim is stopping
//                // StopReplay           -> Replay is stopping
//                if (eventName == "ExternalApp_SimStart" ||
//                    eventName == "ExternalApp_SimStop" ||
//                    eventName == "StopReplay")
//                {
//                    _dddEvents.Add(eventName, true);
//                }
//                else
//                {
//                    _dddEvents.Add(eventName, false);
//                }
//            }
//        }

//        public void SetEventSubscriptionLevel(string eventName, bool isSubscribing)
//        {
//            if (_dddEvents == null)
//                PopulateDDDEventList();

//            if (!_dddEvents.ContainsKey(eventName))
//            {
//                _dddEvents.Add(eventName, false);
//            }

//            if (eventName == "ExternalApp_SimStart" ||
//                    eventName == "ExternalApp_SimStop" ||
//                    eventName == "StopReplay")
//            {
//                return;
//            }

//            _dddEvents[eventName] = isSubscribing;
//        }

//        public bool SubscribeToDDDEvents()
//        {
//            if (_dddConnector == null)
//                return false;

//            foreach (string eventName in _dddEvents.Keys)
//            {
//                if (_dddEvents[eventName])
//                {
//                    _dddConnector.subscribeToDDDEvent(eventName);
//                }
//            }

//            return true;
//        }

//        public void SendTick()
//        {
//            if (_dddConnector == null)
//            {
                
//                return;
//            }
//            try
//            {
//                _dddConnector.tick();
//            }
//            catch
//            {
//                _simulationComplete = true; //have to do this because server seems to disconnect before SimStop event is received.
//            }

//        }

//        public void setRunID(int id)
//        {
//            _runID = id;
//        }

//        public void CallbackMethod(object simEvent)
//        {
//            if (!(simEvent is SimulationEvent))
//                return; //throw error?
//            //this is passed as the callback method, and will receive SimEvents from
//            //the DDDConnector.  Pushes event data into AME.

//            Console.WriteLine(String.Format("Event of type \"{0}\" received:", ((SimulationEvent)simEvent).eventType));

//            SimulationEvent simulationEvent = (SimulationEvent)simEvent;
//            String eventType = simulationEvent.eventType;
//            Controller controller = this;

//            int runID = _runID;
//            int componentID;

//            try
//            {
//                // This switch is generated from the SimulationModel
//                // It will attach events and their basic (non complex type) parameters 
//                // to a "Run" component that is given to this controller as a runID.
//                // Complex parameters need to be added.
//                // AME is SLOW when getting hammered with creates and connects right now
//                // so it will take some time for it to catch up after the DDD is killed.
//                // This is fixable, will look at it later - Mark.
//                switch (eventType)
//                {
//                    case "FailedToCreateChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Message", "" + ((StringValue)simulationEvent["Message"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.SenderDM_ID", "" + ((StringValue)simulationEvent["SenderDM_ID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "JoinChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayerID", "" + ((StringValue)simulationEvent["TargetPlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "TimeTick":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.SimulationTime", "" + ((StringValue)simulationEvent["SimulationTime"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "SimulationTimeEvent":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.SimulationTime", "" + ((StringValue)simulationEvent["SimulationTime"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ResetSimulation":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "SimCoreReady":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "StartupComplete":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RandomSeed":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.SeedValue", "" + ((IntegerValue)simulationEvent["SeedValue"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "NewObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ID", "" + ((StringValue)simulationEvent["ID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectType", "" + ((StringValue)simulationEvent["ObjectType"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, "NewObject", "StateTable", simulationEvent["StateTable"]);
//                        ConnectLink(controller, runID, componentID, "NewObject", "Attributes", simulationEvent["Attributes"]);
//                        break;
//                    case "RevealObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, "RevealObject", "Attributes", simulationEvent["Attributes"]);    
//                        break;
//                    case "SubplatformLaunch":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, "SubplatformLaunch", "LaunchDestinationLocation", simulationEvent["LaunchDestinationLocation"]);
//                        break;
//                    case "History_SubplatformLaunch":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsWeaponLaunch", "" + ((BooleanValue)simulationEvent["IsWeaponLaunch"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, "History_SubplatformLaunch", "LaunchDestinationLocation", simulationEvent["LaunchDestinationLocation"]);
//                        ConnectLink(controller, runID, componentID, "History_SubplatformLaunch", "ParentObjectLocation", simulationEvent["ParentObjectLocation"]);
//                        break;
//                    case "History_Pursue":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "ObjectLocation", simulationEvent["ObjectLocation"]);
//                        ConnectLink(controller, runID, componentID, eventType, "TargetObjectLocation", simulationEvent["TargetObjectLocation"]);
//                        break;
//                    case "WeaponLaunch":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "SubplatformDock":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ObjectCollision":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID1", "" + ((StringValue)simulationEvent["ObjectID1"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID2", "" + ((StringValue)simulationEvent["ObjectID2"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "MoveObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Throttle", "" + ((DoubleValue)simulationEvent["Throttle"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, "MoveObject", "DestinationLocation", simulationEvent["DestinationLocation"]);
//                        break;
//                    case "TransferObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DonorUserID", "" + ((StringValue)simulationEvent["DonorUserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "MoveDone":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Reason", "" + ((StringValue)simulationEvent["Reason"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ClientAttackRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerID", "" + ((StringValue)simulationEvent["PlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.AttackingObjectID", "" + ((StringValue)simulationEvent["AttackingObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.WeaponOrCapabilityName", "" + ((StringValue)simulationEvent["WeaponOrCapabilityName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "AttackObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.CapabilityName", "" + ((StringValue)simulationEvent["CapabilityName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "AttackSucceeded":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetID", "" + ((StringValue)simulationEvent["TargetID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.NewState", "" + ((StringValue)simulationEvent["NewState"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Capabilities", simulationEvent["Capabilities"]);
//                        break;
//                    case "History_AttackedObjectReport":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.AttackSuccess", "" + ((BooleanValue)simulationEvent["AttackSuccess"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.NewState", "" + ((StringValue)simulationEvent["NewState"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "ObjectLocation", simulationEvent["ObjectLocation"]);
//                        break;
//                    case "History_AttackerObjectReport":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.CapabilityName", "" + ((StringValue)simulationEvent["CapabilityName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "ObjectLocation", simulationEvent["ObjectLocation"]);
//                        ConnectLink(controller, runID, componentID, eventType, "TargetObjectLocation", simulationEvent["TargetObjectLocation"]);
//                        break;
//                    case "SelfDefenseAttackStarted":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.AttackerObjectID", "" + ((StringValue)simulationEvent["AttackerObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "StateChange":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.NewState", "" + ((StringValue)simulationEvent["NewState"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ViewProAttributeUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayer", "" + ((StringValue)simulationEvent["TargetPlayer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.OwnerID", "" + ((StringValue)simulationEvent["OwnerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Attributes", simulationEvent["Attributes"]);
//                        break;
//                    case "ViewProActiveRegionUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsVisible", "" + ((BooleanValue)simulationEvent["IsVisible"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DisplayColor", "" + ((IntegerValue)simulationEvent["DisplayColor"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Shape", simulationEvent["Shape"]);
//                        break;
//                    case "ActiveRegionSpeedMultiplierUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ViewProInitializeObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayer", "" + ((StringValue)simulationEvent["TargetPlayer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IconName", "" + ((StringValue)simulationEvent["IconName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.OwnerID", "" + ((StringValue)simulationEvent["OwnerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsWeapon", "" + ((BooleanValue)simulationEvent["IsWeapon"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.LabelColor", "" + ((IntegerValue)simulationEvent["LabelColor"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Location", simulationEvent["Location"]);
//                        break;
//                    case "ViewProMotionUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayer", "" + ((StringValue)simulationEvent["TargetPlayer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.OwnerID", "" + ((StringValue)simulationEvent["OwnerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.MaximumSpeed", "" + ((DoubleValue)simulationEvent["MaximumSpeed"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Throttle", "" + ((DoubleValue)simulationEvent["Throttle"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IconName", "" + ((StringValue)simulationEvent["IconName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsWeapon", "" + ((BooleanValue)simulationEvent["IsWeapon"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Location", simulationEvent["Location"]);
//                        ConnectLink(controller, runID, componentID, eventType, "DestinationLocation", simulationEvent["DestinationLocation"]);
//                        break;
//                    case "ViewProAttackUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.AttackerID", "" + ((StringValue)simulationEvent["AttackerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetID", "" + ((StringValue)simulationEvent["TargetID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.AttackEndTime", "" + ((IntegerValue)simulationEvent["AttackEndTime"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ViewProStopObjectUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "Playfield":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.MapDataFile", "" + ((StringValue)simulationEvent["MapDataFile"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IconLibrary", "" + ((StringValue)simulationEvent["IconLibrary"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UTMZone", "" + ((StringValue)simulationEvent["UTMZone"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UTMNorthing", "" + ((DoubleValue)simulationEvent["UTMNorthing"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UTMEasting", "" + ((DoubleValue)simulationEvent["UTMEasting"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.HorizontalScale", "" + ((DoubleValue)simulationEvent["HorizontalScale"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VerticalScale", "" + ((DoubleValue)simulationEvent["VerticalScale"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Name", "" + ((StringValue)simulationEvent["Name"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Description", "" + ((StringValue)simulationEvent["Description"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "MoveObjectRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Throttle", "" + ((DoubleValue)simulationEvent["Throttle"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "DestinationLocation", simulationEvent["DestinationLocation"]);
//                        break;
//                    case "AttackObjectRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.CapabilityName", "" + ((StringValue)simulationEvent["CapabilityName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "SubplatformLaunchRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "LaunchDestinationLocation", simulationEvent["LaunchDestinationLocation"]);
//                        break;
//                    case "SubplatformDockRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "WeaponLaunchRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ParentObjectID", "" + ((StringValue)simulationEvent["ParentObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetObjectID", "" + ((StringValue)simulationEvent["TargetObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "TextChatRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ChatBody", "" + ((StringValue)simulationEvent["ChatBody"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ChatType", "" + ((StringValue)simulationEvent["ChatType"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetUserID", "" + ((StringValue)simulationEvent["TargetUserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "TransferObjectRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.RecipientID", "" + ((StringValue)simulationEvent["RecipientID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.State", "" + ((StringValue)simulationEvent["State"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "TextChat":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChatBody", "" + ((StringValue)simulationEvent["ChatBody"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UserID", "" + ((StringValue)simulationEvent["UserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetUserID", "" + ((StringValue)simulationEvent["TargetUserID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "SystemMessage":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerID", "" + ((StringValue)simulationEvent["PlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Message", "" + ((StringValue)simulationEvent["Message"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TextColor", "" + ((IntegerValue)simulationEvent["TextColor"]).value, eParamParentType.Component);
//                        //controller.UpdateParameters(componentID, "Event Parameters.DisplayStyle", "" + ((StringValue)simulationEvent["DisplayStyle"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "AuthenticationRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Username", "" + ((StringValue)simulationEvent["Username"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Password", "" + ((StringValue)simulationEvent["Password"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "AuthenticationResponse":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Success", "" + ((BooleanValue)simulationEvent["Success"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Message", "" + ((StringValue)simulationEvent["Message"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "HandshakeGUIRegister":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "HandshakeAvailablePlayers":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetTerminalID", "" + ((StringValue)simulationEvent["TargetTerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "Players", simulationEvent["Players"]);
//                        ConnectLink(controller, runID, componentID, eventType, "AvailablePlayers", simulationEvent["AvailablePlayers"]);
//                        ConnectLink(controller, runID, componentID, eventType, "TakenPlayers", simulationEvent["TakenPlayers"]);
//                        break;
//                    case "HandshakeGUIRoleRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerID", "" + ((StringValue)simulationEvent["PlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "HandshakeInitializeGUI":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerID", "" + ((StringValue)simulationEvent["PlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ScenarioInfo", "" + ((StringValue)simulationEvent["ScenarioInfo"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ScenarioName", "" + ((StringValue)simulationEvent["ScenarioName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ScenarioDescription", "" + ((StringValue)simulationEvent["ScenarioDescription"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.MapName", "" + ((StringValue)simulationEvent["MapName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UTMNorthing", "" + ((DoubleValue)simulationEvent["UTMNorthing"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.UTMEasting", "" + ((DoubleValue)simulationEvent["UTMEasting"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.HorizontalPixelsPerMeter", "" + ((DoubleValue)simulationEvent["HorizontalPixelsPerMeter"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VerticalPixelsPerMeter", "" + ((DoubleValue)simulationEvent["VerticalPixelsPerMeter"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerBrief", "" + ((StringValue)simulationEvent["PlayerBrief"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IconLibrary", "" + ((StringValue)simulationEvent["IconLibrary"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VoiceChatEnabled", "" + ((BooleanValue)simulationEvent["VoiceChatEnabled"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VoiceChatServerName", "" + ((StringValue)simulationEvent["VoiceChatServerName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VoiceChatServerPort", "" + ((IntegerValue)simulationEvent["VoiceChatServerPort"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.VoiceChatUserPassword", "" + ((StringValue)simulationEvent["VoiceChatUserPassword"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "HandshakeInitializeGUIDone":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.PlayerID", "" + ((StringValue)simulationEvent["PlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ClientRemoveObject":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayer", "" + ((StringValue)simulationEvent["TargetPlayer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ExternalApp_Message":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.AppName", "" + ((StringValue)simulationEvent["AppName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Message", "" + ((StringValue)simulationEvent["Message"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ExternalApp_Log":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.AppName", "" + ((StringValue)simulationEvent["AppName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.LogEntry", "" + ((StringValue)simulationEvent["LogEntry"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "DisconnectDecisionMaker":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "DisconnectTerminal":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.TerminalID", "" + ((StringValue)simulationEvent["TerminalID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ExternalApp_SimStart":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ExternalApp_SimStop":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "PauseScenario":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ResumeScenario":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "StopScenario":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "StopReplay":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        _simulationComplete = true;
//                        break;
//                    case "GameSpeed":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.SpeedFactor", "" + ((DoubleValue)simulationEvent["SpeedFactor"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "PlayerControl":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ControlledBy", "" + ((StringValue)simulationEvent["ControlledBy"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RequestChatRoomCreate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.SenderDM_ID", "" + ((StringValue)simulationEvent["SenderDM_ID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "MembershipList", simulationEvent["MembershipList"]);
//                        break;
//                    case "CreateChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "MembershipList", simulationEvent["MembershipList"]);
//                        break;
//                    case "AddToChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.TargetPlayerID", "" + ((StringValue)simulationEvent["TargetPlayerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RequestCloseChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.SenderDM_ID", "" + ((StringValue)simulationEvent["SenderDM_ID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "CloseChatRoom":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.RoomName", "" + ((StringValue)simulationEvent["RoomName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "CreateVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "JoinVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RequestJoinVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "LeaveVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RequestLeaveVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "AddToVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.NewAccessor", "" + ((StringValue)simulationEvent["NewAccessor"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "RemoveFromVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DeletedPlayer", "" + ((StringValue)simulationEvent["DeletedPlayer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "CloseVoiceChannel":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "PushToTalk":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Speaker", "" + ((StringValue)simulationEvent["Speaker"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "PlayVoiceMessage":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.Channel", "" + ((StringValue)simulationEvent["Channel"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.File", "" + ((StringValue)simulationEvent["File"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "StoppedTalking":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ChannelName", "" + ((StringValue)simulationEvent["ChannelName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Speaker", "" + ((StringValue)simulationEvent["Speaker"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ScoreUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ScoreName", "" + ((StringValue)simulationEvent["ScoreName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.ScoreValue", "" + ((DoubleValue)simulationEvent["ScoreValue"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ExternalEmailReceived":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.FromAddress", "" + ((StringValue)simulationEvent["FromAddress"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.WallClockTime", "" + ((StringValue)simulationEvent["WallClockTime"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Subject", "" + ((StringValue)simulationEvent["Subject"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Body", "" + ((StringValue)simulationEvent["Body"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        ConnectLink(controller, runID, componentID, eventType, "ToAddresses", simulationEvent["ToAddresses"]);
//                        ConnectLink(controller, runID, componentID, eventType, "Attachments", simulationEvent["Attachments"]);
//                        break;
//                    case "ActiveRegionUpdate":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.ObjectID", "" + ((StringValue)simulationEvent["ObjectID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsVisible", "" + ((BooleanValue)simulationEvent["IsVisible"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.IsActive", "" + ((BooleanValue)simulationEvent["IsActive"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "EngramValue":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.EngramName", "" + ((StringValue)simulationEvent["EngramName"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.SpecificUnit", "" + ((StringValue)simulationEvent["SpecificUnit"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.EngramValue", "" + ((StringValue)simulationEvent["EngramValue"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.EngramDataType", "" + ((StringValue)simulationEvent["EngramDataType"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ClientSideAssetTransferAllowed":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.EnableAssetTransfer", "" + ((BooleanValue)simulationEvent["EnableAssetTransfer"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "ChangeTagRequest":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UnitID", "" + ((StringValue)simulationEvent["UnitID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.DecisionMakerID", "" + ((StringValue)simulationEvent["DecisionMakerID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Tag", "" + ((StringValue)simulationEvent["Tag"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                    case "UpdateTag":
//                        componentID = controller.CreateComponent(eventType, eventType, "No desc");
//                        controller.Connect(runID, runID, componentID, "Events");
//                        controller.UpdateParameters(componentID, "Event Parameters.UnitID", "" + ((StringValue)simulationEvent["UnitID"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Tag", "" + ((StringValue)simulationEvent["Tag"]).value, eParamParentType.Component);
//                        controller.UpdateParameters(componentID, "Event Parameters.Time", "" + ((IntegerValue)simulationEvent["Time"]).value, eParamParentType.Component);
//                        break;
//                }
//            }
//            // this isn't very good form, but configuration.xml and this generated code
//            // run off the NEW SimulationModel, which will cause crashes e.g.
//            // key not found exceptions, just ignore them for
//            // now until we update the running version of the DDD...
//            catch (ApplicationException e1) 
//            {
//                Console.WriteLine("Exception: "+e1.Message);
//            }
//            catch (KeyNotFoundException e2)
//            {
//                Console.WriteLine("Exception: " + e2.Message);
//            }
//            catch (Exception e3)
//            {
//                // Likely GME - check exception and stack trace!
//                Console.WriteLine("Exception: " + e3.Message);
//                Console.WriteLine(e3.StackTrace);
//            }

//            //Console.WriteLine(SimulationEventFactory.XMLSerialize((SimulationEvent)simulationEvent));

//            //three events that always need to be subscribed to are:
//            // ExternalApp_SimStart -> Sim is starting
//            // ExternalApp_SimStop  -> Sim is stopping
//            // StopReplay           -> Replay is stopping

//            //string eventName = ((SimulationEvent)simulationEvent).eventType;
//            //if (eventName == "ExternalApp_SimStop" ||
//            //        eventName == "StopReplay")
//            //{
//            //    _simulationComplete = true;
//            //}
//        }

//        private void ConnectLink(Controller controller, int runID, int linkedComponentID, string fromComponentType, string parameterName, DataValue parameter)
//        {
//            String parameterType = string.Empty;
//            String componentName = string.Empty;
//            Boolean isComplexType = true; //easier to set to false if its simple, as there is a finite set of simple data types
//            int newComponentID;// = controller.CreateComponent(castValue, castValue, "no desc");

//            switch (parameter.dataType)
//            {

//                case "StringType":
//                    parameterType = "System.String";
//                    isComplexType = false;
//                    break;
//                case "IntegerType":
//                    parameterType = "System.Int32";
//                    isComplexType = false;
//                    break;
//                case "BooleanType":
//                    parameterType = "System.Boolean";
//                    isComplexType = false;
//                    break;
//                case "DoubleType":
//                    parameterType = "System.Double";
//                    isComplexType = false;
//                    break;


//                case "StateTableType":
//                    componentName = "StateTableDictionary";
//                    break;
//                case "AttributeCollectionType":
//                    componentName = "AttributeCollectionDictionary";
//                    break;
//                case "LocationType":
//                    componentName = "LocationValue";
//                    break;
//                case "PolygonType":
//                    componentName = "PolygonList";
//                    break;
//                case "StringListType":
//                    componentName = "StringList";
//                    break;
//                //might need more definitions here
//                case "VelocityType":
//                    componentName = "VelocityValue";
//                    break;
//                case "CapabilityType":
//                    componentName = "CapabilityValue";
//                    break;
//                case "VulnerabilityType":
//                    componentName = "VulnerabilityValue";
//                    break;
//                case "EmitterType":
//                    componentName = "EmitterValue";
//                    break;
//                case "ConeType":
//                    componentName = "ConeValue";
//                    break;
//                case "SensorArrayType":
//                    componentName = "SensorArrayValue";
//                    break;
//                case "SensorType":
//                    componentName = "SensorValue";
//                    break;
//                case "CustomAttributesType":
//                    componentName = "CustomAttributesValue";
//                    break;
//                default:
//                    componentName = parameter.dataType;
                    
//                    break;
//    }

//            if (!isComplexType)
//            {

//                newComponentID = controller.CreateComponent("SimpleDataValue", parameterName, "no desc");
//                //create link between new component and linkedComponent
//                controller.Connect(runID, linkedComponentID, newComponentID, "Events");

//                //update  parameters
//                controller.UpdateParameters(newComponentID, "Event Parameters.Key", parameterName, eParamParentType.Component);
//                controller.UpdateParameters(newComponentID, "Event Parameters.Type", parameterType, eParamParentType.Component);
//                switch (parameter.dataType)
//                {
//                    case "StringType":
//                        controller.UpdateParameters(newComponentID, "Event Parameters.Value", ((StringValue)parameter).value.ToString(), eParamParentType.Component);
//                        break;
//                    case "IntegerType":
//                        controller.UpdateParameters(newComponentID, "Event Parameters.Value", ((IntegerValue)parameter).value.ToString(), eParamParentType.Component);
//                        break;
//                    case "BooleanType":
//                        controller.UpdateParameters(newComponentID, "Event Parameters.Value", ((BooleanValue)parameter).value.ToString(), eParamParentType.Component);
//                        break;
//                    case "DoubleType":
//                        controller.UpdateParameters(newComponentID, "Event Parameters.Value", ((DoubleValue)parameter).value.ToString(), eParamParentType.Component);
//                        break;
//                    default:
//                        Console.WriteLine("Invalid simple data type.");
//                        break;
//}

//                return; //that's it for simple types.
//            }
//            else
//            {//for complex types

//                newComponentID = controller.CreateComponent("ComplexDataValue", "ComplexDataValue", "no desc");
//                controller.Connect(runID, linkedComponentID, newComponentID, "Events");

//                //update  parameters
//                controller.UpdateParameters(newComponentID, "Event Parameters.Key", "ComplexDataValue", eParamParentType.Component);
//                controller.UpdateParameters(newComponentID, "Event Parameters.Type", componentName, eParamParentType.Component);

//                int complexComponentID = controller.CreateComponent(componentName, String.Format("{0} ({1})", componentName,parameterName), "desc?");
//                controller.Connect(runID, newComponentID, complexComponentID, "Events");


//                switch (componentName)
//                {

//                    #region ComplexTypes
//                    case "LocationValue":
//                        if (((LocationValue)parameter).exists)
//                        {
//                            controller.UpdateParameters(complexComponentID, "Event Parameters.X", ((LocationValue)parameter).X.ToString(), eParamParentType.Component);
//                            controller.UpdateParameters(complexComponentID, "Event Parameters.Y", ((LocationValue)parameter).Y.ToString(), eParamParentType.Component);
//                            controller.UpdateParameters(complexComponentID, "Event Parameters.Z", ((LocationValue)parameter).Z.ToString(), eParamParentType.Component);
//                        }
//                        controller.UpdateParameters(complexComponentID, "Event Parameters.Exists", ((LocationValue)parameter).exists.ToString(), eParamParentType.Component);

//                        break;
//                    case "StringList":
//                        StringListValue slv = (StringListValue)parameter;
//                        int listComponentID;

//                        foreach (string s in slv.strings)
//                        {//TODO:different links for simple vs complex parameters...
//                            listComponentID = controller.CreateComponent("StringListValue", "StringListValue", "desc?");
//                            controller.UpdateParameters(listComponentID, "Event Parameters.Value", s, eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, listComponentID, "Events");

//                            //dont recur, need to manually create components here.
//                            //ConnectLink(controller, runID, complexComponentID, componentName, parameterName + ".Value", DataValueFactory.BuildString(s));
//                        }
//                        break;
//                    case "AttributeCollectionDictionary":
//                        AttributeCollectionValue acv = (AttributeCollectionValue)parameter;
//                        foreach (string key in acv.attributes.Keys)
//                        {
//                            ConnectLink(controller, runID, complexComponentID, componentName, key, acv.attributes[key]);
//                        }
//                        break;
//                    case "CustomAttributesValue":
//                        CustomAttributesValue cav = (CustomAttributesValue)parameter;
//                        foreach (string key in cav.attributes.Keys)
//                        {
//                            ConnectLink(controller, runID, complexComponentID, componentName, key, cav.attributes[key]);
//                        }
//                        break;
//                    case "StateTableDictionary":
//                        StateTableValue stv = (StateTableValue)parameter;
//                        foreach (string stateName in stv.states.Keys)
//                        {
//                            ConnectLink(controller, runID, complexComponentID, componentName, parameterName + "." + stateName, stv.states[stateName]);
//                        }
//                        break;
//                    case "PolygonList":
//                        PolygonValue pv = (PolygonValue)parameter;
//                        int pointComponentID;
//                        foreach (PolygonValue.PolygonPoint pp in pv.points)
//                        {
//                            pointComponentID = controller.CreateComponent("PolygonPoint", "PolygonPoint", "desc?");
//                            controller.UpdateParameters(pointComponentID, "Event Parameters.X", pp.X.ToString(), eParamParentType.Component);
//                            controller.UpdateParameters(pointComponentID, "Event Parameters.Y", pp.Y.ToString(), eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, pointComponentID, "Events");

//                            //ConnectLink(controller, runID, complexComponentID, componentName, parameterName + ".Point", pp);//PolygonPoint might not be a DataValue...
//                        }
//                        break;
//                    //case "PolygonPoint": 
//                    //    PolygonValue.PolygonPoint pp = parameter;
//                    //    controller.UpdateParameters(complexComponentID, "X", pp.X.ToString(), eParamParentType.Component);
//                    //    controller.UpdateParameters(complexComponentID, "Y", pp.Y.ToString(), eParamParentType.Component);
//                    //    break;
//                    case "CapabilityValue":
//                        //this could have the same issue as polygons, where effects might not be datavalues.
//                        CapabilityValue cv = (CapabilityValue)parameter;
//                        int effectComponentID;
//                        foreach (CapabilityValue.Effect ef in cv.effects)
//                        {
//                            effectComponentID = controller.CreateComponent("Effect", "Effect", "desc?");
//                            controller.UpdateParameters(effectComponentID, "Event Parameters.Name", ef.name, eParamParentType.Component);
//                            controller.UpdateParameters(effectComponentID, "Event Parameters.Range", ef.range.ToString(), eParamParentType.Component);
//                            controller.UpdateParameters(effectComponentID, "Event Parameters.Intensity", ef.intensity.ToString(), eParamParentType.Component);
//                            controller.UpdateParameters(effectComponentID, "Event Parameters.Probability", ef.probability.ToString(), eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, effectComponentID, "Events");
//                           // ConnectLink(controller, runID, complexComponentID, componentName, parameterName + ".Effect", ef);//Effect might not be a DataValue...                       
//                        }
//                        break;
//                    //case "Effect": //??
//                    //break;

//                    case "VulnerabilityValue":
//                        VulnerabilityValue vv = (VulnerabilityValue)parameter;
//                        int transitionComponentID;
//                        int conditionComponentID;
//                        int effectAppliedComponentID;
//                        foreach (VulnerabilityValue.Transition tr in vv.transitions)
//                        {
//                            transitionComponentID = controller.CreateComponent("Transition", "Transition", "desc?");
//                            controller.UpdateParameters(transitionComponentID, "Event Parameters.State", tr.state, eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, transitionComponentID, "Events");

//                            foreach (VulnerabilityValue.TransitionCondition tc in tr.conditions)
//                            {
//                                conditionComponentID = controller.CreateComponent("TransitionCondition", "TransitionCondition", "desc?");
//                                controller.UpdateParameters(conditionComponentID, "Event Parameters.Capability", tc.capability, eParamParentType.Component);
//                                controller.UpdateParameters(conditionComponentID, "Event Parameters.Effect", tc.effect.ToString(), eParamParentType.Component);
//                                controller.UpdateParameters(conditionComponentID, "Event Parameters.Range", tc.range.ToString(), eParamParentType.Component);
//                                controller.UpdateParameters(conditionComponentID, "Event Parameters.Probability", tc.probability.ToString(), eParamParentType.Component);
//                                controller.Connect(runID, transitionComponentID, conditionComponentID, "Events");

//                                foreach (VulnerabilityValue.EffectApplied ea in tc.effectsApplied)
//                                {
//                                    effectAppliedComponentID = controller.CreateComponent("EffectApplied", "EffectApplied", "desc?");
//                                    controller.UpdateParameters(effectAppliedComponentID, "Event Parameters.Effect", ea.effect.ToString(), eParamParentType.Component);
//                                    controller.UpdateParameters(effectAppliedComponentID, "Event Parameters.RangeApplied", ea.rangeApplied.ToString(), eParamParentType.Component);
//                                    controller.Connect(runID, conditionComponentID, effectAppliedComponentID, "Events");                               
//                                }
//                            }
//                        }
//                        break;
//                    case "VelocityValue":
//                        VelocityValue velVal = (VelocityValue)parameter;
//                        controller.UpdateParameters(complexComponentID, "Event Parameters.VX", velVal.VX.ToString(), eParamParentType.Component);
//                        controller.UpdateParameters(complexComponentID, "Event Parameters.VY", velVal.VY.ToString(), eParamParentType.Component);
//                        controller.UpdateParameters(complexComponentID, "Event Parameters.VZ", velVal.VZ.ToString(), eParamParentType.Component);
//                        break;

//                    case "EmitterValue":
//                        EmitterValue ev = (EmitterValue)parameter;
//                        int isEngramComponentID;
//                        int emitDoubleComponentID;
//                        //controller.UpdateParameters(complexComponentID, "VX", velVal.VX.ToString(), eParamParentType.Component);
//                        foreach (string attName in ev.attIsEngram.Keys)
//                        {
//                            isEngramComponentID = controller.CreateComponent("AttIsEngramBoolean", "AttIsEngramBoolean", "desc?");
//                            controller.UpdateParameters(isEngramComponentID, "Event Parameters.Key", attName, eParamParentType.Component);
//                            controller.UpdateParameters(isEngramComponentID, "Event Parameters.Value", ev.attIsEngram[attName].ToString(), eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, isEngramComponentID, "Events");
//                        }
//                        //TODO: I don't think this is right...
//                        foreach (string attName in ev.emitters.Keys)
//                        {
//                            foreach (string level in ev.emitters[attName].Keys)
//                            {
//                                emitDoubleComponentID = controller.CreateComponent("EmitterDouble", "EmitterDouble", "desc?");
//                                controller.UpdateParameters(emitDoubleComponentID, "Event Parameters.Key", level, eParamParentType.Component);
//                                controller.UpdateParameters(emitDoubleComponentID, "Event Parameters.Value", ev.emitters[attName][level].ToString(), eParamParentType.Component);
//                                controller.Connect(runID, complexComponentID, emitDoubleComponentID, "Events");
//                            }
//                        }
//                        break;

//                    case "SensorArrayValue"://stopped here.
//                        SensorArrayValue sav = (SensorArrayValue)parameter;
//                        int sensorComponentID;
//                        int coneListComponentID;
//                        int coneValueComponentID;
//                        int attEngramComponentID;
//                        foreach (SensorValue sv in sav.sensors)
//                        {
//                            sensorComponentID = controller.CreateComponent("SensorValue", "SensorValue", "desc?");
//                            controller.UpdateParameters(sensorComponentID, "Event Parameters.SensorName", sv.sensorName, eParamParentType.Component);
//                            controller.UpdateParameters(sensorComponentID, "Event Parameters.MaxRange", sv.maxRange.ToString(), eParamParentType.Component);
//                            controller.Connect(runID, complexComponentID, sensorComponentID, "Events");

//                            foreach (string attName in sv.attIsEngram.Keys)
//                            {
//                                attEngramComponentID = controller.CreateComponent("AttIsEngramBoolean", "AttIsEngramBoolean", "desc?");
//                                controller.UpdateParameters(attEngramComponentID, "Event Parameters.Key", attName, eParamParentType.Component);
//                                controller.UpdateParameters(attEngramComponentID, "Event Parameters.Value", sv.attIsEngram[attName].ToString(), eParamParentType.Component);
//                                controller.Connect(runID, sensorComponentID, attEngramComponentID, "Events");
//                            }

//                            foreach (string range in sv.ranges.Keys)
//                            {
//                                coneListComponentID = controller.CreateComponent("ConeListKey", "ConeListKey", "desc?");
//                                controller.UpdateParameters(coneListComponentID, "Event Parameters.Key", range, eParamParentType.Component);
//                                controller.Connect(runID, sensorComponentID, coneListComponentID, "Events");

//                                foreach (ConeValue cone in sv.ranges[range])
//                                {
//                                    coneValueComponentID = controller.CreateComponent("ConeValue", "ConeValue", "desc?");
//                                    controller.UpdateParameters(coneValueComponentID, "Event Parameters.Level", cone.level, eParamParentType.Component);
//                                    controller.UpdateParameters(coneValueComponentID, "Event Parameters.Extent", cone.extent.ToString(), eParamParentType.Component);
//                                    controller.UpdateParameters(coneValueComponentID, "Event Parameters.Spread", cone.spread.ToString(), eParamParentType.Component);
//                                    controller.Connect(runID, coneListComponentID, coneValueComponentID, "Events");
//                                }
//                            }
                        
//                        }
//                        break;
////                    case "SensorValue":
////break;
////                    case "ConeValue":

////                        break;                        

//                    #endregion
//                    default:
//                        //ConnectLink(controller, runID, componentID, castValue, dv);
//                        break;
//                }
//            }

//            //if simple

//            //determine "type"

//            //create SimpleDataValue component

//            //link new component to linkedComponentID



//            //////



//            //if complex

//            //determine type

//            //create ComplexDataValue component
             
//            //link new component to linkedComponentID

//            //based on complex type, recursively call this.













//            return;
//            /*
//            //connect new component.
//            String castValue = "";
//            String eventParameterType = "";
//            switch (parameter.dataType)
//            {
//                case "StringType":
//                    eventParameterType = "System.String";
//                    castValue = "StringValue";
//                    break;
//                case "IntegerType":
//                    eventParameterType = "System.Int32";
//                    castValue = "IntegerValue";
//                    break;
//                case "BooleanType":
//                    eventParameterType = "System.Boolean";
//                    castValue = "BooleanValue";
//                    break;
//                case "DoubleType":
//                    eventParameterType = "System.Double";
//                    castValue = "DoubleValue";
//                    break;
//                case "StateTableType":
//                    castValue = "StateTableValue";
//                    break;
//                case "AttributeCollectionType":
//                    castValue = "AttributeCollectionDictionary";
//                    break;
//                case "LocationType":
//                    castValue = "LocationValue";
//                    break;
//                case "PolygonType":
//                    castValue = "PolygonValue";
//                    break;
//                case "StringListType":
//                    castValue = "StringListValue";
//                    break;
//                    //might need more definitions here
//                default:
//                    castValue = parameter.dataType;
                    
//                    break;
//            }


//            int componentID = controller.CreateComponent(castValue, castValue, "no desc");

//            //create link between new component and linkedComponent
//            controller.Connect(runID, linkedComponentID, componentID, "Events");

//            //update new component values
//            switch (castValue)
//            {
//                #region SimpleTypes
//                case "StringValue":
//                    controller.UpdateParameters(componentID, parameterName, ((StringValue)parameter).value, eParamParentType.Component);
//                    //eventParameterType = "System.String";
//                    break;
//                case "IntegerValue":
//                    controller.UpdateParameters(componentID, parameterName, ((IntegerValue)parameter).value.ToString(), eParamParentType.Component);
//                    //eventParameterType = "System.Int32";
//                    break;
//                case "BooleanValue":
//                    controller.UpdateParameters(componentID, parameterName, ((BooleanValue)parameter).value.ToString(), eParamParentType.Component);
//                    //eventParameterType = "System.Boolean";
//                    break;
//                case "DoubleValue":
//                    controller.UpdateParameters(componentID, parameterName, ((DoubleValue)parameter).value.ToString(), eParamParentType.Component);
//                   // eventParameterType = "System.Double";
//                    break;
//                #endregion
//                #region ComplexTypes
//                case "LocationValue":
//                    if (((LocationValue)parameter).exists)
//                    {
//                        controller.UpdateParameters(componentID, parameterName + ".X", DataValueFactory.BuildDouble(((LocationValue)parameter).X).ToString(), eParamParentType.Component);
//                        controller.UpdateParameters(componentID, parameterName + ".Y", DataValueFactory.BuildDouble(((LocationValue)parameter).Y).ToString(), eParamParentType.Component);
//                        controller.UpdateParameters(componentID, parameterName + ".Z", DataValueFactory.BuildDouble(((LocationValue)parameter).Z).ToString(), eParamParentType.Component);
//                    }
//                    controller.UpdateParameters(componentID, parameterName + ".Exists", DataValueFactory.BuildBoolean(((LocationValue)parameter).exists).ToString(), eParamParentType.Component);

//                    break;
//                case "StringListValue":
//                    StringListValue slv = (StringListValue)parameter;
//                    foreach (string s in slv.strings)
//                    {//TODO:different links for simple vs complex parameters...
//                        ConnectLink(controller, runID, componentID, castValue, parameterName + ".Value", DataValueFactory.BuildString(s));
//                    }
//                    break;
//                case "AttributeCollectionDictionary":
//                    AttributeCollectionValue acv = (AttributeCollectionValue)parameter;
//                    foreach (string key in acv.attributes.Keys)
//                    {
//                        ConnectLink(controller, runID, componentID, castValue, parameterName + "." + key, acv.attributes[key]);
//                    }
//                    break;
//                case "StateTableValue":
//                    StateTableValue stv = (StateTableValue)parameter;
//                    foreach (string stateName in stv.states.Keys)
//                    {
//                        ConnectLink(controller, runID, componentID, castValue, parameterName + "." + stateName, acv.attributes[stateName]);
//                    }
//                    break;
//                case "PolygonValue":
//                    PolygonValue pv = (PolygonValue)parameter;
//                    foreach(PolygonValue.PolygonPoint pp in pv.points)
//                    {
//                        ConnectLink(controller, runID, componentID, castValue, parameterName + ".Point", pp);//PolygonPoint might not be a DataValue...
//                    }
//                    break;
//                    //case "PolygonPoint": //??
//                    //break;
//                case "CapabilityValue":
//                    //this could have the same issue as polygons, where effects might not be datavalues.
//                    CapabilityValue cv = (CapabilityValue)parameter;
//                    foreach (CapabilityValue.Effect ef in cv.effects)
//                    {
//                        ConnectLink(controller, runID, componentID, castValue, parameterName + ".Effect", ef);//Effect might not be a DataValue...                       
//                    }
//                    break;
//                //case "Effect": //??
//                    //break;

//                case "VulnerabilityValue":

//                    break;
//                #endregion
//                default:
//                    //ConnectLink(controller, runID, componentID, castValue, dv);
//                    break;
//            }

//            */
//        //if any component values are complex types, need to recursively call this.
//        }


//    }
//}
