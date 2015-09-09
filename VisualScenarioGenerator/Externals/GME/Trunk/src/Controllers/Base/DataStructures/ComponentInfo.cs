using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class ComponentInfo
    {
        private string name, type, description, eType = eParamParentType.Component.ToString(), holderName = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        public string EType
        {
            get { return eType; }
            set { eType = value; }
        }

        public string HolderName
        {
            get { return holderName; }
            set { holderName = value; }
        }

        public ComponentInfo(string p_Name, string p_Type, string p_Description, string p_eType, string p_HolderName)
        {
            name = p_Name;
            type = p_Type;
            description = p_Description;
            holderName = p_HolderName;
            if (p_eType != null && p_eType.Length > 0)
            {
                eType = p_eType;
            }
        }
    }
}
