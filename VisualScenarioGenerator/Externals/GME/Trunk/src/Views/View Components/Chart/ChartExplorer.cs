using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.IO;
using ChartDirector;
using System.Collections.Generic;
using System.Collections.Specialized;
using AME.Controllers;
using AME.Views.View_Components.Helpers;

namespace AME.Views.View_Components
{
	public enum OutputType 
	{
		MANNING, INFO_ELEMENT
	}

    public class ChartExplorer : UserControl, IViewComponent
	{
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }
		private System.Windows.Forms.StatusBarPanel statusBarPanel;
        private ToolTipTreeView treeView;
		private System.Windows.Forms.ToolBarButton BackPB;
		private System.Windows.Forms.ToolBarButton ForwardPB;
		private System.Windows.Forms.ToolBarButton PreviousPB;
		private System.Windows.Forms.ToolBarButton NextPB;
		private System.Windows.Forms.ToolBarButton ViewSourcePB;
		private System.Windows.Forms.ToolBarButton HelpPB;
        private System.ComponentModel.IContainer components = null;
        private OutputType chartOutputType = OutputType.MANNING;

        private AME.Controllers.IController controller = null;
        private Int32 componentId = -1;
        private Int32 configurationId = -1;
        private String category = "category";
        private String parameter = "filename";
        private XPathNavigator navigator;
        private String fileNameToSave = "";
        private OutputType displayOutputType = OutputType.MANNING;
        private FinanceChartData chartData = null;
        //
		// Array to hold all Windows.ChartViewers in the form to allow processing using loops
		//
		private ChartDirector.WinChartViewer[] chartViewers;
		//
		// Data structure to handle the Back / Forward buttons. We support up to
		// 100 histories. We store histories as the nodes begin selected.
		//
		private TreeNode[] history = new TreeNode[100];
		
		// The array index of the currently selected node in the history array.
        private int currentHistoryIndex = -1;
		private System.Windows.Forms.Panel panel1;
        private ChartDirector.WinChartViewer winChartViewer1;
        private bool hasFinishedInitialization = false;
        private TableLayoutPanel tableLayoutPanel1;
        private WinChartViewer winChartViewer3;
        private WinChartViewer winChartViewer2;
        private SplitContainer splitContainer1;
        
        private Boolean updatingChecks = false;
        private TreeNode first, second, third;
        private int checkCount = 0;
        private ToolTip treeToolTip;
        private TreeNode previousToolTipNode;
        private Dictionary<String, Double> summaryData = new Dictionary<String, Double>();

        private Chart.TablePopUp pop = null;
        private Chart.InfoTablePopUp pop2 = null;
        private HScrollBar hScrollBar1;

        private static DateTime beginDate;
        private DateTime endDate;

        private double dateRange;

        private static double timeIncrementsPerHour;
        private double timeIncrementInSeconds = 1800.0; // a half hour
        private static string timeIncrementInSecondsString;
        private double currentDuration = 0;
        //private double scaleMinutes = 30;

        public XPathNavigator Navigator { set { navigator = value; } }

        public static DateTime BeginDate { get { return beginDate; } } // 

        public static String TimeIncrementInSecondsString { get { return timeIncrementInSecondsString; } }

        public static double TimeIncrementsPerHour { get { return timeIncrementsPerHour; } }

        public virtual OutputType ChartOutputType
        {
            get
            {
                return this.chartOutputType;
            }
            set
            {
                this.chartOutputType = value;
            }
        }

		// The array index of the last valid entry in the history array so that we
		// know if we can use the Forward button.
		private int lastHistoryIndex = -1;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		/// <summary>
		/// ChartExplorer Constructor
		/// </summary>
        public ChartExplorer()
		{
            myHelper = new ViewComponentHelper(this);

            timeIncrementInSecondsString = ""+timeIncrementInSeconds;
            timeIncrementsPerHour = 3600.0 / timeIncrementInSeconds;
            // now in half hours, multiply by 1 below for hours, 2 for half hours, and so on

			//
			// Required for Windows Form Designer support
			//
            ChartDirector.Chart.setLicenseCode("DIST-0000-0536-4cc1-aec1");
			InitializeComponent();
            chartViewers = new WinChartViewer[] {
                winChartViewer1, winChartViewer2, winChartViewer3
            };

            winChartViewer1.ClickHotSpot += new WinHotSpotEventHandler(winChartViewer1_ClickHotSpot);
            winChartViewer2.ClickHotSpot += new WinHotSpotEventHandler(winChartViewer2_ClickHotSpot);
            winChartViewer3.ClickHotSpot += new WinHotSpotEventHandler(winChartViewer3_ClickHotSpot);

            this.treeView.CheckBoxes = true;
            this.treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
            this.treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView_NodeMouseClick);

            treeToolTip = new ToolTip();
            treeToolTip.AutoPopDelay = 10000;
            treeToolTip.InitialDelay = 1200;
            treeToolTip.ReshowDelay = 1200;
            this.treeView.ShowNodeToolTips = false; 
            this.treeView.NodeMouseHover += new TreeNodeMouseHoverEventHandler(treeView_NodeMouseHover);
          
            //if (this.treeView.ImageList == null)
            //{
            //    ImageList tempList = new ImageList();
            //    tempList.ColorDepth = ColorDepth.Depth32Bit;

            //    String green = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\treeGreen.png");
            //    String yellow = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\treeYellow.png");
            //    String white = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\treeWhite.png");
            //    String org = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\organizationalUnit.png");
            //    String billet = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\billet.png");
            //    String role = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\role.png");
            //    String rlList = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\resourceList.png");
            //    String infoProduct = Path.Combine(GMEManager.Instance.ConfigurationPath, "images\\informationProduct.png");

            //    String[] imagePaths = new String[] { white, yellow, green, rlList, org, billet, role, infoProduct };
            //    for(int i = 0; i < imagePaths.Length; i++)
            //    {
            //        FileInfo fileInfo = new FileInfo(imagePaths[i]);
            //        FileStream streamFromFileInfo = fileInfo.Open(FileMode.Open);
            //        Bitmap bitmap = new Bitmap(streamFromFileInfo);
            //        tempList.Images.Add("" + i, new Bitmap(bitmap));
            //        streamFromFileInfo.Close();
            //        bitmap.Dispose();
            //    }

