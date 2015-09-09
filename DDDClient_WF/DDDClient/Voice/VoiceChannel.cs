using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ConaitoLib;

namespace Aptima.Asim.DDD.Client
{
    class VoiceChannel{
    

        //const char kcPushToTalkKey = (char)System.Windows.Forms.Keys.F12;

        //private const int kiPUSH_TO_TALK_ID = 1;
        private bool m_bLoggedIn = false;
        private bool bHotKeyRegistered = false;
        private int m_iCurChannelID = -1;
        private int m_iDDDChannelID = Int32.MaxValue;
        private int m_iUserId = -1;
        //If you get an error here it's because the COM control is not registered.
        //Use "regsvr32.exe EvoVoIP.dll" to register the COM control.
        //Check References in the Solution Explorer and add the COM control named
        //"EvoVoIP 2.1 Type Library"

        // We'll want to set these in a configuration file;
        // for now, they're hard-coded
        //        private int kiSoundsystem = 2; //Direct sound system
        private int kiSoundsystem = 1;  // Windows sound system
        //        private int kiInputDeviceId = 4;
        private int kiInputDeviceId = -1;   //default device
        //        private int kiOutputDeviceId = 6;
        private int kiOutputDeviceId = -1;  //default device

        const int kiUDPPort = 10301;
        const char kcPushToTalkKey = (char)System.Windows.Forms.Keys.F12;

        private const int kiPUSH_TO_TALK_ID = 1;
        private const bool kbDEBUG = true;
        
        private ConaitoLib.EvoVoIP m_voipclient;
        private String _strDM = null;
        private String _strVoiceServerHostname = null;
        private int _iVoiceServerPort = 10300;
        private String _strConaitoServerPasswd = null;
        private int _iChannelID;

        private bool m_joined;
        private bool m_talk;
        private bool m_pushToTalk;
        private bool m_voiceActivated;
        private int m_masterVolume;
        private int m_voiceActivationLevel;
        private IVoiceClientEventCommunicator m_eventCommunicator = null;

        public VoiceChannel()
        {
            
            // Instantiate the client control
            //m_voipclient = new ConaitoLib.EvoVoIPClass();
            /*
            //connected successfully to server. Result of m_voipclient.Connect(..)
            m_voipclient.OnConnectSuccess += new ConaitoLib.IEvoVoIPEvents_OnConnectSuccessEventHandler(this.voipclient_OnConnectSuccess);
            //failed to connect to server. Result of m_voipclient.Connect(..)
            m_voipclient.OnConnectFailed += new ConaitoLib.IEvoVoIPEvents_OnConnectFailedEventHandler(this.voipclient_OnConnectFailed);
            //server dropped connection
            m_voipclient.OnConnectionLost += new ConaitoLib.IEvoVoIPEvents_OnConnectionLostEventHandler(this.voipclient_OnConnectionLost);
            //server accepted login. Result of m_voipclient.DoLogin(..)
            m_voipclient.OnAccepted += new ConaitoLib.IEvoVoIPEvents_OnAcceptedEventHandler(this.voipclient_OnAccepted);
            //logged out of server. Result of m_voipclient.DoLogout()
            //            m_voipclient.OnLoggedOut += new ConaitoLib.IEvoVoIPEvents_OnLoggedOutEventHandler( this.voipclient_OnLoggedOut );
            //a new user entered your channel
            m_voipclient.OnAddUser += new ConaitoLib.IEvoVoIPEvents_OnAddUserEventHandler(this.voipclient_OnAddUser);
            //a user in your channel updated his information, e.g. nickname, status, etc.
            m_voipclient.OnUpdateUser += new ConaitoLib.IEvoVoIPEvents_OnUpdateUserEventHandler(this.voipclient_OnUpdateUser);
            //a user left your channel
            m_voipclient.OnRemoveUser += new ConaitoLib.IEvoVoIPEvents_OnRemoveUserEventHandler(this.voipclient_OnRemoveUser);
            //a user in your channel is talking
            m_voipclient.OnUserTalking += new ConaitoLib.IEvoVoIPEvents_OnUserTalkingEventHandler(this.voipclient_OnUserTalking);
            //a user in your channel stopped talking
            m_voipclient.OnUserStoppedTalking += new ConaitoLib.IEvoVoIPEvents_OnUserStoppedTalkingEventHandler(this.voipclient_OnUserStoppedTalking);
            //a new channel was created on the server
            m_voipclient.OnAddChannel += new ConaitoLib.IEvoVoIPEvents_OnAddChannelEventHandler(this.voipclient_OnAddChannel);
            //a channel updated its information, e.g. its number of users
            m_voipclient.OnUpdateChannel += new ConaitoLib.IEvoVoIPEvents_OnUpdateChannelEventHandler(this.voipclient_OnUpdateChannel);
            //a channel was deleted from the server
            m_voipclient.OnRemoveChannel += new ConaitoLib.IEvoVoIPEvents_OnRemoveChannelEventHandler(this.voipclient_OnRemoveChannel);
            //you have joined a new channel. Result of m_voipclient.DoJoinChannel
            m_voipclient.OnJoinedChannel += new ConaitoLib.IEvoVoIPEvents_OnJoinedChannelEventHandler(this.voipclient_OnJoinedChannel);
            //you have left the channel. Result of m_voipclient.DoLeaveChannel
            m_voipclient.OnLeftChannel += new ConaitoLib.IEvoVoIPEvents_OnLeftChannelEventHandler(this.voipclient_OnLeftChannel);
            //a hotkey has become active
            m_voipclient.OnHotKeyToggle += new ConaitoLib.IEvoVoIPEvents_OnHotKeyToggleEventHandler(this.voipclient_OnHotKeyToggle);
            //notification containing the raw audio (PCM data) which was played when a user was talking.
            m_voipclient.OnUserAudioData += new ConaitoLib.IEvoVoIPEvents_OnUserAudioDataEventHandler(this.voipclient_OnUserAudioData);
            // Error notifications
            m_voipclient.OnCommandError += new ConaitoLib.IEvoVoIPEvents_OnCommandErrorEventHandler(this.voipclient_OnCommandError);
            */
            m_joined = false;
        }

