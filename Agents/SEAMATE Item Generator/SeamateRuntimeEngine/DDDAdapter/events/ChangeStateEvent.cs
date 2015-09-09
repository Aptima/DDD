using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class ChangeStateEvent : DDDEvent
    {
        public String ObjectID = "";
        public String StateName = "";

        public ChangeStateEvent(String objectID, String stateName, int time)
        {
            EVENTTYPE = "StateChange";
            ObjectID = objectID;
            StateName = stateName;
            Time = time;
        }
        public ChangeStateEvent()
            : this("","", 0)
        {         
        }
        public override SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, EVENTTYPE);

            //do stuff
            ((StringValue)ev["ObjectID"]).value = this.ObjectID;
           // ((StringValue)ev["NewState"]).value = this.StateName;  //This was throwing an error... 
            ((StringValue)ev["State"]).value = this.StateName;       //Trying this instead   -Lisa

            return ev;
        }
    }
}
