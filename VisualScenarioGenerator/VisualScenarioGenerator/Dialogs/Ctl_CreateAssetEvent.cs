using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_CreateAssetEvent : UserControl, ICtl_ContentPane__OutboundUpdate
    {
        public Ctl_CreateAssetEvent()
        {
            InitializeComponent();
        }


        #region IVSG_ControlStateOutboundUpdate Members

        void ICtl_ContentPane__OutboundUpdate.Update(Control control, object object_data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        
       
    }
}
