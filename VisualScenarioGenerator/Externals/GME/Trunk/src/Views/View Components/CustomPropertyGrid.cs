using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using AME.Controllers;
using AME.Model;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AME.Views.View_Components
{
    public class CustomPropertyGrid : PropertyGrid
    {
        private int myID;

        public int SelectedID
        {
            get { return myID; }
            set { myID = value; }
        }

        private IController myController;

        public IController Controller
        {
            get { return myController; }
            set { myController = value; }
        }

        protected eParamParentType m_selectedIDType = eParamParentType.Component;

        public eParamParentType SelectedIDType
        {
            get { return m_selectedIDType; }
            set { m_selectedIDType = value; }
        }

        private Dictionary<String, Object> classNameToObject;

        public CustomPropertyGrid()
            : base()
        {
            this.PropertyValueChanged += new PropertyValueChangedEventHandler(CustomPropertyGrid_PropertyValueChanged);
            classNameToObject = new Dictionary<String, Object>();
        }

        private void UpdateCollection(String paramName, Object value)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, value);
            byte[] arrayData = memoryStream.ToArray();

            try
            {   // category.name
                myController.UpdateParameters(SelectedID, paramName, arrayData, m_selectedIDType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                if (myController.ViewUpdateStatus == false)
                {
                    myController.TurnViewUpdateOn(); // refresh
                }
                else
                {
                    this.UpdateViewComponent();
                }
            }
        }

        private void UpdateItem(String paramName, String value)
        {
            try
            {   // category.name
                myController.UpdateParameters(SelectedID,
                paramName,
                value,
                m_selectedIDType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                if (myController.ViewUpdateStatus == false)
                {
                    myController.TurnViewUpdateOn(); // refresh
                }
                else
                {
                    this.UpdateViewComponent();
                }
            }

        }

     
        private void CustomPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            String name, category;
            GridItem parent = e.ChangedItem;
            int level = 0;
            while (parent.Parent.GridItemType != GridItemType.Category)
            {
                parent = parent.Parent;
                level++;
            }

            name = parent.Label;
            category = parent.PropertyDescriptor.Category;
            String paramName;
            Object value;
            switch (level)
            {
                case 0:
                    paramName = category + SchemaConstants.ParameterDelimiter + name;
                    value = e.ChangedItem.Value;
                    if (e.ChangedItem.PropertyDescriptor.Converter is ArrayConverter)
                    {
                        e.ChangedItem.Expanded = true;
                        this.Refresh();

                        UpdateCollection(paramName, value);
                    }
                    else
                    {
                        String sValue = e.ChangedItem.PropertyDescriptor.Converter.ConvertToString(value);
                        UpdateItem(paramName, sValue);
                    }
                    break;
                case 1:
                    if (e.ChangedItem.Parent.PropertyDescriptor.Converter is ArrayConverter)
                    {
                        paramName = category + SchemaConstants.ParameterDelimiter + name;
                        e.ChangedItem.Parent.Expanded = true;
                        this.Refresh();
                        value = e.ChangedItem.Parent.Value;

                        UpdateCollection(paramName, value);
                    }
                    else if(e.ChangedItem.PropertyDescriptor.Converter is ArrayConverter)
                    {
                        String myName = e.ChangedItem.Label;
                        paramName = category + SchemaConstants.ParameterDelimiter +
                            name + SchemaConstants.FieldLeftDelimeter + myName + SchemaConstants.FieldRightDelimeter;
                        value = e.ChangedItem.Value;
                        e.ChangedItem.Expanded = true;
                        this.Refresh();

                        UpdateCollection(paramName, value);
                    }
                    else
                    {
                        String myName = e.ChangedItem.Label;
                        paramName = category + SchemaConstants.ParameterDelimiter +
                            name + SchemaConstants.FieldLeftDelimeter + myName + SchemaConstants.FieldRightDelimeter;
                        value = e.ChangedItem.Value;
                        String sValue = e.ChangedItem.PropertyDescriptor.Converter.ConvertToString(value);
                        UpdateItem(paramName, sValue);
                    }
                    break;
                case 2:
                    if (e.ChangedItem.Parent.PropertyDescriptor.Converter is ArrayConverter)
                    {
                        String myName = e.ChangedItem.Parent.Label;
                        paramName = category + SchemaConstants.ParameterDelimiter +
                            name + SchemaConstants.FieldLeftDelimeter + myName + SchemaConstants.FieldRightDelimeter;
                        e.ChangedItem.Parent.Expanded = true;
                        this.Refresh();
                        value = e.ChangedItem.Parent.Value;

                        UpdateCollection(paramName, value);
                    }
                    break;
                default:
                    break;
            }
        }

        private Assembly executingApp;
                            // entry assembly should be whatever application we ran to produce the config file class...
        private void LoadAssemblyIfNeeded()
        {
            if (executingApp == null)
            {
                //String assemblyName = myController.GetParametersAssemblyName();

                //AssemblyName an = new AssemblyName();
                //an.Name = assemblyName;

                try
                {
                    // entry assembly should be whatever application we ran to produce the config file class...
                    executingApp = Assembly.GetEntryAssembly(); // Assembly.Load(assemblyName);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("DEBUG: Could not load assembly");
                    System.Console.WriteLine(e.InnerException);
                    System.Console.WriteLine(e.Message);
                    System.Console.WriteLine(e.StackTrace);
                }
            }
        }

        private Boolean blank = false;

        public Boolean Blank
        {
            get { return blank; }
        }

        private String previousClassType = "";

        public void UpdateViewComponent()
        {
            LoadAssemblyIfNeeded();

            bool classChange = false;
            String classType = "";

            if (myController == null || this.SelectedID < 0)
            {
                this.SelectedObject = new Object();
                previousClassType = "";
                blank = true;
                return;
            }

            String fromType = "";
            String toType = "";
            String linkType = "";

            if (SelectedIDType == eParamParentType.Component)
            {
                IXPathNavigable doc = this.Controller.GetComponent(this.SelectedID);
                XPathNavigator navigator = doc.CreateNavigator().SelectSingleNode("/Components/Component");

                if (navigator == null)
                {
                    this.SelectedObject = new Object();
                    previousClassType = "";
                    blank = true;
                    return;
                }

                classType = navigator.GetAttribute("Type", navigator.NamespaceURI);
            }
            else if (SelectedIDType == eParamParentType.Link)
            {
                XPathNavigator linkDocumentNav = Controller.GetLink(this.SelectedID, true).CreateNavigator();
                XPathNavigator link = linkDocumentNav.SelectSingleNode("/Links/Link");

                if (link != null)
                {
                    linkType = link.GetAttribute(XmlSchemaConstants.Display.Link.Type, link.NamespaceURI);

                    fromType = link.GetAttribute(XmlSchemaConstants.Display.Link.FromType, link.NamespaceURI);

                    toType = link.GetAttribute(XmlSchemaConstants.Display.Link.ToType, link.NamespaceURI);

                    classType = this.Controller.GetBaseLinkType(linkType) + fromType + toType;
                }
                else
                {
                    this.SelectedObject = new Object();
                    previousClassType = "";
                    blank = true;
                    return;
                }
            }

            blank = false;

            // did we change classes?
            if (!classType.Equals(previousClassType))
            {
                classChange = true;
            }
            else
            {
                classChange = false;
            }

            previousClassType = classType;

            if (this.SelectedObject == null || classChange)
            {
                Object result = LoadClass(classType, "");
                if (result == null)
                {
                    System.Console.WriteLine("ERROR: Could not find assembly!!!");
                    return;
                }
                else
                {
                    this.SelectedObject = result;
                }
            }

            if (SelectedIDType == eParamParentType.Component)
            {
                UpdateFromXML(classType);
            }
            else if (SelectedIDType == eParamParentType.Link)
            {
                UpdateFromXML(linkType, fromType, toType, classType);
            }
        }

        private Object LoadClass(String className, String structName)
        {
            if (executingApp == null)
            {
                return null;
            }

            try
            {
                if (classNameToObject.ContainsKey(className))
                {
                    if (structName.Length > 0)
                    {
                        Object classObj = classNameToObject[className];
                        FieldInfo[] fields = classObj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        FieldInfo childField;
                        for (int i = 0; i < fields.Length; i++)
                        {
                            childField = fields[i];
                            if (childField.FieldType.Name == structName)
                            {
                                return childField.GetValue(classObj);
                            }
                        }
                    }
                    else
                    {
                        return classNameToObject[className];
                    }
                }
                else
                {
                    Object propertyObject = executingApp.CreateInstance("Config.Parameters." + className);

                    if (propertyObject != null)
                    {
                        classNameToObject[className] = propertyObject;
                        return propertyObject;
                    }
                }
                return new Object();
            }
            catch (TargetInvocationException e)
            {
                System.Console.WriteLine("DEBUG:  Assembly loaded, but could not run.  Check XML, (particularly className, value settings)");
                System.Console.WriteLine(e.InnerException);
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
                return new Object();
            }
        }

        private void UpdateFromXML(String classType)
        {
            XPathNavigator nav = myController.GetParametersForComponent(SelectedID).CreateNavigator();
            ProcessNav(nav, eParamParentType.Component, classType);
        }
        private void UpdateFromXML(String linkType, String fromType, String toType, String classType)
        {
            XPathNavigator nav = myController.GetParametersForLink(SelectedID, linkType, fromType, toType).CreateNavigator();
            ProcessNav(nav, eParamParentType.Link, classType);
        }

        private void ProcessNav(XPathNavigator nav, eParamParentType parentType, String classType)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(SelectedObject);

            XPathNodeIterator allComplex = null;

            if (parentType == eParamParentType.Component)
            {
                allComplex = nav.Select("ComponentParameters/Parameter[@type='" + ConfigFileConstants.complexType + "']");
            }
            else if (parentType == eParamParentType.Link)
            {
                allComplex = nav.Select("LinkParameters/Parameter[@type='" + ConfigFileConstants.complexType + "']");
            }

            while (allComplex.MoveNext())
            {
                String category = allComplex.Current.GetAttribute(ConfigFileConstants.category, allComplex.Current.NamespaceURI);
                XPathNodeIterator complexChildren = allComplex.Current.Select("Parameter");

                while (complexChildren.MoveNext())
                {
                    String displayName = complexChildren.Current.GetAttribute(ConfigFileConstants.displayedName, complexChildren.Current.NamespaceURI);
                    String value = complexChildren.Current.GetAttribute(ConfigFileConstants.Value, complexChildren.Current.NamespaceURI);

                    foreach (PropertyDescriptor thisProperty in properties)
                    {
                        if (thisProperty.DisplayName == displayName && thisProperty.Category == category)
                        {
                            UpdateValue(thisProperty, complexChildren.Current, value, this.SelectedObject);

                            // set browsable via reflection
                            String browsable = complexChildren.Current.GetAttribute(ConfigFileConstants.browsable, complexChildren.Current.NamespaceURI);
                            if (browsable != null && browsable.Length > 0)
                            {
                                bool stringToBool = Boolean.Parse(browsable);
   
                                try
                                {
                                    FieldInfo fld;
                                    BrowsableAttribute attr = (BrowsableAttribute)thisProperty.Attributes[BrowsableAttribute.Default.GetType()];

                                    Type attrType = attr.GetType();
                                    fld = attrType.GetField(ConfigFileConstants.browsable, BindingFlags.Instance | BindingFlags.NonPublic);
                                    fld.SetValue(attr, stringToBool);
                                }
                                catch (Exception Ex)
                                {
                                    throw Ex;
                                }
                            }

                            XPathNodeIterator structChildren = complexChildren.Current.Select("Parameters/Parameter");
                            if (structChildren.Count > 0)
                            {
                                Object structObject = LoadClass(classType, displayName);
                                while (structChildren.MoveNext())
                                {
                                    String name = structChildren.Current.GetAttribute(ConfigFileConstants.Name, structChildren.Current.NamespaceURI);
                                    value = structChildren.Current.GetAttribute(ConfigFileConstants.Value, structChildren.Current.NamespaceURI);

                                    foreach (PropertyDescriptor childProperty in thisProperty.GetChildProperties())
                                    {
                                        if (childProperty.DisplayName == name)
                                        {
                                            UpdateValue(childProperty, structChildren.Current, value, structObject);
                                            break;
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            this.Refresh();
        }

        private void UpdateValue(PropertyDescriptor thisProperty, XPathNavigator current, String value, Object onObject)
        {
            if (!(thisProperty.Converter is ArrayConverter))
            {
                // set value
                if (value.Length > 0)
                {
                    if (thisProperty.Converter is EnumConverter) // enums are a special case
                    {
                        StringEnum stringEnum = new StringEnum(thisProperty.PropertyType);
                        Boolean pass = stringEnum.IsStringDefined(value);
                        if (!pass) // normal case, no string value
                        {
                            thisProperty.SetValue(onObject,
                                thisProperty.Converter.ConvertFromString(value));
                        }
                        else // string value, reverse to the enum value
                        {
                            thisProperty.SetValue(onObject, thisProperty.Converter.ConvertFromString(stringEnum.GetEnumValue(value))); 
                        }
                    }
                    else // everyone else
                    {
                        thisProperty.SetValue(onObject,
                            thisProperty.Converter.ConvertFromString(value));
                    }
                }
                else
                {
                    if (thisProperty.PropertyType == typeof(String)) // replace with empty string
                    {
                        thisProperty.SetValue(onObject,
                            thisProperty.Converter.ConvertFromString(String.Empty));
                    }
                }
            }
            else
            {
                if (thisProperty.Converter is ArrayConverter)
                {
                    // select collection children
                    XPathNodeIterator collectionChildren = current.Select("Parameter");
                    int index = 0;
                    Type elementType = thisProperty.PropertyType.GetElementType();
                    Array newValue = Array.CreateInstance(elementType, collectionChildren.Count);
                    foreach (XPathNavigator collectionChild in collectionChildren)
                    {
                        String xmlValue = collectionChild.GetAttribute(ConfigFileConstants.Value, "");
                        Object result = Convert.ChangeType(xmlValue, elementType);
                        newValue.SetValue(result, index);
                        index++;
                    }
                    thisProperty.SetValue(onObject, newValue);
                }
            }
        }
    }
}
         
        

