using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Xml;
using AME.Model;
using System.Security.Cryptography;
using System.Xml.XPath;
using Forms;

namespace AME.Views.View_Components
{
    // A table to store name/value parameter pairs

    public partial class ParameterTable : DataGridView, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        protected IController myController;

        private int m_SelectedID;
        
        protected Boolean updateOnCellLeave;

        protected string crc = "";

        protected Boolean idChange = false;

        public int SelectedID
        {
            get { return m_SelectedID; }
            set { m_SelectedID = value;
                  idChange = true;
                }
        }

        protected eParamParentType m_selectedIDType = eParamParentType.Component;

        public eParamParentType SelectedIDType
        {
            get { return m_selectedIDType; }
            set { m_selectedIDType = value; }
        }

        public ParameterTable()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            updateOnCellLeave = false;

            InitializeComponent();

            this.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.Dock = DockStyle.Fill;

            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.BackgroundColor = SystemColors.Window;

            this.EditMode = DataGridViewEditMode.EditOnEnter;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToResizeRows = false;
            this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            // size to the column header text lengths, don't allow the user to resize
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            this.CellBeginEdit += new DataGridViewCellCancelEventHandler(ParameterTable_CellBeginEdit);
            //this.CellEndEdit += new DataGridViewCellEventHandler(ParameterTable_CellEndEdit);
        }

        #region ViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        private Point recordCurrentCell;

        protected void SaveCurrentCell()
        {
            if (this.CurrentCell != null)
            {
                recordCurrentCell = new Point(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex);
                this.CurrentCell.Selected = false;
            }
            else
            {
                recordCurrentCell = new Point(-1, -1);
            }
        }

        protected void RestoreCurrentCell()
        {
            int row = recordCurrentCell.X;
            int col = recordCurrentCell.Y;

            if (row != -1 && col != -1 && row < this.Rows.Count && col < this.Columns.Count && this[col, row] != null && !idChange)
            {
                this.CurrentCell = this[col, row];

                // if we reload a cell from a DB refresh, the previous value is now the *new* previous value.
                if (this.CurrentCell.Value != null)
                {
                    previousValue = this.CurrentCell.Value.ToString();
                }
                else
                {
                    previousValue = "";
                }
            }
            else
            {
                idChange = false;
                this.CurrentCell = null;
                previousValue = "";
            }

            if (DrawingUtility.Suspend)
            {
                DrawingUtility.ResumeDrawing(this);
            }
        }

        public void UpdateViewComponent()
        {
            if (this.Parent != null)
            {
                updateOnCellLeave = false;

                ISupportInitialize castForBeginEnd = (ISupportInitialize)this;

                castForBeginEnd.BeginInit();

                this.SuspendLayout();

                SaveCurrentCell();

                LoadParameterData();

                RestoreCurrentCell();

                this.ResumeLayout();

                castForBeginEnd.EndInit();

                updateOnCellLeave = true;
            }
        }

        protected virtual void LoadParameterData()
        {
            IXPathNavigable iNavigator = myController.GetParametersForComponent(SelectedID);
            XPathNavigator navigator = iNavigator.CreateNavigator();

            DrawingUtility.SuspendDrawing(this);
            {
                this.Columns.Clear();

                bool columnsLoaded = false;

                XPathNodeIterator parameterNodes;

                if (m_selectedIDType == eParamParentType.Component)
                {

                    parameterNodes = navigator.Select("/ComponentParameters/Parameter");
                }
                else if (m_selectedIDType == eParamParentType.Link)
                {
                    parameterNodes = navigator.Select("/LinkParameters/Parameter");
                }
                else
                {
                    return;
                }

                foreach (XPathNavigator paramNav in parameterNodes)
                {
                    bool attrTest = paramNav.MoveToFirstAttribute();

                    if (columnsLoaded == false)
                    {
                        while (attrTest != false)
                        {
                            this.Columns.Add(paramNav.Name, paramNav.Name);
                            attrTest = paramNav.MoveToNextAttribute();
                        }

                        if (this.Columns.Contains(SchemaConstants.Type))
                        {
                            this.Columns[SchemaConstants.Type].ReadOnly = true;
                        }
                        if (this.Columns.Contains(SchemaConstants.Name))
                        {
                            this.Columns[SchemaConstants.Name].ReadOnly = true;
                        }
                        columnsLoaded = true;
                    }

                    paramNav.MoveToParent();

                    List<String> attributeValues = new List<string>();

                    attrTest = paramNav.MoveToFirstAttribute();
                    while (attrTest != false)
                    {
                        attributeValues.Add(paramNav.Value);
                        attrTest = paramNav.MoveToNextAttribute();
                    }

                    this.Rows.Add(attributeValues.ToArray());
                }
            }

            // sort by name, if applicable, before resetting selected cell
            if (this.Columns.Contains(SchemaConstants.Name))
            {
                this.Sort(this.Columns[SchemaConstants.Name], ListSortDirection.Ascending);
            }
        }
        #endregion

        // It seems I have to override the key events to
        // redirect a keypress in edit mode - for example return to commit data
        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Enter || (key == Keys.V && ModifierKeys == Keys.Control) || key == Keys.Escape)
            {
                return this.ParameterTable_KeyDown(this, new KeyEventArgs(keyData)); // redirect key event   
            }
            return base.ProcessDialogKey(keyData);
        }

        protected virtual void Paste()
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
                            String paramName = this[SchemaConstants.Name, row + i].Value.ToString(); // name
                            myController.UpdateParameters(SelectedID, paramName, elementsPerLine[j], eParamParentType.Component);
                        }
                    }  
                }
            }
            myController.TurnViewUpdateOn();
        }

        protected virtual void keyEnter()
        {
            this.CommitEdit(DataGridViewDataErrorContexts.Commit); // force the edit to complete
            // so the new value is available
            // note: don't use endedit! will restore value
            int row = this.CurrentCell.RowIndex;
            int column = this.CurrentCell.ColumnIndex;

            if (Columns[column].ReadOnly != true)
            {
                String value = "";
                if (this[column, row].Value != null)
                {
                    value = this[column, row].Value.ToString(); // get the value of the cell
                }
                String name = this[SchemaConstants.Name, row].Value.ToString(); // name

                try
                {
                    myController.UpdateParameters(SelectedID, name, value, this.m_selectedIDType); // push
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                    // on exception restore value
                    this.UpdateViewComponent();
                }
            }
        }

        // on return, try push the value down, return success/fail
        protected virtual bool ParameterTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                keyEnter();
            }
            else if (e.KeyCode == Keys.V && ModifierKeys == Keys.Control)
            {
                try
                {
                    Paste();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                    // on exception restore value

                    if (myController.ViewUpdateStatus == false)
                    {
                        myController.TurnViewUpdateOn(); // refresh
                    }
                    else
                    {
                        this.UpdateViewComponent();
                    }
                } // ex
            }
            else if (e.KeyCode == Keys.Escape)
            {
                UpdateViewComponent();
            }

            return true;
        }

        protected String previousValue = "";

        // save previous value on an edit
        private void ParameterTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (updateOnCellLeave) // are we in the middle of UpdateViewComponent()
            {
                if (this[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    previousValue = this[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
                else
                {
                    previousValue = "";
                }
            }
        }

        // restore previous value on an end edit
        // we'll be committing manually so anything else that generates an end edit event
        // (focus, changing cells) will revert the value
        //private void ParameterTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (this[e.ColumnIndex, e.RowIndex].Value != null)
        //    {
        //        String currentValue = this[e.ColumnIndex, e.RowIndex].Value.ToString();

        //        if (!currentValue.Equals(previousValue))
        //        {
        //            this[e.ColumnIndex, e.RowIndex].Value = previousValue;
        //        }
        //    }
        //}

        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            if (updateOnCellLeave) // are we in the middle of UpdateViewComponent?
            {
                DataGridViewCell leaveCell = this[e.ColumnIndex, e.RowIndex];
                // only try to push down if the value changes
                if (leaveCell.EditedFormattedValue != null && leaveCell.EditedFormattedValue.ToString() != previousValue)
                {
                    base.OnCellLeave(e);
                    keyEnter();
                }
            }
        }
    }
}



