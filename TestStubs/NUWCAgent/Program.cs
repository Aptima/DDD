using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.TestStubs.NUWCAgent
{
    class Program
    {
        //string fileName = "baseline.txt";
        private static FileStream output;// = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
        private static StreamWriter fileWriter;// = new StreamWriter(output);

        public static NetworkClient nc = new NetworkClient();

        protected static void MyExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("MyExitHandler");
            nc.Disconnect();                
        }
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: NUWCAgent.exe HOSTNAME PORT ENDTIME");
                return;
            }
            
            string hostname = args[0];
            int port = Int32.Parse(args[1]);
            //int time;
            int endTime = (int)(float.Parse(args[2]) * 1000);
            //NetworkClient nc = new NetworkClient();
            nc.Connect(hostname, port);

            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(MyExitHandler);
            
            nc.Subscribe("RevealObject");
            nc.Subscribe("StateChange");
            nc.Subscribe("TimeTick");            
            List<SimulationEvent> events;

            string state;
            string id;
            int time;

            int revealCount = 0;
            int discoveredCount = 0;
            int pinpointedCount = 0;
            SimulationEvent stateChange = null;

            bool done = false;
            float ratio = 0;

            string fileName = "baseline.txt";
            output = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fileWriter = new StreamWriter(output);
            int count = 0;
            while (nc.IsConnected())
            {
                //output = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                //fileWriter = new StreamWriter(output);
                events = nc.GetEvents();

                foreach (SimulationEvent e in events)
                {
                    if (e.eventType == "RevealObject")
                    {
                        count++;
                    }
                    if (e.eventType == "TimeTick")
                    {
                        if (((IntegerValue)e["Time"]).value == 13000)
                        {
                            Console.Out.WriteLine(String.Format("Adam's reveal object count: {0}", count));
                        }
                    }
                    switch (e.eventType)
                    {
                        case "TimeTick":
                            time = ((IntegerValue)e["Time"]).value;

                            if (time >= endTime && !done)
                            {

                                if (revealCount >= 0)
                                {
                                    ratio = (float)discoveredCount / (float)revealCount;
                                    Console.WriteLine(String.Format("Detection ratio = {0}",ratio));
                                    if (stateChange != null)
                                    {
                                        if (ratio >= 0.6)
                                        {
                                            ((StringValue)stateChange["ObjectID"]).value = "Michigan";
                                            ((StringValue)stateChange["NewState"]).value = "Go";
                                        }
                                        else
                                        {
                                            ((StringValue)stateChange["ObjectID"]).value = "Michigan";
                                            ((StringValue)stateChange["NewState"]).value = "NoGo";
                                        }
                                        nc.PutEvent(stateChange);
                                        done = true;
                                    }
                                }

                                //nc.Disconnect();
                                //return;
                                break;

                            }
                            break;
                        case "RevealObject":
                            id = ((StringValue)e["ObjectID"]).value;
                            //Console.WriteLine(SimulationEventFactory.XMLSerialize(e));
                            Console.WriteLine(String.Format("RevealObject: {0}", id));
                            fileWriter.WriteLine("RevealObject: {0}", id);
                            int intID;
                            try
                            {
                                intID = Convert.ToInt32(id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(String.Format("Error in RevealObject: id = {0}", id));
                                break;
                            }
                            revealCount += 1;
                            break;
                        case "StateChange":
                            id = ((StringValue)e["ObjectID"]).value;
                            state = ((StringValue)e["NewState"]).value;
                            Console.WriteLine(String.Format("StateChange: {0} to {1}", id, state));
                            fileWriter.WriteLine(String.Format("StateChange: {0} to {1}", id, state));
                            int intID2;
                            try
                            {
                                intID2 = Convert.ToInt32(id);
                            }
                            catch (Exception ex)
                            {
                                stateChange = e;
                                Console.WriteLine(String.Format("Error in StateChange: id = {0}", id));
                                break;
                            }
                            if (state == "Discovered")
                            {
                                discoveredCount += 1;
                                Console.WriteLine(String.Format("Number of objects Discovered by LMRS_1: {0}", discoveredCount));                                
                                fileWriter.WriteLine(String.Format("Number of objects Discovered by LMRS_1: {0}", discoveredCount));                                
                            }
                            if (state == "Pinpointed")
                            {
                                pinpointedCount += 1;
                                Console.WriteLine(String.Format("Number of objects Pinpointed by LMRS_1: {0}", pinpointedCount));
                                fileWriter.WriteLine(String.Format("Number of objects Pinpointed by LMRS_1: {0}", pinpointedCount));
                            }
                            break;
                    }
                    
                }
                
                Thread.Sleep(500);
            }
            fileWriter.WriteLine(String.Format("Total Objects Revealed:  {0}", revealCount));
            fileWriter.WriteLine(String.Format("Total Objects Discovered:  {0}", discoveredCount));
            fileWriter.WriteLine(String.Format("Total Objects Pinpointed:  {0}", pinpointedCount));
            fileWriter.Close();
            output.Close();
            return;
        }
    }
}