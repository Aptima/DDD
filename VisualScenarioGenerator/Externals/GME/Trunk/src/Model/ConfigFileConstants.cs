using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Model
{
    public class ConfigFileConstants
    {
        // general
        public const String Name = "name";
        public const String Type = "type";
        public const String Value = "value";

        // for programmatic creation
        public const String uid = "uid";
        public const String component = "component";
        public const String configuration = "configuration";
        public const String schemaRoot = "schemaRoot";
        public const String eType = "eType";
        public const String root = "root";
        public const String ParentValue = "Parent";
        public const String ParentParentValue = "ParentParent";
        public const String SelfValue = "Self";
        public const String RootValue = "Root";
        public const String from = "from";
        public const String to = "to";

        // for complex parameters
        public const String defaultParameter = "defaultParameter";
        public const String complexType = "Complex";
        public const String enumName = "enumName";
        public const String constraintName = "constraintName";
        public const String constraintValue = "constraintValue";
        public const String maxConstraint = "max";
        public const String minConstraint = "min";
        public const String displayedName = "displayedName";
        public const String category = "category";
        public const String description = "description";
        public const String readOnly = "readOnly";
        public const String browsable = "browsable";
        public const String classOnly = "classOnly";
        public const String isOutput = "isOutput";

        // for dynamic linkTypes
        public const String dynamicLinkTypeDelimiter = "_";
        public const String dynamicType = "dynamicType";
        public const String background = "background";
        public const String isRoot = "isRoot";
        public const String appCode = "appCode";
        public const String refLinkType = "refLinkType";

        // for link types
        public const String deepCopy = "deepCopy";
        public const String schemaFilename = "schemaFilename";

        // Functions for Tree Nodes
        public const string Drag = "Drag";
        public const string ObjectType = "ObjectType";
        public const string AddClass = "AddClass";
        public const string DeleteComponent = "DeleteComponent";
        public const string DeleteLink = "DeleteLink";
        public const string RenameComponent = "RenameComponent";
        public const string CreateComponent = "CreateComponent";
        public const string AddListItem = "AddListItem";
        public const string ShowMultipleParameters = "ShowMultipleParameters";
    }
}
