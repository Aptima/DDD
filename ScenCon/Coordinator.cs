using System;

using System.IO;
using System.Xml;
using Aptima.Asim.DDD.CommonComponents;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.TextChatServer;
using Aptima.Asim.DDD.CommonComponents.HandshakeManager;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using System.Windows.Forms;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
using System.Diagnostics;

namespace Aptima.Asim.DDD.ScenarioController
{


    public class Coordinator
    {

        private static Boolean readyToTick = false;
        public static void ReadyToStart()
        {
            readyToTick = true;
        }
        private static string scenarioFile;
        private static string schemaFile;
        private static string replayFile;
        //private static string hostname;
        //private static string portString;
        private static string simModelName;
        private static string contextControl;//*NETWORK"
        private static string updateIncr;
        private static string lowerLevel;//GUI
        private static List<string> loggingTypes;
        private static string debugFile;
        private static SimulationEventDistributor distributor;
        private static double speedFactor = 1.0;
        private static bool encounteredError = false;
        private static object errorLock = new object();
        private static Metronome tickController = Metronome.GetInstance();
        public static bool EncounteredError()
        {
            lock (errorLock)
            {
                return encounteredError;
            }
        }
        private static void SetError(bool value)
        {
            lock (errorLock)
            {
                encounteredError = value;
            }
        }

        private static int timeSlice = 0;
        const int TimeSliceIncrement = 10;


        Thread coordinatorThread;
        private static Thread eCommReceiver;
        //private static Thread textChatServerThread;
        //private static Thread handshakeManagerThread;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scenarioFile"></param>
        /// <param name="schemaFile"></param>
        /// <param name="hostname"></param>
        /// <param name="portString"></param>
        /// <param name="simModelName"></param>
        /// <param name="contextControl"></param>
        /// <param name="updateIncr"></param>
        /// <param name="lowerLevel"></param>
        /// <param name="outputType">Arg0 = Debug, Arg1 = Test</param>
        public Coordinator(
            string scenarioFile,
            string schemaFile,
            string replayFile,
            ref SimulationEventDistributor distributor,
            //string hostname,
            //string portString,
            string simModelName,
            string contextControl,//*NETWORK"
            string updateIncr,
            string lowerLevel,
            List<string> logTypes,
            string debugFile
            )// Needed(?) only so can start a thread
        {
            SetError(false);
            coordinatorThread = null;
            eCommReceiver = null;
            //textChatServerThread = null;
            //handshakeManagerThread = null;
            Coordinator.scenarioFile = scenarioFile;
            Coordinator.schemaFile = schemaFile;
            Coordinator.replayFile = replayFile;
            //Coordinator.hostname = hostname;
            //Coordinator.portString = portString;
            Coordinator.distributor = distributor;
            Coordinator.simModelName = simModelName;
            Coordinator.contextControl = contextControl;//*NETWORK"
            Coordinator.updateIncr = updateIncr;
            Coordinator.lowerLevel = lowerLevel;//GUI
            Coordinator.loggingTypes = logTypes;
            Coordinator.debugFile = debugFile;
        }



        public void Start()
        {
            coordinatorThread = new Thread(new ThreadStart(CoordinateNow));
            //coordinatorThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", true);//for foreign deployments
            coordinatorThread.Start();
        }
        public void Stop()
        {
            //EventCommunicator.SendSimStopEvent();
            //handshakeManagerThread.Abort();
            if (coordinatorThread != null)
            {
                coordinatorThread.Abort();
            }
            if (eCommReceiver != null)
            {
                eCommReceiver.Abort();
            }
            //textChatServerThread.Abort();
            HappeningList.Happenings.Clear();
            TimerQueueClass.Clear();
            IncomingList.Clear();
            Genealogy.Clear();
            StatesForUnits.Clear();
            UnitFacts.CurrentUnitStates.Clear();
            UnitFacts.ClearDMTables();
            UnitFacts.Data.Clear();
            ChatRooms.DropAllRooms();
            VoiceChannels.DropAllChannels();
            WhiteboardRooms.DropAllRooms();
            TimerControl((int)TimerControls.Reset);
    //        ScenarioToQueues.Reset();
             WeaponTable.Clear();
            NetworkTable.Clear();
            SubplatformRecords.Clear();
            Engrams.Clear();
            Scores.Clear();
            DebugLogger.StopLogging();
            NameLists.Clear();
            DecisionMakerType.ClearDMTable();
            SpeciesType.ClearSpeciesTable();
            Metronome.GetInstance().CleanupMetronome();
        }

