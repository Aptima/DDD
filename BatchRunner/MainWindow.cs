using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using Aptima.Asim.DDD.DDDAgentFramework;
using System.Threading;


namespace BatchRunner
{
    public partial class MainWindow : Form
    {
        enum BatchRunnerState { Uninitialized, Loaded, Running, WaitingToResume };
        BatchRunnerState m_state = BatchRunnerState.Uninitialized;

        List<RunInfo> m_loadedRuns;
        RunInfo m_currentRun;
        int m_currentRunFinishTime;
        System.Diagnostics.Process m_externalCommandProcess;
        System.Diagnostics.Process m_dddProcess;
        DDDServerConnection m_ddd;
        public MainWindow()
        {
            InitializeComponent();
            m_loadedRuns = null;
            m_ddd = null;
            m_currentRun = null;
            m_currentRunFinishTime = 0;
            m_externalCommandProcess = null;
            m_dddProcess = null;
        }

        

        private void MainWindow_Load(object sender, EventArgs e)
        {
            batchFilePathTextBox.Text = Properties.Settings.Default.BatchFilePath;
            dddExecutablePathTextBox.Text = Properties.Settings.Default.DDDExecutablePath;
            logPathTextBox.Text = Properties.Settings.Default.DefaultLogPath;
            UpdateView();
        }

        private void UpdateView()
        {
            if (!Properties.Settings.Default.RunNextAutomatically)
            {
                goButton.Visible = true;
            }
            else
            {
                goButton.Visible = false;
            }
            switch (m_state)
            {
                case BatchRunnerState.Uninitialized:
                    runBatchButton.Text = "Start";
                    runBatchButton.Enabled = false;
                    batchFileBrowseButton.Enabled = true;
                    if (Properties.Settings.Default.RunDDDServer)
                    {
                        dddExecutableBrowseButton.Enabled = true;
                    }
                    else
                    {
                        dddExecutableBrowseButton.Enabled = false;
                    }
                    loadBatchButton.Enabled = true;
                    statusRichTextBox.Clear();
                    timeTextBox.Text = "";
                    goButton.Enabled = false;
                    dataTagTextBox.Enabled = true;
                    break;
                case BatchRunnerState.Loaded:
                    runBatchButton.Text = "Start";
                    runBatchButton.Enabled = true;
                    batchFileBrowseButton.Enabled = true;
                    if (Properties.Settings.Default.RunDDDServer)
                    {
                        dddExecutableBrowseButton.Enabled = true;
                    }
                    else
                    {
                        dddExecutableBrowseButton.Enabled = false;
                    }
                    loadBatchButton.Enabled = true;
                    statusRichTextBox.Clear();
                    timeTextBox.Text = "";
                    goButton.Enabled = false;
                    dataTagTextBox.Enabled = true;
                    break;
                case BatchRunnerState.Running:
                    runBatchButton.Text = "Stop";
                    runBatchButton.Enabled = true;
                    batchFileBrowseButton.Enabled = false;
                    dddExecutableBrowseButton.Enabled = false;
                    loadBatchButton.Enabled = false;
                    goButton.Enabled = false;
                    dataTagTextBox.Enabled = false;
                    break;
                case BatchRunnerState.WaitingToResume:
                    runBatchButton.Text = "Stop";
                    runBatchButton.Enabled = true;
                    batchFileBrowseButton.Enabled = false;
                    dddExecutableBrowseButton.Enabled = false;
                    loadBatchButton.Enabled = false;
                    goButton.Enabled = true;
                    dataTagTextBox.Enabled = false;
                    break;
            }

        }

        private void dddExecutableBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = dddExecutablePathTextBox.Text;
            of.Title = "Find DDD Server Executable";
            of.Filter = "EXE (*.exe)|*.exe";
            DialogResult r = of.ShowDialog();
            if (r == DialogResult.OK)
            {
                dddExecutablePathTextBox.Text = of.FileName;
                Properties.Settings.Default.DDDExecutablePath = dddExecutablePathTextBox.Text;
                Properties.Settings.Default.Save();
            }
            UpdateView();
        }

