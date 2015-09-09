using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.Client.Controller
{
    public interface IController
    {
        void ViewProAttributeUpdate(ViewProAttributeUpdate update);
        void ViewProMotionUpdate(ViewProMotionUpdate update);
        void ViewProInitializeUpdate(ViewProMotionUpdate update);
        void AttackUpdate(string attacker, string target, int time, int end_time);
        void TextChatRequest(string user_id, string message, string target_id);
        void WhiteboardLine(string original_id, string user_id, int mode, LocationValue start, LocationValue end, double width,
            double originalScale, int color, string text, string target_id);
        void WhiteboardClear(string user_id, string target_id);
        void WhiteboardClearAll(string user_id, string target_id);
        void WhiteboardUndo(string object_id, string user_id, string target_id);
        void WhiteboardScreenView(string user_id, int originX, int originY, int screenSizeWidth,
            int screenSizeHeight, double screenZoom);
        void WhiteboardSyncScreenView(string user_id, string target_id, string whiteboard_id);
        void HandshakeAvailablePlayers(string[] players);
        void RemoveObject(string object_id);
        void TimeTick(string time);
        void HandshakeInitializeGUI();
        void PauseScenario();
        void ResumeScenario();
        void StopScenario();
        void StopReplay();
        void SystemMessageUpdate(string message, int argbColor);
        void SelectionUpdate();
        void ViewProStopObjectUpdate(string objectID);
        void ZoomUpdate();
        void ScoreUpdate(string score_name, double score_value);
        void AuthenticationResponse(bool authenticated, string error_msg);
        void CreateChatRoom(string tab_name, string room_name, List<string> membership_list);
        void FailedToCreateChatRoom(string message, string sender);
        void CreateWhiteboardRoom(string tab_name, string room_name, List<string> membership_list, string senderDM);
        void ActiveRegionUpdate(string object_id, bool visible, int color, List<CustomVertex.TransformedColored> points);
        void CloseChatRoom(string room_name);
        void TransferObjectToMe(string objectID);
        void TransferObjectToOther(string objectID);
        void SendMapUpdate(bool sendNoMatterWhat);

        void SetClassifications(List<string> classificationEnumeration);
    }
}
