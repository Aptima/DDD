using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
namespace Aptima.Asim.DDD.Simulators.FuelHandling
{
    public class FuelHandlingSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;

        public FuelHandlingSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
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
            //distClient.Subscribe("RevealObject");
            //distClient.Subscribe("TimeTick");
            //distClient.Subscribe("StateChange");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "StateTable", true, false);
            bbClient.Subscribe("PhysicalObject", "Throttle", true, false);
            bbClient.Subscribe("PhysicalObject", "FuelCapacity", true, true);
            bbClient.Subscribe("PhysicalObject", "FuelAmount", true, true);
            bbClient.Subscribe("PhysicalObject", "FuelConsumptionRate", true, true);
            bbClient.Subscribe("PhysicalObject", "FuelDepletionState", true, true);
            bbClient.Subscribe("PhysicalObject", "DockedToParent", true, false);

            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        public void ProcessEvent(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();
            switch (e.eventType)
            {
                case "NewObject":
                    NewObject(e);
                    break;
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "TimeTick":
                    TimeTick(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
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

            string objectId = ((StringValue)e["ObjectID"]).value;
            string attributeName = ((StringValue)e["AttributeName"]).value;
            if (!objectProxies.ContainsKey(objectId))
            {
                return;
            }
            try
            {
                if (!objectProxies[objectId].GetKeys().Contains(attributeName))
                {
                    return;
                }
                if (!objectProxies[objectId][attributeName].IsOwner())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            DataValue attributeValue = e["AttributeValue"];
            if (attributeValue.dataType != "WrapperType")
                return;
            attributeValue = ((WrapperValue)attributeValue).value;
            SimulationObjectProxy obj = null;

            //depending on the attribute, you might have specific functionality
            switch (attributeName)
            {
                case "FuelCapacity":
                    ChangeObjectFuelCapacity(attributeValue as DoubleValue, objectId);
                    break;
                case "FuelAmount":
                    ChangeObjectFuelAmount(attributeValue as DoubleValue, objectId);
                    break;
                case "FuelConsumptionRate":
                    ChangeObjectFuelRate(attributeValue as DoubleValue, objectId);
                    break;
                case "FuelDepletionState":
                    ChangeObjectFuelDepleteState(attributeValue as StringValue, objectId);
                    break;
                default:
                    break;
            }

            //try
            //{
            //    obj[attributeName].SetDataValue(attributeValue);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}

            attributeValue = null;
            obj = null;
        }

        private void ChangeObjectFuelCapacity(DoubleValue newCapacity, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            double fuelAmount = ((DoubleValue)obj["FuelAmount"].GetDataValue()).value;
            double newCapacityValue = Math.Max(newCapacity.value, fuelAmount); //enforce constraint

            obj["FuelCapacity"].SetDataValue(DataValueFactory.BuildDouble(newCapacityValue));
        }

        private void ChangeObjectFuelAmount(DoubleValue newAmount, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            double fuelCapacity = ((DoubleValue)obj["FuelCapacity"].GetDataValue()).value;
            double newAmountValue = Math.Min(newAmount.value, fuelCapacity); //enforce constraint

            obj["FuelAmount"].SetDataValue(DataValueFactory.BuildDouble(newAmountValue));
        }

        private void ChangeObjectFuelRate(DoubleValue newRate, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["FuelConsumptionRate"].SetDataValue(newRate);
        }

        private void ChangeObjectFuelDepleteState(StringValue newState, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["FuelDepletionState"].SetDataValue(newState);
        }

        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
        }
        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string id = ((StringValue)e["ID"]).value;
            string type = ((StringValue)e["ObjectType"]).value;

            if (objectProxies == null)
            {
                objectProxies = new Dictionary<string, SimulationObjectProxy>();
            }
            SimulationObjectProxy prox = bbClient.GetObjectProxy(id);
            if (prox == null)
            { return; }
            if (!objectProxies.ContainsKey(id))
            {
                objectProxies.Add(id, prox);
            }
            else
            {
                objectProxies[id] = prox;
            }

            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];
            if (e["ID"] != null)
            {
                string id2 = ((StringValue)e["ID"]).value;
                if (objectProxies.ContainsKey(id2))
                {
                    SimulationObjectProxy proxi = objectProxies[id2];

                    // initialize any object values I own.

                    foreach (string attname in atts.attributes.Keys)
                    {
                        if (proxi.GetKeys().Contains(attname) && proxi[attname].IsOwner())
                        {
                            proxi[attname].SetDataValue(atts[attname]);
                        }
                    }
                }
            }


        }
        private SimulationObjectProxy GetObjectProxy(string id)
        {
            SimulationObjectProxy prox = null;

            try
            {
                prox = objectProxies[id];
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error retrieving id = '{0}' from object proxies; {1}", id, ex.Message), ex);
                return null;
            }

            return prox;
        }
        private void RevealObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];
            if (e["ObjectID"] != null)
            {
                string id = ((StringValue)e["ObjectID"]).value;
                SimulationObjectProxy prox = null; // objectProxies[id];
                prox = GetObjectProxy(id);
                if (prox == null)
                    return;

                // initialize any object values I need to.

                foreach (string attname in atts.attributes.Keys)
                {
                    if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                    {
                        prox[attname].SetDataValue(atts[attname]);
                    }
                }
            }
        }



        private void TimeTick(SimulationEvent e)
        {
            int oldTime = time;
            DataValue dv = null;


            dv = e["Time"];
            time = ((IntegerValue)dv).value;

            double dTime = ((double)(time - oldTime)) / 1000;
            SimulationObjectProxy obProx = null;

            double throttle = 0;
            double fuelAmount = 0;
            double fuelConsumptionRate = 0;
            bool dockedToParent = false;
            foreach (string id in objectProxies.Keys)
            {
                obProx = objectProxies[id];

                dv = obProx["FuelConsumptionRate"].GetDataValue();
                fuelConsumptionRate = ((DoubleValue)dv).value;
                if (fuelConsumptionRate == 0)
                {
                    continue;
                }
                dv = obProx["DockedToParent"].GetDataValue();
                dockedToParent = ((BooleanValue)dv).value;
                if (dockedToParent)
                {
                    continue;
                }



                dv = obProx["FuelAmount"].GetDataValue();
                fuelAmount = ((DoubleValue)dv).value;
                if (fuelAmount == 0)
                {
                    continue;
                }

                dv = obProx["Throttle"].GetDataValue();
                throttle = ((DoubleValue)dv).value;
                dv = obProx["FuelAmount"].GetDataValue();
                fuelAmount = ((DoubleValue)dv).value;


                fuelAmount -= dTime * throttle * fuelConsumptionRate;

                if (fuelAmount <= 0)
                {
                    fuelAmount = 0;
                    // Send state change
                    dv = obProx["FuelDepletionState"].GetDataValue();
                    if (dv != null)
                    {
                        SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "StateChange");
                        ((StringValue)sc["ObjectID"]).value = id;
                        ((StringValue)sc["NewState"]).value = ((StringValue)dv).value;
                        ((IntegerValue)sc["Time"]).value = time;
                        distClient.PutEvent(sc);
                    }
                }

                dv = obProx["FuelAmount"].GetDataValue();
                ((DoubleValue)dv).value = fuelAmount;
                obProx["FuelAmount"].SetDataValue(dv);

            }
        }
        private void StateChange(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            string id = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy prox = null; // objectProxies[id];
            prox = GetObjectProxy(id);
            if (prox == null)
                return;

            string newState = ((StringValue)e["NewState"]).value;

            DataValue dv = prox["StateTable"].GetDataValue();
            if (((StateTableValue)dv).states.ContainsKey(newState))
            {
                DataValue dv2 = ((StateTableValue)dv).states[newState];
                foreach (string attname in ((AttributeCollectionValue)dv2).attributes.Keys)
                {
                    if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                    {
                        prox[attname].SetDataValue(((AttributeCollectionValue)dv2).attributes[attname]);
                    }
                }
            }

        }
        public string GetSimulatorName()
        {
            return "FuelHandling";
        }
    }
}
