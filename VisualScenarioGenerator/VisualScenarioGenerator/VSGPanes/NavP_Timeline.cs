using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator.VSGPanes
{
    public enum TimelineVisualState : int
    {
        TopLevel, 
        Properties, 
        Flush, 
        Move, 
        Launch, 
        Reveal, 
        Transfer, 
        StateChange,
        ChangeEngram,
        OpenChatRoom, 
        CloseChatRoom, 
        RemoveEngram
    }


    public partial class NavP_Timeline : Ctl_NavigatorPane, ITimelinePanel
    {

        private Dictionary<string, TrackStateInfo> _TrackStates = new Dictionary<string, TrackStateInfo>();
        private TreeNode _current_node = null;
        private Dlg_CreateAssetInstance dlgCreateInstance = new Dlg_CreateAssetInstance();

        
        public NavP_Timeline()
        {
            InitializeComponent();
        }


        private void BindControl(Control control)
        {
            control.Parent = this;
            control.Dock = DockStyle.Fill;
            control.Visible = false;
        }  
        

        #region ITimelinePanel Members

        public bool BeforeNodeAdd(string TrackName, int time_tick)
        {
            return true;
        }

        public void AfterNodeAdd(string TrackName)
        {
        }

        public bool BeforeNodeDelete(string TrackName, int time_tick)
        {
            return true;
        }

        public void AfterNodeDelete(string TrackName)
        {
        }

        public bool BeforeRemoveTimelineTrack(string TrackName)
        {
            return true;
        }

        public bool BeforeAddTimelineTrack(out string TrackName)
        {
            // Never called.
            TrackName = "Move";
            return true;
        }
        public void AfterAddTimelineTrack(string TrackName)
        {
        }

        public void NodeSelectionChange(int time_tick)
        {

        }

        public void TimelineTrackSelectionChange(string track_name)
        {
        }

        #endregion



        private void btnNewNode_Click(object sender, EventArgs e)
        {           
            if (dlgCreateInstance.ShowDialog(Parent) == DialogResult.OK)
            {
                NewAssetInstanceDataStruct s = new NewAssetInstanceDataStruct();
                s.ID = dlgCreateInstance.Name;
                Notify(s);
            }
        }




        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            DeleteAssetInstanceDataStruct d = new DeleteAssetInstanceDataStruct();
            d.ID = treeView1.SelectedNode.Text;
            Notify(d);
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }




        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _current_node = e.Node;
            SelectAssetInstanceDataStruct d = new SelectAssetInstanceDataStruct();
            d.ID = e.Node.Text;
            Notify(d);
        }




        public override void Update(object object_data)
        {
            if (object_data is SelectAssetInstanceDataStruct)
            {
                SelectAssetInstanceDataStruct s = (SelectAssetInstanceDataStruct)object_data;
                if (s.ID == string.Empty)
                {
                    treeView1.SelectedNode = null;
                    return;
                }
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Text == s.ID)
                    {
                        treeView1.SelectedNode = node;
                        break;
                    }
                }
            }            

            if (object_data is NewAssetInstanceDataStruct)
            {
                NewAssetInstanceDataStruct s = (NewAssetInstanceDataStruct)object_data;
                treeView1.Nodes.Add(s.ID);
            }

            if (object_data is StructObjectTypes)
            {
                StructObjectTypes types = (StructObjectTypes)object_data;

                List<string> dms = new List<string>();
                foreach (DecisionMakerDataStruct dm in types.DecisionMakerList)
                {
                    dms.Add(dm.ID);
                }
                dlgCreateInstance.SetDecisionMakers(dms);

                List<string> species = new List<string>();
                foreach (SpeciesDataStruct sp in types.SpeciesList)
                {
                    species.Add(sp.ID);
                }
                dlgCreateInstance.SetSpecies(species);
            }
        }

        
    }
}
