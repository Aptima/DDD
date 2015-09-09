using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using GME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class BoundedCustomParameterTextBox : CustomParameterTextBox
    {
        #region members
        private float _maxValue = 0.0F;
        private float _minValue = 0.0F;
        #endregion

        #region constructors
        public BoundedCustomParameterTextBox()
        {
            InitializeComponent();
        }

        public BoundedCustomParameterTextBox(float min, float max)
        {
           InitializeComponent();
           _maxValue = max;
           _minValue = min;
        }

        public BoundedCustomParameterTextBox(IContainer container, float min, float max)
        {
            container.Add(this);

            InitializeComponent();
            _maxValue = max;
            _minValue = min;
        }


        #endregion

        #region overrides
        protected override void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_maxValue > float.Parse(Text) && _minValue < float.Parse(Text))
                {
                    SetParameterValue();
                }
                else
                {
                    if (_maxValue < float.Parse(Text))
                    {
                        MessageBox.Show(String.Format("Your input exceeded the maximum value ({0}) for this input.", _maxValue, "Invalid Input"));
                    }
                    else if(_minValue > float.Parse(Text))
                    {
                        MessageBox.Show(String.Format("Your input exceeded the minimum value ({0}) for this input.", _minValue, "Invalid Input"));
                    }
                    UpdateViewComponent();
                }
            }

        }

        protected override void CustomTextBox_Leave(object sender, EventArgs e)
        {
            if (_maxValue > float.Parse(Text) && _minValue < float.Parse(Text))
            {
                SetParameterValue();
            }
            else
            {
                if (_maxValue < float.Parse(Text))
                {
                    MessageBox.Show(String.Format("Your input exceeded the maximum value ({0}) for this input.", _maxValue, "Invalid Input"));
                }
                else if (_minValue > float.Parse(Text))
                {
                    MessageBox.Show(String.Format("Your input exceeded the minimum value ({0}) for this input.", _minValue, "Invalid Input"));
                }
                UpdateViewComponent();
            }
        }

        #endregion
    }
}
