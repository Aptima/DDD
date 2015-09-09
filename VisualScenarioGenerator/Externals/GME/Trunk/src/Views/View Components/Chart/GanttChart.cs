using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Windows.Forms;
using ChartDirector;
using AME.Controllers;
using System.Xml.Schema;
using System.IO;
using System.Drawing.Drawing2D;

namespace AME.Views.View_Components
{
    public partial class GanttChart : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private AME.Controllers.IController controller = null;
        private Int32 componentId = -1;
        private String category = "category";
        private String parameter = "filename";
        private XPathNavigator navigator;
        private String fileNameToSave = "";

        private DateTime beginDate;
        private DateTime endDate;
        private DateTime currentDate;

        private double dateRange;
        private double rowRange;

        private double currentDuration = 3600;
        private double scaleMinutes = 30;

        // for Filters, Colors
        private List<CheckedListBox> orLists;
        private List<CheckedListBox> andLists;

        private String orType = "";

        private string nameAttribute = "name";  // for schema
        private string idAttribute = "id";
        private string typeAttribute = "type";
        private string colorAttribute = "color";
        private string filteredIDsElement = "FilteredIDs";

        private Font smallTextFont = new Font("Arial", 8);
        private Font textFont = new Font("Arial", 10);

        private string orFilterXPath = "FilterConfiguration/OrFilters/OrFilter";
        private string andFilterXPath = "FilterConfiguration/AndFilters/AndFilter";

        private string filterConfigurationOrFilters = "Timeline/FilterConfiguration/OrFilters";
        private string filterConfigurationAndFilters = "Timeline/FilterConfiguration/AndFilters";

        private string orIDXPath = "Timeline/FilterConfiguration/OrFilters/FilteredIDs/ID";
        private string andIDXPath = "Timeline/FilterConfiguration/AndFilters/FilteredIDs/ID";

        private Form dialogForm;
        private Form colorForm;

        private ListBox colorList;

        private Panel colorSwatch;
        // /for Filters, Color

        private List<String> timelineRows;
        private List<String> timelineBlockNames;
        private List<DateTime> timelineBlockBegins;
        private List<DateTime> timelineBlockEnds;
        private List<Double> timelineBlockRow;
        private List<Double> timelineBlockZones;
        private List<Int32> timelineBlockColors;
        private List<String> timelineRowNames;
        private List<Int32> timelineRowColors;
        private List<DateTime> timelineMilestoneTimes;
        private List<String>timelineMilestoneNames;
        private List<Int32>timelineMilestoneColors;

        private bool hasFinishedInitialization = false;
        private bool bShowMilestones = true;
        private int scaleMultiplierInMinutes = 0;

        /// <summary>
        /// The component id that has the filename parameter.
        /// </summary>
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

        /// <summary>
        /// The category name for the filename parameter.
        /// </summary>
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
       
        /// <summary>
        /// The name of the filename parameter.
        /// </summary>
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
        
        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public void SetMilestoneCheckText(String text)
        {
            this.showMilestones.Text = text;
        }

        public void SetMilestoneCheckLocation(Point p)
        {
            this.showMilestones.Location = p;
        }

        public void SetTopTitle(String topTitle)
        {
            this.label1.Text = topTitle;
        }

        public void SetTopForeColor(Color c)
        {
            this.label1.ForeColor = c;
        }

        public void SetTopBackColor(Color c)
        {
            this.label1.BackColor = c;
        }


        public void SetScaleLabel(String scalelabelText)
        {
            this.scaleLabel.Text = scalelabelText;
        }

        public void SetScaleValue(int val)
        {
            if (scaleMultiplierInMinutes > 0)
            {
                scaleMinutes = val * scaleMultiplierInMinutes;
            }
            this.scaleTextBox.Text = "" + val;
        }

        public void SetScaleMultiplier(int multiplier)
        {
            this.scaleMultiplierInMinutes = multiplier;
        }

        public void SetLegendVisibility(bool show)
        {
            this.legend.Visible = show;
            this.legendLabel.Visible = show;
        }

        public void SetDataSetupVisibility(bool show)
        {
            this.filterButton.Visible = show;
        }

        public void SetViewSetupVisibility(bool show)
        {
            this.paletteButton.Visible = false;
        }

        public void MoveScaleToLocation(Point p)
        {
            this.scaleLabel.Location = p;
            this.scaleTextBox.Location = new Point(p.X + 3, p.Y + 16);
        }

        public void UpdateViewComponent()
        {
            UpdateViewComponent(true);
        }

        private String[] legendStrings;
        private Int32[] legendColors;
        private double legendFontSize;

        public void AddLegend(String[] text, Int32[] colors, double fontSize)
        {
            legendStrings = text;
            legendColors = colors;
            legendFontSize = fontSize;
        }

        public void UpdateViewComponent(bool rebuildFilters)
        {
            DrawingUtility.SuspendDrawing(legend);
            legend.BeginUpdate();
            legend.Items.Clear();

            if (controller != null && componentId >= 0)
            {
                // Read in the data
                loadData(rebuildFilters);

                if (navigator != null)
                {
                    paletteButton.Enabled = true;
                    filterButton.Enabled = true;

                    dateRange = endDate.Subtract(beginDate).TotalSeconds;

                    winChartViewer1.ViewPortWidth = currentDuration / dateRange;

                    winChartViewer1.ViewPortHeight = 10 / rowRange;

                    // Can update chart now
                    hasFinishedInitialization = true;
                    winChartViewer1.updateViewPort(true, true);
                }
            }
            else
            {
                ResetView();
            }

            winChartViewer1.updateViewPort(true, true);

            legend.EndUpdate();
            DrawingUtility.ResumeDrawing(legend);
        }

        #endregion

