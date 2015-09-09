using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ConaitoLib;


namespace Aptima.Asim.DDD.Client
{
    public partial class VoIPClientControl : UserControl
    {
        #region Attributes
        //If you get an error here it's because the COM control is not registered.
        //Use "regsvr32.exe EvoVoIP.dll" to register the COM control.
        //Check References in the Solution Explorer and add the COM control named
        //"EvoVoIP 2.1 Type Library"
        private ConaitoLib.EvoVoIP m_voipclient;
        private IVoiceClientEventCommunicator m_eventCommunicator = null;
        

        private String _strDM = null;
        private String _strVoiceServerHostname = null;
        private int _iVoiceServerPort = 10300;
        private String _strConaitoServerPasswd = null;

        private System.Windows.Forms.Timer m_timer1;
        private bool m_bLoggedIn = false;
        private bool bHotKeyRegistered = false;
        private int m_iDDDChannelID = Int32.MaxValue;
        private int m_iUserId = -1;

        private String m_curSelectedChannel = String.Empty;

        // key = channelName
        // value = channelID
        private Dictionary<string, int> _aChannels = null;

        private Dictionary<string, VoiceChannel> _voiceChannels = null;

        // key = channelName
        // value = true means DM is authorized to use that channel
        // Keyed by channelName because we don't always have access to the
        // channelID
        private Dictionary<string, bool> _aChannelAuthorizations = null;

        private enum ImageIndex
        {
            USER_TALKING_OLD = 0,
            USER_NOT_TALKING_OLD = 1,
            CHANNEL = 2,
            USER_NOT_JOINED = 3,
            ME_NOT_JOINED = 4,
            USER_TALKING = 5,
            ME_TALKING = 6,
            USER_MUTED = 7,
            ME_MUTED = 8,
            USER_NOT_TALKING = 9,
            ME_NOT_TALKING = 10
        };

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
        private const int VOICETAB_SMALL_HEIGHT = 130;
        private const int VOICETAB_LARGE_HEIGHT = 500;

        private bool forceExpandContract;

        #endregion Attributes

        #region ctor
        public VoIPClientControl()
        {
            InitializeComponent();

            _aChannels = new Dictionary<string, int>();
            _aChannelAuthorizations = new Dictionary<string, bool>();
            
            // Instantiate the client control
            //m_voipclient = new ConaitoLib.EvoVoIPClass();
            /*
            //connected successfully to server. Result of m_voipclient.Connect(..)
            m_voipclient.OnConnectSuccess += new ConaitoLib.IEvoVoIPEvents_OnConnectSuccessEventHandler( this.voipclient_OnConnectSuccess );
            //failed to connect to server. Result of m_voipclient.Connect(..)
            m_voipclient.OnConnectFailed += new ConaitoLib.IEvoVoIPEvents_OnConnectFailedEventHandler( this.voipclient_OnConnectFailed );
            //server dropped connection
            m_voipclient.OnConnectionLost += new ConaitoLib.IEvoVoIPEvents_OnConnectionLostEventHandler( this.voipclient_OnConnectionLost );
            //server accepted login. Result of m_voipclient.DoLogin(..)
            m_voipclient.OnAccepted += new ConaitoLib.IEvoVoIPEvents_OnAcceptedEventHandler( this.voipclient_OnAccepted );
            //logged out of server. Result of m_voipclient.DoLogout()
            //            m_voipclient.OnLoggedOut += new ConaitoLib.IEvoVoIPEvents_OnLoggedOutEventHandler( this.voipclient_OnLoggedOut );
            //a new user entered your channel
            m_voipclient.OnAddUser += new ConaitoLib.IEvoVoIPEvents_OnAddUserEventHandler( this.voipclient_OnAddUser );
            //a user in your channel updated his information, e.g. nickname, status, etc.
            m_voipclient.OnUpdateUser += new ConaitoLib.IEvoVoIPEvents_OnUpdateUserEventHandler( this.voipclient_OnUpdateUser );
            //a user left your channel
            m_voipclient.OnRemoveUser += new ConaitoLib.IEvoVoIPEvents_OnRemoveUserEventHandler( this.voipclient_OnRemoveUser );
            //a user in your channel is talking
            m_voipclient.OnUserTalking += new ConaitoLib.IEvoVoIPEvents_OnUserTalkingEventHandler( this.voipclient_OnUserTalking );
            //a user in your channel stopped talking
            m_voipclient.OnUserStoppedTalking += new ConaitoLib.IEvoVoIPEvents_OnUserStoppedTalkingEventHandler( this.voipclient_OnUserStoppedTalking );
            //a new channel was created on the server
            m_voipclient.OnAddChannel += new ConaitoLib.IEvoVoIPEvents_OnAddChannelEventHandler( this.voipclient_OnAddChannel );
            //a channel updated its information, e.g. its number of users
            m_voipclient.OnUpdateChannel += new ConaitoLib.IEvoVoIPEvents_OnUpdateChannelEventHandler( this.voipclient_OnUpdateChannel );
            //a channel was deleted from the server
            m_voipclient.OnRemoveChannel += new ConaitoLib.IEvoVoIPEvents_OnRemoveChannelEventHandler( this.voipclient_OnRemoveChannel );
            //you have joined a new channel. Result of m_voipclient.DoJoinChannel
            m_voipclient.OnJoinedChannel += new ConaitoLib.IEvoVoIPEvents_OnJoinedChannelEventHandler( this.voipclient_OnJoinedChannel );
            //you have left the channel. Result of m_voipclient.DoLeaveChannel
            m_voipclient.OnLeftChannel += new ConaitoLib.IEvoVoIPEvents_OnLeftChannelEventHandler( this.voipclient_OnLeftChannel );
            //a hotkey has become active
            m_voipclient.OnHotKeyToggle += new ConaitoLib.IEvoVoIPEvents_OnHotKeyToggleEventHandler( this.voipclient_OnHotKeyToggle );
            //notification containing the raw audio (PCM data) which was played when a user was talking.
            m_voipclient.OnUserAudioData += new ConaitoLib.IEvoVoIPEvents_OnUserAudioDataEventHandler( this.voipclient_OnUserAudioData );
            //a channel message
            m_voipclient.OnChannelMessage += new ConaitoLib.IEvoVoIPEvents_OnChannelMessageEventHandler(this.voipclient_OnChannelMessage);
            // Error notifications
            m_voipclient.OnCommandError += new ConaitoLib.IEvoVoIPEvents_OnCommandErrorEventHandler(this.voipclient_OnCommandError);
            */

            this.m_timer1 = new System.Windows.Forms.Timer( this.components );

            // Current mike input level
            vumeterProgressBar.Maximum = 20;
            vumeterProgressBar.Minimum = 0;

            _aChannelAuthorizations.Add( "", true );
            _aChannelAuthorizations.Add( "DDD", true );

            _voiceChannels = new Dictionary<string, VoiceChannel>();

            voiceSettingsP.Visible = false;
            logP.Visible = false;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }
        #endregion ctor

