using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/**
 * A dialog box/form to prompt the user for a mission name.
 * Save's the user's input if they click ok.
 * 
 */

namespace Forms
{
    public partial class InputForm : Form
    {
        private bool m_alphaNumericTop = true, m_alphaNumericBottom = true;

        public bool AlphaNumericTop
        {
            get { return m_alphaNumericTop; }
            set { m_alphaNumericTop = value; }
        }

        public bool AlphaNumericBottom
        {
            get { return m_alphaNumericBottom; }
            set { m_alphaNumericBottom = value; }
        }

        public InputForm(String functionName, String inputType)
        {
            InitializeComponent();
            this.Text = functionName;//"Create " + inputType;
            this.groupBox1.Text = "Enter " + inputType + " Information";
        }

        public string TopInputFieldValue
        {
            get { return top_input_textbox.Text; }
            set { top_input_textbox.Text = value; }
        }

        public string BottomInputFieldValue
        {
            get { return bottom_input_textbox.Text; }
            set { bottom_input_textbox.Text = value; }
        }

        public string ButtonAddText
        {
            get { return buttonAdd.Text; }
            set { buttonAdd.Text = value; }
        }

        public void HideDescriptionField()
        {
            this.bottom_input_textbox.Text = "";
            this.bottom_input_label.Visible = false;
            this.bottom_input_textbox.Visible = false;
            this.groupBox1.Size = new Size(297, 53);
            this.Size = new Size(326, 130);
            this.buttonAdd.Location = new Point(153, 74);
            this.buttonCancel.Location = new Point(234, 74);
        }

        private void buttonAdd_click(object sender, EventArgs e)
        {
            String topText = top_input_textbox.Text;
            String bottomText = bottom_input_textbox.Text;

            if (!topText.Equals(""))
            {
                bool inputIsOK = true;

                if (AlphaNumericTop)
                {
                    for (int i = 0; i < topText.Length; i++)
                    {
                        char test = topText[i];
                        if (Char.IsPunctuation(test) && test != '-' && test != '_')
                        {
                            inputIsOK = false;
                        }
                    }
                }

                if (AlphaNumericBottom)
                {
                    for (int i = 0; i < bottomText.Length; i++)
                    {
                        char test = bottomText[i];
                        if (Char.IsPunctuation(test) && test != '-' && test != '_')
                        {
                            inputIsOK = false;
                        }
                    }
                }

                if (inputIsOK) 
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Only decimal digits, alphabetic characters, hyphens, or underscores are supported in the name and description");
                }
            }
            else
            {
                MessageBox.Show("Please enter a name.");
            }
        }

        private void buttonCancel_click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void InputFormActivated(object sender, EventArgs e)
        {
            top_input_textbox.Focus(); // focus the text field so the user can type right away.
        }
    }
}
