using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;

namespace Aptima.Asim.DDD.CommonComponents.HandshakeManager
{

    public class HandshakeManager
    {
        public class DecisionMakerLoginInfo
        {
            public string DMName = string.Empty;
            public string DMAvail = string.Empty;
            public string DMRole = string.Empty;

            public DecisionMakerLoginInfo()
            { }

            public DecisionMakerLoginInfo(string name, string role, string avail)
            {
                DMName = name;
                DMRole = role;
                DMAvail = avail;
            }
        }
        private static string scenarioName = string.Empty;
        private static string scenarioDescription = string.Empty;
        private static string mapName = string.Empty;
        private static string iconLibrary = string.Empty;
        private static string displayTags = string.Empty;
        private static string displayLabels = string.Empty;
        private static double northing = 0.0;
        private static double easting = 0.0;
        private static double horizMetersPerPixel = 0.0;
        private static double vertMetersPerPixel = 0.0;
        private static bool playfieldIsSet = false;
        private static bool enableAssetTransfers = true;
        private static bool voiceChatEnabled = false;
        private static string voiceChatServerName = string.Empty;
        private static int voiceChatServerPort = 0;
        private static string voiceChatPassword = string.Empty;

        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static Dictionary<string, string> dmToTerminalMap;
        private static object dmMapLock = new object();
        private static object dmLock = new object();
        private static Dictionary<string, string> termToDMMap;
        private static Dictionary<string, bool> dmsIsReady; //dm is the "ID" of the dm
        public static bool stopLoop = false;
        public static Dictionary<string, SimulationEvent> listOfChatRoomCreates;
        public static Dictionary<string, SimulationEvent> listOfWhiteboardRoomCreates;
        public static Dictionary<string, SimulationEvent> listOfVoiceChannelCreates;

        private static bool isForkReplay = false;
        public static bool IsForkReplay
        {
            set { isForkReplay = value; }
            get { return isForkReplay; }
        }
        public static void SetVoiceEnabled(bool value)
        {
            voiceChatEnabled = value;
        }
        public static void SetVoiceServerName(string value)
        {
            voiceChatServerName = value;
        }
        public static void SetVoiceServerPort(int value)
        {
            voiceChatServerPort = value;
        }
        public static void SetVoiceServerPassword(string value)
        {
            voiceChatPassword = value;
        }

        //private static List<string> availableDMs;
        //public static List<string> AvailableDMs
        //{
        //    get { return availableDMs; }
        //}
        public static DecisionMaker GetSpecificDMInfo(string s)
        {
            if (allDMs.ContainsKey(s))
            {
                DecisionMaker dm = new DecisionMaker(allDMs[s].id, allDMs[s].team);
                lock (dmLock)
                {
                    dm.availability = allDMs[s].availability;
                    dm.briefing = allDMs[s].briefing;
                    dm.color = allDMs[s].color;
                    dm.isHuman = allDMs[s].isHuman;
                    dm.role = allDMs[s].role;
                }
                return dm;
            }

            return null;
        }
        public static List<string> GetAllDMs()
        {
            List<string> dms = new List<string>();

            lock (dmLock)
            {
                foreach (string s in allDMs.Keys)
                {
                    dms.Add(s);
                }
            }
            return dms;
        }
        private static Dictionary<string, DecisionMaker> allDMs;
        public static string GetDMAvailability(string dm)
        {
            if (allDMs.ContainsKey(dm))
            {
                if (!allDMs[dm].isHuman)
                    return "Taken";
                switch (allDMs[dm].availability)
                {
                    case DecisionMaker.Availability.AVAILABLE:
                        return "Available";
                    case DecisionMaker.Availability.TAKEN:
                        return "Taken";
                    case DecisionMaker.Availability.READY:
                        return "Ready";
                }
            }
            return string.Empty;
        }
        public static string GetDMRole(string dm)
        {
            if (allDMs.ContainsKey(dm))
            {
                return allDMs[dm].role;
            }
            return string.Empty;
        }

