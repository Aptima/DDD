using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public interface ICtl_Node
    {
        void SetNodeData(string id, int tick);
        string GetNodeId();
        int GetTick();
    }

    public partial class Ctl_Node : UserControl
    {
        public string ID
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }
        public int Tick
        {
            get
            {
                return (int) numericUpDown1.Value;
            }
            set
            {
                numericUpDown1.Value = (decimal)value;
            }
        }
        public Ctl_Node()
        {
            InitializeComponent();
        }
    }
}
