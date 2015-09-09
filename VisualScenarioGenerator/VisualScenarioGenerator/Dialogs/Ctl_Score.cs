using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Score : Ctl_ContentPaneControl
    {
        private ScoreDataStruct _datastore = ScoreDataStruct.Empty;

        public Ctl_Score()
        {
            InitializeComponent();
            bbSelectedRules.SetLabels("Available Rules:", "Selected Rules:");
        }

        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                ScoreDataStruct data = (ScoreDataStruct)object_data;
                txtName.Text = data.ID;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                _datastore.ID = txtName.Text;
                Notify((object)_datastore);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
