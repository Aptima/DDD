/*
 * Class            : RootController
 * File             : RootController.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Inherits from Controller class and IRootController interface.
 * Its purpose is to create and get root components. Root components are little
 * different from other components because they do not have any parent. Root 
 * components are the topmost components. 
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using AME.Model;
using AME.Views.View_Components;

#endregion  //Namespaces

namespace AME.Controllers
{
    public class RootController :
        Controller
        , IRootController
    {

        #region Constructors

        public RootController(AME.Model.Model model, string configuration)
            : base(model, configuration)
        {
        }//constructor

        #endregion

        #region Public Members

        public int CreateRootComponent(string name, string description)
        {
            int rootID = this._CreateRootComponent(name, description);

            if (rootID != -1)
            {
                base.SendUpdateOfType(UpdateType.Component);
            }

            return rootID;
        }//CreateRootComponent

        public void UpdateRootName(int id, string value)
        {
            this._RenameRoot(id, value);
            base.SendUpdateOfType(UpdateType.Component);
        }//UpdateRootName

        public IXPathNavigable GetRootComponents(ComponentOptions compOptions)
        {
            return this._GetComponentsXmlDoc(compOptions).Clone();
        }//GetRootComponents

        #endregion

        #region Private Members

        private int _CreateRootComponent(string name, string description)
        {
            int rootID = -1;

            this._ValidateRootAdd(name);
            rootID = base._CreateComponent(base.RootComponentType, name, description);

            this._AddProgrammaticChildren(rootID, rootID, rootID, base.RootComponentType);

            return rootID;
        }//_CreateRootComponent

        private void _RenameRoot(int id, string name)
        {
            this._ValidateRootRename(id, name);
            base._UpdateComponentName(id, name);
        }//_RenameRoot

        private void _ValidateRootAdd(string name)
        {
            //getting parent xml node. No parent, using Configuration xml node as parent
            string sParentXPath = string.Format("//{0}", Root);  //base.Configuration
            //getting all of the roots
            XmlDocument doc = this._GetXmlDoc_RootValidation();

            base._ValidateAdd(doc, sParentXPath, base.RootComponentType,
                Component.eComponentType.Component, name);
        }//_ValidateRootAdd

        private void _ValidateRootRename(int id, string name)
        {
            //Verifying if Component is of RootType
            if (!base._IsValidRootID(id))
            {
                string sError = string.Format("Component {0} is not a Root component.", id);
                throw new System.Exception(sError);
            }

            XmlDocument doc = this._GetXmlDoc_RootValidation();
            base._ValidateRename(doc, id, name);
        }//_ValidateRename

        private XmlDocument _GetXmlDoc_RootValidation()
        {
            XmlElement configElement;
            XmlDocument doc = base._GetXmlDoc_Validation(out configElement, this._RootConfigSchemaFilePath);
            //get all of the components of type root and add them to configuration xml node
            DataTable rootComponents = base._GetComponentTable(base.RootComponentType);

            Dictionary<int, XmlElement> componentCache = new Dictionary<int,XmlElement>();
            Dictionary<String, String> typeToBaseCache = new Dictionary<String, String>();

            foreach (DataRow rootComponent in rootComponents.Rows)
            {
                XmlElement xmlElement = base._GetXmlTree_Validation(doc, rootComponent, null, true, componentCache,
                    typeToBaseCache, null, null, null);
                if (xmlElement != null)
                {
                    configElement.AppendChild(xmlElement);
                }
            }
            return doc;
        }//_GetXmlDoc_Validation

        private string _RootConfigSchemaFilePath
        {
            get
            {
                string sPath = string.Format(@"{0}.xsd", base.Configuration);
                return sPath;
            }
        }//ConfigSchemaFilePath

        private XmlDocument _GetComponentsXmlDoc(ComponentOptions compOptions)
        {
            //Getting validation document
            XmlDocument validationDoc = this._GetXmlDoc_RootValidation();
            //Reading default values from schema through validation document.
            SchemaDefaults schemaDefs = base._GetDefaultValuesFromSchema(validationDoc);

            XmlElement topNode;
            XmlDocument doc = base._GetComponentsXmlDoc(out topNode);

            //get all of the components of type root and add them to root
            DataTable rootComponents = base._GetComponentTable(base.RootComponentType);

            Dictionary<String, String> typeToBaseCache = new Dictionary<String, String>();
            Dictionary<String, XmlNode> typeToParametersXMLCache = new Dictionary<String, XmlNode>();
            Dictionary<String, List<DataRow>> parameterTableCache = null;

            if (useLoadingCaches)
            {
                parameterTableCache = loadingParameterCache[m_model];
            }
            else
            {
                parameterTableCache = GetParameterTableCache();
            }
 
            foreach (DataRow rootComponent in rootComponents.Rows)
            {
                XmlElement xmlElement = base._GetXmlTree(doc, rootComponent, compOptions, schemaDefs, typeToBaseCache,
                    typeToParametersXMLCache, parameterTableCache);
                if (xmlElement != null)
                {
                    topNode.AppendChild(xmlElement);
                }
            }

            return doc;
        }//_GetComponentsXmlDoc

        #endregion

    }//RootController class
}//namespace AME.Controllers 
