using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.StaticAttribute
{

    public class StaticAttributeSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private Dictionary<string, SimulationObjectProxy> objectProxies;
        private int time;

        public StaticAttributeSim()
        {
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
            //distClient.Subscribe("StateChange");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("BaseObject", "ID", true, false);
            bbClient.Subscribe("DecisionMaker", "RoleName", true, true);
            bbClient.Subscribe("DecisionMaker", "TeamMember", true, true);
            bbClient.Subscribe("PhysicalObject", "ObjectName", true, true);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, true);
            bbClient.Subscribe("PhysicalObject", "ClassName", true, true);
            bbClient.Subscribe("PhysicalObject", "Size", true, true);
            bbClient.Subscribe("PhysicalObject", "MaximumSpeed", true, true);
            bbClient.Subscribe("PhysicalObject", "State", true, true);
            bbClient.Subscribe("PhysicalObject", "StateTable", true, true);
            bbClient.Subscribe("PhysicalObject", "Sensors", true, true);
            bbClient.Subscribe("PhysicalObject", "Emitters", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchDuration", true, true);
            bbClient.Subscribe("PhysicalObject", "AttackDuration", true, true);
            bbClient.Subscribe("PhysicalObject", "EngagementDuration", true, true);
            bbClient.Subscribe("PhysicalObject", "DockingDuration", true, true);
            bbClient.Subscribe("PhysicalObject", "IsWeapon", true, true);
            bbClient.Subscribe("PhysicalObject", "IconName", true, true);
            bbClient.Subscribe("PhysicalObject", "RemoveOnDestruction", true, true);
            bbClient.Subscribe("PhysicalObject", "CustomAttributes", true, true);
            bbClient.Subscribe("PhysicalObject", "CanOwn", true, true);
            bbClient.Subscribe("PhysicalObject", "SubplatformLimit", true, true);
            bbClient.Subscribe("PhysicalObject", "DefaultClassification", true, true);
            bbClient.Subscribe("PhysicalObject", "ClassificationDisplayRules", true, true);
            bbClient.Subscribe("PhysicalObject", "CurrentClassification", true, true);
            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        public void ProcessEvent(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();
            switch (e.eventType)
            {
                case "TimeTick":
                    TimeTick(e);
                    break;
                case "NewObject":
                    NewObject(e);
                    break;
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "TransferObject":
                    TransferObject(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "EngramValue":
                    EngramValue(e);
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
                    return;
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
                case "Size":
                    ChangeObjectSize(attributeValue as DoubleValue, objectId);
                    break;
                case "MaximumSpeed":
                    ChangeObjectMaximumSpeed(attributeValue as DoubleValue, objectId);
                    break;
                case "State":
                    ChangeObjectState(attributeValue as StringValue, objectId);
                    break;
                case "ClassName":
                    //?
                    break;
                case "StateTable":
                    //?
                    break;
                case "Sensors":
                    //?
                    break;
                case "Emitters":
                    //?
                    break;
                case "AttackDuration":
                    ChangeAttackDuration(attributeValue as IntegerValue, objectId);
                    break;
                case "EngagementDuration":
                    ChangeEngagementDuration(attributeValue as IntegerValue, objectId);
                    break;
                case "IconName":
                    ChangeObjectIcon(attributeValue as StringValue, objectId);
                    break;
                case "RemoveOnDestruction":
                    ChangeRemoveOnDestruction(attributeValue as BooleanValue, objectId);
                    break;
                case "CanOwn":
                    ChangeCanOwn(attributeValue as StringListValue, objectId);
                    break;
                case "SubplatformLimit":
                    //ignore for now
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

        private void ChangeObjectSize(DoubleValue newSize, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            //DoubleValue objectSize = obj["Size"].GetDataValue() as DoubleValue;
            obj["Size"].SetDataValue(newSize);
        }

        private void ChangeObjectMaximumSpeed(DoubleValue newSpeed, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            
            obj["MaximumSpeed"].SetDataValue(newSpeed);
            double throttle = ((DoubleValue)obj["Throttle"].GetDataValue()).value;
            LocationValue lv = obj["DestinationLocation"].GetDataValue() as LocationValue;

            //If the object was previously in motion, update that path
            if (lv != null)
            {
                if (lv.exists)
                {
                    ResetObjectMovement(objectID, throttle, lv);
                }
            }
        }

        private void ResetObjectMovement(string objectID, double throttle, LocationValue destination)
        {
            SimulationEvent moveObject = SimulationEventFactory.BuildEvent(ref simModel, "MoveObject");
            moveObject["ObjectID"] = DataValueFactory.BuildString(objectID);
            moveObject["DestinationLocation"] = destination;
            moveObject["Throttle"] = DataValueFactory.BuildDouble(throttle);

            distClient.PutEvent(moveObject);
        }

        private void SendStateChangeEvent(string objectID, string newStateName)
        {
            SimulationEvent stateChange = SimulationEventFactory.BuildEvent(ref simModel, "StateChange");
            stateChange["ObjectID"] = DataValueFactory.BuildString(objectID);
            stateChange["NewState"] = DataValueFactory.BuildString(newStateName);

            distClient.PutEvent(stateChange);
        }

        private void ChangeObjectState(StringValue newState, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            string newStateName = newState.value;
            StateTableValue currentStateTable = obj["StateTable"].GetDataValue() as StateTableValue;

            foreach (String stateName in currentStateTable.states.Keys)
            {
                if (stateName == newStateName)
                {
                    SendStateChangeEvent(objectID, newStateName);
                }
            }
        }

        private void ChangeAttackDuration(IntegerValue newAttackDuration, string objectID)
        { 
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["AttackDuration"].SetDataValue(newAttackDuration);
        }

        private void ChangeEngagementDuration(IntegerValue newEngagementDuration, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["EngagementDuration"].SetDataValue(newEngagementDuration);
        }

        private void ChangeObjectIcon(StringValue newIconName, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["IconName"].SetDataValue(newIconName);
        }

        private void ChangeRemoveOnDestruction(BooleanValue newValue, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["RemoveOnDestruction"].SetDataValue(newValue);
        }

        private void ChangeCanOwn(StringListValue newValue, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["CanOwn"].SetDataValue(newValue);
        }

        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
        }
        private void TimeTick(SimulationEvent e)
        {
            time = ((IntegerValue)e["Time"]).value;

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

            string id2 = ((StringValue)e["ID"]).value;
            SimulationObjectProxy proxi = objectProxies[id2];

            if (proxi.GetKeys().Contains("StateTable"))
            {
                proxi["StateTable"].SetDataValue(e["StateTable"]);
            }


            if (objectProxies.ContainsKey(id2))
            {
                // initialize any object values I own.

                foreach (string attname in atts.attributes.Keys)
                {
                    if (proxi.GetKeys().Contains(attname) && proxi[attname].IsOwner())
                    {
                        proxi[attname].SetDataValue(atts[attname]);
                        if (attname == "Sensors")
                        {
                            double maxSensor = -1.0;
                            SensorArrayValue sav = atts[attname] as SensorArrayValue;
                            foreach (SensorValue sv in sav.sensors)
                            {
                                maxSensor = Math.Max(maxSensor, sv.maxRange);
                            }
                            if (maxSensor >= 0)
                            {
                                ObjectDistances.UpdateObjectSensorRange(id2, maxSensor);
                            }
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
            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];
            string id = ((StringValue)e["ObjectID"]).value;

            SimulationObjectProxy prox = null;

            prox = GetObjectProxy(id);
            if (prox == null)
                return;

            //Set State info?
            //AD: This is kind of a quick fix.  Gabe would have a better idea on a more permanent
            //solution.
            DataValue stv = new DataValue();
            stv = prox["StateTable"].GetDataValue();
            Dictionary<string, DataValue> tempDict = ((StateTableValue)stv).states;
            tempDict = ((AttributeCollectionValue)tempDict[((StringValue)atts["State"]).value]).attributes;

            foreach (KeyValuePair<String, DataValue> kvp in tempDict)
            {
                if (!atts.attributes.ContainsKey(kvp.Key))
                {//if att exists in atts, it should NOT overwrite reveal attributes.
                    atts.attributes.Add(kvp.Key, kvp.Value);
                }
            }
            ////AD
            foreach (string attname in atts.attributes.Keys)
            {
                if (attname == "ID")
                {
                    continue;
                }
                if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                {
                    prox[attname].SetDataValue(atts[attname]);
                    if (attname == "Sensors")
                    {
                        double maxSensor = -1.0;
                        SensorArrayValue sav = atts[attname] as SensorArrayValue;
                        foreach (SensorValue sv in sav.sensors)
                        {
                            maxSensor = Math.Max(maxSensor, sv.maxRange);
                        }
                        if (maxSensor >= 0)
                        {
                            ObjectDistances.UpdateObjectSensorRange(id, maxSensor);
                        }
                    }
                }
            }
            Dictionary<string, DataValue> x = new Dictionary<string, DataValue>();

            EmitterValue em = (EmitterValue)prox["Emitters"].GetDataValue();
            foreach (string attName in em.attIsEngram.Keys)
            {
                if (em.attIsEngram[attName])
                {
                    if (StateDB.engrams.ContainsKey(attName))
                    {
                        x[attName] = DataValueFactory.BuildString(StateDB.engrams[attName].engramValue);
                    }
                }
            }

            prox["CustomAttributes"].SetDataValue(DataValueFactory.BuildCustomAttributes(x));

        }
        private void StateChange(SimulationEvent e)
        {
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
                //AD: Added state to attributes
                DataValue temp = new StringValue();
                ((StringValue)temp).value = newState;
                ((AttributeCollectionValue)dv2).attributes["State"] = temp;
                //AD
                foreach (string attname in ((AttributeCollectionValue)dv2).attributes.Keys)
                {
                    if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                    {
                        prox[attname].SetDataValue(((AttributeCollectionValue)dv2).attributes[attname]);
                        if (attname == "Sensors")
                        {
                            double maxSensor = -1.0;
                            SensorArrayValue sav = ((AttributeCollectionValue)dv2).attributes[attname] as SensorArrayValue;
                            foreach (SensorValue sv in sav.sensors)
                            {
                                maxSensor = Math.Max(maxSensor, sv.maxRange);
                            }
                            if (maxSensor >= 0)
                            {
                                ObjectDistances.UpdateObjectSensorRange(id, maxSensor);
                            }
                        }
                    }
                }
            }
            CustomAttributesValue cav = prox["CustomAttributes"].GetDataValue() as CustomAttributesValue;

            Dictionary<string, DataValue> x = new Dictionary<string, DataValue>();
            if (cav != null)
            {
                x = cav.attributes;
            }

            EmitterValue em = (EmitterValue)prox["Emitters"].GetDataValue();
            foreach (string attName in em.attIsEngram.Keys)
            {
                if (em.attIsEngram[attName])
                {
                    if (StateDB.engrams.ContainsKey(attName))
                    {
                        x[attName] = DataValueFactory.BuildString(StateDB.engrams[attName].engramValue);
                    }
                }
            }

            prox["CustomAttributes"].SetDataValue(DataValueFactory.BuildCustomAttributes(x));
        }

        private void EngramValue(SimulationEvent e)
        {
            SimulationObjectProxy obProx;
            string engramName = ((StringValue)e["EngramName"]).value;
            string engramValue = ((StringValue)e["EngramValue"]).value;
            string engramDataType = ((StringValue)e["EngramDataType"]).value;
            string objectID = string.Empty;

            objectID = ((StringValue)e["SpecificUnit"]).value;
            if (objectID != string.Empty)
            {
                obProx = objectProxies[objectID];
                if (!StateDB.physicalObjects.ContainsKey(objectID))
                {
                    return;
                }
                EmitterValue em = (EmitterValue)obProx["Emitters"].GetDataValue();
                CustomAttributesValue cus = (CustomAttributesValue)obProx["CustomAttributes"].GetDataValue();
                if (em.attIsEngram.ContainsKey(engramName))
                {
                    cus.attributes[engramName] = DataValueFactory.BuildString(engramValue);
                    obProx["CustomAttributes"].SetDataValue(cus);
                }
                //Add the qualified engram name to the collection.  
                //Does not need to be the same qualified name as the scencon uses.
                StateDB.UpdateEngrams(String.Format("{0}|{1}", objectID, engramName), engramValue, engramDataType);

                return;
            }
            else
            {
                StateDB.UpdateEngrams(engramName, engramValue, engramDataType);

                foreach (string id in objectProxies.Keys)
                {
                    obProx = objectProxies[id];
                    if (!StateDB.physicalObjects.ContainsKey(id))
                    {
                        continue;
                    }
                    EmitterValue em = (EmitterValue)obProx["Emitters"].GetDataValue();
                    CustomAttributesValue cus = (CustomAttributesValue)obProx["CustomAttributes"].GetDataValue();
                    if (em.attIsEngram.ContainsKey(engramName))
                    {
                        cus.attributes[engramName] = DataValueFactory.BuildString(engramValue);
                        obProx["CustomAttributes"].SetDataValue(cus);
                    }
                }
                return;
            }
        }

        private void TransferObject(SimulationEvent e)
        {
            string objectID = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy prox = objectProxies[objectID];

            string userID = ((StringValue)e["UserID"]).value;

            string donerUserID = ((StringValue)e["DonorUserID"]).value;

            prox["OwnerID"].SetDataValue(DataValueFactory.BuildString(userID));

            StateDB.physicalObjects[objectID].ownerID = userID;

            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                time,
                                userID,
                                String.Format("Ownership of {0} has been transferred from {1} to {2}", objectID, donerUserID, userID)));
            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                time,
                                donerUserID,
                                String.Format("Ownership of {0} has been transferred from {1} to {2}", objectID, donerUserID, userID)));
        }

        public string GetSimulatorName()
        {
            return "StaticAttribute";
        }
    }

}

