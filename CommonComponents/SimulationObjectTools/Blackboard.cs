using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;


namespace Aptima.Asim.DDD.CommonComponents.SimulationObjectTools
{
    class BBObjectInfo
    {
        public string id;
        public SimulationObject simObject;

        public BBObjectInfo(SimulationObject ob)
        {
            simObject = ob;
            id = ((StringValue)ob["ID"]).value;
        }
    }

    class SubscribeInfo
    {
        public string objectName;
        public string attributeName;
        public bool subscribe;
        public bool publish;
        public SubscribeInfo()
        {
            objectName = null;
            attributeName = null;
            subscribe = false;
            publish = false;
        }
    }
    public class Blackboard
    {
        internal Dictionary<int, BlackboardClient> clients;
        internal Dictionary<int, Dictionary<string, SubscribeInfo>> subscriptions;
        internal List<string> objectAttributes;
        internal Dictionary<string, BBObjectInfo> objects;
        internal SimulationModelInfo simModel;

        internal System.Object blackboardLock;
        public int simTime;
        
        public Blackboard(ref SimulationModelInfo simModelInfo)
        {
            this.simTime = 0;
            blackboardLock = new object();
            simModel = simModelInfo;
            objects = new Dictionary<string, BBObjectInfo>();
            subscriptions = new Dictionary<int, Dictionary<string, SubscribeInfo>>();
            clients = new Dictionary<int, BlackboardClient>();
            objectAttributes = new List<string>();
            foreach (string obkey in simModelInfo.objectModel.objects.Keys)
            {
                foreach (string attkey in simModelInfo.objectModel.objects[obkey].attributes.Keys)
                {
                    objectAttributes.Add(obkey+"."+attkey);
                }
            }
        }
        public void AddObject(SimulationObject ob)
        {
            lock (blackboardLock)
            {
                BBObjectInfo bbob = new BBObjectInfo(ob);
                if (objects.ContainsKey(bbob.id))
                {
                    throw new Exception("Blackboard:  Error, trying to add object that already exists!");
                }
                else
                {
                    objects[bbob.id] = bbob;
                }
            }

        }

        public void ClearObjects()
        {
            lock (blackboardLock)
            {
                objects.Clear();
            }
        }

        

        

        public SimulationObject this[string key]
        {
            get
            {
                SimulationObject ob;


                ob = objects[key].simObject;
                

                return ob;
            }
            
        }
        
        public void RegisterClient(ref BlackboardClient client)
        {
            int newid = 0;
            while (clients.ContainsKey(newid))
            {
                newid++;
            }
            client.id = newid;
            client.blackboard = this;
            clients[newid] = client;
            subscriptions[newid] = new Dictionary<string, SubscribeInfo>();
        }
        public void ClientSubscribe(int clientID, string objectName, string attributeName, bool subscribe, bool publish)
        {
            string obattname = null;

            foreach (string kind in simModel.objectModel.objects.Keys)
            {
                if (simModel.objectModel.IsDerivedFrom(kind, objectName))
                {
                    obattname = kind + "." + attributeName;
                    if (objectAttributes.Contains(obattname))
                    {
                        if (!subscriptions[clientID].ContainsKey(obattname))
                        {
                            SubscribeInfo i = new SubscribeInfo();
                            i.objectName = objectName;
                            i.attributeName = attributeName;
                            i.subscribe = subscribe;
                            i.publish = publish;

                            if (publish)
                            {
                                // make sure nobody else already publishes this object/attribute combo
                                foreach (Dictionary<string, SubscribeInfo> info in subscriptions.Values)
                                {
                                    if (info.ContainsKey(obattname))
                                    {
                                        if (info[obattname].publish)
                                        {
                                            throw new Exception("Error: someone already publishes: " + obattname);
                                        }
                                    }
                                }
                            }

                            subscriptions[clientID][obattname] = i;
                        }
                        else
                        {
                            throw new Exception("Error: client already subscribed");
                        }
                    }
                    else
                    {
                        throw new Exception("Error: object attribute combination doesn't exist");
                    }
                }
            }
            /*
            obattname = objectName + "." + attributeName;
            if (objectAttributes.Contains(obattname))
            {
                if (!subscriptions[clientID].ContainsKey(obattname))
                {
                    SubscribeInfo i = new SubscribeInfo();
                    i.objectName = objectName;
                    i.attributeName = attributeName;
                    i.subscribe = subscribe;
                    i.publish = publish;

                    if (publish)
                    {
                        // make sure nobody else already publishes this object/attribute combo
                        foreach (Dictionary<string, SubscribeInfo> info in subscriptions.Values)
                        {
                            if (info.ContainsKey(obattname))
                            {
                                if (info[obattname].publish)
                                {
                                    throw new Exception("Error: someone already publishes: " + obattname);
                                }
                            }
                        }
                    }

                    subscriptions[clientID][obattname] = i;
                }
                else
                {
                    throw new Exception("Error: client already subscribed");
                }
            }
            else
            {
                throw new Exception("Error: object attribute combination doesn't exist");
            }
             */
        }

