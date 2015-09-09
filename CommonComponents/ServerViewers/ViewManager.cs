using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.HandshakeManager;
using Aptima.Asim.DDD.CommonComponents.AuthenticationManager;
using Aptima.Asim.DDD.ScenarioController;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using System.Windows.Forms;
namespace Aptima.Asim.DDD.CommonComponents.ServerViewers
{
    /// <summary>
    /// This class will ultimately act in a similar fashion to the SimCore, in that it will
    /// manage a set of objects by sending each object the correct event information.
    /// It will loop, receive all of the most recent events, and depending on which
    /// object utilizes that event, send that object the event.  This will reduce the number
    /// of threads running and number of connections to the event distributor.
    /// </summary>
    public class ViewManager
    {

        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static TextChatStreamViewer textChat;
        private static string simModelName;
        private static bool isRunning = true;
        NetworkConnectionViewer networkConnectionViewer;
        EventStreamViewer eventStreamViewer;
        public HandshakeManager.HandshakeManager handshakeManager;
        AuthenticationManager.AuthenticationManager authenticationManager;
        //private static List<string> listOfDMs = new List<string>();
        //public void SetListOfDMs(List<string> theList)
        //{
        //    foreach (string dm in theList)
        //    {
        //        if (!listOfDMs.Contains(dm))
        //        {
        //            listOfDMs.Add(dm);
        //        }
        //    }
        //    HandshakeManager.HandshakeManager.AvailableDMs = listOfDMs;
        //}
        //public List<string> GetListOfDMs()
        //{
        //    return listOfDMs;
        //}


        public ViewManager(string simModelPath, ref SimulationEventDistributor distributor, int numberOfSeats)
        {
            simModelName = simModelPath;
            simModelInfo = smr.readModel(simModelName);
            server = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref server);

            //try
            //{
            //    server.Connect(hostName, Convert.ToInt32(portNumber));
            //}
            //catch
            //{
            //    throw new Exception("View Manager cannot connect to Network Server...");
            //}
            //networkConnectionViewer = new NetworkConnectionViewer();
            //eventStreamViewer = new EventStreamViewer();
            handshakeManager = new HandshakeManager.HandshakeManager(ref server, ref simModelInfo);
            authenticationManager = new AuthenticationManager.AuthenticationManager(ref server, ref simModelInfo, numberOfSeats);
            textChat = new TextChatStreamViewer();
            isRunning = true;
            Thread.Sleep(200);
            //HandshakeManager.HandshakeManager.AvailableDMs = listOfDMs;
        }
        public void StartViewManagerLoop()
        {
            try
            {
                List<SimulationEvent> incomingEvents;

                foreach (string e in simModelInfo.eventModel.events.Keys)
                {
                    server.Subscribe(e);
                }

                while (isRunning)
                {
                    incomingEvents = server.GetEvents();

                    foreach (SimulationEvent e in incomingEvents)
                    {
                        //eventStreamViewer.AddEvent(e.eventType, ((IntegerValue)e.parameters["Time"]).value);
                        switch (e.eventType)
                        {//this should eventually just send handshake manager the event, and HM will determine
                            //how to handle it.
                            case "AuthenticationRequest":
                                authenticationManager.AuthenticationRequest(e);
                                break;
                            case "HandshakeGUIRegister":
                                handshakeManager.GUIRegister(e);
                                break;
                            case "HandshakeGUIRoleRequest":
                                handshakeManager.GUIRoleRequest(e);
                                break;
                            case "HandshakeInitializeGUIDone":
                                handshakeManager.InitializeGUIDone(e);

                                authenticationManager.IncrementUsers();
                                break;
                            case "Playfield":
                                handshakeManager.SetPlayfieldInformation(e);
                                break;
                            //case "TextChat":
                            //    textChat.AddTextMessage(e);
                            //    break;
                            case "DisconnectDecisionMaker":
                                handshakeManager.DisconnectPlayer(e);
                                if(((StringValue)e["DecisionMakerID"]).value != string.Empty)
                                    authenticationManager.DecrementUsers(); //TODO: See if this is called as well as below
                                break;
                            case "DisconnectTerminal":
                                handshakeManager.DisconnectTerminal(e);
                                //authenticationManager.DecrementUsers();  //TODO: See if this is called as well as above
                                break;
                            case "NewObject":
                                if (((StringValue)e["ObjectType"]).value == "DecisionMaker")
                                    handshakeManager.ReceiveDecisionMakerEvent(e);
                                break;
                            case "CreateChatRoom":
                                handshakeManager.CreateChatRoom(e);
                                break;
                            case "CloseChatRoom":
                                handshakeManager.CloseChatRoom(e);
                                break;

                            case "CreateVoiceChannel":
                                handshakeManager.CreateVoiceChannel(e);
                                break;
                            case "CloseVoiceChannel":
                                handshakeManager.CloseVoiceChannel(e);
                                break;
                            case "CreateWhiteboardRoom":
                                handshakeManager.CreateWhiteboardRoom(e);
                                break;
                            case "ClientSideAssetTransferAllowed":
                                handshakeManager.SetAssetTransferFlag(e);
                                break;
                            case "StopScenario":
                                authenticationManager.ResetUserCount();
                                break;
                            default:
                                break;
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception exc)
            {
                MessageBox.Show("An error '" + exc.Message + "' has occurred in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                ErrorLog.Write(exc.ToString() + "\n");
                throw new Exception();
            }
        }
        public void ResetViewManager()
        {
            if (handshakeManager != null)
            {
                handshakeManager.ResetHandshakeManager();
            }
            if (textChat != null)
            {
                textChat.ResetTextStreamViewer();
            }
        }
        public string GetDMsAvail(string dm)
        {
            if (handshakeManager != null)
                return handshakeManager.GetDMsAvail(dm);

            return string.Empty;
        }
        public void StopViewManager()
        {
            isRunning = false;
        }
        public string GetEventStream()
        {
            string eventStream = eventStreamViewer.GetStream();
            string returnStream = string.Empty;
            returnStream = eventStream;
            return returnStream;
        }
        public string[] GetNetworkConnections()
        {
            string[] connections = { string.Empty };

            return connections;
        }
        public string GetTextChatStream()
        {
            string textChatStream = textChat.RetrieveTextStream();
            return textChatStream;
        }
    }
}
