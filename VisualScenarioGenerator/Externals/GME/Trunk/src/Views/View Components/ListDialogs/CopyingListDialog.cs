using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Data;
using System.Xml;

namespace AME.Views.View_Components.ListDialogs
{
    public class CopyingListDialog : ListDialog
    {
        private String xpath;
        public CopyingListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID)
            : base(p_listAddFromScreen, p_listItemType, p_listLinkType, p_listController, p_rootIDForList, p_callingLinkType, p_callingController, p_callingRootID)
        {
            lastAdded = new List<Int32>();
            lastAddedName = new List<String>();
        }

        public CopyingListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID, String pXPath)
            : this(p_listAddFromScreen, p_listItemType, p_listLinkType, p_listController, p_rootIDForList, p_callingLinkType, p_callingController, p_callingRootID)
        {
            xpath = pXPath;
        }

        private List<Int32> lastAdded;
        public List<Int32> LastAdded { get { return lastAdded; } }

        private List<String> lastAddedName;
        public List<String> LastAddedName { get { return lastAddedName; } }

        protected override void CreateColumns()
        {
            this.myScrollingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.item });
        }

        protected override XPathNodeIterator GetListChildren()
        {
            ComponentOptions forFetch = new ComponentOptions();
            forFetch.LevelDown = 2;
            forFetch.CompParams = true;

            IXPathNavigable nav = this.listController.GetComponentAndChildren(m_rootIDForList, m_rootIDForList, m_listLinkType, forFetch);

            if (xpath == null)
            {
                xpath = "//Component[@Type='" + m_listItemType + "']"; 
            }

            XPathNodeIterator search = nav.CreateNavigator().Select(xpath);
            return search;
        }

        protected override string[] GetInstantiatedAndRemaining(CustomListItem currentListItem)
        {
            return new String[] { "0", "1" };
        }

        protected override void AddButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedCol = this.myScrollingListView.SelectedItems;

            Boolean update = false;
            if (callingController.ViewUpdateStatus == true)
            {
                callingController.TurnViewUpdateOff(); // first, turn off updates if we need to
                update = true;
            }

            foreach (ListViewItem item in selectedCol)
            {
                CustomListItem castItem = (CustomListItem)item;

                try
                {   
                    IXPathNavigable iNav = callingController.GetComponent(castItem.getID());
                    XPathNavigator nav = iNav.CreateNavigator();
                    XPathNavigator comp = nav.SelectSingleNode("/Components/Component");
                    String copiedName = comp.GetAttribute("Name", "");
                    int copiedID = callingController.CreateComponent(comp.GetAttribute("Type", ""), copiedName, comp.GetAttribute("Description", ""));
                    callingController.PropagateParameters(castItem.getID(), copiedID);

                    lastAdded.Add(copiedID);
                    lastAddedName.Add(copiedName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (update)
            {
                callingController.TurnViewUpdateOn(); // turn back on updates if we need to
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
