using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;
using ConaitoLib;

namespace Aptima.Asim.DDD.CommonComponents.VoiceServer
{
    public class VoiceServer
    {
        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        //private static int currentTick = 0;
        //private static string timeString = "00:00:00";
        private static bool isRunning;
        private static bool isLoggedIn;
        private static Dictionary<string, List<string>> roomMembership;
        private static Dictionary<string, int> channelIDMap;      //Channel path and Channel ID
        private static Dictionary<string, int> userSpeachStartTime;      //The DDD time the user started speaking
        private static Dictionary<string, string> lastPlayedVoiceFile;      //The last played voice file to a user from a particular user
        private static Dictionary<int, int> userChannelMap;     // Maps userID (key) to the channel that user is on (value)
        private static Dictionary<string, List<int>> channelUserList;    // Lists the current users (by id) for a channel
        private static string voiceServerHostName = "localhost";
        private static int voiceServerHostPort = 10300;
        private static int voiceServerUDPHostPort = 10301;
        private static string voiceServerAdminUsername = "admin";
        private static string voiceServerAdminPass = "admin";
        private static string rootChannelName = "DDD";
        private static int maxTries = 100;  //maximum number of tries to wait for connection or login
        private static VoiceRecorder voiceRecorder;
        private Dictionary<int, Thread> voiceRecorderThread; //holds all of the voice recorder threads by channel number
        private static bool recordChannels = false;
        private static string voiceServerAudioFilePath = "";
        private static bool isReplay = false;
        private static double replaySpeed = 0.0;
        private static Process conaitoServer = null;

        //private System.Windows.Forms.Timer pingTimer;   //to ping the voice server so it doesn't hang up on us.

        //If you get an error here's is because the COM control is not registered.
        //Use "regsvr32.exe EvoVoIPAdmin.dll" to register the COM control.
        //Check References in the Solution Explorer and add the COM control named
        //"EvoVoIP Admin 1.0 Type Library"
        private static ConaitoLib.EvoVoIPAdmin evoadmin;

        // Here's the VoIP Evo voice client
		//If you get a compiler error here it's because the COM control is not
		//registered. Run "regsvr32.exe EvoVoIP.dll" in the COM-folder.
		//private static ConaitoLib.EvoVoIP voipclient;

        public VoiceServer(string simModelPath, ref SimulationEventDistributor distributor, DateTime time, bool bIsReplay, double dbReplaySpeed)
        {
            simModelName = simModelPath;
            simModelInfo = smr.readModel(simModelName);
            isRunning = false;
            isLoggedIn = false;
            roomMembership = new Dictionary<string, List<string>>();
            channelIDMap = new Dictionary<string, int>();
            userChannelMap = new Dictionary<int, int>();
            channelUserList = new Dictionary<string, List<int>>();
            userSpeachStartTime = new Dictionary<string, int>();
            lastPlayedVoiceFile = new Dictionary<string, string>();
            server = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref server);
            isReplay = bIsReplay;
            replaySpeed = dbReplaySpeed;
            
            // Start the voice server
            if (!StartConaitoVoiceServer())
            {
                StopConaitoVoiceServer();
                String errorMessage1 = "Failed to start Conaito Voice server application";
                ErrorLog.Write(errorMessage1 + "\n");
                throw new Exception("Failed to start Conaito Voice server application");
            }

            //connect to the Conaito server COM object
            //Instantiate the COM control
            evoadmin = new ConaitoLib.EvoVoIPAdminClass();
            //evoadmin.CloseVoIP();           //this is just in case we were here before
            if (!evoadmin.InitVoIP())
            {
                String errorMessage1 = "Failed to initialize EvoVoIP";
                //System.Windows.Forms.MessageBox.Show(errorMessage1);
                ErrorLog.Write(errorMessage1 + "\n");
                throw new Exception("Failed to initialize the Voice Server");
            }

