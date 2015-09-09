using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using System.Windows.Forms;
namespace Aptima.Asim.DDD.CommonComponents.NetworkTools
{
    public class NetworkUtility
    {
        public static TcpListener GetTcpListener(String hostname, int port, out IPAddress ip)
        {
            IPAddress[] addresses = System.Net.Dns.GetHostAddresses(hostname);
            ip = addresses[0];
            TcpListener server = new TcpListener(IPAddress.Any, port);
            System.Console.WriteLine("Hostname: " + System.Net.Dns.GetHostName());
            System.Console.WriteLine("ip address: " + addresses[0].ToString());
            return server;
        }
    }
    /// <summary>
    /// The main DDD Server class.
    /// </summary>
    /// <exclude/>
    public class NetworkServer
    {
        enum ServerState
        {
            RUNNING,
            STOPPING
        }
        private ServerState state;
        private static TimeSpan waitTime = new TimeSpan(0, 0, 1);
        private Thread connectionListenerThread;
        //private Thread eventCheckerThread;
        private int port;
        private System.Object clientHandlersLock;
        private List<NetworkServerConnectionHandler> clientHandlers;

        private TcpListener server;
        //private Int32 clientCount;
        public SimulationEventDistributor eventDist;
        public IPAddress ServerIP
        {
            get { return m_serverIP; }
        }
        private IPAddress m_serverIP; 
        
        /// <summary>
        /// The NetworkServer will be instantiated once in the DDD Server.
        /// External applications such as agents and custom GUIs should ignore this class.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="dist"></param>
        public NetworkServer(int port, ref SimulationEventDistributor dist)
        {
            server = null;
            state = ServerState.STOPPING;
            eventDist = dist;
            m_serverIP = null;
            this.port = port;
            connectionListenerThread = null;
            //eventCheckerThread = null;
            clientHandlers = new List<NetworkServerConnectionHandler>();
            clientHandlersLock = new object();
            //clientCount = 0;
        }
        public void ListenForConnections()
        {
            state = ServerState.RUNNING;
            connectionListenerThread = new Thread(new ThreadStart(ConnectionListenerHandler));
            connectionListenerThread.Start();
            //eventCheckerThread = new Thread(new ThreadStart(CheckForEvents));
            //eventCheckerThread.Start();
        }
        public int ClientCount()
        {
            int x;

            lock (clientHandlersLock)
            {
                x = clientHandlers.Count;
            }

            return x;
        }

        public string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
        public List<String> GetConnectedTerminals()
        {
            List<String> terms = new List<string>();
            String term = null;
            foreach (NetworkServerConnectionHandler h in clientHandlers)
            {
                term = h.TerminalID;
                if (!terms.Contains(term))
                {
                    terms.Add(term);
                }
            }

            return terms;
        }
        private void ConnectionListenerHandler()
        {
            //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            try
            {

                server = NetworkUtility.GetTcpListener(System.Net.Dns.GetHostName(), port, out m_serverIP);
                //IPAddress[] addresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
                //server = new TcpListener(addresses[0], port);
                //System.Console.WriteLine("Hostname: " + System.Net.Dns.GetHostName());
                //System.Console.WriteLine("ip address: " + addresses[0].ToString());
                
                server.Start();

                TcpClient client = null;
                NetworkServerConnectionHandler ch = null;

                while (state == ServerState.RUNNING)
                {
                    System.Console.WriteLine("NetworkServer.ConnectionListenerHandler:listening for a connection...");
                    client = server.AcceptTcpClient();
                    
                    System.Console.WriteLine("NetworkServer.ConnectionListenerHandler:connection established...");
                    ch = new NetworkServerConnectionHandler(ref client);
                    ch.networkServer = this;

                    lock (clientHandlersLock)
                    {
                        clientHandlers.Add(ch);
                    }
                    ch.Start();
                }
                return;
            }
            catch (ThreadAbortException) { }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                ErrorLog.Write(exc.ToString() + "\n");
                throw new Exception();
            }
        }

