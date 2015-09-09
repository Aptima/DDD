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
namespace Aptima.Asim.DDD.CommonComponents.ReplayLogger
{
    public class ReplayLogger
    {
        private SimulationEventDistributorClient cc = null;
        //private static NetworkClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static string logPath = "blank.txt";
        private static bool isRunning;
        private static string logMode;
        private static int actualTime = 0;
        private static string productVersion = "Unknown Version";
        private static string compileDate = "Unknown Date";
 
        public static void SetIsRunning(bool value)
        {
            isRunning = value;
        }
        public ReplayLogger(string logsPath, string simModelPath, ref SimulationEventDistributor distributor, string mode,string productVersion, string compileDate):this( logsPath,  simModelPath,  ref distributor,  mode)
        {
            ReplayLogger.productVersion=
                productVersion;
            ReplayLogger.compileDate=compileDate;
        }
        public ReplayLogger(string logsPath, string simModelPath, ref SimulationEventDistributor distributor, string mode)
        {
            logPath = logsPath;
            simModelName = simModelPath;
            simModelInfo = smr.readModel(simModelName);
            isRunning = false;
            logMode = mode;
            //server = new NetworkClient();
            //server.Connect(hostName, Convert.ToInt32(portNumber));
            cc = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref cc);
        }

        public void UpdateLogPath(string logsPath)
        {
            logPath = logsPath;
        }

        public void WriteToLog()
        {
            try
            {
                //SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
                //cc = new SimulationEventDistributorClient();

                List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
                FileStream file = new FileStream(logPath, FileMode.Create);
                StreamWriter sr = new StreamWriter(file);
                sr.WriteLine("<Creator><Version>" + productVersion + "</Version><CompiledOn>" + compileDate + "</CompiledOn></Creator>");
                actualTime = 0;
                //dist.RegisterClient(ref cc);
                foreach (string subString in simModelInfo.eventModel.events.Keys)
                {
                    //cc.Subscribe(subString);
                    //If the user wants the detailed log, or if the event is flagged with the shouldLog
                    //bool value, it should subscribe to that event.  Otherwise, don't subscribe.
                    if ((logMode == "Detailed") 
                        || simModelInfo.eventModel.events[subString].shouldLog)
                    {
                        cc.Subscribe(subString);
                    }
                }
                isRunning = true;
                while (isRunning)
                {
                    //incomingEvents = cc.GetEvents();
                    incomingEvents = cc.GetEvents();

                    if (incomingEvents.Count != 0)
                    {
                        foreach (SimulationEvent se in incomingEvents)
                        {
                            if (se.eventType == "TimeTick")
                            {
                                actualTime = ((IntegerValue)se["Time"]).value;
                            }
                            ((IntegerValue)se["Time"]).value = actualTime;
                            try
                            {
                                sr.WriteLine(SimulationEventFactory.XMLSerialize(se));
                            }
                            catch (Exception exc)
                            {
                                ErrorLog.Write(String.Format("NONFATAL Serialize Error in ReplayLogger: {0}", se.eventType));
                                ErrorLog.Write(exc.ToString());
                                continue;
                            }
                            sr.Flush();

                        }

                        incomingEvents.Clear();

                    }

                    Thread.Sleep(100);


                }
                sr.Close();
                file.Close();
                //dist.RemoveClient(cc.id);  //is this correct?
                //server.Disconnect();

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