        public HandshakeManager(ref SimulationEventDistributorClient netServ, ref SimulationModelInfo simModel)
        {
            //if server != null?
            dmToTerminalMap = new Dictionary<string, string>();
            termToDMMap = new Dictionary<string, string>();
            dmsIsReady = new Dictionary<string, bool>();
            //server = new NetworkClient();
            //availableDMs = new List<string>();
            allDMs = new Dictionary<string, DecisionMaker>();
            simModelInfo = simModel;
            server = netServ;
            listOfChatRoomCreates = new Dictionary<string, SimulationEvent>();
            listOfWhiteboardRoomCreates = new Dictionary<string, SimulationEvent>();
            listOfVoiceChannelCreates = new Dictionary<string, SimulationEvent>();
            enableAssetTransfers = true;
        }

        public void StartHandshakeManager()
        {
            //started in a thread

            List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
            string eventType;

            //if (availableDMs.Count == 0)
            //    return;

            //foreach (string s in availableDMs)
            lock (dmLock)
            {
                foreach (string s in allDMs.Keys)
                {
                    dmToTerminalMap.Add(s, string.Empty);
                }
            }

            server.Subscribe("HandshakeGUIRegister");
            server.Subscribe("HandshakeGUIRoleRequest");
            server.Subscribe("HandshakeInitializeGUIDone");
            server.Subscribe("ClientSideAssetTransferAllowed");
            while (!stopLoop)
            {
                incomingEvents = server.GetEvents();
                foreach (SimulationEvent e in incomingEvents)
                {
                    eventType = e.eventType;

                    switch (eventType)
                    {
                        case "HandshakeGUIRegister":
                            GUIRegister(e);
                            break;
                        case "HandshakeGUIRoleRequest":
                            GUIRoleRequest(e);
                            break;
                        case "HandshakeInitializeGUIDone":
                            InitializeGUIDone(e);
                            break;
                        default:

                            break;

                    }
                }
                Thread.Sleep(200);
            }
        }
        public void ResetHandshakeManager()
        {
            dmToTerminalMap = new Dictionary<string, string>();
            termToDMMap = new Dictionary<string, string>();
            dmsIsReady = new Dictionary<string, bool>();
            //availableDMs = new List<string>();
            allDMs = new Dictionary<string, DecisionMaker>();
            listOfChatRoomCreates = new Dictionary<string, SimulationEvent>();
            listOfWhiteboardRoomCreates = new Dictionary<string, SimulationEvent>();
            listOfVoiceChannelCreates = new Dictionary<string, SimulationEvent>();
            enableAssetTransfers = true;
        }


