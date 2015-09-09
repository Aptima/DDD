using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace AME.Views.View_Components
{
    public class Timeline : UserControl
    {
        private IXPathNavigable document;
        private XPathNavigator navigator;

        private int pixelsPerMinute = 2;

        private bool filtersBuilt = false;

        private Form dialogForm;
        private Form colorForm;

        private List<CheckedListBox> orLists;
        private List<CheckedListBox> andLists;

        private List<Int32> orIDs;
        private List<Int32> andIDs;

        private ListBox colorList;

        private XmlNode root;

        private String fileName = "TimelineColors.xml";

        private Font smallTextFont = new Font("Arial", 8);
        private Font textFont = new Font("Arial", 10);
        private SolidBrush textBrush = new SolidBrush(Color.Black);

        private int xInset = 20;
        private int yInset = 20;

        private int yHeight = 20;

        private int verticalDepth = 0;
        private int greatestHorizontalWidth = 0;
        private int stringCountSum = 0;
        private int stringWidthSum = 0;
        private int durationSum = 0;

        private int sizeX = 0;
        private int sizeY = 0;

        private int sizeAfterFilter = 0;

        private string rowXPath = "TimelineRow";
        private string blockXPath = "TimelineBlock";
        private string milestoneXPath = "TimelineMilestone";
        private string legendColumnXPath = "Legend/LegendColumn";
        private string legendItemXPath = "LegendItem";
        private string orFilterXPath = "FilterConfiguration/OrFilters/OrFilter";
        private string andFilterXPath = "FilterConfiguration/AndFilters/AndFilter";
        private string timelineFilterXPath = "TimelineFilter";

        private string nameAttribute = "name";
        private string idAttribute = "id";
        private string typeAttribute = "type";
        private string startAttribute = "start";
        private string endAttribute = "end";
        private string timeAttribute = "time";
        private string colorAttribute = "color";

        private int missionHours;
        private long missionStartMinutes;

        private Point filterPoint;

        public IXPathNavigable XmlDocument
        {
            get { return document; }
            set
            {
                if (value != null)
                {
                    document = value;
                    navigator = document.CreateNavigator();
                    filtersBuilt = false;
                    sizeAfterFilter = 0;

                    this.Refresh();
                }
            }
        }

        public int PixelsPerMinute
        {
            get { return pixelsPerMinute; }
            set { pixelsPerMinute = value; }
        }

        public Timeline()
        {
            InitializeComponent();

            this.Size = new Size(500, 500);
            this.BackColor = Color.White;

            orLists = new List<CheckedListBox>();
            andLists = new List<CheckedListBox>();

            orIDs = new List<Int32>();
            andIDs = new List<Int32>();

            InitializeXMLDocument();
        }

        private Color CheckForSavedColor(int id, string name, string type)
        {
            StreamReader xmlStream = LoadXML();

            XmlNode check = root.SelectSingleNode(
            "Color[@id = '" + id + "' and " +
                  "@name = '" + name + "' and" +
                  "@type = '" + type + "']");

            if (check != null)
            {
                string value = check.Attributes[colorAttribute].Value;

                Color test = (Color)new ColorConverter().ConvertFromString(value);

                UnloadXML(xmlStream);

                return test;
            }

            UnloadXML(xmlStream);

            return new Color();
        }

        private void BuildFilters(Graphics g)
        {
            if (filterPoint != null)
            {
                XPathNavigator timelineNavigator = navigator.SelectSingleNode("Timeline");

                Button filterButton = new Button();
                filterButton.Text = "Filter Asset-Task Schedule...";
                filterButton.BackColor = SystemColors.Control;
                filterButton.AutoSize = true;
                this.Controls.Add(filterButton);
                filterButton.Location = filterPoint;
                filterButton.UseVisualStyleBackColor = true;
                filterButton.Click += new EventHandler(filterButton_Click);

                Button paletteButton = new Button();
                paletteButton.Text = "Customize Colors...";
                paletteButton.BackColor = SystemColors.Control;
                paletteButton.AutoSize = true;
                this.Controls.Add(paletteButton);
                paletteButton.Location = new Point(filterButton.Location.X, filterButton.Location.Y + 30);
                paletteButton.UseVisualStyleBackColor = true;
                paletteButton.Click += new EventHandler(paletteButton_Click);

                sizeY = paletteButton.Location.Y + 50;

                //Button saveButton = new Button();
                //saveButton.Text = "Save Palette and Filter Settings";
                //saveButton.BackColor = SystemColors.Control;
                //saveButton.AutoSize = true;
                //this.Controls.Add(saveButton);
                //saveButton.Location = new Point(paletteButton.Location.X, paletteButton.Location.Y + 30);
                //saveButton.UseVisualStyleBackColor = true;

                dialogForm = new Form();
                dialogForm.AutoSize = true;
                dialogForm.FormClosed += new FormClosedEventHandler(dialogForm_FormClosed);

                colorForm = new Form();
                colorForm.AutoSize = true;
                colorForm.FormClosed += new FormClosedEventHandler(colorForm_FormClosed);
                colorList = new ListBox();
                colorList.Height = 100;
                colorList.Location = new Point(5, 10);
                colorList.Width = 150;

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
                XPathNodeIterator legendItemIterator = timelineNavigator.Select("Legend/LegendColumn/LegendItem");
                foreach (XPathNavigator legendItem in legendItemIterator)
                {
                    string filterItemName = legendItem.GetAttribute(nameAttribute, legendItem.NamespaceURI);
                    string filterItemID = legendItem.GetAttribute(idAttribute, legendItem.NamespaceURI);
                    string filterItemType = legendItem.GetAttribute(typeAttribute, legendItem.NamespaceURI);
                    string filterItemColor = legendItem.GetAttribute(colorAttribute, legendItem.NamespaceURI);
                    colorList.Items.Add(new TimelineListItem(filterItemName, Int32.Parse(filterItemID), filterItemType, filterItemColor));
                }

                colorList.SelectedIndexChanged += new EventHandler(colorList_SelectedIndexChanged);

                colorForm.Controls.Add(colorList);
                colorForm.Controls.Add(colorSwatch);
                colorForm.Controls.Add(colorPromptButton);

                GroupBox selectRadio = new GroupBox();

                selectRadio.Text = "Select a View";
                selectRadio.Size = new Size(10, 10);
                selectRadio.AutoSize = true;

                XPathNodeIterator orFilterIterator = timelineNavigator.Select(orFilterXPath);

                bool first = true;

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
                    radioX += (int)g.MeasureString(filterName, textFont).Width + 25;

                    selectRadio.Controls.Add(aFilterButton);

                    CheckedListBox filterListBox = new CheckedListBox();
                    filterListBox.Height = 100;
                    filterListBox.Location = new Point(5, 50);
                    filterListBox.Width = 150;
                    filterListBox.CheckOnClick = true;

                    GroupBox filterBy = new GroupBox();

                    filterBy.Text = "Filter by: " + filterType;
                    filterBy.Size = new Size(10, 10);
                    filterBy.AutoSize = true;
                    filterBy.Location = new Point(orX, 60);

                    TimelineButton filterByButton = new TimelineButton();
                    filterByButton.Text = "All " + filterType + "s";
                    filterByButton.AutoSize = true;
                    filterByButton.Location = new Point(5, 20);
                    filterByButton.ListBox = filterListBox;
                    filterByButton.Click += new EventHandler(filterByButton_Click);

                    if (first)
                    {
                        aFilterButton.Checked = true;
                        filterListBox.Enabled = true;
                        first = false;
                    }
                    else
                    {
                        filterListBox.Enabled = false;
                    }

                    aFilterButton.ListBox = filterListBox;

                    orLists.Add(filterListBox);

                    aFilterButton.Click += new EventHandler(aFilterButton_Click);

                    filterBy.Controls.Add(filterByButton);

                    dialogForm.Controls.Add(filterBy);

                    orX += 175;

                    XPathNodeIterator orFilterItemIterator = timelineNavigator.Select("//TimelineRow[@type='" + filterType + "']");
                    foreach (XPathNavigator orFilterItemNavigator in orFilterItemIterator)
                    {
                        string filterItemName = orFilterItemNavigator.GetAttribute(nameAttribute, orFilterNavigator.NamespaceURI);
                        string filterItemID = orFilterItemNavigator.GetAttribute(idAttribute, orFilterNavigator.NamespaceURI);
                        string filterItemType = orFilterItemNavigator.GetAttribute(typeAttribute, orFilterNavigator.NamespaceURI);

                        filterListBox.Items.Add(new TimelineListItem(filterItemName, Int32.Parse(filterItemID), filterItemType));

                    }

                    filterBy.Controls.Add(filterListBox);
                }

                first = true;

                radioX = 10;

                orX = 10;

                XPathNodeIterator andFilterIterator = timelineNavigator.Select(andFilterXPath);

                foreach (XPathNavigator andFilterNavigator in andFilterIterator)
                {
                    string filterName = andFilterNavigator.GetAttribute(nameAttribute, andFilterNavigator.NamespaceURI);
                    string filterType = andFilterNavigator.GetAttribute(typeAttribute, andFilterNavigator.NamespaceURI);

                    radioX += (int)g.MeasureString(filterName, textFont).Width + 25;

                    CheckedListBox filterListBox = new CheckedListBox();
                    filterListBox.Height = 100;
                    filterListBox.Location = new Point(5, 50);
                    filterListBox.Width = 150;
                    filterListBox.CheckOnClick = true;

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

                    // Legend xpath
                    XPathNodeIterator andFilterItemIterator = timelineNavigator.Select("Legend/LegendColumn[@type='" + filterType + "']/LegendItem");
                    foreach (XPathNavigator andFilterItemNavigator in andFilterItemIterator)
                    {
                        string filterItemName = andFilterItemNavigator.GetAttribute(nameAttribute, andFilterNavigator.NamespaceURI);
                        string filterItemID = andFilterItemNavigator.GetAttribute(idAttribute, andFilterNavigator.NamespaceURI);
                        string filterItemType = andFilterItemNavigator.GetAttribute(typeAttribute, andFilterNavigator.NamespaceURI);

                        filterListBox.Items.Add(new TimelineListItem(filterItemName, Int32.Parse(filterItemID), filterItemType));
                    }

                    filterBy.Controls.Add(filterListBox);
                }

                dialogForm.Controls.Add(selectRadio);
            }
        }

        private void colorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Refresh();
        }

        private Panel colorSwatch;

        private void colorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;

            TimelineListItem selectedItem = (TimelineListItem)box.SelectedItem;

            Color convert = Color.FromName(selectedItem.ColorString);

            Color check = CheckForSavedColor(selectedItem.ID, selectedItem.Name, selectedItem.Type);

            if (!check.IsEmpty)
            {
                colorSwatch.BackColor = check;
            }
            else if (convert.IsKnownColor) 
            {
                colorSwatch.BackColor = convert;
            }
        }

        private void colorPromptButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            DialogResult result = dialog.ShowDialog();

            TimelineListItem selectedItem = (TimelineListItem)colorList.SelectedItem;

            if (result.Equals(DialogResult.OK) && selectedItem != null)
            {
                colorSwatch.BackColor = dialog.Color;

                selectedItem.ColorString = "";

                StreamReader xmlStream = LoadXML();

                XmlNode check = root.SelectSingleNode(
                "Color[@id = '"+selectedItem.ID+"' and "+
                      "@name = '"+selectedItem.Name+"' and"+
                      "@type = '"+selectedItem.Type+"']");

                if (check == null)
                {
                    XmlElement colorElement = root.OwnerDocument.CreateElement("Color");

                    XmlAttribute IDAttr = root.OwnerDocument.CreateAttribute(idAttribute);
                    IDAttr.Value = selectedItem.ID.ToString();

                    XmlAttribute NameAttr = root.OwnerDocument.CreateAttribute(nameAttribute);
                    NameAttr.Value = selectedItem.Name;

                    XmlAttribute typeAttr = root.OwnerDocument.CreateAttribute(typeAttribute);
                    typeAttr.Value = selectedItem.Type;

                    XmlAttribute colorAttr = root.OwnerDocument.CreateAttribute(colorAttribute);
                    colorAttr.Value = new ColorConverter().ConvertToString(dialog.Color);

                    colorElement.Attributes.Append(IDAttr);
                    colorElement.Attributes.Append(NameAttr);
                    colorElement.Attributes.Append(typeAttr);
                    colorElement.Attributes.Append(colorAttr);

                    root.AppendChild(colorElement);
                }
                else
                {
                    check.Attributes["color"].Value = new ColorConverter().ConvertToString(dialog.Color);
                }

                UnloadXML(xmlStream);
            }
        }

        private StreamReader LoadXML()
        {
            FileInfo xmlFile = new FileInfo(fileName);
            StreamReader xmlStream = xmlFile.OpenText();
            XmlDocument document = new XmlDocument();
            document.Load(xmlStream);
            root = document.SelectSingleNode("/Colors");

            return xmlStream;
        }

        private void UnloadXML(StreamReader xmlStream)
        {
            xmlStream.Close();

            root.OwnerDocument.Save(fileName);
        }

        private void  paletteButton_Click(object sender, EventArgs e)
        {
            colorForm.ShowDialog();
        }

        private void InitializeXMLDocument()
        {
            FileInfo xmlFile = new FileInfo(fileName);

            if (!xmlFile.Exists)
            {
                XmlDocument doc = new XmlDocument();

                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                doc.AppendChild(declaration);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
                namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                // Create root element
                XmlElement colors = doc.CreateElement("Colors");

                // Add schema information to root.
                XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                schema.Value = "TimelineColors.xsd";
                colors.SetAttributeNode(schema);

                doc.AppendChild(colors);

                doc.Save(fileName);
            }
        }

        private void dialogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // collect IDs

            andIDs.Clear();
            orIDs.Clear();

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

            this.Refresh();
        }

        private void aFilterButton_Click(object sender, EventArgs e)
        {
            foreach (CheckedListBox list in orLists)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, false);
                }

                list.Enabled = false;
            }

            ((TimelineRadioButton)sender).ListBox.Enabled = true;
        }

        private void filterByButton_Click(object sender, EventArgs e)
        {
            CheckedListBox list = ((TimelineButton)sender).ListBox;

            if (list.Enabled)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    list.SetItemChecked(i, !list.GetItemChecked(i));
                }
            }
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            dialogForm.ShowDialog();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (navigator != null && !navigator.InnerXml.Equals(String.Empty))
            {
                Graphics g = e.Graphics;
                Pen thinBlack = new Pen(Color.Black, 1);
                Pen black = new Pen(Color.Black, 3);

                XPathNavigator timelineNavigator = navigator.SelectSingleNode("Timeline");

                String start = timelineNavigator.GetAttribute(startAttribute, timelineNavigator.NamespaceURI);
                String end = timelineNavigator.GetAttribute(endAttribute, timelineNavigator.NamespaceURI);
                DateTime startTime = DateTime.Parse(start);
                DateTime endTime = DateTime.Parse(end);
                //DateTime startTime = XmlConvert.ToDateTime(start, XmlDateTimeSerializationMode.RoundtripKind);
                //DateTime endTime = XmlConvert.ToDateTime(end, XmlDateTimeSerializationMode.RoundtripKind);

                missionStartMinutes = ConvertLongTicksToMinutes(startTime.Ticks);

                long missionLength = endTime.Ticks - startTime.Ticks; // 1 tick = 100 nanoseconds
                missionLength = ConvertLongTicksToSeconds(missionLength); // seconds;
                long missionMinutes = missionLength / 60;
                missionHours = (int)missionMinutes / 60;

                // Draw Rows and blocks
                verticalDepth = 0;
                greatestHorizontalWidth = 0;
                int timelineTop = yInset + 50;

                stringWidthSum = 0;
                stringCountSum = 0;
                durationSum = 0;

                ProcessRow(timelineNavigator, g, 0, false, timelineTop, timelineNavigator);

                int endTimeline = greatestHorizontalWidth + ((int)missionMinutes * PixelsPerMinute);

                String heading = "Mission Time";  // draw centered in the timeline
                int centerX = (endTimeline - xInset) / 2;
                centerX = CenterText(heading, centerX, g, textFont);
                g.DrawString(heading, textFont, textBrush, new PointF(centerX, 2));

                int timelineBottom = timelineTop + ((verticalDepth + 1) * yHeight);

                int yForLine = 0; // draw rows
                for (int i = 0; i <= verticalDepth + 1; i++)
                {
                    yForLine = timelineTop + (i * yHeight);
                    g.DrawLine(black, new Point(xInset, yForLine), new Point(endTimeline, yForLine));
                }

                // frame left and right
                g.DrawLine(black, new Point(xInset, timelineTop), new Point(xInset, timelineBottom));
                g.DrawLine(black, new Point(greatestHorizontalWidth, timelineTop), new Point(greatestHorizontalWidth, timelineBottom));

                // Draw Mission Time

                // How many pixels per hour
                int pixelsPerHour = pixelsPerMinute * 60;
                int pixelsPerQuarter = pixelsPerHour / 4;

                Pen blackDashed = new Pen(Color.Black, 3);
                blackDashed.DashPattern = new float[2] { 1, 1, };
                blackDashed.DashOffset = 1;
                int hourEndInset = 0;
                int endHours = missionHours + 1;
                for (int i = 0; i < endHours; i++)
                {
                    if (i + 1 < endHours)
                    {
                        hourEndInset = greatestHorizontalWidth + ((i + 1) * pixelsPerHour);
                        g.DrawLine(black, new Point(hourEndInset, timelineTop), new Point(hourEndInset, timelineBottom));

                        if (i+2 == endHours)
                        {
                            sizeX = hourEndInset;
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            hourEndInset -= pixelsPerQuarter;
                            g.DrawLine(blackDashed, new Point(hourEndInset, timelineTop), new Point(hourEndInset, timelineBottom));
                        }

                        hourEndInset -= pixelsPerQuarter;
                        String headingHours = "" + (startTime.Hour + i);
                        String headingMinutes = "" + startTime.Minute;

                        if ((startTime.Hour + i) < 10)
                        {
                            headingHours = "0" + headingHours; // e.g. 0900
                        }

                        if (startTime.Minute == 0)
                        {
                            headingMinutes += 0; // e.g. 0900
                        }

                        String total = headingHours + headingMinutes;

                        if (total.Length == 3)
                        {
                            g.DrawString(headingHours + headingMinutes, textFont, textBrush, new PointF(hourEndInset - 10, yInset + 2));
                        }
                        else
                        {
                            g.DrawString(headingHours + headingMinutes, textFont, textBrush, new PointF(hourEndInset - 17, yInset + 2));
                        }
                    }
                }

                verticalDepth = 0;

                ProcessRow(timelineNavigator, g, 0, true, timelineTop, timelineNavigator);

                // Draw Milestones
                XPathNodeIterator milestoneIterator = timelineNavigator.Select(milestoneXPath);
                foreach (XPathNavigator milestoneNavigator in milestoneIterator)
                {
                    string name = milestoneNavigator.GetAttribute(nameAttribute, milestoneNavigator.NamespaceURI);
                    string time = milestoneNavigator.GetAttribute(timeAttribute, milestoneNavigator.NamespaceURI);
                    int positionMilestone = GetPixelPositionOnTimeline(time, greatestHorizontalWidth, missionStartMinutes, PixelsPerMinute);

                    Point[] forFill = new Point[] { new Point(positionMilestone-9, timelineTop-9), 
                                                     new Point(positionMilestone, timelineTop),
                                                       new Point(positionMilestone+9, timelineTop-9) };
                    Point[] forDraw = new Point[] { new Point(positionMilestone-10, timelineTop-10), 
                                                     new Point(positionMilestone, timelineTop),
                                                       new Point(positionMilestone+10, timelineTop-10) };


                    g.FillPolygon(Brushes.LightGreen, forFill);
                    g.DrawPolygon(thinBlack, forDraw);

                    int test = (int)(g.MeasureString(name, smallTextFont).Width / 2);

                    int mileStoneCentered = CenterText(name, positionMilestone, g, smallTextFont);
                    g.DrawString(name, smallTextFont, textBrush, new Point(mileStoneCentered, timelineTop - 23));
                }

                // Draw Legends

                int xLegend = xInset;
                int yLegend;

                XPathNodeIterator legendColumnIterator = timelineNavigator.Select(legendColumnXPath);
                foreach (XPathNavigator legendColumnNavigator in legendColumnIterator)
                {
                    string columnName = legendColumnNavigator.GetAttribute(nameAttribute, legendColumnNavigator.NamespaceURI);

                    yLegend = timelineBottom + 25;

                    g.DrawString(columnName, textFont, textBrush, new Point(xLegend-5, yLegend));

                    int longestItemWidth = 0;

                    XPathNodeIterator legendItemIterator = legendColumnNavigator.Select(legendItemXPath);
                    foreach (XPathNavigator legendItemNavigator in legendItemIterator)
                    {
                        string itemName = legendItemNavigator.GetAttribute(nameAttribute, legendItemNavigator.NamespaceURI);
                        string itemID = legendItemNavigator.GetAttribute(idAttribute, legendItemNavigator.NamespaceURI);
                        string itemType = legendItemNavigator.GetAttribute(typeAttribute, legendItemNavigator.NamespaceURI);

                        Color check = CheckForSavedColor(Int32.Parse(itemID), itemName, itemType);

                        if (check.IsEmpty)
                        {
                            string color = legendItemNavigator.GetAttribute(colorAttribute, legendItemNavigator.NamespaceURI);

                            check = Color.FromName(color);
                        }
                        yLegend += 25;

                        g.FillRectangle(new SolidBrush(check), new Rectangle(xLegend, yLegend, 10, 10));

                        g.DrawString(itemName, textFont, textBrush, new Point(xLegend + 15, yLegend - 3));

                        int itemWidth = (int)g.MeasureString(itemName, textFont).Width;
                        if (itemWidth > longestItemWidth)
                        {
                            longestItemWidth = itemWidth;
                        }
                    }

                    xLegend += longestItemWidth + 15;
                }

                filterPoint = new Point(xLegend + 20, timelineBottom + 25);

                if (!filtersBuilt)
                {
                    double avgPixels = stringWidthSum / stringCountSum;
                    double avgMinutes = durationSum / stringCountSum;

                    double mypixPerMinute = avgPixels / avgMinutes;

                    this.PixelsPerMinute = (int)mypixPerMinute;

                    BuildFilters(g);
                    filtersBuilt = true;
                    sizeAfterFilter++;

                    this.Refresh();
                }

                if (sizeAfterFilter == 2)
                {
                    this.Size = new Size(sizeX, sizeY);
                    sizeAfterFilter++;
                }
                else if (sizeAfterFilter == 1)
                {
                    sizeAfterFilter++;
                }
            }
        }

        public static int CenterText(string text, int xPosition, Graphics g, Font textFont)
        {
            string lower = text.ToLower();
            return xPosition - (int)(g.MeasureString(lower, textFont).Width / 2);
        }

        public static int GetPixelPositionOnTimeline(string dateTimeXML, int leftEdgeOfTimelineInPixels, long p_missionStartMinutes, int p_pixelsPerMinute)
        {
            DateTime dateTime = DateTime.Parse(dateTimeXML);
            //DateTime dateTime = XmlConvert.ToDateTime(dateTimeXML, XmlDateTimeSerializationMode.RoundtripKind);

            long minutes = ConvertLongTicksToMinutes(dateTime.Ticks);

            int position = leftEdgeOfTimelineInPixels + ((int)(minutes - p_missionStartMinutes) * p_pixelsPerMinute);

            return position;
        }

        public static long ConvertLongTicksToMinutes(long time) { return time / 600000000; }

        public static long ConvertLongTicksToSeconds(long time) { return time / 10000000; }

        public void ProcessRow(XPathNavigator rowNavigator, Graphics g, int horizontalDepth, bool blocks, int timelineTop, XPathNavigator timelineNavigator)
        {
            // Draw current row

            bool orCheck = true;

            if (rowNavigator.Name == rowXPath)
            {
                int topRow = timelineTop + (verticalDepth * yHeight);

                orCheck = CheckOr(rowNavigator);

                bool andCheck = CheckAnd(rowNavigator);

                if (orCheck && andCheck)
                {
                    if (blocks == false)
                    {
                        string name = rowNavigator.GetAttribute(nameAttribute, rowNavigator.NamespaceURI);
                        int horizontalPoint = horizontalDepth * xInset;
                        g.DrawString(name, textFont, textBrush, horizontalPoint, topRow);

                        int tempStringWidth = (int)g.MeasureString(name, textFont).Width;

                        stringWidthSum += tempStringWidth;
                        stringCountSum++;

                        int sumWidth = horizontalPoint + tempStringWidth;
                        if (sumWidth > greatestHorizontalWidth)
                        {
                            greatestHorizontalWidth = sumWidth;
                        }
                    }
                    else
                    {
                        //Draw blocks
                        XPathNodeIterator blockIterator = rowNavigator.Select(blockXPath);
                        foreach (XPathNavigator blockNavigator in blockIterator)
                        {
                            String name = blockNavigator.GetAttribute(nameAttribute, blockNavigator.NamespaceURI);

                            String start = blockNavigator.GetAttribute(startAttribute, blockNavigator.NamespaceURI);
                            String end = blockNavigator.GetAttribute(endAttribute, blockNavigator.NamespaceURI);

                            DateTime startTime = DateTime.Parse(start);
                            DateTime endTime = DateTime.Parse(end);
                            //DateTime startTime = XmlConvert.ToDateTime(start, XmlDateTimeSerializationMode.RoundtripKind);
                            //DateTime endTime = XmlConvert.ToDateTime(end, XmlDateTimeSerializationMode.RoundtripKind);

                            int positionStart = GetPixelPositionOnTimeline(start, greatestHorizontalWidth, missionStartMinutes, PixelsPerMinute);

                            long startMinutes = ConvertLongTicksToMinutes(startTime.Ticks);
                            long endMinutes = ConvertLongTicksToMinutes(endTime.Ticks);

                            long startPixels = (int)(startMinutes * PixelsPerMinute);
                            long endPixels = ((int)endMinutes * PixelsPerMinute);
                            int widthPixels = (int)endPixels - (int)startPixels;

                            durationSum += (int)(endMinutes - startMinutes);

                            // look under the filters for a mapping in the legends to find out the color of the block
                            bool foundColor = false;

                            XPathNodeIterator filterIterator = blockNavigator.Select(timelineFilterXPath);
                            foreach (XPathNavigator filterNavigator in filterIterator)
                            {
                                string id = filterNavigator.GetAttribute(idAttribute, filterNavigator.NamespaceURI);

                                XPathNodeIterator legendItemIterator = timelineNavigator.Select(this.legendColumnXPath + "/" + legendItemXPath + "[@id = '" + id + "']");
                                foreach (XPathNavigator legendItemNavigator in legendItemIterator)
                                {
                                    string itemName = legendItemNavigator.GetAttribute(nameAttribute, legendItemNavigator.NamespaceURI);
                                    string itemID = legendItemNavigator.GetAttribute(idAttribute, legendItemNavigator.NamespaceURI);
                                    string itemType = legendItemNavigator.GetAttribute(typeAttribute, legendItemNavigator.NamespaceURI);

                                    Color check = CheckForSavedColor(Int32.Parse(itemID), itemName, itemType);

                                    if (check.IsEmpty)
                                    {
                                        string color = legendItemNavigator.GetAttribute(colorAttribute, legendItemNavigator.NamespaceURI);

                                        check = Color.FromName(color);
                                    }

                                    g.FillRectangle(new SolidBrush(check), new Rectangle(positionStart, topRow + 2, widthPixels, yHeight - 3));

                                    foundColor = true;
                                }
                            }

                            if (!foundColor)
                            {
                                g.FillRectangle(Brushes.Aqua, new Rectangle(positionStart, topRow + 2, widthPixels, yHeight - 3));
                            }

                            int placeText = CenterText(name, positionStart + (int)(widthPixels / 2), g, textFont);

                            g.DrawString(name, textFont, textBrush, new Point(placeText, topRow + 2));
                        }
                    }
                }
            }

            //bool childCheck = true;

            //if (childCheck)
            //{
                // look for children rows
                XPathNodeIterator rowIterator = rowNavigator.Select(rowXPath);

                if (rowIterator.Count > 0 && orCheck)
                {
                    horizontalDepth++;
                }

                foreach (XPathNavigator childRowNavigator in rowIterator)
                {
                    if (orCheck)
                    {
                        verticalDepth++;
                    }
                    ProcessRow(childRowNavigator, g, horizontalDepth, blocks, timelineTop, timelineNavigator);
                }
            //}
        }

        private bool CheckAnd(XPathNavigator rowNavigator)
        {
            if (andIDs.Count == 0)
            {
                return true;
            }

            bool check = false;

            XPathNodeIterator descendants = rowNavigator.Select("./TimelineBlock/TimelineFilter");
            foreach (XPathNavigator descendant in descendants)
            {
                String descendantIDString = descendant.GetAttribute(idAttribute, descendant.NamespaceURI);

                if (descendantIDString != null && descendantIDString.Length > 0)
                {
                    int intID = Int32.Parse(descendantIDString);

                    if (andIDs.Contains(intID))
                    {
                        check = true;
                    }
                }
            }

            return check;
        }

        private bool CheckOr(XPathNavigator rowNavigator)
        {
            if (orIDs.Count == 0)
            {
                return true;
            }

            bool check = false;

            XPathNodeIterator ancestors = rowNavigator.SelectAncestors(XPathNodeType.Element, true);

            foreach (XPathNavigator ancestor in ancestors)
            {
                String ancestorIDString = ancestor.GetAttribute(idAttribute, ancestor.NamespaceURI);

                if (ancestor.Name.Equals("TimelineRow") && ancestorIDString != null && ancestorIDString.Length > 0)
                {
                    int intID = Int32.Parse(ancestorIDString);

                    if (orIDs.Contains(intID))
                    {
                        check = true;
                    }
                }
            }

            XPathNodeIterator descendants = rowNavigator.Select(".//TimelineRow | .//TimelineFilter");
            foreach (XPathNavigator descendant in descendants)
            {
                String descendantIDString = descendant.GetAttribute(idAttribute, descendant.NamespaceURI);

                if (descendantIDString != null && descendantIDString.Length > 0)
                {
                    int intID = Int32.Parse(descendantIDString);

                    if (orIDs.Contains(intID))
                    {
                        check = true;
                    }
                }
            }
            
            return check;
        }

        private bool CheckChildren(XPathNavigator rowNavigator)
        {
            if (orIDs.Count == 0 && andIDs.Count == 0)
            {
                return true;
            }

            bool check = false;

            //XPathNodeIterator ancestors = rowNavigator.SelectAncestors(XPathNodeType.Element, true);

            //foreach (XPathNavigator ancestor in ancestors)
            //{
            //    String ancestorIDString = ancestor.GetAttribute(idAttribute, ancestor.NamespaceURI);

            //    if (ancestor.Name.Equals("TimelineRow") && ancestorIDString != null && ancestorIDString.Length > 0)
            //    {
            //        int intID = Int32.Parse(ancestorIDString);

            //        if (orIDs.Contains(intID) && andIDs.Contains(intID))
            //        {
            //            check = true;
            //        }
            //    }
            //}

            XPathNodeIterator descendants = rowNavigator.Select(".//TimelineRow | .//TimelineFilter");
            foreach (XPathNavigator descendant in descendants)
            {
                String descendantIDString = descendant.GetAttribute(idAttribute, descendant.NamespaceURI);

                if (descendantIDString != null && descendantIDString.Length > 0)
                {
                    int intID = Int32.Parse(descendantIDString);

                    if (orIDs.Count > 0 && andIDs.Count == 0)
                    {
                        if (orIDs.Contains(intID))
                        {
                            check = true;
                        }
                    }
                    else if (orIDs.Count > 0 && andIDs.Count > 0)
                    {
                        if (orIDs.Contains(intID) && andIDs.Contains(intID))
                        {
                            check = true;
                        }
                    }
                }
            }
            return check;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Timeline
            // 
            this.Name = "Timeline";
            this.ResumeLayout(false);

        }
    }

    internal class TimelineRadioButton : RadioButton
    {
        private CheckedListBox myBox;

        public CheckedListBox ListBox
        {
            get { return myBox; }
            set { myBox = value; }
        }

        public TimelineRadioButton() : base() {}
    }

    internal class TimelineButton : Button
    {
        private CheckedListBox myBox;

        public CheckedListBox ListBox
        {
            get { return myBox; }
            set { myBox = value; }
        }

        public TimelineButton() : base() { }
    }

    internal class TimelineListItem
    {
        private string name, type, colorString;

        private int id;

        public string Name
        {
            get { return name; }
        }

        public string Type
        {
            get { return type; }
        }

        public int ID
        {
            get { return id; }
        }

        public String ColorString
        {
            get { return colorString; }
            set { colorString = value; }
        }

        public TimelineListItem(String p_Name, int p_ID, String p_Type)
        {
            this.name = p_Name;
            this.id = p_ID;
            this.type = p_Type;
        }

        public TimelineListItem(String p_Name, int p_ID, String p_Type, String p_ColorString)
            : this(p_Name, p_ID, p_Type)
        {
            this.name = p_Name;
            this.id = p_ID;
            this.type = p_Type;
            this.colorString = p_ColorString;
        }

        public override String ToString() { return name; }
    }
}
