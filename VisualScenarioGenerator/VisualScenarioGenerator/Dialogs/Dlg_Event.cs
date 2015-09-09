using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public struct Struct_DlgEvent: ICloneable
    {
        public string EventName;
        public string CompletionEvent;
        public bool OnCompletion;
        public bool Reiterate;
        public bool ActualTime;
        public string EventType;

        public static Struct_DlgEvent Empty = new Struct_DlgEvent(string.Empty, string.Empty, string.Empty, false, false, false);

        public Struct_DlgEvent(string event_name, string event_type, string completion_event, bool on_completion, bool reiterate, bool actual_time)
        {
            EventName = event_name;
            EventType = event_type;
            CompletionEvent = completion_event;
            OnCompletion = on_completion;
            Reiterate = reiterate;
            ActualTime = actual_time;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Struct_DlgEvent)
            {
                return ((Struct_DlgEvent)obj).EventName == EventName;
            }
            return false;
        }


        #region ICloneable Members

        public object Clone()
        {
            Struct_DlgEvent evt = new Struct_DlgEvent();
            evt.EventName = (string)EventName.Clone();
            evt.EventType = (string)EventType.Clone();
            evt.CompletionEvent = (string)CompletionEvent.Clone();
            evt.OnCompletion = OnCompletion;
            evt.Reiterate = Reiterate;
            evt.ActualTime = ActualTime;
            return evt;
        }

        #endregion
    }


    public partial class Dlg_Event : Form
    {
        public Struct_DlgEvent _data;
        public Struct_DlgEvent Data
        {
            get
            {
                return _data;
            }
        }

        public Dlg_Event()
        {
            InitializeComponent();
        }

        public void SetMode(EventModeEnum mode)
        {
            ctl_Event1.ChangeEventDisplayMode(mode);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (this.ctl_Event1.EventName != string.Empty && this.ctl_Event1.EventType != string.Empty)
            {
                _data.EventName = ctl_Event1.EventName;
                _data.EventType = ctl_Event1.EventType;
                _data.ActualTime = ctl_Event1.DefaultChecked;
                _data.OnCompletion = ctl_Event1.CompletionChecked;
                _data.Reiterate = ctl_Event1.ReiterateChecked;
                _data.CompletionEvent = ctl_Event1.CompletionEvent;
            }
            else
            {
                MessageBox.Show(this.Owner, "Must specify and Event Name and Event Type", "Error Creating Event", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}