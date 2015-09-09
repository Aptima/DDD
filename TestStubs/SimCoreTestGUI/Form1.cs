using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Aptima.Asim.DDD.SimulationEngine;
using Aptima.Asim.DDD.CommonComponents.SimCoreTools;
using Aptima.Asim.DDD.SimCoreServer;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ReplayLogger;

namespace Aptima.Asim.DDD.TestStubs.SimCoreTestGUI
{

    

    public partial class Form1 : Form
    {

        private static string OFDXMLFilter = "XML File(*.xml)|*.xml";
        private static string OFDXSDFilter = "Schema File(*.xsd)|*.xsd";
        private static string OFDTXTFilter = "Text File(*.txt)|*.txt";
        private static OpenFileDialog ofd = new OpenFileDialog();
        private static SaveFileDialog sfd = new SaveFileDialog();
        private static BlackboardClient bbClient = null;
        private static SimEngine simEngine = new SimEngine();

        private static bool paused = false;
        private static double replaySpeed = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ofd.Filter = "XML File(*.xml)|*.xml";
            UpdateForm();
            timer1.Start();
            
        }

        private void UpdateForm()
        {
            statusTextBox.Text = simEngine.simCore.GetState().ToString();
            ObjectCheckBox();
            switch (simEngine.simCore.GetState())
            {
                case SimCoreState.UNINITIALIZED:
                    stateButton.Text = "Start";
                    break;
                case SimCoreState.RUNNING:
                    stateButton.Text = "Stop";
                    //UpdateTree();
                    UpdateObjectsList();
                    timeTextBox.Text = (simEngine.simCore.GetSimTime() / 1000.0).ToString();
                    break;
                case SimCoreState.STOPPING:
                    stateButton.Text = "----";
                    UpdateObjectsList();
                    break;
            }

            connectionsListBox.Items.Clear();
            Dictionary<int, string> clientDesc = simEngine.simCore.GetClientDescriptions();
            string desc;
            foreach (int i in clientDesc.Keys)
            {
                desc = "Client " + i.ToString() + " IP: " + clientDesc[i];
                connectionsListBox.Items.Add(desc);
            }

            
        }
        private void UpdateObjectsList()
        {
            if (!objectStatus.Checked)
            {
                return;
            }
            if (bbClient == null)
            {
                objectListBox.Items.Clear();
                return;
            }
            Dictionary<string, SimulationObjectProxy> objectPoxies = bbClient.GetObjectProxies();

            //objectListBox.Items.Clear();

            foreach (string id in objectPoxies.Keys)
            {
                if (!objectListBox.Items.Contains(id))
                {
                    objectListBox.Items.Add(id);
                }
            }

            List<string> idsToRemove = new List<string>();

            foreach (string id2 in objectListBox.Items)
            {
                if (!objectPoxies.ContainsKey(id2))
                {
                    idsToRemove.Add(id2);
                    //objectListBox.Items.Remove(id2);
                }
            }
            foreach (string id4 in idsToRemove)
            {
                objectListBox.Items.Remove(id4);
            }
            
            //objectAttributeList.Items.Clear();
            //treeView1.Nodes.Clear();

            foreach (TreeNode tn in treeView1.Nodes)
            {
                if (tn.Name != (string)objectListBox.SelectedItem)
                {
                    treeView1.Nodes.Remove(tn);
                }
            }
            SimulationObjectProxy prox = null;
            if (objectListBox.SelectedItem != null)
            {
                string id3 = (string)objectListBox.SelectedItem;
                if (objectPoxies.ContainsKey(id3))
                {
                    prox = objectPoxies[id3];
                    string objectType = prox.GetObjectType();
                    Dictionary<string, string> treeData = new Dictionary<string, string>();
                    string s;
                    DataValue dv;
                    foreach (string key in prox.GetKeys())
                    {
                        dv = prox[key].GetDataValue();
                        s = DataValueFactory.ToString(dv);
                        treeData[key] = key + " = " + s;
                    }


                    if (treeView1.Nodes.ContainsKey(id3))
                    {

                        foreach (string key in treeData.Keys)
                        {
                            treeView1.Nodes[id3].Nodes[key].Text = treeData[key];
                        }
                    }
                    else
                    {
                        treeView1.Nodes.Add(id3.ToString(), objectType + ": " + id3);

                        foreach (string key in treeData.Keys)
                        {
                            treeView1.Nodes[id3.ToString()].Nodes.Add(key, treeData[key]);
                        }
                        treeView1.Nodes[id3.ToString()].Expand();
                    }
                }
                

            }

            //treeView1.Update();
            //treeView1.Refresh();
            
        }
        /*
        private void UpdateTree()
        {
            if (bbClient == null)
            {
                return;
            }
            Dictionary<string, SimulationObjectProxy> objectPoxies = bbClient.GetObjectProxies();

            foreach (string id in objectPoxies.Keys)
            {
                if (treeView1.Nodes.ContainsKey(id))
                {
                    foreach (string key in objectPoxies[id].GetKeys())
                    {
                        treeView1.Nodes[id].Nodes[key].Text = key + " = " + DataValueFactory.ToString(objectPoxies[id][key].GetDataValue());
                    }
                }
                else
                {
                    treeView1.Nodes.Add(id.ToString(), "Object " + id);
                    foreach (string key in objectPoxies[id].GetKeys())
                    {
                        treeView1.Nodes[id.ToString()].Nodes.Add(key, key + " = " + DataValueFactory.ToString(objectPoxies[id][key].GetDataValue()));
                    }
                }
            }
            treeView1.Update();
        }*/


