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
    public partial class VoiceChannel : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 channelId = -1;
        private VSGController controller;
        private String voiceChannelName;

        public VoiceChannel()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }
        
        public Int32 ChannelId
        {
            get
            {
                return channelId;
            }
            set
            {
                channelId = value;
                if (channelId >= 0)
                {
                    customLinkBox1.DisplayRootId = controller.ScenarioId;
                    customLinkBox1.ConnectRootId = channelId;
                    customLinkBox1.ConnectFromId = channelId;

                    customNumericUpDown1.ComponentId = channelId;
                    customParameterTextBox1.ComponentId = channelId;

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }
        
        public String VoiceChannelName
        {
            get
            {
                return voiceChannelName;
            }
            set
            {
                voiceChannelName = value;
                customTabPage1.Description = voiceChannelName;
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
                customLinkBox1.Controller = controller;
                customNumericUpDown1.Controller = controller;
                customParameterTextBox1.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            customLinkBox1.UpdateViewComponent();
            customNumericUpDown1.UpdateViewComponent();
            customParameterTextBox1.UpdateViewComponent();
        }

        #endregion
      
    }
}
