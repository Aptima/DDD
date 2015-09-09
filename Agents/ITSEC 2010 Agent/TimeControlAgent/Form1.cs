using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.DDDAgentFramework;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using System.Threading;

namespace TimeControlAgent
{
    public partial class Form1 : Form
    {
        private delegate void SingleStringDelegate(String s);

        //wait lockables
        private static bool _isCurrentlyWaiting = false;
        //

        //speed Lockables
        private static String _currentSpeedMultiplier = "1x";
        private static String _waitUntilThisTime = "-00:00:00";
        //

        private String _currentTime = "00:00:00";
        private String _dddHostname = "";
        private int _dddPort = -1;
        private DDDServerConnection _connection;
        private Thread _dddLoopThread = null;

        private static Object WaitLock = new Object();
        private static Object SpeedLock = new Object();

        private static bool IsCurrentlyWaiting
        {
            get { lock (WaitLock) { return _isCurrentlyWaiting; } }
            set { lock (WaitLock) { _isCurrentlyWaiting = value; } }
        }

        public Form1()
        {
            InitializeComponent();
            comboBoxSpeedMultiplier.SelectedIndex = 0;
        }

        private void DDDLoop()
        {
            bool isConnected = true;
            lock (_connection)
            {//lock so you can disconnect on the main thread
                isConnected = _connection.IsConnected();
            }

            while (isConnected)
            {
                _connection.ProcessEvents();
                Thread.Sleep(500);
                try
                {
                    lock (_connection)
                    {
                        isConnected = _connection.IsConnected();
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                }
            }

            //MessageBox.Show("DDD Loop Terminated");
        }

        public void UpdateDDDSimTime(String simTime)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(UpdateDDDSimTime), simTime);
            }
            else
            {
                toolStripStatusLabelDDDSimTime.Text = simTime;
            }

        }

        public void UpdateCurrentSimSpeed(String simSpeed)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(UpdateCurrentSimSpeed), simSpeed);
            }
            else
            {
                labelCurrentSpeed.Text = simSpeed;
            }
        }

        public void UpdateCurrentSimStopTime(String stopTime)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(UpdateCurrentSimStopTime), stopTime);
            }
            else
            {
                labelCurrentEndTime.Text = stopTime;
            }
        }

        public void UpdateDDDConnectionStatus(String connectionStatus)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(UpdateDDDConnectionStatus), connectionStatus);
            }
            else
            {
                labelDDDStatus.Text = connectionStatus;
            }
        }

        public void ResetDDDConnectionUI(String nothing)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(ResetDDDConnectionUI), nothing);
            }
            else
            {
                buttonConnect.Enabled = true;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
            }
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == radioButtonTicks && radioButtonTicks.Checked)
            {
                textBoxStopSeconds.Enabled = true;
                dateTimePickerStopTime.Enabled = false;
            }
            else
            {
                textBoxStopSeconds.Enabled = false;
                dateTimePickerStopTime.Enabled = true;
            }
        }

        private bool DisconnectCurrentDDDSession(bool promptUser)
        {
            if (promptUser)
            {
                if (MessageBox.Show("A connection with the DDD currently exists, do you want to terminate this connection?", "Terminate DDD Connection", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;
            }
            if (_connection == null)
                return true;
            lock (_connection)
            {
                try
                {
                    _connection.Disconnect();
                }
                finally
                {
                    _connection = null;
                }
            }

            return true;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _dddHostname = textBoxHostname.Text;
            if (Int32.TryParse(textBoxPort.Text, out _dddPort) == false)
            {
                return;
            }
            try
            {
                if (_connection != null)
                {
                    lock (_connection)
                    {
                        if (_connection.IsConnected())
                        {
                            if (!DisconnectCurrentDDDSession(true))
                            {
                                throw new Exception("Unable to disconnect from DDD Server");
                            }
                        }
                    }
                }

                _connection = new DDDServerConnection();
                string remoteSimulationModel = String.Format(@"\\{0}\DDDClient\SimulationModel.xml", _dddHostname);
                
                lock (_connection)
                {
                    bool simModelResult = _connection.ReadSimModel(remoteSimulationModel);

                    if (!simModelResult)
                    {
                        MessageBox.Show(String.Format("Error in DDD Connection: Failed to read the simulation model at '{0}', please try again.", remoteSimulationModel), "DDD Connection Error");

                        //AD: Here we could also ask the user to point to a local copy of the sim model, then re-ReadSimModel.

                        return;
                    }
                    if (!_connection.ConnectToServer(_dddHostname, _dddPort))
                    {
                        throw new Exception("Connection to DDD failed");
                    }

                    //need Login as DM?  no?
                    _connection.RequestPlayers();
                    List<string> decisionMakers = new List<string>();
                    while (decisionMakers.Count == 0)
                    {
                        decisionMakers = _connection.Players;
                        _connection.ProcessEvents();
                    }
                    string selectedDM = decisionMakers[0]; ;
                    _connection.LoginPlayer(selectedDM, "OBSERVER");
                    _connection.GetDMView(selectedDM); //initialize view...
                    //

                    _connection.AddEventCallback("TimeTick", new DDDServerConnection.ProcessSimulationEvent(TimeTick));
                    _connection.AddEventCallback("ExternalApp_SimStart", new DDDServerConnection.ProcessSimulationEvent(ExternalApp_SimStart));
                    _connection.AddEventCallback("ExternalApp_SimStop", new DDDServerConnection.ProcessSimulationEvent(ExternalApp_SimStop));
                    _connection.AddEventCallback("GameSpeed", new DDDServerConnection.ProcessSimulationEvent(GameSpeed));

                }
                try
                {
                    //just in case
                    _dddLoopThread.Abort();
                    _dddLoopThread = null;
                }
                catch (Exception ex)
                { }

                _dddLoopThread = new Thread(new ThreadStart(DDDLoop));
                _dddLoopThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to DDD Server:\r\n" + ex.Message);
                return;
            }

            UpdateDDDConnectionStatus("CONNECTED");
            ((Button)sender).Enabled = false;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
        }

        private void TimeTick(SimulationEvent ev)
        {
            String simTime = ((StringValue)ev["SimulationTime"]).value;
            UpdateDDDSimTime(simTime);
            lock (_currentTime)
            {
                _currentTime = simTime;
            }


            if (!IsCurrentlyWaiting)
                return;

            String waitTil = "";
            lock (SpeedLock)
            {
                waitTil = _waitUntilThisTime;
            }
            if (waitTil == simTime)
            {
                SendGameSpeedRequest("1x");//back to normal
            }
        }

        private void SendGameSpeedRequest(String multiplier)
        {
            int mult = 1;
            multiplier = multiplier.Replace('x', ' ').Trim();

            if (Int32.TryParse(multiplier, out mult))
            {
                lock (_connection)
                {
                    
                    _connection.SendGameSpeedRequest((double)mult);
                }
            }
        }
        private void GameSpeed(SimulationEvent ev)
        {
            double speedFactor = ((DoubleValue)ev["SpeedFactor"]).value;
            int iSpeedFactor = Convert.ToInt32(speedFactor);
            lock (SpeedLock)
            {
                UpdateCurrentSimSpeed(String.Format("{0}x", iSpeedFactor));
                if (iSpeedFactor == 1)
                {
                    UpdateCurrentSimStopTime("----");
                    IsCurrentlyWaiting = false;
                }
                else
                {
                    UpdateCurrentSimStopTime(_waitUntilThisTime);
                }
            }
        }
        private void ExternalApp_SimStart(SimulationEvent ev)
        { }
        private void ExternalApp_SimStop(SimulationEvent ev)
        {
            MessageBox.Show("DDD Server has stopped");
            DisconnectCurrentDDDSession(false);

            //try
            //{
            //    //just in case
            //    if (_dddLoopThread.IsAlive)
            //    {
            //        _dddLoopThread.Abort();
            //        _dddLoopThread = null;
            //    }
            //}
            //catch (Exception ex)
            //{ }

            UpdateDDDConnectionStatus("NOT CONNECTED");
            ResetDDDConnectionUI("");
        
        }

        private void buttonSetSpeed_Click(object sender, EventArgs e)
        {
            String waitTil = "";
            if (radioButtonTime.Checked)
            {
                //check if it's in the future
                waitTil = dateTimePickerStopTime.Text;
            }
            else
            {
                int seconds = Int32.Parse(textBoxStopSeconds.Text);
                int hrs = 0;
                int mins = 0;
                int secs = 0;
                if (seconds >= 3600)
                {
                    hrs = seconds / 3600;
                    seconds -= hrs * 3600;
                }
                if (seconds >= 60)
                {
                    mins = seconds / 60;
                    seconds -= mins * 60;
                }
                secs = seconds;
                lock (_currentTime)
                {
                    hrs += Int32.Parse(_currentTime.Split(new char[] { ':' })[0]);
                    mins += Int32.Parse(_currentTime.Split(new char[] { ':' })[1]);
                    secs += Int32.Parse(_currentTime.Split(new char[] { ':' })[2]);
                }
                waitTil = String.Format("{0}:{1}:{2}", hrs, mins, secs);
            }

            lock (SpeedLock)
            {
                _currentSpeedMultiplier = comboBoxSpeedMultiplier.Text;
                _waitUntilThisTime = waitTil;
                IsCurrentlyWaiting = true;
                SendGameSpeedRequest(_currentSpeedMultiplier);
            }
        }

        private void buttonClearSpeed_Click(object sender, EventArgs e)
        {
            //revert back to 1x speed?
            //just remove the current wait until time?

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connection != null)
            {
                if (_connection.IsConnected())
                {
                    if (!DisconnectCurrentDDDSession(true))
                    { e.Cancel = true; }
                    else
                    {
                        //eh?
                    }
                }
            }
        }
    }
}
