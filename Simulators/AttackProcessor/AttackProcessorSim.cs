using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

/*
 * Removed these attributes from objects:
 * <Attribute Name="AttackStartTime" DataType="IntegerType" ExcludeFromScenario="true"/>
				<Attribute Name="AttackTimeWindow" DataType="IntegerType" ExcludeFromScenario="true"/>
				<Attribute Name="AttackTargetID" DataType="StringType" ExcludeFromScenario="true"/>
				<Attribute Name="AttackCapability" DataType="StringType" ExcludeFromScenario="true"/>
 * In Place of CurrentAttacks, which is a AttackCollectionType object which encapsulates the different attack info
 */


namespace Aptima.Asim.DDD.Simulators.AttackProcessor
{

    public class AttackProcessorSim : ISimulator
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

        public AttackProcessorSim()
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
            //distClient.Subscribe("NewObject");
            //distClient.Subscribe("RevealObject");
            //distClient.Subscribe("AttackObject");
            //distClient.Subscribe("RandomSeed");
            //distClient.Subscribe("TimeTick");
            //distClient.Subscribe("StateChange");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "State", true, false);
            bbClient.Subscribe("PhysicalObject", "StateTable", true, false);
            bbClient.Subscribe("PhysicalObject", "Capability", true, true);
            bbClient.Subscribe("PhysicalObject", "Vulnerability", true, true);
            bbClient.Subscribe("PhysicalObject", "AttackState", true, true);

