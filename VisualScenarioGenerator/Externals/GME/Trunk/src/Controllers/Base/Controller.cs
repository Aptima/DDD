/*
 * Class            : Controller
 * File             : Controller.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      : 
 * Base class for other controllers. 
 * It inherits from IController interface. 
 * This file has most of the logic of this class. It contains constructors,
 * private and protected variables, properties, and methods.
 * It is the brain of Controller.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using Collections;
using System.Xml;
using AME.Views.View_Components;
using AME;
using AME.Model;
using System.IO;
using AME.Controllers.Base.Data_Structures;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using AME.Controllers.Base.Caching;
using AME.Controllers.Base;
using System.Web.UI.WebControls;
using AME.Controllers.Base.DataStructures;
using System.Globalization;
using AME.Controllers.Base.TypeConversion;

#endregion  //Namespaces

namespace AME.Controllers
{
    public partial class Controller : IController
    {
        protected AME.Model.Model m_model;
        private string m_sConfigType;
        private string m_sRootComponentType;
        private string m_sConfigurationLinkType;
        private Boolean m_allowProgrammaticCreation = true;
        private Boolean m_IgnoreEmptyString = false;
        private String m_addComponentLinkType;

        private static Dictionary<AME.Model.Model, List<IViewComponentHelper>> registeredForUpdates = new Dictionary<AME.Model.Model,List<IViewComponentHelper>>();

        private static Dictionary<AME.Model.Model, Dictionary<string, XmlDocumentFragment>> componentParametersXMLCache = new Dictionary<AME.Model.Model,Dictionary<string,XmlDocumentFragment>>();
        private static Dictionary<AME.Model.Model, Dictionary<string, XmlDocumentFragment>> linkParametersXMLCache = new Dictionary<AME.Model.Model, Dictionary<string, XmlDocumentFragment>>();
        private static Dictionary<AME.Model.Model, Dictionary<string, DataRow>> instanceToClassCache;
        private static Dictionary<AME.Model.Model, Dictionary<string, DataRow>> subclassToClassCache;

        protected static bool useLoadingCaches = false;
        protected static Dictionary<AME.Model.Model, DataTable> loadingComponentTable = new Dictionary<AME.Model.Model, DataTable>();
        protected static Dictionary<AME.Model.Model, Dictionary<String, DataRow>> loadingComponentCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
        protected static Dictionary<AME.Model.Model, Dictionary<String, DataRow>> loadingLinkCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
        protected static Dictionary<AME.Model.Model, Dictionary<String, Dictionary<String, List<DataRow>>>> loadingLTCache = new Dictionary<AME.Model.Model, Dictionary<string, Dictionary<string, List<DataRow>>>>();
        protected static Dictionary<AME.Model.Model, Dictionary<String, List<DataRow>>> loadingParameterCache = new Dictionary<AME.Model.Model, Dictionary<string, List<DataRow>>>();
        protected static Dictionary<AME.Model.Model, Dictionary<String, DataRow>> loadingInstanceToClassCache;
        protected static Dictionary<AME.Model.Model, Dictionary<String, DataRow>> loadingSubClassToClassCache;

        private Dictionary<String, List<Include>> includesMap = new Dictionary<String, List<Include>>();
        private Boolean processingIncludes = false;
        private Boolean progressBarForDeletes;
        private List<ComponentFunction> lastSchemaValuesValidateAdd;
        private String lastValidateAddComponentXPath;
        private String lastValidateAddParentXPath;
        private String lastValidateAddComponentType;
        private String lastValidateAddBaseType;
        private static XMLCache cache;
        private SchemaXmlParser lastValidateSchemaParser = new SchemaXmlParser();

        protected static BulkHelper myBulkHelper;

        #region Constructors

        public Controller(AME.Model.Model model, string configType)
        {
            this.m_model = model;
            this.m_sConfigType = configType;

            if (!this.m_model.GetConfigurationNames().Contains(configType))
            {
                return;
            }

            if (cache == null)
            {
                cache = new XMLCache(this);
            }

            cache.AddModel(model);

            this.m_sRootComponentType = this.m_model.GetRootComponent(this.m_sConfigType);
            this.m_sConfigurationLinkType = this.m_model.GetComponentLinkType(this.m_sConfigType);

            if (!registeredForUpdates.ContainsKey(model))
            {
                registeredForUpdates.Add(model, new List<IViewComponentHelper>());
                componentParametersXMLCache.Add(model, new Dictionary<string, XmlDocumentFragment>());
                linkParametersXMLCache.Add(model, new Dictionary<string, XmlDocumentFragment>());
            }
        }//constructor

        #endregion

        # region Private Methods

        #region General

        private List<string> _Intersect(List<string> coll1, List<string> coll2)
        {
            List<string> lIntersect = new List<string>();

            List<string> smallColl;
            List<string> bigColl;

            if (coll1.Count < coll2.Count)
            {
                smallColl = coll1;
                bigColl = coll2;
            }
            else
            {
                smallColl = coll2;
                bigColl = coll1;
            }

            foreach (string item in smallColl)
            {
                if (bigColl.Contains(item))
                {
                    lIntersect.Add(item);
                }
            }//foreach item in small collection

            return lIntersect;
        }//_Intersect

        private Component.eComponentType _StringToEnum(string sValue)
        {
            Component.eComponentType eType = Component.eComponentType.None;

            foreach (int iOption in Enum.GetValues(typeof(Component.eComponentType)))
            {
                string sOption = Enum.GetName(typeof(Component.eComponentType), iOption);
                if (sValue == sOption)
                {
                    eType = (Component.eComponentType)iOption;
                    break;
                }//if component type matches
            }//foreach option of Component Type

            return eType;
        }//_StringToEnum

        // not called anymore - MW
        //private object _ValidateTypeConversion(string data, Type type, bool allowEmptyString)
        //{
        //    object obj = null;
        //    bool bPerformValidation = true;

        //    if (allowEmptyString && (data == string.Empty))
        //    {
        //        bPerformValidation = (type == typeof(string));
        //    }//if allowEmptyString & data is empty string

        //    if (bPerformValidation)
        //    {
        //        obj = this._ValidateTypeConversion(data, type);
        //    }

        //    return obj;
        //}//_ValidateTypeConversion

        public void ValidateParameterValues(bool replaceFaultyValuesWithDefaults, bool addNew)
        {
            this.TurnViewUpdateOff();
            this.EnableLoadingCache();

            Dictionary<String, String> seenComponentTypes = new Dictionary<String, String>();
            Dictionary<String, LinkFromToType> seenLinkTypes = new Dictionary<String, LinkFromToType>();

            DataTable components = this.GetComponentTable();
            Dictionary<String, String> componentIDMap = new Dictionary<String, String>();
            Dictionary<String, Type> typeMap = new Dictionary<String, Type>();
            foreach (DataRow crow in components.Rows)
            {
                String id = crow[SchemaConstants.Id].ToString();
                String type = crow[SchemaConstants.Type].ToString();
                componentIDMap.Add(id, type);

                if (!seenComponentTypes.ContainsKey(type))
                {
                    seenComponentTypes.Add(type, type);
                }
            }

            Dictionary<String, LinkFromToType> linkIDMap = new Dictionary<String, LinkFromToType>();
            DataTable links = this.GetLinkTable(); ;
            foreach (DataRow lrow in links.Rows)
            {
                String id = lrow[SchemaConstants.Id].ToString();
                String from = lrow[SchemaConstants.From].ToString();
                String to = lrow[SchemaConstants.To].ToString();
                String type = lrow[SchemaConstants.Type].ToString();

                type = GetBaseLinkType(type);

                if (componentIDMap.ContainsKey(from))
                {
                    from = componentIDMap[from];
                }
                else
                {
                    // Delete
                    Console.WriteLine("Invalid component in link type: " + type + ", deleting");
                    useLoadingCaches = false;
                    this.DeleteLink(Int32.Parse(id));
                    useLoadingCaches = true;
                    continue;
                }

                if (componentIDMap.ContainsKey(to))
                {
                    to = componentIDMap[to];
                }
                else
                {
                    // Delete
                    Console.WriteLine("Invalid component in link type: " + type + ", deleting");
                    useLoadingCaches = false;
                    this.DeleteLink(Int32.Parse(id));
                    useLoadingCaches = true;
                    continue;
                }

                linkIDMap.Add(id, new LinkFromToType(from, to, type));

                String linkCombined = from + to + type;

                if (!seenLinkTypes.ContainsKey(linkCombined))
                {
                    seenLinkTypes.Add(linkCombined, new LinkFromToType(from, to, type));
                }
            }

            DataTable parameters = this.GetParameterTable();
            Type cSharpType = null;
            String paramMin = null;
            String paramMax = null;
            String linkType = "";
            String fromType = "";
            String toType = "";
            String componentType = "";
            String paramID;
            String paramName;
            String paramValue;
            String paramEParent;
            int paramParseId;
            eParamParentType paramParseParent;
            ParamConstraint paramConstraint;
            String parameterType = "";
            String parameterTypeConverter = "";
            String name = "", category = "", childField = "";

            Dictionary<String, String> seenNames = new Dictionary<String, String>();

            foreach (DataRow row in parameters.Rows)
            {
                paramMin = null;
                paramMax = null;

                paramID = row[SchemaConstants.ParentId].ToString();
                paramName = row[SchemaConstants.Name].ToString();
                paramValue = row[SchemaConstants.Value].ToString();
                paramEParent = row[SchemaConstants.ParentType].ToString();
                paramParseId = Int32.Parse(paramID);
                paramParseParent = (eParamParentType)Enum.Parse(typeof(eParamParentType), paramEParent);

                if (!seenNames.ContainsKey(paramName))
                {
                    seenNames.Add(paramName, paramName);
                }

                try
                {
                    // category.name -> category, name
                    if (GetCategoryNameAndField(paramName, out category, out name, out childField))
                    {
                        // intialize anything we need...
                        if (paramParseParent == eParamParentType.Link)
                        {
                            if (linkIDMap.ContainsKey(paramID))
                            {
                                fromType = linkIDMap[paramID].From;
                                toType = linkIDMap[paramID].To;
                                linkType = linkIDMap[paramID].Type;
                              
                                componentType = linkType + fromType + toType;
                            }
                            parameterType = this.m_model.GetParameterType(linkType, fromType, toType, category, name, childField);
                            parameterTypeConverter = this.m_model.GetParameterTypeConverter(linkType, fromType, toType, category, name, childField);
                  
                        }
                        else if (paramParseParent == eParamParentType.Component)
                        {
                            componentType = "";
                            if (componentIDMap.ContainsKey(paramID))
                            {
                                componentType = componentIDMap[paramID];
                            }
                            parameterType = this.m_model.GetParameterType(componentType, category, name, childField);
                            parameterTypeConverter = this.m_model.GetParameterTypeConverter(componentType, category, name, childField);
                        }

                        if (typeMap.ContainsKey(parameterType))
                        {
                            cSharpType = typeMap[parameterType];
                        }
                        else
                        {
                            cSharpType = AMEManager.GetType(parameterType, componentType);
                            typeMap.Add(parameterType, cSharpType);
                        }

                        // Look up constraint 
                        paramConstraint = GetConstraint(category, name, childField, paramParseParent, componentType, linkType, fromType, toType);

                        if (paramConstraint != null)
                        {
                            paramMin = paramConstraint.Range.Min;
                            paramMax = paramConstraint.Range.Max;
                        }

                        //For valid type conversion will go fine, else will throw exception.
                        object oValidatedValue = this._ValidateValue(cSharpType, paramValue, paramMin, paramMax, parameterTypeConverter);
                        string sParamValueToStore = (oValidatedValue == null) ? paramValue : oValidatedValue.ToString();
                    }
                    else
                    {
                        throw new Exception("Could not find parameter category and name from: " + paramName);
                    }
                }
                catch (ArgumentException)
                {
                    // Delete
                    Console.WriteLine("Could not find parameter: " + paramName + ", deleting");
                    useLoadingCaches = false;
                    this.DeleteParameter(paramParseId, paramEParent, paramName, false);
                    useLoadingCaches = true;
                }
                catch (OverflowException oe)
                {
                    if (replaceFaultyValuesWithDefaults)
                    {
                        ReplaceWithDefault(paramName, componentType, linkType, fromType, toType, category, name,
                            childField, paramParseParent, paramParseId, paramEParent);
                    }
                    else
                    {
                        Console.WriteLine("Overflow on " + paramName);
                        throw oe;
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("range")) // is this from the controller constraints
                    {
                        if (replaceFaultyValuesWithDefaults)
                        {
                            ReplaceWithDefault(paramName, componentType, linkType, fromType, toType, category, name,
                                childField, paramParseParent, paramParseId, paramEParent);
                        }
                        else
                        {
                            Console.WriteLine("Unhandled Exception on " + e.Message);
                            throw e;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unhandled Exception on " + e.Message);
                        throw e;
                    }
                }
            }

            if (addNew)
            {
                XmlNode configParameters;
                foreach (String ctype in seenComponentTypes.Keys)
                {
                    configParameters = this.m_model.GetParametersXML(ctype);
                    if (configParameters != null)
                    {
                        ValidateParameterValues(configParameters, seenNames, ctype, null, null, null, componentIDMap, null);
                    }
                }
                String addFrom, addTo, addType;
                foreach (String lcombined in seenLinkTypes.Keys)
                {
                    addFrom = seenLinkTypes[lcombined].From;
                    addTo = seenLinkTypes[lcombined].To;
                    addType = seenLinkTypes[lcombined].Type;
                    configParameters = this.m_model.GetParametersXML(addType, addFrom, addTo);
                    if (configParameters != null)
                    {
                        ValidateParameterValues(configParameters, seenNames, null, addFrom, addTo, addType, null, linkIDMap);
                    }
                }
            }

            this.DisableLoadingCache();
            this.TurnViewUpdateOn(false, false);
            this.ClearCache();
        }

        private void ValidateParameterValues(XmlNode configParameters, Dictionary<String, String> seenNames,
                                             String ctype, String from, String to, String type, 
                                             Dictionary<String, String> componentIDMap, 
                                             Dictionary<String, LinkFromToType> linkIDMap)
        {
            XmlNodeList childParameters = configParameters.SelectNodes("Parameter");
            foreach (XmlNode childParameter in childParameters)
            {
                String configCategory = childParameter.Attributes["category"].Value;
                String configDisplayedName = childParameter.Attributes["displayedName"].Value;
                String configCombined = configCategory + SchemaConstants.ParameterDelimiter + configDisplayedName;
                if (!seenNames.ContainsKey(configCombined))
                {
                    String value = childParameter.Attributes["value"].Value;

                    if (componentIDMap != null)
                    {
                        foreach (String id in componentIDMap.Keys)
                        {
                            String mapType = componentIDMap[id];
                            if (mapType == ctype)
                            {
                                Console.WriteLine(configCombined + " created with " + value);
                                this.m_model.CreateParameter(Int32.Parse(id), "Component", configCombined, value, "");
                            }
                        }
                    }
                    if (linkIDMap != null)
                    {
                        foreach (String id in linkIDMap.Keys)
                        {
                            LinkFromToType lftt = linkIDMap[id];
                            if (from == lftt.From && to == lftt.To && type == lftt.Type)
                            {
                                Console.WriteLine(configCombined + " created with " + value);
                                this.m_model.CreateParameter(Int32.Parse(id), "Link", configCombined, value, "");
                            }
                        }
                    }
                }
                ValidateParameterStructValues(configCombined, childParameter, seenNames, ctype, from, to, type, componentIDMap, linkIDMap);
            }
        }

        private void ValidateParameterStructValues(String childCombinedName, XmlNode childParameter, 
                                                   Dictionary<String, String> seenNames,
                                                   String ctype, String from, String to, String type,
                                                   Dictionary<String, String> componentIDMap,
                                                   Dictionary<String, LinkFromToType> linkIDMap)
        {

            XmlNodeList structParameters = childParameter.SelectNodes("Parameters/Parameter");
            foreach (XmlNode structParameter in structParameters)
            {
                String structName = structParameter.Attributes["name"].Value;
                String fullStructName = childCombinedName + SchemaConstants.FieldLeftDelimeter + structName + SchemaConstants.FieldRightDelimeter;
                if (!seenNames.ContainsKey(fullStructName))
                {
                    String structValue = structParameter.Attributes["value"].Value;

                    if (componentIDMap != null)
                    {
                        foreach (String id in componentIDMap.Keys)
                        {
                            String mapType = componentIDMap[id];
                            if (mapType == ctype)
                            {
                                // create new parameter
                                Console.WriteLine(fullStructName + " created with " + structValue);
                                this.m_model.CreateParameter(Int32.Parse(id), "Component", fullStructName, structValue, "");
                            }
                        }
                    }
                    if (linkIDMap != null)
                    {
                        foreach (String id in linkIDMap.Keys)
                        {
                            LinkFromToType lftt = linkIDMap[id];
                            if (from == lftt.From && to == lftt.To && type == lftt.Type)
                            {
                                // create new parameter
                                Console.WriteLine(fullStructName + " created with " + structValue);
                                this.m_model.CreateParameter(Int32.Parse(id), "Link", fullStructName, structValue, "");
                            }
                        }
                    }
                }
            }
        }
              
        private void ReplaceWithDefault(String paramName, String componentType,
            String linkType, String fromType, String toType, String category, String name, 
            String childField, eParamParentType paramParseParent, int paramParseID, String paramEParent)
        {
            //Object defaultInstance = Activator.CreateInstance(cSharpType);
            //String val = defaultInstance.ToString();
            // Replace with default value from config file instead

            XPathNavigator parameter = null;

            if (paramParseParent == eParamParentType.Link)
            {
                parameter = this.m_model.GetParameter(linkType, fromType, toType, category, name, childField);
            }
            else if (paramParseParent == eParamParentType.Component)
            {
                parameter = this.m_model.GetParameter(componentType, category, name, childField);
            }

            String val = parameter.GetAttribute("value", parameter.NamespaceURI);

            Console.WriteLine( paramName + ": replacing with new default value: " + val);
            this.m_model.CreateParameter(paramParseID, paramEParent, paramName, val, "");
        }

        private object _ValidateTypeConversion(string data, Type type)
        {
            return _ValidateTypeConversion(data, type, "");
        }
        /*
         * Currently supports following types
         * 
         * string
         * float
         * double
         * uint
         * int
         * long
         * DateTime
         * Color
         * */
        private object _ValidateTypeConversion(string data, Type type, string typeconverter)
        {
            // IgnoreEmptyString == true allows empty string for any parameter type
            // this 'deletes' the parameter...
            if (IgnoreEmptyString && data.Equals(String.Empty))
            {
                return data;
            }

            object obj = null;

            if (
                (type == typeof(string))
                || (type == typeof(float))
                || (type == typeof(double))
                || (type == typeof(uint))
                || (type == typeof(int))
                || (type == typeof(long))
                )
            {
                if (typeconverter != null && typeconverter.Length > 0)
                {
                    Object converter;
                    try
                    {
                        converter = AMEManager.CreateObject(typeconverter, new object[] { });
                    }
                    catch (Exception)
                    {
                        converter = AMEManager.CreateObject(typeconverter, "AME.Controllers.Base.TypeConversion.", new object[] { });
                    }
                    obj = ((TypeConverter)converter).ConvertFrom(data);
                }
                else
                {
                    obj = Convert.ChangeType(data, type);
                }
            }
            else if (type == typeof(bool))
            {
                obj = Convert.ChangeType(data, type); // booleans should be lower case
                obj = obj.ToString().ToLower();
            }
            else if (type == typeof(DateTime))
            {
                if (data == "")
                {
                    // use today's date
                    DateTime returnDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                    obj = returnDate;
                }
                else
                {
                    if (typeconverter != null && typeconverter.Length > 0)
                    {
                        Object converter;
                        try
                        {
                            converter = AMEManager.CreateObject(typeconverter, new object[] { });
                        }
                        catch (Exception)
                        {
                            converter = AMEManager.CreateObject(typeconverter, "AME.Controllers.Base.TypeConversion.", new object[] { });
                        }
                        obj = ((TypeConverter)converter).ConvertFrom(data);
                    }
                    else
                    {
                        // standard datetime
                        obj = Convert.ChangeType(data, type);
                    }
                }
            }
            else if (type == typeof(Color))
            {
                ColorConverter conv = new ColorConverter();
                obj = conv.ConvertFromString(data);

                Color cast = (Color)obj; // turn into r, g, b, otherwise can store a named color, which is less usable

                obj = "" + cast.R + ", " + cast.G + ", " + cast.B;
                //Color color = (Color)obj;

                // .NET ColorEditor can send down RGB values, so disabling this check - MW
                //if (!color.IsKnownColor || !color.IsNamedColor)
                //{
                //    string sError = string.Format("Invalid value {0} for a Color", data);
                //    throw new System.Exception(sError);
                //}
                //obj = Color.FromName(data);
                //color.IsKnownColor
            }//Color
            else if (type.IsEnum) // new special case - enums
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                try
                {
                    obj = converter.ConvertFromString(data);
                }
                catch (FormatException ex)
                {
                    // Does the value belong to a StringValue attribute of the enumeration?
                    StringEnum stringEnum = new StringEnum(type);
                    Boolean pass = stringEnum.IsStringDefined(data);
                    if (!pass)
                    {
                        throw ex;
                    }
                    obj = data;
                }
            }
            else //all unknown types
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);

                if (converter.CanConvertFrom(data.GetType()))
                {
                    obj = converter.ConvertFromString(data);
                }
                else
                {
                    if (data == String.Empty)
                    {
                        Console.WriteLine("Couldn't convert " + type + ", data " + data + ", converter: " + converter.ToString());
                    }
                    else if (converter is ArrayConverter) // array conversion (experimental)
                    {
                        StringArrayConverter strArray = new StringArrayConverter();
                        Object array = strArray.ConvertFrom(data);
                        // now it's a string array, convert it to the actual typed array
                        Type elementType = type.GetElementType();
                        if (elementType.IsAssignableFrom(typeof(Double))) // this is hard to generalize, so write cases for types
                        {
                            return Array.ConvertAll<String, Double>((String[])array, new Converter<String, Double>(StringToDouble));
                        }
                        else if (elementType.IsAssignableFrom(typeof(Int32)))
                        {
                             return Array.ConvertAll<String, Int32>((String[])array, new Converter<String, Int32>(StringToInt32));
                        }
                        else if (elementType.IsAssignableFrom(typeof(String)))
                        {
                            return array;
                        }
                        else
                        {
                            Console.WriteLine("Couldn't convert " + type + ", data " + data + ", converter: " + converter.ToString());
                        }
                    }
                }
            }

            return obj;
        }//_ValidateTypeConversion

        private Double StringToDouble(String input)
        {
            return Double.Parse(input);
        }

        private Int32 StringToInt32(String input)
        {
            return Int32.Parse(input);
        }

        private object _ValidateValue(Type type, string value, string min, string max)
        {
            return _ValidateValue(type, value, min, max, "");
        }

        private object _ValidateValue(Type type, string value, string min, string max, string typeconverter)
        {
            object oResult = null;
            bool bValid = true;
            object oValue = this._ValidateTypeConversion(value, type, typeconverter);

            // MW, add bools
            //For dates need to store their validated value, instead of user input.
            if (type == typeof(bool) || type == typeof(DateTime) || type == typeof(Color) || (type.BaseType != null && type.BaseType == typeof(Array))
                || !String.IsNullOrEmpty(typeconverter))
            {
                oResult = oValue;
            }

            //if value did not have white space and min or max are defined.
            if ((oValue != null) && ((min != null) || (max != null)))
            {
                if (oValue is IComparable)
                {
                    IComparable icompValue = (IComparable)oValue;
                    object oMin = null;
                    object oMax = null;

                    if (min != null)
                    {
                        oMin = _ValidateTypeConversion(min, type);
                    }

                    if (max != null)
                    {
                        oMax = _ValidateTypeConversion(max, type);
                    }

                    if (oMin != null && oMin is IComparable)
                    {
                        IComparable icompMin = (IComparable)oMin;
                        bValid &= this._IsGreaterOrEqual(icompValue, icompMin);
                    }

                    if (oMax != null && oMax is IComparable)
                    {
                        IComparable icompMax = (IComparable)oMax;
                        bValid &= this._IsLessOrEqual(icompValue, icompMax);
                    }

                    if (!bValid)
                    {
                        StringBuilder sbError = new StringBuilder();
                        sbError.Append("The value ");
                        sbError.Append(value);
                        sbError.Append(" is not in range with");

                        if (oMin != null)
                        {
                            sbError.Append(" Min = ");
                            sbError.Append(min);
                        }

                        if (oMax != null)
                        {
                            sbError.Append(" Max = ");
                            sbError.Append(max);
                        }

                        sbError.Append(".");

                        throw new System.Exception(sbError.ToString());
                    }
                }
            }//if value validation actually happened and passed

            return oResult;
        }//_ValidateValue

        private bool _IsInRange<T>(T value, T min, T max)
            where T : IComparable
        //, IComparable<T>
        {
            bool bResult = false;

            bResult =
                this._IsGreaterOrEqual(value, min)
                &&
                this._IsLessOrEqual<T>(value, max);

            return bResult;
        }//_IsInRange<T>

        private bool _IsGreaterOrEqual<T>(T value, T min)
            where T : IComparable
        //, IComparable<T>
        {
            bool bResult = false;

            bResult = (value.CompareTo(min) >= 0);

            return bResult;
        }//_IsGreaterOrEqual

        private bool _IsLessOrEqual<T>(T value, T max)
            where T : IComparable
        //, IComparable<T>
        {
            bool bResult = false;

            bResult = (value.CompareTo(max) <= 0);

            return bResult;
        }//_IsLessOrEqual


        protected int? _GetID(DataRow component)
        {
            int? iComponentID = null;

            if (component != null)
            {
                iComponentID = (int)component[SchemaConstants.Id];
            }

            return iComponentID;
        }//_GetID

        protected string _GetType(int compID)
        {
            DataRow drComp = this._GetComponent(compID);

            return this._GetType(drComp);
        }//_GetType

        protected string _GetType(DataRow component)
        {
            string sCompType = null;

            if (component != null)
            {
                sCompType = (string)component[SchemaConstants.Type];
            }

            return sCompType;
        }//_GetType

        protected List<int> _GetIDs(DataTable components)
        {
            List<int> componentIDs = new List<int>();

            foreach (DataRow component in components.Rows)
            {
                int componentID = this._GetID(component).Value;
                componentIDs.Add(componentID);
            }

            return componentIDs;
        }//_GetIDs


        private string _GetUltimateBaseComponentType(string compType)
        {
            string sResult = null;

            string sBaseCompType = this._GetBaseComponentType(compType);
            sResult = (sBaseCompType == null) ? compType : sBaseCompType;

            return sResult;
        }//_GetUltimateBaseComponentType

        private bool _IsInheritedComponent(string compType)
        {
            bool bResult = false;

            bResult = (this._GetBaseComponentType(compType) != null);

            return bResult;
        }//_IsInheritedComponent

        protected string _GetBaseComponentType(string compType)
        {
            if (
                (compType != null)
                &&
                (compType != string.Empty)
                )
            {
                return this.m_model.GetBaseComponentType(compType);
            }
            return null;
        }//_GetBaseComponentType

        protected List<string> _GetSubComponents(string compType)
        {
            List<string> lSubComps = new List<string>();

            XmlNodeList nodes = this.m_model.GetSubComponents(this.Configuration, compType);

            foreach (XmlNode node in nodes)
            {
                string subCompType = node.Attributes["name"].Value;
                lSubComps.Add(subCompType);
            }

            return lSubComps;
        }//_GetSubComponents

        protected List<string> _GetDerivedSubComponents(string compType, string baseType)
        {
            List<string> lDerivedSubComps = new List<string>();

            List<string> lSubComps = this._GetSubComponents(compType);
            List<string> lDerivedComps = this.m_model.GetDerivedComponentType(baseType);

            lDerivedSubComps = this._Intersect(lSubComps, lDerivedComps);

            return lDerivedSubComps;
        }//_GetDerivedSubComponents

        #endregion

        #region Components

        protected Component.eComponentType _GetComponentType(DataRow component)
        {
            return (Component.eComponentType)Enum.Parse(typeof(Component.eComponentType), (string)component[SchemaConstants.eType]);
        }//_GetComponentType

        //protected Component.eComponentType _GetComponentType(int compID)
        //{
        //    Component.eComponentType eType = Component.eComponentType.None;

        //    if (!this._ComponentExists(compID))
        //    {
        //        return Component.eComponentType.None;
        //    }

        //    //By parameters verifing if it's a class or instance.
        //    DataTable parameters = this.m_model.GetParameterTable(compID, eParamParentType.Component.ToString());
        //    string expression = SchemaConstants.Name + " = '" + Component.ComponentType.Name + "'";
        //    DataRow[] typeRows = parameters.Select(expression);

        //    if (typeRows.Length > 0)
        //    {
        //        DataRow row = typeRows[0];
        //        string sValue = (string)row[SchemaConstants.Value];
        //        eType = this._StringToEnum(sValue);
        //    }//if type variables exists

        //    //if there is no type stored in table, it is Component.
        //    if (eType == Component.eComponentType.None)
        //    {
        //        eType = Component.eComponentType.Component;
        //    }
        //    return eType;
        //}//_GetComponentType

        protected bool _ComponentExists(int compID)
        {
            DataRow component = this._GetComponent(compID);

            return (component != null);
        }//_ComponentExists

        protected DataRow _GetComponent(int compID)
        {
            DataRow component = null;

            String key = "" + compID;

            if (delayedValidationComponentTableCache != null)
            {
                if (delayedValidationComponentTableCache.ContainsKey(key))
                {
                    component = delayedValidationComponentTableCache[key];
                }
            }

            if (loadingComponentCache.ContainsKey(m_model))
            {
                if (loadingComponentCache[m_model].ContainsKey(key))
                {
                    component = loadingComponentCache[m_model][key];
                }
            }

            if (component == null)
            {
                DataTable components = this.m_model.GetComponent(compID);
                if (components.Rows.Count > 0)
                {
                    component = components.Rows[0];
                }
            }

            return component;
        }//_GetComponent

        protected DataTable _GetComponentTable(string type)
        {
            return this.m_model.GetComponentTable(SchemaConstants.Type, type);
        }//_GetComponentTable


        protected bool _IsValidRootID(int compID)
        {
            DataRow component = this._GetComponent(compID);

            //checking if component exists
            if (component != null)
            {
                //checking if component is of root type
                string sComponentType = (string)component[AME.Model.SchemaConstants.Type];
                string sBaseCompType = this._GetBaseComponentType(sComponentType);
                if (
                    (sComponentType == this.m_sRootComponentType)
                    ||
                    (sBaseCompType == this.m_sRootComponentType)
                    )
                {
                    return true;
                }
            }

            return false;
        }//_IsValidRootID


        #region Creation

        protected int _CreateComponent(string type, string name, string desc)
        {
            int componentID = this.m_model.CreateComponent(type, name, Component.eComponentType.Component.ToString(), desc);

            //Creating the parameters with their default values. Only the parameters which
            //have default values defined in config file, will be processed.
            this.Create_Component_DefaultParameters(componentID, type);

            return componentID;
        }//_CreateComponent

        private int _CreateComponentClass(string type, string name, string desc)
        {
            int classId = this.m_model.CreateComponent(type, name, Component.eComponentType.Class.ToString(), desc);

            if (classId == -1)
            {
                return -1;
            }

            // UseClassName is added in the config file
            // add future class type variables there

            //Marking it as a Class by adding a variable for it.
            //this._Create_Component_NewParameter(
            //    classId,
            //    Component.ComponentType.Type,
            //    eParamParentType.Component,
            //    Component.ComponentType.Name,
            //    Component.eComponentType.Class.ToString(),
            //    null,
            //    null
            //    );

            this.Create_Component_DefaultParameters(classId, type);

            return classId;
        }//_CreateComponentClass

        /// <summary>
        /// general clone method - create subclasses, instances, subinstances, etc.
        /// creates a clone off sourceID using nameForClone, description, and eType
        /// attaches clone to parentID using external linktype, validates this off topID
        /// finally, creates link between source and clone using internal linktype
        /// </summary>
        /// <param name="topID"></param>
        /// <param name="parentID"></param>
        /// <param name="sourceID"></param>
        /// <param name="nameForClone"></param>
        /// <param name="descriptionForClone"></param>
        /// <param name="eTypeForClone"></param>
        /// <param name="internalLinkTypeForClone"></param>
        /// <param name="externalLinkTypeForClone"></param>
        /// <returns></returns>
        protected ComponentAndLinkID _Clone(int topID, int parentID, int sourceID, String sourceType, String nameForClone, String descriptionForClone, Component.eComponentType eTypeForClone, String internalLinkTypeForClone, String externalLinkTypeForClone)
        {
            // pre validate
            this._ValidateAdd(topID, parentID, sourceType, eTypeForClone, nameForClone, externalLinkTypeForClone);

            int cloneID = this.m_model.CreateComponent(sourceType, nameForClone, eTypeForClone.ToString(), descriptionForClone);

            if (cloneID == -1) { return null; }

            this.Create_Component_DefaultParameters(cloneID, sourceType);

            //marking clone with eType variable, e.g. "Type = Instance" 
            //this.m_model.CreateParameter(cloneID, eParamParentType.Component.ToString(),
            //    Component.ComponentType.Name,
            //    eTypeForClone.ToString(), "");

            if (internalLinkTypeForClone != null && internalLinkTypeForClone != String.Empty) // optional
            {
                //Creating internal link between source and clone
                this.m_model.CreateLink(sourceID, cloneID, internalLinkTypeForClone, "");
            }

            // copy parameters from source to clone (can also multi-propogate to children, see controller_parameter)
            this.PropagateParameters(sourceID, cloneID);

            //Creating external link between parent and clone
            int linkID = this.m_model.CreateLink(parentID, cloneID, externalLinkTypeForClone, "");

            cache.AddComponentToCache(this, m_model, topID, parentID, cloneID, lastValidateAddComponentType, lastValidateAddBaseType, linkID, nameForClone, externalLinkTypeForClone, descriptionForClone, eTypeForClone, lastValidateAddParentXPath, lastSchemaValuesValidateAdd);

            return new ComponentAndLinkID(cloneID, linkID);
        }

        #endregion  //Create Component

        #region Add=Creation+Linking

        private ComponentAndLinkID _AddComponent(int topID, int parentID, string type, string name, string linkType, string desc)
        {
            //validating add by xml schema
            this._ValidateAdd(topID, parentID, type, Component.eComponentType.Component, name, linkType);

            int componentID = this._CreateComponent(type, name, desc);
            int linkID = -1;

            if (componentID != -1)
            {
                //Creating link between parent and child component
                linkID = this.m_model.CreateLink(parentID, componentID, linkType, "");

                if (linkID != -1)
                {
                    // default parameters
                    this.Create_Link_DefaultParameters(linkID, linkType, parentID, componentID);
                }
            }

            this._AddProgrammaticChildren(topID, parentID, componentID, type);

            return new ComponentAndLinkID(componentID, linkID);
        }//_AddComponent

        private ComponentAndLinkID _AddComponent(int topID, int parentID, string type, int linkID, string name, string linkType, string desc)
        {
            //validating add by xml schema
            this._ValidateAdd(topID, parentID, type, Component.eComponentType.Component, name, linkType);

            // currentLinkInfo
            DataRow currentLink = _GetLink(linkID);
            int currentFrom = Int32.Parse(currentLink[SchemaConstants.From].ToString());
            int currentTo = Int32.Parse(currentLink[SchemaConstants.To].ToString());
            String currentLinkType = currentLink[SchemaConstants.Type].ToString();

            // get link above and links below from current's parent (from) using this linktype
            DataTable links = this.m_model.GetChildComponentLinks(currentFrom, currentLinkType);

            Boolean currentFound = false;
            int testTo;
            List<DataRow> linksBelowCurrent = new List<DataRow>();
            foreach (DataRow aLink in links.Rows) // should be ordered
            {
                if (!currentFound)
                {
                    testTo = Int32.Parse(aLink[SchemaConstants.To].ToString()); // test until we find current
                    if (testTo == currentTo)
                    {
                        currentFound = true;
                    }
                }
                else // store all other links below current
                {
                    linksBelowCurrent.Add(aLink);
                }
            }

            // disconnect links below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                this.m_model.DeleteLink(Int32.Parse(linkBelow[SchemaConstants.Id].ToString()));
            }

            // create and connect child
            int componentID = this._CreateComponent(type, name, desc);
            int createdLink = -1;
            if (componentID != -1)
            {
                //Creating link between parent and child component
                createdLink = this.m_model.CreateLink(parentID, componentID, linkType, "");
            }

            this._AddProgrammaticChildren(topID, parentID, componentID, type);

            // connect children below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                int linkBelowTo = Int32.Parse(linkBelow[SchemaConstants.To].ToString());
                String linkBelowDescription = linkBelow[SchemaConstants.Description].ToString();
                this.m_model.CreateLink(currentFrom, linkBelowTo, currentLinkType, linkBelowDescription);
            }

            return new ComponentAndLinkID(componentID, createdLink);
        }//_AddComponent

        private ComponentAndLinkID _AddComponentClass(int topID, int parentID, string type, string name, string linkType, string desc)
        {
            //validating add by xml schema
            this._ValidateAdd(topID, parentID, type, Component.eComponentType.Class, name, linkType);

            int classID = this._CreateComponentClass(type, name, desc);
            int linkID = -1;
            if (classID != -1)
            {
                //Creating link between parent and class.
                linkID = this.m_model.CreateLink(parentID, classID, linkType, "");
            }

            this._AddProgrammaticChildren(topID, parentID, classID, type);

            return new ComponentAndLinkID(classID, linkID);
        }//_AddComponentClass

        protected void _AddProgrammaticChildren(int linkTopID, int linkFromID, int linkToNewComponentID, String parentType)
        {
            bool createdBH = false;
            if (myBulkHelper == null)
            {
                createdBH = true;
                myBulkHelper = new BulkHelper();
                myBulkHelper.BeginBulkOperations(this.Configuration);

                addExistingComponentForBulkHelper(linkTopID);

                if (linkTopID != linkFromID)
                {
                    addExistingComponentForBulkHelper(linkFromID);
                }

                if (linkTopID != linkToNewComponentID)
                {
                    addExistingComponentForBulkHelper(linkToNewComponentID);
                }
            }
            
            _AddProgrammaticChildren(""+linkTopID, ""+linkFromID, ""+linkToNewComponentID, parentType);

            if (createdBH) // creator should clean up
            {
                myBulkHelper.EndBulkOperations();
                myBulkHelper = null;
            }
        }

        private void addExistingComponentForBulkHelper(int id)
        {
            XPathNavigator compNav = this.GetComponent(id).CreateNavigator().SelectSingleNode("/Components/Component");
            String name = compNav.GetAttribute(XmlSchemaConstants.Display.Component.Name, "");
            String type = compNav.GetAttribute(XmlSchemaConstants.Display.Component.Type, "");
            String desc = compNav.GetAttribute(XmlSchemaConstants.Display.Component.Description, "");
            // programmatic links will refer by id, so use the id as the name / component helper
            myBulkHelper.AddExistingComponent(id, type, "" + id, desc);
        }

        protected void _AddProgrammaticChildren(String linkTopID, String linkFromID, String linkToNewComponentID, String parentType)
        {
            // Get XML from the <Global> section of the config file
            IXPathNavigable globalComponentXML = m_model.GetComponent(parentType);
            XPathNavigator globalComponentNavigator = globalComponentXML.CreateNavigator();
            _AddProgrammaticChildren(globalComponentNavigator, true, linkTopID, linkFromID, linkToNewComponentID, parentType);
        }

        private string progCreateComponent(string type, string name, string etype)
        {
            String createdID = "";
            // create class or component
            if (etype.Equals(Component.eComponentType.Component.ToString()))
            {
                createdID = myBulkHelper.CreateComponent(type, name, "");
            }
            else if (etype.Equals(Component.eComponentType.Class.ToString()))
            {
                createdID = myBulkHelper.CreateClass(type, name, "");
            }
            return createdID;
        }

        protected void _AddProgrammaticChildren(XPathNavigator componentNavigator, bool isExternalComponent, String linkTopID, String linkFromID, String linkToNewComponentID, String parentType)
        {
            String componentXPath;
            if (isExternalComponent)
            {
                componentXPath = "CreateComponents/CreateComponent";
            }
            else
            {
                componentXPath = "CreateComponent";
            }

            // bug fix - only turn off if it's already on.
            // if it's off, someone else did it, let them handle the state
            bool thisMethodTurnedControllerOff = false;
            if (m_bViewUpdateStatus)
            {
                this.TurnViewUpdateOff(); // don't send updates while connecting, creating
                thisMethodTurnedControllerOff = true;
            }

            if (AllowProgrammaticCreation)
            {
                // Get XML from the <Global> section of the config file
                //IXPathNavigable globalComponentXML = m_model.GetComponent(Configuration, parentType);
                //XPathNavigator globalComponentNavigator = globalComponentXML.CreateNavigator();

                if (isExternalComponent)
                {
                    // creating just links
                    CreateProgrammaticLinks(componentNavigator, linkTopID, linkFromID, linkToNewComponentID, parentType, isExternalComponent);
                }

                // creating components and links
                XPathNodeIterator childIterator = componentNavigator.Select(componentXPath);

                foreach (XPathNavigator child in childIterator)
                {
                    String name = child.GetAttribute(ConfigFileConstants.Name, child.NamespaceURI);
                    String componentType = child.GetAttribute(ConfigFileConstants.component, child.NamespaceURI);
                    String eType = child.GetAttribute(ConfigFileConstants.eType, child.NamespaceURI);
                    String configuration = child.GetAttribute(ConfigFileConstants.configuration, child.NamespaceURI);
                    String uidAttribute = child.GetAttribute(ConfigFileConstants.uid, child.NamespaceURI);

                    //Retrieve Parameter's values from config
                    Dictionary<String, String> paramValues = new Dictionary<String, String>();

                    XPathNavigator testNav = child.CreateNavigator();
                    XPathNodeIterator paramsIterator = child.Select("CreateParameters/CreateParameter");
                    foreach (XPathNavigator param in paramsIterator)
                    {
                        String sParamName = param.GetAttribute(ConfigFileConstants.Name, param.NamespaceURI);
                        String sParamValue = param.GetAttribute(ConfigFileConstants.Value, param.NamespaceURI);

                        paramValues.Add(sParamName, sParamValue);
                    }

                    // Retrieve the appropriate controller based on the configuration
                    Controller fromConfigurationName = AMEManager.Instance.Get(configuration) as Controller;

                    if (fromConfigurationName != null)
                    {
                        // If the parent is a root of a configuration we've created one programmatically 
                        // and need to re-assign topID
                        if (parentType.Equals(fromConfigurationName.RootComponentType))
                        {
                            linkTopID = linkToNewComponentID;
                        }

                        String createdID = "";
                        bool isRef = false;
                        if (uidAttribute != null && uidAttribute.Length > 0)
                        {
                            if (myBulkHelper.ReferenceNameExists(uidAttribute))
                            {
                                createdID = myBulkHelper.GetReferenceID(uidAttribute);
                                isRef = true;
                            }
                            else
                            {
                                createdID = progCreateComponent(componentType, name, eType);
                                myBulkHelper.AddReferenceID(uidAttribute, createdID);
                            }
                        }
                        else
                        {
                            createdID = progCreateComponent(componentType, name, eType);
                        }

                        if (createdID != null && createdID.Length > 0)
                        {
                            // don't create for references!
                            if (!isRef)
                            {
                                // creating component or class parameters
                                foreach (KeyValuePair<String, String> pair in paramValues)
                                {
                                    String sParamName = pair.Key;
                                    String sParamValue = pair.Value;
                                    myBulkHelper.UpdateParameters(createdID, sParamName, sParamValue, eParamParentType.Component);
                                }
                            }
                            CreateProgrammaticLinks(child, linkTopID, linkToNewComponentID, createdID, componentType, false);
                        }
                    }
                }
            }

            // if we turned it off, turn it back on
            if (thisMethodTurnedControllerOff)
            {
                this.TurnViewUpdateOn(false, false);
            }
        }

        private void CreateProgrammaticLinks(XPathNavigator node, String linkTopID, String linkFromID, String linkToNewComponentID, String componentType, bool externalLink)
        {
            // creating component's links
            XPathNodeIterator linksIterator = node.Select("CreateLinks/CreateLink");
            foreach (XPathNavigator link in linksIterator)
            {
                String linkType = link.GetAttribute(ConfigFileConstants.Name, link.NamespaceURI);
                String schemaRoot = link.GetAttribute(ConfigFileConstants.schemaRoot, link.NamespaceURI);
                String configuration = link.GetAttribute(ConfigFileConstants.configuration, link.NamespaceURI);
                String from = link.GetAttribute(ConfigFileConstants.from, link.NamespaceURI);
                String to = link.GetAttribute(ConfigFileConstants.to, link.NamespaceURI);

                // Retrieve the appropriate controller based on the configuration
                Controller fromConfigurationName = (Controller)AMEManager.Instance.Get(configuration);

                //Retrieve Parameter's values from config
                Dictionary<String, String> paramValues = new Dictionary<String, String>();
                XPathNodeIterator paramsIterator = link.Select("CreateParameter");
                foreach (XPathNavigator param in paramsIterator)
                {
                    String sParamName = param.GetAttribute(ConfigFileConstants.Name, param.NamespaceURI);
                    String sParamValue = param.GetAttribute(ConfigFileConstants.Value, param.NamespaceURI);

                    paramValues.Add(sParamName, sParamValue);
                }

                if (fromConfigurationName != null)
                {
                    if (linkType == null || linkType.Length == 0) // use configuration linktype if none is specified
                    {
                        linkType = fromConfigurationName.ConfigurationLinkType;
                    }

                    // from / to
                    String fromID = "";
                    String toID = "";
                    String linkID;

                    if (from.Equals(ConfigFileConstants.ParentValue))
                    {
                        fromID = linkFromID;
                    }
                    else if (from.Equals(ConfigFileConstants.SelfValue))
                    {
                        fromID = linkToNewComponentID;
                    }
                    else if (from.Equals(ConfigFileConstants.RootValue))
                    {
                        fromID = linkTopID;
                    }

                    if (to.Equals(ConfigFileConstants.ParentValue))
                    {
                        toID = linkFromID;
                    }
                    else if (to.Equals(ConfigFileConstants.SelfValue))
                    {
                        toID = linkToNewComponentID;
                    }
                    else if (from.Equals(ConfigFileConstants.RootValue))
                    {
                        toID = linkTopID;
                    }

                    if (schemaRoot.Equals(ConfigFileConstants.ParentValue)) // "Parent"
                    {
                        linkID = myBulkHelper.Connect(fromID, fromID, toID, linkType);
                        CreateLinkParameters(linkID, paramValues);

                        if (!externalLink)
                        {
                            // depth first
                            fromConfigurationName._AddProgrammaticChildren(node, false, fromID, fromID, toID, componentType);
                            // go to global
                            fromConfigurationName._AddProgrammaticChildren(fromID, fromID, toID, componentType);
                        }
                    }
                    else if (schemaRoot.Equals(ConfigFileConstants.RootValue)) // "Root"
                    {
                        linkID = myBulkHelper.Connect(linkTopID, fromID, toID, linkType);
                        CreateLinkParameters(linkID, paramValues);

                        if (!externalLink)
                        {
                            // depth first
                            fromConfigurationName._AddProgrammaticChildren(node, false, linkTopID, fromID, toID, componentType);
                            // go to global
                            fromConfigurationName._AddProgrammaticChildren(linkTopID, fromID, toID, componentType);
                        }
                    }
                    else if (schemaRoot.Equals(ConfigFileConstants.ParentParentValue)) // "ParentParent"
                    {
                        linkID = myBulkHelper.Connect(linkFromID, fromID, toID, linkType);
                        CreateLinkParameters(linkID, paramValues);

                        if (!externalLink)
                        {
                            // depth first
                            fromConfigurationName._AddProgrammaticChildren(node, false, linkFromID, fromID, toID, componentType);
                            // go to global
                            fromConfigurationName._AddProgrammaticChildren(linkFromID, fromID, toID, componentType);
                        }
                    }
                }
            }
        }

        private void CreateLinkParameters(String linkID, Dictionary<String, String> paramValues)
        {
            // creating component or class parameters
            foreach (KeyValuePair<String, String> pair in paramValues)
            {
                String sParamName = pair.Key;
                String sParamValue = pair.Value;
                myBulkHelper.UpdateParameters(linkID, sParamName, sParamValue, eParamParentType.Link);
            }
        }

        // get numeric suffix from ordered list, append to name, addinstance, repeat
        private List<ComponentAndLinkID> _AddComponentInstances(
            int topID, int parentID, int classID,
            string name, string linkType, string desc,
            int count)
        {
            List<ComponentAndLinkID> instances = new List<ComponentAndLinkID>();

            DataRow sourceComponent = this._GetComponent(classID);
            string sourceType = (string)sourceComponent[SchemaConstants.Type];

            List<int> orderedList = this.GetOrderedList(parentID, linkType, classID, sourceType, name, Component.Class.ClassInstanceLinkType);

            // now we're ready to add the instances
            for (int i = 0; i < count; i++)
            {
                // get append
                String append = GetNameFromOrderedList(ref orderedList);

                //int instances = this._GetInstanceIDs(parentID, classID).Count;

                ComponentAndLinkID instanceComponentAndLink = this._Clone(topID, parentID, classID, sourceType, name + append, desc, Component.eComponentType.Instance, Component.Class.ClassInstanceLinkType, linkType);

                if (instanceComponentAndLink != null)
                {
                    instances.Add(instanceComponentAndLink);
                }
            }
            return instances;
        }//_AddComponentInstances

        // get numeric suffix from ordered list, append to name, and clone
        private ComponentAndLinkID _AddSubClass(int topID, int parentID, int sourceID, String nameForSubclass, String descriptionForSubclass, String linkType)
        {
            DataRow sourceComponent = this._GetComponent(sourceID);
            string sourceType = (string)sourceComponent[SchemaConstants.Type];

            List<int> orderedList = this.GetOrderedList(parentID, linkType, sourceID, sourceType, nameForSubclass, Component.Class.ClassSubclassLinkType);

            String append = GetNameFromOrderedList(ref orderedList);

            return this._Clone(topID, parentID, sourceID, sourceType, nameForSubclass + append, descriptionForSubclass, Component.eComponentType.Subclass, Component.Class.ClassSubclassLinkType, linkType);
        }

        // make the numbering code resuable:
        public List<int> GetOrderedList(int parentID, string parentLinkType, int sourceID, String sourceType, String sourceName, String sourceLinkType)
        {
            // of a source under a linktype, find the child IDs of sourceLinkType that belong to a parent
            // record the names of children
            // example would be given a class, find all of the instances currently in use by a particular mission

            // limit to items that match the source type
            DataTable allComponents = this.m_model.GetChildComponents(parentID, SchemaConstants.Type, sourceType, parentLinkType);
            DataTable sourceChildren = this.m_model.GetChildComponents(sourceID, sourceLinkType);

            List<int> allComponentIDs = this._GetIDs(allComponents);

            List<string> commonNames = new List<string>();

            foreach (DataRow component in sourceChildren.Rows)
            {
                int componentID = (int)component[SchemaConstants.Id];
                if (allComponentIDs.Contains(componentID))
                {
                    commonNames.Add(component[SchemaConstants.Name].ToString());
                }
            }

            // Parse the names to ints.  Current format is SourceName01, SourceName02, etc.
            List<int> orderedList = new List<int>();
            foreach (String iName in commonNames)
            {
                String temp = iName;

                // 01, 02, 03
                temp = temp.Substring(sourceName.Length, temp.Length - sourceName.Length);
                orderedList.Add(Int32.Parse(temp));
            }

            // sort
            orderedList.Sort();

            return orderedList;
        }

        // numbering should be reused from GetOrderedList(...) above
        // from a sorted, numbered list, find the next number that can be inserted
        public String GetNameFromOrderedList(ref List<int> orderedList)
        {
            bool inOrder = true;
            int numberToAppendToInstance = -1;
            for (int j = 0; j < orderedList.Count; j++)
            {
                if ((j + 1) != orderedList[j]) // id appends are supposed to go 1, 2, 3, etc. which should equal index+1
                {
                    // index j is off, fix and insert
                    numberToAppendToInstance = j + 1;
                    orderedList.Insert(j, numberToAppendToInstance);
                    inOrder = false;
                    break;
                }
            }

            String append = "";
            if (!inOrder) // not in order, use number we found in the list
            {
                if (numberToAppendToInstance < 10)
                {
                    append = "0" + numberToAppendToInstance; // 01, 02, etc.
                }
                else
                {
                    append = "" + numberToAppendToInstance; ;
                }
            }
            else
            {   // we're already in order
                // if the list has any entries, use the highest number+1
                if (orderedList.Count > 0)
                {
                    numberToAppendToInstance = orderedList[orderedList.Count - 1] + 1;
                    orderedList.Add(numberToAppendToInstance);
                    if (numberToAppendToInstance < 10)
                    {
                        append = "0" + numberToAppendToInstance;
                    }
                    else
                    {
                        append = "" + numberToAppendToInstance;
                    }
                }
                else    //otherwise this is the first add
                {
                    numberToAppendToInstance = 1;
                    orderedList.Add(numberToAppendToInstance);
                    append = "0" + numberToAppendToInstance;
                }
            }

            return append;
        }

        #endregion  //Add Component

        #region Update

        private bool _UpdateComponentDescription(int compID, string value)
        {
            if (this._ComponentExists(compID))
            {
                this.m_model.UpdateComponent(compID, SchemaConstants.Description, value);
                return true;
            }

            return false;
        }//_UpdateComponentDescription

        private void _UpdateComponentName(int topID, int compID, string linkType, string name)
        {
            this._ValidateRename(topID, compID, linkType, name);
            this._UpdateComponentName(compID, name);
        }//_UpdateComponentName

        protected void _UpdateComponentName(int compID, string newName)
        {
            //Verifying if component exists
            DataRow component = this._GetComponent(compID);
            if (component == null)
            {
                string sError = string.Format("Component {0} does not exist.", compID);
                throw new System.Exception(sError);
            }

            String previousName = component[SchemaConstants.Name].ToString();

            Component.eComponentType eType = this._GetComponentType(component);
            switch (eType)
            {
                case Component.eComponentType.Component:
                    this.m_model.UpdateComponent(compID, SchemaConstants.Name, newName);
                    break;

                case Component.eComponentType.Class:

                    //get all of the instances of this class, and update their names also
                    DataTable instances = this._GetChildComponents(compID, Component.Class.ClassInstanceLinkType);
                    DataRowCollection instanceRows = instances.Rows;
                    if (instanceRows.Count > 0)
                    {
                        foreach (DataRow instance in instanceRows)
                        {
                            // name will be like Class01 - need to swap out "Class", leave numbering
                            String instanceNumbering = instance[SchemaConstants.Name].ToString();
                            instanceNumbering = instanceNumbering.Substring(previousName.Length, instanceNumbering.Length - previousName.Length);

                            int instanceID = Int32.Parse(instance[SchemaConstants.Id].ToString());
                            this.m_model.UpdateComponent(instanceID, SchemaConstants.Name, newName + instanceNumbering);
                        }
                    }//if has instances

                    // rename subclasses
                    DataTable subclasses = this._GetChildComponents(compID, Component.Class.ClassSubclassLinkType);
                    DataRowCollection subclassRows = subclasses.Rows;
                    if (subclassRows.Count > 0)
                    {
                        foreach (DataRow subclass in subclassRows)
                        {
                            int subclassID = Int32.Parse(subclass[SchemaConstants.Id].ToString());
                            _UpdateComponentName(subclassID, newName); // update subclass  and *its* instances
                        }
                    }

                    // update self 
                    this.m_model.UpdateComponent(compID, SchemaConstants.Name, newName);

                    break;

                case Component.eComponentType.Instance:
                    //this.m_model.UpdateComponent(compID, SchemaConstants.Name, name);  don't allow this
                    break;

                case Component.eComponentType.Subclass:

                    //get all of the instances of this class, and update their names also
                    instances = this._GetChildComponents(compID, Component.Class.ClassInstanceLinkType);
                    instanceRows = instances.Rows;
                    if (instanceRows.Count > 0)
                    {
                        foreach (DataRow instance in instanceRows)
                        {
                            // name will be like Class01 - need to swap out "Class", leave numbering
                            String instanceNumbering = instance[SchemaConstants.Name].ToString();
                            instanceNumbering = instanceNumbering.Substring(previousName.Length, instanceNumbering.Length - previousName.Length);

                            int instanceID = Int32.Parse(instance[SchemaConstants.Id].ToString());
                            this.m_model.UpdateComponent(instanceID, SchemaConstants.Name, newName + instanceNumbering);
                        }
                    }//if has instances

                    // update self
                    // name will be like Class01 - need to swap out "Class", leave numbering
                    DataTable parents = this.m_model.GetParentComponents(compID, Component.Class.ClassSubclassLinkType);

                    if (parents != null && parents.Rows.Count > 0)
                    {
                        DataRow parent = parents.Rows[0];
                        String parentName = parent[AME.Model.SchemaConstants.Name].ToString();

                        String subclassNumbering = previousName.Substring(parentName.Length, previousName.Length - parentName.Length);

                        this.m_model.UpdateComponent(compID, SchemaConstants.Name, newName + subclassNumbering);
                    }
                    break;
                default:
                    throw new System.Exception("Invalid component.");
            }//switch

        }//_UpdateComponentName

        #endregion  //Update Component

        #region Reading

        private DataTable _GetChildComponents(int sourceID, String linkType)
        {
            DataTable childComponents =
                this.m_model.GetChildComponents(sourceID, linkType);

            return childComponents;
        }//_GetInstanceIDs

        private DataTable _GetChildComponents(int compID, string type, string linkType)
        {
            DataTable childComponents =
                this.m_model.GetChildComponents(compID, AME.Model.SchemaConstants.Type, type, linkType);

            return childComponents;
        }//_GetChildComponents

        private List<int> _GetChildComponentIDs(int compID)
        {
            DataTable chidComponents = this.m_model.GetChildComponents(compID);

            return this._GetIDs(chidComponents);
        }//_GetChildComponentIDs

        protected List<int> _GetChildComponentIDs(int compID, string linkType)
        {
            DataTable components = this.m_model.GetChildComponents(compID, linkType);

            return this._GetIDs(components);
        }//_GetChildComponentIDs

        protected DataTable _GetChildComponents(int compID, string childType, bool isChildBaseType)
        {
            DataTable dtResult = new DataTable();

            if (!isChildBaseType)
            {
                dtResult = this.m_model.GetChildComponents(compID, AME.Model.SchemaConstants.Type, childType); ;
            }
            else
            {
                List<string> lDerComTypes = this.m_model.GetDerivedComponentType(childType);

                if (lDerComTypes.Count > 0)
                {
                    DataTable childComponents = this.m_model.GetChildComponents(compID);

                    if (childComponents.Rows.Count > 0)
                    {

                        dtResult = childComponents.Clone();

                        string sExpression = string.Format("{0} IN (", AME.Model.SchemaConstants.Type);
                        for (int i = 0; i < lDerComTypes.Count; i++)
                        {
                            sExpression += string.Format("'{0}'", lDerComTypes[i]);

                            if (i != (lDerComTypes.Count - 1))
                            {
                                sExpression += ", ";
                            }
                        }//for each derived type
                        sExpression += ")";

                        DataRow[] rowsMatched = childComponents.Select(sExpression);
                        for (int i = 0; i < rowsMatched.Length; i++)
                        {

                            dtResult.ImportRow(rowsMatched[i]);
                        }
                    }
                }//if any derived types in config
            }//baseType

            return dtResult;
        }//_GetChildComponents

        private int _GetClassID(int instanceID)
        {
            int classID = -1;   //be default invalid value

            DataTable classes = this.m_model.GetParentComponents(instanceID, Component.Class.ClassInstanceLinkType);
            if (classes.Rows.Count > 0)
            {
                //getting 1st element only. Instance gets implemented form only one class.
                DataRow drClass = classes.Rows[0];
                classID = (int)drClass[AME.Model.SchemaConstants.Id];
            }

            return classID;
        }//_GetClassID

        protected string _GetParameterValue(int parentID, String parameterName)
        {
            String value = "";
            DataTable parameters = this.m_model.GetParameterTable(parentID, eParamParentType.Component.ToString());
            string sExpression = string.Format("{0} = '{1}'",
                AME.Model.SchemaConstants.Name,
                parameterName);

            DataRow[] parameterRows = parameters.Select(sExpression);
            if (parameterRows.Length > 0)
            {
                value = (string)parameterRows[0][AME.Model.SchemaConstants.Value];
            }

            return value;
        }//_GetUseClassName

        private void _GetClassSubclass(DataRow comp,
            out DataRow drClass, out DataRow drSubclass)
        {
            drClass = null;
            drSubclass = null;

            if (comp != null)
            {
                Component.eComponentType eCompType = this._GetComponentType(comp);
                this._GetClassSubclass(comp, eCompType, out drClass, out drSubclass);
            }
        }//_GetClassSubclass

        private void _GetClassSubclass(DataRow comp, Component.eComponentType eCompType,
            out DataRow drClass, out DataRow drSubclass)
        {
            drClass = null;
            drSubclass = null;

            if (comp != null)
            {
                int compID = (int)comp[AME.Model.SchemaConstants.Id];
                this._GetClassSubclass(compID, eCompType, out drClass, out drSubclass);
            }
        }//_GetClassSubclass

        private void _GetClassSubclass(int compID, Component.eComponentType eCompType,
            out DataRow drClass, out DataRow drSubclass)
        {
            drClass = null;
            drSubclass = null;

            DataRow placeHolder;
            // check use class name controller variable for instances and subclasses
            //int classID = -1;
            //bool setClassID = true;

            DataRow parent = null;

            switch (eCompType)
            {
                case Component.eComponentType.Instance: // if parent is a subclass, use that subclass' class' name instead.

                    if (instanceToClassCache == null)
                    {
                        DataTable parents = this.m_model.GetParentComponents(compID, Component.Class.ClassInstanceLinkType);
                        parent = parents.Rows[0];
                    }
                    else
                    {
                        parent = instanceToClassCache[m_model]["" + compID];
                    }

                    Component.eComponentType ofParent = this._GetComponentType(parent);

                    if (ofParent == Component.eComponentType.Class)
                    {
                        drClass = parent;
                    }
                    else if (ofParent == Component.eComponentType.Subclass) // go up one more level
                    {
                        drSubclass = parent;
                        int parentID = (int)parent[AME.Model.SchemaConstants.Id];
                        this._GetClassSubclass(parentID, ofParent, out drClass, out placeHolder);
                    }

                    break;
                case Component.eComponentType.Subclass: // look up the class' name

                    if (subclassToClassCache == null)
                    {
                        DataTable parents = this.m_model.GetParentComponents(compID, Component.Class.ClassSubclassLinkType);
                        parent = parents.Rows[0];

                    }
                    else
                    {
                        parent = subclassToClassCache[m_model]["" + compID];
                    }

                    drClass = parent;

                    break;
            }//switch
        }//_GetClassSubclass

        private ComponentIDName CheckUseClassName(int componentID, Component.eComponentType eType)
        {
            DataRow drClass;
            DataRow drSubclass;

            this._GetClassSubclass(componentID, eType, out drClass, out drSubclass);

            return this.CheckUseClassName(drClass);
        }//CheckUseClassName

        private ComponentIDName CheckUseClassName(DataRow drClass)
        {
            ComponentIDName result = null;
            String useClassVariableName = Component.Class.InstancesUseClassName.Name;

            if (drClass != null)
            {
                DataRow parent = drClass;
                int parentID = (int)parent[AME.Model.SchemaConstants.Id];
                string parentName = parent[AME.Model.SchemaConstants.Name].ToString();

                if (parentID != -1)
                {
                    String useClassName = this._GetParameterValue(parentID, useClassVariableName);

                    if (useClassName != null && useClassName.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        result = new ComponentIDName(parentID, parentName); // use class name instead
                    }
                }
            }

            return result;
        }//CheckUseClassName

        // returns the string class name and int ID, if component is instance and has a class,
        // or is a subclass and has a class and applicable parameter is set
        // otherwise returns null
        /* old code
        private ComponentIDName CheckUseClassName(int componentID, Component.eComponentType eType)
        {
            // check use class name controller variable for instances and subclasses
            DataTable parents;
            String useClassVariableName = Component.Class.InstancesUseClassName.Name;
            int classID = -1;
            bool setClassID = true;

            switch (eType)
            {
                case Component.eComponentType.Instance: // if parent is a subclass, use that subclass' class' name instead.
                    parents = this.m_model.GetParentComponents(componentID, Component.Class.ClassInstanceLinkType);

                    if (parents != null && parents.Rows.Count > 0)
                    {
                        DataRow parent = parents.Rows[0];
                        int parentID = (int)parent[AME.Model.SchemaConstants.Id];
                        Component.eComponentType ofParent = this._GetComponentType(parent);

                        if (ofParent == Component.eComponentType.Subclass) // go up one more level
                        {
                            classID = parentID; // 'class' ID is the subclass ID, not the class ID
                            setClassID = false;
                            parents = this.m_model.GetParentComponents(parentID, Component.Class.ClassSubclassLinkType);
                        }
                    }
                    break;
                case Component.eComponentType.Subclass: // look up the class' name
                    parents = this.m_model.GetParentComponents(componentID, Component.Class.ClassSubclassLinkType);
                    break;
                default: return null;
            }

            if (parents != null && parents.Rows.Count > 0)
            {
                //getting 1st element only. Instance gets implemented from only one class.
                DataRow parent = parents.Rows[0];
                int parentID = (int)parent[AME.Model.SchemaConstants.Id];
                string parentName = parent[AME.Model.SchemaConstants.Name].ToString();

                if (setClassID)
                {
                    classID = parentID;
                }

                if (parentID != -1)
                {
                    String useClassName = this._GetParameterValue(parentID, useClassVariableName);

                    if (useClassName != null && useClassName.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return new ComponentIDName(parentID, parentName); // use class name instead
                    }
                }
            }
            return null;
        } // CheckUseClassName
        */

        protected List<int> _GetInstanceIDs(int parentID, int classID)
        {
            DataTable allComponents = this.m_model.GetChildComponents(parentID);
            DataTable classInstances = this.m_model.GetChildComponents(classID, Component.Class.ClassInstanceLinkType);

            List<int> allComponentIDs = this._GetIDs(allComponents);
            List<int> classInstanceIDs = this._GetIDs(classInstances);

            List<int> commonInstanceIDs = new List<int>();
            foreach (int id in classInstanceIDs)
            {
                if (allComponentIDs.Contains(id))
                {
                    commonInstanceIDs.Add(id);
                }
            }

            return commonInstanceIDs;
        }//_GetInstanceIDs

        private SortedValueList<int, Controller.ChildComponents> _GetInstancesInComponents(int classID)
        {
            //Getting class' all of the instances.
            List<int> instanceIDs = this._GetChildComponentIDs(classID, Component.Class.ClassInstanceLinkType);

            //Getting all instance's parents (excluding classes)
            SortedValueList<int, Controller.ChildComponents> parentComponents = new SortedValueList<int, ChildComponents>();
            foreach (int instanceID in instanceIDs)
            {
                DataTable parents = this.m_model.GetParentComponents(instanceID);
                foreach (DataRow parent in parents.Rows)
                {
                    int parentID = (int)parent[AME.Model.SchemaConstants.Id];
                    string parentName = (string)parent[AME.Model.SchemaConstants.Name];
                    if (this._GetComponentType(parent) != Component.eComponentType.Class)
                    {
                        if (!parentComponents.ContainsKey(parentID))
                        {
                            parentComponents[parentID] = new Controller.ChildComponents(parentID, parentName);
                            parentComponents[parentID].ChildIDs = this._GetInstanceIDs(parentID, classID);
                        }
                    }//Parent not a class
                }//foreach Parent

            }//foreach instance

            //and for those parents get all of the child instances
            return parentComponents;
        }//_GetInstancesInComponents

        #endregion  //Read Component

        #region Delete

        private bool _DeleteComponentAndDisconnect(int compID)
        {
            DataRow component = this._GetComponent(compID);

            if (component == null)
            {
                return false;
            }

            Component.eComponentType eType = this._GetComponentType(component);
            switch (eType)
            {
                case Component.eComponentType.Class:

                    // subclass check
                    List<int> subclassIDs = this._GetChildComponentIDs(compID, Component.Class.ClassSubclassLinkType);
                    if (subclassIDs.Count > 0)
                    {
                        string sError =
                        string.Format("Cannot delete Class {0}, it has {1} subclasses.  Please delete subclasses first.",
                        component[SchemaConstants.Name].ToString(), subclassIDs.Count);
                        throw new System.Exception(sError);
                    }

                    //Deleting the Class' instances
                    List<int> instanceIDs = this._GetChildComponentIDs(compID, Component.Class.ClassInstanceLinkType);
                    if (instanceIDs.Count > 0)
                    {
                        string prompt = string.Format("Class {0} has {1} instances.  Confirm delete.",
                            component[SchemaConstants.Name].ToString(), instanceIDs.Count);
                        DialogResult deleteCheck = MessageBox.Show(prompt, "Confirm Delete", MessageBoxButtons.YesNo);
                        if (deleteCheck == DialogResult.Yes)
                        {
                            foreach (int instanceID in instanceIDs)
                            {
                                this._DeleteComponentAndDisconnect(instanceID);
                            }
                            this.m_model.DeleteComponent(compID);
                            cache.DeleteComponent(m_model, compID);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.m_model.DeleteComponent(compID);
                        cache.DeleteComponent(m_model, compID);
                    }
                    break;
                case Component.eComponentType.Subclass:
                    //Deleting the Subclass' instances
                    instanceIDs = this._GetChildComponentIDs(compID, Component.Class.ClassInstanceLinkType);
                    if (instanceIDs.Count > 0)
                    {
                        string prompt = string.Format("Subclass {0} has {1} instances.  Confirm delete.",
                            component[SchemaConstants.Name].ToString(), instanceIDs.Count);
                        DialogResult deleteCheck = MessageBox.Show(prompt, "Confirm Delete", MessageBoxButtons.YesNo);
                        if (deleteCheck == DialogResult.Yes)
                        {
                            foreach (int instanceID in instanceIDs)
                            {
                                this._DeleteComponentAndDisconnect(instanceID);
                            }
                            this.m_model.DeleteComponent(compID);
                            cache.DeleteComponent(m_model, compID);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.m_model.DeleteComponent(compID);
                        cache.DeleteComponent(m_model, compID);
                    }
                    break;
                case Component.eComponentType.Instance:
                    this.m_model.DeleteComponent(compID);
                    cache.DeleteComponent(m_model, compID);
                    break;
                case Component.eComponentType.Component:
                    this.m_model.DeleteComponent(compID);
                    cache.DeleteComponent(m_model, compID);
                    break;
                default:
                    return false;
            }//switch eType
            return true;
        }//_DeleteComponentAndDiscoonect

        private bool _DeleteComponentAndGrandChildren(int compID, bool report)
        {
            //Taking delete related common actions without caring about its eType.
            Dictionary<int, int> allComponents  = new Dictionary<int, int>();
            allComponents.Add(compID, compID);
            getAllChildIDs(ref allComponents, compID);

            try
            {
                this.BackgroundSetup("Deleting", "Deleting...", 0, allComponents.Count, 1);

                foreach (int component in allComponents.Keys)
                {
                    this._DeleteComponentAndDisconnect(component);
                    this.BackgroundProgress();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.BackgroundDone();
            }

            return true;
        }//_DeleteComponentAndGrandChildren

        private void getAllChildIDs(ref Dictionary<int, int> allChildIDs, int startingID)
        {
            List<int> childIDs = this._GetChildComponentIDs(startingID);

            foreach (int childID in childIDs)
            {
                if (!allChildIDs.ContainsKey(childID))
                {
                    allChildIDs.Add(childID, childID);
                    getAllChildIDs(ref allChildIDs, childID);
                }
            } 
        }

        private bool _DeleteComponentAndGrandChildren(int compID, string linktype, List<String> ignoreTheseTypes)
        {
            DataRow component = this._GetComponent(compID);

            if (component == null)
            {
                return false;
            }

            String componentType = component[SchemaConstants.Type].ToString();

            //Deleting component's children with linktype 
            DataTable children = this.m_model.GetChildComponents(compID, linktype);

            foreach (DataRow childRow in children.Rows)
            {
                String childType = childRow[SchemaConstants.Type].ToString();

                // do we pass the filter?
                if (ignoreTheseTypes.Count == 0 || !ignoreTheseTypes.Contains(childType))
                {
                    int childID = Int32.Parse(childRow[SchemaConstants.Id].ToString());
                    //Deleting child component.
                    this._DeleteComponentAndGrandChildren(childID, linktype, ignoreTheseTypes);
                }
            }

            Component.eComponentType eType = this._GetComponentType(component);
            switch (eType)
            {
                case Component.eComponentType.Class:
                    //Deleting the Class' instances
                    List<int> instanceIDs = this._GetChildComponentIDs(compID, Component.Class.ClassInstanceLinkType);
                    foreach (int instanceID in instanceIDs)
                    {
                        this.m_model.DeleteComponent(instanceID);
                        cache.DeleteComponent(m_model, instanceID);
                    }

                    //Deleting the Class' subclasses
                    List<int> subClassIDs = this._GetChildComponentIDs(compID, Component.Class.ClassSubclassLinkType);
                    foreach (int subclassID in subClassIDs)
                    {
                        this.m_model.DeleteComponent(subclassID);
                        cache.DeleteComponent(m_model, subclassID);
                    }
                    break;
                case Component.eComponentType.Subclass:
                    //Deleting the Subclass' instances
                    instanceIDs = this._GetChildComponentIDs(compID, Component.Class.ClassInstanceLinkType);
                    foreach (int instanceID in instanceIDs)
                    {
                        this.m_model.DeleteComponent(instanceID);
                        cache.DeleteComponent(m_model, instanceID);
                    }
                    break;
                case Component.eComponentType.Instance:
                case Component.eComponentType.Component:
                    break;

                default:
                    return false;
            }//switch eType

            //Deleting the Component
            // Do we pass the filter?
            if (!ignoreTheseTypes.Contains(componentType))
            {
                this.m_model.DeleteComponent(compID);
                cache.DeleteComponent(m_model, compID);
            }

            return true;
        }//_DeleteComponentAndGrandChildren

        #endregion  //Delete Component

        #endregion  //Components

        #region Links

        private bool _LinkExists(int linkID)
        {
            DataRow link = this._GetLink(linkID);

            return this._LinkExists(link);
        }//_LinkExists

        private bool _LinkExists(DataRow link)
        {
            return (link != null);
        }//_LinkExists

        protected DataRow _GetLink(int linkID)
        {
            DataRow link = null;

            DataTable links = this.m_model.GetLink(linkID);
            if (links.Rows.Count > 0)
            {
                link = links.Rows[0];
            }

            return link;
        }//_GetLink

        protected IXPathNavigable _GetLink(int linkID, bool addFromToNamesAndTypes)
        {
            string sSchemaFilePath = this._DisplayLinkSchemaPath;
            bool bValidSchemaPath = false;
            bValidSchemaPath = Path.HasExtension(sSchemaFilePath);

            if (bValidSchemaPath)
            {
                using (XmlReader schemaXSD = this.GetXSD(sSchemaFilePath))
                {
                    if (schemaXSD == null)
                    {
                        throw new System.IO.FileNotFoundException("File does not exist.", sSchemaFilePath);
                    }
                }
            }

            if (!bValidSchemaPath)
            {
                throw new System.ArgumentException("Invalid schema path.");
            }

            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create root element
            XmlElement root = doc.CreateElement(XmlSchemaConstants.Display.sLink + "s");//"Links"

            // Add schema information to root.
            XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schema.Value = this._DisplayLinkSchemaPath;
            root.SetAttributeNode(schema);

            doc.AppendChild(root);

            DataTable linkTable = this.m_model.GetLink(linkID);

            if (linkTable.Rows.Count == 1)
            {
                DataRow linkData = linkTable.Rows[0];

                int linkFromID = (int)linkData[SchemaConstants.From];
                int linkToID = (int)linkData[SchemaConstants.To];

                //creating element
                XmlElement linkElement = doc.CreateElement(XmlSchemaConstants.Display.sLink);

                //creating attributes
                XmlAttribute attrLinkID = doc.CreateAttribute(XmlSchemaConstants.Display.Link.Id);
                attrLinkID.Value = linkData[SchemaConstants.Id].ToString();

                XmlAttribute attrLinkFromID = doc.CreateAttribute(XmlSchemaConstants.Display.Link.FromId);
                attrLinkFromID.Value = linkFromID.ToString();

                XmlAttribute attrLinkToID = doc.CreateAttribute(XmlSchemaConstants.Display.Link.ToId);
                attrLinkToID.Value = linkToID.ToString();

                XmlAttribute attrLinkType = doc.CreateAttribute(XmlSchemaConstants.Display.Link.Type);
                attrLinkType.Value = linkData[SchemaConstants.Type].ToString();

                XmlAttribute attrLinkDesc = doc.CreateAttribute(XmlSchemaConstants.Display.Link.Description);
                attrLinkDesc.Value = linkData[SchemaConstants.Description].ToString();

                //adding attributes
                linkElement.SetAttributeNode(attrLinkID);
                linkElement.SetAttributeNode(attrLinkFromID);
                linkElement.SetAttributeNode(attrLinkToID);
                linkElement.SetAttributeNode(attrLinkType);
                linkElement.SetAttributeNode(attrLinkDesc);

                if (addFromToNamesAndTypes) // optionally add names and types (requires extra DB work)
                {
                    DataRow fromComponentRow = this._GetComponent(linkFromID);
                    DataRow toComponentRow = this._GetComponent(linkToID);

                    if (fromComponentRow != null && toComponentRow != null)
                    {
                        XmlAttribute attrFromName = doc.CreateAttribute(XmlSchemaConstants.Display.Link.FromName);
                        attrFromName.Value = fromComponentRow[SchemaConstants.Name].ToString();

                        XmlAttribute attrFromType = doc.CreateAttribute(XmlSchemaConstants.Display.Link.FromType);
                        attrFromType.Value = fromComponentRow[SchemaConstants.Type].ToString();

                        XmlAttribute attrToName = doc.CreateAttribute(XmlSchemaConstants.Display.Link.ToName);
                        attrToName.Value = toComponentRow[SchemaConstants.Name].ToString();

                        XmlAttribute attrToType = doc.CreateAttribute(XmlSchemaConstants.Display.Link.ToType);
                        attrToType.Value = toComponentRow[SchemaConstants.Type].ToString();

                        // check use class name controller variable
                        ComponentIDName fromClassName = this.CheckUseClassName(linkFromID, _GetComponentType(fromComponentRow));
                        if (fromClassName != null)
                        {
                            attrFromName.Value = fromClassName.Name;
                        }

                        ComponentIDName toClassName = this.CheckUseClassName(linkToID, _GetComponentType(toComponentRow));
                        if (toClassName != null)
                        {
                            attrToName.Value = toClassName.Name;
                        }

                        linkElement.SetAttributeNode(attrFromName);
                        linkElement.SetAttributeNode(attrFromType);
                        linkElement.SetAttributeNode(attrToName);
                        linkElement.SetAttributeNode(attrToType);
                    }
                }

                if (linkElement != null)
                {
                    root.AppendChild(linkElement);
                }
            }
            return doc;
        }//GetLink

        private int _Connect(int topID, int fromID, int toID, string linkType)
        {
            try
            {
                // prebuild a component cache if we're delaying validation
                if (UseDelayedValidation && delayedValidationComponentTableCache == null)
                {
                    delayedValidationComponentTable = GetComponentTable();
                    delayedValidationComponentTableCache = GetComponentTableCache(delayedValidationComponentTable);
                }

                //Checking if top component exists
                if (!this._ComponentExists(topID))
                {
                    string sError = string.Format("Top {0} does not exist.", topID);
                    throw new System.Exception(sError);
                }

                DataRow fromComponent = this._GetComponent(fromID);
                DataRow toComponent = this._GetComponent(toID);

                //Checking parent is in Database
                if (fromComponent == null)
                {
                    string sError = string.Format("Parent {0} does not exist.", fromID);
                    throw new System.Exception(sError);
                }
                //Checking child is in Database
                if (toComponent == null)
                {
                    string sError = string.Format("Child {0} does not exist.", toID);
                    throw new System.Exception(sError);
                }

                ////Both ids can't be same
                //if (fromID == toID)
                //{
                //    string sError = string.Format("Can not create link to itself.");
                //    throw new System.ArgumentException(sError);
                //}

                //not allowing view to create a link of type Class-Instance
                if (linkType == Component.Class.ClassInstanceLinkType || linkType == Component.Class.ClassSubclassLinkType)
                {
                    string sError =
                    string.Format("Can not create a link of {0} type. It is reserved word.",
                        linkType);
                    throw new System.ArgumentException(sError);
                }

                Component.eComponentType eFromType = this._GetComponentType(fromComponent);
                Component.eComponentType eToType = this._GetComponentType(toComponent);
                string sFromComponentType = (string)fromComponent[AME.Model.SchemaConstants.Type];
                string sToComponentType = (string)toComponent[AME.Model.SchemaConstants.Type];
                string sToName = (string)toComponent[AME.Model.SchemaConstants.Name];
                string sDescription = (string)toComponent[AME.Model.SchemaConstants.Description];

                // I've commented this out for now as per a discussion with Bhavna - MW.
                // This affects freely connecting within diagrams, which in this case 
                // isn't a quanity / instance count issue.

                //For instance validating the # of instances against class' Quantity.
                //if (eToType == Component.eComponentType.Instance)
                //{
                //    //get the class, by its quantity check the instances of the class.
                //    int classID = this._GetClassID(toID);

                //    if (classID == -1)
                //    {
                //        string sError = string.Format("Invalid toID {0} instance. It does not have a class.", toComponent);
                //        throw new System.ArgumentException(sError);
                //    }

                //    int quantity = this._GetQuantity(classID);
                //    List<int> instanceIDs = this._GetInstanceIDs(fromID, classID);
                //    int instances = instanceIDs.Count;

                //    if ((instances >= quantity) && !instanceIDs.Contains(toID))
                //    {
                //        StringBuilder sb = new StringBuilder();
                //        sb.Append("Can not link the two components. ");
                //        sb.Append("Child component ");
                //        sb.Append(toID);
                //        sb.Append(" is an instance of class ");
                //        sb.Append(classID);
                //        sb.Append(". This class' Quantity is ");
                //        sb.Append(quantity);
                //        sb.Append(" and parent component already has ");
                //        sb.Append(instances);
                //        sb.Append(" instances of this class.");

                //        throw new System.Exception(sb.ToString());
                //    }
                //}//if instance

                // sanity check before validation
                ValidateConnect(linkType, sFromComponentType, sToComponentType);

                if (UseDelayedValidation)
                {
                    // for delayed validation e.g. VSG import
                    // store the top ids and the linktypes, use getChildren with validation to 
                    // retrieve those when UseCachedValidation is turned back on
                    // remember to turn off UseCachedValidation when done! MW 3/20/08
                    string storedValidKey = "" + topID + linkType;
                    if (!storedValidationTopIDsAndLinkTypes.ContainsKey(storedValidKey))
                    {
                        storedValidationTopIDsAndLinkTypes.Add(storedValidKey, new TopIDAndLinkType(topID, linkType));
                    }
                }
                else
                {
                    // default, go to db to retrieve validation xml each time
                    this._ValidateAdd(topID, fromID, sToComponentType, eToType, sToName, linkType);
                }

                // DB unique constraints on the linktable will handle this now - MW 3/20/08
                //Checking if a link of same type does not exist between components.
                //if (this._GetChildComponentIDs(fromID, linkType).Contains(toID))
                //{
                //    string sError = string.Format(
                //        "A Link between components {0} and {1} of type {2} already exists.",
                //        fromID, toID, linkType);
                //    throw new System.Exception(sError);
                //}

                //Creating link
                int linkID = this.m_model.CreateLink(fromID, toID, linkType, "");

                if (linkID != -1)
                {
                    // default parameters
                    this.Create_Link_DefaultParameters(linkID, linkType, fromID, toID);
                }

                if (!UseDelayedValidation && linkID != -1)
                {
                    cache.AddComponentToCache(this, m_model, topID, fromID, toID, lastValidateAddComponentType, lastValidateAddBaseType, linkID, sToName, linkType, sDescription, eToType, lastValidateAddParentXPath, lastSchemaValuesValidateAdd);
                }
                return linkID;
            }
            catch (Exception)
            {
                throw;
            }
        }//_Connect

        private void _IncrementLink(int linkID)
        {
            Dictionary<int, int> linkIDChanges = new Dictionary<int, int>();

            // currentLinkInfo
            DataRow currentLink = _GetLink(linkID);
            int currentFrom = Int32.Parse(currentLink[SchemaConstants.From].ToString());
            int currentTo = Int32.Parse(currentLink[SchemaConstants.To].ToString());
            String type = currentLink[SchemaConstants.Type].ToString();

            // get link above and links below from current's parent (from) using this linktype
            DataTable links = this.m_model.GetChildComponentLinks(currentFrom, type);

            Boolean currentFound = false;
            int testTo;
            DataRow linkAboveCurrent = null;
            DataRow previousLink = null;
            List<DataRow> linksBelowCurrent = new List<DataRow>();
            foreach (DataRow aLink in links.Rows) // should be ordered
            {
                if (!currentFound)
                {
                    testTo = Int32.Parse(aLink[SchemaConstants.To].ToString()); // test until we find current
                    if (testTo == currentTo)
                    {
                        currentFound = true;
                        linkAboveCurrent = previousLink; // link above is previous link
                    }
                    else
                    {
                        previousLink = aLink; // store previous until we find current
                    }
                }
                else // store all other links below current
                {
                    linksBelowCurrent.Add(aLink);
                }
            }

            // disconnect link above
            if (linkAboveCurrent != null)
            {
                this.m_model.DeleteLink(Int32.Parse(linkAboveCurrent[SchemaConstants.Id].ToString()));
            }
            else
            {
                return;
            }

            // disconnect links below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                this.m_model.DeleteLink(Int32.Parse(linkBelow[SchemaConstants.Id].ToString()));
            }

            // connect link above
            if (linkAboveCurrent != null)
            {
                int linkAboveTo = Int32.Parse(linkAboveCurrent[SchemaConstants.To].ToString());
                String linkAboveDescription = linkAboveCurrent[SchemaConstants.Description].ToString();
                int newAboveID = this.m_model.CreateLink(currentFrom, linkAboveTo, type, linkAboveDescription);

                linkIDChanges.Add(Int32.Parse(linkAboveCurrent[SchemaConstants.Id].ToString()), newAboveID);
            }

            // connect links below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                int linkBelowTo = Int32.Parse(linkBelow[SchemaConstants.To].ToString());
                String linkBelowDescription = linkBelow[SchemaConstants.Description].ToString();
                int newBelowID = this.m_model.CreateLink(currentFrom, linkBelowTo, type, linkBelowDescription);

                linkIDChanges.Add(Int32.Parse(linkBelow[SchemaConstants.Id].ToString()), newBelowID);
            }

            cache.UpdateLinkIDs(m_model, linkIDChanges);

            SendUpdateOfType(UpdateType.Component);
        }

        private void _DecrementLink(int linkID)
        {
            Dictionary<int, int> linkIDChanges = new Dictionary<int, int>();

            // currentLinkInfo
            DataRow currentLink = _GetLink(linkID);
            int currentFrom = Int32.Parse(currentLink[SchemaConstants.From].ToString());
            int currentTo = Int32.Parse(currentLink[SchemaConstants.To].ToString());
            String type = currentLink[SchemaConstants.Type].ToString();

            // get link above and links below from current's parent (from) using this linktype
            DataTable links = this.m_model.GetChildComponentLinks(currentFrom, type);

            Boolean currentFound = false;
            int testTo;
            List<DataRow> linksBelowCurrent = new List<DataRow>();
            foreach (DataRow aLink in links.Rows) // should be ordered
            {
                if (!currentFound)
                {
                    testTo = Int32.Parse(aLink[SchemaConstants.To].ToString()); // test until we find current
                    if (testTo == currentTo)
                    {
                        currentFound = true;
                    }
                }
                else // store all other links below current
                {
                    linksBelowCurrent.Add(aLink);
                }
            }

            // disconnect current
            if (currentLink != null)
            {
                this.m_model.DeleteLink(Int32.Parse(currentLink[SchemaConstants.Id].ToString()));
            }

            if (linksBelowCurrent.Count == 0)
            {
                return;
            }

            // disconnect links below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                this.m_model.DeleteLink(Int32.Parse(linkBelow[SchemaConstants.Id].ToString()));
            }

            if (linksBelowCurrent.Count >= 1)
            {
                // connect first link in list
                DataRow first = linksBelowCurrent[0];

                int firstTo = Int32.Parse(first[SchemaConstants.To].ToString());
                String firstDescription = first[SchemaConstants.Description].ToString();
                int newFirstID = this.m_model.CreateLink(currentFrom, firstTo, type, firstDescription);

                linkIDChanges.Add(Int32.Parse(first[SchemaConstants.Id].ToString()), newFirstID);

                linksBelowCurrent.RemoveAt(0);
            }

            // connect current
            if (currentLink != null)
            {
                String currentDescription = currentLink[SchemaConstants.Description].ToString();
                int newCurrentID = this.m_model.CreateLink(currentFrom, currentTo, type, currentDescription);

                linkIDChanges.Add(Int32.Parse(currentLink[SchemaConstants.Id].ToString()), newCurrentID);
            }

            // connect links below
            foreach (DataRow linkBelow in linksBelowCurrent)
            {
                int linkBelowTo = Int32.Parse(linkBelow[SchemaConstants.To].ToString());
                String linkBelowDescription = linkBelow[SchemaConstants.Description].ToString();
                int newBelowID = this.m_model.CreateLink(currentFrom, linkBelowTo, type, linkBelowDescription);

                linkIDChanges.Add(Int32.Parse(linkBelow[SchemaConstants.Id].ToString()), newBelowID);
            }

            cache.UpdateLinkIDs(m_model, linkIDChanges);
            
            SendUpdateOfType(UpdateType.Component);
        }

        private bool _DeleteLink(int linkID)
        {
            DataRow link = this._GetLink(linkID);
            return this._DeleteLink(link);
        }

        private bool _DeleteLink(DataRow link)
        {
            bool bResult = false;

            if (this._LinkExists(link))
            {
                int linkID = Int32.Parse(link[SchemaConstants.Id].ToString());
                string sLinkType = (string)link[SchemaConstants.Type];

                //Not allowing View to delete a link between class & instance.
                if (sLinkType == Component.Class.ClassInstanceLinkType)
                {
                    string sError = string.Format("Can not delete a link of type {0}.", sLinkType);
                    throw new System.Exception(sError);
                }

                //Deleting link from-to
                this.m_model.DeleteLink(linkID);
                cache.DeleteLink(m_model, linkID, sLinkType);
                bResult = true;

            }//if link exists

            return bResult;
        }//_DeleteLink

        private bool _DeleteLinks(String linkType)
        {
            bool bResult = false;

            List<Int32> deletedLinkIDs = this.m_model.DeleteLinks(linkType);
            foreach (Int32 linkID in deletedLinkIDs)
            {
                cache.DeleteLink(m_model, linkID, linkType);
            }

            bResult = true;

            return bResult;
        }

        private List<string> _GetChildComponentLinkTypes(int compID)
        {
            List<string> childLinkTypes = new List<string>();

            DataTable childLinks = this.m_model.GetChildComponentLinks(compID);
            foreach (DataRow childLink in childLinks.Rows)
            {
                string sLinkType = (string)childLink[AME.Model.SchemaConstants.Type];
                childLinkTypes.Add(sLinkType);
            }
            return childLinkTypes;
        }//_GetChildComponentLinkTypes

        #endregion  //Links

        #endregion  //Private Methods


    }//Controller class

}//Controllers namespace
