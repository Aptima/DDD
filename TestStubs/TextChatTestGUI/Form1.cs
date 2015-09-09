using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.TestStubs.TextChatTestGUI
{
    public partial class Form1 : Form
    {
        private static string textBoxText;
        private static NetworkClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static bool isRunning = false;
        private static Thread waitForEventsThread = null;
        private static string userID;
        private static string hostName;
        private static string portNumber;

        public static void AddToTextBoxText(string toAdd)
        {
            textBoxText += "\r\n" + toAdd;
        }

        public Form1(string simModel, string host, string port, string user)
        {
            InitializeComponent();
            userID = user;
            hostName = host;
            portNumber = port;
            simModelName = simModel;
            simModelInfo = smr.readModel(simModelName);
            AddToTextBoxText("Welcome " + userID + ".");
            server = new NetworkClient();
            server.Connect(hostName, Convert.ToInt32(portNumber));
            SimulationEventDistributor dist = new SimulationEventDistributor(ref simModelInfo);
            SimulationEventDistributorClient cc = new SimulationEventDistributorClient();

            dist.RegisterClient(ref cc);
            server.Subscribe("TextChat");
            isRunning = true;
            waitForEventsThread = new Thread(new ThreadStart(WaitForEvents));
            waitForEventsThread.Start();

        }

        private static void WaitForEvents()
        {
            List<SimulationEvent> incomingEvents = new List<SimulationEvent>();
            string incomingText = String.Empty;

            while (isRunning)
            {

                incomingEvents = server.GetEvents();

                if (incomingEvents.Count == 0)
                {//do nothing 
                }
                else
                {
                    foreach (SimulationEvent s in incomingEvents)
                    {
                        //You would check to make sure you are the target user,
                        //or target user is ALL.
                        if (((StringValue)s["TargetUserID"]).value == "ALL" ||
                            ((StringValue)s["TargetUserID"]).value == userID)
                        {
                            incomingText = ((StringValue)s["ChatBody"]).value;
                            AddToTextBoxText(incomingText);
                            incomingText = string.Empty;
                        }


                    }
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateForm();
            timer1.Start();

        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            //Check if a '/' command leads the string.
            //construct the send event
            string[] temp = textBoxMessage.Text.Split(' ');
            string sendingString = string.Empty;
            string targetUser = string.Empty;
            string chatType = string.Empty;
            bool send = true;
            if (temp[0] == @"/p")
            { //peer to peer command
                if (temp.Length < 3)
                {
                    AddToTextBoxText("Incorrect Parameters for that message");
                    send = false;
                }
                else
                {
                    sendingString = string.Empty;
                    targetUser = temp[1];
                    temp[0] = string.Empty;
                    temp[1] = string.Empty;
                    chatType = "P2P";
                    foreach (string s in temp)
                    {
                        sendingString += s + " ";
                    }
                }
            }
            else if (temp[0] == @"/t")
            {
                //TEAM CHAT
                sendingString = string.Empty;
                targetUser = "NULL"; //This should be the team name?
                temp[0] = string.Empty;
                temp[1] = string.Empty;
                chatType = "TEAM";
                foreach (string s in temp)
                {
                    sendingString += s + " ";
                }
            }
            else
            {
                //default
                sendingString = string.Empty;
                targetUser = "NULL";
                chatType = "ALL";
                foreach (string s in temp)
                {
                    sendingString += s + " ";
                }
            }

            if (send)
            {
                sendingString = sendingString.Trim();
                SimulationEvent se = SimulationEventFactory.BuildEvent(ref simModelInfo, "TextChatRequest");
                ((StringValue)se["UserID"]).value = userID;
                ((StringValue)se["ChatBody"]).value = sendingString;
                ((StringValue)se["TargetUserID"]).value = targetUser;
                ((StringValue)se["ChatType"]).value = chatType;
                ((IntegerValue)se["Time"]).value = 0;
                server.PutEvent(se);
                if (chatType == "P2P")
                {
                    AddToTextBoxText(String.Format("({0}) (PRIVATE to {1}): {2}", "0", targetUser, sendingString));
                }
                else if (chatType == "TEAM")
                {
                    // AddToTextBoxText(String.Format("({0}) (TEAMNAME): {1}", "0", sendingString));
                }
            }


            //clear textbox
            textBoxMessage.Text = string.Empty;
            textBoxMessage.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            textBoxChatWindow.Text = textBoxText;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (waitForEventsThread != null)
            //{
            //    waitForEventsThread.Abort();
            //    Thread.Sleep(100);
            //}
            KillThread();
            //disconnect from server?
        }
        public void KillThread()
        {
            if (waitForEventsThread != null)
            {
                waitForEventsThread.Abort();
                Thread.Sleep(100);
            }
        }

    }
}