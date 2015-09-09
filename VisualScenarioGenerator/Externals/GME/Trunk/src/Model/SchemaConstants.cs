using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Model
{
    public class SchemaConstants
    {
        //Common fields
        public const String Id = "id";
        public const String Type = "type";
        public const String Name = "name";
        public const String eType = "etype";
        public const String Description = "description";
        public const String Component = "component";
        public const String Link = "link";

        //LinkTable
        public const String To = "toComponentId";
        public const String From = "fromComponentId";
        public const String LinkID = "linkID";

        //ParameterTable
        public const String ParentId = "parentId";
        public const String ParentType = "parentType";
        public const String Value = "value";
        public const String BinaryValue = "binaryvalue";

        public const String ParameterDelimiter = ".";
        public const String FieldLeftDelimeter = "[";
        public const String FieldRightDelimeter = "]";

        public const int NullIndex = -1; 
    }
}
