using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;

namespace AME.Views.View_Components
{
    public partial class CustomLinkComboBox : ComboBox, IViewComponent
    {
        private IController controller;
        private ViewComponentHelper myHelper;

        private Int32 displayRootId = -1;
        private String displayComponent;
        private String displayLinkType;
        private Int32 connectRootId = -1;
        private Int32 connectFromId = -1;
        private String connectLinkType;
        private UInt32 linkLevel = 1;
        private Boolean connectLinkDynamic = false;
        private Int32 connectDynamicId = -1;

        private Boolean updating = false;

        private String xsl;
        private XslCompiledTransform transform;

        public CustomLinkComboBox()
        {
            InitializeComponent();

            myHelper = new ViewComponentHelper(this, UpdateType.Component);
        }

        public CustomLinkComboBox(IContainer container) : this()
        {
            container.Add(this);
        }

        public Int32 DisplayRootId
        {
            get { return displayRootId; }
            set { displayRootId = value; }
        }

        public String DisplayComponent
        {
            get { return displayComponent; }
            set { displayComponent = value; }
        }

        public String DisplayLinkType
        {
            get { return displayLinkType; }
            set { displayLinkType = value; }
        }

        public Int32 ConnectRootId
        {
            get { return connectRootId; }
            set { connectRootId = value; }
        }

        public Int32 ConnectFromId
        {
            get { return connectFromId; }
            set { connectFromId = value; }
        }

        public String ConnectLinkType
        {
            get { return connectLinkType; }
            set { connectLinkType = value; }
        }

        public UInt32 LinkLevel
        {
            get { return linkLevel; }
            set { linkLevel = value; }
        }

        public Boolean ConnectLinkDynamic
        {
            get { return connectLinkDynamic; }
            set { connectLinkDynamic = value; }
        }

        public Int32 ConnectDynamicId
        {
            get { return connectDynamicId; }
            set { connectDynamicId = value; }
        }

        public String Xsl
        {
            get
            {
                return xsl;
            }
            set
            {
                if (value != null)
                {
                    transform = new XslCompiledTransform();
                    try
                    {
                        XmlReader xslreader = controller.GetXSL(value);
                        transform.Load(xslreader);
                        xslreader.Close();
                        xsl = value;
                    }
                    catch (Exception ex)
                    {
                        transform = null;
                        xsl = null;
                        MessageBox.Show(ex.Message, "Failed to load transform - is a controller set? (Diagram)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #region Overrides

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            if (updating)
                return;

            if (this.Tag != null)
            {
                Int32 linkId = (Int32)this.Tag;
                controller.TurnViewUpdateOff();
                controller.DeleteLink(linkId);
                controller.TurnViewUpdateOn(false, false);
            }

            try
            {
                controller.TurnViewUpdateOff();
                CustomLinkComboBoxItem item = this.SelectedItem as CustomLinkComboBoxItem;
                int linkID = -1;
                if (connectLinkDynamic)
                    linkID = controller.Connect(connectRootId, connectFromId, item.Id, controller.GetDynamicLinkType(connectLinkType, connectDynamicId.ToString()));
                else
                    linkID = controller.Connect(connectRootId, connectFromId, item.Id, connectLinkType);
                controller.TurnViewUpdateOn(false, false);
                this.Tag = linkID;
            }
            catch (Exception ex)
            {
                controller.TurnViewUpdateOn(false, false);
                System.Windows.Forms.MessageBox.Show(ex.Message, "Link Error");
                UpdateViewComponent();
            }

            base.OnSelectedValueChanged(e);
        }

        #endregion

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            DrawingUtility.SuspendDrawing(this);

            updating = true;

            // Remove items.
            this.Items.Clear();

            ComponentOptions options = new ComponentOptions(); // Defaults should do.
            options.LevelDown = 1;
            IXPathNavigable iNav = controller.GetComponentAndChildren(displayRootId, displayLinkType, options);
            XPathNavigator nav = iNav.CreateNavigator();

            if (transform != null)
            {
                XmlDocument newDocument = new XmlDocument();
                using (XmlWriter writer = newDocument.CreateNavigator().AppendChild())
                {
                    try
                    {
                        transform.Transform(iNav, (XsltArgumentList)null, writer);
                    }
                    catch (XsltException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                nav = newDocument.CreateNavigator();
            }

            XPathNodeIterator itNav = nav.Select(String.Format("/Components/Component[@ID='{0}']/Component[@Type='{1}']", displayRootId, displayComponent));

            // Add new items.
            while (itNav.MoveNext())
            {
                String atrributeId = itNav.Current.GetAttribute("ID", nav.NamespaceURI);
                String atrributeType = itNav.Current.GetAttribute("Type", nav.NamespaceURI);
                String atrributeName = itNav.Current.GetAttribute("Name", nav.NamespaceURI);

                CustomLinkComboBoxItem item = new CustomLinkComboBoxItem();
                item.Id = Int32.Parse(atrributeId);
                item.Name = atrributeName;
                item.Type = atrributeType;

                this.Items.Add(item);
            }

            options.LevelDown = linkLevel;
            IXPathNavigable iNavConnectLink;
            if (connectLinkDynamic)
                iNavConnectLink = controller.GetComponentAndChildren(connectRootId, controller.GetDynamicLinkType(connectLinkType, connectDynamicId.ToString()), options);
            else
                iNavConnectLink = controller.GetComponentAndChildren(connectRootId, connectLinkType, options);
            XPathNavigator navConnectLink = iNavConnectLink.CreateNavigator();
            XPathNavigator navLink = navConnectLink.SelectSingleNode(String.Format("//Component[@ID='{0}']/Component[@Type='{1}']", connectFromId, displayComponent));

            if (navLink != null)
            {
                String atrributeId = navLink.GetAttribute("ID", nav.NamespaceURI);
                String atrributeType = navLink.GetAttribute("Type", nav.NamespaceURI);
                String atrributeName = navLink.GetAttribute("Name", nav.NamespaceURI);
                String atrributeLinkId = navLink.GetAttribute("LinkID", nav.NamespaceURI);
                Int32 id = Int32.Parse(atrributeId);
                Int32 linkId = Int32.Parse(atrributeLinkId);

                for (Int32 i = 0; i <= (this.Items.Count - 1); i++)
                {
                    CustomLinkComboBoxItem item = this.Items[i] as CustomLinkComboBoxItem;
                    if (id.Equals(item.Id))
                    {
                        this.SelectedItem = item;
                        this.Tag = linkId;
                        item.LinkId = linkId;
                        break;
                    }
                }
                //this.Text = String.Empty;
            }
            else
            {
                this.Tag = null;
                this.Text = String.Empty;
            }

            updating = false;

            DrawingUtility.ResumeDrawing(this);
        }

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        #endregion
    }

    class CustomLinkComboBoxItem
    {
        private Int32 linkId = -1;

        public Int32 LinkId
        {
            get { return linkId; }
            set { linkId = value; }
        }

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

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public override String ToString()
        {
            return name;
        }
    }
}
