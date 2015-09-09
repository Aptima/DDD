using System;
using System.Collections.Generic;
using System.Text;

namespace VisualScenarioGenerator.Dialogs
{
    public interface ITimelinePanel
    {
        bool BeforeNodeAdd(string TrackName, int time_tick);
        void AfterNodeAdd(string TrackName);

        bool BeforeNodeDelete(string TrackName, int time_tick);
        void AfterNodeDelete(string TrackName);

        bool BeforeRemoveTimelineTrack(string TrackName);
        bool BeforeAddTimelineTrack(out string TrackName);
        void AfterAddTimelineTrack(string TrackName);


        void NodeSelectionChange(int time_tick);
        void TimelineTrackSelectionChange(string track_name);
    }
}
