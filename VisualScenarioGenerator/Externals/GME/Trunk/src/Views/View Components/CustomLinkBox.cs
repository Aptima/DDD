using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using AME.Controllers;

namespace AME.Views.View_Components
{
    public partial class CustomLinkBox : CheckedListBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController controller;
        private Int32 displayRootId = -1; 
        private String displayComponentType;
        private String displayLinkType;
        private Boolean displayRecursiveCheck = false;
        private String displayParameterCategory = String.Empty;
        private String displayParameterName = String.Empty;
        private Int32 connectRootId = -1;
        private Int32 connectFromId = -1;
        private String connectLinkType;
        private UInt32 checkLinkLevel = 1;

        private Boolean filterResult = false;
        private String parameterFilterCategory = String.Empty;
        private String parameterFilterName = String.Empty;
        private String parameterFilterValue = String.Empty;

        private Boolean oneToMany = false;
        private Boolean updatingView = false;

        public Boolean UpdatingView
        {
            get { return updatingView; }
        }

        public Int32 DisplayRootId
        {
            get
            {
                return displayRootId;
            }
            set
            {
                displayRootId = value;
            }
        }

        public String DisplayComponentType
        {
            get {
                return displayComponentType; 
            }
            set 
            {
                displayComponentType = value;
            }
        }

        public String DisplayLinkType
        {
            get
            {
                return displayLinkType;
            }
            set
            {
                displayLinkType = value;
            }
        }

        public Boolean DisplayRecursiveCheck
        {
            get
            {
                return displayRecursiveCheck;
            }
            set
            {
                displayRecursiveCheck = value;
            }
        }

        public String DisplayParameterCategory
        {
            get
            {
                return displayParameterCategory;
            }
            set
            {
                displayParameterCategory = value;
            }
        }

        public String DisplayParameterName
        {
            get
            {
                return displayParameterName;
            }
            set
            {
                displayParameterName = value;
            }
        }

        public Int32 ConnectRootId
        {
            get
            {
                return connectRootId;
            }
            set
            {
                connectRootId = value;
            }
        }

        public Int32 ConnectFromId
        {
            get
            {
                return connectFromId;
            }
            set
            {
                connectFromId = value;
            }
        }

        public UInt32 CheckLinkLevel
        {
            get
            {
                return checkLinkLevel;
            }
            set 
            {
                checkLinkLevel = value; 
            }
        }

        public String ConnectLinkType
        {
            get
            {
                return connectLinkType;
            }
            set
            {
                connectLinkType = value;
            }
        }

        public CustomLinkBox()
        {
            InitializeComponent();
        }

        public Boolean OneToMany
        {
            get 
            { 
                return oneToMany; 
            }
            set 
            {
                oneToMany = value; 
            }
        }

        public Boolean FilterResult
        {
            get 
            { 
                return filterResult; 
            }
            set 
            {
                filterResult = value; 
            }
        }

        public String ParameterFilterCategory
        {
            get
            { 
                return parameterFilterCategory;
            }
            set
            {
                parameterFilterCategory = value;
            }
        }

        public String ParameterFilterName
        {
            get
            { 
                return parameterFilterName;
            }
            set
            {
                parameterFilterName = value;
            }
        }
       
        public String ParameterFilterValue
        {
            get 
            { 
                return parameterFilterValue;
            }
            set
            {
                parameterFilterValue = value; 
            }
        }

        public CustomLinkBox(IContainer container)
        {
            myHelper = new ViewComponentHelper(this);

            container.Add(this);

            InitializeComponent();
        }

