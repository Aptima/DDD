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

namespace VSG.ViewComponentPanels
{
    public partial class ScoringViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 rootId = -1;
        private VSGController vsgController;
        private RootController projectController;

        private ScorePanel scorePanel;
        private ScoringRulePanel scoringRulePanel;

        private string current_help_page = string.Empty;

        private bool _isInitialized = false;

        private String myLinkType;

        public ScoringViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this);

            InitializeComponent();

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            projectController = (RootController)AMEManager.Instance.Get("Project");
            vsgController.RegisterForUpdate(this);

            myLinkType = vsgController.ConfigurationLinkType;

            customTreeView1.Controller = vsgController;

            customTreeView1.AddCustomTreeRoot(myLinkType);
            customTreeView1.SetCustomTreeRootXsl(myLinkType, "Scoring.xsl");

            customTreeView1.Level = 3;
            customTreeView1.ShowRoot = false;


            scorePanel = new ScorePanel();
            scorePanel.Controller = vsgController;
            
            scoringRulePanel = new ScoringRulePanel();
            scoringRulePanel.Controller = vsgController;
            
            //scenarioImages1.Controller = vsgController;
            //scenarioInfo1.Controller = vsgController;
            //customCheckedListBox1.Controller = vsgController;
        }

        public void UpdateViewComponent()
        {
            customTreeView1.UpdateViewComponent();
            customTreeView1.ExpandAll();
            scorePanel.UpdateViewComponent();
            scoringRulePanel.UpdateViewComponent();

            // The following is a Hack to insure that a default content page is presented 
            //   the first time a user enters this view;
            if (!_isInitialized)
            {
                loadViewComponent(customTreeView1.Nodes[0].Name, -1);
                _isInitialized = true;
            }
        }

        private void customTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessingNode pNode = e.Node as ProcessingNode;
            Int32 nodeId = pNode.NodeID;
            String nodeName;
            if (pNode.NodeID != -1)
                nodeName = pNode.NodeType;
            else
                nodeName = e.Node.Name;


            loadViewComponent(nodeName, nodeId);
        }

        private void loadViewComponent(String name, Int32 id)
        {
            string pageName;
            switch (name)
            {
                case "Score":
                    //if (!splitContainer1.Panel2.Controls.Contains(decisionMakerVC))
                    //{
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(scorePanel);
                    //scorePage.Controls.Clear();
                    //scorePage.Controls.Add(scorePanel);
                    scorePanel.Dock = DockStyle.Fill;
                    //scorePanel.Dock = DockStyle.Fill;

                    //if (id >= 0)
                    //{
                    //    pageName = vsgController.GetComponentName(id);
                    //    scorePage.Text = String.Format("Score: {0}",pageName);
                    //}
                    scorePanel.ScoreID = id;

                    
                    scorePanel.UpdateViewComponent();

                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                case "Rule":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(scoringRulePanel);
                    //scorePage.Controls.Clear();
                    //scorePage.Controls.Add(scoringRulePanel);
                    
                    scoringRulePanel.Dock = DockStyle.Fill;
                    scoringRulePanel.ScoreRuleID = id;

                    //if (id >= 0)
                    //{
                    //    pageName = vsgController.GetComponentName(id);
                    //    scorePage.Text = String.Format("Score Rule: {0}", pageName);
                    //}

                    scoringRulePanel.UpdateViewComponent();

                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                default:
                    //splitContainer1.Panel2.Text = "";
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(customTabControlBlank);
                    customTabPageBlank.Description = name;
                    customTabControlBlank.Dock = DockStyle.Fill;
                    string help_page = string.Format(@"{0}/InlineHelp/{1}.html", System.Environment.CurrentDirectory, name);
                    if (System.IO.File.Exists(help_page))
                    {
                        if (current_help_page != help_page)
                        {
                            webBrowser1.Navigate(help_page);
                            current_help_page = help_page;
                        }
                    }

                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);

                    break;
            }
        }

        #region IViewComponentPanel Members

        public int RootId
        {
            get
            {
                return rootId;
            }
            set
            {
                rootId = value;
                if (rootId >= 0)
                {
                    // Set playfieldId in appropriate views. Playfield component is created automatically.
                    //ComponentOptions compOptions = new ComponentOptions();
                    //IXPathNavigable inav = projectController.GetComponentAndChildren(rootId, vsgController.ConfigurationLinkType, compOptions);
                    //XPathNavigator nav = inav.CreateNavigator().SelectSingleNode("/Components/Component/Component[@Type='Playfield']");

                    //String playfieldId = nav.GetAttribute(AME.Controllers.XmlSchemaConstants.Display.Component.Id, nav.NamespaceURI);
                    //scenarioImages1.PlayfieldId = Int32.Parse(playfieldId);
                    //scenarioInfo1.ScenarioId = rootId;
                    customTreeView1.SetCustomTreeRootId(myLinkType, rootId);
                    customTreeView1.SetCustomTreeRootDisplayId(myLinkType, rootId);
                    
                    
                }
            }
        }

        #endregion

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
