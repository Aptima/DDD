using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;

namespace AME.Nodes
{
    public class NoActionProcessingNode : ProcessingNode
    {
        public NoActionProcessingNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        public override void process(Object dragTarget) { } // do nothing
    }
}
