using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Aptima.Asim.DDD.ScenarioController;
using Aptima.Asim.DDD.CommonComponents.SimCoreTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.ReplayLogger;
using Aptima.Asim.DDD.CommonComponents.HandshakeManager;
using Aptima.Asim.DDD.CommonComponents.TextChatServer;
using Aptima.Asim.DDD.CommonComponents.WhiteboardServer;
using Aptima.Asim.DDD.CommonComponents.VoiceServer;
using Aptima.Asim.DDD.CommonComponents.ServerViewers;
using Aptima.Asim.DDD.CommonComponents.LogPlayer;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;

namespace Aptima.Asim.DDD.SimulationEngine
  
{
    
    public class SimEngine
    {
        public SimCore simCore;
        public Coordinator coordinator;
        private string simModelFile;
        private int? serverPortNumber;
        private static ReplayLogger replayLogger;
        private static TextChatServer textChatServer;
        private static WhiteboardServer whiteboardServer;
        private static VoiceServer voiceServer;
        //public static HandshakeManager handshakeManager;
        public static ViewManager viewManager;
        private static Player logPlayer;
        private static int simulationLength = 0;
        public int SimulationLength
        {
            get { return simulationLength; }
        }

        private static string productVersion = "Unknown Version";
        private static string compileDate = "Unknown Date";
        private Thread replayLogThread;
        private Thread textChatServerThread;
        private Thread whiteboardServerThread;
        private Thread voiceServerThread;
        private Thread handshakeManagerThread;
        private Thread viewManagerThread;

        public SimEngine(string productVersion, string compileDate)
            : this()
        {
            SimEngine.productVersion = productVersion;
            SimEngine.compileDate = compileDate;
        }
        public SimEngine()
        {
            simCore = new SimCore();
            logPlayer = null;
            coordinator = null;
            simModelFile = null;
            serverPortNumber = null;
            replayLogger = null;
            textChatServer = null;
            whiteboardServer = null;
            voiceServer = null;
            //handshakeManager = null;
            viewManager = null;
        }

        public void Initialize(string simModelFile, int serverPortNumber)
        {
            this.Stop();
            this.simModelFile = simModelFile;
            this.serverPortNumber = serverPortNumber;
        }
        public NetworkServer NetServer
        {
            get { return simCore.NetServer; }
        }
        public void StartSimCore()
        {
            if (simModelFile != null && serverPortNumber != null)
            {
                simCore.Start(simModelFile, (int)serverPortNumber);

            }
            
        }
        public void ResetSimCore()
        {
            simCore.Reset();
        }



        public void StartScenCon(string schemaFile,string scenarioFile, int startState, string debugFile,string replayFile)
        {
            List<string> loggingTypes = new List<string>();
            loggingTypes.Add("test");
            loggingTypes.Add("attributes");
            coordinator = new Coordinator(scenarioFile,
                                          schemaFile,
                                          replayFile,
                                          ref simCore.distributor,
                                          simModelFile,
                                          "NETWORK",
                                          simCore.simModelInfo.GetUpdateFrequency().ToString(),
                                          "GUI",
                                          loggingTypes,
                                          debugFile
                                          );
            int state = 0;
            if (startState == 1)
            {
                state = startState;
            }
            Coordinator.TimerControl(state);
            coordinator.Start();

            SetGameSpeed(1.0);

            Thread.Sleep(300);

        }

        public void StartReplay(string logname, bool loop, double speed)
        {
            logPlayer = new Player(productVersion, compileDate);
            logPlayer.Start(simCore.simModelInfo, ref simCore.distributor, logname, loop);
            SetGameSpeed(speed);
            while (!logPlayer.IsReady())
            {
                Thread.Sleep(100);
            }
            //logPlayer.SetSpeed(speed);
        }

        public void SetGameSpeed(double speed)
        {
            if (logPlayer != null)
            {
                logPlayer.SetSpeed(speed);
            }
            Coordinator.SetGameSpeed(speed);
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simCore.simModelInfo, "GameSpeed");
            ((DoubleValue)e["SpeedFactor"]).value = speed;
            if (simCore != null && simCore.distClient != null)
            {
                simCore.distClient.PutEvent(e);
            }
        }

        public void StopReplay()
        {
            if (logPlayer != null)
            {
                simCore.SendStopReplayEvents();
                logPlayer.Stop();
                logPlayer = null;
            }
            
        }


        public void StopScenario()
        {
            Coordinator.TimerControl(2);
            if (coordinator != null)
            {
                coordinator.Stop();
            }
            if (simCore != null)
            {
                if (coordinator != null)
                {
                    simCore.SendStopScenarioEvents();
                }
                
            }
            coordinator = null;
        }

        public void Pause()
        {
            if (coordinator != null)
            {
                Coordinator.TimerControl(1);
            }
            if (logPlayer != null)
            {
                logPlayer.Pause();
            }
            
        }

