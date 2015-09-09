using System;
using System.Collections.Generic;
using System.Text;

namespace VSG.PreviewSimulator
{
    public class ReiterateEvent : AbstractPreviewEvent, IDisposable
    {
        public AbstractPreviewEvent StartEvent;
        public static ReiterateEvent Empty = new ReiterateEvent();
        public List<MoveEvent> MoveEvents;
        private int index = 0;
        public int MoveComplete_Time = 0;
        public PreviewSimulator Parent = null;

        public ReiterateEvent()
        {
            ID = string.Empty;
            Name = string.Empty;
            Time = 0;
            Parent = null;

            MoveEvents = new List<MoveEvent>();
        }

        public ReiterateEvent(string id, string name, int time)
        {
            ID = id;
            Name = name;
            Time = time;
            Parent = null;

            MoveEvents = new List<MoveEvent>();
        }

        public void ResetIndex()
        {
            index = 0;
        }
        public void SetIndex(int i)
        {
            index = i;
        }


        public void AddEvent(MoveEvent move)
        {
            // Reiterate ignores time.
            move.Time = 0;
            if (MoveEvents.Count == 0)
            {
                this.Name = move.Name;
            }

            MoveEvents.Add(move);           
        }

        public MoveEvent CurrentEvent()
        {
            return MoveEvents[index];
        }

        public void MoveToNextEvent()
        {
            index++;
            if (index == MoveEvents.Count)
            {
                index = 0;
            }
        }

        public void StartNextEvent()
        {
            MoveToNextEvent();
            MoveEvent evt = CurrentEvent();

            if (Parent != null)
            {
                Parent.DoMove(evt, new System.Threading.ThreadStart(StartNextEvent));
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Parent = null;
        }

        #endregion
    }
}