        public GanttChart()
        {
            myHelper = new ViewComponentHelper(this);

            ChartDirector.Chart.setLicenseCode("DIST-0000-0536-4cc1-aec1");
            InitializeComponent();

            this.filterButton.Click += new EventHandler(Filter_Button_Click);
            this.paletteButton.Click += new EventHandler(PaletteButton_Click);

            this.scaleTextBox.Leave += new EventHandler(scaleTextBox_Leave);
            this.scaleTextBox.KeyDown += new KeyEventHandler(scaleTextBox_KeyDown);

            this.showMilestones.CheckedChanged += new EventHandler(showMilestones_CheckedChanged);
        }

        private void showMilestones_CheckedChanged(object sender, EventArgs e)
        {
            bShowMilestones = !bShowMilestones;
            UpdateViewComponent();
        }

        private void UpdateScale()
        {
            try
            {
                if (scaleTextBox.Text != null && scaleTextBox.Text.Length > 0)
                {
                    int tScaleMinutes = Int32.Parse(scaleTextBox.Text);
                    if (tScaleMinutes > 0)
                    {
                        if (scaleMultiplierInMinutes > 0)
                        {
                            tScaleMinutes = tScaleMinutes * scaleMultiplierInMinutes;
                        }
                        scaleMinutes = tScaleMinutes;
                        UpdateViewComponent();
                    }
                    else
                    {
                        scaleTextBox.Text = "" + scaleMinutes;
                    }
                }
                else
                {
                    scaleTextBox.Text = "" + scaleMinutes;
                }
            }
            catch (FormatException e2)
            {
                scaleTextBox.Text = "" + scaleMinutes;
                MessageBox.Show(e2.Message, "Format error");
            }
        }