        internal void RemoveClient(int clientID)
        {
            NetworkServerConnectionHandler found = null;
            lock (clientHandlersLock)
            {
                foreach (NetworkServerConnectionHandler ch in clientHandlers)
                {
                    if (ch.ClientID() == clientID)
                    {
                        found = ch;
                        break;
                    }
                }
            }
            if (found != null)
            {
                lock (clientHandlersLock)
                {
                    clientHandlers.Remove(found);
                    eventDist.RemoveClient(clientID);
                }
            }
        }

        public void Shutdown()
        {
            state = ServerState.STOPPING;
            Thread.Sleep(100);  //Wait for final events to come in and flush out
            lock (clientHandlersLock)
            {
                foreach (NetworkServerConnectionHandler ch in clientHandlers)
                {
                    ch.SendDisconnect();
                }
            }
            try
            {
                connectionListenerThread.Abort();
            }
            catch (Exception ex)
            {
                ErrorLog.Write("Error killing Connection Listener Thread.  Continuing Shutdown() process");
            }
            //eventCheckerThread.Abort();
            server.Stop();

            if (clientHandlers.Count > 0)
            {
                Thread.Sleep(1000);

                while (clientHandlers.Count > 0)
                {
                    clientHandlers[0].ForceClose();
                }
                //foreach (NetworkServerConnectionHandler ch in clientHandlers)
                //{
                //    ch.ForceClose();
                //}

            }
            
        }

