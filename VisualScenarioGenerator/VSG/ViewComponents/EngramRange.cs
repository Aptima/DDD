using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml;
using System.Xml.XPath;
using VSG.Controllers;
using AME.Views.View_Components;
namespace VSG.ViewComponents
{
    public partial class EngramRange : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _controller;
        private int displayID = -1;
        private Boolean updating = false;
        private Boolean populating = false;
        private Boolean allowCheckedChanged = false;
        private Boolean allowSelectedIndexChanged = true;

        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                allowCheckedChanged = true;
                displayID = value;
                includeTextBox.ComponentId = value;
                excludeTextBox.ComponentId = value;
                if (_controller != null)
                {
                    customLinkBox1.DisplayRootId = _controller.ScenarioId;
                    customLinkBox1.ConnectFromId = displayID;
                    customLinkBox1.ConnectRootId = displayID;
                }

                includeTextBox.ParameterCategory = "EngramRange";
                includeTextBox.ParameterName = "Engram Range Include";
                excludeTextBox.ParameterCategory = "EngramRange";
                excludeTextBox.ParameterName = "Engram Range Exclude";
                customParameterTextBoxCompareValue.ComponentId = displayID;
                eventIDEngramUnit.ParentID = displayID;
                eventIDEngramUnit.DisplayID = displayID;
                customCheckBoxHasUnit.ComponentId = displayID;

                checkEngramRange();

