using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using AME.Views.View_Components;
using AME.Controllers;
using Forms;

namespace AME.Views.View_Component_Packages
{
    public partial class ViewComponentPanel : UserControl
    {
        public ViewComponentPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }
    }
}