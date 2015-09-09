using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Xml.XPath;
using AME.Model;
using System.ComponentModel;
using AME.Controllers;
using System.Data;
using System.Windows.Forms;
using Forms;
using AME.Controllers.Base.DataStructures;

namespace AME.Views.View_Components
{
    public class MultiParameterGrid : ParameterTable
    {
        private string linkType = "";

        public string LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }

        String filterByType = "";

        public String FilterByType
        {
            get { return filterByType; }
            set { filterByType = value; }
        }

        private Dictionary<int, int> tableIndexToComponentID;

        private Dictionary<int, bool> paintLeftColumnBorder; // column index
        private Dictionary<int, bool> paintRightColumnBorder;

        private Dictionary<int, String> columnCategory; // column index to category

        private List<String> filterCategories;

        // from designer
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem toolStripMenuItem3;

        public MultiParameterGrid()
            : base()
        {
            InitializeComponent();

            this.AllowUserToResizeColumns = false; // no resize for now

            tableIndexToComponentID = new Dictionary<int,int>();

            paintLeftColumnBorder = new Dictionary<int,bool>();
            paintRightColumnBorder = new Dictionary<int,bool>();

            columnCategory = new Dictionary<int, string>();

            this.EnableHeadersVisualStyles = false; // so border choices will show
            this.CellPainting += new DataGridViewCellPaintingEventHandler(MultiParameterGrid_CellPainting);

            filterCategories = new List<String>();

            // show all
            contextMenuStrip2.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip2_ItemClicked);
        }

     
        private void MultiParameterGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // adjust borders
            if (e.ColumnIndex != -1 && e.RowIndex == -1) // -1 row means column header
            {
                if (paintLeftColumnBorder.ContainsKey(e.ColumnIndex) && paintLeftColumnBorder[e.ColumnIndex])
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }

                if (paintRightColumnBorder.ContainsKey(e.ColumnIndex) && paintRightColumnBorder[e.ColumnIndex])
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }

                // center and paint category string
                if (columnCategory.ContainsKey(e.ColumnIndex - 1) && columnCategory.ContainsKey(e.ColumnIndex))
                {
                    if ( (columnCategory[e.ColumnIndex - 1] != columnCategory[e.ColumnIndex]) || columnCategory.Count - 1 == e.ColumnIndex)
                    {// last category in a sequence

                        int saveValue = e.ColumnIndex - 1;
                        int startFromHere = e.ColumnIndex - 1;

                        String testCategory = columnCategory[e.ColumnIndex - 1];

                        int width = 0;
                        while (columnCategory.ContainsKey(startFromHere) && columnCategory[startFromHere] == testCategory)
                        {
                            width += this.Columns[startFromHere].Width;
                            startFromHere--;
                        }

                        int diff = saveValue - startFromHere;
                        if (diff >= 0)
                        {
                            Rectangle current = e.CellBounds;
                            int startX = current.X;

                            if (columnCategory.Count - 1 == e.ColumnIndex) // if last column, far right cell is not included 
                            {                                              // in calculations, add it in
                                int farRightCell = this.Columns[e.ColumnIndex].Width;;
                                width += farRightCell;
                                startX += farRightCell;

                                // normally this works because we're painting the string over the *previous* header 
                                // that was just painted.  There is a special case where if this is the last header
                                // (we can't cue off the category changing because there's no cell to the right with a different
                                // category), this painting event will be called, the string will be drawn, then the .NET
                                // paint will finish, drawing over the string.  Instead, for this last cell, draw it now, mark as handled,
                                // then paint the string
                                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                                e.Handled = true;
                            }

                            SizeF forString = e.Graphics.MeasureString(testCategory, this.Font);
                            width += (int)forString.Width; // add string width

                            startX = startX - (width / 2); // center x 
                            int startY = current.Height - ((current.Height + (int)forString.Height) / 2); // center y with string height

                            e.Graphics.DrawString(testCategory, this.Font, Brushes.Black, new PointF(startX, startY));
                        }
                    }
                }
            }
        }

        protected override void LoadParameterData()
        {
            String ComponentIDAttribute = XmlSchemaConstants.Display.Component.Id;
            String ComponentNameAttribute = XmlSchemaConstants.Display.Component.Name;

            String ParameterNameAttribute = ConfigFileConstants.displayedName;
            String ParameterCategoryAttribute = ConfigFileConstants.category;
            String ParameterValueAttribute = ConfigFileConstants.Value;

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;
            compOptions.CompParams = true;
            IXPathNavigable iNavigator = myController.GetComponentAndChildren(SelectedID, linkType, compOptions);

            XPathNavigator navigator = iNavigator.CreateNavigator();

            DrawingUtility.SuspendDrawing(this);
            {
                this.Columns.Clear();
                columnCategory.Clear();
                paintLeftColumnBorder.Clear();
                paintRightColumnBorder.Clear();

                this.ContextMenuStrip = null;

                if (navigator.HasChildren)
                {
                    tableIndexToComponentID.Clear();

                    XPathNodeIterator listOfNodes = navigator.Select(String.Format("/Components/Component/Component[@Type='{0}']", filterByType));

                    List<NameIDParams> nameIDs = new List<NameIDParams>();

                    Boolean columnsLoaded = false;
                    Boolean rowLoaded = false;

                    List<NameValuePair> paramNameValues;
                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        XPathNodeIterator parameterNodes = nodeNav.Select("ComponentParameters/Parameter/Parameter");

                        paramNameValues = new List<NameValuePair>();
                        String paramName, paramValue, paramCategory;

                        foreach (XPathNavigator paramNav in parameterNodes)
                        {
                            paramName = paramNav.GetAttribute(ParameterNameAttribute, paramNav.NamespaceURI);
                            paramCategory = paramNav.GetAttribute(ParameterCategoryAttribute, paramNav.NamespaceURI);
                            paramValue = paramNav.GetAttribute(ParameterValueAttribute, paramNav.NamespaceURI);

                            if (!filterCategories.Contains(paramCategory))
                            {
                                paramNameValues.Add(new NameValuePair(paramCategory + SchemaConstants.ParameterDelimiter + paramName, paramValue));
                            }
                        }

                        paramNameValues.Sort();

                        String[] parameterValues = new String[paramNameValues.Count];
                        String[] rowValues = new String[paramNameValues.Count];

                        NameValuePair aColumnNameToAdd;
                        String previousColumnName = "";
                        String combined;

                        for (int i = 0; i < paramNameValues.Count; i++)
                        {
                            aColumnNameToAdd = paramNameValues[i];

                            parameterValues[i] = aColumnNameToAdd.Value;

                            if (!columnsLoaded)
                            {
                                // parse
                                combined = aColumnNameToAdd.Name;

                                int delimiterIndex = combined.IndexOf(SchemaConstants.ParameterDelimiter); // "."
                                if (delimiterIndex > 0)
                                {
                                    String category = combined.Substring(0, delimiterIndex);

                                    int startOfNameIndex = delimiterIndex + 1;
                                    String name = combined.Substring(startOfNameIndex, combined.Length - startOfNameIndex);

                                    rowValues[i] = name;

                                    int addedColumn = -1;
                                    if (!category.Equals(previousColumnName))
                                    {
                                        addedColumn = this.Columns.Add(" ", " ");
                                        columnCategory[addedColumn] = category;

                                        // set previous column to draw right border
                                        if (addedColumn > 0)
                                        {
                                            paintRightColumnBorder[addedColumn - 1] = true;
                                        }

                                        if (i == paramNameValues.Count -1) // last one, paint right
                                        {
                                            paintLeftColumnBorder[addedColumn] = false;
                                            paintRightColumnBorder[addedColumn] = true;
                                        }
                                        else
                                        {
                                            paintLeftColumnBorder[addedColumn] = false;
                                            paintRightColumnBorder[addedColumn] = false;
                                        }
                                    }
                                    else
                                    {
                                        addedColumn = this.Columns.Add(" ", " ");
                                        columnCategory[addedColumn] = category;

                                        if (i == paramNameValues.Count - 1) // last one, paint right
                                        {
                                            paintLeftColumnBorder[addedColumn] = false;
                                            paintRightColumnBorder[addedColumn] = true;
                                        }
                                        else
                                        {
                                            paintLeftColumnBorder[addedColumn] = false;
                                            paintRightColumnBorder[addedColumn] = false;
                                        }
                                    }

                                    previousColumnName = category;

                                    if (addedColumn != -1)
                                    {
                                        this.Columns[addedColumn].SortMode = DataGridViewColumnSortMode.NotSortable;

                                        Size textSize = TextRenderer.MeasureText(rowValues[i], this.Font);
                                        int prefWidth = textSize.Width;
                                        if (prefWidth < 2) 
                                        {
                                            prefWidth = 2;
                                        }
                                        this.Columns[addedColumn].MinimumWidth = prefWidth+1;// buffer, TextRenderer can be off

                                        this.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(MultiParameterGrid_ColumnHeaderMouseClick);
                                    }
                                } // category check
                            } // columns load
                        } // name values

                        columnsLoaded = true;

                        if (!rowLoaded)
                        {
                            if (this.Columns.Count > 0)
                            {
                                this.Rows.Add(rowValues);
                                rowLoaded = true;
                            }
                        }

                        String nodeName = nodeNav.GetAttribute(ComponentNameAttribute, nodeNav.NamespaceURI);
                        int nodeID = Int32.Parse(nodeNav.GetAttribute(ComponentIDAttribute, nodeNav.NamespaceURI));

                        NameIDParams addPair = new NameIDParams(nodeName, nodeID, parameterValues);
                        nameIDs.Add(addPair);
                    } // all params
                    
                    nameIDs.Sort();

                    foreach (NameIDParams aPair in nameIDs)
                    {
                        if (Columns.Count > 0)
                        {
                            int addedRow = this.Rows.Add(aPair.ParameterValues);
                            Rows[addedRow].HeaderCell.Value = aPair.Name;
                            tableIndexToComponentID.Add(addedRow, aPair.Id);
                        }
                    }

                    if (Columns.Count == 0)
                    {
                        this.ContextMenuStrip = contextMenuStrip2; // show contextmenu if no columns
                    }
                } // nav
            }
        }

        protected override void keyEnter()
        {
            this.CommitEdit(DataGridViewDataErrorContexts.Commit); // force the edit to complete
            int row = this.CurrentCell.RowIndex;
            int column = this.CurrentCell.ColumnIndex;

            if (Columns[column].ReadOnly != true)
            {
                String value = "";
                if (this[column, row].Value != null)
                {
                    value = this[column, row].Value.ToString(); // get the value of the cell
                }
                
                String name = this.Rows[0].Cells[column].Value.ToString();
                String category = FindCategory(column);

                if (category != null)
                {
                    String sendDown = category + SchemaConstants.ParameterDelimiter + name;

                    try
                    {
                        myController.UpdateParameters(tableIndexToComponentID[row], sendDown, value, eParamParentType.Component); // push
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                        // on exception restore value
                        this.UpdateViewComponent();
                    }
                }
            }
        }

        //protected override bool ParameterTable_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        this.CommitEdit(DataGridViewDataErrorContexts.Commit); // force the edit to complete
        //        int row = this.CurrentCell.RowIndex;
        //        int column = this.CurrentCell.ColumnIndex;

        //        if (Columns[column].ReadOnly != true)
        //        {
        //            String value = "";
        //            if (this[column, row].Value != null)
        //            {
        //                value = this[column, row].Value.ToString(); // get the value of the cell
        //            }
        //            String name = this.Columns[column].Name;

        //            try
        //            {
        //                myController.UpdateParameters(tableIndexToComponentID[row], name, value, eParamParentType.Component); // push
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
        //                // on exception restore value
        //                this.UpdateViewComponent();
        //            }
        //            return true;
        //        }
        //    }
        //    else if (e.KeyCode == Keys.V && ModifierKeys == Keys.Control)
        //    {
        //        try
        //        {
        //            Paste();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
        //            // on exception restore value

        //            if (myController.ViewUpdateStatus == false)
        //            {
        //                myController.TurnViewUpdateOn(); // refresh
        //            }
        //            else
        //            {
        //                this.UpdateViewComponent();
        //            }
        //        } // ex
        //        return true;
        //    }
        //    return false;
        //}

        protected override void Paste()
        {
            String copiedText = Clipboard.GetText();

            myController.TurnViewUpdateOff();

            char carriagereturn = (char)13;
            char tab = (char)9;

            if (copiedText != null)
            {
                String[] lines = copiedText.Split(new char[] { carriagereturn });

                for (int i = 0; i < lines.Length; i++)
                {
                    String line = lines[i];
                    line = line.Trim();
                    String[] elementsPerLine = line.Split(new char[] { tab });

                    int row = CurrentCell.RowIndex;
                    int col = CurrentCell.ColumnIndex;

                    for (int j = 0; j < elementsPerLine.Length; j++)
                    {
                        if (row + i < this.Rows.Count && col + j < this.Columns.Count)
                        {
                            String name = this.Rows[0].Cells[col+j].Value.ToString();
                            String category = FindCategory(col+j);

                            if (category != null)
                            {
                                String sendDown = category + SchemaConstants.ParameterDelimiter + name;

                                int compID = tableIndexToComponentID[row + i];

                                try
                                {
                                    myController.UpdateParameters(compID, sendDown, elementsPerLine[j], eParamParentType.Component);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Unable to update parameter: ", e.Message);
                                }
                            }
                        }
                    }
                }
            }

            SendKeys.Send("{ESC}");

            myController.TurnViewUpdateOn();
        }

        private String FindCategory(int col)
        {
            if (columnCategory.ContainsKey(col))
            {
                return columnCategory[col];
            }
            else
            {
                MessageBox.Show("Could not find category", "Error updating parameter. Check the format of the parameter and any other constraints");
                return null;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 48);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItem1.Tag = "Remove";
            this.toolStripMenuItem1.Text = "Remove category";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItem2.Tag = "ShowAll";
            this.toolStripMenuItem2.Text = "Show all categories";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(178, 26);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItem3.Tag = "ShowAll";
            this.toolStripMenuItem3.Text = "Show all categories";
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        private string contextMenuStringColumn;

        private void MultiParameterGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStringColumn = columnCategory[e.ColumnIndex]; // record clicked on column, and show
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessContextItem(e.ClickedItem);
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessContextItem(e.ClickedItem);
        }

        private void ProcessContextItem(ToolStripItem param)
        {
            // categories
            if ((String)param.Tag == "Remove")
            {
                if (contextMenuStringColumn != null)
                {
                    filterCategories.Add(contextMenuStringColumn);
                    UpdateViewComponent();
                }
            }
            else if ((String)param.Tag == "ShowAll")
            {
                filterCategories.Clear();
                UpdateViewComponent();
            }
        }

    } // class

    internal class NameIDParams : NameIDPair
    {
        private String[] parameterValues;

        public String[] ParameterValues
        {
            get { return parameterValues; }
            set { parameterValues = value; }
        }

        public NameIDParams(String p_name, int p_id, String[] p_parameterValues) : base(p_name, p_id)
        {
            this.parameterValues = p_parameterValues;
        }
    }
}


