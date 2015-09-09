using System;
using System.Collections.Generic;
using System.Text;


using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.SelfDefense
{

    public class SelfDefenseSim : ISimulator
    {
        /*
        public class Team
        {
            public string id;
            public List<string> hostility;
            public Team(string id, List<string> hostility)
            {
                this.id = id;
                this.hostility = hostility;
            }
        }
        public class DecisionMaker
        {
            public string id;
            public Team team;
            public bool isHuman;

            public DecisionMaker(string id, Team team)
            {
                this.isHuman = false;
                this.id = id;
                this.team = team;
            }

            public bool isHostile(DecisionMaker dm)
            {
                if (team == null)
                {
                    return false;
                }

                if (team.hostility.Contains(dm.team.id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
         * */

        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;


        private int randomSeed;
        private System.Random random;

        //private Dictionary<string, Team> teams;
        //private Dictionary<string, DecisionMaker> decisionMakers;

        public SelfDefenseSim()
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

            //teams = new Dictionary<string, Team>();
            //decisionMakers = new Dictionary<string, DecisionMaker>();
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
            //distClient.Subscribe("HandshakeInitializeGUIDone");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "ClassName", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "Capability", true, false);
            bbClient.Subscribe("PhysicalObject", "Vulnerability", true, false);
            bbClient.Subscribe("PhysicalObject", "IsWeapon", true, false);
            bbClient.Subscribe("PhysicalObject", "PursueStarted", true, false);
            bbClient.Subscribe("PhysicalObject", "PursueTargetID", true, false);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseStartAttack", true, true);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseCapability", true, true);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseTargetID", true, true);
            bbClient.Subscribe("PhysicalObject", "TargetsInRange", true, false);
            bbClient.Subscribe("PhysicalObject", "CurrentAttacks", true, false);
            bbClient.Subscribe("PhysicalObject", "AttackState", true, false);
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
                //case "HandshakeInitializeGUIDone":
                //    HandshakeInitializeGUIDone(e);
                //    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "PlayerControl":
                    PlayerControl(e);
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
            //teams.Clear();
            //decisionMakers.Clear();
            //StateDB.Reset();
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
            if (atts.attributes.ContainsKey("ID"))  //REMOVE WHEN DONE MIGRATING REVEAL
            {
                string id2 = ((StringValue)atts["ID"]).value;
                if (objectProxies.ContainsKey(id2))
                {
                    SimulationObjectProxy proxi = objectProxies[id2];

                    foreach (string attname in atts.attributes.Keys)
                    {
                        if (proxi.GetKeys().Contains(attname) && proxi[attname].IsOwner())
                        {
                            proxi[attname].SetDataValue(atts[attname]);
                        }
                    }
                }
            }

            /*
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
            }*/

        }
        private void PlayerControl(SimulationEvent e)
        {
            string id = ((StringValue)e["DecisionMakerID"]).value;
            string controlledBy = ((StringValue)e["ControlledBy"]).value;

            if (StateDB.decisionMakers.ContainsKey(id))
            {
                if (controlledBy == "HUMAN")
                {
                    StateDB.decisionMakers[id].isHuman = true;
                }
                else
                {
                    StateDB.decisionMakers[id].isHuman = false;
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
            string id = ((StringValue)e["ObjectID"]).value;

            SimulationObjectProxy prox = null; // objectProxies[id];
            prox = GetObjectProxy(id);
            if (prox == null)
                return;


            foreach (string attname in atts.attributes.Keys)
            {
                if (attname == "ID")
                {
                    continue;
                }
                if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                {
                    prox[attname].SetDataValue(atts[attname]);
                }
            }

        }
        /*
        private CapabilityValue.Effect FindCapabilityEffect(CapabilityValue cap, VulnerabilityValue vuln)
        {
            List<string> vulnNames = new List<string>();

            foreach (VulnerabilityValue.Transition t in vuln.transitions)
            {
                foreach (VulnerabilityValue.TransitionCondition tc in t.conditions)
                {
                    if (!vulnNames.Contains(tc.capability))
                    {
                        vulnNames.Add(tc.capability);
                    }
                }
            }

            foreach (CapabilityValue.Effect e in cap.effects)
            {
                if (vulnNames.Contains(e.name))
                {
                    return e;
                }
            }
            return null;
        }*/

        private void TimeTick(SimulationEvent e)
        {
            if (objectProxies == null)
            {
                objectProxies = bbClient.GetObjectProxies();
            }
            time = ((IntegerValue)e["Time"]).value;

            DataValue dv = null;

            SimulationObjectProxy obProx = null, targetProx = null;

            Vec3D myLoc;
            Vec3D targetLoc;
            string targetID;
            bool isWeapon = false;
            bool pursueStarted;

            CapabilityValue capabilities;
            VulnerabilityValue vulnerabilities;
            CapabilityValue.Effect effect;
            double distance;
            LocationValue location;
            
            foreach (string id in objectProxies.Keys)
            {
                obProx = objectProxies[id];

                // update the StateDB info

                if (StateDB.physicalObjects[id].ownerID == "")
                {
                    string owner = ((StringValue)obProx["OwnerID"].GetDataValue()).value;
                    if (StateDB.decisionMakers.ContainsKey(owner))
                    {
                        StateDB.physicalObjects[id].ownerID = owner;
                    }
                }

                if (StateDB.physicalObjects[id].speciesName == "")
                {
                    string speciesName = ((StringValue)obProx["ClassName"].GetDataValue()).value;
                    StateDB.physicalObjects[id].speciesName = speciesName;
                }

                if (StateDB.physicalObjects[id].ownerID != "" && StateDB.physicalObjects[id].teamName == "")
                {
                    if (StateDB.decisionMakers[StateDB.physicalObjects[id].ownerID].team != null)
                    {
                        StateDB.physicalObjects[id].teamName = StateDB.decisionMakers[StateDB.physicalObjects[id].ownerID].team.id;
                    }
                }

                if (((StringValue)obProx["AttackState"].GetDataValue()).value == "BEING_ATTACKED")
                {
                    continue;
                }

                AttackCollectionValue attCV = (AttackCollectionValue)obProx["CurrentAttacks"].GetDataValue();
                if (attCV.GetCurrentSelfDefenseAttacks().Count == 0)
                //if (((StringValue)obProx["AttackTargetID"].GetDataValue()).value == "")
                {
                    dv = obProx["SelfDefenseStartAttack"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["SelfDefenseStartAttack"].SetDataValue(dv);

                    dv = obProx["SelfDefenseCapability"].GetDataValue();
                    ((StringValue)dv).value = "";
                    obProx["SelfDefenseCapability"].SetDataValue(dv);

                    dv = obProx["SelfDefenseTargetID"].GetDataValue();
                    ((StringValue)dv).value = "";
                    obProx["SelfDefenseTargetID"].SetDataValue(dv);
                }

                if (((BooleanValue)obProx["SelfDefenseStartAttack"].GetDataValue()).value == true)
                {
                    continue;
                }

                isWeapon = ((BooleanValue)obProx["IsWeapon"].GetDataValue()).value;
                pursueStarted = ((BooleanValue)obProx["PursueStarted"].GetDataValue()).value;
                targetID = ((StringValue)obProx["PursueTargetID"].GetDataValue()).value;

                capabilities = (CapabilityValue)obProx["Capability"].GetDataValue();

                if (isWeapon && pursueStarted && objectProxies.ContainsKey(targetID))
                {
                    targetProx = objectProxies[targetID];

                    location = (LocationValue)obProx["Location"].GetDataValue();
                    if (!location.exists)
                    {
                        continue;
                    }
                    myLoc = new Vec3D(location);

                    location = (LocationValue)targetProx["Location"].GetDataValue();
                    if (!location.exists)
                    {
                        continue;
                    }
                    targetLoc = new Vec3D(location);

                    distance = myLoc.ScalerDistanceTo(targetLoc);

                    
                    vulnerabilities = (VulnerabilityValue)targetProx["Vulnerability"].GetDataValue();

                    //string cap = FindCapability(capabilities, vulnerabilities);
                    effect = SimUtility.FindCapabilityEffect(capabilities, vulnerabilities);
                    if (effect != null && distance < effect.range)
                    {
                        dv = obProx["SelfDefenseStartAttack"].GetDataValue();
                        ((BooleanValue)dv).value = true;
                        obProx["SelfDefenseStartAttack"].SetDataValue(dv);

                        dv = obProx["SelfDefenseCapability"].GetDataValue();
                        ((StringValue)dv).value = effect.name;
                        obProx["SelfDefenseCapability"].SetDataValue(dv);

                        dv = obProx["SelfDefenseTargetID"].GetDataValue();
                        ((StringValue)dv).value = targetID;
                        obProx["SelfDefenseTargetID"].SetDataValue(dv);

                    }
                    else if (effect == null && distance < 1)
                    {
                        distClient.PutEvent(SimUtility.BuildStateChangeEvent(ref simModel, time, id, "Dead"));
                    }

                    continue;
                }

                string ownerID = ((StringValue)obProx["OwnerID"].GetDataValue()).value;
                if (StateDB.decisionMakers.ContainsKey(ownerID))
                {
                    StateDB.DecisionMaker thisDM = StateDB.decisionMakers[ownerID];
                    List<string> targets = ((StringListValue)obProx["TargetsInRange"].GetDataValue()).strings;

                    //if (targets.Count == 0)
                    //{
                    //    continue;
                    //}
                    foreach (string target in targets)
                    {
                        SimulationObjectProxy obProx2 = objectProxies[target];
                        string ownerID2 = ((StringValue)obProx2["OwnerID"].GetDataValue()).value;
                        if (!StateDB.decisionMakers.ContainsKey(ownerID2))
                        {
                            continue;
                        }
                        StateDB.DecisionMaker thatDM = StateDB.decisionMakers[ownerID2];
                        vulnerabilities = (VulnerabilityValue)obProx2["Vulnerability"].GetDataValue();
                        effect = SimUtility.FindCapabilityEffect(capabilities, vulnerabilities);
                        
                        if (effect != null && thisDM.isHostile(thatDM) && !thisDM.isHuman)
                        {
                            dv = obProx["SelfDefenseStartAttack"].GetDataValue();
                            ((BooleanValue)dv).value = true;
                            obProx["SelfDefenseStartAttack"].SetDataValue(dv);

                            dv = obProx["SelfDefenseCapability"].GetDataValue();
                            ((StringValue)dv).value = effect.name;
                            obProx["SelfDefenseCapability"].SetDataValue(dv);

                            dv = obProx["SelfDefenseTargetID"].GetDataValue();
                            ((StringValue)dv).value = target;
                            obProx["SelfDefenseTargetID"].SetDataValue(dv);
                            break;
                        }
                        

                    }

                    
                }

            }
        }
        public string GetSimulatorName()
        {
            return "SelfDefense";
        }
    }
}
