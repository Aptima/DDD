using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AME.Views.Assessment
{
    public class AssessmentXmlCheckBox : ListView, IAssessmentView
    {
        private List<string> selectedItems = new List<string>();
        private IAssessmentViewHelper myHelper;
        private List<string> columnsOnUpdate = new List<string>();
        private Dictionary<string, string> imageKeysOnUpdate = new Dictionary<string, string>();
        private Dictionary<string, string> additionalText = new Dictionary<string, string>();
        private Boolean updating;
        public IAssessmentViewHelper Helper
        {
            get
            {
                return myHelper;
            }
            set
            {
                myHelper = value;
            }
        }

        public List<string> ColumnsOnUpdate
        {
            get { return columnsOnUpdate; }
            set { columnsOnUpdate = value; }
        }

        public List<string> MySelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }

        public Dictionary<string, string> ImageKeysOnUpdate
        {
            get { return imageKeysOnUpdate; }
            set { imageKeysOnUpdate = value; }
        }

        public Dictionary<string, string> AdditionalText
        {
            get { return additionalText; }
            set { additionalText = value; }
        }

        public AssessmentXmlCheckBox()
            : base()
        {
            myHelper = new DefaultViewHelper();
            this.CheckBoxes = true;
            this.View = View.Details;
            this.ItemCheck += new ItemCheckEventHandler(AssessmentCheckBox_ItemCheck);
        }

        public void ClearSavedSelection()
        {
            selectedItems.Clear();
        }

        private void AssessmentCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!updating)
            {
                String value = this.Items[e.Index].Text;
                foreach (String key in additionalText.Keys)
                {
                    if (value.Contains(key))
                    {
                        value = key;
                        break;
                    }
                }
                if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
                {
                    if (!selectedItems.Contains(value))
                    {
                        selectedItems.Add(value);
                    }
                }
                else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                {
                    if (selectedItems.Contains(value))
                    {
                        selectedItems.Remove(value);
                    }
                }
            }
        }

        public bool ContainsName(String name)
        {
            bool found = false;
            for (int i = 0; i < this.selectedItems.Count; i++)
            {
                if (selectedItems[i].Contains(name))
                {
                    found = true;
                }
            }
            return found;
        }

        public void Populate()
        {
            updating = true;

            this.Items.Clear();

            this.Columns.Clear();

            foreach (String columnName in columnsOnUpdate)
            {
                this.Columns.Add(columnName);
            }

            List<XmlNode> items = myHelper.GetData(); ;

            if (items != null)
            {
                foreach (XmlNode node in items)
                {
                    String nameAttr = node.Attributes["name"].Value;
                    if (additionalText.ContainsKey(nameAttr))
                    {
                        nameAttr = nameAttr + additionalText[nameAttr];
                    }
                    this.Items.Add(nameAttr);
                }

                List<String> cleanUpSelected = new List<String>();
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    string selectedItem = selectedItems[i];
                    bool found = false;
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (this.Items[j].Text.StartsWith(selectedItem))
                        {
                            found = true;
                            this.Items[j].Checked = true;
                            break;
                            //this.SetItemChecked(j, true);
                        }
                    }
                    if (!found)
                    {
                        cleanUpSelected.Add(selectedItem);
                    }
                }
                foreach (String cleanup in cleanUpSelected)
                {
                    selectedItems.Remove(cleanup);
                }
            }

            for (int i = 0; i < this.Items.Count; i++)
            {
                String itemText = this.Items[i].Text;

                if (
                        (SmallImageList != null && imageKeysOnUpdate.ContainsKey(itemText)) ||
                        (LargeImageList != null && imageKeysOnUpdate.ContainsKey(itemText))
                    )
                {
                    this.Items[i].ImageKey = imageKeysOnUpdate[itemText];
                }
            }

            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = -2; // autosize
            }

            updating = false;
        }
    }
}