        public bool Talk
        {
            get
            {
                return m_talk;
            }
            set
            {
                m_talk = value;

                SetPushToTalk(m_pushToTalk);
                SetVoiceActivated(m_voiceActivated);

                if (!m_talk)
                {
                    //System.Diagnostics.Trace.WriteLine(String.Format("Voice Channel: Setting mute   UserName = {0}  UserID = {1}  ChannelID = {2}", _strDM, m_iUserId, _iChannelID));
                }
                else
                {
                    //System.Diagnostics.Trace.WriteLine(String.Format("Voice Channel: Removing mute   UserName = {0}  UserID = {1}  ChannelID = {2}", _strDM, m_iUserId, _iChannelID));
                }
            }
        }

        public void initialize(
           //IVoiceClientEventCommunicator controller,
           String strDM,
           int channelID,
           String strVoiceServerHostname,
           int iVoiceServerPort,
           String strConaitoServerPasswd,
           IVoiceClientEventCommunicator eventCommunicator)
        {
            //if (null == controller)
            //{
            //    throw new Exception("Voice client controller is null");
            //}
            //m_eventCommunicator = controller;
            _strDM = strDM;
            _strVoiceServerHostname = strVoiceServerHostname;
            _iVoiceServerPort = iVoiceServerPort;
            _strConaitoServerPasswd = strConaitoServerPasswd;
            _iChannelID = channelID;
            m_eventCommunicator = eventCommunicator;

            // It can take a while for a remote server to exchange the messages,
            // so give it a few tries to initialize.
            bool bAuthorized = false;
            for (int i = 0; i < 5; i++)
            {
                if (m_voipclient.InitVoIP(true))
                {
                    bAuthorized = true;
                    //logDebugOnly(
                    //    String.Format("Conaito initialization succeeded for user {0}\n", strDM));
                    //logDebugOnly(
                    //    String.Format("  VoiceServerHostname = {0}, VoiceServerPort = {1}\n",
                     //   strVoiceServerHostname, Convert.ToInt32(iVoiceServerPort).ToString()));
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            if (!bAuthorized)
            {
                throw new Exception("Voice client failed to initialize");
            }

            //String strChannelPassword = "";
            //String strTopic = "";
            //String strOpPassword = "";
            bool soundInited = initSoundSystem();
            if (soundInited && Connect())
            {
                // Register push-to-talk key
                bHotKeyRegistered = m_voipclient.RegisterHotKey(
                    kiPUSH_TO_TALK_ID,
                    false,  // control
                    false,  // alt
                    false,  // shift
                    false,  // windows key
                    kcPushToTalkKey);

                //this.m_timer1.Tick += new System.EventHandler( this.timer1_Tick );

                // Give it a little time or you won't get the nodes when you log in
                // I found it works at 1/2 second but not less  -- shorvitz 3/10/08
                System.Threading.Thread.Sleep(500);
                login();
                System.Threading.Thread.Sleep(500);
                //bool joinsuccess = m_voipclient.DoJoinChannel(m_voipclient.GetChannelPath(_iChannelID), strChannelPassword, strTopic, strOpPassword);
                //TODO: Note: This happens on the main thread it seems, so the above sleep for 1000 ms occurs for EACH voice channel
                //this user is allowed to join.  Test a scenario with 6 voice channels that start at time 1, and the client freezes
                //from time 1 to time 7.  Need to figure a reasonable workaround for this.
            }
            
        }

        public bool IsJoined()
        {
            return m_joined;
        }
        public bool JoinChannel()
        {
            String strChannelPassword = "";
            String strTopic = "";
            String strOpPassword = "";
            bool joinsuccess = m_voipclient.DoJoinChannel(m_voipclient.GetChannelPath(_iChannelID), strChannelPassword, strTopic, strOpPassword);
            Console.Write("channel join" + joinsuccess.ToString());
            m_joined = true;
            Talk = true;
            return joinsuccess;
        }

        public void LeaveChannel()
        {
            m_voipclient.DoLeaveChannel();
            m_joined = false;
        }

        private bool login()
        {
            bool bStatus = true;
            if (!m_bLoggedIn)
            {
                if (m_voipclient.DoLogin(_strDM, _strConaitoServerPasswd))
                {
                    //richTextBoxLog.AppendText(String.Format("Login for {0} succeeded\n", _strDM));
                    m_iUserId = m_voipclient.GetMyUserID();
                    m_bLoggedIn = true;
                    // Get the root channel
                    //AddChannel(0);
                }
                else
                {
                    
                    bStatus = false;
                }
            }
            //updateButtonStates();
            return bStatus;
        }
        private bool initSoundSystem()
        {
            int iInputDeviceCount = m_voipclient.GetInputDeviceCount(kiSoundsystem);
            int iInputDeviceID = m_voipclient.GetInputDeviceID(kiSoundsystem, iInputDeviceCount - 1);
            if (m_voipclient.LaunchSoundSystem(kiSoundsystem, iInputDeviceID, kiOutputDeviceId, 32000, 5, true))
            //            if (m_voipclient.LaunchSoundSystem(1, -1, -1, 32000, 5))
            {
                //logDebugOnly("Sound system launched\n");
                //updateButtonStates();
                //m_timer1.Start();
                return true;
            }
            else
            {
                //logDebugOnly("Sound system failed to launched\n");
                return false;
            }
        }

        private bool Connect()
        {
            if (null == _strVoiceServerHostname)
            {
                _strVoiceServerHostname = "localhost";
            }
            if (!m_voipclient.Connect(_strVoiceServerHostname, _iVoiceServerPort, kiUDPPort, 0, 0))
            {
                //logDebugOnly(String.Format("Failed to connect to voice server"));
            }

            //wait until we get connected.
            int tries = 0;
            int maxTries = 20;
            bool isRunning = false;
            while (!isRunning && tries < maxTries)
            {
                Thread.Sleep(100);
                isRunning = m_voipclient.IsConnected();
                tries++;
            }
            return isRunning;

        }

        public bool Disconnect()
        {
            if (m_voipclient.IsConnected() || m_voipclient.IsConnecting())
            {
                bool bStatus = m_voipclient.Disconnect();
                return bStatus;
            }
            else
            {
                return false;
            }
        }

        public void SetPushToTalk(bool val)
        {
            m_pushToTalk = val;
            if (val && Talk)
            {
                // Register push-to-talk key
                bHotKeyRegistered = m_voipclient.RegisterHotKey(
                    kiPUSH_TO_TALK_ID,
                    false,  // control
                    false,  // alt
                    false,  // shift
                    false,  // windows key
                    kcPushToTalkKey);
                
            }
            else
            {
                // UnRegister push-to-talk key
                m_voipclient.UnregisterHotKey(kiPUSH_TO_TALK_ID);
                bHotKeyRegistered = false;
            }
        }

        public void SetVoiceActivated(bool val)
        {
            m_voiceActivated = val;
            if (val && Talk)
            {
                m_voipclient.SetVoiceActivated(true);
            }
            else
            {
                m_voipclient.SetVoiceActivated(false);
            }
        }

        public void SetMasterVolume(int vol)
        {
            m_masterVolume = vol;
            m_voipclient.SetMasterVolume(vol);
        }

        public void SetVoiceActivationLevel(int vol)
        {
            m_voiceActivationLevel = vol;
            m_voipclient.SetVoiceActivationLevel(vol);
        }

        #region Conaito Event Handlers

        private void voipclient_OnAccepted(int nUserID)
        {

        }

        private void voipclient_OnConnectSuccess()
        {

        }

        private void voipclient_OnConnectFailed()
        {
            m_voipclient.Disconnect();

        }

        private void voipclient_OnConnectionLost()
        {
            m_voipclient.Disconnect();

        }

        private void voipclient_OnAddChannel(int nChannelID)
        {

        }

        private void voipclient_OnJoinedChannel(int nChannelID)
        {

        }

        private void voipclient_OnLeftChannel(int nChannelID)
        {

        }

        private void voipclient_OnUpdateChannel(int nChannelID)
        {

        }

        private void voipclient_OnRemoveChannel(int nChannelID)
        {

        }

        private void voipclient_OnAddUser(int nUserID, int nChannelID)
        {

        }

        private void voipclient_OnUpdateUser(int nUserID, int nChannelID)
        {

        }

        private void voipclient_OnRemoveUser(int nUserID, int nChannelID)
        {
 
        }

        private void voipclient_OnHotKeyToggle(int nHotKeyID, bool bActive)
        {
            //a registered hotkey is active
            if (nHotKeyID == kiPUSH_TO_TALK_ID)
            {
                if (bActive)
                {
                    //sendDDDPushToTalkEvent();
                    m_voipclient.StartTransmitting();
                    //UpdateUserIcon(m_voipclient.GetMyUserID(), true);
                }
                else
                {
                    //sendDDDStoppedTalkingEvent();
                    m_voipclient.StopTransmitting();
                    //UpdateUserIcon(m_voipclient.GetMyUserID(), false);
                }
            }
        }

        private void voipclient_OnUserTalking(int nUserID)
        {
            User user = m_voipclient.GetUser(nUserID);
            Channel channel = m_voipclient.GetChannel(_iChannelID);
            string username = user.szNickName;
            string channelname = channel.szName;
            //System.Diagnostics.Trace.WriteLine(String.Format("Conaito OnUserTalking event received on channel {0} from {1}", channelname, username));
            if (m_iUserId == nUserID)
            {
                m_eventCommunicator.sendRequestStartedTalkingVoiceChannelEvent(channelname);
            }
        }

        private void voipclient_OnUserStoppedTalking(int nUserID)
        {
            User user = m_voipclient.GetUser(nUserID);
            Channel channel = m_voipclient.GetChannel(_iChannelID);
            string username = user.szNickName;
            string channelname = channel.szName;
            //System.Diagnostics.Trace.WriteLine(String.Format("Conaito OnUserStoppedTalking event received on channel {0} from {1}", channelname, username));

            if (m_iUserId == nUserID)
            {
                m_eventCommunicator.sendRequestStoppedTalkingVoiceChannelEvent(channelname);
            }
        }

        private void voipclient_OnUserAudioData(int userid, int samplerate, ref System.Array rawAudio, int samples)
        {
            short[] buff = (short[])rawAudio;
        }

        private void voipclient_OnCommandError(int nError)
        {
            //System.Diagnostics.Trace.WriteLine(String.Format("Conaito Error code: " + nError.ToString()));
        }

        #endregion Conaito Event Handlers
    }
}
