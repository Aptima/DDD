using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.PasswordHashing;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.UserTools;
//using Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib;
//using Aptima.Asim.DDD.CommonComponents.PasswordHashing;


namespace Aptima.Asim.DDD.Client.Controller
{

    partial class GUIController
    {
        #region ICommand Members

        public void AuthenticationRequest(string username, string password, string terminal_id)
        {
            _AuthenticationRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "AuthenticationRequest");
            ((StringValue)(_AuthenticationRequest["Username"])).value = username;
            ((StringValue)(_AuthenticationRequest["Password"])).value = PasswordHashUtility.HashPassword(password);
            ((StringValue)(_AuthenticationRequest["TerminalID"])).value = terminal_id;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_AuthenticationRequest);
            }
        }
        public void DoMove(string user_id, string object_id, double throttle, double xpos, double ypos, double zpos)
        {
            _MoveEvent = SimulationEventFactory.BuildEvent(ref _SimModel, "MoveObjectRequest");

            ((StringValue)(_MoveEvent["UserID"])).value = user_id;
            ((StringValue)(_MoveEvent["ObjectID"])).value = object_id;
            ((DoubleValue)(_MoveEvent["Throttle"])).value = throttle;

            ((LocationValue)(_MoveEvent["DestinationLocation"])).X = (double)UTM_Mapping.HorizontalPixelsToMeters((float)xpos);
            ((LocationValue)(_MoveEvent["DestinationLocation"])).Y = (double)UTM_Mapping.VerticalPixelsToMeters((float)ypos);
            ((LocationValue)(_MoveEvent["DestinationLocation"])).Z = zpos;
            ((LocationValue)(_MoveEvent["DestinationLocation"])).exists = true;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_MoveEvent);
            }
            else
            {
                lock (this)
                {
                    if (DemoEvents != null)
                    {
                        DemoEvents.Add(_MoveEvent);
                    }
                }
            }
        }
        public void RequestClassification(string objID, string newClassification, string sender_dm)
        {
            _requestClassification = SimulationEventFactory.BuildEvent(ref _SimModel, "ObjectClassificationRequest");
            ((StringValue)(_requestClassification["ObjectID"])).value = objID;
            ((StringValue)(_requestClassification["UserID"])).value = sender_dm;
            ((StringValue)(_requestClassification["ClassificationName"])).value = newClassification;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_requestClassification);
            }
        }
        public void RequestChatRoomCreate(string room_name, List<string> members, string sender_dm)
        {
            _RequestChatRoom = SimulationEventFactory.BuildEvent(ref _SimModel, "RequestChatRoomCreate");
            ((StringValue)(_RequestChatRoom["RoomName"])).value = room_name;
            ((StringValue)(_RequestChatRoom["SenderDM_ID"])).value = sender_dm;
            ((StringListValue)(_RequestChatRoom["MembershipList"])).strings = members;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_RequestChatRoom);
            }
        }
        public void RequestWhiteboardRoomCreate(string room_name, List<string> members, string sender_dm)
        {
            _RequestChatRoom = SimulationEventFactory.BuildEvent(ref _SimModel, "RequestWhiteboardRoomCreate");
            ((StringValue)(_RequestChatRoom["RoomName"])).value = room_name;
            ((StringValue)(_RequestChatRoom["SenderDM_ID"])).value = sender_dm;
            ((StringListValue)(_RequestChatRoom["MembershipList"])).strings = members;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_RequestChatRoom);
            }
        }

        public void SetClassifications(List<string> classificationEnumeration)
        { 
            
        }

        public void SelectionUpdate()
        {
            if (_receiver != null)
            {
                _receiver.SelectionUpdate();
            }
        }

        public void ZoomUpdate()
        {
            if (_receiver != null)
            {
                _receiver.ZoomUpdate();
            }
        }

    //    So here's the deal.  I created a generic attack request event for you called "ClientAttackRequest".  It has the following attributes in it:

    //* PlayerID - StringValue - Your user's PlayerID
    //* AttackingObjectID - StringValue - The ID of the attacking object
    //* TargetObjectID - StringValue - The ID of the target object
    //* WeaponOrCapabilityName - StringValue - The capability name or the weapon ID of the weapon or capability being used.

        public void DoAttack(string user_id, string object_id, string target_id, string capability)
        {
            _AttackEvent = SimulationEventFactory.BuildEvent(ref _SimModel, "ClientAttackRequest");
            ((StringValue)(_AttackEvent["PlayerID"])).value = user_id;
            ((StringValue)(_AttackEvent["AttackingObjectID"])).value = object_id;
            ((StringValue)(_AttackEvent["TargetObjectID"])).value = target_id;
            ((StringValue)(_AttackEvent["WeaponOrCapabilityName"])).value = capability;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_AttackEvent);
            }
            else
            {
                lock (this)
                {
                    if (DemoEvents != null)
                    {
                        DemoEvents.Add(_AttackEvent);
                    }
                }
            }

        }
        public void SubPlatformLaunch(string object_id, string parent_id, double xpos, double ypos, double zpos)
        {
            _SubPEvent = SimulationEventFactory.BuildEvent(ref _SimModel, "SubplatformLaunchRequest");
            ((StringValue)(_SubPEvent["UserID"])).value = DDD_Global.Instance.PlayerID;
            ((StringValue)(_SubPEvent["ObjectID"])).value = object_id;
            ((StringValue)(_SubPEvent["ParentObjectID"])).value = parent_id;
            ((LocationValue)(_SubPEvent["LaunchDestinationLocation"])).X = (double)UTM_Mapping.HorizontalPixelsToMeters((float)xpos);
            ((LocationValue)(_SubPEvent["LaunchDestinationLocation"])).Y = (double)UTM_Mapping.VerticalPixelsToMeters((float)ypos);
            ((LocationValue)(_SubPEvent["LaunchDestinationLocation"])).Z = zpos;
            ((LocationValue)(_SubPEvent["LaunchDestinationLocation"])).exists = true;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_SubPEvent);
            }
            else
            {
                lock (this)
                {
                    if (DemoEvents != null)
                    {
                        DemoEvents.Add(_SubPEvent);
                    }
                }
            }
        }

        public void SubPlatformDock(string object_id, string parent_id)
        {
            _SubPEvent = SimulationEventFactory.BuildEvent(ref _SimModel, "SubplatformDockRequest");
            ((StringValue)(_SubPEvent["UserID"])).value = DDD_Global.Instance.PlayerID;
            ((StringValue)(_SubPEvent["ObjectID"])).value = object_id;
            ((StringValue)(_SubPEvent["ParentObjectID"])).value = parent_id;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_SubPEvent);
            }
        }

        public void DisconnectDecisionMaker(string dm_id)
        {
            _AttackEvent = SimulationEventFactory.BuildEvent(ref _SimModel, "DisconnectDecisionMaker");
            ((StringValue)(_AttackEvent["DecisionMakerID"])).value = dm_id;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_AttackEvent);
            }

        }

        public void TextChatRequest(string user_id, string message, string type, string target)
        {
            _ChatRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "TextChatRequest");

            ((StringValue)(_ChatRequest["UserID"])).value = user_id;
            ((StringValue)(_ChatRequest["ChatBody"])).value = message;

            //        ALL for a global message, TEAM for a team message, P2P for a private message.
            ((StringValue)(_ChatRequest["ChatType"])).value = type;

            //        Only need to be set if ChatType == P2P, this is the unique ID of the decision maker receiving the message.
            ((StringValue)(_ChatRequest["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_ChatRequest);
            }
        }
        public void BeginTextChatRequest(string user_id, string room_name)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref _SimModel, "BeginTextChatRequest");

            ((StringValue)(ev["UserID"])).value = user_id;
            ((StringValue)(ev["RoomName"])).value = room_name;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(ev);
            }
        }

        public void WhiteboardLineRequest(string user_id, int mode, LocationValue start, LocationValue end, double width, double originalScale, int color, string text, string target)
        {
            _WBRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "WhiteboardLineRequest");

            ((StringValue)(_WBRequest["UserID"])).value = user_id;
            ((IntegerValue)(_WBRequest["Mode"])).value = mode;
            _WBRequest["StartLocation"] = start;
            _WBRequest["EndLocation"] = end;
            ((DoubleValue)(_WBRequest["Width"])).value = width;
            ((DoubleValue)(_WBRequest["OriginalScale"])).value = originalScale;
            ((IntegerValue)(_WBRequest["Color"])).value = color;
            ((StringValue)(_WBRequest["Text"])).value = text;
            ((StringValue)(_WBRequest["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_WBRequest);
            }
        }
        public void BeginWhiteboardLineRequest(string user_id, string target)
        {
            SimulationEvent ev = SimulationEventFactory.BuildEvent(ref _SimModel, "BeginWhiteboardLineRequest");

            ((StringValue)(ev["UserID"])).value = user_id;
            ((StringValue)(ev["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(ev);
            }
        }

        public void WhiteboardClearRequest(string user_id, string target)
        {
            _WBRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "WhiteboardClearRequest");

            ((StringValue)(_WBRequest["UserID"])).value = user_id;
            ((StringValue)(_WBRequest["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_WBRequest);
            }
        }

        public void WhiteboardClearAllRequest(string user_id, string target)
        {
            _WBRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "WhiteboardClearAllRequest");

            ((StringValue)(_WBRequest["UserID"])).value = user_id;
            ((StringValue)(_WBRequest["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_WBRequest);
            }
        }

        public void WhiteboardUndoRequest(string object_id, string user_id, string target)
        {
            _WBRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "WhiteboardUndoRequest");

            ((StringValue)(_WBRequest["ObjectID"])).value = object_id;
            ((StringValue)(_WBRequest["UserID"])).value = user_id;
            ((StringValue)(_WBRequest["TargetUserID"])).value = target;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_WBRequest);
            }
        }

        public void WhiteboardSyncScreenViewRequest(string user_id, string target, string whiteboard_ID)
        {
            _WBRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "WhiteboardSyncScreenViewRequest");

            ((StringValue)(_WBRequest["UserID"])).value = user_id;
            ((StringValue)(_WBRequest["TargetUserID"])).value = target;
            ((StringValue)(_WBRequest["WhiteboardID"])).value = whiteboard_ID;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_WBRequest);
            }
        }

        public void HandshakeGUIRegister(string terminal_id)
        {
             _HandshakeRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "HandshakeGUIRegister");

             ((StringValue)(_HandshakeRequest["TerminalID"])).value = terminal_id;

             if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_HandshakeRequest);
            }

        }
        public void HandshakeGUIRoleRequest(string player_id, string terminal_id)
        {
            _HandshakeRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "HandshakeGUIRoleRequest");

            ((StringValue)(_HandshakeRequest["PlayerID"])).value = player_id;
            ((StringValue)(_HandshakeRequest["TerminalID"])).value = terminal_id;
            ((StringValue)(_HandshakeRequest["LoginType"])).value = "FULL";
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_HandshakeRequest);
            }

        }
        public void HandshakeInitializeGUIDone(string player_id)
        {
            _HandshakeRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "HandshakeInitializeGUIDone");

            ((StringValue)(_HandshakeRequest["PlayerID"])).value = player_id;
            ((StringValue)(_HandshakeRequest["LoginType"])).value = "FULL";
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_HandshakeRequest);
            }
        }

        public void TransferObjectRequest(string player_id, string objectID, string newOwnerID, string objectState)
        {
            _TransferObjectRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "TransferObjectRequest");

            ((StringValue)(_TransferObjectRequest["UserID"])).value = player_id;
            ((StringValue)(_TransferObjectRequest["ObjectID"])).value = objectID;
            ((StringValue)(_TransferObjectRequest["RecipientID"])).value = newOwnerID;
            ((StringValue)(_TransferObjectRequest["State"])).value = objectState;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_TransferObjectRequest);
            }

        }

        public void DockObjectRequest(string player_id, string objectID, string parentObjectID, bool dockingToOther)
        {
            _DockingToOtherRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "SubplatformDockRequest");

            ((StringValue)(_DockingToOtherRequest["UserID"])).value = player_id;
            ((StringValue)(_DockingToOtherRequest["ObjectID"])).value = objectID;
            ((StringValue)(_DockingToOtherRequest["ParentObjectID"])).value = parentObjectID;
            //((BooleanValue)(_DockingToOtherRequest["DockToNonParent"])).value = dockingToOther;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_DockingToOtherRequest);
            }
        }

        public void ChangeTagRequest(string player_id, string objectID, string tag)
        {
            _ChangeTagRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "ChangeTagRequest");

            ((StringValue)(_ChangeTagRequest["UnitID"])).value = objectID;
            ((StringValue)(_ChangeTagRequest["DecisionMakerID"])).value = player_id;
            ((StringValue)(_ChangeTagRequest["Tag"])).value = tag;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_ChangeTagRequest);
            }
        }

        public void DoClientScreenUpdate(int originX, int originY, int screenSizeX, int screenSizeY, float zoomLevel, string dmID)
        {
            _ScreenPositionUpdate = SimulationEventFactory.BuildEvent(ref _SimModel, "ClientMeasure_ScreenView");

            ((IntegerValue)(_ScreenPositionUpdate["Origin-X"])).value = originX;
            ((IntegerValue)(_ScreenPositionUpdate["Origin-Y"])).value = originY;
            ((IntegerValue)(_ScreenPositionUpdate["ScreenSizeWidth"])).value = screenSizeX;
            ((IntegerValue)(_ScreenPositionUpdate["ScreenSizeHeight"])).value = screenSizeY;
            ((DoubleValue)(_ScreenPositionUpdate["ScreenZoom"])).value = zoomLevel;
            ((StringValue)(_ScreenPositionUpdate["UserID"])).value = dmID;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_ScreenPositionUpdate);
            }

            Console.WriteLine(String.Format("New Map Position: X = {0}; Y = {1};  Zoom = {2}; View Dimensions ({3} x {4})", originX, originY, zoomLevel, screenSizeX, screenSizeY));

        }

        public void DoSendWeaponSelectedUpdate(string userID, string parentObjectID, string weaponName, bool isWeapon)
        {
            _MeasureWeaponSelected = SimulationEventFactory.BuildEvent(ref _SimModel, "ClientMeasure_CapabilitySelected");

            ((StringValue)(_MeasureWeaponSelected["UserID"])).value = userID;
            ((StringValue)(_MeasureWeaponSelected["ParentObjectID"])).value = parentObjectID;
            ((StringValue)(_MeasureWeaponSelected["WeaponOrCapabilityName"])).value = weaponName;
            //((BooleanValue)(_MeasureWeaponSelected["IsWeapon"])).value = isWeapon;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_MeasureWeaponSelected);
            }

            Console.WriteLine("Weapon or capability Selected: " + weaponName + "; " + parentObjectID);
        }

        public void DoSendObjectSelectedUpdate(string userID, string objectID, string ownerID)
        {
            _MeasureSelectedObject = SimulationEventFactory.BuildEvent(ref _SimModel, "ClientMeasure_ObjectSelected");

            ((StringValue)(_MeasureSelectedObject["UserID"])).value = userID;
            ((StringValue)(_MeasureSelectedObject["ObjectID"])).value = objectID;
            ((StringValue)(_MeasureSelectedObject["OwnerID"])).value = ownerID;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_MeasureSelectedObject);
            }
            Console.WriteLine("Object Selected: " + objectID + "; " + ownerID);
        }

        public void DoSendTabSelectionUpdate(string userID, string objectID, string tabName)
        {
            _MeasureTabSelected = SimulationEventFactory.BuildEvent(ref _SimModel, "ClientMeasure_ObjectTabSelected");

            ((StringValue)(_MeasureTabSelected["UserID"])).value = userID;
            ((StringValue)(_MeasureTabSelected["TabName"])).value = tabName;
            ((StringValue)(_MeasureTabSelected["ObjectID"])).value = objectID;

            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_MeasureTabSelected);
            }

            Console.WriteLine("Tab Selected: " + objectID + "; " + tabName);
        }

        //DoSendRangeRingDisplay update

        #endregion

        #region IVoiceClientEventCommunicator

        public void sendRequestJoinVoiceChannelEvent( string strChannelName )
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent( ref _SimModel, "RequestJoinVoiceChannel" );
            ( ( StringValue )( _VoiceClientEventRequest["ChannelName"] ) ).value = strChannelName;
            ( ( StringValue )( _VoiceClientEventRequest["DecisionMakerID"] ) ).value = DDD_Global.Instance.PlayerID;
            if ( DDD_Global.Instance.IsConnected )
            {
                DDD_Global.Instance.PutEvent( _VoiceClientEventRequest );
            }
        }

        public void sendRequestLeaveVoiceChannelEvent( string strChannelName )
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent( ref _SimModel, "RequestLeaveVoiceChannel" );
            ( ( StringValue )( _VoiceClientEventRequest["ChannelName"] ) ).value = strChannelName;
            ( ( StringValue )( _VoiceClientEventRequest["DecisionMakerID"] ) ).value = DDD_Global.Instance.PlayerID;
            if ( DDD_Global.Instance.IsConnected )
            {
                DDD_Global.Instance.PutEvent( _VoiceClientEventRequest );
            }
        }

        public void sendRequestStartedTalkingVoiceChannelEvent( string strChannelName )
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent( ref _SimModel, "RequestStartedTalking" );
            ( ( StringValue )( _VoiceClientEventRequest["ChannelName"] ) ).value = strChannelName;
            ( ( StringValue )( _VoiceClientEventRequest["Speaker"] ) ).value = DDD_Global.Instance.PlayerID;
            if ( DDD_Global.Instance.IsConnected )
            {
                DDD_Global.Instance.PutEvent( _VoiceClientEventRequest );
            }
        }

        public void sendRequestStoppedTalkingVoiceChannelEvent( string strChannelName )
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent( ref _SimModel, "RequestStoppedTalking" );
            ( ( StringValue )( _VoiceClientEventRequest["ChannelName"] ) ).value = strChannelName;
            ( ( StringValue )( _VoiceClientEventRequest["Speaker"] ) ).value = DDD_Global.Instance.PlayerID;
            if ( DDD_Global.Instance.IsConnected )
            {
                DDD_Global.Instance.PutEvent( _VoiceClientEventRequest );
            }
        }

        public void sendRequestMuteUserEvent(string strChannelName)
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "RequestMuteUser");
            ((StringValue)(_VoiceClientEventRequest["ChannelName"])).value = strChannelName;
            ((StringValue)(_VoiceClientEventRequest["Speaker"])).value = DDD_Global.Instance.PlayerID;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_VoiceClientEventRequest);
            }
        }

        public void sendRequestUnmuteUserEvent(string strChannelName)
        {
            _VoiceClientEventRequest = SimulationEventFactory.BuildEvent(ref _SimModel, "RequestUnmuteUser");
            ((StringValue)(_VoiceClientEventRequest["ChannelName"])).value = strChannelName;
            ((StringValue)(_VoiceClientEventRequest["Speaker"])).value = DDD_Global.Instance.PlayerID;
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.PutEvent(_VoiceClientEventRequest);
            }
        }

        #endregion IVoiceClientEventCommunicator
    }
}
