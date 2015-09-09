using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using GME.Controllers;
using GME.Model;
using System.Xml.XPath;
using System.Drawing;

namespace GME.Views.View_Components
{
    public class ListDialog : Form, IDataEntryViewComponent
    {
        private IDataEntryController listController;
        private IDataEntryController callingController;
        private String m_listItemType, m_listLinkType;

        private int m_rootIDForList;

        private int m_callingRootID;

        public int CallingRootID
        {
            get { return m_callingRootID; }
            set { m_callingRootID = value; }
        }

        private ListViewItem selectedItem;

        private NumericUpDown newNumericUpDown;
        private ScrollingListView listView1;
        private ColumnHeader item;
        private ColumnHeader noInstantiated;
        private ColumnHeader amountToAdd;
        private Button addButton;
        private Label addButtonLabel;
        private Button buttonCancel;

        private Boolean numericUpDownActive = false;

        public ListDialog(String p_listItemType, String p_listLinkType, DataEntryController p_listController, int p_rootIDForList, DataEntryController p_callingController, int p_callingRootID)
            : base()
        {
            InitializeComponent();

            m_listLinkType = p_listLinkType;
            m_listItemType = p_listItemType;
            m_rootIDForList = p_rootIDForList;
            m_callingRootID = p_callingRootID;

            listController = p_listController;
            callingController = p_callingController;

            newNumericUpDown = new NumericUpDown();
            this.listView1.Controls.Add(newNumericUpDown);
            newNumericUpDown.Hide();

            //image list 
            Dictionary<String, Bitmap> typeImage = listController.GetIcons();
            ImageList tempList = new ImageList();
            Image image;
            foreach (String k in typeImage.Keys)
            {
                image = typeImage[k];
                tempList.Images.Add(k, image);
            }
            this.listView1.SmallImageList = tempList;

            this.listView1.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listView1_ItemSelectionChanged);
            this.listView1.DoubleClick += new EventHandler(ListDialog_DoubleClick);
            this.listView1.HScrollMoved += new EventHandler(listView1_ScrollMoved);
            this.listView1.VScrollMoved += new EventHandler(listView1_ScrollMoved);
            this.listView1.MouseWheelRotated += new EventHandler(listView1_MouseWheelRotated);

            this.newNumericUpDown.KeyDown += new KeyEventHandler(newNumericUpDown_KeyDown);

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 0;

            IXPathNavigable callingInfoIXP = callingController.GetComponentAndChildren(m_callingRootID, p_listLinkType, compOptions);
            XPathNavigator callingNavigator = callingInfoIXP.CreateNavigator();

            XPathNavigator callingRootComponent = callingNavigator.SelectSingleNode("/Components/Component");

            if (callingRootComponent != null)
            {
                String rootName = callingRootComponent.GetAttribute("Name", callingRootComponent.NamespaceURI);
                String rootType = callingRootComponent.GetAttribute("Type", callingRootComponent.NamespaceURI);
                addButtonLabel.Text = "Add selected items to " + rootType + " " + rootName + ":"; 
            }

            IXPathNavigable titleIXP = listController.GetComponentAndChildren(m_rootIDForList, p_listLinkType, compOptions);
            XPathNavigator titleNavigator = titleIXP.CreateNavigator();

            XPathNavigator titleComponent = titleNavigator.SelectSingleNode("/Components/Component");

            if (titleComponent != null)
            {
                String titleType = titleComponent.GetAttribute("Type", titleComponent.NamespaceURI);
                this.Text = "Items of type " + m_listItemType + " from the " + titleType;
            }

            addButton.AutoSize = true;
            addButton.Text = "Add";
            addButton.Click += new EventHandler(addButton_Click);

            buttonCancel.Click += new EventHandler(buttonCancel_Click);

            this.listView1.MultiSelect = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // close form
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // commit spinner if active 
            // to have current value for subitems[4] for controller call below
            this._SaveNumericUpDownValue();

            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedCol = this.listView1.SelectedItems;

            callingController.TurnViewUpdateOff();

            foreach (ListViewItem item in selectedCol)
            {
                CustomListItem castItem = (CustomListItem)item;

                try
                {
                    callingController.AddComponentInstances(CallingRootID, CallingRootID,
                        castItem.getID(), castItem.getName(), this.m_listLinkType,
                        castItem.getDescription(), Int32.Parse(castItem.SubItems[2].Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            callingController.TurnViewUpdateOn();
        }

        void listView1_ScrollMoved(object sender, EventArgs e)
        {
            this._SaveNumericUpDownValue();
        }

        private void listView1_MouseWheelRotated(object sender, EventArgs e)
        {
            this._SaveNumericUpDownValue();
        }

        private void _SaveNumericUpDownValue()
        {
            if (numericUpDownActive)
            {
                if (selectedItem != null)
                {
                    selectedItem.SubItems[2].Text = newNumericUpDown.Value.ToString();
                }
                numericUpDownActive = false;
                newNumericUpDown.Hide();
            }
        }//_SaveNumericUpDownValue

        void newNumericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this._SaveNumericUpDownValue();
            }
        }

        void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (selectedItem != null && !selectedItem.Equals(e.Item) && numericUpDownActive)
            {
                this._SaveNumericUpDownValue();
            }
            selectedItem = e.Item;
        }

