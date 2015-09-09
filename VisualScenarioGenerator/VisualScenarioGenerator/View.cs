using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;
using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator
{
    public class View
    {
        protected Ctl_NavigatorPane _navigation_panel;
        protected Ctl_ContentPane _content_panel;

        public Ctl_NavigatorPane NavigatorPanel
        {
            get
            {
                return _navigation_panel;
            }
        }

        public Ctl_ContentPane ContentPanel
        {
            get
            {
                return _content_panel;
            }
        }


        public View(Ctl_NavigatorPane navigation_panel, Ctl_ContentPane view_panel)
        {
            _navigation_panel = navigation_panel;
            _content_panel = view_panel;
            NavigatorPanel.BindContentPane(this, ContentPanel);
            ContentPanel.BindNavigationPane(this, NavigatorPanel);
        }

        public void BindNavigatorControl(Control host, DockStyle style, bool visible)
        {
            _navigation_panel.Parent = host;
            _navigation_panel.Dock = style;
            _navigation_panel.Visible = visible;
        }
        public void BindViewControl(Control host, DockStyle style, bool visible)
        {
            _content_panel.Parent = host;
            _content_panel.Dock = style;
            _content_panel.Visible = visible;
        }

        public void Hide()
        {
            _navigation_panel.Hide();
            _content_panel.Hide();
        }
        public void Show()
        {
            _navigation_panel.Show();
            _content_panel.Show();
        }
        public virtual void Notify(object object_data)
        {
        }

        public virtual void UpdateNavigatorPanel(Control control, object object_data)
        {

        }
        public virtual void UpdateContentPanel(object object_data)
        {
        }

        public virtual void UpdateView(object object_data)
        {
        }
    }
}
