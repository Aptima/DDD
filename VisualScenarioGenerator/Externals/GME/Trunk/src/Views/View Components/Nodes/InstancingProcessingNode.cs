using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Controllers.Base.Data_Structures;
using Forms;
using System.Windows.Forms;
using System.Drawing;

namespace AME.Nodes
{
    public class InstancingProcessingNode : ProcessingNode
    {
        public InstancingProcessingNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        public override void process(Object dragTarget)
        {
            if (dragTarget is Diagram)
            {
                Diagram diagram = (Diagram)dragTarget;
                CustomTreeView cTree = (CustomTreeView)this.TreeView;

                int numberOfInstancesToAdd = 1;

                if (Control.ModifierKeys == Keys.Control) // control does multi instance
                {
                    NumberForm getInstanceAmount = new NumberForm("How many instances?", "Enter number of instances to add", "OK", "Cancel");
                    getInstanceAmount.StartPosition = FormStartPosition.Manual;
                    int x = Cursor.Position.X;
                    int y = Cursor.Position.Y;
                    getInstanceAmount.Location = new Point(x, y);

                    DialogResult check = getInstanceAmount.ShowDialog(cTree);

                    if (check == DialogResult.OK)
                    {
                        numberOfInstancesToAdd = (int)getInstanceAmount.NumberValue;
                    }
                    else
                    {
                        numberOfInstancesToAdd = 0;
                    }
                }

                if (numberOfInstancesToAdd >= 1)
                {
                    // node is a class, use it as source and parent, and instance onto diagram
                    List<ComponentAndLinkID> list = diagram.Controller.AddComponentInstances(diagram.RootID, diagram.RootID, NodeID, Name, diagram.DiagramName, "", numberOfInstancesToAdd);

                    int startX = diagram.DocPnt.X;
                    int startY = diagram.DocPnt.Y;

                    foreach (ComponentAndLinkID added in list)
                    {
                        int instanceID = added.ComponentID;

                        // also add to tree
                        //cTree.RootID
                        cTree.Controller.Connect(cTree.GetCustomTreeRootId(LinkType), NodeID, instanceID, LinkType);

                        diagram.RecordXYToXML(instanceID, startX.ToString(), startY.ToString(), false); // save location so it will read properly on refresh

                        startY += 40;
                    }
                }
            }
        }
    }
}
