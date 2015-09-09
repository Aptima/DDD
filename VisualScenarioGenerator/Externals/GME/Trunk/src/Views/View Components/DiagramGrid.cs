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

namespace AME.Views.View_Components
{
    public class DiagramGrid : ParameterTable
    {
        private string linkType;

        public string LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }

        private String parameterName = "";

        public String ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        private List<ComponentPair> pairInformation;

        private Dictionary<int, int> tableIndexToComponentID;
        private Dictionary<int, int> componentIDToTableIndex;

        private int displayID;

        public int DisplayID
        {
            get { return displayID; }
            set { displayID = value; }
        }

        public DiagramGrid()
            : base()
        {
            this.InitializeComponent();
        }

        private const string linkRepresentation = "x";
        private const string linkWithNoValueRepresentation = "[ ]";

        protected override void LoadParameterData()
        {
            String IDAttribute = XmlSchemaConstants.Display.Component.Id;
            String NameAttribute = XmlSchemaConstants.Display.Component.Name;
            String LinkIDAttribute = XmlSchemaConstants.Display.Component.LinkID;

            IXPathNavigable iNavigator;

            if (displayID != SelectedID) // requires an additional level; base is now one node below root
            {
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.LevelDown = 3;
                compOptions.LinkParams = true;
                iNavigator = this.myController.GetComponentAndChildren(SelectedID, linkType, compOptions);
            }
            else
            {
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.LevelDown = 2;
                compOptions.LinkParams = true;
                iNavigator = this.myController.GetComponentAndChildren(SelectedID, linkType, compOptions);
            } 

            XPathNavigator navigator = iNavigator.CreateNavigator();

            DrawingUtility.SuspendDrawing(this);
            {
                this.Columns.Clear();
                pairInformation = new List<ComponentPair>();

                bool selectOffRoot = true;

                if (displayID != SelectedID)
                {
                    // set iterator to display ID
                    XPathNodeIterator iterator = navigator.Select("//Component[@" + IDAttribute + "='" + displayID + "']");

                    if (iterator.Count == 1)
                    {
                        iterator.MoveNext();
                        navigator = iterator.Current;
                        selectOffRoot = false; // different xpaths below
                    }
                }

                if (navigator.HasChildren)
                {
                    tableIndexToComponentID = new Dictionary<int, int>();
                    componentIDToTableIndex = new Dictionary<int,int>();

                    XPathNodeIterator listOfNodes;

                    if (selectOffRoot)
                    {
                        listOfNodes = navigator.Select("/Components/Component/Component"); // path relative to root
                    }
                    else
                    {
                        listOfNodes = navigator.Select("Component"); // path relative to any component
                    }

                    List<NameIDPair> nameIDs = new List<NameIDPair>();

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        String nodeName = nodeNav.GetAttribute(NameAttribute, nodeNav.NamespaceURI);
                        int nodeID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));
                        
                        NameIDPair addPair = new NameIDPair(nodeName, nodeID);
                        nameIDs.Add(addPair);
                   }

                    nameIDs.Sort();

