using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    
    public partial class DecisionMaker : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 decisionMakerId = -1;
        private VSGController controller;
        private String decisionMakerName;
        private String[] linktypes;

        public DecisionMaker()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

        }

        public Int32 DecisionMakerId
        {
            get
            {
                return decisionMakerId;
            }
            set
            {
                decisionMakerId = value;
                if (decisionMakerId >= 0)
                {
                    txtRole.ComponentId = decisionMakerId;
                    txtBriefing.ComponentId = decisionMakerId;
                    customLinkBox1.DisplayRootId = controller.ScenarioId;
                    customLinkBox1.ConnectRootId = decisionMakerId;
                    customLinkBox1.ConnectFromId = decisionMakerId;

                    customParameterEnumBox1.ComponentId = decisionMakerId;

                    customLinkBoxChatWith.ConnectRootId = decisionMakerId;
                    customLinkBoxChatWith.ConnectFromId = decisionMakerId;
                    customLinkBoxChatWith.DisplayRootId = controller.ScenarioId;

                    customLinkBoxSpeakWith.ConnectRootId = decisionMakerId;
                    customLinkBoxSpeakWith.ConnectFromId = decisionMakerId;
                    customLinkBoxSpeakWith.DisplayRootId = controller.ScenarioId;

                    customLinkBoxWhiteboardWith.ConnectRootId = decisionMakerId;
                    customLinkBoxWhiteboardWith.ConnectFromId = decisionMakerId;
                    customLinkBoxWhiteboardWith.DisplayRootId = controller.ScenarioId;

                    customLinkBoxReportTo.ConnectFromId = decisionMakerId;
                    customLinkBoxReportTo.ConnectRootId = decisionMakerId;
                    customLinkBoxReportTo.DisplayRootId = controller.ScenarioId;

                    customCheckBox1.ComponentId = decisionMakerId;

                    customCheckBox2.ComponentId = decisionMakerId;

                    customCheckBox3.ComponentId = decisionMakerId;

                    // use dynamic link type for can chat
                    //if (controller != null && customLinkBoxChatWith.ConnectLinkType != null)
                    //{
                    //    String dynamic = controller.GetDynamicLinkType(customLinkBoxChatWith.ConnectLinkType, decisionMakerId.ToString());
                    //    customLinkBoxChatWith.ConnectLinkType = dynamic;
                    //}

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }

        public String DecisionMakerName
        {
            get
            {
                return decisionMakerName; 
            }
            set
            {
                decisionMakerName = value;
                customTabPage1.Description = decisionMakerName;
            }
        }

        private void reset()
        {

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
                controller = (VSGController)value;  // Only give this a vsg controller.
                txtRole.Controller = controller;
                txtBriefing.Controller = controller;
                customLinkBox1.Controller = controller;
                customParameterEnumBox1.Controller = controller;
                customLinkBoxChatWith.Controller = controller;
                customLinkBoxSpeakWith.Controller = controller;
                customLinkBoxWhiteboardWith.Controller = controller;
                customLinkBoxReportTo.Controller = controller;
                customCheckBox1.Controller = controller;
                customCheckBox2.Controller = controller;
                customCheckBox3.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            txtRole.UpdateViewComponent();
            txtBriefing.UpdateViewComponent();
            customLinkBox1.UpdateViewComponent();
            customParameterEnumBox1.UpdateViewComponent();
            customLinkBoxChatWith.UpdateViewComponent();
            customLinkBoxSpeakWith.UpdateViewComponent();
            customLinkBoxWhiteboardWith.UpdateViewComponent();
            customLinkBoxReportTo.UpdateViewComponent();
            customCheckBox1.UpdateViewComponent();
            customCheckBox2.UpdateViewComponent();
            customCheckBox3.UpdateViewComponent();
            try
            {
                colorSwatch.BackColor = Color.FromName(customParameterEnumBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {
                colorSwatch.BackColor = SystemColors.Control;
            }
        }

        #endregion

        private void customParameterEnumBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customParameterEnumBox1.SelectedItem != null)
            {
                try
                {
                    colorSwatch.BackColor = Color.FromName(customParameterEnumBox1.SelectedItem.ToString());
                }
                catch (Exception)
                {
                    colorSwatch.BackColor = SystemColors.Control;
                }
            }
        }
    }
}
