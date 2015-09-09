using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

using DDD.CommonComponents.DataTypeTools;
using DDD.CommonComponents.NetworkTools;
using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.SimulationModelTools;

 
namespace DDD.ScenarioController
{
    public class EventCommunicator
    {
        private static NetworkClient server = null;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;


        public EventCommunicator()
        { }

        public EventCommunicator(NetworkClient s)
        {
            server = s;
            //Dennis: This needs to be updated to your directory
            simModelInfo = smr.readModel(@"C:\SVN\NewDDD\src\DataFiles\SimulationModel.xml");
//simModelInfo = smr.readModel("SimulationModel.xml");
            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);
            server.Subscribe("MoveDone");
        }

        public void AddNetworkClient(NetworkClient s)
        {
            server = s;

            simModelInfo = smr.readModel("SimulationModel.xml");

            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);
            server.Subscribe("MoveDone");
            server.Subscribe("MoveObjectRequest");
            server.Subscribe("AttackObjectRequest");
        }

        public void WaitForEvents()
        { 
            //Receives a move done event from the simcore
            string objID = null;
            string eventType = null;
            List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
            while (true)
            {
                 incomingEvents = server.GetEvents();

                if (incomingEvents.Count == 0)
                {//do nothing 
                }
                else
                {
                    foreach (SimulationEvent e in incomingEvents)
                    {
                        eventType = e.eventType;
                        switch (eventType)
                        { 
                            case "MoveDone":
                                objID = ((StringValue)e["ObjectID"]).value; //see line 222 of MotionSim.cs for reference
                                MoveComplete_Event returnEvent = new MoveComplete_Event(objID.ToString());

                                IncomingList.Add(returnEvent);

                                break;
                            case "MoveObjectRequest":
                                //Figure out from Dennis how to let him know this happened

                                break;
                            case "AttackObjectRequest":
                                //Figure out from Dennis how to let him know this happened

                                break;
                            default:
                                //Should throw an error?  Will getEvents receive events I'm not subscribed to?
                                break;
                        }
                        
                    }
                    incomingEvents = null;
                }
                Thread.Sleep(100);
            }
        }

        public static void SendPlayfield()
        { 
            //Check ICD for specs between two components.
        }

        public static void SendEvent(RootEventType sendingEvent)
        {
            //This will discover the RootEventType's actual Type of event,
            //and then based on that, will break out the information into 
            //a simulation model event, and then putEvent to the NetworkClient
            string eventType;
            eventType = sendingEvent.GetType().Name.ToString();
            SimulationEvent e = null;

            DataValue dv;

            bool hasAtt = false;
            string attName = null,
                   attSettingType = null;

         

            switch (eventType)
            {

                /******************Very Basic Event Type Creation*******************************/
                case "RootEventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "BaseEvent");

                    dv = new IntegerValue();
                    ((IntegerValue)dv).value = sendingEvent.Timer;
                    e.parameters["Time"] = dv;

                    break;

                /******************Base Scenario Event Type Creation****************************/
                case "ScenarioEventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "BaseEvent");

                    dv = new IntegerValue();
                    ((IntegerValue)dv).value = sendingEvent.Timer;
                    e.parameters["Time"] = dv;

                    break;

                /******************New Object Event Type Creation ******************************/
                case "Create_EventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "NewObject");
                    Dictionary<string, DataValue> myAtt;
                    myAtt = new Dictionary<string, DataValue>();


                    dv = new IntegerValue();
                    ((IntegerValue)dv).value = sendingEvent.Timer;
                    e.parameters["Time"] = dv;

                    //((StringValue)e.parameters["ObjectType"]).value = "PhysicalObject";
                    //Replaced by Kind from Scenario file

                    ((StringValue)e.parameters["ObjectType"]).value = ((Create_EventType)sendingEvent).UnitKind.ToString();

// Attribute Insertion //

                    if (((Create_EventType)sendingEvent).UnitID != null)
                    {
                        dv = new StringValue();
                        ((StringValue)dv).value = ((Create_EventType)sendingEvent).UnitID.ToString();
                        myAtt.Add("ID", dv);
                        hasAtt = true;
                    }


                    
                    List<ParameterSettingType> eventsList = ((Create_EventType)sendingEvent).Parameters;

                    foreach (ParameterSettingType key in eventsList)
                    {
                        attName = key.Name;
                        attSettingType = key.Setting.GetType().Name;
                        if (attSettingType == "VectorType")
                        {
                            dv = new LocationValue();
                            ((LocationValue)dv).X = ((VectorType)key.Setting).X;
                            ((LocationValue)dv).Y = ((VectorType)key.Setting).Y;
                            ((LocationValue)dv).Z = ((VectorType)key.Setting).Z;
                            myAtt.Add(attName, dv);
                        }
                        else
                            if (attSettingType == "String")
                            {
                                switch (attName)
                                {
                                    case "ID": //this shouldn't occur?
                                    dv = new StringValue();
                                    ((StringValue)dv).value = key.Setting.ToString();
                                    myAtt.Add(attName, dv);
                                    break;

                                    case "ObjectName":
                                    dv = new StringValue();
                                    ((StringValue)dv).value = key.Setting.ToString();
                                    myAtt.Add(attName, dv);
                                    break;

                                    case "ObjectState":
                                    dv = new StringValue();
                                    ((StringValue)dv).value = key.Setting.ToString();
                                    myAtt.Add(attName, dv);
                                    break;

                                    case "ClassName":
                                    dv = new StringValue();
                                    ((StringValue)dv).value = key.Setting.ToString();
                                    myAtt.Add(attName, dv);
                                    break;

                                    case "MaximumSpeed":
                                    dv = new DoubleValue();
                                    ((DoubleValue)dv).value = Convert.ToDouble(key.Setting.ToString());
                                    myAtt.Add(attName, dv);
                                    break;

                                    case "Throttle":
                                    dv = new DoubleValue();
                                    ((DoubleValue)dv).value = Convert.ToDouble(key.Setting.ToString());
                                    myAtt.Add(attName, dv);
                                    break;

                                    default:

                                    break;
                                }
                            }
                            else ; //Should be a vector type or a string, if not... do nothing.
                        

                    }

                    if (hasAtt)
                        ((AttributeCollectionValue)e.parameters["Attributes"]).attributes = myAtt;

                    break;

                /******************Move Object Event Type Creation******************************/
                case "Move_EventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "MoveObject");

                    if (((Move_EventType)sendingEvent).UnitID != null)
                    {
                        dv = new StringValue();
                        ((StringValue)dv).value = ((Move_EventType)sendingEvent).UnitID.ToString();
                        e.parameters["ObjectID"] = dv;
                    }
                    if (((Move_EventType)sendingEvent).Location != null)
                    {
                        dv = new LocationValue();
                        ((LocationValue)dv).X = ((VectorType)((Move_EventType)sendingEvent).Location).X;
                        ((LocationValue)dv).Y = ((VectorType)((Move_EventType)sendingEvent).Location).Y;
                        ((LocationValue)dv).Z = ((VectorType)((Move_EventType)sendingEvent).Location).Z;
                        e.parameters["DestinationLocation"] = dv;
                    }

                    dv = new DoubleValue();
                    ((DoubleValue)dv).value = ((Double)((Move_EventType)sendingEvent).Throttle);
                    e.parameters["Throttle"] = dv;

                    dv = new IntegerValue();
                    ((IntegerValue)dv).value = sendingEvent.Timer;
                    e.parameters["Time"] = dv;

                    break;

                /******************Tick Event Type Creation*************************************/
                case "TickEventType":
                    e = SimulationEventFactory.BuildEvent(ref simModelInfo, "TimeTick");

                    dv = new IntegerValue();
                    ((IntegerValue)dv).value = sendingEvent.Timer;
                    e.parameters["Time"] = dv;


                    break;
                /******************No valid event entered***************************************/

                default:
                    //What should it do in this case? Nothing?
                    break;
            }

            if (e != null)
                server.PutEvent(e);
        }



    }
}
