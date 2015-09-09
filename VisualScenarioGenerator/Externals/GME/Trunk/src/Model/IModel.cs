using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Specialized;

namespace AME.Model
{
    public interface IModel
    {
        void InitializeDB();
        void ImportSql(String filename);

        DataTable GetComponentTable();
        DataTable GetComponentTable(String columnType, String name);
        DataTable GetComponent(Int32 id);
        DataTable GetLink(Int32 id);
        DataTable GetLinkTable();
        List<String> GetDynamicLinkTypes(String linkType);
        DataTable GetLinkTable(String linkType);
        DataTable GetParameterTable();
        DataTable GetParameterTable(Int32 parentId, String parentType);
        DataTable GetChildComponents(Int32 id);
        DataTable GetChildComponents(Int32 id, String linkType);
        DataTable GetChildComponents(Int32 id, String columnName, String columnValue);
        DataTable GetChildComponents(Int32 id, String columnName, String columnValue, String linkType);
        DataTable GetParentComponents(Int32 id);
        DataTable GetParentComponents(Int32 id, String linkType);
        DataTable GetChildComponentLinks(Int32 id);
        DataTable GetChildComponentLinks(Int32 id, String linkType);
        NameValueCollection GetRootLinkTypes();

        IXPathNavigable GetSimRun(String filename);

        List<String> GetConfigurationNames();
        String GetRootComponent(String configuration);
        String GetComponentLinkType(String configuration);

        Dictionary<String, Bitmap> GetComponentBitmaps(String configuration);

        String GetParameterType(String parameterName, String category, String componentType);
        String GetParameterType(String configuration, String linkType, String fromComponentType, String toComponentType, String parameterName, String category);

        Int32 CreateComponent(String component, String name, String eType, String description);
        Int32 GetComponentId(String component, String name);
        void DeleteComponent(Int32 id);
        Int32 UpdateComponent(Int32 id, String columnName, String columnValue);
        Int32 CreateLink(Int32? fromComponentId, Int32 toComponentId, String type, String description);
        void DeleteLink(Int32 id);
        Int32 CreateParameter(Int32 parentId, String parentType, String name, String value, String description);

        XmlNodeList GetSubComponents(String configuration, String component);
        IXPathNavigable GetComponent(String configuration, String component);
        IXPathNavigable GetConfiguration(String configuration);

        XmlNode GetParametersXML(String configuration, String linkType, String fromType, String toType);
        XmlNode GetParametersXML(String componentType);

        IXPathNavigable GetLinks();
        IXPathNavigable GetLink(String linkType, String fromType, String toType);

        String GetBaseComponentType(String compType);
        List<String> GetDerivedComponentType(String baseCompType);

        XmlNode GetParameter(String parameterName, String category, String componentType);
        XmlNode GetParameter(String configuration, String linkType, String fromComponentType, String toComponentType, String parameterName, String category);
    }//IModel
}//MVC