        public void DeleteAllLinks()
        {
            updatingView = true;
            Boolean internalOff = false;
            if (controller.ViewUpdateStatus)
            {
                internalOff = true;
                controller.TurnViewUpdateOff();
            }
            // Disconnect all other links.
            for (Int32 i = 0; i <= (this.Items.Count - 1); i++)
            {
                if (this.GetItemChecked(i))
                {
                    CustomLinkBoxItem dItem = this.Items[i] as CustomLinkBoxItem;
                    if (controller.DeleteLink(dItem.LinkId))
                    {
                        dItem.LinkId = -1;
                        this.SetItemChecked(i, false);
                    }
                }
            }
            if (internalOff)
            {
                Controller.TurnViewUpdateOn(false, false); // don't send upates
            }
            updatingView = false;
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
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

            // Remove items.
            this.Items.Clear();

            ComponentOptions options = new ComponentOptions(); // Defaults should do.
            options.LevelDown = 1;
            IXPathNavigable iNav = controller.GetComponentAndChildren(displayRootId, displayLinkType, options);
            XPathNavigator nav = iNav.CreateNavigator();

            String displayXpath = String.Format("/Components/Component[@ID='{0}']/Component[@Type='{1}']", displayRootId, displayComponentType);
            if (displayRecursiveCheck)
                displayXpath = String.Format("/Components/Component[@ID='{0}']//Component[@Type='{1}']", displayRootId, displayComponentType);
            XPathNodeIterator itNav = nav.Select(displayXpath);

            // Add new items.
            while (itNav.MoveNext())
            {
                String atrributeId = itNav.Current.GetAttribute("ID", nav.NamespaceURI);
                String atrributeType = itNav.Current.GetAttribute("Type", nav.NamespaceURI);
                String atrributeName = itNav.Current.GetAttribute("Name", nav.NamespaceURI);
                //String attributeLinkId = itNav.Current.GetAttribute("LinkID", nav.NamespaceURI);

                CustomLinkBoxItem linkItem = new CustomLinkBoxItem();
                linkItem.Id = Int32.Parse(atrributeId);
                linkItem.LinkId = -1;// Int32.Parse(attributeLinkId);
                linkItem.Type = atrributeType;
                if (displayParameterName.Equals(String.Empty))
                {
                    linkItem.Name = atrributeName;
                    if (filterResult)
                    {
                        IXPathNavigable iNavParameters = controller.GetParametersForComponent(Int32.Parse(atrributeId));
                        XPathNavigator navParameters = iNavParameters.CreateNavigator();
                        XPathNavigator node = navParameters.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterFilterCategory, parameterFilterName));
                        if (node != null)
                        {
                            String text = node.GetAttribute("value", String.Empty);
                            if (text.Equals(parameterFilterValue))
                            {
                                this.Items.Add(linkItem);
                            }
                        }
                    }
                    else
                        this.Items.Add(linkItem);
                }
                else
                {
                    IXPathNavigable iNavParameters = controller.GetParametersForComponent(Int32.Parse(atrributeId));
                    XPathNavigator navParameters = iNavParameters.CreateNavigator();
                    XPathNavigator navDisplayParameterName = navParameters.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", displayParameterCategory, displayParameterName));
                    if (navDisplayParameterName != null)
                    {
                        String displayParameterValue = navDisplayParameterName.GetAttribute("value", String.Empty);
                        if (!displayParameterValue.Equals(String.Empty))
                        {
                            linkItem.Name = displayParameterValue;
                            if (filterResult)
                            {
                                XPathNavigator navParameterFilterName = navParameters.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterFilterCategory, parameterFilterName));
                                if (navParameterFilterName != null)
                                {
                                    String parameterValue = navParameterFilterName.GetAttribute("value", String.Empty);
                                    if (parameterValue.Equals(parameterFilterValue))
                                    {
                                        this.Items.Add(linkItem);
                                    }
                                }
                            }
                            else
                                this.Items.Add(linkItem);
                        }
                    }
                }                
            }

            // Now check for links.
            updatingView = true;
            checkForLinks();
            updatingView = false;

            DrawingUtility.ResumeDrawing(this);
        }

        #endregion

        private void checkForLinks()
        {
            ComponentOptions options = new ComponentOptions(); // Defaults should do.
            //options.LevelDown = checkLinkLevel;
            IXPathNavigable iNav = controller.GetComponentAndChildren(connectRootId, connectLinkType, options);
            XPathNavigator nav = iNav.CreateNavigator();
            XPathNodeIterator itNav = nav.Select(String.Format("//Component[@ID='{0}']/Component[@Type='{1}']", connectFromId, displayComponentType));

            // Check appropriate items.
            while (itNav.MoveNext())
            {
                String atrributeId = itNav.Current.GetAttribute("ID", nav.NamespaceURI);
                String atrributeType = itNav.Current.GetAttribute("Type", nav.NamespaceURI);
                String atrributeName = itNav.Current.GetAttribute("Name", nav.NamespaceURI);
                String atrributeLinkId = itNav.Current.GetAttribute("LinkID", nav.NamespaceURI);
                Int32 id = Int32.Parse(atrributeId);
                Int32 linkId = Int32.Parse(atrributeLinkId);

                for (Int32 i = 0; i <= (this.Items.Count - 1); i++)
                {
                    CustomLinkBoxItem item = this.Items[i] as CustomLinkBoxItem;
                    if (id.Equals(item.Id))
                    {
                        this.SetItemChecked(i, true);
                        item.LinkId = linkId;
                        this.SetSelected(i, true);
                    }
                }
            }
        }

        private void CustomLinkBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!updatingView)
            {
                Controller.TurnViewUpdateOff();
                updatingView = true;
                // Make connect if checking.
                if (e.NewValue.Equals(CheckState.Checked))
                {
                    if (!oneToMany)
                    {
                        // Disconnect all other links.
                        for (Int32 i = 0; i <= (this.Items.Count - 1); i++)
                        {
                            if (this.GetItemChecked(i))
                            {
                                CustomLinkBoxItem dItem = this.Items[i] as CustomLinkBoxItem;
                                if (controller.DeleteLink(dItem.LinkId))
                                {
                                    dItem.LinkId = -1;
                                    this.SetItemChecked(i, false);
                                }
                            }
                        }
                    }
                    CustomLinkBoxItem cItem = this.Items[e.Index] as CustomLinkBoxItem;
                    try
                    {
                        cItem.LinkId = controller.Connect(connectRootId, connectFromId, cItem.Id, connectLinkType);
                    }
                    catch (System.ArgumentException ae)
                    {
                        System.Windows.Forms.MessageBox.Show(ae.Message, "Link Error");
                        e.NewValue = CheckState.Unchecked;
                    }
                    catch (System.Exception se)
                    {
                        System.Windows.Forms.MessageBox.Show(se.Message, "Link not allowed.");
                        e.NewValue = CheckState.Unchecked;
                    }
                }
                else if (e.NewValue.Equals(CheckState.Unchecked))
                {
                    CustomLinkBoxItem dItem = this.Items[e.Index] as CustomLinkBoxItem;
                    if (controller.DeleteLink(dItem.LinkId))
                    {
                        dItem.LinkId = -1;
                    }
                }
                updatingView = false;
                Controller.TurnViewUpdateOn(true, false); // only do component update
            }
        }

        public Int32 GetSelectedComponentId()
        {
            if (this.SelectedItems.Count > 0)
            {
                CustomLinkBoxItem dItem = this.SelectedItems[0] as CustomLinkBoxItem;
                return dItem.Id;
            }
            else
                return -1;
        }

        public Int32 GetSelectedLinkId()
        {
            if (this.SelectedItems.Count > 0)
            {
                CustomLinkBoxItem dItem = this.SelectedItems[0] as CustomLinkBoxItem;
                return dItem.LinkId;
            }
            else 
                return -1;
        }
    }

    class CustomLinkBoxItem
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
