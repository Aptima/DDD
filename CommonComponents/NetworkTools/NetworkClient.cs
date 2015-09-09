using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;

namespace Aptima.Asim.DDD.CommonComponents.NetworkTools
{
    public class NetworkClient : IEventClient
    {

        private List<SimulationEvent> eventQueue;
        private System.Object eventQueueLock;

        private TcpClient client;
        private Thread clientThread;
        private NetworkStream netStream;
        //private Byte[] recBuffer;
        public Int32 clientID;
        private bool isConnected;
        private String terminalID;
        public String TerminalID
        {
            get { return terminalID; }
        }
        /// <summary>
        /// The NetworkClient should be instantiated once per client application.
        /// It is used to subscribe to and receive events from the DDD Server.
        /// It is also used to send events to the DDD Server.
        /// </summary>
        public NetworkClient()
        {
            isConnected = false;
            eventQueue = new List<SimulationEvent>();
            eventQueueLock = new object();
            client = null;
            clientThread = null;
            netStream = null;
            //recBuffer = new Byte[256];
            clientID = -1;
            terminalID = System.Environment.MachineName + DateTime.Now.Ticks.ToString();
        }
        /// <summary>
        /// This method should be called to determine if the NetworkClient object is connected
        /// to the DDD Server.  If for some reason the connection is lost, this will return false.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return isConnected;
        }

        private void SocketHandler()
        {

            NetMessage m = new NetMessage();
            SimulationEvent e = null;
            while (true)
            {
                try
                {
                    m.Receive(ref netStream);
                    switch (m.type)
                    {
                        case NetMessageType.EVENT:
                            try
                            {
                                e = SimulationEventFactory.XMLDeserialize(m.msg);
                                lock (eventQueueLock)
                                {
                                    eventQueue.Add(e);
                                }
                            }
                            catch (Exception exc)
                            {
                                ErrorLog.Write(String.Format("NONFATAL Deserialize Error in NetworkClient: {0}", m.msg));
                                ErrorLog.Write(exc.ToString());
                            }
                            break;
                        case NetMessageType.DISCONNECT:
                            System.Console.WriteLine("NetworkClient: recieved Shutdown message from server.  Disconnecting...");
                            Disconnect();
                            return;
                        case NetMessageType.PING:
                            break;
                        case NetMessageType.NONE:
                            
                            System.Console.WriteLine("NetworkClient: Message with no type received.  Disconnecting.");
                            Disconnect();
                            return;
                        default:
                            throw new Exception("NetworkClient: recieved unhandled message");
                    }
                }
                catch (System.IO.IOException)
                {
                    System.Console.WriteLine("NetworkClient: Lost connection with remote server!");
                    netStream.Close();
                    netStream.Dispose();
                    isConnected = false;
                    clientThread.Abort();
                    return;
                }
            }
        }
        /// <summary>
        /// Disconnect from the DDD Server.
        /// This should always be called before a client application closes 
        /// in order to gracefully disconnect from the server.
        /// </summary>
        public void Disconnect()
        {
            if (isConnected)
            {
                
                NetMessage m = new NetMessage();
                m.type = NetMessageType.DISCONNECT;
                m.Send(ref netStream,TerminalID);
                
                netStream.Close();
                netStream.Dispose();
                isConnected = false;
                clientThread.Abort();
                //clientThread.Suspend();
            }

        }
        /// <summary>
        /// This method is used to connect to the DDD Server.
        /// </summary>
        /// <param name="servername">The hostname of the machine the DDD Server is running on</param>
        /// <param name="port">The port number that the DDD Server is listening on</param>
        /// <returns>boolean connect success</returns>
        public bool Connect(string servername, int port)
        {
            try
            {
                if (!isConnected)
                {
                    client = new TcpClient(servername, port);
                    netStream = client.GetStream();
                    NetMessage m = new NetMessage();
                    m.type = NetMessageType.REGISTER;
                    m.Send(ref netStream, TerminalID);
                    m.Receive(ref netStream);
                    clientID = m.clientID;


                    clientThread = new Thread(new ThreadStart(SocketHandler));
                    clientThread.Start();
                    isConnected = true;
                    return true;
                }
                else
                {
                    throw new Exception("NetworkClient: already connected");
                }
            }
            catch (System.Net.Sockets.SocketException e)
            {
                System.Console.WriteLine(e.ToString());
                System.Console.WriteLine(e.Message);
                return false;

            }

        }
        /// <summary>
        /// This method is used to send an event to the DDD Server.
        /// </summary>
        /// <param name="e">The event</param>
        public void PutEvent(SimulationEvent e)
        {
            if (isConnected)
            {
                NetMessage m = new NetMessage();
                m.type = NetMessageType.EVENT;
                try
                {
                    m.msg = SimulationEventFactory.XMLSerialize(e);
                }
                catch (Exception exc)
                {
                    ErrorLog.Write(String.Format("NONFATAL Serialize Error in NetworkClient: {0}", e.eventType));
                    ErrorLog.Write(exc.ToString());
                    return;
                }
                m.clientID = clientID;
                m.Send(ref netStream, TerminalID);
            }
        }


        /// <summary>
        /// The method is used to get all waiting events from the DDD Server.
        /// This should be called periodically to see if any events are waiting.
        /// </summary>
        /// <returns>A list of events</returns>
        public List<SimulationEvent> GetEvents()
        {
            List<SimulationEvent> result = null;
            System.Threading.Thread.Sleep(0); // just necessary for stepping through in debugger
            lock (eventQueueLock)
            {
                result = eventQueue;

                eventQueue = new List<SimulationEvent>();
            }
            return result;

        }
        /// <summary>
        /// This method is used to subscribe to event types.
        /// GetEvents will only return events that have been subscribed to with this method.
        /// </summary>
        /// <param name="eventName">The name of an event type that is defined in the simulation model</param>
        public void Subscribe(string eventName)
        {
            if (isConnected)
            {
                NetMessage m = new NetMessage();
                m.type = NetMessageType.SUBSCRIBE;
                m.msg = eventName;
                m.clientID = clientID;
                m.Send(ref netStream, TerminalID);
            }
            else
            {
                throw new Exception("NetworkClient: Must be connected first");
            }
        }

    }
}