            //Register some of the Conaito server events we need to know about
            evoadmin.OnCommandError += new ConaitoLib.IEvoVoIPAdminEvents_OnCommandErrorEventHandler(this.evoadmin_OnCommandError);
            evoadmin.OnConnectSuccess += new ConaitoLib.IEvoVoIPAdminEvents_OnConnectSuccessEventHandler(this.evoadmin_OnConnectSuccess);
            evoadmin.OnConnectFailed += new ConaitoLib.IEvoVoIPAdminEvents_OnConnectFailedEventHandler(this.evoadmin_OnConnectFailed);
            evoadmin.OnConnectionLost += new ConaitoLib.IEvoVoIPAdminEvents_OnConnectionLostEventHandler(this.evoadmin_OnConnectionLost);

            evoadmin.OnAccepted += new ConaitoLib.IEvoVoIPAdminEvents_OnAcceptedEventHandler(this.evoadmin_OnAccepted);

            //get the values of the voice server host name and port number from the server options file
            if (ServerOptions.VoiceServerHostname != null) {
                voiceServerHostName = ServerOptions.VoiceServerHostname;
            }
             if (ServerOptions.VoiceServerPort != 0) {
                voiceServerHostPort = ServerOptions.VoiceServerPort;
            }

             if (!evoadmin.Connect(voiceServerHostName, voiceServerHostPort, voiceServerUDPHostPort))
            {
                String errorMessage1 = "Failed to connect to the voice server";
                System.Windows.Forms.MessageBox.Show(errorMessage1);
                ErrorLog.Write(errorMessage1 + "\n");
                throw new Exception("Failed to connect to the voice server");
            }
            //wait until we get connected.
            int tries = 0;
            while (!isRunning && tries < maxTries)
            {
                isRunning = evoadmin.IsConnected();
                tries += 1;
                Thread.Sleep(100);
            }

            //we timed out, so we did not connect
            if (tries == maxTries)
            {
                String errorMessage1 = "Failed to connect to Voice Server";
                //System.Windows.Forms.MessageBox.Show(errorMessage1);
                ErrorLog.Write(errorMessage1 + "\n");
                throw new Exception("Failed to connect to the Voice Server");
            }

            //Log into the admin account of the voice server
            evoadmin.DoAdminLogin(voiceServerAdminUsername, voiceServerAdminPass);

            //wait until we get logged in.
            tries = 0;
            while (!isLoggedIn && tries < maxTries ) 
            {
                isLoggedIn = evoadmin.IsAuthorized();
                tries++;
                Thread.Sleep(100);
            }
            //we timed out, so we did not get logged in
            if (tries == maxTries)
            {
                String errorMessage1 = "Failed to log into Voice Server";
                //System.Windows.Forms.MessageBox.Show(errorMessage1);
                ErrorLog.Write(errorMessage1 + "\n");
                throw new Exception("Failed to log into the Voice Server");
            }
            // clean things up and set things up just in case we or someone else left something in the voice server
            setUpVoiceServer();

            //Register some more of the Conaito server events we need to know about
            evoadmin.OnAddChannel += new ConaitoLib.IEvoVoIPAdminEvents_OnAddChannelEventHandler(this.evoadmin_OnAddChannel);
            evoadmin.OnRemoveChannel += new ConaitoLib.IEvoVoIPAdminEvents_OnRemoveChannelEventHandler(this.evoadmin_OnRemoveChannel);
            evoadmin.OnAddUser += new ConaitoLib.IEvoVoIPAdminEvents_OnAddUserEventHandler(this.evoadmin_OnAddUser);
            evoadmin.OnRemoveUser += new ConaitoLib.IEvoVoIPAdminEvents_OnRemoveUserEventHandler(this.evoadmin_OnRemoveUser);
            evoadmin.OnAudioFileStatus += new ConaitoLib.IEvoVoIPAdminEvents_OnAudioFileStatusEventHandler(this.evoadmin_OnAudioFileStatus);

            //find out if the user wants to record the voice channels
            recordChannels = ServerOptions.EnableVoiceServerRecordings;

