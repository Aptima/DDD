using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;
using System.Reflection;
using System.Xml.XPath;
using AME.Model;

namespace AME.Views.View_Components.Editors
{
    public class SliderEditor : UITypeEditor
    {
        private static Dictionary<String, Int32> minimumValue = new Dictionary<String, Int32>();
        private static Dictionary<String, Int32> maximumValue = new Dictionary<String, Int32>();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = (IWindowsFormsEditorService)provider.GetService(
                typeof(IWindowsFormsEditorService));

            if (wfes != null)
            {
                SliderForm sliderForm = new SliderForm();
                // type+category+displayedName
                String key = context.PropertyDescriptor.ComponentType.Name + context.PropertyDescriptor.Category + context.PropertyDescriptor.DisplayName;
                bool maxOK = false, minOK = false;
                if (maximumValue.ContainsKey(key))
                {
                    sliderForm.trackBar.Maximum = maximumValue[key];
                    maxOK = true;
                }
                if (minimumValue.ContainsKey(key))
                {
                    sliderForm.trackBar.Minimum = minimumValue[key];
                    minOK = true;
                }

                if (!minOK || !maxOK)
                {
                    CustomPropertyGrid parentGrid;
                    PropertyInfo piOwnerGrid = context.GetType().GetProperty("OwnerGrid");
                    Object grid = piOwnerGrid.GetValue(context, null);
                    if ((grid != null) && (grid is CustomPropertyGrid))
                    {
                        parentGrid = (CustomPropertyGrid)grid;
                    }
                    else
                    {
                        parentGrid = null;
                    }
                    if (parentGrid != null)
                    {
                        // try and get the parameter info to check for constraints - to set min and max
                        IXPathNavigable iNav = parentGrid.Controller.GetParametersForComponent(parentGrid.SelectedID);
                        XPathNavigator nav = iNav.CreateNavigator();
                        XPathNodeIterator constraints = nav.Select("ComponentParameters/Parameter[@category='" + context.PropertyDescriptor.Category +
                            "']/Parameter[@displayedName='" + context.PropertyDescriptor.DisplayName + "']/Constraints/Constraint");

                        foreach (XPathNavigator constraint in constraints)
                        {
                            String constraintName = constraint.GetAttribute(ConfigFileConstants.constraintName, "");
                            String constraintValue = constraint.GetAttribute(ConfigFileConstants.constraintValue, "");
                            Int32 parse;
                            if (constraintName.Equals(ConfigFileConstants.maxConstraint))
                            {
                                parse = Int32.Parse(constraintValue);
                                sliderForm.trackBar.Maximum = parse;
                                maximumValue.Add(key, parse); // save so we don't have to reprocess this every time
                            }
                            else if (constraintName.Equals(ConfigFileConstants.minConstraint))
                            {
                                parse = Int32.Parse(constraintValue);
                                sliderForm.trackBar.Minimum = parse;
                                minimumValue.Add(key, parse);
                            }
                        }
                    }
                }

                sliderForm.trackBar.Value = (int)value;
                sliderForm.wfes = wfes;

                wfes.DropDownControl(sliderForm);

                value = sliderForm.trackBar.Value;
            }
            return value;
        }
    }
}
