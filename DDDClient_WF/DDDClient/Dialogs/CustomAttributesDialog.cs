using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class CustomAttributesDialog : Form
    {
        private string _currentObjectID = string.Empty;
        private string _formHeader = "Custom Attributes of ";
        private string _noneSelected = "<NONE>";
        private Form _owner;

        public CustomAttributesDialog(Form parent)
        {
            _owner = parent;
            InitializeComponent();
            ResizeWindow();
        }

        private void CustomAttributesDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                try
                {
                    ((DDD_MainWinFormInterface)_owner).ToggleCustomAttributesToolStripVisibility(false);
                }
                catch
                { 
                    //nothing
                }
                e.Cancel = true;
            }
        }

        private void ClearDataGridView()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new NoParamDelegate(ClearDataGridView));
            }
            else
            {
                dataGridViewAttributes.Rows.Clear();
                ResizeWindow();
            }
        }
        private delegate void dgviewDelegate(string objectID, Dictionary<string, DataValue> attributePairs);
        public void SetDataGridView(string objectID, Dictionary<string, DataValue> attributePairs)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dgviewDelegate(SetDataGridView), objectID, attributePairs);
            }
            else
            {
                if (objectID != _currentObjectID)
                {
                    ClearDataGridView();
                    _currentObjectID = objectID;
                    UpdateFormHeader();
                    if (objectID == string.Empty)
                    {
                        return;
                    }
                }

                UpdateDataGrid(ProcessDataValueDictionary(attributePairs));
            }
        }

        private Dictionary<string, string> ProcessDataValueDictionary(Dictionary<string, DataValue> dict)
        {
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, DataValue> kvp in dict)
            {
                switch (kvp.Value.dataType)
                { 
                    case "IntegerType":
                    case "DoubleType":
                    case "StringType":
                    case "BooleanType":
                        returnDictionary.Add(kvp.Key, DataValueFactory.ToString(kvp.Value));
                        break;
                    default:
                        //handle case by case.  Location.X, Location.Y probably best way to add multiple entry DVs
                        break;
                }
            }

            return returnDictionary;
        }

        private delegate void UpdateGridDelegate(Dictionary<string, string> attributePairs);
        private void UpdateDataGrid(Dictionary<string, string> attributePairs)
        {
            if (!InvokeRequired)
            {
                string attributeName;

                foreach (DataGridViewRow dr in dataGridViewAttributes.Rows)
                {
                    attributeName = dr.Cells["attribute"].Value.ToString();

                    if (attributePairs.ContainsKey(attributeName))
                    {
                        dr.Cells["value"].Value = attributePairs[attributeName];
                        attributePairs.Remove(attributeName);
                    }
                }

                foreach (string key in attributePairs.Keys)
                {
                    AddAttributeToGrid(key, attributePairs[key]);   
                }

                ResizeWindow();
            }
            else
            {
                BeginInvoke(new UpdateGridDelegate(UpdateDataGrid), attributePairs);
            }
            
        }
        private delegate void TwoStringDelegate(string key, string val);
        private void AddAttributeToGrid(String key, String value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new TwoStringDelegate(AddAttributeToGrid), key, value);
            }
            else
            {
                dataGridViewAttributes.Rows.Add(key, value);
            }
        }
        private delegate void NoParamDelegate();
        private void UpdateFormHeader()
        {
            if (!InvokeRequired)
            {
                string obj = _currentObjectID;
                if (obj == string.Empty)
                {
                    obj = _noneSelected;
                }

                this.Text = String.Format("{0}{1}", _formHeader, obj);
            }
            else
            {
                BeginInvoke(new NoParamDelegate(UpdateFormHeader));
            }
            
        }

        private void ResizeWindow()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new NoParamDelegate(ResizeWindow));
            }
            else
            {
                int nwidth = 0;
                int nheight = 0;

                foreach (DataGridViewColumn col in dataGridViewAttributes.Columns)
                {
                    nwidth += col.Width;
                }

                foreach (DataGridViewRow row in dataGridViewAttributes.Rows)
                {
                    nheight += row.Height;
                }

                nwidth += dataGridViewAttributes.RowHeadersWidth;
                nwidth -= 5; //menu bar
                nheight += dataGridViewAttributes.ColumnHeadersHeight;
                nheight += 29;//side bars

                this.Width = nwidth;
                this.Height = nheight;
            }
        }

    }
}