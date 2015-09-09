//#define TESTING
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Aptima.Asim.DDD.SimulationEngine;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.UserTools;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.HandshakeManager;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using System.Text.RegularExpressions;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.SimCoreServer
{
    public class SimCoreServer
    {
        private static Regex creatorRegex;
        private static string productVersion = "Dynamic Distributed Decision-Making Ver 4.2";
        public static string GetProductVersion()
        {
            return productVersion;
        }
        private static string compileDate = "Compiled on: 05.17.2012";
        public static string GetCompileDate()
        {
            return compileDate;
        }
        private static string supportEmail = "Support e-mail address: support@aptima.com";
        public static string GetSupportEmail()
        {
            return supportEmail;
        }
        private static string supportPhoneNumber = "Support phone number: 866.461.7298";
        public static string GetSupportPhoneNumber()
        {
            return supportPhoneNumber;
        }
        private static SimulationEventDistributorClient eventClient = null;
        public static SimEngine simEngine = new SimEngine(productVersion,compileDate);
        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
        }
        private string _productFamily = "Asim";
        private string _productName = "DDD";
        private string _productDisplayName = "DDD";
        private string _productCode = "_DDD";
        private int _majorVersion = 4;
        private int _minorVersion = 2;
        private string _licenseSoftwareVersion = "4.2";


        public NetworkServer NetServer
        {
            get { return simEngine.NetServer; }
        } 
        private static string log = string.Empty;
        public string Log
        {
            get { return log; }
        }


        private static string debugFile = string.Empty;
        public int SimulationLength
        {
            get { return simEngine.SimulationLength; }
        }

        private static int currentTime = 0;
        public int CurrentTime
        {
            get { return currentTime; }
        }
        public enum ServerState
        {
            STOPPED = 0,
            RUNNING = 1,
        }
        public enum ScenarioState
        {
            STOPPED = 0,
            RUNNING = 1,
            PAUSED = 2,
            LOADING = 3,
        }
        private static ServerState serverState = ServerState.STOPPED;
        private static ScenarioState scenarioState = ScenarioState.STOPPED;
        public static ScenarioState replayState = ScenarioState.STOPPED;

        public static ScenarioState _ScenarioState
        {
            get { return scenarioState; }
        }
        public static ScenarioState _ReplayState
        {
            get { return replayState; }
        }
        public static ServerState _ServerState
        {
            get { return serverState; }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
        }


        public void SendServerStateEvent(String messageType, String messageText)
        {
            if (eventClient != null)
            {
                SimulationEvent e = SimulationEventFactory.BuildEvent(ref simEngine.simCore.simModelInfo, "ServerState");
                ((StringValue)e["MessageType"]).value = messageType;
                ((StringValue)e["MessageText"]).value = messageText;
                eventClient.PutEvent(e);
            }
        }


        public static bool CheckNetworkSettings()
        {
            IPAddress ip;
            ServerOptions.ReadFile();
            TcpListener testServer = NetworkUtility.GetTcpListener(System.Net.Dns.GetHostName(), ServerOptions.PortNumber,out ip);
            try
            {
                testServer.Start();
                testServer.Stop();
                return true;
            }
            catch
            {
                return false;
            }
            
            
        }
        public SimCoreServer()
        {
            ServerOptions.ReadFile();         
            WriteClientAppConfig();
            simEngine.Initialize(ServerOptions.SimulationModelPath, ServerOptions.PortNumber);
            simEngine.StartSimCore();
            simEngine.StartViewManager();
            eventClient = new SimulationEventDistributorClient();
            simEngine.simCore.distributor.RegisterClient(ref eventClient);
            eventClient.Subscribe("PauseScenarioRequest");
            eventClient.Subscribe("ResumeScenarioRequest");
            eventClient.Subscribe("LoadScenarioRequest");
            eventClient.Subscribe("StopScenarioRequest");
            eventClient.Subscribe("SimCoreReady");
            eventClient.Subscribe("ForkReplayStarted");
            eventClient.Subscribe("ForkReplayFinished");
            eventClient.Subscribe("GameSpeedRequest"); //AD: UN-Removed for now

        }

        public string GetDMAvail(string dm)
        {
            return simEngine.GetDMsAvailability(dm);
        }

        public void SimControlLoadScenario(String scenarioPath, String groupName, String outputLogDir)
        {
            if (!File.Exists(scenarioPath))
            {
                SendServerStateEvent("SCENARIO_LOAD_FAILURE", "Specified scenario does not exist");
            }
            if (groupName.Contains("\\") || groupName.Contains("/"))
            {
                SendServerStateEvent("SCENARIO_LOAD_FAILURE", "Invalid input in Group Name.  Cannot contain \\ or /.");
            }
            Authenticator.LoadUserFile();
            if (Authenticator.GetNumUsers() == 0)
            {
                SendServerStateEvent("SCENARIO_LOAD_FAILURE", "You must create at least one user account before starting the server.\nUse the Users-->User Administration menu item.");
            }
            string logType = "NOLOG";
            switch (ServerOptions.EventLogType)
            {
                case "DETAILED":
                    logType = "Detailed";
                    break;
                case "LIMITED":
                    logType = "Limited";
                    break;
                default: break;
            }
            ServerOptions.DefaultScenarioPath = scenarioPath;
            ServerOptions.EventLogDirectory = outputLogDir;
            LoadScenario(logType, DateTime.Now, groupName,null);

        }
        public void SimControlStopScenario(Boolean guiInitiated)
        {
            StopScenario(guiInitiated);
            ResetServer();
            SendServerStateEvent("SCENARIO_STOPPED","");
        }
        public void SimControlLoadReplay(String replayLogPath, Double replaySpeed)
        {

            Authenticator.LoadUserFile();


            if (replayLogPath == this.Log)
            {
                SendServerStateEvent("REPLAY_LOAD_FAILURE", "Your current replay log is set to the same file as the file you'll be replaying from.  To prevent error, change one of these two files before starting the server.");
                return;
            }
            if (Authenticator.GetNumUsers() == 0)
            {
                SendServerStateEvent("REPLAY_LOAD_FAILURE", "You must create at least one user account before loading a replay.\nUse the Users-->User Administration menu item.");
                return;
            }


            try
            {
                StreamReader re = File.OpenText(replayLogPath);

                string input = re.ReadLine();

                re.Close();
                creatorRegex = new Regex(@"^<Creator><Version>" + SimCoreServer.GetProductVersion() + "</Version><CompiledOn>" + SimCoreServer.GetCompileDate() + "</CompiledOn></Creator>$", RegexOptions.Singleline);
                if (!creatorRegex.IsMatch(input))
                {
                    SendServerStateEvent("REPLAY_LOAD_FAILURE", "This file must be replayed in a different version of the DDD.");
                    return;

                }
            }
            catch (System.IO.IOException exc)
            {
                SendServerStateEvent("REPLAY_LOAD_FAILURE", String.Format("The selected file could not be opened for replay.\n{0}",exc.Message));
                return;
            }

            simEngine.StartTextChatServer();
            simEngine.StartWhiteboardServer();

            //  Start the voice server if it exists and is needed
            //    The conditions will be added later
            if (ServerOptions.EnableVoiceServer)
            {
                // Get the simulation start time from the replay filename
                DateTime time = DateTime.Now;

                try
                {
                    string timeString = replayLogPath.Remove(replayLogPath.LastIndexOf('.'));

                    timeString = timeString.Substring(timeString.LastIndexOf('.') + 1);

                    time = DateTime.ParseExact(timeString, "yyyyMMddHHmmss", null);
                }
                catch (Exception exc)
                {
                }

                HandshakeManager.SetVoiceEnabled(true);
                HandshakeManager.SetVoiceServerName(ServerOptions.VoiceServerHostname);
                HandshakeManager.SetVoiceServerPassword(ServerOptions.VoiceServerUserPassword);
                HandshakeManager.SetVoiceServerPort(ServerOptions.VoiceServerPort);
                simEngine.StartVoiceServer(ServerOptions.VoiceServerPort, ServerOptions.VoiceServerUserPassword,
                    ServerOptions.VoiceServerAdminUsername, ServerOptions.VoiceServerAdminPassword, ServerOptions.EnableVoiceServerRecordings, time, true, replaySpeed);
            }
            else
            {
                HandshakeManager.SetVoiceEnabled(false);
                HandshakeManager.SetVoiceServerName(ServerOptions.VoiceServerHostname);
                HandshakeManager.SetVoiceServerPassword(ServerOptions.VoiceServerUserPassword);
                HandshakeManager.SetVoiceServerPort(ServerOptions.VoiceServerPort);
            }

            this.StartReplay(replayLogPath, false);
            this.SimControlPauseScenario();
            SimCoreServer.replayState = Aptima.Asim.DDD.SimCoreServer.SimCoreServer.ScenarioState.LOADING;
            this.SetReplaySpeed(replaySpeed);

        }
        public void SimControlStopReplay()
        {
            StopReplay();
            ResetServer();
            SendServerStateEvent("REPLAY_STOPPED", "");
        }
        public void SimControlLoadForkReplay(String scenarioPath, String groupName, String outputLogDir, String replayLogPath, Double replaySpeed)
        {
            if (!File.Exists(scenarioPath))
            {
                SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", "Specified scenario does not exist");
            }
            if (replayLogPath == this.Log)
            {
                SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", "Your current replay log is set to the same file as the file you'll be replaying from.  To prevent error, change one of these two files before starting the server.");
                return;
            }
            if (groupName.Contains("\\") || groupName.Contains("/"))
            {
                SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", "Invalid input in Group Name.  Cannot contain \\ or /.");
            }
            Authenticator.LoadUserFile();
            if (Authenticator.GetNumUsers() == 0)
            {
                SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", "You must create at least one user account before starting the server.\nUse the Users-->User Administration menu item.");
            }

            try
            {
                StreamReader re = File.OpenText(replayLogPath);

                string input = re.ReadLine();

                re.Close();
                creatorRegex = new Regex(@"^<Creator><Version>" + SimCoreServer.GetProductVersion() + "</Version><CompiledOn>" + SimCoreServer.GetCompileDate() + "</CompiledOn></Creator>$", RegexOptions.Singleline);
                if (!creatorRegex.IsMatch(input))
                {
                    SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", "This file must be replayed in a different version of the DDD.");
                    return;

                }
            }
            catch (System.IO.IOException exc)
            {
                SendServerStateEvent("FORK_REPLAY_LOAD_FAILURE", String.Format("The selected file could not be opened for replay.\n{0}", exc.Message));
                return;
            }

            string logType = "NOLOG";
            switch (ServerOptions.EventLogType)
            {
                case "DETAILED":
                    logType = "Detailed";
                    break;
                case "LIMITED":
                    logType = "Limited";
                    break;
                default: break;
            }
            ServerOptions.DefaultScenarioPath = scenarioPath;
            ServerOptions.EventLogDirectory = outputLogDir;
            LoadScenario(logType, DateTime.Now, groupName, replayLogPath);

        }
        public void SimControlStopForkReplay()
        {
            StopScenario(true);
            ResetServer();
            SendServerStateEvent("FORK_REPLAY_STOPPED", "");
        }


        public void SimControlPauseScenario()
        {
            if (scenarioState != ScenarioState.STOPPED)
            {
                if (scenarioState == ScenarioState.PAUSED)
                {//un-pause 
                    scenarioState = ScenarioState.RUNNING;
                    simEngine.Resume();
                }
                else
                {//pause 
                    scenarioState = ScenarioState.PAUSED;
                    simEngine.Pause();
                }
            }
            if (replayState != ScenarioState.STOPPED)
            {
                if (replayState == ScenarioState.PAUSED)
                {//un-pause 
                    replayState = ScenarioState.RUNNING;
                    simEngine.Resume();
                }
                else
                {//pause 
                    replayState = ScenarioState.PAUSED;
                    simEngine.Pause();
                }
            }
        }


        private void LoadScenario(string logType, DateTime time, string groupName, string replayFile)
        {
            
            ServerOptions.WriteFile();
            serverState = ServerState.RUNNING;

            string scenarioTitle = ServerOptions.DefaultScenarioPath.Remove(ServerOptions.DefaultScenarioPath.LastIndexOf('.'));
            scenarioTitle = scenarioTitle.Remove(0, scenarioTitle.LastIndexOf('\\') + 1);
            debugFile = String.Format("{0}\\Debug\\DDDDebugLog-{1}.{2}.{3:yyyyMMddHHmmss}.txt", ServerOptions.EventLogDirectory, scenarioTitle, groupName, time);
            log = String.Format("{0}\\DDDLog-{1}.{2}.{3:yyyyMMddHHmmss}.ddd", ServerOptions.EventLogDirectory, scenarioTitle, groupName, time);
            //TODO: Add back in later WriteClientAppConfig();
            SendServerStateEvent("LOADING_SCENARIO", scenarioTitle);
            if (System.IO.File.Exists(ServerOptions.SimulationModelPath))
            {
                switch (logType)
                {
                    case "Limited":
                        //Begin Logging

                        simEngine.StartReplayLogger(Log, logType,productVersion,compileDate);
                        break;

                    case "Detailed":
                        //Begin logging 

                        simEngine.StartReplayLogger(Log, logType,productVersion,compileDate);
                        break;
                    default: //Don't log
                        break;
                }
                simEngine.StartTextChatServer();
                simEngine.StartWhiteboardServer();                

                //  Start the voice server if it exists and is needed
                //    The conditions will be added later
                if (ServerOptions.EnableVoiceServer)
                {
                    HandshakeManager.SetVoiceEnabled(true);
                    HandshakeManager.SetVoiceServerName(ServerOptions.VoiceServerHostname);
                    HandshakeManager.SetVoiceServerPassword(ServerOptions.VoiceServerUserPassword);
                    HandshakeManager.SetVoiceServerPort(ServerOptions.VoiceServerPort);
                    simEngine.StartVoiceServer(ServerOptions.VoiceServerPort, ServerOptions.VoiceServerUserPassword,
                        ServerOptions.VoiceServerAdminUsername, ServerOptions.VoiceServerAdminPassword, ServerOptions.EnableVoiceServerRecordings, time, false, 0.0);
                }
                else
                {
                    HandshakeManager.SetVoiceEnabled(false);
                    HandshakeManager.SetVoiceServerName(ServerOptions.VoiceServerHostname);
                    HandshakeManager.SetVoiceServerPassword(ServerOptions.VoiceServerUserPassword);
                    HandshakeManager.SetVoiceServerPort(ServerOptions.VoiceServerPort);
                }
                StartSimulationPaused(ServerOptions.ScenarioSchemaPath, ServerOptions.DefaultScenarioPath,replayFile);
                
            }
            else
            {
                throw new Exception("Error setting simulation model.");
            }
        }

        public void SendOutAvailablePlayers()
        {
            List<String> terms = simEngine.simCore.GetConnectedNetworkTerminals();
            foreach (String term in terms)
            {
                //SimEngine.viewManager
                SimEngine.viewManager.handshakeManager.SendAvailableDMEvent(term);
                
            }
        }


        public void Update()
        {
            if (scenarioState == ScenarioState.LOADING)
            {
                if (simEngine.ErrorLoadingScenario())
                {
                    SendServerStateEvent("SCENARIO_LOAD_FAILURE", "Error Loading Scenario.");
                    this.ResetServer();
                    return;
                }
                if (simEngine.IsReady())
                {
                    scenarioState = ScenarioState.PAUSED;
                    isReady = true;
                    SendServerStateEvent("SCENARIO_LOAD_SUCCESS", "");
                }
            }
            if (replayState == ScenarioState.LOADING)
            {
                if (simEngine.ErrorLoadingScenario())
                {
                    this.StopReplay();
                    SendServerStateEvent("REPLAY_LOAD_FAILURE", "Error Loading Replay.");
                    return;
                }
                if (simEngine.IsReady())
                {
                    replayState = ScenarioState.PAUSED;
                    //SendOutAvailablePlayers();
                    isReady = true;
                    SendServerStateEvent("REPLAY_LOAD_SUCCESS", "");
                }
            }
            currentTime = (simEngine.simCore.GetSimTime() / 1000);
            List<SimulationEvent> events = eventClient.GetEvents();
            if (events.Count > 0)
            {
                foreach (SimulationEvent e in events)
                {
                    switch (e.eventType)
                    {
                        case "PauseScenarioRequest":
                            if (scenarioState == ScenarioState.RUNNING)
                            {
                                SimControlPauseScenario();
                            }
                            break;
                        case "ResumeScenarioRequest":
                            if (scenarioState == ScenarioState.PAUSED)
                            {
                                SimControlPauseScenario();
                            }
                            break;
                        case "LoadScenarioRequest":
                            if (scenarioState == ScenarioState.STOPPED)
                            {
                                SimControlLoadScenario(((StringValue)e["ScenarioPath"]).value,
                                                       ((StringValue)e["GroupName"]).value,
                                                       ((StringValue)e["OutputLogDir"]).value);
                            }
                            break;
                        case "StopScenarioRequest":
                            if (scenarioState != ScenarioState.STOPPED)
                            {
                                SimControlStopScenario(false);
                            }
                            break;
                        case "SimCoreReady":
                            SendOutAvailablePlayers();
                            break;
                        case "GameSpeedRequest":
                            double speed = 1.0;
                            try
                            {
                                speed = ((DoubleValue)e["SpeedFactor"]).value;
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                            this.SetReplaySpeed(speed);
                            break;
                        case "ForkReplayFinished":
                            HandshakeManager.IsForkReplay = false;
                            simEngine.SetGameSpeed(1);
                            if (scenarioState == ScenarioState.RUNNING)
                            {
                                SimControlPauseScenario();
                            }
                            break;
                        case "ForkReplayStarted":
                            HandshakeManager.IsForkReplay = true;
                            simEngine.SetGameSpeed(ServerOptions.ForkReplaySpeed);
                            break;
                    }
                }
            }

        }

        public void WriteClientAppConfig()
        {
            //TODO: Fix path in installer to no longer have ver num.
            string fileName = @"\\" + ServerOptions.DDDClientPath + @"\Aptima.cfg";
//UNDOING the change for debugging purposes.
          //  string execPath = Path.GetDirectoryName(Application.ExecutablePath);
          //  string fileName =  execPath.Substring(0, execPath.LastIndexOf('\\')) +@"\Client\Aptima.cfg";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }



            //string headerHtml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<configuration><appSettings>\n";
            //string footerHtml = "</appSettings></configuration>\n";
            StringBuilder b = new StringBuilder();
            try
            {
                FileStream file = File.Open(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter summary = new StreamWriter(file);
                //b.Append(headerHtml);
                b.AppendFormat("{0}\n{1}", ServerOptions.HostName, ServerOptions.PortNumber);
                //b.Append(String.Format("<add key=\"Hostname\" value=\"{0}\"/>\n", this.HostName));
                //b.Append(String.Format("<add key=\"Port\" value=\"{0}\"/>\n",this.Port));
                //b.Append(footerHtml);
                summary.Write(b.ToString());
                summary.Close();
            }
            catch (Exception m)
            {
                MessageBox.Show("Couldn't write client app config: " + m.Message, fileName);
                return;
            }



        }

        public void WriteScoreSummary()
        {

            string fileName = "\\\\"+ServerOptions.DDDClientPath+ @"\ScoreSummary.htm";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            if (ScoringDB.scores.Count == 0)
            {
                return;
            }


            string headerHtml = "<html><head><title>DDD Scoring</title></head><body><p><h2>DDD Score Summary</h2></p><table border=\"1\">";
            string footerHtml = "</table></body></html>";
            StringBuilder b = new StringBuilder();
            try
            {
                FileStream file = File.Open(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter summary = new StreamWriter(file);
                b.Append(headerHtml);

                b.Append("<tr><th>Decision Maker</th><th>Score Name</th><th>Score Value</th></tr>");
                foreach (StateDB.DecisionMaker dm in StateDB.decisionMakers.Values)
                {
                    foreach (ScoringDB.Score s in ScoringDB.scores.Values)
                    {
                        if (s.calculateDMs.Contains(dm.id))
                        {
                            b.Append("<tr>");
                            b.Append(String.Format("<td>{0}</td>", dm.id));
                            b.Append(String.Format("<td>{0}</td>", s.name));
                            b.Append(String.Format("<td>{0}</td>", s.scoreValue));
                            b.Append("</tr>");
                        }
                    }
                }

                b.Append(footerHtml);
                summary.Write(b.ToString());
                summary.Flush();
                summary.Close();
            }
            catch (Exception m)
            {
                MessageBox.Show("Couldn't write scoring summary: " + m.Message);
                return;
            }

            if (ServerOptions.ShowScoreSummary)
            {
                try
                {
                    System.Diagnostics.Process.Start(fileName);
                }
                catch (Exception m)
                {
                    MessageBox.Show("Couldn't load score summary web page: " + m.Message);
                    return;
                }
            }


        }
        public void ResetServer()
        {
            if (scenarioState != ScenarioState.STOPPED)
            {
                StopScenario(false);
            }
            if (replayState != ScenarioState.STOPPED)
            {
                StopReplay();
            }

            if (ServerOptions.EventLogType != "NOLOG")
            {
                simEngine.StopReplayLogger();
            }
            simEngine.StopTextChatServer();
            simEngine.StopWhiteboardServer();
            simEngine.StopVoiceServer();
            
            simEngine.Reset();
            currentTime = 0;
            isReady = false;
            log = String.Empty;
        }
        public void StopServer()
        {

            
            if ((scenarioState == ScenarioState.STOPPED && replayState != ScenarioState.STOPPED) ||
                (scenarioState != ScenarioState.STOPPED && replayState == ScenarioState.STOPPED))
            {
                StopScenario(true);
            }
            serverState = ServerState.STOPPED;
            if (ServerOptions.EventLogType != "NOLOG")
            {
                simEngine.StopReplayLogger();
            }
            simEngine.StopTextChatServer();
            simEngine.StopWhiteboardServer();
            simEngine.StopVoiceServer();
            simEngine.StopViewManager();
            simEngine.Stop();
            eventClient = null;
            currentTime = 0;
            isReady = false;
        }

        public void StartSimulationPaused(string schemaFile, string scenarioFile,string replayFile)
        {
            scenarioState = ScenarioState.LOADING;
            simEngine.StartScenCon(schemaFile, scenarioFile, 1, debugFile,replayFile);//start as paused
        }

        public void StartReplay(string replayPath, bool loop)
        {
            simEngine.StartReplay(replayPath, loop, 1.0);

            replayState = ScenarioState.RUNNING;
            SendServerStateEvent("LOADING_REPLAY", replayPath);
        }

        public void SetReplaySpeed(double speed)
        {
            simEngine.SetGameSpeed(speed);
        }

        private void StopReplay()
        {
            simEngine.StopReplay();
            scenarioState = ScenarioState.STOPPED;
            replayState = ScenarioState.STOPPED;
            isReady = false;
        }

        private void StopScenario(Boolean guiInitiated)
        {
            if (guiInitiated)
            {
                if (scenarioState != ScenarioState.STOPPED)
                {
                    WriteScoreSummary();
                }
            }
            scenarioState = ScenarioState.STOPPED;
            replayState = ScenarioState.STOPPED;
            simEngine.StopScenario();
            
            isReady = false;
        }

        public string GetEventStream()
        {
            return simEngine.GetEventStream();
        }
    }
}
