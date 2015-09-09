/*
 * Interface        : IController
 * File             : IController.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Interface for Controller class. 
 * Contains the signature of the methods, properties, and events implemented 
 * in Controller class.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using AME.Model;
using AME.Views.View_Components;
using AME.Controllers.Base.Data_Structures;
using AME.Controllers.Base.DataStructures;
using System.IO;

#endregion  //namespaces

namespace AME.Controllers
{
    public interface IController
    {
        string Configuration { get; }
        string RootComponentType { get; }
        string ConfigurationLinkType { get; }

        string ModelPath { get; }
        string OutputPath { get; }
        string DataPath { get; }
        string XmlPath { get; }
        string DocumentationPath { get; }
        string LicensePath { get; }

        XmlReader GetXSD(String xsd);
        XmlReader GetXSL(String xsl);
        XmlReader GetConfigurationReader();
        Stream GetImage(String image);

        bool UseDelayedValidation { get; set; }

        bool AllowProgrammaticCreation { get; set; }

        bool IgnoreEmptyString { get; set; }

        bool AllowNewData { get; set; }

        bool ProgressBarForDeletes { get; set; }

        void EnableLoadingCache();
        void DisableLoadingCache();

        void ValidateAllSchemaLinks();
        void ValidateParameterValues(bool replaceFaultyValuesWithDefaults, bool addNew);

        int GetDynamicPivotFromLinkType(String linkType);
        bool IsLinkTypeDynamic(String linkType);
        string GetDynamicLinkType(String baseLinkType, String componentName);
        string GetDynamicLinkType(String linkType, String componentName, Boolean forImporter);
        string GetBaseLinkType(String dynamicLinkType);

        IXPathNavigable GetConfiguration();
        IXPathNavigable GetLinks();

        void CacheIndexLinkTypes();
        void ClearCache();
        void ClearCache(String linkType);

        ComponentAndLinkID AddComponent(int topID, int parentID, string type, string name, string linkType, string desc);
        ComponentAndLinkID AddComponent(int topID, int parentID, string type, int linkID, string name, string linkType, string desc);
        ComponentAndLinkID AddComponentClass(int topID, int parentID, string type, string name, string linkType, string desc);
        ComponentAndLinkID AddComponentInstance(int topID, int parentID, int classID, string name, string linkType, string desc);
        List<ComponentAndLinkID> AddComponentInstances(int topID, int parentID, int classID, string name, string linkType, string desc, int count);
        NameValueCollection GetRootLinkTypes();
        ComponentAndLinkID AddSubClass(int topID, int parentID, int sourceID, string nameForSubclass, string descriptionForSubclass, string linkType);
        List<Int32> BulkCreateComponents(List<ComponentInfo> bulkComponentInfo, Dictionary<String, String> namesWithParameters);
        List<Int32> BulkCreateLinks(List<LinkInfo> bulkLinkInfo, Dictionary<String, String> namesWithParameters);
        void BulkCreateParameters(List<ParameterInfo> bulkParameterInfo);
        int CreateComponent(string type, string name, string desc);
        int CreateClass(string type, string name, string desc);
        bool UpdateComponentDescription(int compID, string value);
        void UpdateComponentName(int topID, int compID, string linkType, string newName);
        void UpdateComponentName(int compID, string newName);
        bool DeleteComponent(int compID);
        bool DeleteComponentAndChildren(int compID);
        bool DeleteComponentAndChildren(int compID, bool reportProgress);
        bool DeleteComponentAndChildren(int compID, string linktype);
        bool DeleteComponentAndChildren(int compID, string linktype, List<String> ignoreTheseTypes);
        void DeleteParameter(int parentID, String parentType, String name, bool update);
        int GetInstanceCount(int compID, int classID);
        IXPathNavigable GetComponentAndChildren(int compID, string linkType, ComponentOptions compOptions);
        IXPathNavigable GetComponentAndChildren(int topID, int compID, string linkType, ComponentOptions compOptions);
        IXPathNavigable GetComponent(int compID);
        IXPathNavigable GetComponent(String type);
        string GetBaseComponentType(string compType);
        IXPathNavigable GetParametersForComponent(int id);
        String GetComponentParameterValueFromXml(XPathNavigator parametersXML, String fullParameterName);
        IXPathNavigable GetOutputXml(string filename);

        int Connect(int topID, int parentID, int childID, string linkType);
        bool LinkExists(int fromID, int toID, String linkType);
        DataRow GetLink(int fromID, int toID, String linkType);
		int GetLinkID(int fromID, int toID, String linkType);
        bool DeleteLink(int linkID);
        bool DeleteLink(DataRow link);
        bool DeleteLinks(String linkType);
        IXPathNavigable GetLink(int linkID, bool addFromToNamesAndTypes);
        IXPathNavigable GetParametersForLink(int linkID, String linkType, String fromType, String toType);
        IXPathNavigable GetParametersForLink(int linkID);
        void IncrementLink(int linkID);
        void DecrementLink(int linkID);

        Dictionary<String, String> GetParameterTableIndex();
        void PropagateParameters(int sourceId, int targetID);
        void PropagateParameters(int sourceId, String targetLinkType);
        void UpdateParameters(int parentID, string paramName, string value, eParamParentType paramParType, bool sendUpdates);
        void UpdateParameters(int parentID, string paramName, byte[] value, eParamParentType paramParType, bool sendUpdates);
        void UpdateParameters(int parentID, string paramName, string value, eParamParentType paramParType);
        void UpdateParameters(int parentID, string paramName, byte[] value, eParamParentType paramParType);

        Dictionary<String, Bitmap> GetIcons();
        Dictionary<String, Image> GetImages();
        bool ViewUpdateStatus { get; }
        void TurnViewUpdateOn();
        void TurnViewUpdateOn(bool componentUpdate, bool parameterUpdate);
        void TurnViewUpdateOff();
        void UpdateView();
        void ResetUpdateState();

        void RegisterForUpdate(IViewComponent component);
        void RegisterForUpdate(IViewComponent component, List<String> parameterCategories);
        void RegisterForUpdate(IViewComponentPanel component);

        void UnregisterForUpdate(IViewComponent component);
        void UnregisterForUpdate(IViewComponentPanel component);

        void InitializeDB();
        void ImportSql(String filename);
        void DropDatabase();

        void WriteOutputXml(String filename, XmlDocument toWrite);

    }//IControl
}//namespace AME.Controllers
