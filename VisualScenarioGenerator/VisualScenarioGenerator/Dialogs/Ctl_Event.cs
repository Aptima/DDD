using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public enum EventModeEnum : int { Global, Asset }

    public partial class Ctl_Event : UserControl, ICtl_ContentPane__InboundInitilialize
    {
        public const string EVENT_CHANGE_ENGRAM = "Change Engram";
        public const string EVENT_CLOSE_CHATROOM = "Close Chat Room";
        public const string EVENT_FLUSH = "Flush";
        public const string EVENT_LAUNCH = "Launch";
        public const string EVENT_MOVE = "Move";
        public const string EVENT_OPEN_CHATROOM = "Open Chat Room";
        public const string EVENT_REMOVE_ENGRAM = "Remove Engram";
        public const string EVENT_REVEAL = "Reveal";
        public const string EVENT_STATE_CHANGE = "State Change";
        public const string EVENT_TRANSFER = "Transfer";


        private TrackStateInfo _track_state = TrackStateInfo.Empty;
        public string EventName
        {
            get
            {
                return this.txtEventName.Text;
            }
            set
            {
                txtEventName.Text = value;
            }
        }
        public string EventType
        {
            get
            {
                if (cmbAssetEventTypes.Visible)
                {
                    return this.cmbAssetEventTypes.SelectedItem.ToString();
                }
                return cmbGlobalEventTypes.SelectedItem.ToString();
            }
        }
        public string CompletionEvent
        {
            get
            {
                if (cmbCompletionEvent.SelectedItem != null)
                {
                    return this.cmbCompletionEvent.SelectedItem.ToString();
                }
                return string.Empty;
            }
        }
        public bool CompletionChecked
        {
            get
            {
                return radOnCompletion.Checked;
            }
            set
            {
                radOnCompletion.Checked = value;
            }
        }
        public bool DefaultChecked
        {
            get
            {
                return radDefault.Checked;
            }
            set
            {
                radDefault.Checked = value;
            }
        }
        
        public bool ReiterateChecked
        {
            get
            {
                return radReiterate.Checked;
            }
            set
            {
                radReiterate.Checked = value;
            }
        }

       
        public Ctl_Event()
        {
            InitializeComponent();
            cmbGlobalEventTypes.Hide();
        }

        public void ChangeEventDisplayMode(EventModeEnum mode)
        {
            switch (mode)
            {
                case EventModeEnum.Asset:
                    cmbGlobalEventTypes.Hide();
                    cmbAssetEventTypes.Show();
                    break;
                case EventModeEnum.Global:
                    cmbAssetEventTypes.Hide();
                    cmbGlobalEventTypes.Show();
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void SetEventChoices(string[] items)
        {
            this.cmbAssetEventTypes.Items.Clear();
            cmbAssetEventTypes.Items.AddRange(items);
        }
        public void SetOnCompletionChoices(string[] items)
        {
            cmbCompletionEvent.Items.Clear();
            cmbCompletionEvent.Items.AddRange(items);
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            if (Parent is ICtl_ContentPane__OutboundUpdate)
            {
                _track_state.EventType = string.Empty;
                _track_state.CompletionEvent = string.Empty;
                if (cmbCompletionEvent.SelectedItem != null)
                {
                    _track_state.CompletionEvent = cmbCompletionEvent.SelectedItem.ToString();
                }
                if (cmbAssetEventTypes.SelectedItem != null)
                {
                    _track_state.EventType = cmbAssetEventTypes.SelectedItem.ToString();
                }
                _track_state.TrackName = txtEventName.Text;
                _track_state.Reiterate = radReiterate.Checked;
                _track_state.OnComplete = radOnCompletion.Checked;
                _track_state.Default = radDefault.Checked;

                ((ICtl_ContentPane__OutboundUpdate)Parent).Update(this, (object)_track_state);

            }
        }



        #region IVSG_ControlStateInboundUpdate Members

        public void Initialize(object object_data)
        {
            TrackStateInfo _track_state = (TrackStateInfo)object_data;
            Console.WriteLine("Initialize, {0}, {1}", _track_state.TrackName, _track_state.EventType);
            txtEventName.Text = _track_state.TrackName;
            cmbAssetEventTypes.SelectedItem = _track_state.EventType;
            radDefault.Checked = _track_state.Default;
            radReiterate.Checked = _track_state.Reiterate;
            radOnCompletion.Checked = _track_state.OnComplete;
        }

        #endregion
    }
}
