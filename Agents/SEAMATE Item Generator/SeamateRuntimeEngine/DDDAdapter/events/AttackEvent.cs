using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class AttackEvent : DDDEvent
    {
        public String AttackerID = "";
        public String TargetID = "";
        public String CapabilityName = "";
        public AttackEvent(String attackerID, String targetID, String capabilityName, int time)
        {
            EVENTTYPE = "AttackObject";

            AttackerID = attackerID;
            TargetID = targetID;
            CapabilityName = capabilityName;
            Time = time;
        }
        public AttackEvent()
            : this("", "","", 0)
        {         
        }

        public override SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, EVENTTYPE);

            //do stuff
            ((StringValue)ev["ObjectID"]).value = this.AttackerID;

            ((StringValue)ev["TargetObjectID"]).value = this.TargetID;
            ((StringValue)ev["CapabilityName"]).value = this.CapabilityName;

            ((IntegerValue)ev["PercentageApplied"]).value = 100;

            return ev;
        }
    }
}
