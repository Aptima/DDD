using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_EventCommands : UserControl
    {
        public Ctl_EventCommands()
        {
            InitializeComponent();
        }

        private void AddEvent_Click(object sender, EventArgs e)
        {
            if (Parent is IEventCommands)
            {
                ((IEventCommands)Parent).AddEvent("Default");
            }
        }

        private void DeleteEvent_Click(object sender, EventArgs e)
        {
            if (Parent is IEventCommands)
            {
                ((IEventCommands)Parent).DeleteEvent();
            }
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (Parent is IEventCommands)
            {
                ((IEventCommands)Parent).MoveSelectedTrackUp();
            }
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            if (Parent is IEventCommands)
            {
                ((IEventCommands)Parent).MoveSelectedTrackDown();
            }
        }

        private void Properties_Click(object sender, EventArgs e)
        {
            if (Parent is IEventCommands)
            {
                ((IEventCommands)Parent).ShowProperties();
            }
        }


    }
}
