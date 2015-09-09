using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace TestingTool
{
    public partial class Form1 : Form
    {

        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simulationModelPath = "C:\\Program Files\\Aptima\\DDD 4.0\\Client\\SimulationModel.xml";

        //Correlational information
        Dictionary<string, List<string>> teamMembers = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dmAssets = new Dictionary<string, List<string>>();
        // Track voice and chat room names
        List<string> chatRoomNames = new List<string>();
        List<string> voiceChannelNames = new List<string>();


        // Handle display of time 
        delegate void NewTimeCallback(string time);
        public void NewTime(string s)
        {
            if (this.lblTime.InvokeRequired)
            {
                NewTimeCallback d = new NewTimeCallback(NewTime);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                lblTime.Text = s;
            }
            lblTime.Text = ((int)(int.Parse(s) / 1000)).ToString();
        }

        //HAndle chat rooms

     
        public void AddChatRoom(string name)
        {
                chatRoomNames.Add(name);
     
        }
        delegate void CloseChatRoomCallback(string name);
        public void CloseChatRoom(string name)
        {
            if (this.lbxCloseChatNames.InvokeRequired)
            {
                CloseChatRoomCallback d = new CloseChatRoomCallback(CloseChatRoom);
                this.Invoke(d, new object[] { name });
            }
            else
            {

                if (chatRoomNames.Contains(name))
                    chatRoomNames.Remove(name);
                if (lbxCloseChatNames.Items.Contains(name))
                    lbxCloseChatNames.Items.Remove(name);
            }
        }

        //HAndle voice channels


        public void AddVoiceChannel(string name)
        {
            voiceChannelNames.Add(name);

        }
        delegate void CloseVoiceChannelCallback(string name);
        public void CloseVoiceChannel(string name)
        {
            if (this.lbxCloseVoiceNames.InvokeRequired)
            {
                CloseVoiceChannelCallback d = new CloseVoiceChannelCallback(CloseVoiceChannel);
                this.Invoke(d, new object[] { name });
            }
            else
            {

                if (voiceChannelNames.Contains(name))
                    voiceChannelNames.Remove(name);
                if (lbxCloseVoiceNames.Items.Contains(name))
                    lbxCloseVoiceNames.Items.Remove(name);
            }
        }

        //HAndle teams

        delegate void NewTeamCallback(string name);
        public void NewTeam(string name)
        {
            if (this.lbxTeams.InvokeRequired)
            {
                NewTeamCallback d = new NewTeamCallback(NewTeam);
                this.Invoke(d, new object[] { name });
            }
            else
            {

                lbxTeams.Items.Add(name);
                teamMembers[name] = new List<string>();
            }
        }
        // HAndle DMs
        delegate void NewDMCallback(string name, string team);
        public void NewDM(string name, string team)
        {
            if (this.lbxDecisionMakers.InvokeRequired)
            {
                NewDMCallback d = new NewDMCallback(NewDM);
                this.Invoke(d, new object[] { name, team });
            }
            else
            {

                lbxDecisionMakers.Items.Add(name);
                dmAssets[name] = new List<string>();
                teamMembers[team].Add(name);
            }
        }
     


        // HAndle Assets
        delegate void NewAssetCallback(string name, string owner);
        public void NewAsset(string name, string owner)
        {
            if (this.lbxAssets.InvokeRequired)
            {
                NewAssetCallback d = new NewAssetCallback(NewAsset);
                this.Invoke(d, new object[] { name, owner });
            }
            else
            {

                lbxAssets.Items.Add(name);
                dmAssets[owner].Add(name);
            }
        }


        public Form1()
        {
            InitializeComponent();
            simModelInfo = smr.readModel(simulationModelPath);
            rbInvisible.Checked = true;
        }




        private void ChangeTag_CheckedChanged(object sender, EventArgs e)
        {


            if (!rbChangeTag.Checked)
            {
                txArg1.Visible = false;
                txArg1.Enabled = false;
                btnChangeTag.Enabled = false;
                btnChangeTag.FlatStyle = FlatStyle.Flat;
                lbxAssets.Enabled = false;
                lbxTeams.Enabled = false;
                lbxDecisionMakers.Enabled = false;
                lblArg1.Visible = false;
                lblArg1.Text = "";
            }
            else
            {
                txArg1.Visible = true;
                txArg1.Enabled = true;
                btnChangeTag.Enabled = true;
                btnChangeTag.FlatStyle = FlatStyle.Standard;
                lbxAssets.Enabled = true;
                lbxDecisionMakers.Enabled = true;
                lblArg1.Visible = true;
                lblArg1.Text = "New Tag";

            }

        }

        private void btnChangeTag_Click(object sender, EventArgs e)
        {
            string chosenText = "";
            string chosenAsset = "";
            string chosenDM = "";
            if (null != lbxDecisionMakers.SelectedItem)
                chosenDM = lbxDecisionMakers.SelectedItem.ToString();
            if (null != lbxAssets.SelectedItem)
                chosenAsset = lbxAssets.SelectedItem.ToString();
            if (null != txArg1.Text)
                chosenText = txArg1.Text;
            if ("" == chosenDM)
            {
                MessageBox.Show("Please select a Decision Maker");
                return;
            }
            if ("" == chosenAsset)
            {
                MessageBox.Show("Please select an asset");
                return;
            }
            if ("" == chosenText)
            {
                DialogResult dr = MessageBox.Show("Do you wish to remove the current tag from " + chosenAsset + "?", "", MessageBoxButtons.YesNo);
                if (DialogResult.No == dr)
                    return;
            }

            SimulationEvent chTag = SimulationEventFactory.BuildEvent(ref simModelInfo, "ChangeTagRequest");
            chTag["UnitID"] = DataValueFactory.BuildString(chosenAsset);
            chTag["DecisionMakerID"] = DataValueFactory.BuildString(chosenDM);
            chTag["Tag"] = DataValueFactory.BuildString(chosenText);
            EventListener.Network.PutEvent(chTag);

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Enabled = false;
            btnStart.Visible = false;
            lblReady.Visible = true;
        }

        private void lblReady_VisibleChanged(object sender, EventArgs e)
        {
            if (lblReady.Visible)
            {
                EventListener.ParentForm = this;

                ConnectionManager cm = ConnectionManager.MakeConnection("DGeller2", 9999);
                // Get the network
                NetworkClient nc = cm.NetClient;
                // Create the listener
                Thread cmThread = new Thread(cm.StartListening);

                cmThread.Start();
            }

        }

        private void rbTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbTransfer.Checked)
            {
                btnTransfer.Enabled = false;
                btnTransfer.FlatStyle = FlatStyle.Flat;
                lbxTransferReceiver.Visible = false;
                lbxTransferReceiver.Items.Clear();
                lbxTransferReceiver.Items.Add("");
                lblTransferTo.Visible = false;
                lbxTransferReceiver.Enabled = false;
                lblTransferTo.Text = "";
                lbxAssets.Enabled = false;
                lbxDecisionMakers.Enabled = false;

            }
            else
            {
                btnTransfer.Enabled = true;
                btnTransfer.FlatStyle = FlatStyle.Standard;
                lbxTransferReceiver.Visible = true;
                for (int i = 0; i < lbxDecisionMakers.Items.Count; i++)
                    lbxTransferReceiver.Items.Add(lbxDecisionMakers.Items[i]);
                lbxTransferReceiver.Enabled = true;
                lblTransferTo.Visible = true;
                lblTransferTo.Text = "Transfer to";
                lbxAssets.Enabled = true;
                lbxDecisionMakers.Enabled = true;

            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            string chosenAsset = "";
            string chosenDM = "";
            string receivingDM = "";
            if (null != lbxDecisionMakers.SelectedItem)
                chosenDM = lbxDecisionMakers.SelectedItem.ToString();
            if (null != lbxAssets.SelectedItem)
                chosenAsset = lbxAssets.SelectedItem.ToString();
            if (null != lbxTransferReceiver.SelectedItem)
                receivingDM = lbxTransferReceiver.SelectedItem.ToString();
            if ("" == chosenDM)
            {
                MessageBox.Show("Please select a Decision Maker to cause the transfer");
                return;
            }
            if ("" == chosenAsset)
            {
                MessageBox.Show("Please select an asset");
                return;
            }
            if ("" == receivingDM)
            {
                MessageBox.Show("Please select a Decision MAker to receive the asset");

                return;
            }
            SimulationEvent xfer = SimulationEventFactory.BuildEvent(ref simModelInfo, "TransferObjectRequest");
            xfer["ObjectID"] = DataValueFactory.BuildString(chosenAsset);
            xfer["UserID"] = DataValueFactory.BuildString(chosenDM);
            xfer["RecipientID"] = DataValueFactory.BuildString(receivingDM);
            EventListener.Network.PutEvent(xfer);

        }

        private void rbOpenChat_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbOpenChat.Checked)
            {
                btnOpenChat.Enabled = false;
                lbxChatMembers.Enabled = false;
                lbxChatMembers.Items.Clear();
                txtOpenChatName.Text = "";
                lbxDecisionMakers.Enabled = false;
                DMLabel.Text = "Decision Makers";
                lbxDecisionMakers.SelectedItem = null;
                lbxChatMembers.SelectedItems.Clear();
                lblChatMembers.Visible = true;
                lblOpenChatName.Visible = false;
            }
            else
            {
                btnOpenChat.Enabled = true;
                lbxChatMembers.Enabled = true;
                for (int i = 0; i < lbxDecisionMakers.Items.Count - 1; i++)
                    lbxChatMembers.Items.Add(lbxDecisionMakers.Items[i]);

                txtOpenChatName.Enabled = true;
                txtOpenChatName.Text = "Room name";
                lbxDecisionMakers.Enabled = true;
                DMLabel.Text = "Owner";
                lblChatMembers.Visible = true;
                lblOpenChatName.Visible = true;
            }
        }

        private void btnOpenChat_Click(object sender, EventArgs e)
        {
            if (null == lbxDecisionMakers.SelectedItem)
            {
                MessageBox.Show("Please select an owner for the chat room.");
                return;
            }
            if ((null == lbxChatMembers.SelectedItems) || (lbxChatMembers.SelectedItems.Count < 2))
            {
                MessageBox.Show("Please select at lest two members for the chat room.");
                return;
            }
            if ("" == txtOpenChatName.Text)
            {
                MessageBox.Show("Please provide a name for the chat room.");
                return;
            }
            SimulationEvent openChat = SimulationEventFactory.BuildEvent(ref simModelInfo, "RequestChatRoomCreate");
            openChat["RoomName"] = DataValueFactory.BuildString(txtOpenChatName.Text);
            openChat["SenderDM_ID"] = DataValueFactory.BuildString((string)lbxDecisionMakers.SelectedItem);
            StringListValue chatMembers = new StringListValue();
            for(int i=0; i<lbxChatMembers.SelectedItems.Count;i++)
                chatMembers.strings.Add((string)lbxChatMembers.SelectedItems[i]);
            openChat["MembershipList"] = DataValueFactory.BuildFromDataValue(chatMembers);
                EventListener.Network.PutEvent(openChat);


        }





        private void rbOpenVoice_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbOpenVoice.Checked)
            {
                btnOpenVoice.Enabled = false;
                lbxVoiceMembers.Enabled = false;
                lbxVoiceMembers.Items.Clear();
                txtOpenVoiceName.Text = "";
                lbxDecisionMakers.Enabled = false;
                DMLabel.Text = "Decision Makers";
                lbxDecisionMakers.SelectedItem = null;
                lbxVoiceMembers.SelectedItems.Clear();
                lblVoiceMembers.Visible = false;
                lblOpenVoiceName.Visible = false;
            }
            else
            {
                btnOpenVoice.Enabled = true;
                lbxVoiceMembers.Enabled = true;
                for (int i = 0; i < lbxDecisionMakers.Items.Count - 1; i++)
                    lbxVoiceMembers.Items.Add(lbxDecisionMakers.Items[i]);

                txtOpenVoiceName.Enabled = true;
                txtOpenVoiceName.Text = "Channel name";
                lbxDecisionMakers.Enabled = true;
                DMLabel.Text = "Owner";
                lblVoiceMembers.Visible = true;
                lblOpenVoiceName.Visible = true;
            }
        }

        private void btnOpenVoice_Click(object sender, EventArgs e)
        {
            if (null == lbxDecisionMakers.SelectedItem)
            {
                MessageBox.Show("Please select an owner for the voice channel.");
                return;
            }
            if ((null == lbxVoiceMembers.SelectedItems) || (lbxVoiceMembers.SelectedItems.Count < 2))
            {
                MessageBox.Show("Please select at least two members for the voice channel.");
                return;
            }
            if ("" == txtOpenVoiceName.Text)
            {
                MessageBox.Show("Please provide a name for the voice channel.");
                return;
            }
            SimulationEvent openVoice = SimulationEventFactory.BuildEvent(ref simModelInfo, "RequestVoiceChannelCreate");
            openVoice["ChannelName"] = DataValueFactory.BuildString(txtOpenVoiceName.Text);
            openVoice["SenderDM_ID"] = DataValueFactory.BuildString((string)lbxDecisionMakers.SelectedItem);
            StringListValue voiceMembers = new StringListValue();
            for (int i = 0; i < lbxVoiceMembers.SelectedItems.Count; i++)
                voiceMembers.strings.Add((string)lbxVoiceMembers.SelectedItems[i]);
            openVoice["MembershipList"] = DataValueFactory.BuildFromDataValue(voiceMembers);
            EventListener.Network.PutEvent(openVoice);


        }

        private void rbCloseChat_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbCloseChat.Checked)
            {
                btnCloseChat.Enabled = false;
                lbxCloseChatNames.Enabled = false;
                lbxCloseChatNames.Items.Clear();
                lbxDecisionMakers.SelectedItem = null;
                lblCloseChat.Visible = false;
            }
            else
            {
                btnCloseChat.Enabled = true;
                lbxCloseChatNames.Enabled = true;
                for (int i = 0; i < chatRoomNames.Count ; i++)
                    lbxCloseChatNames.Items.Add(chatRoomNames[i]);
                lbxDecisionMakers.Enabled = true;
                lblCloseChat.Visible = true;
            }
        }

        private void btnCloseChat_Click(object sender, EventArgs e)
        {
            if (null == lbxDecisionMakers.SelectedItem)
            {
                MessageBox.Show("Please select an owner for the chat room.");
                return;
            }
 
            if (null == lbxCloseChatNames.SelectedItem) 
            {
                MessageBox.Show("Please select the name of the room to close.");
                return;
            }

            SimulationEvent closeChat = SimulationEventFactory.BuildEvent(ref simModelInfo, "RequestCloseChatRoom");
            closeChat["RoomName"] = DataValueFactory.BuildString((string)lbxCloseChatNames.SelectedItem);
            closeChat["SenderDM_ID"] = DataValueFactory.BuildString((string)lbxDecisionMakers.SelectedItem);
                EventListener.Network.PutEvent(closeChat);


        }

        private void rbCloseVoice_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbCloseVoice.Checked)
            {
                btnCloseVoice.Enabled = false;
                lbxCloseVoiceNames.Enabled = false;
                lbxCloseVoiceNames.Items.Clear();
                lbxDecisionMakers.SelectedItem = null;
                      lblCloseVoiceName.Visible = false;
                    
            }
            else
            {
                btnCloseVoice.Enabled = true;
                lbxCloseVoiceNames.Enabled = true;
                for (int i = 0; i < voiceChannelNames.Count; i++)
                    lbxCloseVoiceNames.Items.Add(voiceChannelNames[i]);
                lbxDecisionMakers.Enabled = true;
                lblCloseVoiceName.Visible = true;

            }
        }

        private void btnCloseVoice_Click(object sender, EventArgs e)
        {
            {
                if (null == lbxDecisionMakers.SelectedItem)
                {
                    MessageBox.Show("Please select an owner for the voice channel.");
                    return;
                }

                if (null == lbxCloseVoiceNames.SelectedItem)
                {
                    MessageBox.Show("Please select the name of the channel to close.");
                    return;
                }

                SimulationEvent closeVoice = SimulationEventFactory.BuildEvent(ref simModelInfo, "RequestCloseVoiceChannel");
                closeVoice["ChannelName"] = DataValueFactory.BuildString((string)lbxCloseVoiceNames.SelectedItem);
                closeVoice["SenderDM_ID"] = DataValueFactory.BuildString((string)lbxDecisionMakers.SelectedItem);
                EventListener.Network.PutEvent(closeVoice);


            }
        }
 

    

       


    }
}