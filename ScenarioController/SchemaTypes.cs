using System;
using System.Collections.Generic;

namespace  DDD.ScenarioController
{
    /// <summary>
    /// These are the class definitions dfor the types found in the schema and scenario
    /// </summary>
    /// 
    /// <summary>
    ///The following classes take their definitions from the schema file
    /// </summary>
    ///  
 


    //  ----------------------- VectorType -----------------------

    public class VectorType 
    {
        private double x;
        public double X
        {
            get
            { return this.x; }
            set 
            { this.x = value; } 
        }
        private double y;
        public double Y
        {
            get
            { return this.y; }
            set
            { this.y = value;  } 
        }
        private double z;
        public double Z
        {
            get 
            { return this.z; }
            set
            { this.z = value; }
        }
        public VectorType(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public VectorType()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public void NewLocation(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    //  ----------------------- ParameterSettingType -----------------------
    public class ParameterSettingType
    {
        private string name;
        public string Name
        {
            get
            { return this.name; }
            set
            { this.name = Name; }
        }
        private Object setting;
        public Object Setting
        {
            get
            { return this.setting; }
            set
            { this.setting = value; }
        }
        public ParameterSettingType(string name, Object setting)
        {
            this.name = name;
            this.setting = setting;
        }
    }

}

