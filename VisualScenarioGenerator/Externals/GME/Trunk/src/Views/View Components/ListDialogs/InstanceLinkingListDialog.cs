using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using System.Windows.Forms;
using System.Xml.XPath;

namespace AME.Views.View_Components.ListDialogs
{
    public class InstanceLinkingListDialog : ListDialog
    {
        private XPathNavigator forCount;
        private List<Int32> added = new List<Int32>();

        public InstanceLinkingListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID)
            : base(p_listAddFromScreen, p_listItemType, p_listLinkType, p_listController, p_rootIDForList, p_callingLinkType, p_callingController, p_callingRootID)
        {

        }

        protected override void CreateColumns()
        {
            this.myScrollingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.item,
            this.noInstantiated,
            this.amountToAdd});
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
                XPathNodeIterator search = nav.CreateNavigator().Select("Components/Component/Component[@Type='" + m_listItemType + "']/Component[@Type='" + m_listItemType + "']");
                
                // forcount
                ComponentOptions countOptions = new ComponentOptions();
                countOptions.LevelDown = 1;
                forCount = callingController.GetComponentAndChildren(m_callingRootID, m_callingRootID, m_callingLinkType, countOptions).CreateNavigator();
                
                return search;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        protected override string[] GetInstantiatedAndRemaining(CustomListItem currentListItem)
        {
            if (forCount != null) 
            {
               XPathNodeIterator checkCountChildren = forCount.Select("Components/Component/Component[@ID='" + currentListItem.getID() + "']");
               return new String[] { checkCountChildren.Count.ToString(), (1-checkCountChildren.Count).ToString() }; // either 1 0 or 0 1
            }
            return new String[] { "n/a", "n/a" };
        }

        public Int32 GetLastAddedId()
        {
            if (added.Count > 0)
                return added[added.Count - 1];
            else
                return -1;
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
                        // just link
                        callingController.Connect(m_callingRootID, m_callingRootID, castItem.getID(), m_callingLinkType);

                        // Get children
                        //m_listLinkType
                        ComponentOptions options = new ComponentOptions();

                        IXPathNavigable iNav = this.listController.GetComponentAndChildren(m_rootIDForList, castItem.getID(), m_listLinkType, options);
                        XPathNavigator nav = iNav.CreateNavigator();
                        XPathNavigator navComponent = nav.SelectSingleNode("Components/Component");
                        if (navComponent != null)
                        {
                            linkChild(navComponent);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (update)
            {
                callingController.TurnViewUpdateOn(); // turn back on updates if we need to
            }
        }
        private void linkChild(XPathNavigator parent)
        {
            XPathNodeIterator children = parent.Select("child::Component");
            if (children.Count > 0)
            {
                while (children.MoveNext())
                {
                    Int32 parentId = Int32.Parse(parent.GetAttribute("ID", parent.NamespaceURI));
                    XPathNavigator child = children.Current.CreateNavigator();
                    Int32 childId = Int32.Parse(child.GetAttribute("ID", child.NamespaceURI));

                    linkChild(child);

                    callingController.Connect(m_callingRootID, parentId, childId, m_callingLinkType);
                }
            }
        }
    }
}
