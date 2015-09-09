using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.ObjectsAttributeCollection;

//This partial implementation of the ViewPro handles all the "Incoming Events".
//An Incoming Event is called from within the ProcessEvent method, which is called
//from the SimCore when a new event is received.  Centralizing all of these methods
//will allow us to more easily make a change to the basic functionality of the methods
//without having to wade through the rest of the methods.  The methods contained within
//are all also contained in the ISimulator interface.
namespace Aptima.Asim.DDD.Simulators.ViewPro
{
    public partial class ViewProSim : ISimulator
    {
        #region Members
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private List<Attack> currentAttacks;
        private static SimulationEvent initialReplaySpeed = null;
        private List<String> ClassificationsEnum;

        #endregion

        #region misc event methods
        public string GetSimulatorName()
        {
            return "ViewPro";
        }
 
        public ViewProSim()
        {
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
            randomSeed = 8675309;
            ZInverse.InitializeInvZTable();
            currentTick = 0;
        }

        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;
            this.dmColorMapping = new Dictionary<string, int>();
            this.teamDefinitions = new Dictionary<string, List<string>>();
            this.networkRosters = new Dictionary<string, List<string>>();
            this.dmTeamsMap = new Dictionary<string, string>();
            this.networkObjects = new Dictionary<string, List<string>>();
            this.listOfObstructionIDs = new List<string>();
            this.obstructions = new Dictionary<string, StateDB.ActiveRegion>();
            this.listOfObjectIDs = new List<string>();
            this.dmOwnedObjects = new Dictionary<string, List<string>>();
            objectViews = new Dictionary<string, ObjectsAttributeCollection>();
            teamClassifications = new Dictionary<string, Dictionary<string, string>>();
            teamClassificationChanges = new Dictionary<string, Dictionary<string, string>>();
            dmViews = new Dictionary<string, ObjectsAttributeCollection>();
            activeDMs = new List<string>();
            activeSensorNetworks = new Dictionary<string, bool>();
            //singletonDMs = new List<string>();
            currentAttacks = new List<Attack>();
            ClassificationsEnum = new List<String>();
            movingObjects = new List<string>();
            randomGenerator = new Random(randomSeed);
            this.dTimeSec = ((double)simModel.GetUpdateFrequency()) / 1000;
            this.dTimeMSec = simModel.GetUpdateFrequency();
            currentTick = 0;
            distributor.RegisterClient(ref distClient);
            blackboard.RegisterClient(ref bbClient);

            // subscribe to events that aren't OwnerObservable

            bbClient.Subscribe("PhysicalObject", "Emitters", true, false);
            bbClient.Subscribe("PhysicalObject", "RemoveOnDestruction", true, false);
            bbClient.Subscribe("PhysicalObject", "ActiveRegionSpeedMultiplier", true, false);
            bbClient.Subscribe("PhysicalObject", "DefaultClassification", true, false);
            bbClient.Subscribe("PhysicalObject", "ClassificationDisplayRules", true, false);

            foreach (KeyValuePair<string, AttributeInfo> kvp in simModel.objectModel.objects["PhysicalObject"].attributes)
            {
                if (kvp.Value.ownerObservable == true)
                {
                    bbClient.Subscribe("PhysicalObject", kvp.Key, true, false);
                }
            }
            bbClient.Subscribe("DecisionMaker", "RoleName", true, false);

            bbClient.Subscribe("SensorNetwork", "DMMembers", true, false);
            bbClient.Subscribe("Region", "Polygon", true, false);
            bbClient.Subscribe("ActiveRegion", "BlocksSensorTypes", true, false);
            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }
 