        private void scaleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateScale();
            }
        }
        
        private void scaleTextBox_Leave(object sender, EventArgs e)
        {
            UpdateScale();
        }

        # region Filter
        // Filter click

        private void PaletteButton_Click(object sender, EventArgs e)
        {
            colorForm.StartPosition = FormStartPosition.CenterParent;
            colorForm.ShowDialog();
        }

        private void colorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;

            TimelineListItem selectedItem = (TimelineListItem)box.SelectedItem;

            Color convert = HexToColor(selectedItem.ColorString);

            colorSwatch.BackColor = convert;
        }

        private Color HexToColor(string hex)
        {
            ColorConverter conv = new ColorConverter();

            if (hex == null || hex.Length == 0)
            {
                return new Color();
            }
            else if(!hex.StartsWith("0x"))
            {
                hex = "0x"+hex;
            }

            Color c = (Color)conv.ConvertFromString(hex);
            return c;
        }


        private string ColorToHex(Color c)
        {
            string rhex = c.R.ToString("X2");
            string ghex = c.G.ToString("X2");
            string bhex = c.B.ToString("X2");

            return rhex + ghex + bhex;
        }


        private void colorPromptButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            DialogResult result = dialog.ShowDialog();

            TimelineListItem selectedItem = (TimelineListItem)colorList.SelectedItem;

            if (result.Equals(DialogResult.OK) && selectedItem != null)
            {
                colorSwatch.BackColor = dialog.Color;

                String hex = ColorToHex(dialog.Color);

                selectedItem.ColorString = hex;

                XPathNodeIterator checkIt = navigator.Select("//TimelineRow[" +
                                                           "@id = '"+selectedItem.ID + "'] [" +
                                                           "@name = '" + selectedItem.Name + "'] [" +
                                                           "@type = '" + selectedItem.Type + "'] | " +
                                                           "//TimelineBlock[" +
                                                           "@id = '"+selectedItem.ID + "'] [" +
                                                           "@name = '" + selectedItem.Name + "'] [" +
                                                           "@type = '" + selectedItem.Type + "'] | " +
                                                           "//TimelineFilter[" +
                                                           "@id = '" + selectedItem.ID + "'] [" +
                                                           "@name = '" + selectedItem.Name + "'] [" +
                                                           "@type = '" + selectedItem.Type + "'] | " +
                                                           "//TimelineMilestone[" +
                                                           "@id = '"+selectedItem.ID + "'] [" +
                                                           "@name = '" + selectedItem.Name + "']");

                foreach (XPathNavigator check in checkIt)
                {
                if (check != null)
                {
                    if (check.MoveToAttribute(colorAttribute, ""))
                    {
                        check.SetValue(hex);
                    }
                    else
                    {
                        check.CreateAttribute("", colorAttribute, "", hex);
                    }
                    }

                    check.MoveToParent();

                    if (check.Name.Equals("TimelineFilter"))
                    {
                        if (check.MoveToParent())
                        {
                            if (check.MoveToAttribute(colorAttribute, ""))
                            {
                                check.SetValue(hex);
                            }
                            else
                            {
                                check.CreateAttribute("", colorAttribute, "", hex);
                            }
                        }
                    }
                }

                    SaveNavigator();

                    UpdateViewComponent(false);
                }
            }

        private void SaveNavigator()
        {
            String path = Path.Combine(this.controller.OutputPath, fileNameToSave + ".xml");
            XmlWriter writer = XmlWriter.Create(path);
            navigator.MoveToRoot();
            writer.WriteNode(navigator, true);
            writer.Close();
        }

        private void Filter_Button_Click(object sender, EventArgs e)
        {
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.ShowDialog();
        }

        private void filterByButton_Click(object sender, EventArgs e) // e.g. clicked "all DMs"
        {
            CheckedListBox list = ((TimelineButton)sender).ListBox;

            if (list.Enabled)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, !list.GetItemChecked(i)); // reverse all items
                }
            }
        }

        private void orButton_Click(object sender, EventArgs e) // e.g. selected "DMs" or "Assets"
        {
            foreach (CheckedListBox list in orLists) // disable all lists, enable selected
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, false);
                }

                list.Enabled = false;
            }

            ((TimelineRadioButton)sender).ListBox.Enabled = true;
        }

        private void dialogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // collect filter IDs
            List<Int32> orIDs = new List<Int32>();
            List<Int32> andIDs = new List<Int32>();

            foreach (CheckedListBox or in orLists)
            {
                if (or.Enabled)
                {
                    for (int i = 0; i < or.Items.Count; i++)
                    {
                        if (or.GetItemChecked(i))
                        {
                            orIDs.Add(((TimelineListItem)or.Items[i]).ID);
                        }
                    }
                }
            }

            foreach (CheckedListBox and in andLists)
            {
                if (and.Enabled)
                {
                    for (int i = 0; i < and.Items.Count; i++)
                    {
                        if (and.GetItemChecked(i))
                        {
                            andIDs.Add(((TimelineListItem)and.Items[i]).ID);
                        }
                    }
                }
            }

            XPathNodeIterator orXPath = navigator.Select(filterConfigurationOrFilters);
            XPathNodeIterator andXPath = navigator.Select(filterConfigurationAndFilters);

            bool write = false;

            foreach (XPathNavigator localNav in orXPath)
            {
                if (localNav.MoveToChild(filteredIDsElement, "")) 
                {
                    localNav.DeleteSelf();
                    write = true;
                }

                if (orIDs.Count > 0)
                {
                    write = true;

                    localNav.AppendChildElement("", filteredIDsElement, "", "");

                    localNav.MoveToChild(filteredIDsElement, "");

                    foreach (int orID in orIDs)
                    {
                        localNav.AppendChildElement("", "ID", "", orID.ToString());
                    }
                }
            }

            foreach (XPathNavigator localNav in andXPath)
            {
                if (localNav.MoveToChild(filteredIDsElement, "")) 
                {
                    localNav.DeleteSelf();
                    write = true;
                }

                if (andIDs.Count > 0)
                {
                    write = true;

                    localNav.AppendChildElement("", filteredIDsElement, "", "");

                    localNav.MoveToChild(filteredIDsElement, "");

                    foreach (int andID in andIDs)
                    {
                        localNav.AppendChildElement("", "ID", "", andID.ToString());
                    }
                }
            }

            if (write)
            {
                SaveNavigator();
            }

            this.UpdateViewComponent();
        }

        private SizeF MeasureWidth(string mystring, Font f)
        {
            Bitmap b = new Bitmap(12, 12);
            Graphics g = Graphics.FromImage(b);
            SizeF CurrentWidth = g.MeasureString(mystring, f);
            g.Dispose();
            b.Dispose();
            return CurrentWidth;
        }

        private bool ListObjectCollectionContains(ListBox.ObjectCollection collection, int testID)
        {
            foreach (Object test in collection)
            {
                if (test is TimelineListItem)
                {
                    if (((TimelineListItem)test).ID == testID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void BuildFilters()
        {
            XPathNavigator timelineNavigator = navigator.SelectSingleNode("Timeline");

            dialogForm = new Form();
            dialogForm.AutoSize = true;
            dialogForm.FormClosed += new FormClosedEventHandler(dialogForm_FormClosed);

            colorForm = new Form();
            colorForm.AutoSize = true;
            colorList = new ListBox();
            colorList.Height = 100;
            colorList.Location = new Point(5, 10);
            colorList.Width = 150;
            colorList.Sorted = true;

            colorSwatch = new Panel();
            colorSwatch.Size = new Size(20, 20);
            colorSwatch.Location = new Point(160, 10);

            Button colorPromptButton = new Button();
            colorPromptButton.Text = "Choose Color...";
            colorPromptButton.BackColor = SystemColors.Control;
            colorPromptButton.AutoSize = true;
            colorPromptButton.Location = new Point(185, 10);
            colorPromptButton.UseVisualStyleBackColor = true;
            colorPromptButton.Click += new EventHandler(colorPromptButton_Click);

            // Legend xpath
            //XPathNodeIterator legendItemIterator = timelineNavigator.Select("Legend/LegendColumn/LegendItem");
            //foreach (XPathNavigator legendItem in legendItemIterator)
            //{
            //    string filterItemName = legendItem.GetAttribute(nameAttribute, legendItem.NamespaceURI);
            //    string filterItemID = legendItem.GetAttribute(idAttribute, legendItem.NamespaceURI);
            //    string filterItemType = legendItem.GetAttribute(typeAttribute, legendItem.NamespaceURI);
            //    string filterItemColor = legendItem.GetAttribute(colorAttribute, legendItem.NamespaceURI);
            //    //colorList.Items.Add(new TimelineListItem(filterItemName, Int32.Parse(filterItemID), filterItemType, filterItemColor));
            //}

            // find blocks without filters, filters, 'DMs', and Milestones
            XPathNodeIterator colorIterator = timelineNavigator.Select("//TimelineBlock | /Timeline/TimelineRow | //TimelineMilestone");
            foreach (XPathNavigator colorItem in colorIterator)
            {
                colorItem.MoveToChild("TimelineFilter", ""); // skip blocks with filter

                string filterItemName = colorItem.GetAttribute(nameAttribute, colorItem.NamespaceURI);
                string filterItemID = colorItem.GetAttribute(idAttribute, colorItem.NamespaceURI);
                string filterItemType = colorItem.GetAttribute(typeAttribute, colorItem.NamespaceURI);
                string filterItemColor = colorItem.GetAttribute(colorAttribute, colorItem.NamespaceURI);

                // also replace milestones with filters (if one exists)
                if (colorItem.Name.Equals("TimelineMilestone"))
                {
                    XPathNavigator filterCheck = timelineNavigator.SelectSingleNode("//TimelineFilter[" +
                                                                                    "@id = '" + filterItemID + "'] [" +
                                                                                    "@name = '" + filterItemName + "'] [" +
                                                                                    "@type = '" + filterItemType + "']");
                    if (filterCheck != null)
                    {
                        filterItemName = filterCheck.GetAttribute(nameAttribute, filterCheck.NamespaceURI);
                        filterItemID = filterCheck.GetAttribute(idAttribute, filterCheck.NamespaceURI);
                        filterItemType = filterCheck.GetAttribute(typeAttribute, filterCheck.NamespaceURI);
                        filterItemColor = filterCheck.GetAttribute(colorAttribute, filterCheck.NamespaceURI);
                    }
                }

                int iParse = Int32.Parse(filterItemID);

                if (!ListObjectCollectionContains(colorList.Items, iParse))
                {
                    colorList.Items.Add(new TimelineListItem(filterItemName, iParse, filterItemType, filterItemColor));
                }
            }

            colorList.SelectedIndexChanged += new EventHandler(colorList_SelectedIndexChanged);

            colorForm.Controls.Add(colorList);
            colorForm.Controls.Add(colorSwatch);
            colorForm.Controls.Add(colorPromptButton);

            colorForm.FormClosed += new FormClosedEventHandler(colorForm_FormClosed);

            orLists = new List<CheckedListBox>();
            andLists = new List<CheckedListBox>();

            List<Int32> orIDs = PopulateOrFilters();

            List<Int32> andIDs = PopulateAndFilters();

            GroupBox selectRadio = new GroupBox();

            selectRadio.Text = "Select a View";
            selectRadio.Size = new Size(10, 10);
            selectRadio.AutoSize = true;

            XPathNodeIterator orFilterIterator = timelineNavigator.Select(orFilterXPath);

            int radioX = 10;

            int orX = 10;

            foreach (XPathNavigator orFilterNavigator in orFilterIterator)
            {
                TimelineRadioButton aFilterButton = new TimelineRadioButton();

                string filterName = orFilterNavigator.GetAttribute(nameAttribute, orFilterNavigator.NamespaceURI);
                string filterType = orFilterNavigator.GetAttribute(typeAttribute, orFilterNavigator.NamespaceURI);

                aFilterButton.Text = filterName;
                aFilterButton.AutoSize = true;
                aFilterButton.Location = new Point(radioX, 20);
                radioX += (int)MeasureWidth(filterName, textFont).Width + 25;

                selectRadio.Controls.Add(aFilterButton);

                CheckedListBox filterListBox = new CheckedListBox();
                filterListBox.Height = 100;
                filterListBox.Location = new Point(5, 50);
                filterListBox.Width = 150;
                filterListBox.CheckOnClick = true;
                filterListBox.Sorted = true;

                GroupBox filterBy = new GroupBox();

                filterBy.Text = "Filter by: " + filterType;
                filterBy.Size = new Size(10, 10);
                filterBy.AutoSize = true;
                filterBy.Location = new Point(orX, 70);

                TimelineButton filterByButton = new TimelineButton();
                filterByButton.Text = "All " + filterType + "s";
                filterByButton.AutoSize = true;
                filterByButton.Location = new Point(5, 20);
                filterByButton.ListBox = filterListBox;
                filterByButton.Click += new EventHandler(filterByButton_Click);

                aFilterButton.Checked = false;
                filterListBox.Enabled = false;

                aFilterButton.ListBox = filterListBox;

                orLists.Add(filterListBox);

                aFilterButton.Click += new EventHandler(orButton_Click);

                filterBy.Controls.Add(filterByButton);

                dialogForm.Controls.Add(filterBy);

                orX += 175;

                XPathNodeIterator orFilterItemIterator = timelineNavigator.Select("//TimelineRow[@type='" + filterType + "']");
                foreach (XPathNavigator orFilterItemNavigator in orFilterItemIterator)
                {
                    string filterItemName = orFilterItemNavigator.GetAttribute(nameAttribute, orFilterNavigator.NamespaceURI);
                    string filterItemID = orFilterItemNavigator.GetAttribute(idAttribute, orFilterNavigator.NamespaceURI);
                    string filterItemType = orFilterItemNavigator.GetAttribute(typeAttribute, orFilterNavigator.NamespaceURI);

                    int iParse = Int32.Parse(filterItemID);

                    if (!ListObjectCollectionContains(filterListBox.Items, iParse))
                    {
                        int at = filterListBox.Items.Add(new TimelineListItem(filterItemName, iParse, filterItemType));

                        if (orIDs.Contains(Int32.Parse(filterItemID)))
                        {
                            filterListBox.SetItemChecked(at, true);

                            if (!orType.Equals(filterItemType))
                            {
                                orType = filterItemType;
                            }

                            if (aFilterButton.Checked == false || filterListBox.Enabled == false)
                            {
                                aFilterButton.Checked = true;
                                filterListBox.Enabled = true;
                            }
                        }
                    }
                }

                filterBy.Controls.Add(filterListBox);
            }

            Button clearAllButton = new Button();
            clearAllButton.Text = "Clear All";
            clearAllButton.AutoSize = true;
            clearAllButton.Location = new Point(radioX - 5, 20);
            selectRadio.Controls.Add(clearAllButton);
            clearAllButton.Click += new EventHandler(clearAllButton_Click);


            radioX = 10;

            orX = 10;

            XPathNodeIterator andFilterIterator = timelineNavigator.Select(andFilterXPath);

            foreach (XPathNavigator andFilterNavigator in andFilterIterator)
            {
                string filterName = andFilterNavigator.GetAttribute(nameAttribute, andFilterNavigator.NamespaceURI);
                string filterType = andFilterNavigator.GetAttribute(typeAttribute, andFilterNavigator.NamespaceURI);

                radioX += (int)MeasureWidth(filterName, textFont).Width + 25;

                CheckedListBox filterListBox = new CheckedListBox();
                filterListBox.Height = 100;
                filterListBox.Location = new Point(5, 50);
                filterListBox.Width = 150;
                filterListBox.CheckOnClick = true;
                filterListBox.Sorted = true;

                GroupBox filterBy = new GroupBox();

                filterBy.Text = "Filter by: " + filterType;
                filterBy.Size = new Size(10, 10);
                filterBy.AutoSize = true;
                filterBy.Location = new Point(orX, 250);

                TimelineButton filterByButton = new TimelineButton();
                filterByButton.Text = "All " + filterType + "s";
                filterByButton.AutoSize = true;
                filterByButton.Location = new Point(5, 20);
                filterByButton.ListBox = filterListBox;
                filterByButton.Click += new EventHandler(filterByButton_Click);

                andLists.Add(filterListBox);

                filterBy.Controls.Add(filterByButton);

                dialogForm.Controls.Add(filterBy);

                orX += 175;

                // filter xpath
                XPathNodeIterator andFilterItemIterator = timelineNavigator.Select("//TimelineFilter[@type='" + filterType + "']");
                foreach (XPathNavigator andFilterItemNavigator in andFilterItemIterator)
                {
                    string filterItemName = andFilterItemNavigator.GetAttribute(nameAttribute, andFilterNavigator.NamespaceURI);
                    string filterItemID = andFilterItemNavigator.GetAttribute(idAttribute, andFilterNavigator.NamespaceURI);
                    string filterItemType = andFilterItemNavigator.GetAttribute(typeAttribute, andFilterNavigator.NamespaceURI);

                    int iParse = Int32.Parse(filterItemID);

                    if (!ListObjectCollectionContains(filterListBox.Items, iParse))
                    {
                        int at = filterListBox.Items.Add(new TimelineListItem(filterItemName, iParse, filterItemType));

                        if (andIDs.Contains(Int32.Parse(filterItemID)))
                        {
                            filterListBox.SetItemChecked(at, true);
                        }
                    }
                }

                filterBy.Controls.Add(filterListBox);
            }

            dialogForm.Controls.Add(selectRadio);
        }

        private void colorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateViewComponent();
        }

        private void clearAllButton_Click(object sender, EventArgs e)
        {
            foreach (CheckedListBox list in orLists)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, false);
                }
            }

            foreach (CheckedListBox list in andLists)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, false);
                }
            }
        }

        private List<Int32> PopulateAndFilters()
        {
            List<Int32> andIDs = new List<Int32>();
            XPathNodeIterator andNavs = navigator.Select(andIDXPath);
            foreach (XPathNavigator andNavigator in andNavs)
            {
                andIDs.Add(Int32.Parse(andNavigator.Value));
            }

            return andIDs;
        }

        private List<Int32> PopulateOrFilters()
        {
            List<Int32> orIDs = new List<Int32>();
            XPathNodeIterator orNavs = navigator.Select(orIDXPath);
            foreach (XPathNavigator orNavigator in orNavs)
            {
                orIDs.Add(Int32.Parse(orNavigator.Value));
            }

            return orIDs;
        }


        #endregion 

        private void loadData(bool rebuildFilters)
        {
            getNavigator();

            if (navigator != null)
            {
                if (rebuildFilters)
                {
                    BuildFilters();
                }

                XPathNavigator navTimeline = navigator.SelectSingleNode("Timeline");
                // Get the dates and times.
                endDate = DateTime.Parse(navTimeline.GetAttribute("end", navigator.NamespaceURI));// DateTime.Now.Date;
                beginDate = DateTime.Parse(navTimeline.GetAttribute("start", navigator.NamespaceURI)); //DateTime.Now.Date.AddYears(-5);
                currentDate = DateTime.Parse(navTimeline.GetAttribute("currentTime", navigator.NamespaceURI)); //DateTime.Now.Date.AddYears(-5);

                // Get the TimelineRows
                timelineRows = new List<String>();
                timelineBlockNames = new List<String>();
                timelineBlockBegins = new List<DateTime>();
                timelineBlockEnds = new List<DateTime>();
                timelineBlockRow = new List<Double>();
                timelineBlockZones = new List<Double>();
                timelineBlockColors = new List<Int32>();
                timelineRowNames = new List<String>();
                timelineRowColors = new List<Int32>();
                timelineMilestoneTimes = new List<DateTime>();
                timelineMilestoneNames = new List<String>();
                timelineMilestoneColors = new List<Int32>();

                //timelineRows.Add("");

                //  Filters
                List<Int32> orIDs = PopulateOrFilters();

                List<Int32> andIDs = PopulateAndFilters();

                bool childAdded = false;
                Double row = 0.5;
                Double zone = 0;
                XPathNodeIterator itNavTimelineRows = navTimeline.Select("/Timeline/TimelineRow");
                while (itNavTimelineRows.MoveNext())
                {
                    childAdded = false; // reset commander check

                    String rowName = itNavTimelineRows.Current.GetAttribute("name", itNavTimelineRows.Current.NamespaceURI);
                    int rowID = Int32.Parse(itNavTimelineRows.Current.GetAttribute(idAttribute, itNavTimelineRows.Current.NamespaceURI));
                    String rowType = itNavTimelineRows.Current.GetAttribute(typeAttribute, itNavTimelineRows.Current.NamespaceURI);

                    if ((rowType == orType && orIDs.Count > 0 && orIDs.Contains(rowID)) || orIDs.Count == 0 || rowType != orType) // for filtering
                    {
                        XPathNodeIterator itNavTimelineRowChildren = itNavTimelineRows.Current.Select("TimelineRow");
                        while (itNavTimelineRowChildren.MoveNext())
                        {
                            String label = itNavTimelineRowChildren.Current.GetAttribute("name", itNavTimelineRowChildren.Current.NamespaceURI);
                            String subRowType = itNavTimelineRowChildren.Current.GetAttribute(typeAttribute, itNavTimelineRowChildren.Current.NamespaceURI);
                            int subRowID = Int32.Parse(itNavTimelineRowChildren.Current.GetAttribute(idAttribute, itNavTimelineRowChildren.Current.NamespaceURI));

                            if ((subRowType == orType && orIDs.Count > 0 && orIDs.Contains(subRowID)) || orIDs.Count == 0 || subRowType != orType) // for filtering
                            {
                                timelineRows.Add(label);

                                if (!childAdded) // only add commander if a child passes filters
                                {
                                    timelineRowNames.Add(rowName);
                                    String rowColor = itNavTimelineRows.Current.GetAttribute("color", itNavTimelineRows.Current.NamespaceURI);
                                    timelineRowColors.Add(Int32.Parse(rowColor, System.Globalization.NumberStyles.AllowHexSpecifier));
                                    childAdded = true;
                                }

                                // Get the TimelineBlocks
                                XPathNodeIterator itNavBlocks = itNavTimelineRowChildren.Current.Select("TimelineBlock");
                                while (itNavBlocks.MoveNext())
                                {
                                    List<int> filtersForThisBlock = new List<int>();
                                    XPathNodeIterator itNavTimeRowChildrenFilters = itNavBlocks.Current.Select("TimelineFilter");
                                    while (itNavTimeRowChildrenFilters.MoveNext())
                                    {
                                        int filterID = Int32.Parse(itNavTimeRowChildrenFilters.Current.GetAttribute(idAttribute, itNavTimeRowChildrenFilters.Current.NamespaceURI));
                                        filtersForThisBlock.Add(filterID);
                                    }

                                    bool passFilter = false;

                                    for (int i = 0; i < filtersForThisBlock.Count; i++)
                                    {
                                        int checkAFilterID = filtersForThisBlock[i];
                                        if (andIDs.Count > 0 && andIDs.Contains(checkAFilterID))
                                        {
                                            passFilter = true;
                                            break;
                                        }
                                    }

                                    if (andIDs.Count == 0)
                                    {
                                        passFilter = true;
                                    }

                                    if (passFilter)
                                    {
                                        String blockName = itNavBlocks.Current.GetAttribute("name", itNavBlocks.Current.NamespaceURI);
                                        String blockBegin = itNavBlocks.Current.GetAttribute("start", itNavBlocks.Current.NamespaceURI);
                                        String blockEnd = itNavBlocks.Current.GetAttribute("end", itNavBlocks.Current.NamespaceURI);
                                        String blockColor = itNavBlocks.Current.GetAttribute("color", itNavBlocks.Current.NamespaceURI);

                                        timelineBlockNames.Add(blockName);
                                        timelineBlockBegins.Add(DateTime.Parse(blockBegin));
                                        timelineBlockEnds.Add(DateTime.Parse(blockEnd));
                                        timelineBlockColors.Add(Int32.Parse(blockColor, System.Globalization.NumberStyles.AllowHexSpecifier));

                                        timelineBlockRow.Add(row);
                                    }
                                }
                                zone++;
                                row++;
                            }
                        }
                        if (childAdded)
                        {
                            timelineBlockZones.Add(zone);
                        }
                    }
                }

                rowRange = (Double)timelineRows.Count;

                XPathNodeIterator itNavMilestones = navTimeline.Select("/Timeline/TimelineMilestone");
                while (itNavMilestones.MoveNext())
                {
                    String milestoneTime = itNavMilestones.Current.GetAttribute("time", itNavMilestones.Current.NamespaceURI);
                    timelineMilestoneTimes.Add(DateTime.Parse(milestoneTime));

                    String milestoneName = itNavMilestones.Current.GetAttribute("name", itNavMilestones.Current.NamespaceURI);
                    timelineMilestoneNames.Add(milestoneName);

                    String milestoneColor = itNavMilestones.Current.GetAttribute("color", itNavMilestones.Current.NamespaceURI);
                    timelineMilestoneColors.Add(Int32.Parse(milestoneColor, System.Globalization.NumberStyles.AllowHexSpecifier));
                }

                // The initial view port is to show 1 hour.
                currentDuration = beginDate.AddMinutes(scaleMinutes).Subtract(beginDate).TotalSeconds;
            }
            else // reset
            {
                ResetView();
            }
        }

        private void ResetView()
        {
            winChartViewer1.Chart = null;

            endDate = DateTime.Now.Date;
            beginDate = DateTime.Now.Date;
            currentDate = DateTime.Now.Date;
            dateRange = endDate.Subtract(beginDate).TotalSeconds;

            paletteButton.Enabled = false;
            filterButton.Enabled = false;

            // Get the TimelineRows
            timelineRows = new List<String>();
            timelineBlockNames = new List<String>();
            timelineBlockBegins = new List<DateTime>();
            timelineBlockEnds = new List<DateTime>();
            timelineBlockRow = new List<Double>();
            timelineBlockZones = new List<Double>();
            timelineBlockColors = new List<Int32>();
            timelineRowNames = new List<String>();
            timelineRowColors = new List<Int32>();
            timelineMilestoneTimes = new List<DateTime>();
            timelineMilestoneNames = new List<String>();
            timelineMilestoneColors = new List<Int32>();
        }

        private void getNavigator()
        {
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.CompParams = true;

            IXPathNavigable iNavParameters = controller.GetParametersForComponent(componentId);
            XPathNavigator navParameters = iNavParameters.CreateNavigator();

            String xpath = String.Format("ComponentParameters/Parameter/Parameter[@category='{0}' and @displayedName='{1}']", category, parameter);
            XPathNavigator navFilename = navParameters.SelectSingleNode(xpath);
            if (navFilename != null)
            {
                String filename = navFilename.GetAttribute("value", navFilename.NamespaceURI);
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

        private XPathNavigator validate(XmlDocument document)
        {
            XPathNavigator navValidatedXml = null;
            XmlReader schemaDatabase = this.controller.GetXSD("VisualizationTimeline.xsd");
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

        private void drawChart(WinChartViewer viewer)
        {
            DateTime viewPortStartDate = beginDate.AddSeconds(Math.Round(viewer.ViewPortLeft * dateRange));
            DateTime viewPortEndDate = viewPortStartDate.AddSeconds(Math.Round(viewer.ViewPortWidth * dateRange));
            TimeSpan hoursCalc = viewPortEndDate.Subtract(viewPortStartDate);
            int hours = (hoursCalc.Days * 24) + hoursCalc.Hours;
            viewPortEndDate = viewPortEndDate.AddMinutes(12 * hours); // hack to show hour labels
            Double axisLowerLimit = 0 + viewer.ViewPortTop * rowRange;
            Double axisUpperLimit = axisLowerLimit + viewer.ViewPortHeight * (rowRange);

            XYChart c = new XYChart(winChartViewer1.Width, this.winChartViewer1.Height, 0xf0f0ff, 0, 1);
            //c.setRoundedFrame(ChartDirector.Chart.CColor(BackColor));
			
			// Set the plotarea at (52, 60) and of size 520 x 192 pixels. Use white (ffffff) 
			// background. Enable both horizontal and vertical grids by setting their colors to 
			// grey (cccccc). Set clipping mode to clip the data lines to the plot area.
            int tryToBeMoreDynamic = this.Parent.Height - 200; // instead of hardcoding height
            c.setPlotArea(100, 50, 600, tryToBeMoreDynamic, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
			c.setClipping();

            c.yAxis().setWidth(2);
            c.yAxis().setDateScale(viewPortStartDate, viewPortEndDate);
            c.yAxis().setMultiFormat(ChartDirector.Chart.StartOfHourFilter(), "<*font=bold*>{value|w hh:nn}", ChartDirector.Chart.AllPassFilter(), "{value|w hh:nn}");

            c.xAxis().setWidth(2);
            c.xAxis().setLinearScale(axisLowerLimit, axisUpperLimit, 1);
            c.xAxis().setRounding(false, false);
            c.xAxis().setColors(0xcccccc, ChartDirector.Chart.Transparent);
            c.xAxis().setReverse();

            // swap the x and y axes to create a horziontal box-whisker chart
            c.swapXY();
            BoxWhiskerLayer layer = c.addBoxWhiskerLayer2(ChartDirector.Chart.CTime(timelineBlockBegins.ToArray()), ChartDirector.Chart.CTime(timelineBlockEnds.ToArray()));
            layer.setBoxColors(timelineBlockColors.ToArray());
            layer.addExtraField(timelineBlockNames.ToArray());
            layer.setXData(timelineBlockRow.ToArray());
            layer.setDataLabelFormat("{field0}");
            layer.setDataLabelStyle("Arial Bold").setAlignment(ChartDirector.Chart.Center);
            layer.setDataWidth(35);
            //layer.setDataGap(1);
            // TimelineRow parent
            Double rowParent = 0;
            Int32 colorParentIndex = 0;
            c.xAxis().addMark(0, 0x000000).setLineWidth(2);
            foreach (Double d in timelineBlockZones)
            {
                Double rowParentBegin = rowParent; // --0.5;
                Double rowParentEnd = d + 0.5; // --0.5;

                c.xAxis().addZone(rowParentBegin, rowParentEnd, timelineRowColors[colorParentIndex]);

                Mark xMark1 = c.xAxis().addMark(rowParentBegin, 0x000000, timelineRowNames[colorParentIndex]);
                xMark1.setLineWidth(2);
                xMark1.setFontStyle("Arial Bold Italic");

                // xMark1.setAlignment(ChartDirector.Chart.Center);

                //Mark xMark2 = c.xAxis().addMark(rowParentBegin, 0x000000);
                //xMark2.setLineWidth(2);

                rowParent = d;

                colorParentIndex++;
            }

            // TimelineRow child;
            double inset = 0.5;
            Int32 colorChildIndex = 0;
            foreach (String s in timelineRows)
            {
                Mark xMark = c.xAxis().addMark(colorChildIndex + inset, 0x000000, s);
                xMark.setLineWidth(0);

                colorChildIndex++;
            }

            // Milestones
            c.yAxis2().setDateScale(viewPortStartDate, viewPortEndDate);
            c.yAxis2().setColors(ChartDirector.Chart.Transparent, ChartDirector.Chart.Transparent, ChartDirector.Chart.Transparent);

            if (bShowMilestones)
            {
                Int32 timelineMilestoneCount = 0;
                int indentY = 40;
                double previousValue = 0.0;

                List<IndexedTime> indexedTimes = new List<IndexedTime>();
                for (int i = 0; i < timelineMilestoneTimes.Count; i++)
                {
                    indexedTimes.Add(new IndexedTime(timelineMilestoneTimes[i], i));
                }
                indexedTimes.Sort();

                int one = Int32.Parse("000000", System.Globalization.NumberStyles.AllowHexSpecifier);
                int two = Int32.Parse("888888", System.Globalization.NumberStyles.AllowHexSpecifier);
                int three = Int32.Parse("BB7700", System.Globalization.NumberStyles.AllowHexSpecifier);
                Int32 lineColor = one;

                foreach (IndexedTime time in indexedTimes)
                {
                    if (indentY > tryToBeMoreDynamic-10)
                    {
                        indentY = 40;
                    }

                    indentY += 20;

                    //Int32 lineColor = timelineMilestoneColors[time.Index];
                    Double value = ChartDirector.Chart.CTime(time.Time);

                    if (!value.Equals(previousValue))
                    {
                        if (lineColor == one)
                        {
                            lineColor = two;
                        }
                        else if (lineColor == two)
                        {
                            lineColor = three;
                        }
                        else if (lineColor == three)
                        {
                            lineColor = one;
                        }
                    }

                    Mark yMark = c.yAxis2().addMark(value, c.dashLineColor(lineColor, ChartDirector.Chart.DashLine), timelineMilestoneNames[time.Index]);

                    yMark.setPos(0, indentY);
                    yMark.setFontColor(lineColor);
                    yMark.setLineWidth(2);
                    yMark.setFontStyle("Arial Bold");
                    timelineMilestoneCount++;
                    previousValue = value;
                }
            }

            if (legendStrings != null)
            {
                LegendBox l = c.addLegend(5, 5);
                l.setFontSize(legendFontSize);
                l.setCols(legendStrings.Length);

                for (int i = 0; i < legendStrings.Length; i++)
                {
                    l.addKey(legendStrings[i], legendColors[i]);
                }
            }

            if (navigator != null && componentId >= 0 && legend.Items.Count == 0)
            {
                XPathNodeIterator iTLFilter = navigator.Select("//TimelineFilter");
                List<Int32> seenIDs = new List<Int32>();

                ImageList forLegend = new ImageList();
                foreach (XPathNavigator tlFilterItem in iTLFilter)
                {
                    string filterItemColor = tlFilterItem.GetAttribute(colorAttribute, tlFilterItem.NamespaceURI);

                    if (!forLegend.Images.ContainsKey(filterItemColor))
                    {
                        try
                        {
                            Bitmap bm = new Bitmap(16, 16);
                            Graphics g = Graphics.FromImage((Image)bm);
                            Brush forGraphics = new SolidBrush(HexToColor(filterItemColor));
                            
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.FillRectangle(forGraphics, new Rectangle(0, 0, bm.Width, bm.Height));
                            Icon icon = Icon.FromHandle(bm.GetHicon());
                            forGraphics.Dispose();
                            g.Dispose();
                            bm.Dispose();

                            forLegend.Images.Add(filterItemColor, icon);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error creating icon");
                        }
                    }
                }

                legend.SmallImageList = forLegend;

                foreach (XPathNavigator tlFilterItem in iTLFilter)
                {
                    string filterItemName = tlFilterItem.GetAttribute(nameAttribute, tlFilterItem.NamespaceURI);
                    string filterItemID = tlFilterItem.GetAttribute(idAttribute, tlFilterItem.NamespaceURI);
                    string filterItemType = tlFilterItem.GetAttribute(typeAttribute, tlFilterItem.NamespaceURI);
                    string filterItemColor = tlFilterItem.GetAttribute(colorAttribute, tlFilterItem.NamespaceURI);

                    int iParse = Int32.Parse(filterItemID);
                    if (!seenIDs.Contains(iParse))
                    {
                        legend.Items.Add(filterItemName, filterItemColor);
                        seenIDs.Add(iParse);
                    }
                }
            }

            //Int32 timelineMilestoneCount = 0;
            //foreach (String milestonName in timelineMilestoneNames)
            //{
            //    Double[] xData = { 0 };
            //    Double[] yData = { ChartDirector.Chart.CTime(timelineMilestoneTimes[timelineMilestoneCount]) };
            //    Int32 color = timelineMilestoneColors[timelineMilestoneCount];
            //    ScatterLayer sLayer = c.addScatterLayer(xData, yData, "", ChartDirector.Chart.TriangleSymbol, 13, color);
            //    timelineMilestoneCount++;

            //    // Add labels to the chart as an extra field
            //    sLayer.addExtraField(timelineMilestoneNames.ToArray());

            //    // Set the data label format to display the extra field
            //    sLayer.setDataLabelFormat("{field0}");
            //}

			viewer.Chart = c;
        }

        private void updateImageMap(WinChartViewer viewer)
        {
            // Include tool tip for the chart
            if (winChartViewer1.ImageMap == null)
            {
                //winChartViewer1.ImageMap = winChartViewer1.Chart.getHTMLImageMap("clickable", "",
                //"title='[{dataSetName}] {x|mmm dd, yyyy}: USD {value|2}'");
            }
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (hasFinishedInitialization)
            {
                // Set the view port based on the scroll bar
                winChartViewer1.ViewPortLeft = ((double)(hScrollBar1.Value - hScrollBar1.Minimum))
                    / (hScrollBar1.Maximum - hScrollBar1.Minimum);

                // Update the chart display without updating the image maps. (We can delay updating
                // the image map until scrolling is completed and the chart display is stable.)
                winChartViewer1.updateViewPort(true, false);
            }
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (hasFinishedInitialization)
            {
                // Set the view port based on the scroll bar
                winChartViewer1.ViewPortTop = ((double)(vScrollBar1.Value - vScrollBar1.Minimum))
                    / (vScrollBar1.Maximum - vScrollBar1.Minimum);

                // Update the chart display without updating the image maps. (We can delay updating
                // the image map until scrolling is completed and the chart display is stable.)
                winChartViewer1.updateViewPort(true, false);
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

            // Synchronize the vertical scroll bar with the WinChartViewer
            vScrollBar1.Enabled = winChartViewer1.ViewPortHeight < 1;
            vScrollBar1.LargeChange = (int)Math.Ceiling(winChartViewer1.ViewPortHeight *
                (vScrollBar1.Maximum - vScrollBar1.Minimum));
            vScrollBar1.SmallChange = (int)Math.Ceiling(vScrollBar1.LargeChange * 0.1);
            vScrollBar1.Value = (int)Math.Round(winChartViewer1.ViewPortTop *
                (vScrollBar1.Maximum - vScrollBar1.Minimum)) + vScrollBar1.Minimum;

            // Update chart and image map if necessary
            if (e.NeedUpdateChart)
                drawChart(winChartViewer1);
            if (e.NeedUpdateImageMap)
                updateImageMap(winChartViewer1);
        }

        private void winChartViewer1_Resize(object sender, EventArgs e)
        {
            if (controller != null && navigator != null && componentId > -1)
                winChartViewer1.updateViewPort(true, false);
        }
    }

    internal class IndexedTime : IComparable
    {
        private DateTime time;
        private Int32 index;
        public IndexedTime(DateTime t, Int32 i)
        {
            time = t;
            index = i;
        }

        public DateTime Time { get { return time; } }
        public Int32 Index { get { return index; } }


        public int CompareTo(object obj)
        {
            if (obj is IndexedTime)
            {
                return this.Time.CompareTo(((IndexedTime)obj).Time);
            }
            else
            {
                return -1;
            }
        }
    }
}
