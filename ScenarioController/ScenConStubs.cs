using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
 

namespace DDD.ScenarioController
{/// <summary>
    /// Stubs for testin scenario conreoller
    /// </summary>
 /*   public class EventCommunicator
    {
        public static void SendEvent(RootEventType r)
        {
            Console.WriteLine(r.GetType().FullName);
            readParse.sink.EventGetter(r);
        }


    }
  */ 


    public class Watcher
    {
        private bool wait = false;
        private  int delta;
        private  int lastTimer = -1;
        private  int thisTimer = -1;
        private  int wakeup = 0;
        public Watcher(int inputDelta)
        {
            delta = inputDelta;
        }
        public void WatcherThread()
        {
            while (wakeup < 100)
            {
                if (lastTimer != thisTimer)
                {
                    Console.WriteLine("Received tick {0}", thisTimer);
                    lastTimer = thisTimer;
                    wait = false;
                }
                Thread.Sleep(delta);
                wakeup += 1;
                
            }
        }
        public bool ShouldIWait()
        {
            return wait;
        }
        public  void EventGetter(RootEventType r)
        {
            if (r.GetType() == typeof(TickEventType))
            {
                thisTimer = r.Timer;
                wait = true;
            }

        }
    }
}