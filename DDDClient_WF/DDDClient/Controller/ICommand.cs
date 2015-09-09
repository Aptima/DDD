using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.Client.Controller
{
    public interface ICommand
    {
        void DoMove(string user_id, string object_id, double throttle, double xpos, double ypos, double zpos);
        void DoAttack(string user_id, string object_id, string target_id, string capability);
        void SubPlatformLaunch(string object_id, string parent_id, double xpos, double ypos, double zpos);
        void SubPlatformDock(string object_id, string parent_id);
        void RequestClassification(string objID, string newClassification, string sender_dm);
        void TextChatRequest(string user_id, string message, string type, string target);
        void RequestChatRoomCreate(string room_name, List<string> members, string sender_dm);
        void RequestWhiteboardRoomCreate(string room_name, List<string> members, string sender_dm);
        void WhiteboardLineRequest(string user_id, int mode, LocationValue start, LocationValue end, double width, double originalScale,
            int color, string text, string target);
        void WhiteboardClearRequest(string user_id, string target);
        void WhiteboardClearAllRequest(string user_id, string target);
        void WhiteboardUndoRequest(string object_id, string user_id, string target);
        void WhiteboardSyncScreenViewRequest(string user_id, string target, string whiteboard_id);
        void HandshakeGUIRegister(string terminal_id);
        void HandshakeGUIRoleRequest(string player_id, string terminal_id);
        void HandshakeInitializeGUIDone(string player_id);
        void AuthenticationRequest(string username, string password, string terminal_id);
        void TransferObjectRequest(string player_id, string objectID, string newOwnerID, string objectState);
        void DockObjectRequest(string player_id, string objectID, string parentObjectID, bool dockingToOther);
        void ChangeTagRequest(string player_id, string objectID, string tag);

        //Client Side Measures
        void DoClientScreenUpdate(int originX, int originY, int screenSizeX, int screenSizeY, float zoomLevel, string dmID);
        void DoSendWeaponSelectedUpdate(string userID, string parentObjectID, string weaponName, bool isWeapon);
        void DoSendObjectSelectedUpdate(string userID, string objectID, string ownerID);
        void DoSendTabSelectionUpdate(string userId, string objectID, string tabName);
        //Range Ring toggle
        //

        // Bypasses network, Internal state notification.
        void SelectionUpdate();
        void ZoomUpdate();
    }
}