        public void initialize(
            IVoiceClientEventCommunicator controller,
            String strDM, 
            String strVoiceServerHostname, 
            int iVoiceServerPort, 
            String strConaitoServerPasswd )
        {
            if ( null == controller )
            {
                throw new Exception( "Voice client controller is null" );
            }
            m_eventCommunicator = controller;
            _strDM = strDM;
            _strVoiceServerHostname = strVoiceServerHostname;
            _iVoiceServerPort = iVoiceServerPort;
            _strConaitoServerPasswd = strConaitoServerPasswd;

            // It can take a while for a remote server to exchange the messages,
            // so give it a few tries to initialize.
            bool bAuthorized = false;
            for ( int i = 0; i < 5; i++ )
            {
                if ( m_voipclient.InitVoIP( true ) )
                {
                    bAuthorized = true;
                    logDebugOnly(
                        String.Format( "Conaito initialization succeeded for user {0}\n", strDM ) );
                    logDebugOnly(
                        String.Format( "  VoiceServerHostname = {0}, VoiceServerPort = {1}\n",
                        strVoiceServerHostname, Convert.ToInt32( iVoiceServerPort ).ToString() ) );
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep( 100 );
                }
            }

            if (!bAuthorized)
            {
                throw new Exception( "Voice client failed to initialize" );
            }

            foreach (String channelName in DDD_Global.Instance.VoiceChannels.Keys)
            {
                notifyVoiceChannelCreated(channelName, DDD_Global.Instance.VoiceChannels[channelName]);
            }

            // Further Voice initialization. Moved from control load event. Bug 4598
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

                this.m_timer1.Tick += new System.EventHandler(this.timer1_Tick);

                // Give it a little time or you won't get the nodes when you log in
                // I found it works at 1/2 second but not less  -- shorvitz 3/10/08
                System.Threading.Thread.Sleep(500);
                login();
            }
            
        }

        // ----------------------------------------------------------------------------

        #region Conaito Event Handlers

        private void voipclient_OnAccepted( int nUserID )
        {
            logDebugOnly( String.Format( "Conaito OnAccepted event received for user {0}\n", nUserID ) );
            m_bLoggedIn = true;
        }

        private void voipclient_OnConnectSuccess()
        {
            logDebugOnly( "Connection to Conaito server succeeded\n" );
        }

        private void voipclient_OnConnectFailed()
        {
            logDebugOnly( "Connection to Conaito server failed\n" );
            m_voipclient.Disconnect();
        }

        private void voipclient_OnConnectionLost()
        {
            logDebugOnly( "Connection to Conaito server lost\n" );
            m_voipclient.Disconnect();
        }

        private void voipclient_OnAddChannel( int nChannelID )
        {
            Channel channel = m_voipclient.GetChannel(nChannelID);
            string strChannelName = channel.szName;
            if ( !_aChannels.ContainsKey( strChannelName ) )
            {
                _aChannels.Add( strChannelName, nChannelID );
                logDebugOnly( String.Format( "_aChannels.Add({0}:{1})\n", strChannelName, Convert.ToInt32( nChannelID ).ToString() ) );
            }

            logDebugOnly( String.Format( "Conaito OnAddChannel event received for channel {0}, ID={1}\n", strChannelName, nChannelID ) );
            if ( _aChannelAuthorizations.ContainsKey( strChannelName ) )
            {
                bool bAuthorized = _aChannelAuthorizations[strChannelName];
                if ( bAuthorized )
                {
                    AddChannel( nChannelID );
                }

                
            }
            else
            {
                logDebugOnly( String.Format( "{0} not authorized to view channel {1}\n", _strDM, strChannelName ) );
            }
        }

        private void voipclient_OnJoinedChannel(int nChannelID)
        {
            logDebugOnly(String.Format("Conaito OnJoinedChannel event received for channel {0}\n", nChannelID));
        }

        private void voipclient_OnLeftChannel( int nChannelID )
        {
            logDebugOnly( String.Format( "Conaito OnLeftChannel event received for channel {0}\n", nChannelID ) );
            TreeNode node = GetChannelNode( nChannelID );
            if ( node != null )
            {
                TreeNodeCollection nodes = channelsTreeView.Nodes;
                for ( int i = 0; i < nodes.Count; )
                {
                    if ( IsNodeVoiceUser(node) )
                        nodes.Remove( node );
                    else
                        i++;
                }
            }
            LeftChannel();
        }

