using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.DDDAgentFramework;

namespace QA_Hardcoded_Tester
{
    public partial class MainForm : Form
    {
        private String _hostname = "";
        private int _port = 0;
        private DDDServerConnection _connection = null;
        private string _actingDMName = "Task Force C";

        public MainForm(String hostname, int port)
        {
            InitializeComponent();
            _hostname = hostname;
            _port = port;

            _connection = new DDDServerConnection();
            

        }

        public bool initialize()
        { 
            if (!_connection.ConnectToServer(_hostname, _port))
            {
                MessageBox.Show(this, "Connection to server failed; Exiting...");
                return false;
            }
            _connection.ReadSimModel(String.Format("\\\\{0}\\DDDClient\\SimulationModel.xml", _hostname));
            _connection.SetLocalPlayer(_actingDMName);
           // _connection.LoginPlayer(_actingDMName, "OBSERVER");
            this.Text = String.Format("QA Test Form (Connected to {0}:{1})", _hostname, _port);
            this.toolStripStatusLabelServerStatus.Text = "CONNECTED";

            return true;
        }

        private void UpdateTextBox(String message)
        {
            textBoxResults.Text = message + "\r\n" + textBoxResults.Text;
        }

        private void Engage(String[] targetIDs, String attackerID, String capabilityName, int percentageApplied)
        {
            int x = 0;
            try
            {
                for (x = 0; x < targetIDs.Length; x++)
                {
                    _connection.SendAttackObject(attackerID, targetIDs[x], capabilityName, percentageApplied);
                }
            }
            catch (Exception e)
            { 
                UpdateTextBox(String.Format("Error in Engage({0},{1},{2}) -> {3}", targetIDs[x],attackerID,capabilityName,e.Message));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //engage group 1 at 33%
            Engage(new String[] { "TargetAsset_0", "TargetAsset_1", "ToughTargetAsset_2" }, "SSN-1", "STRK", 33);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //engage group 2 at 25%
            Engage(new String[] { "TargetAsset_3", "TargetAsset_4", "ToughTargetAsset_5" }, "SSN-1", "STRK", 25);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //engage group 3 at 25%
            Engage(new String[] { "TargetAsset_6", "TargetAsset_7", "ToughTargetAsset_8" }, "SSN-1", "STRK", 25);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //engage group 4 at 33%
            Engage(new String[] { "TargetAsset_9", "TargetAsset_10", "ToughTargetAsset_11" }, "SSN-1", "STRK", 33);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //update group 4 vulnerabilities
            string[] group4 = new String[] { "TargetAsset_9", "TargetAsset_10", "ToughTargetAsset_11" };
            List<string> states = new List<string>(new string[]{"Completed", "Unknown"});
            Dictionary<int, List<string>> capabilities = new Dictionary<int,List<string>>();
            Dictionary<int, List<double>> ranges = new Dictionary<int,List<double>>();
            Dictionary<int, List<int>> intensities = new Dictionary<int,List<int>>();
            Dictionary<int, List<double>> probabilities = new Dictionary<int, List<double>>();

            for (int y = 0; y < states.Count; y++)
            {
                if (!capabilities.ContainsKey(y))
                    capabilities.Add(y, new List<string>());
                if (!ranges.ContainsKey(y))
                    ranges.Add(y, new List<double>());
                if (!intensities.ContainsKey(y))
                    intensities.Add(y, new List<int>());
                if (!probabilities.ContainsKey(y))
                    probabilities.Add(y, new List<double>());
            }

            //transition to completed
            capabilities[0].Add("STRK");
            ranges[0].Add(0);
            intensities[0].Add(1);
            probabilities[0].Add(100);

            //transition to unknown
            capabilities[1].Add("C2");
            ranges[1].Add(0);
            intensities[1].Add(1);
            probabilities[1].Add(100);


            try
            {
                for (int x = 0; x < group4.Length; x++)
                {
                    _connection.UpdateObjectVulnerabilities(group4[x], states, capabilities, ranges, intensities, probabilities);
                }
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error updating group 4 vulnerabilities: {0}", ex.Message));
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //update SSN1's capabilities
            List<String> capabilities = new List<string>();
            List<double> ranges = new List<double>();
            List<int> intensities = new List<int>();
            List<double> probabilities = new List<double>();

            capabilities.Add("STRK");
            ranges.Add(1000);
            intensities.Add(8);
            probabilities.Add(100);

            capabilities.Add("C2");
            ranges.Add(1000);
            intensities.Add(1);
            probabilities.Add(100);

            capabilities.Add("BDA");
            ranges.Add(1000);
            intensities.Add(1);
            probabilities.Add(100);

            try
            {
                _connection.UpdateObjectCapabilities("SSN-1", capabilities, ranges, intensities, probabilities);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error updating SSN-1's capabilities: {0}", ex.Message));
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //change attack window with task 1
            try
            {
                _connection.SendUpdateAttackTimeWindowEvent("SSN-1", "TargetAsset_1", "STRK", 20000);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating Attack Window: {0}", ex.Message));
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //change attack window with task 6
            try
            {
                _connection.SendUpdateAttackTimeWindowEvent("SSN-1", "TargetAsset_6", "STRK", 20000);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating Attack Window: {0}", ex.Message));
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //cancel attack with task 0
            try
            {
                _connection.SendCancelEngagementEvent("SSN-1", "TargetAsset_0", "STRK");
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Cancelling Attack on task 0: {0}", ex.Message));
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //cancel attack with task 7
            try
            {
                _connection.SendCancelEngagementEvent("SSN-1", "TargetAsset_7", "STRK");
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Cancelling Attack on task 7: {0}", ex.Message));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //update ssn 2 location
            try
            {
                _connection.UpdateObjectLocation("SSN-2", 150, 400, 0, true);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-2's location: {0}", ex.Message));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //update ssn 2 throttle
            try
            {
                _connection.UpdateObjectThrottle("SSN-2", 0.28);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-2's throttle: {0}", ex.Message));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //update ssn 2 destination
            try
            {
                _connection.UpdateObjectDestinationLocation("SSN-2", 400, 125, 0, true);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-2's destination: {0}", ex.Message));
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //update ssn 2 fuel amount
            try
            {
                _connection.UpdateObjectFuelAmount("SSN-2", 63);
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-2's fuel amount: {0}", ex.Message));
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //update ssn 1 icon 
            try
            {
                _connection.UpdateObjectIconName("SSN-1", "ImageLib.GroundTrack.Equipment.Special.LandMines.Claymore-Friend.png");
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-1's icon: {0}", ex.Message));
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //update ssn 2 icon
            try
            {
                _connection.UpdateObjectIconName("SSN-2", "ImageLib.SubSurfaceTrack-Unknown.png");
            }
            catch (Exception ex)
            {
                UpdateTextBox(String.Format("Error Updating SSN-2's icon: {0}", ex.Message));
            }
        }
        
    }
}
