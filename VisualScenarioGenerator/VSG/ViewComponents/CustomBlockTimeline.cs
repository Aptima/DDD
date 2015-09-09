using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using VSG.Controllers;


using AME;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Nodes;

namespace VSG.ViewComponents
{
    public delegate void CustomBlockTimelineSelectHandler(CustomBlockTimelineCell cell);

    public partial class CustomBlockTimeline : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController controller = null;
        public int RootID = -1;
        public int DisplayID = -1;
        public string LinkType = string.Empty;

        #region Attributes
        public int RowHeight = 40;
        public int ColumnWidth = 100;
        public SizeType SizeType = SizeType.Absolute;

        public int LastRowIndex
        {
            get
            {
                return tableLayoutPanel1.RowCount - 1;
            }
        }
        public int LastColumnIndex
        {
            get
            {
                return tableLayoutPanel1.ColumnCount - 1;
            }
        }

        public ControlCollection Cells
        {
            get
            {
                return tableLayoutPanel1.Controls;
            }
        }

        private TableLayoutPanelCellPosition _current_position = new TableLayoutPanelCellPosition(0, 0);
        public TableLayoutPanelCellPosition CurrentPosition
        {
            get
            {
                return _current_position;
            }
        }

        public CustomBlockTimelineSelectHandler AfterSelect = null;
        #endregion


        /// <summary>
        /// The Custom Block Timeline is a two dimensional grid that allows us to represent time as stackable
        /// blocks.  Blocks stacked horizontally and vertically.
        /// 
        /// The purpose of this control is to emphasize and display time-based "Events" without needlessy showing
        /// the gaps of "empty" time between them.  
        /// </summary>
        public CustomBlockTimeline()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
            this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.tableLayoutPanel1.Dock = DockStyle.Fill;

            InitializeTableStyles();

