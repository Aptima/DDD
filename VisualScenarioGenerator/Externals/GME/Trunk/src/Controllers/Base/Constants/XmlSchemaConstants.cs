/*
 * Classes          : XmlSchemaConstants
 *                    XmlSchemaConstants.Config
 *                    XmlSchemaConstants.Display
 *                    XmlSchemaConstants.Display.Component
 *                    XmlSchemaConstants.Display.Parameter
 * File             : XmlSchemaConstants.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Config class    - contains the constant names of attributes in validation schema.
 * Display classes - To return components, their children, and parameters to view in XML format 
 * the XML data is validated against schemas. Those constant attribute names 
 * for component and paramter are in Display classes.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;

#endregion  //Namespaces

namespace AME.Controllers
{
    public class XmlSchemaConstants
    {
        public class Config
        {
            public const string Config_LinkType_Seperator = "_";
            public const string eType = "eType";
            public const string Id = "ID";
            public const string Name = "Name";
            public const string LinkType = "LinkType";
            public const string VisibleFunctions = "VisibleFunctions";
            public const string InvisibleFunctions = "InvisibleFunctions";
            public const char FunctionDelimiter = ';';
            public const char FunctionNameValueDelimiter = '=';
        }//Config class

        public class Display
        {
            public const string sComponent = "Component";
            public const string sLink = "Link";
            public const string sParameter = "Parameter";
            public const string sFunction = "Function";
            public const string sComponentParameters = "ComponentParameters";
            public const string sLinkParameters = "LinkParameters";

            public class Component
            {
                public const string Type = "Type";
                public const string BaseType = "BaseType";
                public const string Name = "Name";
                public const string Id = "ID";
                public const string Description = "Description";
                public const string LinkID = "LinkID";
                public const string eType = "eType";
                public const string ClassID = "ClassID";
                public const string SubclassID = "SubclassID";
            }//Component class
            public class Link
            {
                public const string Id = "ID";
                public const string FromId = "FromID";
                public const string ToId = "ToID";
                public const string Type = "Type";
                public const string Description = "Description";
                public const string FromName = "FromName";
                public const string ToName = "ToName";
                public const string FromType = "FromType";
                public const string ToType = "ToType";
            }//Link class
            public class Parameter
            {
                public const string Type = "Type";
                public const string Name = "Name";
                public const string Value = "Value";
            }//Parameter class
            public class Function
            {
                public const string Name = "Name";
                public const string Action = "Action";
                public const string Visible = "Visible";
            }//Function class
        }//Display class
    }//XmlSchemaConstants class
}//Controllers namespace