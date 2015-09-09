using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyGenerator
{
    public interface Parser
    {
        String Parse();
    }

    public class ValueCategory
    {
        private String value, category;

        public String Category
        {
            get { return category; }
        }

        public String Value
        {
            get { return this.value; }
        }

        public ValueCategory(String newValue, String newCategory)
        {
            value = newValue;
            category = newCategory;
        }
    }
}
