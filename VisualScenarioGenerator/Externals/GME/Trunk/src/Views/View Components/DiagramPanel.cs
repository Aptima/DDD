using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Xml.XPath;
using AME.Model;
using Northwoods.Go;
using System.Xml;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Collections.Specialized;

namespace AME.Views.View_Components
{
    public partial class DiagramPanel : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Dictionary<int, Diagram> diagrams = new Dictionary<int, Diagram>();
        private Dictionary<int, DiagramGridPanel> grids = new Dictionary<int, DiagramGridPanel>();
        private Dictionary<int, CustomComboToolStripItem> combos = new Dictionary<int, CustomComboToolStripItem>();

        public delegate void TabClicked();

        public event TabClicked TabClick;

        private Boolean usingGeoReferenceUTM = false;
        private Boolean comboIsSelecting = false;
        private CustomCombo selectingCombo;

        public Boolean UsingGeoReferenceUTM
        {
            get 
            {
                return usingGeoReferenceUTM;
            }
            set
            {
                usingGeoReferenceUTM = value;
            }
        }
        private Decimal xPixel;
        private Decimal xRotation;
        private Decimal yRotation;
        private Decimal yPixel;
        private Decimal easting;
        private Decimal northing;

        private List<Diagram> diagramList = new List<Diagram>();

        public List<Diagram> Diagrams
        {
            get { return diagramList; }
        }

        private List<DiagramGridPanel> gridList = new List<DiagramGridPanel>();

        public List<DiagramGridPanel> Grids
        {
            get { return gridList; }
        }

        public Boolean TopToolStripVisible
        {
            get { return toolStripContainer1.TopToolStripPanel.Visible; }
            set { toolStripContainer1.TopToolStripPanel.Visible = value; }
        }

        public Boolean DropDownStripVisible
        {
            get { return dropDownToolStrip.Visible; }
            set { dropDownToolStrip.Visible = value; }
        }

        public Point DropDownStripLocation
        {
            get { return dropDownToolStrip.Location; }
            set { dropDownToolStrip.Location = value; }
        }

        public ToolStripItemCollection DropDownStripItems
        {
            get { return dropDownToolStrip.Items; }
        }

        public Boolean SelectLinkStripVisible
        {
            get { return selectLinkToolStrip.Visible; }
            set { selectLinkToolStrip.Visible = value; }
        }

        public Boolean DiagramGridStripVisible
        {
            get { return diagramGridToolStrip.Visible; }
            set { diagramGridToolStrip.Visible = value; }
        }

        public Boolean FilterStripVisible
        {
            get { return filterToolStrip.Visible; }
            set { filterToolStrip.Visible = value; }
        }

        public Boolean ZoomStripVisible
        {
            get { return zoomToolStrip.Visible; }
            set { zoomToolStrip.Visible = value; }
        }

        public Point SelectLinkLocation
        {
            get { return selectLinkToolStrip.Location; }
            set { selectLinkToolStrip.Location = value; }
        }

        public Point DiagramGridStripLocation
        {
            get { return diagramGridToolStrip.Location; }
            set { diagramGridToolStrip.Location = value; }
        }
        public Point FilterStripLocation
        {
            get { return filterToolStrip.Location; }
            set { filterToolStrip.Location = value; }
        }

        public Point ZoomStripLocation
        {
            get { return zoomToolStrip.Location; }
            set { zoomToolStrip.Location = value; }
        }

        public void OnTabClicked()
        {
            if (TabClick != null)
            {
                TabClick(); 
            }
        }

        // string list of types to populate filter toolstrip, set in designer
        private String[] filterTypes;

        public String[] FilterTypes
        {
            get { return filterTypes; }
            set { filterTypes = value; }
        }

        public int DiagramTabIndex
        {
            get { return tabControl1.SelectedIndex; }
        }

