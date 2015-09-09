using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.CommonComponents.SimulatorTools
{
    public class SimUtility
    {
        /*
        static public void SkipToReset(ref List<SimulationEvent> eventList)
        {
            bool resetFound = false;

            foreach (SimulationEvent e in eventList)
            {
                if (e.eventType == "ResetSimulation")
                {
                    resetFound = true;
                }
            }

            if (resetFound)
            {
                while (eventList.Count > 0 && eventList[0].eventType != "ResetSimulation")
                {
                    eventList.Remove(eventList[0]);
                }
            }
        }
         */
        static public CapabilityValue.Effect FindCapabilityEffect(CapabilityValue cap, VulnerabilityValue vuln)
        {
            List<string> vulnNames = new List<string>();

            foreach (VulnerabilityValue.Transition t in vuln.transitions)
            {
                foreach (VulnerabilityValue.TransitionCondition tc in t.conditions)
                {
                    if (!vulnNames.Contains(tc.capability))
                    {
                        vulnNames.Add(tc.capability);
                    }
                }
            }

            foreach (CapabilityValue.Effect e in cap.effects)
            {
                if (vulnNames.Contains(e.name))
                {
                    return e;
                }
            }
            return null;
        }

        static public SimulationEvent BuildStateChangeEvent(ref SimulationModelInfo simModel, int time, string id, string state)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "StateChange");
            ((StringValue)sc["ObjectID"]).value = id;
            ((StringValue)sc["NewState"]).value = state;
            ((IntegerValue)sc["Time"]).value = time;
            return sc;
        }

        static public SimulationEvent BuildMoveDoneEvent(ref SimulationModelInfo simModel, int time, string id, string reason)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "MoveDone");
            ((StringValue)sc["ObjectID"]).value = id;
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["Reason"]).value = reason;
            return sc;
        }

        static public SimulationEvent BuildMoveObjectRequestEvent(ref SimulationModelInfo simModel, int time, string userID, string objectID, Vec3D destination, double throttle)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "MoveObjectRequest");
            ((StringValue)sc["UserID"]).value = userID;
            ((StringValue)sc["ObjectID"]).value = objectID;
            ((IntegerValue)sc["Time"]).value = time;
            ((DoubleValue)sc["Throttle"]).value = throttle;
            sc["DestinationLocation"] = destination.ToLocationValue();
            return sc;
        }
        static public SimulationEvent BuildSystemMessageEvent(ref SimulationModelInfo simModel, int time, string playerID, string message)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "SystemMessage");
            ((StringValue)sc["PlayerID"]).value = playerID;
            ((StringValue)sc["Message"]).value = message;
            ((IntegerValue)sc["Time"]).value = time;
            ((IntegerValue)sc["TextColor"]).value = Color.Black.ToArgb();
            ((StringValue)sc["DisplayStyle"]).value = "Default";
            return sc;
        }
        static public SimulationEvent BuildSystemMessageEvent(ref SimulationModelInfo simModel, int time, string playerID, string message, string displayStyle)
        {
            SimulationEvent msg = BuildSystemMessageEvent(ref simModel,  time,  playerID,  message);
            ((StringValue)msg["DisplayStyle"]).value = displayStyle;
            return msg;
        }
    


        static public SimulationEvent BuildHistory_AttackedObjectReportEvent(ref SimulationModelInfo simModel, int time, string objectID, Vec3D location, bool success, string newState)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "History_AttackedObjectReport");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = objectID;
            sc["ObjectLocation"] = (DataValue)location.ToLocationValue();
            ((BooleanValue)sc["AttackSuccess"]).value = success;
            ((StringValue)sc["NewState"]).value = newState;
            return sc;
        }
        static public SimulationEvent BuildHistory_AttackerObjectReportEvent(ref SimulationModelInfo simModel, int time, string objectID, Vec3D location, string targetObjectID, Vec3D targetLocation, string capabilityName, int appliedIntensity)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "History_AttackerObjectReport");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = objectID;
            sc["ObjectLocation"] = (DataValue)location.ToLocationValue();
            ((StringValue)sc["TargetObjectID"]).value = targetObjectID;
            sc["TargetObjectLocation"] = (DataValue)targetLocation.ToLocationValue();
            ((StringValue)sc["CapabilityName"]).value = capabilityName;
            ((IntegerValue)sc["AppliedIntensity"]).value = appliedIntensity;
            return sc;
        }
        static public SimulationEvent BuildHistory_SubplatformLaunchEvent(ref SimulationModelInfo simModel, int time, string objectID, string parentObjectID, Vec3D parentLocation, Vec3D launchDestinationLocation, bool isWeaponLaunch, string targetObjectID)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "History_SubplatformLaunch");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = objectID;
            ((StringValue)sc["ParentObjectID"]).value = parentObjectID;
            sc["ParentObjectLocation"] = (DataValue)parentLocation.ToLocationValue();
            sc["LaunchDestinationLocation"] = (DataValue)launchDestinationLocation.ToLocationValue();
            ((BooleanValue)sc["IsWeaponLaunch"]).value = isWeaponLaunch;
            ((StringValue)sc["TargetObjectID"]).value = targetObjectID;
            
            return sc;
        }
        static public SimulationEvent BuildHistory_PursueEvent(ref SimulationModelInfo simModel, int time, string objectID, Vec3D objectLocation, string targetObjectID, Vec3D targetLocation)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "History_Pursue");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = objectID;
            sc["ObjectLocation"] = (DataValue)objectLocation.ToLocationValue();
            ((StringValue)sc["TargetObjectID"]).value = targetObjectID;
            sc["TargetObjectLocation"] = (DataValue)targetLocation.ToLocationValue();
            return sc;
        }

        static public SimulationEvent BuildScoreUpdateEvent(ref SimulationModelInfo simModel, int time, string dm,string scoreName,double scoreValue)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "ScoreUpdate");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["DecisionMakerID"]).value = dm;
            ((StringValue)sc["ScoreName"]).value = scoreName;
            ((DoubleValue)sc["ScoreValue"]).value = scoreValue;
            return sc;
        }

        static public SimulationEvent BuildActiveRegionSpeedMultiplierUpdateEvent(ref SimulationModelInfo simModel, int time, string id)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "ActiveRegionSpeedMultiplierUpdate");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = id;
            return sc;
        }

        static public SimulationEvent BuildAttackSucceededEvent(ref SimulationModelInfo simModel, int time, string objectID, string targetID, string newState, List<string> capabilities)
        {
            SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "AttackSucceeded");
            ((IntegerValue)sc["Time"]).value = time;
            ((StringValue)sc["ObjectID"]).value = objectID;
            ((StringValue)sc["TargetID"]).value = targetID;
            ((StringValue)sc["NewState"]).value = newState;
            ((StringListValue)sc["Capabilities"]).strings = capabilities;

            return sc;
        }

        static public bool isPhysicalObject(string objectType)
        {
            if (objectType == "AirObject" || objectType == "SeaObject" || objectType == "LandObject")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    public class Team
    {
        public string id;
        public List<string> hostility;
        public Team(string id, List<string> hostility)
        {
            this.id = id;
            this.hostility = hostility;
        }
    }
    public class DecisionMaker
    {
        public enum Availability
        {
            AVAILABLE = 0,
            TAKEN = 1,
            READY = 2,
        }

        public string id;
        public Team team;
        public bool isHuman;
        public int color;
        public string role;
        public Availability availability;
        public string briefing;
        public Boolean isObserver;

        public DecisionMaker(string id, Team team)
        {
            this.isHuman = false;
            this.id = id;
            this.team = team;
            this.color = 000000;
            this.availability = Availability.AVAILABLE;
            this.briefing = string.Empty;
        }

        public bool isHostile(DecisionMaker dm)
        {
            if (team == null)
            {
                return false;
            }

            if (team.hostility.Contains(dm.team.id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }
    
}
