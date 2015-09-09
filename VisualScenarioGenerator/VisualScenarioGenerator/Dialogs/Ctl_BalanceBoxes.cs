using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_BalanceBoxes : UserControl
    {
        protected View _view = null;
        protected Ctl_ContentPane _content_pane = null;
        private Boolean keepSorted;
      /// <summary>
      /// Force lists to be kept in sorted form
      /// </summary>   
        public void SortLists(Boolean indicator)
        {
            keepSorted = indicator;
        }
   

        public void AllowMultipleSelection(Boolean multi)
        {
            if (multi)
            {
                lbxAvailable.SelectionMode = SelectionMode.MultiExtended;
                lbxCurrent.SelectionMode = SelectionMode.MultiExtended;
            }
            else
            {
                lbxAvailable.SelectionMode = SelectionMode.One;
                lbxCurrent.SelectionMode = SelectionMode.One;
            }
        }

        public void SetLabels(string availableLabel, string currentLabel)
        {
            this.lblAvailable.Text = availableLabel;
            this.lblCurrent.Text = currentLabel;

        }
        /// <summary>
        /// Provides lists to display
        /// </summary>
        /// <param name="?"></param>
    public void DisplayLists(List<string> available,List<string>current)
    {
        lbxAvailable.Items.Clear();
            for (int i = 0; i < available.Count; i++)
                lbxAvailable.Items.Add(available[i]);
            lbxCurrent.Items.Clear();
            for (int i = 0; i < current.Count; i++)
                lbxCurrent.Items.Add(current[i]);
  if(keepSorted)
  {
      //Inserting each item onto place might be more efficient, especially with large lists, but probably no difference here.
            List<string> newAvailable = new List<string>();
            for (int i = 0; i < lbxAvailable.Items.Count; i++)
                newAvailable.Add(lbxAvailable.Items[i].ToString());
             newAvailable.Sort();
            lbxAvailable.Items.Clear();
            for (int i = 0; i < newAvailable.Count; i++)
                lbxAvailable.Items.Add(newAvailable[i]);
            List<string> newCurrent = new List<string>();
            for (int i = 0; i < lbxCurrent.Items.Count; i++)
                newCurrent.Add(lbxCurrent.Items[i].ToString());
            newCurrent.Sort();
            lbxCurrent.Items.Clear();
            for (int i = 0; i < newCurrent.Count; i++)
                lbxCurrent.Items.Add(newCurrent[i]);
        }
    }
    /// <summary>
    /// Retrieve   items currently not chosen
    /// </summary>
    /// <returns></returns>
    public List<string> GetAvailable()
        {
            List<string>returnValue=new List<string>();
            for(int i=0;i<lbxAvailable.Items.Count;i++)
                returnValue.Add(lbxAvailable.Items[i].ToString());
            return returnValue;
        }
        /// <summary>
        /// Retrieve currently chosen items
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrent()
        {
            List<string>returnValue=new List<string>();
            for(int i=0;i<lbxCurrent.Items.Count;i++)
                returnValue.Add(lbxCurrent.Items[i].ToString());
            return returnValue;
        }
        public Ctl_BalanceBoxes()
        {
            InitializeComponent();
            lbxAvailable.Items.Clear();
            lbxCurrent.Items.Clear();
            keepSorted = false;
            lbxCurrent.SelectionMode = SelectionMode.One;
            lbxAvailable.SelectionMode = SelectionMode.One;
        }
    

        public void BindContentPane(View view, Ctl_ContentPane control)
        {
            _view = view;
            _content_pane = control;
        }
/*
 * public virtual void Update(object object_data)
        {
            throw new Exception("Update hasn't been implemented");
        }
        public void Notify(object object_data)
        {
            _view.UpdateContentPanel(object_data);
            _view.Notify(object_data);
        }
*/
        private void AddCurrent(object oneItem)
        {
            if (!keepSorted)
            {
                lbxCurrent.Items.Add(oneItem);
            }
            else
            {
                Boolean inserted = false;
                for (int i = 0; i < lbxCurrent.Items.Count; i++)
                {
                    if(0>String.Compare(oneItem.ToString(),lbxCurrent.Items[i].ToString()))
                    {
                        lbxCurrent.Items.Insert(i,oneItem);
                        inserted=true;
                        break;
                    }
                   
                } 
                if (!inserted)
                        lbxCurrent.Items.Add(oneItem);
            }
        }

        private void RemoveCurrent(object oneItem)
        {
            if (!keepSorted)
            {
                lbxAvailable.Items.Add(oneItem);
            }
            else
            {
                Boolean inserted = false;
                for (int i = 0; i < lbxCurrent.Items.Count; i++)
                {
                    if ((lbxAvailable.Items.Count>0)&&(0>String.Compare(oneItem.ToString() , lbxAvailable.Items[i].ToString())))
                    {
                        lbxAvailable.Items.Insert(i, oneItem);
                        inserted = true;
                        break;
                    }                
                }
                if (!inserted)
                        lbxAvailable.Items.Add(oneItem);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lbxAvailable.Items.Count > 0)
            { 
                List<object> items=new List<object>();
                for(int i=0;i<lbxAvailable.SelectedItems.Count;i++)
                 items.Add((object)(lbxAvailable.SelectedItems[i]));
                if (items.Count > 0)
                {
                    lbxAvailable.BeginUpdate();
                    lbxCurrent.BeginUpdate();
                    for (int i = 0; i < items.Count; i++)
                    {
                        string oneItem = items[i].ToString();
                        lbxAvailable.Items.Remove(oneItem);
                        AddCurrent(oneItem);
                    }
                    lbxAvailable.EndUpdate();
                    lbxCurrent.EndUpdate();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxCurrent.Items.Count > 0)
            {
                List<object> items = new List<object>();
                for (int i = 0; i < lbxCurrent.SelectedItems.Count; i++)
                    items.Add((object)(lbxCurrent.SelectedItems[i]));
                if (items.Count > 0)
                {
                    lbxAvailable.BeginUpdate();
                    lbxCurrent.BeginUpdate();
                    for (int i = 0; i < items.Count; i++)
                    {
                        string oneItem = items[i].ToString();
                        lbxCurrent.Items.Remove(oneItem);
                        RemoveCurrent(oneItem);
                    }
                    lbxAvailable.EndUpdate();
                    lbxCurrent.EndUpdate();
                }
            }
        }

 

    }
}
