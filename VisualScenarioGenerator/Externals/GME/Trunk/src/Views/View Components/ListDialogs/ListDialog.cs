using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using AME.Controllers;
using AME.Model;
using System.Xml.XPath;
using System.Drawing;
using AME.Nodes;

namespace AME.Views.View_Components.ListDialogs
{
    public abstract class ListDialog : Form, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        protected IController listController, callingController;

        protected String m_listItemType, m_listLinkType, m_callingLinkType;

        protected int m_rootIDForList, m_callingRootID;

        protected Boolean allowChangeAmountToAdd = false;
        protected Boolean numericUpDownActive = false;

        protected ListViewItem selectedItem;

        protected NumericUpDown amountToAddUpDown;

        protected ScrollingListView myScrollingListView;

        protected ColumnHeader item;
        protected ColumnHeader noInstantiated;
        protected ColumnHeader amountToAdd;

        private Button addButton;
        private Label addButtonLabel;
        private Button closeButton;

        private Boolean useNoInstantiatedAndRemaining = true;

        public ListDialog(String p_listAddFromScreen, String p_listItemType, String p_listLinkType, IController p_listController, int p_rootIDForList, String p_callingLinkType, IController p_callingController, int p_callingRootID)
            : base()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

            CreateColumns();

            m_listLinkType = p_listLinkType;
            m_callingLinkType = p_callingLinkType;

            m_listItemType = p_listItemType;

            m_rootIDForList = p_rootIDForList;
            m_callingRootID = p_callingRootID;

            listController = p_listController;
            callingController = p_callingController;

            amountToAddUpDown = new NumericUpDown();
            this.myScrollingListView.Controls.Add(amountToAddUpDown);
            amountToAddUpDown.Hide();

