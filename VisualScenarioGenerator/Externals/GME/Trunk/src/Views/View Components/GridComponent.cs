using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Xml.XPath;
using Model;
using System.ComponentModel;
using Controllers;
using System.Data;
using System.Windows.Forms;
using Forms;

namespace View_Components
{
    public class GridComponent : ParameterTable
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

        private Dictionary<String, int> nodeNameToID;

        public GridComponent()
            : base()
        {

        }

        protected override void LoadParameterData()
        {
            String IDAttribute = XmlSchemaConstants.Display.Component.Id;
            String NameAttribute = XmlSchemaConstants.Display.Component.Name;
            String LinkIDAttribute = "LinkID";

            IXPathNavigable iNavigator = myController.GetComponentAndChildren(SelectedID, linkType, 2, false, true);
            XPathNavigator navigator = iNavigator.CreateNavigator();

            //// Perform a CRC checksum on the data coming back, only update if it has changed
            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //Byte[] bs = System.Text.Encoding.UTF8.GetBytes(navigator.OuterXml);
            //bs = x.ComputeHash(bs);
            //System.Text.StringBuilder s = new System.Text.StringBuilder();
            //foreach (Byte b in bs)
            //{
            //    s.Append(b.ToString("x2").ToLower());
            //}
            //String newCrc = s.ToString();

            DrawingUtility.SuspendDrawing(this);

            //if (!crc.Equals(newCrc)) // otherwise update with the new data
            {
                //crc = newCrc;

                this.Columns.Clear();
                pairInformation = new List<ComponentPair>();

                if (navigator.HasChildren)
                {
                    Dictionary<String, int> rowNameToIndex = new Dictionary<String, int>();
                    nodeNameToID = new Dictionary<string, int>();

                    XPathNodeIterator listOfNodes = navigator.Select(String.Format("/Components/Component/Component"));

                    List<String> names = new List<String>();

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        int nodeID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));
                        String nodeName = nodeNav.GetAttribute(NameAttribute, nodeNav.NamespaceURI);

                        names.Add(nodeName);
                        nodeNameToID.Add(nodeName, nodeID);
                    }

                    names.Sort();

                    foreach (String aName in names)
                    {
                        int addedColumn = Columns.Add(aName, aName);

                        int addedRow = this.Rows.Add();
                        Rows[addedRow].HeaderCell.Value = aName;
                        rowNameToIndex.Add(aName, addedRow);
                    }

                    int fromID, toID;
                    int rowIndex, linkID;
                    String paramValue, columnName, rowName;
                    String linkString = "";

                    //now, form links.

                    foreach (XPathNavigator nodeNav in listOfNodes)
                    {
                        rowName = nodeNav.GetAttribute(NameAttribute, nodeNav.NamespaceURI);
                        fromID = Int32.Parse(nodeNav.GetAttribute(IDAttribute, nodeNav.NamespaceURI));

                        rowIndex = rowNameToIndex[rowName];

                        if (nodeNav.HasChildren)
                        {
                            XPathNodeIterator listOfChildren = nodeNav.Select("Component");

                            foreach (XPathNavigator childNav in listOfChildren)
                            {
                                columnName = childNav.GetAttribute(NameAttribute, childNav.NamespaceURI);
                                toID = Int32.Parse(childNav.GetAttribute(IDAttribute, childNav.NamespaceURI));
                                linkString = childNav.GetAttribute(LinkIDAttribute, childNav.NamespaceURI);

                                if (linkString != null && linkString != "")
                                {
                                    linkID = Int32.Parse(linkString);
                                }
                                else
                                {
                                    linkID = -1;
                                }

                                pairInformation.Add(new ComponentPair(rowName, columnName, linkID));

                                XPathNavigator pnav = childNav.SelectSingleNode("LinkParameters/Parameter");
                                if (pnav != null)
                                {
                                    paramValue = pnav.GetAttribute("Value", pnav.NamespaceURI);

                                    if (paramValue.Equals(""))
                                    {
                                        this[columnName, rowIndex].Value = "Link";
                                    }
                                    else
                                    {
                                        this[columnName, rowIndex].Value = paramValue;
                                    }
                                }
                                else
                                {
                                    this[columnName, rowIndex].Value = "Link";
                                }
                            }
                        }
                    }
                }
            }
        }

        // on return, try push the value down, return success/fail
        protected override bool ParameterTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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
                        String paramRowName = this.Rows[row].HeaderCell.Value.ToString();
                        String paramColumnName = this.Columns[column].Name;
                        int linkID = -1, fromID = -1, toID = -1;
                        bool found = false;
                        foreach (ComponentPair pair in pairInformation)
                        {
                            if (pair.ColumnName.Equals(paramColumnName) && pair.RowName.Equals(paramRowName))
                            {
                                found = true;

                                linkID = pair.LinkID;
                            }
                        }

                        if (found)
                        {
                            if (value.Equals("d"))
                            {
                                if (nodeNameToID != null && nodeNameToID.ContainsKey(paramColumnName))
                                {
                                    toID = nodeNameToID[paramColumnName];
                                }

                                if (nodeNameToID != null && nodeNameToID.ContainsKey(paramRowName))
                                {
                                    fromID = nodeNameToID[paramRowName];
                                }

                                myController.DeleteLink(linkID);
                            }
                            else
                            {
                                if (parameterName.Length > 0)
                                {
                                    myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                                }
                                else
                                {
                                    // just refresh, don't push 
                                    this.UpdateViewComponent();
                                }
                            }
                        }
                        else // not found - form a link
                        {
                            if (nodeNameToID != null && nodeNameToID.ContainsKey(paramColumnName))
                            {
                                toID = nodeNameToID[paramColumnName];
                            }

                            if (nodeNameToID != null && nodeNameToID.ContainsKey(paramRowName))
                            {
                                fromID = nodeNameToID[paramRowName];
                            }

                            myController.TurnViewUpdateOff();

                            myController.Connect(this.SelectedID, fromID, toID, this.linkType);

                            if (parameterName.Length > 0)
                            {
                                this.UpdateViewComponent();

                                // TODO fix link ID here
                                foreach (ComponentPair pair in pairInformation)
                                {
                                    if (pair.ColumnName.Equals(paramColumnName) && pair.RowName.Equals(paramRowName))
                                    {
                                        linkID = pair.LinkID;
                                    }
                                }

                                myController.UpdateParameters(linkID, parameterName, value, eParamParentType.Link); // push
                                myController.TurnViewUpdateOn(); // refresh
                            }
                            else
                            {
                                myController.TurnViewUpdateOn(); // refresh
                            }
                        } // form link 
                    } // try
                    catch (Exception ex)
                    {
                        WarningForm warnForm = new WarningForm(
                            ex.Message,
                            "Error updating parameter.  Check the format of the parameter and any other constraints",
                            "OK", true,
                            "Cancel", false);
                        warnForm.ShowDialog();
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
                    }
                    return true;
                } // read only
            } // enter
            return false;
        } // method
    } // class

    internal class ComponentPair
    {
        private int linkID;

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        private String rowName, columnName;

        public String RowName
        {
            get { return rowName; }
            set { rowName = value; }
        }

        public String ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public ComponentPair(String p_Row, String p_Column, int p_linkID)
        {
            rowName = p_Row;
            columnName = p_Column;
            linkID = p_linkID;
        }
    }
}


