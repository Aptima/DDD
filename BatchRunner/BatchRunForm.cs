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
    public partial class BatchRunForm : Form
    {
        RunInfo runInfo;
        public BatchRunForm(RunInfo r)
        {
            runInfo = r;
            InitializeComponent();
            runNameTextBox.Text = r.RunName;
            scenarioNameTextBox.Text = r.RunScenarioPath;
            durationNumericUpDown.Value = r.RunDuration;
            runLogDirectoryPathTextBox.Text = r.RunLogDirectoryPath;
            runDataTagTextBox.Text = r.RunDataTag;
            externalSetupCommandTextBox.Text = r.ExternalSetupCommand;
            externalSetupArgumentsTextBox.Text = r.ExternalSetupArguments;
            externalSetupWorkingDirectoryTextBox.Text = r.ExternalSetupWorkingDirectory;
            externalSetupDelayNumericUpDown.Value = r.ExternalSetupDelay;
            externalTeardownCommandTextBox.Text = r.ExternalTeardownCommand;
            externalTeardownArgumentsTextBox.Text = r.ExternalTeardownArguments;
            externalTeardownWorkingDirectoryTextBox.Text = r.ExternalTeardownWorkingDirectory;
            externalTeardownDelayNumericUpDown.Value = r.ExternalTeardownDelay;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            runInfo.RunName = runNameTextBox.Text;
            runInfo.RunScenarioPath = scenarioNameTextBox.Text;
            runInfo.RunDuration =  Decimal.ToInt32(durationNumericUpDown.Value);


            runInfo.RunLogDirectoryPath = runLogDirectoryPathTextBox.Text;
            runInfo.RunDataTag = runDataTagTextBox.Text;
            runInfo.ExternalSetupCommand = externalSetupCommandTextBox.Text;
            runInfo.ExternalSetupArguments = externalSetupArgumentsTextBox.Text;
            runInfo.ExternalSetupWorkingDirectory = externalSetupWorkingDirectoryTextBox.Text;
            runInfo.ExternalSetupDelay = Decimal.ToInt32(externalSetupDelayNumericUpDown.Value);
            runInfo.ExternalTeardownCommand = externalTeardownCommandTextBox.Text;
            runInfo.ExternalTeardownArguments = externalTeardownArgumentsTextBox.Text;
            runInfo.ExternalTeardownWorkingDirectory = externalTeardownWorkingDirectoryTextBox.Text;
            runInfo.ExternalTeardownDelay = Decimal.ToInt32(externalTeardownDelayNumericUpDown.Value);


            runInfo.UpdateListViewItem();
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = scenarioNameTextBox.Text;
            of.Title = "Find DDD Scenario";
            of.Filter = "XML (*.xml)|*.xml";
            DialogResult r = of.ShowDialog();
            if (r == DialogResult.OK)
            {
                scenarioNameTextBox.Text = of.FileName;
            }
        }

        private void runLogDirectoryPathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            fb.SelectedPath = runLogDirectoryPathTextBox.Text;

            DialogResult r = fb.ShowDialog();
            if (r == DialogResult.OK)
            {
                runLogDirectoryPathTextBox.Text = fb.SelectedPath;
            }
        }

        private void externalSetupWorkingDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            fb.SelectedPath = externalSetupWorkingDirectoryTextBox.Text;

            DialogResult r = fb.ShowDialog();
            if (r == DialogResult.OK)
            {
                externalSetupWorkingDirectoryTextBox.Text = fb.SelectedPath;
            }
        }

        private void externalTeardownWorkingDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            fb.SelectedPath = externalTeardownWorkingDirectoryTextBox.Text;

            DialogResult r = fb.ShowDialog();
            if (r == DialogResult.OK)
            {
                externalTeardownWorkingDirectoryTextBox.Text = fb.SelectedPath;
            }
        }
    }
}
