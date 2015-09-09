using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
 
namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class ObjectViewWindow : Form
    {
        private static string selectedObjectID = null;
        private static Dictionary<string, string> objectAttributes;
        private static Dictionary<string, SimulationObjectProxy> objectProxies;

        public ObjectViewWindow()
        {
            InitializeComponent();
            updateTimer.Start();
            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            //retrieve object keys, populate list box.
            //foreach (string s in ((Form1)this.Owner).ListOfObjects)
            //{
            //    if (!listBoxObjects.Items.Contains(s))
            //    {
            //        listBoxObjects.Items.Add(s);
            //    }
            //}
            objectProxies = ((Form1)Owner).GetObjectProxies();

            //update tree view info if an object is selected.
            UpdateAttributeDict();
        }
        private void UpdateAttributeDict()
        {
            if (objectProxies == null)
                return;
            foreach (string id in objectProxies.Keys)
            {
                if (!listBoxObjects.Items.Contains(id))
                {
                    listBoxObjects.Items.Add(id);
                }
            }

            if (listBoxObjects == null)
                return;
            foreach (string id2 in listBoxObjects.Items)
            {
                if (!objectProxies.ContainsKey(id2))
                {
                    listBoxObjects.Items.Remove(id2);
                }
            }

            if (treeViewObjectInfo == null)
                return;
            foreach (TreeNode tn in treeViewObjectInfo.Nodes)
            { 
                if(tn.Name != (string)listBoxObjects.SelectedItem)
                {
                    treeViewObjectInfo.Nodes.Remove(tn);
                }
            }
            if(listBoxObjects.SelectedItem != null)
            {
                string id3 = (string)listBoxObjects.SelectedItem;
                if (treeViewObjectInfo.Nodes.ContainsKey(id3))
                {
                    foreach (string key in objectProxies[id3].GetKeys())
                    {
                        treeViewObjectInfo.Nodes[id3].Nodes[key].Text = String.Format("{0} = {1}", key, DataValueFactory.ToString(objectProxies[id3][key].GetDataValue()));
                    }
                }
                else
                {
                    treeViewObjectInfo.Nodes.Add(id3, String.Format("{0} : {1}", objectProxies[id3].GetObjectType().ToString(), id3));

                    foreach (string key in objectProxies[id3].GetKeys())
                    {
                        treeViewObjectInfo.Nodes[id3].Nodes.Add(key, String.Format("{0} = {1}", key, DataValueFactory.ToString(objectProxies[id3][key].GetDataValue())));
                    }
                    treeViewObjectInfo.Nodes[id3].Expand();
                }

            }

            //string output;
            //if (selectedObjectID != null)
            //{
            //    objectAttributes = ((Form1)Owner).GetObjectAttributes(selectedObjectID);

            //    treeViewObjectInfo.Nodes.Clear();
            //    foreach (string s in objectAttributes.Keys)
            //    {
            //        output = String.Format("{0} = {1}", s, objectAttributes[s]);

            //        treeViewObjectInfo.Nodes.Add(String.Format("{0} = {1}", s, objectAttributes[s]));
            //    }
            
            //}
        }
        private void listBoxObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //retrieve selected object from Form1's object list.  populate tree view.
            selectedObjectID = ((ListBox)sender).SelectedItem.ToString();
            UpdateAttributeDict();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void checkBoxShowLimited_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (objectProxies == null)
        //        return;
        //    if (checkBoxShowLimited.Checked)
        //    {//show all 
        //        listBoxObjects.Items.Clear();
        //        foreach (string obj in objectProxies.Keys)
        //        {
        //            listBoxObjects.Items.Add(obj);
        //        }
        //    }
        //    else
        //    {//show visible only 
        //        listBoxObjects.Items.Clear();
        //        foreach (string obj in objectProxies.Keys)
        //        {
        //            if (((StringValue)objectProxies[obj]["State"].GetDataValue()).value != string.Empty)
        //            {
        //                listBoxObjects.Items.Add(obj);
        //            }
        //        }
        //    }
        //    if (!listBoxObjects.Items.Contains(selectedObjectID))
        //    { 
                
        //    }
        //}
    }
}