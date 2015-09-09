using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.Data_Structures
{
    // simple wrapper class to hold an int top ID and a string linktype
    public class TopIDAndLinkType
    {
        private int topID;
        private string linktype;

        public int TopID
        {
            get { return topID; }
            set { topID = value; }
        }

        public string LinkType
        {
            get { return linktype; }
            set { linktype = value; }
        }

        public TopIDAndLinkType(int p_topID, string p_linkType)
        {
            topID = p_topID;
            linktype = p_linkType;
        }
    }
}