        private void SubscribeToObject(string objectType)
        {
            foreach (string n in simEngine.simCore.simModelInfo.objectModel.objects[objectType].attributes.Keys)
            {
                bbClient.Subscribe(objectType, n, true, false);
            }
        }
        private void Start()
        {
            if (System.IO.File.Exists(simulationModelTextBox.Text))
            {
                simEngine.Initialize(simulationModelTextBox.Text, Int32.Parse(serverPortTextBox.Text));
                simEngine.StartSimCore();
                hostnameTextBox.Text = simEngine.GetServerHostName();
                UpdateForm();
                bbClient = simEngine.GetBlackboardClient();



                
                SubscribeToObject("PhysicalObject");
                SubscribeToObject("DecisionMaker");
                SubscribeToObject("Team");
                SubscribeToObject("SensorNetwork");
                SubscribeToObject("LandRegion");
                SubscribeToObject("ScoringRegion");
                SubscribeToObject("ActiveRegion");


                simulationModelButton.Enabled = false;
                simulationModelTextBox.Enabled = false;
                serverPortTextBox.Enabled = false;

                //start logging thread
                if (checkBoxReplayLog.Checked)
                {
                    string logPath = "log.txt";
                    string logMode = "Detailed";
                    if (textBoxReplayLog.Text != "")
                        logPath = textBoxReplayLog.Text;
                    if (radioButtonLimited.Checked)
                        logMode = "Limited";

                    simEngine.StartReplayLogger(logPath, logMode,SimCoreServer.SimCoreServer.GetProductVersion(),SimCoreServer.SimCoreServer.GetCompileDate());

                }
                checkBoxReplayLog.Enabled = false;
                startReplayButton.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("Must set simulation model file first");
                
            }
        }
        private void Stop()
        {
            simEngine.Stop();
            UpdateForm();
            bbClient = null;
            ObjectCheckBox();
            
            simulationModelButton.Enabled = true;
            simulationModelTextBox.Enabled = true;
            serverPortTextBox.Enabled = true;
            //stop logging thread
            if (checkBoxReplayLog.Checked)
                simEngine.StopReplayLogger();
            checkBoxReplayLog.Enabled = true;
            //historyLogger.StopThread();
            startReplayButton.Enabled = false;
        }

        private void stateButton_Click(object sender, EventArgs e)
        {
            switch (simEngine.simCore.GetState())
            {
                case SimCoreState.RUNNING:
                    Stop();
                    scenarioStartButton.Enabled = false;
                    //startReplayButton.Enabled = false;
                    break;
                case SimCoreState.UNINITIALIZED:
                    Start();
                    scenarioStartButton.Enabled = true;
                    //startReplayButton.Enabled = true;
                    break;
                case SimCoreState.STOPPING:
                    break;
            }
        }

