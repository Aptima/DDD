using System;
using System.IO;
using System.Collections.Generic;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
namespace Aptima.Asim.DDD.ScenarioController
{

    /// <summary>
    /// Tracks all approval commands.
    /// (Initially for AttackRequest Approvals, but allowing for other types in future)
    /// </summary>
    public class ApprovalsList
    {
        private static List<ScenarioEventType> approvals = new List<ScenarioEventType>();
        public static List<ScenarioEventType> Approvals
        {
            get { return approvals; }

        }
        public static void Add(ScenarioEventType s)
        {
            approvals.Add(s);
        }
        public static Boolean AttackApproved(ScenarioEventType s)
        {
            Boolean returnValue = false;
            /*
             * Approval algorithm;
             * If there are no matches on list, return check default condition
             * If there are any matches then any one that gives permission is enough
             * */

            Boolean denied = false;
            if ("Aptima.Asim.DDD.ScenarioController.AttackObjectRequestType" != s.GetType().ToString())
                return returnValue;
            AttackObjectRequestType alias = (AttackObjectRequestType)s;
            for (int i = 0; i < approvals.Count; i++)
            {
                ScenarioEventType candidate = approvals[i];
                if ("Aptima.Asim.DDD.ScenarioController.AttackRequestApprovalType" != candidate.GetType().ToString())
                    continue;
                AttackRequestApprovalType rule = (AttackRequestApprovalType)candidate;
                if (rule.Capability != alias.CapabilityName)
                    continue;

                if (rule.Actor != "")
                {
                    if ((!rule.ActorIsSpecies) && (rule.Actor != alias.UnitID))
                    {

                        continue;
                    }
                    if (rule.ActorIsSpecies && !Genealogy.UnderSpecies(alias.UnitID, rule.Actor))
                    {
                        continue;
                    }

                }
                if (rule.Target != "")
                {
                    if ((!rule.TargetIsSpecies) && (rule.Target != alias.TargetObjectID))
                    {
                        continue;
                    }
                    if (rule.TargetIsSpecies && !Genealogy.UnderSpecies(alias.TargetObjectID, rule.Target))
                    {
                        continue;
                    }

                }
                if (rule.TargetStates.Count != 0)
                {
                    if (!rule.TargetStates.Contains(UnitFacts.CurrentUnitStates[alias.TargetObjectID]))
                        continue;
                }
                // At this point we can say that the rule legitimaly refers to alias.UnitID
                if (!rule.EngramValid(alias.UnitID, alias.TargetObjectID))
                {
                    denied = true;
                    continue;
                }
                if ((rule.UseDefault) && (alias.DecisionMaker != UnitFacts.GetDM(alias.UnitID)))
                {
                    denied = true;
                    continue;
                }
                //Wow. This rule was passed
                returnValue = true;
                break;

            }
            if (!returnValue && !denied)
            {// Now use the default rule
                DecisionMakerType unitDM = DecisionMakerType.GetDM(UnitFacts.GetDM(alias.UnitID));
                if (
                    (alias.DecisionMaker == unitDM.Identifier)
          // &&
           //(UnitFacts.HostileTo(unitDM.Team, DecisionMakerType.GetDM(UnitFacts.GetDM(alias.TargetObjectID)).Team))
                    //AD: Changed to allow for non-hostiles to be engaged, as they are requested by live players.
                    )
                    returnValue = true;
            }
            return returnValue;
        }
    }

    //------------------------- Happening List -----------
    /// <summary>
    /// The list of  events that are contingent upon state changes of various kinds
    /// </summary>
    public class HappeningList
    {
        private static List<HappeningCompletionType> happenings = new List<HappeningCompletionType>();
        public static List<HappeningCompletionType> Happenings
        {
            get
            { return happenings; }
        }
        /// <summary>
        /// Adds an event to the list of contingent events
        /// </summary>
        /// <param name="happeningEvent">Event to add</param>
        public static void Add(HappeningCompletionType happeningEvent)
        {
            happenings.Add(happeningEvent);
        }

        public static void FlushOneUnit(string s)
        {
            // removes all events involving one unit s.UnitID
            List<int> removalList = new List<int>();
            for (int i = 0; i < happenings.Count; i++)
            {
                if (happenings[i].AllUnits.Contains(s))
                {
                    removalList.Add(i);

                }

            }

            for (int i = removalList.Count - 1; 0 <= i; i--)
            {// count back to avound makinhg indices absolute11
                happenings.RemoveAt(i);
            }
        }

