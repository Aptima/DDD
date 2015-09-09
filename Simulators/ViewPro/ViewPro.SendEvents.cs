using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

//This partial implementation of the ViewPro handles all the "Send Events".
//A Send Event is called from within the ViewPro and provides a single calling
//point for creating and sending a simulation event.
namespace Aptima.Asim.DDD.Simulators.ViewPro
{
    public partial class ViewProSim
    {
        #region send events
        /// <summary>
        /// This method will send out a ViewProInitializeObject event to a specific client.
        /// This event will have that player add this object to their playfield.  Once the
        /// object is in the playfield, it is able to be interacted with.
        /// </summary>
        /// <param name="targetPlayerID">Unique ID of the player recieving this event.</param>
        /// <param name="objectID">Unique ID of the object being revealed.</param>
        /// <param name="location">Location at which to display this object.</param>
        /// <param name="iconName">Icon file name used to display to user.</param>
        /// <param name="ownerID">Unique ID of the owner of the object.</param>
        private void SendViewProInitializeObject(string targetPlayerID, string objectID, LocationValue location, string iconName, string ownerID, bool isWeapon)
        {
            if (!activeDMs.Contains(targetPlayerID))
                return;
            if (!location.exists)
                return;

            SimulationEvent initEvent = SimulationEventFactory.BuildEvent(ref simModel, "ViewProInitializeObject");
            initEvent["Time"] = DataValueFactory.BuildInteger(currentTick);
            initEvent["TargetPlayer"] = DataValueFactory.BuildString(targetPlayerID);
            initEvent["ObjectID"] = DataValueFactory.BuildString(objectID);
            initEvent["Location"] = location;
            initEvent["OwnerID"] = DataValueFactory.BuildString(ownerID);
            initEvent["IsWeapon"] = DataValueFactory.BuildBoolean(isWeapon);
            initEvent["LabelColor"] = DataValueFactory.BuildInteger(dmColorMapping[ownerID]);

            String classification = GetClassificationForDM(objectID, targetPlayerID);
            String overrideIcon = GetClassificationBasedIcon(objectID, classification);
            initEvent["CurrentClassification"] = DataValueFactory.BuildString(classification);
            if (overrideIcon != String.Empty)
            {
                initEvent["IconName"] = DataValueFactory.BuildString(overrideIcon);
            }
            else
            {
                initEvent["IconName"] = DataValueFactory.BuildString(iconName);
            }
            distClient.PutEvent(initEvent);
        }

        private void SendViewProInitializeObject(string targetPlayerID, AttributeCollectionValue acv)
        {
            if (!activeDMs.Contains(targetPlayerID))
                return;

            string objectID = ((StringValue)acv["ID"]).value;
            string ownerID;
            string iconName;
            SimulationObjectProxy objectProxy = objectProxies[objectID];
            LocationValue location;
            bool isWeapon = false;
            AttributeCollectionValue playersView = new AttributeCollectionValue();
            if (!dmViews.ContainsKey(targetPlayerID))
            {
                dmViews.Add(targetPlayerID, new Aptima.Asim.DDD.CommonComponents.ObjectsAttributeCollection.ObjectsAttributeCollection());
            }
            if (!dmViews[targetPlayerID].ContainsObject(objectID))
            {
                dmViews[targetPlayerID].UpdateObject(objectID, new AttributeCollectionValue());
            }
            playersView = dmViews[targetPlayerID][objectID];

            
            if (acv.attributes.ContainsKey("Location"))
            {
                location = acv["Location"] as LocationValue;
            }
            else if (playersView.attributes.ContainsKey("Location"))
            {
                location = ((DetectedAttributeValue)playersView["Location"]).value as LocationValue;
            }
            else
            {
                location = objectProxy["Location"].GetDataValue() as LocationValue;
            }
            if (acv.attributes.ContainsKey("OwnerID"))
            {
                ownerID = ((StringValue)acv["OwnerID"]).value;
            }
            else if (playersView.attributes.ContainsKey("OwnerID"))
            {
                ownerID = ((StringValue)((DetectedAttributeValue)playersView["OwnerID"]).value).value;
            }
            else
            {
                ownerID = ((StringValue)objectProxy["OwnerID"].GetDataValue()).value;
            }
            if (acv.attributes.ContainsKey("IconName"))
            {
                iconName = ((StringValue)acv["IconName"]).value;
            }
            else if (playersView.attributes.ContainsKey("IconName"))
            {
                iconName = ((StringValue)((DetectedAttributeValue)playersView["IconName"]).value).value;
            }
            else
            {
                iconName = "ImageLib.Unknown.png";// ((StringValue)objectProxy["IconName"].GetDataValue()).value;
            }
            if (acv.attributes.ContainsKey("IsWeapon"))
            {
                isWeapon = ((BooleanValue)acv["IsWeapon"]).value;
            }
            else if (playersView.attributes.ContainsKey("IsWeapon"))
            {
                isWeapon = ((BooleanValue)((DetectedAttributeValue)playersView["IsWeapon"]).value).value;
            }
            else
            {
                isWeapon = ((BooleanValue)objectProxy["IsWeapon"].GetDataValue()).value;
            }

            SendViewProInitializeObject(targetPlayerID, objectID, location, iconName, ownerID, isWeapon);
        }

