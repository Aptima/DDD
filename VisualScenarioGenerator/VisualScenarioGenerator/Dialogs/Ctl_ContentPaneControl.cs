using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_ContentPaneControl : UserControl
    {
        public Ctl_ContentPane ContentPane = null;
        public Ctl_ContentPaneControl()
        {
            InitializeComponent();
        }
        public virtual void Update(object object_data)
        {
            throw new Exception("Update hasn't been implemented");
        }
        public void Notify(object object_data)
        {
            if (ContentPane != null)
            {
                ContentPane.Notify(object_data);
            }
            else
            {
                throw new NullReferenceException("ContentPane is uninitialized.");
            }
        }

    }
}
