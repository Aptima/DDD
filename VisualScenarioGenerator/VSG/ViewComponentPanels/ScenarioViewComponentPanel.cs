using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using VSG.Controllers;

using AME;
using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponentPanels
{
    public partial class ScenarioViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 rootId = -1;
        private VSGController vsgController;
        private RootController projectController;

        public ScenarioViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this, UpdateType.Parameter);

            InitializeComponent();

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            projectController = (RootController)AMEManager.Instance.Get("Project");
            vsgController.RegisterForUpdate(this);

            mapPlayfield1.Controller = vsgController;
            iconLibrary1.Controller = vsgController;
            scenarioInfo1.Controller = vsgController;
        }

        public void UpdateViewComponent()
        {
            mapPlayfield1.UpdateViewComponent();
            iconLibrary1.UpdateViewComponent();
            scenarioInfo1.UpdateViewComponent();
        }

        #region IViewComponentPanel Members

        public int RootId
        {
            get
            {
                return rootId;
            }
            set
            {
                rootId = value;
                if (rootId >= 0)
                {
                    // Set playfieldId in appropriate views. Playfield component is created automatically.
                    ComponentOptions compOptions = new ComponentOptions();
                    IXPathNavigable inav = projectController.GetComponentAndChildren(rootId, vsgController.ConfigurationLinkType, compOptions);
                    XPathNavigator nav = inav.CreateNavigator().SelectSingleNode("/Components/Component/Component[@Type='Playfield']");

                        String playfieldId = nav.GetAttribute(AME.Controllers.XmlSchemaConstants.Display.Component.Id, nav.NamespaceURI);
                        mapPlayfield1.PlayfieldId = Int32.Parse(playfieldId);
                        iconLibrary1.PlayfieldId = Int32.Parse(playfieldId);
                        scenarioInfo1.ScenarioId = rootId;
                    }
                }
            }

        #endregion
    }
}

