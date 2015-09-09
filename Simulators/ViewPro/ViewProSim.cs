using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.ObjectsAttributeCollection;

//This partial implementation of the ViewPro handles all the "Sensing Methods".
//A Sensing Method is any method that deals with the sensing and detection of 
//objects in the simulation.  Because a lot of these methods take up a lot of code,
//I have split the ViewPro into 4 distinct files.  Each file will limit what its 
//methods deal with, and should help to clarify the modification process.
namespace Aptima.Asim.DDD.Simulators.ViewPro
{
    public partial class ViewProSim
    {
        #region Members
        private SimulationModelInfo simModel;
        private Dictionary<string, SimulationObjectProxy> objectProxies;
        private List<string> listOfObjectIDs;
        private List<string> listOfObstructionIDs;
        private Dictionary<string, StateDB.ActiveRegion> obstructions;
        private Dictionary<string, int> dmColorMapping;
        private Dictionary<string, List<string>> dmOwnedObjects;
        private static Dictionary<string, ObjectsAttributeCollection> objectViews;
        private static Dictionary<string, ObjectsAttributeCollection> dmViews;
        private Dictionary<string, List<string>> teamDefinitions;//lists team hostilities
        private Dictionary<string, List<string>> networkRosters;//lists members within a network
        private Dictionary<string, List<string>> networkObjects;//lists objects within a network
        private Dictionary<string, string> dmTeamsMap;
        private List<string> activeDMs;
        private static Dictionary<string, Dictionary<string, string>> unitTags = null; //[objectid]/{[dm id], [tag]}
        private static Dictionary<string, Dictionary<string, string>> recentUnitTags = null;//[objectid]/{[dm id], [tag]}

        private static Dictionary<string, bool> activeSensorNetworks;
        private List<string> movingObjects;
        private static bool Omniscience = true;
        private double dTimeSec;
        private int dTimeMSec;
        private static int currentTick = 0;
        private static int randomSeed;
        private static Random randomGenerator;
        private static bool isPaused = true;

        private static Dictionary<String, Dictionary<String, String>> teamClassifications; //[team name]/{[object id],[classification]}
        private static Dictionary<String, Dictionary<String, String>> teamClassificationChanges;

        private enum RangeRingLevels {DISABLED = 0, PRIVATE = 1, SENSORNETWORK = 2, FULL = 3,};
        private RangeRingLevels selectedRangeRingLevel = RangeRingLevels.FULL; //DEFAULT
        #endregion

        #region sensory methods

