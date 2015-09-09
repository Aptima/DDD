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
using AME.Controllers.Base.Data_Structures;

namespace AME.Nodes
{
    public abstract class ProcessingNode : TreeNode
    {
        private int myId;
        private String myType;
        private List<Function> functions;
        private String eType;
        private int? linkID = null; // the 'to' link that formed this node, optional (could this be require?)
        private String linkType;
        protected Boolean useDescription = true;

        public ProcessingNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String linkType)
            : base()
        {
            this.myId = p_id;

            this.myType = p_type;

            this.ImageKey = p_imagePath;   // displayed icon is based on image path
            this.SelectedImageKey = p_imagePath;

            this.Name = p_name;
            this.Text = p_name;
            
            this.linkType = linkType;

            this.functions = new List<Function>();

            this.eType = p_eType;
        }

        //default node function implementions - override to change
        public virtual void DeleteLink(CustomContextMenuTag clickedItemTag)
        {
            if (this.LinkID.HasValue)
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;
                if (myTree != null && this.LinkID.HasValue)
                {
                    try
                    {
                        if (myTree.Controller.ViewUpdateStatus == false)
                        {
                            myTree.DeleteNodeWithID(this.LinkID.Value, false, false); // updating is off, don't force
                        }
                        else
                        {
                            myTree.DeleteNodeWithID(this.LinkID.Value, true, false);
                        }

                        myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        //default node function implementions - override to change
        public virtual void DeleteSingleLink(CustomContextMenuTag clickedItemTag)
        {
            if (this.LinkID.HasValue)
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;
                if (myTree != null && this.LinkID.HasValue)
                {
                    try
                    {
                        if (myTree.Controller.ViewUpdateStatus == false)
                        {
                            myTree.DeleteNodeWithID(this.LinkID.Value, false, false, false); // updating is off, don't force
                        }
                        else
                        {
                            myTree.DeleteNodeWithID(this.LinkID.Value, true, false, false);
                        }

                        myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public virtual void DeleteSingleComponent(CustomContextMenuTag clickedItemTag)
        {
            try
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;

                myTree.Controller.DeleteComponent(NodeID);

                myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Delete component and all children, following ALL linktypes
        /// </summary>
        /// <param name="clickedItemTag"></param>
        public virtual void DeleteComponentAndAllChildren(CustomContextMenuTag clickedItemTag)
        {
            try
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;

                myTree.Controller.DeleteComponentAndChildren(NodeID);
                
                myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Delete component and all children, following tree's linktype
        /// </summary>
        /// <param name="clickedItemTag"></param>
        public virtual void DeleteComponentAndLinkTypeChildren(CustomContextMenuTag clickedItemTag)
        {
            try
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;

                myTree.Controller.DeleteComponentAndChildren(NodeID, LinkType);

                myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public virtual void DeleteComponentAndLinkTypeChildren(CustomContextMenuTag clickedItemTag, Object[] args)
        {
            for (int i = 0, length = args.Length; i < length; i++)
            {
                try
                {
                    CustomTreeView myTree = (CustomTreeView)this.TreeView;

                    myTree.Controller.DeleteComponentAndChildren(NodeID, (String)args[i]);

                    myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public virtual void DeleteComponent(CustomContextMenuTag clickedItemTag)
        {
            try
            {
                CustomTreeView myTree = (CustomTreeView)this.TreeView;

                if (myTree.Controller.ViewUpdateStatus == false)
                {
                    myTree.DeleteNodeWithID(NodeID, false, true); // updating is off, don't force
                }
                else
                {
                    myTree.DeleteNodeWithID(NodeID, true, true);
                }

                myTree.OnItemDelete(clickedItemTag.GetFunctionValue(), this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public virtual void RenameComponent(CustomContextMenuTag clickedItemTag, Object[] args)
        {
            InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), clickedItemTag.GetNode().myType);
            tempInput.StartPosition = FormStartPosition.Manual;
            tempInput.ButtonAddText = "OK";
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            tempInput.Location = new Point(x, y);

            if (args.Length >= 1)
            {
                tempInput.AlphaNumericTop = Boolean.Parse((String)args[0]);
            }

            if (args.Length >= 2)
            {
                tempInput.AlphaNumericBottom = Boolean.Parse((String)args[0]);
            }

            CommonRename(tempInput, clickedItemTag);
        }

        public virtual void RenameComponent(CustomContextMenuTag clickedItemTag)
        {
            // bring up a form for input
            InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), clickedItemTag.GetNode().myType); // create the dialog
            tempInput.StartPosition = FormStartPosition.Manual;
            tempInput.ButtonAddText = "OK";
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            tempInput.Location = new Point(x, y);

            CommonRename(tempInput, clickedItemTag);
        }

        public void CommonRename(InputForm tempInput, CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 0;
            IXPathNavigable component = myTree.Controller.GetComponentAndChildren(NodeID, LinkType, compOptions);

            XPathNavigator componentNav = component.CreateNavigator();
            componentNav = componentNav.SelectSingleNode(String.Format("//*[@ID='{0}']", NodeID));

            string name = componentNav.GetAttribute("Name", componentNav.NamespaceURI);
            string description = componentNav.GetAttribute("Description", componentNav.NamespaceURI);

            tempInput.TopInputFieldValue = name;
            tempInput.BottomInputFieldValue = description;

            DialogResult result = tempInput.ShowDialog(myTree);

            if (result == DialogResult.OK)
            {
                String newName = tempInput.TopInputFieldValue;
                String newDescription = tempInput.BottomInputFieldValue;

                if (!newName.Equals(name))
                {
                    try
                    {
                        //myTree.RootID
                        myTree.Controller.UpdateComponentName(myTree.GetCustomTreeRootId(LinkType), NodeID, LinkType, newName);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to update name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if (!newDescription.Equals(description))
                {
                    try
                    {
                        myTree.Controller.UpdateComponentDescription(NodeID, newDescription);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to update description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                myTree.OnItemRename(this.NodeID, this.NodeType, newName, newDescription);
            }
        }

        public virtual void CreateComponent(CustomContextMenuTag clickedItemTag, Object[] args)
        {
            String type = clickedItemTag.GetNode().myType;

            if (args.Length == 1)
            {
                String var = args[0].ToString();
                Boolean useDesc = Boolean.Parse(var);
                this.useDescription = useDesc;
                this.CreateComponent(clickedItemTag);
                this.useDescription = true; // reset
            }
            else if (args.Length >= 2)
            {
                if (args.Length == 3)
                {
                    type = (String)args[2];
                }

                // bring up a form for input
                InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), type); // create the dialog
                tempInput.StartPosition = FormStartPosition.Manual;
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;
                tempInput.Location = new Point(x, y);

                if (args.Length >= 1)
                {
                    tempInput.AlphaNumericTop = Boolean.Parse((String)args[0]);
                }

                if (args.Length >= 2)
                {
                    tempInput.AlphaNumericBottom = Boolean.Parse((String)args[0]);
                }

                if (!useDescription)
                {
                    tempInput.HideDescriptionField();
                }

                CommonCreate(tempInput, clickedItemTag, type);
            }
        }

        public virtual void CommonCreate(InputForm tempInput, CustomContextMenuTag clickedItemTag, String type)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
            DialogResult result = tempInput.ShowDialog(myTree);

            if (result == DialogResult.OK)
            {
                // create a resource
                String name = tempInput.TopInputFieldValue;

                if (NodeType.ToLowerInvariant() != name.ToLowerInvariant())
                {
                    try
                    {
                        int addID = -1;
                        if (NodeID >= 0) // fake nodes are -1
                        {
                            addID = NodeID;
                        }
                        else // find the next parent with non negative ID
                        {
                            bool IDFound = false;

                            object parentTest = this.Parent;

                            while (parentTest != null)
                            {
                                if (parentTest is ProcessingNode && !IDFound)
                                {
                                    ProcessingNode cast = (ProcessingNode)parentTest;
                                    if (cast.NodeID >= 0)
                                    {
                                        IDFound = true;
                                        addID = cast.NodeID;
                                    }
                                    else
                                    {
                                        parentTest = cast.Parent; // keep looking
                                    }
                                }
                                else
                                {
                                    parentTest = null;
                                }
                            }

                            if (!IDFound)
                            {
                                addID = myTree.GetCustomTreeRootId(LinkType);// myTree.RootID; // if no valid parent can be found, use the rootID of the tree
                            }
                        }
                        //myTree.RootID
                        ComponentAndLinkID added = myTree.Controller.AddComponent(myTree.GetCustomTreeRootId(LinkType), addID, type, tempInput.TopInputFieldValue, LinkType, tempInput.BottomInputFieldValue);
                        myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, type, tempInput.TopInputFieldValue, LinkType);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (myTree.Controller != null)
                        {
                            if (!myTree.Controller.ViewUpdateStatus)
                            {
                                // turn updating back on
                                myTree.Controller.TurnViewUpdateOn(false, false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cannot enter item with same name as parent type");
                }
            }
        }

        public virtual void CreateComponent(CustomContextMenuTag clickedItemTag)
        {
            // bring up a form for input
            String type = clickedItemTag.GetNode().myType;
            InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), type); // create the dialog
            tempInput.StartPosition = FormStartPosition.Manual;
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            tempInput.Location = new Point(x, y);

            if (!useDescription)
            {
                tempInput.HideDescriptionField();
            }

            CommonCreate(tempInput, clickedItemTag, type);
        }

        public virtual void AddClass(CustomContextMenuTag clickedItemTag)
        {
            CustomTreeView myTree = (CustomTreeView)this.TreeView;
 
            // bring up a form for input
            InputForm tempInput = new InputForm(clickedItemTag.GetFunctionName(), clickedItemTag.GetNode().myType); // create the dialog
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
                            addID = myTree.GetCustomTreeRootId(LinkType);// myTree.RootID;
                        }
                        //myTree.RootID
                        ComponentAndLinkID added = myTree.Controller.AddComponentClass(myTree.GetCustomTreeRootId(LinkType), addID, NodeType, tempInput.TopInputFieldValue, LinkType, tempInput.BottomInputFieldValue);
                        myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), added.ComponentID, added.LinkID, NodeType, tempInput.TopInputFieldValue, LinkType);
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

        public virtual void AddListItem(CustomContextMenuTag clickedItemTag)
        {
            // C.K.
            // Implementation should be the same for all functions.
            // It seems this call just acts as a pass through to the application side ti implement the behavior.
            // Other calls here have base behavior.
            // We need to streamline this. Lets provide a method to easily extend this class and override behavior.

            CustomTreeView myTree = (CustomTreeView)this.TreeView;

            // C.K.
            // We should call something else besides OnItemAdd. This can be confusing since we are not
            // adding anything here. Instead of piggybacking on this we could create OnCreateList...
            // Also, we have duplicate functionality:
            //   XML - Argument elements under Function elements
            //   Attribute parsing - action(arg1, agr2)
            // Do we really need both? Lets adopt one way and go with it.
            if (clickedItemTag.Arguments.Length > 0)
                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), NodeID, NodeID, clickedItemTag.Arguments[0], Name, LinkType);

            else
                myTree.OnItemAdd(clickedItemTag.GetFunctionValue(), NodeID, NodeID, NodeType, Name, LinkType);

        }

        public int NodeID
        {
            get { return myId; }
            //set { myId = value; }
        }

        public String NodeType
        {
            get { return myType; }
           // set { myType = value; }
        }

        public List<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        public String EType
        {
            get { return eType; }
            //set { eType = value; }
        }

        public int? LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        public String LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }

        public abstract void process(Object dragTarget);
    }

    public class Function
    {
        private String m_functionName, m_functionAction;
        private List<String> m_arguments;

        private Boolean m_visible;
        private Boolean m_enabled;

        public Function(String p_functionName, String p_functionAction, Boolean p_visible)
        {
            m_functionName = p_functionName;
            m_functionAction = p_functionAction;
            m_arguments = new List<String>();
            m_visible = p_visible;
            m_enabled = true;
        }

        public String FunctionAction
        {
            get { return m_functionAction; }
            set { m_functionAction = value; }
        }

        public String FunctionName
        {
            get { return m_functionName; }
            set { m_functionName = value; }
        }

        public String[] Arguments
        {
            get
            {
                return m_arguments.ToArray();
            }
            set
            {
                m_arguments = new List<String>(value);
            }
        }

        public Boolean Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        public Boolean Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }
    }
}
