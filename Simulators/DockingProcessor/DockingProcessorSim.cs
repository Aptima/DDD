using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.DockingProcessor
{
    public class DockingProcessorSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;


        private int randomSeed;
        private System.Random random;

        public DockingProcessorSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
            randomSeed = 0;
            random = new Random(randomSeed);

        }
        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;

            distributor.RegisterClient(ref distClient);
            //distClient.Subscribe("RevealObject");
            //distClient.Subscribe("SubplatformLaunch");
            //distClient.Subscribe("WeaponLaunch");
            //distClient.Subscribe("SubplatformDock");
            //distClient.Subscribe("TimeTick");
            //distClient.Subscribe("ResetSimulation");
            //distClient.Subscribe("StateChange");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "ParentObjectID", true, true);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "DockedToParent", true, true);
            bbClient.Subscribe("PhysicalObject", "ChildObjects", true, true);
            bbClient.Subscribe("PhysicalObject", "DockedObjects", true, true);
            bbClient.Subscribe("PhysicalObject", "DockedWeapons", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchStarted", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchEndTime", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchDestinationLocation", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchDone", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchDuration", true, false);
            bbClient.Subscribe("PhysicalObject", "DockingStarted", true, true);
            bbClient.Subscribe("PhysicalObject", "DockingEndTime", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchIsWeapon", true, true);
            bbClient.Subscribe("PhysicalObject", "LaunchWeaponTargetID", true, true);
            bbClient.Subscribe("PhysicalObject", "DockingDuration", true, false);
            bbClient.Subscribe("PhysicalObject", "IsWeapon", true, false);
            bbClient.Subscribe("PhysicalObject", "ClassName", true, false);

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
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "SubplatformLaunch":
                    SubplatformLaunch(e);
                    break;
                case "WeaponLaunch":
                    WeaponLaunch(e);
                    break;
                case "SubplatformDock":
                    SubplatformDock(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "NewObject":
                    NewObject(e);
                    break;
                case "WeaponLaunchFailure":
                    WeaponLaunchFailure(e);
                    break;
                case "ClientAttackRequest":
                    ClientAttackRequest(e);
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
        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();
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
            bool dockedToParent;
            bool isWeapon;
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];
            string id = ((StringValue)e["ObjectID"]).value;

            SimulationObjectProxy prox = null; // objectProxies[id];
            prox = GetObjectProxy(id);
            if (prox == null)
                return;


            foreach (string attname in atts.attributes.Keys)
            {
                if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                {
                    prox[attname].SetDataValue(atts[attname]);
                }
            }

            SimulationObjectProxy parentProx = null;
            string parentID = null;
            if (atts.attributes.ContainsKey("ParentObjectID"))
            {
                parentID = ((StringValue)atts.attributes["ParentObjectID"]).value;
                if (objectProxies.ContainsKey(parentID))
                {
                    parentProx = objectProxies[parentID];

                }
            }

            if (parentProx != null)
            {
                DataValue dv = parentProx["ChildObjects"].GetDataValue();
                ((StringListValue)dv).strings.Add(id);
                parentProx["ChildObjects"].SetDataValue(dv);

                
                dockedToParent = ((BooleanValue)prox["DockedToParent"].GetDataValue()).value;
                isWeapon = ((BooleanValue)prox["IsWeapon"].GetDataValue()).value;

                if (dockedToParent)
                {
                    if (isWeapon)
                    {
                        dv = parentProx["DockedWeapons"].GetDataValue();
                        ((StringListValue)dv).strings.Add(id);
                        parentProx["DockedWeapons"].SetDataValue(dv);
                    }
                    else
                    {
                        dv = parentProx["DockedObjects"].GetDataValue();
                        ((StringListValue)dv).strings.Add(id);
                        parentProx["DockedObjects"].SetDataValue(dv);
                    }
                }
                
            }

        }
        private void SubplatformLaunch(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            
            string objectID = ((StringValue)e["ObjectID"]).value;
            string parentObjectID = ((StringValue)e["ParentObjectID"]).value;

            SimulationObjectProxy obProx = objectProxies[objectID];
            SimulationObjectProxy parentProx = objectProxies[parentObjectID];

            bool dockedToParent = ((BooleanValue)obProx["DockedToParent"].GetDataValue()).value;
            string realParentID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;
            bool launchStarted = ((BooleanValue)obProx["LaunchStarted"].GetDataValue()).value;

            if (!dockedToParent || launchStarted)
            {
                // TODO Should log an error
                return;
            }
            if (realParentID != parentObjectID)
            {
                // TODO Should log an error
                return;
            }

            int launchDuration = ((IntegerValue)obProx["LaunchDuration"].GetDataValue()).value;

            DataValue dv = obProx["LaunchStarted"].GetDataValue();
            ((BooleanValue)dv).value = true;
            obProx["LaunchStarted"].SetDataValue(dv);

            dv = obProx["LaunchEndTime"].GetDataValue();
            ((IntegerValue)dv).value = time + launchDuration;
            obProx["LaunchEndTime"].SetDataValue(dv);


            obProx["LaunchDestinationLocation"].SetDataValue((LocationValue)e["LaunchDestinationLocation"]);
        }

        private void WeaponLaunch(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string objectID = ((StringValue)e["ObjectID"]).value;
            string parentObjectID = ((StringValue)e["ParentObjectID"]).value;
            string targetObjectID = ((StringValue)e["TargetObjectID"]).value;

            SimulationObjectProxy obProx = objectProxies[objectID];
            SimulationObjectProxy parentProx = objectProxies[parentObjectID];

            bool dockedToParent = ((BooleanValue)obProx["DockedToParent"].GetDataValue()).value;
            string realParentID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;
            bool launchStarted = ((BooleanValue)obProx["LaunchStarted"].GetDataValue()).value;
            bool isWeapon = ((BooleanValue)obProx["IsWeapon"].GetDataValue()).value;

            if (!dockedToParent || launchStarted)
            {
                // TODO Should log an error
                return;
            }
            if (realParentID != parentObjectID)
            {
                // TODO Should log an error
                return;
            }

            if (!isWeapon)
            {
                // TODO Should log an error
                return;
            }

            if (!objectProxies.ContainsKey(targetObjectID))
            {
                // TODO Should log an error
                return;
            }

            int launchDuration = ((IntegerValue)obProx["LaunchDuration"].GetDataValue()).value;

            DataValue dv = obProx["LaunchStarted"].GetDataValue();
            ((BooleanValue)dv).value = true;
            obProx["LaunchStarted"].SetDataValue(dv);

            dv = obProx["LaunchEndTime"].GetDataValue();
            ((IntegerValue)dv).value = time + launchDuration;
            obProx["LaunchEndTime"].SetDataValue(dv);

            Vec3D relLoc = new Vec3D(0, 0, 0);
            dv = relLoc.ToLocationValue();
            ((LocationValue)dv).exists = false;

            obProx["LaunchDestinationLocation"].SetDataValue(dv);


            dv = obProx["LaunchIsWeapon"].GetDataValue();

            ((BooleanValue)dv).value = true;
            obProx["LaunchIsWeapon"].SetDataValue(dv);

            obProx["LaunchWeaponTargetID"].SetDataValue(e["TargetObjectID"]);

        }

        /// <summary>
        /// *Intercepted Event From Client*  When this event is received, you'll need to extract
        /// the weapon's class from the ObjectID field, and use this class along with the 
        /// ParentObjectID to determine which weapon is actually being launched.
        /// </summary>
        /// <param name="e">Incoming event</param>
        private void ClientAttackRequest(SimulationEvent e)
        {
            string playerID = string.Empty;
            string attObjectID = string.Empty;
            string tarObjectID = string.Empty;
            string weaponName = string.Empty;
            int time = ((IntegerValue)e["Time"]).value;
            SimulationObjectProxy attackingObjectProxy;
            SimulationEvent sendingEvent;

            playerID = ((StringValue)e["PlayerID"]).value;
            attObjectID = ((StringValue)e["AttackingObjectID"]).value;
            tarObjectID = ((StringValue)e["TargetObjectID"]).value;
            weaponName = ((StringValue)e["WeaponOrCapabilityName"]).value;

            //// Added now that weapons are sent out with a quantity

            if (weaponName.Contains("x)") && weaponName.Contains("("))
            {//Most likely a weapon with a quantity
                char[] ch = { '(' };
                string[] splits = weaponName.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                weaponName = splits[0].Trim();
            }

            ////

            if (objectProxies.ContainsKey(attObjectID))
            {
                attackingObjectProxy = objectProxies[attObjectID];

                List<string> dockedWeapons = ((StringListValue)attackingObjectProxy["DockedWeapons"].GetDataValue()).strings;
                SimulationObjectProxy weaponProxy;
                foreach (string w in dockedWeapons)
                {
                    weaponProxy = objectProxies[w];
                    if (((StringValue)weaponProxy["ClassName"].GetDataValue()).value == weaponName)
                    {
                        weaponName = w;
                    }
                }

                if (dockedWeapons.Contains(weaponName))
                { //send weapon attack
                    sendingEvent = SimulationEventFactory.BuildEvent(ref simModel, "WeaponLaunchRequest");

                    sendingEvent["UserID"] = DataValueFactory.BuildString(playerID);
                    sendingEvent["ParentObjectID"] = DataValueFactory.BuildString(attObjectID);
                    sendingEvent["TargetObjectID"] = DataValueFactory.BuildString(tarObjectID);
                    sendingEvent["ObjectID"] = DataValueFactory.BuildString(weaponName);
                    sendingEvent["Time"] = DataValueFactory.BuildInteger(time);
                    distClient.PutEvent(sendingEvent);
                    //remove from docked weapons, per MS State bug report.  If weapon launch fails, re-add subplatform to docked weapons
                    DataValue dv = attackingObjectProxy["DockedWeapons"].GetDataValue();
                    bool result = dockedWeapons.Remove(weaponName);
                    if (result)
                    {
                        ((StringListValue)dv).strings = dockedWeapons;
                        attackingObjectProxy["DockedWeapons"].SetDataValue(dv);
                    }
                    else
                    {
                        //should not happen, throw error.
                    }
                }
                else
                {//send capability attack 
                    sendingEvent = SimulationEventFactory.BuildEvent(ref simModel, "AttackObjectRequest");

                    sendingEvent["UserID"] = DataValueFactory.BuildString(playerID);
                    sendingEvent["ObjectID"] = DataValueFactory.BuildString(attObjectID);
                    sendingEvent["TargetObjectID"] = DataValueFactory.BuildString(tarObjectID);
                    sendingEvent["CapabilityName"] = DataValueFactory.BuildString(weaponName);
                    sendingEvent["Time"] = DataValueFactory.BuildInteger(time);
                    distClient.PutEvent(sendingEvent);
                }

            }
        }

        private void WeaponLaunchFailure(SimulationEvent e)
        {
            string weaponID = ((StringValue)e["WeaponObjectID"]).value;
            string parentID = ((StringValue)e["ObjectID"]).value;
            string reason = ((StringValue)e["Reason"]).value;
            SimulationObjectProxy platform;
            List<string> dockedWeapons;
            DataValue dv;

            platform = objectProxies[parentID];
            dv = platform["DockedWeapons"].GetDataValue();
            dockedWeapons = ((StringListValue)dv).strings;
            if (!dockedWeapons.Contains(weaponID))
            {
                dockedWeapons.Add(weaponID);
                ((StringListValue)dv).strings = dockedWeapons;
                platform["DockedWeapons"].SetDataValue(dv);
            }
        }

        private void SubplatformDock(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string objectID = ((StringValue)e["ObjectID"]).value;
            string parentObjectID = ((StringValue)e["ParentObjectID"]).value;

            SimulationObjectProxy obProx = objectProxies[objectID];
            SimulationObjectProxy parentProx = objectProxies[parentObjectID];

            bool dockedToParent = ((BooleanValue)obProx["DockedToParent"].GetDataValue()).value;
            //string realParentID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;

            if (dockedToParent)
            {
                // TODO Should log an error
                return;
            }
            //if (realParentID != parentObjectID)
            //{//now we're able to dock to objects that are not the real parents.

            //need to change the child object's parentID

                DataValue dvp = obProx["ParentObjectID"].GetDataValue();
                ((StringValue)dvp).value = parentObjectID;
                obProx["ParentObjectID"].SetDataValue(dvp);

                //return; //AD removed to handle DockToOther object
            //}

            int dockingDuration = ((IntegerValue)obProx["DockingDuration"].GetDataValue()).value;

            DataValue dv = obProx["DockingStarted"].GetDataValue();
            ((BooleanValue)dv).value = true;
            obProx["DockingStarted"].SetDataValue(dv);

            dv = obProx["DockingEndTime"].GetDataValue();
            ((IntegerValue)dv).value = time + dockingDuration;
            obProx["DockingEndTime"].SetDataValue(dv);
        }

        private void StateChange(SimulationEvent e)
        {
            // If the parent dies, kill the docked children

            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string objectID = ((StringValue)e["ObjectID"]).value;
            string state = ((StringValue)e["NewState"]).value;

            List<string> ids;
            
            if (state == "Dead" && objectProxies.ContainsKey(objectID))
            {
                ids = ((StringListValue)objectProxies[objectID]["DockedObjects"].GetDataValue()).strings;
                foreach (string id in ids)
                {
                    distClient.PutEvent(SimUtility.BuildStateChangeEvent(ref simModel, time, id, "Dead"));
                }
                ids = ((StringListValue)objectProxies[objectID]["DockedWeapons"].GetDataValue()).strings;
                foreach (string id in ids)
                {
                    distClient.PutEvent(SimUtility.BuildStateChangeEvent(ref simModel, time, id, "Dead"));
                }
            }
        }

        private void TimeTick(SimulationEvent e)
        {
            time = ((IntegerValue)e["Time"]).value;

            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            SimulationObjectProxy obProx;
            SimulationObjectProxy parentProx;

            string parentObjectID;
            bool launchStarted, launchDone;
            bool dockingStarted;
            int dockingEndTime;
            if (objectProxies == null)
            {
                objectProxies = bbClient.GetObjectProxies();
            }
            foreach (string objectID in objectProxies.Keys)
            {

                obProx = objectProxies[objectID];
                parentObjectID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;
                if (!objectProxies.ContainsKey(parentObjectID))
                {
                    continue;
                }
                parentProx = objectProxies[parentObjectID];

                launchStarted = ((BooleanValue)obProx["LaunchStarted"].GetDataValue()).value;
                launchDone = ((BooleanValue)obProx["LaunchDone"].GetDataValue()).value;
                DataValue dv;
                if (launchStarted && launchDone)
                {
                    dv = obProx["DockedToParent"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["DockedToParent"].SetDataValue(dv);

                    if (((BooleanValue)obProx["IsWeapon"].GetDataValue()).value)
                    {
                        dv = parentProx["DockedWeapons"].GetDataValue();
                        ((StringListValue)dv).strings.Remove(objectID);
                        parentProx["DockedWeapons"].SetDataValue(dv);
                    }
                    else
                    {
                        dv = parentProx["DockedObjects"].GetDataValue();
                        ((StringListValue)dv).strings.Remove(objectID);
                        parentProx["DockedObjects"].SetDataValue(dv);
                    }

                    dv = obProx["LaunchStarted"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["LaunchStarted"].SetDataValue(dv);

                    dv = obProx["LaunchIsWeapon"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["LaunchIsWeapon"].SetDataValue(dv);

                    dv = obProx["LaunchWeaponTargetID"].GetDataValue();
                    ((StringValue)dv).value = "";
                    obProx["LaunchWeaponTargetID"].SetDataValue(dv);
                }

                dockingStarted = ((BooleanValue)obProx["DockingStarted"].GetDataValue()).value;
                dockingEndTime = ((IntegerValue)obProx["DockingEndTime"].GetDataValue()).value;

                if (dockingStarted && time >= dockingEndTime)
                {
                    dockingStarted = false;
                    dv = obProx["DockingStarted"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["DockingStarted"].SetDataValue(dv);

                    dv = obProx["DockedToParent"].GetDataValue();
                    ((BooleanValue)dv).value = true;
                    obProx["DockedToParent"].SetDataValue(dv);

                    dv = parentProx["DockedObjects"].GetDataValue();
                    ((StringListValue)dv).strings.Add(objectID);
                    parentProx["DockedObjects"].SetDataValue(dv);
                }
            }
            
        }
        public string GetSimulatorName()
        {
            return "DockingProcessor";
        }
    }
}