            DoubleBuffered = true;

        }

        /// <summary>
        /// Erases the entire contents (Cells) of the Block Timeline.
        /// </summary>
        public void ClearTimeline()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.RowCount = 2;
        }

        /// <summary>
        /// Creates a new empty row in the Block Timeline.
        /// </summary>
        /// <returns>Row Count</returns>
        public int CreateRow()
        {
            if (!IsRowEmpty(LastRowIndex))
            {
                this.tableLayoutPanel1.RowCount++;
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType, RowHeight));
            }
            return tableLayoutPanel1.RowCount;
        }



        /// <summary>
        /// Creates a new empty column in the Block Timeline
        /// </summary>
        /// <returns>Column Count</returns>
        public int CreateColumn()
        {
            if (!IsColumnEmpty(LastColumnIndex))
            {
                this.tableLayoutPanel1.ColumnCount++;
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType, ColumnWidth));
            }
            return tableLayoutPanel1.ColumnCount;
        }



        /// <summary>
        /// Moves a timeline cell block to another cell block in the timeline.  If the cell block is not empty, 
        /// then the cell blocks will switch position.  (This may not be desired behavior).
        /// </summary>
        /// <param name="row1">Source Row</param>
        /// <param name="col1">Source Column</param>
        /// <param name="row2">Destination Row</param>
        /// <param name="col2">Destination Column</param>
        public void MoveCell(int row1, int col1, int row2, int col2)
        {

            if (IsValidRow(row1) && IsValidRow(row2) && IsValidColumn(col1) && IsValidColumn(col2) &&
                !IsEqual(row1, col1, row2, col2))
            {
                CustomBlockTimelineCell src_cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col1, row1);
                CustomBlockTimelineCell dest_cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col2, row2);

                if (src_cell != null)
                {
                    tableLayoutPanel1.Controls.Remove(src_cell);
                    if (dest_cell != null)
                    {
                        tableLayoutPanel1.SetCellPosition(dest_cell, new TableLayoutPanelCellPosition(col1, row1));
                    }
                    else
                    {
                        ShiftRowsUp(row1, col1);
                    }
                    tableLayoutPanel1.Controls.Add(src_cell, col2, row2);

                    //DeSelectCell(GetCurrentCell());
                    //SelectCell(row2, col2);
                }
            }
        }

        /// <summary>
        /// Changes the order value of the cell at location row1, col1 to be order.
        /// This changes the position of the cell in the timeline.
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="order"></param>
        public void ChangeCellOrder(int row1, int col1, int order)
        {
            // find the column
            CustomBlockTimelineCell cell = GetCellAt(row1, col1);
            if (cell != null)
            {
                DeleteCell(row1, col1);
                AddCell(cell.Text, order);
            }
        }

        /// <summary>
        /// Allows a CellBlock to be associated with a runtime object.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="payload"></param>
        public void SetCellPayload(int row, int col, object payload)
        {
            CustomBlockTimelineCell cell = GetCellAt(row, col);
            if (cell != null)
            {
                cell.Payload = payload;
            }
        }

        public void SetCellPayload(CustomBlockTimelineCell cell, object payload)
        {
            if (cell != null)
            {
                cell.Payload = payload;
            }
        }


        /// <summary>
        /// Deletes the cell at the specified row and column.
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        public void DeleteCell(int row1, int col1)
        {
            CustomBlockTimelineCell src_cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col1, row1);
            DeleteCell(src_cell);
        }

        /// <summary>
        /// Deletes the specified cell.
        /// </summary>
        /// <param name="cell"></param>
        public void DeleteCell(CustomBlockTimelineCell cell)
        {
            if (cell != null)
            {
                TableLayoutPanelCellPosition pos =  tableLayoutPanel1.GetCellPosition(cell);
                tableLayoutPanel1.Controls.Remove(cell);
                if (IsColumnEmpty(pos.Column))
                {
                    ShiftColumnsLeft(pos.Column);
                    ReduceColumns();
                }
                else
                {
                    ShiftRowsUp(pos.Row, pos.Column);
                    ReduceRows();
                }
                //DeSelectCell(GetCurrentCell());
                //SelectCell(0, 0);
            }
        }
        /// <summary>
        /// Inserts a new cell block into the cell timeline.  A cell block will automatically be inserted "horizontally" after
        /// the last horizontal cell block in the timeline.
        /// Each cell block has an "order" value, that determines its position in the block timeline.
        /// For example 2 will be inserted before 3, but after 0 and 1.  If we're inserting 2 and 2 already exists the cell block will 
        /// be inserted "vertically" after the previous cell block 2.
        /// 
        /// **This is a recursive routine and relies on "private" version of AddCell defined in the "Private Methods" region 
        /// of this file.
        /// </summary>
        /// <param name="text">The text to appear in the cell block.</param>
        /// <param name="order">The cell blocks order value.</param>
        public CustomBlockTimelineCell AddCell(string text, int order)
        {
            int row = 0;
            int col = 0;

            CustomBlockTimelineCell current_cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(_current_position.Column, _current_position.Row);
            //if (current_cell != null)
            //{
            //    current_cell.IsSelected = false;
            //}


            CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(LastColumnIndex, 0);
            if (cell != null)
            {
                //cell.IsSelected = false;

                if (order > cell.Order)
                {
                    CreateColumn();

                    cell = CreateCell(order, text);
                    tableLayoutPanel1.Controls.Add(cell, LastColumnIndex, 0);
                    //SelectCell(cell);

                    return cell;
                }
            }

            return AddCell(text, row, col, order);
        }


        /// <summary>
        /// Shifts a row of cell blocks downward.
        /// </summary>
        /// <param name="starting_row">Starting Row.</param>
        /// <param name="starting_column">Starting Column.</param>
        public void ShiftRowsDown(int starting_row, int starting_column)
        {
            if (GetLastOccupiedRow(starting_column) == LastRowIndex - 1)
            {
                this.tableLayoutPanel1.RowCount++;
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType, RowHeight));
            }
            for (int row = LastRowIndex - 1; row >= starting_row; row--)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(starting_column, row);
                if (cell != null)
                {
                    DeSelectCell(cell);
                    tableLayoutPanel1.SetCellPosition(cell, new TableLayoutPanelCellPosition(starting_column, row + 1));
                }
            }
            //SelectCell(0, 0);
        }


        /// <summary>
        /// Shifts a row of cell blocks upward.
        /// </summary>
        /// <param name="starting_row">Starting row</param>
        /// <param name="starting_column">Starting column</param>
        public void ShiftRowsUp(int starting_row, int starting_column)
        {
            for (int row = starting_row; row <= LastRowIndex; row++)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(starting_column, row);
                if (cell != null)
                {
                    DeSelectCell(cell);
                    tableLayoutPanel1.SetCellPosition(cell, new TableLayoutPanelCellPosition(starting_column, row - 1));
                }
            }
            //SelectCell(0, 0);
        }

        public void SelectCell(int row, int col)
        {
            SelectCell(GetCellAt(row, col));
        }
        public void SelectCell(CustomBlockTimelineCell cell)
        {
            if (cell != null)
            {
                cell.IsSelected = true;
                _current_position = tableLayoutPanel1.GetPositionFromControl(cell);
                Console.WriteLine("CurrentPosition {0}", _current_position.ToString());
            }
        }

        public void DeSelectCell()
        {
            DeSelectCell(GetCurrentCell());
        }
        public void DeSelectCell(int row, int col)
        {
            DeSelectCell(GetCellAt(row, col));
        }
        public void DeSelectCell(CustomBlockTimelineCell cell)
        {
            if (cell != null)
            {
                cell.IsSelected = false;
                //_current_position = new TableLayoutPanelCellPosition(0, 0);
            }
        }

        #region Private Methods/Operations
        private void InitializeTableStyles()
        {
            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                this.tableLayoutPanel1.RowStyles[row].SizeType = SizeType;
                this.tableLayoutPanel1.RowStyles[row].Height = RowHeight;
                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    this.tableLayoutPanel1.ColumnStyles[col].SizeType = SizeType;
                    this.tableLayoutPanel1.ColumnStyles[col].Width = ColumnWidth;
                }
            }
        }

        private CustomBlockTimelineCell CreateCell(int order, string text)
        {
            CustomBlockTimelineCell cell = new CustomBlockTimelineCell();
            cell.Order = order;
            cell.Text = text;
            cell.IsSelected = false;
            cell.Dock = DockStyle.Fill;
            cell.MouseUp += new MouseEventHandler(On_MouseUp);
            return cell;
        }

        private bool IsValidRow(int row)
        {
            return ((row >= 0) && (row <= LastRowIndex - 1));
        }
        private bool IsValidColumn(int col)
        {
            return ((col >= 0) && (col <= LastColumnIndex));
        }
        private bool IsEqual(int row1, int col1, int row2, int col2)
        {
            return ((row1 == row2) && (col1 == col2));
        }

        private CustomBlockTimelineCell AddCell(string text, int row, int col, int order)
        {
            // Orient myself to the right column
            CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
            if (cell != null)
            {
                //cell.IsSelected = false;

                if (order < cell.Order)
                {
                    ShiftColumnsRight(col);

                    cell = CreateCell(order, text);
                    tableLayoutPanel1.Controls.Add(cell, col, row);

                    //DeSelectCell(GetCurrentCell());
                    //SelectCell(cell);

                    return cell;
                }
                else
                    if (order == cell.Order)
                    {
                        return AddCell(text, row + 1, col, order);
                    }
                    else
                    {
                        return AddCell(text, row, col + 1, order);
                    }
            }

            cell = CreateCell(order, text);
            tableLayoutPanel1.Controls.Add(cell, col, row);
            //DeSelectCell(GetCurrentCell());
            //SelectCell(cell);

            if (col == LastColumnIndex)
            {
                CreateColumn();
            }
            if (row == LastRowIndex)
            {
                CreateRow();
            }

            return cell;
        }

        private void ShiftColumnsRight(int starting_column)
        {
            if (IsColumnEmpty(LastColumnIndex))
            {
                this.tableLayoutPanel1.ColumnCount++;
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType, ColumnWidth));
            }

            for (int col = LastColumnIndex - 1; col >= starting_column; col--)
            {
                for (int row = 0; row <= LastRowIndex; row++)
                {
                    if (LastColumnIndex == GetLastOccupiedColumn(row))
                    {
                        CreateColumn();
                    }

                    CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
                    if (cell != null)
                    {
                        cell.IsSelected = false;
                        tableLayoutPanel1.SetCellPosition(cell, new TableLayoutPanelCellPosition(col + 1, row));
                    }
                }
            }
        }

        private void ShiftColumnsLeft(int starting_column)
        {
            if (starting_column >= LastColumnIndex)
            {
                return;
            }
            if (IsColumnEmpty(starting_column))
            {
                for (int row = 0; row <= LastRowIndex; row++)
                {
                    Control c = GetCellAt(row, starting_column + 1);
                    if (c != null)
                    {
                        tableLayoutPanel1.SetCellPosition(c, new TableLayoutPanelCellPosition(starting_column, row));
                        //this.MoveCell(row, starting_column + 1, row, starting_column);
                    }
                }
                ShiftColumnsLeft(starting_column + 1);
            }
        }



        private CustomBlockTimelineCell GetCurrentCell()
        {
            return (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(_current_position.Column, _current_position.Row);
        }
        private CustomBlockTimelineCell GetCellAt(int row, int col)
        {
            return (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
        }
        private CustomBlockTimelineCell GetLastCellByColumn(int col)
        {
            return (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, GetLastOccupiedRow(col));
        }

        private CustomBlockTimelineCell GetLastCellByRow(int row)
        {
            return (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(GetLastOccupiedColumn(row), row);
        }

        private int GetLastOccupiedRow(int col)
        {
            for (int row = LastRowIndex; row >= 0; row--)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
                if (cell != null)
                {
                    return row;
                }
            }
            return 0;
        }

        private int GetLastOccupiedColumn(int row)
        {
            for (int col = LastColumnIndex; col >= 0; col--)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
                if (cell != null)
                {
                    return col;
                }
            }
            return 0;
        }

        private bool IsRowEmpty(int row)
        {
            for (int col = 0; col <= LastColumnIndex; col++)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
                if (cell != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsColumnEmpty(int col)
        {
            for (int row = 0; row <= LastRowIndex; row++)
            {
                CustomBlockTimelineCell cell = (CustomBlockTimelineCell)tableLayoutPanel1.GetControlFromPosition(col, row);
                if (cell != null)
                {
                    return false;
                }
            }

            return true;
        }

        private void ReduceColumns()
        {
            if (LastColumnIndex > 1)
            {
                while (IsColumnEmpty(LastColumnIndex - 1))
                {
                    tableLayoutPanel1.ColumnCount--;
                }
            }
        }
        private void ReduceRows()
        {
            if (LastRowIndex > 0)
            {
                while (IsRowEmpty(LastRowIndex - 1))
                {
                    tableLayoutPanel1.RowCount--;
                }
            }
        }

        #endregion


        #region WinForms Event Handlers
        private void On_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("My MouseUp");
            DeSelectCell(GetCurrentCell());
            CustomBlockTimelineCell cell = (CustomBlockTimelineCell)sender;
            SelectCell(cell);
            if (AfterSelect != null)
            {
                AfterSelect(cell);
            }
        }
        #endregion


        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = (VSGController)value;
            }
        }

        private static string CreateNode = "//*[@Type='CreateEvent' and @ID='{0}']/*";
        private static string NodeTime = "//*[@Type='CreateEvent' and @ID='{0}']/Component[@Type='{1}' and @ID='{2}']/ComponentParameters/Parameter[@category='{1}']/Parameter[@displayedName='Time']";

        public static string ReiterateNodes = "/Components/Component/Component[@Type='ReiterateEvent']";
        public static string OpenChatroomNodes = "/Components/Component/Component[@Type='OpenChatRoomEvent']";
        public static string CloseChatroomNodes = "/Components/Component/Component[@Type='CloseChatRoomEvent']";
        public static string OpenVoiceChannelNodes = "/Components/Component/Component[@Type='OpenVoiceChannelEvent']";
        public static string CloseVoiceChannelNodes = "/Components/Component/Component[@Type='CloseVoiceChannelEvent']";
        public static string RemoveEngramNodes = "/Components/Component/Component[@Type='RemoveEngramEvent']";
        public static string ChangeEngramNodes = "/Components/Component/Component[@Type='ChangeEngramEvent']";
        public static string FlushEventNodes = "/Components/Component/Component[@Type='FlushEvent']";

        public static string ReiterateNode = "/Components/Component/Component[@Type='ReiterateEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string OpenChatroomNode = "/Components/Component/Component[@Type='OpenChatRoomEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string CloseChatroomNode = "/Components/Component/Component[@Type='CloseChatRoomEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string OpenVoiceChannelNode = "/Components/Component/Component[@Type='OpenVoiceChannelEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string CloseVoiceChannelNode = "/Components/Component/Component[@Type='CloseVoiceChannelEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string RemoveEngramNode = "/Components/Component/Component[@Type='RemoveEngramEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string ChangeEngramNode = "/Components/Component/Component[@Type='ChangeEngramEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";
        public static string FlushEventNode = "/Components/Component/Component[@Type='FlushEvent' and @ID='{0}']/ComponentParameters/Parameter/Parameter[@displayedName='Time']";


        public void UpdateViewComponent()
        {
            DrawingUtility.SuspendDrawing(this);
            ClearTimeline();

            ComponentOptions comp = new ComponentOptions();
            comp.CompParams = true;
            IXPathNavigable document = controller.GetComponentAndChildren(RootID, LinkType, comp);
            XPathNavigator navigator = document.CreateNavigator();

            XPathNodeIterator listofRootElements;
            listofRootElements = navigator.Select(string.Format(CreateNode, DisplayID));
            
            if (listofRootElements != null)
            {
                if (listofRootElements.Count > 0)
                {
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string event_type = node.GetAttribute("Type", node.NamespaceURI);
                        string cmp_id = node.GetAttribute("ID", node.NamespaceURI);

                        if (event_type.Contains("Event") && (event_type != "CompletionEvent"))
                        {
                            XPathNavigator event_node = navigator.SelectSingleNode(string.Format(NodeTime, DisplayID, event_type, cmp_id));
                            string time = event_node.GetAttribute("value", event_node.NamespaceURI);
                            SetCellPayload(AddCell(event_type, Convert.ToInt32(time)), Convert.ToInt32(cmp_id));
                        }
                    }
                }
                else
                {
                    listofRootElements = navigator.Select(ReiterateNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(ReiterateNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("ReiterateEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(OpenChatroomNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(OpenChatroomNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("OpenChatRoomEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(CloseChatroomNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(CloseChatroomNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("CloseChatRoomEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(OpenVoiceChannelNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(OpenVoiceChannelNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("OpenVoiceChannelEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(CloseVoiceChannelNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(CloseVoiceChannelNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("CloseVoiceChannelEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(RemoveEngramNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(RemoveEngramNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("RemoveEngramEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(ChangeEngramNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(ChangeEngramNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("ChangeEngramEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                    listofRootElements = navigator.Select(FlushEventNodes);
                    foreach (XPathNavigator node in listofRootElements)
                    {
                        string id = node.GetAttribute("ID", node.NamespaceURI);

                        XPathNavigator time_node = navigator.SelectSingleNode(string.Format(FlushEventNode, id));
                        string time = time_node.GetAttribute("value", time_node.NamespaceURI);

                        SetCellPayload(AddCell("FlushEvent", Convert.ToInt32(time)), Convert.ToInt32(id));
                    }

                }

            }
            DrawingUtility.ResumeDrawing(this);
        }



        #endregion
    }
}
