using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.ScenarioController
{
    // Stubs for testing scenario conreoller
    /// <Summary>
    /// Event Communicator is a stub; On receiving an event to sebd
    /// it simply prints the event to console
    /// </summary>
    /*   public class EventCommunicator
       {
           public static void SendEvent(RootEventType r)
           {
               Coordinator.debugLogger.WriteLine(r.GetType().FullName);
               readParse.sink.EventGetter(r);
           }


       }
     */

    /// <summary>
    /// The watcher runs as a separate thread. It sleeps for a period of time
    /// and then wakes up to see if its timer has been changed (by EventGetter)
    /// </summary>
    public class Watcher
    {
        private bool wait = false;
        private int delta;
        private int lastTimer = -1;
        private int thisTimer = -1;
        private int wakeup = 0;
        /// <summary>
        /// Constructs a watcher
        /// </summary>
        /// <param name="inputDelta">Time to sleep between checking ticks</param>
        public Watcher(int inputDelta)
        {
            delta = inputDelta;
        }
        /// <summary>
        /// The process that is run as a separate thread
        /// </summary>
        public void WatcherThread()
        {
            while (wakeup < 10000)
            {
                if (lastTimer != thisTimer)
                {
                    Coordinator.debugLogger.Writeline("ScenConStubs","Received tick"+ thisTimer.ToString(), "test");
                    lastTimer = thisTimer;
                    wait = false;
                }
                Thread.Sleep(delta);
                wakeup += 1;

            }
        }
        /// <summary>
        /// Tells whether the timer has changed (as known by WatcherThread
        /// </summary>
        /// <returns>True if the timer has not changed</returns>
        public bool ShouldIWait()
        {
            return wait;
        }
        /// <summary>
        /// Gets events but ignores all but timer ticke
        /// </summary>
        /// <param name="r">The event</param>
        public void EventGetter(RootEventType r)
        {
            if (r.GetType() == typeof(TickEventType))
            {
                thisTimer = r.Time;
                wait = true;
            }

        }
    }

    public class TickWatcher
    {
        private static NetworkClient server = null;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private int thisTimer = 0;

        /// <summary>
        /// Constructs a watcher
        /// </summary>
        /// <param name="inputDelta">Time to sleep between checking ticks</param>
        public TickWatcher(NetworkClient s, string simModel)
        {
            string simModelName = simModel;
            simModelInfo = smr.readModel(simModelName);
            server = s;
            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);/////////////////
            server.Subscribe("TimeTick");


        }

        /// <summary>
        /// Gets time tick events but ignores all but timer ticke
        /// </summary>
        /// <param name="r">The event</param>
        public void TickEventGetter()
        {
            List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
  
            while (true)
            {
                incomingEvents = server.GetEvents();
                if (0 < incomingEvents.Count)
                {
                    for (int j = 0; j < incomingEvents.Count; j++)
                    {
                        if ("TimeTick" == incomingEvents[j].eventType)
                        {
                            thisTimer += 1;
                        }
                        incomingEvents.Clear();
                    }
                
                    if (35 == thisTimer)
                    {
                        AttackObjectRequestType attackObjectRequest = new AttackObjectRequestType("BLU01", "3210", "RED01", "???");
                        IncomingList.Add(attackObjectRequest);
                    }
                    if (400 == thisTimer)
                    {
                        MoveObjectRequestType moveObjectRequest = new MoveObjectRequestType("BLU02", "3210", new LocationType(234, 16.6, 7), .65);
                        IncomingList.Add(moveObjectRequest);
                    }
                    if (41 == thisTimer)
                    {
                        StateChangeNotice stateChangeRequest = new StateChangeNotice("RED01", "Dead");

                        IncomingList.Add(stateChangeRequest);
                    }
                    if (43== thisTimer)
                    {
                        MoveObjectRequestType moveObjectRequest = new MoveObjectRequestType("RED01", "1800", new LocationType(234, 16.6, 7), .65);
                        IncomingList.Add(moveObjectRequest);
                    }
                }

                Thread.Sleep(100);

            }
        }
    }
}