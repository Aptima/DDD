namespace VSG.ViewComponentPanels
{
    partial class TimelineViewComponentPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage2 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.createInstanceTree = new AME.Views.View_Components.CustomTreeView();
            this.split_Aware_West_East_Panel1 = new AME.Views.View_Components.Split_Aware_West_East_Panel();
            this.split_Aware_North_South_Panel1 = new AME.Views.View_Components.Split_Aware_North_South_Panel();
            this.customTabControl2 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.customTreeView2 = new AME.Views.View_Components.CustomTreeView();
            this.split_Aware_North_South_Panel2 = new AME.Views.View_Components.Split_Aware_North_South_Panel();
            this.diagramPanel1 = new AME.Views.View_Components.DiagramPanel();
            this.customTabControl3 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.eventPropertiesTabPage = new AME.Views.View_Components.CustomTabPage(this.components);
            this.evtPnl_SendVoiceMessage1 = new VSG.ViewComponents.EvtPnl_SendVoiceMessage();
            this.evtPnl_SendChatMessage1 = new VSG.ViewComponents.EvtPnl_SendChatMessage();
            this.evtPnl_Transfer1 = new VSG.ViewComponents.EvtPnl_Transfer();
            this.evtPnl_StateChange1 = new VSG.ViewComponents.EvtPnl_StateChange();
            this.evtPnl_Reveal1 = new VSG.ViewComponents.EvtPnl_Reveal();
            this.evtPnl_Move1 = new VSG.ViewComponents.EvtPnl_Move();
            this.evtPnl_Launch1 = new VSG.ViewComponents.EvtPnl_Launch();
            this.evtPnl_WeaponLaunch1 = new VSG.ViewComponents.EvtPnl_WeaponLaunch();
            this.evtPnl_Flush1 = new VSG.ViewComponents.EvtPnl_Flush();
            this.evtPnl_EngramRemove1 = new VSG.ViewComponents.EvtPnl_EngramRemove();
            this.evtPnl_EngramChange1 = new VSG.ViewComponents.EvtPnl_EngramChange();
            this.evtPnl_ChatroomOpen1 = new VSG.ViewComponents.EvtPnl_ChatroomOpen();
            this.evtPnl_ChatroomClose1 = new VSG.ViewComponents.EvtPnl_ChatroomClose();
            this.evtPnl_VoiceChannelOpen1 = new VSG.ViewComponents.EvtPnl_VoiceChannelOpen();
            this.evtPnl_VoiceChannelClose1 = new VSG.ViewComponents.EvtPnl_VoiceChannelClose();
            this.evtPnl_Completion1 = new VSG.ViewComponents.EvtPnl_Completion();
            this.evtPnl_Reiterate1 = new VSG.ViewComponents.EvtPnl_Reiterate();
            this.evtPnl_Create1 = new VSG.ViewComponents.EvtPnl_Create();
            this.evtPnl_SpeciesCompletion1 = new VSG.ViewComponents.EvtPnl_SpeciesCompletion();
            this.customTabPage4 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.customBlockTimeline1 = new VSG.ViewComponents.CustomBlockTimeline();
            this.evtPnl_SendVoiceMessageToUser1 = new VSG.ViewComponents.EvtPnl_SendVoiceMessageToUser();
            this.customTabControl1.SuspendLayout();
            this.customTabPage2.SuspendLayout();
            this.split_Aware_West_East_Panel1.Panel1.SuspendLayout();
            this.split_Aware_West_East_Panel1.Panel2.SuspendLayout();
            this.split_Aware_West_East_Panel1.SuspendLayout();
            this.split_Aware_North_South_Panel1.Panel1.SuspendLayout();
            this.split_Aware_North_South_Panel1.Panel2.SuspendLayout();
            this.split_Aware_North_South_Panel1.SuspendLayout();
            this.customTabControl2.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.split_Aware_North_South_Panel2.Panel1.SuspendLayout();
            this.split_Aware_North_South_Panel2.Panel2.SuspendLayout();
            this.split_Aware_North_South_Panel2.SuspendLayout();
            this.customTabControl3.SuspendLayout();
            this.eventPropertiesTabPage.SuspendLayout();
            this.customTabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage2);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(455, 225);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage2
            // 
            this.customTabPage2.Controls.Add(this.createInstanceTree);
            this.customTabPage2.Description = "Create unit instances.";
            this.customTabPage2.Location = new System.Drawing.Point(4, 22);
            this.customTabPage2.Name = "customTabPage2";
            this.customTabPage2.Size = new System.Drawing.Size(447, 199);
            this.customTabPage2.TabIndex = 0;
            this.customTabPage2.Text = "Allocation";
            // 
            // createInstanceTree
            // 
            this.createInstanceTree.AllowDrop = true;
            this.createInstanceTree.AllowUserInput = true;
            this.createInstanceTree.Controller = null;
            this.createInstanceTree.DecorateNodes = false;
            this.createInstanceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createInstanceTree.HideSelection = false;
            this.createInstanceTree.Level = ((uint)(0u));
            this.createInstanceTree.Location = new System.Drawing.Point(0, 23);
            this.createInstanceTree.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.createInstanceTree.Name = "createInstanceTree";
            this.createInstanceTree.ShowRoot = true;
            this.createInstanceTree.Size = new System.Drawing.Size(447, 176);
            this.createInstanceTree.TabIndex = 0;
            this.createInstanceTree.UseNodeMap = false;
            this.createInstanceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.createInstanceTree_AfterSelect);
            this.createInstanceTree.ItemAdd += new AME.Views.View_Components.CustomTreeView.ItemAdded(this.createInstanceTree_ItemAdd);
            // 
            // split_Aware_West_East_Panel1
            // 
            this.split_Aware_West_East_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_Aware_West_East_Panel1.Location = new System.Drawing.Point(0, 0);
            this.split_Aware_West_East_Panel1.Name = "split_Aware_West_East_Panel1";
            // 
            // split_Aware_West_East_Panel1.Panel1
            // 
            this.split_Aware_West_East_Panel1.Panel1.Controls.Add(this.split_Aware_North_South_Panel1);
            // 
            // split_Aware_West_East_Panel1.Panel2
            // 
            this.split_Aware_West_East_Panel1.Panel2.Controls.Add(this.split_Aware_North_South_Panel2);
            this.split_Aware_West_East_Panel1.Size = new System.Drawing.Size(997, 679);
            this.split_Aware_West_East_Panel1.SplitterDistance = 455;
            this.split_Aware_West_East_Panel1.SplitterWidth = 3;
            this.split_Aware_West_East_Panel1.TabIndex = 1;
            // 
            // split_Aware_North_South_Panel1
            // 
            this.split_Aware_North_South_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_Aware_North_South_Panel1.Location = new System.Drawing.Point(0, 0);
            this.split_Aware_North_South_Panel1.Name = "split_Aware_North_South_Panel1";
            this.split_Aware_North_South_Panel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_Aware_North_South_Panel1.Panel1
            // 
            this.split_Aware_North_South_Panel1.Panel1.Controls.Add(this.customTabControl1);
            // 
            // split_Aware_North_South_Panel1.Panel2
            // 
            this.split_Aware_North_South_Panel1.Panel2.Controls.Add(this.customTabControl2);
            this.split_Aware_North_South_Panel1.Size = new System.Drawing.Size(455, 679);
            this.split_Aware_North_South_Panel1.SplitterDistance = 225;
            this.split_Aware_North_South_Panel1.SplitterWidth = 3;
            this.split_Aware_North_South_Panel1.TabIndex = 0;
            // 
            // customTabControl2
            // 
            this.customTabControl2.Controls.Add(this.customTabPage1);
            this.customTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl2.Location = new System.Drawing.Point(0, 0);
            this.customTabControl2.Name = "customTabControl2";
            this.customTabControl2.SelectedIndex = 0;
            this.customTabControl2.Size = new System.Drawing.Size(455, 451);
            this.customTabControl2.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.customTreeView2);
            this.customTabPage1.Description = "Create and edit unit events.";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(447, 425);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Unit\'s Events";
            // 
            // customTreeView2
            // 
            this.customTreeView2.AllowDrop = true;
            this.customTreeView2.AllowUserInput = true;
            this.customTreeView2.Controller = null;
            this.customTreeView2.DecorateNodes = false;
            this.customTreeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTreeView2.HideSelection = false;
            this.customTreeView2.Level = ((uint)(0u));
            this.customTreeView2.Location = new System.Drawing.Point(0, 23);
            this.customTreeView2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.customTreeView2.Name = "customTreeView2";
            this.customTreeView2.ShowRoot = true;
            this.customTreeView2.Size = new System.Drawing.Size(447, 402);
            this.customTreeView2.TabIndex = 0;
            this.customTreeView2.UseNodeMap = false;
            this.customTreeView2.ItemDelete += new AME.Views.View_Components.CustomTreeView.ItemDeleted(this.customTreeView2_ItemDelete);
            this.customTreeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customTreeView2_AfterSelect);
            this.customTreeView2.ItemAdd += new AME.Views.View_Components.CustomTreeView.ItemAdded(this.customTreeView2_ItemAdd);
            // 
            // split_Aware_North_South_Panel2
            // 
            this.split_Aware_North_South_Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_Aware_North_South_Panel2.Location = new System.Drawing.Point(0, 0);
            this.split_Aware_North_South_Panel2.Name = "split_Aware_North_South_Panel2";
            this.split_Aware_North_South_Panel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_Aware_North_South_Panel2.Panel1
            // 
            this.split_Aware_North_South_Panel2.Panel1.Controls.Add(this.diagramPanel1);
            // 
            // split_Aware_North_South_Panel2.Panel2
            // 
            this.split_Aware_North_South_Panel2.Panel2.Controls.Add(this.customTabControl3);
            this.split_Aware_North_South_Panel2.Size = new System.Drawing.Size(539, 679);
            this.split_Aware_North_South_Panel2.SplitterDistance = 463;
            this.split_Aware_North_South_Panel2.SplitterWidth = 3;
            this.split_Aware_North_South_Panel2.TabIndex = 0;
            // 
            // diagramPanel1
            // 
            this.diagramPanel1.Controller = null;
            this.diagramPanel1.DiagramGridStripLocation = new System.Drawing.Point(85, 0);
            this.diagramPanel1.DiagramGridStripVisible = false;
            this.diagramPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramPanel1.DropDownStripLocation = new System.Drawing.Point(111, 75);
            this.diagramPanel1.DropDownStripVisible = false;
            this.diagramPanel1.FilterStripLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.FilterStripVisible = false;
            this.diagramPanel1.FilterTypes = null;
            this.diagramPanel1.LinkTypes = new string[] {
        "Scenario"};
            this.diagramPanel1.Location = new System.Drawing.Point(0, 0);
            this.diagramPanel1.Name = "diagramPanel1";
            this.diagramPanel1.SelectedIndex = -1;
            this.diagramPanel1.SelectLinkLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.SelectLinkStripVisible = false;
            this.diagramPanel1.Size = new System.Drawing.Size(539, 463);
            this.diagramPanel1.TabIndex = 1;
            this.diagramPanel1.TopToolStripVisible = true;
            this.diagramPanel1.UsingGeoReferenceUTM = false;
            this.diagramPanel1.ZoomStripLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.ZoomStripVisible = true;
            // 
            // customTabControl3
            // 
            this.customTabControl3.Controls.Add(this.eventPropertiesTabPage);
            this.customTabControl3.Controls.Add(this.customTabPage4);
            this.customTabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl3.Location = new System.Drawing.Point(0, 0);
            this.customTabControl3.Name = "customTabControl3";
            this.customTabControl3.SelectedIndex = 0;
            this.customTabControl3.Size = new System.Drawing.Size(539, 213);
            this.customTabControl3.TabIndex = 0;
            this.customTabControl3.SelectedIndexChanged += new System.EventHandler(this.customTabControl3_SelectedIndexChanged);
            // 
            // eventPropertiesTabPage
            // 
            this.eventPropertiesTabPage.AutoScroll = true;
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_SendVoiceMessageToUser1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_SpeciesCompletion1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Create1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Reiterate1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Completion1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_VoiceChannelClose1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_VoiceChannelOpen1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_ChatroomClose1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_ChatroomOpen1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_EngramChange1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_EngramRemove1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Flush1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Launch1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_WeaponLaunch1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Move1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Reveal1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_StateChange1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_Transfer1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_SendChatMessage1);
            this.eventPropertiesTabPage.Controls.Add(this.evtPnl_SendVoiceMessage1);
            this.eventPropertiesTabPage.Description = "Show selected event\'s name here";
            this.eventPropertiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.eventPropertiesTabPage.Name = "eventPropertiesTabPage";
            this.eventPropertiesTabPage.Size = new System.Drawing.Size(531, 187);
            this.eventPropertiesTabPage.TabIndex = 0;
            this.eventPropertiesTabPage.Text = "Event Properties";
            this.eventPropertiesTabPage.Click += new System.EventHandler(this.customTabPage3_Click);
            // 
            // evtPnl_SendVoiceMessage1
            // 
            this.evtPnl_SendVoiceMessage1.Controller = null;
            this.evtPnl_SendVoiceMessage1.DisplayID = -1;
            this.evtPnl_SendVoiceMessage1.Location = new System.Drawing.Point(54, 50);
            this.evtPnl_SendVoiceMessage1.MinimumSize = new System.Drawing.Size(225, 61);
            this.evtPnl_SendVoiceMessage1.Name = "evtPnl_SendVoiceMessage1";
            this.evtPnl_SendVoiceMessage1.ParentCompID = -1;
            this.evtPnl_SendVoiceMessage1.Size = new System.Drawing.Size(310, 116);
            this.evtPnl_SendVoiceMessage1.TabIndex = 17;
            // 
            // evtPnl_SendChatMessage1
            // 
            this.evtPnl_SendChatMessage1.Controller = null;
            this.evtPnl_SendChatMessage1.DisplayID = -1;
            this.evtPnl_SendChatMessage1.Location = new System.Drawing.Point(13, 9);
            this.evtPnl_SendChatMessage1.MinimumSize = new System.Drawing.Size(225, 61);
            this.evtPnl_SendChatMessage1.Name = "evtPnl_SendChatMessage1";
            this.evtPnl_SendChatMessage1.ParentCompID = -1;
            this.evtPnl_SendChatMessage1.Size = new System.Drawing.Size(310, 227);
            this.evtPnl_SendChatMessage1.TabIndex = 16;
            // 
            // evtPnl_Transfer1
            // 
            this.evtPnl_Transfer1.Controller = null;
            this.evtPnl_Transfer1.DisplayID = -1;
            this.evtPnl_Transfer1.Location = new System.Drawing.Point(92, 124);
            this.evtPnl_Transfer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Transfer1.MinimumSize = new System.Drawing.Size(238, 302);
            this.evtPnl_Transfer1.Name = "evtPnl_Transfer1";
            this.evtPnl_Transfer1.ParentCompID = -1;
            this.evtPnl_Transfer1.Size = new System.Drawing.Size(573, 658);
            this.evtPnl_Transfer1.TabIndex = 10;
            // 
            // evtPnl_StateChange1
            // 
            this.evtPnl_StateChange1.Controller = null;
            this.evtPnl_StateChange1.DisplayID = -1;
            this.evtPnl_StateChange1.Location = new System.Drawing.Point(2, 125);
            this.evtPnl_StateChange1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_StateChange1.MinimumSize = new System.Drawing.Size(362, 268);
            this.evtPnl_StateChange1.Name = "evtPnl_StateChange1";
            this.evtPnl_StateChange1.ParentCompID = -1;
            this.evtPnl_StateChange1.Size = new System.Drawing.Size(577, 600);
            this.evtPnl_StateChange1.TabIndex = 9;
            // 
            // evtPnl_Reveal1
            // 
            this.evtPnl_Reveal1.Controller = null;
            this.evtPnl_Reveal1.DisplayID = -1;
            this.evtPnl_Reveal1.Location = new System.Drawing.Point(92, 98);
            this.evtPnl_Reveal1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Reveal1.MinimumSize = new System.Drawing.Size(274, 392);
            this.evtPnl_Reveal1.Name = "evtPnl_Reveal1";
            this.evtPnl_Reveal1.ParentCompID = -1;
            this.evtPnl_Reveal1.Size = new System.Drawing.Size(601, 884);
            this.evtPnl_Reveal1.TabIndex = 8;
            // 
            // evtPnl_Move1
            // 
            this.evtPnl_Move1.Controller = null;
            this.evtPnl_Move1.DisplayID = -1;
            this.evtPnl_Move1.Location = new System.Drawing.Point(2, 103);
            this.evtPnl_Move1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Move1.MinimumSize = new System.Drawing.Size(224, 125);
            this.evtPnl_Move1.Name = "evtPnl_Move1";
            this.evtPnl_Move1.ParentCompID = -1;
            this.evtPnl_Move1.Size = new System.Drawing.Size(586, 626);
            this.evtPnl_Move1.TabIndex = 7;
            // 
            // evtPnl_Launch1
            // 
            this.evtPnl_Launch1.Controller = null;
            this.evtPnl_Launch1.DisplayID = -1;
            this.evtPnl_Launch1.Location = new System.Drawing.Point(92, 72);
            this.evtPnl_Launch1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Launch1.MinimumSize = new System.Drawing.Size(734, 982);
            this.evtPnl_Launch1.Name = "evtPnl_Launch1";
            this.evtPnl_Launch1.ParentCompID = -1;
            this.evtPnl_Launch1.Size = new System.Drawing.Size(734, 982);
            this.evtPnl_Launch1.TabIndex = 6;
            // 
            // evtPnl_WeaponLaunch1
            // 
            this.evtPnl_WeaponLaunch1.Controller = null;
            this.evtPnl_WeaponLaunch1.DisplayID = -1;
            this.evtPnl_WeaponLaunch1.Location = new System.Drawing.Point(92, 72);
            this.evtPnl_WeaponLaunch1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_WeaponLaunch1.MinimumSize = new System.Drawing.Size(734, 982);
            this.evtPnl_WeaponLaunch1.Name = "evtPnl_WeaponLaunch1";
            this.evtPnl_WeaponLaunch1.ParentCompID = -1;
            this.evtPnl_WeaponLaunch1.Size = new System.Drawing.Size(734, 982);
            this.evtPnl_WeaponLaunch1.TabIndex = 6;
            // 
            // evtPnl_Flush1
            // 
            this.evtPnl_Flush1.Controller = null;
            this.evtPnl_Flush1.DisplayID = -1;
            this.evtPnl_Flush1.Location = new System.Drawing.Point(2, 79);
            this.evtPnl_Flush1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Flush1.MinimumSize = new System.Drawing.Size(176, 166);
            this.evtPnl_Flush1.Name = "evtPnl_Flush1";
            this.evtPnl_Flush1.ParentCompID = -1;
            this.evtPnl_Flush1.Size = new System.Drawing.Size(176, 166);
            this.evtPnl_Flush1.TabIndex = 5;
            // 
            // evtPnl_EngramRemove1
            // 
            this.evtPnl_EngramRemove1.Controller = null;
            this.evtPnl_EngramRemove1.DisplayID = -1;
            this.evtPnl_EngramRemove1.Location = new System.Drawing.Point(92, 52);
            this.evtPnl_EngramRemove1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_EngramRemove1.Name = "evtPnl_EngramRemove1";
            this.evtPnl_EngramRemove1.ParentCompID = -1;
            this.evtPnl_EngramRemove1.Size = new System.Drawing.Size(99, 27);
            this.evtPnl_EngramRemove1.TabIndex = 4;
            // 
            // evtPnl_EngramChange1
            // 
            this.evtPnl_EngramChange1.Controller = null;
            this.evtPnl_EngramChange1.DisplayID = -1;
            this.evtPnl_EngramChange1.Location = new System.Drawing.Point(2, 52);
            this.evtPnl_EngramChange1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_EngramChange1.Name = "evtPnl_EngramChange1";
            this.evtPnl_EngramChange1.ParentCompID = -1;
            this.evtPnl_EngramChange1.Size = new System.Drawing.Size(419, 402);
            this.evtPnl_EngramChange1.TabIndex = 3;
            // 
            // evtPnl_ChatroomOpen1
            // 
            this.evtPnl_ChatroomOpen1.Controller = null;
            this.evtPnl_ChatroomOpen1.DisplayID = -1;
            this.evtPnl_ChatroomOpen1.Location = new System.Drawing.Point(92, 27);
            this.evtPnl_ChatroomOpen1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_ChatroomOpen1.MinimumSize = new System.Drawing.Size(203, 214);
            this.evtPnl_ChatroomOpen1.Name = "evtPnl_ChatroomOpen1";
            this.evtPnl_ChatroomOpen1.ParentCompID = -1;
            this.evtPnl_ChatroomOpen1.Size = new System.Drawing.Size(203, 214);
            this.evtPnl_ChatroomOpen1.TabIndex = 2;
            // 
            // evtPnl_ChatroomClose1
            // 
            this.evtPnl_ChatroomClose1.Controller = null;
            this.evtPnl_ChatroomClose1.DisplayID = -1;
            this.evtPnl_ChatroomClose1.Location = new System.Drawing.Point(2, 27);
            this.evtPnl_ChatroomClose1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_ChatroomClose1.MinimumSize = new System.Drawing.Size(225, 61);
            this.evtPnl_ChatroomClose1.Name = "evtPnl_ChatroomClose1";
            this.evtPnl_ChatroomClose1.ParentCompID = -1;
            this.evtPnl_ChatroomClose1.Size = new System.Drawing.Size(225, 61);
            this.evtPnl_ChatroomClose1.TabIndex = 1;
            // 
            // evtPnl_VoiceChannelOpen1
            // 
            this.evtPnl_VoiceChannelOpen1.Controller = null;
            this.evtPnl_VoiceChannelOpen1.DisplayID = -1;
            this.evtPnl_VoiceChannelOpen1.Location = new System.Drawing.Point(92, 27);
            this.evtPnl_VoiceChannelOpen1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_VoiceChannelOpen1.MinimumSize = new System.Drawing.Size(203, 214);
            this.evtPnl_VoiceChannelOpen1.Name = "evtPnl_VoiceChannelOpen1";
            this.evtPnl_VoiceChannelOpen1.ParentCompID = -1;
            this.evtPnl_VoiceChannelOpen1.Size = new System.Drawing.Size(203, 214);
            this.evtPnl_VoiceChannelOpen1.TabIndex = 12;
            // 
            // evtPnl_VoiceChannelClose1
            // 
            this.evtPnl_VoiceChannelClose1.Controller = null;
            this.evtPnl_VoiceChannelClose1.DisplayID = -1;
            this.evtPnl_VoiceChannelClose1.Location = new System.Drawing.Point(2, 27);
            this.evtPnl_VoiceChannelClose1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_VoiceChannelClose1.MinimumSize = new System.Drawing.Size(225, 61);
            this.evtPnl_VoiceChannelClose1.Name = "evtPnl_VoiceChannelClose1";
            this.evtPnl_VoiceChannelClose1.ParentCompID = -1;
            this.evtPnl_VoiceChannelClose1.Size = new System.Drawing.Size(225, 61);
            this.evtPnl_VoiceChannelClose1.TabIndex = 1;
            // 
            // evtPnl_Completion1
            // 
            this.evtPnl_Completion1.Controller = null;
            this.evtPnl_Completion1.DisplayID = -1;
            this.evtPnl_Completion1.Location = new System.Drawing.Point(186, 27);
            this.evtPnl_Completion1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Completion1.MinimumSize = new System.Drawing.Size(249, 358);
            this.evtPnl_Completion1.Name = "evtPnl_Completion1";
            this.evtPnl_Completion1.ParentCompID = -1;
            this.evtPnl_Completion1.Size = new System.Drawing.Size(572, 620);
            this.evtPnl_Completion1.TabIndex = 11;
            // 
            // evtPnl_Reiterate1
            // 
            this.evtPnl_Reiterate1.Controller = null;
            this.evtPnl_Reiterate1.DisplayID = -1;
            this.evtPnl_Reiterate1.Location = new System.Drawing.Point(186, 50);
            this.evtPnl_Reiterate1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Reiterate1.MinimumSize = new System.Drawing.Size(198, 111);
            this.evtPnl_Reiterate1.Name = "evtPnl_Reiterate1";
            this.evtPnl_Reiterate1.ParentCompID = -1;
            this.evtPnl_Reiterate1.Size = new System.Drawing.Size(570, 447);
            this.evtPnl_Reiterate1.TabIndex = 12;
            // 
            // evtPnl_Create1
            // 
            this.evtPnl_Create1.Controller = null;
            this.evtPnl_Create1.DisplayID = -1;
            this.evtPnl_Create1.Location = new System.Drawing.Point(102, 9);
            this.evtPnl_Create1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_Create1.MinimumSize = new System.Drawing.Size(244, 276);
            this.evtPnl_Create1.Name = "evtPnl_Create1";
            this.evtPnl_Create1.ParentCompID = -1;
            this.evtPnl_Create1.Size = new System.Drawing.Size(244, 276);
            this.evtPnl_Create1.TabIndex = 13;
            // 
            // evtPnl_SpeciesCompletion1
            // 
            this.evtPnl_SpeciesCompletion1.Controller = null;
            this.evtPnl_SpeciesCompletion1.DisplayID = -1;
            this.evtPnl_SpeciesCompletion1.Location = new System.Drawing.Point(0, 0);
            this.evtPnl_SpeciesCompletion1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.evtPnl_SpeciesCompletion1.Name = "evtPnl_SpeciesCompletion1";
            this.evtPnl_SpeciesCompletion1.ParentCompID = -1;
            this.evtPnl_SpeciesCompletion1.Size = new System.Drawing.Size(493, 262);
            this.evtPnl_SpeciesCompletion1.TabIndex = 15;
            // 
            // customTabPage4
            // 
            this.customTabPage4.Controls.Add(this.customBlockTimeline1);
            this.customTabPage4.Description = "Non Conditional Event Timeline";
            this.customTabPage4.Location = new System.Drawing.Point(4, 22);
            this.customTabPage4.Name = "customTabPage4";
            this.customTabPage4.Size = new System.Drawing.Size(348, 183);
            this.customTabPage4.TabIndex = 1;
            this.customTabPage4.Text = "Events Timeline";
            // 
            // customBlockTimeline1
            // 
            this.customBlockTimeline1.Controller = null;
            this.customBlockTimeline1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customBlockTimeline1.Location = new System.Drawing.Point(0, 23);
            this.customBlockTimeline1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customBlockTimeline1.Name = "customBlockTimeline1";
            this.customBlockTimeline1.Size = new System.Drawing.Size(348, 160);
            this.customBlockTimeline1.TabIndex = 1;
            // 
            // evtPnl_SendVoiceMessageToUser1
            // 
            this.evtPnl_SendVoiceMessageToUser1.Controller = null;
            this.evtPnl_SendVoiceMessageToUser1.DisplayID = -1;
            this.evtPnl_SendVoiceMessageToUser1.Location = new System.Drawing.Point(13, 27);
            this.evtPnl_SendVoiceMessageToUser1.MinimumSize = new System.Drawing.Size(225, 61);
            this.evtPnl_SendVoiceMessageToUser1.Name = "evtPnl_SendVoiceMessageToUser1";
            this.evtPnl_SendVoiceMessageToUser1.ParentCompID = -1;
            this.evtPnl_SendVoiceMessageToUser1.Size = new System.Drawing.Size(310, 116);
            this.evtPnl_SendVoiceMessageToUser1.TabIndex = 18;
            // 
            // TimelineViewComponentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.split_Aware_West_East_Panel1);
            this.Name = "TimelineViewComponentPanel";
            this.Size = new System.Drawing.Size(997, 679);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage2.ResumeLayout(false);
            this.split_Aware_West_East_Panel1.Panel1.ResumeLayout(false);
            this.split_Aware_West_East_Panel1.Panel2.ResumeLayout(false);
            this.split_Aware_West_East_Panel1.ResumeLayout(false);
            this.split_Aware_North_South_Panel1.Panel1.ResumeLayout(false);
            this.split_Aware_North_South_Panel1.Panel2.ResumeLayout(false);
            this.split_Aware_North_South_Panel1.ResumeLayout(false);
            this.customTabControl2.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.split_Aware_North_South_Panel2.Panel1.ResumeLayout(false);
            this.split_Aware_North_South_Panel2.Panel2.ResumeLayout(false);
            this.split_Aware_North_South_Panel2.ResumeLayout(false);
            this.customTabControl3.ResumeLayout(false);
            this.eventPropertiesTabPage.ResumeLayout(false);
            this.customTabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.DiagramPanel diagramPanel1;
        private AME.Views.View_Components.Split_Aware_North_South_Panel split_Aware_North_South_Panel2;
        private AME.Views.View_Components.CustomTabControl customTabControl3;
        private AME.Views.View_Components.CustomTabPage eventPropertiesTabPage;
        private AME.Views.View_Components.CustomTabPage customTabPage4;
        private VSG.ViewComponents.CustomBlockTimeline customBlockTimeline1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage2;
        private AME.Views.View_Components.CustomTreeView createInstanceTree;
        private AME.Views.View_Components.Split_Aware_West_East_Panel split_Aware_West_East_Panel1;
        private AME.Views.View_Components.Split_Aware_North_South_Panel split_Aware_North_South_Panel1;
        private AME.Views.View_Components.CustomTabControl customTabControl2;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private AME.Views.View_Components.CustomTreeView customTreeView2;
        private VSG.ViewComponents.EvtPnl_EngramChange evtPnl_EngramChange1;
        private VSG.ViewComponents.EvtPnl_ChatroomOpen evtPnl_ChatroomOpen1;
        private VSG.ViewComponents.EvtPnl_ChatroomClose evtPnl_ChatroomClose1;
        private VSG.ViewComponents.EvtPnl_VoiceChannelOpen evtPnl_VoiceChannelOpen1;
        private VSG.ViewComponents.EvtPnl_VoiceChannelClose evtPnl_VoiceChannelClose1;
        private VSG.ViewComponents.EvtPnl_Transfer evtPnl_Transfer1;
        private VSG.ViewComponents.EvtPnl_StateChange evtPnl_StateChange1;
        private VSG.ViewComponents.EvtPnl_Reveal evtPnl_Reveal1;
        private VSG.ViewComponents.EvtPnl_Move evtPnl_Move1;
        private VSG.ViewComponents.EvtPnl_Launch evtPnl_Launch1;
        private VSG.ViewComponents.EvtPnl_WeaponLaunch evtPnl_WeaponLaunch1;
        private VSG.ViewComponents.EvtPnl_Flush evtPnl_Flush1;
        private VSG.ViewComponents.EvtPnl_EngramRemove evtPnl_EngramRemove1;
        private VSG.ViewComponents.EvtPnl_Completion evtPnl_Completion1;
        private VSG.ViewComponents.EvtPnl_Reiterate evtPnl_Reiterate1;
        private VSG.ViewComponents.EvtPnl_Create evtPnl_Create1;
        private VSG.ViewComponents.EvtPnl_SpeciesCompletion evtPnl_SpeciesCompletion1;
        private VSG.ViewComponents.EvtPnl_SendChatMessage evtPnl_SendChatMessage1;
        private VSG.ViewComponents.EvtPnl_SendVoiceMessage evtPnl_SendVoiceMessage1;
        private VSG.ViewComponents.EvtPnl_SendVoiceMessageToUser evtPnl_SendVoiceMessageToUser1;
        //private VSG.ViewComponents.Armament armament1;
        //private VSG.ViewComponents.Subplatform subplatform1;
    }
}
