using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Threading;

using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.SimulationModelTools;

using DDD.CommonComponents.DataTypeTools;
using DDD.CommonComponents.NetworkTools;

namespace ScenarioController
{
    static class Program
    {

        public static NetworkClient c = null;

        private static void MyExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("ScenCon exiting...");
            c.Disconnect();

            //args.Cancel = true;

        }

        [MTAThread]
        //[STAThread]
        static void Main(string[] args)
        {
            string hostname = args[0];
            int port = Int32.Parse(args[1]);
            string simModelName = args[2];

            SimulationModelReader smr = new SimulationModelReader();
            SimulationModelInfo simModelInfo = smr.readModel(simModelName);

            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);
            cc.Subscribe("ALL");

            ScenarioReader scenarioReader = new ScenarioReader();
            QueueManager queueManager = QueueManager.UniqueInstance();

            

            c = new NetworkClient();
            c.Connect(hostname, port);
            EventListener myEL = EventListener.UniqueInstance(c);
            int t = 0;
            int dt = simModelInfo.simulationExecutionModel.updateFrequency;

            SimulationEvent tick = SimulationEventFactory.BuildEvent(ref simModelInfo, "TimeTick");
            ((IntegerValue)tick["Time"]).value = t;

            ConsoleKeyInfo cki;
            //Console.TreatControlCAsInput = false;  //This explodes my code for some reason, but is in Gabe's client code and works fine, what does it do?
            Console.CancelKeyPress += new ConsoleCancelEventHandler(MyExitHandler);

            List<SimulationEvent> events = null;

            while (c.IsConnected() && queueManager.count() > 0)
            {

                //Read incoming events queue
                //if any events deal with a conditional event, remove the conditional
                //event from the conditional list, and place it onto the event queue
                //if a unit dies, remove them from the event queue and condition list

                while (c.IsConnected() && !(queueManager.eventsAtTime(t)))
                {
                    events = c.GetEvents();
                    foreach (SimulationEvent e in events)
                    {
                        if (e.eventType == "MoveDone")
                            c.PutEvent(myEL.MoveDoneReceived(e, simModelInfo, tick));

                        System.Console.WriteLine(SimulationEventFactory.XMLSerialize(e));
                    }



                    ((IntegerValue)tick["Time"]).value = t;
                    c.PutEvent(tick);
                    //Console.WriteLine("Sending...");
                    //Console.WriteLine(SimulationEventFactory.XMLSerialize(tick));
                    Thread.Sleep(dt);

                    t += dt;
                }

                if (c.IsConnected())
                {
                    QueueManager.sendEventsAtTime(t, c);
                    ((IntegerValue)tick["Time"]).value = t;
                    c.PutEvent(tick);
                    //Console.WriteLine("Sending...");
                    //Console.WriteLine(SimulationEventFactory.XMLSerialize(e));
                    t += dt;
                }
                


            }

            while (c.IsConnected())
            {
                ((IntegerValue)tick["Time"]).value = t;
                c.PutEvent(tick);
                //Console.WriteLine("Sending...");
                //Console.WriteLine(SimulationEventFactory.XMLSerialize(tick));
                Thread.Sleep(dt);

                t += dt;
            }
        }
    }
}