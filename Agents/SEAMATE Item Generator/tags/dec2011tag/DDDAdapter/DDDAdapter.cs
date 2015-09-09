using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.DDDAgentFramework;
using System.Threading;
using Aptima.Asim.DDD.DDDAgentFramework.UIHelpers;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace SeamateAdapter.DDD
{
    public class DDDAdapter
    {
        private class ScheduledAttack
        {
            public string attackerid = "";
            public string targetid = "";
            public int attacktime = 0;
            public bool isfirstattack = true;
            public ScheduledAttack() { }
            public ScheduledAttack(String attacker, String target, int time, bool first)
            {
                attackerid = attacker;
                targetid = target;
                attacktime = time;
                isfirstattack = first;
            }

        }
        private Dictionary<int, List<ScheduledAttack>> _scheduledAttacks = null;
        public delegate void ConnectCompleteDelegate(bool success, String msg);
        public delegate void TimeTickDelegate(int time);
        private DDDServerConnection _dddConnection;
        private Object _dddLock = new Object();
        private ConnectCompleteDelegate _connectCompleteCallback = null;
        private String _simModelPath = "";
        private SimulationModelInfo _simModel = null;
        private Object _connectionLock = new Object();

        private OrderedEventsList _eventsQueue;
        private TimeTickDelegate _tickCallback = null;

        private Thread _connectThread;
        private Thread _dddLoopThread;

        /**** objects for keeping track of asset affiliation, availability, etc ****/
        private static Object GroundTruthLock = new object();
        private Dictionary<String, SeamateObject> _revealedObjects = null;
        private Dictionary<String, SeamateObject> _unrevealedObjects = null;
        private Dictionary<String, PolygonValue> _seaLanes = null;
        private Dictionary<String, PolygonValue> _entryPoints = null;
        private Dictionary<String, List<String>> _objectOwnershipMaps = null;
        private Dictionary<String, String> _reverseOwnershipMaps = null;
        private Dictionary<String, Dictionary<String, AttributeCollectionValue>> _speciesPossibleStates = null; //[Species]:{[State]:[Attributes]}

        /*** END ****/

        /*** Attack scheduler ***/
        private static Object AttacksLock = new object();
        private void ScheduleAttack(String attacker, String target, int msTime, bool firstTime)
        {
            lock (AttacksLock)
            {
                if (!_scheduledAttacks.ContainsKey(msTime))
                {
                    _scheduledAttacks.Add(msTime, new List<ScheduledAttack>());
                }
                _scheduledAttacks[msTime].Add(new ScheduledAttack(attacker, target, msTime, firstTime));
            }
        }

        public void ScheduleAttack(String attacker, String target, int msTime)
        {
            ScheduleAttack(attacker, target, msTime, true);
        }

        private List<ScheduledAttack> PopReadyAttacks(int currentMsTime)
        {
            List<ScheduledAttack> attacks = new List<ScheduledAttack>();

            lock (AttacksLock)
            {
                List<int> timesToRemove = new List<int>();
                foreach (int time in _scheduledAttacks.Keys)
                {
                    if (time <= currentMsTime)
                        timesToRemove.Add(time);
                }
                foreach (int time in timesToRemove)
                {
                    foreach (ScheduledAttack at in _scheduledAttacks[time])
                    {
                        attacks.Add(at);
                    }
                    _scheduledAttacks.Remove(time);
                }
            }

            return attacks;
        }

        /*** END ***/

        public DDDAdapter()
        {
            _eventsQueue = new OrderedEventsList();
            _dddConnection = new DDDServerConnection();
            _revealedObjects = new Dictionary<string, SeamateObject>();
            _unrevealedObjects = new Dictionary<string, SeamateObject>();
            _scheduledAttacks = new Dictionary<int, List<ScheduledAttack>>();
            _seaLanes = new Dictionary<string, PolygonValue>();
            _entryPoints = new Dictionary<string, PolygonValue>();
            _objectOwnershipMaps = new Dictionary<string, List<string>>();
            _reverseOwnershipMaps = new Dictionary<string, string>();
            _speciesPossibleStates = new Dictionary<string, Dictionary<string, AttributeCollectionValue>>();
        }

        public void StartConnect(string host, string shareName, int port, ConnectCompleteDelegate callback)
        {
            if (_dddConnection != null)
            {
                if (_dddConnection.IsConnected())
                    _dddConnection.Disconnect();
            }
            _simModelPath = String.Format(@"\\{0}\{1}\SimulationModel.xml", host, shareName);
            SimulationModelReader rdr = new SimulationModelReader();
            _simModel = rdr.readModel(_simModelPath);
            _dddConnection.DDDClientPath = String.Format(@"\\{0}\{1}", host, shareName);
            _connectCompleteCallback = callback;
            //bool result = false;// = _dddConnection.ConnectToServer(host, port);
            //_connectRunning = true;
            //while (true)
            //{
            //    lock (_connectionLock)
            //    {
            //        if (!_connectRunning)
            //        {
            //            break;
            //        }
            //    }

            //    Thread.Sleep(1000);
            //}
            _connectThread = new Thread(new ParameterizedThreadStart(T_Connect));
            _connectThread.Start(new object[] { host, port });
        }

        public void T_Connect(object p)
        {
            object[] param = p as object[];
            String host = "";
            int port = -1;
            try
            {
                host = param[0] as string;
                port = (int)param[1];
            }
            catch (Exception ex)
            {
                if (_connectCompleteCallback != null)
                {
                    _connectCompleteCallback(false, "Hostname or port invalid");
                }
            }

            bool result = false;
            lock (_dddLock)
            {
                result = _dddConnection.ConnectToServer(host, port);
                if (!result)
                    goto end;
                result = (result && _dddConnection.ReadSimModel());
                if (!result)
                    goto end;


                String myDM = "AgentDM";
                //PlayerLoginDialog dlg = new PlayerLoginDialog(ref _dddConnection);
                //dlg.ShowDialog();
                // if (dlg.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                // {
                //    result = false;
                //    goto end;
                //}
                //myDM = _dddConnection.PlayerID;
                //event callbacks
                _dddConnection.AddEventCallback("TimeTick", new DDDServerConnection.ProcessSimulationEvent(TimeTick));
                _dddConnection.AddEventCallback("RevealObject", new DDDServerConnection.ProcessSimulationEvent(RevealObject));
                _dddConnection.AddEventCallback("AttackObject", new DDDServerConnection.ProcessSimulationEvent(AttackObject));
                _dddConnection.AddEventCallback("StateChange", new DDDServerConnection.ProcessSimulationEvent(StateChange));
                _dddConnection.AddEventCallback("SelfDefenseAttackStarted", new DDDServerConnection.ProcessSimulationEvent(SelfDefense));
                _dddConnection.AddEventCallback("TransferObject", new DDDServerConnection.ProcessSimulationEvent(TransferObject));
                _dddConnection.AddEventCallback("ViewProActiveRegionUpdate", new DDDServerConnection.ProcessSimulationEvent(ViewProActiveRegionUpdate));
                _dddConnection.AddEventCallback("NewObject", new DDDServerConnection.ProcessSimulationEvent(NewObject));
                _dddConnection.AddEventCallback("MoveObject", new DDDServerConnection.ProcessSimulationEvent(MoveObject));
                _dddConnection.AddEventCallback("MoveDone", new DDDServerConnection.ProcessSimulationEvent(MoveDone));
                _dddConnection.AddEventCallback("AttackSucceeded", new DDDServerConnection.ProcessSimulationEvent(AttackSucceeded));
                _dddConnection.RequestPlayers();
                while (_dddConnection.Players.Count == 0)
                {
                    Thread.Sleep(500);
                    _dddConnection.ProcessEvents();
                }
                _dddConnection.LoginPlayer(myDM, "OBSERVER");
                _dddConnection.GetDMView(myDM); //initializes DM View
            }



            _dddLoopThread = new Thread(new ThreadStart(StartDDDLoop));
            _dddLoopThread.Start();

        end:
            String msg = "";
            if (result)
            {
                msg = "Connected to DDD";
            }
            else
            {
                msg = "Connection failed to DDD (host: " + host + ";port: " + port.ToString() + ")";
            }

            if (_connectCompleteCallback != null)
            {
                _connectCompleteCallback(result, msg);
            }
        }

        private void StartDDDLoop()
        {
            bool isConnected = true;
            lock (_dddLock)
            {//lock so you can disconnect on the main thread
                isConnected = _dddConnection.IsConnected();
            }

            while (isConnected)
            {
                _dddConnection.ProcessEvents();
                Thread.Sleep(500);
                try
                {
                    lock (_dddLock)
                    {
                        isConnected = _dddConnection.IsConnected();
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                }
            }
            //UpdateDDDConnectionStatus("not");
        }

        public void WaitShutdownConnection()
        {
            lock (_dddLock)
            {
                if (_dddConnection != null)
                {
                    _dddConnection.Disconnect();
                }
                if (_connectThread != null)
                {
                    _connectThread.Abort();
                }
                if (_dddLoopThread != null)
                {
                    _dddLoopThread.Abort();
                }
            }
        }

        #region DDD Callbacks
        protected void ViewProAttributeUpdate(SimulationEvent ev)
        {
            lock (GroundTruthLock)
            {
                String id = ((StringValue)ev["ObjectID"]).value;
                AttributeCollectionValue atts = ev["Attributes"] as AttributeCollectionValue;
                if (_revealedObjects.ContainsKey(id))
                {
                    _revealedObjects[id].SetAttributes(atts);
                }
            }
        }
        protected void TimeTick(SimulationEvent ev)
        {
            //TODO: When a Time tick is received from the DDD, pass that tick back to the SeamateRuntime engine to see if any Items are triggered, and then
            //check the DDDAdapter's DDD Event Queue to see if any new events need to be sent to the DDD.

            int time = ((IntegerValue)ev["Time"]).value; // /1000?  do we want seconds or millis?

            //call tick on RuntimeEngine (callback)
            if (_tickCallback != null)
            {
                _tickCallback(time);
            }

            List<ScheduledAttack> attacksToStart = PopReadyAttacks(time);


            lock (GroundTruthLock)
            {
                List<DDDEvent> eventsToSend = _eventsQueue.GetEventsUpTo(time);
                foreach (ScheduledAttack a in attacksToStart)
                {
                    if (_revealedObjects.ContainsKey(a.attackerid) && _revealedObjects.ContainsKey(a.targetid))
                    {
                        eventsToSend.Add(new AttackEvent(a.attackerid, a.targetid, "Surface to surface", time));
                    }
                }
                
                foreach (DDDEvent e in eventsToSend)
                {
                    _dddConnection.SendSimEvent(e.ToSimulationEvent(ref _simModel));
                }
            }
            Console.WriteLine("Tick");
        }
        protected void RevealObject(SimulationEvent ev)
        {
            lock (GroundTruthLock)
            {
                String id = ((StringValue)ev["ObjectID"]).value;
                AttributeCollectionValue atts = ev["Attributes"] as AttributeCollectionValue;
                SeamateObject seamateObject = null;
                if (_unrevealedObjects.ContainsKey(id))
                {
                    seamateObject = _unrevealedObjects[id];
                    _unrevealedObjects.Remove(id);
                    Console.WriteLine("Removed from UnrevealedObjects: " + id);
                }
                else
                {
                    seamateObject = new SeamateObject(id);
                }
                seamateObject.SetAttributes(atts);
                if (_revealedObjects.ContainsKey(id))
                {
                    _revealedObjects.Remove(id);
                }
                _revealedObjects.Add(id, seamateObject);
                Console.WriteLine("Added to RevealedObjects: " + id);
                if (atts.attributes.ContainsKey("OwnerID"))
                {
                    EstablishOwnership(id, ((StringValue)atts.attributes["OwnerID"]).value);
                }

            }
        }
        protected void AttackSucceeded(SimulationEvent ev)
        { //ObjectID, TargetID, NewState
            String state = ((StringValue)ev["NewState"]).value;

            if (state == "Dead")
                return;
            String attacker = ((StringValue)ev["ObjectID"]).value;
            String target = ((StringValue)ev["TargetID"]).value;

            ScheduleAttack(attacker, target, 0);//schedule it as immediate.
        }
        protected void MoveDone(SimulationEvent ev)
        {//ObjectID, Reason
            String id = ((StringValue)ev["ObjectID"]).value;
            if (!_revealedObjects.ContainsKey(id))
                return;
            AttributeCollectionValue atts = new AttributeCollectionValue();
            atts.attributes.Add("DestinationLocation", new LocationValue());
            atts.attributes.Add("Throttle", DataValueFactory.BuildDouble(0));

            String owner = _revealedObjects[id].Owner;
            foreach (PolygonValue region in GetAllEntryRegions())
            {
                if (Polygon2D.IsPointInside(new Polygon2D(region), new Vec2D(GetSeamateObject(id).Location))
                    && owner == "Merchant DM")
                {
                    Console.WriteLine("Detected vessel " + id + " ending move in entry region, turning state to Dead.");
                    //atts.attributes.Add("State", DataValueFactory.BuildString("Dead"));
                    _dddConnection.SendStateChange(id, "Dead");
                }
            }


            //LocationValue destLoc =  as LocationValue;
            _revealedObjects[id].SetAttributes(atts);
        }
        protected void MoveObject(SimulationEvent ev)
        { //ObjectID, DestinationLocation, Throttle, 
            String id = ((StringValue)ev["ObjectID"]).value;
            if (!_revealedObjects.ContainsKey(id))
                return;
            AttributeCollectionValue atts = new AttributeCollectionValue();
            atts.attributes.Add("DestinationLocation", ev["DestinationLocation"]);
            atts.attributes.Add("Throttle", ev["Throttle"]);

            //LocationValue destLoc =  as LocationValue;
            _revealedObjects[id].SetAttributes(atts);
        }
        protected void AttackObject(SimulationEvent ev)
        {//ObjectID,TargetObjectID,CapabilityName,PercentageApplied

            String targetID = ((StringValue)ev["TargetObjectID"]).value;
            _dddConnection.SendObjectAttributeUpdateEvent(targetID, "Throttle", DataValueFactory.BuildDouble(0));
        }
        protected void StateChange(SimulationEvent ev)
        {//ObjectID, NewState
            String state = ((StringValue)ev["NewState"]).value;
            if (state == "Dead")
            {
                String objID = ((StringValue)ev["ObjectID"]).value;
                lock (GroundTruthLock)
                {
                    if (_revealedObjects.ContainsKey(objID))
                    {
                        _revealedObjects.Remove(objID);
                    }
                    if (_unrevealedObjects.ContainsKey(objID))
                    {
                        _unrevealedObjects.Remove(objID);
                    }
                }
            }
        }
        protected void TransferObject(SimulationEvent ev)
        {//UserID,ObjectID,DonorUserID
        }


        protected void SelfDefense(SimulationEvent ev)
        //Set throttle of target to 0 when pirate attacks (the "attack" is actually a self defense attack)
        {
            String targetID = ((StringValue)ev["TargetObjectID"]).value;
            _dddConnection.SendObjectAttributeUpdateEvent(targetID, "Throttle", DataValueFactory.BuildDouble(0));

        }

        protected void ViewProActiveRegionUpdate(SimulationEvent ev)
        {//ObjectID,IsVisible,DisplayColor,Shape
        }
        protected void NewObject(SimulationEvent ev)
        {//ID,ObjectType,StateTable,Attributes
            //ObjectType can be LandRegion, ActiveRegion, AirObject, SeaObject, LandObject
            lock (GroundTruthLock)
            {
                String objectType = ((StringValue)ev["ObjectType"]).value;
                switch (objectType)
                {
                    case "LandRegion":
                    case "ActiveRegion":
                        ProcessRegion(ev);
                        break;
                    case "AirObject":
                    case "SeaObject":
                    case "LandObject":
                        ProcessObject(ev);
                        break;
                    default:

                        break;
                }
            }
        }
        private void ProcessRegion(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ID"]).value;
            AttributeCollectionValue atts = ((AttributeCollectionValue)ev["Attributes"]);
            PolygonValue pv = atts.attributes["Polygon"] as PolygonValue;

            if (atts.attributes.ContainsKey("CurrentAbsolutePolygon"))
            {
                pv = atts["CurrentAbsolutePolygon"] as PolygonValue;
            }
            if (id.StartsWith("SeaLane-"))
            {
                if (!_seaLanes.ContainsKey(id))
                { _seaLanes.Add(id, pv); }
                else
                {
                    _seaLanes[id] = pv;
                }
            }
            else if (id.StartsWith("Entry-"))
            {
                if (!_entryPoints.ContainsKey(id))
                { _entryPoints.Add(id, pv); }
                else
                {
                    _entryPoints[id] = pv;
                }
            }
            else if (id.StartsWith("AO-"))
            {
                //AO's handled differently now
            }
        }
        private void ProcessObject(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ID"]).value;
            String objectType = ((StringValue)((AttributeCollectionValue)ev["Attributes"])["ClassName"]).value;
            AttributeCollectionValue atts = ev["Attributes"] as AttributeCollectionValue;
            SeamateObject seamateObject = null;
            if (_unrevealedObjects.ContainsKey(id))
            {
            }
            else
            {
                _unrevealedObjects.Add(id, new SeamateObject(id));
                Console.WriteLine("Added to UnrevealedObjects: " + id);
            }
            _unrevealedObjects[id].SetAttributes(atts);
            if (atts.attributes.ContainsKey("OwnerID"))
            {
                EstablishOwnership(id, ((StringValue)atts.attributes["OwnerID"]).value);
            }
            if (ev.parameters.ContainsKey("StateTable") && !_speciesPossibleStates.ContainsKey(objectType))
            {
                _speciesPossibleStates.Add(objectType, new Dictionary<string, AttributeCollectionValue>());
                StateTableValue stv = (StateTableValue)ev["StateTable"];
                AttributeCollectionValue stateAttributes = null;
                foreach (String stateName in stv.states.Keys)
                {
                    stateAttributes = (AttributeCollectionValue)stv[stateName];
                    _speciesPossibleStates[objectType].Add(stateName, new AttributeCollectionValue());
                    foreach (String att in stateAttributes.attributes.Keys)
                    {
                        _speciesPossibleStates[objectType][stateName].attributes.Add(att, DataValueFactory.BuildFromDataValue(stateAttributes[att]));
                    }
                }
            }
        }
        private void ProcessDecisionMaker(SimulationEvent ev)
        {
            //not needed?
        }
        private void EstablishOwnership(String playfieldObjectID, String dmID)
        {
            lock (GroundTruthLock)
            {
                if (!_objectOwnershipMaps.ContainsKey(dmID))
                    _objectOwnershipMaps.Add(dmID, new List<string>());
                _objectOwnershipMaps[dmID].Add(playfieldObjectID);
                if (!_reverseOwnershipMaps.ContainsKey(playfieldObjectID))
                {
                    _reverseOwnershipMaps.Add(playfieldObjectID, dmID);
                }
                else
                {
                    _reverseOwnershipMaps[playfieldObjectID] = dmID;
                }
                Console.WriteLine("Ownership: " + playfieldObjectID + " => " + dmID);
            }
        }

        #endregion
        /// <summary>
        /// Creates a ForceUpdateObjectAttribute event.
        /// </summary>
        /// <param name="dddObjectID"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue">Depending upon the attribute type, this object should be a String, Bool, Int, Float[3](Location), Float[3](Velocity), String[] (StringList), etc.</param>
        /// <param name="type"></param>
        public void UpdateObjectAttribute(String dddObjectID, String attributeName, Object attributeValue, String updateType)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref _simModel, "ForceUpdateObjectAttribute");
            ((StringValue)ev["ObjectID"]).value = dddObjectID;
            ((StringValue)ev["AttributeName"]).value = attributeName;
            ((StringValue)ev["UpdateType"]).value = updateType;
            ((WrapperValue)ev["AttributeValue"]).value = CreateAppropriateDataValue(attributeName, attributeValue);

            _dddConnection.SendSimEvent(ev);
        }
        private DataValue CreateAppropriateDataValue(String attributeName, Object attributeValue)
        {
            DataValue dv = null;

            String t = _simModel.objectModel.objects["PhysicalObject"].attributes[attributeName].dataType;
            dv = DataValueFactory.BuildValue(t);
            switch (t)
            {
                case "StringType":
                    ((StringValue)dv).value = (String)attributeValue;
                    break;
                case "DoubleType":
                    ((DoubleValue)dv).value = (Double)attributeValue;
                    break;
                case "IntegerType":
                    ((IntegerValue)dv).value = (Int32)attributeValue;
                    break;
                case "BooleanType":
                    ((BooleanValue)dv).value = (Boolean)attributeValue;
                    break;
                case "LocationType":
                    double[] xyz = (double[])attributeValue;
                    ((LocationValue)dv).X = xyz[0];
                    ((LocationValue)dv).Y = xyz[1];
                    ((LocationValue)dv).Z = xyz[2];
                    ((LocationValue)dv).exists = true;

                    break;
                case "VelocityType":
                    double[] vxvyvz = (double[])attributeValue;
                    ((LocationValue)dv).X = vxvyvz[0];
                    ((LocationValue)dv).Y = vxvyvz[1];
                    ((LocationValue)dv).Z = vxvyvz[2];
                    break;
                default:

                    break;
            }
            return dv;
        }
        public void AddDDDEventToQueue(DDDEvent ev)
        {
            if (ev == null)
                return;
            lock (GroundTruthLock)
            {
                _eventsQueue.Add(ev);
            }
        }

        public void SetTickCallback(TimeTickDelegate timeTickDelegate)
        {
            _tickCallback = timeTickDelegate;
        }
        #region API
        public LocationValue GetObjectLocation(String objType, String objID)
        {
            return new LocationValue(); //AD: TODO
        }
        private PolygonValue BAMS_AO = null;
        private PolygonValue GetBamsAORegion()
        {
            if (BAMS_AO == null)
            {
                BAMS_AO = new PolygonValue();
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(200, 76200));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(0, 8800));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(39100, 8600));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(40500, 24000));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(45700, 40700));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(55800, 55900));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(66400, 66500));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(79200, 76000));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(93000, 82400));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(109600, 86950));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(126500, 88200));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(126200, 103200));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(28900, 103200));
                BAMS_AO.points.Add(new PolygonValue.PolygonPoint(200, 76200));
            }

            return BAMS_AO;
        }
        private PolygonValue Firescout_AO = null;
        private PolygonValue GetFirescoutAORegion()
        {
            if (Firescout_AO == null)
            {
                Firescout_AO = new PolygonValue();
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(39000, 8400));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(125800, 8700));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(126500, 88200));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(109600, 86950));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(93000, 82400));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(79200, 76000));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(66400, 66500));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(55800, 55900));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(45700, 40700));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(40500, 24000));
                Firescout_AO.points.Add(new PolygonValue.PolygonPoint(39000, 8400));
            }

            return Firescout_AO;
        }
        /// <summary>
        /// Returns a polygon describing the area of responsibility for a specific DM
        /// </summary>
        /// <param name="dmID"></param>
        /// <returns></returns>
        public PolygonValue GetDecisionMakersAreaOfResponsibility(String dmID)
        {
            if (dmID == "BAMS DM")
            {
                return GetBamsAORegion();
            }
            return GetFirescoutAORegion();
        }
        /// <summary>
        /// For a given decision maker, get all of their object locations
        /// </summary>
        /// <param name="dmID"></param>
        /// <returns></returns>
        public List<LocationValue> GetDecisionMakersObjectPositions(String dmID)
        {
            List<LocationValue> positions = new List<LocationValue>();
            lock (GroundTruthLock)
            {
                DMView view = _dddConnection.GetDMView(dmID);
                if (view != null)
                {
                    foreach (String s in view.MyObjects.Keys)
                    {
                        if (view.MyObjects[s].Location.exists)
                        {
                            positions.Add(DataValueFactory.BuildFromDataValue(view.MyObjects[s].Location) as LocationValue);
                        }
                    }
                }
            }
            return positions;
        }
        public class SensorRange
        {
            public LocationValue CenterPoint = new LocationValue();
            public double SensorRadius = 0.0;
            public SensorRange()
            { }
            public SensorRange(LocationValue centerPoint, double sensorRadius)
            {
                CenterPoint = centerPoint;
                SensorRadius = sensorRadius;
            }
        }
        /// <summary>
        /// For a given decision maker, get all sensor ranges which they currently have (max range per sensor object)
        /// </summary>
        /// <param name="dmID"></param>
        /// <returns></returns>
        public List<SensorRange> GetDecisionMakersSensorRanges(String dmID)
        {
            List<SensorRange> ranges = new List<SensorRange>();
            lock (GroundTruthLock)
            {
                DMView view = _dddConnection.GetDMView(dmID);
                SensorRange range = null;
                if (view != null)
                {
                    foreach (String s in view.MyObjects.Keys)
                    {
                        if (view.MyObjects[s].Location.exists)
                        {
                            double maxRange = 0;
                            foreach (SensorValue sv in view.MyObjects[s].Sensors.sensors)
                            {
                                if (sv.maxRange > maxRange)
                                {
                                    maxRange = sv.maxRange;
                                }
                            }
                            range = new SensorRange(DataValueFactory.BuildFromDataValue(view.MyObjects[s].Location) as LocationValue, maxRange);
                            ranges.Add(range);
                        }
                    }
                }
            }
            return ranges;
        }
        public class SeamateObject : SimObject
        {
            public SensorRange SensorRange = new SensorRange();
            public double Heading = 0.0;
            public String GroundTruthIFF = "";
            public String ObjectName = "";
            public SeamateObject(String id)
                : base(id)
            {


            }
            public static SeamateObject FromDDDObject(SimObject baseObject)
            {
                SeamateObject obj = new SeamateObject(baseObject.ID);
                obj.Capabilities = baseObject.Capabilities;
                obj.CapabilityList = baseObject.CapabilityList;
                obj.CapabilityRangeRings = baseObject.CapabilityRangeRings;
                obj.ChildObjects = baseObject.ChildObjects;
                obj.ClassName = baseObject.ClassName;
                obj.CustomAttributes = baseObject.CustomAttributes;
                obj.DestinationLocation = baseObject.DestinationLocation;
                obj.DockedObjects = baseObject.DockedObjects;
                obj.DockedToParent = baseObject.DockedToParent;
                obj.DockedWeapons = baseObject.DockedWeapons;
                obj.FuelAmount = baseObject.FuelAmount;
                obj.FuelCapacity = baseObject.FuelCapacity;
                obj.FuelConsumptionRate = baseObject.FuelConsumptionRate;
                obj.IconName = baseObject.IconName;
                obj.InActiveRegions = baseObject.InActiveRegions;
                obj.IsWeapon = baseObject.IsWeapon;
                obj.Location = baseObject.Location;
                obj.MaximumSpeed = baseObject.MaximumSpeed;
                obj.Owner = baseObject.Owner;
                obj.ParentObjectID = baseObject.ParentObjectID;
                obj.Sensors = baseObject.Sensors;
                obj.ShouldProject = baseObject.ShouldProject;
                obj.State = baseObject.State;
                obj.TeamName = baseObject.TeamName;
                obj.Throttle = baseObject.Throttle;
                obj.Velocity = baseObject.Velocity;
                obj.Vulnerabilities = baseObject.Vulnerabilities;
                obj.VulnerabilityList = baseObject.VulnerabilityList;
                obj.VulnerabilityRangeRings = baseObject.VulnerabilityRangeRings;

                //obj.GroundTruthIFF;
                //obj.Heading;
                //obj.SensorRange;
                return obj;
            }

            internal void SetAttributes(AttributeCollectionValue atts)
            {
                if (atts.attributes.ContainsKey("GroundTruthIFF"))
                {
                    this.GroundTruthIFF = ((StringValue)atts.attributes["GroundTruthIFF"]).value;
                }
                if (atts.attributes.ContainsKey("Heading"))
                {
                    this.Heading = ((DoubleValue)atts.attributes["Heading"]).value;
                }

                if (atts.attributes.ContainsKey("ClassName"))
                {
                    this.ClassName = ((StringValue)atts.attributes["ClassName"]).value;
                }
                if (atts.attributes.ContainsKey("State"))
                {
                    this.State = ((StringValue)atts.attributes["State"]).value;
                }
                if (atts.attributes.ContainsKey("Location"))
                {
                    this.Location = ((LocationValue)atts.attributes["Location"]);
                }
                if (atts.attributes.ContainsKey("Throttle"))
                {
                    this.Throttle = ((DoubleValue)atts.attributes["Throttle"]).value;
                }
                if (atts.attributes.ContainsKey("MaximumSpeed"))
                {
                    this.MaximumSpeed = ((DoubleValue)atts.attributes["MaximumSpeed"]).value;
                }
                if (atts.attributes.ContainsKey("DestinationLocation"))
                {
                    this.DestinationLocation = ((LocationValue)atts.attributes["DestinationLocation"]);
                }
                if (atts.attributes.ContainsKey("TeamName"))
                {
                    this.TeamName = ((StringValue)atts.attributes["TeamName"]).value;
                }
                if (atts.attributes.ContainsKey("OwnerID"))
                {
                    this.Owner = ((StringValue)atts.attributes["OwnerID"]).value;
                }
                if (atts.attributes.ContainsKey("Heading"))
                {
                    this.Heading = ((DoubleValue)atts.attributes["Heading"]).value;
                }
                if (atts.attributes.ContainsKey("ObjectName"))
                {
                    this.ObjectName = ((StringValue)atts.attributes["ObjectName"]).value;
                }
            }
        }
        /// <summary>
        /// Gets all seaobjects which exist in the simulation
        /// </summary>
        /// <returns></returns>
        public List<SeamateObject> GetAllRevealedSeaVessels()
        {
            List<SeamateObject> objects = new List<SeamateObject>();
            lock (GroundTruthLock)
            {
                //AD: TODO; need at least position, speed, heading, ground truth IFF
                // OwnerID can tell you if they belong to the pirate DM or not, which gives you their true intention
                //need to get all objects, not just a dm view.
                objects.AddRange(_revealedObjects.Values);
            }
            return objects;
        }
        /// <summary>
        /// Retrieves all the state names for a given species.
        /// </summary>
        /// <param name="speciesName"></param>
        /// <returns></returns>
        public List<String> GetSpeciesStates(String speciesName)
        {
            List<String> states = new List<string>();
            lock (GroundTruthLock)
            {
                if (_speciesPossibleStates.ContainsKey(speciesName))
                    states.AddRange(_speciesPossibleStates[speciesName].Keys);
            }
            return states;
        }
        /// <summary>
        /// For a given Species and State name, returns a dictionary with name-value pairs for the default parameters for that species-state pair.
        /// Values in this collection are of the generic type DataValue, which will need to be cast correct to retrieve the data values from it.
        /// </summary>
        /// <param name="speciesName"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public Dictionary<String, DataValue> GetSpeciesStateParameters(String speciesName, String stateName)
        {
            Dictionary<String, DataValue> stateParameters = new Dictionary<string, DataValue>();
            lock (GroundTruthLock)
            {
                if (_speciesPossibleStates.ContainsKey(speciesName))
                {
                    if (_speciesPossibleStates[speciesName].ContainsKey(stateName))
                    {
                        stateParameters = _speciesPossibleStates[speciesName][stateName].attributes;
                    }
                }
            }
            return stateParameters;
        }
        /// <summary>
        /// Returns null if id not found in revealed or unrevealed objects, otherwise returns a seamateobject with that object's info.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SeamateObject GetSeamateObject(String id)
        {
            lock (GroundTruthLock)
            {
                if (_revealedObjects.ContainsKey(id))
                    return _revealedObjects[id];
                if (_unrevealedObjects.ContainsKey(id))
                    return _unrevealedObjects[id];
            }
            return null;
        }
        /// <summary>
        /// Gets all objects that have CreateEvents, but have not been Revealed yet.
        /// </summary>
        /// <param name="isPirate">IF NULL, then you don't care if it's a pirate.  If true, will only return pirate vessels. </param>
        /// <param name="isSeaVessel">IF NULL, then you don't care if it's a Sea Vessel.  If true, will only return sea vessels.</param>
        /// <param name="objectSpecies">IF NULL, then you don't care what Type of object is returned.  If some string is set, will only return objects who match that Species.</param>
        /// <returns></returns>
        public List<String> GetAllUnrevealedObjectIds(bool? isPirate, bool? isSeaVessel, String objectSpecies)
        {
            List<String> objectIds = new List<string>();
            bool mustBePirate = false;
            bool mustBeNotPirate = false;
            bool mustBeSeaVessel = false;
            if (isPirate != null)
            {
                mustBeNotPirate = !isPirate.Value;
                mustBePirate = isPirate.Value;
            }
            if (isSeaVessel != null)
            {
                mustBeSeaVessel = isSeaVessel.Value;
            }

            lock (GroundTruthLock)
            {
                foreach (SeamateObject seamateObject in _unrevealedObjects.Values)
                {
                    if (mustBePirate && seamateObject.Owner != "Pirate DM")
                        continue;
                    if (mustBeNotPirate && seamateObject.Owner != "Merchant DM")
                        continue;
                    if (mustBeSeaVessel && (seamateObject.ClassName.Contains("BAMS") || seamateObject.ClassName.Contains("Firescout")))
                        continue;

                    if (objectSpecies != null)
                    {
                        if (objectSpecies != seamateObject.ClassName)
                            continue;
                    }

                    objectIds.Add(seamateObject.ID);
                }
            }
            return objectIds;
        }
        /// <summary>
        /// Gets all AirObjects which current exist in the simulation
        /// </summary>
        /// <returns></returns>
        public List<SeamateObject> GetAllRevealedAirObjects()
        {
            List<SeamateObject> objects = new List<SeamateObject>();
            //AD: TODO; need at least position and sensor range
            lock (GroundTruthLock)
            {
                foreach (SeamateObject seamateObject in _revealedObjects.Values)
                {
                    if (!(seamateObject.ClassName.Contains("BAMS") || seamateObject.ClassName.Contains("Firescout")))
                        continue;
                    objects.Add(seamateObject);
                }
            }
            return objects;
        }
        /// <summary>
        /// Gets the polygon values for all SeaLanes.  This might change to return a lane name and polygon if needed.
        /// </summary>
        /// <returns></returns>
        public List<PolygonValue> GetAllSeaLanes()
        {
            List<PolygonValue> seaLanes = new List<PolygonValue>();
            //AD: TODO; Sea Lanes are regions that start with SeaLane-
            lock (GroundTruthLock)
            {
                seaLanes.AddRange(_seaLanes.Values);
            }
            return seaLanes;
        }

        public List<PolygonValue> GetAllEntryRegions()
        {
            List<PolygonValue> entryRegions = new List<PolygonValue>();
            //AD: TODO; Sea Lanes are regions that start with SeaLane-
            lock (GroundTruthLock)
            {
                entryRegions.AddRange(_entryPoints.Values);
            }
            return entryRegions;
        }
        public void SendStimulusEvent(String itemID, String dmID, String objectID, String stimulusType, int time)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref _simModel, "SEAMATE_StimulusSent");

            ((StringValue)ev["ItemID"]).value = itemID;
            ((StringValue)ev["DM_ID"]).value = dmID;
            ((StringValue)ev["ObjectID"]).value = objectID;
            ((StringValue)ev["StimulusType"]).value = stimulusType;
            ((IntegerValue)ev["Time"]).value = time;

            _dddConnection.SendSimEvent(ev);
        }
        #endregion
        private String GetAttributeType(String attName)
        {
            foreach (String s in _simModel.objectModel.objects["PhysicalObject"].attributes.Keys)
            {
                if (s == attName)
                {
                    return _simModel.objectModel.objects["PhysicalObject"].attributes[s].dataType;
                }
            }
            return "DataValue";
        }
        public DataValue GetCorrectDataValue(string key, string value)
        {
            DataValue dv = null;
            String attType = GetAttributeType(key);
            switch (attType)
            {
                case "StringType":
                    dv = DataValueFactory.BuildString(value);
                    break;
                case "IntegerType":
                    dv = DataValueFactory.BuildInteger(Int32.Parse(value));
                    break;
                //case "LocationType":
                //    dv = DataValueFactory.BuildLocation(value);
                //    break;
                //case "VelocityType":
                //    dv = DataValueFactory.BuildVelocity(Int32.Parse(value));
                //    break;
                case "DoubleType":
                    dv = DataValueFactory.BuildDouble(double.Parse(value));
                    break;
                case "BooleanType":
                    dv = DataValueFactory.BuildBoolean(bool.Parse(value));
                    break;
                //case "StringListType":
                //    dv = DataValueFactory.BuildStringList(value);
                //    break;

                default: //state table, capability, vulnerability, detectedattribute, sensor array, emitter, custom attributes, attack collection, ClassificationDisplayRulesType
                    break;
            }
            return dv;
        }
    }
}