        private void simulationModelButton_Click(object sender, EventArgs e)
        {
            ofd.Filter = OFDXMLFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                simulationModelTextBox.Text = ofd.FileName;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void scenarioBrowseButton_Click(object sender, EventArgs e)
        {
            ofd.Filter = OFDXMLFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                scenarioTextBox.Text = ofd.FileName;
            }
        }

        private void scenarioStartButton_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(schemaTextBox.Text))
            {
                MessageBox.Show("Schema file must exist.");
                return;
            }
            if (scenarioTextBox.Text == "")
            {
                MessageBox.Show("Must select a scenario file");
                return;
            }
            simEngine.StartScenCon(schemaTextBox.Text,scenarioTextBox.Text, 0, @"C:\",null);
            scenarioStartButton.Enabled = false;
            startReplayButton.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(simEngine.simCore.GetState() == SimCoreState.RUNNING)
            {
                Stop();
            }
         
        }

        private void schemaBrowseButton_Click(object sender, EventArgs e)
        {
            ofd.Filter = OFDXSDFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                schemaTextBox.Text = ofd.FileName;
            }
        }

        private void checkBoxReplayLog_CheckedChanged(object sender, EventArgs e)
        {
            bool state = checkBoxReplayLog.Checked;
            textBoxReplayLog.Enabled = state;
            buttonReplayLog.Enabled = state;
            groupBoxLogDetail.Enabled = state;
        }

        private void buttonReplayLog_Click(object sender, EventArgs e)
        {
            sfd.Filter = OFDTXTFilter;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBoxReplayLog.Text = sfd.FileName;
            }
        }

        private void ObjectCheckBox()
        {
            if (objectStatus.Checked)
            {
                objectListBox.Enabled = true;
                treeView1.Enabled = true;
            }
            else
            {
                objectListBox.Items.Clear();
                objectListBox.Enabled = false;
                treeView1.Nodes.Clear();
                treeView1.Enabled = false;
            }
            treeView1.Refresh();
        }
        private void objectStatus_CheckedChanged(object sender, EventArgs e)
        {
            ObjectCheckBox();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            paused = !paused;
            if (paused)
            {
                pauseButton.Text = "Resume";
                simEngine.Pause();
            }
            else
            {
                pauseButton.Text = "Pause";
                simEngine.Resume();
            }
        }

        private void replayBrowseButton_Click(object sender, EventArgs e)
        {
            ofd.Filter = OFDTXTFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                replayTextBox.Text = ofd.FileName;
            }
        }

        private void startReplayButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(replayTextBox.Text))
            {
                try
                {
                    simEngine.StartReplay(replayTextBox.Text, replayLoopCheckbox.Checked, replaySpeed);
                    startReplayButton.Enabled = false;
                    scenarioStartButton.Enabled = false;
                }
                catch (Exception except){
                    
                        throw except;
                    }
            }
        }

        private void speedRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 0.25;
            replaySpeedLabel.Text = "Speed: 1/4";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000);
        }
        private void speedRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 0.50;
            replaySpeedLabel.Text = "Speed: 1/2";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000);
        }
        private void speedRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 1;
            replaySpeedLabel.Text = "Speed: 1";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000 / replaySpeed);
        }

        private void speedRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 2;
            replaySpeedLabel.Text = "Speed: 2";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000 / replaySpeed);
        }

        private void speedRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 4;
            replaySpeedLabel.Text = "Speed: 4";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000 / replaySpeed);
        }

        private void speedRadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            replaySpeed = 8;
            replaySpeedLabel.Text = "Speed: 8";
            simEngine.SetGameSpeed(replaySpeed);
            timer1.Interval = (int)(1000 / replaySpeed);
        }


    }
    internal class AttListBoxItem
    {
        public string name;
        public string value;
        public AttListBoxItem(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name + " = " + value;
        }
    }
}