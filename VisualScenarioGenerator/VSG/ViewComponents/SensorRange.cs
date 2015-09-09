using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;

using VSG.Controllers;
using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class SensorRange : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _vsgController;
        private int _sensorID = -1;
        private int _sensorRangeID = -1;
        private string _sensorRangeName = string.Empty;
        private Boolean populating = false;
        private Boolean updating = false;
        private Boolean _isCustomAttribute = false;

        public Boolean IsCustomAttribute
        {
            get
            {
                return _isCustomAttribute;
            }
            set
            {
                _isCustomAttribute = value;
            }
        }

        public int SensorID
        {
            get
            {
                return _sensorID;
            }
            set
            {
                _sensorID = value;
                //fillLevels();
            }
        }
        public int SensorRangeID
        {
            get
            {
                return _sensorRangeID;
            }
            set
            {
                _sensorRangeID = value;
                this.txtRange.ComponentId = value;
                this.txtSpread.ComponentId = value;
                this.txtXpos.ComponentId = value;
                this.txtYpos.ComponentId = value;
                this.txtZpos.ComponentId = value;
                UpdateViewComponent();
            }
        }
        public string SensorRangeName
        {
            get
            {
                return _sensorRangeName;
            }
            set
            {
                _sensorRangeName = value;
                this.customTabPage1.Description = value;
            }
        }

        public SensorRange()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        private void fillLevels()
        {
            populating = true;

            comboBox1.Items.Clear();
            comboBox1.Text = "<Select Level>";

            IXPathNavigable iNavSensor = _vsgController.GetParametersForComponent(_sensorID);
            XPathNavigator navSensor = iNavSensor.CreateNavigator();
            _isCustomAttribute = Convert.ToBoolean(navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Custom_Attribute_Sensor']").GetAttribute("value", navSensor.NamespaceURI));
            XPathNavigator sensorAttributeParameter;// = navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Attribute']");
            if (_isCustomAttribute)
            {
                sensorAttributeParameter = navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Custom_Attribute']");
            }
            else
            {
                sensorAttributeParameter = navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Attribute']");
            }

            if (sensorAttributeParameter != null)
            {
                String sensorAttribute = sensorAttributeParameter.GetAttribute("value", navSensor.NamespaceURI);
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.LevelDown = 4;
                compOptions.CompParams = true;
                IXPathNavigable iNavScenario = _vsgController.GetComponentAndChildren(_vsgController.ScenarioId, _vsgController.ConfigurationLinkType, compOptions);
                XPathNavigator navScenario = iNavScenario.CreateNavigator();
                XPathNodeIterator itNavEmitters = navScenario.Select("/Components/Component/Component[@Type='Species']/Component[@Type='State']/Component[@Type='Emitter']");
                while (itNavEmitters.MoveNext())
                {
                    XPathNavigator emitterAttributeParameter;
                    if (_isCustomAttribute)
                    {
                        emitterAttributeParameter = itNavEmitters.Current.SelectSingleNode("ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Custom_Attribute']");
                    }
                    else
                    {
                        emitterAttributeParameter = itNavEmitters.Current.SelectSingleNode("ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Attribute']");
                    }
                    if (emitterAttributeParameter != null)
                    {
                        String emitterAttribute = emitterAttributeParameter.GetAttribute("value", navScenario.NamespaceURI);

                        if (sensorAttribute.Equals(emitterAttribute))
                        {                            
                            XPathNodeIterator itNavLevels = itNavEmitters.Current.Select("Component[@Type='Level']");
                            while (itNavLevels.MoveNext())
                            {
                                String levelName = itNavLevels.Current.GetAttribute("Name", navScenario.NamespaceURI);
                                if (!comboBox1.Items.Contains(levelName))
                                    comboBox1.Items.Add(levelName);
                            }
                        }
                    }
                }
            }

            selectItem();

            populating = false;
        }

        private void selectItem()
        {
            if (comboBox1.Items.Count > 0)
            {
                String text = String.Empty;

                IXPathNavigable inav = _vsgController.GetParametersForComponent(_sensorRangeID);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode("ComponentParameters/Parameter[@category='SensorRange']/Parameter[@displayedName='Level']");
                if (node != null)
                {
                    text = node.GetAttribute("value", String.Empty);
                    if (!text.Equals(String.Empty))
                    {
                        Int32 index = this.comboBox1.FindStringExact(text);
                        if (index == -1) // No match so default to first item.
                            index = 0;
                        this.comboBox1.SelectedItem = comboBox1.Items[index];
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!populating & !updating)
            {
                try
                {
                    _vsgController.UpdateParameters(_sensorRangeID, "SensorRange.Level", this.comboBox1.SelectedItem.ToString(), eParamParentType.Component);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return _vsgController;
            }
            set
            {
                _vsgController = (VSGController)value;
                this.txtRange.Controller = _vsgController;
                this.txtSpread.Controller = _vsgController;
                this.txtXpos.Controller = _vsgController;
                this.txtYpos.Controller = _vsgController;
                this.txtZpos.Controller = _vsgController;
            }
        }

        public void UpdateViewComponent()
        {
            updating = true;
            fillLevels();
            this.txtRange.UpdateViewComponent();
            this.txtSpread.UpdateViewComponent();
            this.txtXpos.UpdateViewComponent();
            this.txtYpos.UpdateViewComponent();
            this.txtZpos.UpdateViewComponent();
            updating = false;
        }

        #endregion
    }
}
