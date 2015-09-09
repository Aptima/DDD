using System;
using System.Collections.Generic;
using System.Text;

using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.SimulationModelTools;

using DDD.CommonComponents.DataTypeTools;
using DDD.CommonComponents.NetworkTools;

namespace ScenarioController
{
    class ScenarioReader
    {
        private static SimulationEvent populateQueue()
        {
            SimulationEvent ee = new SimulationEvent();
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();

            ee.eventType = "NewObject";
            DataValue myDV = new StringValue();
            ((StringValue)(myDV)).value = "PhysicalObject";
            ee.parameters.Add("ObjectType", myDV);


            // START OF ATTRIBUTE DEFINITIONS //
            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 0;
            myAtt.Add("ID", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "First Object";
            myAtt.Add("ObjectName", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "flying";
            myAtt.Add("ObjectState", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "NoClassesYet";
            myAtt.Add("ClassName", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 0;
            ((LocationValue)(myDV)).Y = 0;
            ((LocationValue)(myDV)).Z = 0;
            myAtt.Add("Location", myDV);

            myDV = new VelocityValue();
            ((VelocityValue)(myDV)).VX = 0;
            ((VelocityValue)(myDV)).VY = 0;
            ((VelocityValue)(myDV)).VZ = 0;
            myAtt.Add("Velocity", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = 1;
            myAtt.Add("MaximumSpeed", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = 0.0;
            myAtt.Add("Throttle", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 0;
            ((LocationValue)(myDV)).Y = 0;
            ((LocationValue)(myDV)).Z = 0;
            myAtt.Add("DestinationLocation", myDV);

            // END OF ATTRIBUTE DEFINITIONS //

            myDV = new AttributeCollectionValue();
            ((AttributeCollectionValue)(myDV)).attributes = myAtt;
            ee.parameters.Add("Attributes", myDV);

            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 5000;
            ee.parameters.Add("Time", myDV);

            return ee;

        }

        private static SimulationEvent populateQueue3()
        {
            SimulationEvent ee = new SimulationEvent();
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();

            ee.eventType = "NewObject";
            DataValue myDV = new StringValue();
            ((StringValue)(myDV)).value = "PhysicalObject";
            ee.parameters.Add("ObjectType", myDV);


            // START OF ATTRIBUTE DEFINITIONS //
            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 1;
            myAtt.Add("ID", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "Second Object";
            myAtt.Add("ObjectName", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "flying";
            myAtt.Add("ObjectState", myDV);

            myDV = new StringValue();
            ((StringValue)(myDV)).value = "NoClassesYet";
            myAtt.Add("ClassName", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 100;
            ((LocationValue)(myDV)).Y = 100;
            ((LocationValue)(myDV)).Z = 0;
            myAtt.Add("Location", myDV);

            myDV = new VelocityValue();
            ((VelocityValue)(myDV)).VX = 0;
            ((VelocityValue)(myDV)).VY = 0;
            ((VelocityValue)(myDV)).VZ = 0;
            myAtt.Add("Velocity", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = 1;
            myAtt.Add("MaximumSpeed", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = 0.0;
            myAtt.Add("Throttle", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 0;
            ((LocationValue)(myDV)).Y = 0;
            ((LocationValue)(myDV)).Z = 0;
            myAtt.Add("DestinationLocation", myDV);

            // END OF ATTRIBUTE DEFINITIONS //

            myDV = new AttributeCollectionValue();
            ((AttributeCollectionValue)(myDV)).attributes = myAtt;
            ee.parameters.Add("Attributes", myDV);

            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 5000;
            ee.parameters.Add("Time", myDV);

            return ee;

        }

        private static SimulationEvent populateQueue2()
        {
            SimulationEvent ee = new SimulationEvent();
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();

            ee.eventType = "MoveObject";

            DataValue myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 0;
            ee.parameters.Add("ObjectID", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 0;
            ((LocationValue)(myDV)).Y = 0;
            ((LocationValue)(myDV)).Z = 0;
            ee.parameters.Add("DestinationLocation", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = .5;
            ee.parameters.Add("Throttle", myDV);

            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 10000;
            ee.parameters.Add("Time", myDV);

            return ee;

        }

        private static SimulationEvent populateQueue4()
        {
            SimulationEvent ee = new SimulationEvent();
            Dictionary<string, DataValue> myAtt = new Dictionary<string, DataValue>();

            ee.eventType = "MoveObject";

            DataValue myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 1;
            ee.parameters.Add("ObjectID", myDV);

            myDV = new LocationValue();
            ((LocationValue)(myDV)).X = 100;
            ((LocationValue)(myDV)).Y = 100;
            ((LocationValue)(myDV)).Z = 100;
            ee.parameters.Add("DestinationLocation", myDV);

            myDV = new DoubleValue();
            ((DoubleValue)(myDV)).value = .5;
            ee.parameters.Add("Throttle", myDV);

            myDV = new IntegerValue();
            ((IntegerValue)(myDV)).value = 10000;
            ee.parameters.Add("Time", myDV);

            return ee;

        }

        public ScenarioReader()
        {
            Queue<SimulationEvent> q = new Queue<SimulationEvent>();

            QueueManager EventQueue = QueueManager.UniqueInstance();

            SimulationEvent dequeued;

            //simulates reading in from the XML file
            q.Enqueue(populateQueue());
            q.Enqueue(populateQueue2());
            q.Enqueue(populateQueue3());
            q.Enqueue(populateQueue4());
            //Only would dequeue if events are not conditional.
            //After done reading from XML file, send time queue to queue manager
            while (q.Count > 0)
            {
                dequeued = new SimulationEvent();
                dequeued = q.Dequeue();
                EventQueue.AddEvent(((IntegerValue)dequeued.parameters["Time"]).value, dequeued);
            }      
        }
    }
}
