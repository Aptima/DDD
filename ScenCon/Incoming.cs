using System;
using System.Collections.Generic;

namespace Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// Type for items removed from the Incoming event list.
    /// </summary>
    public class IncomingItemType{
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
        /// <summary>
        /// Constructs an item being returned from the Incoming list
        /// This is a container for the actual event
        /// </summary>
        /// <param name="theEvent">The event fas ound on the list</param>
        /// <param name="theIndex">The sequential position of the event</param>
        public IncomingItemType(ScenarioEventType theEvent, int theIndex)
        {
            this.theEvent = theEvent;
            this.theIndex = theIndex;
        }

    }
    /// <summary>
    /// The list of incoming events
    /// Items placed on this derive from events published by lower levels
    /// </summary>
    public class IncomingList
    {
        private static List<ScenarioEventType> incoming = new List<ScenarioEventType>();
        private static object syncObject = new object();
/// <summary>
/// Adds an item to the Incoming event list
/// </summary>
/// <param name="e">An event (published by a lower level)</param>
        public static void Add(ScenarioEventType e)
        {
            lock(syncObject)
            {
            incoming.Add(e);
            }
        }
        /// <summary>
        /// Produces a list of the units affected by at least one of the events on the Incoming list
        /// </summary>
        /// <returns>A list of unit identifiers</returns>
        public static List<string> AffectedUnits()
        {
            lock(syncObject)
            {
            List<string> returnList=new List<string>();
            for (int i = 0; i < incoming.Count; i++)
            {
                string u = incoming[i].UnitID;
                if (!returnList.Contains(u))
                {
                    returnList.Add(u);
                }
                List<string> otherList = incoming[i].AllUnits;
                for (int other = 0; other < otherList.Count; other++)
                {
                    if (!returnList.Contains(otherList[other]))
                    {
                        returnList.Add(otherList[other]);
                    }
                }
             }
             return returnList;
            }
          }
   /// <summary>
   /// Retrieves all the items on the incoming list that afect a given unit
   /// </summary>
   /// <param name="u"></param>
   /// <returns></returns>
        public static List<IncomingItemType> Retrieve(string u)
        { // look for all items with unit id u
            // return list of same with positions
            // return void if none found empty list
            lock(syncObject)
            {
            List<IncomingItemType> returnList = new List<IncomingItemType>();
            for (int i = 0; i < incoming.Count; i++)
            {
                if (incoming[i].UnitID == u)
                {
                    returnList.Add(new IncomingItemType(incoming[i], i));
                }
                else
                {
                    List<string> otherList = incoming[i].AllUnits;
                    if (otherList.Contains(u))
                        {
                            returnList.Add(new IncomingItemType(incoming[i], i));
                        }
                    
                }
            }
            return returnList;
            }
        }
        /// <summary>
        /// Removes a whole bunch of elements from the list
        /// </summary>
        /// <param name="indexList">List of indices of elements to remove</param>
        public static void RemoveUnit(List<int> indexList)
        {
            lock(syncObject)
            {
            indexList.Sort();
            for (int i = indexList.Count - 1; i >= 0; i--)
            {
                incoming.RemoveAt(indexList[i]);
            }

    }
    }
        public static void Clear()
        {
            incoming.Clear();
        }
    }
}
