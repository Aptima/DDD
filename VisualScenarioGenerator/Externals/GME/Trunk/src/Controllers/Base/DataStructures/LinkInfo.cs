using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class LinkInfo
    {
        private int rootID, fromID, toID;
        private string type, holderName, description = "";

        public int RootID
        {
            get { return rootID; }
            set { rootID = value; }
        }

        public int FromID
        {
            get { return fromID; }
            set { fromID = value; }
        }

        public int ToID
        {
            get { return toID; }
            set { toID = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string HolderName
        {
            get { return holderName; }
            set { holderName = value; }
        }

        public LinkInfo(int p_RootID, int p_FromID, int p_ToID, string p_Type, string p_Description, string p_holderName)
        {
            rootID = p_RootID;
            fromID = p_FromID;
            toID = p_ToID;
            type = p_Type;
            description = p_Description;
            holderName = p_holderName;
        }
    }
}