        public bool CanClientPublishAttribute(int clientId, string attribute)
        {
            if (!subscriptions.ContainsKey(clientId))
                return false;
            if (!subscriptions[clientId].ContainsKey(attribute))
                return false;

            return subscriptions[clientId][attribute].publish;
        }

    }



    public class DataValueProxy
    {
        private Blackboard blackboard;
        private string objectID;
        private string attributeName;
        private bool publish;
        public DataValueProxy(ref Blackboard bboard, string objectID, string attName,bool publish)
        {
            this.blackboard = bboard;
            this.objectID = objectID;
            this.attributeName = attName;
            this.publish = publish;
        }
        public DataValue GetDataValue()
        {
            DataValue dv;

            lock (blackboard.blackboardLock)
            {
                dv = blackboard[objectID][attributeName];
            }
            return dv;
        }
        public void SetDataValue(DataValue dv)
        {
            lock (blackboard.blackboardLock)
            {
                if (publish)
                {
                    blackboard[objectID][attributeName] = dv;
                    ObjectLog.Write(blackboard.simTime, objectID, attributeName, DataValueFactory.XMLSerialize(dv));
                }
                else
                {
                    throw new Exception("Error: Simulator tried to modify attribute it doesn't own.");
                }
            }
        }
        public bool IsOwner()
        {
            return publish;
        }
    }



    public class SimulationObjectProxy
    {
        private Dictionary<string,DataValueProxy> attributes;
        private string objectType;
        public SimulationObjectProxy(string objectType)
        {
            attributes = new Dictionary<string, DataValueProxy>();
            this.objectType = objectType;
        }
        public string GetObjectType()
        {
            return objectType;
        }
        public int Count()
        {
            return attributes.Keys.Count;
        }
        public DataValueProxy this[string key]
        {
            get
            {
                return attributes[key];
            }
            set
            {
                attributes[key] = value;
            }
        }
        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();

            foreach (string k in attributes.Keys)
            {
                keys.Add(k);
            }

            return keys;
        }
        

    }

    public class BlackboardClient
    {
        public Blackboard blackboard;
        public int id;
        public BlackboardClient()
        {
            blackboard = null;
            id = 0;
        }
        public void Subscribe(string objectName, string attributeName, bool subscribe, bool publish)
        {
            blackboard.ClientSubscribe(id, objectName,attributeName,subscribe,publish);
        }

        public SimulationObjectProxy GetObjectProxy(string objectID)
        {
            SimulationObjectProxy proxy = null;
            string key = null;
            DataValueProxy dvProx = null;

            lock (blackboard.blackboardLock)
            {
                BBObjectInfo ob = blackboard.objects[objectID];
                proxy = new SimulationObjectProxy(ob.simObject.objectType);
                foreach (string attname in ob.simObject.attributes.Keys)
                {
                    //key = ob.simObject.objectType + "." + attname;
                    key = String.Format("{0}.{1}", ob.simObject.objectType, attname);
                    if (blackboard.subscriptions[id].ContainsKey(key))
                    {
                        dvProx = new DataValueProxy(ref blackboard, ob.id, attname, blackboard.subscriptions[id][key].publish);
                        proxy[attname] = dvProx;
                    }
                }
                if (proxy.Count() == 0)
                {
                    proxy = null;
                }
            }

            return proxy;
        }

        public Dictionary<string, SimulationObjectProxy> GetObjectProxies()
        {
            Dictionary<string, SimulationObjectProxy> proxies = new Dictionary<string, SimulationObjectProxy>();

            string key = null;
            SimulationObjectProxy obProx = null;
            DataValueProxy dvProx = null;
            lock (blackboard.blackboardLock)
            {
                foreach (BBObjectInfo ob in blackboard.objects.Values)
                {
                    obProx = new SimulationObjectProxy(ob.simObject.objectType);
                    foreach (string attname in ob.simObject.attributes.Keys)
                    {
                        //key = ob.simObject.objectType + "." + attname;
                        key = String.Format("{0}.{1}", ob.simObject.objectType, attname);
                        if (blackboard.subscriptions[id].ContainsKey(key))
                        {
                            dvProx = new DataValueProxy(ref blackboard, ob.id, attname, blackboard.subscriptions[id][key].publish);
                            obProx[attname] = dvProx;
                        }
                    }
                    if (obProx.Count() > 0)
                    {
                        proxies[ob.id] = obProx;
                    }
                }
            }
            return proxies;
        }

        public bool CanPublishAttribute(string attribute)
        {
            return blackboard.CanClientPublishAttribute(this.id, attribute);
        }
    }
}
