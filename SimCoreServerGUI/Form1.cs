using System;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.HandshakeManager;
using Aptima.Asim.DDD.SimCoreServer;
using Aptima.Asim.DDD.SimulationEngine;
using Aptima.Asim.DDD.CommonComponents.SimCoreTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;
using Aptima.Asim.DDD.CommonComponents.UserTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using System.Text.RegularExpressions;
namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class Form1 : Form
    {
        /*
This regex based on ReplayLogger.cs
sr.WriteLine("<Creator><Version>" + productVersion + "</Version><CompiledOn>" + compileDate + "</CompiledOn></Creator>");
*/
        //private static Regex creatorRegex;

        private class DMInfoManager
        {
            private static List<HandshakeManager.DecisionMakerLoginInfo> dmList;
            private static object owner;
            public DMInfoManager(object Owner)
            {
                dmList = new List<HandshakeManager.DecisionMakerLoginInfo>();
                owner = Owner;
            }
            public string GetDMAvail(string name)
            {
                foreach (HandshakeManager.DecisionMakerLoginInfo s in dmList)
                {
                    if (s.DMName == name)
                    {
                        return s.DMAvail;
                    }
                }
                return string.Empty;
            }

            public void UpdateDMStatus(string name, string role, string avail)
            {
                bool dmExists = false;

                foreach (HandshakeManager.DecisionMakerLoginInfo dm in dmList)
                {
                    if (name == dm.DMName)
                    {
                        dmExists = true;
                        dm.DMAvail = avail;

                        //update info, update grid
                        UpdateGrid(name, role, avail);
                    }
                }
                if (dmExists == false)
                {//dm wasnt in list, add
                    dmList.Add(new HandshakeManager.DecisionMakerLoginInfo(name, role, avail));
                    UpdateGrid(name, role, avail);
                }
            }
            private void UpdateGrid(string name, string role, string avail)
            {
                DataGridView dv = ((Form1)owner).GetGrid();
                bool exists = false;

                foreach (DataGridViewRow dr in dv.Rows)
                {
                    if ((string)(dr.Cells["DMName"].Value) == name)
                    {
                        exists = true;
                        dr.Cells["DMAvail"].Value = (string)avail;
                        dr.Cells["DMRole"].Value = (string)role;
                    }
                }
                if (exists == false)
                {
                    string[] stringArray = { name, role, avail };
                    dv.Rows.Add(stringArray);
                }
            }
            public void Clear()
            {
                dmList.Clear();
            }
        }

        SimulationEventDistributorClient eventClient = null;
        String serverState;
        private bool forceFormClose = false;
        private static DMInfoManager DMInfo;
        private static string applicationPath = Application.ExecutablePath;
        public static string ApplicationPath
        {
            get { return applicationPath; }
        }

        private static SimCoreServer.SimCoreServer server;

        public DataGridView GetGrid()
        {
            return dataGridViewDMs;
        }
        public Form1()
        {
            
            string errorMsg = string.Empty;
            int seats = -1;

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                ServerOptions.ReadFile();
                String portString = Environment.GetCommandLineArgs()[1];
                ServerOptions.PortNumber = Int32.Parse(portString);
                ServerOptions.WriteFile();
            }
            else
            {
                ServerConfigDialog d = new ServerConfigDialog();
                d.DialogMessage = "Configure the server port:";
                if (d.ShowDialog(this) == DialogResult.Cancel)
                {
                    MessageBox.Show(this, "Closing DDD", "User Requested Shutdown", MessageBoxButtons.OK);
                    this.Close();
                    return;
                }
                while (!SimCoreServer.SimCoreServer.CheckNetworkSettings())
                {
                    d.DialogMessage = "The specified port is not available.\nPlease shutdown any other application\nusing the port or choose a different one.";
                    d.ShowDialog(this);
                }
            }
            InitializeComponent();
            server = new SimCoreServer.SimCoreServer();
            LoadServerOptions();
            
                    
  
            eventClient = new SimulationEventDistributorClient();
            SimCoreServer.SimCoreServer.simEngine.simCore.distributor.RegisterClient(ref eventClient);
            eventClient.Subscribe("ServerState");
            eventClient.Subscribe("PauseScenario");
            eventClient.Subscribe("ResumeScenario");
            serverState = "SCENARIO_STOPPED";

            comboBoxReplaySpeed.SelectedIndex = 4;
            comboBoxReplaySpeed.Enabled = false;
            toolStripStatusLabelServerStatus.Text = "Server: STOPPED";
            toolStripStatusLabelSimStatus.Text = "Simulation: STOPPED";



            DMInfo = new DMInfoManager(this);

            
            UpdateScenarioNameLabel();
            updateTimer.Start();
            UpdateVoiceServerButton();
        }

        void LoadServerOptions()
        {
            textBoxScenario.Text = ServerOptions.DefaultScenarioPath;
            //hostNameTextBox.Text = ServerOptions.HostName;

            hostNameTextBox.Text = String.Format("{0} ({1})", System.Net.Dns.GetHostName(), server.NetServer.ServerIP);
            portNumberTextBox.Text = ServerOptions.PortNumber.ToString();
            eventLogTextBox.Text = ServerOptions.EventLogDirectory;
        }



        public static int SimulationLength
        {
            get { return server.SimulationLength; }
        }
        private static double SimulationCompletionRatio = 0.0;

        private int GetSimulationCompletionInt(int maxProgressBarValue, int currentSeconds, int maxSeconds)
        {
            if (maxSeconds == 0)
                return maxSeconds;
            SimulationCompletionRatio = ((double)currentSeconds / (double)maxSeconds);
            if (currentSeconds > maxSeconds) //likely that there is no max value.
                return maxProgressBarValue;

            return (int)(maxProgressBarValue * SimulationCompletionRatio);
        }



        private void loadScenarioFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// If this is true, the tool bar will be locked.  Otherwise, it will be unlocked.
        /// </summary>
        /// <param name="value"></param>
        private void LockToolbarButtons(bool value)
        {
            usersToolStripMenuItem.Enabled = !value;
        }
        private void ClearDMList()
        {
            dataGridViewDMs.Rows.Clear();
            DMInfo.Clear();
        }


        private void buttonLoadScenario_Click(object sender, EventArgs e)
        {
            if (serverState == "SCENARIO_STOPPED" || serverState == "REPLAY_STOPPED")
            {
                //TODO: Get Current User/Passwords
                int userAccountCount = UserAdministration.GetUserCount();

                //if not, display message and return
                if (userAccountCount < 1)
                {
                    MessageBox.Show("You need to have at least 1 user account created to run a simulation.  Go to Users > User Administration to enter user accounts.");
                    return;
                }

                try
                {
                    server.SimControlLoadScenario(textBoxScenario.Text, textBoxGroupName.Text, eventLogTextBox.Text);
                }
                catch (Exception ex)
                {
                    server.ResetServer();
                    MessageBox.Show(ex.Message);
                    return;
                }
                if (ServerOptions.EnableVoiceServer)
                {
                    string errString = "Missing Voice Server Credentials:";

                    if (ServerOptions.VoiceServerAdminUsername == string.Empty)
                        errString += "\r\n\tAdmin Username";

                    if (ServerOptions.VoiceServerHostname == string.Empty)
                        errString += "\r\n\tServer Hostname";

                    if (ServerOptions.VoiceServerPort < 0)
                        errString += "\r\n\tServer Port Number";

                    if (errString != "Missing Voice Server Credentials:")
                    {
                        errString += "\r\nPlease close the server and locate your Server Options file and modify these values";
                        MessageBox.Show(errString, "Error Validating Voice Server Settings");
                        return;
                    }
                }                        
            }
            else
            {

                if (DialogResult.No == (MessageBox.Show("A simulation is currently running, are you sure that you want to stop?", "Warning", MessageBoxButtons.YesNo)))
                {
                    return;
                }

                server.SimControlStopScenario(true);
            }

        }


        private bool AreThereUnassignedDMs()
        {
            foreach (DataGridViewRow dr in dataGridViewDMs.Rows)
            {
                if (dr.Cells["DMAvail"].Value != null)
                {
                    if (dr.Cells["DMAvail"].Value.ToString() == "Available")
                        return true;
                }
            }

            return false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            server.SimControlPauseScenario();
        }
        private void UpdatePauseButton()
        {
            UpdatePauseButton(false);
        }
        private void UpdatePauseButton(bool resumeIsStart)
        {

            //based on the new state, adjust labels accordingly
            if (SimCoreServer.SimCoreServer._ScenarioState == SimCoreServer.SimCoreServer.ScenarioState.PAUSED
                || SimCoreServer.SimCoreServer._ReplayState == SimCoreServer.SimCoreServer.ScenarioState.PAUSED)
            {
                toolStripStatusLabelSimStatus.Text = "Simulation: PAUSED";
                if (resumeIsStart)
                {
                    buttonPauseSim.Text = "Start";
                }
                else
                {
                    buttonPauseSim.Text = "Resume";
                }
            }
            else
            {
                toolStripStatusLabelSimStatus.Text = "Simulation: RUNNING";
                buttonPauseSim.Text = "Pause";
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceFormClose)
            {//force form close will override the user's choice
                if (DialogResult.Yes == (MessageBox.Show("Server is currently running, are you sure that you want to exit?", "Warning", MessageBoxButtons.YesNo)))
                { //force server shutdown
                    server.StopServer();
                    
                }
                else
                {//do not quit
                    e.Cancel = true;
                }
            }
            else
            {
                server.StopServer();
            }

            ServerOptions.WriteFile();
        }

        private DateTime ExtractTime(int time)
        {
            int hours = time / 3600;
            time -= (hours * 3600);
            int mins = time / 60;
            time -= (mins * 60);
            int secs = time;
            DateTime currTime = new DateTime(1, 1, 1, hours, mins, secs);

            return currTime;
        }
        
        private void HandleServerStateEvent(String messageType, String messageText)
        {
            serverState = messageType;
            switch (messageType)
            {
                case "SCENARIO_STOPPED":
                    ClearDMList();
                    openReplayLogToolStripMenuItem.Enabled = true;
                    LockToolbarButtons(false);
                    buttonLoadScenario.Text = "Load Scenario";
                    replayLogBrowse.Enabled = true;
                    replayLogPathTextBox.Enabled = true;
                    replayStartButton.Enabled = true;
                    comboBoxReplaySpeed.Enabled = false;
                    replayStartButton.Enabled = true;
                    toolStripStatusLabelServerStatus.Text = "Server: STOPPED";
                    textBoxGroupName.Enabled = true;
                    portNumberTextBox.Enabled = true;
                    eventLogTextBox.Enabled = true;
                    eventLogPathButton.Enabled = true;
                    textBoxScenario.Enabled = true;
                    buttonScenario.Enabled = true;
                    buttonPauseSim.Enabled = false;
                    groupBox4.Enabled = true;
                    break;
                case "REPLAY_STOPPED":
                    LockToolbarButtons(false);
                    replayStartButton.Text = "Load Replay";
                    replayLogBrowse.Enabled = true;
                    portNumberTextBox.Enabled = true;
                    eventLogTextBox.Enabled = true;
                    eventLogPathButton.Enabled = true;
                    replayLogPathTextBox.Enabled = true;
                    buttonLoadScenario.Enabled = true;
                    textBoxGroupName.Enabled = true;
                    textBoxScenario.Enabled = true;
                    buttonScenario.Enabled = true;
                    buttonPauseSim.Enabled = false;
                    toolStripStatusLabelServerStatus.Text = "Server: STOPPED";
                    toolStripStatusLabelSimStatus.Text = "Simulation: STOPPED";
                    comboBoxReplaySpeed.Enabled = false;
                    break;
                case "LOADING_SCENARIO":
                    ClearDMList();
                    textBox1.Text = String.Format("{0:HH:mm:ss}", ExtractTime(0));
                    toolStripStatusLabelSimLength.Text = String.Format("{0:HH:mm:ss}", ExtractTime(server.SimulationLength));
                    buttonLoadScenario.Text = "Stop Scenario";
                    buttonLoadScenario.Enabled = false;

                    labelScenarioIsLoading.Visible = true;
                    buttonLoadScenario.Enabled = false;
                    buttonPauseSim.Enabled = false;
                    LockToolbarButtons(true);
                    textBoxGroupName.Enabled = false;
                    textBoxScenario.Enabled = false;
                    portNumberTextBox.Enabled = false;
                    eventLogTextBox.Enabled = false;
                    eventLogPathButton.Enabled = false;
                    buttonScenario.Enabled = false;

                    //start server
                    toolStripStatusLabelServerStatus.Text = "Server: RUNNING";

                    //load scenario
                    toolStripStatusLabelSimStatus.Text = "Simulation: LOADING";
                    buttonPauseSim.Text = "Loading Simulation";

                    replayLogBrowse.Enabled = false;
                    replayLogPathTextBox.Enabled = false;
                    replayStartButton.Enabled = false;

                    groupBox4.Enabled = false;
                    break;
                case "LOADING_REPLAY":

                    ClearDMList();
                    LockToolbarButtons(true);
                    replayStartButton.Enabled = false;
                    labelScenarioIsLoading.Visible = true;
                    portNumberTextBox.Enabled = false;
                    eventLogTextBox.Enabled = false;
                    eventLogPathButton.Enabled = false;
                    toolStripStatusLabelServerStatus.Text = "Server: RUNNING";
                    toolStripStatusLabelSimStatus.Text = "Simulation: LOADING";
                    buttonPauseSim.Enabled = false;
                    replayStartButton.Text = "Stop Replay";
                    replayLogBrowse.Enabled = false;
                    replayLogPathTextBox.Enabled = false;
                    buttonLoadScenario.Enabled = false;
                    textBoxGroupName.Enabled = false;
                    textBoxScenario.Enabled = false;
                    buttonScenario.Enabled = false;
                    break;
                case "SCENARIO_LOAD_SUCCESS":
                    buttonLoadScenario.Enabled = true;
                    buttonPauseSim.Enabled = true;
                    toolStripStatusLabelSimStatus.Text = "Simulation: PAUSED";
                    buttonPauseSim.Text = "Start Simulation";
                    labelScenarioIsLoading.Visible = false;
                    buttonLoadScenario.Enabled = true;
                    buttonPauseSim.Enabled = true;
                    int time = server.CurrentTime;
                    textBox1.Text = String.Format("{0:HH:mm:ss}", ExtractTime(time));
                    UpdatePauseButton(true);
                    UpdateDMDisplay();
                    break;
                case "REPLAY_LOAD_FAILURE":
                case "FORK_REPLAY_LOAD_FAILURE":
                case "SCENARIO_LOAD_FAILURE":
                    MessageBox.Show(messageText);
                    openReplayLogToolStripMenuItem.Enabled = true;
                    //stop the server

                    LockToolbarButtons(false);

                    buttonLoadScenario.Text = "Load Scenario";

                    toolStripStatusLabelSimStatus.Text = "Simulation: STOPPED";
                    buttonPauseSim.Enabled = false;

                    //buttonViewChatGUI.Enabled = false;
                    replayLogBrowse.Enabled = true;
                    replayLogPathTextBox.Enabled = true;
                    replayStartButton.Enabled = true;
                    comboBoxReplaySpeed.Enabled = false;

                    replayStartButton.Enabled = true;
                    toolStripStatusLabelServerStatus.Text = "Server: STOPPED";
                    textBoxGroupName.Enabled = true;
                    portNumberTextBox.Enabled = true;
                    eventLogTextBox.Enabled = true;
                    eventLogPathButton.Enabled = true;
                    textBoxScenario.Enabled = true;
                    buttonScenario.Enabled = true;
                    buttonPauseSim.Enabled = false;
                    buttonLoadScenario.Enabled = true;
                    labelScenarioIsLoading.Visible = false;
                    break;
                case "REPLAY_LOAD_SUCCESS":
                    labelScenarioIsLoading.Visible = false;
                    replayStartButton.Enabled = true;
                    comboBoxReplaySpeed.Enabled = true;

                    buttonPauseSim.Enabled = true;

                    UpdateDMDisplay();
                    UpdatePauseButton(true);
                    break;
                
                default:
                    break;
            }
        }

        private void HandlePauseScenarioEvent()
        {
            UpdatePauseButton();
        }
        private void HandleResumeScenarioEvent()
        {
            UpdatePauseButton();
        }
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.Update();
                List<SimulationEvent> events = eventClient.GetEvents();
                foreach (SimulationEvent ev in events)
                {
                    switch (ev.eventType)
                    {
                        case "ServerState":
                            HandleServerStateEvent(((StringValue)ev["MessageType"]).value, ((StringValue)ev["MessageText"]).value);
                            break;
                        case "PauseScenario":
                            HandlePauseScenarioEvent();
                            break;
                        case "ResumeScenario":
                            HandleResumeScenarioEvent();
                            break;
                    }
                }
                UpdateDMDisplay();
                int time = server.CurrentTime;
                textBox1.Text = String.Format("{0:HH:mm:ss}", ExtractTime(time));

            }  
        }

        private void UpdateDMDisplay()
        {
            List<string> allDMs = HandshakeManager.GetAllDMs();
            if (allDMs != null)
            {//if this is null, we have a problem 

                foreach (string s in allDMs)
                {
                    string avail = HandshakeManager.GetDMAvailability(s);
                    string role = HandshakeManager.GetDMRole(s);
                    if (DMInfo.GetDMAvail(s) != avail)
                    {
                        switch (avail)
                        {
                            case "Available":
                            case "Taken":
                            case "Ready":
                                DMInfo.UpdateDMStatus(s, role, avail);
                                break;
                            default:
                                throw new Exception("Incorrect return value for DM availability");
                        }
                    }
                }
            }

        }

        private void replayLogBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                replayLogPathTextBox.Text = ofd.FileName;
            }
            if (replayLogPathTextBox.Text == server.Log)
            {
                MessageBox.Show("Your current replay log is set to the same file as the file you'll be replaying from.  To prevent error, change one of these two files before starting the server.");
            }
        }

        private void replayStartButton_Click(object sender, EventArgs e)
        {
            if (!forkReplayCheckbox.Checked)
            {

                if (serverState == "SCENARIO_STOPPED" || serverState == "REPLAY_STOPPED")
                {
                    server.SimControlLoadReplay(replayLogPathTextBox.Text, GetReplaySpeed());
                }
                else
                {
                    server.SimControlStopReplay();
                }
            }
            else
            {
                if (serverState == "SCENARIO_STOPPED" || serverState == "REPLAY_STOPPED")
                {
                    server.SimControlLoadForkReplay(textBoxScenario.Text, textBoxGroupName.Text, eventLogTextBox.Text, replayLogPathTextBox.Text, GetReplaySpeed());
                }
                else
                {
                    server.SimControlStopForkReplay();
                }
            }
           
        }

        private void replayLogPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(replayLogPathTextBox.Text))
            {
                replayStartButton.Enabled = true;
            }
        }

        private void buttonScenario_Click(object sender, EventArgs e)
        {
            string OFDXMLFilter = "XML File(*.xml)|*.xml";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = OFDXMLFilter;

            if (File.Exists(textBoxScenario.Text))
            {
                ofd.FileName = textBoxScenario.Text;
            }
            else
            {
                ofd.InitialDirectory = Form1.ApplicationPath;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxScenario.Text = ofd.FileName;
                ServerOptions.DefaultScenarioPath = textBoxScenario.Text;
                ServerOptions.WriteFile();
            }
            UpdateScenarioNameLabel();
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void aboutDDD40ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string header = "About Aptima DDD 4.2";

            MessageBox.Show(string.Format("{0}\r\n{1}\r\nFor more info: http://aptima.com/products/ddd", SimCoreServer.SimCoreServer.GetProductVersion(),
                SimCoreServer.SimCoreServer.GetCompileDate()), header, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }



        private double GetReplaySpeed()
        {
            double v = 1;
            switch (comboBoxReplaySpeed.SelectedIndex)
            {
                case 0:
                    v = 5;
                    break;
                case 1:
                    v = 4;
                    break;
                case 2:
                    v = 2.5;
                    break;
                case 3:
                    v = 1.5;
                    break;
                case 4:
                    v = 1;
                    break;
                case 5:
                    v = 0.75;
                    break;
                case 6:
                    v = 0.5;
                    break;
                case 7:
                    v = 0.25;
                    break;

            }
            return v;

        }

        private void comboBoxReplaySpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (server != null)
            {
                if (server.IsReady)
                    server.SetReplaySpeed(GetReplaySpeed());
            }
        }

        private void UpdateScenarioNameLabel()
        {
            if (ServerOptions.DefaultScenarioPath == null)
            {
                labelScenarioName.Text = string.Empty;
            }
            if (ServerOptions.DefaultScenarioPath == string.Empty)
            {
                return;
            }
            string str = ServerOptions.DefaultScenarioPath.Remove(0, ServerOptions.DefaultScenarioPath.LastIndexOf(@"\") + 1);
            labelScenarioName.Text = str.Remove(str.LastIndexOf('.'));
        }

        private void contactUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("{0}\r\n{1}", SimCoreServer.SimCoreServer.GetSupportEmail(),
                SimCoreServer.SimCoreServer.GetSupportPhoneNumber()), "Aptima DDD Support Information");

        }

        public static void OpenWebBrowser(string filePath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = filePath;
            process.Start();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Get Aptima.com help site, figure out how to get content for specific minor versions.

            string exePath = String.Format("\\\\{0}\\help\\help.html", ServerOptions.DDDClientPath);
            if (File.Exists(exePath))
            {
                OpenWebBrowser(exePath);
            }
            else if (!File.Exists(exePath))
            {
                MessageBox.Show("Missing the Aptima DDD 4.2 help file.  Please try using the internet version, or contact Aptima for a replacement file.");
            }

        }


        private void regToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Aptima").OpenSubKey("DDD 4.0");
            string dateVal = Convert.ToString(rKey.GetValue("ExpirationDate"));
        }

        private void toolStripUserAdministration_Click(object sender, EventArgs e)
        {
            UserAdministration ua = new UserAdministration();
            ua.ShowDialog();
        }

        private void eventLogPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                eventLogTextBox.Text = fbd.SelectedPath;
                ServerOptions.EventLogDirectory = eventLogTextBox.Text;
                ServerOptions.WriteFile();
            }
        }

        private void SetLicenseControlledControls(bool enabled)
        {
            eventLogPathButton.Enabled = enabled;
            buttonScenario.Enabled = enabled;
            buttonLoadScenario.Enabled = enabled;
            replayStartButton.Enabled = enabled;
            replayLogBrowse.Enabled = enabled;
            replayStartButton.Enabled = enabled;

        }

        private void openReplayLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = string.Format("{0}\\LogFileViewer.exe", Application.StartupPath); ;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error loading LogFileViewer: {0}", ex.Message));

            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void UpdateVoiceServerButton()
        {
            if (ServerOptions.EnableVoiceServer)
            {
                buttonToggleVoice.Text = "Disable Voice Comm";
            }
            else
            {
                buttonToggleVoice.Text = "Enable Voice Comm";
            }
            updatingCheckBox = true;
            checkBoxRecordSimVoices.Checked = ServerOptions.EnableVoiceServerRecordings;
            updatingCheckBox = false;
        }

        private void buttonToggleVoice_Click(object sender, EventArgs e)
        {
            ServerOptions.EnableVoiceServer = !ServerOptions.EnableVoiceServer;
            UpdateVoiceServerButton();
        }

        private bool updatingCheckBox = false;

        private void checkBoxRecordSimVoices_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingCheckBox)
            {
                ServerOptions.EnableVoiceServerRecordings = !ServerOptions.EnableVoiceServerRecordings;

                UpdateVoiceServerButton();
            }
        }

        //End State methods
    }
}