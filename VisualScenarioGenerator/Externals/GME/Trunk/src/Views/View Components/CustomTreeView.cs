using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Threading;
using System.Drawing;
using AME.Controllers;
using AME.Model;
using Forms;
using System.Xml;
using AME.Views.View_Component_Packages;
using AME.Views.View_Components;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Xml.XPath;
using System.Reflection;
using AME.Nodes;

namespace AME.Views.View_Components
{
    public delegate void NodeFunctionListEventHandler(object sender, NodeFunctionListEventArgs e);

    // A generic treeview 
    public class CustomTreeView : TreeView, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private XPathExpression imageCategoryParameter = XPathExpression.Compile("ComponentParameters/Parameter/Parameter");
        private String categoryMatch = "Image";
        private String displayedNameAttr = "displayedName";
        private String categoryAttr = "category";
        private Boolean branching = false;

        private Dictionary<String, Boolean> isIDExpanded; // An Expansion Dictionary, maps path+index to expanded state t/f
        private Dictionary<int, int> topNodes;
        //private Dictionary<String, ImageList> configToImageList; // map configuration to an image list (allow duplicate component type names 
        // across configurations with different images)
        protected IController myController;
        //protected String xsl;
        //protected XslCompiledTransform transform;
        //protected int m_DisplayID = -1;
        protected String crc = "";
        //private int m_RootID = -1;
        //public int RootID
        //{
        //    get { return m_RootID; }
        //    set { m_RootID = value; }
        //}
        //public int DisplayID
        //{
        //    get { return m_DisplayID; }
        //    set
        //    {
        //        if (m_DisplayID != value)
        //        {
        //            CheckExpansion(); // save before changing and signal change
        //            idChange = true;
        //        }
        //        m_DisplayID = value;
        //    }
        //}
        
        //protected String m_LinkType;
        private Dictionary<String, CustomTreeRoot> roots;
        //public CustomTreeRoot GetCustomTreeRoot(String linkType)
        //{
        //    CustomTreeRoot root = null;
        //    if (roots.TryGetValue(linkType, root))
        //        return root;
        //    return root;
        //}
        public void ClearRoots()
        {
            roots.Clear();
        }

        public void AddCustomTreeRoot(String linkType)
        {
            if (!roots.ContainsKey(linkType))
            {
                CustomTreeRoot root = new CustomTreeRoot();
                root.LinkType = linkType;
                root.RootId = -1;
                root.DisplayId = -1;
                root.MyTree = this;
                roots.Add(linkType, root);
            }
        }
        public void RemoveCustomTreeRoot(String linkType)
        {
            if (roots.ContainsKey(linkType))
                roots.Remove(linkType);
        }
        public void ChangeCustomTreeRoot(String newLinkType, String oldLinkType)
        {
            CustomTreeRoot oldRoot = null;
            if (roots.TryGetValue(oldLinkType, out oldRoot))
            {
                CustomTreeRoot newRoot = new CustomTreeRoot();
                newRoot.LinkType = newLinkType;
                newRoot.RootId = oldRoot.RootId;
                newRoot.DisplayId = oldRoot.DisplayId;
                roots.Remove(oldLinkType);
                roots.Add(newLinkType, newRoot);
            }
        }
        public void SetCustomTreeRootId(String linkType, Int32 id)
        {
            CustomTreeRoot root = null;
            if (roots.TryGetValue(linkType, out root))
            {
                root.RootId = id;
            }
        }
        public Int32 GetCustomTreeRootId(String linkType)
        {
            CustomTreeRoot root = null;
            Int32 id = -1;
            if (roots.TryGetValue(linkType, out root))
                id = root.RootId;
            return id;
        }
        public void SetCustomTreeRootDisplayId(String linkType, Int32 id)
        {
            CustomTreeRoot root = null;
            if (roots.TryGetValue(linkType, out root))
            {
                root.DisplayId = id;
            }
        }
        public Int32 GetCustomTreeDisplayId(String linkType)
        {
            CustomTreeRoot root = null;
            Int32 id = -1;
            if (roots.TryGetValue(linkType, out root))
                id = root.DisplayId;
            return id;
        }
        public void SetCustomTreeRootXsl(String linkType, String xsl)
        {
            CustomTreeRoot root = null;
            if (roots.TryGetValue(linkType, out root))
            {
                root.Xsl = xsl;
            }
        }
        public Dictionary<Int32, List<ProcessingNode>> GetCustomTreeNodeMap(String linkType)
        {
            CustomTreeRoot root = null;
            if (roots.TryGetValue(linkType, out root))
                return root.GetNodeMap();
            return null;
        }

        private uint m_level = 0;
        public uint Level
        {
            get { return m_level; }
            set { m_level = value; }
        }
        private bool useNodeMap = false;
        public bool UseNodeMap
        {
            get { return useNodeMap; }
            set { useNodeMap = value; }
        }
        private bool decorateNodes = false;
        public bool DecorateNodes
        {
            get { return decorateNodes; }
            set { decorateNodes = value; }
        }

        protected string IDAttribute, NameAttribute, TypeAttribute, DescriptionAttribute, ETypeAttribute, LinkIDAttribute, LinkTypeAttribute;
        private TreeNode previousToolTipNode;
        private ToolTip treeToolTip;

        private Boolean showRoot = true;
        public Boolean ShowRoot
        {
            get { return showRoot; }
            set { showRoot = value; }
        }

        private Boolean m_allowInput = true;

        public Boolean AllowUserInput // response to key and mouse clicks
        {
            get { return m_allowInput; }
            set { m_allowInput = value; }
        }

        public CustomTreeView()
            : base()
        {
            //LinkTypes = new LinkTypeCollection(this);
            roots = new Dictionary<String, CustomTreeRoot>();

            myHelper = new ViewComponentHelper(this);

            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;
            TypeAttribute = XmlSchemaConstants.Display.Component.Type;
            DescriptionAttribute = XmlSchemaConstants.Display.Component.Description;
            ETypeAttribute = XmlSchemaConstants.Display.Component.eType;
            LinkIDAttribute = XmlSchemaConstants.Display.Component.LinkID;
            LinkTypeAttribute = XmlSchemaConstants.Display.Link.Type;

            this.HideSelection = false;

            InitializeComponent();

            isIDExpanded = new Dictionary<String, bool>();
            topNodes = new Dictionary<int, int>();
            //configToImageList = new Dictionary<string, ImageList>();

            treeToolTip = new ToolTip();
            treeToolTip.AutoPopDelay = 10000;
            treeToolTip.InitialDelay = 1000;
            treeToolTip.ReshowDelay = 1000;

            this.Dock = DockStyle.Fill;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, false);