        [DefaultValue(0)]
        public Int32 SelectedIndex
        {
            get
            {
                return this.tabControl1.SelectedIndex;
            }
            set
            {
                if (value >= 0)
                    this.tabControl1.SelectedIndex = value;
            }
        }
        
        
        public DiagramPanel()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.selection.Click += new EventHandler(selection_Click);
            this.linking.Click += new EventHandler(linking_Click);
            this.diagramView.Click += new EventHandler(diagramView_Click);
            this.gridView.Click += new EventHandler(gridView_Click);

            selection.CheckState = CheckState.Checked;
            diagramView.CheckState = CheckState.Checked;

            // default to invisible
            DropDownStripVisible = false;
        }

        private void selection_Click(object sender, EventArgs e)
        {
            selection.CheckState = CheckState.Checked;
            linking.CheckState = CheckState.Unchecked;

            foreach(Diagram theGoView in diagrams.Values)
            {
                theGoView.SelectAndMoveToolsOnly();
            }
        }

        private void linking_Click(object sender, EventArgs e)
        {
            selection.CheckState = CheckState.Unchecked;
            linking.CheckState = CheckState.Checked;

            foreach(Diagram theGoView in diagrams.Values)
            {
                theGoView.LinkingToolsOnly();
            }
        }

        private void diagramView_Click(object sender, EventArgs e)
        {
            if (gridView.CheckState == CheckState.Checked)
            {
                gridView.CheckState = CheckState.Unchecked;
                diagramView.CheckState = CheckState.Checked;
                
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (diagrams.ContainsKey(i))
                    {
                        tabControl1.TabPages[i].Controls.Remove(grids[i]);
                        tabControl1.TabPages[i].Controls.Add(diagrams[i]);
                    }
                }
                UpdateViewComponent();
            }
        }

        private void gridView_Click(object sender, EventArgs e)
        {
            if (diagramView.CheckState == CheckState.Checked)
            {
                diagramView.CheckState = CheckState.Unchecked;
                gridView.CheckState = CheckState.Checked;

                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (diagrams.ContainsKey(i))
                    {
                        tabControl1.TabPages[i].Controls.Remove(diagrams[i]);

                        DiagramGridPanel toAdd = grids[i];

                        if (toAdd.ParameterName != null && toAdd.ParameterName != "")
                        {
                           toAdd.Label = "Link Parameter: " + toAdd.ParameterName;
                        }
                        else
                        {
                            toAdd.Label = toAdd.LinkName + " Matrix View";
                        }

                        tabControl1.TabPages[i].Controls.Add(grids[i]);
                    }
                }
                UpdateViewComponent();
            }
        }

        private void Diagram_MouseMove(object sender, MouseEventArgs evt)
        {
            Diagram dg = (Diagram)sender;

            ProcessHoverOrDragWithDocPoint(dg.LastInput.DocPoint, dg);
        }

        private void Diagram_DragOver(object sender, DragEventArgs e)
        {
            Diagram dg = (Diagram)sender;

            ProcessHoverOrDragWithDocPoint(dg.LastInput.DocPoint, dg);
        }

        private void ProcessHoverOrDragWithDocPoint(PointF docPoint, Diagram dg)
        {
            if (usingGeoReferenceUTM)
            {
                Double xGeo = Convert.ToDouble(this.easting) + docPoint.X * Convert.ToDouble(this.xPixel) + docPoint.Y * Convert.ToDouble(this.xRotation);
                Double yGeo = Convert.ToDouble(this.northing) + docPoint.Y * Convert.ToDouble(this.yPixel) + docPoint.X * Convert.ToDouble(this.yRotation);
                labelCoord.Text = String.Format("{0:0.00} E {1:0.00} N", xGeo, yGeo);
            }
            else
            {
                // use custom transform
                int printX = (int)docPoint.X;
                int printY = (int)docPoint.Y;

                if (dg != null && dg.CoordinateTransformer != null)
                {
                    printX = (int)dg.CoordinateTransformer.StoreX(printX);
                    printY = (int)dg.CoordinateTransformer.StoreY(printY);
                }
                labelCoord.Text = String.Format("X: {0} Y: {1}", printX, printY);
            }
        }

        public CustomCombo GetCurrentDynamicCombo()
        {
            if (combos.ContainsKey(tabControl1.SelectedIndex))
            {
                CustomComboToolStripItem toolStripCombo = combos[tabControl1.SelectedIndex];
                return toolStripCombo.CustomComboControl;
            }
            else
            {
                return null;
            }
        }

        public CustomCombo GetDynamicCombo(Diagram d)
        {
            int index = diagramList.IndexOf(d);
            if (index != -1)
            {
                if (combos.ContainsKey(index))
                {
                    return combos[index].CustomComboControl;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnTabClicked();
            UpdateViewComponent();
        }

        private void LoadDiagrams(IController aController, String[] linkTypes)
        {
            if (aController == null || linkTypes == null || linkTypes.Length == 0)
            {
                return;
            }

            List<String> linkTypesList = new List<String>(linkTypes);

            diagrams.Clear();
            grids.Clear();
            combos.Clear();

            myController = aController;

            IXPathNavigable globalComponent = (myController).GetLinks();

            if (globalComponent == null)
            {
                return;
            }

            XPathNavigator nav = globalComponent.CreateNavigator();
            XPathNodeIterator diagramIterator = nav.Select("Link");

            String name;
            String linkType;
            String description;
            String dynamicType;

            Diagram templateDiagram;
            DiagramGridPanel templateGrid;
            int index = tabControl1.TabPages.Count;

            foreach (XPathNavigator collectionNav in diagramIterator)
            {
                linkType = collectionNav.GetAttribute(ConfigFileConstants.Type, collectionNav.NamespaceURI);

                if (linkTypesList.Contains(linkType))
                {
                    name = collectionNav.GetAttribute(ConfigFileConstants.Name, collectionNav.NamespaceURI);
                    description = collectionNav.GetAttribute(ConfigFileConstants.description, collectionNav.NamespaceURI);
                    dynamicType = collectionNav.GetAttribute(ConfigFileConstants.dynamicType, collectionNav.NamespaceURI);

                    //diagram
                    templateDiagram = new Diagram();
                    templateDiagram.DiagramName = linkType;
                    templateDiagram.Controller = myController;
                    templateDiagram.Dock = DockStyle.Fill;
                    templateDiagram.MouseMove += new MouseEventHandler(Diagram_MouseMove);
                    templateDiagram.DragOver += new DragEventHandler(Diagram_DragOver);
                    templateDiagram.PropertyChanged += new PropertyChangedEventHandler(templateDiagram_PropertyChanged);
                    
                    // turn off linking by default
                    templateDiagram.ReplaceMouseTool(typeof(GoToolLinking), null);
                    templateDiagram.ReplaceMouseTool(typeof(GoToolLinkingNew), null);
                    templateDiagram.ReplaceMouseTool(typeof(GoToolRelinking), null);
                    //

                    diagrams.Add(index, templateDiagram);

                    //grid
                    templateGrid = new DiagramGridPanel();
                    templateGrid.LinkType = linkType;
                    templateGrid.LinkName = name;
                    templateGrid.Controller = myController;
                    templateGrid.ParameterName = GetParameterName(collectionNav);
                    templateGrid.Dock = DockStyle.Fill;
                    grids.Add(index, templateGrid);

                    // initialize tab name and place the diagram in the appropriate tab
                    CustomTabPage tabPage = new CustomTabPage();
                    tabPage.Text = name;
                    tabPage.Description = description;
                    tabPage.Controls.Add(templateDiagram);
                    tabControl1.TabPages.Add(tabPage);

                    if (dynamicType != null && dynamicType.Length > 0)
                    {
                        CustomToolStrip toolStripAboveDiagram = new CustomToolStrip();
                        ToolStripLabel toolStripLabel = new ToolStripLabel();
                        ToolStripSeparator toolStripSeperator = new ToolStripSeparator();
                        CustomComboToolStripItem toolStripCombo = new CustomComboToolStripItem();
                        toolStripAboveDiagram.Dock = DockStyle.Top;

                        foreach (Control c in tabPage.Controls)
                        {
                            if (c is Diagram)
                            {
                                c.Dock = DockStyle.None; // anchor instead of dock
                                c.Location = new Point(0, 55); // move down
                                c.Anchor = AnchorStyles.Bottom | AnchorStyles.Top |
                                            AnchorStyles.Left | AnchorStyles.Right;
                                Size set = new Size();
                                set.Width = tabPage.Width - 5; // 5 for buffer
                                set.Height = tabPage.Height - 25 - 28 - 5; // 25 for the combo, 28 for the label, 5 for buffer
                                c.Size = set;
                            }
                        }

                        tabPage.Controls.Add(toolStripAboveDiagram);

                        toolStripAboveDiagram.Controller = myController;
                        toolStripAboveDiagram.Location = new Point(0, 0);
                        toolStripAboveDiagram.Size = new System.Drawing.Size(tabControl1.Width, 25);

                        toolStripLabel.Text = dynamicType;

                        toolStripCombo.Controller = myController;
                        toolStripCombo.Type = dynamicType; // Event EventTask
                        toolStripCombo.LinkType = myController.ConfigurationLinkType; // e.g. Tree
                        toolStripCombo.SelectedIDChangedEvent += new CustomComboToolStripItem.SelectedIDChanged(toolStripCombo_SelectedIDChangedEvent);

                        toolStripAboveDiagram.Items.AddRange(new ToolStripItem[] { toolStripLabel, toolStripSeperator, toolStripCombo });

                        combos.Add(index, toolStripCombo); // update hashmap
                    }

                    index++;
                } 
            }

            diagramList.Clear();
            foreach (Diagram d in diagrams.Values)
            {
                diagramList.Add(d);
            }

            gridList.Clear();
            foreach (DiagramGridPanel g in grids.Values)
            {
                gridList.Add(g);
            }

            // initialize filters for all diagrams
            if (filterTypes != null)
            {
                int filterCount = filterTypes.Length;
                if (filterCount > 0)
                {
                    for (int i = 0; i < filterCount; i++)
                    {
                        String filterTypeString = filterTypes[i];
                        ToolStripButton filterButton = new ToolStripButton(filterTypeString);
                        filterButton.Checked = true; // initially checked
                        filterButton.Click += new EventHandler(filterButton_Click);

                        foreach (Diagram d in diagramList) // initially, all types are shown
                        {
                            d.AddTypeFilter(filterButton.Text);
                            d.AddTypeFilter("fill");
                        }

                        filterToolStrip.Items.Add(filterButton);
                    }
                }
                else
                {
                    FilterStripVisible = false;
                }
            }
            else
            {
                FilterStripVisible = false;
            }
        }

        private void templateDiagram_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DocScale"))
            {
                Diagram dg = (Diagram)sender;

                if (dg.Equals(this.Diagrams[this.DiagramTabIndex]))
                {
                    float docScaleF = (dg.DocScale / 1.0f) * 100.0f;

                    int docScaleInt = (int)docScaleF;

                    zoomComboBox.Text = docScaleInt.ToString();
                }
            }
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            ToolStripButton filterItem = (ToolStripButton)sender;

            if (filterItem.CheckState == CheckState.Checked)
            {
                filterItem.CheckState = CheckState.Unchecked;
            }
            else if (filterItem.CheckState == CheckState.Unchecked)
            {
                filterItem.CheckState = CheckState.Checked;
            }

            foreach (Diagram d in diagrams.Values)
            {
                if (filterItem.CheckState == CheckState.Checked)
                {
                    d.AddTypeFilter(filterItem.Text);
                }
                else if (filterItem.CheckState == CheckState.Unchecked)
                {
                    d.RemoveTypeFilter(filterItem.Text);
                }

            }

            int selectedIndex = tabControl1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                diagramList[selectedIndex].ResetCRC();
                diagramList[selectedIndex].UpdateViewComponent();
            }
        }

        private void zoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        private void zoomComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateZoom();
            }
        }

        private void UpdateZoom()
        {
            String text = zoomComboBox.Text;
            try
            {
                float zoomLevel = float.Parse(text);
                if (diagrams.ContainsKey(tabControl1.SelectedIndex))
                {
                    Diagram selected = diagrams[tabControl1.SelectedIndex];

                    float zoomPercentage = zoomLevel / 100.0f;

                    float scale = 1.0f * zoomPercentage;

                    selected.DocScale = scale;
                }
            }
            catch (Exception) {
                throw;
            }
        }

        public delegate void DynamicComboIDChanged(CustomCombo source, int ID, String Name);

        public event DynamicComboIDChanged DynamicComboIDChangedEvent;

        private void toolStripCombo_SelectedIDChangedEvent(CustomCombo source, int ID, string Name)
        {
            comboIsSelecting = true;
            selectingCombo = source;

            UpdateDiagramFromDynamicCombo(source, ID, Name);

            if (DynamicComboIDChangedEvent != null)
            {
                DynamicComboIDChangedEvent(source, ID, Name); // forward the combo event to external listeners
            }

            comboIsSelecting = false;
            selectingCombo = null;
        }

        private void UpdateDiagramFromDynamicCombo(CustomCombo dynamicCombo, int ID, string Name)
        {
            if (diagrams.ContainsKey(tabControl1.SelectedIndex) && grids.ContainsKey(tabControl1.SelectedIndex))
            {
                Diagram theGoView = diagrams[tabControl1.SelectedIndex];
                DiagramGridPanel theGrid = grids[tabControl1.SelectedIndex];

                String newDynamicLinkType = this.Controller.GetDynamicLinkType(theGoView.DiagramName, ID.ToString());

                theGoView.RootID = ID;
                theGoView.DisplayID = ID;
                theGoView.DiagramName = newDynamicLinkType;
                theGrid.LinkType = newDynamicLinkType;

                UpdateViewComponent();
            }
        }

        private String GetParameterName(XPathNavigator collectionNav)
        {
            XPathNavigator parametersTest = collectionNav.SelectSingleNode("ComplexParameters");

            if (parametersTest == null)
            {
                return "";
            }
            else
            {
                String returnString = parametersTest.GetAttribute(ConfigFileConstants.defaultParameter, parametersTest.NamespaceURI);
                if (returnString == null)
                {
                    return "";
                }
                else
                {
                    // need category too
                    XPathNavigator belowComplex = parametersTest.SelectSingleNode("Parameters/Parameter[@displayedName='" + returnString + "']");
                    String categoryAttr = belowComplex.GetAttribute(ConfigFileConstants.category, belowComplex.NamespaceURI);
                    if (categoryAttr != null)
                    {
                        returnString = categoryAttr + SchemaConstants.ParameterDelimiter + returnString;
                    }
                    return returnString;
                }
            }
        }

        public void SelectNodeWithID(int newSelected)
        {
            if (diagrams.ContainsKey(tabControl1.SelectedIndex))
            {
                GoView theGoView = diagrams[tabControl1.SelectedIndex];
                theGoView.Selection.Clear();

                foreach (GoObject goObj in theGoView.Document)
                {
                    if (goObj is HasNodeID)
                    {
                        HasNodeID test = (HasNodeID)goObj;
                        if (test.NodeID == newSelected)
                        {
                            theGoView.Selection.Select(goObj);
                            goObj.Layer.MoveAfter(null, goObj); // update z-order
                            theGoView.ScrollRectangleToVisible(goObj.Bounds);
                            break;
                        }
                    }
                }
            }

            if (combos.ContainsKey(tabControl1.SelectedIndex))
            {
                CustomComboToolStripItem toolStripCombo = combos[tabControl1.SelectedIndex];
                CustomCombo internalCombo = toolStripCombo.CustomComboControl;

                foreach (Object comboItem in internalCombo.Items)
                {
                    int testID = ((ComboItem)comboItem).MyID;
                    if (testID == newSelected)
                    {
                        internalCombo.SelectedID = testID;
                        internalCombo.SelectedItem = comboItem;
                        
                        UpdateDiagramFromDynamicCombo(internalCombo, testID, ((ComboItem)comboItem).MyName);
                        break;
                    }
                }
            }
        }

        public void SetID(int newID)
        {
            foreach (Diagram d in diagrams.Values)
            {
                d.DisplayID = newID;
                d.RootID = newID;
            }

            foreach (DiagramGridPanel g in grids.Values)
            {
                g.DisplayID = newID;
                g.SelectedID = newID;
            }

            foreach (CustomComboToolStripItem c in combos.Values)
            {
                c.DisplayID = newID;
            }
        }

        public void SetDiagramBackground(Image background, String linktype)
        {
            foreach (Diagram d in diagrams.Values)
            {
                if (d.DiagramName.Equals(linktype))
                {
                    if (background != null)
                    {
                        d.BackgroundImage = background;
                        d.Document.Size = background.Size;
                        d.Document.FixedSize = true; // doc top left, docscale doc position
                    }
                    else
                    {
                        d.BackgroundImage = null;
                        d.Document.Size = d.ComputeDocumentBounds().Size;
                        d.Document.FixedSize = false;
                    }
                }
            }
            this.diagramView.PerformClick();
        }

        public void SetGeoReferenceUTM(Decimal xpixel, Decimal xrotation, Decimal yrotation, Decimal ypixel, Decimal easting, Decimal northing)
        {
            this.xPixel = xpixel;
            this.xRotation = xrotation;
            this.yRotation = yrotation;
            this.yPixel = ypixel;
            this.easting = easting;
            this.northing = northing;
        }

        IController myController;

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;

                this.LoadDiagrams(myController, linkTypes);
            }
        }

        private String[] linkTypes;

        public String[] LinkTypes
        {
            get { return linkTypes; }
            set 
            {
                linkTypes = value;

                this.LoadDiagrams(myController, linkTypes);
            }
        }

        public void UpdateViewComponent()
        {
            int selectedIndex = tabControl1.SelectedIndex;

            if (selectedIndex >= 0)
            {
                if (tabControl1.TabPages.Count > 0)
                {
                    foreach (Control c in tabControl1.TabPages[selectedIndex].Controls)
                    {
                        if (c is IViewComponent)
                        {
                            // don't update the combo if it's selecting
                            if (comboIsSelecting)
                            {
                                if (c is CustomToolStrip)
                                {
                                    if ((!((CustomToolStrip)c).Contains(selectingCombo)))
                                    {
                                        ((IViewComponent)c).UpdateViewComponent();
                                    }
                                }
                                else
                                {
                                    ((IViewComponent)c).UpdateViewComponent();
                                }
                            }
                            else
                            {
                                ((IViewComponent)c).UpdateViewComponent();
                            }

                            if (c is Diagram)
                            {
                                Diagram dg = (Diagram)c;

                                if (dg.Equals(this.Diagrams[this.DiagramTabIndex]))
                                {
                                    float docScaleF = (dg.DocScale / 1.0f) * 100.0f;

                                    int docScaleInt = (int)docScaleF;

                                    zoomComboBox.Text = docScaleInt.ToString();
                                }

                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
