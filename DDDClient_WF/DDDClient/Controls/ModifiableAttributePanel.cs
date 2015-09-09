using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Controls
{
    public struct ModifiableAttributePair
    {
        public string Attribute;
        public string Value;
        public bool Modifiable;
    }

    public partial class ModifiableAttributePanel : UserControl
    {
        private Dictionary<string, ModifiableAttributePair> _attributeList;
        private Dictionary<string, int> _attributeGridMapping; //[att name]/[row]
        private Dictionary<int, string> _gridAttributeMapping; //[row]/[att name]
        private EventHandler _modifyCallback;

        private const int _pageSize = 6; //hardcoded, as the size is as well; number of attributes per "page"
        private int _currentPage = 1;

        public void Clear()
        {
            if (!InvokeRequired)
            {
                try
                {
                    _currentPage = 1;
                    _attributeList.Clear();
                    _attributeGridMapping.Clear();
                    _gridAttributeMapping.Clear();
                    tableLayoutPanelMain.Controls.Clear();
                    SetGridButtons();
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new ClearDelegate(Clear));
            }
            
        }
        public void SetCallbackFunction(EventHandler ev)
        {
            _modifyCallback = ev;
        }

        public void ModifyCallback(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13) //enter key
                {
                    //send update tag function
                    //string att = ((TextBox)sender).Name.Replace("_val_", "");
                    //string value = ((TextBox)sender).Text;
                    tableLayoutPanelMain.Focus();
                    //Console.WriteLine(String.Format("Attribute Update: {0} -> {1}", att, value));
                    //ModifiableAttributePair newPair = new ModifiableAttributePair();
                    //newPair.Attribute = att;
                    //newPair.Value = value;
                    //newPair.Modifiable = true;
                    //_attributeList[att] = newPair;

                    if (_modifyCallback != null)
                    {
                        _modifyCallback(sender, e);
                    }
                    else
                    {
                        Console.WriteLine("Caller's callback function is not defined.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public delegate bool AddAttributeDelegate(ModifiableAttributePair att);
        public delegate void ClearDelegate();


        public bool AddAttribute(ModifiableAttributePair att)
        {
            if (att.Attribute.Length == 0)
                return false;
            if (!InvokeRequired)
            {
                try
                {
                    if (_attributeList.ContainsKey(att.Attribute)) //Update Existing attribute
                    {
                        int row = _attributeGridMapping[att.Attribute];

                        if (_attributeList[att.Attribute].Modifiable != att.Modifiable) //Modify previous control type
                        {
                            /*
                             * //Trying to replace existing labels with textboxes is proving difficult, and not really req'd
                            Control con = null;
                            bool stop = false;
                            try
                            {
                                 con = tableLayoutPanelMain.Controls.Find(String.Format("_val_{0}", att.Attribute), true)[0];
                       
                            }
                            catch (Exception e)
                            {
                                stop = true;
                            }
                            if (!stop)
                            {
                                int indexOfCon = tableLayoutPanelMain.Controls.IndexOf(con);
                                int rowInd = 0;
                                int columnInd = 1;
                                if (att.Modifiable) //Make this attribute modifiable
                                {
                                    tableLayoutPanelMain.Controls.RemoveAt(indexOfCon);
                                    tableLayoutPanelMain.Controls.Add(CreateControl(GetControlName(att.Attribute, true), att.Value, true, false));
                                    SetGridToPage(_currentPage);
                                }
                                else //make this attribute not-modifiable
                                {
                                    //tableLayoutPanelMain.Controls[indexOfCon] = CreateControl(GetControlName(att.Attribute, true), att.Value, false, false);
                                }
                            }
                             */

                        }
                        if (_attributeList[att.Attribute].Value != att.Value) //Modify previous value
                        {
                            try
                            {
                                Control c = tableLayoutPanelMain.Controls.Find(GetControlName(att.Attribute, true), true)[0];
                                c.Text = att.Value;
                            }
                            catch (Exception e)
                            {
                                if (e is IndexOutOfRangeException)
                                {
                                    Console.WriteLine("Error in ModifiableAttributePanel.AddAttribute.  Attempting to add value '{0}' to attribute '{1}', resulted in error: {2}.", att.Value, att.Attribute, e.Message);
                                }
                                else
                                {
                                    throw e;
                                }
                            }
                        }
                        _attributeList[att.Attribute] = att;
                    }
                    else //Add New Attribute
                    {
                        int row = _attributeList.Count; //works for 0-based index

                        _attributeList.Add(att.Attribute, att);

                        int tableRow = row % _pageSize;
                        int tablePage = row / _pageSize + 1;

                        if (tablePage == _currentPage)
                        {
                            //create attribute name label
                            tableLayoutPanelMain.Controls.Add(CreateControl(GetControlName(att.Attribute, false), att.Attribute, false, true), 0, tableRow);
                            //create attribute value label
                            tableLayoutPanelMain.Controls.Add(CreateControl(GetControlName(att.Attribute, true), att.Value, att.Modifiable, false), 1, tableRow);
                        }
                        _attributeGridMapping.Add(att.Attribute, row);
                        _gridAttributeMapping.Add(row, att.Attribute);
                        SetGridButtons();
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new AddAttributeDelegate(AddAttribute), att);
            }

            

            return true;
        }

        private string GetControlName(string attributeName, bool isValue)
        {
            if (isValue)
            {
                return String.Format("_val_{0}", attributeName);
            }
            else
            {
                return String.Format("_att_{0}", attributeName);
            }
        }

        private void AddNewAttribute(string name, string value, int row, bool modifiable)
        {

        }

        private Control CreateControl(string name, string text, bool modifiable, bool isAttributeName)
        {
            Control c;

            if (isAttributeName) //create basic attribute label
            {
                c = new Label();
                //other formatting
                c.Text = text;
                c.Name = name;
                c.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);
                c.Dock = DockStyle.Fill;
                return c;
            }

            //is a value

            if (modifiable) //create a text box and attach event handler
            {
                c = new TextBox();
                //other formatting
                c.Text = text;
                c.Name = name;
                c.Dock = DockStyle.Fill;
                c.KeyPress += ModifyCallback;
                return c;
            }

            //is not modifiable

            //create value label
            c = new Label();
            //other formatting
            c.Text = text;
            c.Name = name;
            c.Dock = DockStyle.Fill;
            return c;

        }

        public void SetLabelBackgroundColor(Color c)
        {
            this.labelMain.BackColor = c;
            this.panel1.BackColor = c;
        }

        public void SetLabelForegroundColor(Color c)
        {
            this.labelMain.ForeColor = c;
        }

        public void SetLabelFont(Font f)
        {
            this.labelMain.Font = f;
        }

        public void SetLabelName(string name)
        {
            this.labelMain.Text = name; //length constraint?
        }

        public ModifiableAttributePanel()
        {
            InitializeComponent();
            _attributeList = new Dictionary<string, ModifiableAttributePair>();
            _attributeGridMapping = new Dictionary<string, int>();
            _gridAttributeMapping = new Dictionary<int, string>();
            SetGridButtons();
        }

        private void PrevNext_Click(object sender, EventArgs e)
        {
            int prevCurrentPage = _currentPage;

            if (((Button)sender).Name == "buttonPrev") //go back 1 page
            {
                _currentPage = Math.Max(_currentPage - 1, 1); //prevents errors with page < 1
                //set grid info
            }
            else //go forward one page
            {
                _currentPage = Math.Min(_currentPage + 1, _attributeList.Count / _pageSize + 1);  //prevents overrun errors
                //set grid info
            }

            if (prevCurrentPage != _currentPage) //pages shifted, clear out grid before setting new values
            {
                tableLayoutPanelMain.Controls.Clear();
            }
            SetGridToPage(_currentPage);
        }

        private void SetGridToPage(int page)
        {

            //the +1 and -1 are because this is a 1-based index
            int startRange = (page - 1) * _pageSize;
            int endRange = startRange + _pageSize - 1;
            /*
            int startRange = page * _pageSize;
            int endRange = startRange + _pageSize;
*/
            tableLayoutPanelMain.SuspendLayout();

            for (int row = 0; row < _pageSize; row++)
            {
                if (_gridAttributeMapping.Count > row + startRange)
                {
                    string att = _gridAttributeMapping[row + startRange];
                    tableLayoutPanelMain.Controls.Add(CreateControl(GetControlName(att, false), att, false, true), 0, row);
                    tableLayoutPanelMain.Controls.Add(CreateControl(GetControlName(att, true), _attributeList[att].Value, _attributeList[att].Modifiable, false), 1, row);

                }
            }

            tableLayoutPanelMain.ResumeLayout(true);

            SetGridButtons();
        }

        private void SetGridButtons()
        {
            //the +1 and -1 are because _currentPage has a 1-based index
            int startRange = (_currentPage - 1) * _pageSize + 1;
            int endRange = startRange + _pageSize - 1;

            buttonPrev.Enabled = !(_currentPage == 1);

            buttonNext.Enabled = (endRange < _attributeGridMapping.Count);
        }

        public void DisableDrawing()
        {
            this.SuspendLayout();
        }

        public void ResumeDrawing()
        { 
            this.ResumeLayout(true);
        }
    }
}
