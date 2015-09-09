using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.Data_Structures
{
    // simple wrapper class to hold two integers - a component and link ID
    public class ComponentAndLinkID
    {
        private int componentID, linkID;

        public int ComponentID
        {
            get { return componentID; }
            set { componentID = value; }
        }

        public int LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        public ComponentAndLinkID(int p_componentID, int p_linkID)
        {
            componentID = p_componentID;
            linkID = p_linkID;
        }
    }
}