        private void batchFileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = batchFilePathTextBox.Text;
            of.Title = "Open Batch File XML";
            of.Filter = "XML (*.xml)|*.xml";
            DialogResult r = of.ShowDialog();
            if (r == DialogResult.OK)
            {
                batchFilePathTextBox.Text = of.FileName;
                Properties.Settings.Default.BatchFilePath = batchFilePathTextBox.Text;
                Properties.Settings.Default.Save();
                m_state = BatchRunnerState.Uninitialized;
            }
            UpdateView();
        }

        private void loadBatchButton_Click(object sender, EventArgs e)
        {
            // load stuff, if successful, change the state
            m_loadedRuns = RunInfo.LoadBatchFile(batchFilePathTextBox.Text);
            if (m_loadedRuns != null)
            {
                m_state = BatchRunnerState.Loaded;
            }
            UpdateView();
        }

        void StartDDD()
        {

            if (Properties.Settings.Default.RunDDDServer)
            {
                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.FileName = dddExecutablePathTextBox.Text;
                procInfo.WorkingDirectory = Path.GetDirectoryName(dddExecutablePathTextBox.Text);
                procInfo.Arguments = Properties.Settings.Default.DDDPort.ToString();
                procInfo.CreateNoWindow = true;
                procInfo.UseShellExecute = true;
                m_dddProcess = System.Diagnostics.Process.Start(procInfo);

                Thread.Sleep(1000);
            }
            m_ddd = new DDDServerConnection();
            m_ddd.ConnectToServer(Properties.Settings.Default.DDDHostname, Properties.Settings.Default.DDDPort);
            m_ddd.DDDClientPath = Properties.Settings.Default.DDDClientPath;
            if (!m_ddd.ReadSimModel())
            {
                MessageBox.Show(String.Format("Unable to read sim model"));
                return;
            }
            
        }

        void StopDDD()
        {
            m_ddd.Disconnect();
            m_ddd = null;
            if (Properties.Settings.Default.RunDDDServer)
            {
                if (m_dddProcess != null)
                {
                    m_dddProcess.Kill();
                }
                m_dddProcess = null;
            }
        }

        void StartNextRun()
        {
            if (m_loadedRuns == null || m_loadedRuns.Count == 0)
            {
                statusRichTextBox.AppendText("Nothing to do...\n");
                m_state = BatchRunnerState.Uninitialized;
                return;
            }
            if (m_ddd == null)
            {
                StartDDD();
            }
            m_currentRun = m_loadedRuns[0];
            m_loadedRuns.Remove(m_currentRun);
            m_currentRunFinishTime = m_currentRun.RunDuration * 1000;
            //m_ddd.ResetForNewSession();
            String dataTag = dataTagTextBox.Text;
            if (m_currentRun.RunDataTag != String.Empty)
            {
                dataTag = m_currentRun.RunDataTag;
            }
            String logPath = logPathTextBox.Text;
            if (m_currentRun.RunLogDirectoryPath != String.Empty)
            {
                logPath = m_currentRun.RunLogDirectoryPath;
            }

            statusRichTextBox.AppendText("Starting new run:\n");
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "RunName", m_currentRun.RunName));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "RunScenarioPath", m_currentRun.RunScenarioPath));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "RunLogDirectoryPath", logPath));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "RunDataTag", dataTag));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "RunDuration", m_currentRun.RunDuration));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "ExternalCommandSetup", m_currentRun.ExternalSetupCommand));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "ExternalCommandSetupDelay", m_currentRun.ExternalSetupDelay));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "ExternalCommandTeardown", m_currentRun.ExternalTeardownCommand));
            statusRichTextBox.AppendText(String.Format("   {0}: {1}\n", "ExternalCommandTeardownDelay", m_currentRun.ExternalTeardownDelay));
            statusRichTextBox.Select(statusRichTextBox.Text.Length - 1, 0);
            statusRichTextBox.ScrollToCaret();
            //m_ddd.SendStopScenarioRequest();
            //Thread.Sleep(2000);
            m_ddd.SendLoadScenarioRequest(m_currentRun.RunScenarioPath, dataTag, logPath);
            Thread.Sleep(2000);

            if (m_currentRun.ExternalSetupCommand != String.Empty)
            {
                //TODO external command start stuff
                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.FileName = m_currentRun.ExternalSetupCommand;
                procInfo.WorkingDirectory = m_currentRun.ExternalSetupWorkingDirectory;
                procInfo.Arguments = m_currentRun.ExternalSetupArguments + String.Format(" 1>>{0}\\{1}.log 2>&1", logPath, dataTag);
                procInfo.CreateNoWindow = true;
                m_externalCommandProcess = System.Diagnostics.Process.Start(procInfo);
                Thread.Sleep(m_currentRun.ExternalSetupDelay * 1000);
            }

            if (Properties.Settings.Default.RunNextAutomatically)
            {
                m_ddd.SendResumeScenarioRequest();
            }
            else
            {
                m_state = BatchRunnerState.WaitingToResume;
            }
            UpdateView();

            
        }

        void FinishCurrentRun()
        {
            m_ddd.SendStopScenarioRequest();
            Thread.Sleep(2000);
            if (m_externalCommandProcess != null)
            {
                if (!m_externalCommandProcess.HasExited)
                {
                    m_externalCommandProcess.Kill();
                }
                m_externalCommandProcess = null;
                if (m_currentRun.ExternalTeardownCommand != String.Empty)
                {
                    System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                    procInfo.FileName = m_currentRun.ExternalTeardownCommand;
                    procInfo.WorkingDirectory = m_currentRun.ExternalTeardownWorkingDirectory;
                    procInfo.Arguments = m_currentRun.ExternalTeardownArguments;
                    procInfo.CreateNoWindow = true;
                    System.Diagnostics.Process proc = System.Diagnostics.Process.Start(procInfo);
                    Thread.Sleep(m_currentRun.ExternalTeardownDelay * 1000);
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }

                
            }

            //TODO external command stop stuff
            statusRichTextBox.AppendText(String.Format("Finished run\n"));
            
            m_currentRun = null;
            if (Properties.Settings.Default.StopDDDServerBetweenRuns)
            {
                StopDDD();
            }
            else
            {
                if (m_ddd != null)
                {
                    m_ddd.ResetForNewSession();
                    
                }
            }
            StartNextRun();
        }

        private void runBatchButton_Click(object sender, EventArgs e)
        {
            if (m_state == BatchRunnerState.Loaded)
            {
                m_state = BatchRunnerState.Running;
                StartNextRun();
                
            }
            else if (m_state == BatchRunnerState.Running || m_state == BatchRunnerState.WaitingToResume)
            {
                m_loadedRuns.Clear();
                FinishCurrentRun();
                m_state = BatchRunnerState.Uninitialized;
            }
            UpdateView();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (m_ddd != null)
            {
                if (!m_ddd.IsConnected())
                {
                    MessageBox.Show("Lost connection to the DDD Server", "DDD Connection Error");
                    Environment.Exit(0);
                }

                m_ddd.ProcessEvents();

                timeTextBox.Text = m_ddd.DDDTimeString;

                if (m_currentRun != null)
                {
                    if (m_ddd.DDDTimeInt >= m_currentRunFinishTime)
                    {
                        FinishCurrentRun();
                    }
                }
            }
            else
            {
                timeTextBox.Text = "N/A";
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_ddd != null)
            {
                if (m_ddd.IsConnected())
                {
                    m_ddd.Disconnect();
                }
                
                
            }
            if (m_dddProcess != null)
            {
                m_dddProcess.Kill();
            }
            Environment.Exit(0);
        }



        private void goButton_Click(object sender, EventArgs e)
        {
            m_ddd.SendResumeScenarioRequest();
            m_state = BatchRunnerState.Running;
            UpdateView();
        }

        private void newBatchButton_Click(object sender, EventArgs e)
        {
            BatchEditorForm b = new BatchEditorForm();
            b.BatchFilePath = batchFilePathTextBox.Text;
            b.ShowDialog();
            if (b.DialogResult == DialogResult.OK)
            {
                batchFilePathTextBox.Text = b.BatchFilePath;
                Properties.Settings.Default.BatchFilePath = batchFilePathTextBox.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void logPathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            fb.SelectedPath = logPathTextBox.Text;

            DialogResult r = fb.ShowDialog();
            if (r == DialogResult.OK)
            {
                logPathTextBox.Text = fb.SelectedPath;
                Properties.Settings.Default.DefaultLogPath = logPathTextBox.Text;
                Properties.Settings.Default.Save();
            }
        }






    }
}