            //    this.treeView.ImageList = tempList;
            //}
        }

        private void winChartViewer1_ClickHotSpot(object sender, WinHotSpotEventArgs e)
        {
            hotSpot(0, winChartViewer1, e);
        }

        private void winChartViewer2_ClickHotSpot(object sender, WinHotSpotEventArgs e)
        {
            hotSpot(1, winChartViewer2, e);
        }

        private void winChartViewer3_ClickHotSpot(object sender, WinHotSpotEventArgs e)
        {
            hotSpot(2, winChartViewer3, e);
        }

        private void hotSpot(int index, WinChartViewer viewer, WinHotSpotEventArgs e)
        {
            String nodeName;

            Hashtable table = e.AttrValues;
            String chartSubType = table["dataSetName"].ToString();
            if (chartSubType.Length == 0)
            {
                bool found = false;
                // clicking is inexact, try and see if we're nearby something that is clickable
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        // check ++, +-, -+, --
                        if (!found)
                        {
                            table = viewer.GetHotSpot(e.X + i, e.Y + j);
                            if (table != null)
                            {
                                chartSubType = table["dataSetName"].ToString();
                                if (chartSubType.Length > 0)
                                {
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            table = viewer.GetHotSpot(e.X + i, e.Y - j);
                            if (table != null)
                            {
                                chartSubType = table["dataSetName"].ToString();
                                if (chartSubType.Length > 0)
                                {
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            table = viewer.GetHotSpot(e.X - i, e.Y + j);
                            if (table != null)
                            {
                                chartSubType = table["dataSetName"].ToString();
                                if (chartSubType.Length > 0)
                                {
                                    found = true;
                                }
                            }
                        }
                    }
                }
                if (!found) //if there is no dataSetName -- there is nothing to show on the popup
                {
                    return;
                }
            }

            String xLabel = table["x"].ToString();

            switch (index)
            {
                case 0:
                    nodeName = first.Text;
                    break;
                case 1:
                    nodeName = second.Text;
                    break;
                case 2:
                    nodeName = third.Text;
                    break;
                default: nodeName = "unknown"; break;
            }
            if (this.ChartOutputType.Equals(OutputType.INFO_ELEMENT))
            {
                if (pop2 != null && pop2.Visible.Equals(true))
                {
                    pop2.Visible = false;
                }
                pop2 = new Chart.InfoTablePopUp(xLabel, nodeName, chartData.GetDataForDetailPopup(this.ChartOutputType, chartSubType, nodeName + "-" + xLabel));
                pop2.Visible = true;
            }
            else
            {
                if (pop != null && pop.Visible.Equals(true))
                {
                    pop.Visible = false;
                }
                pop = new Chart.TablePopUp(xLabel, nodeName, chartData.GetDataForDetailPopup(this.ChartOutputType, chartSubType, nodeName + "-" + xLabel));
                pop.Visible = true;
            }
            //Console.WriteLine("X: " + e.AttrValues["x"] + " Value: " + e.AttrValues["value"] + " Node: " + nodeName);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!updatingChecks)
            {
                //detect node click vs. clicking on the check box
                if (e.Node != null && (e.X >= e.Node.Bounds.Left) && (e.X <= e.Node.Bounds.Right) &&
                        (e.Y >= e.Node.Bounds.Top) && (e.Y <= e.Node.Bounds.Bottom))
                {

                    e.Node.Checked = !e.Node.Checked; // simulate check
                }
            }
        }

        private void treeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn != null)
            {
                if (tn != previousToolTipNode)
                {
                    previousToolTipNode = tn;
                    if (treeToolTip != null && this.treeToolTip.Active)
                    {
                        treeToolTip.Active = false;
                    }
                    treeToolTip.SetToolTip(this.treeView, tn.ToolTipText);
                    treeToolTip.Active = true;
                }
            }
        }

		public void UpdateViewComponent()
		{
            try
            {
                if (controller != null && componentId >= 0)
                {
                    checkCount = 0;
                    first = null;
                    second = null;
                    third = null;
                    loadData();
                    treeView.Nodes.Clear();
                    for (int i = 0; i < chartViewers.Length; ++i)
                    {
                        chartViewers[i].Visible = false;
                    }

                    if (this.ChartOutputType.Equals(OutputType.MANNING))
                    {
                        BuildManningOutput();
                    }
                    else
                    {
                        BuildInformationOutput();
                    }

                    // mutiply by timeIncrementsPerHour to get the total number of data points
                    dateRange = endDate.Subtract(beginDate).TotalHours * timeIncrementsPerHour;
    
                    winChartViewer1.ViewPortWidth = currentDuration / dateRange;
                    //winChartViewer1.ViewPortHeight = 10 / rowRange;
                    winChartViewer2.ViewPortWidth = currentDuration / dateRange;
                    //winChartViewer2.ViewPortHeight = 10 / rowRange;
                    winChartViewer3.ViewPortWidth = currentDuration / dateRange;
                    //winChartViewer3.ViewPortHeight = 10 / rowRange;
                    // Can update chart now
                    hasFinishedInitialization = true;
                    winChartViewer1.updateViewPort(true, true);
                    winChartViewer2.updateViewPort(true, true);
                    winChartViewer3.updateViewPort(true, true);
                }
                else
                {
                    treeView.Nodes.Clear();
                    for (int i = 0; i < chartViewers.Length; ++i)
                    {
                        chartViewers[i].Visible = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not initialize assessment - check scenario import and billets?");
            }
            finally
            {
                hasFinishedInitialization = true;
            }
		}

        private void loadData()
        {
            summaryData.Clear();

            getNavigator();

            if (navigator != null)
            {
                XPathNavigator navOutput = navigator.SelectSingleNode("Output");
                String duration = navOutput.GetAttribute("duration", navOutput.NamespaceURI);
                if (String.IsNullOrEmpty(duration))
                    duration = "86400";
                beginDate = DateTime.Parse(navOutput.GetAttribute("start", navOutput.NamespaceURI));
                DateTime missionTime = beginDate;
                TimeSpan dTimeSpan = TimeSpan.FromSeconds(Double.Parse(duration));
                endDate = beginDate.Add(dTimeSpan);

                XPathNodeIterator itDataSet = navOutput.Select("/Output/DataSet");
                chartData = new FinanceChartData();
                double isThereDetailDataToDisplay = 0.0;
                while (itDataSet.MoveNext())
                {
                    XPathNodeIterator itDataSetObservations = itDataSet.Current.Select("/Output/DataSet/Observation");
                    while (itDataSetObservations.MoveNext())
                    {

                        string obsElement = Convert.ToString(itDataSetObservations.Current.SelectSingleNode("ObsElement").Value);
                        string obsElementType = itDataSetObservations.Current.SelectSingleNode("ObsElement").GetAttribute("elementType", itDataSetObservations.Current.NamespaceURI);
                        double obsValue = Convert.ToDouble(itDataSetObservations.Current.SelectSingleNode("ObsValue").Value);
                        string obsValueType = itDataSetObservations.Current.SelectSingleNode("ObsValue").GetAttribute("valueType", itDataSetObservations.Current.NamespaceURI);

                        if (obsValueType.Equals("Summary"))
                        {
                            if (!summaryData.ContainsKey(obsElement))
                            {
                                summaryData.Add(obsElement, obsValue);
                            }
                        }
                        else
                        {
                            double timePeriod = Convert.ToDouble(itDataSetObservations.Current.SelectSingleNode("TimePeriod").Value);
                            timePeriod = (timePeriod / timeIncrementInSeconds);

                            int timePeriodInt = (int)timePeriod;
                            double difference = timePeriod - (double)timePeriodInt;
                            if (difference != 0.0)
                            {
                                // skip any values that do not evently divide into the time increment
                                // in this case anything is not on the hour or half hour
                                continue;
                            }

                            //reset this flag everytime you read a new record
                            isThereDetailDataToDisplay = 0.0;
                            XPathNodeIterator itDataSetObsUpdatedBy = itDataSetObservations.Current.Select("UpdatedBy");
                            while (itDataSetObsUpdatedBy.MoveNext())
                            {
                                string name = Convert.ToString(itDataSetObsUpdatedBy.Current.SelectSingleNode("Name").Value);
                                string id = Convert.ToString(itDataSetObsUpdatedBy.Current.SelectSingleNode("ID").Value);
                                double seconds = Convert.ToDouble(itDataSetObsUpdatedBy.Current.SelectSingleNode("SecondsSinceUpdate").Value);
                                TimeSpan span = TimeSpan.FromSeconds(seconds);
                                DateTime time = missionTime.Add(span);
                                chartData.AddUpdatedChartData(timePeriod, obsElement, obsElementType, name, id, seconds);
                                isThereDetailDataToDisplay = 2.0;
                            }
                            XPathNodeIterator itDataSetObsDemandedBy = itDataSetObservations.Current.Select("DemandedBy");
                            while (itDataSetObsDemandedBy.MoveNext())
                            {
                                string name = Convert.ToString(itDataSetObsDemandedBy.Current.SelectSingleNode("Name").Value);
                                string id = Convert.ToString(itDataSetObsDemandedBy.Current.SelectSingleNode("ID").Value);
                                //this value is not currently being used (using value from parent element)
                                double completeness = Convert.ToDouble(itDataSetObsDemandedBy.Current.SelectSingleNode("Completeness").Value);

                                chartData.AddDemandedChartData(timePeriod, obsElement, obsElementType, name, id, obsValue);
                                isThereDetailDataToDisplay = 2.0;
                            }
                            XPathNodeIterator itDataSetObsWorkingOn = itDataSetObservations.Current.Select("WorkingOn");
                            while (itDataSetObsWorkingOn.MoveNext())
                            {
                                string name = Convert.ToString(itDataSetObsWorkingOn.Current.SelectSingleNode("Name").Value);
                                string id = Convert.ToString(itDataSetObsWorkingOn.Current.SelectSingleNode("ID").Value);

                                chartData.AddWorkingOnChartData(timePeriod, obsElement, obsElementType, name, id);
                                isThereDetailDataToDisplay = 2.0;
                            }
                            chartData.BuildChartData(timePeriod, obsElement, obsElementType, obsValue, obsValueType, isThereDetailDataToDisplay);
                        }
                    }
                }
                int durationParse = Int32.Parse(duration) / (int)timeIncrementInSeconds;
                // fix InfoProduct data - add zeroes where there are gaps

                foreach (string key in chartData.GetElementKeysForObsElementHashtable("InfoElement"))
                {
                    FinanceChartData.ChartDataClass infoData = chartData.GetDataForNVCChart("InfoElement", key);
                    NameValueCollection timeDataCopy = new NameValueCollection();
                    foreach (String tdKey in infoData.TimeData.AllKeys)
                    {
                        timeDataCopy.Add(tdKey, infoData.TimeData[tdKey]);
                    }

                    infoData.HighData = new List<double>();
                    infoData.TimeData = new NameValueCollection();
                    infoData.TimeStamps = new List<double>();
                    infoData.RealDataAtThisPoint = new List<bool>();

                    for (int i = 0; i < durationParse; i++)
                    {
                        infoData.TimeStamps.Add(i);
                        String data = timeDataCopy["" + i];
                        if (data != null)
                        {
                            Double d_data = Double.Parse(data);

                            infoData.HighData.Add(d_data);
                            infoData.TimeData.Add("" + i, data);
                            infoData.RealDataAtThisPoint.Add(true);
                        }
                        else
                        {
                            infoData.HighData.Add(0.0);
                            infoData.TimeData.Add("" + i, "0.0");
                            infoData.RealDataAtThisPoint.Add(false);
                        }
                    }
                }
                chartData.Normalize(durationParse);

                currentDuration = 24;// endDate.Subtract(beginDate).TotalHours;
            }
        }

		private void BuildManningOutput()
		{
			//
			// Array to hold all Windows.ChartViewers in the form to allow processing 
			// using loops
			//
			//
			// Build the tree view on the left to represent available demo modules
			//
			TreeNode manningNode = new TreeNode("Manning");
            manningNode.ImageKey = "0"; // hardcorded!
            manningNode.SelectedImageKey = "0";
            treeView.Nodes.Add(manningNode);

            TreeNode orgElement = new TreeNode("Organization");
            orgElement.ImageKey = "1"; // hardcorded!
            orgElement.SelectedImageKey = "1";

            List<String> keys = new List<String>();
            foreach (string key in chartData.GetElementKeysForObsElementHashtable("OrgElement"))
            {
                keys.Add(key);
            }
            keys.Sort();

            foreach (string key in keys)
            {
                orgElement.Nodes.Add(MakeNode(new Chart.FinanceChart(chartData.GetDataForNVCChart("OrgElement", key), key, OutputType.MANNING, "OrgElement", "% Employment"), "Organization"));
            }
            manningNode.Nodes.Add(orgElement);

            orgElement = new TreeNode("Role");
            orgElement.ImageKey = "2"; // hardcorded!
            orgElement.SelectedImageKey = "2";

            keys.Clear();
            foreach (string key in chartData.GetElementKeysForObsElementHashtable("Role"))
            {
                keys.Add(key);
            }
            keys.Sort();

            foreach (string key in keys)
            {
                orgElement.Nodes.Add(MakeNode(new Chart.FinanceChart(chartData.GetDataForNVCChart("Role", key), key, OutputType.MANNING, "Role", "% Employment"), "Role"));
            }
            manningNode.Nodes.Add(orgElement);

            orgElement = new TreeNode("Billet");
            orgElement.ImageKey = "3"; // hardcorded!
            orgElement.SelectedImageKey = "3";

            keys.Clear();
            foreach (string key in chartData.GetElementKeysForObsElementHashtable("Billet"))
            {
                keys.Add(key);
            }
            keys.Sort();

            foreach (string key in keys)
            {
                orgElement.Nodes.Add(MakeNode(new Chart.FinanceChart(chartData.GetDataForNVCChart("Billet", key), key, OutputType.MANNING, "Billet", "Workload"), "Billet"));
            }
            manningNode.Nodes.Add(orgElement);
			treeView.SelectedNode = getNextChartNode(treeView.Nodes[0]);
		}

		private void BuildInformationOutput()
		{
			//
			// Build the tree view on the left to represent available demo modules
			//
			TreeNode analysisNode = new TreeNode("Information");
            analysisNode.ImageKey = "0"; // hardcorded!
            analysisNode.SelectedImageKey = "0";
			//treeView.Nodes.Add(analysisNode);

            TreeNode infoElement = new TreeNode("Information Product");
            infoElement.ImageKey = "4"; // hardcorded!
            infoElement.SelectedImageKey = "4";

            List<String> keys = new List<String>();
            foreach (string key in chartData.GetElementKeysForObsElementHashtable("InfoElement"))
            {
                keys.Add(key);
            }
            keys.Sort();

            foreach (string key in keys)
            {
                infoElement.Nodes.Add(MakeNode(new Chart.FinanceChart(chartData.GetDataForNVCChart("InfoElement", key), key, OutputType.INFO_ELEMENT, "InfoElement", "Completeness"), "Information Product"));
            }
            treeView.Nodes.Add(infoElement);
            //analysisNode.Nodes.Add(infoElement);			
			treeView.SelectedNode = getNextChartNode(treeView.Nodes[0]);
		}

		/// <summary>
		/// Help method to add a chart module to the tree
		/// </summary>
        private TreeNode MakeNode(Chart.IChartExplorer module, String parentNodeName)
        {
            ThresholdData data = null;

            bool found = false;
            for (int i = 0; i < thresholdData.AllSets.Length; i++)
            {
                ThresholdData check = thresholdData.AllSets[i];
                if (check.Name.Equals(parentNodeName))
                {
                    data = check;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception("Could not find threshold data: " + parentNodeName);
            }

            double summaryValue = -1;
            String key = "0";
            String name = module.getName();

            if (summaryData.ContainsKey(name))
            {
                summaryValue = summaryData[name];
            }
            else if (name.EndsWith(")")) // hack to filter out parentheses, ugh
            {
                name = name.Substring(0, name.Length - 4);
                if (summaryData.ContainsKey(name))
                {
                    summaryValue = summaryData[name];
                }
                else
                {
                    name = name.Substring(0, name.Length - 1);
                    if (summaryData.ContainsKey(name))
                    {
                        summaryValue = summaryData[name];
                    }
                }
            }

            if (summaryValue != -1) // compare summary values to thresholds
            {
                String lowerKey = parentNodeName + "lc";
                String upperKey = parentNodeName + "uc";
                if (data.LowerCheck) // lower 
                {
                    if (summaryValue <= data.LowerValue)
                    {
                        key = lowerKey;
                    }
                }
                else
                {
                    if (summaryValue < data.LowerValue)
                    {
                        key = lowerKey;
                    }
                }

                if (data.UpperCheck) // upper
                {
                    if (summaryValue >= data.UpperValue)
                    {
                        key = upperKey;
                    }
                }
                else
                {
                    if (summaryValue > data.UpperValue)
                    {
                        key = upperKey;
                    }
                }

                if (key == "0") // not upper or lower, must be middle?
                {
                    key = parentNodeName + "mc";
                }

                TreeNode node = new TreeNode(module.getName());
                node.ImageKey = "" + key;
                node.SelectedImageKey = "" + key;
                node.Tag = module;	// The demo module is attached to the node as the Tag property.
                // round to two decimals
                summaryValue = Math.Round(summaryValue, 2);
                node.ToolTipText = summaryValue.ToString();
                return node;
            }
            else
            {
                Console.WriteLine("(Summary)Had to use default node: " + module.getName() + ", " + summaryData);
                TreeNode node = new TreeNode(module.getName());
                node.ImageKey = "" + 0;
                node.SelectedImageKey = "" + 0;
                node.Tag = module;	// The demo module is attached to the node as the Tag property.
                return node;
            }
        }
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			//
			// Standard code generated by Visual Studio.NET
			//
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.BackPB = new System.Windows.Forms.ToolBarButton();
            this.ForwardPB = new System.Windows.Forms.ToolBarButton();
            this.PreviousPB = new System.Windows.Forms.ToolBarButton();
            this.NextPB = new System.Windows.Forms.ToolBarButton();
            this.ViewSourcePB = new System.Windows.Forms.ToolBarButton();
            this.HelpPB = new System.Windows.Forms.ToolBarButton();
            this.treeView = new AME.Views.View_Components.ToolTipTreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.winChartViewer3 = new ChartDirector.WinChartViewer();
            this.winChartViewer2 = new ChartDirector.WinChartViewer();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel.Name = "statusBarPanel";
            this.statusBarPanel.Text = " Please select chart to view";
            this.statusBarPanel.Width = 816;
            // 
            // BackPB
            // 
            this.BackPB.Enabled = false;
            this.BackPB.ImageIndex = 0;
            this.BackPB.Name = "BackPB";
            this.BackPB.Text = "Back";
            // 
            // ForwardPB
            // 
            this.ForwardPB.Enabled = false;
            this.ForwardPB.ImageIndex = 1;
            this.ForwardPB.Name = "ForwardPB";
            this.ForwardPB.Text = "Forward";
            // 
            // PreviousPB
            // 
            this.PreviousPB.ImageIndex = 2;
            this.PreviousPB.Name = "PreviousPB";
            this.PreviousPB.Text = "Previous";
            // 
            // NextPB
            // 
            this.NextPB.ImageIndex = 3;
            this.NextPB.Name = "NextPB";
            this.NextPB.Text = "Next";
            // 
            // ViewSourcePB
            // 
            this.ViewSourcePB.ImageIndex = 4;
            this.ViewSourcePB.Name = "ViewSourcePB";
            this.ViewSourcePB.Text = "View Code";
            // 
            // HelpPB
            // 
            this.HelpPB.ImageIndex = 5;
            this.HelpPB.Name = "HelpPB";
            this.HelpPB.Text = "View Doc";
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HotTracking = true;
            this.treeView.ItemHeight = 16;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(197, 504);
            this.treeView.TabIndex = 5;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(631, 504);
            this.panel1.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.winChartViewer3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.winChartViewer2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.winChartViewer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.hScrollBar1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(627, 500);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // winChartViewer3
            // 
            this.winChartViewer3.Location = new System.Drawing.Point(4, 326);
            this.winChartViewer3.Dock = DockStyle.Fill;
            this.winChartViewer3.Margin = new System.Windows.Forms.Padding(4);
            this.winChartViewer3.Name = "winChartViewer3";
            this.winChartViewer3.ScrollDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer3.Size = new System.Drawing.Size(618, 153);
            this.winChartViewer3.TabIndex = 8;
            this.winChartViewer3.TabStop = false;
            this.winChartViewer3.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer3_ViewPortChanged);
            this.winChartViewer3.Resize += new System.EventHandler(this.winChartViewer3_Resize);
            // 
            // winChartViewer2
            // 
            this.winChartViewer2.Location = new System.Drawing.Point(4, 165);
            this.winChartViewer2.Dock = DockStyle.Fill;
            this.winChartViewer2.Margin = new System.Windows.Forms.Padding(4);
            this.winChartViewer2.Name = "winChartViewer2";
            this.winChartViewer2.ScrollDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer2.Size = new System.Drawing.Size(618, 151);
            this.winChartViewer2.TabIndex = 7;
            this.winChartViewer2.TabStop = false;
            this.winChartViewer2.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer2_ViewPortChanged);
            this.winChartViewer2.Resize += new System.EventHandler(this.winChartViewer2_Resize);
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Location = new System.Drawing.Point(4, 4);
            this.winChartViewer1.Dock = DockStyle.Fill;
            this.winChartViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.ScrollDirection = ChartDirector.WinChartDirection.HorizontalVertical;
            this.winChartViewer1.Size = new System.Drawing.Size(618, 152);
            this.winChartViewer1.TabIndex = 6;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged_1);
            this.winChartViewer1.Resize += new System.EventHandler(this.winChartViewer1_Resize);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 483);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(627, 17);
            this.hScrollBar1.TabIndex = 9;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(832, 504);
            this.splitContainer1.SplitterDistance = 197;
            this.splitContainer1.TabIndex = 8;
            // 
            // ChartExplorer
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChartExplorer";
            this.Size = new System.Drawing.Size(832, 504);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Handler for the TreeView BeforeExpand event
		/// </summary>
		private void treeView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// Change the node to use the open folder icon when the node expands
			//e.Node.SelectedImageIndex = e.Node.ImageIndex = 1;
		}

		/// <summary>
		/// Handler for the TreeView BeforeCollapse event
		/// </summary>
		private void treeView_BeforeCollapse(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// Change the node to use the clode folder icon when the node collapse
			//e.Node.SelectedImageIndex = e.Node.ImageIndex = 0;
		}

        /// <summary>
        /// Handler for the TreeView AfterCheck event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!updatingChecks)
            {
                this.updatingChecks = true;

                if (e.Node.Nodes.Count > 0) // only check leafs
                {
                    e.Node.Checked = false;
                    this.updatingChecks = false;
                    return;
                }

                if (e.Node.Checked)
                {
                    switch (checkCount)
                    {
                        case 0:
                            if (first != null)
                            {
                                first.Checked = false;
                            }
                            first = e.Node;
                            checkCount++;
                            break;
                        case 1:
                            if (second != null)
                            {
                                second.Checked = false;
                            }
                            second = e.Node;
                            checkCount++;
                            break;
                        case 2:
                            if (third != null)
                            {
                                third.Checked = false;
                            }
                            third = e.Node;
                            checkCount = 0;
                            break;
                    }
                }
                else
                {
                    if (e.Node.Equals(first))
                    {
                        first = null;
                        first = second;
                        second = third;
                        third = null;
                    }
                    else if (e.Node.Equals(second))
                    {
                        second = null;
                        second = third;
                        third = null;
                    }
                    else if (e.Node.Equals(third))
                    {
                        third = null;
                    }

                    if (third == null)
                    {
                        checkCount = 2;
                    }
                    if (second == null)
                    {
                        checkCount = 1;
                    }
                    if (first == null)
                    {
                        checkCount = 0;
                    }
                }
                this.updatingChecks = false;

                // Clear all ChartViewers
                for (int i = 0; i < chartViewers.Length; ++i)
                {
                    chartViewers[i].Visible = false;
                }

                int index = 0;

                newCheckLogic(first, index);
                if (first != null)
                {
                    index++;
                }

                newCheckLogic(second, index);
                if (second != null)
                {
                    index++;
                }

                newCheckLogic(third, index);
            }
        }
        //Dictionary<WinChartViewer, Chart.IChartExplorer> charts = new Dictionary<WinChartViewer, AME.Views.View_Components.Chart.IChartExplorer>();
        /// <summary>
        /// Do this logic when we check a node, based on the logic that used to be in selection
        /// </summary>
        private void newCheckLogic(TreeNode n, Int32 index)
        {
            chartViewers[index].Image = null;
            if (n == null)
            {
                return;
            }
            // Check if a demo module node is selected. Demo module is attached to the node
            // as the Tag property
            Chart.IChartExplorer chart = (Chart.IChartExplorer)n.Tag;
            if (chart != null)
            {
                //charts[chartViewers[index]] = chart;
                chart.createChart(chartViewers[index], beginDate, dateRange);
                chartViewers[index].Visible = true;
                chartViewers[index].Tag = chart;
            }

            // Update the state of the buttons, status bar, etc.
            updateControls();
        }

		/// <summary>
		/// Handler for the TreeView AfterSelect event
		/// </summary>
		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
            //// Check if a demo module node is selected. Demo module is attached to the node
            //// as the Tag property
            //bool didNotdisplayChart = true;
            //Chart.IChartExplorer chart = (Chart.IChartExplorer)e.Node.Tag;
            //if (chart != null)
            //{
            //    // Clear all ChartViewers
            //    //for (int i = 0; i < chartViewers.Length; ++i)
            //    //	chartViewers[i].Visible = false;
            //    //winChartViewer1.Visible = false;
                
            //    // Each demo module can display a number of charts
            //    //int noOfCharts = chart.getNoOfCharts();
            //    for (int i = 0; i < chartViewers.Length; ++i)
            //    //{
            //    {
            //        if (chartViewers[i].Visible.Equals(false))
            //        {
            //            chartViewers[i].Image = null;
            //            chart.createChart(chartViewers[i]);
            //            chartViewers[i].Visible = true;
            //            didNotdisplayChart = false;
            //            break;
            //        }
            //    }
            //    if (didNotdisplayChart)
            //    {
            //        chartViewers[firstChartToReplace].Image = null;
            //        chart.createChart(chartViewers[firstChartToReplace]);
            //        chartViewers[firstChartToReplace].Visible = true;
            //        if (firstChartToReplace == (chartViewers.Length-1))
            //        {
            //            firstChartToReplace = 0;
            //        }
            //        else
            //        {
            //            firstChartToReplace = firstChartToReplace + 1;
            //        }
            //    }
            //        //chartViewers[i].Visible = true;
            //    //}
						
            //    // Now perform flow layout of the charts (viewers) 
            //    //layoutCharts();

            //    // Add current node to the history array to support Back/Forward browsing
            //    addHistory(e.Node);
            //}

            //// Update the state of the buttons, status bar, etc.
            //updateControls();
		}

		/// <summary>
		/// Helper method to perform a flow layout (left to right, top to bottom) of
		/// the chart.
		/// </summary>
		private void layoutCharts()
		{
			// Margin between the charts
			int margin = 5;

			// The first chart is always at the position as seen on the visual designer
            ChartDirector.WinChartViewer viewer = winChartViewer1;   //chartViewers[0];
			//viewer.Top = line.Bottom + margin;

			// The next chart is at the left of the first chart
			int currentX = viewer.Left + viewer.Width + margin;
			int currentY = viewer.Top;

			// The line height of a line of charts is that of the tallest chart in the line
			int lineHeight = viewer.Height;
			
			// Now layout subsequent charts (other than the first chart)
			//for (int i = 1; i < chartViewers.Length; ++i)
			//{
                viewer = winChartViewer1;
				
				// Layout only visible charts
				//if (!viewer.Visible)
				//	continue;

				// Check for enough space on the left before it hits the panel border
				if (currentX + viewer.Width > panel1.Width)
				{
					// Not enough space, so move to the next line

					// Start of line is to align with the left of the first chart
                    currentX = winChartViewer1.Left;

					// Adjust vertical by lineHeight plus a margin
					currentY += lineHeight + margin;

					// Reset the lineHeight
					lineHeight = 0;
				}
				
				// Put the chart at the current position
				viewer.Left = currentX;
				viewer.Top = currentY;

				// Advance the current position to the left prepare for the next chart
				currentX += viewer.Width + margin;

				// Update the lineHeight to reflect the tallest chart so far encountered
				// in the same line
				if (lineHeight < viewer.Height)
					lineHeight = viewer.Height;
			//}
		}

		/// <summary>
		/// Add a selected node to the history array
		/// </summary>
		private void addHistory(TreeNode node)
		{
			// Don't add if selected node is current node to avoid duplication.
			if ((currentHistoryIndex >= 0) && (node == history[currentHistoryIndex]))
				return;

			// Check if the history array is full
			if (currentHistoryIndex + 1 >= history.Length)
			{
				// History array is full. Remove oldest 25% from the history array.
				// We add 1 to make sure at least 1 item is removed.
				int itemsToDiscard = history.Length / 4 + 1;

				// Remove the oldest items by shifting the array. 
				for (int i = itemsToDiscard; i < history.Length; ++i)
					history[i - itemsToDiscard] = history[i];
				
				// Adjust index because array is shifted.
				currentHistoryIndex = history.Length - itemsToDiscard;
			}
			
			// Add node to history array
			history[++currentHistoryIndex] = node;

			// After adding a new node, the forward button is always disabled. (This
			// is consistent with normal browser behaviour.) That means the last 
			// history node is always assumed to be the current node. 
			lastHistoryIndex = currentHistoryIndex;
		}

		/// <summary>
		/// Handler for the ToolBar ButtonClick event
		/// </summary>
		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			//
			// Execute handler depending on which button is pressed
			//
			if (e.Button == BackPB)
				backHistory();
			else if (e.Button == ForwardPB)
				forwardHistory();
			else if (e.Button == NextPB)
				nextNode();
			else if (e.Button == PreviousPB)
				prevNode();
		}
		
		/// <summary>
		/// Handler for the Back button
		/// </summary>
		private void backHistory()
		{
			// Select the previous node in the history array
			if (currentHistoryIndex > 0)
				treeView.SelectedNode = history[--currentHistoryIndex];
		}

		/// <summary>
		/// Handler for the Forward button
		/// </summary>
		private void forwardHistory()
		{
			// Select the next node in the history array
			if (lastHistoryIndex > currentHistoryIndex)
				treeView.SelectedNode = history[++currentHistoryIndex];
		}

		/// <summary>
		/// Handler for the Next button
		/// </summary>
		private void nextNode()
		{
			// Getnext chart node of the current selected node by going down the tree
			TreeNode node = getNextChartNode(treeView.SelectedNode);
			
			// Display the node if available
			if (node != null)
				treeView.SelectedNode = node;
		}
		
		/// <summary>
		/// Helper method to go to the next chart node down the tree
		/// </summary>
		private TreeNode getNextChartNode(TreeNode node)
		{
			// Get the next node of by going down the tree
			node = getNextNode(node);
			
			// Skip nodes that are not associated with charts (e.g the folder nodes)
			while ((node != null) && (node.Tag == null))
				node = getNextNode(node);

			return node;
		}
			
		/// <summary>
		/// Helper method to go to the next node down the tree
		/// </summary>
		private TreeNode getNextNode(TreeNode node)
		{
			if (node == null)
				return null;
			
			// If the current node is a folder, the next node is the first child.
			if (node.FirstNode != null)
				return node.FirstNode;
			
			while (node != null)
			{
				// If there is a next sibling node, it is the next node.
				if (node.NextNode != null)
					return node.NextNode;

				// If there is no sibling node, the next node is the next sibling 
				// of the parent node. So we go back to the parent and loop again.
				node = node.Parent;
			}

			// No next node - must be already the last node.
            return null;
		}

		/// <summary>
		/// Handler for the Previous button
		/// </summary>
		private void prevNode()
		{
			// Get previous chart node of the current selected node by going up the tree
			TreeNode node = getPrevChartNode(treeView.SelectedNode);

			// Display the node if available
			if (node != null)
				treeView.SelectedNode = node;
		}
		
		/// <summary>
		/// Helper method to go to the previous chart node down the tree
		/// </summary>
		private TreeNode getPrevChartNode(TreeNode node)
		{
			// Get the prev node of by going up the tree
			node = getPrevNode(node);
			
			// Skip nodes that are not associated with charts (e.g the folder nodes)
			while ((node != null) && (node.Tag == null))
				node = getPrevNode(node);

			return node;
		}
		
		/// <summary>
		/// Helper method to go to the previous node up the tree
		/// </summary>
		private TreeNode getPrevNode(TreeNode node)
		{
			if (node == null)
				return null;
			
			// If there is no previous sibling node, the previous node must be its
			// parent. 
			if (node.PrevNode == null)
				return node.Parent;
		
			// If there is a previous sibling node, the previous node is the last
			// child of the previous sibling (if it has any child at all).
			node = node.PrevNode;
			while (node.LastNode != null)
				node = node.LastNode;

			return node;
		}


		/// <summary>
		/// Helper method to update the various controls
		/// </summary>
		private void updateControls()
		{
			//
			// Enable the various buttons there is really something they can do.
			//
			BackPB.Enabled = (currentHistoryIndex > 0);
			ForwardPB.Enabled = (lastHistoryIndex > currentHistoryIndex);
			NextPB.Enabled = (getNextChartNode(treeView.SelectedNode) != null);
			PreviousPB.Enabled = (getPrevChartNode(treeView.SelectedNode) != null);

			// The status bar always reflects the selected demo module
			if ((null != treeView.SelectedNode) && (null != treeView.SelectedNode.Tag))
			{
                Chart.IChartExplorer m = (Chart.IChartExplorer)treeView.SelectedNode.Tag;
				statusBarPanel.Text = " Module " + m.GetType().Name + ": " + m.getName();
			}
			else
				statusBarPanel.Text = "";
		}

		/// <summary>
		/// Handler for the panel layout event
		/// </summary>
		private void rightPanel_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			// Perform flow layout of the charts 
			//if (chartViewers != null)
				layoutCharts();
		}

		/// <summary>
		/// Handler for the ClickHotSpot event, which occurs when the mouse clicks on 
		/// a hot spot on the chart
		/// </summary>
		/// 
		
		private void chartViewer_ClickHotSpot(object sender, ChartDirector.WinHotSpotEventArgs e)
		{
			// In this demo, just list out the information provided by ChartDirector about hot spot
			//new ParamViewer().Display(sender, e);
		}

        private void getNavigator()
        {
            if (navigator == null)
            {
                IXPathNavigable iNavParameters = controller.GetParametersForComponent(componentId);
                XPathNavigator navParameters = iNavParameters.CreateNavigator();

                String xpath = String.Format("ComponentParameters/Parameter/Parameter[@category='{0}' and @displayedName='{1}']", category, parameter);
                XPathNavigator navFilename = navParameters.SelectSingleNode(xpath);

                if (navFilename != null)
                {
                    String filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);
                    //JA REMOVE
                    //String filename = "JA2_sub_Visualization.Filename_2009-04-06T16-32-57";
                    IXPathNavigable iNavOutput = null;
                    try
                    {
                        iNavOutput = controller.GetOutputXml(filename);
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, String.Format("Failed to get output xml '{0}'", filename));
                        iNavOutput = null;
                    }
                    if (iNavOutput != null)
                    {
                        XmlDocument document = (XmlDocument)iNavOutput;
                        navigator = validate(document);
                        fileNameToSave = filename;
                    }
                    else
                    {
                        navigator = null;
                    }
                }
                else
                {
                    navigator = null;
                }
            }
        }
        private void winChartViewer1_ViewPortChanged_1(object sender, WinViewPortEventArgs e)
        {
            // Synchronize the horizontal scroll bar with the WinChartViewer
            hScrollBar1.Enabled = winChartViewer1.ViewPortWidth < 1;
            hScrollBar1.LargeChange = (int)Math.Ceiling(winChartViewer1.ViewPortWidth *
                (hScrollBar1.Maximum - hScrollBar1.Minimum));
            hScrollBar1.SmallChange = (int)Math.Ceiling(hScrollBar1.LargeChange * 0.1);
            hScrollBar1.Value = (int)Math.Round(winChartViewer1.ViewPortLeft *
                (hScrollBar1.Maximum - hScrollBar1.Minimum)) + hScrollBar1.Minimum;

            // Update chart and image map if necessary
            if (e.NeedUpdateChart)
                drawChart(winChartViewer1);
        }

        private void winChartViewer1_Resize(object sender, EventArgs e)
        {
            if (controller != null && navigator != null && componentId > -1)
                winChartViewer1.updateViewPort(true, false);
        }

        private void winChartViewer2_Resize(object sender, EventArgs e)
        {
            if (controller != null && navigator != null && componentId > -1)
                winChartViewer2.updateViewPort(true, false);
        }

        private void winChartViewer3_Resize(object sender, EventArgs e)
        {
            if (controller != null && navigator != null && componentId > -1)
                winChartViewer3.updateViewPort(true, false);
        }

        private XPathNavigator validate(XmlDocument document)
        {
            XPathNavigator navValidatedXml = null;
            XmlReader schemaDatabase = this.controller.GetXSD("VisualizationOutput.xsd");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaDatabase);

            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

            //Create reader with settings
            StringReader strReader = new StringReader(document.OuterXml);
            XmlReader xmlReader = XmlReader.Create(strReader, settings);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlReader);
                navValidatedXml = doc.CreateNavigator();
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Validation Error");
            }
            finally
            {
                schemaDatabase.Close();
                strReader.Close();
                xmlReader.Close();
            }
            return navValidatedXml;
        }

        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }
            set
            {
                componentId = value;
            }
        }
        public String Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
            }
        }
        public String Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
            }
        }
        public OutputType DisplayOutputType
        {
            get
            {
                return displayOutputType;
            }
            set
            {
                displayOutputType = value;
            }
        }

        private ThresholdDataSet thresholdData;
        public ThresholdDataSet ThresholdData
        {
            set
            {
                thresholdData = value;

                for (int i = 0; i < thresholdData.AllSets.Length; i++)
                {
                    ThresholdData data = thresholdData.AllSets[i];
                    String name = data.Name;

                    treeView.ImageList.Images.RemoveByKey(name + "lc");
                    treeView.ImageList.Images.RemoveByKey(name + "mc");
                    treeView.ImageList.Images.RemoveByKey(name + "uc");

                    treeView.ImageList.Images.Add(name + "lc", ColorHelper.IconFromColor(data.LowerColor));
                    treeView.ImageList.Images.Add(name + "mc", ColorHelper.IconFromColor(data.MiddleColor));
                    treeView.ImageList.Images.Add(name + "uc", ColorHelper.IconFromColor(data.UpperColor));
                }
            }
        }

        #region IViewComponent Members
        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                if (value != null)
                {
                    controller = value;

                    if (this.treeView.ImageList == null)
                    {
                        ImageList tempList = new ImageList();
                        tempList.ColorDepth = ColorDepth.Depth32Bit;

                        Stream rlList = this.controller.GetImage("resourceList.png");
                        Stream org = this.controller.GetImage("organizationalUnit.png");
                        Stream role = this.controller.GetImage("role.png");
                        Stream billet = this.controller.GetImage("billet.png");
                        Stream infoProduct = this.controller.GetImage("informationProduct.png");

                        Stream[] imageStreams = new Stream[] { rlList, org, billet, role, infoProduct };
                        for (int i = 0; i < imageStreams.Length; i++)
                        {
                            Bitmap bitmap = new Bitmap(imageStreams[i]);
                            tempList.Images.Add("" + i, new Bitmap(bitmap));
                            bitmap.Dispose();
                            imageStreams[i].Close();
                        }

                        this.treeView.ImageList = tempList;
                    }
                }
            }
        }
        #endregion

        private void validationEventHandler(object sender, ValidationEventArgs args)
        {
            String message = String.Empty;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    message = "Import Error: " + args.Message;
                    break;
                case XmlSeverityType.Warning:
                    message = "Import Warning: " + args.Message;
                    break;
            }

            throw new System.Xml.Schema.XmlSchemaValidationException(args.Message);
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (hasFinishedInitialization)
            {
                // Set the view port based on the scroll bar
                winChartViewer1.ViewPortLeft = ((double)(hScrollBar1.Value - hScrollBar1.Minimum))
                    / (hScrollBar1.Maximum - hScrollBar1.Minimum);
                winChartViewer2.ViewPortLeft = ((double)(hScrollBar1.Value - hScrollBar1.Minimum))
                    / (hScrollBar1.Maximum - hScrollBar1.Minimum);
                winChartViewer3.ViewPortLeft = ((double)(hScrollBar1.Value - hScrollBar1.Minimum))
                    / (hScrollBar1.Maximum - hScrollBar1.Minimum);

                // Update the chart display without updating the image maps. (We can delay updating
                // the image map until scrolling is completed and the chart display is stable.)
                winChartViewer1.updateViewPort(true, false);
                winChartViewer2.updateViewPort(true, false);
                winChartViewer3.updateViewPort(true, false);
            }
        }

        private void winChartViewer2_ViewPortChanged(object sender, WinViewPortEventArgs e)
        {
            // Synchronize the horizontal scroll bar with the WinChartViewer
            hScrollBar1.Enabled = winChartViewer2.ViewPortWidth < 1;
            hScrollBar1.LargeChange = (int)Math.Ceiling(winChartViewer2.ViewPortWidth *
                (hScrollBar1.Maximum - hScrollBar1.Minimum));
            hScrollBar1.SmallChange = (int)Math.Ceiling(hScrollBar1.LargeChange * 0.1);
            hScrollBar1.Value = (int)Math.Round(winChartViewer2.ViewPortLeft *
                (hScrollBar1.Maximum - hScrollBar1.Minimum)) + hScrollBar1.Minimum;

            // Update chart and image map if necessary
            if (e.NeedUpdateChart)
                drawChart(winChartViewer2);
        }

        private void winChartViewer3_ViewPortChanged(object sender, WinViewPortEventArgs e)
        {
            // Synchronize the horizontal scroll bar with the WinChartViewer
            hScrollBar1.Enabled = winChartViewer3.ViewPortWidth < 1;
            hScrollBar1.LargeChange = (int)Math.Ceiling(winChartViewer3.ViewPortWidth *
                (hScrollBar1.Maximum - hScrollBar1.Minimum));
            hScrollBar1.SmallChange = (int)Math.Ceiling(hScrollBar1.LargeChange * 0.1);
            hScrollBar1.Value = (int)Math.Round(winChartViewer3.ViewPortLeft *
                (hScrollBar1.Maximum - hScrollBar1.Minimum)) + hScrollBar1.Minimum;

            // Update chart and image map if necessary
            if (e.NeedUpdateChart)
                drawChart(winChartViewer3);
        }

        private void drawChart(WinChartViewer viewer)
        {
            Chart.IChartExplorer chart = viewer.Tag as Chart.IChartExplorer;
            if (chart != null)
                chart.createChart(viewer, beginDate, dateRange);
        }
    }
}
