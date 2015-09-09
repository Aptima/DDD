using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class InteractionEvent : DDDEvent
    {
        public String ObjectID = "";
        public String TargetID = "";
        public String CapabilityName = "";
        public InteractionEvent(String instigatorID, String targetID, String capabilityName, int time)
        {
            EVENTTYPE = "AttackObject";
            ObjectID = instigatorID;
            TargetID = targetID;
            CapabilityName = capabilityName;
            Time = time;
        }
        public InteractionEvent()
            : this("","","", 0)
        {         
        }
        public override SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, EVENTTYPE);

            //do stuff
            ((StringValue)ev["ObjectID"]).value = this.ObjectID;
            ((StringValue)ev["TargetObjectID"]).value = this.TargetID;
            ((StringValue)ev["CapabilityName"]).value = this.CapabilityName;            

            return ev;
        }
    }
}
