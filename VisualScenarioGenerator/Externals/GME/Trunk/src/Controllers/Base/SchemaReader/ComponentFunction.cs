/*
 * Class            : ComponentFunction
 * File             : ComponentFunction.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Represents component function in schema.
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
    public class ComponentFunction
    {
        private string m_sName;
        private string m_sAction;
        private bool m_bVisible;

        #region Constructors

        #region Public constructors

        public ComponentFunction()
        {
            this.Init();
        }//constructor

        public ComponentFunction(string name, string action, bool visible) : this()
        {
            this.m_sName = name;
            this.m_sAction = action;
            this.m_bVisible = visible;
        }//constructor

        private void Init()
        {
            this.m_sName = string.Empty;
            this.m_sAction = string.Empty;
            this.m_bVisible = false;
        }//Init

        #endregion  //public constructors

        #endregion  //constructors

        #region Properties

        #region Public properties

        public string Name
        {
            get { return this.m_sName; }
            set { this.m_sName = value; }
        }//Name

        public string Action
        {
            get { return this.m_sAction; }
            set { this.m_sAction = value; }
        }//Action

        public bool Visible
        {
            get { return this.m_bVisible; }
            set { this.m_bVisible = value; }
        }//Visible

        #endregion  //public properties

        #endregion  //properties

    }//ComponentFunction class
}//namespace AME.Controllers
