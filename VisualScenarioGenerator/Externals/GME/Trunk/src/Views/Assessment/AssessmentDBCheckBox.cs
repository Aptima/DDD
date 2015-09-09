using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AME.Controllers;
using System.Xml.XPath;
using AME.Views.View_Components;

namespace AME.Views.Assessment
{
    public class AssessmentDBCheckBox : ListView, IViewComponent
    {
        private IController myController;
        private int id;
        private string itemtype, linktype;
        private List<CheckBoxItem> selectedItems = new List<CheckBoxItem>();
        private List<string> columnsOnUpdate = new List<string>();
        //private Dictionary<int, string> imageKeysOnUpdate = new Dictionary<int, string>();
        private IViewComponentHelper myHelper;
        private Boolean updating = false;
        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
            set { myHelper = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string ItemType
        {
            get { return itemtype; }
            set { itemtype = value; }
        }

        public string LinkType
        {
            get { return linktype; }
            set { linktype = value; }
        }

        public IController Controller
        {
            get { return myController; }
            set { myController = value; }
        }

        public List<string> ColumnsOnUpdate
        {
            get { return columnsOnUpdate; }
            set { columnsOnUpdate = value; }
        }

        //public Dictionary<int, string> ImageKeysOnUpdate
        //{
        //    get { return imageKeysOnUpdate; }
        //    set { imageKeysOnUpdate = value; }
        //}

        public AssessmentDBCheckBox()
            : base()
        {
            myHelper = new ViewComponentHelper(this);
            this.CheckBoxes = true;
            this.MultiSelect = false;
            this.View = View.Details;
            this.ItemCheck += new ItemCheckEventHandler(AssessmentCheckBox_ItemCheck);
        }

        public void ClearSelectedItems()
        {
            selectedItems.Clear();
        }

        public void GenerateItemCheck(ItemCheckEventArgs args)
        {
            this.OnItemCheck(args);
        }

        public List<CheckBoxItem> GetSelectedItems()
        {
            return selectedItems;
        }

        public List<string> GetParameterValues(String parameterName)
        {
            List<string> returnList = new List<string>();
            for (int j = 0; j < this.selectedItems.Count; j++)
            {
                CheckBoxItem fetch = this.selectedItems[j];

                int id = fetch.Id;

                ComponentOptions options = new ComponentOptions();
                options.CompParams = true;
                options.LevelDown = 1;
                IXPathNavigable raw = myController.GetComponentAndChildren(id, linktype, options);
                
                
                //IXPathNavigable nav = myController.GetParametersForComponent(id);

                if (raw != null)
                {
                    String value = GetParameter(raw.CreateNavigator(), parameterName, true);

                    if (value != null && value.Length > 0)
                    {
                        returnList.Add(value);
                    }
                }
            }
            return returnList;
        }

        public bool ItemWithIDIsChecked(int p_ID)
        {
            for (int j = 0; j < this.selectedItems.Count; j++)
            {
                CheckBoxItem fetch = (CheckBoxItem)this.selectedItems[j];

                //if (fetch.Checked)
                if (fetch.Id.Equals(p_ID))
                {
                    return true;
                }
            }
            return false;
        }

        //public bool ContainsItemWithID(int p_ID)
        //{
        //    for (int j = 0; j < this.Items.Count; j++)
        //    {
        //        CheckBoxItem fetch = (CheckBoxItem)this.Items[j];

        //        if (fetch.Id.Equals(p_ID))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public int IndexOfItemWithID(int p_ID)
        {
            for (int j = 0; j < this.selectedItems.Count; j++)
            {
                CheckBoxItem fetch = (CheckBoxItem)this.selectedItems[j];

                if (fetch.Id.Equals(p_ID))
                {
                    return j;
                }
            }
            return -1;
        }

        public void RemoveSelectedItemAt(int index)
        {
            this.selectedItems.RemoveAt(index);
        }

        public string IDToName(int p_ID)
        {
            for (int j = 0; j < this.Items.Count; j++)
            {
                CheckBoxItem fetch = (CheckBoxItem)this.Items[j];

                if (fetch.Id.Equals(p_ID))
                {
                    return fetch.Text;
                }
            }
            return "";
        }

        private string GetParameter(XPathNavigator paramNav, String fullName, bool top)
        {
            String[] split = fullName.Split('.');
            String category = split[0];
            String name = split[1];

            String xpath;

            if (top)
            {
                xpath = String.Format("Components/Component/Component/ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", category, name);
            }
            else
            {
                xpath = String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", category, name);
            }

            XPathNavigator parameter = paramNav.SelectSingleNode(xpath);
            if (parameter != null)
            {
                return parameter.GetAttribute("value", "");
            }
            else
            {
                return null;
            }
        }

        private void AssessmentCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!updating)
            {
                CheckBoxItem value = (CheckBoxItem)this.Items[e.Index];
                int testId = value.Id;
                if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
                {
                    if (!this.ItemWithIDIsChecked(testId))
                    {
                        selectedItems.Add(value);
                    }
                }
                else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                {
                    if (this.ItemWithIDIsChecked(testId))
                    {
                        int index = this.IndexOfItemWithID(testId);
                        if (index != -1)
                        {
                            selectedItems.RemoveAt(index);
                        }
                        else
                        {
                            selectedItems.Clear();
                            MessageBox.Show("Could not find item to remove");
                        }
                    }
                }
            }
        }