            this.ItemDrag += new ItemDragEventHandler(CustomTreeView_ItemDrag);
            this.NodeMouseHover += new TreeNodeMouseHoverEventHandler(CustomTreeView_NodeMouseHover);

            // key, mouse events 

            this.MouseClick += new MouseEventHandler(CustomTreeView_MouseClick); // for selection

            this.KeyDown += new KeyEventHandler(CustomTreeView_KeyDown);
            this.NodeMouseClick += new TreeNodeMouseClickEventHandler(CustomTreeView_NodeMouseClick);

            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenuStrip_ItemClicked);
        }

        // selection is reloaded on update, so clone if we're doing anything that could cause
        // an update
        private List<ProcessingNode> CloneSelectedNodes()
        {
            List<ProcessingNode> clonedList = new List<ProcessingNode>();
            foreach (ProcessingNode selected in selectedNodes)
            {
                clonedList.Add(selected);
            }
            return clonedList;
        }

        // key, mouse events
        protected void CustomTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (AllowUserInput)
            {
                if (e.KeyCode == Keys.Delete) // delete pressed
                {
                    myController.UnregisterForUpdate(this);
                    myController.TurnViewUpdateOff();
                    List<ProcessingNode> forDelete = CloneSelectedNodes();
                    foreach (ProcessingNode delNode in forDelete)
                    {
                        List<Function> functions = delNode.Functions;

                        bool found = false;
                        foreach (Function f in functions)
                        {
                            switch (f.FunctionAction)
                            {
                                case ConfigFileConstants.DeleteLink:
                                    found = true;
                                    delNode.DeleteLink(new CustomContextMenuTag(delNode, f.FunctionName, f.FunctionAction));
                                    break;
                                case ConfigFileConstants.DeleteComponent:
                                    found = true;
                                    delNode.DeleteComponent(new CustomContextMenuTag(delNode, f.FunctionName, f.FunctionAction));
                                    break;
                            }
                        }

                        if (!found)
                        {
                            foreach (Function f in functions)
                            {
                                // try reflection delete
                                if (f.FunctionAction.Contains("Delete"))
                                {
                                    BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                                    MethodInfo[] methods = delNode.GetType().GetMethods(flags);

                                    foreach (MethodInfo aMethod in methods)
                                    {
                                        if (aMethod.Name == f.FunctionAction)
                                        {
                                            try
                                            {
                                                CustomContextMenuTag customTag = new CustomContextMenuTag(delNode, f.FunctionName, f.FunctionAction);
                                            
                                                Object returnObject = aMethod.Invoke(delNode, new object[] { customTag  });

                                                break;
                                            }
                                            catch (TargetParameterCountException tpce)
                                            {
                                                MessageBox.Show("Could not match specified arguments to function " + f.FunctionAction, tpce.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    myController.RegisterForUpdate(this);
                    myController.TurnViewUpdateOn();
                }
                else if (e.KeyCode == Keys.Up && e.Shift)
                {
                    MoveSelectedNodeUp();
                }
                else if (e.KeyCode == Keys.Down && e.Shift)
                {
                    MoveSelectedNodeDown();
                }
                else if (e.KeyCode == Keys.Up) // select previous node
                {
                    if (selectedNodes.Count == 1)
                    {
                        ProcessingNode currentSelect = selectedNodes[0];

                        ProcessingNode previous = (ProcessingNode)currentSelect.PrevVisibleNode;

                        if (previous != null)
                        {
                            UnpaintSelectedNodes();
                            selectedNodes.Clear();
                            selectedNodes.Add(previous);
                            PaintSelectedNodes();
                            OnAfterSelect(new TreeViewEventArgs(previous));
                        }
                    }
                }
                else if (e.KeyCode == Keys.Down) // select next node
                {
                    if (selectedNodes.Count == 1)
                    {
                        ProcessingNode currentSelect = selectedNodes[0];

                        ProcessingNode next = (ProcessingNode)currentSelect.NextVisibleNode;

                        if (next != null)
                        {
                            UnpaintSelectedNodes();
                            selectedNodes.Clear();
                            selectedNodes.Add(next);
                            PaintSelectedNodes();
                            OnAfterSelect(new TreeViewEventArgs(next));
                        }
                    }
                }
            }
        }

        public void MoveSelectedNodeUp()
        {
            if (selectedNodes.Count == 1)
            {
                ProcessingNode moveMe = selectedNodes[0];
                if (moveMe.LinkID.HasValue && moveMe.Index > 0)
                {
                    this.Controller.IncrementLink(moveMe.LinkID.Value);
                }
            }
        }

        public void MoveSelectedNodeDown()
        {
            if (selectedNodes.Count == 1)
            {
                ProcessingNode moveMe = selectedNodes[0];
                if (moveMe.LinkID.HasValue)
                {
                    if (moveMe.Index < moveMe.Parent.Nodes.Count - 1)
                    {
                        this.Controller.DecrementLink(moveMe.LinkID.Value);
                    }
                }
            }
        }

        // Define an event based on the above delegate
        public event NodeFunctionListEventHandler NodeFunctionList;

        protected virtual void OnNodeFunctionList(NodeFunctionListEventArgs e)
        {
            NodeFunctionListEventHandler handler = NodeFunctionList;
            if (handler != null)
            {
                handler(this, e); 
            }
        }

        protected void CustomTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs args)
        {
            if (AllowUserInput)
            {
                if (args.Button == MouseButtons.Right) // right click
                {
                    this.ContextMenuStrip.Items.Clear();

                    // detect node click
                    if (args.Node != null && (args.X >= args.Node.Bounds.Left) && (args.X <= args.Node.Bounds.Right) &&
                            (args.Y >= args.Node.Bounds.Top) && (args.Y <= args.Node.Bounds.Bottom))
                    {
                        ProcessingNode cast = (ProcessingNode)args.Node;
       
                        // handle right click by populating with node functions
                        List<Function> functions = cast.Functions;

                        if (functions != null)
                        {
                            ToolStripItem newItem;

                            if (functions.Count > 0)
                            {
                                NodeFunctionListEventArgs e = new NodeFunctionListEventArgs(cast, functions);
                                OnNodeFunctionList(e);
                            }

                            foreach (Function f in functions)
                            {
                                if (f.Visible)
                                {
                                    newItem = this.ContextMenuStrip.Items.Add(f.FunctionName);
                                    if (!f.Enabled)
                                        newItem.Enabled = f.Enabled;
                                    CustomContextMenuTag customTag = new CustomContextMenuTag(cast, f.FunctionName, f.FunctionAction);
                                    customTag.Arguments = f.Arguments;
                                    newItem.Tag = customTag;
                                }
                            }
                        }
                    } // arg check
                } // right click
            } // input
        }

        private void ContextMenuStrip_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;

            CustomContextMenuTag tag = (CustomContextMenuTag)item.Tag;

            if (tag != null)
            {
                String functionValue = tag.GetFunctionValue();

                string[] args = null;

                if (functionValue.Contains("(")) // does the function have arguments?
                {
                    // build them
                    int leftParen = functionValue.IndexOf("(");
                    int leftParenPlusPlus = leftParen+1;
                    int rightParen = functionValue.IndexOf(")");

                    String functionArgs = functionValue.Substring(leftParenPlusPlus, rightParen - leftParenPlusPlus);

                    args = functionArgs.Split(new char[] { ',' }); // split on comma to parse args

                    for (int i = 0; i < args.Length; i++) // trim any spaces
                    {
                        String argTemp = args[i];
                        args[i] = argTemp.Trim();
                    }

                    functionValue = functionValue.Substring(0, leftParen); // set functionValue to just the method name
                }

                // process function by reflection

                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

                MethodInfo[] methods = tag.GetNode().GetType().GetMethods(flags);
                int paramLength = 0;
                foreach (MethodInfo aMethod in methods)
                {
                    if (aMethod.Name == functionValue) 
                    {
                        try
                        {
                            paramLength = aMethod.GetParameters().Length;
                            if (args != null)
                            {
                                if (paramLength == 2)
                                {
                                    // pass tag, args
                                    Object returnObject = aMethod.Invoke(tag.GetNode(), new object[] { tag, args });
                                    break;
                                }
                            }
                            else
                            {
                                if (paramLength == 1)
                                {
                                    // pass tag
                                    Object returnObject = aMethod.Invoke(tag.GetNode(), new object[] { tag });
                                    break;
                                }
                            }
                        }
                        catch (TargetParameterCountException tpce)
                        {
                            MessageBox.Show("Could not match specified arguments to function " + functionValue, tpce.Message);
                        }
                    }
                }

                // send an event to anyone who could listen/process from outside the tree
                OnFunctionProcessed(tag.GetNode(), tag.GetFunctionName(), functionValue);
            }
        }

        public void ClearExpansionState()
        {
            if (isIDExpanded != null && topNodes != null)// && configToImageList != null)
            {
                isIDExpanded.Clear();
                topNodes.Clear();
                //configToImageList.Clear();
            }
        }

        protected List<ProcessingNode> selectedNodes = new List<ProcessingNode>();

        public ProcessingNode LastNodeSelected
        {
            get
            {
                if (selectedNodes.Count == 1)
                {
                    return selectedNodes[0];
                }
                else
                {
                    return null;
                }
            }
        }

        private Color fc;
        private Color bc;
        private bool colorsSet = false;

        private void CustomTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            ProcessingNode n = (ProcessingNode)this.GetNodeAt(e.Location);
            if (n != null)
            {
                if (!colorsSet)
                {
                    fc = new Color();
                    bc = new Color();

                    colorsSet = true;
                }

                if (ModifierKeys == Keys.Shift)
                {
                    if (LastNodeSelected != null && !n.Equals(LastNodeSelected))
                    {
                        if (LastNodeSelected.Parent != null && n.Parent != null && LastNodeSelected.Parent.Equals(n.Parent))
                        {   // same parents

                            int lastSelectedIndex = LastNodeSelected.Index;
                            TreeNode lastSelectedNodeParent = LastNodeSelected.Parent;

                            if (lastSelectedIndex < n.Index) // add everything from last to n
                            {
                                UnpaintSelectedNodes();

                                selectedNodes.Clear();

                                for (int i = lastSelectedIndex; i <= n.Index; i++)
                                {
                                    selectedNodes.Add((ProcessingNode)lastSelectedNodeParent.Nodes[i]);
                                }
                                PaintSelectedNodes();
                            }
                            else if (lastSelectedIndex > n.Index) // add everything from n to lastSelect
                            {
                                UnpaintSelectedNodes();
                                selectedNodes.Clear();

                                for (int i = n.Index; i <= lastSelectedIndex; i++)
                                {
                                    selectedNodes.Add((ProcessingNode)lastSelectedNodeParent.Nodes[i]);
                                }
                                PaintSelectedNodes();
                            }
                            else
                            {
                                // same index
                                // do nothing
                            }
                        }
                        else // different parents, do nothing, preserve previous lastSelect
                        {
                        }
                    }//shift
                }
                else // single click
                {
                    UnpaintSelectedNodes();
                    selectedNodes.Clear();
                    selectedNodes.Add(n);
                    PaintSelectedNodes();

                    OnAfterSelect(new TreeViewEventArgs(n));
                }
            }
        }

        private void selectNodeAfterAdd(Int32 id, String linkType)
        {
            selectNodeAfterAdd(this.Nodes, id, linkType); 
        }

        public TreeNode FindNode(Int32 id)
        {
            return FindNode(this.Nodes, id);
        }

        private TreeNode FindNode(TreeNodeCollection nodes, Int32 id)
        {
            foreach (ProcessingNode child in nodes)
            {
                if (child.NodeID == id)
                {
                    return child;
                }
                else
                {
                    TreeNode test = FindNode(child.Nodes, id);
                    if (test != null)
                    {
                        return test;
                    }
                }
            }
            return null;
        }

        private void selectNodeAfterAdd(TreeNodeCollection nodes, Int32 id, String linkType)
        {
            foreach (ProcessingNode child in nodes)
            {
                if (child.NodeID == id && child.LinkType.Equals(linkType))
                {
                    UnpaintSelectedNodes();
                    selectedNodes.Clear();
                    selectedNodes.Add(child);
                    child.EnsureVisible();
                    PaintSelectedNodes();
                    OnAfterSelect(new TreeViewEventArgs(child));
                }
                else
                {
                    selectNodeAfterAdd(child.Nodes, id, linkType);
                }
            }
        }

        //private void selectNodeBeforeDelete(Int32 id, String linkType)
        //{
        //    selectNodeBeforeDelete(this.Nodes, id, linkType);
        //}

        //private void selectNodeBeforeDelete(TreeNodeCollection nodes, Int32 id, String linkType)
        //{
        //    ProcessingNode previousChild = null;
        //    foreach (ProcessingNode child in nodes)
        //    {
        //        if (child.NodeID == id && child.LinkType.Equals(linkType))
        //        {
        //            if (previousChild != null)
        //            {
        //                UnpaintSelectedNodes();
        //                selectedNodes.Clear();
        //                selectedNodes.Add(previousChild);
        //                previousChild.EnsureVisible();
        //                PaintSelectedNodes();
        //                OnAfterSelect(new TreeViewEventArgs(previousChild));
        //            }
        //        }
        //        else
        //        {
        //            previousChild = child;
        //            selectNodeAfterAdd(child.Nodes, id, linkType);
        //        }
        //    }
        //}

        public void SelectRoot()
        {
            if (this.Nodes.Count > 0)
            {
                ProcessingNode root = (ProcessingNode)Nodes[0];
                UnpaintSelectedNodes();
                selectedNodes.Clear();
                selectedNodes.Add(root);
                root.EnsureVisible();
                PaintSelectedNodes();
                OnAfterSelect(new TreeViewEventArgs(Nodes[0]));
            }
        }

        public void DoNewSelectionWithID(Int32 targetID, String linkType)
        {
            DoNewSelectionWithID(this.Nodes, targetID, linkType);
        }

        public void ClearSelection()
        {
            UnpaintSelectedNodes();
            selectedNodes.Clear();
        }

        private void DoNewSelectionWithID(TreeNodeCollection col, int targetID, String linkType)
        {
            foreach (ProcessingNode child in col)
            {
                if (linkType.Equals(String.Empty))
                {
                    if (child.NodeID == targetID)
                    {
                        UnpaintSelectedNodes();
                        selectedNodes.Clear();
                        selectedNodes.Add(child);
                        child.EnsureVisible();
                        PaintSelectedNodes();
                        //OnAfterSelect(new TreeViewEventArgs(child)); comment out for now
                    }
                    else
                    {
                        DoNewSelectionWithID(child.Nodes, targetID, linkType);
                    }
                }
                else
                {

                    if (child.NodeID == targetID && child.LinkType.Equals(linkType))
                    {
                        UnpaintSelectedNodes();
                        selectedNodes.Clear();
                        selectedNodes.Add(child);
                        child.EnsureVisible();
                        PaintSelectedNodes();
                        //OnAfterSelect(new TreeViewEventArgs(child)); comment out for now
                    }
                    else
                    {
                        DoNewSelectionWithID(child.Nodes, targetID, linkType);
                    }
                }
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void PaintSelectedNodes()
        {
            foreach (ProcessingNode n in selectedNodes)
            {
                n.BackColor = SystemColors.Highlight;
                n.ForeColor = SystemColors.HighlightText;
            }
        }

        private void UnpaintSelectedNodes()
        {
            foreach (ProcessingNode n in selectedNodes)
            {
                n.BackColor = bc;
                n.ForeColor = fc;
            }
        }

        private void CustomTreeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn != null)
            {
                if (tn != previousToolTipNode)
                {
                    previousToolTipNode = tn;
                    if (treeToolTip != null && this.treeToolTip.Active)
                        treeToolTip.Active = false;

                    treeToolTip.SetToolTip(this, tn.ToolTipText);
                    treeToolTip.Active = true;
                }
            }
        }

        public delegate void FunctionProcessed(ProcessingNode node, String functionName, String functionValue);


        public event FunctionProcessed FunctionProc;

        public void OnFunctionProcessed(ProcessingNode node, String functionName, String functionValue)
        {
            if (FunctionProc != null)
            {
                FunctionProc(node, functionName, functionValue);
            }
        }

        public delegate void ItemAdded(String addItemString, int nodeID, int linkID, String itemType, String itemName, String linkType);

        // Define an event based on the above delegate
        public event ItemAdded ItemAdd;

        public void OnItemAdd(String addItemString, int nodeID, int linkID, String itemType, String itemName, String linkType)
        {
            if (ItemAdd != null)
            {
                ItemAdd(addItemString, nodeID, linkID, itemType, itemName, linkType); // fire event
            }
            this.selectNodeAfterAdd(nodeID, linkType);
        }

        public delegate void ItemDeleted(String deleteFunction, ProcessingNode deletedItem);

        public event ItemDeleted ItemDelete;

        public void OnItemDelete(String deleteFunction, ProcessingNode deletedItem)
        {
            if (ItemDelete != null)
            {
                ItemDelete(deleteFunction, deletedItem);
            }
            NoActionProcessingNode node = new NoActionProcessingNode(-1, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);
            OnAfterSelect(new TreeViewEventArgs(node));
        }

        public delegate void ItemRenamed(int nodeID, string nodeType, string nodeName, string nodeDescription);

        public event ItemRenamed ItemRename;

        public void OnItemRename(int nodeID, string nodeType, string nodeName, string nodeDescription)
        {
            if (ItemRename != null)
            {
                ItemRename(nodeID, nodeType, nodeName, nodeDescription);
            }
        }

        private void CustomTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            CustomTreeView thisTree = (CustomTreeView)sender;
            ProcessingNode item = (ProcessingNode)e.Item;

            if (item != null)
            {
                if (item is NoActionProcessingNode) // for dragging nodes with no action
                {
                    return;
                }
                else if (selectedNodes.Count > 1)
                {
                    // Start the drag-and-drop operation
                    thisTree.DoDragDrop(CloneSelectedNodes(), DragDropEffects.Copy);
                }
                else
                {
                    List<ProcessingNode> send = new List<ProcessingNode>();
                    send.Add(item);
                    thisTree.DoDragDrop(send, DragDropEffects.Copy);
                }
            }
        }

        //public String Xsl
        //{
        //    get
        //    {
        //        return xsl;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            transform = new XslCompiledTransform();
        //            try
        //            {
        //                transform.Load(String.Format(myController.ConfigurationPath + @"\" + value));
        //                xsl = value;
        //            }
        //            catch (Exception ex)
        //            {
        //                transform = null;
        //                xsl = null;
        //                MessageBox.Show(ex.Message, "Failed to load transform - is a controller set? (Tree)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //}

        private int lastSelectID = -1;

        protected virtual void loadTree()
        {
            Int32 topNodeId = -1;
            if (TopNode != null)
                topNodeId = ((ProcessingNode)TopNode).NodeID;
            showRoot = true; // FOR NOW - MIGHT WANT TO CHANGE
            List<ProcessingNode> pNodes = new List<ProcessingNode>();
            foreach (CustomTreeRoot root in roots.Values)
            {
                IXPathNavigable iNavigator;
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.InstanceUseClassName = true;
                compOptions.CompParams = true;
                //compOptions.LinkParams = true;

                if (m_level != 0)
                {
                    compOptions.LevelDown = m_level;
                    iNavigator = myController.GetComponentAndChildren(GetCustomTreeRootId(root.LinkType), GetCustomTreeDisplayId(root.LinkType), root.LinkType, compOptions);
                }
                else
                {
                    iNavigator = myController.GetComponentAndChildren(GetCustomTreeRootId(root.LinkType), GetCustomTreeDisplayId(root.LinkType), root.LinkType, compOptions);
                }

                XPathNavigator navigator = iNavigator.CreateNavigator();

                if (root.Transform != null)
                {
                    XmlDocument newDocument = new XmlDocument();
                    using (XmlWriter writer = newDocument.CreateNavigator().AppendChild())
                    {
                        root.Transform.Transform(iNavigator, (XsltArgumentList)null, writer);
                    }
                    navigator = newDocument.CreateNavigator();
                }

                // Perform a CRC checksum on the data coming back, only update if it has changed
                //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
                //Byte[] bs = System.Text.Encoding.UTF8.GetBytes(navigator.OuterXml);
                //bs = x.ComputeHash(bs);
                //System.Text.StringBuilder s = new System.Text.StringBuilder();
                //foreach (Byte b in bs)
                //{
                //    s.Append(b.ToString("x2").ToLower());
                //}
                //String newCrc = s.ToString();

                //// bug fix - add image list handle to crc so if images get updated, we refresh
                //if (this.ImageList != null)
                //{
                //    newCrc = newCrc + this.ImageList.Handle;
                //}

                //if (!crc.Equals(newCrc)) // otherwise update with the new data
                {
                    CheckImages(myController);

                    // If CRC fails why do we need idChange????
                    // EXPANSION AND SCROLLING
                    //if (idChange) // if we changed ids, don't save the current state under this *new* id
                    //{
                    //    idChange = false;
                    //}
                    //else
                    //{
                    //    CheckExpansion(); // only re-save if we're refreshing an existing tree.
                    //}
                    CheckExpansion();
                    this.Nodes.Clear();
                    if (useNodeMap)
                    {
                        root.ClearNodeMap();
                    }
                    this.ContextMenuStrip.Items.Clear();

                    lastSelectID = -1;
                    if (this.selectedNodes.Count == 1 && this.LastNodeSelected != null && LastNodeSelected.NodeID >= 0)  // save, clear selection
                    {
                        lastSelectID = this.LastNodeSelected.NodeID;
                    }
                    this.selectedNodes.Clear();

                    //crc = newCrc;

                    XPathNavigator rootXML = navigator.SelectSingleNode(String.Format("//*[@ID='{0}']", GetCustomTreeDisplayId(root.LinkType)));

                    //List<ProcessingNode> pNodes = new List<ProcessingNode>();

                    if (rootXML != null)
                    {
                        ProcessingNode rootNode = CreateNodeAndProcessFunctions(rootXML, root.LinkType);

                        if (useNodeMap)
                        {
                            root.AddNodeToMap(rootNode);
                        }

                        //if (showRoot)
                        //{
                        //    Nodes.Add(rootNode); // load the root
                            pNodes.Add(rootNode);
                            loadNodes(root, rootXML, rootNode);
                        //}
                        //else
                        //{
                        //    loadNodeRange(rootXML, pNodes, root.LinkType);
                        //}

                        // and all of its siblings 
                        bool test = rootXML.MoveToNext();

                        while (test != false)
                        {
                            throw new Exception("Invalid data - check xsl!");
                            //siblingTreeNode = CreateNodeAndProcessFunctions(rootXML, root.LinkType);

                            //if (showRoot)
                            //{
                            //    Nodes.Add(siblingTreeNode);
                            //}
                            //else
                            //{
                            //    pNodes.Add(siblingTreeNode);
                            //}





                            //loadNodes(rootXML, siblingTreeNode); // load children

                            //test = rootXML.MoveToNext();  // update sibling
                        }

                        //if (!showRoot)
                        //{
                        //    Nodes.AddRange(pNodes.ToArray());
                        //}


                        // EXPANSION AND SCROLLING
                        // have we seen this node before?
                        //if (!topNodes.ContainsKey(DisplayID))
                        //{
                        //    CheckExpansion(); // set initial state
                        //}

                        //this.RestoreExpansion();
                        //if (topNodes.ContainsKey(DisplayID))
                        //{
                        //    FindTopNode = null;
                        //    this.FindNodeWithID(this.Nodes, topNodes[DisplayID]);
                        //    if (FindTopNode != null)
                        //    {
                        //        this.TopNode = FindTopNode;
                        //    }
                        //}
                    } //root
                } // crc
            }
            if (pNodes.Count <= 0)
            {
                this.Nodes.Clear();
                return;
            }
            Nodes.AddRange(pNodes.ToArray());
            this.RestoreExpansion();
            if (topNodeId != -1)
            {
                FindTopNode = null;
                this.FindNodeWithID(this.Nodes, topNodeId);
                if (FindTopNode != null)
                {
                    this.TopNode = FindTopNode;
                }
            }
        }
        private void loadNodes(CustomTreeRoot root, XPathNavigator nav, ProcessingNode tParent)
        {
            XPathNodeIterator nodes = nav.Select("Component");

            foreach (XPathNavigator navChild in nodes)
            {
                ProcessingNode childNode = CreateNodeAndProcessFunctions(navChild, tParent.LinkType);

                if (useNodeMap)
                {
                    root.AddNodeToMap(childNode);
                }

                // do we need to branch?
                if (childNode.Functions.Count > 0 && childNode.Functions[0].FunctionName.Equals("RootBranch") && !branching)
                {
                    branching = true;
                    String dynamicLink = this.Controller.GetDynamicLinkType(root.LinkType, ""+childNode.NodeID);
                    IXPathNavigable iRootNav = this.Controller.GetComponentAndChildren(childNode.NodeID, childNode.NodeID, dynamicLink, new ComponentOptions());
                    XPathNavigator newRootNav = iRootNav.CreateNavigator();
                    // optional transform
                    try
                    {
                        CustomTreeRoot xslBuilder = new CustomTreeRoot();
                        xslBuilder.Controller = this.Controller;
                        xslBuilder.Xsl = childNode.Functions[0].FunctionAction;
                        if (xslBuilder.Transform != null)
                        {
                            XmlDocument newDocument = new XmlDocument();
                            using (XmlWriter writer = newDocument.CreateNavigator().AppendChild())
                            {
                                xslBuilder.Transform.Transform(iRootNav, (XsltArgumentList)null, writer);
                            }
                            newRootNav = newDocument.CreateNavigator();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        newRootNav.SelectSingleNode("Components/Component/@LinkID").SetValue("" + childNode.LinkID.Value);
                        XPathNavigator newRootComponents = newRootNav.SelectSingleNode("Components");
                        if (newRootComponents != null)
                        {
                            tParent.LinkType = dynamicLink;
                            loadNodes(root, newRootComponents, tParent);
                            branching = false;
                        }
                    }
                }
                else
                {
                    tParent.Nodes.Add(childNode);

                    if (navChild.HasChildren)
                    {
                        loadNodes(root, navChild, childNode);
                    }
                }
            }
        }
        private void loadNodeRange(CustomTreeRoot root, XPathNavigator nav, List<ProcessingNode> tParent, String linkType)
        {
            XPathNodeIterator nodes = nav.Select("Component");

            foreach (XPathNavigator navChild in nodes)
            {
                ProcessingNode childNode = CreateNodeAndProcessFunctions(navChild, linkType);

                if (useNodeMap)
                {
                    root.AddNodeToMap(childNode);
                }

                tParent.Add(childNode);

                if (navChild.HasChildren)
                {
                    loadNodes(root, navChild, childNode);
                }
            }
        }

        // Given node xml, create the node and process its functions
        // the drag function determines the type of node created
        // the other functions are stored in the node and can be quickly looked up
        // (e.g. on a right click context menu)
        private ProcessingNode CreateNodeAndProcessFunctions(XPathNavigator nodeNavigator, String linkType)
        {
            ProcessingNode returnNode = null;

            int nodeID = Convert.ToInt32(nodeNavigator.GetAttribute(IDAttribute, nodeNavigator.NamespaceURI));
            String nodeType = nodeNavigator.GetAttribute(TypeAttribute, nodeNavigator.NamespaceURI);
            String nodeName = nodeNavigator.GetAttribute(NameAttribute, nodeNavigator.NamespaceURI);
            String nodeDescription = nodeNavigator.GetAttribute(DescriptionAttribute, nodeNavigator.NamespaceURI);
            String nodeEType = nodeNavigator.GetAttribute(ETypeAttribute, nodeNavigator.NamespaceURI);
            String nodeLinkID = nodeNavigator.GetAttribute(LinkIDAttribute, nodeNavigator.NamespaceURI);
            String nodeLinkType = linkType;

            List<Function> nodeFunctions = FunctionHelper.GetFunctions(nodeNavigator);

            Dictionary<String, String> parameterMap = new Dictionary<String, String>();

            XPathNodeIterator parameters = nodeNavigator.Select(imageCategoryParameter);

            String name, category, value;
            while (parameters.MoveNext())
            {
                category = parameters.Current.GetAttribute(categoryAttr, String.Empty);
                if (category.Equals(categoryMatch)) // Image
                {
                    name = parameters.Current.GetAttribute(displayedNameAttr, String.Empty);
                    value = parameters.Current.GetAttribute(ConfigFileConstants.Value, String.Empty);
                    parameterMap.Add(String.Concat(category, SchemaConstants.ParameterDelimiter, name), value);
                }
            }

            String imagePath = FunctionHelper.ProcessNavForImage(this.Controller, this.ImageList, nodeType, nodeNavigator, nodeFunctions, parameterMap);  // update node image.

            foreach (Function nodeFunction in nodeFunctions)
            {
                if (nodeFunction.FunctionName == ConfigFileConstants.Drag || nodeFunction.FunctionName == ConfigFileConstants.ObjectType)
                {
                    // instantiate class based on function name
                    Type type = AMEManager.GetType(nodeFunction.FunctionAction); 

                    if (type != null)
                    {
                        //returnNode = (ProcessingNode)Activator.CreateInstance(type, new object[] { nodeID, nodeType, nodeName, nodeEType, imagePath, linkType });
                        returnNode = createNode(type, new object[] { nodeID, nodeType, nodeName, nodeEType, imagePath, linkType });
                        break;
                    }
                }
            }

            if (returnNode == null) // no-op is default
            {
                returnNode = new NoActionProcessingNode(nodeID, nodeType, nodeName, nodeEType, imagePath, nodeLinkType);
            }

            // set linkID, if available
            if (nodeLinkID != null && nodeLinkID.Length > 0)
            {
                returnNode.LinkID = Int32.Parse(nodeLinkID);
            }

            returnNode.ToolTipText = nodeDescription;
            returnNode.Functions = nodeFunctions;
            if (decorateNodes)
            {
                XPathNodeIterator links = nodeNavigator.Select("LinkParameters/Parameter/Parameter");
                StringBuilder decoraterBuilder = new StringBuilder();
                decoraterBuilder.Append(returnNode.Name);
                decoraterBuilder.Append(" :");
                foreach (XPathNavigator link in links)
                {
                    String paramName = link.GetAttribute(ConfigFileConstants.displayedName, String.Empty);
                    String paramValue = link.GetAttribute(ConfigFileConstants.Value, String.Empty);

                    if (!String.IsNullOrEmpty(paramValue))
                    {
                        decoraterBuilder.Append(" ");
                        decoraterBuilder.Append(paramName);
                        decoraterBuilder.Append(" [");
                        decoraterBuilder.Append(paramValue);
                        decoraterBuilder.Append("]");
                    }
                }
                returnNode.Text = decoraterBuilder.ToString();
            }

            // **** Maintain Selection when we reload ****
            if (lastSelectID >= 0 && lastSelectID == nodeID)
            {
                // don't add if the eType is none - e.g. for XSL
                if (!returnNode.EType.Equals("None"))
                {
                    selectedNodes.Add(returnNode);
                }
            }

            return returnNode;
        }

        protected virtual ProcessingNode createNode(Type type, object[] args)
        {
            ProcessingNode returnNode = (ProcessingNode)Activator.CreateInstance(type, args);
            return returnNode;
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            //foreach (ImageList list in configToImageList.Values)
            //{
            //    foreach (Image img in list.Images)
            //    {
            //        img.Dispose(); // manually free image space (Graphics are a special case)
            //    }
            //}
            //configToImageList = null;
        }

        private void RestoreNodeExpansion(ProcessingNode n)
        {
            //if (DisplayID + n.FullPath + n.Index == selectedItemPath)
            //{
            //    //this.SelectedNode = n; ***********************************
            //}
            if (isIDExpanded.ContainsKey(n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index))
            {

                bool check = isIDExpanded[n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index];
                if (check)
                {
                    n.Expand();
                }
                else
                {
                    n.Collapse();
                }
            }
            // check children
            foreach (ProcessingNode child in n.Nodes)
            {
                RestoreNodeExpansion(child);
            }
        }

        private void CheckNodeExpansion(ProcessingNode n)
        {
            //if (this.SelectedNode != null && this.SelectedNode.Equals(n))
            //{
            //    selectedItemPath = DisplayID + n.FullPath + n.Index; *************************
            //}
            if (n.IsExpanded)
            {
                if (isIDExpanded.ContainsKey(n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index))
                {
                    isIDExpanded[n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index] = true;
                }
                else
                {
                    isIDExpanded.Add(n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index, true);
                }
            }
            else
            {
                if (isIDExpanded.ContainsKey(n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index))
                {
                    isIDExpanded[n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index] = false;
                }
                else
                {
                    isIDExpanded.Add(n.LinkType + this.GetCustomTreeDisplayId(n.LinkType) + n.FullPath + n.Index, false);
                }
            }
            // check children
            foreach (ProcessingNode child in n.Nodes)
            {
                CheckNodeExpansion(child);
            }
        }

        private void CheckExpansion()
        {
            //if (this.TopNode != null)
            //{
            //    if (!topNodes.ContainsKey(DisplayID))
            //    {
            //        topNodes.Add(DisplayID, ((ProcessingNode)TopNode).NodeID);
            //    }
            //    else
            //    {
            //        topNodes[DisplayID] = ((ProcessingNode)TopNode).NodeID;
            //    }
            //}

            foreach (ProcessingNode n in this.Nodes)
            {
                CheckNodeExpansion(n);
            }
        }
        private void RestoreExpansion()
        {
            foreach (ProcessingNode n in this.Nodes)
            {
                RestoreNodeExpansion(n);
            }
        }

        private void CheckImages(IController c)
        {
            // *** track node icon assignments ***
            // do we have an image list?
            if (this.ImageList == null)
            {
                Dictionary<String, Bitmap> typeImage = c.GetIcons();
                ImageList tempList = new ImageList();
                tempList.ColorDepth = ColorDepth.Depth32Bit;
                Image image;

                foreach (String k in typeImage.Keys)
                {
                    image = typeImage[k];
                    tempList.Images.Add(k, image);
                }

                this.ImageList = tempList;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomTreeView
            // 
            this.AllowDrop = true;
            this.HideSelection = false;
            this.ResumeLayout(false);
        }

        #region ViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;

                foreach (CustomTreeRoot root in roots.Values)
                {
                    root.Controller = value;
                }
            }
        }

        public void DeleteTreeAsLinks()
        {
            foreach (CustomTreeRoot root in roots.Values)
            {
                DeleteNodeWithID(root.RootId, true, false);
            }
        }

        public void DeleteTreeAsComponents()
        {
            foreach (CustomTreeRoot root in roots.Values)
            {
                DeleteNodeWithID(root.RootId, true, true);
            }
        }

        public void DeleteNodeWithID(int targetID, bool updateOnDelete, bool deleteAsComponent)
        {
            DeleteNodeWithID(targetID, updateOnDelete, deleteAsComponent, true);
        }

        public void DeleteNodeWithID(int targetID, bool updateOnDelete, bool deleteAsComponent, bool recursive)
        {
            List<int> deletedList = new List<int>();

            if (updateOnDelete)
            {
                myController.TurnViewUpdateOff();
                DeleteNodeWithID(this.Nodes, targetID, true, deleteAsComponent, deletedList, recursive);
                myController.TurnViewUpdateOn();
            }
            else
            {
                DeleteNodeWithID(this.Nodes, targetID, true, deleteAsComponent, deletedList, recursive);
            }
        }

        private void DeleteNodeWithID(TreeNodeCollection nodeContainer, int targetNodeID, 
            bool allowRootDeletion, bool deleteNodesAsComponents, List<int> deleted, bool recursive)
        {
            foreach (ProcessingNode node in nodeContainer)
            {
                if ( (deleteNodesAsComponents && !deleted.Contains(node.NodeID) && node.NodeID == targetNodeID) || 
                        (!deleteNodesAsComponents && node.LinkID.HasValue && !deleted.Contains(node.LinkID.Value) && node.LinkID.Value == targetNodeID) ||
                          (!deleteNodesAsComponents && targetNodeID == node.NodeID))
                {
                    if (recursive)
                    {
                        foreach (ProcessingNode subNode in node.Nodes)
                        {
                            if (deleteNodesAsComponents)
                            {
                                DeleteNodeWithID(node.Nodes, subNode.NodeID, false, deleteNodesAsComponents, deleted, recursive); // recursively delete children, ignoring 'fake' root nodes
                            }
                            else if (subNode.LinkID.HasValue)
                            {
                                DeleteNodeWithID(node.Nodes, subNode.LinkID.Value, false, deleteNodesAsComponents, deleted, recursive);
                            }
                            else
                            {
                                DeleteNodeWithID(node.Nodes, -1, false, deleteNodesAsComponents, deleted, recursive);
                            }
                        }
                    }

                    if (targetNodeID == GetCustomTreeRootId(node.LinkType))//RootID)
                    {
                        if (allowRootDeletion)
                        {
                            try
                            {
                                bool success = myController.DeleteComponent(node.NodeID); // delete root
                                if (success)
                                {
                                    deleted.Add(node.NodeID); // prevent further matches
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (deleteNodesAsComponents)
                            {
                                bool success = myController.DeleteComponent(node.NodeID); // delete the target node as component
                                if (success)
                                {
                                    deleted.Add(node.NodeID); // prevent further matches
                                }
                            }
                            else if (node.LinkID.HasValue)
                            {
                                bool success = myController.DeleteLink(node.LinkID.Value); // delete target node as link
                                if (success)
                                {
                                    deleted.Add(node.LinkID.Value); // prevent further matches
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
                else
                {
                    DeleteNodeWithID(node.Nodes, targetNodeID, allowRootDeletion, deleteNodesAsComponents, deleted, recursive); // otherwise keep looking below
                }
            }
        }

        private void FindNodeWithID(TreeNodeCollection collection, int targetID)
        {
            foreach (TreeNode node in collection)
            {
                if (((ProcessingNode)node).NodeID == targetID)
                {
                    FindTopNode = node;
                }
                else
                {
                    FindNodeWithID(node.Nodes, targetID);
                }
            }
        }

        private TreeNode FindTopNode;

        public void UpdateViewComponent()
        {
            DrawingUtility.SuspendDrawing(this.Parent);
            DrawingUtility.SuspendDrawing(this);

            UpdateTree(myController);

            DrawingUtility.ResumeDrawing(this);
            DrawingUtility.ResumeDrawing(this.Parent);
        }

        #endregion

        private void UpdateTree(IController c)
        {
            this.loadTree();

            // maintain selection when we reload
            PaintSelectedNodes();
        }

        //public class CustomTreeInfoCollection
        //{
        //    readonly CustomTreeView tree;

        //    internal CustomTreeInfoCollection(CustomTreeView tree)
        //    {
        //        this.tree = tree;
        //    }

        //    public String this[Int32 index]
        //    {
        //        get
        //        {
        //            return tree.CustomTreeInfo[index];
        //        }
        //        set
        //        {
        //            tree.CustomTreeInfo[index] = value;
        //        }
        //    }
        //}
    }

    public class CustomTreeRoot
    {
        private CustomTreeView myTree;

        public CustomTreeView MyTree
        {
            set { myTree = value; }
        }

        private IController controller;

        public IController Controller
        {
            set { controller = value; }
        }

        private String linkType;
        public String LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }
        private Int32 rootId;
        public Int32 RootId
        {
            get { return rootId; }
            set { rootId = value; }
        }
        private Int32 displayId;
        public Int32 DisplayId
        {
            get { return displayId; }
            set { displayId = value; }
        }
        private static Dictionary<String, XslCompiledTransform> cachedTransforms = new Dictionary<String, XslCompiledTransform>();
        private String xsl;
        public String Xsl
        {
            //get { return xsl; }
            set
            {
                if (value != null)
                {
                    if (cachedTransforms.ContainsKey(value))
                    {
                        transform = cachedTransforms[value];
                        xsl = value;
                    }
                    else
                    {
                        transform = new XslCompiledTransform();
                        xsl = value;
                        try
                        {
                            if (myTree != null && myTree.Controller != null && controller == null)
                            {
                                controller = myTree.Controller;
                            }

                            if (controller != null)
                            {
                                XmlReader xslreader = controller.GetXSL(value);
                                transform.Load(xslreader);
                                xslreader.Close();
                                cachedTransforms.Add(value, transform);
                            }
                            else
                            {
                                MessageBox.Show("(internal) Please provide a controller before adding tree roots");
                            }
                        }
                        catch (Exception ex)
                        {
                            transform = null;
                            xsl = null;
                            MessageBox.Show(ex.Message, "Failed to load transform ("+value+") - is a controller set? (Tree)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private XslCompiledTransform transform;
        public XslCompiledTransform Transform
        {
            get { return transform; }
        }

        private Dictionary<Int32, List<ProcessingNode>> nodeMap = new Dictionary<Int32, List<ProcessingNode>>();

        public Dictionary<Int32, List<ProcessingNode>> GetNodeMap()
        {
            return nodeMap;
        }

        public void ClearNodeMap()
        {
            nodeMap.Clear();
        }

        public void AddNodeToMap(ProcessingNode node)
        {
            if (!nodeMap.ContainsKey(node.NodeID))
            {
                nodeMap.Add(node.NodeID, new List<ProcessingNode>());
  
            }
            nodeMap[node.NodeID].Add(node);
        }

        public List<ProcessingNode> GetNodes(Int32 nodeID)
        {
            if (nodeMap.ContainsKey(nodeID))
            {
                return nodeMap[nodeID];
            }
            else
            {
                MessageBox.Show("Node map does not contain node ID: " + nodeID);
                return null;
            }
        }
    }

    public class CustomContextMenuTag
    {
        private ProcessingNode node;
        private String functionName;
        private String functionValue;

        private List<String> arguments;

        public CustomContextMenuTag(ProcessingNode p_node, String p_functionName, String p_functionValue)
        {
            node = p_node;
            functionName = p_functionName;
            functionValue = p_functionValue;

            arguments = new List<String>();
        }

        public ProcessingNode GetNode() { return node; }
        public String GetFunctionName() { return functionName; }
        public String GetFunctionValue() { return functionValue; }
        public String[] Arguments
        {
            get
            {
                return arguments.ToArray();
            }
            set
            {
                arguments = new List<String>(value);
            }
        }
    }

    public class NodeFunctionListEventArgs : EventArgs
    {
        private readonly ProcessingNode node;
        private readonly List<Function> functions;

        public NodeFunctionListEventArgs(ProcessingNode node, List<Function> functions)
        {
            this.node = node;
            this.functions = functions;
        }

        public ProcessingNode Node
        {
            get
            {
                return node;
            }
        }

        public List<Function> Functions
        {
            get
            { 
                return functions;
            }       
        }
    }
}
