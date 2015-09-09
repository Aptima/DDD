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

namespace Aptima.Asim.DDD.CommonComponents.WhiteboardServer
{
    public class WhiteboardServer
    {
        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static int currentTick = 0;
        private static string timeString = "00:00:00";
        private static bool isRunning;
        private static Dictionary<string, List<string>> roomMembership;
        private static Dictionary<string, int> roomNextObjectIndex;
        private static object isRunningLock = new object();

        public static void IsRunning(bool value)
        {
            lock (isRunningLock)
            {
                isRunning = value;
            }
        }

        public WhiteboardServer(string simModelPath, ref SimulationEventDistributor distributor)
        {
            simModelName = simModelPath;
            simModelInfo = smr.readModel(simModelName);
            lock (isRunningLock)
            {
                isRunning = false;
            }
            roomMembership = new Dictionary<string, List<string>>();
            roomNextObjectIndex = new Dictionary<string, int>();
            server = new SimulationEventDistributorClient();
            distributor.RegisterClient(ref server);
            
        }

        public void ClearTextServerVariables()
        {
            simModelName = String.Empty;
            simModelInfo = null;
            roomMembership = new Dictionary<string, List<string>>();
            roomNextObjectIndex = new Dictionary<string, int>();
        }

        private static void SendWhiteboardLine(string object_ID, string userID, string targetUserID,
            int mode, LocationValue start, LocationValue end, double width, double originalScale, int color, string text)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "WhiteboardLine");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["ObjectID"]).value = object_ID;
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            ((IntegerValue)(sendingEvent["Mode"])).value = mode; 
            sendingEvent["StartLocation"] = start;
            sendingEvent["EndLocation"] = end;
            ((DoubleValue)(sendingEvent["Width"])).value = width;
            ((DoubleValue)(sendingEvent["OriginalScale"])).value = originalScale;
            ((IntegerValue)(sendingEvent["Color"])).value = color;
            ((StringValue)sendingEvent["Text"]).value = text;
            server.PutEvent(sendingEvent);        
        }
        private static void SendWhiteboardClear(string userID, string targetUserID)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "WhiteboardClear");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            server.PutEvent(sendingEvent);
        }
        private static void SendWhiteboardClearAll(string userID, string targetUserID)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "WhiteboardClearAll");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            server.PutEvent(sendingEvent);
        }
        private static void SendWhiteboardUndo(string object_ID, string userID, string targetUserID)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "WhiteboardUndo");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["ObjectID"]).value = object_ID;
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            server.PutEvent(sendingEvent);
        }
        private static void SendWhiteboardSyncScreenView(string userID, string targetUserID, string whiteboardID)
        {
            SimulationEvent sendingEvent = SimulationEventFactory.BuildEvent(ref simModelInfo, "WhiteboardSyncScreenView");
            DataValue dv = new StringValue();
            ((StringValue)sendingEvent["UserID"]).value = userID;
            ((StringValue)sendingEvent["TargetUserID"]).value = targetUserID;
            ((StringValue)sendingEvent["WhiteboardID"]).value = whiteboardID;
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
        public void StartWhiteboardServer()
        {
            try
            {
                List<SimulationEvent> incomingEvents = new List<SimulationEvent>();

                server.Subscribe("WhiteboardLineRequest");
                server.Subscribe("WhiteboardClearRequest");
                server.Subscribe("WhiteboardClearAllRequest");
                server.Subscribe("WhiteboardUndoRequest");
                server.Subscribe("WhiteboardSyncScreenViewRequest");
                server.Subscribe("TimeTick");
                server.Subscribe("CreateWhiteboardRoom");

                bool localIsRunning = true;
                {
                    isRunning = true; //This can be set false elsewhere, and end the loop.
                }
                while (localIsRunning)
                {
                    incomingEvents = server.GetEvents();

                    if (incomingEvents.Count != 0)
                    {
                        foreach (SimulationEvent se in incomingEvents)
                        {
                            switch (se.eventType)
                            {
                                case "WhiteboardLineRequest":
                                    WhiteboardLineRequest(se);
                                    break;
                                case "WhiteboardClearRequest":
                                    WhiteboardClearRequest(se);
                                    break;
                                case "WhiteboardClearAllRequest":
                                    WhiteboardClearAllRequest(se);
                                    break;
                                case "WhiteboardUndoRequest":
                                    WhiteboardUndoRequest(se);
                                    break;
                                case "WhiteboardSyncScreenViewRequest":
                                    WhiteboardSyncScreenViewRequest(se);
                                    break;
                                case "TimeTick":
                                    currentTick = ((IntegerValue)se["Time"]).value;
                                    timeString = ((StringValue)se["SimulationTime"]).value;
                                    break;
                                case "CreateWhiteboardRoom":
                                    CreateWhiteboardRoom(se);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(500);
                    lock (isRunningLock)
                    {
                        localIsRunning = isRunning;
                    }

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
        private void WhiteboardLineRequest(SimulationEvent e)
        {
            string objectID = null;
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetRoom = ((StringValue)e["TargetUserID"]).value;
            if (!roomNextObjectIndex.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending whiteboard message: You do not have permission to whiteboard in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            int currentObjectIndex = roomNextObjectIndex[targetRoom];
            int mode = ((IntegerValue)e["Mode"]).value;
            double width = ((DoubleValue)e["Width"]).value;
            double orginalScale = ((DoubleValue)e["OriginalScale"]).value;
            int color = ((IntegerValue)e["Color"]).value;
            LocationValue start = ((LocationValue)e["StartLocation"]);
            LocationValue end = ((LocationValue)e["EndLocation"]);
            string text = ((StringValue)e["Text"]).value;

            objectID = requestingID + "_" + currentObjectIndex.ToString();
            currentObjectIndex++;
            roomNextObjectIndex[targetRoom] = currentObjectIndex;
            SendWhiteboardLine(objectID, requestingID, targetRoom, mode, start, end, width, orginalScale, color, text);
        }

        private void WhiteboardClearRequest(SimulationEvent e)
        {
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetRoom = ((StringValue)e["TargetUserID"]).value;
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending whiteboard message: You do not have permission to whiteboard in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            SendWhiteboardClear(requestingID, targetRoom);
        }

        private void WhiteboardClearAllRequest(SimulationEvent e)
        {
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetRoom = ((StringValue)e["TargetUserID"]).value;
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending whiteboard message: You do not have permission to whiteboard in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            SendWhiteboardClearAll(requestingID, targetRoom);
        }

        private void WhiteboardUndoRequest(SimulationEvent e)
        {
            string objectID = ((StringValue)e["ObjectID"]).value;
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetRoom = ((StringValue)e["TargetUserID"]).value;
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending whiteboard message: You do not have permission to whiteboard in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            SendWhiteboardUndo(objectID, requestingID, targetRoom);
        }

        private void WhiteboardSyncScreenViewRequest(SimulationEvent e)
        {
            string requestingID = ((StringValue)e["UserID"]).value;
            string targetUser = ((StringValue)e["TargetUserID"]).value;
            string targetRoom = ((StringValue)e["WhiteboardID"]).value;
            if (!roomMembership.ContainsKey(targetRoom))
            {
                return;
            }
            if (!roomMembership[targetRoom].Contains(requestingID))
            {
                SendSystemErrorMessage(String.Format("Error in sending whiteboard message: You do not have permission to whiteboard in the specified room \"{0}\"", targetRoom), requestingID);
                return;
            }

            SendWhiteboardSyncScreenView(requestingID, targetUser, targetRoom);
        }

        private void NewObject(SimulationEvent e)
        {
            string dmID = ((StringValue)e["ID"]).value;

            if (((StringValue)e["ObjectType"]).value == "DecisionMaker")
            { 
                
            }
        }

        private void CreateWhiteboardRoom(SimulationEvent e)
        {
            string roomName = ((StringValue)e["RoomName"]).value;
            List<string> membership = ((StringListValue)e["MembershipList"]).strings;
            if (!roomMembership.ContainsKey(roomName))
            {
                roomMembership.Add(roomName, membership);
                roomNextObjectIndex.Add(roomName, 1);
            }
            else
            {
                roomMembership[roomName] = membership;
                roomNextObjectIndex[roomName] = 1;
            }
        }
    }
}