        public void CoordinateNow()
        {
            try
            {
                Coordinate(scenarioFile,
                schemaFile,
                ref distributor,
                    //hostname,
                    //portString,
                simModelName,
                contextControl,//*NETWORK"
                updateIncr,
                lowerLevel, //GUI
                loggingTypes,
                debugFile
                );
            }
            catch (ThreadAbortException) { }
            catch (Exception exc)
            {
                if (exc.Message.StartsWith("User Cancelled"))
                {//This means a missing map or icon library, and the user wanted to stop the server.  Do not write to error log, just stop the server. 
                    SetError(true);
                    this.Stop(); //need to clear out event queues.
                    return;
                }
                MessageBox.Show("An error " + exc.Message + "  has occurred in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error."); 
                ErrorLog.Write(exc.ToString() + "\n");

                //Application.Exit();//this method of exiting lead to server hangs
                //AD: This method should re-set the server.
                SetError(true);
                this.Stop(); //need to clear out event queues.
                return;
                //throw new Exception();
            }

        }
        private static int updateIncrement;
        public static int UpdateIncrement
        {
            get { return updateIncrement; }
        }

        public static DebugLogger debugLogger;
        private static Boolean pause = false;
        public enum TimerControls { Resume = 0, Pause = 1, Reset = 2 };

        public static void SetGameSpeed(double speed)
        {
            speedFactor = speed;
            tickController.SpeedFactor = speed;
        }

        public static void TimerControl(int command)
        {
            switch (command)
            {
                case (int)TimerControls.Resume:
                    tickController.Resume();
                    EventCommunicator.SendResumeScenarioEvent();
                    pause = false;
                    break;
                case (int)TimerControls.Pause:
                    tickController.Pause();
                    EventCommunicator.SendPauseScenarioEvent();
                    pause = true;
                    break;
                case (int)TimerControls.Reset:
                    tickController.Reset();
                    TimerTicker.Timer = 0;
                    break;
                default:
                    ApplicationException e = new ApplicationException("Invalid command value " + command.ToString() + " sent to timer");
                    throw (e);

            }
        }