        /*
        private void CheckForEvents()
        {
            List<NetworkServerConnectionHandler> forceClose = new List<NetworkServerConnectionHandler>();
            while (state == ServerState.RUNNING)
            {
                lock (clientHandlersLock)
                {
                    foreach (NetworkServerConnectionHandler ch in clientHandlers)
                    {
                        if (forceClose.Count > 0)
                        {
                            forceClose.Clear();
                        }
                        try
                        {
                            ch.SendEvents();
                        }
                        catch (System.IO.IOException exc)
                        {
                            forceClose.Add(ch);
                        }
                    }
                }

                foreach (NetworkServerConnectionHandler ch in forceClose)
                {
                    ch.ForceClose();

                }
                Thread.Sleep(10);
            }
            return;
        }
        */
        public Dictionary<int, string> GetClientDescriptions()
        {
            Dictionary<int, string> r = new Dictionary<int, string>();
            //int id;
            //string descr;
            lock (clientHandlersLock)
            {
                foreach (NetworkServerConnectionHandler ch in clientHandlers)
                {
                    if (ch.ClientID() >= 0)
                    {
                        r[ch.ClientID()] = ch.ClientDescription();
                    }
                }
            }
            return r;
        }

    }

    class NetworkServerConnectionHandler
    {
        private TcpClient client;
        private NetworkStream netStream;
        private Thread recThread;
        private Thread sendThread;
        public NetworkServer networkServer;
        private SimulationEventDistributorClient eventDistClient;
        private string m_terminalID;
        public String TerminalID
        {
            get { return m_terminalID; }
        }
        internal int ClientID()
        {
            if (eventDistClient == null)
            {
                return -1;
            }
            else
            {
                return eventDistClient.id;
            }
        }
        internal string ClientDescription()
        {
            return client.Client.RemoteEndPoint.ToString();
        }
        public NetworkServerConnectionHandler(ref TcpClient c)
        {
            networkServer = null;
            eventDistClient = null;
            client = c;
            recThread = null;
            sendThread = null;
            netStream = client.GetStream();
            
        }
        public void Start()
        {
            recThread = new Thread(new ThreadStart(ConnectionHandler));
            recThread.Start();

            sendThread = new Thread(new ThreadStart(SendEvents));
            sendThread.Start();
        }

        public void SendEvents()
        {
            NetMessage m = new NetMessage();
            List<SimulationEvent> events = null;
            int count = 0;
            while (true)
            {
                try
                {
                    if (eventDistClient != null)
                    {
                        events = eventDistClient.GetEvents();
                        foreach (SimulationEvent e in events)
                        {
                            m.type = NetMessageType.EVENT;
                            m.clientID = eventDistClient.id;
                            try
                            {
                                m.msg = SimulationEventFactory.XMLSerialize(e);
                            }
                            catch (Exception exc)
                            {
                                ErrorLog.Write(String.Format("NONFATAL Serialize Error in NetworkServer: {0}", e.eventType));
                                ErrorLog.Write(exc.ToString());
                                continue;
                            }
                            m.Send(ref netStream,String.Empty);
                        }
                        Thread.Sleep(50);
                        count += 1;

                        if (count >= 100)
                        {

                            count = 0;
                            m.type = NetMessageType.PING;
                            m.clientID = eventDistClient.id;
                            m.Send(ref netStream,String.Empty);
                        }
                    }
                }
                catch (System.IO.IOException exc)
                {
                    ForceClose();
                    return;
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("An error has occured in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                    ErrorLog.Write(exc.ToString() + "\n");
                    throw new Exception();
                }
            }
        }

        public void ForceClose()
        {
            netStream.Close(0);
            System.Console.WriteLine("NetworkServerConnectionHandler.ConnectionHandler:ForceClose");
            netStream.Dispose();
            

            SimulationEvent ev = new SimulationEvent();
            ev.eventType = "DisconnectTerminal";
            ev["Time"] = DataValueFactory.BuildInteger(0);
            ev["TerminalID"] = DataValueFactory.BuildString(m_terminalID);
            if (eventDistClient != null)
            {
                eventDistClient.PutEvent(ev);
                networkServer.RemoveClient(eventDistClient.id);
            }


            recThread.Abort();
            sendThread.Abort();
        }

        internal void SendDisconnect()
        {
            NetMessage m = new NetMessage();
            m.type = NetMessageType.DISCONNECT;
            m.clientID = eventDistClient.id;
            m.Send(ref netStream,String.Empty);
        }

        private void ConnectionHandler()
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
                        case NetMessageType.REGISTER:
                            eventDistClient = new SimulationEventDistributorClient();
                            networkServer.eventDist.RegisterClient(ref eventDistClient);
                            m_terminalID = m.TerminalID;
                            m.type = NetMessageType.REGISTER_RESPONSE;
                            //m.clientID = networkServer.RegisterClient();
                            m.clientID = eventDistClient.id;
                            m.Send(ref netStream, m.TerminalID);
                            break;
                        case NetMessageType.SUBSCRIBE:
                            eventDistClient.Subscribe(m.msg);
                            break;
                        case NetMessageType.EVENT:
                            try
                            {
                                e = SimulationEventFactory.XMLDeserialize(m.msg);
                                eventDistClient.PutEvent(e);
                            }
                            catch (Exception exc)
                            {
                                ErrorLog.Write(String.Format("NONFATAL Deserialize Error in NetworkServer: {0}", m.msg));
                                ErrorLog.Write(exc.ToString());
                            }
                            //networkServer.EventFromClient(e);
                            break;
                        case NetMessageType.DISCONNECT:
                            //netStream.Close(0);
                            System.Console.WriteLine("NetworkServerConnectionHandler.ConnectionHandler:connection closed");
                            //netStream.Dispose();
                            //networkServer.RemoveClient(eventDistClient.id);
                            sendThread.Abort();
                            ForceClose();
                            return;
                        case NetMessageType.NONE:
                            ErrorLog.Write(String.Format("NONFATAL Deserialize Error in NetworkServer: {0}", m.msg));
                            ErrorLog.Write(String.Format("TYPE: {0}; MSG: {1};",m.type, m.msg));
                            break;
                        default:
                            throw new Exception("connection handler got an invalid event");
                            

                    }
                }
                catch (System.IO.IOException exc)
                {
                    System.Console.WriteLine("NetworkServerConnectionHandler.ConnectionHandler:lost connection with client");
                    ForceClose();
                    return;
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("An error has occured in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                    ErrorLog.Write(exc.ToString() + "\n");
                    throw new Exception();
                }

            }
        }

    }
}