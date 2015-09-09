using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;

using AME.Views.View_Components;
using AME.Controllers;

using VSG.Controllers;

namespace VSG.ViewComponents
{
    public partial class IconListView : ListView, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController controller;
        private Int32 componentId = -1;
        private String iconParameterCategory;
        private String iconParameterName;
        private eParamParentType iconParameterType = eParamParentType.Component;
        private IntPtr handle;
        private Boolean updateingView = false;

        public IconListView()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

            base.MultiSelect = false;
            base.HideSelection = false;
        }

        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }
            set
            {
                componentId = value;
            }
        }

        public String IconParameterName
        {
            get
            {
                return iconParameterName;
            }
            set
            {
                iconParameterName = value;
            }
        }

        public String IconParameterCategory
        {
            get
            {
                return iconParameterCategory;
            }
            set
            {
                iconParameterCategory = value;
            }
        }

        public eParamParentType IconParameterType
        {
            get
            {
                return iconParameterType;
            }
            set
            {
                iconParameterType = value;
            }
        }

        public new Boolean MultiSelect
        {
            get
            {
                return base.MultiSelect;
            }
            set
            {
                base.MultiSelect = false;
            }
        }

        public new Boolean HideSelection
        {
            get
            {
                return base.HideSelection;
            }
            set
            {
                base.HideSelection = false;
            }
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
                controller = value as VSGController;
            }
        }

        public void UpdateViewComponent()
        {
            updateingView = true;
            if (controller != null && controller.CurrentIconLibrary != null)
            {
                if (!handle.Equals(controller.CurrentIconLibrary.Handle))
                {
                    SmallImageList = controller.CurrentIconLibrary;
                    LargeImageList = controller.CurrentIconLibrary;


                    Items.Clear();
                    foreach (string name in controller.CurrentIconLibrary.Images.Keys)
                    {
                        Items.Add(name, name, controller.CurrentIconLibrary.Images.IndexOfKey(name));
                    }

                    handle = controller.CurrentIconLibrary.Handle;
                }

                if (componentId >= 0)
                {
                    if (!iconParameterCategory.Equals(String.Empty) && !iconParameterName.Equals(String.Empty))
                    {
                        IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                        XPathNavigator nav = inav.CreateNavigator();
                        XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", iconParameterCategory, iconParameterName));
                        if (node != null)
                        {
                            String key = node.GetAttribute("value", String.Empty);
                            ListViewItem[] items = this.Items.Find(key, true);
                            if (items.Length > 0 && items[0] != null)
                                this.Items[items[0].Index].Selected = true;
                        }
                    }
                }
            }
            else
            {
                Items.Clear();
            }
            updateingView = false;
        }

        #endregion

        private void IconListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedItems.Count > 0)
            {
                if (!updateingView)
                {
                    if (controller != null && componentId >= 0)
                    {
                        try
                        {
                            controller.UpdateParameters(componentId, iconParameterCategory + "." + iconParameterName, this.SelectedItems[0].Text, iconParameterType);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                        }
                    }
                }
            }
        }
    }
}
