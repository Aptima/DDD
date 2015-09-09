using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.TestStubs.NetworkClientStub
{
    class Program
    {
        public static NetworkClient nc = new NetworkClient();
        protected static void MyExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("MyExitHandler");
            nc.Disconnect();
            
            //args.Cancel = true;
            
        }
        static void Main(string[] args)
        {
            string hostname = args[0];
            int port = Int32.Parse(args[1]);

            //NetworkClient nc = new NetworkClient();
            nc.Connect(hostname, port);

            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(MyExitHandler);
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(MyExitHandler,nc);
            //ConsoleCancelEventArgs
            



            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    nc.Subscribe(args[i]);
                }
            }
            else
            {
                nc.Subscribe("ALL");
            }
            List<SimulationEvent> events;

            while (nc.IsConnected())
            {
                events = nc.GetEvents();
                foreach (SimulationEvent e in events)
                {
                    System.Console.WriteLine(SimulationEventFactory.XMLSerialize(e));
                }

                Thread.Sleep(50);
            }

            return;
        }
    }
}
