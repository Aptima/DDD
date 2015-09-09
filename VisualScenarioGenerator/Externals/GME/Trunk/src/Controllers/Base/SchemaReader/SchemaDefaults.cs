/*
 * Class            : SchemaDefaults
 * File             : SchemaDefaults.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Contains the default values in the schema file which are not part of XML file.
 * It works as a reader for functions from schema.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using AME.Model;
using System.Xml;

#endregion  //Namespaces

namespace AME.Controllers
{
    public class SchemaDefaults
    {
        private Dictionary<int, Dictionary<string, ComponentFunction>> m_compFuncs;
        private SchemaXmlParser m_schemaXmlParser;

        #region Constructors

        #region Public constructors

        public SchemaDefaults()
        {
            this.Init();
        }//constructor

        private void Init()
        {
            this.m_compFuncs = new Dictionary<int, Dictionary<string, ComponentFunction>>();
            this.m_schemaXmlParser = new SchemaXmlParser();
        }//Init

        #endregion  //public constructors

        #endregion  //constructors

        #region Properties

        #region Public properties

        public Dictionary<int, Dictionary<string, ComponentFunction>> ComponentFunctions
        {
            get { return this.m_compFuncs; }
            set { this.m_compFuncs = value; }
        }//ComponentFunctions

        #endregion  //public properties

        #endregion  //properties

        #region Methods

        #region Public methods

        public void AddComponentFunctions(int comID, string name, string value)
        {
            if (!this.m_compFuncs.ContainsKey(comID))
            {
                this.m_compFuncs.Add(comID, new Dictionary<string, ComponentFunction>());
            }

            List<ComponentFunction> lCompFuncs = this.m_schemaXmlParser.ParseComponentFunction(name, value);
            if (lCompFuncs != null)
            {
                foreach (ComponentFunction compFunc in lCompFuncs)
                {
                    if (!this.m_compFuncs[comID].ContainsKey(compFunc.Name))
                    {
                        this.m_compFuncs[comID].Add(compFunc.Name, compFunc);
                    }
                }//foreach function
            }

        }//AddComponentFunctions

        #endregion  //public methods

        #endregion  //methods

    }//SchemaDefaults class
}//namespace AME.Controllers
