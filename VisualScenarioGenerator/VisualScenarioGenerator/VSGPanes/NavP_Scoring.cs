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
    public enum Scoring_NodeTypes : int { Actors, Location, Scoring_Rule, Existence_Rule, Score }
    public partial class NavP_Scoring : Ctl_NavigatorPane
    {
        //private const string Actor_Node_Str = "Actor_Node";
        //private const string Location_Node_Str = "Location_Node";
        private const string Scoring_Rule_Node_Str = "Scoring_Node";
        //private const string Existence_Rule_Node_Str = "Existence_Node";
        private const string Score_Node_Str = "Score_Node";

        public TreeNode SelectedNode
        {
            get
            {
                return treeView1.SelectedNode;
            }
        }

       
        public NavP_Scoring()
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
                    //case Actor_Node_Str:
                    //    name = "Actor";
                    //    Node.Nodes.Add(name, name, 2, 2);
                    //    Notify(((View_Scoring)_view).CreateActor(name));
                    //    break;
                    //case Location_Node_Str:
                    //    name = "Location";
                    //    if (!Node.Nodes.Contains(new TreeNode(name)))
                    //    {
                    //        Node.Nodes.Add(name, name, 4, 4);
                    //        Notify(((View_Scoring)_view).CreateLocation(name));
                    //    }
                    //    break;
                    case Scoring_Rule_Node_Str:
                        name = "ScoringRule";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 6, 6);
                            Notify(((View_Scoring)_view).CreateScoringRule(name));
                        }
                        break;
                    //case Existence_Rule_Node_Str:
                    //    name = "ExistenceRule";
                    //    if (!Node.Nodes.Contains(new TreeNode(name)))
                    //    {
                    //        Node.Nodes.Add(name, name, 5, 5);
                    //        Notify(((View_Scoring)_view).CreateExistenceRule(name));
                    //    }
                    //    break;
                    case Score_Node_Str:
                        name = "Score";
                        if (!Node.Nodes.Contains(new TreeNode(name)))
                        {
                            Node.Nodes.Add(name, name, 3, 3);
                            Notify(((View_Scoring)_view).CreateScore(name));
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
                        //case Actor_Node_Str:
                        //    ((View_Scoring)_view).RemoveActor(Node.Index);
                        //    treeView1.Nodes.Remove(Node);
                        //    break;
                        //case Location_Node_Str:
                        //    ((View_Scoring)_view).RemoveLocation(Node.Index);
                        //    treeView1.Nodes.Remove(Node);
                        //    break;
                        case Scoring_Rule_Node_Str:
                            ((View_Scoring)_view).RemoveScoringRule(Node.Index);
                            treeView1.Nodes.Remove(Node);
                            break;
                        //case Existence_Rule_Node_Str:
                        //    ((View_Scoring)_view).RemoveExistenceRule(Node.Index);
                        //    treeView1.Nodes.Remove(Node);
                        //    break;
                        case Score_Node_Str:
                            ((View_Scoring)_view).RemoveScore(Node.Index);
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
            //if (e.Node.Name.StartsWith(Actor_Node_Str))
            //{
            //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Actors);
            //}

            //if (e.Node.Name.StartsWith(Location_Node_Str))
            //{
            //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Location);
            //}

            if (e.Node.Name.StartsWith(Scoring_Rule_Node_Str))
            {
                ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Scoring_Rule);
            }

            //if (e.Node.Name.StartsWith(Existence_Rule_Node_Str))
            //{
            //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Existence_Rule);
            //}

            if (e.Node.Name.StartsWith(Score_Node_Str))
            {
                ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Score);
            }

            if (e.Node.Parent != null)
            {
                /* We've changed to a child node, therefore refresh the content display with stored values.
                 * */
                object data = null;
                //if (e.Node.Parent.Name.StartsWith(Actor_Node_Str))
                //{
                //    data = ((View_Scoring)_view).GetActor(e.Node.Index);
                //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Actors);

                //}
                //if (e.Node.Parent.Name.StartsWith(Location_Node_Str))
                //{
                //    data = ((View_Scoring)_view).GetLocation(e.Node.Index);
                //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Location);

                //}
                if (e.Node.Parent.Name.StartsWith(Scoring_Rule_Node_Str))
                {
                    data = ((View_Scoring)_view).GetScoringRule(e.Node.Index);
                    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Scoring_Rule);

                }
                //if (e.Node.Parent.Name.StartsWith(Existence_Rule_Node_Str))
                //{
                //    data = ((View_Scoring)_view).GetExistenceRule(e.Node.Index);
                //    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Existence_Rule);

                //}
                if (e.Node.Parent.Name.StartsWith(Score_Node_Str))
                {
                    data = ((View_Scoring)_view).GetScore(e.Node.Index);
                    ((View_Scoring)_view).GetContentPanel().ShowPanel(Scoring_NodeTypes.Score);
                }
                _view.UpdateContentPanel(data);

            }
            else
            {
                object data = null;
                //if (e.Node.Name.StartsWith(Actor_Node_Str))
                //{
                //    data = ActorDataStruct.Empty;
                //}
                //if (e.Node.Name.StartsWith(Location_Node_Str))
                //{
                //    data = LocationDataStruct.Empty;
                //}
                if (e.Node.Name.StartsWith(Scoring_Rule_Node_Str))
                {
                    data = ScoringRuleDataStruct.Empty;
                }
                //if (e.Node.Name.StartsWith(Existence_Rule_Node_Str))
                //{
                //    data = ExistenceRuleDataStruct.Empty;
                //}
                if (e.Node.Name.StartsWith(Score_Node_Str))
                {
                    data = ScoreDataStruct.Empty;
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
                        //case Actor_Node_Str:
                        //    ActorDataStruct s1 = (ActorDataStruct)object_data;
                        //    treeView1.SelectedNode = Node.Nodes.Add(s1.ID, s1.ID, 2, 2);
                        //    index = SelectedNode.Index;
                        //    break;

                        //case Location_Node_Str:
                        //    LocationDataStruct s2 = (LocationDataStruct)object_data;
                        //    treeView1.SelectedNode = Node.Nodes.Add(s2.ID, s2.ID, 4, 4);
                        //    index = SelectedNode.Index;
                        //    break;

                        case Scoring_Rule_Node_Str:
                            ScoringRuleDataStruct s3 = (ScoringRuleDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s3.ID, s3.ID, 6, 6);
                            index = SelectedNode.Index;
                            break;

                        //case Existence_Rule_Node_Str:
                        //    ExistenceRuleDataStruct s4 = (ExistenceRuleDataStruct)object_data;
                        //    treeView1.SelectedNode = Node.Nodes.Add(s4.ID, s4.ID, 5, 5);
                        //    index = SelectedNode.Index;
                        //    break;

                        case Score_Node_Str:
                            ScoreDataStruct s5 = (ScoreDataStruct)object_data;
                            treeView1.SelectedNode = Node.Nodes.Add(s5.ID, s5.ID, 3, 3);
                            index = SelectedNode.Index;
                            break;

                        default:
                            //if (object_data is ActorDataStruct)
                            //{
                            //    Node.Text = ((ActorDataStruct)object_data).ID;
                            //}
                            //if (object_data is LocationDataStruct)
                            //{
                            //    Node.Text = ((LocationDataStruct)object_data).ID;
                            //}
                            if (object_data is ScoringRuleDataStruct)
                            {
                                Node.Text = ((ScoringRuleDataStruct)object_data).ID;
                            }
                            //if (object_data is ExistenceRuleDataStruct)
                            //{
                            //    Node.Text = ((ExistenceRuleDataStruct)object_data).ID;
                            //}
                            if (object_data is ScoreDataStruct)
                            {
                                Node.Text = ((ScoreDataStruct)object_data).ID;
                            }
                            break;
                    }
                    Node.Expand();
                }
            }
        }

    }
}