        private void voipclient_OnUpdateChannel( int nChannelID )
        {
            logDebugOnly(String.Format("Conaito OnUpdateChannel event received for channel {0}\n", nChannelID));
            UpdateChannel( nChannelID );
        }

        private void voipclient_OnRemoveChannel( int nChannelID )
        {
            logDebugOnly( String.Format( "Conaito OnRemoveChannel event received for channel {0}\n", nChannelID ) );
        }

        private void voipclient_OnAddUser(int nUserID, int nChannelID)
        {
            logDebugOnly(String.Format("Conaito OnAddUser event received for channel {0}\n", nChannelID));
            AddUser(nUserID, nChannelID);

        }

        private void voipclient_OnUpdateUser( int nUserID, int nChannelID )
        {
            //if ( m_iUserId == nUserID )
            //{
                logDebugOnly( String.Format( "Conaito OnUpdateUser event received for channel {0}\n", nChannelID ) );
                UpdateUser( nUserID, nChannelID );
            //}
        }

        private void voipclient_OnRemoveUser( int nUserID, int nChannelID )
        {
            //if ( m_iUserId == nUserID )
            //{
                logDebugOnly( String.Format( "Conaito OnRemoveUser event received for channel {0}\n", nChannelID ) );
                RemoveUser( nUserID, nChannelID );
            //}
        }

        private string GetUserNickName(int nUserID)
        {
            User user = m_voipclient.GetUser(nUserID);
            string username = user.szNickName;

            if ((username == null) && (nUserID == m_iUserId))
            {
                username = _strDM;
            }

            return username;
        }

        private void voipclient_OnHotKeyToggle( int nHotKeyID, bool bActive )
        {
            logDebugOnly( String.Format( "Conaito OnHotKeyToggle event received\n" ) );
            if ( nHotKeyID == kiPUSH_TO_TALK_ID )
            {
                int nUserID = m_voipclient.GetMyUserID();
                if ( bActive )
                {
                    sendDDDRequestStartedTalkingEvent();
                    //m_voipclient.StartTransmitting();
                }
                else
                {
                    sendDDDRequestStoppedTalkingEvent();
                    //m_voipclient.StopTransmitting();
                }
            }
        }

        private void voipclient_OnUserTalking( int nUserID )
        {            
            // make sure the user is this user
            if (m_iUserId == nUserID)
            {
                //switch to talking icon
                logDebugOnly(String.Format("Conaito OnUserTalking event received from my userid\n"));
                sendDDDRequestStartedTalkingEvent();
            }
            else
            {
                User user = m_voipclient.GetUser(nUserID);
                string username = user.szNickName;

                logDebugOnly(String.Format("Conaito OnUserTalking event received from %s\n", username));
            }
        }

        private void voipclient_OnUserStoppedTalking( int nUserID )
        {
            //make sure the user is this user
            if (m_iUserId == nUserID)
            {
                //switch to non-talking icon
                logDebugOnly(String.Format("Conaito OnUserStoppedTalking event received from my userid\n"));
                sendDDDRequestStoppedTalkingEvent();
            }
            else
            {
                User user = m_voipclient.GetUser(nUserID);
                string username = user.szNickName;

                logDebugOnly(String.Format("Conaito OnUserStoppedTalking event received from %s\n", username));
            }
        }

        private void voipclient_OnUserAudioData( int userid, int samplerate, ref System.Array rawAudio, int samples )
        {
            short[] buff = ( short[] )rawAudio;
        }

        private void voipclient_OnCommandError(int nError)
        {
            logDebugOnly(String.Format("Conaito Error code: " + nError.ToString() + "\n"));
        }

        private void voipclient_OnChannelMessage(int nChannelID, int nFromUserID, string message)
        {
            //System.Diagnostics.Trace.WriteLine("Channel Message = " + message + "ChannelID = " + nChannelID.ToString());

            if (message.StartsWith("RequestMuteStatus:"))
            {
                string username = "";
                string channelName = "";
                string [] split = message.Split(new Char[] {' ','='});
                string requestedByUsername = "";

                if (split.Length != 7)
                {
                    return;
                }

                username = split[2];
                channelName = split[4];
                requestedByUsername = split[6];

                if ((string.Compare(username, _strDM) == 0) && (_voiceChannels[channelName] != null))
                {
                    if (_voiceChannels[channelName].Talk)
                    {
                        m_voipclient.DoChannelMessage("MuteStatus: Unmuted Username=" + username +
                            " ChannelName=" + channelName + " RequestedBy=" + requestedByUsername);
                    }
                    else
                    {
                        m_voipclient.DoChannelMessage("MuteStatus: Muted Username=" + username +
                            " ChannelName=" + channelName + " RequestedBy=" + requestedByUsername);
                    }
                }
            }
            else if (message.StartsWith("MuteStatus:"))
            {
                string username = "";
                string channelName = "";
                string requestedByUsername = "";
                string[] split = message.Split(new Char[] { ' ', '=' });
                bool bMuted = false;

                if (split.Length != 8)
                {
                    return;
                }

                username = split[3];
                channelName = split[5];
                requestedByUsername = split[7];

                if (string.Compare(requestedByUsername, _strDM) != 0)
                {
                    return;
                }

                if (string.Compare(split[1], "Muted") == 0)
                {
                    bMuted = true;
                }

                // Change the mute icon
                UpdateUserIcon(username, channelName, false, bMuted);
            }
        }

