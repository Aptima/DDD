using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.ObjectsAttributeCollection;

//This partial implementation of the ViewPro handles all the "Helper Methods".
//A Helper Event is called from within the ViewPro and some basic functionality
//that is shared across any of the ViewPro's methods.  This includes methods
//that combine attribute collections, and methods that strip out nested data values
namespace Aptima.Asim.DDD.CalamityvilleSimulators.ViewPro
{
    public partial class ViewProSim
    {
        private class Attack
        {
            public string attacker;
            public string target;
            public string capabilityName;
            public int startTime;
            public int attackLength;
            public int endTime;

            public Attack(string attacker, string target, string capabilityName)
            {
                this.attacker = attacker;
                this.target = target;
                this.capabilityName = capabilityName;
            }
            public void SetTimes(int startTime, int length)
            {
                this.startTime = startTime;
                this.attackLength = length;
                this.endTime = startTime + length;
            }
        }

        #region Attribute Collection methods
        /// <summary>
        /// Given a detected value and an attribute collection, the detected value will be added to the collection only
        /// if the attribute is already not-existant within the collection, or if its confidence is higher.
        /// </summary>
        /// <param name="ACV"></param>
        /// <param name="attributeName"></param>
        /// <param name="dav"></param>
        private void AddAttributeToACV(ref AttributeCollectionValue ACV, string attributeName, DetectedAttributeValue dav)
        {
            if (ACV.attributes.ContainsKey(attributeName))
            {
                if (((DetectedAttributeValue)dav).stdDev > ((DetectedAttributeValue)ACV[attributeName]).stdDev)
                {
                    ACV[attributeName] = dav;
                }
            }
            else
            {
                ACV.attributes.Add(attributeName, dav);
            }
        }
 
        /// <summary>
        /// Given two attribute collections, they will merge into one based on confidence values.
        /// </summary>
        /// <param name="startingACV"></param>
        /// <param name="incomingACV"></param>
        private void MergeTwoAttributeCollections(ref AttributeCollectionValue startingACV, AttributeCollectionValue incomingACV)
        {
            foreach (string attName in incomingACV.attributes.Keys)
            {
                if (startingACV.attributes.ContainsKey(attName))
                {
                    if (((DetectedAttributeValue)startingACV[attName]).stdDev < ((DetectedAttributeValue)incomingACV[attName]).stdDev)
                    {
                        startingACV[attName] = incomingACV[attName];
                    }
                }
                else
                { //add attribute
                    startingACV.attributes.Add(attName, incomingACV[attName]);
                }
            }
        }
 
        /// <summary>
        /// Given an attribute collection containing DetectedValues, this method will strip the 
        /// detected value wrapper from the data values, and return the nested DV's.  This will
        /// NOT return any non-DetectedValue entries.
        /// </summary>
        /// <param name="ACV"></param>
        /// <returns></returns>
        private AttributeCollectionValue ExtractDetectedValuesFromACV(AttributeCollectionValue ACV)
        {
            AttributeCollectionValue returnACV = new AttributeCollectionValue();
            foreach (KeyValuePair<string, DataValue> kvp in ACV.attributes)
            {
                if (kvp.Value is DetectedAttributeValue)
                {
                    returnACV.attributes.Add(kvp.Key, ((DetectedAttributeValue)kvp.Value).value);
                }
                else if (!(kvp.Value is DetectedAttributeValue))
                {
                    returnACV.attributes.Add(kvp.Key, kvp.Value);
                }
            }
            return returnACV;
        }

        #endregion

        #region View Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dmID"></param>
        /// <param name="incoming"></param>
        private void UpdateDMsViews(string dmID, ref Dictionary<string, AttributeCollectionValue> incoming)
        {
            foreach (string obj in incoming.Keys)
            {
                AttributeCollectionValue acv = incoming[obj];
                dmViews[dmID].UpdateObjectsAttributes(ref acv, obj);
            }
        }
 
