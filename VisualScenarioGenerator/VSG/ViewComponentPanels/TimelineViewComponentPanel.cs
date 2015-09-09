using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;
using VSG.ViewComponents;

using AME;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Nodes;
using Northwoods.Go;

namespace VSG.ViewComponentPanels
{
    public partial class TimelineViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController vsgController;

        private List<Diagram> diagrams;

        private EventProperties currentProperties;

        private string myLinkType;

        public TimelineViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this);

            InitializeComponent();
            currentProperties = EventProperties.Blank;

            customBlockTimeline1.AfterSelect = new CustomBlockTimelineSelectHandler(OnBlockTimelineSelection);

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            vsgController.RegisterForUpdate(this);

            myLinkType = vsgController.ConfigurationLinkType;

            diagramPanel1.Controller = vsgController;
            diagrams = diagramPanel1.Diagrams;
            
            createInstanceTree.Controller = vsgController;

            createInstanceTree.AddCustomTreeRoot(myLinkType);
            createInstanceTree.SetCustomTreeRootXsl(myLinkType, "CreateEventDecisionMaker.xslt");

            createInstanceTree.Level = 3;
            createInstanceTree.ShowRoot = false;

            customTreeView2.Controller = vsgController;

            customTreeView2.AddCustomTreeRoot(myLinkType);
            customTreeView2.SetCustomTreeRootXsl(myLinkType, "EventTree.xsl");
  
            //customTreeView2.Level = 10;
            customTreeView2.ShowRoot = true;

            customBlockTimeline1.Controller = vsgController;
            customBlockTimeline1.LinkType = vsgController.ConfigurationLinkType;


            this.evtPnl_ChatroomClose1.Controller = vsgController;
            this.evtPnl_ChatroomOpen1.Controller = vsgController;
            this.evtPnl_SendChatMessage1.Controller = vsgController;
            this.evtPnl_VoiceChannelClose1.Controller = vsgController;
            this.evtPnl_VoiceChannelOpen1.Controller = vsgController;
            this.evtPnl_SendVoiceMessage1.Controller = vsgController;
            this.evtPnl_SendVoiceMessageToUser1.Controller = vsgController;
            this.evtPnl_EngramChange1.Controller = vsgController;
            this.evtPnl_EngramRemove1.Controller = vsgController;
            this.evtPnl_Flush1.Controller = vsgController;
            this.evtPnl_Launch1.Controller = vsgController;
            this.evtPnl_WeaponLaunch1.Controller = vsgController;
            this.evtPnl_Move1.Controller = vsgController;
            this.evtPnl_Reveal1.Controller = vsgController;
            this.evtPnl_StateChange1.Controller = vsgController;
            this.evtPnl_Transfer1.Controller = vsgController;
            this.evtPnl_Reiterate1.Controller = vsgController;
            this.evtPnl_Completion1.Controller = vsgController;
            this.evtPnl_SpeciesCompletion1.Controller = vsgController;
            this.evtPnl_Create1.Controller = vsgController;
            //this.subplatform1.Controller = vsgController;
            //this.armament1.Controller = vsgController;
            InitializePropertyPanels();
            HideAllProperties();
        }

        // we need to do this or the diagram won't refresh when sensor data is changed
        // the diagram will pass its crc because no actual data in the diagram has 
        // changed, but sensor data, which we have to go out to application code to
        // fetch, has
        public void ResetCRC()
        {
            foreach (Diagram d in diagrams)
            {
                d.ResetCRC();
            }
        }

        public void UpdateViewComponent()
        {
            if (myHelper.LatestEventFromController == UpdateType.Component)
            {
                cEvent_ComponentUpdate();
            }
            else if (myHelper.LatestEventFromController == UpdateType.Parameter)
            {
                cEvent_ParameterUpdate();
            }
            else if (myHelper.LatestEventFromController == UpdateType.ComponentAndParameter)
            {
                createInstanceTree.UpdateViewComponent();
                customTreeView2.UpdateViewComponent();
                customTreeView2.ExpandAll();
                if (customTabControl3.SelectedIndex == 1)
                {
                    customBlockTimeline1.UpdateViewComponent();
                }

                try
                {
                    if (vsgController != null && vsgController.CurrentMapLocation != null)
                    {
                        if (vsgController.CurrentMapLocation != "")
                        {
                            this.diagramPanel1.SetDiagramBackground(Image.FromFile(vsgController.CurrentMapLocation), "Scenario");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error in Map Background", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                UpdateProperties(); //do we need update parameters during component update?

                UpdateCoordinateTransform();

                foreach (Diagram d in diagrams)
                {
                    d.UpdateViewComponent();
                }
            }
        }

        private void cEvent_ComponentUpdate()
        {
            if (this.Parent != null)
            {
                createInstanceTree.UpdateViewComponent();
                //createInstanceTree.ExpandAll();
                customTreeView2.UpdateViewComponent();
                customTreeView2.ExpandAll();
                if (customTabControl3.SelectedIndex == 1)
                {
                    customBlockTimeline1.UpdateViewComponent();
                }


                try
                {
                    if (vsgController != null && vsgController.CurrentMapLocation != null)
                    {
                        if (vsgController.CurrentMapLocation != "")
                        {
                            this.diagramPanel1.SetDiagramBackground(Image.FromFile(vsgController.CurrentMapLocation), "Scenario");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error in Map Background", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                UpdateProperties(); //do we need update parameters during component update?

                UpdateCoordinateTransform();

                foreach (Diagram d in diagrams)
                {
                    d.UpdateViewComponent();
                }

                //this.evtPnl_ChatroomClose1.UpdateViewComponent();
                //this.evtPnl_ChatroomOpen1.UpdateViewComponent();
                //this.evtPnl_EngramChange1.UpdateViewComponent();
                //this.evtPnl_EngramRemove1.UpdateViewComponent();
                //this.evtPnl_Flush1.UpdateViewComponent();
                //this.evtPnl_Launch1.UpdateViewComponent();
                //this.evtPnl_Move1.UpdateViewComponent();
                //this.evtPnl_Reveal1.UpdateViewComponent();
                //this.evtPnl_StateChange1.UpdateViewComponent();
                //this.evtPnl_Transfer1.UpdateViewComponent();
                //this.evtPnl_Reiterate1.UpdateViewComponent();
                //this.evtPnl_Completion1.UpdateViewComponent();
                //this.evtPnl_Create1.UpdateViewComponent();
            }
        }

        private void cEvent_ParameterUpdate()
        {
            if (this.Parent != null)
            {
                if (customTabControl3.SelectedIndex == 0)
                {
                    //createInstanceTree.UpdateViewComponent();
                    //createInstanceTree.ExpandAll();
                    //customTreeView2.UpdateViewComponent();
                    //customTreeView2.ExpandAll();
                    UpdateProperties();
                }
                if (customTabControl3.SelectedIndex == 1)
                {
                    customBlockTimeline1.UpdateViewComponent();
                }
                
                try
                {
                    if (vsgController != null && vsgController.CurrentMapLocation != null)
                    {

                        if (vsgController.CurrentMapLocation != "")
                        {
                            this.diagramPanel1.SetDiagramBackground(Image.FromFile(vsgController.CurrentMapLocation), "PlayfieldRegions");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error in Map Background", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                UpdateCoordinateTransform();

                foreach (Diagram d in diagrams)
                {
                    d.UpdateViewComponent();
                }
            }
        }

        private void UpdateCoordinateTransform()
        {
            if (vsgController != null && vsgController.CoordinateTransform != null)
            {
                foreach (Diagram d in diagramPanel1.Diagrams)
                {
                    if (d.CoordinateTransformer != null && !d.CoordinateTransformer.Equals(vsgController.CoordinateTransform))
                    {
                        d.ResetCRC(); // reset so we see new coords.
                    }
                    d.CoordinateTransformer = vsgController.CoordinateTransform;
                }
            }
        }

        private void createInstanceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessingNode pNode = e.Node as ProcessingNode;
            Int32 nodeId = pNode.NodeID;
            String nodeName;
            if (pNode.NodeID != -1)
                nodeName = pNode.NodeType;
            else
                nodeName = e.Node.Name;

            //customTreeView2.DoNewSelectionWithID(-1);
            customTreeView2.ClearSelection();

            loadViewComponent(nodeName, nodeId);
            //Show_Properties(EventProperties.Create, nodeId, -1);
            diagramPanel1.SelectNodeWithID(-1);

            if ((pNode.NodeType == "CreateEvent") || (pNode.NodeType =="DecisionMaker"))
            {
                customTabPage1.Text = "Unit's Events";
                customTabPage1.Description = "Create and edit unit events.";
                foreach (Diagram d in diagrams)
                {
                    d.Xsl = "TimelineDiagram.xsl";
                    d.DisplayID = nodeId;
                    d.UpdateViewComponent();
                }
            }
            else if (pNode.NodeType == "GlobalEvent")
            {
                customTabPage1.Text =  "Global Events";
                customTabPage1.Description = "Create and edit global events.";
                foreach (Diagram d in diagrams)
                {
                    d.Xsl = "TimelineDiagramGlobalEvents.xsl";
                    d.DisplayID = vsgController.ScenarioId;
                    d.UpdateViewComponent();


                }
            }
            else if (pNode.NodeType == "Subplatform")
            {
                customTabPage1.Text = "Unit's Events";
                customTabPage1.Description = "Create and edit unit events.";

                if (pNode.NodeID != -1)
                {
                    Show_Properties(EventProperties.Subplatform, pNode.NodeID, ((ProcessingNode)pNode.Parent.Parent).NodeID);
                    SelectBlockTimelineCell(pNode.NodeID);
                    customTreeView2.SetCustomTreeRootDisplayId(myLinkType, ((ProcessingNode)pNode.Parent.Parent).NodeID);
                    customTreeView2.UpdateViewComponent();
                    foreach (Diagram d in diagrams)
                    {
                        d.Xsl = "TimelineDiagram.xsl";
                        d.DisplayID = ((ProcessingNode)pNode.Parent.Parent).NodeID;
                        d.UpdateViewComponent();
                    }
                }
                else
                {
                    customTreeView2.SetCustomTreeRootDisplayId(myLinkType, ((ProcessingNode)pNode.Parent).NodeID);
                    customTreeView2.UpdateViewComponent();
                    foreach (Diagram d in diagrams)
                    {
                        d.Xsl = "TimelineDiagram.xsl";
                        d.DisplayID = ((ProcessingNode)pNode.Parent).NodeID;
                        d.UpdateViewComponent();
                    }
                }
                return;
            }

            else if (pNode.NodeType == "Armament")
            {
                customTabPage1.Text = "Unit's Events";
                customTabPage1.Description = "Create and edit unit events.";

                customTreeView2.SetCustomTreeRootDisplayId(myLinkType, ((ProcessingNode)pNode.Parent.Parent.Parent).NodeID);
                customTreeView2.UpdateViewComponent();
                Show_Properties(EventProperties.Armament, pNode.NodeID, ((ProcessingNode)pNode.Parent).NodeID);
                SelectBlockTimelineCell(pNode.NodeID);
                foreach (Diagram d in diagrams)
                {
                    d.Xsl = "TimelineDiagram.xsl";
                    d.DisplayID = ((ProcessingNode)pNode.Parent.Parent.Parent).NodeID;
                    d.UpdateViewComponent();
                }
                return;
            }
            
            
        }
        private void d_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            
            GoObject clickedObject = e.GoObject;

            if (clickedObject.ParentNode != null && clickedObject.ParentNode is HasNodeID)
            {
                int newSelected = ((HasNodeID)clickedObject.ParentNode).NodeID;

                //createInstanceTree.ClearSelection();
                customTreeView2.ClearSelection();
                createInstanceTree.DoNewSelectionWithID(newSelected, myLinkType);
                customTreeView2.DoNewSelectionWithID(newSelected, myLinkType);
                string name = vsgController.GetComponentName(newSelected);

                switch (name)
                {
                    case "RevealEvent":
                        Show_Properties(EventProperties.Reveal, newSelected, -1);
                        break;
                    case "MoveEvent":
                        Show_Properties(EventProperties.Move, newSelected, -1);
                        break;
                    case "LaunchEvent":
                        Show_Properties(EventProperties.Launch, newSelected, -1);
                        break;
                    case "WeaponLaunchEvent":
                        Show_Properties(EventProperties.WeaponLaunch, newSelected, -1);
                        break;
                    default:
                        break;
                }
            }
        }

        private void loadViewComponent(String name, Int32 id)
        {
            switch (name)
            {
                case "CreateEvent":
                    //DrawingUtility.SuspendDrawing(splitContainer1.Panel2);

                    //customTreeView2.RootID = id;
                    customTreeView2.SetCustomTreeRootDisplayId(myLinkType, id);
                    customTreeView2.UpdateViewComponent();
                    //if (customTreeView2.Nodes.Count > 0)
                    //{
                    //    customTreeView2.DoNewSelectionWithID(id);
                    //    customTreeView2.ExpandAll();
                    //}

                    customBlockTimeline1.DisplayID = id;
                    customBlockTimeline1.UpdateViewComponent();
                    Show_Properties(EventProperties.Create, id, -1);

                    break;
                case "Global Events":
                    customBlockTimeline1.DisplayID = id;
                    customBlockTimeline1.UpdateViewComponent();

                    customTreeView2.SetCustomTreeRootDisplayId(myLinkType, vsgController.ScenarioId);
                    customTreeView2.UpdateViewComponent();
                    if (customTreeView2.Nodes.Count > 0)
                    {
                        customTreeView2.DoNewSelectionWithID(id, myLinkType);
                        customTreeView2.ExpandAll();
                    }
                    HideAllProperties();
                    break;
                default:
                    HideAllProperties();
                    break;
            }
        }

        #region IViewComponentPanel Members
        private int _rootId = -1;
        public int RootId
        {
            get
            {
                return _rootId;
            }
            set
            {
               _rootId = value;
               createInstanceTree.SetCustomTreeRootId(myLinkType, _rootId);
               createInstanceTree.SetCustomTreeRootDisplayId(myLinkType, _rootId);
               customTreeView2.SetCustomTreeRootId(myLinkType, _rootId);
               customBlockTimeline1.RootID = _rootId;
               foreach (Diagram d in diagrams)
               {
                   d.RootID = _rootId;
                   d.ObjectSingleClicked += new GoObjectEventHandler(d_ObjectSingleClicked);
                   d.BackgroundSingleClicked += new GoInputEventHandler(d_BackgroundSingleClicked);
                   d.AddTypeFilter("MoveEvent");
                   d.AddTypeFilter("RevealEvent");
                   d.AddTypeFilter("LaunchEvent");
                   d.AddTypeFilter("WeaponLaunchEvent");
                   d.Xsl = "TimelineDiagram.xsl";
                   d.Level = 20;
                   
               }
               //customTreeView2.DisplayID = _rootId;
            }

        }

        void d_BackgroundSingleClicked(object sender, GoInputEventArgs e)
        {
            customTreeView2.ClearSelection();
        }

        #endregion

        private void customTabPage3_Click(object sender, EventArgs e)
        {

        }

        public void InitializePropertyPanels()
        {
            this.evtPnl_Reveal1.Dock = DockStyle.None;
            this.evtPnl_Reveal1.Location = new Point(0, 0);
            this.evtPnl_Reveal1.MinimumSize = new Size(801, 964);
            
            this.evtPnl_Create1.Dock = DockStyle.None;
            this.evtPnl_Create1.Location = new Point(0, 0);
            this.evtPnl_Create1.MinimumSize = new Size(372, 162);

            this.evtPnl_Move1.Dock = DockStyle.None;
            this.evtPnl_Move1.Location = new Point(0, 0);
            this.evtPnl_Move1.MinimumSize = new Size(390, 455);

            this.evtPnl_StateChange1.Dock = DockStyle.None;
            this.evtPnl_StateChange1.Location = new Point(0, 0);
            this.evtPnl_StateChange1.MinimumSize = new Size(539, 466);

            this.evtPnl_Transfer1.Dock = DockStyle.None;
            this.evtPnl_Transfer1.Location = new Point(0, 0);
            this.evtPnl_Transfer1.MinimumSize = new Size(388, 530);

            this.evtPnl_Reiterate1.Dock = DockStyle.None;
            this.evtPnl_Reiterate1.Location = new Point(0, 0);
            this.evtPnl_Reiterate1.MinimumSize = new Size(296, 300);

            this.evtPnl_Completion1.Dock = DockStyle.None;
            this.evtPnl_Completion1.Location = new Point(0, 0);
            this.evtPnl_Completion1.MinimumSize = new Size(401, 450);

            this.evtPnl_SpeciesCompletion1.Dock = DockStyle.None;
            this.evtPnl_SpeciesCompletion1.Location = new Point(0, 0);
            this.evtPnl_SpeciesCompletion1.MinimumSize = new Size(493, 262);
            
            this.evtPnl_Launch1.Dock = DockStyle.None;
            this.evtPnl_Launch1.Location = new Point(0, 0);
            this.evtPnl_Launch1.MinimumSize = new Size(734, 982);

            this.evtPnl_WeaponLaunch1.Dock = DockStyle.None;
            this.evtPnl_WeaponLaunch1.Location = new Point(0, 0);
            this.evtPnl_WeaponLaunch1.MinimumSize = new Size(734, 982);

            this.evtPnl_Flush1.Dock = DockStyle.None;
            this.evtPnl_Flush1.Location = new Point(0, 0);
            this.evtPnl_Flush1.MinimumSize = new Size(388, 136);

            this.evtPnl_EngramRemove1.Dock = DockStyle.None;
            this.evtPnl_EngramRemove1.Location = new Point(0, 0);
            this.evtPnl_EngramRemove1.MinimumSize = new Size(309, 202);

            this.evtPnl_EngramChange1.Dock = DockStyle.None;
            this.evtPnl_EngramChange1.Location = new Point(0, 0);
            this.evtPnl_EngramChange1.MinimumSize = new Size(303, 208);

            this.evtPnl_ChatroomOpen1.Dock = DockStyle.None;
            this.evtPnl_ChatroomOpen1.Location = new Point(0, 0);
            this.evtPnl_ChatroomOpen1.MinimumSize = new Size(271, 264);

            this.evtPnl_SendChatMessage1.Dock = DockStyle.None;
            this.evtPnl_SendChatMessage1.Location = new Point(0, 0);
            this.evtPnl_SendChatMessage1.MinimumSize = new Size(271, 264);
            
            this.evtPnl_ChatroomClose1.Dock = DockStyle.None;
            this.evtPnl_ChatroomClose1.Location = new Point(0, 0);
            this.evtPnl_ChatroomClose1.MinimumSize = new Size(309, 86);

            this.evtPnl_VoiceChannelOpen1.Dock = DockStyle.None;
            this.evtPnl_VoiceChannelOpen1.Location = new Point(0, 0);
            this.evtPnl_VoiceChannelOpen1.MinimumSize = new Size(271, 264);


            this.evtPnl_VoiceChannelClose1.Dock = DockStyle.None;
            this.evtPnl_VoiceChannelClose1.Location = new Point(0, 0);
            this.evtPnl_VoiceChannelClose1.MinimumSize = new Size(309, 86);

            this.evtPnl_SendVoiceMessage1.Dock = DockStyle.None;
            this.evtPnl_SendVoiceMessage1.Location = new Point(0, 0);
            this.evtPnl_SendVoiceMessage1.MinimumSize = new Size(271, 264);

            this.evtPnl_SendVoiceMessageToUser1.Dock = DockStyle.None;
            this.evtPnl_SendVoiceMessageToUser1.Location = new Point(0, 0);
            this.evtPnl_SendVoiceMessageToUser1.MinimumSize = new Size(271, 264);

            //this.subplatform1.Dock = DockStyle.None;
            //this.subplatform1.Location = new Point(0, 0);
            //this.subplatform1.MinimumSize = new Size(256, 168);


            //this.armament1.Dock = DockStyle.None;
            //this.armament1.Location = new Point(0, 0);
            //this.armament1.MinimumSize = new Size(252, 173);

        }
        public void HideAllProperties() // bug fix - hide all regardless of visibility (panels could be
                                        // blocked by timeline)
        {
            this.eventPropertiesTabPage.Description = "";

            this.evtPnl_ChatroomClose1.Hide();

            this.evtPnl_VoiceChannelOpen1.Hide();

            this.evtPnl_VoiceChannelClose1.Hide();

            this.evtPnl_SendVoiceMessage1.Hide();

            this.evtPnl_SendVoiceMessageToUser1.Hide();

            this.evtPnl_Create1.Hide();

            this.evtPnl_ChatroomOpen1.Hide();

            this.evtPnl_SendChatMessage1.Hide();

            this.evtPnl_EngramChange1.Hide();

            this.evtPnl_EngramRemove1.Hide();

            this.evtPnl_Flush1.Hide();

            this.evtPnl_Launch1.Hide();

            this.evtPnl_WeaponLaunch1.Hide();

            this.evtPnl_Move1.Hide();

            this.evtPnl_Reveal1.Hide();

            this.evtPnl_StateChange1.Hide();

            this.evtPnl_Transfer1.Hide();

            this.evtPnl_Reiterate1.Hide();

            this.evtPnl_Completion1.Hide();

            this.evtPnl_SpeciesCompletion1.Hide();

            //this.subplatform1.Hide();

            //this.armament1.Hide();
        }
        
        public enum EventProperties : int
        {
            Blank,
            Create,
            ChatroomClose, 
            ChatroomOpen, 
            SendChatMessage,
            EngramChange, 
            EngramRemove,
            Flush, 
            Launch, 
            WeaponLaunch,
            Move, 
            Reveal, 
            StateChange, 
            Transfer,
            Completion,
            SpeciesCompletion,
            Reiterate,
            Subplatform,
            VoiceChannelOpen,
            VoiceChannelClose,
            SendVoiceMessage,
            Armament,
            SendVoiceMessageToUser
        }

        public void Show_Properties(EventProperties property_panel, int compID, int parentCompID)
        {
            currentProperties = property_panel;
            if (property_panel == EventProperties.Blank)
            {
                
                HideAllProperties();
            }

            if (this.evtPnl_ChatroomClose1.Visible && (property_panel != EventProperties.ChatroomClose))
            {
                this.evtPnl_ChatroomClose1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.ChatroomClose)
                {
                    if (!this.evtPnl_ChatroomClose1.Visible)
                    {
                        this.evtPnl_ChatroomClose1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Close Chatroom Event Properties";
                    this.evtPnl_ChatroomClose1.DisplayID = compID;
                    this.evtPnl_ChatroomClose1.ParentCompID = parentCompID;
                    this.evtPnl_ChatroomClose1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_VoiceChannelClose1.Visible && (property_panel != EventProperties.VoiceChannelClose))
            {
                this.evtPnl_VoiceChannelClose1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.VoiceChannelClose)
                {
                    if (!this.evtPnl_VoiceChannelClose1.Visible)
                    {
                        this.evtPnl_VoiceChannelClose1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Close Voice Channel Event Properties";
                    this.evtPnl_VoiceChannelClose1.DisplayID = compID;
                    this.evtPnl_VoiceChannelClose1.ParentCompID = parentCompID;
                    this.evtPnl_VoiceChannelClose1.UpdateViewComponent();
                }

            }

            if (this.evtPnl_Create1.Visible && (property_panel != EventProperties.Create))
            {
                this.evtPnl_Create1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Create)
                {
                    if (!this.evtPnl_Create1.Visible)
                    {
                        this.evtPnl_Create1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Create Event Properties";
                    this.evtPnl_Create1.DisplayID = compID;
                    this.evtPnl_Create1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_ChatroomOpen1.Visible && (property_panel != EventProperties.ChatroomOpen))
            {
                this.evtPnl_ChatroomOpen1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.ChatroomOpen)
                {
                    if (!this.evtPnl_ChatroomOpen1.Visible)
                    {
                        this.evtPnl_ChatroomOpen1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Open Chatroom Event Properties";
                    this.evtPnl_ChatroomOpen1.DisplayID = compID;
                    this.evtPnl_ChatroomOpen1.ParentCompID = parentCompID;
                    this.evtPnl_ChatroomOpen1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_SendChatMessage1.Visible && (property_panel != EventProperties.SendChatMessage))
            {
                this.evtPnl_SendChatMessage1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.SendChatMessage)
                {
                    if (!this.evtPnl_SendChatMessage1.Visible)
                    {
                        this.evtPnl_SendChatMessage1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Send Chat Message Event Properties";
                    this.evtPnl_SendChatMessage1.DisplayID = compID;
                    this.evtPnl_SendChatMessage1.ParentCompID = parentCompID;
                    this.evtPnl_SendChatMessage1.UpdateViewComponent();
                }

            }


            if (this.evtPnl_VoiceChannelOpen1.Visible && (property_panel != EventProperties.VoiceChannelOpen))
            {
                this.evtPnl_VoiceChannelOpen1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.VoiceChannelOpen)
                {
                    if (!this.evtPnl_VoiceChannelOpen1.Visible)
                    {
                        this.evtPnl_VoiceChannelOpen1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Open Voice Channel Event Properties";
                    this.evtPnl_VoiceChannelOpen1.DisplayID = compID;
                    this.evtPnl_VoiceChannelOpen1.ParentCompID = parentCompID;
                    this.evtPnl_VoiceChannelOpen1.UpdateViewComponent();
                }

            }

            if (this.evtPnl_SendVoiceMessage1.Visible && (property_panel != EventProperties.SendVoiceMessage))
            {
                this.evtPnl_SendVoiceMessage1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.SendVoiceMessage)
                {
                    if (!this.evtPnl_SendVoiceMessage1.Visible)
                    {
                        this.evtPnl_SendVoiceMessage1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Send Voice Message Event Properties";
                    this.evtPnl_SendVoiceMessage1.DisplayID = compID;
                    this.evtPnl_SendVoiceMessage1.ParentCompID = parentCompID;
                    this.evtPnl_SendVoiceMessage1.UpdateViewComponent();
                }

            }

            //evtPnl_SendVoiceMessageToUser1
            if (this.evtPnl_SendVoiceMessageToUser1.Visible && (property_panel != EventProperties.SendVoiceMessageToUser))
            {
                this.evtPnl_SendVoiceMessageToUser1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.SendVoiceMessageToUser)
                {
                    if (!this.evtPnl_SendVoiceMessageToUser1.Visible)
                    {
                        this.evtPnl_SendVoiceMessageToUser1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Send Voice Message To User Event Properties";
                    this.evtPnl_SendVoiceMessageToUser1.DisplayID = compID;
                    this.evtPnl_SendVoiceMessageToUser1.ParentCompID = parentCompID;
                    this.evtPnl_SendVoiceMessageToUser1.UpdateViewComponent();
                }

            }

            if (this.evtPnl_EngramChange1.Visible && (property_panel != EventProperties.EngramChange))
            {
                this.evtPnl_EngramChange1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.EngramChange)
                {
                    if (!this.evtPnl_EngramChange1.Visible)
                    {
                        this.evtPnl_EngramChange1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Change Engram Event Properties";
                    this.evtPnl_EngramChange1.DisplayID = compID;
                    this.evtPnl_EngramChange1.ParentCompID = parentCompID;
                    this.evtPnl_EngramChange1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_EngramRemove1.Visible && (property_panel != EventProperties.EngramRemove))
            {
                this.evtPnl_EngramRemove1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.EngramRemove)
                {
                    if (!this.evtPnl_EngramRemove1.Visible)
                    {
                        this.evtPnl_EngramRemove1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Remove Engram Event Properties";
                    this.evtPnl_EngramRemove1.DisplayID = compID;
                    this.evtPnl_EngramRemove1.ParentCompID = parentCompID;
                    this.evtPnl_EngramRemove1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_Flush1.Visible && (property_panel != EventProperties.Flush))
            {
                this.evtPnl_Flush1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Flush)
                {
                    if (!this.evtPnl_Flush1.Visible)
                    {
                        this.evtPnl_Flush1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Flush Event Properties";
                    this.evtPnl_Flush1.DisplayID = compID;
                    this.evtPnl_Flush1.ParentCompID = parentCompID;
                    this.evtPnl_Flush1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_Launch1.Visible && (property_panel != EventProperties.Launch))
            {
                this.evtPnl_Launch1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Launch)
                {
                    if (!this.evtPnl_Launch1.Visible)
                    {
                        this.evtPnl_Launch1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Launch Event Properties";
                    this.evtPnl_Launch1.DisplayID = compID;
                    this.evtPnl_Launch1.ParentCompID = parentCompID;
                    //this.evtPnl_Launch1.CreateCompID = customTreeView2.DisplayID;
                    this.evtPnl_Launch1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_WeaponLaunch1.Visible && (property_panel != EventProperties.WeaponLaunch))
            {
                this.evtPnl_WeaponLaunch1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.WeaponLaunch)
                {
                    if (!this.evtPnl_WeaponLaunch1.Visible)
                    {
                        this.evtPnl_WeaponLaunch1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Weapon Launch Event Properties";
                    this.evtPnl_WeaponLaunch1.DisplayID = compID;
                    this.evtPnl_WeaponLaunch1.ParentCompID = parentCompID;
                    //this.evtPnl_Launch1.CreateCompID = customTreeView2.DisplayID;
                    this.evtPnl_WeaponLaunch1.UpdateViewComponent();
                }

            }

            if (this.evtPnl_Move1.Visible && (property_panel != EventProperties.Move))
            {
                this.evtPnl_Move1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Move)
                {
                    if (!this.evtPnl_Move1.Visible)
                    {
                        this.evtPnl_Move1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Move Event Properties";
                    this.evtPnl_Move1.DisplayID = compID;
                    this.evtPnl_Move1.ParentCompID = parentCompID;
                    this.evtPnl_Move1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_Reveal1.Visible && (property_panel != EventProperties.Reveal))
            {
                this.evtPnl_Reveal1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Reveal)
                {
                    if (!this.evtPnl_Reveal1.Visible)
                    {
                        this.evtPnl_Reveal1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Reveal Event Properties";
                    this.evtPnl_Reveal1.DisplayID = compID;
                    this.evtPnl_Reveal1.ParentCompID = parentCompID;
                    this.evtPnl_Reveal1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_StateChange1.Visible && (property_panel != EventProperties.StateChange))
            {
                this.evtPnl_StateChange1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.StateChange)
                {
                    if (!this.evtPnl_StateChange1.Visible)
                    {
                        this.evtPnl_StateChange1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "State Change Event Properties";
                    this.evtPnl_StateChange1.DisplayID = compID;
                    this.evtPnl_StateChange1.ParentCompID = parentCompID;
                    //this.evtPnl_StateChange1.CreateCompID = customTreeView2.DisplayID;
                    this.evtPnl_StateChange1.UpdateViewComponent();
                    
                }
                
            }

            if (this.evtPnl_Transfer1.Visible && (property_panel != EventProperties.Transfer))
            {
                this.evtPnl_Transfer1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Transfer)
                {
                    if (!this.evtPnl_Transfer1.Visible)
                    {
                        this.evtPnl_Transfer1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Transfer Event Properties";
                    this.evtPnl_Transfer1.DisplayID = compID;
                    this.evtPnl_Transfer1.ParentCompID = parentCompID;
                    this.evtPnl_Transfer1.UpdateViewComponent();
                }
                
            }

            if (this.evtPnl_Completion1.Visible && (property_panel != EventProperties.Completion))
            {
                this.evtPnl_Completion1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Completion)
                {
                    if (!this.evtPnl_Completion1.Visible)
                    {
                        this.evtPnl_Completion1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Completion Event Properties";
                    this.evtPnl_Completion1.DisplayID = compID;
                    this.evtPnl_Completion1.ParentCompID = parentCompID;
                    //this.evtPnl_Completion1.CreateCompID = customTreeView2.DisplayID;
                    this.evtPnl_Completion1.UpdateViewComponent();
                }
                
            }
            if (this.evtPnl_SpeciesCompletion1.Visible && (property_panel != EventProperties.SpeciesCompletion))
            {
                this.evtPnl_SpeciesCompletion1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.SpeciesCompletion)
                {
                    if (!this.evtPnl_SpeciesCompletion1.Visible)
                    {
                        this.evtPnl_SpeciesCompletion1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "SpeciesCompletion Event Properties";
                    this.evtPnl_SpeciesCompletion1.DisplayID = compID;
                    this.evtPnl_SpeciesCompletion1.ParentCompID = parentCompID;
                    //this.evtPnl_SpeciesCompletion1.CreateCompID = customTreeView2.DisplayID;
                    this.evtPnl_SpeciesCompletion1.UpdateViewComponent();
                }

            }

            if (this.evtPnl_Reiterate1.Visible && (property_panel != EventProperties.Reiterate))
            {
                this.evtPnl_Reiterate1.Hide();
            }
            else
            {
                if (property_panel == EventProperties.Reiterate)
                {
                    if (!this.evtPnl_Reiterate1.Visible)
                    {
                        this.evtPnl_Reiterate1.Show();
                    }
                    this.eventPropertiesTabPage.Description = "Reiterate Event Properties";
                    this.evtPnl_Reiterate1.DisplayID = compID;
                    this.evtPnl_Reiterate1.ParentCompID = parentCompID;
                    this.evtPnl_Reiterate1.UpdateViewComponent();
                }
                
            }

            //if (this.subplatform1.Visible && (property_panel != EventProperties.Subplatform))
            //{
            //    this.subplatform1.Hide();
            //}
            //else
            //{
            //    if (property_panel == EventProperties.Subplatform)
            //    {
            //        if (!this.subplatform1.Visible)
            //        {
            //            this.subplatform1.Show();
            //        }
            //        this.eventPropertiesTabPage.Description = "Subplatform Properties";
            //        this.subplatform1.DisplayID = compID;
            //        this.subplatform1.ParentCompID = parentCompID;
            //        this.subplatform1.UpdateViewComponent();
            //    }

            //}


            //if (this.armament1.Visible && (property_panel != EventProperties.Armament))
            //{
            //    this.armament1.Hide();
            //}
            //else
            //{
            //    if (property_panel == EventProperties.Armament)
            //    {
            //        if (!this.armament1.Visible)
            //        {
            //            this.armament1.Show();
            //        }
            //        this.eventPropertiesTabPage.Description = "Armament Properties";
            //        this.armament1.DisplayID = compID;
            //        this.armament1.ParentCompID = parentCompID;
            //        this.armament1.UpdateViewComponent();
            //    }

            //}  

        }
        private void UpdateProperties()
        {

            switch (currentProperties)
            {
                case EventProperties.Blank:
                    HideAllProperties();
                    break;
                case EventProperties.Create:
                    this.evtPnl_Create1.UpdateViewComponent();
                    break;
                case EventProperties.ChatroomClose:
                    this.evtPnl_ChatroomClose1.UpdateViewComponent();
                    break;
                case EventProperties.ChatroomOpen:
                    this.evtPnl_ChatroomOpen1.UpdateViewComponent();
                    break;
                case EventProperties.SendChatMessage:
                    this.evtPnl_SendChatMessage1.UpdateViewComponent();
                    break;
                case EventProperties.VoiceChannelClose:
                    this.evtPnl_VoiceChannelClose1.UpdateViewComponent();
                    break;
                case EventProperties.VoiceChannelOpen:
                    this.evtPnl_VoiceChannelOpen1.UpdateViewComponent();
                    break;
                case EventProperties.SendVoiceMessage:
                    this.evtPnl_SendVoiceMessage1.UpdateViewComponent();
                    break;
                case EventProperties.SendVoiceMessageToUser:
                    this.evtPnl_SendVoiceMessageToUser1.UpdateViewComponent();
                    break;
                case EventProperties.EngramChange:
                    this.evtPnl_EngramChange1.UpdateViewComponent();
                    break;
                case EventProperties.EngramRemove:
                    this.evtPnl_EngramRemove1.UpdateViewComponent();
                    break;
                case EventProperties.Flush:
                    this.evtPnl_Flush1.UpdateViewComponent();
                    break;
                case EventProperties.Launch:
                    this.evtPnl_Launch1.UpdateViewComponent();
                    break;
                case EventProperties.WeaponLaunch:
                    this.evtPnl_WeaponLaunch1.UpdateViewComponent();
                    break;
                case EventProperties.Move:
                    this.evtPnl_Move1.UpdateViewComponent();
                    break;
                case EventProperties.Reveal:
                    this.evtPnl_Reveal1.UpdateViewComponent();
                    break;
                case EventProperties.StateChange:
                    this.evtPnl_StateChange1.UpdateViewComponent();
                    break;
                case EventProperties.Transfer:
                    this.evtPnl_Transfer1.UpdateViewComponent();
                    break;
                case EventProperties.Completion:
                    this.evtPnl_Completion1.UpdateViewComponent();
                    break;
                case EventProperties.SpeciesCompletion:
                    this.evtPnl_SpeciesCompletion1.UpdateViewComponent();
                    break;
                case EventProperties.Reiterate:
                    this.evtPnl_Reiterate1.UpdateViewComponent();
                    break;

                //case EventProperties.Subplatform:
                //    this.subplatform1.UpdateViewComponent();
                //    break;


                //case EventProperties.Armament:
                //    this.armament1.UpdateViewComponent();
                //    break;
            }
        }

        private void customTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessingNode myNode = (ProcessingNode)e.Node;

            //createInstanceTree.ClearSelection();
            diagramPanel1.SelectNodeWithID(-1);

            HideAllProperties();
            if (myNode.NodeType == "MoveEvent")
            {
                Show_Properties(EventProperties.Move,myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                diagramPanel1.SelectNodeWithID(myNode.NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "CreateEvent")
            {
                Show_Properties(EventProperties.Create, myNode.NodeID, -1);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "CloseChatRoomEvent")
            {
                Show_Properties(EventProperties.ChatroomClose, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "OpenChatRoomEvent")
            {
                Show_Properties(EventProperties.ChatroomOpen, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "SendChatMessageEvent")
            {
                Show_Properties(EventProperties.SendChatMessage, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "CloseVoiceChannelEvent")
            {
                Show_Properties(EventProperties.VoiceChannelClose, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "OpenVoiceChannelEvent")
            {
                Show_Properties(EventProperties.VoiceChannelOpen, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "SendVoiceMessageEvent")
            {
                Show_Properties(EventProperties.SendVoiceMessage, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "SendVoiceMessageToUserEvent")
            {
                Show_Properties(EventProperties.SendVoiceMessageToUser, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "ChangeEngramEvent")
            {
                Show_Properties(EventProperties.EngramChange, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "RemoveEngramEvent")
            {
                Show_Properties(EventProperties.EngramRemove, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "FlushEvent")
            {
                Show_Properties(EventProperties.Flush, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "LaunchEvent")
            {
                Show_Properties(EventProperties.Launch, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                diagramPanel1.SelectNodeWithID(myNode.NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "WeaponLaunchEvent")
            {
                Show_Properties(EventProperties.WeaponLaunch, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                diagramPanel1.SelectNodeWithID(myNode.NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "RevealEvent")
            {
                Show_Properties(EventProperties.Reveal, myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                diagramPanel1.SelectNodeWithID(myNode.NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "StateChangeEvent")
            {
                Show_Properties(EventProperties.StateChange, myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "TransferEvent")
            {
                Show_Properties(EventProperties.Transfer, myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "CompletionEvent")
            {
                Show_Properties(EventProperties.Completion, myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "SpeciesCompletionEvent")
            {
                Show_Properties(EventProperties.SpeciesCompletion, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }
            if (myNode.NodeType == "ReiterateEvent")
            {
                Show_Properties(EventProperties.Reiterate, myNode.NodeID,((ProcessingNode)myNode.Parent).NodeID);
                SelectBlockTimelineCell(myNode.NodeID);
                return;
            }

            //if (myNode.NodeType == "Subplatform")
            //{
            //    Show_Properties(EventProperties.Subplatform, myNode.NodeID, ((ProcessingNode)myNode.Parent.Parent).NodeID);
            //    SelectBlockTimelineCell(myNode.NodeID);
            //    return;
            //}


            //if (myNode.NodeType == "Armament")
            //{
            //    Show_Properties(EventProperties.Armament, myNode.NodeID, ((ProcessingNode)myNode.Parent).NodeID);
            //    SelectBlockTimelineCell(myNode.NodeID);
            //    return;
            //}

            //if (myNode.NodeType == "Armaments")
            //{
            //    HideAllProperties();
            //    return;
            //}

        }

        private void createInstanceTree_ItemAdd(string addItemString, int nodeID, int linkID, string itemType, string itemName, string linkType)
        {
            //loadViewComponent(itemName, nodeID);
        }

        private void customTreeView2_ItemAdd(string addItemString, int nodeID, int linkID, string itemType, string itemName, string linkType)
        {
        }

        private void SelectBlockTimelineCell(int NodeID)
        {
            // Highlight the correct cell in the Block Timeline.
            foreach (Control c in customBlockTimeline1.Cells)
            {
                if (c is CustomBlockTimelineCell)
                {
                    CustomBlockTimelineCell cell = (CustomBlockTimelineCell)c;
                    if (((int)cell.Payload) == NodeID)
                    {
                        customBlockTimeline1.DeSelectCell();
                        customBlockTimeline1.SelectCell(cell);
                    }
                }
            }
        }

        private void DeleteBlockTimelineCell(int NodeID)
        {
            foreach (Control c in customBlockTimeline1.Cells)
            {
                if (c is CustomBlockTimelineCell)
                {
                    CustomBlockTimelineCell cell = (CustomBlockTimelineCell)c;
                    if (((int)cell.Payload) == NodeID)
                    {
                        customBlockTimeline1.DeleteCell(cell);
                    }
                }
            }
        }
        public void OnBlockTimelineSelection(CustomBlockTimelineCell cell)
        {
            if (cell.Payload != null)
            {
                customTreeView2.DoNewSelectionWithID((int)cell.Payload, myLinkType);
            }
            else
            {
                Console.WriteLine("Cell Name {0}", cell.Text);
            }
        }

        private void customTreeView2_ItemDelete(string deleteFunction, ProcessingNode deletedItem)
        {
            DeleteBlockTimelineCell(deletedItem.NodeID);
        }

        private void customTabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customTabControl3.SelectedIndex == 0)
            {
                //createInstanceTree.UpdateViewComponent();
                //createInstanceTree.ExpandAll();
                //customTreeView2.UpdateViewComponent();
                //customTreeView2.ExpandAll();
                UpdateProperties();
            }
            if (customTabControl3.SelectedIndex == 1)
            {
                customBlockTimeline1.UpdateViewComponent();
            }
        }
    }
}

