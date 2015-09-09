using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;

namespace AME.Views.View_Components
{
    public partial class CustomTabControl : TabControl
    {
        public CustomTabControl()
        {
            InitializeComponent();
        }

        public CustomTabControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [Editor(typeof(CustomTabPageCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }
    }
}
