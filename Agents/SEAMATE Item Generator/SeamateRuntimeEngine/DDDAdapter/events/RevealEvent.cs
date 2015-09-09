using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace SeamateAdapter.DDD
{
    public class RevealEvent : DDDEvent
    {
        public String ObjectID = "";
        public LocationValue Location = null;
        public String State = "";

        //optional
        public String ObjectType = "";
        public String OwnerID = "";
        public Dictionary<String, DataValue> StartupParameters;
        //

        public RevealEvent(String objectID, LocationValue location, String state, int time)
        {
            EVENTTYPE = "RevealObject";

            ObjectID = objectID;
            Location = location;
            State = state;
            Time = time;
            StartupParameters = new Dictionary<string, DataValue>();
        }
        public RevealEvent()
            : this("", new LocationValue(), "", 0)
        {         
        }

        public override SimulationEvent ToSimulationEvent(ref SimulationModelInfo simModel)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, EVENTTYPE);
            AttributeCollectionValue attributes = new AttributeCollectionValue();

            attributes.attributes.Add("Location", this.Location);
            attributes.attributes.Add("State", DataValueFactory.BuildString(this.State));

            if(ObjectType != null && ObjectType != String.Empty)
            {
                attributes.attributes.Add("ClassName", DataValueFactory.BuildString(ObjectType));
            }
            if(OwnerID != null && OwnerID != String.Empty)
            {
                attributes.attributes.Add("OwnerID", DataValueFactory.BuildString(OwnerID));
            }

            if(StartupParameters != null)
            {
                foreach(String s in StartupParameters.Keys)
                {
                    attributes.attributes.Add(s, StartupParameters[s]);
                }
            }


            //do stuff
            ((StringValue)ev["ObjectID"]).value = this.ObjectID;
            ev["Attributes"] = attributes;



            return ev;
        }
    }
}
