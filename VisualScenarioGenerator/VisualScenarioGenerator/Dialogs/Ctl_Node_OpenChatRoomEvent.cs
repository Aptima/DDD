using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Node_OpenChatRoomEvent : UserControl, ICtl_Node
    {
        public Ctl_Node_OpenChatRoomEvent()
        {
            InitializeComponent();
        }

        #region ICtl_Node Members

        public void SetNodeData(string id, int tick)
        {
            //this.ctl_Node1.ID = id;
            //this.ctl_Node1.Tick = tick;
        }

        public string GetNodeId()
        {
            //return ctl_Node1.ID;
            return string.Empty;
        }

        public int GetTick()
        {
            //return ctl_Node1.Tick;
            return 0;
        }

        #endregion

    }
}