        private void ListDialog_DoubleClick(object sender, EventArgs e)
        {
            int upDownX = selectedItem.SubItems[2].Bounds.X;
            int upDownY = selectedItem.SubItems[2].Bounds.Y;
            int upDownWidth = selectedItem.SubItems[2].Bounds.Width;
            int upDownHeight = selectedItem.SubItems[2].Bounds.Height;

            //UpDown
            newNumericUpDown.Location = new System.Drawing.Point(upDownX, upDownY);
            newNumericUpDown.Size = new System.Drawing.Size(upDownWidth, upDownHeight);

            newNumericUpDown.Minimum = 0;

            newNumericUpDown.Value = 0;
            newNumericUpDown.Enabled = true;

            numericUpDownActive = true;
            newNumericUpDown.Show();
            newNumericUpDown.Focus();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListDialog));
            this.addButton = new System.Windows.Forms.Button();
            this.addButtonLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listView1 = new GME.Views.View_Components.ScrollingListView();
            this.item = new System.Windows.Forms.ColumnHeader();
            this.noInstantiated = new System.Windows.Forms.ColumnHeader();
            this.amountToAdd = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(252, 237);
            this.addButton.Margin = new System.Windows.Forms.Padding(6);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "button1";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // addButtonLabel
            // 
            this.addButtonLabel.AutoSize = true;
            this.addButtonLabel.Location = new System.Drawing.Point(12, 9);
            this.addButtonLabel.Margin = new System.Windows.Forms.Padding(6);
            this.addButtonLabel.Name = "addButtonLabel";
            this.addButtonLabel.Size = new System.Drawing.Size(35, 13);
            this.addButtonLabel.TabIndex = 3;
            this.addButtonLabel.Text = "label1";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(339, 237);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.item,
            this.noInstantiated,
            this.amountToAdd});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(15, 31);
            this.listView1.Margin = new System.Windows.Forms.Padding(6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(397, 194);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // item
            // 
            this.item.Text = "Item";
            this.item.Width = 196;
            // 
            // noInstantiated
            // 
            this.noInstantiated.Text = "No. Instantiated";
            this.noInstantiated.Width = 98;
            // 
            // amountToAdd
            // 
            this.amountToAdd.Text = "Amount to Add";
            this.amountToAdd.Width = 99;
            // 
            // ListDialog
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(427, 275);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.addButtonLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ListDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    ListView thisList = (ListView)sender;
        //    CustomListItem item = (CustomListItem)e.Item;

        //    if (item != null)
        //    {
        //        // Start the drag-and-drop operation
        //        item.setToAddCount(Int32.Parse(item.SubItems[4].Text));
        //        thisList.DoDragDrop(item, DragDropEffects.Copy);
        //    }
        //}

        #region ViewComponentUpdate Members

        public IDataEntryController Controller
        {
            get
            {
                return listController;
            }
            set
            {
                listController = value;
            }
        }

        public void UpdateViewComponent()
        {
            int restoreViewAtThisItem = -1;
            if (this.listView1.TopItem != null)
            {
                restoreViewAtThisItem = this.listView1.Items.IndexOf(this.listView1.TopItem);
            }

            DrawingUtility.SuspendDrawing(this);

            this.listView1.Items.Clear();

            List<CustomListItem> updatedListItems = listController.GetRightClickListItems(m_rootIDForList, m_callingRootID, this.m_listItemType, this.m_listLinkType);

            updatedListItems.Sort();

            foreach (CustomListItem item in updatedListItems)
            {
                item.ImageKey = item.getType();
                this.listView1.Items.Add(item);
            }

            if (restoreViewAtThisItem != -1 && this.listView1.Items.Count > 0)
            {
                this.listView1.EnsureVisible(this.listView1.Items.Count - 1); // last item
                this.listView1.EnsureVisible(restoreViewAtThisItem); // top item   
            }

            // autosize columns
            bool skipFirst = true;
            foreach(ColumnHeader colHead in this.listView1.Columns)
            {
                if (skipFirst == true)
                {
                    skipFirst = false;
                }
                else
                {
                    colHead.Width = -2;
                }
            }

            DrawingUtility.ResumeDrawing(this);
        }

        #endregion
    }//ListDialog class

    public class CustomListItem : ListViewItem, IComparable
    {
        private int id;
        private String name;
        private String type;
        private String description;

        public CustomListItem(String passName, String passType, int passID, string passDescription)
            : base(passName)
        {
            this.name = passName;
            this.type = passType;
            this.id = passID;
            this.Text = passName;
            this.description = passDescription;
        }

        public int getID() { return id; }
        public String getName() { return name; }
        public String getType() { return type; }
        public String getDescription() { return description; }

        public override String ToString() { return name; }

        #region IComparable Members

        public int CompareTo(object newObj)
        {
            if (newObj is CustomListItem)
            {
                CustomListItem toCompare = (CustomListItem)newObj;

                return this.name.CompareTo(toCompare.name);
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}