        public void Resume()
        {
            if (coordinator != null)
            {
                Coordinator.TimerControl(0);
            }
            if (logPlayer != null)
            {
                logPlayer.Resume();
            }
        }

        public void Stop()
        {
            

            if (coordinator != null)
            {
                coordinator.Stop();
                coordinator = null;
            }
            //Thread.Sleep(100);
            if (simCore != null)
            {
                simCore.Stop();
            }
            if (logPlayer != null)
            {
                logPlayer.Stop();
                logPlayer = null;
            }

            if (viewManager != null)
            {
                viewManager.ResetViewManager();
            }
            simCore = new SimCore();
            
        }
        public void Reset()
        {


            if (coordinator != null)
            {
                coordinator.Stop();
                coordinator = null;
            }
            //Thread.Sleep(100);
            if (simCore != null)
            {
                simCore.Reset();
            }
            if (logPlayer != null)
            {
                logPlayer.Stop();
                logPlayer = null;
            }

            if (viewManager != null)
            {
                viewManager.ResetViewManager();
            }
            

        }
        public BlackboardClient GetBlackboardClient()
        {
            return simCore.GetBlackboardClient();
        }

        public string GetServerHostName()
        {
            return simCore.GetServerHostName();
        }
        public void StartReplayLogger(string outputPath, string mode,string productVersion,string compileDate)
        {
            replayLogger = 
                new ReplayLogger(outputPath, simModelFile, ref simCore.distributor, mode,productVersion,compileDate   );

            replayLogThread = new Thread(new ThreadStart(replayLogger.WriteToLog));
            replayLogThread.Start();
        
        }
        public void StopReplayLogger()
        {
            ReplayLogger.SetIsRunning(false);
            Thread.Sleep(100);
            if (replayLogThread != null)
            replayLogThread.Abort();
        }
        public void StartTextChatServer()
        {
            textChatServer = new TextChatServer(simModelFile, ref simCore.distributor);
            textChatServerThread = new Thread(new ThreadStart(textChatServer.StartTextChatServer));
            textChatServerThread.Start();
        }
        public void StopTextChatServer()
        {
            //TextChatServer.IsRunning = false;
            //Thread.Sleep(100);
            if(textChatServerThread != null)
                textChatServerThread.Abort();
        }
        public void StartWhiteboardServer()
        {
            whiteboardServer = new WhiteboardServer(simModelFile, ref simCore.distributor);
            whiteboardServerThread = new Thread(new ThreadStart(whiteboardServer.StartWhiteboardServer));
            whiteboardServerThread.Start();
        }
        public void StopWhiteboardServer()
        {
            
            //WhiteboardServer.IsRunning = false;
            //Thread.Sleep(100);
            if (whiteboardServerThread != null)
            {
                WhiteboardServer.IsRunning(false);
                whiteboardServerThread.Abort();
            }
        }
        public void StartVoiceServer(int voicePort, string voiceServerPassword, string voiceAdminUser, 
            string voiceAdminPassword, bool voiceRecordChannel, DateTime time, bool isReplay, double replaySpeed)
        {
            voiceServer = new VoiceServer(simModelFile, ref simCore.distributor, time, isReplay, replaySpeed);
            voiceServerThread = new Thread(new ThreadStart(voiceServer.StartVoiceServer));
            voiceServerThread.Name = "DDD VoiceServer";
            voiceServerThread.SetApartmentState(ApartmentState.MTA);
            voiceServerThread.Start();
        }
        public void StopVoiceServer()
        {
            //voiceServer.IsRunning = false;
            //Thread.Sleep(100);
            if (voiceServerThread != null)
                voiceServerThread.Abort();
        }
        /// <summary>
        /// Returns true if the given DM is confirmed, false if they're taken but not confirmed, and
        /// null if they are not taken.
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public string GetDMsAvailability(string dm)
        {
            if (viewManager != null)
                return viewManager.GetDMsAvail(dm);

            return null;
        }
        public void StartViewManager()
        {
            //For memory reasons, disabled for now.  Once an efficient way is determined, re-enable.
            viewManager = new ViewManager(simModelFile, ref simCore.distributor, 99);
            //set stuff
            viewManagerThread = new Thread(new ThreadStart(viewManager.StartViewManagerLoop));
            viewManagerThread.Start();
             
        }
        public void StopViewManager()
        {
            if (viewManager != null)
            {
                viewManager.StopViewManager();
                Thread.Sleep(100);
                viewManagerThread.Abort();
            }
        }
        public string GetEventStream()
        {
            if(viewManager != null)
                return viewManager.GetEventStream();

            return string.Empty;
        }
        public bool IsReady()
        {
            if (simCore == null)
            {
                if(logPlayer == null)
                    return false;

                return logPlayer.IsReady();
            }
            return (simCore.IsReady());
        }
        public bool ErrorLoadingScenario()
        {
            return Coordinator.EncounteredError();
        }
    }
}
