using System;
using System.Collections.Generic;
using System.Text;

namespace VisualScenarioGenerator.Dialogs
{
    public struct StructNode
    {
        public string Id;
        public int Tick;
    }
    public struct FlushNode
    {
        public StructNode Node_Info;
    }
    public struct LaunchNode
    {
        public StructNode Node_Info;
    }
    public struct MoveNode
    {
        public StructNode Node_Info;
    }
    public struct RevealNode
    {
        public StructNode Node_Info;
    }
    public struct OpenChatRoomNode
    {
        public StructNode Node_Info;
    }
    public struct RemoveEngramNode
    {
        public StructNode Node_Info;
    }
    public struct StateChangeNode
    {
        public StructNode Node_Info;
    }
    public struct TransferNode
    {
        public StructNode Node_Info;
    }
    public struct ChangeEngramNode
    {
        public StructNode Node_Info;
    }
    public struct CloseChatRoomNode
    {
        public StructNode Node_Info;
    }




    public class TrackStateInfo
    {
        public static TrackStateInfo Empty = new TrackStateInfo(string.Empty, string.Empty, false, false, false, string.Empty);
        public string TrackName;
        public string EventType;
        public bool Reiterate;
        public bool Default;
        public bool OnComplete;
        public string CompletionEvent;

        public SortedDictionary<int, object> Ticks;

        public TrackStateInfo(string track_name, string event_type, bool reiterate_option, bool default_option, bool complete_option, string completion_event)
        {
            TrackName = track_name;
            EventType = event_type;
            Reiterate = reiterate_option;
            Default = default_option;
            OnComplete = complete_option;
            CompletionEvent = completion_event;
            Ticks = new SortedDictionary<int, object>();
        }
        public void SetTrackState(string track_name, string event_type, bool reiterate_option, bool default_option, bool complete_option, string completion_event)
        {
            TrackName = track_name;
            EventType = event_type;
            Reiterate = reiterate_option;
            Default = default_option;
            OnComplete = complete_option;
            CompletionEvent = completion_event;
            Console.WriteLine("TrackName={0}, EventType={1}", TrackName, EventType);
        }

    }

    // Generic Interface for UI controls
    interface ICtl_ContentPane__OutboundUpdate
    {
        void Update(System.Windows.Forms.Control control, object object_data);
    }

    interface ICtl_ContentPane__InboundInitilialize
    {
        void Initialize(object object_data);
    }
}
