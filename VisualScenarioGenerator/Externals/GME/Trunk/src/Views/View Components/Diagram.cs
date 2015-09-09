/*
 *  Copyright © Northwoods Software Corporation, 1998-2006. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AME.Controllers;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Data;
using AME.Model;
using Northwoods.Go;
using System.Xml.XPath;
using System.Security.Cryptography;
using Mvp.Xml.Common.XPath;
using AME.Nodes;
using System.Text;
using AME;
using System.Xml.Xsl;
using System.Reflection;
using AME.Views.View_Components.CoordinateTransform;
using System.Collections.Specialized;

namespace AME.Views.View_Components
{
    public class Diagram : GoView, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private int displayID;

        public int DisplayID
        {
            get { return displayID; }
            set
            {
                displayID = value;
            }
        }

        private int m_RootID = -1;

        public int RootID
        {
            get { return m_RootID; }
            set { m_RootID = value; }
        }

        private String diagramName;  // this is the type/kind of diagram this is
        // links are recorded using this name

        public String DiagramName
        {
            get { return diagramName; }
            set { diagramName = value; }
        }

        protected XslCompiledTransform transform;
        private String xsl;

        /// <summary>
        /// Sets an XSL for the Diagram.  Please set a Controller for this Diagram before setting this property!
        /// </summary>
        public String Xsl
        {
            get
            {
                return xsl;
            }
            set
            {
                if (value != null)
                {
                    transform = new XslCompiledTransform();
                    try
                    {
                        XmlReader xslreader = myController.GetXSL(value);
                        transform.Load(xslreader);
                        xslreader.Close();
                        xsl = value;
                    }
                    catch (Exception ex)
                    {
                        transform = null;
                        xsl = null;
                        MessageBox.Show(ex.Message, "Failed to load transform - is a controller set? (Diagram)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }

        private uint m_level = 0;
        public uint Level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        //private Dictionary<String, Bitmap> nodeImages;

        private IController myController;

        private String fileName, IDAttribute, NameAttribute, TypeAttribute, LinkIDAttribute;

        public String FileName
        {
            get { return fileName; }
        }

        #region IViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                if (value != null)
                {
                    myController = value;
                    InitializeXmlFile();
                }
            }
        }

        private XmlDocument currentDoc;

        public XmlDocument CurrentDoc
        {
            get { return currentDoc; }
        }

        private XmlNode root;

        public XmlNode Root
        {
            get { return root; }
        }

        private Point docPnt;

        public Point DocPnt
        {
            get { return docPnt; }
        }

        Boolean fillPolygon = true;
        Boolean outlinePolygon = false;

        public Boolean FillPolygon
        {
            get { return fillPolygon; }
            set { fillPolygon = value; }
        }

        public Boolean OutlinePolygon
        {
            get { return outlinePolygon; }
            set { outlinePolygon = value; }
        }

        private float labelFontSize = 0.0f;
        public float LabelFontSize
        {
            get { return labelFontSize; }
            set { labelFontSize = value; }
        }

        // use identity as default
        ICoordinateTransform m_coordinateTransformer = new IdentityCoordinateTransform();
        public ICoordinateTransform CoordinateTransformer
        {
            get { return m_coordinateTransformer; }
            set { m_coordinateTransformer = value; }
        }

        private string crc = "";
        private ImageList globalIL;
        private IndexingXPathNavigator pointNav;
        private XPathExpression categories = XPathExpression.Compile("ComponentParameters/Parameter/Parameter");
        private String categoryImage = "Image";
        private String categoryLocation = "Location";
        private String categoryColor = "Color";
        private String displayedNameAttr = "displayedName";
        private String categoryAttr = "category";

        private static int iconHeight = 20;
        private static int iconWidth = 20;

        public static int IconWidth
        {
            get { return iconWidth; }
            set { iconWidth = value; }
        }

        public static int IconHeight
        {
            get { return iconHeight; }
            set { iconHeight = value; }
        }

        private void CheckImageList()
        {
            if (myController != null && diagramName != null && globalIL == null)
            {
                globalIL = new ImageList();
                globalIL.ColorDepth = ColorDepth.Depth32Bit;

                Dictionary<String, Bitmap> typeImage = myController.GetIcons();
                Bitmap image;
                globalIL.ImageSize = new Size(iconWidth, iconHeight);

                foreach (String k in typeImage.Keys)
                {
                    image = typeImage[k];
                    if (image.Width > 48 || image.Height > 48)
                    {
                        globalIL.ImageSize = new Size(48, 48);
                    }
                    globalIL.Images.Add(k, image);
                }

                this.ImageList = globalIL;
            }
        }

        Boolean updating = false; // set this to true when we open the diagrams.xml file 
        // so we won't try to read it when it is being updated

        public void UpdateViewComponent()
        {
            if (!updating)
            {
                CheckImageList();
                LoadDiagram();
            }
        }

        /// <summary>
        /// Removes all nodes and links from this Diagram
        /// </summary>
        public void Clear()
        {
            this.DeleteCache(this.RootID); // clean up Diagrams.xml file

            // delete all nodes, and all of their links
            GoLayerCollectionObjectEnumerator all = this.Document.GetEnumerator();
            foreach (GoObject item in all)
            {
                if (item is DiagramNode)
                {
                    DiagramNode cast = (DiagramNode)item;

                    this.myController.DeleteLink(cast.LinkID);

                    foreach (DiagramLink link in cast.Links)
                    {
                        this.myController.DeleteLink(link.LinkID);
                    }
                }
                else if (item is HasLinkID)
                {
                    this.myController.DeleteLink(((HasLinkID)item).LinkID);
                }
            }
        }

        private void LoadDiagram()
        {
            if (diagramName == null)
            {
                return;
            }

            //go to controller, get info from DB  
            DrawingUtility.SuspendDrawing(this);

            IXPathNavigable inav;

            if (displayID != RootID) // requires an additional level; base is now one node below root
            {
                ComponentOptions compOptions = new ComponentOptions();

                if (m_level != 0) // use level if specified, otherwise use default.
                {
                    compOptions.LevelDown = m_level;
                }
                else
                {
                    compOptions.LevelDown = 3;
                }

                compOptions.LinkParams = true;
                compOptions.CompParams = true;
                compOptions.InstanceUseClassName = true;
                inav = this.myController.GetComponentAndChildren(this.RootID, this.displayID, DiagramName, compOptions);
            }
            else
            {
                ComponentOptions compOptions = new ComponentOptions();

                if (m_level != 0) // use level if specified, otherwise use default.
                {
                    compOptions.LevelDown = m_level;
                }
                else
                {
                    compOptions.LevelDown = 2;
                }

                compOptions.LinkParams = true;
                compOptions.CompParams = true;
                compOptions.InstanceUseClassName = true;
                inav = this.myController.GetComponentAndChildren(this.RootID, this.RootID, DiagramName, compOptions);
            }

            XPathNavigator nav = inav.CreateNavigator();

            if (transform != null)
            {
                XmlDocument newDocument = new XmlDocument();
                using (XmlWriter writer = newDocument.CreateNavigator().AppendChild())
                {
                    transform.Transform(inav, (XsltArgumentList)null, writer);
                }
                nav = newDocument.CreateNavigator();
            }

            // Perform a CRC checksum on the data coming back, only update if it has changed
            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //Byte[] bs = System.Text.Encoding.UTF8.GetBytes(nav.OuterXml);
            //bs = x.ComputeHash(bs);
            //System.Text.StringBuilder s = new System.Text.StringBuilder();
            //foreach (Byte b in bs)
            //{
            //    s.Append(b.ToString("x2").ToLower());
            //}
            //String newCrc = s.ToString();
            //if (!crc.Equals(newCrc)) // otherwise update with the new data
            {
                //crc = newCrc;

                this.Document.Clear();

                // filter based on displayID
                bool selectOffRoot = true;

                if (displayID != RootID)
                {
                    // set iterator to display ID
                    XPathNodeIterator iterator = nav.Select("//Component[@" + IDAttribute + "='" + displayID + "']");

                    if (iterator.Count == 1)
                    {
                        iterator.MoveNext();
                        nav = iterator.Current;
                        selectOffRoot = false; // different xpaths below
                    }
                }

                if (nav.HasChildren)
                {
                    Dictionary<int, DiagramNode> docNodes = new Dictionary<int, DiagramNode>();

                    XPathDocument pointReadDoc = new XPathDocument(fileName);
                    pointNav = new IndexingXPathNavigator(pointReadDoc.CreateNavigator());
                    pointNav.AddKey("IDKey", "/Nodes/Node", "concat(@ID, '||', @RootID, '||', @DiagramName)");
                    pointNav.BuildIndexes();

                    XPathNodeIterator listOfNodes = null;

                    if (selectOffRoot)
                    {
                        listOfNodes = nav.Select("/Components/Component/Component"); // path relative to root
                    }
                    else
                    {
                        listOfNodes = nav.Select("Component"); // path relative to any component
                    }

                    //bool promptOnce = true;
                    int defaultNodePositionX = 20;
                    int defaultNodePositionY = 20;

                    // for save information
                    StreamReader tempStream = LoadDiagramsXML();

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        string componentType = nodeNav.GetAttribute(TypeAttribute, nodeNav.NamespaceURI);

                        if (allowedTypes.Count > 0)  // filter if types are provided
                        {
                            if (allowedTypes.Contains(componentType))
                            {
                                CreateNode(nodeNav, docNodes, ref defaultNodePositionX, ref defaultNodePositionY);
                            }
                        }
                        else
                        {
                            CreateNode(nodeNav, docNodes, ref defaultNodePositionX, ref defaultNodePositionY);
                        }
                    }

                    UnloadDiagramsXML(tempStream, true);

                    int toID, fromID, linkID;
                    String linkString;

                    //now, form links.

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        fromID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));

                        if (nodeNav.HasChildren)
                        {
                            XPathNodeIterator listOfChildren = nodeNav.Select("Component");

                            foreach (XPathNavigator childNav in listOfChildren)
                            {
                                toID = Int32.Parse(childNav.GetAttribute(IDAttribute, childNav.NamespaceURI));

                                linkString = childNav.GetAttribute(LinkIDAttribute, childNav.NamespaceURI);

                                if (linkString.Length > 0)
                                {

                                    linkID = Int32.Parse(linkString);
                                }
                                else
                                {
                                    linkID = -1;
                                }

                                string componentType = childNav.GetAttribute(TypeAttribute, childNav.NamespaceURI);

                                if (allowedTypes.Count > 0)  // filter if types are provided
                                {
                                    if (allowedTypes.Contains(componentType))
                                    {
                                        this.CreateLink(childNav, linkID, docNodes, fromID, toID);
                                    }
                                }
                                else
                                {
                                    this.CreateLink(childNav, linkID, docNodes, fromID, toID);
                                }
                            }
                        }
                    }
                }
            }

            // ensure if we're at a fixed size that the objects fit inside the document
            // expand if needed, e.g. negative coordinates
            if (this.Document.FixedSize == true && this.BackgroundImage != null)
            {
                RectangleF newBounds = this.Document.ComputeBounds();
                Boolean tlChanged = false;

                int newWidth = this.BackgroundImage.Width, newHeight = this.BackgroundImage.Height;

                int newBoundsBottom = (int)(newBounds.Y+newBounds.Height);
                int newBoundsRight = (int)(newBounds.X + newBounds.Width);

                if (newBounds.X < 0)
                {
                    // we've expanded
                    newWidth = newWidth + (int)(0 - newBounds.X);
                    tlChanged = true;
                }

                if (newBoundsRight > this.BackgroundImage.Width)
                {
                    // we've expanded
                    newWidth = newWidth + (int)(newBoundsRight - this.BackgroundImage.Width);
                    tlChanged = true;
                }

                if (newBounds.Y < 0)
                {
                    newHeight = newHeight + (int)(0 - newBounds.Y);
                    tlChanged = true;
                }


                if (newBoundsBottom > this.BackgroundImage.Height)
                {
                    newHeight = newHeight + (int)(newBoundsBottom - this.BackgroundImage.Height);
                    tlChanged = true;
                }

                if (tlChanged)
                {
                    if (newBounds.X < 0)
                    {
                        this.Document.TopLeft = new PointF(newBounds.X, this.Document.TopLeft.Y);
                    }
                    else
                    {
                        this.Document.TopLeft = new PointF(0, this.Document.TopLeft.Y);
                    }

                    if (newBounds.Y < 0)
                    {
                        this.Document.TopLeft = new PointF(this.Document.TopLeft.X, newBounds.Y);
                    }
                    else
                    {
                        this.Document.TopLeft = new PointF(this.Document.TopLeft.X, 0);
                    }
                     
                    this.Document.Size = new SizeF(newWidth, newHeight);
                    this.DocPosition = this.LimitDocPosition(this.DocPosition);
                }
                else
                {
                    this.Document.TopLeft = new PointF(0, 0);
                    this.Document.Size = this.BackgroundImage.Size;
                    this.DocPosition = this.LimitDocPosition(this.DocPosition);
                }
            }
            DrawingUtility.ResumeDrawing(this);
        }
    
        private void CreateLink(XPathNavigator childNav, int linkID, Dictionary<int, DiagramNode> docNodes, int fromID, int toID)
        {
            DiagramLink newLink;
            DiagramNode toNode, fromNode;
            String firstParam;

            newLink = new DiagramLink(linkID);
            newLink.ToArrow = true;
            newLink.PenWidth = 2;

            XPathNavigator pnav = childNav.SelectSingleNode("LinkParameters/Parameter/Parameter");
            if (pnav != null)
            {
                firstParam = "[ " + pnav.GetAttribute("value", pnav.NamespaceURI) + " ]";

                GoText label = new GoText();
                label.Text = firstParam;
                label.Selectable = false;
                newLink.MidLabel = label;
            }

            if (docNodes.ContainsKey(toID) && docNodes.ContainsKey(fromID))
            {
                toNode = docNodes[toID];
                fromNode = docNodes[fromID];

                newLink.ToPort = toNode.Port;
                newLink.FromPort = fromNode.Port;

                this.Document.Add(newLink);
            }
        }

        private void CreateNode(XPathNavigator nodeNav, Dictionary<int, DiagramNode> docNodes, ref int startX, ref int startY)
        {
            // base data
            int nodeID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));
            String nodeType = nodeNav.GetAttribute(TypeAttribute, nodeNav.NamespaceURI);
            String nodeName = nodeNav.GetAttribute(NameAttribute, nodeNav.NamespaceURI);

            String linkString = nodeNav.GetAttribute(LinkIDAttribute, nodeNav.NamespaceURI);
            int linkID;

            if (linkString.Length > 0)
            {

                linkID = Int32.Parse(linkString);
            }
            else
            {
                linkID = -1;
            }

            // Trim names
            nodeName = nodeName.Trim();

            List<Function> nodeFunctions = FunctionHelper.GetFunctions(nodeNav);

            List<ParsedFunction> parsedFunctions = FunctionHelper.ParseFunctions(nodeFunctions);

            // X and Y that are passed in are 'default' x if no Point exists in the file OR in the DB
            // starts at 20, 20 

            //start
            Dictionary<String, String> parameterMap = new Dictionary<String, String>();
           
            XPathNodeIterator parameters = nodeNav.Select(categories);

            String name, category, value;
            while (parameters.MoveNext())
            {
                category = parameters.Current.GetAttribute(categoryAttr, String.Empty);
                if (category.Equals(categoryImage) || category.EndsWith(categoryLocation) || category.Equals(categoryColor))
                {
                    name = parameters.Current.GetAttribute(displayedNameAttr, String.Empty);
                    value = parameters.Current.GetAttribute(ConfigFileConstants.Value, String.Empty);
                    parameterMap.Add(String.Concat(category, SchemaConstants.ParameterDelimiter, name), value);
                }
            }
            //end

            Boolean xyFromDB = false;

            // first do the MoveAction and replace XY with DB XY if it exists
            ParsedFunction moveActionDBInsert = parsedFunctions.Find(delegate(ParsedFunction pf) { return pf.FunctionName == "MoveAction" && pf.ActionIdentifier == "InsertXYIntoDB"; });
            if (moveActionDBInsert != null)
            {
                List<String> moveActionValues = moveActionDBInsert.ActionValues;

                if (moveActionValues.Count >= 2)
                {
                    String x = parameterMap[moveActionValues[0]];
                    String y = parameterMap[moveActionValues[1]];

                    startX = (int)float.Parse(x);
                    startY = (int)float.Parse(y);

                    xyFromDB = true;
                }
            }

            ParsedFunction vrPF = parsedFunctions.Find(delegate(ParsedFunction pf) { return pf.FunctionName == "VisualRepresentation"; });

            if (vrPF != null)
            {
                String functionName = vrPF.FunctionName;
                String actionIdentifier = vrPF.ActionIdentifier;
                List<string>  actionValues = vrPF.ActionValues;

                String imagePath = String.Empty;

                switch (actionIdentifier)
                {
                    case "Image":      // parse VisualRepresentation=Image@Image.Image File         
                        if (actionValues.Count >= 1)
                        {
                            String paramValueForImage = parameterMap[actionValues[0]];

                            try
                            {
                                imagePath = FunctionHelper.UpdateImageList(this.Controller, globalIL, paramValueForImage, true);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Image Load");
                                // use type
                                imagePath = nodeType;
                            }

                            LoadNode(nodeID, linkID, nodeName, nodeType, nodeFunctions, imagePath, xyFromDB, ref startX, ref startY, docNodes);
                        }
                        break;

                    case "Polygon":              // parse VisualRepresentation=Polygon@Location.Polygon Points
                        // param name should have three parameters - poly points, color, alpha
                        if (actionValues.Count >= 3)
                        {
                            String polyPoints = parameterMap[actionValues[0]];
                            String polyColor = parameterMap[actionValues[1]];
                            String alpha = parameterMap[actionValues[2]];

                            DiagramPolygon polygon = new DiagramPolygon(this, nodeID, linkID, nodeName, nodeType, actionValues[0]);
                            polygon.Style = GoPolygonStyle.Line;

                            if (polyColor.Length > 0)
                            {
                                try
                                {
                                    ColorConverter convertFromString = new ColorConverter();

                                    Color convert = (Color)convertFromString.ConvertFrom(polyColor);

                                    byte alphaByte = 255;

                                    if (alpha.Length > 0)
                                    {
                                        alphaByte = Byte.Parse(alpha);
                                    }

                                    convert = Color.FromArgb(alphaByte, convert);
                                    if (FillPolygon)
                                    {
                                        polygon.Brush = new SolidBrush(convert);
                                    }

                                    if (OutlinePolygon)
                                    {
                                        polygon.Pen = new Pen(convert, 2.0f);
                                    }
                                }
                                catch (FormatException fex)
                                {
                                    MessageBox.Show("Could not convert color", fex.Message);
                                }
                            }
                            polygon.Functions = nodeFunctions;

                            try
                            {
                                // this will throw an exception if an invalid string is used
                                List<PointF> polygonPoints = FunctionHelper.ParsePolygonString(polyPoints, nodeType, m_coordinateTransformer);

                                foreach (PointF polyPoint in polygonPoints)
                                {
                                    polygon.AddPoint(polyPoint);
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Invalid format for vertex list", e.Message);
                            }

                            if (polygon.PointsCount > 0)
                            {
                                this.Document.Add(polygon);
                            }
                        }
                        break;
                    default:
                        {
                            // The visual representation is not recognized  - try external code
    
                            Type type = AMEManager.GetType(actionIdentifier);
    
                            if (type != null)
                            {
                /*
                 * (Diagram dg, List<String> actionValues, XPathNavigator nodeNav, Dictionary<String, Bitmap> nodeImages,
                                int nodeID, int linkID, String nodeName, String nodeType, List<Function> nodeFunctions, 
                                    bool xyFromDB, ref int startX, ref int startY, Dictionary<int, DiagramNode> docNodes)
                 */
                                try
                                {
                                    Activator.CreateInstance(type, new object[] {this, actionValues, nodeNav, globalIL, nodeID, 
                                    linkID, nodeName, nodeType, nodeFunctions, xyFromDB, startX, startY, docNodes});
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.Message);
                                }
                            }
                            break;
                        }
                } // switch
            }// if 
            else // no VR function, use type as default image
            {
                LoadNode(nodeID, linkID, nodeName, nodeType, nodeFunctions, xyFromDB, ref startX, ref startY, docNodes);
            }
        }

        private void LoadNode(int nodeID, int linkID, String nodeName, String nodeType, List<Function> nodeFunctions, Boolean xyFromDB, ref int startX, ref int startY, Dictionary<int, DiagramNode> docNodes)
        {
            LoadNode(nodeID, linkID, nodeName, nodeType, nodeFunctions, nodeType, xyFromDB, ref startX, ref startY, docNodes); // use type as image
        }

        public void LoadNode(int nodeID, int linkID, String nodeName, String nodeType, List<Function> nodeFunctions, String nodeImagePath, Boolean xyFromDB, ref int startX, ref int startY, Dictionary<int, DiagramNode> docNodes)
        {
            int index = globalIL.Images.IndexOfKey(nodeImagePath);

            if (index < 0)
            {
                index = globalIL.Images.IndexOfKey(nodeType);

                if (index < 0)
                {
                    if (globalIL.Images.Count >= 1)
                    {
                        StringEnumerator keys = globalIL.Images.Keys.GetEnumerator();
                        keys.MoveNext();
                        index = globalIL.Images.IndexOfKey(keys.Current); // just use first key
                    }
                    else
                    {
                        // replace with debug logging log4net
                        MessageBox.Show("Could not load image at: " + nodeImagePath + "Diagram.cs line 499");
                        return;
                    }
                }
            }


            DiagramNode newNode = null;

            if (xyFromDB == false)
            {
                Point diskPoint = this.GetPoint(nodeID);

                if (diskPoint != Point.Empty) // XY is from disk
                {
                    newNode = new DiagramNode(labelFontSize, nodeID, linkID, nodeName, nodeType, index, diskPoint);
                    newNode.Functions = nodeFunctions;
                }
                else // not found, use base values
                {
                    newNode = new DiagramNode(labelFontSize, nodeID, linkID, nodeName, nodeType, index, new PointF(startX, startY));
                    newNode.Functions = nodeFunctions;

                    SaveNode(newNode, false); // save to diagram XML
                    startX += 40; // and increment the base start
                }
            }
            else  // XY id from DB
            {
                // use transform
                startX = (int)m_coordinateTransformer.RetrieveX(startX);
                startY = (int)m_coordinateTransformer.RetrieveY(startY);

                newNode = new DiagramNode(labelFontSize, nodeID, linkID, nodeName, nodeType, index, new PointF(startX, startY));
                newNode.Functions = nodeFunctions;
            }

            // set tooltip to help with user input
            newNode.ToolTipText = "To link:  Click and drag on a node's image to another image" + '\n' + "To move: Click and drag on a node's text label";

            docNodes.Add(nodeID, newNode);

            this.Document.Add(newNode);
        }
            
        private Point GetPoint(int id)
        {
            XPathNodeIterator nodeIterator = pointNav.Select("key('IDKey', '"+id+"||"+RootID+"||"+diagramName+"')");

            if (nodeIterator != null && nodeIterator.Count > 0)
            {
                nodeIterator.MoveNext();
                XPathNavigator nodeNav = nodeIterator.Current;
                Point savedLocation;

                String xString = nodeNav.GetAttribute("X", nodeNav.NamespaceURI);
                String yString = nodeNav.GetAttribute("Y", nodeNav.NamespaceURI);

                int xValue = Int32.Parse(xString);
                int yValue = Int32.Parse(yString);

                // use transform
                xValue = (int)m_coordinateTransformer.RetrieveX(xValue);
                yValue = (int)m_coordinateTransformer.RetrieveY(yValue);

                savedLocation = new Point(xValue, yValue);

                return savedLocation;
            }
            else 
            {
                return Point.Empty;
            }
        }

        #endregion

        private List<GoTool> mouseMoveCopy, mouseUpCopy, mouseDownCopy;

        private List<String> allowedTypes;

        private bool deleteAsComponent = false;

        public bool DeleteAsComponent
        {
            get { return deleteAsComponent; }
            set { deleteAsComponent = value; }
        }

        public Diagram()
            : base()
        {
            myHelper = new ViewComponentHelper(this);

            this.ObjectResized += new GoSelectionEventHandler(Diagram_ObjectResized);
            this.LinkCreated += new GoSelectionEventHandler(GraphView_LinkCreated);
            this.SelectionDeleting += new CancelEventHandler(GraphView_SelectionDeleting);
            this.SelectionMoved += new EventHandler(Diagram_SelectionMoved);
            this.LinkRelinked += new GoSelectionEventHandler(Diagram_LinkRelinked);
            this.ObjectContextClicked += new GoObjectEventHandler(Diagram_ObjectSingleClicked);

            this.IDAttribute = XmlSchemaConstants.Display.Component.Id;
            this.NameAttribute = XmlSchemaConstants.Display.Component.Name;
            this.TypeAttribute = XmlSchemaConstants.Display.Component.Type;
            this.LinkIDAttribute = XmlSchemaConstants.Display.Component.LinkID;

            this.DragsRealtime = true;
            this.NoFocusSelectionColor = this.PrimarySelectionColor;
            
            // auto scroll
            this.AutoScrollDelay = 1000; // wait 1 second before auto scrolling
            this.AutoScrollTime = 100;  // autoscroll every 100 milliseconds
            this.AutoScrollRegion = new Size(16, 16); // detect auto scroll +- 16 pixels from margins

            // remove cut
            this.AllowCopy = false;

            // this will force the mvp xml dll to load...
            IndexingXPathNavigator tempNav = new IndexingXPathNavigator(new XmlDocument().CreateNavigator());
        
            //Copy Tools
            mouseMoveCopy = new List<GoTool>();
            mouseUpCopy = new List<GoTool>();
            mouseDownCopy = new List<GoTool>();

            allowedTypes = new List<String>();

            foreach (GoTool copyMe in MouseMoveTools)
            {
                mouseMoveCopy.Add(copyMe);
            }

            foreach (GoTool copyMe in MouseUpTools)
            {
                mouseUpCopy.Add(copyMe);
            }

            foreach (GoTool copyMe in MouseDownTools)
            {
                mouseDownCopy.Add(copyMe);
            }

            // no transform to start
            transform = null;
        }

        private int remove;
        private int intersect;
        private PointF polygonClicked;

        public void ResetCRC()
        {
            crc = String.Empty;
        }

        public void AddTypeFilter(String type)
        {
            if (!allowedTypes.Contains(type))
            {
                allowedTypes.Add(type);
            }
        }

        public void RemoveTypeFilter(String type)
        {
            if (allowedTypes.Contains(type))
            {
                allowedTypes.Remove(type);
            }
        }

        private void Diagram_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (this.ContextMenuStrip == null)
            {
                this.ContextMenuStrip = new ContextMenuStrip();
            }
            else
            {
                this.ContextMenuStrip.Items.Clear();
            }

            // right click on inside of poly adds points
            if (e.GoObject is DiagramPolygon && e.Buttons == MouseButtons.Right)
            {
                DiagramPolygon cast = (DiagramPolygon)e.GoObject;
                List<PointF> points = cast.GetPoints();

                Boolean deleteFound = false;
                remove = -1;
                intersect = -1; //checked in add/delete function calls

                for (int i = 0; i < points.Count; i++)
                {
                    PointF point = points[i];

                    if (Math.Abs(point.X - e.DocPoint.X) <= 10 && Math.Abs(point.Y - e.DocPoint.Y) <= 10)
                    {
                        deleteFound = true;
                        remove = i;
                        ToolStripItem item = ContextMenuStrip.Items.Add("Delete this point");
                        item.Click += new EventHandler(deletePoint);
                        break;
                    }
                }

                if (!deleteFound)
                {
                    polygonClicked = e.DocPoint;
                    intersect = cast.GetSegmentNearPoint(polygonClicked, 10);
                    if (intersect != -1)
                    {
                        ToolStripItem item = ContextMenuStrip.Items.Add("Add point here");
                        item.Click += new EventHandler(addPoint);
                    }
                }

                ContextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
                ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);
            }
        }

        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenuStrip.Items.Clear();
        }

        // override for smoother scrolling
        // provided from http://www.nwoods.com/forum/forum_posts.asp?TID=1832&KW=Panning
        public override PointF ComputeAutoScrollDocPosition(Point viewPnt)
        {
            PointF docpos = this.DocPosition;
            Point viewpos = ConvertDocToView(docpos);
            Size margin = this.AutoScrollRegion;
            int deltaX = this.ScrollSmallChange.Width;
            int deltaY = this.ScrollSmallChange.Height;
            Rectangle dispRect = this.DisplayRectangle;
            if (viewPnt.X >= dispRect.X && viewPnt.X < dispRect.X + margin.Width)
            {
                viewpos.X -= deltaX;
            }
            else if (viewPnt.X <= dispRect.X + dispRect.Width && viewPnt.X > dispRect.X + dispRect.Width - margin.Width)
            {
                viewpos.X += deltaX;
            }
            if (viewPnt.Y >= dispRect.Y && viewPnt.Y < dispRect.Y + margin.Height)
            {
                viewpos.Y -= deltaY;
            }
            else if (viewPnt.Y <= dispRect.Y + dispRect.Height && viewPnt.Y > dispRect.Y + dispRect.Height - margin.Height)
            {
                viewpos.Y += deltaY;
            }
            docpos = ConvertViewToDoc(viewpos);
            return docpos;
        }

        private void addPoint(object sender, EventArgs e)
        {
            if (this.Selection != null && this.Selection.First != null && this.Selection.First is GoPolygon)
            {
                DiagramPolygon cast = (DiagramPolygon)this.Selection.First;

                if (polygonClicked != null)
                {
                    if (intersect + 1 < cast.PointsCount)
                    {
                        cast.InsertPoint(intersect + 1, polygonClicked);
                    }
                    else
                    {
                        cast.AddPoint(polygonClicked);
                    }

                    this.Controller.UpdateParameters(cast.NodeID, cast.ParameterName, cast.GetPointString(), eParamParentType.Component);
                }
            }
        }

        private void deletePoint(object sender, EventArgs e)
        {
            if (this.Selection != null && this.Selection.First != null && this.Selection.First is GoPolygon)
            {
                DiagramPolygon cast = (DiagramPolygon)this.Selection.First;

                if (remove != -1)
                {
                    cast.RemovePoint(remove);

                    this.Controller.UpdateParameters(cast.NodeID, cast.ParameterName, cast.GetPointString(), eParamParentType.Component);
                }
            }
        }

        public void LoadDefaultTools()
        {
            this.MouseMoveTools.Clear();
            this.MouseUpTools.Clear();
            this.MouseDownTools.Clear();

            foreach (GoTool copyMe in mouseMoveCopy)
            {
                MouseMoveTools.Add(copyMe);
            }

            foreach (GoTool copyMe in mouseUpCopy)
            {
                MouseUpTools.Add(copyMe);
            }

            foreach (GoTool copyMe in mouseDownCopy)
            {
                MouseDownTools.Add(copyMe);
            }
        }

        public void SelectAndMoveToolsOnly()
        {
            this.LoadDefaultTools();

            this.ReplaceMouseTool(typeof(GoToolLinking), null);
            this.ReplaceMouseTool(typeof(GoToolLinkingNew), null);
            this.ReplaceMouseTool(typeof(GoToolRelinking), null);
        }

        public void LinkingToolsOnly()
        {
            this.LoadDefaultTools();

            this.ReplaceMouseTool(typeof(GoToolDragging), null);
            this.ReplaceMouseTool(typeof(GoToolRubberBanding), null);
        }

        private void InitializeXmlFile()
        {
            fileName = Path.Combine(myController.XmlPath, "Diagrams.xml");
            FileInfo xmlFile = new FileInfo(fileName);

            if (!xmlFile.Exists)
            {
                XmlDocument doc = new XmlDocument();

                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                doc.AppendChild(declaration);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
                namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                // Create root element
                XmlElement root = doc.CreateElement("Nodes");

                // Add schema information to root.
                XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                schema.Value = "diagrams.xsd";
                root.SetAttributeNode(schema);

                doc.AppendChild(root);

                doc.Save(fileName);
            }
        }

        // deletes all nodes with this rootID
        public void DeleteCache(int rootID)
        {
            StreamReader xmlStream = LoadDiagramsXML();

            XmlNodeList deleteList = currentDoc.SelectNodes(String.Format("/Nodes/Node[@DiagramName='{0}'][@RootID='{1}']", diagramName, rootID));

            if (deleteList != null && root != null)
            {
                foreach (XmlNode deleteMe in deleteList)
                {
                    root.RemoveChild(deleteMe);
                }
            }

            UnloadDiagramsXML(xmlStream, true);
        }

        public void DeleteNodeWithID(int nodeID)
        {
            GoLayerCollectionObjectEnumerator all = this.Document.GetEnumerator();
            foreach (GoObject item in all)
            {
                if (item is HasNodeID)
                {
                    HasNodeID cast = (HasNodeID)item;
                    if (cast.NodeID == nodeID)
                    {
                        DeleteGoObject(item);
                    }
                }
            }
        }
                       
        private void DeleteGoObject(GoObject item)
        {
            if (deleteAsComponent && item is DiagramNode)
            {
                DiagramNode node = (DiagramNode)item;
                this.myController.DeleteComponent(node.NodeID);
                DeleteDiagramNode(node);
            }
            else
            {
                if (item is HasLinkID)
                {
                    this.myController.DeleteLink(((HasLinkID)item).LinkID);
                }

                if (item is DiagramNode)
                {
                    DiagramNode node = (DiagramNode)item;

                    DeleteDiagramNode(node);

                    foreach (DiagramLink link in node.Links)
                    {
                        this.myController.DeleteLink(link.LinkID);
                    }
                }
            }
        }

        private void DeleteDiagramNode(DiagramNode node)
        {
            StreamReader xmlStream = LoadDiagramsXML();

            XmlNode delete = currentDoc.SelectSingleNode(String.Format("/Nodes/Node[@ID='{1}'][@DiagramName='{0}'][@RootID='{2}']", diagramName, node.NodeID, RootID));

            if (delete != null && root != null)
            {
                root.RemoveChild(delete);
            }

            UnloadDiagramsXML(xmlStream, true);
        }

        private void GraphView_SelectionDeleting(object sender, CancelEventArgs e)
        {
            myController.TurnViewUpdateOff();

            GoCollectionEnumerator selectionEnum = this.Selection.GetEnumerator();

            foreach (GoObject selectedItem in selectionEnum)
            {
                DeleteGoObject(selectedItem);
            }

            myController.TurnViewUpdateOn();
        }

        private void GraphView_LinkCreated(object sender, GoSelectionEventArgs e)
        {
            GoLink link = (GoLink)e.GoObject;
            DiagramNode to = (DiagramNode)link.ToNode;
            DiagramNode from = (DiagramNode)link.FromNode;
            
            this.Document.Remove(link);

            try
            {
                this.myController.Connect(this.RootID, from.NodeID, to.NodeID, DiagramName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to create link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Diagram_LinkRelinked(object sender, GoSelectionEventArgs e)
        {
            myController.TurnViewUpdateOff();

            DiagramLink link = (DiagramLink)e.GoObject;
            this.Document.Remove(link);

            DiagramNode to = (DiagramNode)link.ToNode;
            DiagramNode from = (DiagramNode)link.FromNode;

            if (this.Tool is GoToolRelinking)
            {
                GoToolRelinking castTool = (GoToolRelinking)this.Tool;

                GoPort start, end;

                if (castTool.Forwards)
                {
                    start = (GoPort)castTool.OriginalStartPort;
                    end = (GoPort)castTool.OriginalEndPort;
                }
                else
                {
                    //swap
                    start = (GoPort)castTool.OriginalEndPort;
                    end = (GoPort)castTool.OriginalStartPort;
                }

                DiagramNode startNode = (DiagramNode)start.Node;
                DiagramNode endNode = (DiagramNode)end.Node;

                try
                {
                    // record the relink in the database
                    this.myController.Connect(this.RootID, from.NodeID, to.NodeID, DiagramName);

                    // and remove the old link
                    this.Controller.DeleteLink(link.LinkID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to create link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // the old link was removed from the display on the unsuccessful relink
                    // recreate it (it's still in the db, but this is more efficient)

                    DiagramLink newLink = new DiagramLink();
                    newLink.ToArrow = true;

                    newLink.FromPort = startNode.Port;
                    newLink.ToPort = endNode.Port;

                    this.Document.Add(newLink);
                }
            }
            myController.TurnViewUpdateOn();
        }

        private void UnloadDiagramsXML(StreamReader xmlStream, Boolean save)
        {
            updating = false;

            xmlStream.Close();

            if (save)
            {
                currentDoc.Save(fileName);
            }
        }

        private StreamReader LoadDiagramsXML()
        {
            updating = true;

            FileInfo xmlFile = new FileInfo(fileName);
            StreamReader xmlStream = xmlFile.OpenText();
            currentDoc = new XmlDocument();
            currentDoc.Load(xmlStream);
            root = currentDoc.SelectSingleNode("/Nodes");

            return xmlStream;
        }

        // Handle a drop from an external window
        protected override IGoCollection DoExternalDrop(DragEventArgs evt)
        {
            IDataObject data = evt.Data;
            List<ProcessingNode> treeNodes = (List<ProcessingNode>)data.GetData(typeof(List<ProcessingNode>));
            if (treeNodes != null && treeNodes is List<ProcessingNode>)
            {
                Point screenPnt = new Point(evt.X, evt.Y);
                Point viewPnt = PointToClient(screenPnt);
                PointF convertedForDocument = ConvertViewToDoc(viewPnt);
                docPnt = new Point((int)convertedForDocument.X, (int)convertedForDocument.Y);

                StreamReader tempStream = LoadDiagramsXML(); // process call below can often create, and save, nodes and their locations, so open diagrams xml

                myController.TurnViewUpdateOff();

                int previousID = -1;
                foreach (ProcessingNode dropNode in treeNodes)
                {
                    int nodeID = dropNode.NodeID;
                    
                    if (nodeID != RootID && nodeID != previousID)
                    {
                        try
                        {
                            dropNode.process(this);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        finally
                        {
                            docPnt.Y = docPnt.Y + 40;
                            previousID = nodeID;
                        }
                    }
                }

                UnloadDiagramsXML(tempStream, true); // tempStream will be closed

                myController.TurnViewUpdateOn();

            }
            return base.DoExternalDrop(evt);
        }

        // Convert dragged objects into go objects, if applicable
        private GoObject GetGoObjectFromDrag(DragEventArgs evt)
        {
            IDataObject data = evt.Data;
            List<ProcessingNode> treeNodes = (List<ProcessingNode>)data.GetData(typeof(List<ProcessingNode>));
            if (treeNodes != null && treeNodes is List<ProcessingNode>)
            {
                Point screenPnt = new Point(evt.X, evt.Y);
                Point viewPnt = PointToClient(screenPnt);
                PointF convertedForDocument = ConvertViewToDoc(viewPnt);
                docPnt = new Point((int)convertedForDocument.X, (int)convertedForDocument.Y);

                if (treeNodes.Count > 0)
                {
                    ProcessingNode first = treeNodes[0];

                    int imageIndex = this.ImageList.Images.Keys.IndexOf(first.NodeType);
                    if (imageIndex < 0)
                    {
                        imageIndex = this.ImageList.Images.Keys.IndexOf(first.ImageKey);
                        if (imageIndex < 0)
                        {
                            return null;
                        }
                    }

                    DiagramNode node = new DiagramNode(
                        labelFontSize,
                        first.NodeID,
                        first.Name,
                        first.NodeType,
                        imageIndex,
                        //first.TreeView.ImageList,
                        //imageIndex,
                        docPnt);

                    return node;
                }
                return null;
            }
            else return null;
        }

        // This override controls what is shown (if anything) during
        // an external drag into this view.
        protected override GoObject GetExternalDragImage(DragEventArgs evt)
        {
            GoObject toAdd = GetGoObjectFromDrag(evt);
            if (toAdd != null)
            {
                return toAdd;
            }
            else
            {
                return base.GetExternalDragImage(evt);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Diagram
            // 
            this.Border3DStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.ResumeLayout(false);

        }

        public Boolean ContainsData()
        {
            GoLayerCollectionObjectEnumerator all = this.Document.GetEnumerator();
            return all.MoveNext();
        }

        private void Diagram_SelectionMoved(object sender, EventArgs e)
        {
            // default behaviors - sync nodes with Diagrams.xml for xy
            // sync polygons with their points parameter
            StreamReader tempStream = LoadDiagramsXML();

            foreach (GoObject moved in this.Selection.GetEnumerator())
            {
                bool success = TrySaveGoObjectLocation(moved, true, tempStream);
                if (!success)
                {
                    return; // skip the rest of the enumeration
                }
            }

            UnloadDiagramsXML(tempStream, true);

            this.UpdateViewComponent();
        }

        public void SaveAllGoObjectLocations(Boolean updateAfter)
        {
            StreamReader tempStream = LoadDiagramsXML();

            foreach (GoObject go in this.Document.GetEnumerator())
            {
                bool success = TrySaveGoObjectLocation(go, updateAfter, tempStream);
                if (!success)
                {
                    return; // skip the rest of the enumeration
                }
            }

            UnloadDiagramsXML(tempStream, true);
        }

        private bool TrySaveGoObjectLocation(GoObject go, Boolean updateAfter, StreamReader xmlStream)
        {
            try
            {
                SaveGoObjectLocation(go, updateAfter);
            }
            catch (Exception ex)
            {
                ResetCRC();
                UnloadDiagramsXML(xmlStream, true);
                this.UpdateViewComponent();

                if (this.Controller.ViewUpdateStatus == false)
                {
                    this.Controller.TurnViewUpdateOn(false, false);
                }

                MessageBox.Show(ex.Message, "Error moving event");
                return false;
                //This is necessary because if both x and y break the max value, 
                //the catch triggers for one, which modifies the enumeration which then yields another error.
            }
            return true;
        }

        private void SaveGoObjectLocation(GoObject go, Boolean updateAfter)
        {
            if (go is DiagramNode)
            {
                DiagramNode dgNode = (DiagramNode)go;

                SaveNode(dgNode, updateAfter);
            }
            else if (go is DiagramPolygon)
            {
                DiagramPolygon poly = (DiagramPolygon)go;

                this.Controller.UpdateParameters(poly.NodeID, poly.ParameterName, poly.GetPointString(), eParamParentType.Component);
            }
        }

        private void Diagram_ObjectResized(object sender, GoSelectionEventArgs e)
        {
            this.StopAutoScroll(); // from DiagramPolygon resizingevent

            GoObject test = e.GoObject;

            if (test is DiagramPolygon)
            {
                DiagramPolygon poly = (DiagramPolygon)test;

                this.Controller.UpdateParameters(poly.NodeID, poly.ParameterName, poly.GetPointString(), eParamParentType.Component);
            }
        }

        public void RecordXYToXML(int NodeID, List<Function> functions, String newX, String newY, Boolean parameterUpdateAfterMoveFunction)
        {
            ProcessMoveFunction(NodeID, functions, newX, newY, parameterUpdateAfterMoveFunction);
        }

        public void RecordXYToXML(int NodeID, String newX, String newY, Boolean parameterUpdateAfterMoveFunction)
        {
            ProcessMoveFunction(NodeID, FunctionHelper.GetFunctions(RootID, NodeID, DiagramName, myController), newX, newY, parameterUpdateAfterMoveFunction);
        }

        private void SaveNode(DiagramNode dgNode , Boolean parameterUpdateAfterMoveFunction)
        {
            String newX = ((int)dgNode.Location.X).ToString();
            String newY = ((int)dgNode.Location.Y).ToString();

            RecordXYToXML(dgNode.NodeID, dgNode.Functions, newX, newY, parameterUpdateAfterMoveFunction);
        }

        private void ProcessMoveFunction(int NodeID, List<Function> functions, String newX, String newY, Boolean parameterUpdateAfterMoveFunction)
        {
            // use transform
            int storeX = (int)float.Parse(newX);
            int storeY = (int)float.Parse(newY);

            newX = m_coordinateTransformer.StoreX(storeX).ToString();
            newY = m_coordinateTransformer.StoreY(storeY).ToString();

            List<ParsedFunction> parsedFunctions = FunctionHelper.ParseFunctions(functions);
 
            ParsedFunction move = parsedFunctions.Find(delegate(ParsedFunction del) { return del.FunctionName == "MoveAction" && del.ActionIdentifier == "InsertXYIntoDB"; });
            if (move != null)
            {
                String firstParam = move.ActionValues[0];
                String secondParam = move.ActionValues[1];

                Boolean update = false;
                if (parameterUpdateAfterMoveFunction && Controller.ViewUpdateStatus == true)
                {
                    update = true;
                    this.Controller.TurnViewUpdateOff();
                }

                this.Controller.UpdateParameters(NodeID, firstParam, newX, eParamParentType.Component);
                this.Controller.UpdateParameters(NodeID, secondParam, newY, eParamParentType.Component);

                if (update)
                {
                    this.Controller.TurnViewUpdateOn();
                }
            }
            else // record XY in XML file
            {
                XmlNode nodeWithID = currentDoc.SelectSingleNode(String.Format("/Nodes/Node[@DiagramName='{0}'][@ID='{1}'][@RootID='{2}']",
                                                   diagramName, NodeID, RootID));

                if (nodeWithID != null)
                {
                    nodeWithID.Attributes["X"].Value = newX;
                    nodeWithID.Attributes["Y"].Value = newY;
                }
                else
                {
                    XmlElement nodeElement = currentDoc.CreateElement("Node");

                    XmlAttribute nodeIDAttr = currentDoc.CreateAttribute("ID");
                    nodeIDAttr.Value = NodeID.ToString();

                    XmlAttribute DiagramNameAttr = currentDoc.CreateAttribute("DiagramName");
                    DiagramNameAttr.Value = DiagramName;

                    XmlAttribute RootIDAttr = currentDoc.CreateAttribute("RootID");
                    RootIDAttr.Value = RootID.ToString();

                    XmlAttribute x = currentDoc.CreateAttribute("X");
                    XmlAttribute y = currentDoc.CreateAttribute("Y");
                    x.Value = newX;
                    y.Value = newY;

                    nodeElement.Attributes.Append(nodeIDAttr);
                    nodeElement.Attributes.Append(DiagramNameAttr);
                    nodeElement.Attributes.Append(RootIDAttr);
                    nodeElement.Attributes.Append(x);
                    nodeElement.Attributes.Append(y);

                    root.AppendChild(nodeElement);
                }
            }
        }
            
        //protected override void PaintPaperColor(Graphics g, RectangleF cliprect) { }
        //protected override void PaintBackgroundDecoration(Graphics g, RectangleF clipRect)
        //{
        //    if (BackgroundImage != null)
        //    {
        //        float[][] matrixItems ={ 
        //           new float[] {1, 0, 0, 0, 0},
        //           new float[] {0, 1, 0, 0, 0},
        //           new float[] {0, 0, 1, 0, 0},
        //           new float[] {0, 0, 0, 0.2f, 0}, 
        //           new float[] {0, 0, 0, 0, 1}};
        //        System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(matrixItems);

        //        // Create an ImageAttributes object and set its color matrix.
        //        System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes();
        //        imageAtt.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

        //        int iWidth = BackgroundImage.Width;
        //        int iHeight = BackgroundImage.Height;
        //        g.DrawImage(BackgroundImage, new Rectangle(0, 0, (int)clipRect.Width, (int)clipRect.Height), 0, 0, clipRect.Width, clipRect.Height, GraphicsUnit.Pixel, imageAtt);
        //    }
        //    base.PaintBackgroundDecoration(g, clipRect);
        //}
    }

    public class DiagramNode : GoIconicNode, HasLinkID, HasNodeID
    {
        private int nodeID;
        private int linkID;
        private List<Function> functions;

        public List<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }
        private String nodeType;

        public String NodeType
        {
            get { return nodeType; }
            set { nodeType = value; }
        }

        public DiagramNode(float labelFontSize, int p_nodeID, int p_linkID, String name, String type, int index, PointF locationPoint)
            : this(labelFontSize, p_nodeID, name, type, index, locationPoint)
        {
            this.LinkID = p_linkID;
        }

        public DiagramNode(float labelFontSize, int p_nodeID, String name, String type, int index, PointF locationPoint)
            : base()
        {
            this.Initialize(null, index, name);
 
            if (labelFontSize != 0)
            {
                this.Label.FontSize = labelFontSize;
            }
          
            this.Icon.Height = Diagram.IconHeight;
            this.Icon.Width = Diagram.IconWidth;
    
            this.Location = locationPoint;
            this.NodeType = type;
            this.NodeID = p_nodeID;
            functions = new List<Function>();
        }

        public override String GetToolTip(GoView view)
        {
            view.ToolTip.AutoPopDelay = 10000;
            view.ToolTip.InitialDelay = 2000;
            view.ToolTip.ReshowDelay = 2000;
            return base.GetToolTip(view);
        }
    }

    public class DiagramLink : GoLabeledLink, HasLinkID
    {
        private int linkID;

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        public DiagramLink()
            : base()
        { }

        public DiagramLink(int p_linkID)
        {
            this.linkID = p_linkID;
        }
    }

    public class DiagramPolygon : GoPolygon, HasLinkID, HasNodeID
    {
        private GoView myDiagram;

        private int nodeID;
        private int linkID;
        private List<Function> functions;
        private string parameterName;
        private string nodeName;
        private string nodeType;

        public string NodeName
        {
            get { return nodeName; }
            set { nodeName = value; }
        }

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        public List<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public String NodeType
        {
            get { return nodeType; }
            set { nodeType = value; }
        }

        private List<PointF> myPoints;

        public List<PointF> GetPoints()
        {
            if (myPoints == null)
            {
                List<PointF> points = new List<PointF>();
                for (int i = 0; i < this.PointsCount; i++)
                {
                    points.Add(GetPoint(i));
                }
                myPoints = points;
            }
            return myPoints;
        }

        public String GetPointString()
        {
            StringBuilder pointBuilder = new StringBuilder();
            for (int i = 0; i < this.PointsCount; i++)
            {
                PointF current = this.GetPoint(i);

                // use Transform
                current = new PointF(
                    ((Diagram)myDiagram).CoordinateTransformer.StoreX(current.X),
                    ((Diagram)myDiagram).CoordinateTransformer.StoreY(current.Y));

                if (i == this.PointsCount - 1)
                {
                    pointBuilder.Append("(" + (int)current.X + ", " + (int)current.Y + ")");
                }
                else
                {
                    pointBuilder.Append("(" + (int)current.X + ", " + (int)current.Y + "), ");
                }
            }

            return pointBuilder.ToString();
        }

        public override void Paint(Graphics g, GoView view)
        {
            base.Paint(g, view);

            // draw name inside polygon
            // use bounds for now (can be incorrect, proper method involves centering among n vertices)
            g.DrawString(this.NodeName, view.Font, Brushes.Black, new PointF(this.Left+this.Bounds.Width/2, this.Top+this.Bounds.Height/2));
        }

        public DiagramPolygon(GoView v)
            : base()
        {
            myDiagram = v;
            this.ResizesRealtime = true; // for autoscrolling when resizing
        }

        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            this.myDiagram.DoAutoScroll(this.myDiagram.LastInput.ViewPoint);
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
        }

        public DiagramPolygon(GoView v,int p_nodeID, int p_linkID, String p_name, String p_type, String p_parameterName)
            : this(v)
        {
            this.LinkID = p_linkID;
            this.NodeType = p_type;
            this.NodeID = p_nodeID;
            this.NodeName = p_name;
            this.ParameterName = p_parameterName;
            functions = new List<Function>();
        }

        public override String GetToolTip(GoView view)
        {
            view.ToolTip.AutoPopDelay = 10000;
            view.ToolTip.InitialDelay = 2000;
            view.ToolTip.ReshowDelay = 2000;
            return base.GetToolTip(view);
        }
    }

    public interface HasNodeID
    {
        int NodeID { get; set; }
    }

    public interface HasLinkID
    {
        int LinkID { get; set; }
    }
}
