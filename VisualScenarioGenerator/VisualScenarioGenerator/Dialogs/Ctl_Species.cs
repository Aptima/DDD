using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VisualScenarioGenerator.VSGPanes;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Species : Ctl_ContentPaneControl
    {
        public static ObjectTypeLists<SpeciesDataStruct> Species = new ObjectTypeLists<SpeciesDataStruct>();
  
        private SpeciesDataStruct _datastore = SpeciesDataStruct.Empty();

        public Ctl_Species()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                _datastore.ID = textBox1.Text;
                Notify((object)_datastore);
            }
        }


        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                SpeciesDataStruct data = (SpeciesDataStruct)object_data;
                textBox1.Text = data.ID;
            }
        }
    }

    public class SpeciesDataStruct : VisualScenarioGenerator.VSGPanes.ObjectTypeStructure
    {

        public static SpeciesDataStruct Empty()
        {
            return new SpeciesDataStruct("");
        }

        public SpeciesDataStruct(string name):base(name,true)
        {
   
        }

        #region ICloneable Members

        public object Clone()
        {
            SpeciesDataStruct obj = new SpeciesDataStruct(ID);
 
            return obj;
        }

        #endregion
    }

}
