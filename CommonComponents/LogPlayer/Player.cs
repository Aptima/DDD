using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using System.Windows.Forms;


namespace Aptima.Asim.DDD.CommonComponents.LogPlayer
{
    public class Player
    {
        private Thread playerThread;
        //private NetworkClient nc;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;
        private int time;
        private bool paused;
        private int updateFrequency;
        private bool loop;
        private string logname;
        private double replaySpeed;
        private bool isReady;
        private string productVersion = "Unknown product version";
        private string compileDate = "Unknown compile date";

        private List<SimulationEvent> events;


        public Player(string  productVersion,string compilerDate):this()
        {
            this.productVersion = productVersion;
            this.compileDate = compileDate;
            
        }
        public Player()
        {
          
            playerThread = null;
            distClient = null;
            simModel = null;
            time = 0;
            paused = false;
            updateFrequency = 0;
            events = new List<SimulationEvent>();
            loop = false;
            logname = null;
            isReady = false;
            replaySpeed = 1;
        }

        private void LoadEvents(string fileName)
        {
            StreamReader re = File.OpenText(fileName);
            string input = null;
            SimulationEvent ev = null;
            events.Clear();

            input = re.ReadLine(); // thriow away the line with the version number


            while ((input = re.ReadLine()) != null)
            {
                try
                {
                    ev = SimulationEventFactory.XMLDeserialize(input);
                    if (SimulationEventFactory.ValidateEvent(ref simModel, ev))
                    {
                        if (simModel.eventModel.events[ev.eventType].shouldReplay)
                        {
                            events.Add(ev);
                        }
                    }
                    else
                    {
                        throw new Exception("error reading: " + input);
                    }
                }
                catch (Exception e)
                {
                    ErrorLog.Write(String.Format("NONFATAL Deserialize Error in LogPlayer.Player: {0}", input));
                    ErrorLog.Write(e.ToString());
                }
            }
            re.Close();
        }

        public bool IsDone()
        {
            if (events.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsReady()
        {
            return isReady;
        }
        public void Start(SimulationModelInfo simModel, ref SimulationEventDistributor distributor, string logName, bool loop)
        {
            try
            {
                playerThread = new Thread(new ThreadStart(EventLoop));
                //nc = new NetworkClient();
                //nc.Connect(hostname, port);
                distClient = new SimulationEventDistributorClient();
                distributor.RegisterClient(ref distClient);
                this.simModel = simModel;
                time = 0;
                this.loop = loop;
                this.logname = logName;
                updateFrequency = simModel.simulationExecutionModel.updateFrequency;

                SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "ResetSimulation");
                distClient.PutEvent(ev);
                LoadEvents(logName);

                playerThread.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetSpeed(double speed)
        {
            replaySpeed = speed;
        }

        public void Stop()
        {
            if (playerThread != null)
            {
                playerThread.Abort();
                playerThread = null;
            }

            if (distClient != null)
            {
                distClient.Disconnect();
                distClient = null;
            }
            simModel = null;
            isReady = false;

        }

        public void Pause()
        {
            paused = true;
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "PauseScenario");
            e["Time"] = DataValueFactory.BuildInteger(time);

            if (distClient != null)
            {
                distClient.PutEvent(e);
            }
        }

        public void Resume()
        {
            paused = false;
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "ResumeScenario");
            e["Time"] = DataValueFactory.BuildInteger(time);

            if (distClient != null)
            {
                distClient.PutEvent(e);
            }
        }

        private void EventLoop()
        {
            try
            {
                SimulationEvent ev;
                while (distClient != null)
                {
                    if (!paused)
                    {
                        while (events.Count > 0)
                        {
                            ev = events[0];
                            if (((IntegerValue)ev["Time"]).value >= time &&
                                ((IntegerValue)ev["Time"]).value < (time + updateFrequency))
                            {
                                events.Remove(ev);
                                distClient.PutEvent(ev);
                            }
                            else
                            {
                                break;
                            }
                        }

                        time += updateFrequency;
                        if (time == updateFrequency)
                        {//first tick done
                            isReady = true;
                        }
                    }
                    Thread.Sleep((int)(updateFrequency / replaySpeed));

                    if (IsDone() && loop)
                    {
                        ev = SimulationEventFactory.BuildEvent(ref simModel, "ResetSimulation");
                        distClient.PutEvent(ev);
                        time = 0;
                        LoadEvents(logname);
                    }


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
