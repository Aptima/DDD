using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.XPath;

namespace AssemblyGenerator
{
    public class XMLParser : Parser
    {
        private String xml;
        private XmlDocument myDoc;
        private List<String> processedClasses = new List<String>();

        public XMLParser(String newXML)
        {
            xml = newXML;
            GetXML(); // read/load
        }

        public XmlDocument GetXML()
        {
            if (myDoc == null)
            {
                myDoc = new XmlDocument();
                FileInfo xmlFile = new FileInfo(xml);
                myDoc.Load(xmlFile.OpenText());
            }
            return myDoc;
        }

        private void GetParameterValues(XmlNode aParameter, out String type, out String category, out String propertyName, out String parameterName)
        {
            type = ProcessType(aParameter);
            String name;
            XmlAttribute categoryAttr = aParameter.Attributes[ParameterConstants.category];
            if (categoryAttr != null)
            {
                category = aParameter.Attributes[ParameterConstants.category].Value;
                name = aParameter.Attributes[ParameterConstants.displayedName].Value;
                GetPropertyName(category, name, out propertyName, out parameterName);
            }
            else
            {
                name = aParameter.Attributes[ParameterConstants.attributeName].Value;
                name = name.Replace("-", "");
                name = name.Replace("/", "");
                parameterName = "m_" + name;
                propertyName = name[0].ToString().ToUpper() + name.Substring(1);
                category = null;
            }
        }

        private void GetPropertyName(String category, String displayedName, out String propertyName, out String parameterName)
        {
            category = category.Trim();
            category = category.Replace(" ", "");
            category = category.ToLower();
            displayedName = displayedName.Trim();
            displayedName = displayedName.Replace(" ", "");
            displayedName = displayedName.Replace("-", "");
            displayedName = displayedName.Replace("/", "");
            displayedName = displayedName.ToLower();

            String first = "" + category[0];
            first = first.ToUpper();
            String rest = category.Substring(1);

            propertyName = first + rest + displayedName;
            parameterName = "m_" + propertyName;
        }

        private String ProcessType(XmlNode aParameter)
        {
            XmlAttribute parameterAttribute = aParameter.Attributes[ParameterConstants.type];

            String type = parameterAttribute.Value;
            if (type.Contains(","))
            {
                // e.g.
                //"System.Drawing.Color, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
                type = type.Substring(0, type.IndexOf(","));
            }
            return type;
        }