        public static void Coordinate(
            string scenarioFile,
            string schemaFile,
            ref SimulationEventDistributor distributor,
            //string hostname,
            //string portString,
            string simModelName,
            string contextControl,//*NETWORK"
            string updateIncr,
            string lowerLevel, //GUI
            List<string> logTypes,
            string debugFile
            )
        {
            /// <summary>


            debugLogger = new DebugLogger();
            //DateTime dt = DateTime.Now;
            if (!Directory.Exists(debugFile.Remove(debugFile.LastIndexOf("\\"))))
            {
                Directory.CreateDirectory(debugFile.Remove(debugFile.LastIndexOf("\\")));
            }

            DebugLogger.SetDebugStyleFile(debugFile);
            //DebugLogger.SetDebugStyle(DebugLogger.DebugStyleValues.FileReporting);
            DebugLogger.SetLoggingType("general", true);
            foreach (string s in logTypes)
            {
                DebugLogger.SetLoggingType(s, true);
                debugLogger.Writeline("Coordinator", s + " is being recorded.", "general");
            }

            // DebugLogger.SetLoggingType("all", false);


            debugLogger.Writeline("Coordinator", "Hello", "general");

            //int port = int.Parse(portString);
            SimulationModelReader smr = new SimulationModelReader();
            SimulationModelInfo simModelInfo = smr.readModel(simModelName);

            if (distributor == null)
            {
                distributor = new SimulationEventDistributor(ref simModelInfo);
            }

            //SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            //SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            updateIncrement = simModelInfo.GetUpdateFrequency();
            int enteredIncrement = Int32.Parse(updateIncr);
            if (enteredIncrement > 0)
            {
                updateIncrement = enteredIncrement;
            }
            tickController.UpdateIncrement = updateIncrement;
            try
            {
                new ScenarioToQueues(scenarioFile, schemaFile);
                new ForkReplayToQueues(replayFile,simModelInfo);
            }
            catch (System.Exception f)
            {
                if (f.Message.StartsWith("User Cancelled"))
                {//This means a missing map or icon library, and the user wanted to stop the server.  Do not write to error log, just stop the server. 
                    throw f;
                }
                throw new ApplicationException("Failure in ScenarioToQueues: " + f.Message);
            }

            //NetworkClient c = new NetworkClient();
            SimulationEventDistributorClient distClient = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref distClient);
            try
            {
                //c.Connect(hostname, port);

                EventCommunicator eventCommunicator = new EventCommunicator(ref distClient, simModelName);
                eCommReceiver = new Thread(new ThreadStart(eventCommunicator.WaitForEvents));
                eCommReceiver.CurrentCulture = new System.Globalization.CultureInfo("en-US", true);
                eCommReceiver.Start();


            }
            catch (System.Exception e)
            {
                //Coordinator.debugLogger.WriteLine("Unable to  connect");
                Coordinator.debugLogger.Writeline("Coordinator", "Error in startup: System message: "+e.Message, "general");
                MessageBox.Show("Startup error: " + e.Message);
                Application.Exit();
            }
            EventCommunicator.SendSimStartEvent();

            //dist.RegisterClient(ref cc);


            /*
             * Temporary event  watcher to allow for insertion of events from below
             */
            if (lowerLevel != "GUI")
            {
                //NetworkClient client = new NetworkClient();
                //client.Connect(hostname, port);
                /* 
                       // Tickwatcher is used only to simulate events from lower levels
                              TickWatcher sink = new TickWatcher(client, simModelName);
                              ThreadStart stub = new ThreadStart(sink.TickEventGetter);
                              Thread stubThread = new Thread(stub);
                              stubThread.Start();
                */
            }
            TimerQueueClass.SendImmediates();

            if ("NETWORK" == contextControl)
            {
                while (!readyToTick)
                {
                    Thread.Sleep(1000);
                }
                tickController.SetCallback(new Metronome.SendTimeTick(SendTimeTick));
                tickController.Start();
                while (true)
                {
                    while (pause)
                    {
                        Thread.Sleep(1000);
                    }
                    timeSlice = (timeSlice + 1) % TimeSliceIncrement;

                    //next tick will have 100ms between calls, will not handle sending time ticks, just recent incoming events.
                    //a callback will handle sending time ticks

                    TimerTicker.NextTimeSlice();
                    Thread.Sleep((int)((updateIncrement / TimeSliceIncrement) / speedFactor));
                }
            }

            Coordinator.debugLogger.Writeline("Coordinator", "The End", "general");

        }