        /*
               public static void FlushAllUnits(ScenarioEventType s)
               {
         * // removs all happenin events for all units mentioned in s from the happenings queue
                   List<int> removalList = new List<int>();
                   List<string> allUnits = s.AllUnits;
                   for (int i = 0; i < happenings.Count; i++)
                   {
                       List<string> happeningsUnits = happenings[i].AllUnits;
                       for (int j = 0; j < happeningsUnits.Count; j++)
                       {
                           if (allUnits.Contains(happeningsUnits[j]))
                           {
                               removalList.Add(i);

                               break;
                           }
                       }
                   }
                   for (int i = removalList.Count - 1; 0 <= i; i--)
                   {// count back to avound makinhg indices absolute11
                       happenings.RemoveAt(i);
                   }
               }
               */
    }



    public class SpeciesHappeningList
    {
        private static List<SpeciesCompletionType> happenings = new List<SpeciesCompletionType>();
        public static List<SpeciesCompletionType> Happenings
        {
            get
            { return happenings; }
        }
        /// <summary>
        /// Adds an event to the list of contingent events
        /// </summary>
        /// <param name="happeningEvent">Event to add</param>
        public static void Add(SpeciesCompletionType happeningEvent)
        {
            happenings.Add(happeningEvent);
        }
    }


    public class AttackSuccessfulList
    {
        private static List<AttackSuccessfulCompletionType> happenings = new List<AttackSuccessfulCompletionType>();
        public static List<AttackSuccessfulCompletionType> Happenings
        {
            get
            { return happenings; }
        }
        /// <summary>
        /// Adds an event to the list of contingent events
        /// </summary>
        /// <param name="happeningEvent">Event to add</param>
        public static void Add(AttackSuccessfulCompletionType happeningEvent)
        {
            happenings.Add(happeningEvent);
        }
    }


    // ------------------------ Timer Queue ------------------/
    // -------------------- TimeNodeClass ------------
    /// <summary>
    /// The format of each node on the timer queue
    /// </summary>
    public class TimeNodeClass
    {
        private int triggerTime;
        public int TriggerTime
        {
            get
            { return this.triggerTime; }
        }

        private Dictionary<string, int> theseUnits;
        public Dictionary<string, int> TheseUnits
        {
            get
            { return theseUnits; }
        }

        /// <remarks>
        /// The list theseUnits structure -- one for each time -- maintains a list of the units affected
        /// at this time period, but does not point to their events
        /// </remarks>
        /// 
        private List<ScenarioEventType> theseEvents;
        public List<ScenarioEventType> TheseEvents
        {
            get
            { return theseEvents; }
        }
        /// <summary>
        /// Removes all events involving a given unit from the node
        /// </summary>
        /// <param name="unit">unit Identifier</param>
        /// <returns>true if node has no events</returns>
        public Boolean FlushUnit(string unit)
        {

            if (theseUnits.ContainsKey(unit))
            {
                List<ScenarioEventType> newList = new List<ScenarioEventType>();
                for (int i = 0; i < theseEvents.Count; i++)
                {
                    if (!theseEvents[i].AllUnits.Contains(unit))
                    {
                        newList.Add(theseEvents[i]);
                    }
                }
                theseUnits.Remove(unit);
                theseEvents = newList;
            }
            return (0 == theseEvents.Count);

        }

        /// <summary>
        /// Constructs a timer queue node. This is a list of all the events due to fire at this particular time.
        /// </summary>
        /// <param name="triggerTime">Time at which event is fired</param>
        public TimeNodeClass(int triggerTime) // like all times, this is an absolute time
        {
            this.triggerTime = triggerTime;
            theseUnits = new Dictionary<string, int>();
            // each unit is mapped to the number of times it appears in the queue
            theseEvents = new List<ScenarioEventType>();
            /* ?? to allow explicit dispose           GC.SuppressFinalize(this); */
        }
        /// <summary>
        /// Adds an event to this node
        /// </summary>
        /// <param name="e"></param>
        public void AddEvent(ScenarioEventType e)
        {
            theseEvents.Add(e);
            if (e is ScenarioEventType)
            {

                if (!(theseUnits.ContainsKey(e.UnitID)))
                {
                    theseUnits.Add(e.UnitID, 1);
                }
                else
                {
                    theseUnits[e.UnitID] += 1;
                }

                List<string> allUnits = e.AllUnits;
                for (int other = 0; other < allUnits.Count; other++)
                {
                    if (!(TheseUnits.ContainsKey(allUnits[other])))
                    {
                        theseUnits.Add(allUnits[other], 1);
                    }
                    else
                    {
                        theseUnits[allUnits[other]] += 1;
                    }

                }
            }
        }
        /// <summary>
        /// Removes an event from this node
        /// </summary>
        /// <param name="index"></param>
        public void DropEvent(int index)
        {
            ScenarioEventType e = theseEvents[(int)index];

            theseUnits[e.UnitID] -= 1;
            if (theseUnits[e.UnitID] <= 0)
            {
                theseUnits.Remove(e.UnitID);
            }
            theseEvents.RemoveAt((int)index);

        }

    }

