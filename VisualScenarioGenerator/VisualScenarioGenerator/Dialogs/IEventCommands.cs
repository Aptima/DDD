using System;
using System.Collections.Generic;
using System.Text;

namespace VisualScenarioGenerator.Dialogs
{
    interface IEventCommands
    {
        void AddEvent(string event_track);
        void DeleteEvent();
        void MoveSelectedTrackUp();
        void MoveSelectedTrackDown();
        void ShowProperties();
    }
}