        public String Parse()
        {
            StringBuilder allClasses = new StringBuilder();

            allClasses.Append("using System.Drawing; " + Environment.NewLine);
            allClasses.Append("using System.ComponentModel; " + Environment.NewLine);
            allClasses.Append("using System.Collections; " + Environment.NewLine);
            allClasses.Append("using System.Collections.Generic; " + Environment.NewLine);
            allClasses.Append("using System.Drawing.Design; " + Environment.NewLine);
            allClasses.Append("using System.Windows.Forms.Design; " + Environment.NewLine);
            allClasses.Append("using System; " + Environment.NewLine);
            allClasses.Append("using System.Text; " + Environment.NewLine);
            allClasses.Append("using AME.Controllers.Base.TypeConversion; " + Environment.NewLine);

            XmlDocument doc = GetXML();

            // global enums
            XmlNodeList enums = doc.SelectNodes("/GME/Global/Enums/Enum");
            StringBuilder codeBuilder = new StringBuilder();
            foreach (XmlNode anEnum in enums)
            {
                codeBuilder = ProcessEnum(anEnum, codeBuilder);
            }

            allClasses.Append(codeBuilder.ToString());

            allClasses.Append(@"
            internal class ArrayConverterToString : ArrayConverter
            {
                public override object ConvertTo(ITypeDescriptorContext context,
                                         System.Globalization.CultureInfo culture,
                                         object value, Type destType)
                {
                    if (destType == typeof(string) && value is Array)
                    {
                        Array arrayVal = (Array)value;

                        StringBuilder nameBuilder = new StringBuilder();
                        for (int i = 0; i < arrayVal.Length; i++)
                        {
                            if (i == 0)
                            {
                                nameBuilder.Append('[');
                            }
                            if (i != 0)
                            {
                                nameBuilder.Append(" + "\", \"" + @");
                            }
                            nameBuilder.Append(arrayVal.GetValue(i).ToString());
                            if (i == arrayVal.Length-1)
                            {
                                nameBuilder.Append(']');
                            }
                        }
                        return nameBuilder.ToString();
                    }
                    return base.ConvertTo(context, culture, value, destType);
                }
            }
            ");

            allClasses.Append(@"internal class Util
            {
                public static Boolean IsNull(Object obj)
                {
                    return obj == null;
                }
            }");

            XmlNodeList components = doc.SelectNodes("/GME/Global/Components/Component");

            foreach (XmlNode node in components)
            {
                String nodeName = node.Attributes[ParameterConstants.attributeName].Value;
                String code = ProcessLinkOrComponent(nodeName, "/GME/Global/Components/Component[@name='" + nodeName + "']/ComplexParameters", false);

                if (code.Length > 0)
                {
                    allClasses.Append(code);
                }
            }

            XmlNodeList links = doc.SelectNodes("/GME/Global/Links/Link");

            foreach (XmlNode link in links)
            {
                String linkType = link.Attributes[ParameterConstants.linkType].Value;

                String code = ProcessLinkOrComponent(linkType, "/GME/Global/Links/Link[@type='" + linkType + "']/Connect/ComplexParameters", false);

                if (code.Length > 0)
                {
                    allClasses.Append(code);
                }
            }

            return allClasses.ToString();
        }
        
        private String ProcessLinkOrComponent(String componentName, String rootPath, Boolean isStruct)
        {
            // process root
            XmlDocument doc = GetXML();
            XmlNodeList roots = doc.SelectNodes(rootPath);

            if (roots == null || roots.Count == 0) // major error
            {
                System.Console.WriteLine("Couldn't find ComplexParameters root node: " + rootPath);
                return "";
            }
            else
            {
                return ProcessLinkOrComponent(componentName, roots, isStruct);
            }
        }

        private String ProcessLinkOrComponent(String componentName, XmlNodeList roots, Boolean isStruct)
        {
            // Load XML, turn into C# source
            StringBuilder code = new StringBuilder();

            foreach (XmlNode root in roots)
            {
                XmlNodeList pCheck = root.SelectNodes("Parameters/Parameter");
                if (pCheck.Count == 0)
                {
                    continue; // skip
                }

                string className = componentName;

                XmlNode parentConnect = root.ParentNode;
                XmlAttribute from = parentConnect.Attributes[ParameterConstants.from];
                XmlAttribute to = parentConnect.Attributes[ParameterConstants.to];

                if (from != null && to != null)  // for links
                {
                    className = className + from.Value + to.Value;
                }

                if (!processedClasses.Contains(className))
                {
                    processedClasses.Add(className);

                    code = ProcessHeaderAndClass(root, code, className);

                    //Enums
                    XmlNodeList enums = root.SelectNodes("Enums/Enum");

                    foreach (XmlNode anEnum in enums)
                    {
                        code = ProcessEnum(anEnum, code);
                    }

                    // process parameters
                    XmlNodeList parameters = root.SelectNodes("Parameters/Parameter");

                    // variable declarations
                    foreach (XmlNode aParameter in parameters)
                    {
                        code = ProcessDeclarations(aParameter, code);
                    }

                    // constructor
                    code = ProcessConstructor(parameters, code, className, isStruct);

                    foreach (XmlNode aParameter in parameters)
                    {
                        String parameterType = aParameter.Attributes[ParameterConstants.type].Value;

                        String subComponentCode = ProcessLinkOrComponent(parameterType, aParameter.SelectNodes("."), true);
                        code.Append(subComponentCode);
                        code.Append(Environment.NewLine);

                        int count = 0;
                        code.Append("[");
                        foreach (XmlAttribute anAttribute in aParameter.Attributes)
                        {
                            code = ProcessAttribute(anAttribute, code, ref count); // process attributes before (e.g. category, description)
                        }

                        // add DisplayName for structs
                        if (isStruct)
                        {
                            String realStructName = aParameter.Attributes[ParameterConstants.attributeName].Value;

                            if (count > 0)
                            {
                                code.Append(", ");
                            }
                            code.Append("DisplayNameAttribute(@\"");
                            code.Append(realStructName);
                            code.Append("\")");
                            count++;
                        }

                        // array types use the a new converter that displays the children as a string
                        Type cSharpType = Type.GetType(parameterType);
                        if (cSharpType != null && cSharpType.BaseType != null && cSharpType.BaseType == typeof(Array))
                        {
                            code.Append(", TypeConverter(typeof(ArrayConverterToString))");
                        }

                        if (subComponentCode.Length > 0)
                        {
                            code.Append(", TypeConverter(typeof(ExpandableObjectConverter))");
                        }
                        code.Append("]" + Environment.NewLine);

                        code = ProcessParameter(aParameter, code); // the Property declaration
                    }

                    code.Append(" } " + Environment.NewLine); // source done, compile
                } // if check for already processed this class
            }
            return code.ToString();
        }

        private StringBuilder ProcessEnum(XmlNode anEnum, StringBuilder code)
        {
            String name = anEnum.Attributes[ParameterConstants.enumName].Value;

            code.Append("public enum ");
            code.Append(name);
            code.Append("{ ");

            XmlNodeList values = anEnum.SelectNodes("Value");

            int count = 0;
            foreach (XmlNode value in values)
            {
                if (count > 0)
                {
                    code.Append(", ");
                }

                String enumText = value.InnerText;

                if (enumText.Contains("-")) // bug fix - hyphens in enums.  
                // remove the hyphens, use stringvalue to declare the actual value
                // controller will check this
                {
                    code.Append("[StringValue(\"" + enumText.Clone() + "\")]");
                    enumText = enumText.Replace("-", "");
                    code.Append(enumText);
                }
                else if (enumText.Contains(" ")) // bug fix - spaces in enums.  
                {
                    code.Append("[StringValue(\"" + enumText.Clone() + "\")]");
                    enumText = enumText.Replace(" ", "");
                    code.Append(enumText);
                }
                else if (enumText.Contains("/")) // bug fix - spaces in enums.  
                {
                    code.Append("[StringValue(\"" + enumText.Clone() + "\")]");
                    enumText = enumText.Replace("/", "");
                    code.Append(enumText);
                }
                else
                {
                    code.Append(enumText);
                }

                count++;
            }

            code.Append(" } ");
            code.Append(Environment.NewLine);

            return code;
        }

        private StringBuilder ProcessDeclarations(XmlNode aParameter, StringBuilder code)
        {
            // private Point
            String type, propertyName, parameterName, category;
            GetParameterValues(aParameter, out type, out category, out propertyName, out parameterName);

            code.Append("private ");
            code.Append(type);
            code.Append(" ");

            // location;
            code.Append(parameterName);
            code.Append(";" + Environment.NewLine);

            return code;
        }

        private StringBuilder ProcessConstructor(XmlNodeList parameters, StringBuilder code, String className, Boolean isStruct)
        {
            //    public ParameterClass() {
            code.Append("public ");
            code.Append(className);
            code.Append("() { " + Environment.NewLine);

            code.Append("AttributeCollection attributes; " + Environment.NewLine);
            code.Append("DefaultValueAttribute myAttribute; " + Environment.NewLine);
            code.Append("TypeConverter converter; " + Environment.NewLine);
            code.Append("String value; " + Environment.NewLine);

            foreach (XmlNode aParameter in parameters)
            {
                code = ProcessParameterInConstructor(aParameter, code);
            }

            code.Append(" } " + Environment.NewLine);

            if (isStruct)
            {
                code.Append("public override String ToString() {");
                code.Append(Environment.NewLine);

                code.Append("StringBuilder structNameBuilder = new StringBuilder();");
                code.Append(Environment.NewLine);
                code.Append("structNameBuilder.Append(" + "\"[\"" + ");");
                code.Append(Environment.NewLine);

                foreach (XmlNode aParameter in parameters)
                {
                    String type, propertyName, parameterName, category;
                    GetParameterValues(aParameter, out type, out category, out propertyName, out parameterName);

                    code.Append("if (!Util.IsNull(" + parameterName + ")) {");
                    code.Append(Environment.NewLine);
                    code.Append("structNameBuilder.Append(" + parameterName + ".ToString());");
                    code.Append("structNameBuilder.Append(" + "\", \"" + ");");
                    code.Append(" } ");
                    code.Append(Environment.NewLine);
                }

                code.Append("if (structNameBuilder.Length >= 3)");
                code.Append(Environment.NewLine);
                code.Append("{");
                code.Append(Environment.NewLine);
                code.Append("structNameBuilder.Remove(structNameBuilder.Length - 2, 2);");
                code.Append(Environment.NewLine);
                code.Append("}");
                code.Append(Environment.NewLine);

                code.Append("structNameBuilder.Append(" + "\"]\"" + ");");
                code.Append(Environment.NewLine);
                code.Append("return structNameBuilder.ToString(); }");
                code.Append(Environment.NewLine);
            }

            return code;
        }

        private StringBuilder ProcessHeaderAndClass(XmlNode root, StringBuilder code, String className)
        {
            // using System.Drawing; using System.ComponentModel; using System; [DefaultPropertyAttribute("StartTime")]
            XmlAttribute rootAttribute = root.Attributes[ParameterConstants.defaultParameter];
            if (rootAttribute != null)
            {
                code.Append("[DefaultPropertyAttribute(\"");
                code.Append(rootAttribute.Value);
                code.Append("\")]" + Environment.NewLine);
            }

            // public class ParameterClass {

            code.Append("public class ");
            code.Append(className);
            code.Append(" { " + Environment.NewLine);

            return code;
        }

        private StringBuilder ProcessParameter(XmlNode aParameter, StringBuilder code)
        {
            // private Point
            String type, propertyName, parameterName, category;
            GetParameterValues(aParameter, out type, out category, out propertyName, out parameterName);

            // public Point Location {  get { return location; } set { location = value; } }
            code.Append("public ");
            code.Append(type);
            code.Append(" ");
            code.Append(propertyName);
            code.Append(" { " + Environment.NewLine);
            code.Append(" get { return ");
            code.Append(parameterName);
            code.Append("; } " + Environment.NewLine);

            // check constraints
            XmlNodeList constraints = aParameter.SelectNodes("Constraints/Constraint");
            if (constraints.Count > 0)
            {
                code = ProcessConstraints(constraints, code, parameterName);
            }
            else
            {
                code.Append("set { ");
                code.Append(parameterName);
                code.Append(" = value; }");
                code.Append(" } " + Environment.NewLine);
            }
            return code;
        }

        private StringBuilder ProcessConstraints(XmlNodeList constraints, StringBuilder code, String paramName)
        {
            code.Append("set { ");

            int count = 0;
            foreach (XmlNode aConstraint in constraints)
            {
                String constraintName = aConstraint.Attributes[ParameterConstants.constraintName].Value;
                String constraintValue = aConstraint.Attributes[ParameterConstants.constraintValue].Value;
                switch (constraintName)
                {
                    case ParameterConstants.maxConstraint:
                        {
                            if (count > 0)
                            {
                                code.Append(" else ");
                            }
                            code.Append("if (value > ");
                            code.Append(constraintValue);
                            code.Append(") { ");
                            code.Append(" throw new ArgumentException(\" Could not satisfy constraint: " + constraintName + ", value: " + constraintValue + " \"); }");
                            break;
                        }
                    case ParameterConstants.minConstraint:
                        {
                            if (count > 0)
                            {
                                code.Append(" else ");
                            }
                            code.Append("if (value < ");
                            code.Append(constraintValue);
                            code.Append(") { ");
                            code.Append(" throw new ArgumentException(\" Could not satisfy constraint: " + constraintName + ", value: " + constraintValue + " \"); }");
                            break;
                        }
                }
            }

            code.Append(" else { ");
            code.Append(paramName);
            code.Append(" = value; } } }" + Environment.NewLine);

            return code;
        }

        private StringBuilder ProcessParameterInConstructor(XmlNode aParameter, StringBuilder code)
        {
            /*
            attributes = TypeDescriptor.GetProperties(this)["Location"].Attributes;
            myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
            value = myAttribute.Value.ToString();
            converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Point));
            location = (Point)converter.ConvertFromString(value);
            */

            String type, propertyName, parameterName, category;
            GetParameterValues(aParameter, out type, out category, out propertyName, out parameterName);

            // skip this for array types (can't be initialized from string values this way)
            Type cSharpType = Type.GetType(type);
            if (cSharpType != null && cSharpType.BaseType != null && cSharpType.BaseType == typeof(Array))
            {
                return code;
            }

            code.Append("attributes = TypeDescriptor.GetProperties(this)[\"");
            code.Append(propertyName);
            code.Append("\"].Attributes; " + Environment.NewLine);
            code.Append("myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; " + Environment.NewLine);
            code.Append("if (myAttribute != null) {");
            code.Append("value = myAttribute.Value.ToString(); " + Environment.NewLine);
            code.Append("converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(");
            code.Append(type);
            code.Append(")); " + Environment.NewLine);
            code.Append("if (value.Length > 0) { " + Environment.NewLine);
            code.Append(parameterName);
            code.Append(" = ");
            code.Append("(");
            code.Append(type);
            code.Append(")");
            code.Append("converter.ConvertFromString(value); }" + Environment.NewLine);
            code.Append("}");
            code.Append(Environment.NewLine);

            XmlNodeList structParameters = aParameter.SelectNodes("Parameters/Parameter");
            if (structParameters.Count > 0)
            {
                code.Append(parameterName);
                code.Append(" = new ");
                code.Append(type);
                code.Append("(); " + Environment.NewLine);
            }

            code.Append(Environment.NewLine);

            return code;
        }

        private StringBuilder ProcessAttribute(XmlAttribute anAttribute, StringBuilder code, ref int count)
        {
            //    [CategoryAttribute("Cat"),
            //    DefaultValueAttribute("5, 13")]
            switch (anAttribute.Name)
            {
                case ParameterConstants.value:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        code.Append("DefaultValueAttribute(@\"");
                        code.Append(anAttribute.Value);
                        code.Append("\")");
                        count++;
                        break;
                    }

                case ParameterConstants.category:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("CategoryAttribute(\"");
                        code.Append(value);
                        code.Append("\")");
                        count++;
                        break;
                    }

                case ParameterConstants.description:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("DescriptionAttribute(\"");
                        code.Append(value);
                        code.Append("\")");
                        count++;
                        break;
                    }

                case ParameterConstants.displayedName:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("DisplayName(\"");
                        code.Append(value);
                        code.Append("\")");
                        count++;
                        break;
                    }

                case ParameterConstants.readOnly:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("ReadOnlyAttribute(");
                        code.Append(value);
                        code.Append(")");
                        count++;
                        break;
                    }
                case ParameterConstants.browsable:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("BrowsableAttribute(");
                        code.Append(value);
                        code.Append(")");
                        count++;
                        break;
                    }
                case ParameterConstants.editor:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("EditorAttribute(typeof(" + value + "), typeof(UITypeEditor))");
                        count++;
                        break;
                    }
                case ParameterConstants.typeconverter:
                    {
                        if (count > 0)
                        {
                            code.Append(", ");
                        }
                        String value = anAttribute.Value;
                        code.Append("TypeConverterAttribute(typeof(" + value + "))");
                        count++;
                        break;
                    }
            }
            return code;
        }
    }
}
