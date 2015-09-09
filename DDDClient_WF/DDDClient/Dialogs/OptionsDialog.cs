using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();


            LoadCurrentSettings();
            //get current values from DDD_Global.
            
        }

        private Dictionary<int, Color> _colorCodes = null;
        private Color GetNamedColorFromArgb(int argb)
        {
            if (_colorCodes == null)
            {
                _colorCodes = new Dictionary<int, Color>();
            }
            if (_colorCodes.ContainsKey(argb))
            {
                return _colorCodes[argb];
            }

            foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
            {
                Color c = Color.FromName(colorName);
                if (!c.IsSystemColor && c.ToArgb() == argb)
                {
                    _colorCodes.Add(argb, c);
                    return c;
                }
            }

            return Color.White; // white, if not found, which is bad.
        }

        public void LoadCurrentSettings()
        {
            PopulateRangeRingTab();
        }

        public void SaveDialogSettings()
        {
            SaveRangeRingTab();
        }

        private void SetSwatchColor(Panel colorPanel, int alpha, Color color)
        {
            colorPanel.BackColor = Color.FromArgb(alpha, color);
        }


        private void PopulateRangeRingTab()
        {
            //set sensor color
            Color sensorColor = GetNamedColorFromArgb(DDD_RangeRings.GetInstance().SensorRangeRings.RingColor);
            if (!sensorColor.IsNamedColor)
            {
                sensorColor = Color.FromArgb(sensorColor.R, sensorColor.G, sensorColor.B);
            }
             comboBoxSensorsColor.Text = sensorColor.Name; //will update swatch
            
            //set sensor opacity
            int opacityPercent = DDD_RangeRings.GetInstance().SensorRangeRings.GetOpacityAsPercentage(1);
            trackBarSensorsOpacity.Value = opacityPercent / 10; //will update swatch
           

            //set Capability color
            Color capabilityColor = GetNamedColorFromArgb(DDD_RangeRings.GetInstance().CapabilityRangeRings.RingColor);
            if (!capabilityColor.IsNamedColor)
            {
                capabilityColor = Color.FromArgb(capabilityColor.R, capabilityColor.G, capabilityColor.B);
            }
            comboBoxCapabilitiesColor.Text = capabilityColor.Name; //will update swatch

            //set Capability opacity
            opacityPercent = DDD_RangeRings.GetInstance().CapabilityRangeRings.GetOpacityAsPercentage(1);
            trackBarCapabilitiesOpacity.Value = opacityPercent / 10; //will update swatch

            //set Vulnerability color
            Color vulnerabilityColor = GetNamedColorFromArgb(DDD_RangeRings.GetInstance().VulnerabilityRangeRings.RingColor);
            if (!vulnerabilityColor.IsNamedColor)
            {
                vulnerabilityColor = Color.FromArgb(vulnerabilityColor.R, vulnerabilityColor.G, vulnerabilityColor.B);
            }
            comboBoxVulnerabilitiesColor.Text = vulnerabilityColor.Name; //will update swatch

            //set Vulnerability opacity
            opacityPercent = DDD_RangeRings.GetInstance().VulnerabilityRangeRings.GetOpacityAsPercentage(1);
            trackBarVulnerabilitiesOpacity.Value = opacityPercent / 10; //will update swatch


        }

        private string GetColorName(ComboBox control)
        { 
            string comboColor = control.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }

            return comboColor;
        }

        private void SaveRangeRingTab()
        {
            //saves current UI values to DDD_Global

            //save sensors
            string sensorColorName = GetColorName(comboBoxSensorsColor);
            int sensorOpacitySlider = trackBarSensorsOpacity.Value;
            float sensorOpacity = sensorOpacitySlider * 10f * 2.55f;
            DDD_RangeRings.GetInstance().SensorRangeRings.RingColor = Color.FromName(sensorColorName).ToArgb();
            DDD_RangeRings.GetInstance().SensorRangeRings.Opacity = Convert.ToInt32(Math.Round(sensorOpacity));
            
            
            //save capabilities
            string capabilityColorName = GetColorName(comboBoxCapabilitiesColor);
            int capabilityOpacitySlider = trackBarCapabilitiesOpacity.Value;
            float capabilityOpacity = capabilityOpacitySlider * 10f * 2.55f;
            DDD_RangeRings.GetInstance().CapabilityRangeRings.RingColor = Color.FromName(capabilityColorName).ToArgb();
            DDD_RangeRings.GetInstance().CapabilityRangeRings.Opacity = Convert.ToInt32(Math.Round(capabilityOpacity));
            
            //save vulnerabilities
            string vulnerabilityColorName = GetColorName(comboBoxVulnerabilitiesColor);
            int vulnerabilityOpacitySlider = trackBarVulnerabilitiesOpacity.Value;
            float vulnerabilityOpacity = vulnerabilityOpacitySlider * 10f * 2.55f;
            DDD_RangeRings.GetInstance().VulnerabilityRangeRings.RingColor = Color.FromName(vulnerabilityColorName).ToArgb();
            DDD_RangeRings.GetInstance().VulnerabilityRangeRings.Opacity = Convert.ToInt32(Math.Round(vulnerabilityOpacity));
        }

        private void UpdatedSensorsTrackbar()
        {
            labelSensorsOpacity.Text = String.Format("{0}%", trackBarSensorsOpacity.Value * 10);

            string comboColor = comboBoxSensorsColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelSensorsColorSwatch, Convert.ToInt32(Math.Round(trackBarSensorsOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }
        private void UpdatedSensorsComboBox()
        {
            string comboColor = comboBoxSensorsColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelSensorsColorSwatch, Convert.ToInt32(Math.Round(trackBarSensorsOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }

        private void UpdatedCapabilitiesTrackbar()
        {
            labelCapabilitiesOpacity.Text = String.Format("{0}%", trackBarCapabilitiesOpacity.Value * 10);

            string comboColor = comboBoxCapabilitiesColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelCapabilitiesColorSwatch, Convert.ToInt32(Math.Round(trackBarCapabilitiesOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }
        private void UpdatedCapabilitiesComboBox()
        {
            string comboColor = comboBoxCapabilitiesColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelCapabilitiesColorSwatch, Convert.ToInt32(Math.Round(trackBarCapabilitiesOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }

        private void UpdatedVulnerabilitiesTrackbar()
        {
            labelVulnerabilitiesOpacity.Text = String.Format("{0}%", trackBarVulnerabilitiesOpacity.Value * 10);

            string comboColor = comboBoxVulnerabilitiesColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelVulnerabilitiesColorSwatch, Convert.ToInt32(Math.Round(trackBarVulnerabilitiesOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }
        private void UpdatedVulnerabilitiesComboBox()
        {
            string comboColor = comboBoxVulnerabilitiesColor.Text;
            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            if (Color.FromName(comboColor) != Color.Empty)
            {
                SetSwatchColor(panelVulnerabilitiesColorSwatch, Convert.ToInt32(Math.Round(trackBarVulnerabilitiesOpacity.Value * 10 * 2.55)), Color.FromName(comboColor));
            }
        }

        private void trackBarValueChanged(object sender, EventArgs e)
        {
            string trackBarTag = ((TrackBar)sender).Tag.ToString();

            switch (trackBarTag)
            { 
                case "Sensors":
                    UpdatedSensorsTrackbar();
                    break;
                case "Capabilities":
                    UpdatedCapabilitiesTrackbar();
                    break;
                case "Vulnerabilities":
                    UpdatedVulnerabilitiesTrackbar();
                    break;
                default:
                    break;
            }

        }
        private void comboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string comboBoxTag = ((ComboBox)sender).Tag.ToString();

            switch (comboBoxTag)
            {
                case "Sensors":
                    UpdatedSensorsComboBox();
                    break;
                case "Capabilities":
                    UpdatedCapabilitiesComboBox();
                    break;
                case "Vulnerabilities":
                    UpdatedVulnerabilitiesComboBox();
                    break;
                default:
                    break;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public new DialogResult ShowDialog(IWin32Window _owner)
        {
            this.DialogResult = DialogResult.Cancel;
            return base.ShowDialog(_owner);
        }

        private void colorSwatchPanel_Click(object sender, EventArgs e)
        {
            string comboColor = "NONE";
            string senderName = ((Panel)sender).Name;
            ComboBox selected = null;

            switch(senderName)
            {
                case "panelSensorsColorSwatch":
                    selected = comboBoxSensorsColor;
                    break;
                case "panelCapabilitiesColorSwatch":
                    selected = comboBoxCapabilitiesColor;
                    break;
                case "panelVulnerabilitiesColorSwatch":
                    selected = comboBoxVulnerabilitiesColor;
                    break;
                default:
                     MessageBox.Show("Incorrect Panel selected");
                    return;
                    break;
            }
            comboColor = selected.Text;

            if (comboColor.Contains(" "))
            {
                comboColor = comboColor.Replace(" ", "");
            }
            ColorPalette cp = new ColorPalette(comboColor);
            if (cp.ShowDialog(this) == DialogResult.OK)
            { 
                //set color
                string newColor = cp.GetColor().Name;
                if (newColor.StartsWith("Light"))
                {
                    newColor = newColor.Insert(newColor.IndexOf('t') + 1, " ");
                }

                selected.Text = newColor; //should update swatch
            }

            cp.Dispose();
            cp = null;
        }
    }
}