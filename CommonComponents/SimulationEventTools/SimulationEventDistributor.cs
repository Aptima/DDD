using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimulationEventTools
{
    /// <summary>
    /// A DDD Server internal object.
    /// </summary>
    /// <exclude/>
    public class SimulationEventDistributor
    {
        private System.Object distributorLock;
        private Dictionary<int, SimulationEventDistributorClient> clients;
        private Dictionary<int, List<string>> subscriptions;
        private List<string> eventTypes;

        private System.Object incomingListLock;
        private List<SimulationEvent> incomingList;
        private System.Object incomingLock;
        private System.Object outgoingLock;
        private bool incoming;
        private int currentTimeTick = 0;

        public List<SimulationEvent> viewProBackChannelEvents;

        public SimulationEventDistributor(ref SimulationModelInfo model)
        {
            viewProBackChannelEvents = new List<SimulationEvent>();
            distributorLock = new object();
            
            lock (distributorLock)
            {
                clients = new Dictionary<int, SimulationEventDistributorClient>();
                subscriptions = new Dictionary<int, List<string>>();
                eventTypes = new List<string>();
            }
            foreach (string key in model.eventModel.events.Keys)
            {
                eventTypes.Add(key);
            }
            incomingList = new List<SimulationEvent>();
            incomingListLock = new object();
            incoming = true;
            incomingLock = new object();
            outgoingLock = new object();
        }

        public void RegisterClient(ref SimulationEventDistributorClient client)
        {
            lock (distributorLock)
            {
                int newid = 0;
                while (clients.ContainsKey(newid))
                {
                    newid++;
                }
                client.id = newid;
                client.dist = this;

                clients[newid] = client;
                subscriptions[newid] = new List<string>();
            }
        }

        public void PutEvent(SimulationEvent e)
        {
            lock (distributorLock)
            {
                if (e.eventType == "TimeTick")
                {
                    currentTimeTick = ((IntegerValue)e["Time"]).value;
                }

                e["Time"] = DataValueFactory.BuildInteger(currentTimeTick);

                if (incoming == true)
                {
                    AddToOutgoingQueues(e);
                }
                else
                {
                    AddToIncomingQueue(e);
                }
            }
        }

        private void AddToOutgoingQueues(SimulationEvent e)
        {
            //lock (outgoingLock)
            //{
            foreach (SimulationEventDistributorClient c in clients.Values)
            {
                if (subscriptions[c.id].Contains("ALL") || subscriptions[c.id].Contains(e.eventType))
                {

                    c.EventFromDistributor(e);
                }
            }
            //}
        }
        private void AddToIncomingQueue(SimulationEvent e)
        {
            lock (incomingListLock)
            {
                incomingList.Add(e);
            }
        }

        public void StopIncoming()
        {
            lock (incomingLock)
            {
                incoming = false;
            }
        }

        public void ResumeIncoming()
        {
            lock (incomingLock)
            {
                incoming = true;
            }
            lock (incomingListLock)
            {
                foreach (SimulationEvent e in incomingList)
                {
                    AddToOutgoingQueues(e);
                }
                incomingList.Clear();
            }
        }

        public void ClientSubscribe(int clientID, string eventName)
        {
            lock (distributorLock)
            {
                if (eventTypes.Contains(eventName))
                {
                    if (!subscriptions[clientID].Contains(eventName))
                    {
                        subscriptions[clientID].Add(eventName);
                    }
                }
                else if (eventName == "ALL")
                {
                    subscriptions[clientID].Add("ALL");
                }
                else
                {
                    throw new Exception("Error: event type '"+eventName+"'  doesn't exist");
                }
            }
        }
        public void RemoveClient(int clientID)
        {
            lock (distributorLock)
            {
                clients.Remove(clientID);
                subscriptions.Remove(clientID);
            }
        }

    }

    public interface IEventClient
    {
        void Subscribe(string eventName);
        List<SimulationEvent> GetEvents();
        void PutEvent(SimulationEvent e);
        void Disconnect();
    }

    /// <summary>
    /// A DDD Server internal object.
    /// </summary>
    /// <exclude/>
    public class SimulationEventDistributorClient : IEventClient
    {
        public int id;
        private List<SimulationEvent> queue;
        private System.Object queueLock;
        
        public SimulationEventDistributor dist;
        public SimulationEventDistributorClient()
        {
            queueLock = new object();
            queue = new List<SimulationEvent>();
            id = -1;
            dist = null;
        }
        public void Subscribe(string eventName)
        {
            dist.ClientSubscribe(id, eventName);
        }

        public void EventFromDistributor(SimulationEvent e)
        {
            lock (queueLock)
            {
                queue.Add(e);
            }
        }

        public List<SimulationEvent> GetEvents()
        {
            System.Threading.Thread.Sleep(0); // just necessary for stepping through in debugger
            List<SimulationEvent> results = null;
            lock (queueLock)
            {
                results = queue;
                queue = new List<SimulationEvent>();
            }
            return results;
        }
        public void PutEvent(SimulationEvent e)
        {
            dist.PutEvent(e);
        }
        public void Disconnect()
        {
            dist.RemoveClient(id);
        }
    }
}
