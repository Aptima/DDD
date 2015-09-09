using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
namespace DDD_Test_Agent
{
    public class EventGetter
    {

        static NetworkClient network = null;
        public static NetworkClient Network
        {
            set { network = value; }
            get { return network; }
        }
        private List<SimulationEvent> events = null;
        private Boolean isActive = false;
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private void subscribeToEvents()
        {
            network.Subscribe("RevealObject");
            network.Subscribe("StateChange");
            network.Subscribe("MoveDone");
            network.Subscribe("MoveObject");
            network.Subscribe("SimulationTimeEvent");
            network.Subscribe("ViewProMotionUpdate");
        }
        public void StartListening()
        {

            while (!network.IsConnected())
            {
                Thread.Sleep(100);
            }
            subscribeToEvents();
            isActive = true;
            while (network.IsConnected())
            {// as long as there's a network we remain open to input events
                while (isActive && network.IsConnected())
                {

                    events = network.GetEvents();

                    foreach (SimulationEvent e in events)
                    {
                        switch (e.eventType)
                        {

                            case "RevealObject":
                                if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))
                                {
                                    AttributeCollectionValue attributes = (AttributeCollectionValue)e["Attributes"];
                                    LocationValue locus = (LocationValue)attributes["Location"];
                                    ScudLauncher.AddUnit(((StringValue)e["ObjectID"]).value, locus.X, locus.Y);
                                }
                                else if (Target.IsTarget((((StringValue)e["ObjectID"]).value)))
                                {
                                    Target.AddUnit(((StringValue)e["ObjectID"]).value);
                                }

                                break;
                            case "StateChange":
                                if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))
                                {
                                    if ("Dead" == ((StringValue)e["NewState"]).value)
                                    {
                                        if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))
                                            if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))

                                                ScudLauncher.DropUnit(((StringValue)e["ObjectID"]).value);
                                            else if (Target.IsTarget((((StringValue)e["ObjectID"]).value)))
                                                Target.DropUnit(((StringValue)e["ObjectID"]).value);
                                    }
                                }
                                break;
                            /*        case "MoveDone":
                                        if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))
                                            ScudLauncher.GetScudLauncher((((StringValue)e["ObjectID"]).value)).InMotion = false;
 
                                        break;*/
                            case "SimulationTimeEvent":
                                if (0 == ((IntegerValue)e["Time"]).value % 1000)// only bother with full seconds
                                {
                                    ScudLauncher.Tick(((IntegerValue)e["Time"]).value);
                                }
                                break;
                            case "ViewProMotionUpdate":
                                if (ScudLauncher.AgentControls(((StringValue)e["ObjectID"]).value))
                                {
                                    string objectID=((StringValue)e["ObjectID"]).value;
                                    LocationValue locVal=(LocationValue)e["Location"];
                                    Location current=new Location(locVal.X,locVal.Y);
                                     locVal=(LocationValue)e["DestinationLocation"];
                                     Location destination=new Location(locVal.X,locVal.Y);

                                     Boolean moving = (current.X != destination.X) || (current.Y != destination.Y);
                                     if (moving != ScudLauncher.IsMoving(objectID))
                                         if(!moving)
                                         {
                                             Console.WriteLine("Updating position of "+objectID+" to ("+destination.X.ToString()+","+destination.Y.ToString()+")");
                                             ScudLauncher.SetLocation(objectID, current);

                                         }
                      ScudLauncher.SetMovement(objectID, moving);
                      
                                }
                                break;


                        }
                    }

                    Thread.Sleep(100);
                }
                if (!network.IsConnected())
                    isActive = false;
            }
            Console.WriteLine("Lost connection.");
            network = null;
        }
        public EventGetter() { }
    }
}
