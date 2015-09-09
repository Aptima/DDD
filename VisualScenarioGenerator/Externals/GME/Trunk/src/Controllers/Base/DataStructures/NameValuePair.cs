using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class NameValuePair : IComparable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string value;

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public NameValuePair(String p_name, String p_value)
        {
            name = p_name;
            value = p_value;
        }

        public int CompareTo(object newObj)
        {
            if (newObj is NameValuePair)
            {
                NameValuePair toCompare = (NameValuePair)newObj;

                return this.name.CompareTo(toCompare.name);
            }
            else
            {
                return -1;
            }
        }
    }
}
