using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using AME.Model;
using AME.Views.View_Components;
using System.Xml.XPath;
using System.Security.Cryptography;
using AME.Views.View_Components.ListDialogs;
using AME;

namespace Forms
{
    public partial class ProjectForm : Form
    {
        private int m_SelectedProjectID = -1;

        public int SelectedProjectID
        {
            get { return m_SelectedProjectID; }
            set { m_SelectedProjectID = value; }
        }

        private Boolean m_IsProjectSelected, m_WasNewSelection;

        public Boolean IsProjectSelected
        {
            get { return m_IsProjectSelected; }
            set { m_IsProjectSelected = value; }
        }

        public Boolean WasNewSelection
        {
            get { return m_WasNewSelection; }
            set { m_WasNewSelection = value; }
        }

        private RootController rc;

        private String m_warningText, m_warningCaption, m_LinkType, IDAttribute, NameAttribute, m_SelectedProjectName;

        private string crc = "";

        public string LinkType
        {
            get { return m_LinkType; }
            set { m_LinkType = value; }
        }

        public string SelectedProjectName
        {
            get { return m_SelectedProjectName; }
            set { m_SelectedProjectName = value; }
        }

        public enum ProjectFormType { New, Open, Delete, Clone };

        private ProjectFormType m_MyProjectFormType;

        public ProjectFormType MyProjectFormType
        {
            get { return m_MyProjectFormType; }
            set { m_MyProjectFormType = value; }
        }

        public void CloseProject()
        {
            m_SelectedProjectID = -1;
            m_IsProjectSelected = false;
            m_WasNewSelection = false;
        }

        public void SetListIcon(Icon ico)
        {
            this.listView1.SmallImageList = new ImageList();
            this.listView1.SmallImageList.Images.Add(ico);
        }

        public ProjectForm(RootController rootControllerPass)
        {
            rc = rootControllerPass;
            m_warningText = "Are you sure you wish to delete this project?\nThis will delete all components created by this project.";
            m_warningCaption = "Delete Project"; 

            InitializeComponent();

            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;

            this.CancelButton = cancelButton;

            this.listView1.View = View.Details;
            this.listView1.MultiSelect = false;
            this.listView1.Columns.Add(rc.RootComponentType + "s");
            this.listView1.HideSelection = false;
            this.listView1.SelectedIndexChanged += new EventHandler(listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new EventHandler(listView1_DoubleClick);
            this.listView1.Columns[0].Width = -2;

            this.OKButton.Click += new EventHandler(OKButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);
            this.cloneButton.Click += new EventHandler(cloneButton_Click);
            this.VisibleChanged += new EventHandler(ProjectForm_VisibleChanged);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (MyProjectFormType)
            {
                case ProjectFormType.Clone:
                    SetTextboxToSelectedItemName();
                    break;

                case ProjectFormType.New:
                    break;

                case ProjectFormType.Delete:
                    break;

                case ProjectFormType.Open:
                    break;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            switch (MyProjectFormType)
            {
                case ProjectFormType.Clone:
                    break;

                case ProjectFormType.New:
                    break;

                case ProjectFormType.Delete:
                    break;

                case ProjectFormType.Open:
                    OpenSelectedProject();
                    break;
            }
        }

        private void ProjectForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                this.WasNewSelection = false;

                flowLayoutPanel1.Controls.Clear();

                RefreshList();

                switch (MyProjectFormType)
                {
                    case ProjectFormType.Clone:
                        instructionLabel.Text = "Select a project in the list and type in a new name (optional) in the text box and click copy to copy the project";
                        this.Text = "Copy a Project";
                        flowLayoutPanel1.Controls.Add(createBox);
                        flowLayoutPanel1.Controls.Add(cloneButton);
                        this.Controls.Add(flowLayoutPanel1);
                        AcceptButton = cloneButton;
                        createBox.Text = "";
                        this.Controls.Remove(OKButton);
                        cancelButton.Text = "Close";
                        createBox.Focus();
                        break;

                    case ProjectFormType.New:
                        instructionLabel.Text = "Type in a new project name in the text box and click Create to create a new project";
                        this.Text = "Create a new Project";
                        flowLayoutPanel1.Controls.Add(createBox);
                        flowLayoutPanel1.Controls.Add(createButton);
                        this.Controls.Add(flowLayoutPanel1);
                        AcceptButton = createButton;
                        createBox.Text = "";
                        this.Controls.Remove(OKButton);
                        cancelButton.Text = "Close";
                        createBox.Focus();
                        break;
                    case ProjectFormType.Delete:
                        instructionLabel.Text = "Click to select a project in the list and click delete to delete a project";
                        this.Text = "Delete a Project";
                        flowLayoutPanel1.Controls.Add(deleteButton);
                        this.Controls.Add(flowLayoutPanel1);
                        this.Controls.Remove(OKButton);
                        cancelButton.Text = "Close";
                        AcceptButton = deleteButton;
                        break;
                    case ProjectFormType.Open:
                        instructionLabel.Text = "Click to select a project in the list and click Open to open a project";
                        this.Text = "Open a Project";
                        this.Controls.Remove(flowLayoutPanel1);
                        this.Controls.Add(OKButton);
                        OKButton.Text = "Open";
                        cancelButton.Text = "Cancel";
                        AcceptButton = OKButton;
                        break;
                }
            }
        }

