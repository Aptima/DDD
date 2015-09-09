using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.TestStubs.EventGeneratorTestGUI
{


    enum NetworkState
    {
        CONNECTED,
        DISCONNECTED
    }
    enum ClockState
    {
        RUNNING,
        STOPPED
    }
    public partial class Form1 : Form
    {
        private int simulationTime;
        private int updateFrequency;
        private NetworkState netState;
        private ClockState clockState;
        private OpenFileDialog ofd;
        private OpenFileDialog ofd2;
        private SaveFileDialog sfd;
        
        

        public NetworkClient netClient;

        private SimulationModelInfo simModel;
        public Form1()
        {
            netState = NetworkState.DISCONNECTED;
            clockState = ClockState.STOPPED;
            simulationTime = 0;
            updateFrequency = 1000;
            InitializeComponent();
            ofd = new OpenFileDialog();
            ofd.Filter = "XML File(*.xml)|*.xml";
            ofd2 = new OpenFileDialog();
            ofd2.Filter = "Text File(*.txt)|*.txt";
            sfd = new SaveFileDialog();

            simModel = null;
            netClient = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SimulationModelEnable();
            NetworkInitialize();
            EventsInitialize();
            
        }

        private void EventsInitialize()
        {
            loadEventsButton.Enabled = false;
            saveEventsButton.Enabled = false;
            sendEventButton.Enabled = false;
            manualCheckBox.Enabled = false;
            eventsListBox.Enabled = false;
        }
        private void EventsEnable()
        {
            loadEventsButton.Enabled = true;
            saveEventsButton.Enabled = true;
            eventsListBox.Enabled = true;
        }

        private void SetSimulationModel()
        {
            string name = simulationModelTextBox.Text;
            if (System.IO.File.Exists(name))
            {
                SimulationModelReader r = new SimulationModelReader();
                simModel = r.readModel(name);
                updateFrequency = simModel.GetUpdateFrequency();
                NetworkEnable();
                EventsEnable();
                SimulationModelDisable();
                
            }
            else
            {
                MessageBox.Show("Invalid simulation model file!");
            }
        }
        private void SimulationModelEnable()
        {
            simulationModelTextBox.Enabled = true;
            simulationModelButton.Text = "Set";
            simulationModelButton.Enabled = true;
            simModelBrowseButton.Enabled = true;
        }

        private void SimulationModelDisable()
        {
            simulationModelTextBox.Enabled = false;
            //simulationModelButton.Text = "---";
            simulationModelButton.Enabled = false;
            simModelBrowseButton.Enabled = false;
        }
        private void ClockInitialize()
        {
            startButton.Text = "Start";
            startButton.Enabled = false;
            resetButton.Text = "Reset";
            resetButton.Enabled = false;
            
            clockTextBox.Enabled = false;
            manualCheckBox.Enabled = false;
            ClockReset();
            FormUpdate();
        }

        private void ClockEnable()
        {
            startButton.Text = "Start";
            startButton.Enabled = true;
            resetButton.Text = "Reset";
            resetButton.Enabled = true;
            clockTextBox.Text = (simulationTime / 1000).ToString();
            clockTextBox.Enabled = true;
            manualCheckBox.Enabled = true;
        }

        private void ClockStart()
        {
            switch (clockState)
            {
                case ClockState.STOPPED:
                    clockState = ClockState.RUNNING;
                    startButton.Text = "Pause";
                    clockTimer.Interval = updateFrequency;
                    clockTimer.Start();
                    SimulationEvent tick = SimulationEventFactory.BuildEvent(ref simModel, "TimeTick");
                    ((IntegerValue)tick["Time"]).value = simulationTime;
                    netClient.PutEvent(tick);

                    manualCheckBox.Enabled = false;

                    break;
                case ClockState.RUNNING:
                    clockState = ClockState.STOPPED;
                    startButton.Text = "Start";
                    clockTimer.Stop();
                    break;

            }
        }

        private void ClockReset()
        {
            simulationTime = 0;
            FormUpdate();
            clockState = ClockState.RUNNING;
            manualCheckBox.Enabled = true;
            ClockStart();
        }


        private void NetworkInitialize()
        {
            hostnameTextBox.Text = System.Net.Dns.GetHostName();
            hostnameTextBox.Enabled = false;
            portTextBox.Text = "9999";
            portTextBox.Enabled = false;
            connectButton.Text = "Connect";
            connectButton.Enabled = false;
            connectTextBox.Text = netState.ToString();
            netState = NetworkState.DISCONNECTED;

            if (netClient != null)
            {
                netClient.Disconnect();
                netClient = null;
            }

            netClient = new NetworkClient();

            ClockInitialize();
        }

        private void NetworkEnable()
        {
            hostnameTextBox.Text = System.Net.Dns.GetHostName();
            hostnameTextBox.Enabled = true;
            portTextBox.Text = "9999";
            portTextBox.Enabled = true;
            connectButton.Text = "Connect";
            connectButton.Enabled = true;
            connectTextBox.Text = netState.ToString();
            netState = NetworkState.DISCONNECTED;
            //ClockInitialize();
        }

        private void NetworkConnect()
        {
            if (netClient.Connect(hostnameTextBox.Text, Int32.Parse(portTextBox.Text)))
            {
                netState = NetworkState.CONNECTED;
                hostnameTextBox.Enabled = false;
                portTextBox.Enabled = false;
                connectButton.Text = "Disconnect";
                connectTextBox.Text = netState.ToString();
                ClockEnable();
                
                networkCheckTimer.Start();
            }
            else
            {
                NetworkInitialize();
                NetworkEnable();
                MessageBox.Show("Unable to connect to server.");
            }
            FormUpdate();
        }
        private void NetworkDisconnect()
        {
            NetworkInitialize();
            NetworkEnable();
            FormUpdate();
            networkCheckTimer.Stop();
            
        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            switch (netState)
            {
                case NetworkState.DISCONNECTED:
                    NetworkConnect();
                    break;

                case NetworkState.CONNECTED:
                    NetworkDisconnect();
                    break;
            }
        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {

            simulationTime += updateFrequency;
            SimulationEvent tick = SimulationEventFactory.BuildEvent(ref simModel, "TimeTick");
            ((IntegerValue)tick["Time"]).value = simulationTime;
            netClient.PutEvent(tick);

            EventListBoxItem lbi = null;
            if (!manualCheckBox.Checked)
            {
                while (eventsListBox.Items.Count > 0)
                {
                    lbi = (EventListBoxItem) eventsListBox.Items[0];
                    if (((IntegerValue)lbi.simEvent["Time"]).value >= simulationTime &&
                        ((IntegerValue)lbi.simEvent["Time"]).value < (simulationTime+updateFrequency))
                    {
                        eventsListBox.Items.Remove(lbi);
                        netClient.PutEvent(lbi.simEvent);
                    }
                    else
                    {
                        break;
                    }
                }
            }




            FormUpdate();

        }
        private void FormUpdate()
        {
            clockTextBox.Text = (simulationTime / 1000).ToString();
            connectTextBox.Text = netState.ToString();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ClockStart();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ClockReset();
        }

        private void simulationModelButton_Click(object sender, EventArgs e)
        {
            SetSimulationModel();
        }

        private void simModelBrowseButton_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                simulationModelTextBox.Text = ofd.FileName;
            }
        }

        private void networkCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!netClient.IsConnected())
            {
                NetworkDisconnect();
            }
        }

        private void loadEventsButton_Click(object sender, EventArgs e)
        {
            if (ofd2.ShowDialog() == DialogResult.OK)
            {

                StreamReader re = File.OpenText(ofd2.FileName);
                string input = null;
                SimulationEvent ev = null;
                eventsListBox.Items.Clear();
                while ((input = re.ReadLine()) != null)
                {
                    ev = SimulationEventFactory.XMLDeserialize(input);
                    if (SimulationEventFactory.ValidateEvent(ref simModel, ev))
                    {
                        eventsListBox.Items.Add(new EventListBoxItem(ev));
                    }
                    else
                    {
                        MessageBox.Show("event file contains invalid event: \""+input+"\"");
                    }
                }
                re.Close();
            }
        }

        private void saveEventsButton_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter wr = new StreamWriter(sfd.FileName);
                foreach (EventListBoxItem lbi in eventsListBox.Items)
                {
                    wr.WriteLine(SimulationEventFactory.XMLSerialize(lbi.simEvent));
                }
                wr.Flush();
                wr.Close();
            }
        }

        private void sendEventButton_Click(object sender, EventArgs e)
        {
            EventListBoxItem lbi = (EventListBoxItem)eventsListBox.Items[eventsListBox.SelectedIndex];
            ((IntegerValue)lbi.simEvent["Time"]).value = simulationTime;
            netClient.PutEvent(lbi.simEvent);
            eventsListBox.Items.Remove(lbi);
        }

        private void manualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (manualCheckBox.Checked)
            {
                sendEventButton.Enabled = true;
            }
            else
            {
                sendEventButton.Enabled = false;
            }
        }





    }

    internal class EventListBoxItem
    {
        public SimulationEvent simEvent;
        public EventListBoxItem(SimulationEvent se)
        {
            simEvent = se;
        }
        public override string ToString()
        {
            string s = "";

            s = simEvent.eventType + "(";
            s += "Time: " + ((IntegerValue)simEvent["Time"]).value.ToString();
            s += ")";
            return s;
        }
    }
}