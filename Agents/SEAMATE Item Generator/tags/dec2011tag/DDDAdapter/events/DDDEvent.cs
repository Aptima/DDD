using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class DDDEvent
    {
        public int Time = 0;
        protected String EVENTTYPE = "BASE";
        public DDDEvent():this(0)
        { }
        public DDDEvent(int time)
        {
            Time = time;
        }
        public virtual SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
           // SimulationEventFactory.BuildEvent(simModel, EVENTTYPE);
            return null;
        }
    }
}