    /// <summary>
    /// The list of events to which specific times have been assigned
    /// </summary>
    public class TimerQueueClass
    {
        private static Dictionary<int, TimeNodeClass> queue = new Dictionary<int, TimeNodeClass>();

        /*for debugging purposes need access to the queue*/
        public static Dictionary<int, TimeNodeClass> Queue
        {
            get { return queue; }
        }

        private static Queue<RootEventType> primaryImmediateQueue = new Queue<RootEventType>();
        private static Queue<RootEventType> secondaryImmediateQueue = new Queue<RootEventType>();


        /// <summary>
        /// Add an event to the time queue
        /// </summary>
        /// <param name="time">The time at which this event fires</param>
        /// <param name="newEvent">The event to add</param>
        public static void Add(int time, ScenarioEventType newEvent)
        {
            time = Math.Max(_lastRetrievedTime + 1, time); //AD: put in place so you don't enqueue an event in the past
            //that should be sent out on the next time tick.
            if (!queue.ContainsKey(time))
            {
                queue.Add(time, new TimeNodeClass(time));
            }
            queue[time].AddEvent(newEvent);
        }

        private static int _lastRetrievedTime = 0;
        /// <summary>
        /// Returns the list of events scheduled for this time
        /// </summary>
        /// <param name="t">The time</param>
        /// <returns></returns>
        public static List<ScenarioEventType> RetrieveEvents(int t)
        {
            _lastRetrievedTime = t;
            if (queue.ContainsKey(t))
            {
                return queue[t].TheseEvents;
            }
            return null;
        }
        /// <summary>
        /// Removes an entire node from the tree
        /// </summary>
        /// <param name="t">time that indexes the node</param>
        public static void DropNode(int t)
        {
            queue.Remove(t);
        }
        /// <summary>
        /// Removes a single event from the queue
        /// </summary>
        /// <param name="t">The time at which to look for the event</param>
        /// <param name="p">The event</param>
        public static void DropEvent(int t, int p)
        {
            queue[t].DropEvent(p);
        }
        /// <summary>
        /// Makse sure events on immediate Queue are sent befoe first tick
        /// </summary>
        /// <param name="r">event to send</param>
        /// <remarks>The intent is for this to be used only before the queue starts to send messages of coordination</remarks>
        public static void SendBeforeStartup(RootEventType r)
        {
            primaryImmediateQueue.Enqueue(r);
        }
        public static void SecondarySendBeforeStartup(RootEventType r)
        {
            secondaryImmediateQueue.Enqueue(r);
        }

        /// <summary>
        /// Send those events that were required before ticking proper begins
        /// </summary>
        public static void SendImmediates()
        {
            while (primaryImmediateQueue.Count > 0)
            {
                EventCommunicator.SendEvent(primaryImmediateQueue.Dequeue());
            }
            while (secondaryImmediateQueue.Count > 0)
            {
                RootEventType r = secondaryImmediateQueue.Dequeue();
                r.Time = 0; // Even if specified as 1.
                if (r is ScenarioEventType)
                {
                    if (((ScenarioEventType)r).Range != null)
                    {
                        if (!((ScenarioEventType)r).EngramValid("", ""))
                        {
                            Coordinator.debugLogger.Writeline("TimerQueueClass", "Time: SecondaryImmediate event ignored due to value of " + ((ScenarioEventType)r).Range.Name, "test");
                            continue;
                        }
                    }
                }
                EventCommunicator.SendEvent(r);

            }
            EventCommunicator.SendEvent(new StartupCompleteNotice());

        }

        public static void FlushOneUnit(string s)
        {

            List<int> removalList = new List<int>(); // cannot remove from set during a for...in loop
            foreach (int t in queue.Keys)
            {

                Boolean empty = queue[t].FlushUnit(s);
                if (empty)
                {
                    removalList.Add(t);
                }
            }
            for (int t = 0; t < removalList.Count; t++)
            {
                queue.Remove(removalList[t]);
            }
        }


        /// <summary>
        /// Remove from the queue all events that are referenced by the input
        /// </summary>
        /// <param name="s">an event</param>
        /// 
        /*
        public static void FlushUnits(ScenarioEventType s)
        {
            List<string> allUnits = s.AllUnits;
            List<int> removalList = new List<int>(); // cannot remove from set during a for...in loop
            foreach (int t in queue.Keys)
            {
                foreach (string unit in allUnits)
                {

                    Boolean empty = queue[t].FlushUnit(unit);
                    if (empty)
                    {
                        removalList.Add(t);
                    }

                }
            }
            for (int t = 0; t < removalList.Count; t++)
            {
                queue.Remove(removalList[t]);
            }
        }
        */
        public static void Clear()
        {
            queue.Clear();
            primaryImmediateQueue.Clear();//need to clear these between scenario runs too.
            secondaryImmediateQueue.Clear();
            _lastRetrievedTime = 0;
        }
    }



}
