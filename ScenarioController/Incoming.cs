using System;
using System.Collections.Generic;
 
namespace DDD.ScenarioController
{
    /// <summary>
    /// Describes the queue that holds events from below
    /// </summary>

    public class IncomingItemType
    { // this is how items on the queue are returned
        private ScenarioEventType theEvent;
        public ScenarioEventType TheEvent
        {
            get
            { return this.theEvent; }
            set
            { this.theEvent = value; }
        }
        private int theIndex;
        public int TheIndex
        {
            get
            { return this.theIndex; }
        }
        public IncomingItemType(ScenarioEventType theEvent, int theIndex)
        {
            this.theEvent = theEvent;
            this.theIndex = theIndex;
        }

    }
    public class IncomingList
    {

        private static List<ScenarioEventType> incoming = new List<ScenarioEventType>();
        public static void Add(ScenarioEventType e)
        {
            incoming.Add(e);

        }
 
        public static List<string> AffectedUnits()
        {
            List<string> returnList=new List<string>();
            for (int i = 0; i < incoming.Count; i++)
            {
                string u = incoming[i].UnitID;
                if (!returnList.Contains(u))
                {
                    returnList.Add(u);
                }
             }
             return returnList;
          }
  
 
        
        public static List<IncomingItemType> Retrieve(string u)
        { // look for all items with unit id u
            // return list of same with positions
            // return void if none found empty list
            List<IncomingItemType> returnList = new List<IncomingItemType>();
            for (int i = 0; i < incoming.Count; i++)
            {
                if (incoming[i].UnitID == u)
                {
                    IncomingItemType incomingItem = new IncomingItemType(incoming[i], i);
                    returnList.Add(incomingItem);
                }
            }
            return returnList;

        }
        public static void RemoveUnit(List<int> indexList)
        {
            indexList.Sort();
            for (int i = indexList.Count - 1; i >= 0; i--)
            {
                incoming.RemoveAt(indexList[i]);
            }

    }
    }
}
