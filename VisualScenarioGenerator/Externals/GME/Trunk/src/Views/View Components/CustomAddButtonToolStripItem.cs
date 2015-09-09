using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using Forms;
using System.Drawing;
using AME.Controllers.Base.Data_Structures;

namespace AME.Views.View_Components
{
    public class CustomAddButtonToolStripItem : ToolStripButton, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController myController;

        private String inputPrompt, errorText, formName;

        public String ErrorText
        {
            get { return errorText; }
            set { errorText = value; }
        }

        public String InputPrompt
        {
            get { return inputPrompt; }
            set { inputPrompt = value; }
        }

        public String FormName
        {
            get { return formName; }
            set { formName = value; }
        }

        public CustomAddButtonToolStripItem()
            : base()
        {
            myHelper = new ViewComponentHelper(this);

            this.AutoSize = true;
            this.Click += AddClick;
        }

        private String m_LinkType;

        public string LinkType
        {
            get { return m_LinkType; }
            set { m_LinkType = value; }
        }

        private int m_RootID = -1;

        public int RootID
        {
            get { return m_RootID; }
            set { m_RootID = value; }
        }

        private int m_lastCreatedID = -1;

        public int LastCreatedID
        {
            get { return m_lastCreatedID; }
            set { m_lastCreatedID = value; }
        }

        private String type = "";

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        // Add button
        private void AddClick(object sender, EventArgs e)
        {
            if (Enabled)
            {
                CustomAddButtonToolStripItem callingButton = (CustomAddButtonToolStripItem)sender;

                // bring up a form for input
                InputForm tempInput = new InputForm(formName, inputPrompt); // create the dialog
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;
                tempInput.Location = new Point(x, y);

                DialogResult result = tempInput.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // add a root component (mission plan, organization, etc.)
                    try
                    {
                        ComponentAndLinkID clid = myController.AddComponent(this.RootID, this.RootID, type, tempInput.TopInputFieldValue, m_LinkType, tempInput.BottomInputFieldValue);
                        m_lastCreatedID = clid.ComponentID;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to add item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.AddClick(sender, e);
                    }
                }
                else if (result == DialogResult.Retry)
                {
                    MessageBox.Show(errorText);
                    this.AddClick(sender, e);
                }
                else
                {
                    m_lastCreatedID = -1;
                }
            }
            else
            {
                MessageBox.Show("Please select a Project first.  Go to File->Select/Create Project");
            }
        }

        #region ViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        public void UpdateViewComponent()
        {
            //
        }

        #endregion
    }
}