        public void DisconnectPlayer(SimulationEvent e)
        {
            string dmID = ((StringValue)e["DecisionMakerID"]).value;
            
            if (dmToTerminalMap.ContainsKey(dmID))
            {
                string termID = dmToTerminalMap[dmID];
                dmToTerminalMap.Remove(dmID);
                termToDMMap.Remove(termID);
            }
            if (dmsIsReady.ContainsKey(dmID))
            {
                dmsIsReady.Remove(dmID);
            }
            if (allDMs.ContainsKey(dmID))
            {
                allDMs[dmID].availability = DecisionMaker.Availability.AVAILABLE;
            }
            SimulationEvent player = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayerControl");
            ((StringValue)player["DecisionMakerID"]).value = dmID;
            ((StringValue)player["ControlledBy"]).value = "COMPUTER";
            server.PutEvent(player);
        }
        public void DisconnectTerminal(SimulationEvent e)
        {
            string termID = ((StringValue)e["TerminalID"]).value;
            string dmID = "";
            if (termToDMMap.ContainsKey(termID))
            {
                dmID = termToDMMap[termID]; //.ToUpper()];
            }


            if (dmToTerminalMap.ContainsKey(dmID))
            {
                termID = dmToTerminalMap[dmID];
                dmToTerminalMap.Remove(dmID);
                termToDMMap.Remove(termID);
            }
            if (dmsIsReady.ContainsKey(dmID))
            {
                dmsIsReady.Remove(dmID);
            }
            if (allDMs.ContainsKey(dmID))
            {
                allDMs[dmID].availability = DecisionMaker.Availability.AVAILABLE;
            }

            SimulationEvent player = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayerControl");
            ((StringValue)player["DecisionMakerID"]).value = dmID;
            ((StringValue)player["ControlledBy"]).value = "COMPUTER";
            server.PutEvent(player);

            player = SimulationEventFactory.BuildEvent(ref simModelInfo, "DisconnectDecisionMaker");
            ((StringValue)player["DecisionMakerID"]).value = dmID;
            server.PutEvent(player);
        }
        public void ReceiveDecisionMakerEvent(SimulationEvent dmEvent)
        {
            string dmID = ((StringValue)dmEvent["ID"]).value;
            string dmRole = ((StringValue)((AttributeCollectionValue)dmEvent["Attributes"])["RoleName"]).value;
            string team = ((StringValue)((AttributeCollectionValue)dmEvent["Attributes"])["TeamMember"]).value;
            int dmColor = ((IntegerValue)((AttributeCollectionValue)dmEvent["Attributes"])["Color"]).value;
            string dmBriefing = ((StringValue)((AttributeCollectionValue)dmEvent["Attributes"])["Briefing"]).value;
            Boolean isObserver = ((BooleanValue)((AttributeCollectionValue)dmEvent["Attributes"])["IsObserver"]).value;
            DecisionMaker dm = new DecisionMaker(dmID, null); //STUB; need teams info
            dm.role = dmRole;
            dm.color = dmColor;
            dm.briefing = dmBriefing;
            dm.isHuman = !((BooleanValue)((AttributeCollectionValue)dmEvent["Attributes"])["ComputerControlled"]).value;
            if (!allDMs.ContainsKey(dmID))
                allDMs.Add(dmID, dm);
            dm.isObserver = isObserver;
            //SimulationEvent player = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayerControl");
            //((StringValue)player["DecisionMakerID"]).value = dmID;
            //((StringValue)player["ControlledBy"]).value = "COMPUTER";
            //server.PutEvent(player);

        }
        public void SetPlayfieldInformation(SimulationEvent e)
        {
            string str = string.Empty;
            if (e.parameters.ContainsKey("MapDataFile"))
            {
                str = ((StringValue)e["MapDataFile"]).value;
                if (str.Contains(".geo"))
                {
                    str = str.Replace(".geo", ".jpg");
                }
                mapName = str;
                str = null;

            }
            if (e.parameters.ContainsKey("UTMNorthing"))
            {
                northing = ((DoubleValue)e["UTMNorthing"]).value;
            }
            if (e.parameters.ContainsKey("UTMEasting"))
            {
                easting = ((DoubleValue)e["UTMEasting"]).value;
            }
            if (e.parameters.ContainsKey("HorizontalScale"))
            {
                horizMetersPerPixel = ((DoubleValue)e["HorizontalScale"]).value;
            }
            if (e.parameters.ContainsKey("VerticalScale"))
            {
                vertMetersPerPixel = ((DoubleValue)e["VerticalScale"]).value;
            }
            if (e.parameters.ContainsKey("Name"))
            {
                str = ((StringValue)e["Name"]).value;
                scenarioName = str;
                str = null;

            }
            if (e.parameters.ContainsKey("Description"))
            {
                str = ((StringValue)e["Description"]).value;
                scenarioDescription = str;
                str = null;

            }
            if (e.parameters.ContainsKey("IconLibrary"))
            {
                iconLibrary = ((StringValue)e["IconLibrary"]).value;
            }
            if (e.parameters.ContainsKey("EnableAssetTransfer"))
            {
                enableAssetTransfers = ((BooleanValue)e["EnableAssetTransfer"]).value;
            }
            if (e.parameters.ContainsKey("DefaultDisplayLabels"))
            {
                displayLabels = ((StringValue)e["DefaultDisplayLabels"]).value;
            }
            if (e.parameters.ContainsKey("DefaultDisplayTags"))
            {
                displayTags = ((StringValue)e["DefaultDisplayTags"]).value;
            }
            playfieldIsSet = true;
        }
        public void GUIRegister(SimulationEvent e)
        { //event contains TerminalID
            string terminalID;
            // foreach (string s in availableDMs)
            lock (dmLock)
            {
                foreach (string s in allDMs.Keys)
                {
                    if (!dmToTerminalMap.ContainsKey(s))
                        dmToTerminalMap.Add(s, string.Empty);
                }
            }
            //take terminal id, add to map, then send out available DMs event
            try
            {
                terminalID = ((StringValue)e["TerminalID"]).value;
            }
            catch
            {
                throw new Exception("Terminal ID does not exist in this event.");
            }
            //This is where you would see if the registering terminal already has logged in
            //if so, re-send them their info, otherwise, continue.
            if (!termToDMMap.ContainsKey(terminalID))
            {
                termToDMMap.Add(terminalID, string.Empty);

            }

            //send out available DMs
            if (termToDMMap[terminalID] == string.Empty)
            {
                SendAvailableDMEvent(terminalID);
            }
            //else
            //{//user already logged in, refresh their view 
            //    SendOutScenarioInfo(termToDMMap[terminalID]);
            //}
        }
        public void GUIRoleRequest(SimulationEvent e)
        {//event contains TerminalID and PlayerID
            string terminalID;
            string playerID;
            string loginType;
            bool success = true;
            //lock the map, see if dm is available, see if terminal id is not currently assigned a dm

            try
            {
                terminalID = ((StringValue)e["TerminalID"]).value;
                playerID = ((StringValue)e["PlayerID"]).value;
                loginType = ((StringValue)e["LoginType"]).value;
            }
            catch
            {
                throw new Exception("Missing a required attribute for this event.");
            }

            if (loginType == "FULL")
            {
                lock (dmLock)
                {
                    foreach (string s in allDMs.Keys)
                    {
                        if (!dmToTerminalMap.ContainsKey(s))
                            dmToTerminalMap.Add(s, string.Empty);
                    }
                }

                lock (dmMapLock)
                {
                    //series of conditions for the player to be accepted.
                    if (!allDMs.ContainsKey(playerID))
                    {
                        success = false;
                    }

                    //if (!dmToTerminalMap.ContainsKey(playerID))
                    //{
                    //    if (dmToTerminalMap[playerID] == terminalID)
                    //    {
                    //        SendOutScenarioInfo(playerID);
                    //    }
                    //    return;
                    //}

                    if (!termToDMMap.ContainsKey(terminalID))
                    {
                        success = false;
                    }

                    if (dmToTerminalMap[playerID] != string.Empty)
                    {
                        success = false;
                    }

                    if (termToDMMap[terminalID] != string.Empty)
                    {
                        success = false;
                    }

                    if (success)
                    {
                        dmToTerminalMap[playerID] = terminalID;
                        termToDMMap[terminalID] = playerID;
                    }
                }
                //add to DMs ready a false val
                if (success)
                {
                    dmsIsReady.Add(playerID, false);
                    allDMs[playerID].availability = DecisionMaker.Availability.TAKEN;
                    SendOutScenarioInfo(playerID,dmToTerminalMap[playerID]);
                }
                else
                {
                    SendAvailableDMEvent(terminalID);
                }
            }
            else if (loginType == "OBSERVER")
            {
                SendOutScenarioInfo(playerID,terminalID);
            }
            //if any changes are made to available DMs, send out an available DMs event.

            //send specific player scenario info
            

            //Thread.Sleep(100);

            //send out available DMs
            //SendAvailableDMEvent();

        }
        public void InitializeGUIDone(SimulationEvent e)
        { //event contains PlayerID
            string playerID;
            string loginType;
            try
            {
                playerID = ((StringValue)e["PlayerID"]).value;
                loginType = ((StringValue)e["LoginType"]).value;
            }
            catch
            {
                throw new Exception("Player ID does not exist in this event.");
            }
            if (loginType == "FULL")
            {
                //add to DMs ready a true val for the specified dm
                dmsIsReady[playerID] = true;
                allDMs[playerID].availability = DecisionMaker.Availability.READY;
                Thread.Sleep(100);//delay so client can synch up
                SendSystemMessageToAll(String.Format("SYSTEM: New user ({0}) has joined the simulation.", playerID));
                SimulationEvent player = SimulationEventFactory.BuildEvent(ref simModelInfo, "PlayerControl");
                ((StringValue)player["DecisionMakerID"]).value = playerID;
                ((StringValue)player["ControlledBy"]).value = "HUMAN";
                server.PutEvent(player);

                SimulationEvent assetTransferEnabled = SimulationEventFactory.BuildEvent(ref simModelInfo, "ClientSideAssetTransferAllowed");
                assetTransferEnabled["EnableAssetTransfer"] = DataValueFactory.BuildBoolean(enableAssetTransfers);
                server.PutEvent(assetTransferEnabled);

                ////The following section is in place for clients that join after a CreateChatRoom
                ////Event  has been sent out.  Without this, new tabbed chat rooms would not open
                ////if the room was created before they joined.
                foreach (SimulationEvent ev in listOfChatRoomCreates.Values)
                {
                    if (((StringListValue)ev["MembershipList"]).strings.Contains(playerID))
                    {
                        server.PutEvent(ev);
                    }
                }

                foreach (SimulationEvent ev in listOfWhiteboardRoomCreates.Values)
                {
                    if (((StringListValue)ev["MembershipList"]).strings.Contains(playerID))
                    {
                        server.PutEvent(ev);
                    }
                }
                ////The following section is in place for clients that join after a CreateVoiceChannel
                ////event is sent out
                foreach (SimulationEvent ev in listOfVoiceChannelCreates.Values)
                {
                    if (((StringListValue)ev["MembershipList"]).strings.Contains(playerID))
                    {
                        server.PutEvent(ev);
                    }
                }
            }
        }

