using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class ParameterValidationInfo
    {
        private Type type;
        private String paramMin, paramMax;

        public String ParamMin
        {
            get { return paramMin; }
            set { paramMin = value; }
        }

        public String ParamMax
        {
            get { return paramMax; }
            set { paramMax = value; }
        }

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        public ParameterValidationInfo(Type p_Type, String p_ParamMin, String p_ParamMax)
        {
            type = p_Type;
            paramMin = p_ParamMin;
            paramMax = p_ParamMax;
        }
    }
}
