using System;
using System.Collections.Generic;
using System.Text;
using DDD.CommonComponents.SimulationModelTools;
using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.DataTypeTools;
using DDD.CommonComponents.NetworkTools;

namespace ScenarioController
{
    public class EventListener
    {
        private static QueueManager myQM= null;
        private static EventListener _uniqueInstance = null;

        public static EventListener UniqueInstance(NetworkClient server)
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new EventListener(server);

            return _uniqueInstance;
        }


        protected EventListener(NetworkClient server)
        {
            myQM = QueueManager.UniqueInstance();

            SimulationModelReader smr = new SimulationModelReader();
            SimulationModelInfo simModelInfo = smr.readModel("SimulationModel.xml");

            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);
            server.Subscribe("MoveDone");

        }

        public SimulationEvent MoveDoneReceived(SimulationEvent e, SimulationModelInfo model, SimulationEvent theTick)
        { 
            //Should receieve a MoveDone event, and create a new move event and send
            //it to the Queue Manager with time ticks + 1.

            SimulationEvent ee = SimulationEventFactory.BuildEvent(ref model, "MoveObject");

            ee.eventType = "MoveObject";

            DataValue myDV = new IntegerValue();
            myDV = e["ObjectID"];
            ee["ObjectID"] = myDV;

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 150;
            ((LocationValue)(myDV)).Y = 150;
            ((LocationValue)(myDV)).Z = 50;
            ee["DestinationLocation"] = myDV;

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = .5;
            ee["Throttle"] = myDV;

            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 
                ((IntegerValue)theTick.parameters["Time"]).value + 1000;
            ee["Time"] = myDV;

            return ee;



        }

        //Need to get events from SimCore, push to StateManager/QueueManager(until state is complete)
    }
}