        public void ProcessBackChannelEvents()
        {
            List<SimulationEvent> events = distributor.viewProBackChannelEvents;
            distributor.viewProBackChannelEvents = new List<SimulationEvent>();

            foreach (SimulationEvent e in events)
            {
                switch (e.eventType)
                {
                    case "MoveDone":
                        MoveDone(e);
                        break;
                    case "ActiveRegionSpeedMultiplierUpdate":
                        ActiveRegionSpeedMultiplierUpdate(e);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region event processing methods
        public void ProcessEvent(SimulationEvent e)
        {
            //With the new way the simcore runs, with each event that is received, the simcore passes
            //that event out to each simulator.  Thus, the view pro can trigger events for its clients
            //on events besides TimeTick now.

            /****
             * ViewProInitializeObject events should only be sent to a player who can sense that
             * object's Location attribute. 
             * 
             * ViewProMotionUpdates should be broadcast to each client, and it will be up to that 
             * client to process the motion if it contains that object in its playfield.
             * 
             * When weapons and subplatforms are revealed, add the objects to the dmOwnedObjects list.  These
             * objects are not added to these lists upon a reveal.
             */

            //objectProxies = bbClient.GetObjectProxies();//should I limit how often this is called.
            switch (e.eventType)
            {
                case "RandomSeed":
                    randomSeed = ((IntegerValue)e["SeedValue"]).value;
                    randomGenerator = new Random(randomSeed);
                    break;
                case "NewObject":
                    NewObject(e);
                    break;
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "TimeTick":
                    ProcessBackChannelEvents();
                    TimeTick(e);
                    // SendTimeToClient(); //Not needed anymore.
                    break;
                case "MoveObject":
                    MoveObject(e);
                    break;
                //case "MoveDone":
                //    MoveDone(e);
                //    break;
                case "WeaponLaunch":
                    WeaponLaunch(e);
                    break;

                case "SubplatformLaunch":
                    SubplatformLaunch(e);
                    break;
                case "SubplatformDock":
                    SubplatformDock(e);
                    break;
                case "AttackObject":
                    AttackObject(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    isPaused = true;
                    break;
                case "HandshakeInitializeGUIDone":
                    HandshakeInitializeGUIDone(e);
                    break;
                case "DisconnectDecisionMaker":
                    DisconnectDecisionMaker(e);
                    break;
                case "PauseScenario":
                    PauseScenario();
                    isPaused = true;
                    break;
                case "ResumeScenario":
                    ResumeScenario();
                    isPaused = false;
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "SelfDefenseAttackStarted":
                    SelfDefenseAttackStarted(e);
                    break;
                ////
                //Intercepted events from client: Processed, then sent to ScenCon.
                ////
                //case "ClientSubplatformLaunchRequest":
                //    ClientSubplatformLaunchRequest(e);
                //    break;

                case "GameSpeed":
                    GameSpeed(e);
                    break;

                case "TransferObject":
                    TransferObject(e);
                    break;
                case"UpdateTag":
                    UpdateTag(e);
                    break;
                //case "ActiveRegionSpeedMultiplierUpdate":
                //    ActiveRegionSpeedMultiplierUpdate(e);
                //    break;
                case "ClientSideRangeRingDisplayLevel":
                    //Determines how range ring visibilities are calculated for the user.
                    ClientSideRangeRingDisplayLevel(e);
                    break;
                case "ForceUpdateObjectAttribute":
                    ForceUpdateObjectAttribute(e);
                    break;
                    //AD
                //consider UpdateAttackTimeWindow and CancelEngagement here if the ViewPro timeTick event doesn't pick up the change...
                case "CancelEngagement":
                    CancelEngagement(e);
                    break;
                case "UpdateAttackTimeWindow":
                    UpdateAttackTimeWindow(e);
                    break;
                case "ObjectClassificationRequest":
                    ObjectClassificationRequest(e);
                    break;
                case "InitializeClassifications":
                    InitializeClassifications(e);
                    break;
                case "AttackSucceeded":
                    AttackSucceeded(e);
                    break;
                default:
                    break;
            }
        }

        private void AttackSucceeded(SimulationEvent e)
        { 
            //TargetID
            //NewState
            string targetId = ((StringValue)e["TargetID"]).value;
            string state = ((StringValue)e["NewState"]).value;
            if (state == "Dead")
            {
                List<string> sensingObjects = new List<string>();

                
                foreach (String viewAccordingToThisDm in activeDMs)
                {
                    foreach (string sn in networkRosters.Keys)
                    {
                        if (networkRosters[sn].Contains(viewAccordingToThisDm))
                        {
                            foreach (string objID in networkObjects[sn])
                            {
                                if (!sensingObjects.Contains(objID))
                                {
                                    sensingObjects.Add(objID);
                                }
                            }
                        }
                    }
                    SendViewProAttributeForAllObjects(viewAccordingToThisDm, sensingObjects);
                }
            }
        }

        private void ObjectClassificationRequest(SimulationEvent e)
        {
            string userID = ((StringValue)e["UserID"]).value;
            string objectID = ((StringValue)e["ObjectID"]).value;
            string classificationName = ((StringValue)e["ClassificationName"]).value;

            if (dmTeamsMap.ContainsKey(userID))
            {
                if (!teamClassifications.ContainsKey(dmTeamsMap[userID]))
                {
                    teamClassifications[dmTeamsMap[userID]] = new Dictionary<string, string>();
                }

                teamClassifications[dmTeamsMap[userID]][objectID] = classificationName;
                String team = dmTeamsMap[userID];

                foreach (String dm in dmTeamsMap.Keys)
                {
                    if (dmTeamsMap[dm] == team)
                    {
                        if (!teamClassificationChanges.ContainsKey(dm))
                        {
                            teamClassificationChanges[dm] = new Dictionary<string, string>();
                        }

                        teamClassificationChanges[dm][objectID] = classificationName;
                    }
                }
            }
        }

        private String GetClassificationForDM(String objectID, String dmID)
        {
            String classification = String.Empty;

            if (objectProxies.ContainsKey(objectID))
            {
                SimulationObjectProxy ob = objectProxies[objectID];
                classification = ((StringValue)ob["DefaultClassification"].GetDataValue()).value;
            }

            if (dmTeamsMap.ContainsKey(dmID) && teamClassifications.ContainsKey(dmTeamsMap[dmID]) && teamClassifications[dmTeamsMap[dmID]].ContainsKey(objectID))
            {
                classification = teamClassifications[dmTeamsMap[dmID]][objectID];
            }

            return classification;
        }

        private String GetChangedClassificationForDM(String objectID, String dmID)
        {
            if (teamClassificationChanges.ContainsKey(dmID) && teamClassificationChanges[dmID].ContainsKey(objectID))
            {
                String classification = teamClassificationChanges[dmID][objectID];
                teamClassificationChanges[dmID].Remove(objectID);
                return classification;
            }
            else
            {
                return null;
            }
            
        }

        private String GetClassificationBasedIcon(String objectID, String classification)
        {
            String icon = String.Empty;

            if (objectProxies.ContainsKey(objectID))
            {
                SimulationObjectProxy ob = objectProxies[objectID];
                String curState = "Dead";
                curState = ((StringValue)ob["State"].GetDataValue()).value;
                ClassificationDisplayRulesValue cdr = (ClassificationDisplayRulesValue)ob["ClassificationDisplayRules"].GetDataValue();

                foreach (ClassificationDisplayRulesValue.ClassificationDisplayRule r in cdr.rules)
                {
                    if (r.Classification == classification && r.State == curState)
                    {
                        icon = r.DisplayIcon;
                        break;
                    }
                }
            }

            return icon;
        }

        private void UpdateAttackTimeWindow(SimulationEvent e)
        { 
            string targetObjectId = ((StringValue)e["TargetObjectID"]).value;
            string attackingObjectId = ((StringValue)e["AttackingObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;
            int window = ((IntegerValue)e["NewAttackTimeWindow"]).value;

            for (int x = 0; x < this.currentAttacks.Count; x++)
            {
                if (currentAttacks[x].attacker == attackingObjectId && currentAttacks[x].target == targetObjectId && currentAttacks[x].capabilityName == capabilityName)
                {
                    currentAttacks[x].SetTimes(currentAttacks[x].startTime, window);
                }
            }
        }

        private void CancelEngagement(SimulationEvent e)
        {
            string targetObjectId = ((StringValue)e["TargetObjectID"]).value;
            string attackingObjectId = ((StringValue)e["AttackingObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;

            for (int x = 0; x < this.currentAttacks.Count; x++)
            {
                if (currentAttacks[x].attacker == attackingObjectId && currentAttacks[x].target == targetObjectId && currentAttacks[x].capabilityName == capabilityName)
                {
                    currentAttacks.RemoveAt(x);

                    Attack tmp = new Attack(attackingObjectId, targetObjectId, capabilityName);
                    tmp.SetTimes(currentAttacks[x].startTime,0); //remove in client view with tmp
                    SendAttackEvent(tmp);
                    return;
                }
            }
        }

        private void ForceUpdateObjectAttribute(SimulationEvent e)
        {
            //if location send location/movement update
            string attributeName = ((StringValue)e["AttributeName"]).value;
            DataValue val = ((WrapperValue)e["AttributeValue"]).value;
            string objectID = ((StringValue)e["ObjectID"]).value;
            if (!objectProxies.ContainsKey(objectID))
            {
                return;
            }
            if (attributeName == "Location")
            {
                LocationValue origLocation = objectProxies[objectID]["Location"].GetDataValue() as LocationValue;
                LocationValue origDestination = objectProxies[objectID]["DestinationLocation"].GetDataValue() as LocationValue;

                AttributeCollectionValue acv = new AttributeCollectionValue();
                acv.attributes.Add(attributeName, val);
                acv.attributes.Add("ObjectID", DataValueFactory.BuildString(objectID));
                if (DataValueFactory.CompareDataValues(origDestination, origLocation))
                {
                    acv.attributes.Add("DestinationLocation", val); // if object wasn't moving, keep it that way.
                }
                SendViewProMotionUpdate(acv);
            }

                return;

        }

        private void ClientSideRangeRingDisplayLevel(SimulationEvent e)
        {
            String value = ((StringValue)e["Value"]).value;
            value = value.ToLower();
            if (value == "disabled")
            {
                selectedRangeRingLevel = RangeRingLevels.DISABLED;
            }
            else if (value == "personal")
            {
                selectedRangeRingLevel = RangeRingLevels.PRIVATE;
            }
            else if (value == "sharedsensornetwork")
            {
                selectedRangeRingLevel = RangeRingLevels.SENSORNETWORK;
            }
            else if(value == "full")
            {
                selectedRangeRingLevel = RangeRingLevels.FULL;
            }
        }
 
        private void TransferObject(SimulationEvent e)
        {
            string objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy prox = objectProxies[objectID];

            string userID = ((StringValue)e["UserID"]).value;

            string donorUserID = ((StringValue)e["DonorUserID"]).value;
            string objectState = ((StringValue)prox["State"].GetDataValue()).value;
            if (objectState == "Dead")
            {
                bool removeOnDestruction = ((BooleanValue)prox["RemoveOnDestruction"].GetDataValue()).value;
                if (removeOnDestruction)
                {
                    return; //don't display this dead object to the user.
                }
            }


            ClearObjectFromViews(objectID);

            ////Add the object to the new DMs owned list.  It should then get picked up in the sensory calc


            if (dmOwnedObjects[donorUserID].Contains(objectID))
            {
                dmOwnedObjects[donorUserID].Remove(objectID);
                //dmViews[donerUserID].RemoveObject(objectID);
            }
            if (!dmOwnedObjects[userID].Contains(objectID))
            {
                dmOwnedObjects[userID].Add(objectID);
                //don't add, CompareWithPrevious... will add and send initialize event
            }
            foreach (string sensorNet in networkRosters.Keys)
            {
                if (networkRosters[sensorNet].Contains(donorUserID))
                {
                    if (networkObjects[sensorNet].Contains(objectID))
                    {
                        networkObjects[sensorNet].Remove(objectID);
                    }
                }
                if (networkRosters[sensorNet].Contains(userID))
                {
                    networkObjects[sensorNet].Add(objectID);
                }

            }
            CalculateSensoryAlgorithm();

            /*
            //return;
            string errorAt = "OwnerID";
            //string ownerID;
            string iconName;
            bool isWeapon = false;
            //SensorArrayValue sav;
            LocationValue location;

            location = (LocationValue)prox["Location"].GetDataValue();
            LocationValue dest = (LocationValue)prox["DestinationLocation"].GetDataValue();
            double maxSpeed = ((DoubleValue)(prox["MaximumSpeed"].GetDataValue())).value;
            double throttle = ((DoubleValue)(prox["Throttle"].GetDataValue())).value;
            iconName = ((StringValue)prox["IconName"].GetDataValue()).value;
            isWeapon = ((BooleanValue)prox["IsWeapon"].GetDataValue()).value;
            double activeRegionSpeedMultiplier = ((DoubleValue)(prox["ActiveRegionSpeedMultiplier"].GetDataValue())).value;

            
            foreach (string dm in dmViews.Keys)
            {
                SendRemoveObjectEvent(objectID, dm);
                SendViewProInitializeObject(dm, objectID, location, iconName, userID, isWeapon);
            }
            SendViewProMotionUpdate(objectID, userID, location, dest, maxSpeed, throttle, iconName, isWeapon, activeRegionSpeedMultiplier);
            foreach (string dmID in dmViews.Keys)
            {
                if (!dmViews[dmID].ContainsObject(objectID))
                {
                    continue;
                }

                SendViewProAttributeUpdate(dmID, dmViews[dmID].GetObjectsAttributeCollection(objectID));
            }
            //ResumeScenario();
             */
        }

        private void UpdateTag(SimulationEvent e)
        {
            string unitID = ((StringValue)e["UnitID"]).value;
            string tag = ((StringValue)e["Tag"]).value;
            List<string> teamMembers = ((StringListValue)e["TeamMembers"]).strings;
            if (null == unitTags)
                unitTags = new Dictionary<string, Dictionary<string, string>>();
            if (!unitTags.ContainsKey(unitID))
                unitTags[unitID] = new Dictionary<string, string>();
            if (teamMembers.Count > 0)
            {
                unitTags[unitID][dmTeamsMap[teamMembers[0]]] = tag;
            }
            //for (int mbr = 0; mbr < teamMembers.Count; mbr++)
            //    unitTags[unitID][teamMembers[mbr]] = tag;

            if (null == recentUnitTags)
                recentUnitTags = new Dictionary<string, Dictionary<string, string>>();
            if (!recentUnitTags.ContainsKey(unitID))
                recentUnitTags[unitID] = new Dictionary<string, string>();
            for (int mbr = 0; mbr < teamMembers.Count; mbr++)
                recentUnitTags[unitID][teamMembers[mbr]] = tag;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void GameSpeed(SimulationEvent e)
        {
            initialReplaySpeed = e;
            AttributeCollectionValue atts;
            List<string> myObjects = new List<string>(movingObjects);

            foreach (string obID in myObjects)
            {
                atts = new AttributeCollectionValue();
                atts.attributes.Add("ObjectID", DataValueFactory.BuildString(obID));
                SendViewProStopObjectUpdate(obID);
                SendViewProMotionUpdate(atts);
            }
        }
 
        private void ActiveRegionSpeedMultiplierUpdate(SimulationEvent e)
        {
            AttributeCollectionValue atts;

            atts = new AttributeCollectionValue();
            string id = ((StringValue)e["ObjectID"]).value;
            atts.attributes.Add("ObjectID", DataValueFactory.BuildString(id));
            SendViewProStopObjectUpdate(id);
            SendViewProMotionUpdate(atts);

        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void SelfDefenseAttackStarted(SimulationEvent e)
        {
            string attacker = ((StringValue)e["AttackerObjectID"]).value;
            string target = ((StringValue)e["TargetObjectID"]).value;

            if (DoesAttackListContainThisPair(attacker, target))
                return;

            SimulationObjectProxy obj = objectProxies[attacker];
            AttackCollectionValue attCV = (AttackCollectionValue)obj["CurrentAttacks"].GetDataValue();
            List<AttackCollectionValue.AttackValue> selfDefenses = attCV.GetCurrentSelfDefenseAttacks();
            
            AttackCollectionValue.AttackValue attackValue = null;
            if (selfDefenses.Count > 1)
            {
                Console.WriteLine("SelfDefenseAttackStarted, attacker contains more than one active self defense attack. -> " + selfDefenses.Count.ToString());
                foreach (AttackCollectionValue.AttackValue av in selfDefenses)
                {
                    if (av.attackingObjectId == attacker && av.targetObjectId == target)
                    {
                        attackValue = av;
                    }
                }
            }
            else if(selfDefenses.Count == 1)
            {
                attackValue = selfDefenses[0];
            }
            if (selfDefenses.Count == 0 || attackValue == null)
                return; //self defense not found
            int attackStart = attackValue.attackStartTime;
            int attackLength = attackValue.attackTimeWindow;
            Attack at = new Attack(attacker, target, attackValue.capabilityName);
            at.SetTimes(attackStart, attackLength);

            currentAttacks.Add(at);

        }
  
        /// <summary>
        /// When a NewObject is received, the information about the object will be used to populate some of the
        /// static lists in this simulator.  A sensor network definition will add a sensor network and that network's
        /// roster to the network lists.  A team definition adds that teams hostilities to that list.  A sensor
        /// blocking region gets added to the list of sensor blocking regions.  A DM gets its color added to dmColorMapping
        /// And a new playable object gets its ID added to listOfObjectIDs, and its added to its owner's list.
        /// </summary>
        /// <param name="e">Incoming Event</param>
        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();

            string id = ((StringValue)e["ID"]).value;
            string type = ((StringValue)e["ObjectType"]).value;

            if (objectProxies == null)
            {
                objectProxies = new Dictionary<string, SimulationObjectProxy>();
            }
            SimulationObjectProxy prox = bbClient.GetObjectProxy(id);
            if (prox != null)
            {
                if (!objectProxies.ContainsKey(id))
                {
                    objectProxies.Add(id, prox);
                }
                else
                {
                    objectProxies[id] = prox;
                }
            }

            switch (((StringValue)e["ObjectType"]).value)
            {
                case "Team":
                    List<string> hostilities = ((StringListValue)((AttributeCollectionValue)e["Attributes"]).attributes["TeamHostility"]).strings;
                    if (teamDefinitions.ContainsKey(id))
                    {
                        throw new Exception(String.Format("Error in ViewPro: Trying to add a team ({0}) which already exists.", id));
                    }
                    teamDefinitions.Add(id, hostilities);
                    teamClassifications[id] = new Dictionary<string, string>();
                    break;
                case "SensorNetwork":
                    List<string> roster = ((StringListValue)((AttributeCollectionValue)e["Attributes"]).attributes["DMMembers"]).strings;
                    if (networkRosters.ContainsKey(id))
                    {
                        throw new Exception(String.Format("Error in ViewPro: Trying to add a sensor network ({0}) which already exists.", id));
                    }
                    networkObjects.Add(id, new List<string>());
                    networkRosters.Add(id, roster);
                    if (!activeSensorNetworks.ContainsKey(id))
                    {
                        activeSensorNetworks.Add(id, false);
                    }
                    //foreach (string member in roster)
                    //{
                    //    if (activeDMs.Contains(member))
                    //    {
                    //        activeSensorNetworks[id] = true;
                    //    }
                    //    //singletonDMs.Remove(member);
                    //}
                    break;
                case "DecisionMaker":
                    string team = ((StringValue)((AttributeCollectionValue)e["Attributes"]).attributes["TeamMember"]).value;
                    if (dmColorMapping.ContainsKey(id))
                    {
                        throw new Exception(String.Format("Error in ViewPro: Trying to add a Decision Maker ({0}) that has already been added.", id));
                    }
                    dmColorMapping.Add(id, ((IntegerValue)((AttributeCollectionValue)e["Attributes"]).attributes["Color"]).value);
                    dmOwnedObjects.Add(id, new List<string>());
                    dmViews.Add(id, new ObjectsAttributeCollection());
                    dmTeamsMap.Add(id, team);
                    foreach (KeyValuePair<string, List<string>> nets in networkRosters)
                    {
                        if (nets.Value.Contains(id))
                        {
                            if (!activeSensorNetworks.ContainsKey(nets.Key))
                                activeSensorNetworks.Add(nets.Key, false);

                            activeSensorNetworks[nets.Key] = true;
                        }
                    }
                    //singletonDMs.Add(id);
                    break;
                case "ActiveRegion":
                    listOfObstructionIDs.Add(id);
                    if (!((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("BlocksSensorTypes"))
                        break;
                    obstructions.Add(id, new StateDB.ActiveRegion(id, new Polygon3D(((DoubleValue)((AttributeCollectionValue)e["Attributes"])["StartHeight"]).value, ((DoubleValue)((AttributeCollectionValue)e["Attributes"])["EndHeight"]).value)));
                    foreach (PolygonValue.PolygonPoint vert in ((PolygonValue)((AttributeCollectionValue)e["Attributes"])["Polygon"]).points)
                    {
                        obstructions[id].poly.AddVertex(new Vec2D(vert.X, vert.Y));
                    }
                    obstructions[id].sensorsBlocked = ((StringListValue)((AttributeCollectionValue)e["Attributes"])["BlocksSensorTypes"]).strings;
                    break;
                case "LandRegion":
                case "ScoringRegion":
                    //do nothing with these regions
                    break;
                default:
                    //If the object type is not one of the above types, then it is a playable object.
                    listOfObjectIDs.Add(id);
                    AttributeCollectionValue atts;
                    atts = e["Attributes"] as AttributeCollectionValue;
                    string ownerID = string.Empty;
                    try
                    {
                        ownerID = ((StringValue)atts["OwnerID"]).value;
                    }
                    catch
                    {
                        throw new Exception(String.Format("Error in ViewPro: Tried to retrieve 'OwnerID' from object ({0}), but OwnerID was missing from NewObject attributes.", id));
                    }
                    if (!dmOwnedObjects.ContainsKey(ownerID))
                    {
                        dmOwnedObjects.Add(ownerID, new List<string>());
                    }
                    foreach (string sensorNet in networkRosters.Keys)
                    {
                        if (networkRosters[sensorNet].Contains(ownerID))
                        {
                            networkObjects[sensorNet].Add(id);
                        }
                    }
                    dmOwnedObjects[ownerID].Add(id);
                    break;
            }
        }
  
        /// <summary>
        /// When an object is revealed, it is not necessarily revealed to the playfield yet.  Docked objects receive
        /// a RevealObject event, but are given a location whose .exists value is false.
        /// When an object is revealed, and is visible, then a shallow sensing algorithm should be called to
        /// see which objects can sense the new object.  Each object that can view the object determines which
        /// sensor network can see the object, and each sensor network which sees the object tells all of its 
        /// members about this object by sending a ViewProInitializeObject event.
        /// </summary>
        /// <param name="e">Incoming Event</param>
        private void RevealObject(SimulationEvent e)
        {
            string id = ((StringValue)e["ObjectID"]).value;
            AttributeCollectionValue atts = e["Attributes"] as AttributeCollectionValue;
            LocationValue location;
            try
            {
                location = atts["Location"] as LocationValue;
            }
            catch
            {
                throw new Exception(String.Format("Error in ViewPro: Missing Location value for object ({0}).", id));
            }
            if (location.exists == false)
            {//this object is not initially visible.  True for weapons and subplatforms. 
                return;
            }
            // Store its initial tag value -- for each team
              if (null == unitTags)
                        unitTags = new Dictionary<string, Dictionary<string, string>>();
                    // there will be no field for id since we only get to this point once per id
              unitTags[id] = new Dictionary<string, string>();
              if (atts.attributes.ContainsKey("InitialTag"))
              {
                  DataValue initialTagDV = atts["InitialTag"];
                  string initialTag = ((StringValue)initialTagDV).value;
                  foreach (string teamID in teamDefinitions.Keys)
                      unitTags[id][teamID] = initialTag;
                  if (recentUnitTags == null)
                      recentUnitTags = new Dictionary<string, Dictionary<string, string>>();
                  foreach (string dmID in dmTeamsMap.Keys)
                  {
                      if (teamDefinitions.ContainsKey(dmTeamsMap[dmID]))
                      {
                          if(!recentUnitTags.ContainsKey(id))
                              recentUnitTags.Add(id, new Dictionary<string,string>());
                          if(!recentUnitTags[id].ContainsKey(dmID))
                              recentUnitTags[id].Add(dmID,"");
                          recentUnitTags[id][dmID] = initialTag;
                      }
                  }
              }
                    

            //Just send out an initializing event to its owner.  the rest of the objects will be notified during the
            //next time tick.
            string errorAt = "OwnerID";
            string ownerID;
            string iconName;
            bool isWeapon = false;
            SensorArrayValue sav;

            try
            {
                ownerID = ((StringValue)objectProxies[id]["OwnerID"].GetDataValue()).value;// ((StringValue)atts["OwnerID"]).value;
                errorAt = "IconName";
                iconName = ((StringValue)atts["IconName"]).value;
                errorAt = "IsWeapon";
                isWeapon = ((BooleanValue)atts["IsWeapon"]).value;
                //This snippet is to determine whether or not each player has an omniscient view of the playfield.
                //if each object has no sensors, then Omniscience should remain true.  This value is checked when
                //sensors are being calculated, and if its true, then the object gets all of the target object's
                //attributes.
                errorAt = "Sensors";
                sav = atts["Sensors"] as SensorArrayValue;
                if (sav.sensors.Count > 0)
                {
                    Omniscience = false;
                }

            }
            catch
            {
                throw new Exception(String.Format("Error in ViewPro: Missing {0} value for object ({1}).", errorAt, id));
            }

            //dmViews[ownerID].UpdateObjectsAttributes(ref atts, id);
            SendViewProInitializeObject(ownerID, id, location, iconName, ownerID, isWeapon);
            //At the next time tick, each object in the ownerID's sensor network(s) will become aware of this object,
            //and will receive a VPIO event then.  For now, only the owner of this object is aware of the launch.

        }
  
        /// <summary>
        /// With each time tick, the sensory algorithm will be called.  If any attributes on objects
        /// change, those updates are sent to the DMs that were viewing the object.  If a sensor network
        /// gets a view containing a previously un-viewed object, then it sends out to each of its
        /// members a ViewProInitializeObject event.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void TimeTick(SimulationEvent e)
        {
            if (currentTick == 0)
            { //first tick
                //    AssignUnassignedDMs();
            }
            //objectProxies = bbClient.GetObjectProxies();
            currentTick = ((IntegerValue)e["Time"]).value;

            if (currentTick % 1000 != 0)
            {
                return;
            }
            SendSimulationTimeEvent(e);
            CalculateSensoryAlgorithm();

            /* 
             * For this attack algorithm, I go through the list of attacks sending out attack
             * updates for each attack in the list.  The method "SendAttackEvent" returns false
             * if the time of the attack is up.  If this is true, then that attack's index
             * is added to a list of ints.  The int list is reversed so that the attacks are
             * removed from the end first, rather than from the start.  This is to ensure that
             * by removing an attack at the start of the list, that the attacks that follow after 
             * that one in the list have the same index that I capture in the previous loop.
             */
            int counter = 0;
            List<int> attacksToRemove = new List<int>();
            foreach (Attack at in currentAttacks)
            {
                if (SendAttackEvent(at) == false)
                {
                    attacksToRemove.Add(counter);
                }
                counter++;
            }
            attacksToRemove.Reverse();
            foreach (int i in attacksToRemove)
            {
                currentAttacks.RemoveAt(i);
            }

            if (recentUnitTags == null)
            {
                recentUnitTags = new Dictionary<string, Dictionary<string, string>>();
            }
            else
            {
               // recentUnitTags.Clear(); //recent tags are kept on a tick-to-tick basis.
                //AD: We're clearing out DM's after view pro's sent
            }
        }
 
        /// <summary>
        /// When a move object is received, a view pro motion update must be sent out.  Add the object
        /// to the MovingObjects list.  A single ViewProMotionUpdate event is then sent out, and
        /// each client will process this only if they contain this object in their view already.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void MoveObject(SimulationEvent e)
        {
            string id = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy obj = objectProxies[id];
            string ownerID = ((StringValue)obj["OwnerID"].GetDataValue()).value;
            LocationValue initLocation = obj["Location"].GetDataValue() as LocationValue;
            LocationValue destLocation = e["DestinationLocation"] as LocationValue;
            double maxSpeed = ((DoubleValue)obj["MaximumSpeed"].GetDataValue()).value;
            double throttle = ((DoubleValue)e["Throttle"]).value;
            string iconName = ((StringValue)obj["IconName"].GetDataValue()).value;
            bool isWeapon = ((BooleanValue)obj["IsWeapon"].GetDataValue()).value;
            double activeRegionSpeedMultiplier = ((DoubleValue)obj["ActiveRegionSpeedMultiplier"].GetDataValue()).value;

            SendViewProMotionUpdate(id, ownerID, initLocation, destLocation, maxSpeed, throttle, iconName, isWeapon, activeRegionSpeedMultiplier);
        }
 
        /// <summary>
        /// When a move done event is received, then a normalizing ViewProMotionUpdate is sent out,
        /// and that object is removed from the MovingObjects list.  The VPMU that is sent out gives
        /// a destination location that is equal to its current location, and a throttle of 0.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void MoveDone(SimulationEvent e)
        {
            string id = ((StringValue)e["ObjectID"]).value;
            double throttle = ((DoubleValue)objectProxies[id]["Throttle"].GetDataValue() as DoubleValue).value;
            //LocationValue location = objectProxies[id]["DestinationLocation"].GetDataValue() as LocationValue;
            LocationValue location = objectProxies[id]["Location"].GetDataValue() as LocationValue;

            AttributeCollectionValue newACV = new AttributeCollectionValue();
            newACV.attributes.Add("ObjectID", DataValueFactory.BuildString(id));
            newACV.attributes.Add("Throttle", DataValueFactory.BuildDouble(throttle));
            newACV.attributes.Add("Time", DataValueFactory.BuildInteger(currentTick));
            newACV.attributes.Add("Location", location);
            newACV.attributes.Add("DestinationLocation", location);

            if (movingObjects.Contains(id))
            {
                movingObjects.Remove(id);
            }
            SendViewProStopObjectUpdate(id);
            SendViewProMotionUpdate(newACV);
        }
 
        /// <summary>
        /// When a weapon is launched, a ViewProInitializeObject event must be first sent out to its 
        /// owner, and then a ViewProMotionUpdate must be sent out.  A weapon launch event only contains
        /// an ObjectID (of weapon), a ParentObjectID (the object launching the weapon), and a
        /// TargetObjectID (object being attacked by weapon.  We'll need to extract the target's
        /// location, the ParentObjectID's location for the initial Location, and the weapon's
        /// MaximumSpeed and icon for the VPMU event.  IsWeapon is set to true, and throttle is 1.
        /// Other objects will become aware of this weapon on the next time tick when the sensing
        /// algorithm occurs.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void WeaponLaunch(SimulationEvent e)
        {
            string weaponID = ((StringValue)e["ObjectID"]).value;
            string parentID = ((StringValue)e["ParentObjectID"]).value;
            string targetID = ((StringValue)e["TargetObjectID"]).value;
            string errorAt = "OwnerID";
            string errorAtOwner = weaponID;
            string ownerID;
            string iconName;
            LocationValue initialLocation;
            LocationValue destinationLocation;
            double throttle = 1;
            double maxSpeed = 0.0;
            bool isWeapon = true;
            AttributeCollectionValue newACV = new AttributeCollectionValue();
            newACV.attributes.Add("ObjectID", DataValueFactory.BuildString(weaponID));
            newACV.attributes.Add("Throttle", DataValueFactory.BuildDouble(throttle));
            newACV.attributes.Add("IsWeapon", DataValueFactory.BuildBoolean(isWeapon));
            SimulationObjectProxy obj;

            try
            {
                errorAtOwner = weaponID;
                obj = objectProxies[weaponID];
                errorAt = "OwnerID";
                ownerID = ((StringValue)obj["OwnerID"].GetDataValue()).value;
                newACV.attributes.Add("OwnerID", DataValueFactory.BuildString(ownerID));

                errorAt = "IconName";
                iconName = ((StringValue)obj["IconName"].GetDataValue()).value;
                newACV.attributes.Add("IconName", DataValueFactory.BuildString(iconName));

                errorAt = "MaximumSpeed";
                maxSpeed = ((DoubleValue)obj["MaximumSpeed"].GetDataValue()).value;
                newACV.attributes.Add("MaximumSpeed", DataValueFactory.BuildDouble(maxSpeed));

                errorAtOwner = parentID;
                obj = objectProxies[parentID];
                errorAt = "Location";
                initialLocation = obj["Location"].GetDataValue() as LocationValue;
                newACV.attributes.Add("Location", initialLocation as DataValue);

                errorAtOwner = targetID;
                obj = objectProxies[targetID];
                errorAt = "DestinationLocation";
                destinationLocation = obj["Location"].GetDataValue() as LocationValue;
                newACV.attributes.Add("DestinationLocation", destinationLocation as DataValue);
            }
            catch
            {
                throw new Exception(String.Format("Error in ViewPro: Error retrieving {0} attribute from the object {1}.", errorAt, errorAtOwner));
            }
            SendViewProInitializeObject(ownerID, weaponID, initialLocation, iconName, ownerID, true);
            SendViewProMotionUpdate(newACV);
        }
 
        /// <summary>
        /// When a subplatform is launched, a ViewProInitializeObject event must be first sent out to
        /// its owner.  If we decide to be able to deploy an object to a specific location, then a
        /// VPMU must be sent out afterwards.  A subplatform launch event only contains the ParentObjectID,
        /// the ObjectID, and the RelativeLocation.  You'll need to get the ownerID, and icon name of the 
        /// subplatform, and the location of the launching object (to add the relative location to) for
        /// the VPIO event.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void SubplatformLaunch(SimulationEvent e)
        {
            string parentID = ((StringValue)e["ParentObjectID"]).value;
            string objectID = ((StringValue)e["ObjectID"]).value;
            string ownerID;
            string iconName;
            //LocationValue relativeLocation = e["RelativeLocation"] as LocationValue;
            LocationValue parentLocation;
            SimulationObjectProxy obProx = objectProxies[objectID];
            ownerID = ((StringValue)obProx["OwnerID"].GetDataValue()).value;
            iconName = ((StringValue)obProx["IconName"].GetDataValue()).value;
            obProx = objectProxies[parentID];
            parentLocation = obProx["Location"].GetDataValue() as LocationValue;
            //relativeLocation.X += parentLocation.X;
            //relativeLocation.Y += parentLocation.Y;
            //relativeLocation.Z += parentLocation.Z;

            //SimulationEvent initEvent = SimulationEventFactory.BuildEvent(ref simModel, "ViewProInitializeObject");
            //initEvent["TargetPlayer"] = DataValueFactory.BuildString(ownerID);
            //initEvent["ObjectID"] = DataValueFactory.BuildString(objectID);
            //initEvent["Location"] = relativeLocation;
            //initEvent["IconName"] = DataValueFactory.BuildString(iconName);
            //initEvent["OwnerID"] = DataValueFactory.BuildString(ownerID);
            //initEvent["Time"] = DataValueFactory.BuildInteger(currentTick);

            //distClient.PutEvent(initEvent);
            ////
            if (!dmOwnedObjects[ownerID].Contains(objectID))
            {
                dmOwnedObjects[ownerID].Add(objectID);
            }
            foreach (string sensorNet in networkRosters.Keys)
            {
                if (networkRosters[sensorNet].Contains(ownerID))
                {
                    if (!networkObjects[sensorNet].Contains(objectID))
                    {
                        networkObjects[sensorNet].Add(objectID);
                    }
                }
            }
            CalculateSensoryAlgorithm();
            ////
            SendViewProInitializeObject(ownerID, objectID, parentLocation, iconName, ownerID, false);
        }
 
        /// <summary>
        /// When a subplatform is docking, a ClientRemoveObject event must be broadcast to remove
        /// the object from all clients' playfields.  The object should also be removed from
        /// MovingObjects and CurrentAttacks, and go through each dm and sensor network view to
        /// remove the object.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void SubplatformDock(SimulationEvent e)
        {
            //SimulationEvent removeEvent = SimulationEventFactory.BuildEvent(ref simModel, "ClientRemoveObject");
            //removeEvent["Time"] = DataValueFactory.BuildInteger(currentTick);
            string objectID = ((StringValue)e["ObjectID"]).value;
            //removeEvent["ObjectID"] = e["ObjectID"];
            foreach (string dm in activeDMs)
            {
                //removeEvent["TargetPlayer"] = DataValueFactory.BuildString(dm);
                //distClient.PutEvent(removeEvent);
                SendRemoveObjectEvent(objectID, dm);
            }
            if (movingObjects.Contains(objectID))
            {
                movingObjects.Remove(objectID);
            }

            /* 
             * For this attack algorithm, I go through the list of attacks sending out attack
             * updates for each attack in the list.  The method "SendAttackEvent" returns false
             * if the time of the attack is up.  If this is true, then that attack's index
             * is added to a list of ints.  The int list is reversed so that the attacks are
             * removed from the end first, rather than from the start.  This is to ensure that
             * by removing an attack at the start of the list, that the attacks that follow after 
             * that one in the list have the same index that I capture in the previous loop.
             */
            int counter = 0;
            List<int> attacksToRemove = new List<int>();
            foreach (Attack at in currentAttacks)
            {
                if (at.attacker == objectID ||
                    at.target == objectID)
                {
                    attacksToRemove.Add(counter);
                }
                counter++;
            }
            attacksToRemove.Reverse();
            foreach (int i in attacksToRemove)
            {
                currentAttacks.RemoveAt(i);
            }

            ClearObjectFromViews(objectID);
            /*
            foreach (string dm in dmViews.Keys)
            {
                dmViews[dm].RemoveObject(objectID);
            }
             */
            //not sure that this is needed.
            //if (allObjectViews.ContainsKey(objectID))
            //{
            //    allObjectViews.Remove(objectID);
            //}
        }
 
        /// <summary>
        /// Add the given object to the CurrentAttacks list.  At time ticks, ViewProAttackUpdates
        /// are sent out to clients using the attacks on the attack list.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void AttackObject(SimulationEvent e)
        {
            string attackingID = ((StringValue)e["ObjectID"]).value;
            string targetID = ((StringValue)e["TargetObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;
            //int timeStart = ((IntegerValue)e["Time"]).value;
            SimulationObjectProxy obj = objectProxies[attackingID]; 
            AttackCollectionValue attCV = (AttackCollectionValue)obj["CurrentAttacks"].GetDataValue();
            AttackCollectionValue.AttackValue thisAttack = null;
            foreach (AttackCollectionValue.AttackValue av in attCV.GetCurrentAttacks())
            {
                if (av.attackingObjectId == attackingID && av.capabilityName == capabilityName && av.targetObjectId == targetID)
                { //ignore time for now.
                    thisAttack = av;
                }
            }
            if (thisAttack != null)
            {
                int timeStart = thisAttack.attackStartTime;
                int duration = thisAttack.attackTimeWindow;
                Attack theAttack = new Attack(attackingID, targetID, thisAttack.capabilityName);

                theAttack.SetTimes(timeStart, duration);
                currentAttacks.Add(theAttack);
            }
            else
            { 
                //this is bad.
            }
        }
 
        /// <summary>
        /// When this is received, all the maintained data structures need to be reset.  Each object
        /// needs to have a ClientRemoveObject sent out for it to each currently attached client.
        /// </summary>
        private void ResetSimulation()
        {
            foreach (string dm in activeDMs)
            //only dms which are active in the simulation need this event.
            {
                foreach (string objID in listOfObjectIDs)
                //This is a safe way.  The efficient way would be to go through
                //dmViews and clear each object that a dm has in that view, but
                //there might be some mismatching data, so this is safer.
                {
                    SendRemoveObjectEvent(objID, dm);
                }
            }

            //clear out data structures:
            this.dmColorMapping = new Dictionary<string, int>();
            this.teamDefinitions = new Dictionary<string, List<string>>();
            this.networkRosters = new Dictionary<string, List<string>>();
            this.dmTeamsMap = new Dictionary<string, string>();
            this.obstructions = new Dictionary<string, StateDB.ActiveRegion>();
            this.networkObjects = new Dictionary<string, List<string>>();
            this.listOfObstructionIDs = new List<string>();
            this.listOfObjectIDs = new List<string>();
            this.dmOwnedObjects = new Dictionary<string, List<string>>();
            objectViews = new Dictionary<string, ObjectsAttributeCollection>();
            dmViews = new Dictionary<string, ObjectsAttributeCollection>();
            //singletonDMs = new List<string>();
            //this.activeDMs = new List<string>();
            //activeSensorNetworks = new Dictionary<string, bool>();
            this.currentAttacks = new List<Attack>();
            ClassificationsEnum = new List<String>();
            this.movingObjects = new List<string>();
            ZInverse.InitializeInvZTable();
            currentTick = 0;
            recentUnitTags = new Dictionary<string, Dictionary<string, string>>();
            unitTags = new Dictionary<string, Dictionary<string, string>>();
        }

        private void InitializeClassifications(SimulationEvent ev)
        { 
            this.ClassificationsEnum = ((StringListValue)ev["Classifications"]).strings;
        }
 
        /// <summary>
        /// This event is sent from the client to server once a client is ready to participate
        /// in the simulation.  Once this event is received, that DM is added to the activeDMs list.
        /// This list acts as a filter; only DMs on this list will have any view pro events sent to them.
        /// This is to limited the amount of traffic being sent out if only a few DMs are attached
        /// to the system.
        /// This is also used when currentTick is > 0 to send out that client's view.  If currentTick
        /// is above 0 and this event is received, then this client will need to get ViewProInitializeObject
        /// events for each object it has in its view, and then it will need motion updates for those
        /// objects in its view that are also in the movement list.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void HandshakeInitializeGUIDone(SimulationEvent e)
        {
            string dm = ((StringValue)e["PlayerID"]).value;

            if (!activeDMs.Contains(dm))
            {
                activeDMs.Add(dm);
            }
            foreach (KeyValuePair<string, List<string>> nets in networkRosters)
            {
                if (nets.Value.Contains(dm))
                {
                    activeSensorNetworks[nets.Key] = true;
                }
            }

            if (currentTick > 0)
            {
                if (initialReplaySpeed != null)
                {
                    distClient.PutEvent(initialReplaySpeed);
                }
                else
                {
                    SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "GameSpeed");
                    ev["SpeedFactor"] = DataValueFactory.BuildDouble(1); //default speed of 1
                    ev["Time"] = DataValueFactory.BuildInteger(currentTick);
                    distClient.PutEvent(ev);
                }
            }

            // send classifications enum to user
            SimulationEvent classifications = SimulationEventFactory.BuildEvent(ref simModel, "InitializeClassifications");
            classifications["Classifications"] = DataValueFactory.BuildStringList(ClassificationsEnum);
            classifications["Time"] = DataValueFactory.BuildInteger(currentTick);
            distClient.PutEvent(classifications);

            //SimulationEvent initEvent = SimulationEventFactory.BuildEvent(ref simModel, "ViewProInitializeObject");
            //initEvent["Time"] = DataValueFactory.BuildInteger(currentTick);
            List<string> visibleObjects = new List<string>();
            if (currentTick > 0)
            //The simulation has already begun, need to setup DM its view
            {
                if (!dmOwnedObjects.ContainsKey(dm))
                    dmOwnedObjects.Add(dm, new List<string>());

                //foreach (string id in dmOwnedObjects[dm])
                //{
                //    if (objectViews.ContainsKey(id))
                //    {
                //        objectViews[id] = new ObjectsAttributeCollection();
                //    }
                //}

                //If time > 0, call a method that takes each object in this DMs sensor networks,
                //and compiles their total view, and then send off the appropriate init/att updates.
                if (currentTick > 0)
                {
                    List<string> sensingObjects = new List<string>();

                    foreach (string sn in networkRosters.Keys)
                    {
                        if (networkRosters[sn].Contains(dm))
                        {
                            foreach (string objID in networkObjects[sn])
                            {
                                if (!sensingObjects.Contains(objID))
                                {
                                    sensingObjects.Add(objID);
                                }
                            }
                        }
                    }

                    Dictionary<string, AttributeCollectionValue> thisDMsView = new Dictionary<string, AttributeCollectionValue>();
                    AttributeCollectionValue ACVptr;
                    foreach (string obj in sensingObjects)
                    {
                        if (!objectViews.ContainsKey(obj))
                            continue;

                        foreach (string targetObj in objectViews[obj].GetObjectKeys())
                        {
                            if (!thisDMsView.ContainsKey(targetObj))
                            {
                                thisDMsView.Add(targetObj, new AttributeCollectionValue());
                            }
                            ACVptr = thisDMsView[targetObj];
                            MergeTwoAttributeCollections(ref ACVptr, objectViews[obj].GetObjectsAttributeCollection(targetObj));
                        }
                    }
                    dmViews[dm] = new ObjectsAttributeCollection();

                    foreach (string o in thisDMsView.Keys)
                    {
                        dmViews[dm].UpdateObject(o, thisDMsView[o]);
                        AttributeCollectionValue tempACV = ExtractDetectedValuesFromACV(thisDMsView[o]);
                        SendViewProInitializeObject(dm, tempACV);
                        //add tag info to tempACV
                        if (unitTags.ContainsKey(o))
                        {
                            if (unitTags[o].ContainsKey(dmTeamsMap[dm]))
                            {
                                tempACV.attributes.Add("InitialTag", DataValueFactory.BuildString(unitTags[o][dmTeamsMap[dm]]));
                            }
                        }
                        SendViewProAttributeUpdate(dm, tempACV);
                    }
                }
            }
            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
                SendViewProActiveRegionUpdate(reg.id, reg.isVisible, reg.displayColor, reg.currentAbsolutePolygon);
            }
        }

        private void SendViewProAttributeForAllObjects(String viewAccordingToThisDm, List<String> sensingObjects)
        {
            Dictionary<string, AttributeCollectionValue> thisDMsView = new Dictionary<string, AttributeCollectionValue>();
            AttributeCollectionValue ACVptr;
            foreach (string obj in sensingObjects)
            {
                if (!objectViews.ContainsKey(obj))
                    continue;

                foreach (string targetObj in objectViews[obj].GetObjectKeys())
                {
                    if (!thisDMsView.ContainsKey(targetObj))
                    {
                        thisDMsView.Add(targetObj, new AttributeCollectionValue());
                    }
                    ACVptr = thisDMsView[targetObj];
                    MergeTwoAttributeCollections(ref ACVptr, objectViews[obj].GetObjectsAttributeCollection(targetObj));
                }
            }
            dmViews[viewAccordingToThisDm] = new ObjectsAttributeCollection();

            foreach (string o in thisDMsView.Keys)
            {
                dmViews[viewAccordingToThisDm].UpdateObject(o, thisDMsView[o]);
                AttributeCollectionValue tempACV = ExtractDetectedValuesFromACV(thisDMsView[o]);
                SendViewProInitializeObject(viewAccordingToThisDm, tempACV);
                //add tag info to tempACV
                if (unitTags.ContainsKey(o))
                {
                    if (unitTags[o].ContainsKey(dmTeamsMap[viewAccordingToThisDm]))
                    {
                        tempACV.attributes.Add("InitialTag", DataValueFactory.BuildString(unitTags[o][dmTeamsMap[viewAccordingToThisDm]]));
                    }
                }
                SendViewProAttributeUpdate(viewAccordingToThisDm, tempACV);
            }
        }
 
        /// <summary>
        /// This event is sent out from the client when it is disconnecting from the server.  When
        /// this happens, ViewPro should remove the DM from the activeDMs list so that DM no longer
        /// receives view pro events.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void DisconnectDecisionMaker(SimulationEvent e)
        {
            string dm = ((StringValue)e["DecisionMakerID"]).value;
            if (activeDMs.Contains(dm))
            {
                activeDMs.Remove(dm);
            }
            foreach (KeyValuePair<string, List<string>> nets in networkRosters)
            {
                activeSensorNetworks[nets.Key] = false;
                foreach (string s in nets.Value)
                {
                    if (activeDMs.Contains(s))
                    {
                        activeSensorNetworks[nets.Key] = true;
                    }
                }
            }
        }

        /// <summary>
        /// When a pause event is received, foreach object in the movement list, a ViewProMotionUpdate
        /// must be sent out with a throttle of 0 (all the rest of the values can remain the same from
        /// an object proxy).
        /// </summary>
        private void PauseScenario()
        {
            AttributeCollectionValue tempACV = new AttributeCollectionValue();
            tempACV.attributes.Add("Throttle", DataValueFactory.BuildDouble(0));
            tempACV.attributes.Add("ObjectID", DataValueFactory.BuildString(string.Empty));

            foreach (string objectID in movingObjects)
            {
                tempACV["ObjectID"] = DataValueFactory.BuildString(objectID);
                SendViewProMotionUpdate(tempACV);
            }

        }

        /// <summary>
        /// This is used as a normalizer, in which every client's view is reset to the server's view
        /// for that client.  Foreach object in each player's view, send out a ViewProInitializeObject
        /// event, and then foreach object on the motion list, send out motion updates.
        /// </summary>
        private void ResumeScenario()
        {
            if (initialReplaySpeed != null)
            {
                distClient.PutEvent(initialReplaySpeed);
            }
            else
            {
                SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "GameSpeed");
                ev["SpeedFactor"] = DataValueFactory.BuildDouble(1); //default speed of 1
                ev["Time"] = DataValueFactory.BuildInteger(currentTick);
                distClient.PutEvent(ev);
            }
            //initialize views:
            foreach (string dm in dmViews.Keys)
            {
                foreach (string objID in dmViews[dm].GetObjectKeys())
                {
                    if (!dmViews[dm][objID].attributes.ContainsKey("Location"))
                    {//if a dm doesn't know an object's location, then that object 
                        //is not visible to the dm.
                        continue;
                    }
                    AttributeCollectionValue acv = new AttributeCollectionValue();
                    acv["ID"] = DataValueFactory.BuildString(objID);
                    SendViewProInitializeObject(dm, acv);

                }
            }

            //send motion updates:
            AttributeCollectionValue tempACV = new AttributeCollectionValue();
            tempACV.attributes.Add("ObjectID", DataValueFactory.BuildString(string.Empty));

            foreach (string objectID in movingObjects)
            {
                tempACV["ObjectID"] = DataValueFactory.BuildString(objectID);
                //SendViewProStopObjectUpdate(objectID);
                SendViewProMotionUpdate(tempACV);
            }

            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
               
                SendViewProActiveRegionUpdate(reg.id, reg.isVisible, reg.displayColor, reg.currentAbsolutePolygon);
            }
        }
  
        /// <summary>
        /// When an object's state changes, nothing really needs to be done, unless that state change 
        /// puts the object into a "Dead" state.  When a state change event is received that says
        /// that an object is moving to a "Dead" state, then you should go through each object and
        /// sensor network view, and if its "RemoveOnDestruction" flag is set to true, send out a 
        /// "ClientRemoveObject" event to that client.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void StateChange(SimulationEvent e)
        {
            string objectID = ((StringValue)e["ObjectID"]).value;
            string newState = ((StringValue)e["NewState"]).value;
            SimulationObjectProxy obj = objectProxies[objectID];

            if (!((BooleanValue)obj["RemoveOnDestruction"].GetDataValue()).value ||
                !(newState == "Dead"))
            {//if the object should not be removed on destruction,
                //OR if the event is not moving an object to a dead state.
                //the return without doing the following process.
                return;
            }

            ClearObjectFromViews(objectID);
            //Need to remove from object id list now, but shouldn't later on
            listOfObjectIDs.Remove(objectID);
            //
        }
 
        /// <summary>
        /// *Intercepted Event From Client*  When this event is received, you'll need to extract
        /// the subplatform class from the ObjectID field, and use this class along with the 
        /// ParentObjectID to determine which object is actually being launched.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void ClientSubplatformLaunchRequest(SimulationEvent e)
        {
            //coming soon.
        }
 
        /// <summary>
        /// This method is called when a client sends out a "ClientAttackRequest" event.
        /// This method takes in weapon or capability name, determines whether it is
        /// a weapon or a capability, and then sends the appropriate attack message to
        /// the scenario controller.
        /// </summary>
        /// <param name="e"></param>
        private void ProcessAttack(SimulationEvent e)
        {
            string playerID = string.Empty;
            string attObjectID = string.Empty;
            string tarObjectID = string.Empty;
            string weaponName = string.Empty;
            int time = ((IntegerValue)e["Time"]).value;
            SimulationObjectProxy attackingObjectProxy;
            SimulationEvent sendingEvent;

            playerID = ((StringValue)e["PlayerID"]).value;
            attObjectID = ((StringValue)e["AttackingObjectID"]).value;
            tarObjectID = ((StringValue)e["TargetObjectID"]).value;
            weaponName = ((StringValue)e["WeaponOrCapabilityName"]).value;

            //// Added now that weapons are sent out with a quantity

            if (weaponName.Contains("x)") && weaponName.Contains("("))
            {//Most likely a weapon with a quantity
                char[] ch = { '(' };
                string[] splits = weaponName.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                weaponName = splits[0].Trim();
            }

            ////

            if (objectProxies.ContainsKey(attObjectID))
            {
                attackingObjectProxy = objectProxies[attObjectID];

                List<string> dockedWeapons = ((StringListValue)attackingObjectProxy["DockedWeapons"].GetDataValue()).strings;
                SimulationObjectProxy weaponProxy;
                foreach (string w in dockedWeapons)
                {
                    weaponProxy = objectProxies[w];
                    if (((StringValue)weaponProxy["ClassName"].GetDataValue()).value == weaponName)
                    {
                        weaponName = w;
                    }
                }

                if (dockedWeapons.Contains(weaponName))
                { //send weapon attack
                    sendingEvent = SimulationEventFactory.BuildEvent(ref simModel, "WeaponLaunchRequest");

                    sendingEvent["UserID"] = DataValueFactory.BuildString(playerID);
                    sendingEvent["ParentObjectID"] = DataValueFactory.BuildString(attObjectID);
                    sendingEvent["TargetObjectID"] = DataValueFactory.BuildString(tarObjectID);
                    sendingEvent["ObjectID"] = DataValueFactory.BuildString(weaponName);
                    sendingEvent["Time"] = DataValueFactory.BuildInteger(time);
                    distClient.PutEvent(sendingEvent);
                }
                else
                {//send capability attack 
                    sendingEvent = SimulationEventFactory.BuildEvent(ref simModel, "AttackObjectRequest");

                    sendingEvent["UserID"] = DataValueFactory.BuildString(playerID);
                    sendingEvent["ObjectID"] = DataValueFactory.BuildString(attObjectID);
                    sendingEvent["TargetObjectID"] = DataValueFactory.BuildString(tarObjectID);
                    sendingEvent["CapabilityName"] = DataValueFactory.BuildString(weaponName);
                    sendingEvent["Time"] = DataValueFactory.BuildInteger(time);
                    distClient.PutEvent(sendingEvent);
                }

            }
        }
  
        private void SendSimulationTimeEvent(SimulationEvent inc)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "SimulationTimeEvent");
            e["SimulationTime"] = inc["SimulationTime"];
            e["Time"] = inc["Time"];
            distClient.PutEvent(e);
        }

        #endregion
    }
}
