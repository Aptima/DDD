using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace VisualScenarioGenerator.Dialogs
{
    public partial  class Ctl_ContentPane : UserControl
    {
        protected View _view = null;
        protected Ctl_NavigatorPane _navigation_control = null;

        public Ctl_ContentPane()
        {
            InitializeComponent();
        }

        public void BindNavigationPane(View view, Ctl_NavigatorPane control)
        {
            _view = view;
            _navigation_control = control;
        }

        public virtual void Update(object object_data)
        {
            throw new Exception("Update hasn't been implemented");
        }
        public void Notify(object object_data)
        {
            _view.UpdateNavigatorPanel(this, object_data);
            _view.Notify(object_data);
        }
        
    }
}