            this.myScrollingListView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listView1_ItemSelectionChanged);
            this.myScrollingListView.DoubleClick += new EventHandler(ListDialog_DoubleClick);
            this.myScrollingListView.HScrollMoved += new EventHandler(listView1_ScrollMoved);
            this.myScrollingListView.VScrollMoved += new EventHandler(listView1_ScrollMoved);
            this.myScrollingListView.MouseWheelRotated += new EventHandler(listView1_MouseWheelRotated);

            this.amountToAddUpDown.KeyDown += new KeyEventHandler(newNumericUpDown_KeyDown);

            // initialize label
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 0;

            IXPathNavigable callingInfoIXP = callingController.GetComponentAndChildren(m_callingRootID, p_callingLinkType, compOptions);
            XPathNavigator callingNavigator = callingInfoIXP.CreateNavigator();

            XPathNavigator callingRootComponent = callingNavigator.SelectSingleNode("/Components/Component");

            if (callingRootComponent != null)
            {
                String rootName = callingRootComponent.GetAttribute("Name", callingRootComponent.NamespaceURI);
                String rootType = callingRootComponent.GetAttribute("Type", callingRootComponent.NamespaceURI);
                addButtonLabel.Text = "Add selected items to " + rootType + " " + rootName + ":";
            }

            // initialize title
            this.Text = "Items of type " + m_listItemType + " from the " + p_listAddFromScreen;

            addButton.AutoSize = true;
            addButton.Text = "Add";
            addButton.Click += new EventHandler(AddButton_Click);
            closeButton.Click += new EventHandler(buttonCancel_Click);

            this.myScrollingListView.MultiSelect = true;

            // update / close hooks:
            callingController.RegisterForUpdate(this);
            this.FormClosing += new FormClosingEventHandler(ListDialog_FormClosing);

            // default images
            Dictionary<String, Bitmap> typeImage = listController.GetIcons();
            ImageList tempList = new ImageList();
            Image image;

            foreach (String k in typeImage.Keys)
            {
                image = typeImage[k];
                tempList.Images.Add(k, image);
            }

            this.myScrollingListView.SmallImageList = tempList;
        }

        private List<CustomListItem> selected = new List<CustomListItem>();
        public List<CustomListItem> SelectedListItems { get { return selected; } }
        public void ReplaceAddWithSelection()
        {
            addButton.Click -= new EventHandler(AddButton_Click);
            addButton.Click += new EventHandler(addButton_Click2);
        }

        private void addButton_Click2(object sender, EventArgs e)
        {
            selected.Clear();
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedCol = this.myScrollingListView.SelectedItems;
            foreach (ListViewItem item in selectedCol)
            {
                CustomListItem castItem = (CustomListItem)item;
                selected.Add(new CustomListItem(castItem.getName(), castItem.getType(), castItem.getID(), castItem.getDescription()));
            }
            this.DialogResult = DialogResult.OK;
        }

        private void ListDialog_FormClosing(object sender, FormClosingEventArgs e) // unregister update hooks
        {
            callingController.UnregisterForUpdate(this);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // close form
        }
         
        // implemented by subclasses:  behavior when add is clicked, the children that are fetched
        // and what to display for number instantiated and amount to add
        protected abstract void AddButton_Click(object sender, EventArgs e);
        protected abstract XPathNodeIterator GetListChildren();
        protected abstract String[] GetInstantiatedAndRemaining(CustomListItem currentItem);
        protected abstract void CreateColumns();

        public Label MessageLabel
        {
            get { return addButtonLabel; }
        }

        public Boolean UseNoInstantiatedAndRemaining
        {
            set { useNoInstantiatedAndRemaining = value; }
        }

        private void listView1_ScrollMoved(object sender, EventArgs e) // scrolling list breaks focus, save up down
        {
            this._SaveNumericUpDownValue();
        }

        private void listView1_MouseWheelRotated(object sender, EventArgs e) // same
        {
            this._SaveNumericUpDownValue();
        }

        protected void _SaveNumericUpDownValue()
        {
            if (numericUpDownActive)
            {
                if (selectedItem != null)
                {
                    selectedItem.SubItems[2].Text = amountToAddUpDown.Value.ToString();
                }
                numericUpDownActive = false;
                amountToAddUpDown.Hide();
            }
        }//_SaveNumericUpDownValue

        private void newNumericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this._SaveNumericUpDownValue();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (selectedItem != null && !selectedItem.Equals(e.Item) && numericUpDownActive)
            {
                this._SaveNumericUpDownValue();
            }
            selectedItem = e.Item;
        }

        private void ListDialog_DoubleClick(object sender, EventArgs e)
        {
            if (allowChangeAmountToAdd)
            {
                int upDownX = selectedItem.SubItems[2].Bounds.X;
                int upDownY = selectedItem.SubItems[2].Bounds.Y;
                int upDownWidth = selectedItem.SubItems[2].Bounds.Width;
                int upDownHeight = selectedItem.SubItems[2].Bounds.Height;

                //UpDown
                amountToAddUpDown.Location = new System.Drawing.Point(upDownX, upDownY);
                amountToAddUpDown.Size = new System.Drawing.Size(upDownWidth, upDownHeight);

                amountToAddUpDown.Minimum = 0;

                amountToAddUpDown.Value = Int32.Parse(selectedItem.SubItems[2].Text);
                
                amountToAddUpDown.Enabled = true;
                numericUpDownActive = true;

                amountToAddUpDown.Show();
                amountToAddUpDown.Focus();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListDialog));
            this.addButton = new System.Windows.Forms.Button();
            this.addButtonLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.myScrollingListView = new AME.Views.View_Components.ScrollingListView();
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
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(339, 237);
            this.closeButton.Margin = new System.Windows.Forms.Padding(6);
            this.closeButton.Name = "buttonCancel";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.myScrollingListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.myScrollingListView.FullRowSelect = true;
            this.myScrollingListView.GridLines = true;
            this.myScrollingListView.Location = new System.Drawing.Point(15, 31);
            this.myScrollingListView.Margin = new System.Windows.Forms.Padding(6);
            this.myScrollingListView.Name = "listView1";
            this.myScrollingListView.Size = new System.Drawing.Size(397, 194);
            this.myScrollingListView.TabIndex = 1;
            this.myScrollingListView.UseCompatibleStateImageBehavior = false;
            this.myScrollingListView.View = System.Windows.Forms.View.Details;
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
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addButtonLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.myScrollingListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ListDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region ViewComponentUpdate Members

        public IController Controller
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

        public virtual List<CustomListItem> ProcessItemsBeforeUpdate(List<CustomListItem> items, XPathNodeIterator itemXml)
        {
            return items;
        }

        public void UpdateViewComponent() // general update - uses subclass implementations
        {
            int restoreViewAtThisItem = -1;
            if (this.myScrollingListView.TopItem != null)
            {
                restoreViewAtThisItem = this.myScrollingListView.Items.IndexOf(this.myScrollingListView.TopItem);
            }

            DrawingUtility.SuspendDrawing(this);

            this.myScrollingListView.Items.Clear();

            XPathNodeIterator children = null;

            try
            {
                children = this.GetListChildren();
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read children for list dialog", e.Message);
            }

            if (children == null)
            {
                return;
            }

            // temp load in here, then sort
            List<CustomListItem> updatedListItems = new List<CustomListItem>();

            foreach (XPathNavigator child in children)
            {
                String childType = child.GetAttribute(XmlSchemaConstants.Display.Component.Type, child.NamespaceURI);

                if (childType.Equals(m_listItemType)) // filter by type
                {
                    int childID = Convert.ToInt32(child.GetAttribute(XmlSchemaConstants.Display.Component.Id, child.NamespaceURI));
                    String childName = child.GetAttribute(XmlSchemaConstants.Display.Component.Name, child.NamespaceURI);
                    String childDescription = child.GetAttribute(XmlSchemaConstants.Display.Component.Description, child.NamespaceURI);

                    List<Function> itemFunctions = FunctionHelper.GetFunctions(child); // update image list
                    String imagePath = FunctionHelper.ProcessNavForImage(this.Controller, this.myScrollingListView.SmallImageList, childType, child, itemFunctions);

                    CustomListItem temp = new CustomListItem(childName, childType, childID, childDescription, imagePath);

                    try
                    {
                        temp.SubItems.AddRange(this.GetInstantiatedAndRemaining(temp));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Could not read children for list dialog", e.Message);
                    }
                    
                    updatedListItems.Add(temp);
                }
            }

            updatedListItems = ProcessItemsBeforeUpdate(updatedListItems, children);

            updatedListItems.Sort();

            foreach (CustomListItem item in updatedListItems)
            {
                this.myScrollingListView.Items.Add(item);
            }

            if (restoreViewAtThisItem != -1 && this.myScrollingListView.Items.Count > 0)
            {
                this.myScrollingListView.EnsureVisible(this.myScrollingListView.Items.Count - 1); // last item
                this.myScrollingListView.EnsureVisible(restoreViewAtThisItem); // top item   
            }

            // autosize columns
            bool skipFirst = true;
            int count = 0;
            foreach (ColumnHeader colHead in this.myScrollingListView.Columns)
            {
                if (skipFirst == true)
                {
                    skipFirst = false;
                }
                else
                {
                    if (useNoInstantiatedAndRemaining)
                    {
                        colHead.Width = -2;
                    }
                    else if (count > 2) // first is name, skip no instantiated and remaining
                    {
                        colHead.Width = -2;
                    }
                    else
                    {
                        colHead.Width = 0; // and make them 0
                    }
                }
                count++;
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
            : this(passName, passType, passID, passDescription, "") // no image path specified
        {
        }


        public CustomListItem(String passName, String passType, int passID, string passDescription, String p_ImagePath)
            : base(passName)
        {
            this.name = passName;
            this.type = passType;
            this.id = passID;
            this.Text = passName;
            this.description = passDescription;

            this.ImageKey = p_ImagePath;   // displayed icon is based on image path
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

