using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;
using V3DVTE_Utils;

namespace Aptima.Asim.DDD.Simulators.ExternalCommunication
{

    public class ExternalCommunicationSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private Dictionary<string, SimulationObjectProxy> objectProxies;
        private int time;

        private V3DVTE_Publisher v3;

        private const string _baseEntity = "BaseEntity";
        private const string _physicalEntity = "BaseEntity.PhysicalEntity";
        private const string _munition = "BaseEntity.PhysicalEntity.Munition";
        private const string _aircraft = "BaseEntity.PhysicalEntity.Platform.Aircraft";
        private const string _seaSurface = "BaseEntity.PhysicalEntity.Platform.SurfaceVessel";
        private const string _landUnit = "BaseEntity.PhysicalEntity.Platform.GroundVehicle";

        public ExternalCommunicationSim()
        {
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
            v3 = null;
        }
        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;

            //distributor.RegisterClient(ref distClient);
            //distClient.Subscribe("NewObject");
            //distClient.Subscribe("RevealObject");
            //distClient.Subscribe("AttackSucceeded");
            //distClient.Subscribe("AttackObject");
            //distClient.Subscribe("WeaponLaunch");
            //distClient.Subscribe("ExternalApp_SimStop");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "ParentObjectID", true, false);
            //bbClient.Subscribe("PhysicalObject", "ClassName", true, true);
            //bbClient.Subscribe("PhysicalObject", "Size", true, true);
            //bbClient.Subscribe("PhysicalObject", "MaximumSpeed", true, true);
            //bbClient.Subscribe("PhysicalObject", "State", true, true);
            //bbClient.Subscribe("PhysicalObject", "StateTable", true, true);
            //bbClient.Subscribe("PhysicalObject", "Sensors", true, true);
            //bbClient.Subscribe("PhysicalObject", "Emitters", true, true);
            //bbClient.Subscribe("PhysicalObject", "LaunchDuration", true, true);
            //bbClient.Subscribe("PhysicalObject", "AttackDuration", true, true);
            //bbClient.Subscribe("PhysicalObject", "DockingDuration", true, true);
            bbClient.Subscribe("PhysicalObject", "IsWeapon", true, false);
            //bbClient.Subscribe("PhysicalObject", "IconName", true, true);
            //bbClient.Subscribe("PhysicalObject", "RemoveOnDestruction", true, true);
            //bbClient.Subscribe("PhysicalObject", "CustomAttributes", true, true);
            objectProxies = new Dictionary<string, SimulationObjectProxy>();


            v3 = new V3DVTE_Publisher();
            string errMsg = string.Empty;

            //v3.Initialize("Aptima_DDD_Test",
            //              @"C:\svnroot\phoenix\DataFiles\v3_dvte.fed",
            //              @"C:\svnroot\phoenix\DataFiles\v3_dvte.xml",
            //              out errMsg);
            bool v3Result = v3.Initialize(ServerOptions.HLAFederationExecutionName,
                          ServerOptions.HLAFederationFilePath,
                          ServerOptions.HLAXMLFilePath,
                          out errMsg);
            if (!v3Result)
            {
                ServerOptions.HLAExport = false;
                throw new Exception("Error initializing V3DVTE Publisher. " + errMsg);
            }
            List<string> interactions = new List<string>();
            interactions.Add("DamageAssessment");
            interactions.Add("DVTEVehicleEmbark.DVTEDisembarkResponse");
            interactions.Add("WeaponFire");
            interactions.Add("MunitionDetonation");

