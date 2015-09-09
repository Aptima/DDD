using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;

using VSG.Controllers;
using VSG.ViewComponents;

using AME;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Nodes;

namespace VSG.ViewComponentPanels
{
    public partial class ObjectTypesViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 rootId = -1;
        private VSGController vsgController;

        private DecisionMaker decisionMakerVC = new DecisionMaker();
        private Team teamVC = new Team();
        private Classification classificationVC = new Classification();
        private Network networkVC = new Network();
        private Engram engramVC = new Engram();
        private Sensor sensorVC = new Sensor();
        private Emitter emitterVC = new Emitter();
      //  private VoiceChannel voiceChannelVC = new VoiceChannel();

        private Proximity proximityVC = new Proximity();
        private Effect effectVC = new Effect();

        private Singleton singletonVC = new Singleton();
        private Transition transitionVC = new Transition();
        private Contribution contributionVC = new Contribution();
        private Combo comboVC = new Combo();
        private State stateVC = new State();
        private Species speciesVC = new Species();
        private Level levelVC = new Level();
        private SensorRange sensor_rangeVC = new SensorRange();

        private bool _isResourceTypeInitialized = false;
        private bool _isPlayerTypeInitialized = false;

        private string current_help_page = string.Empty;

        private string myLinkType;

        public ObjectTypesViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this);

            InitializeComponent();

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            vsgController.RegisterForUpdate(this);

            myLinkType = vsgController.ConfigurationLinkType;

            customTreeView1.Controller = vsgController;

            customTreeView1.AddCustomTreeRoot(myLinkType);
            customTreeView1.SetCustomTreeRootXsl(myLinkType, "PlayerObjectTypes.xsl"); //"ResourcesObjectTypes.xsl";
            customTreeView1.FunctionProc += new CustomTreeView.FunctionProcessed(customTreeView1_FunctionProc);
            customTreeView2.FunctionProc += new CustomTreeView.FunctionProcessed(customTreeView1_FunctionProc);


            customTreeView1.Level = 3;
            customTreeView1.ShowRoot = false;

            customTreeView2.Controller = vsgController;

            customTreeView2.AddCustomTreeRoot(myLinkType);
            customTreeView2.SetCustomTreeRootXsl(myLinkType, "ResourcesObjectTypes.xsl");//"PlayerObjectTypes.xsl";

            customTreeView2.Level = 5;
            customTreeView2.ShowRoot = false;
            //polyTool.PolygonCreatedWithPoints += new PolygonDrawingTool.OnPolygonCreatedWithPoints(polyTool_PolygonCreatedWithPoints);
            customTreeView2.NodeFunctionList += new NodeFunctionListEventHandler(customTreeView2_NodeFunctionList);


            decisionMakerVC.Controller = vsgController;
            teamVC.Controller = vsgController;
            classificationVC.Controller = vsgController;
            networkVC.Controller = vsgController;
            engramVC.Controller = vsgController;
            sensorVC.Controller = vsgController;
            emitterVC.Controller = vsgController;
            proximityVC.Controller = vsgController;
            effectVC.Controller = vsgController;
            singletonVC.Controller = vsgController;
            transitionVC.Controller = vsgController;
            comboVC.Controller = vsgController;
            contributionVC.Controller = vsgController;
            stateVC.Controller = vsgController;
            speciesVC.Controller = vsgController;
            levelVC.Controller = vsgController;
            sensor_rangeVC.Controller = vsgController;
            //voiceChannelVC.Controller = vsgController;

            this.customTabControl1.SelectedIndexChanged += new EventHandler(customTabControl1_SelectedIndexChanged);
        }

        void customTreeView1_FunctionProc(ProcessingNode node, string functionName, string functionValue)
        {
            //throw new NotImplementedException();
            if (functionName == "Rename" && (node.NodeType == "Classification" || node.NodeType == "State"))
            {
                String oldVal = node.Text;
                int id = node.NodeID;
                IXPathNavigable iNavComponents = vsgController.GetComponent(id);
                XPathNavigator navComponents = iNavComponents.CreateNavigator();
                XPathNavigator component = navComponents.SelectSingleNode("/Components/Component");
                String newVal = component.GetAttribute("Name", component.NamespaceURI);
                //update classification rules
                UpdateAllClassificationRules(node.NodeType, id, oldVal, newVal);
            }
        }

        void UpdateAllClassificationRules(String whatWasUpdated, int id, String oldVal, String newVal)
        {
            IXPathNavigable iNavSpecies = vsgController.GetComponentAndChildren(1, "SpeciesType", new ComponentOptions() );//.GetComponent("Species");
            XPathNavigator navSpecies = iNavSpecies.CreateNavigator();
            XPathNodeIterator species = navSpecies.Select("/Components/Component/Component[@Type='Species']");
            while (species.MoveNext())
            {
                int speciesId = Int32.Parse(species.Current.GetAttribute("ID", navSpecies.NamespaceURI));
                IXPathNavigable iNavSpeciesParameters = vsgController.GetParametersForComponent(speciesId);
                XPathNavigator navParams = iNavSpeciesParameters.CreateNavigator();
                XPathNavigator param = navParams.SelectSingleNode("/ComponentParameters/Parameter/Parameter[@displayedName='ClassificationDisplayRules']");
                if (param == null)
                    continue;
                String classificationDisplayRule = param.GetAttribute("value", param.NamespaceURI); ///.Value;
                if(classificationDisplayRule.Contains(String.Format("<{0}>{1}",whatWasUpdated, oldVal)))
                {
                //update that val
                    List<VSG.Helpers.ClassificationDisplayRule> cr = VSG.Helpers.ClassificationDisplayRules.FromXML(classificationDisplayRule);
                    foreach (VSG.Helpers.ClassificationDisplayRule rule in cr)
                    {
                        if (whatWasUpdated == "Classification")
                        {
                            if (rule.Classification == oldVal)
                            {
                                rule.Classification = newVal;
                            }
                        }
                        else if (whatWasUpdated == "State")
                        {
                            if (rule.StateName == oldVal)
                            {
                                rule.StateName = newVal;
                            }
                        }
                    }
                    VSG.Helpers.ClassificationDisplayRules rules = new Helpers.ClassificationDisplayRules();
                    rules.Rules = cr;
                    param.SetValue(rules.ToXML());
                    vsgController.UpdateParameters(speciesId, "Species.ClassificationDisplayRules", param.ToString(), eParamParentType.Component);
                }
            }
            
        }

        private void customTreeView2_NodeFunctionList(object sender, NodeFunctionListEventArgs e)
        {
            if (e.Node.Name.Equals("Levels"))
            {
                AME.Nodes.ProcessingNode parentNode = e.Node.Parent as AME.Nodes.ProcessingNode;
                if (parentNode.NodeType.Equals("Emitter"))
                {
                    // get parameters using node id
                    IXPathNavigable iNavParameters = vsgController.GetParametersForComponent(parentNode.NodeID);
                    XPathNavigator navParameters = iNavParameters.CreateNavigator();
                    XPathNavigator navAttribute = navParameters.SelectSingleNode("/ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Attribute_Emitter']");
                    XPathNavigator navCustomAttribute = navParameters.SelectSingleNode("/ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Custom_Attribute_Emitter']");

                    if (navAttribute.GetAttribute("value", navAttribute.NamespaceURI).ToLower() == "true")
                    {//standard attribute emitter
                        navAttribute = navParameters.SelectSingleNode("/ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Attribute']");
                        if (navAttribute != null)
                        {
                            String attributeValue = navAttribute.GetAttribute("value", navAttribute.NamespaceURI);
                            if (attributeValue.Equals("Default") || attributeValue.Equals("Invisible"))
                            {
                                foreach (AME.Nodes.Function function in e.Functions)
                                {
                                    if (function.FunctionName.Equals("Create Level"))
                                    {
                                        function.Enabled = false;
                                    }
                                }
                                MessageBox.Show(string.Format("Emitter \'{0}\' is defined as a Default or Invisible.\nLevels can only be used with Attribute Emitters.", parentNode.Name),
                                     "Using Default or Invisible Emitters", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }
                    }
                    else if (navCustomAttribute.GetAttribute("value", navAttribute.NamespaceURI).ToLower() == "true")
                    { //Custom attribute emitter
                        navAttribute = navParameters.SelectSingleNode("/ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Custom_Attribute']");
                    }
                    //if (navAttribute != null)
                    //{
                    //    String attributeValue = navAttribute.GetAttribute("value", navAttribute.NamespaceURI);
                    //    if (attributeValue.Equals("Default") || attributeValue.Equals("Invisible"))
                    //    {
                    //        foreach (AME.Nodes.Function function in functions)
                    //        {
                    //            if (function.FunctionName.Equals("Create Level"))
                    //            {
                    //                function.Enabled = false;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }


            if (e.Node.Name == "Sensor Ranges")
            {
                AME.Nodes.ProcessingNode parentNode = e.Node.Parent as AME.Nodes.ProcessingNode;
                IXPathNavigable iNavParameters = vsgController.GetParametersForComponent(parentNode.NodeID);
                XPathNavigator navParameters = iNavParameters.CreateNavigator();
                XPathNavigator navAttribute = navParameters.SelectSingleNode("/ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Global_Sensor']");

                if (navAttribute.GetAttribute("value", navAttribute.NamespaceURI).ToLower() == "true")
                {
                    MessageBox.Show(string.Format("Sensor \'{0}\' is defined as a Global Sensor.\nSensor Ranges can only be used with Attribute Sensors.", parentNode.Name),
                        "Using Global Sensors", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (AME.Nodes.Function function in e.Functions)
                    {
                        if (function.FunctionName == "Create Sensor Range")
                        {
                            function.Enabled = false;
                        }
                    }
                }

            }
        }

        void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customTabControl1.SelectedIndex == 0)
            {
                customTreeView1.ImageList = vsgController.CurrentIconLibrary;
                customTreeView1.UpdateViewComponent();
            }
            else if (customTabControl1.SelectedIndex == 1)
            {
                customTreeView2.ImageList = vsgController.CurrentIconLibrary;
                customTreeView2.UpdateViewComponent();
            }
        }

        private void loadViewComponent(Int32 id, String type, String name, Int32 linkId)
        {
            switch (type)
            {
                case "DecisionMaker":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(decisionMakerVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(decisionMakerVC);
                        decisionMakerVC.Dock = DockStyle.Fill;
                    }
                    decisionMakerVC.DecisionMakerId = id;
                    decisionMakerVC.DecisionMakerName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "Team":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(teamVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(teamVC);
                        teamVC.Dock = DockStyle.Fill;
                    }
                    teamVC.TeamId = id;
                    teamVC.TeamName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                case "Classification":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(classificationVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(classificationVC);
                        classificationVC.Dock = DockStyle.Fill;
                    }
                    classificationVC.ClassificationId = id;
                    classificationVC.ClassificationName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                case "Network":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(networkVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(networkVC);
                        networkVC.Dock = DockStyle.Fill;                        
                    }
                    networkVC.NetworkId = id;
                    networkVC.NetworkName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                /*case "OpenVoiceChannelEvent":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(voiceChannelVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(voiceChannelVC);
                        voiceChannelVC.Dock = DockStyle.Fill;
                    }
                    voiceChannelVC.ChannelId = id;
                    voiceChannelVC.VoiceChannelName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "CloseVoiceChannelEvent":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(voiceChannelVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(voiceChannelVC);
                        voiceChannelVC.Dock = DockStyle.Fill;
                    }
                    voiceChannelVC.ChannelId = id;
                    voiceChannelVC.VoiceChannelName = name;
                    customTreeView1.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;*/

                case "Engram":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(engramVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(engramVC);
                        engramVC.Dock = DockStyle.Fill;                        
                    }
                    engramVC.EngramId = id;
                    engramVC.EngramName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                case "Sensor":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(sensorVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(sensorVC);
                        sensorVC.Dock = DockStyle.Fill;                        
                    }
                    sensorVC.SensorId = id;
                    sensorVC.SensorName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "Species":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(speciesVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(speciesVC);
                        speciesVC.Dock = DockStyle.Fill;                        
                    }
                    speciesVC.SpeciesId = id;
                    speciesVC.SpeciesName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;


                case "Emitter":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(emitterVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(emitterVC);
                        emitterVC.Dock = DockStyle.Fill;                        
                    }
                    emitterVC.EmitterId = id;
                    emitterVC.EmitterName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                
                case "Proximity":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(proximityVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(proximityVC);
                        proximityVC.Dock = DockStyle.Fill;
                    }
                    proximityVC.ProximityId = id;
                    proximityVC.ProximityName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "Effect":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(effectVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(effectVC);
                        effectVC.Dock = DockStyle.Fill;                        
                    }
                    effectVC.EffectId = id;
                    effectVC.EffectName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "Singleton":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(singletonVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(singletonVC);
                        singletonVC.Dock = DockStyle.Fill;                        
                    }
                    singletonVC.SingletonId = id;
                    singletonVC.SingletonName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
                case "Transition":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(transitionVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(transitionVC);
                        transitionVC.Dock = DockStyle.Fill;                        
                    }
                    Int32 singletonId = vsgController.GetParentFromLink(linkId);
                    transitionVC.SpeciesId = vsgController.GetSpeciesOfSingleton(singletonId);
                    transitionVC.TransitionId = id;
                    transitionVC.TransitionName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;



                case "Combo":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(comboVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(comboVC);
                        comboVC.Dock = DockStyle.Fill;
                    }
                    //Int32 speciesId = vsgController.GetParentFromLink(linkId);
                    comboVC.SpeciesId = vsgController.GetSpeciesOfCombo(id);
                    comboVC.ComboId = id;
                    comboVC.ComboName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
/*
                case "Contribution":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(contributionVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(contributionVC);
                        contributionVC.Dock = DockStyle.Fill;
                    }
                    Int32 comboId = vsgController.GetParentFromLink(linkId);
                    contributionVC.SpeciesId = vsgController.GetSpeciesOfCombo(comboId);
                    contributionVC.ContributionId = id;
                    contributionVC.ContributionName = name;
                    customTreeView1.DoNewSelectionWithID(id);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;
*/


                case "Contribution":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(contributionVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(contributionVC);
                        contributionVC.Dock = DockStyle.Fill;
                    }
                    contributionVC.ContributionId = id;
                    contributionVC.ContributionName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

      

                case "State":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(stateVC))
                    {                        
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(stateVC);
                        stateVC.Dock = DockStyle.Fill;                        
                    }
                    stateVC.SpeciedId = vsgController.GetParentFromLink(linkId);
                    stateVC.StateId = id;
                    stateVC.StateName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "Level":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(levelVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(levelVC);
                        levelVC.Dock = DockStyle.Fill;
                    }
                    levelVC.EmitterID = vsgController.GetParentFromLink(linkId);
                    levelVC.LevelID = id;
                    levelVC.LevelName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                case "SensorRange":
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    if (!splitContainer1.Panel2.Controls.Contains(sensor_rangeVC))
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(sensor_rangeVC);
                        sensor_rangeVC.Dock = DockStyle.Fill;
                    }
                    sensor_rangeVC.SensorID = vsgController.GetParentFromLink(linkId);
                    sensor_rangeVC.SensorRangeID = id;
                    sensor_rangeVC.SensorRangeName = name;
                    customTreeView2.DoNewSelectionWithID(id, myLinkType);
                    DrawingUtility.ResumeDrawing(splitContainer1.Panel2);
                    break;

                default:
                    DrawingUtility.SuspendDrawing(splitContainer1.Panel2);
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(customTabControlBlank);
                    customTabControlBlank.Dock = DockStyle.Fill;
                    customTabPage2.Description = type;
                    //string help_page = string.Format(@"{0}/InlineHelp/{1}.html", System.Environment.CurrentDirectory, type);
                    string help_page = string.Format(@"{0}/InlineHelp/{1}.html", Application.StartupPath, type);
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

        public void UpdateViewComponent()
        {
            if (customTabControl1.SelectedIndex == 0)
            {
                customTreeView1.ImageList = vsgController.CurrentIconLibrary;
                customTreeView1.UpdateViewComponent();
                if (!_isPlayerTypeInitialized)
                {
                    loadViewComponent(-1, "Teams", string.Empty, -1);
                    _isPlayerTypeInitialized = true;
                }
            }
            else if (customTabControl1.SelectedIndex == 1)
            {
                customTreeView2.ImageList = vsgController.CurrentIconLibrary;
                customTreeView2.UpdateViewComponent();
                if (!_isResourceTypeInitialized)
                {
                    loadViewComponent(-1, "Engrams", string.Empty, -1);
                    _isResourceTypeInitialized = true;
                }

            }
        }

        private void customTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessingNode pNode = e.Node as ProcessingNode;
            Int32 nodeLinkId = pNode.LinkID != null ? (int)pNode.LinkID : -1;
            Int32 nodeId = pNode.NodeID;
            String nodeName = pNode.Name;
            String nodeType;
            if (pNode.NodeID != -1)
                nodeType = pNode.NodeType;
            else
                nodeType = e.Node.Name;

            loadViewComponent(nodeId, nodeType, nodeName, nodeLinkId);
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
                customTreeView1.SetCustomTreeRootId(myLinkType, rootId);
                customTreeView1.SetCustomTreeRootDisplayId(myLinkType, rootId);

                customTreeView2.SetCustomTreeRootId(myLinkType, rootId);
                customTreeView2.SetCustomTreeRootDisplayId(myLinkType, rootId);

                //Int32 index = customTabControl1.SelectedIndex;
                //customTreeView1.UpdateViewComponent();
                //customTreeView2.UpdateViewComponent();
                //customTabControl1.SelectedIndex = index;

                // Remove the creation screen on Id change.
                //splitContainer1.Panel2.Controls.Clear();
            }
        }

        #endregion

        private void customTreeView1_ItemAdd(string addItemString, int itemId, int linkID, string itemType, string itemName, string linkType)
        {
            loadViewComponent(itemId, itemType, itemName, linkID);
        }

        private void customTreeView2_ItemAdd(string addItemString, int nodeID, int linkID, string itemType, string itemName, string linkType)
        {
            loadViewComponent(nodeID, itemType, itemName, linkID);
        }

        private void customTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessingNode pNode = e.Node as ProcessingNode;
            Int32 nodeLinkId = pNode.LinkID != null ? (int)pNode.LinkID : -1;
            Int32 nodeId = pNode.NodeID;
            String nodeName = pNode.Name;
            String nodeType;
            if (pNode.NodeID != -1)
                nodeType = pNode.NodeType;
            else
                nodeType = e.Node.Name;

            loadViewComponent(nodeId, nodeType, nodeName, nodeLinkId);
        }

        private void customTabControl1_Selected(object sender, TabControlEventArgs e)
        {
            int i = 0;
            ProcessingNode pNode = null;
            switch (e.TabPageIndex)
            {
                case 0:
                    pNode = customTreeView1.LastNodeSelected;
                    if (pNode == null)
                    {
                        loadViewComponent(-1, "Teams", string.Empty, -1);
                        return;
                    }
                    break;
                case 1:
                    pNode = customTreeView2.LastNodeSelected;
                    if (pNode == null)
                    {
                        loadViewComponent(-1, "Engrams", string.Empty, -1);
                        _isResourceTypeInitialized = true;
                        return;
                    }
                    break;
            }
            if (pNode != null)
            {
                Int32 nodeLinkId = pNode.LinkID != null ? (int)pNode.LinkID : -1;
                Int32 nodeId = pNode.NodeID;
                String nodeName = pNode.Name;
                String nodeType;
                if (pNode.NodeID != -1)
                    nodeType = pNode.NodeType;
                else
                    nodeType = pNode.Name;

                loadViewComponent(nodeId, nodeType, nodeName, nodeLinkId);
            }
            

        }
    }
}

