using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.CommonComponents.ServerViewers
{
    public class EventStreamViewer
    {
        private string eventStream;
        private static object streamLock = new object();

        public EventStreamViewer()
        {
            eventStream = string.Empty;
        }

        public void AddEvent(string eventType, int time)
        {
            lock (streamLock)
            {
                eventStream = eventStream.Insert(0, String.Format("{0} at time {1}.\r\n", eventType, time));
            }
        }

        public string GetStream()
        {
            lock (streamLock)
            {
                return eventStream;
            }
        }
    }
}
