using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.ObjectsAttributeCollection
{
    public class ObjectsAttributeCollection
    {
        Dictionary<string, AttributeCollectionValue> objectsList;

        public ObjectsAttributeCollection()
        {
            objectsList = new Dictionary<string, AttributeCollectionValue>();
        }
        /// <summary>
        /// Retrieves a list of all objects contained within this data structure
        /// </summary>
        /// <returns></returns>
        public List<string> GetObjectKeys()
        {
            List<string> returnList = new List<string>();
            foreach (string s in objectsList.Keys)
            {
                returnList.Add(s);
            }

            return returnList; 
        }
        public void GetObjectKeys(ref List<string> objectKeys)
        {
            foreach (string s in objectKeys)
            {
                if (!objectsList.ContainsKey(s))
                {
                    objectKeys.Remove(s);
                }
            }
            foreach (string ss in objectsList.Keys)
            {
                if (!objectKeys.Contains(ss))
                {
                    objectKeys.Add(ss);
                }
            }
        }
        public bool ContainsObject(string objectID)
        {
            if (objectsList.ContainsKey(objectID))
                return true;

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public AttributeCollectionValue GetObjectsAttributeCollection(string objectID)
        {
            if (objectsList.ContainsKey(objectID))
            {
                return objectsList[objectID];
            }

            return new AttributeCollectionValue();
        }
        /// <summary>
        /// Retrieves a list of all attributes contained in a specific object's attribute collection.
        /// </summary>
        /// <param name="objectID">The object to retrieve attribute names of.</param>
        /// <returns></returns>
        public List<string> GetObjectsAttributeKeys(string objectID)
        {
            List<string> returnList = new List<string>();
            foreach (string s in objectsList[objectID].attributes.Keys)
            {
                returnList.Add(s);
            }

            return returnList; 
        }
        /// <summary>
        /// Returns a specified object's attribute collection value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AttributeCollectionValue this[string key]
        {
            get
            {
                try
                {
                    return objectsList[key];
                }
                catch
                {
                    throw new Exception(String.Format("The unit {0} is not given in this collection", key));
                    //return new AttributeCollectionValue();
                }
            }

        }
        /// <summary>
        /// **This method should not be called by the client-side GUI.**
        /// This method toggles the visibility of all objects contained within this data structure.  Mainly
        /// used at the start of each time tick, toggling each object's visibility to false, and then switching
        /// back to true only if they are updated allows for only sensed objects to be displayed.
        /// </summary>
        /// <param name="visibility"></param>
        public void SetAllObjectsVisibility(bool visibility)
        {
            foreach (string objectID in objectsList.Keys)
            {
                if (!objectsList[objectID].attributes.ContainsKey("IsVisible"))
                { 
                    objectsList[objectID].attributes.Add("IsVisible", new BooleanValue());
                }
                ((BooleanValue)objectsList[objectID]["IsVisible"]).value = visibility;
            }
        }

        public void SetSpecificObjectVisibility(string objectID,bool visibility)
        {
            if (!objectsList[objectID].attributes.ContainsKey("IsVisible"))
            {
                objectsList[objectID].attributes.Add("IsVisible", new BooleanValue());
            }
            ((BooleanValue)objectsList[objectID]["IsVisible"]).value = visibility;
        }

        /// <summary>
        /// Returns a specified DataValue attribute for a given object.  If the attribute or object do not
        /// exist, then it returns null.  You can test the result versus a null value to determine if the
        /// attribute is known.
        /// </summary>
        /// <param name="objectID">The objectID of the object you are trying to retrieve info about.</param>
        /// <param name="attributeName">The attribute name, as according to the Simulation Model, to be
        /// retrived for the specified object.</param>
        /// <returns></returns>
        public DataValue GetObjectsAttribute(string objectID, string attributeName)
        {
            if (objectsList.ContainsKey(objectID))
            {
                if (objectsList[objectID].attributes.ContainsKey(attributeName))
                {
                    return objectsList[objectID][attributeName];
                }
            }
            
            return null;
        }

        private DataValue GetNestedDataValue(DataValue dv)
        {
            if (dv is DetectedAttributeValue)
            {
                return ((DetectedAttributeValue)dv).value;
            }

            return dv;
        }
        /// <summary>
        /// Given an object's unique ID, if this collection contains the object,
        /// the object will be removed, and this method will return true.  If
        /// the object is not in this collection, it will return false.
        /// </summary>
        /// <param name="objectID">An object's unique ID</param>
        /// <returns></returns>
        public bool RemoveObject(string objectID)
        {
            if (!objectsList.ContainsKey(objectID))
                return false;

            objectsList.Remove(objectID);

            return true;
        }
        /// <summary>
        /// This method takes in the updated attribute collection value for a specified object,
        /// and each attribute in the collection that differs from the pre-existing attribute value.
        /// New attributes that did not exist before and modified attributes are added to an ACV which
        /// is returned as a collection of modified attributes.
        /// </summary>
        /// <param name="objectID">The unique object identifier of the object.</param>
        /// <param name="ACV">The collection of updated attributes for the specified object.</param>
        /// <returns>AttributeCollectionValue: Returns a collection of attributes that were not in
        /// or were different from pre-existing attributes in this object.</returns>
        public AttributeCollectionValue UpdateObject(string objectID, AttributeCollectionValue ACV)
        { 
            AttributeCollectionValue returnACV = new AttributeCollectionValue();
            DataValue dv1;
            DataValue dv2;

            if (!objectsList.ContainsKey(objectID))
            { 
                objectsList.Add(objectID, new AttributeCollectionValue());
            }
            foreach (KeyValuePair<string, DataValue> kvp in ACV.attributes)
            {
                

                if (objectsList[objectID].attributes.ContainsKey(kvp.Key))
                {//this object already contains the attribute
                    dv1 = GetNestedDataValue(objectsList[objectID][kvp.Key]);
                    dv2 = GetNestedDataValue(ACV[kvp.Key]);
                    if (DataValueFactory.CompareDataValues(dv1, dv2) == true)
                    {
                        //no change required
                    }
                    else
                    {//Need to update data, add to return ACV. 
                        objectsList[objectID][kvp.Key] = ACV[kvp.Key];
                        returnACV.attributes.Add(kvp.Key, kvp.Value);
                    }
                }
                else
                { //this object does not already contain the attribute
                    objectsList[objectID].attributes.Add(kvp.Key, kvp.Value);
                    returnACV.attributes.Add(kvp.Key, kvp.Value);                
                }            
            }

            if (returnACV.attributes.Count == 0)
            {
                return null;
            }
            //returned objects need ID and OwnerID passed.
            if (!returnACV.attributes.ContainsKey("ID"))
            {
                returnACV.attributes.Add("ID", ACV["ID"]);
            }
            if (!returnACV.attributes.ContainsKey("OwnerID"))
            {
                returnACV.attributes.Add("OwnerID", ACV["OwnerID"]);
            }
            return returnACV;
        }
        /// <summary>
        /// Called by the GUI, updates attributes for a specified object without returning a new ACV, or
        /// creating more memory objects.
        /// </summary>
        /// <param name="ACV"></param>
        /// <param name="objectID"></param>
        public void UpdateObjectsAttributes(ref AttributeCollectionValue ACV, string objectID)
        {
            if (!objectsList.ContainsKey(objectID))
            {
                objectsList.Add(objectID, new AttributeCollectionValue());
            }
            foreach (KeyValuePair<string, DataValue> kvp in ACV.attributes)
            {
                if (objectsList[objectID].attributes.ContainsKey(kvp.Key))
                {//this object already contains the attribute 
                    objectsList[objectID][kvp.Key] = ACV[kvp.Key];                    
                }
                else
                { //this object does not already contain the attribute
                    objectsList[objectID].attributes.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