        /// <summary>
        /// This method will run through each object and determine what those objects can sense.
        /// </summary>
        private void CalculateSensoryAlgorithm()
        {
            //objectProxies = bbClient.GetObjectProxies();
            Dictionary<string, Dictionary<string, AttributeCollectionValue>> allObjectsViews = new Dictionary<string, Dictionary<string, AttributeCollectionValue>>();
            Dictionary<string, Dictionary<string, AttributeCollectionValue>> allSensorNetworksViews = new Dictionary<string, Dictionary<string, AttributeCollectionValue>>();
            Dictionary<string, Dictionary<string, AttributeCollectionValue>> allDMsViews = new Dictionary<string, Dictionary<string, AttributeCollectionValue>>();

            //each object senses each other object, and the allObjectsViews is populated and returned.
            SenseAllObjects(ref allObjectsViews);

            //each sensor network goes through the views of objects contained within that network, and
            //creates its best view for all its members.
            RetrieveSensorNetworkView(ref allSensorNetworksViews, ref allObjectsViews);

            //Go through each sensor network, and create the best DM view for each active DM.
            RetrieveAllDMViews(ref allDMsViews, ref allSensorNetworksViews);

            //Send out view pro updates.
            List<string> recentlyDiscoveredObjects = new List<string>();
            bool sendAttUpdate = true;
            foreach (string dm in dmOwnedObjects.Keys)
            {
                if (!allDMsViews.ContainsKey(dm))
                    continue;
                CompareNewDMViewWithPrevious(dm, allDMsViews[dm], ref recentlyDiscoveredObjects);

            }
            foreach (string obj in recentlyDiscoveredObjects)
            {
                if (!movingObjects.Contains(obj))
                    continue;
                AttributeCollectionValue atts = new AttributeCollectionValue();
                atts.attributes.Add("ObjectID", DataValueFactory.BuildString(obj));
                SendViewProMotionUpdate(atts);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allObjectViews"></param>
        private void SenseAllObjects(ref Dictionary<string, Dictionary<string, AttributeCollectionValue>> allObjectsViews)
        {
            SimulationObjectProxy sensingProxy = null;
            SimulationObjectProxy targetProxy = null;
            Vec3D sensingLocation = null;
            Vec3D targetsLocation = null;
            LocationValue senLocation = null;
            LocationValue tarLocation = null;
            double distance = 0.0;
            SensorArrayValue sav = null;
            DataValue targetsAttribute = null;
            string ownerID = null;
            string objectType = null;
            DetectedAttributeValue detectedAttribute;
            AttributeCollectionValue singleAttributeCollection = null;
            bool isSensed = false;

            // The dictionary is constantly created in this method //
            Dictionary<string, AttributeCollectionValue> singleObjectView;

            //AD to improve this later:
            // for each object in the visible ObjectDistances collection
            //    for each object after the current index
            //       check for the index object's view of the nested object
            //       check for the nested object's view of the index object
            //Possible hangup is the "All" emitter.
            




            int x = 0;
            //Each object senses each other object
            //foreach (string sensorObjectID in listOfObjectIDs)
            foreach (KeyValuePair<string, List<string>> networks in networkRosters)
            {
                if (!activeSensorNetworks[networks.Key])
                    continue;
                foreach (string sensorObjectID in networkObjects[networks.Key])
                {
                    sensingProxy = objectProxies[sensorObjectID];
                    objectType = sensingProxy.GetObjectType();
                    senLocation = sensingProxy["Location"].GetDataValue() as LocationValue;
                    if (senLocation.exists)
                    {
                        ownerID = ((StringValue)sensingProxy["OwnerID"].GetDataValue()).value;
                        singleObjectView = new Dictionary<string, AttributeCollectionValue>();
                        x++;
                        //foreach (string targetObjectID in listOfObjectIDs)
                        foreach (List<string> objects in networkObjects.Values)
                        {
                            if (objects.Contains(sensorObjectID))
                            {
                                if (((EmitterValue)sensingProxy["Emitters"].GetDataValue()).emitters.ContainsKey("Invisible"))
                                    //OR if it's the "master" object for a region?? -lisa
                                {
                                    continue;
                                }
                                string target = sensorObjectID;

                                singleAttributeCollection = new AttributeCollectionValue();
                                foreach (KeyValuePair<string, AttributeInfo> simModelAtt in simModel.objectModel.objects[objectType].attributes)
                                {
                                    if (!simModelAtt.Value.ownerObservable == true)
                                        continue;
                                    //if (!sensingProxy.GetKeys().Contains(simModelAtt.Key))
                                    //    continue;
                                    DataValue t = sensingProxy[simModelAtt.Key].GetDataValue();
                                    if (t.dataType == "CustomAttributesType")
                                    {
                                        Dictionary<string, DataValue> copiedDict = CopyFromCustomAttributes(((CustomAttributesValue)t).attributes);
                                        t = new CustomAttributesValue();
                                        ((CustomAttributesValue)t).attributes = copiedDict;
                                    }
                                    detectedAttribute = new DetectedAttributeValue();
                                    detectedAttribute.stdDev = 100;
                                    detectedAttribute.value = DataValueFactory.BuildFromDataValue(t);
                                    AddAttributeToACV(ref singleAttributeCollection, simModelAtt.Key, detectedAttribute);
                                }//end foreach sensable attribute

                                if (!allObjectsViews.ContainsKey(sensorObjectID))
                                {
                                    allObjectsViews.Add(sensorObjectID, new Dictionary<string, AttributeCollectionValue>());
                                }
                                if (!objectViews.ContainsKey(sensorObjectID))
                                {
                                    objectViews.Add(sensorObjectID, new ObjectsAttributeCollection());
                                }
                                //update the global "allObjectViews".  The return from UpdateObject is the collection of attributes
                                //that have changed.  These attributes are stored in allObjectsViews and then sent out
                                //to users.
                                AttributeCollectionValue changedAttributes = objectViews[sensorObjectID].UpdateObject(target, singleAttributeCollection);

                                //if (changedAttributes != null && selectedRangeRingLevel != RangeRingLevels.DISABLED)
                                //{
                                //    CalculateRangeRings(ref changedAttributes, ref sensingProxy);
                                //}

                                if (changedAttributes != null)
                                {
                                    allObjectsViews[sensorObjectID].Add(target, changedAttributes);
                                }
                                else//this is so there is at least the empty entry for the detected object so 
                                //you still know it is detected.
                                {
                                    if (!allObjectsViews[sensorObjectID].ContainsKey(target))
                                    {
                                        allObjectsViews[sensorObjectID].Add(target, new AttributeCollectionValue());
                                    }
                                }
                            }
                            else
                            {
                                foreach (string targetObjectID in objects)
                                {//sensing and target objects are not the same 
                                    targetProxy = objectProxies[targetObjectID];
                                    tarLocation = targetProxy["Location"].GetDataValue() as LocationValue;
                                    if (tarLocation.exists)
                                    {
                                        EmitterValue emitters = targetProxy["Emitters"].GetDataValue() as EmitterValue;
                                        if (emitters.emitters.ContainsKey("All"))
                                        {
                                            //if an object has an all emitter, retrieve all attributes without sensing algorithm
                                            AttributeCollectionValue atts = new AttributeCollectionValue();
                                            if (singleObjectView.ContainsKey(targetObjectID))
                                            {
                                                atts = singleObjectView[targetObjectID];
                                            }
                                            SenseAllAttributes(ref atts, targetProxy);
                                            singleObjectView[targetObjectID] = atts;
                                            isSensed = true;

                                            if (!allObjectsViews.ContainsKey(sensorObjectID))
                                            {
                                                allObjectsViews.Add(sensorObjectID, new Dictionary<string, AttributeCollectionValue>());
                                            }
                                            if (!objectViews.ContainsKey(sensorObjectID))
                                            {
                                                objectViews.Add(sensorObjectID, new ObjectsAttributeCollection());
                                            }
                                            //update the global "allObjectViews".  The return from UpdateObject is the collection of attributes
                                            //that have changed.  These attributes are stored in allObjectsViews and then sent out
                                            //to users.
                                            AttributeCollectionValue changedAttributes = objectViews[sensorObjectID].UpdateObject(targetObjectID, atts);

                                            //if (changedAttributes != null && selectedRangeRingLevel == RangeRingLevels.FULL)
                                            //{//the only way to sense a "target" here is for FULL ring display
                                            //    CalculateRangeRings(ref changedAttributes, ref targetProxy);
                                            //}

                                            if (changedAttributes != null)
                                            {
                                                allObjectsViews[sensorObjectID].Add(targetObjectID, changedAttributes);
                                            }
                                            else//this is so there is at least the empty entry for the detected object so 
                                            //you still know it is detected.
                                            {
                                                if (!allObjectsViews[sensorObjectID].ContainsKey(targetObjectID))
                                                {
                                                    allObjectsViews[sensorObjectID].Add(targetObjectID, new AttributeCollectionValue());
                                                }
                                            }
                                        }
                                        else if (emitters.emitters.ContainsKey("Invisible"))
                                            //or if object is "master" for moving region -lisa
                                        {
                                            continue;
                                        }
                                        else
                                        {//object does not have an all emitter, continue to check each emitter value 
                                            AttributeCollectionValue atts = new AttributeCollectionValue();
                                            if (Omniscience)
                                            {
                                                if (singleObjectView.ContainsKey(targetObjectID))
                                                {
                                                    atts = singleObjectView[targetObjectID];
                                                }
                                                SenseAllAttributes(ref atts, targetProxy);
                                                singleObjectView[targetObjectID] = atts;
                                                isSensed = true;
                                                if (!allObjectsViews.ContainsKey(sensorObjectID))
                                                {
                                                    allObjectsViews.Add(sensorObjectID, new Dictionary<string, AttributeCollectionValue>());
                                                }
                                                if (!objectViews.ContainsKey(sensorObjectID))
                                                {
                                                    objectViews.Add(sensorObjectID, new ObjectsAttributeCollection());
                                                }
                                                //update the global "allObjectViews".  The return from UpdateObject is the collection of attributes
                                                //that have changed.  These attributes are stored in allObjectsViews and then sent out
                                                //to users.
                                                AttributeCollectionValue changedAttributes = objectViews[sensorObjectID].UpdateObject(targetObjectID, atts);

                                                //if (changedAttributes != null && selectedRangeRingLevel == RangeRingLevels.FULL)
                                                //{//the only way to sense a "target" here is for FULL ring display
                                                //    CalculateRangeRings(ref changedAttributes, ref targetProxy);
                                                //}

                                                if (changedAttributes != null)
                                                {
                                                    allObjectsViews[sensorObjectID].Add(targetObjectID, changedAttributes);
                                                }
                                                else//this is so there is at least the empty entry for the detected object so 
                                                //you still know it is detected.
                                                {
                                                    if (!allObjectsViews[sensorObjectID].ContainsKey(targetObjectID))
                                                    {
                                                        allObjectsViews[sensorObjectID].Add(targetObjectID, new AttributeCollectionValue());
                                                    }
                                                }
                                                continue;
                                            }

                                            sensingLocation = new Vec3D(senLocation);
                                            targetsLocation = new Vec3D(tarLocation);
                                            isSensed = false;
                                            distance = sensingLocation.ScalerDistanceTo(targetsLocation);
                                            sav = sensingProxy["Sensors"].GetDataValue() as SensorArrayValue;

                                            foreach (SensorValue sv in sav.sensors)
                                            {
                                                if (distance < sv.maxRange)
                                                {
                                                    //Find obstructions
                                                    List<SimulationObjectProxy> obstructions = FindObstructions(sensingLocation, targetsLocation);

                                                    foreach (KeyValuePair<string, List<ConeValue>> kvp in sv.ranges)
                                                    { //Key is the attribute being sensed, value is a list of cones
                                                        if (kvp.Key == "All")
                                                        {
                                                            atts = new AttributeCollectionValue();
                                                            if (singleObjectView.ContainsKey(targetObjectID))
                                                            {
                                                                atts = singleObjectView[targetObjectID];
                                                            }
                                                            SenseAllAttributes(ref atts, targetProxy);
                                                            singleObjectView[targetObjectID] = atts;
                                                            isSensed = true;
                                                        }
                                                        else //not an All sensor, so run detection algorithm
                                                        {//Added in main-line
                                                            if (!emitters.emitters.ContainsKey(kvp.Key))
                                                            {
                                                                continue;
                                                            }
                                                            //Custom attributes fix:
                                                            DataValue currentDataValue;
                                                            try
                                                            {
                                                                currentDataValue = targetProxy[kvp.Key].GetDataValue();
                                                            }
                                                            catch
                                                            {
                                                                currentDataValue = targetProxy["CustomAttributes"].GetDataValue();
                                                                currentDataValue = ((CustomAttributesValue)currentDataValue)[kvp.Key];
                                                                //will throw an error here if object doesn't contain custom atts, or 
                                                                //if it doesnt contain the specified custom att.
                                                            }

                                                            bool isObstructed = false;
                                                            foreach (SimulationObjectProxy reg in obstructions)
                                                            {
                                                                foreach (string attributeBlocked in ((StringListValue)reg["BlocksSensorTypes"].GetDataValue()).strings)
                                                                {
                                                                    if (kvp.Key == attributeBlocked)
                                                                    {
                                                                        isObstructed = true;
                                                                    }
                                                                }
                                                            }
                                                            if (isObstructed)
                                                            {
                                                                detectedAttribute = new DetectedAttributeValue();
                                                                detectedAttribute.value = DataValueFactory.BuildValue(currentDataValue.dataType);
                                                                detectedAttribute.confidence = 0;
                                                                //continue;
                                                            }
                                                            else
                                                            {
                                                                targetsAttribute = DataValueFactory.BuildFromDataValue(currentDataValue);
                                                                //ev is emitters
                                                                Dictionary<string, double> emitterCollection = emitters.emitters[kvp.Key];

                                                                detectedAttribute = new DetectedAttributeValue();
                                                                detectedAttribute = ObjectMath.Detection(senLocation, tarLocation, targetsAttribute, kvp.Value, emitterCollection, obstructions, ref randomGenerator);
                                                            }
                                                            if (detectedAttribute != null)
                                                            {
                                                                singleAttributeCollection = new AttributeCollectionValue();
                                                                if (singleObjectView.ContainsKey(targetObjectID))
                                                                {
                                                                    singleAttributeCollection = singleObjectView[targetObjectID];
                                                                }
                                                                AddAttributeToACV(ref singleAttributeCollection, kvp.Key, detectedAttribute);
                                                                isSensed = true;
                                                            }
                                                        }
                                                    }//end foreach attribute in sensor

                                                    //if the object has any attributes sensed, fill in some other info for the object's view.
                                                    if (isSensed)
                                                    {
                                                        detectedAttribute = new DetectedAttributeValue();
                                                        detectedAttribute.stdDev = 100;
                                                        detectedAttribute.value = targetProxy["ID"].GetDataValue();
                                                        if (singleAttributeCollection == null)
                                                        {
                                                            singleAttributeCollection = new AttributeCollectionValue();
                                                        }
                                                        if (!singleObjectView.ContainsKey(targetObjectID))
                                                        {
                                                            singleObjectView.Add(targetObjectID, singleAttributeCollection);
                                                        }
                                                        singleAttributeCollection = singleObjectView[targetObjectID];
                                                        AddAttributeToACV(ref singleAttributeCollection, "ID", detectedAttribute);

                                                        detectedAttribute = new DetectedAttributeValue();
                                                        detectedAttribute.stdDev = 100;
                                                        detectedAttribute.value = targetProxy["OwnerID"].GetDataValue();
                                                        AddAttributeToACV(ref singleAttributeCollection, "OwnerID", detectedAttribute);
                                                    }//end if isSensed.
                                                }
                                            }//end foreach sensor in sensor array

                                            if (singleObjectView.ContainsKey(targetObjectID))
                                            {
                                                AttributeCollectionValue attr = singleObjectView[targetObjectID];
                                                if (!allObjectsViews.ContainsKey(sensorObjectID))
                                                {
                                                    allObjectsViews.Add(sensorObjectID, new Dictionary<string, AttributeCollectionValue>());
                                                }
                                                if (!objectViews.ContainsKey(sensorObjectID))
                                                {
                                                    objectViews.Add(sensorObjectID, new ObjectsAttributeCollection());
                                                }

                                                if (attr.attributes.ContainsKey("CustomAttributes"))
                                                {
                                                    DataValue t = ((DetectedAttributeValue)attr["CustomAttributes"]).value;
                                                    double conf = ((DetectedAttributeValue)attr["CustomAttributes"]).confidence;
                                                    Dictionary<string, DataValue> copiedDict = CopyFromCustomAttributes(((CustomAttributesValue)t).attributes);
                                                    t = new CustomAttributesValue();
                                                    ((CustomAttributesValue)t).attributes = copiedDict;
                                                    attr.attributes.Remove("CustomAttributes");
                                                    attr.attributes.Add("CustomAttributes", DataValueFactory.BuildDetectedValue(t, Convert.ToInt32(conf)));
                                                }
                                                //update the global "allObjectViews".  The return from UpdateObject is the collection of attributes
                                                //that have changed.  These attributes are stored in allObjectsViews and then sent out
                                                //to users.
                                                AttributeCollectionValue changedAttributes = objectViews[sensorObjectID].UpdateObject(targetObjectID, attr);

                                                //if (changedAttributes != null && selectedRangeRingLevel == RangeRingLevels.FULL)
                                                //{//the only way to sense a "target" here is for FULL ring display
                                                //    CalculateRangeRings(ref changedAttributes, ref targetProxy);
                                                //}

                                                if (changedAttributes != null)
                                                {
                                                    allObjectsViews[sensorObjectID].Add(targetObjectID, changedAttributes);
                                                }
                                                else//this is so there is at least the empty entry for the detected object so 
                                                //you still know it is detected.
                                                {
                                                    if (!allObjectsViews[sensorObjectID].ContainsKey(targetObjectID))
                                                    {
                                                        allObjectsViews[sensorObjectID].Add(targetObjectID, new AttributeCollectionValue());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (objectViews.ContainsKey(sensorObjectID))
                                                {
                                                    if (objectViews[sensorObjectID].ContainsObject(targetObjectID))
                                                    {
                                                        objectViews[sensorObjectID].RemoveObject(targetObjectID);
                                                        //if you once knew of this object, and now don't, remove it from object's view.
                                                    }
                                                }
                                            }
                                        }//end emitter detection
                                    }//end if target is visible
                                }
                            }
                        }//end foreach target object
                    }//end if sensor is visible
                }//end of foreach sensing object
            }//end of Foreach sensor network
            if (x > 0)
            {
                //Console.Out.WriteLine("ViewPro: {0} objects were sensing at time {1}.", x, currentTick / 1000);
            }
        }
        /// <summary>
        /// This method goes through each sensor network roster, and generates the networks' best views.
        /// </summary>
        /// <param name="sensorNetworkViews"></param>
        /// <param name="allObjectViews"></param>
        private void RetrieveSensorNetworkView(ref Dictionary<string, Dictionary<string, AttributeCollectionValue>> sensorNetworkViews,
                                               ref Dictionary<string, Dictionary<string, AttributeCollectionValue>> allObjectViews)
        {
            Dictionary<string, AttributeCollectionValue> singleSensorNetworkView;
            string userID;

            foreach (KeyValuePair<string, List<string>> sensorNetworks in networkRosters)
            {
                singleSensorNetworkView = new Dictionary<string, AttributeCollectionValue>();
                foreach (string snMemeber in sensorNetworks.Value)
                {
                    userID = snMemeber;
                    //if(dmViews.ContainsKey(userID))
                    if (dmOwnedObjects.ContainsKey(userID))
                    {
                        //foreach(string objectsID in dmViews[userID].GetObjectKeys())
                        foreach (string objectsID in dmOwnedObjects[userID])
                        {
                            if (allObjectViews.ContainsKey(objectsID))
                            {
                                if (allObjectViews[objectsID].Count == 0)
                                {//This occurs when the object has no changes in its view. 
                                    if (!singleSensorNetworkView.ContainsKey(objectsID))
                                    {
                                        singleSensorNetworkView.Add(objectsID, new AttributeCollectionValue());
                                    }
                                    //MergeTwoAttributeCollections(ref singleSensorNetworkView[objectsID], new AttributeCollectionValue());
                                }
                                else
                                    if (allObjectViews[objectsID].Count > 0)
                                    {
                                        foreach (KeyValuePair<string, AttributeCollectionValue> objectView in allObjectViews[objectsID])
                                        {
                                            if (singleSensorNetworkView.ContainsKey(objectView.Key))
                                            {
                                                AttributeCollectionValue tempACV = singleSensorNetworkView[objectView.Key];
                                                MergeTwoAttributeCollections(ref tempACV, objectView.Value);
                                            }
                                            else
                                            {
                                                singleSensorNetworkView.Add(objectView.Key, objectView.Value);
                                            }
                                        }
                                    }
                            }
                        }
                    }
                }//end foreach member
                sensorNetworkViews.Add(sensorNetworks.Key, singleSensorNetworkView);
            }//end foreach network
        }
        /// <summary>
        /// This method takes the sensor network views, and finds each DM's best view.
        /// </summary>
        /// <param name="dmViews"></param>
        /// <param name="sensorNetworkViews"></param>
        private void RetrieveAllDMViews(ref Dictionary<string, Dictionary<string, AttributeCollectionValue>> allDmViews,
                                        ref Dictionary<string, Dictionary<string, AttributeCollectionValue>> sensorNetworkViews)
        {
            string userID;
            Dictionary<string, AttributeCollectionValue> singleDMView;

            foreach (KeyValuePair<string, List<string>> networks in networkRosters)
            {
                foreach (string dmName in networks.Value)
                {
                    userID = dmName;
                    singleDMView = new Dictionary<string, AttributeCollectionValue>();

                    foreach (string objectID in sensorNetworkViews[networks.Key].Keys)
                    {
                        if (singleDMView.ContainsKey(objectID))
                        {
                            AttributeCollectionValue tempACV = singleDMView[objectID];
                            MergeTwoAttributeCollections(ref tempACV, sensorNetworkViews[networks.Key][objectID]);
                        }
                        else
                        {
                            singleDMView.Add(objectID, sensorNetworkViews[networks.Key][objectID]);
                        }
                    }

                    if (allDmViews.ContainsKey(userID))
                    {//need to merge 
                        foreach (string objectID in singleDMView.Keys)
                        {
                            if (allDmViews[userID].ContainsKey(objectID))
                            {
                                AttributeCollectionValue tempACV = allDmViews[userID][objectID];
                                MergeTwoAttributeCollections(ref tempACV, singleDMView[objectID]);
                            }
                            else
                            {
                                allDmViews[userID].Add(objectID, singleDMView[objectID]);
                            }
                        }
                    }
                    else
                    {
                        allDmViews.Add(userID, singleDMView);
                    }
                }//end foreach member
            }//end foreach network
        }
        /// <summary>
        /// This method retrieves all sensable attributes from the target proxy into the referenced attribute collection value.
        /// </summary>
        /// <param name="ACV">Current collection to add attributes into.</param>
        /// <param name="targetProxy">SimulationProxy to retrieve attributes from.</param>
        private void SenseAllAttributes(ref AttributeCollectionValue singleObjectACV, SimulationObjectProxy targetProxy)
        {
            //for the target object, go through each observable attribute, and add to the view with full confidence.
            string targetObjectID = ((StringValue)targetProxy["ID"].GetDataValue()).value;
            string objectType = targetProxy.GetObjectType();
            DetectedAttributeValue dav;
            List<string> keys = targetProxy.GetKeys();
            foreach (string att in simModel.objectModel.objects[objectType].attributes.Keys)
            {
                if (simModel.objectModel.objects[objectType].attributes[att].otherObservable)
                {
                    if (keys.Contains(att))
                    {
                        dav = new DetectedAttributeValue();

                        if (att == "CustomAttributes")
                        {
                            DataValue t = targetProxy[att].GetDataValue();
                            if (t.dataType == "CustomAttributesType")
                            {
                                Dictionary<string, DataValue> copiedDict = CopyFromCustomAttributes(((CustomAttributesValue)t).attributes);
                                t = new CustomAttributesValue();
                                ((CustomAttributesValue)t).attributes = copiedDict;
                            }

                            dav = new DetectedAttributeValue();
                            dav.stdDev = 100;
                            dav.value = DataValueFactory.BuildFromDataValue(t);
                            //AddAttributeToACV(ref singleAttributeCollection, simModelAtt.Key, detectedAttribute);
                        }
                        else
                        {
                            dav.value = DataValueFactory.BuildFromDataValue(targetProxy[att].GetDataValue());
                            dav.stdDev = 100;
                        }
                        if (((AttributeCollectionValue)singleObjectACV).attributes.ContainsKey(att))
                        {
                            ((AttributeCollectionValue)singleObjectACV)[att] = dav;
                        }
                        else
                        {
                            ((AttributeCollectionValue)singleObjectACV).attributes.Add(att, dav);
                        }
                    }
                }
            }
        }

        #endregion

        //private void AssignUnassignedDMs()
        //{
        //    foreach (string dm in singletonDMs)
        //    {
        //        if (!networkRosters.ContainsKey(dm))
        //        {
        //            networkRosters.Add(dm, new List<string>());
        //        }
        //        networkRosters[dm].Add(dm);
        //    }
        //}
    }
}