            List<string> attributes = new List<string>();
            attributes.Add("comment");
            attributes.Add("WorldLocation");
            bool bInteractions = v3.PublishInteractions(interactions);
            bool bObjects = v3.PublishObjectClass(_physicalEntity, attributes);
            bObjects = v3.PublishObjectClass(_aircraft, attributes);
            bObjects = v3.PublishObjectClass(_seaSurface, attributes);
            bObjects = v3.PublishObjectClass(_landUnit, attributes);
            bObjects = v3.PublishObjectClass(_munition, attributes);



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
                case "AttackSucceeded":
                    AttackSucceeded(e);
                    break;
                case "AttackObject":
                    AttackObject(e);
                    break;
                case "ResetSimulation":
                    ResetSimulation();
                    break;
                case "WeaponLaunch":
                    WeaponLaunch(e);
                    break;
                case "ExternalApp_SimStop":
                    ExternalApp_SimStop(e);
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

        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
        }
        private void TimeTick(SimulationEvent e)
        {
            Dictionary<string, string> objAttributes = new Dictionary<string, string>();
            time = ((IntegerValue)e["Time"]).value;

            LocationValue lv;
            if (v3 != null)
            {
                foreach (string id in objectProxies.Keys)
                {
                    string type = objectProxies[id].GetObjectType();
                    bool isWeapon = ((BooleanValue)objectProxies[id]["IsWeapon"].GetDataValue()).value;

                    if (isWeapon)
                    {
                        type = "Munition";
                    }
                    objAttributes.Clear();
                    lv = (LocationValue)objectProxies[id]["Location"].GetDataValue();
                    switch (type)
                    {
                        case "AirObject":

                            objAttributes.Add("comment", time.ToString());
                            objAttributes.Add("WorldLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", lv.X, lv.Y, lv.Z));
                            v3.UpdateObjectAttributes(id, _aircraft, objAttributes, time.ToString());
                            break;
                        case "SeaObject":
                            objAttributes.Add("comment", time.ToString());
                            objAttributes.Add("WorldLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", lv.X, lv.Y, lv.Z));
                            v3.UpdateObjectAttributes(id, _seaSurface, objAttributes, time.ToString());
                            break;
                        case "LandObject":
                            objAttributes.Add("comment", time.ToString());
                            objAttributes.Add("WorldLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", lv.X, lv.Y, lv.Z));
                            v3.UpdateObjectAttributes(id, _landUnit, objAttributes, time.ToString());
                            break;

                        case "Munition":
                            objAttributes.Add("comment", time.ToString());
                            objAttributes.Add("WorldLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", lv.X, lv.Y, lv.Z));
                            v3.UpdateObjectAttributes(id, _munition, objAttributes, time.ToString());
                            break;
                        default:
                            Console.WriteLine("ExternalCommunicationSim: On TimeTick, object type \"" + type + "\" not found for object " + id);
                            break;
                    }


                }
                v3.Tick();

            }

        }
        private void NewObject(SimulationEvent e)
        {
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

            if (v3 != null)
            {

            }

        }
        private void RevealObject(SimulationEvent e)
        {
            if (v3 != null)
            {
                string id = ((StringValue)e["ObjectID"]).value;
                string type = objectProxies[id].GetObjectType();
                bool isWeapon = ((BooleanValue)objectProxies[id]["IsWeapon"].GetDataValue()).value;

                if (isWeapon)
                {
                    type = "Munition";
                }
                switch (type)
                {
                    case "AirObject":
                        v3.RegisterObject(id, _aircraft);
                        break;
                    case "SeaObject":
                        v3.RegisterObject(id, _seaSurface);
                        break;
                    case "LandObject":
                        v3.RegisterObject(id, _landUnit);
                        break;

                    case "Munition":
                        v3.RegisterObject(id, _munition);
                        break;
                }

            }

        }
        private void AttackSucceeded(SimulationEvent e)
        {
            if (v3 != null)
            {
                string id = ((StringValue)e["ObjectID"]).value;
                string firingID = null;
                bool isWeapon = ((BooleanValue)objectProxies[id]["IsWeapon"].GetDataValue()).value;

                if (isWeapon)
                {
                    firingID = ((StringValue)objectProxies[id]["ParentObjectID"].GetDataValue()).value;
                }
                else
                {
                    firingID = id;
                }
                string targetID = ((StringValue)e["TargetID"]).value;
                string newState = ((StringValue)e["NewState"]).value;
                LocationValue targetlv = (LocationValue)objectProxies[targetID]["Location"].GetDataValue();
                LocationValue firinglv = (LocationValue)objectProxies[firingID]["Location"].GetDataValue();

                Dictionary<string, string> stateChangeParams = new Dictionary<string, string>();
                stateChangeParams.Add("FiringObjectIdentifier", firingID);
                stateChangeParams.Add("FiringObjectLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", firinglv.X, firinglv.Y, firinglv.Z));
                //stateChangeParams.Add("MunitionName", "Sensor");
                stateChangeParams.Add("TargetObjectIdentifier", targetID);
                stateChangeParams.Add("TargetObjectLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", targetlv.X, targetlv.Y, targetlv.Z));
                stateChangeParams.Add("NewDamageState", newState);
                v3.SendInteraction("DamageAssessment", stateChangeParams, time.ToString());
                v3.Tick();
            }

        }

        private void WeaponLaunch(SimulationEvent e)
        {
            if (v3 != null)
            {
                string munitionID = ((StringValue)e["ObjectID"]).value;
                string targetID = ((StringValue)e["TargetObjectID"]).value;
                string parentID = ((StringValue)e["ParentObjectID"]).value;



                Dictionary<string, string> launchParams = new Dictionary<string, string>();
                launchParams.Add("FiringObjectIdentifier", parentID);
                launchParams.Add("MunitionObjectIdentifier", munitionID);
                launchParams.Add("TargetObjectIdentifier", targetID);
                v3.SendInteraction("WeaponFire", launchParams, time.ToString());
                v3.Tick();
            }
        }

        private void AttackObject(SimulationEvent e)
        {
            if (v3 != null)
            {
                string id = ((StringValue)e["ObjectID"]).value;
                string targetID = ((StringValue)e["TargetObjectID"]).value;
                string capName = ((StringValue)e["CapabilityName"]).value;
                LocationValue lv = (LocationValue)objectProxies[id]["Location"].GetDataValue();

                Dictionary<string, string> attackParams = new Dictionary<string, string>();
                attackParams.Add("MunitionObjectIdentifier", id);
                attackParams.Add("DetonationLocation", String.Format("<X>{0}</X><Y>{1}</Y><Z>{2}</Z>", lv.X, lv.Y, lv.Z));
                attackParams.Add("MunitionType", capName);
                attackParams.Add("TargetObjectIdentifier", targetID);
                v3.SendInteraction("MunitionDetonation", attackParams, time.ToString());
                v3.Tick();
            }
        }

        private void ExternalApp_SimStop(SimulationEvent e)
        {
            if (v3 != null)
            {
                v3.Cleanup();
                v3 = null;
            }
        }

        public string GetSimulatorName()
        {
            return "ExternalCommunication";
        }
    }

}