        /// <summary>
        /// Sends out a ClientRemoveObject event for a specified object to a specified user.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="playerID"></param>
        private void SendRemoveObjectEvent(string objectID, string playerID)
        {
            if (!activeDMs.Contains(playerID))
                return;

            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "ClientRemoveObject");
            e["TargetPlayer"] = DataValueFactory.BuildString(playerID);
            e["ObjectID"] = DataValueFactory.BuildString(objectID);
            e["Time"] = DataValueFactory.BuildInteger(currentTick);
            dmViews[playerID].RemoveObject(objectID);

            distClient.PutEvent(e);
        }

        /// <summary>
        /// This event receives an attack object, sends out the attack, and returns false if the current time
        /// is the same as the attack end time.  This info is used to remove the attack from the attack list if the
        /// time is up.
        /// </summary>
        /// <param name="attack"></param>
        /// <returns></returns>
        private bool SendAttackEvent(Attack attack)
        {
            bool returnBool = true;
            int endTime = attack.endTime;
            if (endTime <= currentTick)
            { //attack is over
                returnBool = false;
            }

            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "ViewProAttackUpdate");
            e["AttackerID"] = DataValueFactory.BuildString(attack.attacker);
            e["TargetID"] = DataValueFactory.BuildString(attack.target);
            e["AttackEndTime"] = DataValueFactory.BuildInteger(attack.endTime);
            e["Time"] = DataValueFactory.BuildInteger(currentTick);
            distClient.PutEvent(e);

