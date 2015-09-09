using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;


namespace TestingTool
{
   
    public 
        class ConnectionManager
        {
            private EventListener eventListener;
            private NetworkClient netClient;
            public NetworkClient NetClient
            {
                get { return netClient; }
            }
            public static ConnectionManager MakeConnection(string hostName, int port)
            {
       
                // Create network client object
                NetworkClient nc = new NetworkClient();
                //Connect to DDD Server
                try
                {while (!nc.IsConnected())
                    {
                        Thread.Sleep(100);
                    nc.Connect(hostName, port);  
                    }
                }
                catch (SystemException e)
                {
                    //Replace with message box when move to windows
                    Console.WriteLine("Could not Connect because of " + e.Message);
                    Environment.Exit(1001);
                }
                Console.WriteLine("Made connection");



                ConnectionManager returnValue = new ConnectionManager();
                returnValue.netClient = nc;
                return returnValue;

            }
            public void StartListening()
            {
     
                eventListener = new EventListener();
                EventListener.Network = this.netClient;

                Thread listenerThread = new Thread(new ThreadStart(eventListener.StartListening));
                listenerThread.Start();

            }

        }
    }