        private void ClearObjectFromViews(string objectID)
        {
            //continue processing the state change.
            List<string> dmsThatSeeTheDeadObject = new List<string>();
            foreach (string dm in dmViews.Keys)
            {
                if (dmViews[dm].GetObjectKeys().Contains(objectID))
                {
                    dmsThatSeeTheDeadObject.Add(dm);
                }
            }

            foreach (string dmID in dmsThatSeeTheDeadObject)
            {
                SendRemoveObjectEvent(objectID, dmID);
                dmViews[dmID].RemoveObject(objectID);
            }
                foreach (string ob in objectViews.Keys)
                {
                    //if (objectViews[ob].ContainsObject(objectID))
                    //{
                        objectViews[ob].RemoveObject(objectID);
                    //}
                }
            
            foreach (string dmName in dmOwnedObjects.Keys)
            {
                if (dmOwnedObjects[dmName].Contains(objectID))
                {
                    dmOwnedObjects[dmName].Remove(objectID);
                }
            }
            if (objectViews.ContainsKey(objectID))
            {
                objectViews.Remove(objectID);
            }
            foreach (string network in networkObjects.Keys)
            {
                if (networkObjects[network].Contains(objectID))
                {
                    networkObjects[network].Remove(objectID);
                }
            }
            //List<int> attacksToRemove = new List<int>(); //Contains the indices of attacks to remove
            //int count = 0;
            Attack newAtt = new Attack("", "","");
            foreach (Attack att in currentAttacks)
            {
                if (att.attacker == objectID)
                { //attacker is dead, set attacker to target to keep attack counter on screen, but
                    //only on the target, as opposed to from nowhere to the target.
                    newAtt.attacker = att.target;
                    newAtt.target = att.target;
                    newAtt.capabilityName = att.capabilityName;
                    newAtt.endTime = Convert.ToInt32(att.endTime);
                    att.endTime = currentTick;
                }
                if (att.target == objectID)
                {//target died, set attack end time to now
                    att.endTime = currentTick;
                }
                //count++;
            }
            if (newAtt.target != "")
            {
                currentAttacks.Add(newAtt);
            }
            //attacksToRemove.Reverse();
            //foreach (int index in attacksToRemove)
            //{
            //    currentAttacks.RemoveAt(index);
            //}
            if (movingObjects.Contains(objectID))
            {
                movingObjects.Remove(objectID);
            }
        }
 
