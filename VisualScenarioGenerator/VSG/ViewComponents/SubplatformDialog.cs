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
using VSG.Controllers;

namespace VSG.ViewComponents
{
    public partial class SubplatformDialog : Form
    {
        private int displayID = -1;
        private IController _controller;

        public SubplatformDialog()
        {
            InitializeComponent();

            
        }
        public void Setup()
        {
            kindLinkBox.DisplayComponentType = "Species";
            kindLinkBox.DisplayLinkType = "Scenario";
            kindLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            kindLinkBox.UpdateViewComponent();
            kindLinkBox.ConnectFromId = DisplayID;
            kindLinkBox.ConnectRootId = DisplayID;
            kindLinkBox.ConnectLinkType = "SubplatformKind";
            kindLinkBox.UpdateViewComponent();
        }

        public IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
                kindLinkBox.Controller = value;
            }
        }

        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                displayID = value;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }


    }
}