        private void SetTextboxToSelectedItemName()
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;

            if (items.Count == 1)
            {
                CustomListItem anItem = (CustomListItem)items[0];
                createBox.Text = anItem.getName();
            }
            else
            {
                createBox.Text = "";
            }
        }

        private void cloneButton_Click(object sender, EventArgs e)
        {
            String userEnteredName = createBox.Text;

            Cursor.Current = Cursors.WaitCursor;

            System.Windows.Forms.ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
            bool nameOK = true;
            if (items.Count > 0)
            {
                CustomListItem anItem = (CustomListItem)items[0];
                int projectIDToClone = anItem.getID();

                try
                {
                    AME.AMEManager.CopyParameters parameters = new AMEManager.CopyParameters();
                    parameters.ClearWriteDB = false;
                    parameters.ReadingController = rc;
                    parameters.StartingRootID = projectIDToClone;
                    if (userEnteredName != null && userEnteredName.Length > 0)
                    {
                        // add date time if needed
                        XPathNavigator self = rc.GetComponent(projectIDToClone).CreateNavigator().SelectSingleNode("Components/Component");
                        String rootName = self.GetAttribute(XmlSchemaConstants.Display.Component.Name, String.Empty);
                        if (rootName.Equals(userEnteredName))
                        {
                            userEnteredName = userEnteredName + " " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
                        }

                        parameters.NewName = userEnteredName;

                        // does this match any other project names?
                        XPathNavigator projectNavigator = rc.GetRootComponents(new ComponentOptions()).CreateNavigator();
                        XPathNodeIterator listofRootElements = projectNavigator.Select("//Components/Component");
                        Dictionary<String, String> projects = new Dictionary<String, String>();

                        foreach (XPathNavigator paramNav in listofRootElements)
                        {
                            String name = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                            projects.Add(name, name);
                        }
                        if (projects.ContainsKey(userEnteredName))
                        {
                            nameOK = false;
                            MessageBox.Show("Project name: " + userEnteredName + " already exists.  Please choose another name.");
                        }
                    }

                    if (nameOK)
                    {
                        String newName;
                        int newProjectID = AMEManager.Instance.Copy(parameters, out newName);

                        if (newProjectID != -1)
                        {
                            // do open after create
                            m_IsProjectSelected = true;
                            WasNewSelection = true;

                            this.SelectedProjectID = newProjectID;
                            this.SelectedProjectName = newName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                Cursor.Current = Cursors.Default;
            }
            if (nameOK)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; 
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            switch (MyProjectFormType)
            {
                case ProjectFormType.Clone:
                    {
                        DialogResult = DialogResult.OK;
                        break;
                    }
                case ProjectFormType.New:
                    {
                        DialogResult = DialogResult.OK; 
                        break;
                    }
                case ProjectFormType.Delete:
                    {
                        DialogResult = DialogResult.OK; 
                        break;
                    }
                case ProjectFormType.Open:
                    {
                        OpenSelectedProject(); 
                        break;
                    }
            }
        }

        private void OpenSelectedProject()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                CheckForNewSelection();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No project selected.");
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            // check to make sure the project doesn't already exist
            String userEnteredProject = createBox.Text;

            if (!userEnteredProject.Equals(""))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    int newProjectID = rc.CreateRootComponent(userEnteredProject, "");

                    // do open after create
                    m_IsProjectSelected = true;
                    WasNewSelection = true;

                    this.SelectedProjectID = newProjectID;
                    this.SelectedProjectName = userEnteredProject;

                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Please enter a name for the new project in the text field.");
            }
        }

        private void RefreshList()
        {
            ComponentOptions compOptions = new ComponentOptions();
            IXPathNavigable iNavigator = rc.GetRootComponents(compOptions);

            XPathNavigator navigator = iNavigator.CreateNavigator();

            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            Byte[] bs = System.Text.Encoding.UTF8.GetBytes(navigator.OuterXml);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (Byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String newCrc = s.ToString();

            if (!crc.Equals(newCrc)) // CRC check - see CustomTreeView and ParameterTable
            {
                DrawingUtility.SuspendDrawing(this);

                listView1.Items.Clear();

                crc = newCrc;

                XPathNodeIterator listofRootElements;
                listofRootElements = navigator.Select("//Components/Component");

                if (listofRootElements != null)
                {
                    string nodeName;
                    int nodeID;

                    foreach (XPathNavigator paramNav in listofRootElements)
                    {
                        //add the name/id paired item.
                        nodeName = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                        nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));

                        CustomListItem item = new CustomListItem(nodeName, "", nodeID, "");
                        if (listView1.SmallImageList != null && listView1.SmallImageList.Images.Count > 0)
                        {
                            item.ImageIndex = 0;
                        }
                        listView1.Items.Add(item);
                    }
                }

                DrawingUtility.ResumeDrawing(this);
            }

            listView1.SelectedItems.Clear();

            // delete / open display selected project
            if (MyProjectFormType == ProjectFormType.Delete || MyProjectFormType == ProjectFormType.Open)
            {
                System.Windows.Forms.ListView.ListViewItemCollection items = this.listView1.Items;

                foreach(ListViewItem anItem in items)
                {
                    CustomListItem theItem = (CustomListItem)anItem;

                    if (theItem.getID() == SelectedProjectID)
                    {
                        theItem.Selected = true;
                        this.listView1.Select();
                    }
                }
            }

            this.Show();
        }
    
        private void Delete_Project_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems ;

            if (items.Count > 0)
            {
                DialogResult result = MessageBox.Show(m_warningText, m_warningCaption, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    CustomListItem anItem = (CustomListItem)items[0];
                    int deleteID = anItem.getID();

                    if (deleteID == SelectedProjectID)
                    {
                        IsProjectSelected = false; // Trigger close, deleting currently selected project
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    rc.TurnViewUpdateOff();

                    rc.DeleteComponentAndChildren(deleteID, true);

                    RefreshList();

                    rc.TurnViewUpdateOn();

                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("No project selected");
            }
        }

        private void CheckForNewSelection()
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;

            if (items.Count == 1)
            {
                m_IsProjectSelected = true;

                CustomListItem theItem = (CustomListItem)items[0];

                if (theItem.getID() != SelectedProjectID)
                {
                    this.WasNewSelection = true;

                    this.SelectedProjectID = theItem.getID();
                    this.SelectedProjectName = theItem.getName();
                }
            }
        }
    }
}
