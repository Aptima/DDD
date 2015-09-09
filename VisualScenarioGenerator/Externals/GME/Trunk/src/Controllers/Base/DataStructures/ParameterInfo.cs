using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class ParameterInfo
    {
        private string category, name, field, combined, paramValue, paramMin, paramMax, description = "";
        private int parentID;
        private eParamParentType parentType;
        private byte[] binaryData;

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String Value
        {
            get { return paramValue; }
            set { paramValue = value; }
        }

        public String Combined
        {
            get { return combined; }
            set { combined = value; }
        }

        public String Category
        {
            get { return category; }
            set { category = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Field
        {
            get { return field; }
            set { field = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public eParamParentType ParentType
        {
            get { return parentType; }
            set { parentType = value; }
        }

        public byte[] BinaryData
        {
            get { return binaryData; }
            set { binaryData = value; }
        }

        public String ParamMin
        {
            get { return paramMin; }
            set { paramMin = value; }
        }

        public String ParamMax
        {
            get { return paramMax; }
            set { paramMax = value; }
        }

        public ParameterInfo(int p_ParentID, String p_Combined, eParamParentType p_ParentType, String p_Value, String p_Description)
            : this("", "", "", p_Combined, p_ParentType, p_Value, p_Description, null, null)
        {
            parentID = p_ParentID;
        }

        public ParameterInfo(String p_Category, String p_Name, String p_Field, String p_Combined, eParamParentType p_ParentType, String p_Value, String p_Description, String p_paramMin, String p_paramMax)
        {
            category = p_Category;
            name = p_Name;
            field = p_Field;
            combined = p_Combined;
            parentType = p_ParentType;
            paramValue = p_Value;
            description = p_Description;
            binaryData = null;
            paramMin = p_paramMin;
            paramMax = p_paramMax;
        }

        public ParameterInfo(String p_Category, String p_Name, String p_Field, String p_Combined, eParamParentType p_ParentType, String p_Value, String p_Description, String p_paramMin, String p_paramMax, byte[] p_binaryData)
            : this(p_Category, p_Name, p_Field, p_Combined, p_ParentType, p_Value, p_Description, p_paramMin, p_paramMax)
        {
            this.binaryData = p_binaryData;
        }
    }
}