                    foreach (NameIDPair aPair in nameIDs)
                    {
                        int addedColumn = Columns.Add(aPair.Name, aPair.Name);
                        int addedRow = this.Rows.Add();

                        this.Columns[addedColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.Columns[addedColumn].MinimumWidth = this.Columns[addedColumn].HeaderCell.PreferredSize.Width;

                        Rows[addedRow].HeaderCell.Value = aPair.Name;
                        tableIndexToComponentID.Add(addedRow, aPair.Id);
                        componentIDToTableIndex.Add(aPair.Id, addedRow);
                    }

                    int fromID, toID;
                    int rowIndex, columnIndex, linkID;
                    String linkString = "", paramValue = "";

                    //now, form links.

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        fromID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));
                        rowIndex = componentIDToTableIndex[fromID];

                        if (nodeNav.HasChildren)
                        {
                            XPathNodeIterator listOfChildren = nodeNav.Select("Component");

                            foreach (XPathNavigator childNav in listOfChildren)
                            {
                                toID = Int32.Parse(childNav.GetAttribute(IDAttribute, childNav.NamespaceURI));

                                if (componentIDToTableIndex.ContainsKey(toID))
                                {
                                    columnIndex = componentIDToTableIndex[toID];
                                    linkString = childNav.GetAttribute(LinkIDAttribute, childNav.NamespaceURI);

                                    if (linkString != "")
                                    {
                                        linkID = Int32.Parse(linkString);
                                    }
                                    else
                                    {
                                        linkID = -1;
                                    }

                                    pairInformation.Add(new ComponentPair(fromID, toID, linkID));

                                    XPathNavigator pnav = childNav.SelectSingleNode("LinkParameters/Parameter/Parameter");
                                    if (pnav != null)
                                    {
                                        paramValue = pnav.GetAttribute("value", pnav.NamespaceURI);

                                        if (paramValue.Equals(""))
                                        {
                                            this[columnIndex, rowIndex].Value = linkWithNoValueRepresentation;
                                        }
                                        else
                                        {
                                            this[columnIndex, rowIndex].Value = paramValue;
                                        }
                                    }
                                    else
                                    {
                                        this[columnIndex, rowIndex].Value = linkRepresentation;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //protected override void OnCellLeave(DataGridViewCellEventArgs e)
        //{
        //    if (updateOnCellLeave)
        //    {
        //        int row = e.RowIndex;
        //        int column = e.ColumnIndex;

        //        if (this[column, row].EditedFormattedValue != null)
        //        {
        //            String currentValue = this[column, row].EditedFormattedValue.ToString(); // get the value of the cell
        //            if (previousValue.Equals(String.Empty) && currentValue.Equals(String.Empty))
        //            {
        //                // do nothing for now
        //            }
        //            else
        //            {
        //                keyEnter();
        //            }
        //        }
        //    }
        //}

        protected override void keyEnter()
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
                try
                {
                    int linkID = -1, fromID = -1, toID = -1;

                    if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(row))
                    {
                        fromID = tableIndexToComponentID[row];
                    }

                    if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(column))
                    {
                        toID = tableIndexToComponentID[column];
                    }

                    if (fromID == -1 || toID == -1)
                    {
                        MessageBox.Show("Can't index row/column for grid display");
                    }

                    bool found = false;
                    foreach (ComponentPair pair in pairInformation)
                    {
                        if (pair.FromID.Equals(fromID) && pair.ToID.Equals(toID))
                        {
                            found = true;
                            linkID = pair.LinkID;
                        }
                    }

                    if (found)
                    {
                        if (value.Equals("d")) // delete link
                        {
                            myController.DeleteLink(linkID);
                        }
                        else if (value.Equals(String.Empty)) // non parameter link, when push empty string, delete link
                        {
                            myController.DeleteLink(linkID);
                        }
                        else if (parameterName.Length > 0) // just update param value
                        {
                            myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                        }

                        else // non parameter link, link is already formed, nothing is done, so just recopy the 'x'
                        {    // for visual consistency
                            this.CurrentCell.Value = previousValue;
                        }
                    }
                    else // not found - form a link
                    {
                        myController.TurnViewUpdateOff();

                        linkID = myController.Connect(this.SelectedID, fromID, toID, this.linkType);

                        if (parameterName.Length > 0)
                        {
                            myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                            myController.TurnViewUpdateOn(); // refresh
                        }
                        else
                        {
                            myController.TurnViewUpdateOn(); // refresh
                        }
                    } // form link 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                    // on exception restore value

                    if (myController.ViewUpdateStatus == false)
                    {
                        //this.crc = "";
                        myController.TurnViewUpdateOn(); // refresh
                    }
                    else
                    {
                        this.UpdateViewComponent();
                    }
                } // ex
                finally
                {
                    if (myController.ViewUpdateStatus == false)
                    {
                        myController.TurnViewUpdateOn(); // refresh
                    }
                }
            }
        }

        // on return, try push the value down, return success/fail
        //protected override bool ParameterTable_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        this.CommitEdit(DataGridViewDataErrorContexts.Commit); // force the edit to complete
        //        // so the new value is available
        //        // note: don't use endedit! will restore value
        //        int row = this.CurrentCell.RowIndex;
        //        int column = this.CurrentCell.ColumnIndex;

        //        if (Columns[column].ReadOnly != true)
        //        {
        //            String value = "";
        //            if (this[column, row].Value != null)
        //            {
        //                value = this[column, row].Value.ToString(); // get the value of the cell
        //            }
        //            try
        //            {
        //                int linkID = -1, fromID = -1, toID = -1;

        //                if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(row))
        //                {
        //                    fromID = tableIndexToComponentID[row];
        //                }

        //                if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(column))
        //                {
        //                    toID = tableIndexToComponentID[column];
        //                }

        //                if (fromID == -1 || toID == -1)
        //                {
        //                    MessageBox.Show("Can't index row/column for grid display");
        //                    return false;
        //                }

        //                bool found = false;
        //                foreach (ComponentPair pair in pairInformation)
        //                {
        //                    if (pair.FromID.Equals(fromID) && pair.ToID.Equals(toID))
        //                    {
        //                        found = true;
        //                        linkID = pair.LinkID;
        //                    }
        //                }

        //                if (found)
        //                {
        //                    if (value.Equals("d")) // delete link
        //                    {
        //                        myController.DeleteLink(linkID);
        //                    }
        //                    else if (parameterName.Length > 0) // just update param value
        //                    {
        //                        myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
        //                    }
        //                    else if (value.Equals(String.Empty)) // non parameter link, when push empty string, delete link
        //                    {
        //                        myController.DeleteLink(linkID);
        //                    }
        //                }
        //                else // not found - form a link
        //                {
        //                    myController.TurnViewUpdateOff();

        //                    myController.Connect(this.SelectedID, fromID, toID, this.linkType);

        //                    if (parameterName.Length > 0)
        //                    {
        //                        this.UpdateViewComponent();

        //                        // TODO fix link ID here
        //                        foreach (ComponentPair pair in pairInformation)
        //                        {
        //                            if (pair.FromID.Equals(fromID) && pair.ToID.Equals(toID))
        //                            {
        //                                linkID = pair.LinkID;
        //                            }
        //                        }

        //                        myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
        //                        myController.TurnViewUpdateOn(); // refresh
        //                    }
        //                    else
        //                    {
        //                        myController.TurnViewUpdateOn(); // refresh
        //                    }
        //                } // form link 
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
        //                // on exception restore value

        //                if (myController.ViewUpdateStatus == false)
        //                {
        //                    //this.crc = "";
        //                    myController.TurnViewUpdateOn(); // refresh
        //                }
        //                else
        //                {
        //                    this.UpdateViewComponent();
        //                }
        //            } // ex

        //            return true;
        //        } // read only
        //    } // enter
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
        //} // method

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
                            String value = elementsPerLine[j];

                            int linkID = -1, fromID = -1, toID = -1;

                            if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(row+i))
                            {
                                fromID = tableIndexToComponentID[row+i];
                            }

                            if (tableIndexToComponentID != null && tableIndexToComponentID.ContainsKey(col+j))
                            {
                                toID = tableIndexToComponentID[col+j];
                            }

                            if (fromID == -1 || toID == -1)
                            {
                                MessageBox.Show("Can't index row/column for grid display");
                                return;
                            }

                            bool found = false;
                            foreach (ComponentPair pair in pairInformation)
                            {
                                if (pair.FromID.Equals(fromID) && pair.ToID.Equals(toID))
                                {
                                    found = true;
                                    linkID = pair.LinkID;
                                }
                            }

                            if (found)
                            {
                                if (parameterName.Length > 0)
                                {
                                    if (value.Equals(linkWithNoValueRepresentation)) // can't send down "[ ]"
                                    {
                                        value = String.Empty;
                                    }
                                    myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                                }
                            }
                            else // not found - form a link
                            {
                                linkID = myController.Connect(this.SelectedID, fromID, toID, this.linkType);

                                if (parameterName.Length > 0)
                                {
                                    this.UpdateViewComponent();

                                    if (!value.Equals(linkWithNoValueRepresentation)) // [ ] has no value to use.
                                    {
                                        myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                                    }
                                }
                            } // form link 
                        }// bounds check
                    } // foreach value
                } // foreach line
            }
            
            SendKeys.Send("{ESC}");

            myController.TurnViewUpdateOn();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DiagramGrid
            // 
            //this.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DiagramGrid_CellEnter);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    } // class

    internal class NameIDPair : IComparable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public NameIDPair(String p_name, int p_id)
        {
            name = p_name;
            id = p_id;
        }

        public int CompareTo(object newObj)
        {
            if (newObj is NameIDPair)
            {
                NameIDPair toCompare = (NameIDPair)newObj;

                return this.name.CompareTo(toCompare.name);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class ComponentPair
    {
        private int linkID;

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        private int fromID, toID;

        public int FromID
        {
            get { return fromID; }
            set { fromID = value; }
        }

        public int ToID
        {
            get { return toID; }
            set { toID = value; }
        }

        public ComponentPair(int p_from, int p_to, int p_linkID)
        {
            fromID = p_from;
            toID = p_to;
            linkID = p_linkID;
        }
    }
}


