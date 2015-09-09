using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class MoveEvent : DDDEvent
    {
        public String ObjectID = "";
        public LocationValue DestinationLocation = null;
        public double Throttle = 1.0;
        public MoveEvent(String objectID, LocationValue location, int time)
        {
            EVENTTYPE = "MoveObject";

            ObjectID = objectID;
            DestinationLocation = location;
            Time = time;
        }
        public MoveEvent(): this("", new LocationValue(), 0)
        {         
        }

        public void setThrottle(double throttle) {
            Throttle = throttle;
        }

        public override SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, EVENTTYPE);

            //do stuff
            ((StringValue)ev["ObjectID"]).value = this.ObjectID;

            ((DoubleValue)ev["Throttle"]).value = this.Throttle;
            
            ev["DestinationLocation"] = this.DestinationLocation;

            return ev;
        }
    }
}
