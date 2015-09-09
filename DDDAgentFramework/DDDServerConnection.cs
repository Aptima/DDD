using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    /// <summary>
    /// The main object for connecting to the DDD and sending/receiving events.
    /// </summary>
    public class DDDServerConnection
    {
        /// <summary>
        /// The SessionStateType contains information about where the DDDServerConnection is in the login process.
        /// </summary>
        public enum SessionStateType
        {
            /// <summary>
            /// The DDDServerConnection is still waiting for available players.
            /// </summary>
            WAITING_FOR_PLAYERS,
            /// <summary>
            /// The DDDServerConnection has received the available players and is ready have the ProcessEvents method called.
            /// </summary>
            LOGGED_IN,
        }
        /// <summary>
        /// This delegate defines a method which will handle an incoming simulation event
        /// </summary>
        /// <param name="ev"></param>
        public delegate void ProcessSimulationEvent(SimulationEvent ev);
        private Dictionary<String, List<ProcessSimulationEvent>> _eventCallbacks;

        private SessionStateType m_sessionState = SessionStateType.WAITING_FOR_PLAYERS;
        private NetworkClient m_netClient = null;
        private SimulationModelInfo m_simModel = null;
        private String m_hostname;
        private int m_port;
        private String m_playerID;
        private String m_terminalID;
        private List<String> m_players = null;
        private String m_dddClientPath;

        private List<String> m_subscibedEvents = null;
        private List<SimulationEvent> m_eventQueue = null;

        private Dictionary<String, PolygonValue> m_regionDefinitions = null;
        private DMView m_groundTruthView;
        private Dictionary<String, DMView> m_dmViews;
        public static Object PublicDmViewLock = new object();
        private ScenarioData m_scenario = null;

        private int m_dddTimeInt;
        private String m_dddTimeString;

        public int DDDTimeInt
        {
            get { return m_dddTimeInt; }
            set { m_dddTimeInt = value; }
        }
        public String DDDTimeString
        {
            get { return m_dddTimeString; }
            set { m_dddTimeString = value; }
        }

        /// <summary>
        /// The name of the scenario that is currently running.
        /// </summary>
        public ScenarioData Scenario
        {
            get { return m_scenario; }
            set { m_scenario = value; }
        }
        /// <summary>
        /// Set this property the location of the DDDClient share directory on the server machine.
        /// <example>If the DDD Server is running on a machine caller DDDSERVER, than set this property to \\DDDSERVER\DDDClient.</example>
        /// </summary>
        public String DDDClientPath
        {
            set { m_dddClientPath = value; }
            get { return m_dddClientPath; }
        }

        /// <summary>
        /// The current place in the login process.
        /// </summary>
        public SessionStateType State
        {
            get
            {
                return m_sessionState;
            }
        }

        /// <summary>
        /// The auto-generated id used to identify this computer when logging into the DDD Server.
        /// </summary>
        public String TerminalID
        {
            get
            {
                return m_terminalID;
            }
        }

        /// <summary>
        /// The decision maker that has been selected by either the 
        /// </summary>
        public String PlayerID
        {
            get
            {
                return m_playerID;
            }
        }
        /// <summary>
        /// Get the list of decision makers in the DDD simulation.
        /// </summary>
        public List<String> Players
        {
            get
            {
                if (m_players != null)
                {
                    return m_players;
                }
                else
                {
                    return new List<String>();
                }
            }
        }


        public void ResetForNewSession()
        {
            m_players = new List<string>();
            lock (PublicDmViewLock)
            {
                m_dmViews = new Dictionary<string, DMView>();
            }
            m_subscibedEvents = new List<string>();
            m_eventQueue = new List<SimulationEvent>();
            m_dddClientPath = "";
            m_dddTimeInt = 0;
            m_dddTimeString = "0:00:00";
            m_playerID = "";
            m_regionDefinitions = new Dictionary<string, PolygonValue>();
            m_groundTruthView = new DMView("Ground_Truth");
        }

        /// <summary>
        /// The constructor for the DDDServerConnection object.
        /// </summary>
        /// 
        public DDDServerConnection()
        {
            m_subscibedEvents = new List<string>();
            m_eventQueue = new List<SimulationEvent>();
            m_netClient = new NetworkClient();
            m_players = new List<string>();
            lock (PublicDmViewLock)
            { m_dmViews = new Dictionary<string, DMView>(); }
            m_dddClientPath = "";
            m_dddTimeInt = 0;
            m_dddTimeString = "0:00:00";
            _eventCallbacks = new Dictionary<string, List<ProcessSimulationEvent>>();
            m_regionDefinitions = new Dictionary<string, PolygonValue>();
            m_groundTruthView = new DMView("Ground_Truth");
        }

        /// <summary>
        /// Subscribe to specific DDD events.  Must be called before ConnectToServer(hostname,port).
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        public void SubscribeToEvent(String eventName)
        {
            if (!m_subscibedEvents.Contains(eventName))
            {
                m_subscibedEvents.Add(eventName);
                if (IsConnected())
                {
                    m_netClient.Subscribe(eventName);
                }
            }

            
        }
        /// <summary>
        /// Call this method to connect to the DDD Server.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectToServer(string hostname, int port)
        {

            if (!m_netClient.Connect(hostname, port))
            {
                return false;
            }
            m_hostname = hostname;
            m_port = port;

            List<String> subscriptions = new List<string>();

            subscriptions.Add("HandshakeAvailablePlayers");
            subscriptions.Add("ViewProInitializeObject");
            subscriptions.Add("ViewProAttributeUpdate");
            subscriptions.Add("ViewProMotionUpdate");
            subscriptions.Add("ViewProAttackUpdate");
            subscriptions.Add("ViewProStopObjectUpdate");
            subscriptions.Add("ViewProActiveRegionUpdate");
            subscriptions.Add("AttackSucceeded");
            subscriptions.Add("HandshakeInitializeGUI");
            subscriptions.Add("TimeTick");
            subscriptions.Add("StateChange");

            foreach (String et in subscriptions)
            {
                m_netClient.Subscribe(et);
            }
            foreach (String et in m_subscibedEvents)
            {
                if (!subscriptions.Contains(et))
                {
                    m_netClient.Subscribe(et);
                }
            }
            m_terminalID = m_netClient.TerminalID;

            return true;
        }

        /// <summary>
        /// Get all the subscibed events that have arrived since the last time this method was called.
        /// </summary>
        /// <returns></returns>
        public List<SimulationEvent> GetEvents()
        {
            List<SimulationEvent> result = new List<SimulationEvent>(m_eventQueue);
            m_eventQueue.Clear();
            return result;
        }

        /// <summary>
        /// Call this method to disconnect from the DDD Server.
        /// </summary>
        public void Disconnect()
        {
            m_netClient.Disconnect();
        }


        /// <summary>
        /// Check to see if you are currently connected to the DDD Server.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_netClient.IsConnected();
        }

        /// <summary>
        /// Call this to read in the simulation configuration information that is located on the DDD Server machine.
        /// </summary>
        /// <returns></returns>
        public bool ReadSimModel()
        {
            return ReadSimModel(DDDClientPath + "\\SimulationModel.xml");
        }

        /// <summary>
        /// A helper function used by ReadSimModel().
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ReadSimModel(String path)
        {
            SimulationModelReader reader = new SimulationModelReader();
            try
            {
                m_simModel = reader.readModel(path);
                return true;
            }
            catch
            {
                m_simModel = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the instance of the sim model, for creating their own events.
        /// </summary>
        /// <returns></returns>
        public SimulationModelInfo GetSimModel()
        {
            return m_simModel;
        }

        private void ProcessSimEvent(SimulationEvent ev)
        {
            //Console.WriteLine(SimulationEventFactory.XMLSerialize(ev));

            switch (ev.eventType)
            {
                case "TimeTick":
                    DDDTimeString = ((StringValue)ev["SimulationTime"]).value;
                    DDDTimeInt = ((IntegerValue)ev["Time"]).value;
                    break;
            }
            lock (PublicDmViewLock)
            {
                foreach (String dm in m_dmViews.Keys)
                {
                    switch (ev.eventType)
                    {
                        case "ViewProInitializeObject":
                            if (((StringValue)ev["TargetPlayer"]).value == dm)
                            {
                                m_dmViews[dm].ViewProInitializeObject(ev);
                            }
                            break;
                        case "ViewProAttributeUpdate":
                            if (((StringValue)ev["TargetPlayer"]).value == dm)
                            {
                                m_dmViews[dm].ViewProAttributeUpdate(ev);
                            }
                            break;
                        case "ViewProMotionUpdate":

                            m_dmViews[dm].ViewProMotionUpdate(ev);

                            break;
                        case "ViewProAttackUpdate":
                            m_dmViews[dm].ViewProAttackUpdate(ev);
                            break;
                        case "ViewProStopObjectUpdate":
                            m_dmViews[dm].ViewProStopObjectUpdate(ev);
                            break;
                        case "ViewProActiveRegionUpdate":
                            m_dmViews[dm].ViewProActiveRegionUpdate(ev);
                            break;
                        case "AttackSucceeded":
                            m_dmViews[dm].AttackSucceeded(ev);
                            break;
                        case "TimeTick":
                            m_dmViews[dm].TimeTick(ev);
                            break;
                        case "StateChange":
                            m_dmViews[dm].StateChange(ev);
                            break;
                        default:
                            break;
                    }
                }
            }
            if (m_groundTruthView != null)
            {//TODO: Need to make sure that the object that the event is dealing with is owned by the "Target" DM
                switch (ev.eventType)
                {
                    case "ViewProInitializeObject":
                        if (((StringValue)ev["TargetPlayer"]).value == ((StringValue)ev["OwnerID"]).value)
                        {
                            m_groundTruthView.ViewProInitializeObject(ev);
                        }
                        break;
                    case "ViewProAttributeUpdate":
                        //if (((StringValue)ev["TargetPlayer"]).value == dm)
                        //{
                            m_groundTruthView.ViewProAttributeUpdate(ev);
                        //}
                        break;
                    case "ViewProMotionUpdate":

                        m_groundTruthView.ViewProMotionUpdate(ev);

                        break;
                    case "ViewProAttackUpdate":
                        m_groundTruthView.ViewProAttackUpdate(ev);
                        break;
                    case "ViewProStopObjectUpdate":
                        m_groundTruthView.ViewProStopObjectUpdate(ev);
                        break;
                    case "ViewProActiveRegionUpdate":
                        m_groundTruthView.ViewProActiveRegionUpdate(ev);
                        break;
                    case "AttackSucceeded":
                        m_groundTruthView.AttackSucceeded(ev);
                        break;
                    case "TimeTick":
                        m_groundTruthView.TimeTick(ev);
                        break;
                    case "StateChange":
                        m_groundTruthView.StateChange(ev);
                        break;
                    default:
                        break;
                }
            }

            if (_eventCallbacks.ContainsKey(ev.eventType))
            {
                foreach (ProcessSimulationEvent callback in _eventCallbacks[ev.eventType])
                {
                    callback(ev);
                }
            }
        }
        /// <summary>
        /// Call this on a periodic basis to receive events from the DDD Server and populate the SimObject objects contained in the DMView.
        /// This will also tell and PlayerAgents and ObjectControlAgents to update themselves.
        /// </summary>
        public void ProcessEvents()
        {
            List<SimulationEvent> events;

            events = m_netClient.GetEvents();
            //string playerID;
            foreach (SimulationEvent e in events)
            {
                switch (e.eventType)
                {
                    case "HandshakeAvailablePlayers":
                        StringListValue playerList = (StringListValue)e["Players"];
                        m_players = playerList.strings;
                        break;
                    case "HandshakeInitializeGUI":
                        if (((StringValue)e["PlayerID"]).value == PlayerID)
                        {
                            Scenario = new ScenarioData(e);

                        }
                        break;
                    default:
                        ProcessSimEvent(e);
                        break;
                }
                if (m_subscibedEvents.Contains(e.eventType))
                {
                    m_eventQueue.Add(e);
                }
            }
            foreach (String dm in m_dmViews.Keys)
            {
                m_dmViews[dm].ProjectObjectLocations();
            }
            UpdateAgents();


        }

        void UpdateAgents()
        {
            foreach (String dm in m_dmViews.Keys)
            {
                foreach (String id in m_dmViews[dm].AllObjects.Keys)
                {
                    //Console.Out.WriteLine(String.Format("DDDSeverConnection.UpdateAgents() ControlAgent.Update {0} {1}",dm,id));
                    m_dmViews[dm].AllObjects[id].ControlAgent.Update(this, m_dmViews[dm]);
                }
                if (m_dmViews[dm].PlayerAgent != null)
                {
                    //Console.Out.WriteLine(String.Format("DDDSeverConnection.UpdateAgents() PlayerAgent.Update {0}", dm));
                    m_dmViews[dm].PlayerAgent.Update();
                }
            }
        }

        /// <summary>
        /// Call this method to ask the DDD Server what players are available.  This initiates the login process.
        /// </summary>
        public void RequestPlayers()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "HandshakeGUIRegister");
            ((StringValue)(e["TerminalID"])).value = TerminalID;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Call this method to set which player you want to be the primary decision maker of this DDDServerConnection.  You can find out what players are available from The AvailablePlayers property.
        /// The method does not do a full DDD login and a DDDClient UI is still able to login as this player.
        /// </summary>
        /// <param name="dm"></param>
        public void SetLocalPlayer(String dm)
        {
            m_playerID = dm;
            m_sessionState = SessionStateType.LOGGED_IN;
        }
        /// <summary>
        /// This method completes a full DDD login.  Call this method if you want your application to replace the DDDClient for the specified decision maker.  
        /// No DDDClient UI will be able to login for this player.
        /// Call LoginFinished() until it returns true, indicating you have received the scenario initialization information, then call SendHandshakeInitializeGUIDone() to tell the DDD Server that you are done processing the initialization information.
        /// </summary>
        /// <param name="dm">The decision maker logging in.</param>
        /// <param name="loginType">Either FULL or OBSERVER. A decision maker can have only one client with a FULL login at a time, but unlimited OBSERVER logins.</param>
        public void LoginPlayer(String dm, String loginType)
        {
            SetLocalPlayer(dm);
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "HandshakeGUIRoleRequest");
            ((StringValue)(e["TerminalID"])).value = TerminalID;
            ((StringValue)(e["PlayerID"])).value = dm;
            ((StringValue)(e["LoginType"])).value = loginType;
            m_netClient.PutEvent(e);


            while (!LoginFinished())
            {
                ProcessEvents();
                Thread.Sleep(10);
            }

            SendHandshakeInitializeGUIDone(loginType);

        }
        /// <summary>
        /// This method completes a full DDD login.  Call this method if you want your application to replace the DDDClient for the specified decision maker.  
        /// No DDDClient UI will be able to login for this player.
        /// Call LoginFinished() until it returns true, indicating you have received the scenario initialization information, then call SendHandshakeInitializeGUIDone() to tell the DDD Server that you are done processing the initialization information.
        /// Defaults to FULL login.
        /// </summary>
        /// <param name="dm">The decision maker logging in.</param>
        public void LoginPlayer(String dm)
        {
            LoginPlayer(dm, "FULL");
        }

        /// <summary>
        /// This method will tell you if we have received the Scenario initialization information from the DDD Server.
        /// </summary>
        /// <returns></returns>
        public bool LoginFinished()
        {
            if (Scenario == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Tell the DDD Server that you are done processing the initialization information.  
        /// This is only necessary if you are completing a full login using the LoginPlayer(dm) instead of the SetLocalPlayer(dm) mothod.
        /// LoginType should be "FULL" or "OBSERVER".
        /// </summary>
        public void SendHandshakeInitializeGUIDone(string loginType)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "HandshakeInitializeGUIDone");
            ((StringValue)(e["PlayerID"])).value = PlayerID;
            ((StringValue)(e["LoginType"])).value = loginType;
            m_netClient.PutEvent(e);
        }


        /// <summary>
        /// Ask the DDD to move the specified object.  The decision maker that you are logged in as is provided automatically and will be used by the DDD Server to determine if you have authority to move this object.  If you want to move an object you don't own, use the version of this method that supplies a decision maker and supply the decision maker that owns the object.
        /// </summary>
        /// <param name="objectID">The object you want to move.</param>
        /// <param name="destination">Where</param>
        /// <param name="throttle"></param>
        public void SendMoveObjectRequest(String objectID, LocationValue destination, double throttle)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "MoveObjectRequest");
            ((StringValue)(e["UserID"])).value = PlayerID;
            ((StringValue)(e["ObjectID"])).value = objectID;
            e["DestinationLocation"] = destination;
            ((DoubleValue)(e["Throttle"])).value = throttle;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Ask the DDD to move the specified object.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="ownerID"></param>
        /// <param name="destination"></param>
        /// <param name="throttle"></param>
        public void SendMoveObjectRequest(String objectID, String ownerID, LocationValue destination, double throttle)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "MoveObjectRequest");
            ((StringValue)(e["UserID"])).value = ownerID;
            ((StringValue)(e["ObjectID"])).value = objectID;
            e["DestinationLocation"] = destination;
            ((DoubleValue)(e["Throttle"])).value = throttle;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Ask the DDD to initiate an engagement.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="capabilityName"></param>
        public void SendAttackObjectRequest(String objectID, String targetObjectID, String capabilityName)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "AttackObjectRequest");
            ((StringValue)(e["UserID"])).value = PlayerID;
            ((StringValue)(e["ObjectID"])).value = objectID;
            ((StringValue)(e["TargetObjectID"])).value = targetObjectID;
            ((StringValue)(e["CapabilityName"])).value = capabilityName;
            ((IntegerValue)(e["PercentageApplied"])).value = 100;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Ask the DDD to initiate an engagement.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="capabilityName"></param>
        /// <param name="percentageApplied">0 - 100, the percent of the effect to apply to the target</param>
        public void SendAttackObject(String objectID, String targetObjectID, String capabilityName, int percentageApplied)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "AttackObject");
            ((StringValue)(e["ObjectID"])).value = objectID;
            ((StringValue)(e["TargetObjectID"])).value = targetObjectID;
            ((StringValue)(e["CapabilityName"])).value = capabilityName;
            ((IntegerValue)(e["PercentageApplied"])).value = percentageApplied;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Ask the DDD to initiate a weapon launch.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="weaponID"></param>
        /// <param name="targetObjectID"></param>
        public void SendWeaponLaunchRequest(String parentID, String weaponID, String targetObjectID)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "WeaponLaunchRequest");
            ((StringValue)(e["UserID"])).value = PlayerID;
            ((StringValue)(e["ObjectID"])).value = weaponID;
            ((StringValue)(e["TargetObjectID"])).value = targetObjectID;
            ((StringValue)(e["ParentObjectID"])).value = parentID;
            m_netClient.PutEvent(e);
        }

        /// <summary>
        /// Tell the DDD to change the state of an object.
        /// </summary>
        /// <param name="obID"></param>
        /// <param name="newState"></param>
        public void SendStateChange(string obID, string newState)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "StateChange");
            ((StringValue)(e["ObjectID"])).value = obID;
            ((StringValue)(e["NewState"])).value = newState;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Ask the DDD to transfer ownership for the specified object.
        /// </summary>
        /// <param name="obID"></param>
        /// <param name="userID"></param>
        /// <param name="newOwner"></param>
        /// <param name="currentState"></param>
        public void SendTransferObjectRequest(string obID, String userID, String newOwner, String currentState)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "TransferObjectRequest");
            ((StringValue)(e["UserID"])).value = userID;
            ((StringValue)(e["ObjectID"])).value = obID;
            ((StringValue)(e["RecipientID"])).value = newOwner;
            ((StringValue)(e["State"])).value = currentState;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Tell the DDD to pause the scenario.  Will not work with DDD version before 4.1.
        /// </summary>
        public void SendPauseScenarioRequest()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "PauseScenarioRequest");
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Tell the DDD to resume the scenario.  Will not work with DDD version before 4.1.
        /// </summary>
        public void SendResumeScenarioRequest()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "ResumeScenarioRequest");
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Tell the DDD to load a scenario.  Will not work with DDD version before 4.1.
        /// </summary>
        public void SendLoadScenarioRequest(String scenarioPath, String groupName, String logDir)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "LoadScenarioRequest");
            ((StringValue)(e["ScenarioPath"])).value = scenarioPath;
            ((StringValue)(e["GroupName"])).value = groupName;
            ((StringValue)(e["OutputLogDir"])).value = logDir;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Tell the DDD to stop the scenario.  Will not work with DDD version before 4.1.
        /// </summary>
        public void SendStopScenarioRequest()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "StopScenarioRequest");
            m_netClient.PutEvent(e);
        }

        public void SendGameSpeedRequest(Double speedFactor)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "GameSpeedRequest");
            ((DoubleValue)e["SpeedFactor"]).value = speedFactor;
            m_netClient.PutEvent(e);
        }

        /// <summary>
        /// Send an ExternalApp_Message event to communicate with another external application.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="message"></param>
        public void SendExternalAppMessage(String appName, AttributeCollectionValue message)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "ExternalApp_Message");
            ((StringValue)(e["AppName"])).value = appName;
            e["Message"] = message;
            m_netClient.PutEvent(e);
        }

        /// <summary>
        /// Send an ExternalApp_Log event to allow an external application to place a message into the DDD log file.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="message"></param>
        public void SendExternalAppLog(String appName, String message)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "ExternalApp_Log");
            ((StringValue)(e["AppName"])).value = appName;
            ((StringValue)e["LogEntry"]).value = message;
            m_netClient.PutEvent(e);
        }
        /// <summary>
        /// Send an arbitrary simulation event.
        /// </summary>
        /// <param name="ev"></param>
        public void SendSimEvent(SimulationEvent ev)
        {
            m_netClient.PutEvent(ev);
        }

        /// <summary>
        /// Get the DMView object that contains DDD object state information.
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public DMView GetDMView(String dm)
        {
            if (m_dmViews.ContainsKey(dm))
            {
                return m_dmViews[dm];
            }
            else
            {
                lock (PublicDmViewLock)
                {
                    m_dmViews[dm] = new DMView(dm);
                }
                return m_dmViews[dm]; //fine to return ref out of lock, consumer of this object needs to use the lock while accessing dmView
            }
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's location.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="x">x-location in meters</param>
        /// <param name="y">y-location in meters</param>
        /// <param name="z">altitude in meters</param>
        /// <param name="exists">True is this coordinate exists, False if the coordinate does not.</param>
        public void UpdateObjectLocation(String objectID, double x, double y, double z, bool exists)
        {
            DataValue dv = DataValueFactory.BuildLocation(x, y, z, exists);
            SendObjectAttributeUpdateEvent(objectID, "Location", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's throttle.  This should
        /// immediately impact the simulation environment, and if the object is in motion it will update the movement.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="throttle">Between 0 and 1, this is the percentage of the object's Maximum Speed that it is currently moving at</param>
        public void UpdateObjectThrottle(String objectID, double throttle)
        {
            DataValue dv = DataValueFactory.BuildDouble(throttle);
            SendObjectAttributeUpdateEvent(objectID, "Throttle", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's destination location.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="x">x-location in meters</param>
        /// <param name="y">y-location in meters</param>
        /// <param name="z">altitude in meters</param>
        /// <param name="exists">True is this coordinate exists, False if the coordinate does not.</param>
        public void UpdateObjectDestinationLocation(String objectID, double x, double y, double z, bool exists)
        {
            DataValue dv = DataValueFactory.BuildLocation(x, y, z, exists);
            SendObjectAttributeUpdateEvent(objectID, "DestinationLocation", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's fuel capacity.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="fuelCapacity">The unit capacity for fuel for the specified object</param>
        public void UpdateObjectFuelCapacity(String objectID, double fuelCapacity)
        {
            DataValue dv = DataValueFactory.BuildDouble(fuelCapacity);
            SendObjectAttributeUpdateEvent(objectID, "FuelCapacity", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's fuel amount.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="fuelAmount">Number of units of fuel this object should have</param>
        public void UpdateObjectFuelAmount(String objectID, double fuelAmount)
        {
            DataValue dv = DataValueFactory.BuildDouble(fuelAmount);
            SendObjectAttributeUpdateEvent(objectID, "FuelAmount", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's fuel consumption rate.  This should
        /// immediately impact the simulation environment and if the object is in motion, their current movement.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="fuelConsumptionRate">decimal value for rate of units/second consumed by motion</param>
        public void UpdateObjectFuelConsumptionRate(String objectID, double fuelConsumptionRate)
        {
            DataValue dv = DataValueFactory.BuildDouble(fuelConsumptionRate);
            SendObjectAttributeUpdateEvent(objectID, "FuelConsumptionRate", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's fuel depletion state.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="fuelDepletionState">This is the state that this object transitions to when it runs out of fuel</param>
        public void UpdateObjectFuelDepletionState(String objectID, String fuelDepletionState)
        {
            DataValue dv = DataValueFactory.BuildString(fuelDepletionState);
            SendObjectAttributeUpdateEvent(objectID, "FuelDepletionState", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's Capability object.
        /// This is a complicated data structure.  It contains a series of lists, whose values are related.
        /// These lists contain a series of Capabilities, Ranges, Intensities, and Probabilities.  The index of each of these
        /// collections should correlate such that Capabilities[1], Ranges[1], Intensities[1] and Probabilities[1] are all
        /// for a single capability.  As these are TIGHTLY correlated, you cannot skip inserting any values into this collection.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="capabilities">The value list index relates to the other List collections passed in as parameters.</param>
        /// <param name="ranges">The value list index relates to the other List collections passed in as parameters.</param>
        /// <param name="intensities">The value list index relates to the other List collections passed in as parameters.</param>
        /// <param name="probabilities">The value list index relates to the other List collections passed in as parameters.</param>
        public void UpdateObjectCapabilities(String objectID, List<String> capabilities, List<Double> ranges, List<int> intensities, List<Double> probabilities)
        {
            CapabilityValue dv = new CapabilityValue();
            //construct list of CapName/Range/Intensity/Probability
            if ((capabilities.Count == ranges.Count) && (ranges.Count == intensities.Count) && (intensities.Count == probabilities.Count))
            {
                for (int x = 0; x < capabilities.Count; x++)
                {
                    CapabilityValue.Effect ef = new CapabilityValue.Effect(capabilities[x], ranges[x], intensities[x], probabilities[x]);
                    dv.effects.Add(ef);
                }
                SendObjectAttributeUpdateEvent(objectID, "Capability", dv);
            }
            else
            {
                Console.WriteLine("In UpdateObjectCapabilities: Lists of Capabilities, Ranges, Intensities and Probabilities have different counts, no event sent");
            }
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's Vulnerability object.
        /// This is a very complicated data structure.  It contains a list of states to transition to.
        /// The index of these states should be used to index into the dictionaries for Capabilities, Ranges, Intensities, and Probabilities.
        /// Within each state, there are a series of Capabilities, Ranges, Intensities, and Probabilities.  The index of each of these
        /// collections should correlate such that Capabilities[1], Ranges[1], Intensities[1] and Probabilities[1] are all
        /// for a single transition.  As these are TIGHTLY correlated, you cannot skip inserting any values into this collection.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="states">Simple list of state names to transition to</param>
        /// <param name="capabilities">Dictionary whose index is related to the index of it's corresponding state.  The value list index relates to the other Dictionary/List collections passed in as parameters.</param>
        /// <param name="ranges">Dictionary whose index is related to the index of it's corresponding state.  The value list index relates to the other Dictionary/List collections passed in as parameters.</param>
        /// <param name="intensities">Dictionary whose index is related to the index of it's corresponding state.  The value list index relates to the other Dictionary/List collections passed in as parameters.</param>
        /// <param name="probabilities">Dictionary whose index is related to the index of it's corresponding state.  The value list index relates to the other Dictionary/List collections passed in as parameters.</param>
        public void UpdateObjectVulnerabilities(String objectID, List<String> states, Dictionary<int, List<String>> capabilities, Dictionary<int, List<Double>> ranges, Dictionary<int, List<int>> intensities, Dictionary<int, List<Double>> probabilities)
        {
            VulnerabilityValue dv = new VulnerabilityValue();
            //construct list of conditions -> CapName/Range/Effect/Probability;
            //conditions map to a state to change to.  state index in "states" corresponds to key of dictionary in other dictionaries
            try
            {
                for (int x = 0; x < states.Count; x++)
                {
                    if ((capabilities[x].Count == ranges[x].Count) && (ranges[x].Count == intensities[x].Count) && (intensities[x].Count == probabilities[x].Count))
                    {
                        VulnerabilityValue.Transition tr = new VulnerabilityValue.Transition(states[x]);
                        for (int y = 0; y < capabilities[x].Count; y++)
                        {
                            VulnerabilityValue.TransitionCondition tc = new VulnerabilityValue.TransitionCondition(capabilities[x][y], intensities[x][y], ranges[x][y], probabilities[x][y]);
                            tr.conditions.Add(tc);
                        }
                        if (tr.conditions.Count > 0)
                        {
                            dv.transitions.Add(tr);
                        }
                    }
                    else
                    {
                        Console.WriteLine("In UpdateObjectVulnerabilities: Lists of Capabilities, Ranges, Intensities and Probabilities for state "+states[x]+" have different counts");
                    }
                    
                }
            }
            catch (Exception e)
            {
                return;
            }
           
            if(dv.transitions.Count > 0)
                SendObjectAttributeUpdateEvent(objectID, "Vulnerability", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's size.  This should
        /// immediately impact the simulation environment.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="objectSize">The object's size in meters.</param>
        public void UpdateObjectSize(String objectID, double objectSize)
        {
            DataValue dv = DataValueFactory.BuildDouble(objectSize);
            SendObjectAttributeUpdateEvent(objectID, "Size", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's Maximum Speed.  If the object is
        /// currently in motion, this should affect its current movement.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="maximumSpeed">Represents the object's new speed in meters/sec.</param>
        public void UpdateObjectMaximumSpeed(String objectID, double maximumSpeed)
        {
            DataValue dv = DataValueFactory.BuildDouble(maximumSpeed);
            SendObjectAttributeUpdateEvent(objectID, "MaximumSpeed", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's icon value.  This should
        /// immediately impact a client's display.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="stateName"></param>
        public void UpdateObjectState(String objectID, String stateName)
        {
            DataValue dv = DataValueFactory.BuildString(stateName);
            SendObjectAttributeUpdateEvent(objectID, "State", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's attack duration.  This affects
        /// the underlying model, and the next time that this object engages another, this
        /// duration will be taken into affect.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="attackDuration">The value in seconds for how long an engagement originating
        /// from this object will take</param>
        public void UpdateObjectAttackDuration(String objectID, int attackDuration)
        {
            DataValue dv = DataValueFactory.BuildInteger(attackDuration);
            SendObjectAttributeUpdateEvent(objectID, "AttackDuration", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's engagement duration.  This
        /// affects the underlying model, and the next time that this object is engaged, this 
        /// duration will be taken into affect.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="engagementDuration">The value in seconds for how long an engagement targetting
        /// from this object will take</param>
        public void UpdateObjectEngagementDuration(String objectID, int engagementDuration)
        {
            DataValue dv = DataValueFactory.BuildInteger(engagementDuration);
            SendObjectAttributeUpdateEvent(objectID, "EngagementDuration", dv);
        }
        /// <summary>
        /// Sends an event to the server which updates a single object's icon value.  This should
        /// immediately impact a client's display.
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="iconName">An image name in the appropriate Icon Library</param>
        public void UpdateObjectIconName(String objectID, String iconName)
        {
            DataValue dv = DataValueFactory.BuildString(iconName);
            SendObjectAttributeUpdateEvent(objectID, "IconName", dv);
        }
        /// <summary>
        /// Sends a ForceUpdateObjectAttribute event to the Simulation.  These events update a specific object's attribute. 
        /// This event contains an object ID, an attribute name, and an attribute value.  The attribute value can be any
        /// data type in the system
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="updatedAttributeName">An object's Attribute name</param>
        /// <param name="updatedAttributeValue">An object's updated Attribute value</param>
        public void SendObjectAttributeUpdateEvent(String objectID, String updatedAttributeName, DataValue updatedAttributeValue)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "ForceUpdateObjectAttribute");
            ((StringValue)e["ObjectID"]).value = objectID;
            ((StringValue)e["AttributeName"]).value = updatedAttributeName;
            ((WrapperValue)e["AttributeValue"]).value = updatedAttributeValue; //the WrapperValue contains any single DataValue class
            ((StringValue)e["UpdateType"]).value = "AGENT";

            SendSimEvent(e);
        }

        /// <summary>
        /// Sends a UpdateAttackTimeWindowEvent event to the Simulation.  These events update an attack length for a specific  
        /// attack occurring in the system.  Updating the window to 0 will stop the attack.  Setting the applyToAllAttackers
        /// flag will apply this to all object attacking the target.
        /// </summary>
        /// <param name="attackingObjectID">The attacking object's ID</param>
        /// <param name="targetObjectID">The target object's ID</param>
        /// <param name="capabilityName">The capability being used on the target</param>
        /// <param name="attackWindow">The REMAINING length in MILLISECONDS of the attack</param>
        public void SendUpdateAttackTimeWindowEvent(String attackingObjectID, String targetObjectID, String capabilityName, int attackWindow)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "UpdateAttackTimeWindow");
            ((StringValue)e["AttackingObjectID"]).value = attackingObjectID;
            ((StringValue)e["TargetObjectID"]).value = targetObjectID;
            ((StringValue)e["CapabilityName"]).value = capabilityName;
            ((IntegerValue)e["NewAttackTimeWindow"]).value = attackWindow; 

            SendSimEvent(e);
        }

        /// <summary>
        /// Sends a CancelEngagement event to the simulation.  these events cancel a specific attack between an attacker and target.
        /// To cancel all engagements on a target, you'll need to get the attacker list first, then iterate over that list and send separate
        /// CancelEngagement events.
        /// </summary>
        /// <param name="attackingObjectID">The attacking object's ID</param>
        /// <param name="targetObjectID">The target object's ID</param>
        /// <param name="capabilityName">The capability being used on the target</param>
        public void SendCancelEngagementEvent(String attackingObjectID, String targetObjectID, String capabilityName)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "CancelEngagement");
            ((StringValue)e["AttackingObjectID"]).value = attackingObjectID;
            ((StringValue)e["TargetObjectID"]).value = targetObjectID;
            ((StringValue)e["CapabilityName"]).value = capabilityName;

            SendSimEvent(e);
        }

        public void SendNewObjectEvent(String objectID, String objectType, String ownerID, Dictionary<String, DataValue> attributes, StateTableValue stv)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "NewObject");


            e["Time"] = DataValueFactory.BuildInteger(0);
            e["ObjectType"] = DataValueFactory.BuildString(objectType);
            e["ID"] = DataValueFactory.BuildString(objectID);
            e["Attributes"] = DataValueFactory.BuildAttributeCollection(attributes);

            e["StateTable"] = stv;

            ((AttributeCollectionValue)e["Attributes"]).attributes.Add("OwnerID", DataValueFactory.BuildString(ownerID));
            //((AttributeCollectionValue)e["Attributes"]).attributes.Add("ClassName", DataValueFactory.BuildString(objectType));


            SendSimEvent(e);
        }

        /// <summary>
        /// Sends a RevealObject event to the simulation.  
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="attributes">The objects attributes</param>
        public void SendRevealEvent(String objectID, Dictionary<String, DataValue> attributes)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "RevealObject");
            ((StringValue)e["ObjectID"]).value = objectID;
            ((AttributeCollectionValue)e["Attributes"]).attributes = attributes;

            SendSimEvent(e);
        }

        /// <summary>
        /// Sends a DynamicCreate event to the simulation.  
        /// </summary>
        /// <param name="objectID">The new object's ID</param>
        /// <param name="kind">The species of the new object</param>
        /// <param name="owner">The owner of the new object</param>
        public void SendDynamicCreateEvent(String objectID, String kind, String owner)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "DynamicCreate");
            ((StringValue)e["ObjectID"]).value = objectID;
            ((StringValue)e["Kind"]).value = kind;
            ((StringValue)e["Owner"]).value = owner;

            SendSimEvent(e);
        }

        /// <summary>
        /// Sends a DynamicReveal event to the simulation.  
        /// </summary>
        /// <param name="objectID">The object's ID</param>
        /// <param name="kind">The species of the new object</param>
        /// <param name="owner">The owner of the new object</param>
        public void SendDynamicRevealEvent(String objectID, int timeSec, String initialState,Double x, Double y, Double z)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref m_simModel, "DynamicReveal");
            ((StringValue)e["ObjectID"]).value = objectID;
            ((StringValue)e["InitialState"]).value = initialState;
            ((IntegerValue)e["RevealTime"]).value = timeSec;
            ((LocationValue)e["InitialLocation"]).X = x;
            ((LocationValue)e["InitialLocation"]).Y = y;
            ((LocationValue)e["InitialLocation"]).Z = z;
            ((LocationValue)e["InitialLocation"]).exists = true;

            SendSimEvent(e);
        }

        /// <summary>
        /// Adds a callback which is called during ProcessEvents for a given event name
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void AddEventCallback(string eventName, ProcessSimulationEvent eventHandler)
        {
            if (!_eventCallbacks.ContainsKey(eventName))
            {
                _eventCallbacks.Add(eventName, new List<ProcessSimulationEvent>());
            }
            _eventCallbacks[eventName].Add(eventHandler);
            SubscribeToEvent(eventName);
        }

        /// <summary>
        /// clears out the callback list
        /// </summary>
        public void ClearEventCallbacks()
        {
            if (_eventCallbacks == null)
                _eventCallbacks = new Dictionary<string, List<ProcessSimulationEvent>>();
            _eventCallbacks.Clear();
        }
    }
}
