using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

 

namespace DDD.ScenarioController
{
    /// <summary>
    /// What to do on each tick of the SceneCon clock
    /// We do the events for the current time (starting at 0) and then 
    /// bump the timer
    /// </summary>
    public class TimerTicker
    {
        private static int timer = 0;
        public static int Timer
        {
            get
            { return timer; }
            set
            { timer = value; }
        }
        public static void NextTick()
        {
            while (readParse.sink.ShouldIWait())
            {
                Thread.Sleep(100);
            }
/* 1. Process the incoming list of events from below */
            // Which units are currently on the incoming list
            List<string> unitList = IncomingList.AffectedUnits();
            for (int i = 0; i < unitList.Count; i++)
            {
                List<IncomingItemType> eventList = IncomingList.Retrieve(unitList[i]);
  

                /* do things with these events */
                for (int j = 0; j < eventList.Count; j++)
                {
                    ScenarioEventType  thisItem = eventList[j].TheEvent;
                    if(thisItem.GetType()==typeof(MoveComplete_Event))
                    {
                        Console.WriteLine("Unit {0} MoveComplete discovered at t={1}", thisItem.UnitID, timer);
                            // scan happenings list for matches
                            for (int h = HappeningList.Happenings.Count-1; 0<=h ; h--)
                            {
                                ScenarioEventType incident = HappeningList.Happenings[h];
                                if (incident.UnitID == thisItem.UnitID)
                                {
                                    ScenarioEventType newEvent = ((HappeningCompletionType)incident).DoThis;
                                   newEvent.Timer = timer;
                                    TimerQueueClass.Add(timer, newEvent);
                                    HappeningList.Happenings.RemoveAt(h);

                                }
                            }// all happenings affected by this Move_complete have been queued
                    }
                    else
                    {
                            Console.WriteLine("Unknown incoming list item {0}", thisItem.GetType());
                    }
              
                }

                Console.WriteLine("Unit {0} has {1} elements", unitList[i], eventList.Count);
        /* and then take them off the incoming list */
                List<int> indexList = new List<int>();
                for (int j = 0; j < eventList.Count; j++)
                {
                    indexList.Add(eventList[j].TheIndex);
                }
                IncomingList.RemoveUnit(indexList);
            }
/* 2. Send the tick */
            EventCommunicator.SendEvent(new TickEventType(timer));
/* 3. Pull the events for this tick off the queue and handle them */
            List<ScenarioEventType> events = TimerQueueClass.RetrieveEvents(timer);

            if (events != null)
            {
                for (int v = 0; v < events.Count; v++)
                {
                    EventCommunicator.SendEvent(events[v]);
                    //Console.WriteLine("Timer {0} unit {1} type {2}", timer, events[v].UnitID, events[v].GetType().FullName);
                }
            }


/* Very Last Thing                   */
            timer += 1;
        }






    }
}