        public void CreateWhiteboardRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            if (listOfWhiteboardRoomCreates.ContainsKey(roomName))
                return;

            List<string> members = ((StringListValue)e["MembershipList"]).strings;

            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateWhiteboardRoom");
            ev["RoomName"] = DataValueFactory.BuildString(roomName);
            ((StringListValue)ev["MembershipList"]).strings = members;

            listOfWhiteboardRoomCreates.Add(roomName, ev);
        }
        /// <summary>
        /// This is not called yet, but wanted to be proactive and define it in case.
        /// </summary>
        /// <param name="e"></param>
        public void CloseWhiteboardtRoom(SimulationEvent e)
        {
            if (listOfWhiteboardRoomCreates.ContainsKey(((StringValue)e["RoomName"]).value))
            {
                listOfWhiteboardRoomCreates.Remove(((StringValue)e["RoomName"]).value);
            }
        }

        public void CreateChatRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            if (listOfChatRoomCreates.ContainsKey(roomName))
                return;

            List<string> members = ((StringListValue)e["MembershipList"]).strings;
            
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateChatRoom");
            ev["RoomName"] = DataValueFactory.BuildString(roomName);
            ((StringListValue)ev["MembershipList"]).strings = members;

            listOfChatRoomCreates.Add(roomName, ev);
        }
        public void CloseChatRoom(SimulationEvent e)
        {
            if (listOfChatRoomCreates.ContainsKey(((StringValue)e["RoomName"]).value))
            {
                listOfChatRoomCreates.Remove(((StringValue)e["RoomName"]).value);
            }
        }
        public void CreateVoiceChannel(SimulationEvent e)
        {
            string roomName = ((StringValue)e["ChannelName"]).value;

            if (listOfVoiceChannelCreates.ContainsKey(roomName))
                return;

            List<string> members = ((StringListValue)e["MembershipList"]).strings;

            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModelInfo, "CreateVoiceChannel");
            ev["ChannelName"] = DataValueFactory.BuildString(roomName);
            ((StringListValue)ev["MembershipList"]).strings = members;

            listOfVoiceChannelCreates.Add(roomName, ev);
        }
        public void CloseVoiceChannel(SimulationEvent e)
        {
            if (listOfVoiceChannelCreates.ContainsKey(((StringValue)e["ChannelName"]).value))
            {
                listOfVoiceChannelCreates.Remove(((StringValue)e["ChannelName"]).value);
            }
        }

        public void SendAvailableDMEvent(string targetTerminalID)
        {
            List<string> available = new List<string>();
            List<string> all = new List<string>();

            foreach (string dm in allDMs.Keys)
            {
                if (allDMs[dm].availability == DecisionMaker.Availability.AVAILABLE)
                {
                    available.Add(dm);
                }

                all.Add(dm);
            }

            SimulationEvent availEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "HandshakeAvailablePlayers");
            ((StringValue)availEvent["TargetTerminalID"]).value = targetTerminalID;
            ((StringListValue)availEvent["AvailablePlayers"]).strings = available;
            //((StringListValue)availEvent["TakenPlayers"]).strings = taken;
            //Remove me once client gui is caught up//
            ((StringListValue)availEvent["Players"]).strings = all;
            server.PutEvent(availEvent);
            //--------------------------------------//
            /*

            //foreach (string s in availableDMs)
            lock (dmLock)
            {
                foreach (string s in allDMs.Keys)
                {
                    if (!dmsIsReady.ContainsKey(s))
                    {
                        if(allDMs[s].isHuman)
                            available.Add(s);
                    }
                }
            }

            if (available.Count != 0)
            {
                //send out avail
                SimulationEvent availEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "HandshakeAvailablePlayers");
                DataValue stringList = new StringListValue();
                ((StringListValue)stringList).strings = available;
                availEvent["Players"] = stringList;
                server.PutEvent(availEvent);
            }
             */
        }
        private void SendOutScenarioInfo(string playerID,string terminalID)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "HandshakeInitializeGUI");
            e["PlayerID"] = DataValueFactory.BuildString(playerID);
            //e["TerminalID"] = DataValueFactory.BuildString(dmToTerminalMap[playerID]);
            e["TerminalID"] = DataValueFactory.BuildString(terminalID);
            e["ScenarioInfo"] = DataValueFactory.BuildString("BASIC SCENARIO INFO **PLACEHOLDER**");
            e["ScenarioName"] = DataValueFactory.BuildString(scenarioName);
            e["ScenarioDescription"] = DataValueFactory.BuildString(scenarioDescription);
            e["MapName"] = DataValueFactory.BuildString(mapName);
            e["UTMNorthing"] = DataValueFactory.BuildDouble(northing);
            e["UTMEasting"] = DataValueFactory.BuildDouble(easting);
            e["HorizontalPixelsPerMeter"] = DataValueFactory.BuildDouble(horizMetersPerPixel);
            e["VerticalPixelsPerMeter"] = DataValueFactory.BuildDouble(vertMetersPerPixel); 
            e["PlayerBrief"] = DataValueFactory.BuildString(allDMs[playerID].briefing);
            e["IconLibrary"] = DataValueFactory.BuildString(iconLibrary);
            e["VoiceChatEnabled"] = DataValueFactory.BuildBoolean(voiceChatEnabled);
            e["VoiceChatServerName"] = DataValueFactory.BuildString(voiceChatServerName);
            e["VoiceChatServerPort"] = DataValueFactory.BuildInteger(voiceChatServerPort);
            e["VoiceChatUserPassword"] = DataValueFactory.BuildString(voiceChatPassword);
            e["IsObserver"] = DataValueFactory.BuildBoolean(allDMs[playerID].isObserver);
            e["IsForkReplay"] = DataValueFactory.BuildBoolean(isForkReplay);
            e["DefaultDisplayLabels"] = DataValueFactory.BuildString(displayLabels);
            e["DefaultDisplayTags"] = DataValueFactory.BuildString(displayTags);

            server.PutEvent(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="message"></param>
        private void SendSystemMessageToAll(string message)
        {
            //might be nice to send out a confirmation of player X's joining to the server
            //and selection of a DM.  The msg would be a text chat?
            SimulationEvent e;
            

            foreach (string playerID in allDMs.Keys)
            {
                if (allDMs[playerID].availability != DecisionMaker.Availability.AVAILABLE)
                {
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SystemMessage");
                    e["Message"] = DataValueFactory.BuildString(message);
                    e["TextColor"] = DataValueFactory.BuildInteger(System.Drawing.Color.Black.ToArgb());
                    e["PlayerID"] = DataValueFactory.BuildString(playerID);
                    server.PutEvent(e);
                }
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="message"></param>
        private void SendSystemMessageToPlayer(string playerID, string message)
        {
            //might be nice to send out a confirmation of player X's joining to the server
            //and selection of a DM.  The msg would be a text chat?
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SystemMessage");
            e["Message"] = DataValueFactory.BuildString(message);
            e["PlayerID"] = DataValueFactory.BuildString(playerID);
            e["TextColor"] = DataValueFactory.BuildInteger(System.Drawing.Color.Red.ToArgb());
            server.PutEvent(e);
        }
        /// <summary>
        /// Gets a DMs availability.  True if they've confirmed connection, false if they have no confirmed.
        /// Will return null if the DM is not taken yet.
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public string GetDMsAvail(string dm)
        {
            if (allDMs.ContainsKey(dm))
            {
                switch (allDMs[dm].availability)
                {
                    case DecisionMaker.Availability.AVAILABLE:
                        return "Available";
                    case DecisionMaker.Availability.TAKEN:
                        return "Taken";
                    case DecisionMaker.Availability.READY:
                        return "Ready";
                }
            }
            return string.Empty;
        }

        public void SetAssetTransferFlag(SimulationEvent e)
        {
            enableAssetTransfers = ((BooleanValue)e["EnableAssetTransfer"]).value;
        }

    }
}
