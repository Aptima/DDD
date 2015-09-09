using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_NavigatorPane : UserControl
    {
        protected View _view = null;
        protected Ctl_ContentPane _content_pane = null;

        public Ctl_NavigatorPane()
        {
            InitializeComponent();
        }

        public void BindContentPane(View view, Ctl_ContentPane control)
        {
            _view = view;
            _content_pane = control;
        }

        public virtual void Update(object object_data)
        {
            throw new Exception("Update hasn't been implemented");
        }
        public void Notify(object object_data)
        {
            _view.UpdateContentPanel(object_data);
            _view.Notify(object_data);
        }

    }
}
