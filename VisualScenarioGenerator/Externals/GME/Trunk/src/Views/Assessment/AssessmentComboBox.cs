using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AME.Views.Assessment
{
    public class AssessmentComboBox : ComboBox, IAssessmentView
    {
        private String saveSelection;
        private IAssessmentViewHelper myHelper;
        public IAssessmentViewHelper Helper
        {
            get
            {
                return myHelper;
            }
            set
            {
                myHelper = value;
            }
        }

        public String SavedSelected
        {
            get { return saveSelection; }
        }

        public void ClearSavedSelection()
        {
            saveSelection = null;
        }

        public AssessmentComboBox()
            : base()
        {
            myHelper = new DefaultViewHelper();
            this.DropDownStyle = ComboBoxStyle.DropDownList; // non editable list
            this.SelectionChangeCommitted += new EventHandler(AssessmentComboBox_SelectionChangeCommitted);
        }

        private void AssessmentComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            saveSelection = (String)this.SelectedItem;
        }

        public void Populate()
        {
            Items.Clear();

            List<XmlNode> items = myHelper.GetData();

            this.SelectedItem = null;

            if (items != null)
            {
                Object forSelection = null;
                foreach (XmlNode node in items)
                {
                    String nameAttr = node.Attributes["name"].Value;
                    this.Items.Add(nameAttr);
                    if (nameAttr.Equals(saveSelection))
                    {
                        forSelection = nameAttr;
                    }
                }

                if (forSelection != null)
                {
                    this.SelectedItem = forSelection;
                }
                else
                {
                    saveSelection = null;
                }
            }
        }
    }
}