        private void CompareNewDMViewWithPrevious(string dm, Dictionary<string, AttributeCollectionValue> singleDMView, ref List<string> recentDiscoveries)
        {
            if (!dmViews.ContainsKey(dm))
                return;
            ObjectsAttributeCollection prevDMView = dmViews[dm];
            List<string> objectsToRemove = new List<string>();
            List<string> objectsToAdd = new List<string>();
            List<string> previouslyNotVisibleObjects = new List<string>();
            foreach (string obj in prevDMView.GetObjectKeys())
            {
                if (!singleDMView.ContainsKey(obj))
                    objectsToRemove.Add(obj);
                if (prevDMView[obj].attributes.ContainsKey("Location"))
                {
                    if (!((LocationValue)((DetectedAttributeValue)prevDMView[obj]["Location"]).value).exists)
                    {
                        previouslyNotVisibleObjects.Add(obj);
                    }
                }
            }
            foreach (string obj in singleDMView.Keys)
            {
                if (!prevDMView.ContainsObject(obj) && singleDMView[obj].attributes.ContainsKey("Location"))
                    objectsToAdd.Add(obj);
            }
            AttributeCollectionValue acv = new AttributeCollectionValue();
            //acv.attributes.Add("ID", new StringValue() as DataValue);
            foreach (string obj in objectsToAdd.ToArray())
            {
                //
                acv = singleDMView[obj]; //singleDMView stores DetectedValues, which need to be
                //stripped before sending to SendVPIO
                if (acv.attributes.ContainsKey("Location"))
                {
                    if (!((LocationValue)((DetectedAttributeValue)acv["Location"]).value).exists)
                    {
                        objectsToRemove.Add(obj);
                        objectsToAdd.Remove(obj);
                        continue;
                    }
                    else
                    {
                        //if (!previouslyNotVisibleObjects.Contains(obj))
                        //    continue;
                        //objectsToAdd.Add(obj);
                        //objectsToRemove.Remove(obj);
                    }
                }
                if (!acv.attributes.ContainsKey("ID"))
                {
                    acv["ID"] = DataValueFactory.BuildDetectedValue(DataValueFactory.BuildString(obj), 100);
                }
                if (!acv.attributes.ContainsKey("OwnerID"))
                {
                    acv["OwnerID"] = DataValueFactory.BuildDetectedValue(objectProxies[obj]["OwnerID"].GetDataValue(), 100);
                }
                SendViewProInitializeObject(dm, ExtractDetectedValuesFromACV(acv));
            }

            foreach (string obj in objectsToRemove)
            {
                SendRemoveObjectEvent(obj, dm);
                prevDMView.RemoveObject(obj);
            }

            bool anyAttributeChanged = false;
            acv = null;

            foreach (string obj in objectsToAdd)
            {
                //AttributeCollectionValue atts = new AttributeCollectionValue(); //prevDMView.UpdateObject(obj, singleDMView[obj]);
                //SendViewProAttributeUpdate(dm, atts);
                prevDMView.UpdateObject(obj, new AttributeCollectionValue());
                if (!movingObjects.Contains(obj))
                    continue;
                if (!activeDMs.Contains(dm))
                    continue;
                if (!recentDiscoveries.Contains(obj))
                {
                    recentDiscoveries.Add(obj);
                }
                //atts["ObjectID"] = DataValueFactory.BuildString(obj);//this is needed as the
                //                                                     //above Update call will
                //                                                     //add "ID", not "ObjectID"
                //SendViewProMotionUpdate(atts);
            }
            foreach (string obj in prevDMView.GetObjectKeys())
            {
                acv = prevDMView.UpdateObject(obj, singleDMView[obj]);
                //add recent tags
                if (recentUnitTags != null)
                {
                    if (recentUnitTags.ContainsKey(obj))
                    {
                        if (recentUnitTags[obj].ContainsKey(dm))
                        {
                            if (acv == null)
                            {
                                acv = new AttributeCollectionValue();
                            }
                            acv.attributes["InitialTag"] = DataValueFactory.BuildString(recentUnitTags[obj][dm]);
                            recentUnitTags[obj].Remove(dm); //remove once the DM has been notified.
                            acv.attributes["ID"] = DataValueFactory.BuildString(obj);
                            acv.attributes["OwnerID"] = prevDMView[obj]["OwnerID"];

                        }
                    }
                }

                String classification = GetChangedClassificationForDM(obj, dm);
                if (classification != null)
                {
                    if (acv == null)
                    {
                        acv = new AttributeCollectionValue();
                        acv.attributes["ID"] = DataValueFactory.BuildString(obj);
                        acv.attributes["OwnerID"] = prevDMView[obj]["OwnerID"];
                    }
                    acv.attributes["CurrentClassification"] = DataValueFactory.BuildString(classification);
                }

                if (acv == null)
                    continue;
                if (acv.attributes.ContainsKey("Location"))
                {
                    if (((LocationValue)((DetectedAttributeValue)acv["Location"]).value).exists)
                    {
                        //SendViewProInitializeObject(dm, ExtractDetectedValuesFromACV(prevDMView[obj]));
                    }
                }

                SendViewProAttributeUpdate(dm, acv);

                //Console.WriteLine(String.Format("New Location for {0}: {1},{2}", obj, ((LocationValue)((DetectedAttributeValue)acv.attributes["Location"]).value).X, ((LocationValue)((DetectedAttributeValue)acv.attributes["Location"]).value).Y));
                if (acv.attributes.ContainsKey("OwnerID") && acv.attributes.ContainsKey("CurrentAttacks"))
                {
                    if (((StringValue)((DetectedAttributeValue)acv.attributes["OwnerID"]).value).value == dm)
                    {
                        AttackCollectionValue attacks = (AttackCollectionValue)((DetectedAttributeValue)acv.attributes["CurrentAttacks"]).value;
                        foreach (AttackCollectionValue.AttackValue av in attacks.GetCurrentAttacks())
                        {
                            foreach (Attack a in currentAttacks)
                            {
                                if (a.attacker == av.attackingObjectId && a.target == av.targetObjectId && a.capabilityName == av.capabilityName)
                                {
                                    a.SetTimes(av.attackStartTime, av.attackTimeWindow);
                                }
                            }
                        }
                    }
                }
                if (acv.attributes.ContainsKey("DestinationLocation") ||
                    acv.attributes.ContainsKey("Throttle"))
                {
                    if (!recentDiscoveries.Contains(obj))
                    {
                        recentDiscoveries.Add(obj);
                    }
                    //acv["ObjectID"] = acv["ID"];
                    //SendViewProMotionUpdate(ExtractDetectedValuesFromACV(acv));
                }
                acv = null;
            }
        }

