using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using System.Windows.Forms;
using AME.Model;
using AME.Views.View_Components;
using Forms;
using System.Drawing;
using System.Xml.XPath;
using AME.Nodes;
using AME.Controllers.Base.Data_Structures;
using VSG.Controllers;



namespace ApplicationNodes
{
    class PlayerObjectTypesTreeNode : ProcessingNode
    {
        private int myId;
        private String myType;
        private List<Function> functions;
        private String eType;

        public PlayerObjectTypesTreeNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        public virtual void deleteDecisionMaker(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            VSGController myController = (VSGController)myTree.Controller;
            List<int> ids = myController.GetChildIDs(myController.ScenarioId, "CreateEvent", "Scenario");
            List<int> dmIDs;
            bool stopDelete = false;
            foreach (int id in ids)
            {
                dmIDs = myController.GetChildIDs(id, "DecisionMaker", "Scenario");
                if (dmIDs.Contains(NodeID))
                {
                    stopDelete = true;
                }

            }

            if (stopDelete)
            {
                MessageBox.Show("Warning: Cannot delete Decision Maker because it owns units.\nPlease delete or reassign units with the Scenario Director first.");
                return;
            }
            this.DeleteComponent(clickedItemTag);
        }

        public virtual void createObjectInstance(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;

            // bring up a form for input
            //InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), clickedItemTag.GetNode().NodeType); // create the dialog
            InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), "CreateEvent"); // create the dialog
            tempInput.StartPosition = FormStartPosition.Manual;
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            tempInput.Location = new Point(x, y);

            DialogResult result = tempInput.ShowDialog(myTree);

            if (result == DialogResult.OK)
            {
                // create a resource
                String name = tempInput.TopInputFieldValue;

                if (NodeType.ToLowerInvariant() != name.ToLowerInvariant())
                {
                    try
                    {
                        int addID;
                        if (NodeID >= 0) // fake nodes are -1
                        {
                            addID = NodeID;
                        }
                        else
                        {
                            addID = myTree.GetCustomTreeRootId(this.LinkType);
                        }
                        //int added = myTree.Controller.AddComponent(GetCustomTreeRootId(this.LinkType), addID, NodeType, tempInput.TopInputFieldValue, this.LinkType, tempInput.BottomInputFieldValue);
                        //myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added, NodeType, tempInput.TopInputFieldValue);
                        //int ceId = myTree.Controller.CreateComponent("CreateEvent", tempInput.TopInputFieldValue, tempInput.BottomInputFieldValue);
                        ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(this.LinkType), myTree.GetCustomTreeRootId(this.LinkType), "CreateEvent", tempInput.TopInputFieldValue, this.LinkType, tempInput.BottomInputFieldValue);
                        int ceLinkId = myTree.Controller.Connect(myTree.GetCustomTreeRootId(this.LinkType), added.ComponentID, clickedItemTag.GetNode().NodeID, this.LinkType);
                        myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, ceLinkId, "CreateEvent", tempInput.TopInputFieldValue, this.LinkType);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot enter item with same name as parent type");
                }
            }
        }
        public override void process(Object dragTarget) { } // do nothing
    }
}