            bbClient.Subscribe("PhysicalObject", "CurrentAttacks", true, true);
            bbClient.Subscribe("PhysicalObject", "AttackerList", true, true);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseStartAttack", true, false);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseCapability", true, false);
            bbClient.Subscribe("PhysicalObject", "SelfDefenseTargetID", true, false);
            bbClient.Subscribe("PhysicalObject", "IsWeapon", true, false);
            bbClient.Subscribe("PhysicalObject", "AttackDuration", true, false); //set in static att sim, true for all attacks per state per species
            bbClient.Subscribe("PhysicalObject", "EngagementDuration", true, false);
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
                case "AttackObject":
                    AttackObject(e);
                    break;
                case "RandomSeed":
                    RandomSeed(e);
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "ForceUpdateObjectAttribute":
                    ForceUpdateObjectAttribute(e);
                    break;
                case "UpdateAttackTimeWindow":
                    UpdateAttackTimeWindow(e);
                    break;
                case "CancelEngagement":
                    CancelEngagement(e);
                    break;
                default:
                    break;
            }
        }

        private void UpdateAttackTimeWindow(SimulationEvent e)
        {
            string targetObjectId = ((StringValue)e["TargetObjectID"]).value;
            string attackingObjectId = ((StringValue)e["AttackingObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;
            int newAttackWindow = ((IntegerValue)e["NewAttackTimeWindow"]).value;

            //get attacker proxy
            SimulationObjectProxy attacker = objectProxies[attackingObjectId];
            AttackCollectionValue attacks = attacker["CurrentAttacks"].GetDataValue() as AttackCollectionValue;

            //find engagement between attacker and target
            AttackCollectionValue.AttackValue av = null;
            foreach (AttackCollectionValue.AttackValue att in attacks.GetCurrentAttacksOnTarget(targetObjectId))
            {
                if (att.capabilityName == capabilityName && att.attackingObjectId == attackingObjectId && att.targetObjectId == targetObjectId)
                {
                    av = att;
                    break;
                }
            }

            if (av != null)
            { 
                //set the CurrentAttacks time window accordingly            
                av.attackTimeWindow = newAttackWindow;

                attacker["CurrentAttacks"].SetDataValue(attacks);
            }
        }

        private void CancelEngagement(SimulationEvent e)
        {
            string targetObjectId = ((StringValue)e["TargetObjectID"]).value;
            string attackingObjectId = ((StringValue)e["AttackingObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;

            CancelAttack(attackingObjectId, targetObjectId, capabilityName);
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
            //Capability
            //Vulnerability
            //Attack State
            //Current Attacks
            //Attacker List
            switch (attributeName)
            {
                case "Capability":
                    //change object's capability set, update attacks
                    CapabilityValue capVal = attributeValue as CapabilityValue;
                    ChangeObjectCapability(capVal, objectId);
                    return;
                    break;
                case "Vulnerability":
                    //change object's vulnerability set, update attacks
                    VulnerabilityValue vulValue = attributeValue as VulnerabilityValue;
                    ChangeObjectVulnerability(vulValue, objectId);
                    return;
                    break;
                case "AttackState":
                    //if changing from "BEING_ATTACKED" to not, then cancel all attacks

                    return;
                    break;
                case "CurrentAttacks":
                    //for now, do not let them modify this
                    return;
                    break;
                case "AttackerList":
                    //any missing attackers should have their attacks cancelled

                    return;
                    break;
                default:
                    //somehow you are able to publish, but it's not one of the current attributes as of 12/9
                    return;
                    break;
            }

            try
            {
                obj[attributeName].SetDataValue(attributeValue);
            }
            catch (Exception ex)
            {
                return;
            }

            attributeValue = null;
            obj = null;
        }

        private void ChangeObjectCapability(CapabilityValue newCapabilitySet, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;

            CapabilityValue previousCapabilityValue = obj["Capability"].GetDataValue() as CapabilityValue;
            List<string> previousCapabilityNames = previousCapabilityValue.GetCapabilityNames();
            List<string> newCapabilityNames = newCapabilitySet.GetCapabilityNames();
            List<string> missingCapabilities = new List<string>();
            foreach (string capName in previousCapabilityNames)
            {
                if (!newCapabilityNames.Contains(capName))
                {
                    if (!missingCapabilities.Contains(capName))
                    {
                        missingCapabilities.Add(capName);
                    }
                }
            }
            foreach (string capName in missingCapabilities)
            {
                CancelCapabilityAttack(capName, objectID);
            }

            obj["Capability"].SetDataValue(newCapabilitySet);
            missingCapabilities.Clear();
            newCapabilityNames.Clear();
            previousCapabilityNames.Clear();
        }

        private void ChangeObjectVulnerability(VulnerabilityValue newVulnerabilitySet, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;

            VulnerabilityValue previousVulnerabilityValue = obj["Vulnerability"].GetDataValue() as VulnerabilityValue;
            List<string> previousVulnerabilityNames = new List<string>();
            foreach (VulnerabilityValue.Transition tr in previousVulnerabilityValue.transitions)
            {
                foreach (VulnerabilityValue.TransitionCondition tc in tr.conditions)
                {
                    if (!previousVulnerabilityNames.Contains(tc.capability))
                    {
                        previousVulnerabilityNames.Add(tc.capability);
                    }
                }
            }
            List<string> newVulnerabilityNames = new List<string>();
            foreach (VulnerabilityValue.Transition tr in newVulnerabilitySet.transitions)
            {
                foreach (VulnerabilityValue.TransitionCondition tc in tr.conditions)
                {
                    if (!newVulnerabilityNames.Contains(tc.capability))
                    {
                        newVulnerabilityNames.Add(tc.capability);
                    }
                }
            }
            List<string> missingVulnerabilityNames = new List<string>();
            foreach (string capName in previousVulnerabilityNames)
            {
                if (!newVulnerabilityNames.Contains(capName))
                {
                    if (!missingVulnerabilityNames.Contains(capName))
                    {
                        missingVulnerabilityNames.Add(capName);
                    }
                }
            }
            List<string> attackerList = ((StringListValue)obj["AttackerList"].GetDataValue()).strings;
            List<string> updatedAttackerList = new List<string>();
            foreach (string attacker in attackerList)
            { 
                //get attacker's current attacks
                AttackCollectionValue currentAttacks = objectProxies[attacker]["CurrentAttacks"].GetDataValue() as AttackCollectionValue;
                List<AttackCollectionValue.AttackValue> attacksOnTarget = currentAttacks.GetCurrentAttacksOnTarget(objectID);
                List<AttackCollectionValue.AttackValue> allAttacks = currentAttacks.GetCurrentAttacks();

                //foreach attack
                foreach (AttackCollectionValue.AttackValue av in attacksOnTarget)
                {
                    if (missingVulnerabilityNames.Contains(av.capabilityName))
                    {
//if this attack is targetting my object with a capability that's in missingVulnerabilities, cancel the attack
                        CancelAttack(attacker, objectID, av.capabilityName);
                    }
                }
                if (allAttacks.Count > attacksOnTarget.Count)
                {
                    updatedAttackerList.Add(attacker);
                }
            }
            obj["AttackerList"].SetDataValue(DataValueFactory.BuildStringList(updatedAttackerList));
            obj["Vulnerability"].SetDataValue(newVulnerabilitySet);
            previousVulnerabilityNames.Clear();
            newVulnerabilityNames.Clear();

        }

        private void ChangeObjectAttackState(string newAttackState, string objectID)
        { }

        private void ChangeObjectAttackerList(List<string> newAttackerList, string objectID)
        { 
            //updated attacker list

            //get objectID's attacker list

            //see which previous attackers are not on this updated attacker list

            //foreach missing attacker

            //get the attacker proxy

            //get attacker CurrentAttacks

            //remove all attacks which target my object id
        
        }

        private bool CancelAttack(string attackingObjectID, string targetObjectID, string capabilityName)
        {
            //get attacker proxy
            SimulationObjectProxy attacker = objectProxies[attackingObjectID];

            //find engagement between attacker and target
            AttackCollectionValue attacks = attacker["CurrentAttacks"].GetDataValue() as AttackCollectionValue;
            List<AttackCollectionValue.AttackValue> attacksToRemove = new List<AttackCollectionValue.AttackValue>();

            foreach (AttackCollectionValue.AttackValue att in attacks.GetCurrentAttacksOnTarget(targetObjectID))
            {
                if (att.capabilityName == capabilityName && att.attackingObjectId == attackingObjectID && att.targetObjectId == targetObjectID)
                {
                    //cancel out Transition amount applied for this attack
                    RemoveIntensityFromTargetsVulnerability(targetObjectID, capabilityName, att.appliedIntensity);
                    attacksToRemove.Add(att);
                }
            }

            //for attacker, cancel the attack.
            foreach (AttackCollectionValue.AttackValue atv in attacksToRemove)
            {
                attacks.RemoveAttack(atv);
            }

            attacker["CurrentAttacks"].SetDataValue(attacks);

            return true;
        }

        private void RemoveIntensityFromTargetsVulnerability(String targetObjectID, String capabilityName, int intensityToReduceBy)
        {
            //get target proxy
            SimulationObjectProxy target = objectProxies[targetObjectID];

            //get vulnerabilities
            VulnerabilityValue vv = target["Vulnerability"].GetDataValue() as VulnerabilityValue;

            //remove effect from vulnerability
            foreach(VulnerabilityValue.Transition t in vv.transitions)
            {
                if (t.RemoveSingleEffect(capabilityName, intensityToReduceBy))
                {
                    bool removed = true;// send message instead?
                }
            }
            //save vulnerabilities
            target["Vulnerability"].SetDataValue(vv);
        }

        private void CancelCapabilityAttack(string capabilityName, string attackingObjectID)
        {
            SimulationObjectProxy attacker = objectProxies[attackingObjectID];
            if (attacker == null)
                return;

            List<AttackCollectionValue.AttackValue> attacksToRemove = new List<AttackCollectionValue.AttackValue>();
            AttackCollectionValue currentAttacks = attacker["CurrentAttacks"].GetDataValue() as AttackCollectionValue;

            foreach (AttackCollectionValue.AttackValue av in currentAttacks.GetCurrentAttacks())
            {
                if (av.capabilityName == capabilityName && attackingObjectID == av.attackingObjectId)
                {
                    attacksToRemove.Add(av);
                }
            }

            foreach (AttackCollectionValue.AttackValue av in attacksToRemove)
            {
                RemoveIntensityFromTargetsVulnerability(av.targetObjectId, av.capabilityName, av.appliedIntensity);
                currentAttacks.RemoveAttack(av);
            }

            attacker["CurrentAttacks"].SetDataValue(currentAttacks);
            attacksToRemove.Clear();
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
            if (atts.attributes.ContainsKey("ID")) 
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

            SimulationObjectProxy prox = null;// objectProxies[id];
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
        private void AttackObject(SimulationEvent e)
        {
            string attackerID = ((StringValue)e["ObjectID"]).value;
            string targetID = ((StringValue)e["TargetObjectID"]).value;
            string capabilityName = ((StringValue)e["CapabilityName"]).value;
            int percentageApplied = ((IntegerValue)e["PercentageApplied"]).value;

            if (percentageApplied == 0)
                percentageApplied = 100; //legacy checks.

            AttackObject(attackerID, targetID, capabilityName, percentageApplied, false);
        }

        private void AttackObject(string attackerID, string targetID, string capabilityName, int percentageApplied, bool isSelfDefense)
        {
            DataValue dv = null;


            //AD Note: on 9/23/09, we decided that Vulnerabilities would now have an "EngagementDuration", which is the Vulnerability
            // version of Capability's "AttackDuration".  If an EngagementDuration exists, it will override a Capability's AttackDuration.

            SimulationObjectProxy attackerProx = objectProxies[attackerID];
            SimulationObjectProxy targetProx = objectProxies[targetID];

            CapabilityValue attackerCap = (CapabilityValue)attackerProx["Capability"].GetDataValue();
            VulnerabilityValue targetVul = (VulnerabilityValue)targetProx["Vulnerability"].GetDataValue();

            // Check to see if this attack can start

            if (((StringValue)attackerProx["AttackState"].GetDataValue()).value == "BEING_ATTACKED")
            {
                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                       time,
                                                                       ((StringValue)(attackerProx["OwnerID"].GetDataValue())).value,
                                                                       attackerID + " is being engaged -- it can't initiate an engagement."));

                return;
            }
            if (targetVul.transitions.Count == 0)
            {
                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                       time,
                                                                       ((StringValue)(attackerProx["OwnerID"].GetDataValue())).value,
                                                                       attackerID + " can't engage " + targetID + " -- object has no vulnerabilities."));
                return;
            }

            //AD 11/16/2009: New Attack Logic: Allow for partial capabilities to be applied to attacks.

            AttackCollectionValue attackCollection = (AttackCollectionValue)attackerProx["CurrentAttacks"].GetDataValue();
            int attackDuration = -1;

            AttackCollectionValue.AttackValue newAttack = new AttackCollectionValue.AttackValue(time, attackDuration, targetID, attackerID, capabilityName, percentageApplied, isSelfDefense);
            string errMsg = String.Empty;

            attackDuration = ((IntegerValue)attackerProx["AttackDuration"].GetDataValue()).value;
            int defenseDuration = ((IntegerValue)targetProx["EngagementDuration"].GetDataValue()).value;
            int attackTimeWindow = attackDuration;
            bool attackSuccess = true;
            if (defenseDuration > 0)
            {
                attackTimeWindow = defenseDuration;
            }
            Console.WriteLine("AttackObject: Attack duration for " + attackerID + " is " + attackDuration.ToString());
            Console.WriteLine("AttackObject: Attack duration for attack is " + attackTimeWindow.ToString());
            newAttack.attackTimeWindow = attackTimeWindow;

            if (attackCollection.AddAttack(newAttack, out errMsg) == false)
            {
                string msg = "The attack between " + attackerID + " and " + targetID + " encountered the following problem: " + errMsg;
                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(attackerProx["OwnerID"].GetDataValue())).value,
                                                                   msg));
                attackSuccess = false;
            }
            else if (errMsg != String.Empty)
            {
                string msg = "The attack between " + attackerID + " and " + targetID + " encountered the following problem: " + errMsg;
                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(attackerProx["OwnerID"].GetDataValue())).value,
                                                                   msg));
            }
            else //success
            {
                dv = targetProx["AttackState"].GetDataValue();
                ((StringValue)dv).value = "BEING_ATTACKED";
                targetProx["AttackState"].SetDataValue(dv);

                dv = targetProx["AttackerList"].GetDataValue();
                ((StringListValue)dv).strings.Add(attackerID);
                targetProx["AttackerList"].SetDataValue(dv);
            }

            

            if (!attackSuccess)
            {
                attackerProx["CurrentAttacks"].SetDataValue(attackCollection); //set below if attackSuccess is true
                return;
            }


            //End new attack logic.


            //
            Vec3D attackerPosition = new Vec3D((LocationValue)attackerProx["Location"].GetDataValue());
            Vec3D targetPosition = new Vec3D((LocationValue)targetProx["Location"].GetDataValue());
            double distance = attackerPosition.ScalerDistanceTo(targetPosition); //FIX find actual diatance

            int appliedIntensity = -1;
            List<CapabilityValue.Effect> capabilities = attackerCap.GetOrderedEffectsByCapability(capabilityName);
            foreach (CapabilityValue.Effect eff in capabilities)
            {
                if (eff.name == capabilityName && distance <= eff.range)
                {
                    int r = random.Next(0, 100);
                    if (r <= ((int)(eff.probability * 100)))
                    {
                        appliedIntensity = Convert.ToInt32(Math.Round(((double)eff.intensity * (double)newAttack.percentageApplied / 100), MidpointRounding.AwayFromZero));
                        newAttack.appliedIntensity = appliedIntensity;
                        targetVul.ApplyEffect(eff.name, appliedIntensity, distance, ref random);
                        
                    }
                    //break outside of the if because if the probability failed, you dont want a second chance with a different range.
                    break; //break because we only want to apply the first range found that satisfied the distance restraint
                }
            }
            attackerProx["CurrentAttacks"].SetDataValue(attackCollection);
            targetProx["Vulnerability"].SetDataValue((DataValue)targetVul);


            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(attackerProx["OwnerID"].GetDataValue())).value,
                                                                   attackerID + " has engaged " + targetID));
            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(targetProx["OwnerID"].GetDataValue())).value,
                                                                   targetID + " has been engaged by " + attackerID));

            distClient.PutEvent(SimUtility.BuildHistory_AttackerObjectReportEvent(ref simModel,
                                                                                  time,
                                                                                  attackerID,
                                                                                  attackerPosition,
                                                                                  targetID,
                                                                                  targetPosition,
                                                                                  capabilityName,
                                                                                  appliedIntensity));


        }

        private void RandomSeed(SimulationEvent e)
        {
            randomSeed = ((IntegerValue)e["SeedValue"]).value;
            random = new Random(randomSeed);
        }
        private void TimeTick(SimulationEvent e)
        {
            time = ((IntegerValue)e["Time"]).value;

            DataValue dv = null;

            SimulationObjectProxy targetProx = null;
            Vec3D targetLoc = new Vec3D(0, 0, 0);
            SimulationObjectProxy obProx = null;

            bool selfDefenseStartAttack;
            string selfDefenseCapability;
            string selfDefenseTargetID;
            Dictionary<string, Dictionary<string, List<AttackCollectionValue.AttackValue>>> currentAttackCollection = new Dictionary<string,Dictionary<string,List<AttackCollectionValue.AttackValue>>>();
            //[ [TargetID]/[ CapabilityUsed]/[List of Attacks] ]
            Dictionary<string, List<AttackCollectionValue.AttackValue>> attacksToRemove = new Dictionary<string, List<AttackCollectionValue.AttackValue>>();
            // [AttackerID]/[List of attacks to remove]
        //as you clean up attacks, add them to this list.  once done iterating over targets, go through this list and update the attacks in the keys.
            foreach (string id in objectProxies.Keys)
            {
                obProx = objectProxies[id];
                //Generate Attack dictionary
                AttackCollectionValue attacks = (AttackCollectionValue)obProx["CurrentAttacks"].GetDataValue();

                foreach(AttackCollectionValue.AttackValue av in attacks.GetCurrentAttacks())
                {
                    if(!currentAttackCollection.ContainsKey(av.targetObjectId))
                    {
                        currentAttackCollection.Add(av.targetObjectId, new Dictionary<string,List<AttackCollectionValue.AttackValue>>());
                    }
                    if(!currentAttackCollection[av.targetObjectId].ContainsKey(av.capabilityName))
                    {
                        currentAttackCollection[av.targetObjectId].Add(av.capabilityName, new List<AttackCollectionValue.AttackValue>());
                    }
                    currentAttackCollection[av.targetObjectId][av.capabilityName].Add(av); //store pointer
                }

                selfDefenseStartAttack = ((BooleanValue)obProx["SelfDefenseStartAttack"].GetDataValue()).value;

                if (selfDefenseStartAttack)
                {
                    selfDefenseCapability = ((StringValue)obProx["SelfDefenseCapability"].GetDataValue()).value;
                    selfDefenseTargetID = ((StringValue)obProx["SelfDefenseTargetID"].GetDataValue()).value;

                    targetProx = objectProxies[selfDefenseTargetID];

                    if (((AttackCollectionValue)obProx["CurrentAttacks"].GetDataValue()).GetCurrentAttacks().Count == 0 &&
                        ((StringValue)obProx["State"].GetDataValue()).value != "Dead" &&
                        ((StringValue)targetProx["State"].GetDataValue()).value != "Dead")
                    {
                        AttackObject(id, selfDefenseTargetID, selfDefenseCapability, 100, true);
                        if (((StringValue)obProx["AttackState"].GetDataValue()).value == "")
                        {
                            SendSelfDefenseAttackStarted(id, selfDefenseTargetID);
                        }
                    }
                }
            }

            foreach (string targetID in objectProxies.Keys)
            {
                targetProx = objectProxies[targetID];
                string currentState = ((StringValue)objectProxies[targetID]["State"].GetDataValue()).value;
                dv = targetProx["AttackState"].GetDataValue();
                if (((StringValue)dv).value == "BEING_ATTACKED")
                {
                    if (!currentAttackCollection.ContainsKey(targetID))
                    {
                        currentAttackCollection.Add(targetID, new Dictionary<string, List<AttackCollectionValue.AttackValue>>());
                        //this should not happen, or we're in trouble
                    }
                    int capabilitiesCompleted = 0; //this gets incremented as you add to attacksToRemove
                    foreach (String capability in currentAttackCollection[targetID].Keys)
                    { 
                        //update attack windows for each attack object?
                        int attackEndTime = -1;
                        foreach (AttackCollectionValue.AttackValue av in currentAttackCollection[targetID][capability])
                        {
                            if (attackEndTime == -1)
                            {
                                attackEndTime = av.attackStartTime + av.attackTimeWindow;
                            }
                            else
                            { 
                                attackEndTime = Math.Min(attackEndTime, av.attackStartTime + av.attackTimeWindow);
                            }
                        }
                        int newDuration = attackEndTime - time;
                        foreach (AttackCollectionValue.AttackValue av in currentAttackCollection[targetID][capability])
                        {
                            av.attackTimeWindow = attackEndTime - av.attackStartTime;// newDuration;
                        }

                        //check attack window vs current time
                        if (time >= attackEndTime)
                        {

                            //cleanup if needed
                            
                            //add attacks to remove list
                            
                            foreach (AttackCollectionValue.AttackValue av in currentAttackCollection[targetID][capability])
                            {
                                if (!attacksToRemove.ContainsKey(av.attackingObjectId))
                                {
                                    attacksToRemove.Add(av.attackingObjectId, new List<AttackCollectionValue.AttackValue>());
                                }
                                attacksToRemove[av.attackingObjectId].Add(av);
                            }
                            
                            //check vulnerabilities
                            VulnerabilityValue targetVul = (VulnerabilityValue)targetProx["Vulnerability"].GetDataValue();
                            bool attackSuccess = false;
                            List<string> capabilitiesApplied;
                            List<string> attackers = new List<string>();
 
                            foreach (VulnerabilityValue.Transition t in targetVul.transitions)
                            {
                                foreach (String cap in t.GetAppliedCapabilities())
                                {
                                    if (!currentAttackCollection[targetID].ContainsKey(cap))
                                        continue; //workaround for issue at USF; for some reason capability was not added to current attack collection.
                                    foreach (AttackCollectionValue.AttackValue val in currentAttackCollection[targetID][cap])
                                    {
                                        string attackerID = val.attackingObjectId;
                                        if (!attackers.Contains(attackerID))
                                        {
                                            attackers.Add(attackerID);
                                        }
                                    }
                                }
                                if (t.ConditionsMet())
                                {
                                    
                                    capabilitiesApplied = t.GetAppliedCapabilities();
                                    // Send state change
                                    string newState = t.state;
                                    SimulationEvent sc = SimulationEventFactory.BuildEvent(ref simModel, "StateChange");
                                    ((StringValue)sc["ObjectID"]).value = targetID;
                                    ((StringValue)sc["NewState"]).value = newState;
                                    ((IntegerValue)sc["Time"]).value = time;
                                    distClient.PutEvent(sc);
                                    foreach (string attackerID in attackers)
                                    {
                                        distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                                               time,
                                                                                               ((StringValue)(objectProxies[attackerID]["OwnerID"].GetDataValue())).value,
                                                                                               attackerID + " has succesfully engaged " + targetID));
                                        distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                                               time,
                                                                                               ((StringValue)(targetProx["OwnerID"].GetDataValue())).value,
                                                                                               targetID + " has been succesfully engaged by " + attackerID));

                                        ScoringDB.UpdateScore_StateChange(new ScoringDB.ActorFrame(attackerID,
                                                                                                   StateDB.physicalObjects[attackerID].speciesName,
                                                                                                   StateDB.physicalObjects[attackerID].ownerID,
                                                                                                   StateDB.physicalObjects[attackerID].activeRegions),
                                                                                                   currentState,
                                                                                                   t.state,
                                                                                                   new ScoringDB.ActorFrame(targetID,
                                                                                                   StateDB.physicalObjects[targetID].speciesName,
                                                                                                   StateDB.physicalObjects[targetID].ownerID,
                                                                                                   StateDB.physicalObjects[targetID].activeRegions));
                                    }
                                    
                                    
                                    t.ClearAppliedEffects();

                                    distClient.PutEvent(SimUtility.BuildHistory_AttackedObjectReportEvent(ref simModel,
                                                                                                          time,
                                                                                                          targetID,
                                                                                                          targetLoc,
                                                                                                          true,
                                                                                                          t.state));
                                    distClient.PutEvent(SimUtility.BuildAttackSucceededEvent(ref simModel, time, attackers[0], targetID, newState, capabilitiesApplied));
                                    attackSuccess = true;
                                    break;
                                }

                            }
                            //send messages
                            if (!attackSuccess)
                            {
                                foreach (String attackerID in attackers)
                                {
                                    distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                                           time,
                                                                                           ((StringValue)(objectProxies[attackerID]["OwnerID"].GetDataValue())).value,
                                                                                           attackerID + " has failed in engagement of " + targetID));
                                    distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                                           time,
                                                                                           ((StringValue)(targetProx["OwnerID"].GetDataValue())).value,
                                                                                           targetID + " has been unsuccesfully engaged by " + attackerID));
                                }
                                foreach (VulnerabilityValue.Transition t in targetVul.transitions)
                                {
                                    t.ClearAppliedEffects();
                                }

                                distClient.PutEvent(SimUtility.BuildHistory_AttackedObjectReportEvent(ref simModel,
                                                                                                      time,
                                                                                                      targetID,
                                                                                                      targetLoc,
                                                                                                      false,
                                                                                                      ""));
                            }

                            capabilitiesCompleted++;
                            //if there are more capabilities being applied than this one, don't remove target's attack state.
                            if (currentAttackCollection[targetID].Count - capabilitiesCompleted == 0)
                            {// this occurs when all attacks will be removed in this loop
                                dv = targetProx["AttackState"].GetDataValue();
                                ((StringValue)dv).value = "";
                                targetProx["AttackState"].SetDataValue(dv);
                            }
                        }
                    }

                    foreach (string attackerID in attacksToRemove.Keys)
                    {
                        SimulationObjectProxy attackerProxy = objectProxies[attackerID];
                        if (attackerProxy != null)
                        {
                            AttackCollectionValue acv = attackerProxy["CurrentAttacks"].GetDataValue() as AttackCollectionValue;
                            foreach (AttackCollectionValue.AttackValue attackVal in attacksToRemove[attackerID])
                            {
                                if (!acv.RemoveAttack(attackVal))
                                {
                                    acv.RemoveAttack(attackVal.capabilityName, attackVal.targetObjectId, attackVal.attackingObjectId, attackVal.attackStartTime);
                                }
                            }

                            attackerProxy["CurrentAttacks"].SetDataValue(acv);
                            if (((BooleanValue)attackerProxy["IsWeapon"].GetDataValue()).value)
                            {
                                distClient.PutEvent(SimUtility.BuildStateChangeEvent(ref simModel, time, attackerID, "Dead"));
                            }
                        }
                    }

                    if (capabilitiesCompleted > 0)
                    {//some attacks were removed, actually remove them from the currentAttackCollection, update attacker list. 
                    //update attack lists (this will require some iteration over the attacks.
                        List<string> attackers = null;
                        dv = targetProx["AttackerList"].GetDataValue();
                        attackers = ((StringListValue)dv).strings;
                        attackers.Clear();

                        foreach (String capability in currentAttackCollection[targetID].Keys)
                        {
                            foreach (AttackCollectionValue.AttackValue av in currentAttackCollection[targetID][capability])
                            {
                                if (!attackers.Contains(av.attackingObjectId))
                                {
                                    attackers.Add(av.attackingObjectId);
                                }
                            }
                        }

                        ((StringListValue)dv).strings = attackers;
                        targetProx["AttackerList"].SetDataValue(dv);
                    }
                }
            }
        }
        private void SendSelfDefenseAttackStarted(string attacker, string target)
        {
            SimulationEvent send = SimulationEventFactory.BuildEvent(ref simModel, "SelfDefenseAttackStarted");

            send["AttackerObjectID"] = DataValueFactory.BuildString(attacker);
            send["TargetObjectID"] = DataValueFactory.BuildString(target);
            send["Time"] = DataValueFactory.BuildInteger(time);

            distClient.PutEvent(send);
        }

        private void StateChange(SimulationEvent e)
        {
            ChangeObjectState(((StringValue)e["ObjectID"]).value, ((StringValue)e["NewState"]).value);
        }
        private void ChangeObjectState(string id, string newState)
        {
            SimulationObjectProxy prox = null; // objectProxies[id];
            prox = GetObjectProxy(id);
            if (prox == null)
                return;

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
            return "AttackProcessor";
        }
    }
}