        /// <summary>
        /// This method will take the RESULT of an ObjectAttributeCollection.UpdateObject call, which only contains attributes
        /// which have CHANGED, and will determine if either the sensors, vulnerabilties, or capabilities (including docked weapons) has
        /// changed.  If so, then new attributes will be added to the attribute collection value.
        /// </summary>
        /// <param name="acv"></param>
        /// <param name="fullObjectView"></param>
        /// <returns></returns>
        private void CalculateRangeRings(ref AttributeCollectionValue acv, ref SimulationObjectProxy fullObjectView)
        {
            if (acv.attributes.ContainsKey("Sensors"))
            {
                //detected contains a sensor array type
                CustomAttributesValue sensorCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue sensorRing;
                SensorArrayValue detectedSensors = ((DetectedAttributeValue)acv["Sensors"]).value as SensorArrayValue;

                foreach (SensorValue sv in detectedSensors.sensors)
                { 
                    sensorRing = DataValueFactory.BuildRangeRingDisplayValue(sv.sensorName, "Sensors", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                    sensorRing.rangeIntensities.Add(Convert.ToInt32(sv.maxRange), -1);

                    sensorCollection.attributes.Add(sv.sensorName, sensorRing);
                }

                if (sensorCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("SensorRangeRings", sensorCollection);
                }
                else
                {
                    Console.WriteLine("No SensorRangeRings added to ACV");
                }
            }
            if (acv.attributes.ContainsKey("Vulnerability"))
            {
                //gets detected values, containing a vulnerability type
                CustomAttributesValue vulnerabilityCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue vulnerabilityRing;
                VulnerabilityValue detectedVulnerability = ((DetectedAttributeValue)acv["Vulnerability"]).value as VulnerabilityValue;
                Dictionary<string, int> longestRange = new Dictionary<string,int>();//[Capability],[Range]

                foreach (VulnerabilityValue.Transition tr in detectedVulnerability.transitions)
                {
                    foreach(VulnerabilityValue.TransitionCondition tc in tr.conditions)
                    {
                        if(!longestRange.ContainsKey(tc.capability))
                        {
                            longestRange.Add(tc.capability, -1);
                        }
                        if (longestRange[tc.capability] < Convert.ToInt32(tc.range))
                        {
                            longestRange[tc.capability] = Convert.ToInt32(tc.range);
                        }
                    }
                }

                foreach (KeyValuePair<string, int> kvp in longestRange)
                { 
                    vulnerabilityRing = DataValueFactory.BuildRangeRingDisplayValue(kvp.Key, "Vulnerability", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                    vulnerabilityRing.rangeIntensities.Add(kvp.Value, -1);

                    vulnerabilityCollection.attributes.Add(kvp.Key, vulnerabilityRing);
                }                
                

                if (vulnerabilityCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("VulnerabilityRangeRings", vulnerabilityCollection);
                }
                else
                {
                    Console.WriteLine("No VulnerabilityRangeRings added to ACV");
                }
            }
            if (acv.attributes.ContainsKey("Capability") || acv.attributes.ContainsKey("DockedWeapons"))
            {
                CustomAttributesValue capabilityCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue capabilityRing;

                Dictionary<string, int> longestWeaponRange = new Dictionary<string, int>();//[Capability],[Range]


                //docked weapons gets string list of IDs
                if (acv.attributes.ContainsKey("DockedWeapons"))
                { 
                   StringListValue dockedWeapons = ((DetectedAttributeValue)acv["DockedWeapons"]).value as StringListValue;
                   foreach (String weaponID in dockedWeapons.strings)
                   {
                       //get weapon id
                       //get proxy info for weapon
                       SimulationObjectProxy weapon = objectProxies[weaponID];
                       string species = ((StringValue)weapon["ClassName"].GetDataValue()).value;
                       if (longestWeaponRange.ContainsKey(species))
                       {
                           continue;
                           //For now, assume that all weapons of the same species type have the same ranges.
                           //this will cut back on unneccessary loops, and for the most part is 100% true.
                       }

//get max speed, maxfuel or current fuel, get fuel consumption rate, get SHORTEST capability range
                       double maxSpeed = ((DoubleValue)weapon["MaximumSpeed"].GetDataValue()).value;
                       double maxFuel = ((DoubleValue)weapon["FuelCapacity"].GetDataValue()).value;
                       double fuelConsumptionRate = ((DoubleValue)weapon["FuelConsumptionRate"].GetDataValue()).value;
                       double shortCapabilityRange = -1;

                       CapabilityValue weaponCV = (CapabilityValue)weapon["Capability"].GetDataValue();
                       Dictionary<string, double> capRanges = new Dictionary<string, double>();
                       
                       foreach (CapabilityValue.Effect ef in weaponCV.effects)
                       {
                           if (!capRanges.ContainsKey(ef.name))
                           {
                               capRanges.Add(ef.name, ef.range);
                           }
                           else
                           {
                               if (capRanges[ef.name] > ef.range)
                               {//You want the smaller range here because that's how weapons work.  Auto-attacks use the SHORTEST range to trigger.
                                   capRanges[ef.name] = ef.range;
                               }
                           }
                       }
                       //but here, you want the LONGEST of the ranges that could trigger an auto-attack.
                       foreach (KeyValuePair<string, double> kvp in capRanges)
                       {
                           if (kvp.Value > shortCapabilityRange)
                           {
                               shortCapabilityRange = kvp.Value;
                           }
                       }

                       double thisRange = maxSpeed * maxFuel * fuelConsumptionRate + shortCapabilityRange;
                       longestWeaponRange.Add(species, Convert.ToInt32(thisRange));
                   }
                    //
                }

                Dictionary<string, Dictionary<double, double>> capabilityRanges = new Dictionary<string, Dictionary<double, double>>();
                if (acv.attributes.ContainsKey("Capability"))
                { 
                    CapabilityValue detectedCapability = ((DetectedAttributeValue)acv["Capability"]).value as CapabilityValue;
                    

                    foreach (CapabilityValue.Effect ef in detectedCapability.effects)
                    {
                        if (!capabilityRanges.ContainsKey(ef.name))
                        {
                            capabilityRanges.Add(ef.name, new Dictionary<double, double>());
                        }
                       
                        capabilityRanges[ef.name].Add(ef.range, ef.intensity);
                        
                    }
                }

                //add all capabilities to ring collection
                foreach (string capName in capabilityRanges.Keys)
                {
                    if (!capabilityCollection.attributes.ContainsKey(capName))
                    {
                        capabilityRing = DataValueFactory.BuildRangeRingDisplayValue(capName, "Capability", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                        Dictionary<int, int> convertedRanges = new Dictionary<int, int>();
                        foreach (KeyValuePair<double, double> kvp in capabilityRanges[capName])
                        {
                            convertedRanges.Add(Convert.ToInt32(kvp.Key), Convert.ToInt32(kvp.Value));
                        }
                        capabilityRing.AddAndSortRanges(convertedRanges); //this sorts as well

                        capabilityCollection.attributes.Add(capName, capabilityRing);
                    }
                    else
                    {
                        Console.WriteLine("Failed to add duplicate capability to collection, {0}", capName);
                    }
                }

                foreach (string weaponSpeciesName in longestWeaponRange.Keys)
                {
                    if (!capabilityCollection.attributes.ContainsKey(weaponSpeciesName))
                    {
                        capabilityRing = DataValueFactory.BuildRangeRingDisplayValue(weaponSpeciesName, "Capability", true, new Dictionary<int, int>()) as RangeRingDisplayValue;
                        capabilityRing.rangeIntensities.Add(longestWeaponRange[weaponSpeciesName], -1); //TODO: Maybe add intensity above?
                        capabilityCollection.attributes.Add(weaponSpeciesName, capabilityRing);
                    }
                    else
                    {
                        Console.WriteLine("Failed to add duplicate capability(Weapon species) to collection, {0}", weaponSpeciesName);
                    }
                }

                if (capabilityCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("CapabilityRangeRings", capabilityCollection);
                }
                else
                {
                    Console.WriteLine("No CapabilityRangeRings added to ACV");
                }
            }

        }

        /// <summary>
        /// Similar to CalculateRangeRings, this is done with ABSOLUTE data, not detected values.  
        /// This method is called in a ViewProAttributeUpdate call, which only contains attributes
        /// which have CHANGED, and will determine if either the sensors, vulnerabilties, or capabilities (including docked weapons) has
        /// changed.  If so, then new attributes will be added to the attribute collection value.
        /// </summary>
        /// <param name="acv"></param>
        /// <param name="fullObjectView"></param>
        /// <returns></returns>
        private void AddRangeRings(ref AttributeCollectionValue acv, ref SimulationObjectProxy fullObjectView)
        {
            if (acv.attributes.ContainsKey("Sensors"))
            {
                //detected contains a sensor array type
                CustomAttributesValue sensorCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue sensorRing;
                SensorArrayValue detectedSensors = acv["Sensors"] as SensorArrayValue;

                foreach (SensorValue sv in detectedSensors.sensors)
                { 
                    sensorRing = DataValueFactory.BuildRangeRingDisplayValue(sv.sensorName, "Sensors", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                    sensorRing.rangeIntensities.Add(Convert.ToInt32(sv.maxRange), -1);

                    sensorCollection.attributes.Add(sv.sensorName, sensorRing);
                }

                if (sensorCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("SensorRangeRings", sensorCollection);
                }
                else
                {
                    Console.WriteLine("No SensorRangeRings added to ACV");
                }
            }
            if (acv.attributes.ContainsKey("Vulnerability"))
            {
                //gets detected values, containing a vulnerability type
                CustomAttributesValue vulnerabilityCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue vulnerabilityRing;
                VulnerabilityValue detectedVulnerability = acv["Vulnerability"] as VulnerabilityValue;
                Dictionary<string, int> longestRange = new Dictionary<string,int>();//[Capability],[Range]

                foreach (VulnerabilityValue.Transition tr in detectedVulnerability.transitions)
                {
                    foreach(VulnerabilityValue.TransitionCondition tc in tr.conditions)
                    {
                        if(!longestRange.ContainsKey(tc.capability))
                        {
                            longestRange.Add(tc.capability, -1);
                        }
                        if (longestRange[tc.capability] < Convert.ToInt32(tc.range))
                        {
                            longestRange[tc.capability] = Convert.ToInt32(tc.range);
                        }
                    }
                }

                foreach (KeyValuePair<string, int> kvp in longestRange)
                { 
                    vulnerabilityRing = DataValueFactory.BuildRangeRingDisplayValue(kvp.Key, "Vulnerability", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                    vulnerabilityRing.rangeIntensities.Add(kvp.Value, -1);

                    vulnerabilityCollection.attributes.Add(kvp.Key, vulnerabilityRing);
                }                
                

                if (vulnerabilityCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("VulnerabilityRangeRings", vulnerabilityCollection);
                }
                else
                {
                    Console.WriteLine("No VulnerabilityRangeRings added to ACV");
                }
            }
            if (acv.attributes.ContainsKey("Capability") || acv.attributes.ContainsKey("DockedWeapons"))
            {
                CustomAttributesValue capabilityCollection = DataValueFactory.BuildCustomAttributes(new Dictionary<string, DataValue>()) as CustomAttributesValue;
                RangeRingDisplayValue capabilityRing;

                Dictionary<string, int> longestWeaponRange = new Dictionary<string, int>();//[Capability],[Range]


                //docked weapons gets string list of IDs
                if (acv.attributes.ContainsKey("DockedWeapons"))
                { 
                   StringListValue dockedWeapons = acv["DockedWeapons"] as StringListValue;
                   foreach (String weaponID in dockedWeapons.strings)
                   {
                       //get weapon id
                       //get proxy info for weapon
                       SimulationObjectProxy weapon = objectProxies[weaponID];
                       string species = ((StringValue)weapon["ClassName"].GetDataValue()).value;
                       if (longestWeaponRange.ContainsKey(species))
                       {
                           continue;
                           //For now, assume that all weapons of the same species type have the same ranges.
                           //this will cut back on unneccessary loops, and for the most part is 100% true.
                       }

//get max speed, maxfuel or current fuel, get fuel consumption rate, get SHORTEST capability range
                       double maxSpeed = ((DoubleValue)weapon["MaximumSpeed"].GetDataValue()).value;
                       double maxFuel = ((DoubleValue)weapon["FuelCapacity"].GetDataValue()).value;
                       double fuelConsumptionRate = ((DoubleValue)weapon["FuelConsumptionRate"].GetDataValue()).value;
                       double shortCapabilityRange = -1;

                       CapabilityValue weaponCV = (CapabilityValue)weapon["Capability"].GetDataValue();
                       Dictionary<string, double> capRanges = new Dictionary<string, double>();
                       
                       foreach (CapabilityValue.Effect ef in weaponCV.effects)
                       {
                           if (!capRanges.ContainsKey(ef.name))
                           {
                               capRanges.Add(ef.name, ef.range);
                           }
                           else
                           {
                               if (capRanges[ef.name] > ef.range)
                               {//You want the smaller range here because that's how weapons work.  Auto-attacks use the SHORTEST range to trigger.
                                   capRanges[ef.name] = ef.range;
                               }
                           }
                       }
                       //but here, you want the LONGEST of the ranges that could trigger an auto-attack.
                       foreach (KeyValuePair<string, double> kvp in capRanges)
                       {
                           if (kvp.Value > shortCapabilityRange)
                           {
                               shortCapabilityRange = kvp.Value;
                           }
                       }

                       double thisRange = maxSpeed * maxFuel * fuelConsumptionRate + shortCapabilityRange;
                       longestWeaponRange.Add(species, Convert.ToInt32(thisRange));
                   }
                    //
                }

                Dictionary<string, Dictionary<double, double>> capabilityRanges = new Dictionary<string, Dictionary<double, double>>();
                if (acv.attributes.ContainsKey("Capability"))
                { 
                    CapabilityValue detectedCapability = acv["Capability"] as CapabilityValue;
                    

                    foreach (CapabilityValue.Effect ef in detectedCapability.effects)
                    {
                        if (!capabilityRanges.ContainsKey(ef.name))
                        {
                            capabilityRanges.Add(ef.name, new Dictionary<double, double>());
                        }
                       
                        capabilityRanges[ef.name].Add(ef.range, ef.intensity);
                        
                    }
                }

                //add all capabilities to ring collection
                foreach (string capName in capabilityRanges.Keys)
                {
                    if (!capabilityCollection.attributes.ContainsKey(capName))
                    {
                        capabilityRing = DataValueFactory.BuildRangeRingDisplayValue(capName, "Capability", false, new Dictionary<int, int>()) as RangeRingDisplayValue;
                        Dictionary<int, int> convertedRanges = new Dictionary<int, int>();
                        foreach (KeyValuePair<double, double> kvp in capabilityRanges[capName])
                        {
                            convertedRanges.Add(Convert.ToInt32(kvp.Key), Convert.ToInt32(kvp.Value));
                        }
                        capabilityRing.AddAndSortRanges(convertedRanges); //this sorts as well

                        capabilityCollection.attributes.Add(capName, capabilityRing);
                    }
                    else
                    {
                        Console.WriteLine("Failed to add duplicate capability to collection, {0}", capName);
                    }
                }

                foreach (string weaponSpeciesName in longestWeaponRange.Keys)
                {
                    if (!capabilityCollection.attributes.ContainsKey(weaponSpeciesName))
                    {
                        capabilityRing = DataValueFactory.BuildRangeRingDisplayValue(weaponSpeciesName, "Capability", true, new Dictionary<int, int>()) as RangeRingDisplayValue;
                        capabilityRing.rangeIntensities.Add(longestWeaponRange[weaponSpeciesName], -1); //TODO: Maybe add intensity above?
                        capabilityCollection.attributes.Add(weaponSpeciesName, capabilityRing);
                    }
                    else
                    {
                        Console.WriteLine("Failed to add duplicate capability(Weapon species) to collection, {0}", weaponSpeciesName);
                    }
                }

                if (capabilityCollection.attributes.Count > 0)
                {
                    acv.attributes.Add("CapabilityRangeRings", capabilityCollection);
                }
                else
                {
                    Console.WriteLine("No CapabilityRangeRings added to ACV");
                }
            }

        }

        #endregion

        #region Misc Helper methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool DoesAttackListContainThisPair(string attacker, string target)
        {
            foreach (Attack at in currentAttacks)
            {
                if (at.attacker != attacker)
                    continue;
                if (at.target != target)
                    continue;
                return true;
            }
            return false;
        }
 
        private List<SimulationObjectProxy> FindObstructions(Vec3D sensorPoint, Vec3D emitterPoint)
        {
            List<SimulationObjectProxy> obstructionList = new List<SimulationObjectProxy>();
            //SimulationObjectProxy blocker;
            foreach (KeyValuePair<string, StateDB.ActiveRegion> br in obstructions)
            {
                if (Polygon3D.SensorDoesLineCross(br.Value.poly, sensorPoint, emitterPoint))
                {
                    obstructionList.Add(objectProxies[br.Key]);
                }
            }

            return obstructionList;
        }
 
        /// <summary>
        /// This method takes in a capabilities list, and a docked weapons list, and quantifies the weapons and 
        /// combines those results with the capabilities list for a returned "CapabilitiesList", which the
        /// client displays to its user.
        /// </summary>
        /// <param name="atts"></param>
        private void AddCapabilitiesAndWeaponsList(ref AttributeCollectionValue atts)
        {
            List<string> capabilities = new List<string>();
            Dictionary<string, int> weaponsAndQuantities = new Dictionary<string, int>();
            string objectID = ((StringValue)atts["ID"]).value;
            SimulationObjectProxy obj = objectProxies[objectID];
            CapabilityValue cv;
            if (atts.attributes.ContainsKey("Capability"))
            {
                cv = atts["Capability"] as CapabilityValue;
               // atts.attributes.Remove("Capability");
            }
            else
            {
                cv = obj["Capability"].GetDataValue() as CapabilityValue;
            }
            foreach (CapabilityValue.Effect ef in ((CapabilityValue)cv).effects)
            {
                if (!capabilities.Contains(ef.name))
                {
                    capabilities.Add(ef.name);
                }
            }

            StringListValue sl;
            SimulationObjectProxy wep;
            string className;
            if (atts.attributes.ContainsKey("DockedWeapons"))
            {
                sl = atts["DockedWeapons"] as StringListValue;
            }
            else
            {
                sl = obj["DockedWeapons"].GetDataValue() as StringListValue;
            }
            foreach (string weapon in sl.strings)
            {
                wep = objectProxies[weapon];
                className = ((StringValue)wep["ClassName"].GetDataValue()).value;
                if (!weaponsAndQuantities.ContainsKey(className))
                {
                    weaponsAndQuantities.Add(className, 0);
                }
                weaponsAndQuantities[className]++;
            }

            List<string> CapabilitiesAndWeaponsList = new List<string>();
            foreach (string c in capabilities)
            {
                CapabilitiesAndWeaponsList.Add(c);
            }
            foreach (string w in weaponsAndQuantities.Keys)
            {
                CapabilitiesAndWeaponsList.Add(String.Format("{0} ({1}x)", w, weaponsAndQuantities[w]));
            }
            DataValue retDV = DataValueFactory.BuildValue("StringListType");
            ((StringListValue)retDV).strings = CapabilitiesAndWeaponsList;

            atts.attributes.Add("CapabilitiesList", retDV);
        }
 
        /// <summary>
        /// Call this method only if the object is being removed before the attack time is up.
        /// </summary>
        /// <param name="objectID"></param>
        private void RemoveAttackContainingUnit(string objectID)
        {
            int counter = 0;
            List<int> attacksToRemove = new List<int>();
            foreach (Attack at in currentAttacks)
            {
                if (at.attacker == objectID || at.target == objectID)
                {
                    attacksToRemove.Add(counter);
                }
                counter++;
            }
            attacksToRemove.Reverse();
            foreach (int i in attacksToRemove)
            {
                currentAttacks.RemoveAt(i);
            }
        }
 
        private Dictionary<string, DataValue> CopyFromCustomAttributes(Dictionary<string, DataValue> incoming)
        {
            Dictionary<string, DataValue> returnDict = new Dictionary<string, DataValue>();

            foreach (KeyValuePair<string, DataValue> kvp in incoming)
            {
                returnDict.Add(kvp.Key, DataValueFactory.BuildFromDataValue(kvp.Value));
            }

            return returnDict;
        }

        private bool AreDecisionMakersInSharedNetwork(string playerOne, string playerTwo)
        {
            int playersFound = 0;

            foreach (string sensorNetworks in networkRosters.Keys)
            {
                foreach (string player in networkRosters[sensorNetworks])
                {
                    if (player == playerOne || player == playerTwo)
                    {
                        playersFound++;
                    }
                }
                if (playersFound >= 2)
                {
                    return true;
                }
                playersFound = 0;
            }

            return false;
        }

        #endregion













    }

}