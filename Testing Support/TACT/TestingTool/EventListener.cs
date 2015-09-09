using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace TestingTool
{
    public

        class EventListener
    {
        private static Form1 parentForm;
        public static Form1 ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }

        static NetworkClient network = null;
        public static NetworkClient Network
        {
            set { network = value; }
            get { return network; }
        }
        private List<SimulationEvent> events = null;
        private Boolean isActive = false;
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private void subscribeToEvents()
        {
  /*          network.Subscribe("NewObject");

            network.Subscribe("SimulationTimeEvent");
            network.Subscribe("UpdateTag");
            network.Subscribe("TransferObject");
   */
            network.Subscribe("ALL");
        }
        public void StartListening()
        {

            while (!network.IsConnected())
            {
                Thread.Sleep(300);
                try
                {
                    network.Connect("DGeller2", 9999);
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Connection attempt failed:" + e.Message + ". Retrying ...");
                }
            }
            subscribeToEvents();
            isActive = true;
            while (network.IsConnected())
            {// as long as there's a network we remain open to input events
                while (isActive && network.IsConnected())
                {

                    events = network.GetEvents();

                    foreach (SimulationEvent e in events)
                    {
                        switch (e.eventType)
                        {

                            case "SimulationTimeEvent":
                                int currentTime = ((IntegerValue)e["Time"]).value;
                                if (0 == currentTime % 1000)// only bother with full seconds
                                    parentForm.NewTime(currentTime.ToString().PadLeft(6));

                                break;
                            case "NewObject":

                                if ((e.parameters.ContainsKey("ObjectType"))
                                    && ("Team" == ((StringValue)e.parameters["ObjectType"]).value))
                                {
                                    string teamName = ((StringValue)e["ID"]).value;

                                    parentForm.NewTeam(teamName);
                                }
                                else if ((e.parameters.ContainsKey("ObjectType"))
                                     && ("DecisionMaker" == ((StringValue)e.parameters["ObjectType"]).value))
                                {
                                    string DMName = ((StringValue)e["ID"]).value;
                                    string myTeamName = ((StringValue)((AttributeCollectionValue)e.parameters["Attributes"])["TeamMember"]).value;
                                    parentForm.NewDM(DMName, myTeamName);
                                }
                                else if ((e.parameters.ContainsKey("ObjectType"))
                           && (
                                    ("LandObject" == ((StringValue)e.parameters["ObjectType"]).value)
                         || ("SeaObject" == ((StringValue)e.parameters["ObjectType"]).value)
                             || ("AirObject" == ((StringValue)e.parameters["ObjectType"]).value)
                                    )
                                    && ((AttributeCollectionValue)e.parameters["Attributes"]).attributes.ContainsKey("OwnerID")
                                    )
                                {
                                    string assetName = ((StringValue)e["ID"]).value;
                                    string myOwnerName = ((StringValue)((AttributeCollectionValue)e.parameters["Attributes"])["OwnerID"]).value;
                                    parentForm.NewAsset(assetName, myOwnerName);
                                }
                                break;
                            case "UpdateTag":
                                string updateAsset = ((StringValue)e.parameters["UnitID"]).value;
                                string updateTag = ((StringValue)e.parameters["Tag"]).value;
                                List<string> updateDMs = ((StringListValue)e.parameters["TeamMembers"]).strings;
                                string updateMessage = "Received UpdateTag for '" + updateAsset + "' \nwith tag '" + updateTag + "'\n for '" + updateDMs[0] + "'";
                                for (int i = 1; i < updateDMs.Count; i++)
                                    updateMessage += ", '" + updateDMs[i] + "'";
                                MessageBox.Show(updateMessage);
                                break;
                            case "SystemMessage":
                                string msgRecipient = ((StringValue)e["PlayerID"]).value;
                                string msgSystemMessage = ((StringValue)e["Message"]).value;
                                string msgColor = ((IntegerValue)e["TextColor"]).value.ToString();
                                string msgStyle = ((StringValue)e["DisplayStyle"]).value;
                                string msgMessage = "Received SystemMessage for '" + msgRecipient + "' with color '"
                                     + msgColor + "' and style '" + msgStyle + "'\n'" + msgSystemMessage + "'";
                                MessageBox.Show(msgMessage);
                                break;
                            case "TransferObject":
                                string xferReceiver = ((StringValue)e["UserID"]).value;
                                string xferDonor = ((StringValue)e["DonorUserID"]).value;
                                string xferObject = ((StringValue)e["ObjectID"]).value;
                                string xferMessage = "Object '" + xferObject + "' transferred from '" + xferDonor + "' to '" + xferReceiver + '"';
                                MessageBox.Show(xferMessage);
                                break;
                            case "CreateChatRoom":
                                string chatRoomName = ((StringValue)e["RoomName"]).value;

                                List<string> chatMembers = ((StringListValue)e.parameters["MembershipList"]).strings;
                           if(0==chatMembers.Count)
                               MessageBox.Show(" Create chat room '"+chatRoomName+"' has no members.");
                           else{
                                string createChatMsg = "Create chat room '" + chatRoomName + "' with '";
                        createChatMsg+=chatMembers[0]+"' ";
                               for (int i=1;i<chatMembers.Count;i++)
                                   createChatMsg+=", '"+chatMembers[i]+"'";
                               MessageBox.Show(createChatMsg);
                               parentForm.AddChatRoom(chatRoomName);
                           }    
                                break;
                       case "CloseChatRoom":
                           string closeChatRoomName = ((StringValue)e["RoomName"]).value;
                               MessageBox.Show("Closing chat room '"+closeChatRoomName+"'.");
                               parentForm.CloseChatRoom(closeChatRoomName);                          
                              break;

                            case "TextChatRequest":
                                string textChatBody = ((StringValue)(e["ChatBody"])).value;
                                string textUserID=((StringValue)(e["UserID"])).value;
                                string textTargetUserID = ((StringValue)(e["TargetUserID"])).value;
                                string textChatMessage = "Message '" + textChatBody + "' sent from '" + textUserID + "' to '" + textTargetUserID + "'";
                                MessageBox.Show(textChatMessage);
                                break;
                            case "CreateVoiceChannel":
                                string voiceChannelName = ((StringValue)e["ChannelName"]).value;

                                List<string> channelMembers = ((StringListValue)e.parameters["MembershipList"]).strings;
                                if (0 == channelMembers.Count)
                                    MessageBox.Show(" Create voice channel '" + voiceChannelName + "' has no members.");
                                else
                                {
                                    string createVoiceMsg = "Create voice channel '" + voiceChannelName + "' with '";
                                    createVoiceMsg += channelMembers[0] + "' ";
                                    for (int i = 1; i < channelMembers.Count; i++)
                                        createVoiceMsg += ", '" + channelMembers[i] + "'";
                                    MessageBox.Show(createVoiceMsg);
                                    parentForm.AddVoiceChannel(voiceChannelName);
                                }

                                break;
                            case "CloseVoiceChannel":
                                string closeVoiceChannelName = ((StringValue)e["ChannelName"]).value;
                                MessageBox.Show("Closing voice channel '" + closeVoiceChannelName + "'.");
                                parentForm.CloseVoiceChannel(closeVoiceChannelName);
                                break;
                            case "AddToVoiceChannel":
                                string addToChannel = ((StringValue)e["ChannelName"]).value;
                                string addToDM = ((StringValue)e["NewAccessor"]).value;
                                MessageBox.Show("Adding '" + addToDM + "' to channel '" + addToChannel + "'");
                                break;
                            case "RemoveFromVoiceChannel":
                                string removeFromChannel = ((StringValue)e["ChannelName"]).value;
                                string deletedPlayer = ((StringValue)e["DeletedPlayer"]).value;
                                MessageBox.Show("Removing '" + deletedPlayer + "' from channel '" + removeFromChannel + "'");
                                break;

                        }
                    }

                    Thread.Sleep(100);
                }
                if (!network.IsConnected())
                    isActive = false;
            }
            Console.WriteLine("Lost connection.");
            network = null;
        }
        public EventListener() { }
    }
}
