using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;


namespace Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// The Event Communicator acts as the adapter piece between
    /// the ScenarioController and the SimulationCore of the DDD.
    /// The EvComm takes the ScenCon internal data structure and
    /// converts the info in it to a format that the SimulationCore
    /// can use.  It also has a method which runs in its own thread
    /// which waits for incoming messages to translate for the ScenCon.
    /// </summary>
    public class EventCommunicator
    {
        private static SimulationEventDistributorClient server = null;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static Dictionary<string, string> scenCon2SimCore = new Dictionary<string, string>();
        private static List<string> simModelIgnoreList = new List<string>();
        private static int latestTick = 0;

        /// <summary>
        /// This method takes an attribute string defined in the ScenCon
        /// and returns the corresponding attribute string defined
        /// in the SimCore.
        /// </summary>
        /// <param name="scenConString">
        /// Attribute String for a ScenCon event
        /// </param>
        /// <returns></returns>
        public static string convertScenConToSimCore(string scenConString)
        {
            string returnValue = scenConString;

            if (scenCon2SimCore.ContainsKey(scenConString))
            {
                returnValue = scenCon2SimCore[scenConString];
            }

            return returnValue;
        }

        /// <summary>
        /// This method takes an attribute string defined in the SimCore
        /// and returns the corresponding attribute string defined
        /// in the ScenCon.
        /// </summary>
        /// <param name="simCoreString">
        /// Attribute String for a SimCore event
        /// </param>
        /// <returns></returns>
        public static string convertSimCoreToScenCon(string simCoreString)
        {
            string returnValue = simCoreString;

            foreach (KeyValuePair<string, string> kvp in scenCon2SimCore)
            {
                if (kvp.Value.ToString() == simCoreString)
                    return kvp.Key.ToString();
            }

            return returnValue;
        }

        private void InitializeScenCon2SimCore()
        {
            //Key is ScenarioController attribute names.
            //Value is SimulationCore attribute names.
            if (scenCon2SimCore.Count == 0)
            {
                //<<[ScenCon], [SimCore]>>
                //scenCon2SimCore.Add("InitialLocation", "Location");
                scenCon2SimCore.Add("Owner", "OwnerID");
                scenCon2SimCore.Add("UnitKind", "ClassName");
                scenCon2SimCore.Add("UnitID", "ID");
                //scenCon2SimCore.Add("MaximumVelocity", "MaximumSpeed");
                scenCon2SimCore.Add("Destination", "DestinationLocation");
                scenCon2SimCore.Add("DmRole", "RoleName");
                scenCon2SimCore.Add("StateList", "StateTable");
                scenCon2SimCore.Add("Icon", "IconName");
                // Added 3/8/7:
                scenCon2SimCore.Add("TimeToAttack", "AttackDuration");
                scenCon2SimCore.Add("InitialFuelLoad", "FuelAmount");

            }
        }

        private void InitializeIgnoreList()
        {
            if (simModelIgnoreList.Count == 0)
            {
                if (simModelInfo == null)
                {
                    simModelIgnoreList.Add("AttackState");
                    //simModelIgnoreList.Add("AttackStartTime");
                    //simModelIgnoreList.Add("AttackTimeWindow");
                    //simModelIgnoreList.Add("AttackTargetID");
                    //simModelIgnoreList.Add("AttackCapability");
                    simModelIgnoreList.Add("AttackerList");
                }
                else
                {
                    foreach (ObjectInfo oi in simModelInfo.objectModel.objects.Values)
                    {
                        foreach (AttributeInfo ai in oi.attributes.Values)
                        {
                            if (ai.excludeFromScenario && !simModelIgnoreList.Contains(ai.name))
                            {
                                simModelIgnoreList.Add(ai.name);
                            }
                        }
                    }
                }
            }
        }

        public EventCommunicator()
        {
            InitializeScenCon2SimCore();
            InitializeIgnoreList();
        }

        /// <summary>
        /// This constructor sets up the simModel and server strings.
        /// Parameters:
        /// NetworkClient: the network client for the program, required
        /// to subscribe to incoming events.
        /// SimModel: string containing the path to the simulation model
        /// which allows the code to create SimCore events for transfer.
        /// </summary>
        public EventCommunicator(ref SimulationEventDistributorClient s, string simModel)
        {
            simModelName = simModel;
            simModelInfo = smr.readModel(simModelName);
            InitializeScenCon2SimCore();
            InitializeIgnoreList();
            server = s;

            AddNetworkClient(server);
        }

        /// <summary>
        /// This method allows either the user or the constructor to
        /// set up an existing network connection.
        /// Parameters:
        /// NetworkClient: The client to connect to.
        /// Subscriptions to incoming events are set here.
        /// </summary>
        public void AddNetworkClient(SimulationEventDistributorClient s)
        {
            server = s;

            server.Subscribe("SimCoreReady");
            server.Subscribe("MoveDone");
            server.Subscribe("StateChange");
            server.Subscribe("MoveObjectRequest");
            server.Subscribe("AttackObjectRequest");
            server.Subscribe("WeaponLaunchRequest");
            server.Subscribe("RequestChatRoomCreate");
            server.Subscribe("RequestCloseChatRoom");
            server.Subscribe("RequestWhiteboardRoomCreate");
            server.Subscribe("SubplatformLaunchRequest");
            server.Subscribe("SubplatformDockRequest");
            server.Subscribe("SelfDefenseAttackStarted");
            server.Subscribe("AttackSucceeded");
            server.Subscribe("ClientSideAssetTransferAllowed");
            server.Subscribe("ChangeTagRequest");
            server.Subscribe("TransferObjectRequest");
            server.Subscribe("DynamicCreate");
            server.Subscribe("DynamicReveal");

            
        }

        /// <summary>
        /// This method loops while the program runs, and every 100
        /// milliseconds (1/10 of a sec) calls the server's "GetEvents"
        /// method.  This retrieves events that the EvComm has 
        /// subscribed to.  At which point, the list is parsed, and the
        /// events are converted back to ScenCon styled events.
        /// </summary>
        public void WaitForEvents()
        {
            //Receives an incoming message from the simcore
            string objID = null;
            string parentID = null;
            string targetObjID = null;
            string dm = null;
            string capability = null;
            List<String> capabilities;
            string newState = null;
            LocationType vect = null;
            string weaponID = null;
            double throttle;
            string eventType = null;
            string roomName=null;
            string channelName = null;
            string requestingDM=null;
            string textString=null;
            List<string> membershipList;
            List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
            bool allowAssetTransferRequests = true;
            try
            {
                while (true)
                {

                    incomingEvents = server.GetEvents();

                    if (incomingEvents.Count == 0)
                    {//do nothing 
                    }
                    else
                    {
                        foreach (SimulationEvent e in incomingEvents)
                        {
                            eventType = e.eventType;
                            switch (eventType)
                            {
                                case "SimCoreReady":

                                    Coordinator.debugLogger.Writeline("EventCommunicator", "Ready To Start Scenario", "test");
                                    Coordinator.ReadyToStart();
                                    break;
                                case "SelfDefenseAttackStarted":
                                    objID = ((StringValue)e["AttackerObjectID"]).value;
                                    targetObjID = ((StringValue)e["TargetObjectID"]).value;
                                    SelfDefenseAttackNotice selfDefenseNotice = new SelfDefenseAttackNotice(objID, targetObjID);
                                    selfDefenseNotice.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(selfDefenseNotice);
                                    Coordinator.debugLogger.Writeline("EventCommunicator", "Self defense attack started", "test");
                                    break;
                                case "SubplatformDockRequest":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    parentID = ((StringValue)e["ParentObjectID"]).value;
                                    dm = ((StringValue)e["UserID"]).value;
                                    SubplatformDockRequestType dockRequest = new SubplatformDockRequestType(dm, objID, parentID);
                                    dockRequest.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(dockRequest);
                                    Coordinator.debugLogger.Writeline("EventCommunicator", "Subplatform dock request received", "test");
                                    break;
                                case "SubplatformLaunchRequest":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    parentID = ((StringValue)e["ParentObjectID"]).value;
                                    dm = ((StringValue)e["UserID"]).value;
                                    vect = new LocationType(((LocationValue)e["LaunchDestinationLocation"]).X,
                                                            ((LocationValue)e["LaunchDestinationLocation"]).Y,
                                                            ((LocationValue)e["LaunchDestinationLocation"]).Z);
                                    SubplatformLaunchRequestType subPlaunchRequest = new SubplatformLaunchRequestType(dm, objID, parentID, vect);
                                    subPlaunchRequest.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(subPlaunchRequest);
                                    Coordinator.debugLogger.Writeline("EventCommunicator", "Subplatform launch request received", "test");
                                    break;
                                case "WeaponLaunchRequest":
                                    objID = ((StringValue)e["ParentObjectID"]).value; //the launching object
                                    weaponID = ((StringValue)e["ObjectID"]).value;// the weapon id being launched
                                    //parentID = ((StringValue)e["ParentObjectID"]).value; 
                                    targetObjID = ((StringValue)e["TargetObjectID"]).value; //The target object
                                    dm = ((StringValue)e["UserID"]).value; //the dm launching the weapon
                                    WeaponLaunchRequestType launchRequest = new WeaponLaunchRequestType(objID, dm, targetObjID, weaponID);
                                    launchRequest.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(launchRequest);
                                    Coordinator.debugLogger.Writeline("EventCommunicator", "Weapon launch request received", "test");
                                    break;
                                case "MoveDone":
                                    objID = ((StringValue)e["ObjectID"]).value; //see line 222 of MotionSim.cs for reference

                                    MoveComplete_Event moveDone = new MoveComplete_Event(objID.ToString());
                                    moveDone.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(moveDone);
                                    break;

                                case "MoveObjectRequest":
                                    dm = String.Empty;
                                    dm = ((StringValue)e["UserID"]).value;
                                    objID = String.Empty;
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    vect = new LocationType();
                                    //X is location in UTM Easting
                                    ((LocationType)vect).X = ((LocationValue)e["DestinationLocation"]).X;
                                    //Y is location in UTM Northing
                                    ((LocationType)vect).Y = ((LocationValue)e["DestinationLocation"]).Y;
                                    //Z is altitude in meters
                                    ((LocationType)vect).Z = ((LocationValue)e["DestinationLocation"]).Z;

                                    throttle = new double();
                                    throttle = ((DoubleValue)e["Throttle"]).value;

                                    MoveObjectRequestType moveObjectRequest = new MoveObjectRequestType(objID, dm, vect, throttle);
                                    moveObjectRequest.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(moveObjectRequest);
                                    break;

                                case "AttackObjectRequest":
                                    dm = String.Empty;
                                    dm = ((StringValue)e["UserID"]).value;
                                    objID = String.Empty;
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    targetObjID = String.Empty;
                                    targetObjID = ((StringValue)e["TargetObjectID"]).value;
                                    capability = ((StringValue)e["CapabilityName"]).value;

                                    AttackObjectRequestType attackObjectRequest = new AttackObjectRequestType(objID, dm, targetObjID, capability);
                                    attackObjectRequest.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(attackObjectRequest);
                                    break;



                                case "TransferObjectRequest":
                                    if (allowAssetTransferRequests)
                                    {
                                        string recipientDM = String.Empty;
                                        recipientDM = ((StringValue)e["RecipientID"]).value;
                                        objID = String.Empty;
                                        objID = ((StringValue)e["ObjectID"]).value;
                                        string requesterID = String.Empty;
                                        requesterID = ((StringValue)e["UserID"]).value;
                                        string currentState = ((StringValue)e["State"]).value;
                                        TransferObjectRequest transferObjectRequest = new TransferObjectRequest(requesterID, recipientDM, objID, currentState);
                                        transferObjectRequest.Time = ((IntegerValue)e["Time"]).value;
                                        IncomingList.Add(transferObjectRequest);
                                    }
                                    else
                                    { 
                                        //client side asset transfers were not allowed
                                        Coordinator.debugLogger.Writeline("EventCommunicator", "TransferObjectRequest denied due to scenario constraints", "test");
                                    }
                                    break;


                                case "StateChange":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    newState = ((StringValue)e["NewState"]).value;
                                    StateChangeNotice changeNotice = new StateChangeNotice(objID, newState);
                                    changeNotice.Time = ((IntegerValue)e["Time"]).value;
                                    IncomingList.Add(changeNotice);
                                    break;
                                    /*
                                case "JoinChatRoom" :           
                                    string  roomToJoin=((StringValue)e["RoomName"]).value;
                                    string targetPlayerID=((StringValue)e["TargetPlayerID"]).value;
                                    if(!UnitFacts.IsDM(targetPlayerID))
                                        throw new ApplicationException("'" + targetPlayerID + "' is not a valid decision maker name. Cannot add to chatroom '" + roomToJoin + "'");
                                    List<string> members = ChatRooms.GetChatMembers(roomToJoin);
                                    Boolean canJoin = true;
                                    DecisionMakerType candidateDM=DecisionMakerType.GetDM(targetPlayerID);
                                    for(int i=0;i<members.Count;i++)
                                        if (!candidateDM.CanChatWith(members[i]))
                                        {
                                            canJoin = false;
                                            break;
                                        }
                                    if (canJoin)
                                    {
                                        ChatRooms.AddChatMember(roomToJoin, targetPlayerID);
                                        //notify chat server
                                        SimulationEvent f = SimulationEventFactory.BuildEvent(ref simModelInfo, "AddToChatRoom");
                                        f["RoomName"] = DataValueFactory.BuildString("roomToJoin");
                                        f["TargetPlayerID"] = DataValueFactory.BuildString("targetPlayerID");
                                        server.PutEvent(f);
                                    }
                                    break ;*/
                                case "RequestCloseChatRoom":
                                    roomName = ((StringValue)e["RoomName"]).value;
                                    Boolean closedChatOK=ChatRooms.DropChatRoom(roomName);
                                    if (closedChatOK)
                                    {
                                        SimulationEvent reportClosedChat = SimulationEventFactory.BuildEvent(ref simModelInfo, "CloseChatRoom");
                                        reportClosedChat["RoomName"] = DataValueFactory.BuildString(roomName);
                                        server.PutEvent(reportClosedChat);
                                    }
                                    break;

                                case "RequestCloseVoiceChannel":
                                    channelName = ((StringValue)e["ChannelName"]).value;
                                    Boolean closedVoiceOK=VoiceChannels.DropVoiceChannel(channelName);
                                    if (closedVoiceOK)
                                    {
                                        SimulationEvent reportClosedVoice = SimulationEventFactory.BuildEvent(ref simModelInfo, "CloseVoiceChannel");
                                        reportClosedVoice["ChannelName"] = DataValueFactory.BuildString(channelName);
                                        server.PutEvent(reportClosedVoice);
                                    }
                                    break;
                                case "RequestChatRoomCreate":
                                    {
                                        // HAndled at once -- so no delay is seen even if server is not active
                                        roomName = ((StringValue)e["RoomName"]).value;
                                        requestingDM = ((StringValue)e["SenderDM_ID"]).value;
                                        membershipList = new List<string>();
                                        List<string> allMembers = ((StringListValue)e["MembershipList"]).strings;

                                        string chatFailureReason = "";
                                        OpenChatRoomType openEvent = null; // needed bacause it appears to compiler that this is not defined on every path
                                        if (!UnitFacts.IsDM(requestingDM))
                                            chatFailureReason = "Illegal DM '" + requestingDM + "' in create chat room request";
                                        else
                                        {
                                            for (int i = 0; i < allMembers.Count; i++)
                                                membershipList.Add(allMembers[i]);

                                            for (int i = 0; i < membershipList.Count; i++)
                                            {
                                                DecisionMakerType DMi = DecisionMakerType.GetDM(membershipList[i]);
                                                for (int m = 1 + i; m < membershipList.Count; m++)
                                                {
                                                    if (!DMi.CanChatWith(membershipList[m]))
                                                    {
                                                        chatFailureReason = "Decison Makers '" + DMi.Identifier + "' cannot be in a chat room with '" + membershipList[m] + "'";
                                                        break;
                                                    }
                                                    if ("" != chatFailureReason)
                                                        break;
                                                }

                                            }

                                            if ("" == chatFailureReason)
                                            {

                                                // Now validate and queue response

                                                openEvent = new OpenChatRoomType(1 + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/), requestingDM, roomName, membershipList);
                                                chatFailureReason = ChatRooms.AddRoom(openEvent);
                                            }
                                            if ("" != chatFailureReason)
                                            {
                                                EventCommunicator.SendEvent(new SystemMessage(
                                                    1 + (int)(TimerTicker.Timer / 1000), requestingDM, chatFailureReason));
                                            }
                                            else
                                            {
                                                EventCommunicator.SendEvent(openEvent);
                                            }
                                        }
                                    }
                                    break;


                                case "RequestWhiteboardRoomCreate":
                                    {
                                        // HAndled at once -- so no delay is seen even if server is not active
                                        roomName = ((StringValue)e["RoomName"]).value;
                                        requestingDM = ((StringValue)e["SenderDM_ID"]).value;
                                        membershipList = new List<string>();
                                        List<string> allMembers = ((StringListValue)e["MembershipList"]).strings;

                                        string whiteboardFailureReason = "";
                                        OpenWhiteboardRoomType openEvent = null; // needed bacause it appears to compiler that this is not defined on every path
                                        if (!UnitFacts.IsDM(requestingDM))
                                            whiteboardFailureReason = "Illegal DM '" + requestingDM + "' in create whiteboard room request";
                                        else
                                        {
                                            for (int i = 0; i < allMembers.Count; i++)
                                                membershipList.Add(allMembers[i]);

                                            for (int i = 0; i < membershipList.Count; i++)
                                            {
                                                DecisionMakerType DMi = DecisionMakerType.GetDM(membershipList[i]);
                                                for (int m = 1 + i; m < membershipList.Count; m++)
                                                {
                                                    if (!DMi.CanWhiteboardWith(membershipList[m]))
                                                    {
                                                        whiteboardFailureReason = "Decison Makers '" + DMi.Identifier + "' cannot be in a whiteboard room with '" + membershipList[m] + "'";
                                                        break;
                                                    }
                                                    if ("" != whiteboardFailureReason)
                                                        break;
                                                }

                                            }

                                            if ("" == whiteboardFailureReason)
                                            {

                                                // Now validate and queue response

                                                openEvent = new OpenWhiteboardRoomType(1 + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/), requestingDM, roomName, membershipList);
                                                whiteboardFailureReason = WhiteboardRooms.AddRoom(openEvent);
                                            }
                                            if ("" != whiteboardFailureReason)
                                            {
                                                EventCommunicator.SendEvent(new SystemMessage(
                                                    1 + (int)(TimerTicker.Timer / 1000), requestingDM, whiteboardFailureReason));
                                            }
                                            else
                                            {
                                                EventCommunicator.SendEvent(openEvent);
                                            }
                                        }
                                    }
                                    break;


                                case "AttackSucceeded":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    targetObjID = ((StringValue)e["TargetID"]).value;
                                    capabilities = ((StringListValue)e["Capabilities"]).strings;
                                    newState = ((StringValue)e["NewState"]).value;

                                    IncomingList.Add(new AttackSuccessfulNotice(objID, targetObjID, capabilities, newState));
                                    break;
                                case "ChangeTagRequest":
                                     objID = ((StringValue)e["UnitID"]).value;
                                     requestingDM = ((StringValue)e["DecisionMakerID"]).value;
                                     textString = ((StringValue)e["Tag"]).value;
                                    // This event is handled in place for more rapid turnaround
                                     membershipList = new List<string>();
                                     string thisTeam = UnitFacts.DMTeams[requestingDM];
                                     for (int mbrs = 0; mbrs < UnitFacts.TeamDMs[thisTeam].Count; mbrs++)
                                     {
                                         membershipList.Add(UnitFacts.TeamDMs[thisTeam][mbrs]);
                                     }
                                     EventCommunicator.SendEvent(new UpdateTagType(objID, textString, membershipList));
                                    break;

                                
                                case "ClientSideAssetTransferAllowed":
                                    allowAssetTransferRequests = ((BooleanValue)e["EnableAssetTransfer"]).value;
                                    break;

                                case "DynamicCreate":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    dm = ((StringValue)e["Owner"]).value;
                                    String kind = ((StringValue)e["Kind"]).value;
                                    Aptima.Asim.DDD.ScenarioParser.pCreateType c;
                                    c = new Aptima.Asim.DDD.ScenarioParser.pCreateType(objID, kind, dm);
                                    Create_EventType createEvent = new Create_EventType(c);
                                    Genealogy.Add(createEvent.UnitID, createEvent.UnitBase);
                                    createEvent.Genus = Genealogy.GetGenus(createEvent.UnitID);
                                    UnitFacts.AddUnit(createEvent.UnitID, createEvent.Owner, createEvent.UnitBase);
                                    createEvent.Parameters = new ObjectDictionary();

                                    SpeciesType speciesInfo = (SpeciesType)NameLists.speciesNames[createEvent.UnitBase];

                                    foreach (KeyValuePair<string, StateBody> kvp in StatesForUnits.StateTable[createEvent.UnitBase])
                                    {
                                        string stateName = kvp.Key;
                                        ExtendedStateBody extended = new ExtendedStateBody(kvp.Value);

                                        //end species dependent atts
                                        createEvent.Parameters.Add(stateName, (object)extended);
                                    }

                                    NameLists.unitNames.New(createEvent.UnitID, null);
                                    //TimerQueueClass.SecondarySendBeforeStartup(createEvent);
                                    
                                    TimerQueueClass.Add(1, createEvent);
                                    UnitFacts.CurrentUnitStates.Add(createEvent.UnitID, "");
                                    break;
                                case "DynamicReveal":
                                    objID = ((StringValue)e["ObjectID"]).value;
                                    LocationValue loc = (LocationValue)e["InitialLocation"];
                                    String initialState = ((StringValue)e["InitialState"]).value;
                                    Aptima.Asim.DDD.ScenarioParser.pLocationType pLoc = new Aptima.Asim.DDD.ScenarioParser.pLocationType();
                                    pLoc.X = loc.X;
                                    pLoc.Y = loc.Y;
                                    pLoc.Z = loc.Z;
                                    int revealTime = ((IntegerValue)e["RevealTime"]).value;
                                    Aptima.Asim.DDD.ScenarioParser.pRevealType r;
                                    r = new Aptima.Asim.DDD.ScenarioParser.pRevealType(objID, null, revealTime, pLoc,initialState, new Dictionary<string, object>());
                                    Reveal_EventType revealEvent = new Reveal_EventType(r);
                                    revealEvent.Genus = Genealogy.GetGenus(revealEvent.UnitID);
                                    TimerQueueClass.Add(r.Time, revealEvent);
                                    //if (r.Time > 1)
                                    //{
                                    //    TimerQueueClass.Add(r.Time, revealEvent);
                                    //}
                                    //else
                                    //{
                                    //    TimerQueueClass.SecondarySendBeforeStartup(revealEvent);
                                    //}
                                    //Coordinator.debugLogger.Writeline("ScenarioToQueues", "revealEvent for  " + revealEvent.UnitID + " at time" + r.Time.ToString(), "test");
                                    ScenarioToQueues.RevealDocked(r.UnitID, r.Time);
                                    break;
                                default:
                                    throw new ApplicationException("Received unknown event in EventCommunicator:" + eventType);

                            }
                        }
                        incomingEvents.Clear();
                    }
                    Thread.Sleep(100);
                }
            }
            /*
        catch (ThreadAbortException e)
        {
            //throw new ApplicationException("In WaitForEvents", e);

        }
        */
            finally { }//removes a warning caused bu the null catch. If catch is needed, this won't be
        }

        private static DataValue DefineVulnerabilityValue(Dictionary<string, SingletonVulnerabilityType> singletons,
            List<ComboVulnerabilityType> combo)
        {
            DataValue returnVulnerabilityValue =
                (VulnerabilityValue)DataValueFactory.BuildValue("VulnerabilityType");
            VulnerabilityValue.Transition t;
            VulnerabilityValue.TransitionCondition tc;

            string state;
            int effect;
            double range;
            double probability;

            foreach (ComboVulnerabilityType c in combo)
            {
                state = c.NewState;
                t = new VulnerabilityValue.Transition(state);
                foreach (ContributionType cont in c.Contributions)
                {
                    effect = cont.Effect;
                    string capability = cont.Capability;
                    range = cont.Range;
                    probability = cont.Probability;
                    tc = new VulnerabilityValue.TransitionCondition(capability, effect, range, probability);
                    t.conditions.Add(tc);
                }
                ((VulnerabilityValue)returnVulnerabilityValue).transitions.Add(t);
            }

            foreach (string capability in singletons.Keys)
            {
                singletons[capability].Transitions.Sort();
                foreach (TransitionType transType in singletons[capability].Transitions)
                {
                    effect = transType.Effect;
                    state = transType.State;
                    range = transType.Range;
                    probability = transType.Probability;
                    tc = new VulnerabilityValue.TransitionCondition(capability, effect, range, probability);

                    t = new VulnerabilityValue.Transition(state);
                    t.conditions.Add(tc);
                    ((VulnerabilityValue)returnVulnerabilityValue).transitions.Add(t);
                }
            }


            return returnVulnerabilityValue;
        }
        /*
         * Note: This was not being called. If it is needed later, note that after this revision it
         * is no longer in sycnh with the other 'overload.' best to create a temp variable inside this,
         * convert the input to that variable, and then pass to the other 'overload'
                private static DataValue DefineVulnerabilityValue(ObjectDictionary singletons,
                    ObjectDictionary combo)
                {
                    DataValue returnVulnerabilityValue =
                        (VulnerabilityValue)DataValueFactory.BuildValue("VulnerabilityType");
                    VulnerabilityValue.Transition t;
                    VulnerabilityValue.TransitionCondition tc;

                    string state;
                    int effect;

                    foreach (string capability in singletons.GetKeys())
                    {

                        foreach (TransitionType transType in ((SingletonVulnerabilityType)singletons[capability]).Transitions)
                        {
                            effect = transType.Effect;
                            state = transType.State;
                            tc = new VulnerabilityValue.TransitionCondition(capability, effect);

                            t = new VulnerabilityValue.Transition(state);
                            t.conditions.Add(tc);
                            ((VulnerabilityValue)returnVulnerabilityValue).transitions.Add(t);
                        }
                    }

                    foreach (object c in combo.GetDictionary().Values)
                    {
                        state = ((ComboVulnerabilityType)c).NewState;
                        t = new VulnerabilityValue.Transition(state);
                        foreach (ContributionType cont in ((ComboVulnerabilityType)c).Contributions)
                        {
                            effect = cont.Effect;
                            string capability = cont.Capability;
                            tc = new VulnerabilityValue.TransitionCondition(capability, effect);
                            t.conditions.Add(tc);
                        }
                        ((VulnerabilityValue)returnVulnerabilityValue).transitions.Add(t);
                    }

                    return returnVulnerabilityValue;
                }
        */
        private static DataValue DefineCapabilityValue(Dictionary<string, CapabilityType> capabilityList)
        {
            DataValue returnCapabilityValue = (CapabilityValue)DataValueFactory.BuildValue("CapabilityType");
            CapabilityValue.Effect effect;
            int intensity;
            double probability,
                   range;

            foreach (string name in capabilityList.Keys)
            {
                CapabilityType capability = capabilityList[name];
                capability.ProximityList.Sort();
                foreach (ProximityType prox in capability.ProximityList)
                {
                    range = prox.Range;
                    prox.EffectList.Sort();
                    foreach (EffectType e in prox.EffectList)
                    {
                        intensity = e.Intensity;
                        probability = e.Probability;
                        effect = new CapabilityValue.Effect(name, range, intensity, probability);
                        ((CapabilityValue)returnCapabilityValue).effects.Add(effect);
                    }
                }
            }
            return returnCapabilityValue;
        }
        /*
         *  Note: This was not being called. If it is needed later, note that after this revision it
         * is no longer in sycnh with the other 'overload.' best to create a temp variable inside this,
         * convert the input to that variable, and then pass to the other 'overload'

                private static DataValue DefineCapabilityValue(ObjectDictionary capabilityList)
                {
                    DataValue returnCapabilityValue = (CapabilityValue)DataValueFactory.BuildValue("CapabilityType");
                    //Dictionary<string, CapabilityType> capabilityList = (Dictionary<string, CapabilityType>)incomingDictionary.GetDictionary();
                    CapabilityValue.Effect effect;
                    int intensity;
                    double probability,
                           range;

                    foreach (string name in capabilityList.GetKeys())
                    {
                        CapabilityType capability = (CapabilityType)capabilityList[name];

                        foreach (ProximityType prox in capability.ProximityList)
                        {
                            range = prox.Range;

                            foreach (EffectType e in prox.EffectList)
                            {
                                intensity = e.Intensity;
                                probability = e.Probability;
                                effect = new CapabilityValue.Effect(name, range, intensity, probability);
                                ((CapabilityValue)returnCapabilityValue).effects.Add(effect);
                            }
                        }
                    }
                    return returnCapabilityValue;
                }
 
        */
        private static void SendStartupCompleteEvent()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "StartupComplete");
            server.PutEvent(e);
        }
        /// <summary>
        /// This method takes in a Create_EventType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The Create_EventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendCreateEvent(Create_EventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();
            Dictionary<string, object> DictionaryOfStates;
            DataValue dv;
            ObjectInfo myObject;
            string objectType = incoming.Genus.ToString();
            myObject = simModelInfo.objectModel.objects[objectType];
            dv = new StateTableValue();

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectType"] = DataValueFactory.BuildString(objectType); //ConvertString(objectType);
                e["ID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);

                /*********** Define the State Table ***********************/
                DictionaryOfStates = incoming.Parameters.GetDictionary();
            }
            catch
            {
                throw new Exception("Missing Required Data for Create Event");
            }
            Dictionary<string, DataValue> SimCoreStateTable = new Dictionary<string, DataValue>();

            foreach (KeyValuePair<string, object> kvp in DictionaryOfStates)
            {
                DataValue capabilities,
                          vulnerabilities,
                          currentSimCoreState;

                capabilities = new CapabilityValue();
                vulnerabilities = new VulnerabilityValue();
                currentSimCoreState = new AttributeCollectionValue();
                ExtendedStateBody currentScenConState = new ExtendedStateBody();

                currentScenConState = (ExtendedStateBody)incoming.Parameters[kvp.Key];

                capabilities = DefineCapabilityValue(currentScenConState.Capabilities);
                ((AttributeCollectionValue)currentSimCoreState).attributes.Add("Capability", capabilities);

                vulnerabilities = DefineVulnerabilityValue(currentScenConState.Vulnerabilities, currentScenConState.Combinations);
                ((AttributeCollectionValue)currentSimCoreState).attributes.Add("Vulnerability", vulnerabilities);

                DataValue sensorArray = DataValueFactory.BuildValue("SensorArrayType");
                DataValue sensor;

                Dictionary<string, SensorType> sensors = new Dictionary<string, SensorType>();

                Dictionary<string, List<ConeValue>> ranges;
                sensors = currentScenConState.Sensors;

                foreach (string sensorKind in sensors.Keys)
                {
                    ranges = new Dictionary<string, List<ConeValue>>();
                    sensor = DataValueFactory.BuildValue("SensorType");
                    SensorType typeOfSensor = sensors[sensorKind];
                    string attributeSensed = typeOfSensor.Attribute;
                    List<Cone> cones = typeOfSensor.Cones;
                    double maxRange = 0.0;

                    ((SensorValue)sensor).attIsEngram[attributeSensed] = sensors[sensorKind].IsEngram;
                    List<ConeValue> simCoreCones = new List<ConeValue>();

                    foreach (Cone aCone in sensors[sensorKind].Cones)
                    {
                        LocationValue direction = new LocationValue();
                        direction.X = aCone.Direction.X;
                        direction.Y = aCone.Direction.Y;
                        direction.Z = aCone.Direction.Z;
                        direction.exists = true;
                        ConeValue cv = new ConeValue();
                        cv.direction = direction;
                        cv.extent = aCone.Extent;
                        if (aCone.Extent > maxRange)
                            maxRange = aCone.Extent;
                        cv.level = aCone.Level;
                        cv.spread = aCone.Spread;
                        simCoreCones.Add(cv);
                    }

                    ranges.Add(attributeSensed, simCoreCones);
                    ((SensorValue)sensor).ranges = ranges;
                    ((SensorValue)sensor).sensorName = sensorKind;
                    if (((SensorValue)sensor).maxRange < maxRange)
                    {
                        ((SensorValue)sensor).maxRange = maxRange;
                    }
                    ((SensorArrayValue)sensorArray).sensors.Add((SensorValue)sensor);
                }


                ((AttributeCollectionValue)currentSimCoreState).attributes.Add("Sensors", sensorArray);

                //Emitters
                DataValue emitter = DataValueFactory.BuildValue("EmitterType");

                Dictionary<string, EmitterType> scenConEmitters = currentScenConState.Emitters;
                EmitterType emission;
                foreach (string s in scenConEmitters.Keys)
                {
                    emission = scenConEmitters[s].DeepCopy();
                    string attributeName = s;
                    ((EmitterValue)emitter).attIsEngram[s] = emission.IsEngram;
                    // string level;
                    //        double variance;
                    Dictionary<string, double> levels = new Dictionary<string, double>();
                    foreach (string level in emission.Levels.Keys)
                    {
                        //           levels.Add(level, (double)(emission[level]));
                        double dublet = Double.Parse((emission.Levels[level].ToString()));
                        levels.Add(level, dublet);
                    }
                    ((EmitterValue)emitter).emitters.Add(attributeName, levels);
                }

                ((AttributeCollectionValue)currentSimCoreState).attributes.Add("Emitters", emitter);

                foreach (AttributeInfo attr in myObject.attributes.Values)
                {
                    string attrType = attr.dataType;
                    string simCoreName = attr.name;
                    string scenConKey = convertSimCoreToScenCon(simCoreName);

                    if (currentScenConState.Parameters.ContainsKey(scenConKey))
                    {
                        switch (attrType)
                        {
                            case "StringType":
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildString(Convert.ToString(currentScenConState.Parameters[scenConKey])));
                                break;/*ConvertString(Convert.ToString(currentScenConState.Parameters[scenConKey])*/

                            case "IntegerType":
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildInteger(Convert.ToInt32(currentScenConState.Parameters[scenConKey])));
                                break;/*ConvertInteger(Convert.ToInt32(currentScenConState.Parameters[scenConKey])*/

                            case "DoubleType":
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildDouble(Convert.ToDouble(currentScenConState.Parameters[scenConKey])));
                                break;/*ConvertDouble(Convert.ToDouble(currentScenConState.Parameters[scenConKey])*/

                            case "LocationType":
                                LocationType lt = currentScenConState.Parameters[scenConKey] as LocationType;
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildLocation(lt.X, lt.Y, lt.Z, false));
                                break;/*ConvertLocation((LocationType)currentScenConState.Parameters[scenConKey])*/

                            case "VelocityType":
                                VelocityType vt = currentScenConState.Parameters[scenConKey] as VelocityType;
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildVelocity(vt.VX, vt.VY, vt.VZ));
                                break;/*ConvertVelocity((VelocityType)currentScenConState.Parameters[scenConKey])*/
                            case "BooleanType":
                                if (currentScenConState.Parameters.ContainsKey(attr.name))
                                {
                                    string booleanVal = currentScenConState.Parameters[attr.name].ToString();
                                    bool value = false;
                                    if (booleanVal == "true" || booleanVal == "True" || booleanVal == "TRUE")
                                    {
                                        value = true;
                                    }
                                    ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildBoolean(value));
                                }/*ConvertBoolean(value)*/
                                break;
                            case "StringListType":
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, DataValueFactory.BuildStringList((List<String>)currentScenConState.Parameters[scenConKey]));
                                break;
                            case "ClassificationDisplayRulesType":
                                ((AttributeCollectionValue)currentSimCoreState).attributes.Add(simCoreName, (DataValue)currentScenConState.Parameters[scenConKey]);
                                break;
                            default:
                                break;
                        }
                    }
                }

                SimCoreStateTable.Add(kvp.Key, currentSimCoreState);
            }
            dv = new StateTableValue();
            ((StateTableValue)dv).states = SimCoreStateTable;
            e["StateTable"] = dv;

            ((AttributeCollectionValue)e["Attributes"]).attributes.Add("OwnerID", DataValueFactory.BuildString(incoming.Owner));/*ConvertString(    .Owner)*/
            ((AttributeCollectionValue)e["Attributes"]).attributes.Add("ClassName", DataValueFactory.BuildString(incoming.UnitBase));/*ConvertString(incoming.UnitBase)*/


            server.PutEvent(e);
        }
        /// <summary>
        /// This method takes in a Move_EventType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The Move_EventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendMoveEvent(Move_EventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "MoveObject");

            try
            {
                e["DestinationLocation"] = DataValueFactory.BuildLocation(incoming.Destination.X, incoming.Destination.Y, incoming.Destination.Z, true);//ConvertLocation(incoming.Destination, true);
                e["Throttle"] = DataValueFactory.BuildDouble(incoming.Throttle);//ConvertDouble(incoming.Throttle);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);// ConvertInteger(incoming.Timer);
            }
            catch
            {
                throw new Exception("Missing Required Data for Move Event");
            }
            server.PutEvent(e);
        }

        private static void SendUpdateTagEvent(UpdateTagType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo,"UpdateTag");
            try
            {
                e["UnitID"]=DataValueFactory.BuildString(incoming.UnitID);
                e["Tag"]=DataValueFactory.BuildString(incoming.NewTag);
                StringListValue teamMembers = new StringListValue();
                teamMembers.strings = incoming.TeamMembers;
                e["TeamMembers"]=(DataValue)teamMembers;
  
            }
            catch
            {
                throw new Exception("Error updating tag '"+incoming.NewTag+"'");
            }
            server.PutEvent(e);
        }

        private static void SendSubplatformLaunchEvent(SubplatformLaunchType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SubplatformLaunch");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);

                e["ParentObjectID"] = DataValueFactory.BuildString(incoming.ParentUnit);//ConvertString(incoming.UnitID);
                if (incoming.Destination.X == 0 &&
                    incoming.Destination.Y == 0 &&
                    incoming.Destination.Z == 0)
                {
                    e["LaunchDestinationLocation"] = DataValueFactory.BuildLocation(incoming.Destination.X, incoming.Destination.Y, incoming.Destination.Z, false);//ConvertLocation(incoming.Destination, true);
                }
                else
                {
                    e["LaunchDestinationLocation"] = DataValueFactory.BuildLocation(incoming.Destination.X, incoming.Destination.Y, incoming.Destination.Z, true);//ConvertLocation(incoming.Destination, true);     
                }
            }
            catch
            {
                throw new Exception("Missing Required Data for Launch Event");
            }
            server.PutEvent(e);
        }

        private static void SendSubplatformDockEvent(SubplatformDockType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SubplatformDock");

            try
            {
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);
                e["ParentObjectID"] = DataValueFactory.BuildString(incoming.ParentUnit);
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
            }
            catch
            {
                throw new Exception("Missing required data for subplatform dock event.");
            }
            server.PutEvent(e);
        }
        /// <summary>
        /// This method takes in a LAunchEventType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The Move_EventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendLaunchEvent(LaunchEventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SubplatformLaunch");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.Child);

                e["ParentObjectID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);
                if (incoming.RelativeLocation.X == 0 &&
                    incoming.RelativeLocation.Y == 0 &&
                    incoming.RelativeLocation.Z == 0)
                {
                    e["LaunchDestinationLocation"] = DataValueFactory.BuildLocation(incoming.RelativeLocation.X, incoming.RelativeLocation.Y, incoming.RelativeLocation.Z, false);//ConvertLocation(incoming.Destination, true);
                }
                else
                {
                    e["LaunchDestinationLocation"] = DataValueFactory.BuildLocation(incoming.RelativeLocation.X, incoming.RelativeLocation.Y, incoming.RelativeLocation.Z, true);//ConvertLocation(incoming.Destination, true);     
                }
            }
            catch
            {
                throw new Exception("Missing Required Data for Launch Event");
            }
            server.PutEvent(e);
        }

        private static void SendWeaponLaunchEvent(WeaponLaunchEventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "WeaponLaunch");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.Child);

                e["ParentObjectID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);
                e["TargetObjectID"] = DataValueFactory.BuildString(incoming.Target);//ConvertString(incoming.UnitID);
                
            }
            catch
            {
                throw new Exception("Missing Required Data for Launch Event");
            }
            server.PutEvent(e);
        }

        private static void SendEngramValue(EngramSettingType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "EngramValue");

            try
            {

                e["EngramName"] = DataValueFactory.BuildString(incoming.Name);
                if (incoming.Unit != "")
                    e["SpecificUnit"] = DataValueFactory.BuildString(incoming.Unit);

                e["EngramValue"] = DataValueFactory.BuildString(incoming.Value);
                e["EngramDataType"] = DataValueFactory.BuildString(incoming.Type);
            }
            catch
            {
                throw new Exception("Missing Required Data for Engram Setting");
            }
            server.PutEvent(e);
        }
        private static void SendEngramValue(ChangeEngramType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "EngramValue");

            try
            {

                e["EngramName"] = DataValueFactory.BuildString(incoming.Name);
                if (incoming.UnitID != "")
                    e["SpecificUnit"] = DataValueFactory.BuildString(incoming.UnitID);
                e["EngramValue"] = DataValueFactory.BuildString(incoming.EngramValue);
                e["EngramDataType"] = DataValueFactory.BuildString(incoming.GetType().ToString());
            }
            catch
            {
                throw new Exception("Missing Required Data for Engram Setting");
            }
            server.PutEvent(e);
        }

        /// <summary>
        /// This method takes in a StateChangeType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The StateChangeEventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendStateChangeEvent(StateChangeEvent incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "StateChange");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);
                e["NewState"] = DataValueFactory.BuildString(incoming.NewState);
            }
            catch
            {
                throw new Exception("Missing Required Data for Launch Event");
            }
            server.PutEvent(e);
        }

     private static void SendTransferEvent(TransferEvent incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "TransferObject");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["UserID"] = DataValueFactory.BuildString(incoming.To);
                e["DonorUserID"] = DataValueFactory.BuildString(incoming.From);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);
            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for Transfer notice.", ev);

            }
            server.PutEvent(e);

        }
 
        /// <summary>
        /// This method sends a command to open a chatroom after the command has been validated
        /// </summary>
        /// <param name="incoming">The   definition of the room, giving the name, the members and the requestor</param>
        private static void SendOpenChatRoomEvent(OpenChatRoomType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateChatRoom");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["RoomName"] = DataValueFactory.BuildString(incoming.Room);
                // ...
                e["SenderDM_ID"] = DataValueFactory.BuildString(incoming.Owner);
                List<string> members = new List<string>();
                for (int i = 0; i < incoming.Members.Count; i++)
                {
                    members.Add(incoming.Members[i]);
                }
                StringListValue listOfMembers = new StringListValue();
                listOfMembers.strings = members;
                e["MembershipList"] = (DataValue)listOfMembers;

            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for OpenChatRoom.", ev);

            }
            server.PutEvent(e);

        }
        /*
                private static void SendCreateChatRoomFailureType(CreateChatRoomFailureType incoming)
                {
                    SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "FailedToCreateChatRoom");

                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                        e["Message"] = DataValueFactory.BuildString(incoming.Message);
                        e["SenderDM_ID"] = DataValueFactory.BuildString(incoming.RequestingDM);
                    }
                    catch (SystemException ev)
                    {
                        throw new Exception("Missing a required attribute for OpenChatRoomFailureNotice.", ev);

                    }
                    server.PutEvent(e);

                }

        */
    

        private static void SendCloseChatRoomType(CloseChatRoomType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "CloseChatRoom");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["RoomName"] = DataValueFactory.BuildString(incoming.Room);
 // the text chat server doesn't need to know who is making the request
            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for CloseChatRoom.", ev);

            }
            server.PutEvent(e);

        }

        /// <summary>
        /// This method sends a command to open a whiteboardroom after the command has been validated
        /// </summary>
        /// <param name="incoming">The   definition of the room, giving the name, the members and the requestor</param>
        private static void SendOpenWhiteboardRoomEvent(OpenWhiteboardRoomType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateWhiteboardRoom");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["RoomName"] = DataValueFactory.BuildString(incoming.Room);
                // ...
                e["SenderDM_ID"] = DataValueFactory.BuildString(incoming.Owner);
                List<string> members = new List<string>();
                for (int i = 0; i < incoming.Members.Count; i++)
                {
                    members.Add(incoming.Members[i]);
                }
                StringListValue listOfMembers = new StringListValue();
                listOfMembers.strings = members;
                e["MembershipList"] = (DataValue)listOfMembers;

            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for OpenWhiteboardRoom.", ev);

            }
            server.PutEvent(e);

        }

        private static void SendAddToVoiceChannelEvent(GrantVoiceAccessType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "AddToVoiceChannel");
            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["ChannelName"] = DataValueFactory.BuildString(incoming.VoiceChannel);
                e["NewAccessor"] = DataValueFactory.BuildString(incoming.DecisionMaker);
            }
            catch(Exception ev)
            {
                throw new Exception("Missing a required attribute for GrantVoiceChannelAccess.", ev);
            }
            server.PutEvent(e);
        }


        private static void SendRemoveFromVoiceChannelEvent(RemoveVoiceAccessType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "RemoveFromVoiceChannel");
            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["ChannelName"] = DataValueFactory.BuildString(incoming.VoiceChannel);
                e["DeletedPlayer"] = DataValueFactory.BuildString(incoming.DecisionMaker);
            }
            catch (Exception ev)
            {
                throw new Exception("Missing a required attribute for RemoveVoiceChannelAccess.", ev);
            }
            server.PutEvent(e);
        }
        private static void SendOpenVoiceChannelEvent(OpenVoiceChannelType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateVoiceChannel");
            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["ChannelName"] = DataValueFactory.BuildString(incoming.Channel);
                // ...
                //e["SenderDM_ID"] = DataValueFactory.BuildString(incoming.Owner);
                List<string> members = new List<string>(incoming.InitialMembers.ToArray());
                //for (int i = 0; i < incoming.InitialMembers.Count;i++)
                //    members.Add(incoming.InitialMembers[i]);
               
                StringListValue listOfMembers = new StringListValue();
                listOfMembers.strings = members;
                e["MembershipList"] = (DataValue)listOfMembers;

            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for OpenVoiceChannel.", ev);
            }
            server.PutEvent(e);

        }


        private static void SendCloseVoiceChannelType(CloseVoiceChannelType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "CloseVoiceChannel");

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ChannelName"] = DataValueFactory.BuildString(incoming.Channel);
                // the voice server doesn't need to know who is making the request
            }
            catch (SystemException ev)
            {
                throw new Exception("Missing a required attribute for CloseVoiceChannel.", ev);

            }
            server.PutEvent(e);

        }

        /// <summary>
        /// This method takes in an ActiveREgionUpdateType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The Move_EventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendActiveRegionUpdateEvent(ActiveRegionUpdateType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ActiveRegionUpdate");
            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);
                e["IsVisible"] = DataValueFactory.BuildBoolean(incoming.IsVisible);
                e["IsActive"] = DataValueFactory.BuildBoolean(incoming.IsActive);
            }
            catch (SystemException f)
            {
                throw new ApplicationException("Missing Required Data for Active Region Update:", f);
            }
            server.PutEvent(e);
        }
        /// <summary>
        /// This method takes in a WeaponLaunch_EventType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">The Move_EventType object whose data is packaged
        /// into an outgoing SimulationEvent.</param>
        private static void SendWeaponLaunchEvent(WeaponLaunch_EventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "WeaponLaunch");

            try
            {
                e["TargetObjectID"] = DataValueFactory.BuildString(incoming.TargetID);//ConvertString(incoming.TargetID);
                e["ParentObjectID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.WeaponID);//ConvertString(incoming.WeaponID);
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
            }
            catch
            {
                throw new Exception("Missing Required Data for Weapon Launch");
            }
            server.PutEvent(e);
        }

        private static void SendWeaponLaunchFailureEvent(WeaponLaunchFailure_EventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "WeaponLaunchFailure");

            try
            {
                e["Reason"] = DataValueFactory.BuildString(incoming.ReasonForFailure);
                e["WeaponObjectID"] = DataValueFactory.BuildString(incoming.WeaponID);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.ParentID);
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);
            }
            catch
            {
                throw new Exception("Missing Required Data for Weapon Launch");
            }
            server.PutEvent(e);
        }

        /// <summary>
        /// This method takes in a Reveal_EventType (ScenCon defined object), and retrieves
        /// the data from the object, packages up the data into a SimulationEvent (SimCore defined
        /// object), and then sends the event to the network server.  This method was pulled out of
        /// the main block of code to simplify sending the event and code readability.
        /// </summary>
        /// <param name="incoming">
        /// The Reveal_EventType object whose data is packaged into an outgoing SimulationEvent.
        ///</param>
        private static void SendRevealEvent(Reveal_EventType incoming)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "RevealObject");
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();
            Dictionary<string, object> InitialAttributes;
            ObjectInfo myObject;
            //StateBody sb;
            string objectType = incoming.Genus.ToString();
            myObject = simModelInfo.objectModel.objects[objectType];
            if (incoming.Parameters != null)
            {
                InitialAttributes = incoming.Parameters.GetDictionary();
            }
            else
            {
                InitialAttributes = new Dictionary<string, object>();
            }

            try
            {
                e["Time"] = DataValueFactory.BuildInteger(incoming.Time);//ConvertInteger(incoming.Timer);
                e["ObjectID"] = DataValueFactory.BuildString(incoming.UnitID);//ConvertString(incoming.UnitID);
                //The "true" in the ConvertLocation function is to let the SimCore know
                //that the specified object is now "revealed", and can be affected
                //in the scenario.
                myAtt.Add("DockedToParent", DataValueFactory.BuildBoolean(incoming.DockedToParent));
                /*ConvertBoolean(incoming.DockedToParent)*/
                if (incoming.DockedToParent)
                {
                    myAtt.Add("Location", DataValueFactory.BuildLocation(0, 0, 0, false));
                }/*ConvertLocation(new LocationType(), false)*/
                else
                {
                    myAtt.Add("Location", DataValueFactory.BuildLocation(incoming.InitialLocation.X, incoming.InitialLocation.Y, incoming.InitialLocation.Z, true));
                }/*ConvertLocation(incoming.InitialLocation, true)*/
                myAtt.Add("State", DataValueFactory.BuildString(incoming.InitialState));/*ConvertString(incoming.InitialState)*/
                myAtt.Add("IsWeapon", DataValueFactory.BuildBoolean(incoming.IsWeapon));/*ConvertBoolean(incoming.IsWeapon)*/
                /*remove ID and state table from dictionary, as those don't need to be set.*/
                if (InitialAttributes.ContainsKey("UnitID"))
                    InitialAttributes.Remove("UnitID");
                if (InitialAttributes.ContainsKey("InitialLocation"))
                    InitialAttributes.Remove("InitialLocation");  //location is set above.
                if (InitialAttributes.ContainsKey("State"))
                    InitialAttributes.Remove("State");


                Dictionary<string, DataValue> tempDictionary = ParseAttributesList(myObject.attributes, InitialAttributes);

                foreach (KeyValuePair<string, DataValue> kvp in myAtt)
                {
                    if (tempDictionary.ContainsKey(kvp.Key))
                    {
                        tempDictionary[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        tempDictionary.Add(kvp.Key, kvp.Value);
                    }
                }
                e["Attributes"] = DataValueFactory.BuildAttributeCollection(tempDictionary); //ConvertAttributeCollection(tempDictionary);

            }
            catch
            {
                throw new Exception("Missing an essential attribute for Reveal");
            }
            server.PutEvent(e);
        }

        public static void SendSimStartEvent()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ExternalApp_SimStart");
            e["Time"] = DataValueFactory.BuildInteger(latestTick);//ConvertInteger(latestTick);
            server.PutEvent(e);
        }
        public static void SendSimStopEvent()
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "StopScenario");
            e["Time"] = DataValueFactory.BuildInteger(latestTick);//ConvertInteger(latestTick);
            server.PutEvent(e);
            e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ExternalApp_SimStop");
            e["Time"] = DataValueFactory.BuildInteger(latestTick);//ConvertInteger(latestTick);
            server.PutEvent(e);
        }
        /// <summary>
        /// This method receives a ScenCon event, and transforms the
        /// data into a valid SimCore event.  At this point, the 
        /// event is sent out to the network, and received by those
        /// objects subscribing to those types of events.
        /// Parameter:
        /// sendingEvent: A base ScenCon event, using GetType we can
        /// find out what type of event it is, and based on that info
        /// we can create the correct SimCore event to distribute.
        /// <param name="sendingEvent">
        /// <br><B>Sending event</B> is a Dennis style event, whose info gets converted
        /// and packaged in a Gabe styled event, and then added to the outgoing queue.
        /// </br></param>
        /// </summary>
        public static void SendEvent(RootEventType sendingEvent)
        {
            //This will discover the RootEventType's actual Type of event,
            //and then based on that, will break out the information into 
            //a simulation model event, and then putEvent to the NetworkClient
            string eventType;
            eventType = sendingEvent.GetType().Name.ToString();
            SimulationEvent e = null;
            Dictionary<string, DataValue> myAtt;

            switch (eventType)
            {
                case "StartupCompleteNotice":
                    SendStartupCompleteEvent();
                    break;

                /******************New Object Event Type Creation ******************************/
                case "Create_EventType":
                    SendCreateEvent((Create_EventType)sendingEvent);
                    break; //Break from New Event Type

                /******************Move Object Event Type Creation******************************/
                case "Move_EventType":
                    SendMoveEvent((Move_EventType)sendingEvent);
                    break;
                /*****************Update Tag Event Type ***************/
                case "UpdateTagType":
                    SendUpdateTagEvent((UpdateTagType)sendingEvent);
                    break;

                /******************Tick Event Type Creation*************************************/
                case "TickEventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "TimeTick");
                    e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//ConvertInteger(sendingEvent.Timer);
                    e["SimulationTime"] = DataValueFactory.BuildString(((TickEventType)sendingEvent).SimulationTime);
                    latestTick = sendingEvent.Time;
                    server.PutEvent(e);
                    break;

                /******************Attack Object Event Type Creation****************************/
                case "AttackObjectEvent":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "AttackObject");

                    try
                    {
                        e["ObjectID"] = DataValueFactory.BuildString(((AttackObjectEvent)sendingEvent).UnitID);//ConvertString(((AttackObjectEvent)sendingEvent).UnitID);
                        e["TargetObjectID"] = DataValueFactory.BuildString(((AttackObjectEvent)sendingEvent).TargetObjectID);//ConvertString(((AttackObjectEvent)sendingEvent).TargetObjectID);
                        e["CapabilityName"] = DataValueFactory.BuildString(((AttackObjectEvent)sendingEvent).CapabilityName);//ConvertString(((AttackObjectEvent)sendingEvent).CapabilityName);
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//(sendingEvent.Timer);
                    }
                    catch
                    {
                        throw new Exception("Missing required members of Attack Object Event");
                    }
                    server.PutEvent(e);
                    break;

                /******************Playfield Event Entered *************************************/
                case "PlayfieldEventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "Playfield");

                    try
                    {
                        e["MapDataFile"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).MapFileName);//ConvertString(((PlayfieldEventType)sendingEvent).MapFileName);
                        e["IconLibrary"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).IconLibrary);//ConvertString(((PlayfieldEventType)sendingEvent).IonLibrary);
                        e["UTMZone"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).UTMZone); //ConvertString(((PlayfieldEventType)sendingEvent).UTMZone);


                        //e["UTMNorthing"] = DataValueFactory.BuildDouble(((PlayfieldEventType)sendingEvent).VerticalScale);
                        //e["UTMEasting"] = DataValueFactory.BuildDouble(((PlayfieldEventType)sendingEvent).HorizontalScale);
                        e["VerticalScale"] = DataValueFactory.BuildDouble(((PlayfieldEventType)sendingEvent).VerticalScale);
                        e["HorizontalScale"] = DataValueFactory.BuildDouble(((PlayfieldEventType)sendingEvent).HorizontalScale);


                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//ConvertInteger(sendingEvent.Timer);
                        e["Name"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).ScenarioName);//ConvertString(((PlayfieldEventType)sendingEvent).ScenarioName);
                        string description = ((PlayfieldEventType)sendingEvent).Description;
                        description = description.Replace("\n", " ");
                        description = description.Replace("\t", " ");
                        e["Description"] = DataValueFactory.BuildString(description);//ConvertString(description);

                        e["DefaultDisplayLabels"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).DisplayLabels);//ConvertString(description);
                        e["DefaultDisplayTags"] = DataValueFactory.BuildString(((PlayfieldEventType)sendingEvent).DisplayTags);//ConvertString(description);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Playfield");
                    }
                    server.PutEvent(e);
                    break;

                /******************Decision Maker Entered **************************************/

                case "DecisionMakerType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");
                    myAtt = new Dictionary<string, DataValue>();

                    try
                    {
                        e["ObjectType"] = DataValueFactory.BuildString("DecisionMaker");
                        myAtt.Add("RoleName", DataValueFactory.BuildString(((DecisionMakerType)sendingEvent).Role));
                        myAtt.Add("TeamMember", DataValueFactory.BuildString(((DecisionMakerType)sendingEvent).Team));
                        myAtt.Add("Color", DataValueFactory.BuildInteger(((DecisionMakerType)sendingEvent).Chroma));
                        myAtt.Add("Briefing", DataValueFactory.BuildString(((DecisionMakerType)sendingEvent).Briefing));
                        myAtt.Add("CanTransfer", DataValueFactory.BuildBoolean(((DecisionMakerType)sendingEvent).HasTransferAuthority));
                        myAtt.Add("ReportsTo", DataValueFactory.BuildStringList(((DecisionMakerType)sendingEvent).Supervisors));
                        myAtt.Add("CanForceTransfer", DataValueFactory.BuildBoolean(((DecisionMakerType)sendingEvent).HasForcedTransferAuthority));
                        myAtt.Add("CanChat", DataValueFactory.BuildStringList(((DecisionMakerType)sendingEvent).ChatPartners));
                        myAtt.Add("CanWhiteboard", DataValueFactory.BuildStringList(((DecisionMakerType)sendingEvent).WhiteboardPartners));
                        myAtt.Add("CanSpeak", DataValueFactory.BuildStringList(((DecisionMakerType)sendingEvent).VoicePartners));
                        myAtt.Add("IsObserver", DataValueFactory.BuildBoolean(((DecisionMakerType)sendingEvent).IsObserver));
                        ////myAtt.Add("ComputerControlled", DataValueFactory.BuildBoolean(((DecisionMakerType)sendingEvent).ComputerControlled));
                        myAtt.Add("ComputerControlled", DataValueFactory.BuildBoolean(false));
                        e["ID"] = DataValueFactory.BuildString(((DecisionMakerType)sendingEvent).Identifier);
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        e["Attributes"] = DataValueFactory.BuildAttributeCollection(myAtt);
                        
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Decision Maker");
                    }
                    server.PutEvent(e);
                    break;

                /*************** Reveal Event Entered ****************************/

                case "Reveal_EventType":
                    SendRevealEvent((Reveal_EventType)sendingEvent);
                    break;

                /*************** Random Seed Event Entered ****************************/
                case "RandomSeedType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "RandomSeed");
                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//ConvertInteger(sendingEvent.Timer);
                        e["SeedValue"] = DataValueFactory.BuildInteger(((RandomSeedType)sendingEvent).seed);//ConvertInteger(((RandomSeedType)sendingEvent).seed);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Random Seed.");
                    }
                    server.PutEvent(e);
                    break;

                /**********************************Team Definition Event *********************/

                case "TeamType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");

                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        myAtt = new Dictionary<string, DataValue>();
                        myAtt.Add("TeamName", DataValueFactory.BuildString(((TeamType)sendingEvent).Name));
                        List<string> hostilities = new List<string>();
                        for (int x = 0; x < ((TeamType)sendingEvent).Count(); x++)
                        {
                            hostilities.Add(((TeamType)sendingEvent)[x]);
                        }
                        StringListValue listOfHostiles = new StringListValue();
                        listOfHostiles.strings = hostilities;
                        myAtt.Add("TeamHostility", (DataValue)listOfHostiles);
                        e["Attributes"] = DataValueFactory.BuildAttributeCollection(myAtt);
                        e["ID"] = DataValueFactory.BuildString(((TeamType)sendingEvent).Name);
                        e["ObjectType"] = DataValueFactory.BuildString("Team");
                        //e["StateTable"] = DataValueFactory.BuildString(string.Empty);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Team definition.");
                    }
                    server.PutEvent(e);
                    break;

                /**********************************Network Definition Event *********************/

                case "NetworkType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");

                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        myAtt = new Dictionary<string, DataValue>();
                        List<string> members = new List<string>();
                        for (int x = 0; x < ((NetworkType)sendingEvent).Count(); x++)
                        {
                            members.Add(((NetworkType)sendingEvent)[x]);
                        }
                        StringListValue listOfMembers = new StringListValue();
                        listOfMembers.strings = members;
                        myAtt.Add("DMMembers", (DataValue)listOfMembers);
                        e["Attributes"] = DataValueFactory.BuildAttributeCollection(myAtt);
                        e["ID"] = DataValueFactory.BuildString(((NetworkType)sendingEvent).Name);
                        e["ObjectType"] = DataValueFactory.BuildString("SensorNetwork");
                        //e["StateTable"] = DataValueFactory.BuildString(string.Empty);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Network definition.");
                    }
                    server.PutEvent(e);
                    break;

                /******************* Region Definition ********************************************/

                case "RegionEventType":
 
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");

                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        myAtt = new Dictionary<string, DataValue>();
                        List<PolygonValue.PolygonPoint> listOfPoints = new List<PolygonValue.PolygonPoint>();
                        PolygonValue.PolygonPoint point;
                        foreach (PointType pt in ((RegionEventType)sendingEvent).Vertices)
                        {
                            point = new PolygonValue.PolygonPoint(pt.X, pt.Y);
                            listOfPoints.Add(point);
                        }
                        PolygonValue pointsList = new PolygonValue();
                        pointsList.points = listOfPoints;
                        myAtt.Add("Polygon", (DataValue)pointsList);
                        e["ObjectType"] = DataValueFactory.BuildString("LandRegion");
                        if (((RegionEventType)sendingEvent).End != null)
                        {
                            myAtt.Add("EndHeight", DataValueFactory.BuildDouble((double)((RegionEventType)sendingEvent).End));
                            e["ObjectType"] = DataValueFactory.BuildString("ScoringRegion");
                        }
                        if (((RegionEventType)sendingEvent).Start != null)
                        {
                            myAtt.Add("StartHeight", DataValueFactory.BuildDouble((double)((RegionEventType)sendingEvent).Start));
                        }

                        if (((RegionEventType)sendingEvent).ReferencePoint != null) {
                            PointType refPoint = ((RegionEventType)sendingEvent).ReferencePoint;
                            myAtt.Add("ReferencePoint", DataValueFactory.BuildLocation(refPoint.X,refPoint.Y,0,true));
                        }

                        if (((RegionEventType)sendingEvent).IsDynamicRegion != null)
                        {
                       
                            myAtt.Add("IsDynamicRegion", DataValueFactory.BuildBoolean((bool)((RegionEventType)sendingEvent).IsDynamicRegion));
                        }

                        if (((RegionEventType)sendingEvent).SpeedMultiplier != null)
                        {
                            myAtt.Add("SpeedMultiplier", DataValueFactory.BuildDouble((double)((RegionEventType)sendingEvent).SpeedMultiplier));
                            e["ObjectType"] = DataValueFactory.BuildString("ActiveRegion");
                        }
                        if (((RegionEventType)sendingEvent).BlocksMovement != null)
                        {
                            myAtt.Add("BlocksMovement", DataValueFactory.BuildBoolean((Boolean)((RegionEventType)sendingEvent).BlocksMovement));
                        }
                        //if (((RegionEventType)sendingEvent).BlocksMovement != null)
                        //{
                        //    myAtt.Add("BlocksMovement", DataValueFactory.BuildBoolean((Boolean)((RegionEventType)sendingEvent).BlocksMovement));
                        //}


                        List<string> sensorsBlocked = new List<string>();
                        if (
                            (((RegionEventType)sendingEvent).SensorsBlocked != null)
                            &&
                            (((RegionEventType)sendingEvent).SensorsBlocked.Count > 0))
                        {
                            //                string sensorBlocked;
                            for (int sensor = 0; sensor < ((RegionEventType)sendingEvent).SensorsBlocked.Count; sensor++)
                            {

                                sensorsBlocked.Add(((RegionEventType)sendingEvent).SensorsBlocked[sensor]);
                            }

                        }
                        StringListValue mySensorsBlocked = new StringListValue();
                        mySensorsBlocked.strings = sensorsBlocked;
                        myAtt.Add("BlocksSensorTypes", (DataValue)mySensorsBlocked);

                        myAtt.Add("IsVisible", DataValueFactory.BuildBoolean((Boolean)((RegionEventType)sendingEvent).IsVisible));
                        myAtt.Add("IsActive", DataValueFactory.BuildBoolean((Boolean)((RegionEventType)sendingEvent).IsActive));
 
                        myAtt.Add("DisplayColor", DataValueFactory.BuildInteger((int)((RegionEventType)sendingEvent).Chroma));
                        try
                        {
                            if (((RegionEventType)sendingEvent).ObstructedViewImage != "" && ((RegionEventType)sendingEvent).ObstructedViewImage != null)
                            {
                                myAtt.Add("ObstructedViewImage", DataValueFactory.BuildString(((RegionEventType)sendingEvent).ObstructedViewImage));
                            }
                        }catch(Exception ex)
                        {}
                        try
                        { 
                            myAtt.Add("ObstructionOpacity", DataValueFactory.BuildString(((RegionEventType)sendingEvent).ObstructionOpacity.ToString()));
                        }catch(Exception ex2)
                        {}

                        //e["BlocksSensorTypes"]=
                        e["ID"] = DataValueFactory.BuildString(((RegionEventType)sendingEvent).UnitID);
                        e["Attributes"] = DataValueFactory.BuildAttributeCollection(myAtt);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for Region definition");
                    }
                    server.PutEvent(e);

                    break;

                case "ActiveRegionUpdateType":
                    SendActiveRegionUpdateEvent((ActiveRegionUpdateType)sendingEvent);
                    break;
                case "WeaponLaunch_EventType":
                    SendWeaponLaunchEvent((WeaponLaunch_EventType)sendingEvent);
                    break;
                case "WeaponLaunchFailure_EventType":
                    SendWeaponLaunchFailureEvent((WeaponLaunchFailure_EventType)sendingEvent);
                    break;
                case "LaunchEventType":
                    SendLaunchEvent((LaunchEventType)sendingEvent);
                    break;
                case "WeaponLaunchEventType":
                    SendWeaponLaunchEvent((WeaponLaunchEventType)sendingEvent);
                    break;
                case "SubplatformLaunchType":
                    SendSubplatformLaunchEvent((SubplatformLaunchType)sendingEvent);
                    break;
                case "SubplatformDockType":
                    SendSubplatformDockEvent((SubplatformDockType)sendingEvent);
                    break;
                case "StateChangeEvent":
                    SendStateChangeEvent((StateChangeEvent)sendingEvent);
                    break;
                case "OpenChatRoomType":
                    SendOpenChatRoomEvent((OpenChatRoomType)sendingEvent);
                    break;
                case "CloseChatRoomType":
                    SendCloseChatRoomType((CloseChatRoomType)sendingEvent);
                    break;
                case "OpenWhiteboardRoomType":
                    SendOpenWhiteboardRoomEvent((OpenWhiteboardRoomType)sendingEvent);
                    break;
                case "OpenVoiceChannelType":
                    SendOpenVoiceChannelEvent((OpenVoiceChannelType)sendingEvent);
                    break;
                case "CloseVoiceChannelType":
                    SendCloseVoiceChannelType((CloseVoiceChannelType)sendingEvent);
                    break;
                    /*
                case "GrantVoiceAccessType":
                    SendAddToVoiceChannelEvent((GrantVoiceAccessType)sendingEvent);
                    break;
                case "RemoveVoiceAccessType":
                    SendRemoveFromVoiceChannelEvent((RemoveVoiceAccessType)sendingEvent);
                    break;
                    
                case "CreateChatRoomFailureType":
                    SendCreateChatRoomFailureType((CreateChatRoomFailureType)sendingEvent);
                    break;*/
                case "TransferEvent":
                    SendTransferEvent((TransferEvent)sendingEvent);
                    break;
                    case "EngramSettingType":
                    SendEngramValue(((EngramSettingType)sendingEvent));
                    break;
                case "ChangeEngramType":
                    SendEngramValue(((ChangeEngramType)sendingEvent));
                    break;
                case "ClientSideAssetTransferType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ClientSideAssetTransferAllowed");
                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//ConvertInteger(sendingEvent.Timer);
                        e["EnableAssetTransfer"] = DataValueFactory.BuildBoolean(((ClientSideAssetTransferType)sendingEvent).assetTransferEnabled);//ConvertInteger(((RandomSeedType)sendingEvent).seed);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for ClientSideAssetTransferType");
                    }
                    server.PutEvent(e);
                    break;
                case "ClientSideStartingLabelVisibleType":
                    /* AD: TODO
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ClientSideAssetTransferAllowed");
                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);//ConvertInteger(sendingEvent.Timer);
                        e["EnableAssetTransfer"] = DataValueFactory.BuildBoolean(((ClientSideAssetTransferType)sendingEvent).assetTransferEnabled);//ConvertInteger(((RandomSeedType)sendingEvent).seed);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for ClientSideAssetTransferType");
                    }
                    server.PutEvent(e);
                     */
                    break;
                case "ClientSideRangeRingVisibilityType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ClientSideRangeRingDisplayLevel");
                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        e["Value"] = DataValueFactory.BuildString(((ClientSideRangeRingVisibilityType)sendingEvent).clientSideRangeRingVisibility);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for ClientSideRangeRingVisibilityType");
                    }
                    server.PutEvent(e);
                    break;
                case "SendChatMessageType":
                    SendChatMessageType scm = (SendChatMessageType)sendingEvent;
                    /* Done in Timer...  
                       * if (!ChatRooms.IsRoom(scm.RoomName))
                           break;  */

                    try
                    {
                        e = SimulationEventFactory.BuildEvent(ref simModelInfo, "TextChatRequest");
                        e["ChatBody"] = DataValueFactory.BuildString(scm.Message);
                        e["UserID"] = DataValueFactory.BuildString(scm.Sender);
                        e["TargetUserID"] = DataValueFactory.BuildString(scm.RoomName);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for SendChatMessage");
                    }
                    server.PutEvent(e);

                    break;
                case "SendVoiceMessageType":
                    SendVoiceMessageType svm = (SendVoiceMessageType)sendingEvent;
                    try
                    {
                        e = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayVoiceMessage");
                        e["Channel"] = DataValueFactory.BuildString(svm.ChannelName);
                        e["File"] = DataValueFactory.BuildString(svm.FilePath);
                        e["Time"] = DataValueFactory.BuildInteger(svm.Time);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for SendVoiceMessage");
                    }
                    server.PutEvent(e);

                    break;

                case "SendVoiceMessageToUserType":
                    SendVoiceMessageToUserType svmtu = (SendVoiceMessageToUserType)sendingEvent;
                    try
                    {
                        e = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayVoiceMessageToUser");
                        e["DecisionMakerID"] = DataValueFactory.BuildString(svmtu.DecisionMakerID);
                        e["File"] = DataValueFactory.BuildString(svmtu.FilePath);
                        e["Time"] = DataValueFactory.BuildInteger(svmtu.Time);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for SendVoiceMessageToUser");
                    }
                    server.PutEvent(e);

                    break;

                case "SystemMessage":
                    SystemMessage sm = (SystemMessage)sendingEvent;
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SystemMessage");
                    e["PlayerID"] = DataValueFactory.BuildString(sm.Addressee);
                    e["Message"] = DataValueFactory.BuildString(sm.Message);
                    e["TextColor"] = DataValueFactory.BuildInteger(sm.TextColor);
                    e["DisplayMode"] = DataValueFactory.BuildString(sm.DisplayMode);
                    server.PutEvent(e);
                    break;

                case "ClassificationsType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "InitializeClassifications");
                    try
                    {
                        e["Time"] = DataValueFactory.BuildInteger(sendingEvent.Time);
                        e["Classifications"] = DataValueFactory.BuildStringList(((ClassificationsType)sendingEvent).classifications);
                    }
                    catch
                    {
                        throw new Exception("Missing a required attribute for ClientSideRangeRingVisibilityType");
                    }
                    server.PutEvent(e);
                    break;
                case "ForkReplayEventType":
                    server.PutEvent(((ForkReplayEventType)sendingEvent).Event);
                    break;
                /******************No valid event entered***************************************/
                /******************Very Basic Event Type Creation*******************************/
                case "RootEventType":
                    throw new Exception("RootEventType events should not be created");

                /******************Base Scenario Event Type Creation****************************/
                case "ScenarioEventType":
                    throw new Exception("ScenarioEventType events should not be created");  
                default:
                    //What should it do in this case? Nothing?
                    Console.Out.Write("In EventCommunicator: Missing event type to send: " + eventType);
                    break;
            }
        }
        /// <summary>
        /// This method takes in a dictionary that represents the SimulationModel parameters, and a Dictionary
        /// that is to be used to populate a DataValue dictionary given the Simulation Model.  Foreach entry
        /// in the SimModel dictionary, if the same key (witch is converted from SimCoreKey to ScenConKey)
        /// exists in the given DataValue dictionary, then that value is used rather than a default DataValue.
        /// The newly created DataValue dictionary represents every member in the SimModel dictionary, and uses
        /// the available data from the given DataValue dictionary.
        /// </summary>
        /// <param name="objectAttributes">
        /// This Dictionary is indexed by a string that is the parameter name, and the value is an AttributeInfo
        /// object that contains the name and type of the parameter.
        /// </param>
        /// <param name="SendingEventAttributes">
        /// This dictionary contains attributes given from the ScenCon, to be passed on to the SimCore.  The 
        /// data is copied over to the correct entry in the resulting dictionary.
        /// </param>
        /// <returns></returns>
        private static Dictionary<string, DataValue> ParseAttributesList(Dictionary<string, AttributeInfo> objectAttributes, Dictionary<string, object> SendingEventAttributes)
        {
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();
            DataValue dv;
            string scenConKey,
                   simCoreKey,
                   attributeType;

            foreach (KeyValuePair<string, AttributeInfo> pair in objectAttributes)
            {
                if (!simModelIgnoreList.Contains(pair.Key))
                {
                    simCoreKey = pair.Key;
                    scenConKey = convertSimCoreToScenCon(simCoreKey);
                    if (SendingEventAttributes.ContainsKey(scenConKey))
                    {//Copy over the data 
                        attributeType = pair.Value.dataType;
                        switch (attributeType)
                        { //attribute type will be either the system defined name, or Dennis' type
                            case "StringType":

                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    myAtt.Add(simCoreKey, DataValueFactory.BuildString(Convert.ToString(SendingEventAttributes[scenConKey])));
                                }/*ConvertString(Convert.ToString(SendingEventAttributes[scenConKey])*/
                                else
                                {
                                    myAtt.Add(simCoreKey, DataValueFactory.BuildString(string.Empty));
                                }/*ConvertString(String.Empty)*/

                                break;
                            case "IntegerType":
                                dv = new IntegerValue();
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    dv = DataValueFactory.BuildInteger(Convert.ToInt32(SendingEventAttributes[scenConKey]));//ConvertInteger(Convert.ToInt32(SendingEventAttributes[scenConKey]));
                                }
                                myAtt.Add(simCoreKey, dv);
                                break;
                            case "BooleanType":
                                dv = new BooleanValue();
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    dv = DataValueFactory.BuildBoolean(Convert.ToBoolean(SendingEventAttributes[scenConKey]));//ConvertBoolean(Convert.ToBoolean(SendingEventAttributes[scenConKey]));
                                }
                                myAtt.Add(simCoreKey, dv);
                                break;
                            case "DoubleType":
                                dv = new DoubleValue();
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    dv = DataValueFactory.BuildDouble(Convert.ToDouble(SendingEventAttributes[scenConKey]));//ConvertDouble(Convert.ToDouble(SendingEventAttributes[scenConKey]));
                                }
                                myAtt.Add(simCoreKey, dv);
                                break;

                            case "LocationType":

                                dv = new LocationValue();
                                ((LocationValue)dv).exists = false;
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    LocationType lt = SendingEventAttributes[scenConKey] as LocationType;
                                    dv = DataValueFactory.BuildLocation(lt.X, lt.Y, lt.Z, true);//ConvertLocation((LocationType)SendingEventAttributes[scenConKey], true);
                                }
                                myAtt.Add(simCoreKey, dv);
                                break;
                            case "VelocityType":

                                dv = new VelocityValue();
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    VelocityType vt = SendingEventAttributes[scenConKey] as VelocityType;
                                    dv = DataValueFactory.BuildVelocity(vt.VX, vt.VY, vt.VZ);//ConvertVelocity((VelocityType)SendingEventAttributes[scenConKey]);
                                }
                                myAtt.Add(simCoreKey, dv);
                                break;

                            case "StringListType":
                                dv = new StringListValue();
                                if (SendingEventAttributes.ContainsKey(scenConKey))
                                {
                                    ((StringListValue)dv).strings = (List<string>)SendingEventAttributes[scenConKey];
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return myAtt;
        }
        public static void SendPauseScenarioEvent()
        {
            if (latestTick > 0)
            {
                SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "PauseScenario");
                e["Time"] = DataValueFactory.BuildInteger(latestTick);

                server.PutEvent(e);
            }
        }
        public static void SendResumeScenarioEvent()
        {
            //if (latestTick > 0)
            //{
                SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ResumeScenario");
                e["Time"] = DataValueFactory.BuildInteger(latestTick);

                server.PutEvent(e);
            //}
        }
        /// <summary>
        /// Given a dictionary of [string, datavalue], a new DataValue is created, and assigned as an 
        /// Attribute Collection Value.  This AttributeCollectionValue's attribute setting is set to the input
        /// dictionary, and then the AttributeCollectionValue is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// A dictionary of [string, DataValue] to be assigned to an AttributeCollectionValue.
        /// </param>
        /// <returns></returns>
        private static DataValue ConvertAttributeCollection(Dictionary<string, DataValue> input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new AttributeCollectionValue();
            ((AttributeCollectionValue)dv).attributes = input;
            return dv;
        }
        /// <summary>
        /// Given a string, a new DataValue is created, and assigned as a 
        /// StringValue.  This StringValue's value setting is set to the input
        /// string, and then the StringValue is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// A string to be assigned to a StringValue.
        /// </param>
        /// <returns>
        ///Returns an abstract class DataValue, which is a StringValue containing 
        /// a given string for its "value" member.
        ///</returns>
        private static DataValue ConvertString(string input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new StringValue();
            ((StringValue)dv).value = input;
            return dv;
        }
        /// <summary>
        /// Given an integer, a new DataValue is created, and assigned as an 
        /// IntegerValue.  This IntegerValue's value setting is set to the input
        /// int, and then the IntegerValue is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// An integer to be assigned to a IntegerValue.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertInteger(int input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new IntegerValue();
            ((IntegerValue)dv).value = input;
            return dv;
        }
        /// <summary>
        /// Given a boolean, a new DataValue is created, and assigned as an 
        /// BooleanValue.  This BooleanValue's value setting is set to the input
        /// bool, and then the BooleanValue is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// An bool to be assigned to a BooleanValue.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertBoolean(bool input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new BooleanValue();
            ((BooleanValue)dv).value = input;
            return dv;
        }
        /// <summary>
        /// Given a double, a new DataValue is created, and assigned as a 
        /// DoubleValue.  This DoubleValue's value setting is set to the input
        /// double, and then the DoubleValue is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// A double to be assigned to a DoubleValue.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertDouble(double input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new DoubleValue();
            ((DoubleValue)dv).value = input;
            return dv;
        }
        /// <summary>
        /// Given a LocationType (ScenCon defined object), a new DataValue is created, and assigned as a 
        /// LocationValue.  This LocationValue's coordinate settings are set to the input
        /// LocationType, and then the LocationType is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// A LocationType object, whose coordinates are assigned to a LocationValue.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertLocation(LocationType input)
        {//Replaced by method in DataValueFactory...
            return ConvertLocation(input, false);
        }
        /// <summary>
        /// Given a LocationType (ScenCon defined object), a new DataValue is created, and assigned as a 
        /// LocationValue.  This LocationValue's coordinate settings are set to the input
        /// LocationType, and then the LocationType is returned as a DataValue.  A boolean value
        /// is passed along with the LocationType object which determines whether or not the
        /// object is revealed or not.
        /// </summary>
        /// <param name="input">
        /// A LocationType object, whose coordinates are assigned to a LocationValue.
        /// </param>
        /// <param name="exists">
        /// A boolean value.  If exists is true, then the object is visible to the Simulator, if
        /// the value is false then it is not visible, and cannot be manipulated within the scenario.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertLocation(LocationType input, bool exists)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new LocationValue();
            ((LocationValue)dv).X = input.X;
            ((LocationValue)dv).Y = input.Y;
            ((LocationValue)dv).Z = input.Z;
            ((LocationValue)dv).exists = exists;
            return dv;
        }
        /// <summary>
        /// Given a VelocityType (ScenCon defined object), a new DataValue is created, and assigned as a 
        /// VelocityValue.  This LocationValue's coordinate settings are set to the input
        /// VelocityType, and then the VelocityType is returned as a DataValue.
        /// </summary>
        /// <param name="input">
        /// A VelocityType object, whose coordinates are assigned to a VelocityValue.
        /// </param>
        /// <returns>
        ///</returns>
        private static DataValue ConvertVelocity(VelocityType input)
        {//Replaced by method in DataValueFactory...
            DataValue dv = new VelocityValue();
            ((VelocityValue)dv).VX = input.VX;
            ((VelocityValue)dv).VY = input.VY;
            ((VelocityValue)dv).VZ = input.VZ;
            return dv;
        }
    }
}
