/*
 * Classes          : Component
 *                    Component.Class
 * File             : Component.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Used in base Controller class to manage components differently based on
 * their eType like -
 *      Component
 *      Class
 *      Instance
 * and to figure out component's eType
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

#endregion  //Namespaces

namespace AME.Controllers
{

    public class Component
    {
        private static Parameter m_ComponentType;

        static Component()
        {
            m_ComponentType = new Parameter("Type", typeof(string));
        }//static constructor

        public static Parameter ComponentType
        {
            get { return m_ComponentType; }
        }//ComponentType

        public enum eComponentType
        {
            None,
            Component,
            Class,
            Subclass,
            Instance
        };//eComponentType

        public class Class
        {
            private static Parameter m_InstanceUseClassNameParameter;
            //private static Parameter m_SubclassUseClassNameParameter; removing this for now. -mw 8/28/07

            static Class()
            {
                m_InstanceUseClassNameParameter = new Parameter("Class.Instances Use Class Name", typeof(bool), Boolean.FalseString);
                //m_SubclassUseClassNameParameter = new Parameter("Class.Subclasses Use Class Name", typeof(bool), Boolean.FalseString);
            }

            public const string ClassInstanceLinkType = "ClassInstance";
            public const string ClassSubclassLinkType = "ClassSubclass";

            public static Parameter InstancesUseClassName
            {
                get { return m_InstanceUseClassNameParameter; }
            }//InstancesUseClassName

            //public static Parameter SubclassesUseClassName
            //{
            //    get { return m_SubclassUseClassNameParameter; }
            //}//SubclassesUseClassName

        }//Class class
    }//Component class
}//Controllers namespace