            if (recordChannels)
            {
                string audioFilePath;

                // Setup audio file base path
                string scenarioTitle = ServerOptions.DefaultScenarioPath.Remove(ServerOptions.DefaultScenarioPath.LastIndexOf('.'));
                scenarioTitle = scenarioTitle.Remove(0, scenarioTitle.LastIndexOf('\\') + 1);

                if ((ServerOptions.VoiceServerAudioLogDir == null) ||
                    (ServerOptions.VoiceServerAudioLogDir.Length == 0))
                {
                    string assemblyDir = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                    string drive = Path.GetPathRoot(assemblyDir);
                    audioFilePath = Path.Combine(drive, "DDDVoiceRecordings");
                }
                else
                {
                    audioFilePath = ServerOptions.VoiceServerAudioLogDir;
                }
                if (!Directory.Exists(audioFilePath))
                {
                    Directory.CreateDirectory(audioFilePath);
                }
                audioFilePath = Path.Combine(audioFilePath, scenarioTitle);

                if ((!isReplay) && (!Directory.Exists(audioFilePath)))
                {
                    try
                    {
                        Directory.CreateDirectory(audioFilePath);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (Directory.Exists(audioFilePath.ToString()))
                {
                    audioFilePath = Path.Combine(audioFilePath, String.Format("{0:yyyyMMddHHmmss}", time));

                    if ((!isReplay) && (!Directory.Exists(audioFilePath)))
                    {
                        try
                        {
                            Directory.CreateDirectory(audioFilePath);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (Directory.Exists(audioFilePath.ToString()))
                    {
                        voiceServerAudioFilePath = audioFilePath;
                    }
                }
                

                // Setup directory for saved audio files
                evoadmin.DoSubscribe((int)_Subscriptions.SUBSCRIBE_AUDIO);
            }
            
            //setup the client so we can find out when users are talking.
            //voipclient = new ConaitoLib.EvoVoIPClass();

            //register some client events
            //voipclient.OnUserTalking += new ConaitoLib.IEvoVoIPEvents_OnUserTalkingEventHandler(this.voipclient_OnUserTalking);


            //Set up the ping timer
            //pingTimer = new System.Windows.Forms.Timer();
            //this.pingTimer.Tick += new System.EventHandler(this.TimerPinger);
            //pingTimer.Interval = 1000;
            //pingTimer.Start();
        }

        public static void IsRunning(bool value)
        {
            lock (channelIDMap)
            {
                isRunning = value;
            }
        }

        public static void IsLoggedIn(bool value)
        {
            lock (channelIDMap)
            {
                isLoggedIn = value;
            }
        }

        public void StartVoiceServer()
        {

            try
            {
                List<SimulationEvent> incomingEvents = new List<SimulationEvent>();

                server.Subscribe("ExternalApp_SimStart");
                server.Subscribe("ExternalApp_SimStop");
                server.Subscribe("StopScenario");
                server.Subscribe("CreateVoiceChannel");
                server.Subscribe("CloseVoiceChannel");
                server.Subscribe("RequestStartedTalking");
                server.Subscribe("RequestStoppedTalking");
                server.Subscribe("RequestMuteUser");
                server.Subscribe("RequestUnmuteUser");
                server.Subscribe("RequestJoinVoiceChannel");
                server.Subscribe("RequestLeaveVoiceChannel");
                server.Subscribe("PlayVoiceMessage");
                server.Subscribe("PlayVoiceMessageToUser");

                if (isReplay)
                {
                    // Need to subscribe to more events if this is a replay
                    server.Subscribe("StartedTalking");
                }

                isRunning = true; //This can be set false elsewhere, and end the loop.
                while (isRunning)
                {
                    incomingEvents = server.GetEvents();

                    if (incomingEvents.Count != 0)
                    {
                        foreach (SimulationEvent se in incomingEvents)
                        {
                            switch (se.eventType)
                            {
                                case "CreateVoiceChannel":
                                    DDDEvent_CreateVoiceChannel(se);
                                    break;
                                case "CloseVoiceChannel":
                                    DDDEvent_CloseVoiceChannel(se);
                                    break;
                                case "ExternalApp_SimStart":
                                    DDDEvent_ExternalApp_SimStart(se);
                                    break;
                                case "ExternalApp_SimStop":
                                    DDDEvent_ExternalApp_SimStop(se);
                                    break;
                                case "StopScenario":
                                    DDDEvent_StopScenario(se);
                                    break;
                                case "RequestStartedTalking":
                                    DDDEvent_RequestStartedTalking(se);
                                    break;
                                case "RequestStoppedTalking":
                                    DDDEvent_RequestStoppedTalking(se);
                                    break;
                                case "RequestMuteUser":
                                    DDDEvent_RequestMuteUser(se);
                                    break;
                                case "RequestUnmuteUser":
                                    DDDEvent_RequestUnmuteUser(se);
                                    break;
                                case "RequestJoinVoiceChannel":
                                    DDDEvent_RequestJoinVoiceChannel(se);
                                    break;
                                case "RequestLeaveVoiceChannel":
                                    DDDEvent_RequestLeaveVoiceChannel(se);
                                    break;
                                case "StartedTalking":
                                    DDDEvent_StartedTalking(se);
                                    break;
                                case "PlayVoiceMessage":
                                    DDDEvent_PlayVoiceMessage(se);
                                    break;
                                case "PlayVoiceMessageToUser":
                                    DDDEvent_PlayVoiceMessageToUser(se);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(500);

                }
            }
            catch (ThreadAbortException) {
                StopConaitoVoiceServer();
            }
            catch (Exception exc)
            {
                System.Windows.Forms.MessageBox.Show("An error '" + exc.Message + "' has occurred in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                ErrorLog.Write(exc.ToString() + "\n");
                throw new Exception();
            }
        }

        private void setUpVoiceServer()
        {
            //first clean up anything that might be left from a previous run
            //remove the root channel called DDD
            int rootChannelID = evoadmin.GetChannelID("/" + rootChannelName);

            if (rootChannelID != -1)
            {
                evoadmin.DoRemoveChannel(rootChannelID);
            }
            else
            {
                ErrorLog.Write("VoiceServer: Root channel ID not found.");
            }

            //Now set up things for the DDD to run with.
            evoadmin.DoMakeChannel("/" + rootChannelName, "", "", "", 20000, 100);

        }

        private bool StartConaitoVoiceServer()
        {
            bool retVal = false;

            if (conaitoServer == null)
            {
                string assemblyDir = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                assemblyDir = Path.GetDirectoryName(assemblyDir);

                conaitoServer = new Process();
                Process[] existingEvoserver = Process.GetProcessesByName("evoserver");

                if (existingEvoserver.Length > 0)
                {
                    ErrorLog.Write("When loading Conaito Voice Server, previous Process exists.  Attaching to this server\r\n");
                    conaitoServer = existingEvoserver[0];
                    retVal = !conaitoServer.HasExited;
                }
                else
                {

                    conaitoServer.StartInfo.FileName = Path.Combine(assemblyDir, "evoserver.exe");
                    conaitoServer.StartInfo.Arguments = "-nd";

                    conaitoServer.StartInfo.UseShellExecute = false;
                    conaitoServer.StartInfo.CreateNoWindow = true;
                    conaitoServer.StartInfo.WorkingDirectory = assemblyDir;

                    retVal = conaitoServer.Start();

                    if (retVal)
                    {
                        Thread.Sleep(1000);

                        if (conaitoServer.HasExited)
                        {
                            retVal = false;
                        }
                    }
                }
            }
 
            return retVal;
        }

        private void StopConaitoVoiceServer()
        {
            if ((conaitoServer != null) && (conaitoServer.HasExited == false))
            {
                conaitoServer.Kill();
                conaitoServer = null;
            }
        }


        //Helper methods to do handle the DDD server events

        private void DDDEvent_ExternalApp_SimStart(SimulationEvent se)
        {
            String errorMessage = "ExternalApp_SimStart Event received.";
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");

        }

        private void DDDEvent_ExternalApp_SimStop(SimulationEvent se)
        {
            String errorMessage = "ExternalApp_SimStop Event received.";
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");
            evoadmin.DoQuit();                //log out of the admin account of the voice server
            evoadmin.CloseVoIP();           //close down the voice server

            // Stop the voice server
            StopConaitoVoiceServer();

            IsLoggedIn(false);
            IsRunning(false);

        }

        private void DDDEvent_StopScenario(SimulationEvent se)
        {
            String errorMessage = "StopScenario Event received.";
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");

        }
        
        
        //Create the voice channel
        private void DDDEvent_CreateVoiceChannel(SimulationEvent se)
        {
            String channelPath = ((StringValue)se["ChannelName"]).value;
            String errorMessage = "Request to Open Channel " + channelPath;
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");
            ErrorLog.Write("Request to Open Channel; Connected to voice Server as Admin: " + evoadmin.IsAuthorized());
            ErrorLog.Write("Request to Open Channel; Is connecting to voice server: " + evoadmin.IsConnecting());
            string membershipString = ((StringListValue)se["MembershipList"]).ToString();
            //we are using the channel topic to store the list of DMs that have access to the channel.
            //this will be picked up by the DDD client to decide if a user can join the channel
            evoadmin.DoMakeChannel("/" + rootChannelName + "/" + channelPath, "", membershipString, "", 20000, 100);
            List<string> membership = ((StringListValue)se["MembershipList"]).strings;
            if (!roomMembership.ContainsKey(channelPath))
            {
                roomMembership.Add(channelPath, new List<string>());
            }
            roomMembership[channelPath] = membership;


        }
        private void DDDEvent_CloseVoiceChannel(SimulationEvent se)
        {
            String channelName = ((StringValue)se["ChannelName"]).value;
            String errorMessage = "Request to Closed Channel" + channelName;
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");
            if (channelIDMap.ContainsKey(channelName))
            {
                evoadmin.DoRemoveChannel(channelIDMap[channelName]);
                roomMembership.Remove(channelName);

                channelIDMap.Remove(channelName);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Trying to close channel:" + channelName + " but it was never opened.");
            }
        }


        private void DDDEvent_RequestStartedTalking(SimulationEvent se)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "StartedTalking");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["Speaker"]).value = ((StringValue)se["Speaker"]).value;
            server.PutEvent(sendingEvent);

            // Save the time this user started talking if we have not already done so
            int time = ((IntegerValue)sendingEvent["Time"]).value;
            string username = ((StringValue)se["Speaker"]).value;
            string userAndChannelString = username + "_" + ((StringValue)se["ChannelName"]).value;

            //System.Diagnostics.Trace.WriteLine(String.Format("StartedTalking: Username={0}  Time={1}",
            //    username, time));

            if (!userSpeachStartTime.ContainsKey(userAndChannelString))
            {
                userSpeachStartTime.Add(userAndChannelString, time);
            }
            else if (userSpeachStartTime[userAndChannelString] == -1)
            {
                userSpeachStartTime[userAndChannelString] = time;
            }
        }

        private void DDDEvent_RequestStoppedTalking(SimulationEvent se)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "StoppedTalking");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["Speaker"]).value = ((StringValue)se["Speaker"]).value;
            server.PutEvent(sendingEvent);
        }

        private void DDDEvent_RequestMuteUser(SimulationEvent se)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "MuteUser");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["Speaker"]).value = ((StringValue)se["Speaker"]).value;
            server.PutEvent(sendingEvent);
        }

        private void DDDEvent_RequestUnmuteUser(SimulationEvent se)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "UnmuteUser");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["Speaker"]).value = ((StringValue)se["Speaker"]).value;
            server.PutEvent(sendingEvent);
        }

        private void DDDEvent_RequestJoinVoiceChannel( SimulationEvent se )
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "JoinVoiceChannel");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["DecisionMakerID"]).value = ((StringValue)se["DecisionMakerID"]).value;
            //((IntegerValue)sendingEvent["Time"]).value = time;
            server.PutEvent(sendingEvent); 
        }

        private void DDDEvent_RequestLeaveVoiceChannel( SimulationEvent se )
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "LeaveVoiceChannel");
            ((StringValue)sendingEvent["ChannelName"]).value = ((StringValue)se["ChannelName"]).value;
            ((StringValue)sendingEvent["DecisionMakerID"]).value = ((StringValue)se["DecisionMakerID"]).value;
            //((IntegerValue)sendingEvent["Time"]).value = time;
            server.PutEvent(sendingEvent); 
        }

        private void DDDEvent_StartedTalking(SimulationEvent se)
        {
            string channelPath = ((StringValue)se["ChannelName"]).value;
            string username = ((StringValue)se["Speaker"]).value;
            int time = ((IntegerValue)se["Time"]).value;

            // Determine if a saved sound file exists
            if ((recordChannels) && (isReplay) && (replaySpeed == 1.0) && (voiceServerAudioFilePath.Length > 0) &&
                (channelPath != null) &&
                (channelPath.Length > 0) && (string.Compare(channelPath, "DDD") != 0))
            {
                string audioFilePath = Path.Combine(voiceServerAudioFilePath, channelPath);
                string audioFileName = Path.Combine(audioFilePath, time.ToString("D8") + "_" + username +
                    ".wav");

                if (!File.Exists(audioFileName))
                {
                    return;
                }

                // Loop through the members of this channel
                lock (channelUserList)
                {
                    if (channelUserList.ContainsKey(channelPath))
                    {
                        foreach (int channelUserID in channelUserList[channelPath])
                        {
                            User channelUser = evoadmin.GetUser(channelUserID);
                            string channelUserName = channelUser.szNickName;
                            string lastPlayedFileKey = username + "_" + channelUserName;

                            if (!lastPlayedVoiceFile.ContainsKey(lastPlayedFileKey) ||
                                (string.Compare(lastPlayedVoiceFile[lastPlayedFileKey], Path.GetFileName(audioFileName), true) != 0))
                            {
                                // Stream the file
                                evoadmin.DoStreamFileToUser(channelUserID, audioFileName,
                                    channelUser.nFrequency, 5);

                                // Record that this file was streamed to the user
                                if (!lastPlayedVoiceFile.ContainsKey(lastPlayedFileKey))
                                {
                                    lastPlayedVoiceFile.Add(lastPlayedFileKey, Path.GetFileName(audioFileName));
                                }
                                else
                                {
                                    lastPlayedVoiceFile[lastPlayedFileKey] = Path.GetFileName(audioFileName);
                                }
                            }
                        }
                    }
                }



            }

        }

        private void DDDEvent_PlayVoiceMessage(SimulationEvent se)
        {
            string channelPath = ((StringValue)se["Channel"]).value;
            string audioFileName = ((StringValue)se["File"]).value;
            int time = ((IntegerValue)se["Time"]).value;

            // Only play sound during a replay if it is at normal speed
            if ((isReplay) && (replaySpeed != 1.0))
            {
                return;
            }

            // Determine if a saved sound file exists
            if ((channelPath != null) &&
                (channelPath.Length > 0) && (string.Compare(channelPath, "DDD") != 0))
            {
                if (!File.Exists(audioFileName))
                {
                    return;
                }

                // Loop through the members of this channel
                lock (channelUserList)
                {
                    if (channelUserList.ContainsKey(channelPath))
                    {
                        foreach (int channelUserID in channelUserList[channelPath])
                        {
                            User channelUser = evoadmin.GetUser(channelUserID);
                            string channelUserName = channelUser.szNickName;

                            // Stream the file
                            evoadmin.DoStreamFileToUser(channelUserID, audioFileName,
                                channelUser.nFrequency, 5);
                        }
                    }
                }
            }
        }

        private void DDDEvent_PlayVoiceMessageToUser(SimulationEvent se)
        {
            string user = ((StringValue)se["DecisionMakerID"]).value;
            string audioFileName = ((StringValue)se["File"]).value;
            int time = ((IntegerValue)se["Time"]).value;

            // Only play sound during a replay if it is at normal speed
            if ((isReplay) && (replaySpeed != 1.0))
            {
                return;
            }

            // Determine if a saved sound file exists
            if (!File.Exists(audioFileName))
            {
                return;
            }

            // Loop through the members of this channel
            lock (channelUserList)
            {
                if (channelUserList.ContainsKey("DDD"))
                {
                    foreach (int channelUserID in channelUserList["DDD"])
                    {
                        User channelUser = evoadmin.GetUser(channelUserID);
                        string channelUserName = channelUser.szNickName;

                        if (string.Compare(user, channelUserName) == 0)
                        {
                            // Stream the file
                            evoadmin.DoStreamFileToUser(channelUserID, audioFileName,
                                channelUser.nFrequency, 5);
                        }
                    }
                }
            }
        }

        // These are the Conaito events to be handled
        private void evoadmin_OnCommandError(int errorNo)
        {
            string errorMsg = evoadmin.GetErrorMessage(errorNo);
            ErrorLog.Write("Voice Server Error Number: " + errorNo + ", " + errorMsg);
        }

        private void evoadmin_OnConnectSuccess()
        {
            String errorMessage = "Successfully connected to the server";
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            ErrorLog.Write(errorMessage + "\n");
            IsRunning(true);
            //UpdateState();
        }

        private void evoadmin_OnConnectFailed()
        {
            IsRunning(false);
            IsLoggedIn(false);
            System.Windows.Forms.MessageBox.Show("Failed to connect to the voice server");
            ErrorLog.Write("Failed to connect to the voice server" + "\n");
            throw new Exception();
            //UpdateState();
        }

        private void evoadmin_OnConnectionLost()
        {
            IsRunning(false);
            IsLoggedIn(false);
            //System.Windows.Forms.MessageBox.Show("Lost connection to server");
            ErrorLog.Write("Connect lost to the voice server" + "\n");
            throw new Exception();
            //UpdateState();
        }

        private void evoadmin_OnAccepted(int userid)
        {
            IsLoggedIn(true);
            //The DoAdminLogin was successfull. We can now issue other commands.
            String errorMessage = "User login to voice server accepted.";
            ErrorLog.Write(errorMessage);
            //System.Windows.Forms.MessageBox.Show(errorMessage);
            //UpdateState();
        }

        private void evoadmin_OnAddChannel(int channelID)
        {
            //The DoAddChannel was successfull. We can now do our bookkeeping.
            //System.Windows.Forms.MessageBox.Show("Add channel sucessful.");
            String errorMessage = "Voice Server Add channel event called.";
            ErrorLog.Write(errorMessage);
            if (isLoggedIn && isRunning)
            {
                Channel channel = evoadmin.GetChannel(channelID);
                String channelPath = channel.szName;

                if ((recordChannels) && (!isReplay) && (voiceServerAudioFilePath.Length > 0) && (channelPath != null) &&
                    (channelPath.Length > 0) && (string.Compare(channelPath, "DDD") != 0))
                {
                    String audioFilePath = Path.Combine(voiceServerAudioFilePath, channelPath);

                    if (!Directory.Exists(audioFilePath))
                    {
                        try
                        {
                            Directory.CreateDirectory(audioFilePath);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (Directory.Exists(audioFilePath.ToString()))
                    {
                        evoadmin.SetChannelAudioFolder(channelID, audioFilePath);
                    }
                }

                channelIDMap.Add(channelPath, channelID);

                lock (channelUserList)
                {
                    if (!channelUserList.ContainsKey(channelPath))
                    {
                        channelUserList.Add(channelPath, new List<int>());
                    }
                }

            }
            else
            {
                ErrorLog.Write("Reached voice server add channel handler, but not logged into voice server");
            }
        }

        private void evoadmin_OnRemoveChannel(int channelID)
        {
            //The DoRemoveChannel was successfull. We can now do our bookkeeping.
            //System.Windows.Forms.MessageBox.Show("Remove channel sucessful.");
            String errorMessage = "Voice Server remove channel event called.";
            ErrorLog.Write(errorMessage);
            Channel channel = evoadmin.GetChannel(channelID);
            String channelPath = channel.szName;
            channelIDMap.Remove(channelPath);

            lock (channelUserList)
            {
                if (channelUserList.ContainsKey(channelPath))
                {
                    channelUserList.Remove(channelPath);
                }
            }

        }
        private void evoadmin_OnAddUser(int userID)
        {
            int channelID;
            User user = evoadmin.GetUser(userID);
            string username = user.szNickName;
            channelID = user.nChannelID;
            Channel channel = evoadmin.GetChannel(channelID);
            string channelName = channel.szName;

            lock (userChannelMap)
            {
                if (!userChannelMap.ContainsKey(userID))
                {
                    userChannelMap.Add(userID, channelID);
                }
                else
                {
                    userChannelMap[userID] = channelID;
                }
            }

            lock (channelUserList)
            {
                if (channelUserList.ContainsKey(channelName))
                {
                    channelUserList[channelName].Add(userID);
                }
            }

            ErrorLog.Write("User ID: " + userID + ", joined channelID: " + channelID);
            //issue the DDD server event that a user has joined the channel.
        }

        private void evoadmin_OnRemoveUser(int userID)
        {
            int channelID;
            User user = evoadmin.GetUser(userID);
            string username = user.szNickName;
            channelID = user.nChannelID;
            Channel channel = evoadmin.GetChannel(channelID);
            string channelName = channel.szName;


            lock (userChannelMap)
            {
                if (!userChannelMap.ContainsKey(userID))
                {
                    ErrorLog.Write("User ID: " + userID + ", tried to be removed from non-existent channel: " + channelID);
                }
                else
                {
                    userChannelMap.Remove(userID);
                }
            }

            lock (channelUserList)
            {
                if (channelUserList.ContainsKey(channelName))
                {
                    if (channelUserList[channelName].Contains(userID))
                    {
                        channelUserList[channelName].Remove(userID);
                    }
                }
            }

            ErrorLog.Write("User ID: " + userID + ", removed from channelID: " + channelID);
            //Issue the DDD server event that the user was removed from the channel 
        }

        private void evoadmin_OnAudioFileStatus(int nUserID, _AudioFileStatus audioFileStatus,
            string szFileName, int nDuration)
        {
            User user = evoadmin.GetUser(nUserID);
            string username = user.szNickName;

            //System.Diagnostics.Trace.WriteLine(String.Format("AudioFileStatus: UserId={0}  Status={1}  Filename={2}   Duration={3}",
            //    username, audioFileStatus, szFileName, nDuration));

            if (audioFileStatus == _AudioFileStatus.WAV_FINISHED)
            {
                // Strip out the channel name from the filename
                string channelName = szFileName.Remove(szFileName.LastIndexOf('\\'));
                channelName = channelName.Substring(channelName.LastIndexOf('\\') + 1);

                // Change the filename to match the DDD time when the user last started speaking.
                // This is helpful for playback.
                string userAndChannelString = username + "_" + channelName;
                if (userSpeachStartTime.ContainsKey(userAndChannelString) &&
                    (userSpeachStartTime[userAndChannelString] != -1))
                {
                    int time = userSpeachStartTime[userAndChannelString];
                    string filename = Path.GetFileName(szFileName);
                    string newFilename = szFileName.Replace(filename.Remove(filename.IndexOf('_')),
                        time.ToString("D8"));

                    try
                    {
                        File.Move(szFileName, newFilename);
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine(String.Format("Error in OnAudioFileStatus, performing File.Move.  szFileName = {0}; newFilename = {1};", szFileName, newFilename));
                    }
                    userSpeachStartTime[userAndChannelString] = -1; 

                    //System.Diagnostics.Trace.WriteLine(String.Format("AudioFileStatus: Filename changed to {0}",
                    //    newFilename));
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("OnAudioStatus failed");
                }
            }
        }

        private void TimerPinger(object sender, System.EventArgs e)
        {
            //it isn't clear that we need to ping the server.  This might only be needed on the client
            //IsRunning(evoadmin.IsConnected());
        }

        private void ShutDownvoiceServer()
        {
            //pingTimer.Stop();
        }

    }
    public class VoiceRecorder
    {
        private int channelID;

        public VoiceRecorder (int channelNo)
        {
            //simModelName = simModelPath;
            //simModelInfo = smr.readModel(simModelName);
            //isRunning = false;
            //isLoggedIn = false;
            //roomMembership = new Dictionary<string, List<string>>();
            //channelIDMap = new Dictionary<string, int>();
            //userChannelMap = new Dictionary<int, List<int>>();
            //server = new SimulationEventDistributorClient();
            //distributor.RegisterClient(ref server);

           channelID = channelNo;

        }

        public void StartVoiceRecord () 
        {

            ErrorLog.Write("Recording started on channel " + channelID);
        }
   }

}
