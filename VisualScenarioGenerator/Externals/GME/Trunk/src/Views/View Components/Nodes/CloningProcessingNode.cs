using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Controllers.Base.Data_Structures;

namespace AME.Nodes
{
    public class CloningProcessingNode : ProcessingNode
    {
        public CloningProcessingNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        public override void process(Object dragTarget)
        {
            if (dragTarget is Diagram)
            {
                Diagram diagram = (Diagram)dragTarget;

                if (diagram.Controller.IsLinkTypeDynamic(diagram.DiagramName))
                {
                    int dynamicPivot = diagram.Controller.GetDynamicPivotFromLinkType(diagram.DiagramName);

                    if (dynamicPivot != -1)
                    {
                        ComponentAndLinkID added = diagram.Controller.AddSubClass(diagram.RootID, dynamicPivot, NodeID, Name, "", diagram.Controller.ConfigurationLinkType);

                        int cloneID = added.ComponentID;

                        diagram.Controller.Connect(diagram.RootID, diagram.RootID, cloneID, diagram.DiagramName);

                        diagram.RecordXYToXML(cloneID, diagram.DocPnt.X.ToString(), diagram.DocPnt.Y.ToString(), false); // save location so it will read properly on refresh
                    }
                }
            }
        }
    }
}
