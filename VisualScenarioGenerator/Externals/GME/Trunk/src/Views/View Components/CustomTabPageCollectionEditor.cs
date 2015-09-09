using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Views.View_Components
{
    public class CustomTabPageCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public CustomTabPageCollectionEditor(System.Type type)
            : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(CustomTabPage);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(CustomTabPage) };
        }
    }
}
