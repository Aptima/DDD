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
    public enum Types_NodeTypes : int { DM_Node, Species_Node, Define_Engram_Node, Sensor_Node, State_Node, Team_Node,Network_Node, Emitter_Node,Capability_Node,Vulnerability_Node }
    public struct NodeTypeStruct
    {
        public string NodeKey;
        public string NodeText;
    }

    public partial class NavP_Types : Ctl_NavigatorPane
    {
        private int _dm_index = 1;
        private int _sp_index = 1;
        private int _en_index = 1;
        private int _sn_index = 1;
        private int _st_index = 1;
        private int _tm_index = 1;

        private const string DM_Node_Str = "DM_Node";
        private const string Species_Node_Str = "Species_Node";
        private const string Define_Engram_Node_Str = "Define_Engram_Node";
        private const string Emitter_Node_Str = "Emitter_Node";
        private const string Sensor_Node_Str = "Sensor_Node";
        private const string State_Node_Str = "State_Node";
        private const string Team_Node_Str = "Team_Node";
        private const string Network_Node_Str = "Network_Node";
        private const string Capability_Node_Str = "Capability_Node";
        private const string Vulnerability_Node_Str = "Vulnerability_Node";


        public TreeNode SelectedNode
        {
            get
            {
                return treeView1.SelectedNode;
            }
        }


        public NavP_Types()
        {
            InitializeComponent();
        }

        private void btnNewNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string name = string.Empty;
                string key = string.Empty;

                TreeNode Node = treeView1.SelectedNode;
                switch (Node.Name)
                {
                    case DM_Node_Str:
                        name = "DecisionMaker";
                        while (((View_ObjectTypes)_view).NameInUse(Types_NodeTypes.DM_Node, name,true))
                        {
                            name = "DecisionMaker" + _dm_index;
                            _dm_index++;
                        }
                        Node.Nodes.Add(name, name, 2, 2);
                        Notify(((View_ObjectTypes)_view).CreateDecisionMaker(name));
                        break;
                    case Species_Node_Str:
                        name = "Species";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 4, 4);
                            Notify(((View_ObjectTypes)_view).CreateSpecies(name));
                        }
                        break;
                    case Define_Engram_Node_Str:
                        name = "Define_Engram";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 6, 6);
                            Notify(((View_ObjectTypes)_view).CreateEngram(name));
                        }
                        break;
                    case Sensor_Node_Str:
                        name = "Sensor";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 5, 5);
                            Notify(((View_ObjectTypes)_view).CreateSensor(name));
                        }
                        break;
                    case State_Node_Str:
                        name = "State";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 3, 3);
                            Notify(((View_ObjectTypes)_view).CreateState(name));
                        }
                        break;
                    case Team_Node_Str:
                        name = "Team";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 5, 5);
                            Notify(((View_ObjectTypes)_view).CreateTeam(name));
                        }
                        break;
                    case Network_Node_Str:
                        name = "Network";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 5, 5);
                            Notify(((View_ObjectTypes)_view).CreateNetwork(name));
                        }
                        break;
                    case Capability_Node_Str:
                        name = "Capability";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 5, 5);
                            Notify(((View_ObjectTypes)_view).CreateCapability(name));
                        }
                        break;
                }
                Node.Expand();
            }
        }

        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode Node = treeView1.SelectedNode;
                if (Node.Parent != null)
                {
                    switch (Node.Parent.Name)
                    {
                        case DM_Node_Str:
                            ((View_ObjectTypes)_view).RemoveDecisionMaker(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case Species_Node_Str:
                            ((View_ObjectTypes)_view).RemoveSpecies(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case Define_Engram_Node_Str:
                            ((View_ObjectTypes)_view).RemoveEngram(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case Sensor_Node_Str:
                            ((View_ObjectTypes)_view).RemoveSensor(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case State_Node_Str:
                            ((View_ObjectTypes)_view).RemoveState(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case Team_Node_Str:
                            Ctl_Teams.Teams.RemoveListItem(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        case Network_Node_Str:
                            Ctl_Networks.Networks.RemoveListItem(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void treeView1_Enter(object sender, EventArgs e)
        {
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CntP_Types type = ((View_ObjectTypes)_view).GetContentPanel();
            if (e.Node.Name.StartsWith(Team_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Team_Node, true);
            }
            else if (e.Node.Name.StartsWith(Network_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Network_Node, true);
            }
            else if (e.Node.Name.StartsWith(DM_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.DM_Node, true);
            }

            else if (e.Node.Name.StartsWith(Species_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Species_Node, true);
            }

            else if (e.Node.Name.StartsWith(Define_Engram_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Define_Engram_Node, true);
            }
            else if (e.Node.Name.StartsWith(Emitter_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Emitter_Node, true);
            }

            else if (e.Node.Name.StartsWith(Sensor_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Sensor_Node, true);
            }
            else if (e.Node.Name.StartsWith(Capability_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Capability_Node, true);
            }

            else if (e.Node.Name.StartsWith(State_Node_Str))
            {
                ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.State_Node, true);
            }



            if (e.Node.Parent != null)
            {
                /* We've changed to a child node, therefore refresh the content display with stored values.
                 * */
                object data = null;
                if (e.Node.Parent.Name.StartsWith(DM_Node_Str))
                {
                 //  data = ((View_ObjectTypes)_view).GetDecisionMaker(e.Node.Index);
                    data = Ctl_DecisionMaker.DecisionMakers.GetFromList(e.Node.Text);


                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.DM_Node, false);
  
                }
                if (e.Node.Parent.Name.StartsWith(Species_Node_Str))
                {
                    data = ((View_ObjectTypes)_view).GetSpecies(e.Node.Index);
                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Species_Node, false);

                }
                if (e.Node.Parent.Name.StartsWith(Define_Engram_Node_Str))
                {
                    data = ((View_ObjectTypes)_view).GetEngram(e.Node.Index);
                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Define_Engram_Node, false);

                }
                if (e.Node.Parent.Name.StartsWith(Emitter_Node_Str))
                {
              //      data = ((View_ObjectTypes)_view).GetEmitter(e.Node.Index);
                    data = Ctl_Emitters.Emitters.GetFromList(e.Node.Text);

                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Emitter_Node, false);

                }
                if (e.Node.Parent.Name.StartsWith(Sensor_Node_Str))
                {
 //                   data = ((View_ObjectTypes)_view).GetSensor(e.Node.Index);
                    data = Ctl_Sensors.Sensors.GetFromList(e.Node.Text);
                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Sensor_Node, false);
                }
                if (e.Node.Parent.Name.StartsWith(State_Node_Str))
                {
                    data = ((View_ObjectTypes)_view).GetState(e.Node.Index);
                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.State_Node, false);

                }
                if (e.Node.Parent.Name.StartsWith(Team_Node_Str))
                {
                    data = Ctl_Teams.Teams.GetFromList(e.Node.Text);

                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Team_Node, false);
                }
                if (e.Node.Parent.Name.StartsWith(Network_Node_Str))
                {
                    data = Ctl_Networks.Networks.GetFromList(e.Node.Text);

                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Network_Node, false);
                }
                if (e.Node.Parent.Name.StartsWith(Capability_Node_Str))
                {
                    data = Ctl_Capabilities.Capabilities.GetFromList(e.Node.Text);

                    ((View_ObjectTypes)_view).GetContentPanel().ShowPanel(Types_NodeTypes.Capability_Node, false);
                }
                _view.UpdateContentPanel(data);

            }
            else
            {
                object data = null;
                if (e.Node.Name.StartsWith(DM_Node_Str))
                {
                    data = DecisionMakerDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Species_Node_Str))
                {
                    data = SpeciesDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Define_Engram_Node_Str))
                {
                    data = EngramDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Emitter_Node_Str))
                {
                    data = EmitterDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Sensor_Node_Str))
                {
                    data = SensorDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(State_Node_Str))
                {
                    data = StateDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Team_Node_Str))
                {
                    data = TeamDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Network_Node_Str))
                {
                    data = NetworkDataStruct.Empty();
                }
                if (e.Node.Name.StartsWith(Capability_Node_Str))
                {
                    data = CapabilityDataStruct.Empty();
                }
                _view.UpdateContentPanel(data);

            }
        }



        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                TreeNode Node = treeView1.SelectedNode;
                if (Node != null)
                {
                    int index = 0;
                    switch (Node.Name)
                    {
                        case "DM_Node":
                            DecisionMakerDataStruct s1 = (DecisionMakerDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s1.ID, s1.ID, 2, 2);
                            index = SelectedNode.Index;
                            break;

                        case "Species_Node":
                            SpeciesDataStruct s2 = (SpeciesDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s2.ID, s2.ID, 4, 4);
                            index = SelectedNode.Index;
                            break;

                        case "Define_Engram_Node":
                            EngramDataStruct s3 = (EngramDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s3.ID, s3.ID, 6, 6);
                            index = SelectedNode.Index;
                            break;

                        case "Sensor_Node":
                            SensorDataStruct s4 = (SensorDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s4.ID, s4.ID, 5, 5);
                            index = SelectedNode.Index;
                            break;



                        case "State_Node":
                            StateDataStruct s5 = (StateDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s5.ID, s5.ID, 3, 3);
                            index = SelectedNode.Index;
                            break;

       
                 case "Team_Node":
                            TeamDataStruct s6 = (TeamDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s6.ID, s6.ID, 3, 3);
                            index = SelectedNode.Index;
                            break;

                        case "Network_Node":
                            NetworkDataStruct s7 = (NetworkDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s7.ID, s7.ID, 3, 3);
                            index = SelectedNode.Index;
                            break;

                        case "Emitter_Node":
                            EmitterDataStruct s8 = (EmitterDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s8.ID, s8.ID, 5, 5);
                            index = SelectedNode.Index;
                            break;
                        case "Capability_Node":
                            CapabilityDataStruct s9 = (CapabilityDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s9.ID, s9.ID, 5, 5);
                            index = SelectedNode.Index;
                            break;

                        default:
                            if (object_data is DecisionMakerDataStruct)
                            {
                                Node.Text = ((DecisionMakerDataStruct)object_data).ID;
                            }
                            if (object_data is SpeciesDataStruct)
                            {
                                Node.Text = ((SpeciesDataStruct)object_data).ID;
                            }
                            if (object_data is EngramDataStruct)
                            {
                                Node.Text = ((EngramDataStruct)object_data).ID;
                            }
                            if (object_data is SensorDataStruct)
                            {
                                Node.Text = ((SensorDataStruct)object_data).ID;
                            }
                            if (object_data is StateDataStruct)
                            {
                                Node.Text = ((StateDataStruct)object_data).ID;
                            }
                            if (object_data is TeamDataStruct)
                            {
                                Node.Text = ((TeamDataStruct)object_data).ID;
                            }
                            if (object_data is NetworkDataStruct)
                            {
                                Node.Text = ((NetworkDataStruct)object_data).ID;
                            }
                            break;
                    }
                    Node.Expand();
                }
            }
        }



    }
}
