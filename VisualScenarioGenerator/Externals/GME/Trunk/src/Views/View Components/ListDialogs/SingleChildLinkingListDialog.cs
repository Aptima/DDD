using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using AME.Controllers;
using System.Windows.Forms;
using System.Xml;

namespace AME.Views.View_Components.ListDialogs {

	public class SingleChildLinkingListDialog : ListDialog {

		private int parentId = -1;
        private List<Int32> added = new List<Int32>();
        private Boolean removeDuplicates = false;

        public Boolean RemoveDuplicates { set { removeDuplicates = value; } }

		public SingleChildLinkingListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID, int parentID)
            : base(p_listAddFromScreen, p_listItemType, p_listLinkType, p_listController, p_rootIDForList, p_callingLinkType, p_callingController, p_callingRootID)
        {
			this.parentId = parentID;
        }

        protected override void CreateColumns()
        {
            this.myScrollingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.item });
        }

        protected override XPathNodeIterator GetListChildren()
        {
            ComponentOptions forFetch = new ComponentOptions();
            forFetch.LevelDown = 3;
            forFetch.CompParams = true;
            forFetch.InstanceUseClassName = true;

            try
            {
                IXPathNavigable nav = this.listController.GetComponentAndChildren(m_rootIDForList, m_rootIDForList, m_listLinkType, forFetch);
                //XPathNodeIterator search = nav.CreateNavigator().Select("Components/Component/Component[@Type='" + m_listItemType + "']");
				XPathNodeIterator search = nav.CreateNavigator().Select("//Component[@Type='" + m_listItemType + "']");

                XPathNavigator returnNav = null;
                if (removeDuplicates)
                {
                    // remove duplicate IDs
                    XPathNavigator[] searchTemp = new XPathNavigator[search.Count];
                    int index = 0;
                    foreach (XPathNavigator searchItem in search)
                    {
                        searchTemp[index] = searchItem;
                        index++;
                    }

                    XmlDocument returnDoc = new XmlDocument();
                    XmlElement rootEle = returnDoc.CreateElement("Root");
                    returnDoc.AppendChild(rootEle);
                    returnNav = returnDoc.CreateNavigator();
                    XPathNavigator rootNav = returnNav.SelectSingleNode("/Root");

                    Dictionary<String, String> seenIDs = new Dictionary<String, String>();
                    for (int i = searchTemp.Length - 1; i >= 0; i--)
                    {
                        String ID = searchTemp[i].GetAttribute(XmlSchemaConstants.Display.Component.Id, "");
                        if (!seenIDs.ContainsKey(ID))
                        {
                            seenIDs.Add(ID, ID);
                            rootNav.AppendChild(searchTemp[i]);
                        }
                    }
                }

                if (removeDuplicates)
                {
                    return returnNav.Select("/Root/*");
                }
                else
                {
                    return search;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        protected override string[] GetInstantiatedAndRemaining(CustomListItem currentListItem)
        {
			return new String[] { "0", "1" };
        }

        public Int32 GetLastAddedId()
        {
            if (added.Count > 0)
                return added[added.Count - 1];
            else
                return -1;
        }

        public List<Int32> GetLastAddedIds()
        {
            return added;
        }

        protected override void AddButton_Click(object sender, EventArgs e)
        {
            added.Clear();
            // commit spinner if active 
            // to have current value for subitems[2] for controller call below
            this._SaveNumericUpDownValue();

            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedCol = this.myScrollingListView.SelectedItems;

            Boolean update = false;

            foreach (ListViewItem item in selectedCol)
            {
                CustomListItem castItem = (CustomListItem)item;

                int amountToAdd = Int32.Parse(castItem.SubItems[2].Text);

                if (amountToAdd > 0)
                {
                    if (callingController.ViewUpdateStatus == true)
                    {
                        callingController.TurnViewUpdateOff(); // first, turn off updates if we need to
                        update = true;
                    }

                    added.Add(castItem.getID());

                    try
                    {
						int fromId = -1;
						if (this.parentId == -1)
							fromId = m_callingRootID;
						else
							fromId = this.parentId;
                        // just link
						callingController.Connect(m_callingRootID, fromId, castItem.getID(), m_callingLinkType);
                        // Get children
                        //m_listLinkType
						//ComponentOptions options = new ComponentOptions();

						//IXPathNavigable iNav = this.listController.GetComponentAndChildren(m_rootIDForList, castItem.getID(), m_listLinkType, options);
						//XPathNavigator nav = iNav.CreateNavigator();
						//XPathNavigator navComponent = nav.SelectSingleNode("Components/Component");
						//if (navComponent != null)
						//{
						//    linkChild(navComponent);
						//}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        this.DialogResult = DialogResult.Cancel;

                    }
                }
            }

            if (update)
            {
                callingController.TurnViewUpdateOn(); // turn back on updates if we need to
            }
            this.DialogResult = DialogResult.OK;
        }
        
        //private void linkChild(XPathNavigator parent)
        //{
        //    XPathNodeIterator children = parent.Select("child::Component");
        //    if (children.Count > 0)
        //    {
        //        while (children.MoveNext())
        //        {
        //            Int32 parentId = Int32.Parse(parent.GetAttribute("ID", parent.NamespaceURI));
        //            XPathNavigator child = children.Current.CreateNavigator();
        //            Int32 childId = Int32.Parse(child.GetAttribute("ID", child.NamespaceURI));

        //            linkChild(child);

        //            callingController.Connect(m_callingRootID, parentId, childId, m_callingLinkType);
        //        }
        //    }
        //}
	}
}
