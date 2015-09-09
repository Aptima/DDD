using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Node_RemoveEngramEvent : UserControl, ICtl_ContentPane__OutboundUpdate, ICtl_Node
    {
        public Ctl_Node_RemoveEngramEvent()
        {
            InitializeComponent();
        }


        #region IVSG_ControlStateOutboundUpdate Members

        void ICtl_ContentPane__OutboundUpdate.Update(Control control, object object_data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICtl_Node Members

        public void SetNodeData(string id, int tick)
        {
            this.ctl_Node1.ID = id;
            this.ctl_Node1.Tick = tick;
        }

        public string GetNodeId()
        {
            return ctl_Node1.ID;
        }

        public int GetTick()
        {
            return ctl_Node1.Tick;
        }

        #endregion

    }
}
