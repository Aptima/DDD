using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class LinkFromToType
    {
        private String from = "", to = "", type = "";

        public String From
        {
            get { return from; }
            set { from = value; }
        }

        public String To
        {
            get { return to; }
            set { to = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public LinkFromToType(String p_from, String p_to, String p_type)
        {
            from = p_from;
            to = p_to;
            type = p_type;
        }

        public override String ToString()
        {
            return from + to + type;
        }
    }
}