                string selection = GetRangeType();
                allowCheckedChanged = false;
                radioButtonInclude.Checked = false;
                radioButtonExclude.Checked = false;
                radioButtonCompare.Checked = false;
                allowCheckedChanged = true;
                if (selection == "Include")
                {
                    radioButtonInclude.Checked = true;
                }
                else if (selection == "Exclude")
                {
                    radioButtonExclude.Checked = true;
                }
                else
                {
                    radioButtonCompare.Checked = true;
                }
            }
        }

        public EngramRange()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                allowCheckedChanged = true;
                _controller = (VSGController)value;
                customLinkBox1.Controller = value;
                includeTextBox.Controller = value;
                excludeTextBox.Controller = value;
                customParameterTextBoxCompareValue.Controller = value;
                eventIDEngramUnit.Controller = value;
                customCheckBoxHasUnit.Controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            if (!updating)
            {
                if (useEngramRangeCheckbox.Checked)
                {
                    updating = true;
                    allowCheckedChanged = false;
                    customLinkBox1.UpdateViewComponent();
                    includeTextBox.UpdateViewComponent();
                    excludeTextBox.UpdateViewComponent();
                    customParameterTextBoxCompareValue.UpdateViewComponent();
                    UpdateInequality();

                    eventIDEngramUnit.UpdateViewComponent();
                    customCheckBoxHasUnit.UpdateViewComponent();

                    eventIDEngramUnit.Enabled = customCheckBoxHasUnit.Checked;

                    includeTextBox.Enabled = radioButtonInclude.Checked;//true;
                    excludeTextBox.Enabled = radioButtonExclude.Checked;//true;
                    comboBoxCompareInequality.Enabled = radioButtonCompare.Checked;//
                    customParameterTextBoxCompareValue.Enabled = radioButtonCompare.Checked;
                    allowCheckedChanged = true;
                    updating = false;
                }
            }
        }

        private void ClearViewComponents()
        {
            AME.Views.View_Components.DrawingUtility.SuspendDrawing(this);
            customLinkBox1.UpdateViewComponent(); // Update before deleting...
            _controller.TurnViewUpdateOff();
            customLinkBox1.DeleteAllLinks();
            includeTextBox.Text = "";
            excludeTextBox.Text = "";
            includeTextBox.SetParameterValue();
            excludeTextBox.SetParameterValue();
            customParameterTextBoxCompareValue.SetParameterValue();
            SetInequality();

            customCheckBoxHasUnit.Checked = false;
            customCheckBoxHasUnit.UpdateViewComponent();

            useEngramRangeCheckbox.Checked = false;
            groupBox1.Enabled = false;
            includeTextBox.Enabled = false;
            excludeTextBox.Enabled = false;
            AME.Views.View_Components.DrawingUtility.ResumeDrawing(this);
            _controller.TurnViewUpdateOn(false, false);    // shouldn't need to update here - the values are set to "" and disabled 
        }

        #endregion

        private string GetRangeType()
        {
            if (displayID > 0 && _controller != null)
            {
                IXPathNavigable iNav = _controller.GetParametersForComponent(displayID);
                XPathNavigator pathNav = iNav.CreateNavigator();
                return pathNav.SelectSingleNode("/ComponentParameters/Parameter[@category='EngramRange']/Parameter[@displayedName='Selected Engram Type']").GetAttribute("value", pathNav.NamespaceURI);
            }

            return string.Empty;
        }

        private void SetRangeType(string type)
        {
            if (displayID > 0 && _controller != null)
            {
                _controller.UpdateParameters(displayID, "EngramRange.Selected Engram Type", type, eParamParentType.Component);
            }

        }
        private void checkEngramRange()
        {
            if (displayID >= 0)
            {
                updating = true;
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.LevelDown = 1;
                IXPathNavigable iNav = _controller.GetComponentAndChildren(displayID, customLinkBox1.ConnectLinkType, compOptions);
                XPathNavigator nav = iNav.CreateNavigator();
                if (nav.SelectSingleNode("/Components/Component/Component[@Type='Engram']") != null)
                {
                    useEngramRangeCheckbox.Checked = true;

                    groupBox1.Enabled = true;

                    bool storeUpdating = updating;

                    updating = false;

                    UpdateViewComponent();

                    updating = storeUpdating;

                }
                else
                {
                    ClearViewComponents();
                }
                updating = false;
            }
        }



        private void useEngramRangeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                if (!useEngramRangeCheckbox.Checked)
                {
                    ClearViewComponents();
                }
                else
                {
                    bool storeUpdating = updating;

                    updating = false;

                    groupBox1.Enabled = true;
                    UpdateViewComponent();

                    updating = storeUpdating;
                }
            }
        }



        private void excludeTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!updating)
            //{
            //    if (includeTextBox.Text != String.Empty)
            //    {
            //        updating = true;
            //        includeTextBox.Text = String.Empty;

            //        includeTextBox.SetParameterValue();
            //        updating = false;
            //    }
            //}
        }

        private void includeTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!updating)
            //{
            //    if (excludeTextBox.Text != String.Empty)
            //    {
            //        updating = true;
            //        excludeTextBox.Text = String.Empty;

            //        excludeTextBox.SetParameterValue();
            //        updating = false;
            //    }
            //}
        }

        private void EngramRangeRadioButtonChecked(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked && allowCheckedChanged)
            {
                switch (((RadioButton)sender).Name)
                {
                    case "radioButtonCompare":
                        CompareSelected();
                        break;
                    case "radioButtonInclude":
                        IncludeSelected();
                        break;
                    case "radioButtonExclude":
                        ExcludeSelected();
                        break;
                    default:
                        break;
                }
            }
        }

        private void CompareSelected()
        {
            ClearIncludeValue();
            ClearExcludeValue();
            comboBoxCompareInequality.Enabled = true;
            customParameterTextBoxCompareValue.Enabled = true;
            UpdateInequality();
            SetRangeType("Compare");
            customParameterTextBoxCompareValue.UpdateViewComponent();
        }

        private void IncludeSelected()
        {
            ClearCompareValue();
            ClearExcludeValue();
            includeTextBox.Enabled = true;
            SetRangeType("Include");
            includeTextBox.UpdateViewComponent();
        }

        private void ExcludeSelected()
        {
            ClearCompareValue();
            ClearIncludeValue();
            excludeTextBox.Enabled = true;
            SetRangeType("Exclude");
            excludeTextBox.UpdateViewComponent();
        }

        private void UpdateInequality()
        {
            if (_controller == null)
                return;

            populating = true;

            IXPathNavigable iNavParent = _controller.GetParametersForComponent(displayID);
            XPathNavigator navParent = iNavParent.CreateNavigator();
            XPathNavigator InequalityParameter = navParent.SelectSingleNode("ComponentParameters/Parameter[@category='EngramRange']/Parameter[@displayedName='Engram Compare Inequality']");//.GetAttribute("value", navSensor.NamespaceURI);// navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Attribute']");
            string ineq = "EQ";
            if (InequalityParameter != null)
            {
                ineq = InequalityParameter.GetAttribute("value", navParent.NamespaceURI);
            }
            ineq = ConvertStringToInequality(ineq);
            ////get enums
            for (int ind = 0; ind < comboBoxCompareInequality.Items.Count; ind++)
            {
                if (comboBoxCompareInequality.Items[ind].ToString() == ineq)
                {
                    allowSelectedIndexChanged = false;
                    comboBoxCompareInequality.SelectedIndex = ind;
                    allowSelectedIndexChanged = true;
                }
            }

            populating = false;
        }

        private void SetInequality()
        {

            if (displayID > 0 && _controller != null)
            {
                string selected = comboBoxCompareInequality.Text;
                _controller.UpdateParameters(displayID, "EngramRange.Engram Compare Inequality", ConvertInequalityToString(selected), eParamParentType.Component);
            }
        }

        private void ClearCompareValue()
        {
            if (comboBoxCompareInequality.Items.Count > 0)
            {
                comboBoxCompareInequality.SelectedIndex = 0;
                comboBoxCompareInequality.Enabled = false;
                UpdateInequality();
            }

            customParameterTextBoxCompareValue.Text = string.Empty;
            customParameterTextBoxCompareValue.Enabled = false;
            customParameterTextBoxCompareValue.SetParameterValue();
        }

        private void ClearIncludeValue()
        {
            includeTextBox.Text = string.Empty;
            includeTextBox.Enabled = false;
            includeTextBox.SetParameterValue();
        }

        private void ClearExcludeValue()
        {
            excludeTextBox.Text = string.Empty;
            excludeTextBox.Enabled = false;
            excludeTextBox.SetParameterValue();
        }

        private void comboBoxCompareInequality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allowSelectedIndexChanged)
            {
                SetInequality();
            }
        }
        /// <summary>
        /// Gets "==" , returns "EQ"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ConvertInequalityToString(string inequality)
        {
            switch (inequality)
            {
                case ">":
                    return "GT";
                    break;

                case ">=":
                    return "GE";
                    break;

                case "<":
                    return "LT";
                    break;

                case "<=":
                    return "LE";
                    break;

                case "==":
                    return "EQ";
                    break;

                default:
                    break;

            }

            return "EQ";
        }
        /// <summary>
        /// Gets "EQ", returns "=="
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ConvertStringToInequality(string str)
        {
            switch (str)
            {
                case "GT":
                    return ">";
                    break;

                case "GE":
                    return ">=";
                    break;

                case "LT":
                    return "<";
                    break;

                case "LE":
                    return "<=";
                    break;

                case "EQ":
                    return "==";
                    break;

                default:
                    break;

            }

            return "==";
        }

        private void checkBoxHasUnit_CheckedChanged(object sender, EventArgs e)
        {
            if (populating || updating)
                return;
           
            eventIDEngramUnit.UpdateViewComponent();
            eventIDEngramUnit.Enabled = ((CheckBox)sender).Checked;
        }
    }
}
