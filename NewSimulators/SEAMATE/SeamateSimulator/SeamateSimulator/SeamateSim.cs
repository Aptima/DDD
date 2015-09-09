using System.Collections.Generic;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using System;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.SeamateSimulator
{
    public class SeamateProcessorSim : ISimulator
    {
        private class ItemInfo
        {
            public String targetObject = "";
            public int stimulusTime = 0;
            public string itemID;
            public string userId;
            public string objectId;
            public double ff_dif = 0.0f;
            public double tt_dif = 0.0f;
            public string StimulusType;

            public ItemInfo(string itemID, string userId, string objectId, double ff_dif, double tt_dif, string StimulusType, string targetObject, int time)
            {
                this.itemID = itemID;
                this.userId = userId;
                this.objectId = objectId;
                this.ff_dif = ff_dif;
                this.tt_dif = tt_dif;
                this.StimulusType = StimulusType;
                this.targetObject = targetObject;
                this.stimulusTime = time;
            }
        }
        private Dictionary<String, ItemInfo> mostRecentItemsForStimulators;
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;
        private Dictionary<string, SimulationObjectProxy> objectProxies;
        private int time = 0;
        private Dictionary<String, bool> scoreRulesExist;
        private Dictionary<String, int> scoreValues;
        private Dictionary<String, String> _terminalToDMMap = null;
        private List<String> handledAttacks;
        private List<String> dms;
        private Dictionary<String, String> classifications;
        private Dictionary<String, String> objectAssignmentList;
        private bool IndividualDMIsLoggedIn = false;
        private const String HostileDestroyed = "Hostile Targets Destroyed";
        private const String FriendlyDestroyedByPlayers = "Number of friendlies destroyed by players";
        private const String FriendlyLost = "Number of friendly vessels that were lost";
       // private const String HitsTaken = "Number of hits taken by friendly air assets";
        private const String RulesOfEngagementViolated = "Rules of Engagement Violations";
        private List<String> _attackTargetHashes = null;

        public SeamateProcessorSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
            scoreRulesExist = null;
            handledAttacks = null;
            dms = null;
            classifications = null;
            _terminalToDMMap = null;
            IndividualDMIsLoggedIn = false;
            objectAssignmentList = null;
            _attackTargetHashes = null;
            mostRecentItemsForStimulators = null;
        }
        #region ISimulator Members

        public string GetSimulatorName()
        {
            return "SeamateProcessor";
        }

        public void Initialize(ref CommonComponents.SimulationModelTools.SimulationModelInfo simModel, ref CommonComponents.SimulationObjectTools.Blackboard blackboard, ref CommonComponents.SimulationEventTools.SimulationEventDistributor distributor)
        {
            IndividualDMIsLoggedIn = false;
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;
            scoreRulesExist = new Dictionary<string, bool>();
            handledAttacks = new List<string>();
            classifications = new Dictionary<string, string>();
            _terminalToDMMap = new Dictionary<string, string>();
            dms = new List<string>();
            objectAssignmentList = new Dictionary<string, string>();
            mostRecentItemsForStimulators = new Dictionary<string,ItemInfo>();
            _attackTargetHashes = new List<string>();
            dms.Add("BAMS DM");
            dms.Add("Firescout DM");
            dms.Add("Individual DM");
            distributor.RegisterClient(ref distClient);

            blackboard.RegisterClient(ref bbClient);
            //only need to register for attributes we're concerned with: ID, OwnerID, Location, DefaultClassification
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "State", true, false);
            bbClient.Subscribe("PhysicalObject", "ClassName", true, false);

            bbClient.Subscribe("PhysicalObject", "DefaultClassification", true, false);
            bbClient.Subscribe("PhysicalObject", "CurrentClassification", true, false); //this might not be sent to simulators, only DDD Clients
            bbClient.Subscribe("PhysicalObject", "InActiveRegions", true, false);
            bbClient.Subscribe("PhysicalObject", "DestinationLocation", true, false);
            bbClient.Subscribe("PhysicalObject", "Intent", true, true);


            #region Attributes Specifically For SEAMATE
            bbClient.Subscribe("PhysicalObject", "RevealTime", true, true); //int
            bbClient.Subscribe("PhysicalObject", "DetectTime", true, true); //int
            bbClient.Subscribe("PhysicalObject", "IdentifyTime", true, true); //int 
            bbClient.Subscribe("PhysicalObject", "TrackingTime", true, true); //int, not sure how this will be changed yet.
            bbClient.Subscribe("PhysicalObject", "DestroyedTime", true, true); //int
            bbClient.Subscribe("PhysicalObject", "TrackedBy", true, true);
            bbClient.Subscribe("PhysicalObject", "DestroyedBy", true, true);
            bbClient.Subscribe("PhysicalObject", "IdentifiedBy", true, true);
            bbClient.Subscribe("PhysicalObject", "ClassifiedBy", true, true);
            bbClient.Subscribe("PhysicalObject", "DetectedBy", true, true);
            /*
             * GroundTruthIFF is either "Hostile", "Unknown", or "Friendly".  On reveal, everything should be set to Unknown.
             * If this object attacks any sea vessel, it should be set to "Hostile".
             * UserClassifiedIFF is either "Hostile", "Unknown", or "Friendly", and is set when a user classifies an object.
             * This is their PERCEIVED friendliness of an object.
             */
            bbClient.Subscribe("PhysicalObject", "GroundTruthIFF", true, true); //string 
            bbClient.Subscribe("PhysicalObject", "UserClassifiedIFF", true, true); //string
            bbClient.Subscribe("PhysicalObject", "HostileActionTime", true, true); //int
            bbClient.Subscribe("PhysicalObject", "IsInSeaLane", true, true); //bool
            bbClient.Subscribe("PhysicalObject", "IsGoingTowardsPort", true, true); //bool

            #endregion

            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        public void ProcessEvent(CommonComponents.SimulationEventTools.SimulationEvent e)
        {
            Console.Out.WriteLine("in processevent with eventType: " + e.eventType);
            switch (e.eventType)
            {
                case "NewObject":
                    NewObject(e);
                    break;
                case "TimeTick":
                    TimeTick(e);
                    break;
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "AttackObject":
                    AttackObject(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "ForceUpdateObjectAttribute":
                    ForceUpdateObjectAttribute(e);
                    break;
                case "ClientMeasure_ObjectSelected":
                    ClickHandler(e);
                    break;
                case "ObjectClassificationRequest":
                    ClassificationChanged(e);
                    break;
                case "AttackSucceeded":
                    AttackSucceeded(e);
                    break;
                case "SEAMATE_TrackAdded":
                    Console.Out.WriteLine("----------------SEAMATE_TrackAdded");
                    TrackAdded(e);
                    break;
                case "SEAMATE_TrackRemoved":
                    Console.Out.WriteLine("----------------SEAMATE_TrackRemoved");
                    TrackRemoved(e);
                    break;
                //case "AuthenticationRequest":
                //case "AuthenticationResponse":
                //case "HandshakeGUIRegister":
                //case "HandshakeGUIRoleRequest":
                case "HandshakeInitializeGUI":
                    String playerID = ((StringValue)e["PlayerID"]).value;
                    if (playerID == "AgentDM")
                        return;
                    String terminalID = ((StringValue)e["TerminalID"]).value;
                    if (_terminalToDMMap.ContainsKey(terminalID))
                    {
                        _terminalToDMMap[terminalID] = playerID;
                    }
                    else
                    {
                        _terminalToDMMap.Add(terminalID, playerID);
                    }
                    if (playerID.ToLower().StartsWith("individual"))
                    {
                        IndividualDMIsLoggedIn = true;
                    }
                    break;
                case "SEAMATE_RequestMyDecisionMaker":
                    String compName = ((StringValue)e["ComputerName"]).value;
                    String terminal = ((StringValue)e["TerminalID"]).value;
                    foreach (String s in _terminalToDMMap.Keys)
                    {
                        if (s.StartsWith(compName))
                        {
                            SendDecisionMakerResponseEvent(terminal, _terminalToDMMap[s]);
                            break;
                        }
                    }
                    SendDecisionMakerResponseEvent(terminal, "");
                    break;
                case "SEAMATE_StimulusSent":
                    StimulusItem(e);
                    break;
                case "SelfDefenseAttackStarted":
                    SelfDefenseAttackStarted(e);
                    break;
                default:
                    break;
            }
        }

        private void SendDecisionMakerResponseEvent(string terminal, string dmID)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "SEAMATE_ResponseDecisionMaker");
            ((StringValue)ev["TerminalID"]).value = terminal;
            ((StringValue)ev["DM_ID"]).value = dmID;

            distributor.PutEvent(ev);
        }

        private bool DMAssignedAsset(String dmID, String objectID)
        { 
            if(objectAssignmentList.ContainsKey(objectID))
            {
                if (objectAssignmentList[objectID].Contains(dmID))
                    return true;
            }

            return false;
        }

        #endregion

        #region Handlers
        private void SelfDefenseAttackStarted(SimulationEvent e)
        {
            //<SelfDefenseAttackStarted>
            //<Parameter><Name>AttackerObjectID</Name><Value><StringType>3734</StringType></Value></Parameter>
            //<Parameter><Name>TargetObjectID</Name><Value><StringType>2213</StringType></Value></Parameter>
            //<Parameter><Name>Time</Name><Value><IntegerType>62000</IntegerType></Value></Parameter></SelfDefenseAttackStarted>
            String attackerID = ((StringValue)e["AttackerObjectID"]).value;
            String targetID = ((StringValue)e["TargetObjectID"]).value;
            String hash = attackerID + "_" + targetID;
            if (_attackTargetHashes.Contains(hash))
                return;//only handle one attack stimuli from one target-attacker pair
            SimulationObjectProxy attProx = objectProxies[attackerID];
            SimulationObjectProxy targProx = objectProxies[targetID];
            if (attProx == null || targProx == null)
                return;
            //if attacker is owned by pirate DM, send stimulus event.
            if (((StringValue)attProx["OwnerID"].GetDataValue()).value == "Pirate DM")
            { 
              //i don't know an item id unfortunately.
                String itemID = "UNK?";
                String dmID = "";
                String type = "Attack";
                if (objectAssignmentList.ContainsKey(attackerID))
                {
                    dmID = objectAssignmentList[attackerID];
                }
                ItemInfo lastItem = null;
                if (mostRecentItemsForStimulators.ContainsKey(attackerID))
                {
                    lastItem = mostRecentItemsForStimulators[attackerID];// = new ItemInfo(itemID, userId, objectId, ff_dif, tt_dif, StimulusType, "", time);
                }
                else
                {
                    lastItem = new ItemInfo(itemID, dmID, attackerID, 0.0, 0.0, type, targetID, 0);
                }
                
                SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "SEAMATE_StimulusSent");
                ((StringValue)ev["DM_ID"]).value = lastItem.userId;
                ((StringValue)ev["ItemID"]).value = lastItem.itemID;
                ((StringValue)ev["ObjectID"]).value = lastItem.objectId;
                ((StringValue)ev["StimulusType"]).value = type;
                ((DoubleValue)ev["FFDifficulty"]).value = lastItem.ff_dif;
                ((DoubleValue)ev["TTDifficulty"]).value = lastItem.tt_dif;
                distributor.PutEvent(ev);
                _attackTargetHashes.Add(hash);
            }

        }
        private void StimulusItem(SimulationEvent e)
        {
            string objectId = ((StringValue)e["ObjectID"]).value;
            string userId = ((StringValue)e["DM_ID"]).value;
            string itemID = ((StringValue)e["ItemID"]).value;
            string StimulusType = ((StringValue)e["StimulusType"]).value;
            double ff_dif = ((DoubleValue)e["FFDifficulty"]).value;
            double tt_dif = ((DoubleValue)e["TTDifficulty"]).value;
            int time = ((IntegerValue)e["Time"]).value;
            if (objectAssignmentList.ContainsKey(objectId))
            {
                objectAssignmentList[objectId] = objectAssignmentList[objectId]+"|"+userId;
            }
            else
            {
                objectAssignmentList.Add(objectId, userId);
            }
            if (mostRecentItemsForStimulators.ContainsKey(objectId))
            {
                mostRecentItemsForStimulators[objectId] = new ItemInfo(itemID, userId, objectId, ff_dif, tt_dif, StimulusType,"",time);
            }
            else
            { 
                mostRecentItemsForStimulators.Add(objectId,new ItemInfo(itemID, userId, objectId, ff_dif, tt_dif, StimulusType,"",time));
            }
        }

        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string id = ((StringValue)e["ID"]).value;
            string type = ((StringValue)e["ObjectType"]).value;

            if (objectProxies == null)
            {
                objectProxies = new Dictionary<string, SimulationObjectProxy>();
            }
            SimulationObjectProxy prox = bbClient.GetObjectProxy(id);
            if (prox == null)
            { return; }
            if (!objectProxies.ContainsKey(id))
            {
                objectProxies.Add(id, prox);
            }
            else
            {
                objectProxies[id] = prox;
            }

            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];
            if (atts.attributes.ContainsKey("ID"))
            {
                string id2 = ((StringValue)atts["ID"]).value;
                if (objectProxies.ContainsKey(id2))
                {
                    SimulationObjectProxy proxi = objectProxies[id2];

                    foreach (string attname in atts.attributes.Keys)
                    {
                        if (proxi.GetKeys().Contains(attname) && proxi[attname].IsOwner())
                        {
                            proxi[attname].SetDataValue(atts[attname]);
                        }
                    }
                }
            }

        }

        private void TimeTick(SimulationEvent e)
        {
            //update time
            if (((IntegerValue)e["Time"]).value % 1000 == 0)
                time = ((IntegerValue)e["Time"]).value / 1000; // time is in ms, we want seconds
            /*
             * "Time" is an attribute of all events.  The SimulationModel.xml file lists all of the top-level attributes for each event.
             * Certain events have an additional "Attribute" attribute, which contains a key-value pair collection of additional attributes.
             * See RevealObject for an example of this.
             */
            if (((IntegerValue)e["Time"]).value == 1000)
            {
                InitializeAllScores();
            }
            SimulationObjectProxy obProx;
            foreach (string id in objectProxies.Keys)
            {
                obProx = objectProxies[id];
                bool isInSealane = false;
                bool movingTowardsPort = false;
                StringListValue slv = obProx["InActiveRegions"].GetDataValue() as StringListValue;
                LocationValue dest = obProx["DestinationLocation"].GetDataValue() as LocationValue;
                if(dest.exists)
                {
                    Vec2D destVec = new Vec2D(dest);
                    Polygon2D p;
                    foreach(Aptima.Asim.DDD.CommonComponents.SimulatorTools.StateDB.ActiveRegion a in StateDB.activeRegions.Values)
                    {
                        if (!a.id.Contains("Entry-"))
                            continue;
                        p = new Polygon2D(); 
                        p = a.poly.Footprint;

                        if (Aptima.Asim.DDD.CommonComponents.SimMathTools.Polygon2D.IsPointInside(p, destVec))
                        {
                            movingTowardsPort = true;
                        }
                    }
                }
                
                

                if (slv.strings.Count > 0)
                    isInSealane = true;                

                obProx["IsInSeaLane"].SetDataValue(DataValueFactory.BuildBoolean(isInSealane));
                obProx["IsGoingTowardsPort"].SetDataValue(DataValueFactory.BuildBoolean(movingTowardsPort));
            }
        }

        private void RevealObject(SimulationEvent e)
        {

            String objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy proxy = objectProxies[objectID];

            //initialize values for times to -1
            proxy["DetectTime"].SetDataValue(DataValueFactory.BuildInteger(-1));
            proxy["IdentifyTime"].SetDataValue(DataValueFactory.BuildInteger(-1));
            proxy["TrackingTime"].SetDataValue(DataValueFactory.BuildInteger(-1));
            proxy["DestroyedTime"].SetDataValue(DataValueFactory.BuildInteger(-1));
            proxy["HostileActionTime"].SetDataValue(DataValueFactory.BuildInteger(-1));
            proxy["GroundTruthIFF"].SetDataValue(DataValueFactory.BuildString("Unknown"));
            proxy["UserClassifiedIFF"].SetDataValue(DataValueFactory.BuildString("Unknown"));
            proxy["RevealTime"].SetDataValue(DataValueFactory.BuildInteger(time));
            proxy["TrackedBy"].SetDataValue(DataValueFactory.BuildString(""));

            if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("DefaultClassification"))
            {
                //has IFF on
                String iff = ((StringValue)((AttributeCollectionValue)e["Attributes"]).attributes["DefaultClassification"]).value;
                proxy["GroundTruthIFF"].SetDataValue(DataValueFactory.BuildString(iff));
                proxy["UserClassifiedIFF"].SetDataValue(DataValueFactory.BuildString(iff));
            }
            else
            { 
                String owner = ((StringValue)proxy["OwnerID"].GetDataValue()).value;
                if (owner.ToLower().Contains("pirate"))
                {
                    proxy["GroundTruthIFF"].SetDataValue(DataValueFactory.BuildString("Suspect"));
                }

            }
            proxy["IsInSeaLane"].SetDataValue(DataValueFactory.BuildBoolean(false));
            proxy["IsGoingTowardsPort"].SetDataValue(DataValueFactory.BuildBoolean(false));

        }


        private void ClickHandler(SimulationEvent e)
        {
            String objectID = ((StringValue)e["ObjectID"]).value;
            String dmID = ((StringValue)e["UserID"]).value;
            if (objectID == string.Empty)
                return;//weird edge case
            if (objectProxies.Count == 0)
                return; //another weird edge case
            SimulationObjectProxy proxy = objectProxies[objectID];
            //AD: TODO Need to only set this if the target object's Item belongs to the clicking DM
            //TODO": add attributes for onCourse, onLocation, booleans for if the objects are in the correct regions and moving towards the correct regions

            if (DMAssignedAsset(dmID, objectID) || IndividualDMIsLoggedIn)
            {
                if (((IntegerValue)proxy["DetectTime"].GetDataValue()).value != -1)
                {
                    return;
                }
                else
                {
                    proxy["DetectTime"].SetDataValue(DataValueFactory.BuildInteger(time));
                    proxy["DetectedBy"].SetDataValue(DataValueFactory.BuildString(((StringValue)e["UserID"]).value));
                }
            }
        }

        private void ClassificationChanged(SimulationEvent e)
        {
            String objectID = ((StringValue)e["ObjectID"]).value;
            if (objectProxies.Count == 0)
                return; //another weird edge case
            SimulationObjectProxy proxy = objectProxies[objectID];
            proxy["IdentifyTime"].SetDataValue(DataValueFactory.BuildInteger(time));
            proxy["IdentifiedBy"].SetDataValue(DataValueFactory.BuildString(((StringValue)e["UserID"]).value));
            proxy["ClassifiedBy"].SetDataValue(DataValueFactory.BuildString(((StringValue)e["UserID"]).value));

            String classification = ((StringValue)e["ClassificationName"]).value;
            proxy["UserClassifiedIFF"].SetDataValue(DataValueFactory.BuildString(classification));

            if (!classifications.ContainsKey(objectID))
                classifications.Add(objectID, "");
            classifications[objectID] = classification;
        }

        private void AttackSucceeded(SimulationEvent e)
        {
            if (objectProxies.Count == 0)
                return; //another weird edge case
            try
            {
                String objectID = ((StringValue)e["ObjectID"]).value;
                SimulationObjectProxy proxy = objectProxies[objectID];

                String targetID = ((StringValue)e["TargetID"]).value;
                SimulationObjectProxy targetProxy = objectProxies[targetID];

                String transitionState = ((StringValue)e["NewState"]).value;

                // It's a sea vessel if it's not a BAMS or Firescout
                String targetClassName = ((StringValue)targetProxy["ClassName"].GetDataValue()).value;
                if (targetClassName != "BAMS" && targetClassName != "Firescout" && transitionState == "Dead")
                {
                    targetProxy["DestroyedTime"].SetDataValue(DataValueFactory.BuildInteger(time));
                    targetProxy["DestroyedBy"].SetDataValue(DataValueFactory.BuildString(objectID));

                    //AD: TODO Send view pro attribute event for these objects.
                    SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "ViewProAttributeUpdate");
                    AttributeCollectionValue acv = new AttributeCollectionValue();
  //                  acv.attributes.Add("DestroyedTime", DataValueFactory.BuildDetectedValue(DataValueFactory.BuildInteger(time), 100));
//                    acv.attributes.Add("DestroyedBy", DataValueFactory.BuildDetectedValue(DataValueFactory.BuildString(objectID), 100));
                    acv.attributes.Add("DestroyedTime", DataValueFactory.BuildInteger(time));
                    acv.attributes.Add("DestroyedBy", DataValueFactory.BuildString(objectID));


                    ((StringValue)ev["TargetPlayer"]).value = "BAMS DM";
                    ((StringValue)ev["ObjectID"]).value = ((StringValue)targetProxy["ID"].GetDataValue()).value;
                    ((StringValue)ev["OwnerID"]).value = ((StringValue)targetProxy["OwnerID"].GetDataValue()).value;
                    ((IntegerValue)ev["Time"]).value = time*1000;
                    ev["Attributes"] = acv;

                    distClient.PutEvent(ev);

                    ((StringValue)ev["TargetPlayer"]).value = "Firescout DM";
                    distClient.PutEvent(ev);
                    ((StringValue)ev["TargetPlayer"]).value = "Individual DM";
                    distClient.PutEvent(ev);

                }

                String targetOwnerID = ((StringValue)targetProxy["OwnerID"].GetDataValue()).value;
                String attackerOwnerID = ((StringValue)proxy["OwnerID"].GetDataValue()).value;


                if (transitionState == "Dead")
                {
                    //Clear the intent of any other vessel mentioned in target's intent (pursued or pursuee).
                    String targetIntent = ((StringValue)targetProxy["Intent"].GetDataValue()).value;
                    if (targetIntent != "")
                    {
                        String[] intentArray = targetIntent.Split(":".ToCharArray());
                        if (intentArray.Length >1) {
                            SimulationObjectProxy vesselProxyToClear = objectProxies[intentArray[1]];
                            vesselProxyToClear["Intent"].SetDataValue(DataValueFactory.BuildString(""));
                        }
                    }

                    //IF friendly, increment counter
                    if (!targetOwnerID.ToLower().Contains("pirate"))
                    {
                        IncrementFriendliesLost(1);
                        String attackClassName = ((StringValue)proxy["ClassName"].GetDataValue()).value;
                        if (attackClassName == "BAMS" || attackClassName == "Firescout")
                        {
                            //IF friendly, and attacker was BAMS/FS
                            IncrementFriendliesDestroyedByPlayers(1);
                        }

                    }
                    else
                    {
                        //if hostile increment counter
                        IncrementHostileTargetsDestroyed(1);
                    }
                }
                //if (targetClassName == "BAMS" || targetClassName == "Firescout")
                //{
                //    //if bams/firescout, increment counter
                //    IncrementHitsTakenByAssets(1);
                //}

                if (!attackerOwnerID.Contains("BAMS") && !attackerOwnerID.Contains("Firescout") && !attackerOwnerID.Contains("Individual"))
                    return; //the following section is only for YOUR attacks, so return if its not BAMS or FS

                String attackClass = ((StringValue)proxy["ClassName"].GetDataValue()).value;
                String targetClassification = "";
                try
                {
                    targetClassification = classifications[targetID];
                }
                catch (Exception exc)
                { }
                if (targetClassification != "Hostile" && (targetClassName != "BAMS" || targetClassName != "Firescout"))
                {
                    //if asset was not classified as hostile, violated ROE
                    IncrementRulesOfEngagementViolations(1);
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void AttackObject(SimulationEvent e)
        {
            String objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy proxy = objectProxies[objectID];
            String targetID = ((StringValue)e["TargetObjectID"]).value;
            SimulationObjectProxy targetProxy = objectProxies[targetID];

            // It's a sea vessel if it's not a BAMS or Firescout
            String className = ((StringValue)proxy["ClassName"].GetDataValue()).value;
            if (className != "BAMS" && className != "Firescout")
            {
                proxy["HostileActionTime"].SetDataValue(DataValueFactory.BuildInteger(time));
                proxy["GroundTruthIFF"].SetDataValue(DataValueFactory.BuildString("Hostile"));
            }
            

        }

        private void TrackAdded(SimulationEvent e)
        {
            if (objectProxies.Count == 0)
                return; //another weird edge case
            String objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy proxy = objectProxies[objectID];

            string id = ((StringValue)e["UserID"]).value;
            proxy["TrackedBy"].SetDataValue(DataValueFactory.BuildString(id));
            proxy["TrackingTime"].SetDataValue(DataValueFactory.BuildInteger(time));
        }

        private void TrackRemoved(SimulationEvent e)
        {
            if (objectProxies.Count == 0)
                return; //another weird edge case
            String objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy proxy = objectProxies[objectID];

            proxy["TrackedBy"].SetDataValue(DataValueFactory.BuildString(""));

        }


        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
            ClearAllScores();
            //Initialize(ref simModel, ref blackboard, ref distributor);
        }
        private void ForceUpdateObjectAttribute(SimulationEvent e)
        {

            string objectId = ((StringValue)e["ObjectID"]).value;
            string attributeName = ((StringValue)e["AttributeName"]).value;
            if (!objectProxies.ContainsKey(objectId))
            {
                return;
            }
            try
            {
                if (!objectProxies[objectId].GetKeys().Contains(attributeName))
                    return;
                if (!objectProxies[objectId][attributeName].IsOwner())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            DataValue attributeValue = e["AttributeValue"];
            if (attributeValue.dataType != "WrapperType")
                return;
            attributeValue = ((WrapperValue)attributeValue).value;
            SimulationObjectProxy obj = null;

            //depending on the attribute, you might have specific functionality
            switch (attributeName)
            {
                case "Intent":
                    ChangeIntent(attributeValue as StringValue, objectId);
                    break;
                default:
                    break;
            }

            //try
            //{
            //    obj[attributeName].SetDataValue(attributeValue);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}

            attributeValue = null;
            obj = null;
        }
        #endregion
        #region Send Events
        private void InitializeAllScores()
        {
            return;
            if (!ScoringDB.scores.ContainsKey(HostileDestroyed))
            {
                ScoringDB.scores.Add(HostileDestroyed, new ScoringDB.Score(HostileDestroyed, dms, dms, 0.0));
            }
            if (!ScoringDB.scores.ContainsKey(FriendlyDestroyedByPlayers))
            {
                ScoringDB.scores.Add(FriendlyDestroyedByPlayers, new ScoringDB.Score(FriendlyDestroyedByPlayers, dms, dms, 0.0));
            }
            if (!ScoringDB.scores.ContainsKey(FriendlyLost))
            {
                ScoringDB.scores.Add(FriendlyLost, new ScoringDB.Score(FriendlyLost, dms, dms, 0.0));
            }
            //if (!ScoringDB.scores.ContainsKey(HitsTaken))
            //{
            //    ScoringDB.scores.Add(HitsTaken, new ScoringDB.Score(HitsTaken, dms, dms, 0.0));
            //}
            if (!ScoringDB.scores.ContainsKey(RulesOfEngagementViolated))
            {
                ScoringDB.scores.Add(RulesOfEngagementViolated, new ScoringDB.Score(RulesOfEngagementViolated, dms, dms, 0.0));
            }
        }
        private void ClearAllScores()
        {
            if (ScoringDB.scores.ContainsKey(HostileDestroyed))
            {
                ScoringDB.scores.Remove(HostileDestroyed);
            }
            if (ScoringDB.scores.ContainsKey(FriendlyDestroyedByPlayers))
            {
                ScoringDB.scores.Remove(FriendlyDestroyedByPlayers);
            }
            if (ScoringDB.scores.ContainsKey(FriendlyLost))
            {
                ScoringDB.scores.Remove(FriendlyLost);
            }
            //if (ScoringDB.scores.ContainsKey(HitsTaken))
            //{
            //    ScoringDB.scores.Remove(HitsTaken);
            //}
            if (ScoringDB.scores.ContainsKey(RulesOfEngagementViolated))
            {
                ScoringDB.scores.Remove(RulesOfEngagementViolated);
            }
        }
        private void IncrementHostileTargetsDestroyed(int incrementBy)
        {
            return;
            if (!ScoringDB.scores.ContainsKey(HostileDestroyed))
            {
                ScoringDB.scores.Add(HostileDestroyed, new ScoringDB.Score(HostileDestroyed, dms, dms, 0.0));
            }
            ScoringDB.scores[HostileDestroyed].scoreValue += incrementBy;
        }
        private void IncrementFriendliesDestroyedByPlayers(int incrementBy)
        {
            return;
            if (!ScoringDB.scores.ContainsKey(FriendlyDestroyedByPlayers))
            {
                ScoringDB.scores.Add(FriendlyDestroyedByPlayers, new ScoringDB.Score(FriendlyDestroyedByPlayers, dms, dms, 0.0));
            }
            ScoringDB.scores[FriendlyDestroyedByPlayers].scoreValue += incrementBy;
        }
        private void IncrementFriendliesLost(int incrementBy)
        {
            return;
            if (!ScoringDB.scores.ContainsKey(FriendlyLost))
            {
                ScoringDB.scores.Add(FriendlyLost, new ScoringDB.Score(FriendlyLost, dms, dms, 0.0));
            }
            ScoringDB.scores[FriendlyLost].scoreValue += incrementBy;
        }
        //private void IncrementHitsTakenByAssets(int incrementBy)
        //{
        //    if (!ScoringDB.scores.ContainsKey(HitsTaken))
        //    {
        //        ScoringDB.scores.Add(HitsTaken, new ScoringDB.Score(HitsTaken, dms, dms, 0.0));
        //    }
        //    ScoringDB.scores[HitsTaken].scoreValue += incrementBy;
        //}
        private void IncrementRulesOfEngagementViolations(int incrementBy)
        {
            return;
            if (!ScoringDB.scores.ContainsKey(RulesOfEngagementViolated))
            {
                ScoringDB.scores.Add(RulesOfEngagementViolated, new ScoringDB.Score(RulesOfEngagementViolated, dms, dms, 0.0));
            }
            ScoringDB.scores[RulesOfEngagementViolated].scoreValue += incrementBy;
        }
        #endregion
        private void ChangeIntent(StringValue newIntent, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["Intent"].SetDataValue(newIntent);
            
        }
    }
}
