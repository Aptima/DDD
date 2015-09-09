using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;


namespace Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// What to do on each tick of the SceneCon clock
    /// We do the events for the current time (starting at 0) and then 
    /// bump the timer
    /// </summary>
    public class TimerTicker
    {
        public const string name_Space = "Aptima.Asim.DDD.ScenarioController.";
        private static int timer = 0;
        public static int Timer
        {
            get
            { return timer; }
            set
            { timer = value; }
        }
        // New processing for Happenings
        public delegate Boolean Matches(string unitID, string dataToTest, HappeningCompletionType someHappening);
        public static Matches VerifyMatch;
        public static Boolean ActionMatch(string unitID, string actionPerformed, HappeningCompletionType someHappening)
        {
            return ((unitID == someHappening.UnitID) && (actionPerformed == someHappening.Action));

        }
        public static Boolean StateMatch(string unitID, string newState, HappeningCompletionType someHappening)
        {
            return ((unitID == someHappening.UnitID) && (newState == someHappening.NewState));
        }
        public static void SeekHappening(string unitID, string dataToTest)
        {
            Boolean specialHandling = false;
            if ("MoveComplete_Event" == dataToTest) specialHandling = true;
            for (int h = 0; h < HappeningList.Happenings.Count; h++)
            {
                HappeningCompletionType incident = new HappeningCompletionType(HappeningList.Happenings[h]);
                if (VerifyMatch(unitID, dataToTest, incident))
                {
                    Boolean specialConditionMet = false;
                    if (specialHandling)
                    {
                        switch (dataToTest)
                        {
                            case "MoveComplete_Event":
                                specialConditionMet = incident.HoldsReiteration;
                                break;
                            default:
                                break;
                        }
                    }
                    if (!specialConditionMet)
                    {
                        incident.EnqueueEvents();
                        HappeningList.Happenings.RemoveAt(h);
                    }
                    else
                    {
                        switch (dataToTest)
                        {
                            case "MoveComplete_Event":
                                //Queue the top element of the reiteration list
                                // Ignore engrams in this case
                                if (incident.Range != null)
                                {
                                    if (!incident.Range.Valid("", ""))
                                    {
                                        break; //ends reiteration
                                    }
                                }
                                List<ScenarioEventType> clone = incident.DoThisList;
                                Move_EventType mQueued = (Move_EventType)(clone[0]);
                                Move_EventType mQCopy = (Move_EventType)(clone[0]);
                                mQueued.Time = 1 + (int)(TimerTicker.Timer / 1000);
                                //why isn't this Coordinator.UpdateIncrement or similar?
                                TimerQueueClass.Add(mQueued.Time, mQueued);
                                clone.RemoveAt(0);
                                clone.Add(mQCopy);
                                HappeningCompletionType envelope = HappeningList.Happenings[h];
                                envelope.SwapList(clone);
                                envelope.UnitID = mQueued.UnitID;
                                HappeningList.Add(envelope);
                                HappeningList.Happenings.RemoveAt(h);

                                break;
                            default:
                                break;
                        }
                    }
                    break;

                }
            }

            // The species happenings list is approached backwards
            // presuming that the species hierarchy is given linearly
            // and therefore this lets us find the most specific one that matches



        }

        public static void SeekSpeciesHappening(ScenarioEventType thisItem, string dataToTest)
        {
            for (int sce = SpeciesHappeningList.Happenings.Count - 1; sce >= 0; sce--)
            {
                if (SpeciesHappeningList.Happenings[sce].Matches(thisItem, dataToTest))
                {
                    SpeciesHappeningList.Happenings[sce].EnqueueEvents(thisItem.UnitID);
                    break;

                }


            }
        }

        //End New Processing for Happenings
        /// <summary>
        /// DEPRECATED.  Functionality for time slices is in NextTimeSlice, functionality for Time Ticks is in coordinator now.
        /// </summary>
        /// <param name="updateIncrement"></param>
        public static void NextTick(int updateIncrement, int timeSlice)
        {//update increment and time slice are no longer used here.
        }

        /// <summary>
        /// Coordinates all the processing for each time slice
        /// </summary>
        /// <param name="updateIncrement"></param>
        public static void NextTimeSlice()
        {


            /*
             * Initially the only items that would be named in events would be actual units.
             * Now that other things (ActiveRegions,for example) can also appear in events
             * it is necessary to allow UnitFActs to know those names too, to keep various checks on
             * object name legality from failing
             */
            /* 1. Process the incoming list of events from below */
            // Which units are currently on the incoming list?
            List<string> unitList = IncomingList.AffectedUnits();
            for (int i = 0; i < unitList.Count; i++)
            {
                List<IncomingItemType> eventList = IncomingList.Retrieve(unitList[i]);


                /* do things with these events */
                for (int j = 0; j < eventList.Count; j++)
                {
                    ScenarioEventType thisItem = eventList[j].TheEvent;
                    Coordinator.debugLogger.Writeline("Timer", "Time: " + timer + " Unit " + unitList[i] + " has incoming " + thisItem.GetType().ToString(), "test");

                    if (UnitFacts.AnyDead(thisItem.AllUnits))
                    {
                        continue;
                    }
                    Boolean launchedByOwner;
                    switch (thisItem.GetType().ToString())
                    {

                        case name_Space + "MoveComplete_Event":
                            {
                                Coordinator.debugLogger.Writeline("Timer", "Unit " + thisItem.UnitID + " MoveComplete discovered at t=" + timer.ToString(), "test");
                                // scan happenings list for matches                          
                                VerifyMatch = ActionMatch;
                                SeekHappening(thisItem.UnitID, "MoveComplete_Event");


                                SeekSpeciesHappening(thisItem, "MoveComplete_Event");

                            }
                            break;
                        case name_Space + "StateChangeNotice":
                            string newState = ((StateChangeNotice)thisItem).NewState;
                            Coordinator.debugLogger.Writeline("Timer", "Unit " + thisItem.UnitID + " StateChange to " + newState + " discovered at t=" + timer.ToString(), "test");

                            VerifyMatch = StateMatch;
                            List<string> affectedUnitList = new List<string>();
                            if ("Dead" != newState)
                            {
                                affectedUnitList.Add(thisItem.UnitID);

                                UnitFacts.CurrentUnitStates[thisItem.UnitID] = newState;// newState;

                            }
                            else              // Purge queues if the new state is "Dead"
                            {

                                affectedUnitList = SubplatformRecords.DeepDockedList(thisItem.UnitID);
                            }
                            for (int someUnit = 0; someUnit < affectedUnitList.Count; someUnit++)
                            {
                                if ("Dead" == newState)
                                {
                                    UnitFacts.CurrentUnitStates[affectedUnitList[someUnit]] = "Dead";// newState;
                                    //first drop all timer events involving this unit
                                    TimerQueueClass.FlushOneUnit(affectedUnitList[someUnit]);
                                }
                                SeekHappening(affectedUnitList[someUnit], newState);

                                //Then if dead, remove all happening events involving this unit
                                if ("Dead" == newState)
                                {
                                    HappeningList.FlushOneUnit(affectedUnitList[someUnit]);
                                    Coordinator.debugLogger.Writeline("Timer", " stateChangeEvent for  " + affectedUnitList[someUnit], "test");
                                }
                                SeekSpeciesHappening(thisItem, newState);
                            }
                            break;
                        case name_Space + "AttackSuccessfulNotice":
                            AttackSuccessfulNotice asn = (AttackSuccessfulNotice)thisItem;
                            for (int asi = 0; asi < AttackSuccessfulList.Happenings.Count; asi++)
                            {

                                AttackSuccessfulCompletionType tryThisEvent = AttackSuccessfulList.Happenings[asi];
                                if (tryThisEvent.Matches(asn))
                                {
                                    if (tryThisEvent.DoThisList.Count > 0)
                                        AttackSuccessfulCompletionType.EnqueueEvents(asn.UnitID, asn.Target, tryThisEvent.DoThisList);

                                    break;
                                }
                            }
                            break;
                        case name_Space + "AttackObjectRequestType":

                            Coordinator.debugLogger.Writeline("Timer", "Attack request for  " + thisItem.UnitID + "attack Request discovered at t=" + timer.ToString(), "test");

                            AttackObjectRequestType recastAOR = (AttackObjectRequestType)thisItem;

                            if (ApprovalsList.AttackApproved(recastAOR))
                            {
                                string attacker = recastAOR.UnitID;
                                string target = recastAOR.TargetObjectID;
                                AttackObjectEvent attack = new AttackObjectEvent(attacker, target, recastAOR.CapabilityName);
                                attack.AddOtherUnit(target);
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/), attack);
                            }
                            else
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000), new SystemMessage(1, recastAOR.UnitID, "Attack against " + recastAOR.TargetObjectID + " failed."));


                            break;

                        case name_Space + "WeaponLaunchRequestType":
                            WeaponLaunchRequestType recastWL = (WeaponLaunchRequestType)thisItem;
                            string unitID = recastWL.UnitID;
                            string weaponID = recastWL.WeaponID;
                            string targetID = recastWL.TargetObjectID;
                            Coordinator.debugLogger.Writeline("Timer", "Weapon Launch request for  " + unitID + " using " + weaponID + " Attack Request discovered at t=" + timer.ToString(), "test");

                            string platformOwner = UnitFacts.Data[unitID].Owner;
                            string weaponOwner = UnitFacts.Data[weaponID].Owner;
                            launchedByOwner = SpeciesType.GetSpecies(UnitFacts.Data[weaponID].Species).LaunchedByOwner;
                            //   )
                            if (
                               SubplatformRecords.IsLaunchableWeapon(unitID, weaponID)
                               &&
                               (
                               (platformOwner == recastWL.DecisionMaker)
                               ||
                               (weaponOwner == recastWL.DecisionMaker) && launchedByOwner
                                )
                               )
                            {
                                // validate attack
                                DecisionMakerType targetDM = DecisionMakerType.GetDM(UnitFacts.GetDM(targetID));
                                if (
                                    (launchedByOwner && UnitFacts.HostileTo(DecisionMakerType.GetDM(platformOwner).Team, targetDM.Team))
                                    ||
                                    (UnitFacts.HostileTo(DecisionMakerType.GetDM(weaponOwner).Team, targetDM.Team))
                                    //Removed !launchedByOwner from second statement
                                    )
                                {
                                    //NB. THe code that builds and sends the event was missing from here. It was last seen in rev 232.
                                    // its removeal was probably an accident?
                                    // build event to send down
                                    WeaponLaunch_EventType wEvent = new WeaponLaunch_EventType(unitID, targetID, weaponID);
                                    TimerQueueClass.Add(1 + (int)(timer / 1000), wEvent);
                                    // replace by new platform handling       

                                }
                            }
                            else
                            {
                                //need to throw an event back to ViewPro to re-allocate weapon
                                String reason = "";
                                if (!UnitFacts.Data.ContainsKey(unitID))
                                {
                                    reason = "Platform object is unknown by the system.";
                                }
                                else if (UnitFacts.Data[unitID].Owner != recastWL.DecisionMaker)
                                {
                                    reason = "Requesting DM does not own the Platform object.";
                                }

                                WeaponLaunchFailure_EventType failEvent = new WeaponLaunchFailure_EventType(weaponID, unitID, reason);
                                failEvent.Time = (int)(timer / 1000/*Coordinator.UpdateIncrement*/);
                                TimerQueueClass.Add(1 + (int)(timer / 1000/*Coordinator.UpdateIncrement*/), failEvent);
                            }
                            break;

                        case name_Space + "SubplatformLaunchRequestType":
                            SubplatformLaunchRequestType recastSL = (SubplatformLaunchRequestType)thisItem;
                            string childID = recastSL.UnitID;
                            string requestor = recastSL.Requestor;
                            string parentID = recastSL.ParentUnit;
                            LocationType destination = recastSL.Destination;
                            Coordinator.debugLogger.Writeline("Timer", "Subplatform Launch request for  " + childID + " from " + parentID + "  discovered at t=" + timer.ToString(), "test");
                            launchedByOwner = SpeciesType.GetSpecies(UnitFacts.Data[childID].Species).LaunchedByOwner;

                            if (
                             (UnitFacts.Data[parentID].Owner == requestor)
                             ||
                             (UnitFacts.Data[childID].Owner == requestor) && launchedByOwner
                             )
                            {
                                // build event to send down
                                SubplatformLaunchType slEvent = new SubplatformLaunchType(childID, parentID, destination);
                                TimerQueueClass.Add(1 + (int)(timer / 1000), slEvent);


                            }
                            /**/

                            break;
                        case name_Space + "SubplatformDockRequestType":
                            SubplatformDockRequestType recastSD = (SubplatformDockRequestType)thisItem;
                            string orphanID = recastSD.UnitID;
                            string dockRequestor = recastSD.Requestor;
                            string adopterID = recastSD.ParentUnit;

                            Coordinator.debugLogger.Writeline("Timer", "Subplatform Dock request for  " + orphanID + " from " + adopterID + "  discovered at t=" + timer.ToString(), "test");

                            if ("" != adopterID)
                            {
                                if (UnitFacts.Data.ContainsKey(orphanID)
                                    && UnitFacts.Data.ContainsKey(adopterID)
                                    && ((UnitFacts.Data[orphanID].Owner == dockRequestor)
                                    //     && (UnitFacts.Data[adopterID].Owner == dockRequestor)
                                        )
                                    && SpeciesType.GetSpecies(UnitFacts.GetSpecies(adopterID)).CanHaveSubplatform(UnitFacts.GetSpecies(orphanID))
                                    )
                                {
                                    /* for dock to any object*/
                                    SubplatformDockType sdEvent = new SubplatformDockType(orphanID, adopterID);
                                    TimerQueueClass.Add(1 + (int)(timer / 1000), sdEvent);



                                }
                            }// if  the parent is "" just ignore the request
                            else
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000), new SystemMessage(1, dockRequestor, "Docking of " + orphanID + "to " + adopterID + " failed."));

                            break;
                        case name_Space + "MoveObjectRequestType":
                            // does this dM control this object
                            MoveObjectRequestType recastMove = (MoveObjectRequestType)thisItem;

                            Coordinator.debugLogger.Writeline("Timer", "Move request for  " + thisItem.UnitID + " discovered at t=" + timer.ToString(), "test");


                            if (UnitFacts.Data.ContainsKey(recastMove.UnitID) && (UnitFacts.Data[recastMove.UnitID].Owner == recastMove.DecisionMaker))
                            {
                                Move_EventType mover = new Move_EventType(recastMove.UnitID, TimerTicker.Timer, recastMove.Destination, recastMove.Throttle);
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/), mover);
                            }
                            else
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000), new SystemMessage(1, recastMove.DecisionMaker, "Move of " + recastMove.UnitID + " denied."));

                            break;

                        case name_Space + "TransferObjectRequest":
                            Boolean transferred = false;
                            TransferObjectRequest recastTransferRequest = (TransferObjectRequest)thisItem;
                            /*
                              * Transfer is allowed when
                              * (1) requestor is owner of asset and has transfer authority
                              * (2) owner of unit is  subordinate of requestor, who has forced transfer authority
                              * Unit is in a stealable state
                              * * */
                            DecisionMakerType unitDM = DecisionMakerType.GetDM(UnitFacts.GetDM(recastTransferRequest.UnitID));
                            DecisionMakerType requestingDM = DecisionMakerType.GetDM(recastTransferRequest.Requestor);
                            DecisionMakerType receivingDM = DecisionMakerType.GetDM(recastTransferRequest.Recipient);

                            // To know if stealable, need current state
                            Boolean inStealableState = false;
                            Dictionary<string, StateBody> allStates = StatesForUnits.GetExpandedStatesOf(recastTransferRequest.UnitID);
                            if (allStates.ContainsKey(recastTransferRequest.State))
                            {
                                StateBody thisState = allStates[recastTransferRequest.State];
                                inStealableState = thisState.Parameters.ContainsKey("Stealable");
                            }


                            if ((unitDM.Identifier == requestingDM.Identifier) && (requestingDM.HasTransferAuthority)
                                ||// does the path from requestor to unit contain and forced transfers?
                                (requestingDM.CanForceTransferOf(unitDM)
                                ||
                            (
                                inStealableState && (recastTransferRequest.Requestor == receivingDM.Identifier))))
                            {


                                if (receivingDM.CanOwn(recastTransferRequest.UnitID))
                                    transferred = UnitFacts.ChangeDM(recastTransferRequest.UnitID, recastTransferRequest.Recipient);
                            }
                            if (transferred)
                            {
                                TransferEvent confirmation = new TransferEvent(recastTransferRequest.UnitID, UnitFacts.GetDM(recastTransferRequest.UnitID), recastTransferRequest.Recipient);
                                EventCommunicator.SendEvent(confirmation);
                                VerifyMatch = ActionMatch;
                                SeekHappening(thisItem.UnitID, "TransferObject");
                                SeekSpeciesHappening(thisItem, "TransferObject");

                                //Note: Event here is successful transfer, not request
                            }
                            else
                                TimerQueueClass.Add(1 + (int)(TimerTicker.Timer / 1000), new SystemMessage(1, recastTransferRequest.Requestor, "Transfer of " + recastTransferRequest.UnitID + " to " + recastTransferRequest.Recipient + " denied."));

                            break;


                        case name_Space + "SelfDefenseAttackNotice":
                            /*
                             * Notice: Assuming that self-defense only against enemy and that the condition is satisfied
                             * by the attack, without any attention to who the enemy is.  Will surely need a "Team" level happenings queue in time
                             */
                            {
                                Coordinator.debugLogger.Writeline("Timer", "Unit " + thisItem.UnitID + " SelfDefenseAttackNotice discovered at t=" + timer.ToString(), "test");
                                // scan happenings list for matches    
                                VerifyMatch = ActionMatch;
                                SeekHappening(thisItem.UnitID, "SelfDefenseAttackNotice");




                                SeekSpeciesHappening(thisItem, "SelfDefenseAttackNotice");
                                break;
                            }// 



                        default:
                            {
                                Coordinator.debugLogger.Writeline("Timer", "Unknown incoming list item " + thisItem.GetType().ToString(), "debug");
                                break;
                            }

                    }
                }
                /* and then take them off the incoming list */
                List<int> indexList = new List<int>();
                for (int j = 0; j < eventList.Count; j++)
                {
                    indexList.Add(eventList[j].TheIndex);
                }
                IncomingList.RemoveUnit(indexList);
            }
        }
    }
}
