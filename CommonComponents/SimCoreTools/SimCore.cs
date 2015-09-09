using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

using Aptima.Asim.DDD.Simulators.BlackboardManager;
using Aptima.Asim.DDD.Simulators.StaticAttribute;
using Aptima.Asim.DDD.Simulators.Motion;
using Aptima.Asim.DDD.Simulators.ViewPro;
using Aptima.Asim.DDD.Simulators.CollisionDetection;
using Aptima.Asim.DDD.Simulators.AttackProcessor;
using Aptima.Asim.DDD.Simulators.FuelHandling;
using Aptima.Asim.DDD.Simulators.DockingProcessor;
using Aptima.Asim.DDD.Simulators.SelfDefense;
using Aptima.Asim.DDD.Simulators.Scoring;
using Aptima.Asim.DDD.Simulators.ExternalCommunication;


using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;
using System.Windows.Forms;
using System.Reflection;

namespace Aptima.Asim.DDD.CommonComponents.SimCoreTools
{
    public enum SimCoreState
    {
        UNINITIALIZED,
        RUNNING,
        STOPPING
    }
    public class SimCore
    {
        private System.Object simCoreLock;
        private System.Object simTimeLock;
        private SimCoreState state;


        private int serverPort;
        private string serverHostname;
        private NetworkServer server;

        public SimulationEventDistributor distributor;
        private Blackboard blackboard;
        public SimulationEventDistributorClient distClient;


        private string simModelFile;
        public SimulationModelInfo simModelInfo;
        private Dictionary<string, ISimulator> simulators;

        public int simulationTime;
        private int updateFrequency;
        private Thread eventLoopThread;


        public SimCore()
        {
            simCoreLock = new object();
            simTimeLock = new object();
            state = SimCoreState.UNINITIALIZED;
            eventLoopThread = null;
        }

        public NetworkServer NetServer
        {
            get { return server; }
        }

        public void InitializeDynamicData()
        {
            simulationTime = 0;
            state = SimCoreState.UNINITIALIZED;
        }

