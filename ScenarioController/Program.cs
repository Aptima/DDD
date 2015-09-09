using System;
 
using System.IO;
using System.Xml;
using DDD.CommonComponents;
using System.Xml.Schema;
 
using System.Threading;
using DDD.CommonComponents.NetworkTools;
using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.SimulationModelTools;

namespace  DDD.ScenarioController
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class readParse
    {
        public static Watcher sink;

        /*
            STAThread means the app uses Single-Threaded Apartment threading. 
            It specifies the threading model used when you use COM interop. 
            Likely it won't affect your program, since COM isn't usually 
            needed in .NET in most cases.
    */
        [STAThread]
        static void Main(string[] args)
        {
            string scenarioFile = args[0];
            new ScenarioToQueues(scenarioFile);
 //           string hostname = "dgeller";
            string hostname=args[1];
//            int port = 9999;
            int port = int.Parse(args[2]);
 //           string simModelName = "SimulationModel.xml";
            string simModelName = args[3];
            NetworkClient c = new NetworkClient();
            c.Connect(hostname, port);
            EventCommunicator eventCommunicator = new EventCommunicator(c);

            SimulationModelReader smr = new SimulationModelReader();
            SimulationModelInfo simModelInfo = smr.readModel(simModelName);

            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);



            sink = new Watcher(400);
            ThreadStart stub = new ThreadStart(sink.WatcherThread);
            Thread stubThread = new Thread(stub);
            stubThread.Start();


            for (int i = 0; i < 5; i++) // in test the move happens at time 2
            {
                TimerTicker.NextTick();
            }
            IncomingList.Add(new MoveComplete_Event("UNIT0"));
            for (int i = 0; i < 2; i++)
            {
                TimerTicker.NextTick();
            }
 
            Console.WriteLine("The end");


        }





    }
}





