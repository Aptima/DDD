using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace AME.Views.View_Components
{
    public partial class CardLayoutPanel : Panel
    {
        private String current;
        public string Current
        {
            get { return current; }
            set
            {
                if (current.Equals(value))
                    return;

                if (value.Equals(String.Empty))
                {
                    Controls[current].Visible = false;
                    current = value;
                    return;
                }

                if (!Controls.ContainsKey(value))
                    return;

                if (!current.Equals(String.Empty))
                    Controls[current].Visible = false;

                Controls[value].Visible = true;
                current = value;
            }
        }
        public CardLayoutPanel()
        {
            InitializeComponent();
            current = String.Empty;
        }

        public CardLayoutPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            current = String.Empty;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Dock = DockStyle.Fill;
            e.Control.Visible = false;
            //Current = e.Control.Name;   

            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (current.Equals(e.Control.Name))
                current = String.Empty; 

            base.OnControlRemoved(e);
        }
    }
}
