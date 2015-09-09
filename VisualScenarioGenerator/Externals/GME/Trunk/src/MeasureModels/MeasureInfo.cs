using System;
using System.Collections.Generic;
using System.Text;

namespace AME.MeasureModels
{
    public class MeasureInfo : IMeasureInfo
    {
        private string m_sClassName;
        private string m_sDisplayName;
        private string m_sGraphType;

        #region Constructors

        public MeasureInfo(string className)
        {
            this.m_sClassName = className;
        }//constructor

        #endregion  //constructors


        #region Properties

        public string ClassName
        {
            get { return this.m_sClassName; }
        }//ClassName

        public string DisplayName
        {
            get { return this.m_sDisplayName; }
            set { this.m_sDisplayName = value; }
        }//DisplayName

        public string GraphType
        {
            get { return this.m_sGraphType; }
            set { this.m_sGraphType = value; }
        }//GraphType

        #endregion   //properties


    }//MeasureInfo class
}//namespace GME.MeasureModels
