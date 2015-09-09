using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public struct DlgMoveEventStruct
    {
        public string AssetId;
        public int Time;
        public string[] EngramRanges;
        public int XLocation;
        public int YLocation;
        public int ZLocation;
        public int Throttle;

        public static DlgMoveEventStruct Empty = new DlgMoveEventStruct(string.Empty, 0, null, 0, 0, 0, 0);

        public DlgMoveEventStruct(string id, int time, string[] engrams, int x, int y, int z, int throttle)
        {
            AssetId = id;
            Time = time;
            EngramRanges = engrams;
            XLocation = x;
            YLocation = y;
            ZLocation = z;
            Throttle = throttle;
        }
    }


    public partial class Dlg_Node_MoveEvent : Form
    {
        private DlgMoveEventStruct _datastore = DlgMoveEventStruct.Empty;
        public DlgMoveEventStruct Datastore
        {
            get
            {
                return _datastore;
            }
        }

        public Dlg_Node_MoveEvent()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}