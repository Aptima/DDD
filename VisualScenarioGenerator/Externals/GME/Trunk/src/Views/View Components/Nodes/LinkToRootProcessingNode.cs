using System;
using System.Collections.Generic;
using System.Text;
using AME.Views.View_Components;
using AME.Controllers;

namespace AME.Nodes
{
    public class LinkToRootProcessingNode : ProcessingNode
    {
        public LinkToRootProcessingNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        public override void process(Object dragTarget)
        {
            if (dragTarget is Diagram)
            {
                Diagram diagram = (Diagram)dragTarget;

                diagram.Controller.Connect(diagram.RootID, diagram.RootID, NodeID, diagram.DiagramName);

                diagram.RecordXYToXML(NodeID, diagram.DocPnt.X.ToString(), diagram.DocPnt.Y.ToString(), false); // save location so it will read properly on refresh
            }
        }
    }
}
