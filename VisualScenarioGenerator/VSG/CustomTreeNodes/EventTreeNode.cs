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

using VSG.Controllers;
using AME.Controllers.Base.Data_Structures;


namespace ApplicationNodes
{
    class EventTreeNode : ProcessingNode
    {
        private int myId;
        private String myType;
        private List<Function> functions;
        private String eType;

        public EventTreeNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }





        public virtual void createChild(CustomContextMenuTag clickedItemTag, String[] args)
        {

            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            VSGController myController = (VSGController)myTree.Controller;
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

                ((VSGController)myTree.Controller).TurnViewUpdateOff();


                ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(this.LinkType), addID, args[0], args[0], this.LinkType, "");
                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, args[0], args[0], this.LinkType);
                //ProcessingNode parent = (ProcessingNode)Parent;
                if (NodeType == "CreateEvent")
                {
                    myTree.Controller.Connect(added.ComponentID, added.ComponentID, NodeID, "EventID");
                }
                if (NodeType == "ReiterateEvent")
                {

                    List<int> ids = myController.GetChildIDs(NodeID, "CreateEvent", "EventID");
                    if (ids.Count > 0)
                    {
                        myTree.Controller.Connect(added.ComponentID, added.ComponentID, ids[0], "EventID");
                    }
                }

                ((VSGController)myTree.Controller).TurnViewUpdateOn(true, false); // only do component update
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public virtual void createSibling(CustomContextMenuTag clickedItemTag, String[] args)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            VSGController myController = (VSGController)myTree.Controller;
            try
            {
                int addID;
                addID = ((ProcessingNode)this.Parent).NodeID;

                ((VSGController)myTree.Controller).TurnViewUpdateOff();

                int linkID = this.LinkID != null ? (int)this.LinkID : -1;

                ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(this.LinkType), addID, args[0], linkID, args[0], this.LinkType, "");

                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, args[0], args[0], this.LinkType);

                ProcessingNode parent = (ProcessingNode)Parent;
                if (parent != null && parent.NodeType == "CreateEvent")
                {
                    myTree.Controller.Connect(added.ComponentID, added.ComponentID, parent.NodeID, "EventID");
                }
                if (parent != null && parent.NodeType == "ReiterateEvent")
                {

                    List<int> ids = myController.GetChildIDs(parent.NodeID, "CreateEvent", "EventID");
                    if (ids.Count > 0)
                    {
                        myTree.Controller.Connect(added.ComponentID, added.ComponentID, ids[0], "EventID");
                    }
                }

                ((VSGController)myTree.Controller).TurnViewUpdateOn(true, false); // only do component update
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public virtual void createSubplatform(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            try
            {
                int addID;
                addID = ((ProcessingNode)Parent).NodeID;
                ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(this.LinkType), addID, "Subplatform", "Subplatform", this.LinkType, "");
                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, "Subplatform", "Subplatform", this.LinkType);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public virtual void createArmament(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            try
            {
                int addID;
                addID = NodeID;
                ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(this.LinkType), addID, "Armament", "Armament", this.LinkType, "");
                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, "Armament", "Armament", this.LinkType);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public virtual void moveUp(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            //ProcessingNode prevNode = (ProcessingNode)this.PrevNode;
            //ProcessingNode myNode = (ProcessingNode)this;
            myTree.MoveSelectedNodeUp();


            //String prevName = "";
            //String myName = "";

            //if (prevNode == null)
            //{
            //    return;
            //}

            //int myNum = GetNumFromIDString(myNode.Name);
            //int prevNum = GetNumFromIDString(prevNode.Name);
            //((VSGController)myTree.Controller).ViewUpdateStatus = false;
            //myName = UpdateIDString(prevNum, myNode.Name);
            //prevName = UpdateIDString(myNum, prevNode.Name);

            //((VSGController)myTree.Controller).UpdateComponentName(myTree.GetCustomTreeRootId(this.LinkType), myNode.NodeID, this.LinkType, myName);
            //((VSGController)myTree.Controller).UpdateComponentName(myTree.GetCustomTreeRootId(this.LinkType), prevNode.NodeID, this.LinkType, prevName);
            //((VSGController)myTree.Controller).ViewUpdateStatus = true;
        }
        public virtual void moveDown(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            //ProcessingNode nextNode = (ProcessingNode)this.NextNode;
            //ProcessingNode myNode = (ProcessingNode)this;
            myTree.MoveSelectedNodeDown();

            //String nextName = "";
            //String myName = "";
            //
            //if (nextNode == null)
            //{
            //    return;
            //}

            //int myNum = GetNumFromIDString(myNode.Name);
            //int nextNum = GetNumFromIDString(nextNode.Name);
            //((VSGController)myTree.Controller).ViewUpdateStatus = false;
            //myName = UpdateIDString(nextNum, myNode.Name);
            //nextName = UpdateIDString(myNum, nextNode.Name);

            //((VSGController)myTree.Controller).UpdateComponentName(myTree.GetCustomTreeRootId(this.LinkType), myNode.NodeID, this.LinkType, myName);
            //((VSGController)myTree.Controller).UpdateComponentName(myTree.GetCustomTreeRootId(this.LinkType), nextNode.NodeID, this.LinkType, nextName);
            //((VSGController)myTree.Controller).ViewUpdateStatus = true;

        }
        public virtual void deleteEvent(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            ProcessingNode myNode = (ProcessingNode)this;
            myNode.DeleteComponent(clickedItemTag);
        }
        public virtual void deleteCreateEvent(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            ProcessingNode myNode = (ProcessingNode)this;
            VSGController myController = (VSGController)myTree.Controller;
            List<String> ignore = new List<string>();
            ignore.Add("DecisionMaker");

            myController.TurnViewUpdateOff();

            myController.DeleteComponentAndChildren(myNode.NodeID, "Scenario", ignore);
            //myNode.DeleteComponent(clickedItemTag);

            myController.TurnViewUpdateOn();
        }

        public virtual void copyCreateEvent(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            ProcessingNode myNode = (ProcessingNode)this;
            VSGController myController = (VSGController)myTree.Controller;
            try
            {
                String name = String.Format("{0}_copy", myController.GetComponentName(myNode.NodeID));
                List<String> ignoreComp = new List<string>();
                ignoreComp.Add("DecisionMaker");

                myController.TurnViewUpdateOff();
                myController.EventClone(myTree.GetCustomTreeRootId(this.LinkType), myNode.NodeID, name, "Scenario", ignoreComp, myTree.GetCustomTreeRootId(this.LinkType), "Scenario", -1, -1);
                myController.TurnViewUpdateOn(false, false); // don't push out events

                myTree.UpdateViewComponent(); // manaully update
            }
            catch (Exception e)
            {
                myController.TurnViewUpdateOn(false, false); // don't push out events
                myTree.UpdateViewComponent(); // manaully update
                System.Windows.Forms.MessageBox.Show(e.Message, "Duplication Error");
            }
        }

        public virtual void copyMultipleCreateEvent(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            ProcessingNode myNode = (ProcessingNode)this;
            VSGController myController = (VSGController)myTree.Controller;
            int incrementedSuffix = 0;
            int counter = 0;
            int numberOfDuplicates;
            VSG.Dialogs.SingleIntegerInputDialog input = new VSG.Dialogs.SingleIntegerInputDialog("How many duplicates would you like to create?", "Amount of duplication");
            if (input.ShowDialog(out numberOfDuplicates) == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                String baseName = myController.GetComponentName(myNode.NodeID);
                String insertName;
                List<String> ignoreComp = new List<string>();

                ignoreComp.Add("DecisionMaker");

                List<string> componentNames = new List<string>();
                System.Data.DataTable dt = myController.GetComponentTable();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    componentNames.Add(dr["name"].ToString());    
                }

                myController.TurnViewUpdateOff();

                while (counter < numberOfDuplicates)
                {
                    insertName = String.Format("{0}_{1}", baseName, incrementedSuffix);
                    while (componentNames.Contains(insertName))
                    {
                        insertName = String.Format("{0}_{1}", baseName, ++incrementedSuffix);
                    }
                    myController.EventClone(myTree.GetCustomTreeRootId(this.LinkType), myNode.NodeID, insertName, "Scenario", ignoreComp, myTree.GetCustomTreeRootId(this.LinkType), "Scenario", -1, -1);
                    componentNames.Add(insertName);
                    incrementedSuffix++;
                    counter++;
                }

                myController.TurnViewUpdateOn(false, false); // don't push out events

                myTree.UpdateViewComponent(); // manaully update
            }
            catch (Exception e)
            {
                myController.TurnViewUpdateOn(false, false); // don't push out events
                myTree.UpdateViewComponent(); // manaully update
                System.Windows.Forms.MessageBox.Show(e.Message, "Duplication Error");
            }
        }

        public override void process(Object dragTarget) { } // do nothing
    }
}

