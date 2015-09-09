/*
 * Class            : ComponentOptions
 * File             : ComponentOptions.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Used as a package of options by view to read data from controller. Controller makes
 * decision based on this data package, which data to return or not or in what format.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using AME.Model;
using AME.Views.View_Components;
using AME.Controllers.Base.DataStructures;

#endregion  //namespaces

namespace AME.Controllers
{
    public interface IComponentOnlyOptions
    {
        bool CompParams { get; set; }
        bool LinkParams { get; set; }
        bool ClassInstanceInfo { get; set; }
        bool SubclassInstanceInfo { get; set; }
        bool InstanceUseClassName { get; set; }
    }

    public interface IChildrenOptions
    {
        uint? LevelDown { get; set; }
    }

    public interface IComponentOptions : IComponentOnlyOptions, IChildrenOptions
    {}


    public class ComponentOptions : IComponentOptions
    {
        private uint? _iLevelDown;
        private bool _bCompParams;
        private bool _bLinkParams;
        private bool _bClassInstanceInfo;//currently depends on _bInstanceUseClassName
        private bool _bSubclassInstanceInfo;
        private bool _bInstanceUseClassName;
        private List<Include> includes;
        #region Constructors

        #region Public Constructors

        public ComponentOptions()
        {
            this.Init();
        }//constructor

        #endregion  //public constructors

        private void Init()
        {
            this._iLevelDown = null;
            this._bCompParams = false;
            this._bLinkParams = false;
            this._bClassInstanceInfo = false;
            this._bSubclassInstanceInfo = false;
            this._bInstanceUseClassName = false;
        }//Init

        #endregion  //constructors

        #region Properties

        #region Public Properties

        public List<Include> Includes
        {
            get { return this.includes; }
            set { this.includes = value; }
        }//Includes

        public uint? LevelDown
        {
            get { return this._iLevelDown; }
            set { this._iLevelDown = value; }
        }//LevelDown

        public bool CompParams
        {
            get { return this._bCompParams; }
            set { this._bCompParams = value; }
        }//CompParams

        public bool LinkParams
        {
            get { return this._bLinkParams; }
            set { this._bLinkParams = value; }
        }//LinkParams

        public bool ClassInstanceInfo
        {
            get { return this._bClassInstanceInfo; }
            set { this._bClassInstanceInfo = value; }
        }//ClassInstanceInfo

        public bool SubclassInstanceInfo
        {
            get { return this._bSubclassInstanceInfo; }
            set { this._bSubclassInstanceInfo = value; }
        }//SubclassInstanceInfo

        public bool InstanceUseClassName
        {
            get { return this._bInstanceUseClassName; }
            set { this._bInstanceUseClassName = value; }
        }//InstanceUseClassName

        #endregion  //public properties

        #endregion  //properties

        #region Methods

        #region Public Methods

        public ComponentOptions Clone()
        {
            ComponentOptions clonedObj = new ComponentOptions();

            if (this.LevelDown.HasValue)
            {
                clonedObj.LevelDown = this.LevelDown.Value;
            }

            clonedObj.CompParams = this.CompParams;
            clonedObj.LinkParams = this.LinkParams;
            clonedObj.ClassInstanceInfo = this.ClassInstanceInfo;
            clonedObj.SubclassInstanceInfo = this.SubclassInstanceInfo;
            clonedObj.InstanceUseClassName = this.InstanceUseClassName;

            return clonedObj;
        }//Clone

        #endregion  //public methods

        #endregion  //methods

    }//ComponentOptions class
}//namespace AME.Controllers
