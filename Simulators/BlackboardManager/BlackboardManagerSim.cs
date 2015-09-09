using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.Simulators.BlackboardManager
{
    public class BlackboardManagerSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        public BlackboardManagerSim()
        {
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
        }
        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;

            distributor.RegisterClient(ref distClient);
            //distClient.Subscribe("NewObject");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("BaseObject", "ID", true, true);
            
        }

        public void ProcessEvent(SimulationEvent e)
        {
            switch (e.eventType)
            {
                case "NewObject":
                    //System.Console.WriteLine(GetSimulatorName() + " recieved " + e.eventType);
                    NewObject(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "TimeTick":
                    TimeTick(e);
                    break;
                case "ForceUpdateObjectAttribute":
                    ForceUpdateObjectAttribute(e);
                    break;
                default:
                    break;
            }
        }

        private void ForceUpdateObjectAttribute(SimulationEvent e)
        {
                return;

        }
        private void TimeTick(SimulationEvent e)
        {

            blackboard.simTime = ((IntegerValue)e["Time"]).value;
        }
        public string GetSimulatorName()
        {
            return "BlackboardManager";
        }

        private void ResetSimulation()
        {
            blackboard.ClearObjects();
            StateDB.Reset();
            ScoringDB.Reset();
        }

        private void NewObject(SimulationEvent e)
        {
            string objectType = ((StringValue)e["ObjectType"]).value;
            Console.WriteLine(String.Format("Blackboard gets new object; ID: {0}; Type: {1}", ((StringValue)e["ID"]).value, objectType));
            SimulationObject ob = SimulationObjectFactory.BuildObject(ref simModel, objectType);

            ob.attributes["ID"] = e["ID"];

            

            // REFACTOR: I shouldn't actually do this.  I should only initialize the values that I own.
            //AD: I'm populating the values passed in the NewObject event into the SimulationObject
            foreach (string attname in ((AttributeCollectionValue)e["Attributes"]).attributes.Keys)
            {
                ob.attributes[attname] =  ((AttributeCollectionValue)e["Attributes"])[attname];
            }
            blackboard.AddObject(ob);

            // Initialize StateDB objects

            if (((StringValue)e["ObjectType"]).value == "Team")
            {
                StateDB.Team t = new StateDB.Team(((StringValue)e["ID"]).value,
                                  ((StringListValue)((AttributeCollectionValue)e["Attributes"]).attributes["TeamHostility"]).strings);

                StateDB.teams[t.id] = t;
            }
            else if (((StringValue)e["ObjectType"]).value == "DecisionMaker")
            {
                string teamID = ((StringValue)((AttributeCollectionValue)e["Attributes"]).attributes["TeamMember"]).value;
                StateDB.DecisionMaker d = new StateDB.DecisionMaker(((StringValue)e["ID"]).value,
                                                    StateDB.teams.ContainsKey(teamID) ? StateDB.teams[teamID] : null);

                StateDB.decisionMakers[d.id] = d;
            }
            else if (SimUtility.isPhysicalObject(objectType))
            {
                String linkedRegion = "";
                if (e.parameters.ContainsKey("LinkedRegion"))
                    linkedRegion = ((StringValue)e["LinkedRegion"]).value;

                StateDB.AddPhysicalObject(((StringValue)e["ID"]).value, objectType,linkedRegion);
            }

        }
    }
}

