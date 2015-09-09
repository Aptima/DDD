using System;
using System.Collections.Generic;
using System.Text;

using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.SimulationModelTools;

using DDD.CommonComponents.DataTypeTools;
using DDD.CommonComponents.NetworkTools;

namespace ScenarioController
{
    public class QueueManager
    {
        private static QueueManager _uniqueInstance = null;
        private static Dictionary<int?, List<SimulationEvent>> EventQueue = new Dictionary<int?, List<SimulationEvent>>();


        public static QueueManager UniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new QueueManager();

            return _uniqueInstance;
        }

        protected QueueManager()
        { }

        public void AddEvent(int? time, SimulationEvent theEvent)
        {
            if (EventQueue.ContainsKey(time))
            { //Another entry with this key exists, add onto list (done below)
            }
            else 
            {//No entries exist with this key, create new list, stick it into Queue
                List<SimulationEvent> tempList = new List<SimulationEvent>();
                EventQueue[time] = tempList;
            }

            EventQueue[time].Add(theEvent);
        }

        public static void sendEventsAtTime(int? time, NetworkClient target)
        { 
            //while loop at removes and sends to SimCore all events at time = time
            while (EventQueue.ContainsKey(time) && EventQueue[time].Count > 0)
            {
                Console.WriteLine("Sending...");
                Console.WriteLine(SimulationEventFactory.XMLSerialize(EventQueue[time][0]));
                target.PutEvent(EventQueue[time][0]);
                EventQueue[time].RemoveAt(0);
            
            }
        }

        public int count()
        {
            return EventQueue.Count; //returns the number of lists, not number of events
        }

        public bool eventsAtTime(int? time)
        {
            bool returnVal = false;

            if (EventQueue.ContainsKey(time)) returnVal = true;

            return returnVal;
        }
    }
}
