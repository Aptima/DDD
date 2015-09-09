using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Data;

namespace AME.Views.View_Components.ListDialogs
{
    public class InstancingListDialog : ListDialog
    {
        public InstancingListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID)
            : base(p_listAddFromScreen, p_listItemType, p_listLinkType, p_listController, p_rootIDForList, p_callingLinkType, p_callingController, p_callingRootID)
        {
            allowChangeAmountToAdd = true; // instancing dialog can change the amount to add count
        }

        private XPathNavigator forCount;

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
            forFetch.LevelDown = 2;
            forFetch.CompParams = true;

            IXPathNavigable nav = this.listController.GetComponentAndChildren(m_rootIDForList, m_rootIDForList, m_listLinkType, forFetch);
            XPathNodeIterator search = nav.CreateNavigator().Select("Components/Component/Component");

            // forcount
            ComponentOptions countOptions = new ComponentOptions();
            countOptions.LevelDown = 1;
            countOptions.ClassInstanceInfo = true;
            countOptions.InstanceUseClassName = true;
            forCount = callingController.GetComponentAndChildren(m_callingRootID, m_callingRootID, m_callingLinkType, countOptions).CreateNavigator();
            
            return search;
        }

        protected override string[] GetInstantiatedAndRemaining(CustomListItem currentListItem)
        {
            XPathNodeIterator checkCountChildren = forCount.Select("Components/Component/Component[@ClassID='" + currentListItem.getID() + "']");
            return new String[] { checkCountChildren.Count.ToString(), amountToAddUpDown.Value.ToString() }; // use up down
        }

        protected override void AddButton_Click(object sender, EventArgs e)
        {
            // commit spinner if active 
            // to have current value for subitems[2] for controller call below
            this._SaveNumericUpDownValue();

            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedCol = this.myScrollingListView.SelectedItems;

            callingController.TurnViewUpdateOff();

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

                    try
                    {   // instance
                        callingController.AddComponentInstances(m_callingRootID, m_callingRootID,
                            castItem.getID(), castItem.getName(), this.m_listLinkType,
                            castItem.getDescription(), Int32.Parse(castItem.SubItems[2].Text));
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
    }
}
