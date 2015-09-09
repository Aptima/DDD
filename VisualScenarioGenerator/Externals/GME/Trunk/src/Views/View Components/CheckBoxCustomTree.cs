using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AME.Views.View_Components;
using System.Windows.Forms;

namespace AME.Views.View_Components
{
    public partial class CheckBoxCustomTree : CustomTreeView
    {
        public CheckBoxCustomTree() : base()
        {
            InitializeComponent();
            this.CheckBoxes = true;
            this.UseNodeMap = true;
        }

        public CheckBoxCustomTree(IContainer container) : this()
        {
            container.Add(this);
        }

        public void ClearCheckBoxes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                ClearCheckBoxes(node.Nodes);
            }
        }

        //protected override GME.Nodes.ProcessingNode createNode(Type type, object[] args)
        //{
        //    GME.Nodes.ProcessingNode returnNode = (GME.Nodes.ProcessingNode)Activator.CreateInstance(type, args);
        //    if (returnNode.Level > 2)
        //    {
        //        returnNode.tr
        //    }
        //    return base.createNode(type, args);
        //}
    }
}