            return returnBool;
        }

        private void SendViewProStopObjectUpdate(string objectID)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModel, "ViewProStopObjectUpdate");
            ((StringValue)e["ObjectID"]).value = objectID;
            ((IntegerValue)e["Time"]).value = currentTick;

            distClient.PutEvent(e);

            if (movingObjects.Contains(objectID))
            {
                movingObjects.Remove(objectID);
            }
        }
 
        /// <summary>
        /// This method extracts an objects attributes from DetectedValues in an ACV, and then sends
        /// out the appropriate info to a specified player.
        /// </summary>
        /// <param name="destinationPlayerID"></param>
        /// <param name="objectsAttributes"></param>
        /// <param name="time"></param>
        private void SendViewProAttributeUpdate(string destinationPlayerID, AttributeCollectionValue objectsAttributes)
        {
            if (!activeDMs.Contains(destinationPlayerID))
                return;

            SimulationEvent vpu = null;
            objectsAttributes = ExtractDetectedValuesFromACV(objectsAttributes);
            AddCapabilitiesAndWeaponsList(ref objectsAttributes);
            if (objectsAttributes.attributes.ContainsKey("Vulnerability"))
            {
                List<string> vulnerabilityList = new List<string>();
                foreach (VulnerabilityValue.Transition t in ((VulnerabilityValue)objectsAttributes["Vulnerability"]).transitions)
                {
                    foreach (VulnerabilityValue.TransitionCondition tc in t.conditions)
                    {
                        if (!vulnerabilityList.Contains(tc.capability))
                        {
                            vulnerabilityList.Add(tc.capability);
                        }
                    }
                }
//                objectsAttributes.attributes.Remove("Vulnerability");
                StringListValue sl = new StringListValue();
                sl.strings = vulnerabilityList;
                objectsAttributes.attributes.Add("VulnerabilityList", sl as DataValue);
            }
            if (objectsAttributes.attributes.ContainsKey("Sensors"))
            {
                List<string> sensorList = new List<string>();
                foreach (SensorValue sv in ((SensorArrayValue)objectsAttributes["Sensors"]).sensors)
                {
                    if (!sensorList.Contains(sv.sensorName))
                    {
                        sensorList.Add(sv.sensorName);
                    }
                }

//                objectsAttributes.attributes.Remove("Sensors");
                StringListValue sl = new StringListValue();
                sl.strings = sensorList;
                objectsAttributes.attributes.Add("SensorList", sl as DataValue);
            }
            objectsAttributes["DockedObjects"] = new StringListValue();
            List<string> strList = new List<string>();
            //if (((StringValue)objectProxies[((StringValue)objectsAttributes["ID"]).value]["ParentObjectID"].GetDataValue()).value != string.Empty)
            //{
            //    strList.Add("Dock To Parent");
            //}
            strList.AddRange(((StringListValue)objectProxies[((StringValue)objectsAttributes["ID"]).value]["DockedObjects"].GetDataValue()).strings);
            ((StringListValue)objectsAttributes["DockedObjects"]).strings = strList;
            vpu = SimulationEventFactory.BuildEvent(ref simModel, "ViewProAttributeUpdate");
            if (!objectsAttributes.attributes.ContainsKey("MaximumSpeed"))
            {
                DoubleValue dv = new DoubleValue();
                dv.value = ((DoubleValue)objectProxies[((StringValue)objectsAttributes["ID"]).value]["MaximumSpeed"].GetDataValue()).value *
                    ((DoubleValue)objectProxies[((StringValue)objectsAttributes["ID"]).value]["ActiveRegionSpeedMultiplier"].GetDataValue()).value;
                objectsAttributes.attributes.Add("MaximumSpeed", dv as DataValue);
            }

            String classification = GetClassificationForDM(((StringValue)objectsAttributes["ID"]).value, destinationPlayerID);
            objectsAttributes["CurrentClassification"] = DataValueFactory.BuildString(classification);
            String overrideIcon = GetClassificationBasedIcon(((StringValue)objectsAttributes["ID"]).value, classification);
            if (overrideIcon != String.Empty)
            {
                objectsAttributes["IconName"] = DataValueFactory.BuildString(overrideIcon);
            }
            else
            {
                SimulationObjectProxy ob = objectProxies[((StringValue)objectsAttributes["ID"]).value];
                objectsAttributes["IconName"] = DataValueFactory.BuildString(((StringValue)ob["IconName"].GetDataValue()).value);
            }

            vpu["TargetPlayer"] = DataValueFactory.BuildString(destinationPlayerID);
            vpu["ObjectID"] = objectsAttributes["ID"];
            vpu["OwnerID"] = objectsAttributes["OwnerID"];

            //RANGE RING LOGIC
            string ownerId = ((StringValue)objectsAttributes["OwnerID"]).value;

            if (( (destinationPlayerID == ownerId || selectedRangeRingLevel == RangeRingLevels.FULL) ||
                     (selectedRangeRingLevel == RangeRingLevels.SENSORNETWORK && AreDecisionMakersInSharedNetwork(destinationPlayerID, ownerId)) ) &&
                    selectedRangeRingLevel != RangeRingLevels.DISABLED) //this needs to be based on ownership, etc.
            {
                SimulationObjectProxy objProxy = null;
                if (objectProxies.ContainsKey(((StringValue)objectsAttributes["ID"]).value))
                {
                    objProxy = objectProxies[((StringValue)objectsAttributes["ID"]).value];
                    AddRangeRings(ref objectsAttributes, ref objProxy);
                }
                else
                {
                    //if not, something's wrong
                    Console.WriteLine("HELP");
                }
            }
            else
            {
                objectsAttributes.attributes.Remove("Vulnerability");
                objectsAttributes.attributes.Remove("Sensors");
                objectsAttributes.attributes.Remove("Capability");
            }
            //

            //if (objectsAttributes.attributes.ContainsKey("MaximumSpeed"))
            //{
            //    Console.Out.Write(String.Format("{0} moving at {1}", ((StringValue)objectsAttributes["ID"]).value, ((DoubleValue)objectsAttributes["MaximumSpeed"]).value));
            //    //foreach (string s in objectsAttributes.attributes.Keys)
            //    //{
            //    //    Console.Out.Write(String.Format("{0}, ", s));
            //    //}
            //    Console.Out.WriteLine();
            //}
            vpu["Attributes"] = objectsAttributes;
            vpu["Time"] = DataValueFactory.BuildInteger(currentTick);

            string xml = DataValueFactory.XMLSerialize(objectsAttributes);
            AttributeCollectionValue temp = new AttributeCollectionValue();
            temp.FromXML(xml);


            distClient.PutEvent(vpu);
        }

        /// <summary>
        /// This event is broadcast out to each client.  That client will attempt to put the object in motion, but will only
        /// succeed if the object already exists in its playfield.
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="ownerID"></param>
        /// <param name="location"></param>
        /// <param name="desLocation"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="throttle"></param>
        /// <param name="time"></param>
        /// <param name="iconName"></param>
        /// <param name="isWeapon"></param>
        private void SendViewProMotionUpdate(string objectID, string ownerID, LocationValue location, LocationValue desLocation, double maxSpeed, double throttle, string iconName, bool isWeapon, double activeRegionSpeedMultiplier)
        {
            SimulationEvent vpmu = null;
            vpmu = SimulationEventFactory.BuildEvent(ref simModel, "ViewProMotionUpdate");

            vpmu["ObjectID"] = DataValueFactory.BuildString(objectID);
            vpmu["OwnerID"] = DataValueFactory.BuildString(ownerID);
            vpmu["Location"] = location;
            vpmu["DestinationLocation"] = desLocation;
            //if (objectID == "Fighter01_Troop_2")
            //{
            //    Console.Out.Write(String.Format("\n{0} is moving at {1}*{2}\n", objectID, maxSpeed, activeRegionSpeedMultiplier));
            //}
            vpmu["MaximumSpeed"] = DataValueFactory.BuildDouble(maxSpeed * activeRegionSpeedMultiplier);
            vpmu["Throttle"] = DataValueFactory.BuildDouble(throttle);
            vpmu["Time"] = DataValueFactory.BuildInteger(currentTick);
            vpmu["IconName"] = DataValueFactory.BuildString(iconName);
            //add label color to the mix
            vpmu["LabelColor"] = DataValueFactory.BuildInteger(dmColorMapping[ownerID]);
            vpmu["IsWeapon"] = DataValueFactory.BuildBoolean(isWeapon);
            distClient.PutEvent(vpmu);
            if (!movingObjects.Contains(objectID) &&
                !DataValueFactory.CompareDataValues(location, desLocation))
            {
                movingObjects.Add(objectID);
            }
        }

        private void SendViewProActiveRegionUpdate(string objectID, bool isVisible, int displayColor, Polygon3D poly)
        {
            SimulationEvent vpmu = null;
            vpmu = SimulationEventFactory.BuildEvent(ref simModel, "ViewProActiveRegionUpdate");

            vpmu["ObjectID"] = DataValueFactory.BuildString(objectID);
            vpmu["IsVisible"] = DataValueFactory.BuildBoolean(isVisible);
            vpmu["DisplayColor"] = DataValueFactory.BuildInteger(displayColor);
            vpmu["Shape"] = poly.Footprint.GetPolygonValue();

            distClient.PutEvent(vpmu);
        }

        /// <summary>
        /// This event is broadcast out to each client.  The client will attempt to put the object in motion, but
        /// will only succeed if the object already exists in its playfield.
        /// </summary>
        /// <param name="attributes">This is a collection of object attributes.  If any necessary attributes for
        /// a ViewProMotionUpdate are missing from this collection, the appropriate attribute values will be
        /// retrieved from that object's blackboard proxy.</param>
        private void SendViewProMotionUpdate(AttributeCollectionValue attributes)
        {
            string objectID = ((StringValue)attributes["ObjectID"]).value;
            string ownerID;
            string iconName;
            SimulationObjectProxy objectProxy = objectProxies[objectID];
            LocationValue location;
            LocationValue destLocation;
            double maxSpeed;
            double throttle;
            bool isWeapon = false;
            double activeRegionSpeedMultiplier;

            activeRegionSpeedMultiplier = ((DoubleValue)objectProxy["ActiveRegionSpeedMultiplier"].GetDataValue()).value;

            if (attributes.attributes.ContainsKey("Location"))
            {
                location = attributes["Location"] as LocationValue;
            }
            else
            {
                location = objectProxy["Location"].GetDataValue() as LocationValue;
            }

            if (attributes.attributes.ContainsKey("DestinationLocation"))
            {
                destLocation = attributes["DestinationLocation"] as LocationValue;
            }
            else
            {
                destLocation = objectProxy["DestinationLocation"].GetDataValue() as LocationValue;
            }

            if (attributes.attributes.ContainsKey("OwnerID"))
            {
                ownerID = ((StringValue)attributes["OwnerID"]).value;
            }
            else
            {
                ownerID = ((StringValue)objectProxy["OwnerID"].GetDataValue()).value;
            }

            if (attributes.attributes.ContainsKey("MaximumSpeed"))
            {
                maxSpeed = ((DoubleValue)attributes["MaximumSpeed"]).value;
            }
            else
            {
                maxSpeed = ((DoubleValue)objectProxy["MaximumSpeed"].GetDataValue()).value;
            }

            if (attributes.attributes.ContainsKey("Throttle"))
            {
                throttle = ((DoubleValue)attributes["Throttle"]).value;
            }
            else
            {
                throttle = ((DoubleValue)objectProxy["Throttle"].GetDataValue()).value;
            }
            if (attributes.attributes.ContainsKey("IconName"))
            {
                iconName = ((StringValue)attributes["IconName"]).value;
            }
            else
            {
                iconName = ((StringValue)objectProxy["IconName"].GetDataValue()).value;
            }
            if (attributes.attributes.ContainsKey("IsWeapon"))
            {
                isWeapon = ((BooleanValue)attributes["IsWeapon"]).value;
            }
            else
            {
                isWeapon = ((BooleanValue)objectProxy["IsWeapon"].GetDataValue()).value;
            }
            SendViewProMotionUpdate(objectID, ownerID, location, destLocation, maxSpeed, throttle, iconName, isWeapon, activeRegionSpeedMultiplier);
        }

        #endregion
    }

}