        public void UpdateViewComponent()
        {
            updating = true;

            this.Items.Clear();

            this.Columns.Clear();

            foreach (String columnName in columnsOnUpdate)
            {
                this.Columns.Add(columnName);
            }

            if (myController == null)
                return;

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;
            compOptions.CompParams = true;

            IXPathNavigable document = myController.GetComponentAndChildren(id, linktype, compOptions);
            XPathNavigator navigator = document.CreateNavigator();

            DrawingUtility.SuspendDrawing(this);


            XPathNodeIterator elements = null;

            if (itemtype != "")
            {
                elements = navigator.Select(String.Format(
                    "/Components/Component/Component[@Type='{0}' or @BaseType='{0}']",
                    itemtype
                    ));
            }

            if (elements != null)
            {
                String elementName, elementType;
                int elementID;

                foreach (XPathNavigator element in elements)
                {
                    String runName = GetParameter(element, "Simulation.Simulation Run Name", false);

                    //add the name/id paired item.
                    elementName = element.GetAttribute("Name", element.NamespaceURI);
                    elementType = element.GetAttribute("Type", element.NamespaceURI);
                    elementID = Int32.Parse(element.GetAttribute("ID", element.NamespaceURI));

                    if (runName != null)
                    {
                        elementName = runName + "-" + elementID;
                    }

                    CheckBoxItem add = new CheckBoxItem();
                    add.DisplayName = elementName;
                    add.Id = elementID;
                    add.Type = elementType;

                    this.Items.Add(add);
                }

                List<int> indicesToRemove = new List<int>();
                List<int> indicesToCheck = new List<int>();
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    CheckBoxItem selectedItem = selectedItems[i];
                    bool found = false;
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (((CheckBoxItem)this.Items[j]).Id.Equals(selectedItem.Id))
                        {
                            indicesToCheck.Add(j);
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        indicesToRemove.Add(i);
                    }
                }

                indicesToRemove.Reverse(); // remove in reverse order

                for (int i = 0; i < indicesToRemove.Count; i++)
                {
                    if (indicesToRemove[i] < selectedItems.Count)
                    {
                        selectedItems.RemoveAt(indicesToRemove[i]);
                    }
                }

                foreach (int checkme in indicesToCheck)
                {
                    this.Items[checkme].Checked = true;
                }

                //for (int i = 0; i < this.Items.Count; i++)
                //{
                //    int itemID = ((CheckBoxItem)this.Items[i]).Id;

                //    if (
                //            (SmallImageList != null && imageKeysOnUpdate.ContainsKey(itemID)) ||
                //            (LargeImageList != null && imageKeysOnUpdate.ContainsKey(itemID))
                //        )
                //    {
                //        //this.Items[i].ImageKey = imageKeysOnUpdate[itemID];
                //    }
                //}
            }

            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = -2; // autosize
            }

            DrawingUtility.ResumeDrawing(this);

            updating = false;
        }
    }

    public class CheckBoxItem : ListViewItem
    {
        private Int32 id;

        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        private String name;

        public String DisplayName
        {
            get { return name; }
            set { 
                    name = value;
                    this.Text = value;
                }
        }

        public CheckBoxItem() : base() { }

        public override String ToString()
        {
            return name;
        }
    }
}