        #endregion Conaito Event Handlers

        // ----------------------------------------------------------------------------

        #region Helper Functions

        private bool ChannelAlreadyExists(TreeNode parent, string strChannelName)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.Tag != null)
                {
                    string channelName = node.Text;

                    if (string.Compare(channelName, strChannelName) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddChannel( int nChannelID )
        {
            Channel channel = m_voipclient.GetChannel(nChannelID);
            string strChannelName = channel.szName;
            string path = m_voipclient.GetChannelPath( nChannelID );
            if ( null == path )
            {
                path = "/";
            }

            if ( 0 == path.CompareTo( "/DDD/" ) )
            {
                String strChannelPassword = "";
                String strTopic = "";
                String strOpPassword = "";

                // create the root node
                channelsTreeView.Nodes.Clear();
                TreeNode node = new TreeNode( strChannelName, ( int )ImageIndex.CHANNEL, ( int )ImageIndex.CHANNEL, new TreeNode[0] );
                node.Tag = nChannelID;
                
                m_iDDDChannelID = nChannelID;
                channelsTreeView.Nodes.Add( node );
                m_voipclient.DoJoinChannel(path, strChannelPassword, strChannelPassword, strOpPassword);
            }
            else
            {
                if ( path.Contains( "DDD" ) )
                {
                    string[] tokens = TokenizeChannelPath( path );
                    int i = 0;
                    path = "/";
                    while ( i < tokens.Length - 1 )//strip the new channel
                        path += tokens[i++] + "/";

                    //add new sub channel
                    TreeNode node = new TreeNode( tokens[tokens.Length - 1], ( int )ImageIndex.CHANNEL, ( int )ImageIndex.CHANNEL, new TreeNode[0] );
                    node.Tag = nChannelID;
                    node.ToolTipText = "Double click to join or leave the voice channel";
                    TreeNode parent = GetChannelNode( channel.nParentID );
                    if ( parent != null )
                    {
                        // Make sure this parent does not already contain this channel
                        if (ChannelAlreadyExists(parent, strChannelName))
                        {
                            return;
                        }

                        parent.Nodes.Add( node );
                        forceExpandContract = true;
                        parent.Expand();
                        forceExpandContract = false;
                        if ( kbDEBUG )
                        {
                            logDebugOnly( String.Format( "Channel {0} : {1} added\n", strChannelName, nChannelID ) );
                        }
                        else
                        {
                            // We actually want the user to see this
                            richTextBoxLog.AppendText( String.Format( "Channel {0} added\n", strChannelName ) );
                        }
                    }

                    _voiceChannels[strChannelName] = new VoiceChannel();
                    _voiceChannels[strChannelName].initialize(_strDM, _aChannels[strChannelName], _strVoiceServerHostname, _iVoiceServerPort, _strConaitoServerPasswd,
                        m_eventCommunicator);
                    _voiceChannels[strChannelName].SetMasterVolume(255);//set to max, temp bug fix
                    
                    _voiceChannels[strChannelName].SetVoiceActivated(voiceActRadio.Checked);
                    _voiceChannels[strChannelName].SetVoiceActivationLevel(voiceactTrackBar.Value);
                    _voiceChannels[strChannelName].SetPushToTalk(pushToTalkRadio.Checked);
                   
                }
            }
        }

        public void UpdateChannel( int nChannelID )
        {
        }

        public void RemoveChannel( int nChannelID )
        {
            TreeNode node = GetChannelNode(nChannelID);
            if ( node != null )
            {
                node.Parent.Nodes.Remove( node );
                String strChannelName = null;
                foreach ( string strKey in _aChannels.Keys )
                {
                    if ( nChannelID == _aChannels[strKey] )
                    {
                        strChannelName = strKey;
                        break;
                    }
                }
                if ( null != strChannelName )
                {
                    richTextBoxLog.AppendText( String.Format( "Channel {0} removed", strChannelName ) );
                }
                logDebugOnly( String.Format( "Channel {0} removed\n", nChannelID ) );

                _voiceChannels[strChannelName].Disconnect();
                _voiceChannels.Remove(strChannelName);
            }
        }

        private bool Connect()
        {

            if ( null == _strVoiceServerHostname )
            {
                _strVoiceServerHostname = "localhost";
            }
            if (!m_voipclient.Connect( _strVoiceServerHostname, _iVoiceServerPort, kiUDPPort, 0, 0 ))
            {
                logDebugOnly(String.Format("Failed to connect to voice server: Name={0} Port={1}",
                    _strVoiceServerHostname, _iVoiceServerPort));
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
            if (isRunning)
            {
                logDebugOnly( "Connection to Conaito server succeeded\n" );
                // wait for OnConnectSuccess or OnConnectFailed event
                return true;
            }
            else
            {
                logDebugOnly(String.Format("Connection to Conaito server failed: Name={0} Port={1}\n",
                    _strVoiceServerHostname, _iVoiceServerPort));
                return false;
            }
        }

        private bool Disconnect()
        {
            if (m_voipclient.IsConnected() || m_voipclient.IsConnecting())
            {
                bool bStatus = m_voipclient.Disconnect();
                if ( bStatus )
                {
                    logDebugOnly( "Disconnection from Conaito server succeeded\n" );
                }
                else
                {
                    logDebugOnly( "Disconnection from Conaito server failed\n" );
                }
                return bStatus;
            }
            else
            {
                return false;
            }
        }

        private void ClearState()
        {
            channelsTreeView.Nodes.Clear();
        }

        private bool IsNodeVoiceUser(TreeNode node)
        {
            return ((node.ImageIndex == (int)ImageIndex.USER_NOT_JOINED) ||
                (node.ImageIndex == (int)ImageIndex.ME_NOT_JOINED) ||
                (node.ImageIndex == (int)ImageIndex.USER_TALKING) ||
                (node.ImageIndex == (int)ImageIndex.ME_TALKING) ||
                (node.ImageIndex == (int)ImageIndex.USER_MUTED) ||
                (node.ImageIndex == (int)ImageIndex.ME_MUTED) ||
                (node.ImageIndex == (int)ImageIndex.USER_NOT_TALKING) ||
                (node.ImageIndex == (int)ImageIndex.ME_NOT_TALKING));
        }

        public TreeNode GetChannelNode( int nChannelID )
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            foreach ( TreeNode node in channelsTreeView.Nodes )
                queue.Enqueue( node );

            while ( queue.Count > 0 )
            {
                TreeNode node = queue.Dequeue();
                if ( node.ImageIndex == ( int )ImageIndex.CHANNEL && ( int )node.Tag == nChannelID )
                    return node;

                foreach ( TreeNode n in node.Nodes )
                {
                    queue.Enqueue( n );
                }
            }
            return null;
        }

        public List<TreeNode> GetUserNodes(string username)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            List<TreeNode> nodes = new List<TreeNode>();

            foreach (TreeNode node in channelsTreeView.Nodes)
                queue.Enqueue(node);

            while (queue.Count > 0)
            {
                TreeNode node = (TreeNode)queue.Dequeue();
                if (IsNodeVoiceUser(node))
                    if (string.Compare(username, GetUserNickName((int)node.Tag)) == 0)
                        nodes.Add(node);

                foreach (TreeNode n in node.Nodes)
                {
                    queue.Enqueue(n);
                }
            }
            return nodes;
        }

        public TreeNode GetUserNode(int nUserID)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            foreach ( TreeNode node in channelsTreeView.Nodes )
                queue.Enqueue( node );

            while ( queue.Count > 0 )
            {
                TreeNode node = ( TreeNode )queue.Dequeue();

                if (IsNodeVoiceUser(node))
                    if ((int)node.Tag == nUserID)
                        return node;

                foreach ( TreeNode n in node.Nodes )
                {
                    queue.Enqueue( n );
                }
            }
            return null;
        }

        public int GetSelectedUser()
        {
            TreeNode node = channelsTreeView.SelectedNode;
            if ( node != null )
            {
                if ( IsNodeVoiceUser(node) )
                    return ( int )node.Tag;
            }

            return -1;	//user IDs are always positive
        }

        public void RequestMuteStatus(int nUserID, string strChannelName)
        {
            User user = m_voipclient.GetUser(nUserID);
            m_voipclient.DoChannelMessage("RequestMuteStatus: Username=" + user.szNickName +
                " ChannelName=" + strChannelName + " RequestedBy=" + _strDM);
        }

        private bool initSoundSystem()
        {
            int iInputDeviceCount = m_voipclient.GetInputDeviceCount( kiSoundsystem );
            int iInputDeviceID = m_voipclient.GetInputDeviceID( kiSoundsystem, iInputDeviceCount - 1 );
            if ( m_voipclient.LaunchSoundSystem( kiSoundsystem, iInputDeviceID, kiOutputDeviceId, 32000, 5, true ) )
            {
                logDebugOnly( "Sound system launched\n" );
                m_timer1.Start();
                return true;
            }
            else
            {
                logDebugOnly("Sound system failed to launched\n");
                return false;
            }
        }

        public void LeftChannel()
        {
            TreeNode node = null;

            TreeNodeCollection nodes = node.Nodes;
            for (int i = 0; i < nodes.Count; )
                if (IsNodeVoiceUser(nodes[i]))
                    nodes.RemoveAt(i);
                else
                    i++;

        }

        private bool login()
        {            
            bool bStatus = true;
            if ( !m_bLoggedIn )
            {
                if ( m_voipclient.DoLogin( _strDM, _strConaitoServerPasswd ) )
                {
                    richTextBoxLog.AppendText( String.Format( "Login for {0} succeeded\n", _strDM ) );
                    m_iUserId = m_voipclient.GetMyUserID();
                    m_bLoggedIn = true;

                    // Get the root channel
                    AddChannel( m_voipclient.GetRootChannelID() );
                }
                else
                {
                    if ( kbDEBUG )
                    {
                        logDebugOnly( String.Format( "Login for {0} failed with password {1}\n", _strDM, _strConaitoServerPasswd ) );
                    }
                    else
                    {
                        richTextBoxLog.AppendText( String.Format( "Login for {0} failed\n", _strDM ) );
                    }
                    bStatus = false;
                }
            }
            return bStatus;
        }

        public static string[] TokenizeChannelPath( string channelpath )
        {
            string[] tokens = channelpath.Split( '/' );
            ArrayList list = new ArrayList();
            for ( int i = 0; i < tokens.Length; i++ )
                list.Add( tokens[i] );
            for ( int i = 0; i < list.Count; )
                if ( ( ( string )list[i] ).Length == 0 )
                    list.RemoveAt( i );
                else
                    i++;

            string[] newtokens = new string[list.Count];
            for ( int i = 0; i < list.Count; i++ )
                newtokens[i] = ( string )list[i];
            return newtokens;
        }

        private bool IsUserMe(int nUserID)
        {
            string username = GetUserNickName(nUserID);

            if (username == null)
            {
                return false;
            }

            return (string.Compare(username, DDD_Global.Instance.PlayerID) == 0);
        }

        public void AddUser( int nUserID, int nChannelID )
        {
            TreeNode node = GetChannelNode(nChannelID);
            Channel channel = m_voipclient.GetChannel(nChannelID);
            string channelName = channel.szName;
            bool channelAdded = false;
            if (node == null)
            {
                if (_aChannelAuthorizations.ContainsKey(channelName))
                {
                    if (_aChannelAuthorizations[channelName])
                    { 
                        AddChannel(nChannelID);
                        node = GetChannelNode(nChannelID);
                        channelAdded = true;
                    }
                }
                // Need to add this channel to the tree
                
            }
            if (!channelAdded && node == null)
            {
                return;
            }

            if (nChannelID == m_iDDDChannelID)
            {
                // Do not need to add icon for DDD global channel
                return;
            }

            if (node != null)
            {
                int iImageIndex = ( int )ImageIndex.USER_NOT_TALKING;
                TreeNode newnode = null;

                if (IsUserMe(nUserID))
                {
                    iImageIndex = (int)ImageIndex.ME_NOT_TALKING;
                }
                else
                {
                    if (_voiceChannels[channelName] != null)
                    {
                        RequestMuteStatus(nUserID, channelName);
                    }
                }

                newnode = new TreeNode(GetUserNickName(nUserID), iImageIndex, iImageIndex, new TreeNode[0]);
                if (IsUserMe(nUserID))
                {
                    newnode.ToolTipText = "Double click to toggle mute";
                }
                newnode.Tag = nUserID;
                node.Nodes.Add( newnode );
                forceExpandContract = true;
                node.Expand();
                forceExpandContract = false;
            }
        }
        
        public void UpdateUser( int nUserID, int nChannelID )
        {
            //if ( m_iUserId == nUserID )
            //{
                TreeNode node = GetUserNode( nUserID );
                if ( node != null )
                    node.Text = GetUserNickName( nUserID );
            //}
        }
        
        private void UpdateUserIcon( string username, string channelName, bool bTalking, bool bMuted )
        {
            List<TreeNode> nodes = GetUserNodes( username );
            bool isUserMe = string.Compare(username, _strDM) == 0;
            string nodeChannelName;

            foreach (TreeNode node in nodes)
            {
                if (node.Parent != null)
                {
                    Channel channel = m_voipclient.GetChannel((int)node.Parent.Tag);
                    nodeChannelName = channel.szName;

                    if (node.Parent.ImageIndex != (int)ImageIndex.CHANNEL)
                    {
                        continue;
                    }

                    if (string.Compare(channelName, nodeChannelName) != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                if (bMuted)
                {
                    if (isUserMe)
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.ME_MUTED;
                    }
                    else
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.USER_MUTED;
                    }
                }
                else if (bTalking)
                {
                    if ((node.SelectedImageIndex == (int)ImageIndex.USER_MUTED) ||
                        (node.SelectedImageIndex == (int)ImageIndex.ME_MUTED))
                    {
                        continue;
                    }

                    if (isUserMe)
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.ME_TALKING;
                    }
                    else
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.USER_TALKING;
                    }
                }
                else
                {
                    if (isUserMe)
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.ME_NOT_TALKING;
                    }
                    else
                    {
                        node.SelectedImageIndex = node.ImageIndex = (int)ImageIndex.USER_NOT_TALKING;
                    }
                }
            }
        }

        public void RemoveUser( int nUserID, int nChannelID )
        {
            //if ( m_iUserId == nUserID )
            //{
                TreeNode node = GetUserNode( nUserID );
                if ( node != null )
                    node.Parent.Nodes.Remove( node );
            //}
        }

        #endregion Helper Functions

        // ----------------------------------------------------------------------------

        #region Forms Event Handlers

        private void VoIPClientControl_Load( object sender, EventArgs e )
        {
            // Expand all nodes on the first load of the client control
            forceExpandContract = true;
            channelsTreeView.ExpandAll();
            forceExpandContract = false;
        }

        private void timer1_Tick( object sender, System.EventArgs e )
        {
            if (m_voipclient.IsSoundSystemInitialized())
            {
                int nValue = m_voipclient.GetCurrentInputLevel();
                vumeterProgressBar.Value = nValue > 20 ? 20 : nValue;
            }
        }

        void channelsTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (forceExpandContract == false)
            {
                e.Cancel = true;
            }
        }

        void channelsTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (forceExpandContract == false)
            {
                e.Cancel = true;
            }
        }

        private void channelsTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Get the Node information
            int iChannelId = (int)e.Node.Tag;
            Channel channel = m_voipclient.GetChannel(iChannelId);
            String channelName = channel.szName;
            String userName = GetUserNickName((int)e.Node.Tag);

            if (channelName != null && _voiceChannels.ContainsKey(channelName) &&
                !IsNodeVoiceUser(e.Node))
            {
                // Want to Join or Leave a channel
                if (channelName != null && _voiceChannels.ContainsKey(channelName))
                {
                    if (!_voiceChannels[channelName].IsJoined())
                    {
                        // Join the channel
                        sendDDDRequestJoinVoiceChannelEvent(channelName, iChannelId);
                    }
                    else
                    {
                        // Leave the channel
                        sendDDDRequestLeaveVoiceChannelEvent(channelName);
                    }
                }

            }
            else if (userName != null)
            {
                // Make sure the parent of this node is a voice room
                if (e.Node.Parent != null)
                {
                    iChannelId = (int)e.Node.Parent.Tag;
                    Channel parentChannel = m_voipclient.GetChannel(iChannelId);
                    channelName = parentChannel.szName;

                    if (e.Node.Parent.ImageIndex != (int)ImageIndex.CHANNEL)
                    {
                        return;
                    }

                    if (_voiceChannels[channelName] != null)
                    {
                        if (_voiceChannels[channelName].Talk)
                        {
                            if (IsUserMe((int) e.Node.Tag))
                            {
                                _voiceChannels[channelName].Talk = false;
                                sendDDDRequestMuteUserEvent(channelName);
                            }
                        }
                        else
                        {
                            if (IsUserMe((int) e.Node.Tag))
                            {
                                _voiceChannels[channelName].Talk = true;
                                sendDDDRequestUnmuteUserEvent(channelName);
                            }
                        }
                    }
                }
            }
        }

        private void mastervolTrackBar_ValueChanged( object sender, EventArgs e )
        {
            //m_voipclient.SetMasterVolume(mastervolTrackBar.Value);
            foreach (VoiceChannel vc in _voiceChannels.Values)
            {
                vc.SetMasterVolume(mastervolTrackBar.Value);
            }
        }

        private void voiceactTrackBar_ValueChanged( object sender, EventArgs e )
        {
            //m_voipclient.SetVoiceActivationLevel( voiceactTrackBar.Value );
            foreach (VoiceChannel vc in _voiceChannels.Values)
            {
                vc.SetVoiceActivationLevel(voiceactTrackBar.Value);
            }
        }

        private void pushToTalkRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (VoiceChannel vc in _voiceChannels.Values)
            {
                vc.SetPushToTalk(pushToTalkRadio.Checked);
            }
            //if (pushToTalkRadio.Checked)
            //{
            //    // Register push-to-talk key
            //    bHotKeyRegistered = m_voipclient.RegisterHotKey(
            //        kiPUSH_TO_TALK_ID,
            //        false,  // control
            //        false,  // alt
            //        false,  // shift
            //        false,  // windows key
            //        kcPushToTalkKey);
            //    richTextBoxLog.AppendText(String.Format("Push To Talk activated.\n"));
            //}
            //else
            //{
            //    // UnRegister push-to-talk key
            //    m_voipclient.UnregisterHotKey(kiPUSH_TO_TALK_ID);
            //    bHotKeyRegistered = false;
            //}
        }

        private void voiceActRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (VoiceChannel vc in _voiceChannels.Values)
            {
                vc.SetVoiceActivated(voiceActRadio.Checked);
            }

            //m_voipclient.SetVoiceActivated(voiceActRadio.Checked);
            //if (voiceActRadio.Checked)
            //{
            //    richTextBoxLog.AppendText(String.Format("Voice activated talking activated\n"));
            //}

        }

        private void closeButton_Click( object sender, EventArgs e )
        {
            foreach (String channelName in _voiceChannels.Keys)
            {
                if (_voiceChannels[channelName].IsJoined())
                {
                    sendDDDRequestLeaveVoiceChannelEvent(channelName);
                }
            }

            if ( this.m_bLoggedIn )
            {
                m_voipclient.DoLogout();
            }

            if ( m_voipclient.IsConnected() )
            {
                m_voipclient.Disconnect();
            }

            if ( m_voipclient.ShutdownSoundSystem() )
            {
                m_timer1.Stop();
            }
            m_voipclient.CloseVoIP();

            this.ParentForm.Close();
        }

        #endregion Forms Event Handlers

        // ----------------------------------------------------------------------------
        #region DDD events
        #region DDD events sent from voice client
        private void sendDDDRequestJoinVoiceChannelEvent(string strChannelName, int iChannelId )
        {
            richTextBoxLog.AppendText( String.Format( "DDD Request Join Voice Channel {0} Event\n", strChannelName ) );
            m_eventCommunicator.sendRequestJoinVoiceChannelEvent( strChannelName );
        }

        private void sendDDDRequestLeaveVoiceChannelEvent(String channelName)
        {
            String strChannelName = channelName;

            if (String.Empty != strChannelName)
            {
                richTextBoxLog.AppendText(String.Format("DDD Request Leave Voice Channel {0} Event\n", strChannelName));
                m_eventCommunicator.sendRequestLeaveVoiceChannelEvent(strChannelName);
            }
        }
        private void sendDDDRequestLeaveVoiceChannelEvent()
        {
            sendDDDRequestLeaveVoiceChannelEvent(m_curSelectedChannel);
        }

        private void sendDDDRequestStartedTalkingEvent()
        {
            richTextBoxLog.AppendText( "DDD RequestStartedTalking event\n" );

            List<TreeNode> nodes = GetUserNodes(_strDM);
            string channelName;

            foreach (TreeNode node in nodes)
            {
                if (node.Parent != null)
                {
                    Channel channel = m_voipclient.GetChannel((int)node.Parent.Tag);
                    channelName = channel.szName;

                    if (node.Parent.ImageIndex != (int)ImageIndex.CHANNEL)
                    {
                        continue;
                    }

                    if ((_voiceChannels[channelName] == null) || (!_voiceChannels[channelName].Talk))
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                m_eventCommunicator.sendRequestStartedTalkingVoiceChannelEvent(channelName);
            }
        }

        private void sendDDDRequestStoppedTalkingEvent()
        {
            richTextBoxLog.AppendText( "DDD RequestStoppedTalkingEvent\n" );

            List<TreeNode> nodes = GetUserNodes(_strDM);
            string channelName;

            foreach (TreeNode node in nodes)
            {
                if (node.Parent != null)
                {
                    Channel channel = m_voipclient.GetChannel((int)node.Parent.Tag);
                    channelName = channel.szName;

                    if (node.Parent.ImageIndex != (int)ImageIndex.CHANNEL)
                    {
                        continue;
                    }

                    if ((_voiceChannels[channelName] == null) || (!_voiceChannels[channelName].Talk))
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                m_eventCommunicator.sendRequestStoppedTalkingVoiceChannelEvent(channelName);
            }
        }

        private void sendDDDRequestMuteUserEvent(string channelName)
        {
            richTextBoxLog.AppendText("DDD RequestMuteUser event\n");

            m_eventCommunicator.sendRequestMuteUserEvent(channelName);
        }

        private void sendDDDRequestUnmuteUserEvent(string channelName)
        {
            richTextBoxLog.AppendText("DDD RequestUnmuteUser event\n");

            m_eventCommunicator.sendRequestUnmuteUserEvent(channelName);
        }

        #endregion DDD events sent from voice client

        #region DDD events listened to by voice client
        // We don't add the channel when we get the Conaito AddChannel event because we don't want
        // to display channels unless the user is authorized to view them.  We get the
        // authorization list here.  So this is how we resolve the timing of when we
        // display the channels.
        public void notifyVoiceChannelCreated( string strChannelName, List<string> astrMembershipList )
        {
            if ( _aChannelAuthorizations.ContainsKey( strChannelName ) )
            {
                _aChannelAuthorizations.Remove( strChannelName );
            }
            StringBuilder sb = new StringBuilder();
            for ( int i = 0; i < astrMembershipList.Count; i++ )
            {
                sb.Append( astrMembershipList[i] );
                if ( i < astrMembershipList.Count - 1 )
                {
                    sb.Append( ", " );
                }
            }
            logDebugOnly( String.Format( "NotifyVoiceChannelCreated for {0}: {1}\n", strChannelName, sb.ToString() ) );
            if ( _aChannelAuthorizations.ContainsKey( strChannelName ) )
            {
                _aChannelAuthorizations.Remove( strChannelName );
            }
            _aChannelAuthorizations.Add( strChannelName, astrMembershipList.Contains( _strDM ) );

            // We don't know the order in which we receive DDD vs conaito events, so if we
            // already got the conaito event, add the channel now.
            if ( _aChannels.ContainsKey( strChannelName ) && astrMembershipList.Contains( _strDM ) )
            {
                AddChannel( _aChannels[strChannelName] );

                
            }


            

        }

        public void notifyVoiceChannelClosed( string strChannelName )
        {
            logDebugOnly( String.Format( "NotifyVoiceChannelClosed for {0}\n", strChannelName ) );
            sendDDDRequestLeaveVoiceChannelEvent();
            if ( _aChannelAuthorizations.ContainsKey( strChannelName ) )
            {
                _aChannelAuthorizations.Remove( strChannelName );
            }

            if ( _aChannels.ContainsKey( strChannelName ) )
            {
                RemoveChannel( _aChannels[strChannelName] );
                _aChannels.Remove( strChannelName );
            }
        }

        public void notifyJoinVoiceChannel( string strChannelName )
        {
            if ( _aChannels.ContainsKey( strChannelName ) )
            {
                int iChannelId = _aChannels[strChannelName];
                String strChannelPassword = "";
                String strTopic = "";
                String strOpPassword = "";

                _voiceChannels[strChannelName].JoinChannel();
                
            }
            else
            {
                throw new Exception( String.Format( "{0} is not authorized to join {1}", _strDM, strChannelName ) );
            }
        }

        public void notifyLeaveVoiceChannel( string strChannelName )
        {
            
            _voiceChannels[strChannelName].LeaveChannel();
        }

        public void notifyStartedTalking(string strUsername, string strChannelName)
        {
            //System.Diagnostics.Trace.WriteLine(String.Format("DDD StartedTalking event received for user {0} on channel {1}", strUsername, strChannelName));

            UpdateUserIcon(strUsername, strChannelName, true, false);
        }

        public void notifyStoppedTalking(string strUsername, string strChannelName)
        {
            //System.Diagnostics.Trace.WriteLine(String.Format("DDD StoppedTalking event received for user {0} on channel {1}", strUsername, strChannelName));

            UpdateUserIcon(strUsername, strChannelName, false, false);
        }

        public void notifyMuteUser(string strUsername, string strChannelName)
        {
            //System.Diagnostics.Trace.WriteLine(String.Format("DDD MuteUser event received for user {0} on channel {1}", strUsername, strChannelName));

            UpdateUserIcon(strUsername, strChannelName, false, true);
        }

        public void notifyUnmuteUser(string strUsername, string strChannelName)
        {
            //System.Diagnostics.Trace.WriteLine(String.Format("DDD UnmuteUser event received for user {0} on channel {1}", strUsername, strChannelName));

            UpdateUserIcon(strUsername, strChannelName, false, false);
        }

        #endregion DDD events listened to by voice client
        #endregion DDD events

        #region Logging

        private void logDebugOnly( string strMessage )
        {
            if ( kbDEBUG )
            {
                richTextBoxLog.AppendText( strMessage );
            }
        }
        #endregion Logging

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void voiceOptionsB_Click(object sender, EventArgs e)
        {
            if (string.Compare(voiceOptionsB.Text, "Show Options") == 0)
            {
                voiceOptionsB.Text = "Hide Options";
                voiceSettingsP.Visible = true;
                logP.Visible = false;
            }
            else
            {
                voiceOptionsB.Text = "Show Options";
                voiceSettingsP.Visible = false;
                logP.Visible = false;
            }
        }
    }
}