        public void Start(string simModelFile, int serverPort)
        {
            lock (simCoreLock)
            {
                InitializeDynamicData();
                InitializeSimulationModel(simModelFile);
                InitializeDistributor();
                InitializeBlackboard();
                InitializeServer(serverPort);
                InitializeSimulators();

                state = SimCoreState.RUNNING;
                server.ListenForConnections();
            }

            eventLoopThread = new Thread(new ThreadStart(EventLoop));
            eventLoopThread.Start();

        }
        public void Reset()
        {
            lock (simCoreLock)
            {
                state = SimCoreState.STOPPING;
            }
            while (state != SimCoreState.UNINITIALIZED)
            {
                Thread.Sleep(20);
            }
            lock (simCoreLock)
            {
                InitializeDynamicData();
                InitializeBlackboard();
                InitializeSimulators();
                state = SimCoreState.RUNNING;
            }

            eventLoopThread = new Thread(new ThreadStart(EventLoop));
            eventLoopThread.Start();

            
        }
        public Dictionary<int, string> GetClientDescriptions()
        {
            if (server != null)
            {
                return server.GetClientDescriptions();
            }
            else
            {
                return new Dictionary<int, string>();
            }
        }
        public void SendStopScenarioEvents()
        {
            if (simModelInfo != null)
            {
                SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "StopScenario");
                e["Time"] = DataValueFactory.BuildInteger(simulationTime);//ConvertInteger(latestTick);
                distClient.PutEvent(e);
                e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ExternalApp_SimStop");
                e["Time"] = DataValueFactory.BuildInteger(simulationTime);//ConvertInteger(latestTick);
                distClient.PutEvent(e);
            }
        }
        public void SendStopReplayEvents()
        {
            if (simModelInfo != null)
            {
                SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "StopReplay");
                e["Time"] = DataValueFactory.BuildInteger(simulationTime);//ConvertInteger(latestTick);
                distClient.PutEvent(e);
                e = SimulationEventFactory.BuildEvent(ref simModelInfo, "ExternalApp_SimStop");
                e["Time"] = DataValueFactory.BuildInteger(simulationTime);//ConvertInteger(latestTick);
                distClient.PutEvent(e);
            }
        }
        public void Stop()
        {
            lock (simCoreLock)
            {
                if (server != null)
                {
                    server.Shutdown();
                    state = SimCoreState.STOPPING;
                }
            }
        }

        

        public int GetSimTime()
        {
            int t;
            lock (simTimeLock)
            {
                t = simulationTime;
            }
            return t;
        }

        public string GetServerHostName()
        {
            string h;
            lock (simCoreLock)
            {
                h = serverHostname;
            }
            return h;
        }

        public int GetServerPort()
        {
            int p;
            lock (simCoreLock)
            {
                p = serverPort;
            }
            return p;
        }

        public int ClientCount()
        {
            int c;

            lock (simCoreLock)
            {
                c = server.ClientCount();
            }

            return c;
        }

        public SimCoreState GetState()
        {
            SimCoreState s;
            lock (simCoreLock)
            {
                s = state;
            }
            return s;
        }

        private bool isReady = false;
        public bool IsReady()
        {
            return isReady;
        }

        public BlackboardClient GetBlackboardClient()
        {
            BlackboardClient c = new BlackboardClient();
            blackboard.RegisterClient(ref c);
            return c;
        }

        private void InitializeSimulationModel(string simModelFile)
        {
            this.simModelFile = simModelFile;
            SimulationModelReader smr = new SimulationModelReader();
            simModelInfo = smr.readModel(simModelFile);
            updateFrequency = simModelInfo.GetUpdateFrequency();
        }
        private void InitializeDistributor()
        {
            distributor = new SimulationEventDistributor(ref simModelInfo);
            distClient = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref distClient);
            

            foreach (Aptima.Asim.DDD.CommonComponents.SimulationModelTools.EventInfo e in simModelInfo.eventModel.events.Values)
            {
                if (e.simCoreSubscribe)
                {
                    distClient.Subscribe(e.name);
                }
            }
            
        }
        private void InitializeBlackboard()
        {
            blackboard = new Blackboard(ref simModelInfo);
        }
        private void InitializeServer(int port)
        {
            this.serverPort = port;
            server = new NetworkServer(serverPort, ref distributor);
            this.serverHostname = server.GetHostName();
        }
        private void InitializeSimulators()
        {
            simulators = new Dictionary<string, ISimulator>();
            String exeDir = AppDomain.CurrentDomain.BaseDirectory;
            String dllPath = "";
            bool success = true;
            StringBuilder sbError = new StringBuilder("Error initializing Simulators:");
            foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
            {
                try{
                    if (sei.simulatorName == "SeamateProcessor")
                    {
                        break;
                    }
                String simName = sei.simulatorName + "Sim";
                String dll = sei.dllName;
                String className = "";
                dllPath = dll;
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(dllPath); //try CurrentDir first
                }
                catch (Exception inEx)
                {
                    dllPath = System.IO.Path.Combine(exeDir, dll); //try exe dir
                    assembly = Assembly.LoadFrom(dllPath);
                }
                className = dll.Replace(".dll", ".") + simName;
                Type t = assembly.GetType(className);
                ISimulator sim = (ISimulator)Activator.CreateInstance(t);
                simulators[sei.simulatorName] = sim;
                }catch(Exception ex)
                {
                    success = false;
                    sbError.Append("\r\nError loading '" + sei.simulatorName + "' Simulator. ("+ex.Message+")");
                    ErrorLog.Write("Error loading '"+sei.simulatorName+"' Simulator.  Going to continue gracefully.");
                    ErrorLog.Write(ex.StackTrace+"\r\n");
                }
            }
            if (!success)
            {
                throw new Exception(sbError.ToString());
                return;
            }
            /*
            simulators["BlackboardManager"] = new BlackboardManagerSim();
            simulators["StaticAttribute"] = new StaticAttributeSim();
            simulators["DockingProcessor"] = new DockingProcessorSim();            
            simulators["FuelHandling"] = new FuelHandlingSim();
            simulators["Motion"] = new MotionSim();
            simulators["CollisionDetection"] = new CollisionDetectionSim();
            simulators["SelfDefense"] = new SelfDefenseSim();
            simulators["AttackProcessor"] = new AttackProcessorSim();
            simulators["Scoring"] = new ScoringSim();
            simulators["SeamateProcessor"] = new Aptima.Asim.DDD.Simulators.SeamateSimulator.SeamateProcessorSim();
            simulators["ViewPro"] = new ViewProSim();
             */
            if (ServerOptions.HLAExport)
            {
                string err = string.Empty;
                if (!System.IO.File.Exists(ServerOptions.HLAFederationFilePath))
                {
                    err = "Cannot find HLA Federation File (.fed)\r\n";
                }
                if (!System.IO.File.Exists(ServerOptions.HLAXMLFilePath))
                {
                    err += "Cannot find HLA XML File (.xml) ";
                }
                if (err != string.Empty)
                {
                    ServerOptions.HLAExport = false;
                    System.Windows.Forms.MessageBox.Show("Error locating HLA Files: \r\n" + err, "Error initializing HLA Paths");
                }
                else
                {
                    simulators["ExternalCommunication"] = new ExternalCommunicationSim();
                }
            }
            //foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
            foreach (KeyValuePair<String, ISimulator> sim in simulators)
            {
            //    simulators[sei.simulatorName].Initialize(ref simModelInfo, ref blackboard, ref distributor);
                sim.Value.Initialize(ref simModelInfo, ref blackboard, ref distributor);

            }

        }

        //public void SendPause()
        //{
        //    SimulationEvent pause = SimulationEventFactory.BuildEvent(ref simModelInfo, "PauseScenario");
        //    server.eventDist.PutEvent(pause);
        //    
        //}

        //public void SendResume()
        //{
        //    SimulationEvent resume = SimulationEventFactory.BuildEvent(ref simModelInfo, "ResumeScenario");
        //    server.eventDist.PutEvent(resume);
        //}
        public List<String> GetConnectedNetworkTerminals()
        {
            return server.GetConnectedTerminals();
        }

        private void EventLoop()
        {
            try
            {
                List<SimulationEvent> events = null;
                DateTime nowTimer = new DateTime();
                DateTime tickTimer = DateTime.Now;
                DateTime startTime = new DateTime();
                int eventCounter = 0;

                long start;
                long end;
                Dictionary<string, long> simTimes = new Dictionary<string, long>();

                foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
                {
                    simTimes[sei.simulatorName] = 0;
                }

                while (true)
                {
                    nowTimer = DateTime.Now;
                    lock (simCoreLock)
                    {
                        switch (state)
                        {
                            case SimCoreState.UNINITIALIZED:
                                throw new Exception("SimCore: shouldn't be in event loop if uninitialized");
                            case SimCoreState.RUNNING:
                                //distributor.StopIncoming();
                                events = distClient.GetEvents();
                                eventCounter = events.Count;

                                foreach (SimulationEvent e in events)
                                {
                                    switch (e.eventType)
                                    {
                                        case "TimeTick":
                                            lock (simTimeLock)
                                            {
                                                simulationTime = (int)((IntegerValue)e["Time"]).value;

                                                if (ServerOptions.UsePerformanceLog)
                                                {
                                                    StringBuilder b = new StringBuilder();
                                                    b.AppendFormat("SimCore Metric: SimTime: {0}; Processing Time: {1}.", simulationTime / 1000, DateTime.Now - tickTimer);
                                                    PerformanceLog.Write(b.ToString());
                                                }
                                                tickTimer = DateTime.Now;
                                                if (simulationTime == 1000)
                                                {
                                                    startTime = DateTime.Now;
                                                }
                                            }
                                            break;
                                        case "ExternalApp_SimStop":
                                            //Console.Out.WriteLine("SimCore Metric: Exiting SimCore loop.  Total run time: {0}; Total ticks: {1}.", DateTime.Now - startTime, (simulationTime / 1000) - 1);//-1 because it is starting at time 1
                                            isReady = false;
                                            break;
                                        //case "PauseScenario":
                                        //    if (simulators.ContainsKey("ViewPro"))
                                        //    {
                                        //        ((ViewProSim)simulators["ViewPro"]).PauseScenario();
                                        //    }
                                        //    break;
                                        //case "ResumeScenario":
                                        //    if (simulators.ContainsKey("ViewPro"))
                                        //    {
                                        //        ((ViewProSim)simulators["ViewPro"]).ResumeScenario();
                                        //    }
                                        //    break;
                                        case "ResetSimulation":
                                            lock (simTimeLock)
                                            {
                                                simulationTime = 0;
                                                isReady = false;
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    //foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
                                    foreach(KeyValuePair<String, ISimulator> sim in simulators)
                                    {
                                        start = DateTime.Now.Ticks;
                                        //simulators[sei.simulatorName].ProcessEvent(e);
                                        sim.Value.ProcessEvent(e);
                                        end = DateTime.Now.Ticks;

                                        //simTimes[sei.simulatorName] += (end - start);
                                        simTimes[sim.Key] += (end - start);

                                    }

                                    if (e.eventType == "StartupComplete")
                                    {
                                        SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModelInfo, "SimCoreReady");
                                        isReady = true;
                                        distClient.PutEvent(ev);
                                        if (ServerOptions.UsePerformanceLog)
                                        {
                                            foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
                                            {
                                                if (!simTimes.ContainsKey(sei.simulatorName))
                                                    continue;
                                                StringBuilder b = new StringBuilder();
                                                b.AppendFormat("SimCore Metric: Initialization time in: {0} was {1} seconds.", sei.simulatorName, simTimes[sei.simulatorName] / 10000000.0);
                                                PerformanceLog.Write(b.ToString());
                                                simTimes[sei.simulatorName] = 0;
                                            }
                                        }
                                    }
                                }
                                if (eventCounter > 0)
                                {
                                    //Console.Out.WriteLine("SimCore Metric: Events Processed: {0}; processing time: {1}.", eventCounter, DateTime.Now - nowTimer);
                                    //distributor.ResumeIncoming(); 
                                }
                                break;
                            case SimCoreState.STOPPING:
                                
                                long total = 0;
                                if (ServerOptions.UsePerformanceLog)
                                {
                                    foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
                                    {
                                        if (!simTimes.ContainsKey(sei.simulatorName))
                                            continue;
                                        total += simTimes[sei.simulatorName];
                                    }
                                    foreach (SimulatorExecutionInfo sei in simModelInfo.simulationExecutionModel.simulators)
                                    {
                                        if (!simTimes.ContainsKey(sei.simulatorName))
                                            continue;
                                        StringBuilder b = new StringBuilder();
                                        b.AppendFormat("SimCore Metric: Total time in: {0} was {1} seconds which is {2}% of total simulator time.", sei.simulatorName, simTimes[sei.simulatorName] / 10000000.0, (((double)simTimes[sei.simulatorName]) / total) * 100.0);
                                        PerformanceLog.Write(b.ToString());
                                    }
                                }
                                state = SimCoreState.UNINITIALIZED;
                                return;

                        }
                    }
                    Thread.Sleep(updateFrequency / 10);
                }

            }
            catch (ThreadAbortException) { }
            catch (Exception exc)
            {
                MessageBox.Show("An error '" + exc.Message + "' has occurred in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                ErrorLog.Write(exc.ToString() + "\n");
                throw new Exception();
            }
        } 

    }
}
 