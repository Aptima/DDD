using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
using System.Windows.Forms;
namespace Aptima.Asim.DDD.CommonComponents.TextChatServer
{
    public class TextChatServer
    {
        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static int currentTick = 0;
        private static string timeString = "00:00:00";
        private static bool isRunning;
        private static Dictionary<string, List<string>> roomMembership;

        public static void IsRunning(bool value)
        {
            isRunning = value;
        }

        public TextChatServer(string simModelPath, ref SimulationEventDistributor distributor)
        {
            simModelName = simModelPath;
            simModelInfo = smr.readModel(simModelName);
            isRunning = false;
            roomMembership = new Dictionary<string, List<string>>();
            server = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref server);
            
        }

        public void ClearTextServerVariables()
        {
            simModelName = String.Empty;
            simModelInfo = null;
            roomMembership = new Dictionary<string, List<string>>();
        }

        private static void SendTextChat(string userID, string targetUserID, string chatBody, int time)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "TextChat");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["ChatBody"]).value = chatBody;
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            ((IntegerValue)sendingEvent["Time"]).value = time;
            server.PutEvent(sendingEvent);        
        }
        private void SendSystemErrorMessage(string text, string playerID)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "SystemMessage");
            e["Message"] = DataValueFactory.BuildString(text);
            e["TextColor"] = DataValueFactory.BuildInteger(-65536);//(System.Drawing.Color.Red.ToArgb());
            e["PlayerID"] = DataValueFactory.BuildString(playerID);
            server.PutEvent(e);
        }
        public void StartTextChatServer()
        {
            try
            {
                List<SimulationEvent> incomingEvents = new List<SimulationEvent>();

                server.Subscribe("TextChatRequest");
                server.Subscribe("TimeTick");
                server.Subscribe("CreateChatRoom");
                server.Subscribe("AddToChatRoom");
                server.Subscribe("CloseChatRoom");

                isRunning = true; //This can be set false elsewhere, and end the loop.
                while (isRunning)
                {
                    incomingEvents = server.GetEvents();

                    if (incomingEvents.Count != 0)
                    {
                        foreach (SimulationEvent se in incomingEvents)
                        {
                            switch (se.eventType)
                            { 
                                case "TextChatRequest":
                                    TextChatRequest(se);
                                    break;
                                case "TimeTick":
                                    currentTick = ((IntegerValue)se["Time"]).value;
                                    timeString = ((StringValue)se["SimulationTime"]).value;
                                    break;
                                case "CreateChatRoom":
                                    CreateChatRoom(se);
                                    break;
                                case "CloseChatRoom":
                                    CloseChatRoom(se);
                                    break;
                                case "AddToChatRoom":
                                    AddToChatRoom(se);
                                    break;
                                default:
                                    break;
                            }
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
        private void TextChatRequest(SimulationEvent e)
        {
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetRoom = ((StringValue)e["TargetUserID"]).value;
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (requestingID != "EXP" && !roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending text chat: You do not have permission to chat in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            string message = String.Format("({0}) {1}: {2}", timeString, requestingID, ((StringValue)e["ChatBody"]).value);

            SendTextChat(requestingID, targetRoom, message, currentTick);
        }

        private void NewObject(SimulationEvent e)
        {
            string dmID = ((StringValue)e["ID"]).value;

            if (((StringValue)e["ObjectType"]).value == "DecisionMaker")
            { 
                
            }
        }

        private void CreateChatRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            List<string> membership = ((StringListValue)e["MembershipList"]).strings;
            if (!roomMembership.ContainsKey(roomName))
            {
                roomMembership.Add(roomName, new List<string>());
            }
            roomMembership[roomName] = membership;

        }

        private void CloseChatRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            if (roomMembership.ContainsKey(roomName))
            {
                roomMembership.Remove(roomName);
            }
        }

        private void AddToChatRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            if (!roomMembership.ContainsKey(roomName))
            {
                return;
            }

            string playerID = ((StringValue)e["TargetPlayerID"]).value;

            if (roomMembership[roomName].Contains(playerID))
            {
                return;
            }

            roomMembership[roomName].Add(playerID);
        }
    }
}
