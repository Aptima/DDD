using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BatchRunner
{
    public partial class BatchEditorForm : Form
    {
        private String batchFilePath;

        public String BatchFilePath
        {
            get { return batchFilePath; }
            set 
            { 
                batchFilePath = value;
                
                


            }
        }
        public BatchEditorForm()
        {
            InitializeComponent();
            

        }

        private void newRunButton_Click(object sender, EventArgs e)
        {
            RunInfo runInfo = new RunInfo();
            runInfo.RunName = "new run";
            runInfo.RunDuration = 1;
            runInfo.RunDataTag = "";
            runInfo.RunLogDirectoryPath = "";
            runInfo.ExternalSetupArguments = "";
            runInfo.ExternalSetupCommand = "";
            runInfo.ExternalSetupDelay = 1;
            runInfo.ExternalSetupWorkingDirectory = "";
            runInfo.ExternalTeardownArguments = "";
            runInfo.ExternalTeardownCommand = "";
            runInfo.ExternalTeardownDelay = 1;
            runInfo.ExternalTeardownWorkingDirectory = "";
            runInfo.UpdateListViewItem();

            BatchRunForm brf = new BatchRunForm(runInfo);
            brf.ShowDialog();
            if (brf.DialogResult == DialogResult.OK)
            {
                runListView.Items.Add(runInfo);
            }
            

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            batchFilePath = "";
            DialogResult = DialogResult.Cancel;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Batch As";
            sfd.FileName = BatchFilePath;
            sfd.Filter = "XML (*.xml)|*.xml";
            DialogResult r = sfd.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                BatchFilePath = sfd.FileName;
                List<RunInfo> runs = new List<RunInfo>();
                foreach (RunInfo ri in runListView.Items)
                {
                    runs.Add(ri);
                }
                RunInfo.SaveBatchFile(runs, BatchFilePath);


                DialogResult = DialogResult.OK;
            }
        }

        private void runListView_DoubleClick(object sender, EventArgs e)
        {
            RunInfo selected = null;
            foreach (RunInfo ri in runListView.SelectedItems)
            {
                selected = ri;
                break;
            }
            if (selected != null)
            {
                BatchRunForm brf = new BatchRunForm(selected);
                brf.ShowDialog();

            }
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            RunInfo selected = null;
            foreach (RunInfo ri in runListView.SelectedItems)
            {
                selected = ri;
                break;
            }
            if (selected != null)
            {
                int i = runListView.Items.IndexOf(selected);
                if (i > 0)
                {
                    runListView.Items.RemoveAt(i);
                    runListView.Items.Insert(i - 1, selected);
                    
                    
                }
                selected.Selected = true;
                runListView.Refresh();
                runListView.Focus();
            }
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            RunInfo selected = null;
            foreach (RunInfo ri in runListView.SelectedItems)
            {
                selected = ri;
                break;
            }
            if (selected != null)
            {
                int i = runListView.Items.IndexOf(selected);
                if (i < runListView.Items.Count - 1)
                {
                    runListView.Items.RemoveAt(i);
                    runListView.Items.Insert(i +1, selected);
                    
                    
                }
                selected.Selected = true;
                runListView.Refresh();
                runListView.Focus();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            RunInfo selected = null;
            foreach (RunInfo ri in runListView.SelectedItems)
            {
                selected = ri;
                break;
            }
            if (selected != null)
            {
                runListView.Items.Remove(selected);
            }
        }

        private void BatchEditorForm_Load(object sender, EventArgs e)
        {
            if (BatchFilePath != String.Empty)
            {
                List<RunInfo> runs = RunInfo.LoadBatchFile(BatchFilePath);
                runListView.Items.Clear();
                foreach (RunInfo ri in runs)
                {
                    ri.UpdateListViewItem();
                    runListView.Items.Add(ri);
                }
            }
        }
    }
}