        public static void SendTimeTick(object timeObj)
        {
            /* 2. Send the tick */
            //  timer += updateIncrement;
            int time = (int)timeObj;
            int timer = time / 1000;
            int secs = timer % 60;
            int mins = (timer / 60) % 60;
            int hours = timer / 3600;
            const string name_Space = TimerTicker.name_Space;
            DateTime currTime = new DateTime(1, 1, 1, hours, mins, secs);
            string simulationTime = String.Format("{0:HH:mm:ss}", currTime);
            EventCommunicator.SendEvent(new TickEventType(time, simulationTime));
            //   current = DateTime.Now;
            //   TimeSpan ts = current - last;
            //   Console.WriteLine("timeslice: {0:+hh.mm.ss.ffffzz}", ts.ToString());
            //last = current;

            /* 3. Pull the events for this tick off the queue and handle them */
            List<ScenarioEventType> events = TimerQueueClass.RetrieveEvents(timer);
            if (events != null)
            {
                for (int v = 0; v < events.Count; v++)
                {
                    Coordinator.debugLogger.Writeline("Timer", "Time: " + time.ToString()
                        + "  ID: " + events[v].UnitID.ToString() +
                        " Event: " + events[v].GetType().ToString(), "test");
                    // track current state
                    if (UnitFacts.AnyDead(events[v].AllUnits))
                    {// skip an event that refers to a dead unit
                        continue;
                    }
                    string thisUnit = events[v].UnitID;
                    string thisTarget = "";
                    // Only a few outgoing events require two substitutions
                    switch (events[v].GetType().ToString())
                    {
                        case name_Space + "AttackObjectEvent":
                            thisTarget = ((AttackObjectEvent)events[v]).TargetObjectID;
                            break;

                        case name_Space + "LaunchEventType":
                            thisTarget = ((LaunchEventType)events[v]).Child;
                            break;
                        case name_Space + "WeaponLaunchEventType":
                            thisTarget = ((WeaponLaunchEventType)events[v]).Child;
                            break;
                        case name_Space + "SubplatformLaunchType":
                            thisTarget = ((SubplatformLaunchType)events[v]).ParentUnit;
                            break;
                        case name_Space + "SubplatformDockType":
                            thisTarget = ((SubplatformDockType)events[v]).ParentUnit;
                            break;
                        default:
                            break;

                    }

                    if (!events[v].EngramValid(thisUnit, thisTarget))
                    {
                        Coordinator.debugLogger.Writeline("Timer", "Time: Command" + events[v].GetType().ToString() + " ignored due to value of " + events[v].Range.Name, "test");
                    }
                    else
                    {
                        switch (events[v].GetType().ToString())
                        {
                            /* SInce the active region update requires both activity and visibility,
                             * must find current values of non-updated field just before sending event
                             */
                            case name_Space + "ActiveRegionActivityUpdateType":
                                ActiveRegionStateType currentStateForVisibility = (ActiveRegionStateType)(NameLists.activeRegionNames.ContainedData[((ActiveRegionActivityUpdateType)(events[v])).UnitID]);
                                currentStateForVisibility.IsActive = ((ActiveRegionActivityUpdateType)(events[v])).IsActive;
                                NameLists.activeRegionNames.ContainedData[((ActiveRegionActivityUpdateType)(events[v])).UnitID] = currentStateForVisibility;
                                ActiveRegionUpdateType activityUpdate = new ActiveRegionUpdateType(((ActiveRegionActivityUpdateType)(events[v])).UnitID, ((ActiveRegionActivityUpdateType)(events[v])).Time, currentStateForVisibility);
                                EventCommunicator.SendEvent(activityUpdate);
                                break;

                            case name_Space + "ActiveRegionVisibilityUpdateType":
                                ActiveRegionStateType currentStateForActivity = (ActiveRegionStateType)(NameLists.activeRegionNames.ContainedData[((ActiveRegionVisibilityUpdateType)(events[v])).UnitID]);
                                currentStateForActivity.IsVisible = ((ActiveRegionVisibilityUpdateType)(events[v])).IsVisible;
                                NameLists.activeRegionNames.ContainedData[((ActiveRegionVisibilityUpdateType)(events[v])).UnitID] = currentStateForActivity;
                                ActiveRegionUpdateType visibilityUpdate = new ActiveRegionUpdateType(((ActiveRegionVisibilityUpdateType)(events[v])).UnitID, ((ActiveRegionVisibilityUpdateType)(events[v])).Time, currentStateForActivity);
                                EventCommunicator.SendEvent(visibilityUpdate);
                                break;

                            case name_Space + "CloseChatRoomType":
                                if (ChatRooms.DropChatRoom(((CloseChatRoomType)(events[v])).Room))
                                    EventCommunicator.SendEvent(events[v]);
                                break;
                            case name_Space + "CloseVoiceChannelType":
                                if (VoiceChannels.DropVoiceChannel(((CloseVoiceChannelType)(events[v])).Channel))
                                    EventCommunicator.SendEvent(events[v]);
                                break;
                            case name_Space + "Reveal_EventType":
                                UnitFacts.CurrentUnitStates[events[v].UnitID] = ((Reveal_EventType)events[v]).InitialState;
                                EventCommunicator.SendEvent(events[v]);
                                break;
                            case name_Space + "StateChangeEvent":
                                string currentState = UnitFacts.CurrentUnitStates[events[v].UnitID];
                                if (("Dead" != currentState)
                                    && (!((StateChangeEvent)events[v]).IsException(currentState)))
                                {
                                    if ((((StateChangeEvent)events[v]).HasPrecursors())
                                        && (((StateChangeEvent)events[v]).IsPrecursor(currentState)))
                                    {
                                        EventCommunicator.SendEvent(events[v]);
                                    }
                                    else if (!((StateChangeEvent)events[v]).HasPrecursors())
                                    {
                                        EventCommunicator.SendEvent(events[v]);
                                    }
                                }
                                break;
                            case name_Space + "TransferEvent":
                                    if (UnitFacts.ChangeDM(events[v].UnitID,
                                        /*   ((TransferEvent)events[v]).From, */((TransferEvent)events[v]).To))
                                    {
                                        EventCommunicator.SendEvent(events[v]);
                                    }// and if not? Throw exception??
                                    else
                                    {
                                        Coordinator.debugLogger.LogError("Timed Queue Manager", "Transfer of Unit " + events[v].UnitID +
                                            " from " + ((TransferEvent)events[v]).From + " failed.");

                                    }
                                    break;
                                case name_Space + "SubplatformLaunchType":
                                    SubplatformLaunchType alias = ((SubplatformLaunchType)events[v]);
                                    List<string> nowDocked = SubplatformRecords.GetDocked(alias.ParentUnit);
                                    if (nowDocked.Contains(alias.UnitID))
                                    {
                                        SubplatformRecords.UnDock(alias.ParentUnit, alias.UnitID);
                                        EventCommunicator.SendEvent(events[v]);
                                    }
                                    break;
                                case name_Space + "SubplatformDockType":
                                    SubplatformDockType dock = ((SubplatformDockType)events[v]);
                                    string adopterID = dock.ParentUnit;
                               
                                    if (SubplatformRecords.CountDockedOfSpecies(adopterID, UnitFacts.GetSpecies(dock.UnitID)) <= SpeciesType.GetSpecies(UnitFacts.GetSpecies(adopterID)).GetSubplatformCapacity(UnitFacts.GetSpecies(dock.UnitID)))
                                    {
                                        if (SubplatformRecords.RecordDocking(thisTarget, dock.UnitID) == true)
                                        {
                                            EventCommunicator.SendEvent(events[v]);
                                        }
                                        else
                                        {
                                            string someError = "This failed because the subplatform wasn't able to be docked to this platform";
                                        }
                                    }
                                    break;
                            //SubplatformLaunchType is a response to a launch request,
                            //LaunchEventType was intended to be from the scenario. This needs to be rethought.                                   
                                case name_Space + "LaunchEventType":
                                    // Note -- it isn't an error if the thing isn't docked any more

                                    LaunchEventType launch = (LaunchEventType)events[v];
                                    // This comes from scenario so we only check if there is something to launch
                                    if ("" != launch.Child)
                                    {

                                        List<string> dockedNow = SubplatformRecords.GetDocked(launch.UnitID);
                                        if (dockedNow.Contains(launch.Child))
                                        {
                                            SubplatformRecords.UnDock(launch.UnitID, launch.Child);

                                            ///???        Subplatforms.GetDocked(launch.UnitID);
                                            // not in sim model      launch.Genus = Genealogy.GetGenus(launch.Child);
                                            SubplatformRecords.UnDock(launch.UnitID, launch.Child);
                                            EventCommunicator.SendEvent(launch);

                                        }
                                    }

                                    break;
                                case name_Space + "WeaponLaunchEventType":
                                    // Note -- it isn't an error if the thing isn't docked any more

                                    WeaponLaunchEventType launchWeapon = (WeaponLaunchEventType)events[v];
                                    // This comes from scenario so we only check if there is something to launch
                                    if ("" != launchWeapon.Child)
                                    {

                                        List<string> dockedNow = SubplatformRecords.GetDocked(launchWeapon.UnitID);
                                        if (dockedNow.Contains(launchWeapon.Child))
                                        {
                                            SubplatformRecords.UnDock(launchWeapon.UnitID, launchWeapon.Child);
                                            SubplatformRecords.UnDock(launchWeapon.UnitID, launchWeapon.Child);
                                            EventCommunicator.SendEvent(launchWeapon);

                                        }
                                    }

                                    break;
                            case name_Space + "EngramSettingType":
                                //This is the inital value so it was set in the table at parset time
                                EventCommunicator.SendEvent(events[v]);
                                break;

                            case name_Space + "ChangeEngramType":
                                ChangeEngramType newValue = ((ChangeEngramType)events[v]);
                                Engrams.Set(newValue.Name, newValue.UnitID, newValue.EngramValue);
                                EventCommunicator.SendEvent(new EngramSettingType(newValue.Name, newValue.UnitID, newValue.EngramValue, Engrams.GetType(newValue.Name)));
                                break;
                            case name_Space + "RemoveEngramEvent":
                                Engrams.Remove(((RemoveEngramEvent)events[v]).Name);
                                break;
                            case name_Space + "OpenChatRoomType":
                                //Requests from below are handled by EventCommunicator
                                //This processing is for commands in script -- ??
                                string chatFailureMessage = "";
                                OpenChatRoomType ocr = ((OpenChatRoomType)events[v]);
                                if (("EXP" != ocr.Owner) && !UnitFacts.IsDM(ocr.Owner))
                                    chatFailureMessage = "The name '" + ocr.Owner + "' is not a valid decision maker.";
                                else
                                {
                                    for (int i = 0; i < ocr.Members.Count; i++)
                                    {
                                        DecisionMakerType DMi = DecisionMakerType.GetDM(ocr.Members[i]);
                                        for (int mbr = 1 + i; mbr < ocr.Members.Count; mbr++)
                                        {
                                            if (!DMi.CanChatWith(ocr.Members[mbr]))
                                            {
                                                chatFailureMessage = "Decison Makers '" + DMi.Identifier + "' cannot be in a chat room with '" + ocr.Members[mbr] + "'";
                                                break;
                                            }
                                            if ("" != chatFailureMessage)
                                                break;
                                        }

                                    }

                                }
                                if ("" == chatFailureMessage)
                                {
                                    chatFailureMessage = ChatRooms.AddRoom(ocr);
                                    EventCommunicator.SendEvent(ocr);
                                }
                                if ("" != chatFailureMessage)
                                {
                                    // DON'T throw new ApplicationException("Could not create chatroom '"+ocr.Range+"': "+chatFailureMessage);
                                    // send a system message instead
                                }
                                break;
                            case name_Space + "SendChatMessageType":
                                // Script induced. Only need to check if there is such a room
                                if (ChatRooms.IsRoom(((SendChatMessageType)(events[v])).RoomName))
                                    EventCommunicator.SendEvent(events[v]);
                                break;
                            case name_Space + "OpenWhiteboardRoomType":
                                //Requests from below are handled by EventCommunicator
                                //This processing is for commands in script -- ??
                                string whiteboardFailureMessage = "";
                                OpenWhiteboardRoomType owr = ((OpenWhiteboardRoomType)events[v]);
                                if (("EXP" != owr.Owner) && !UnitFacts.IsDM(owr.Owner))
                                    whiteboardFailureMessage = "The name '" + owr.Owner + "' is not a valid decision maker.";
                                else
                                {
                                    for (int i = 0; i < owr.Members.Count; i++)
                                    {
                                        DecisionMakerType DMi = DecisionMakerType.GetDM(owr.Members[i]);
                                        for (int mbr = 1 + i; mbr < owr.Members.Count; mbr++)
                                        {
                                            if (!DMi.CanWhiteboardWith(owr.Members[mbr]))
                                            {
                                                whiteboardFailureMessage = "Decison Makers '" + DMi.Identifier + "' cannot be in a whiteboard room with '" + owr.Members[mbr] + "'";
                                                break;
                                            }
                                            if ("" != whiteboardFailureMessage)
                                                break;
                                        }

                                    }

                                }
                                if ("" == whiteboardFailureMessage)
                                    whiteboardFailureMessage = WhiteboardRooms.AddRoom(owr);
                                if ("" != whiteboardFailureMessage)
                                {
                                    // DON'T throw new ApplicationException("Could not create whiteboardroom '"+owr.Range+"': "+whiteboardFailureMessage);
                                    // send a system message instead
                                }
                                break;
                            case name_Space + "OpenVoiceChannelType":
                                //Requests from below would be/are handled by EventCommunicator
                                //This processing is for commands in script -- ??
                                string voiceFailureMessage = "";
                                OpenVoiceChannelType ovct = ((OpenVoiceChannelType)events[v]);
                                //if (("EXP" != ovct.Owner) && !UnitFacts.IsDM(ovct.Owner))
                                //{
                                //    voiceFailureMessage = "Open Voice Channel: The name '" + ovct.Owner + "' is not a valid decision maker.";
                                //    ErrorLog.Write(voiceFailureMessage);
                                //}
                                //else    
                                //{
                                voiceFailureMessage = VoiceChannels.AddChannel(ovct);
                                if ("" != voiceFailureMessage)
                                    ErrorLog.Write(voiceFailureMessage);
                                else
                                {
                                    //ovct.InitialMembers = DecisionMakerType.GetVoiceChannelMembers(ovct.Channel); //TODO: AD: Is this even NECESSARY?
                                    EventCommunicator.SendEvent(ovct);
                                }
                                //}
                                break;

                            case name_Space + "FlushEvents":
                                //first drop all timer events involving this unit
                                TimerQueueClass.FlushOneUnit(((FlushEvents)events[v]).UnitID);
                                // Then all events on happenin queue about this unit
                                for (int h = 0; h < HappeningList.Happenings.Count; h++)
                                {// Find first happening event that keys off this state change
                                    HappeningCompletionType incident = HappeningList.Happenings[h];
                                    if (incident.UnitID == ((FlushEvents)events[v]).UnitID)
                                    {
                                        HappeningList.Happenings.RemoveAt(h);
                                    }
                                    else //  examine all the DoThisLists
                                    {
                                        List<int> removals = new List<int>();
                                        for (int d = 0; d < incident.DoThisList.Count; d++)
                                        {
                                            if (incident.DoThisList[d].Involves(((FlushEvents)events[v]).UnitID))
                                            {
                                                removals.Add(d);
                                            }
                                            //then remove from back to front to preserve indices
                                            for (int r = removals.Count - 1; r >= 0; r--)
                                            {
                                                incident.DoThisList.RemoveAt(r);
                                            }
                                        }

                                    }


                                }
                                break;
                            case name_Space + "ForkReplayEventType":
                                EventCommunicator.SendEvent(events[v]);
                                break;
                            default:
                                EventCommunicator.SendEvent(events[v]);
                                break;
                        }

                    }
                }
                TimerQueueClass.DropNode(time);

            }


        }

    }

}


