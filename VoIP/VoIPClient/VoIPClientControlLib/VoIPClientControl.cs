using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib
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
        private int m_iCurChannelID = -1;
        private int m_iDDDChannelID = Int32.MaxValue;
        private int m_iUserId = -1;

        // key = channelName
        // value = channelID
        private Dictionary<string, int> _aChannels = null;

        // key = channelName
        // value = true means DM is authorized to use that channel
        // Keyed by channelName because we don't always have access to the
        // channelID
        private Dictionary<string, bool> _aChannelAuthorizations = null;

        private enum ImageIndex
        {
            USER_TALKING = 0,
            USER_NOT_TALKING = 1,
            CHANNEL = 2
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

        #endregion Attributes

        #region ctor
        public VoIPClientControl()
        {
            InitializeComponent();

            _aChannels = new Dictionary<string, int>();
            _aChannelAuthorizations = new Dictionary<string, bool>();

            // Instantiate the client control
            m_voipclient = new ConaitoLib.EvoVoIPClass();

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

            this.m_timer1 = new System.Windows.Forms.Timer( this.components );

            // Current mike input level
            vumeterProgressBar.Maximum = 20;
            vumeterProgressBar.Minimum = 0;

            _aChannelAuthorizations.Add( "", true );
            _aChannelAuthorizations.Add( "DDD", true );

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
        }

        // ----------------------------------------------------------------------------

        #region Conaito Event Handlers

        private void voipclient_OnAccepted( int nUserID )
        {
            logDebugOnly( String.Format( "Conaito OnAccepted event received for user {0}\n", nUserID ) );
            m_bLoggedIn = true;
            updateButtonStates();
        }

        private void voipclient_OnConnectSuccess()
        {
            logDebugOnly( "Connection to Conaito server succeeded\n" );
            updateButtonStates();
        }

        private void voipclient_OnConnectFailed()
        {
            logDebugOnly( "Connection to Conaito server failed\n" );
            //call disconnect to clear out any state information
            m_voipclient.Disconnect();
            updateButtonStates();
        }

        private void voipclient_OnConnectionLost()
        {
            logDebugOnly( "Connection to Conaito server lost\n" );
            //call disconnect to clear out any state information
            m_voipclient.Disconnect();
            updateButtonStates();
        }

        private void voipclient_OnAddChannel( int nChannelID )
        {
            //new channel has been created
            string strChannelName = m_voipclient.GetChannelName( nChannelID );
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

        private void voipclient_OnJoinedChannel( int nChannelID )
        {
            logDebugOnly( String.Format( "Conaito OnJoinedChannel event received for channel {0}\n", nChannelID ) );
            // you have joined a channel
            TreeNode node = GetChannelNode( m_iCurChannelID );
            if ( node != null )
            {
                TreeNodeCollection nodes = channelsTreeView.Nodes;
                for ( int i = 0; i < nodes.Count; )
                {
                    if ( ( node.ImageIndex == ( int )ImageIndex.USER_TALKING ) ||
                        ( node.ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) )
                        nodes.Remove( node );
                    else
                        i++;
                }
            }

            JoinedNewChannel( nChannelID );	//users will be posted next
            updateButtonStates();
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
                    if ( ( node.ImageIndex == ( int )ImageIndex.USER_TALKING ) ||
                        ( node.ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) )
                        nodes.Remove( node );
                    else
                        i++;
                }
            }
            LeftChannel();
            updateButtonStates();
        }

        private void voipclient_OnUpdateChannel( int nChannelID )
        {
            //channel has been updated
            logDebugOnly( String.Format( "Conaito OnUpdateChannel event received for channel {0}\n", nChannelID ) );
            UpdateChannel( nChannelID );
        }

        private void voipclient_OnRemoveChannel( int nChannelID )
        {
            //a channel has been removed
            logDebugOnly( String.Format( "Conaito OnRemoveChannel event received for channel {0}\n", nChannelID ) );
        }

        private void voipclient_OnAddUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                //new user has arrived in channel
                logDebugOnly( String.Format( "Conaito OnAddUser event received for channel {0}\n", nChannelID ) );
                AddUser( nUserID, nChannelID );
            }
        }

        private void voipclient_OnUpdateUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                //user has been updated
                logDebugOnly( String.Format( "Conaito OnUpdateUser event received for channel {0}\n", nChannelID ) );
                UpdateUser( nUserID, nChannelID );
            }
        }

        private void voipclient_OnRemoveUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                //user has been removed from channel
                logDebugOnly( String.Format( "Conaito OnRemoveUser event received for channel {0}\n", nChannelID ) );
                RemoveUser( nUserID, nChannelID );
            }
        }

        private void voipclient_OnHotKeyToggle( int nHotKeyID, bool bActive )
        {
            logDebugOnly( String.Format( "Conaito OnHotKeyToggle event received\n" ) );
            //a registered hotkey is active
            if ( nHotKeyID == kiPUSH_TO_TALK_ID )
            {
                if ( bActive )
                {
                    sendDDDPushToTalkEvent();
                    m_voipclient.StartTransmitting();
                    UpdateUserIcon( m_voipclient.GetMyUserID(), true );
                }
                else
                {
                    sendDDDStoppedTalkingEvent();
                    m_voipclient.StopTransmitting();
                    UpdateUserIcon( m_voipclient.GetMyUserID(), false );
                }
            }
        }

        private void voipclient_OnUserTalking( int nUserID )
        {   // make sure the user is this user
            if (m_iUserId == nUserID)
            {
                //switch to talking icon
                logDebugOnly(String.Format("Conaito OnUserTalking event received\n"));
                sendDDDPushToTalkEvent();
                UpdateUserIcon(nUserID, true);
            }
        }

        private void voipclient_OnUserStoppedTalking( int nUserID )
        {   //make sure the user is this user
            if (m_iUserId == nUserID)
            {
                //switch to non-talking icon
                logDebugOnly(String.Format("Conaito OnUserStoppedTalking event received\n"));
                sendDDDStoppedTalkingEvent();
                UpdateUserIcon(nUserID, false);
            }
        }

        private void voipclient_OnUserAudioData( int userid, int samplerate, ref System.Array rawAudio, int samples )
        {
            short[] buff = ( short[] )rawAudio;
        }

        #endregion Conaito Event Handlers

        // ----------------------------------------------------------------------------

        #region Helper Functions
        public void AddChannel( int nChannelID )
        {
            string strChannelName = m_voipclient.GetChannelName( nChannelID );
            string path = m_voipclient.GetChannelPath( nChannelID );
            //MessageBox.Show( String.Format( "AddChannel: channelpath = {0} for channel {1} ID={2}", path, strChannelName, nChannelID ) );
            if ( null == path )
            {
                path = "/";
            }

            if ( 0 == path.CompareTo( "/DDD/" ) )
            {
                // create the root node
                channelsTreeView.Nodes.Clear();
                TreeNode node = new TreeNode( strChannelName, ( int )ImageIndex.CHANNEL, ( int )ImageIndex.CHANNEL, new TreeNode[0] );
                node.Tag = nChannelID;
                m_iDDDChannelID = nChannelID;
                channelsTreeView.Nodes.Add( node );
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
                    TreeNode parent = GetChannelNode( m_voipclient.GetChannelParentID( nChannelID ) );
                    if ( parent != null )
                    {
                        parent.Nodes.Add( node );
                        parent.Expand();
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
                }
            }
        }

        public void UpdateChannel( int nChannelID )
        {
        }

        public void RemoveChannel( int nChannelID )
        {
            TreeNode node = GetChannelNode( nChannelID );
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
                logDebugOnly( String.Format( "Failed to connect to voice server" ) );
            }

            //wait until we get connected.
            int tries = 0;
            int maxTries = 20;
            bool isRunning = false;
            while (!isRunning && tries < maxTries)
            {
                Thread.Sleep(100);
                isRunning = m_voipclient.IsConnected();
            }
            if (isRunning)
            {
                logDebugOnly( "Connection to Conaito server succeeded\n" );
                // wait for OnConnectSuccess or OnConnectFailed event
                updateButtonStates();
                return true;
            }
            else
            {
                logDebugOnly( "Connection to Conaito server failed\n" );
                return false;
            }
        }

        private bool Disconnect()
        {
            if ( m_voipclient.IsConnected() || m_voipclient.IsConnecting() )
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

        public TreeNode GetUserNode( int nUserID )
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            foreach ( TreeNode node in channelsTreeView.Nodes )
                queue.Enqueue( node );

            while ( queue.Count > 0 )
            {
                TreeNode node = ( TreeNode )queue.Dequeue();
                if ( ( node.ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) ||
                    ( node.ImageIndex == ( int )ImageIndex.USER_TALKING ) )
                    if ( ( int )node.Tag == nUserID )
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
                if ( ( node.ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) ||
                    ( node.ImageIndex == ( int )ImageIndex.USER_TALKING ) )
                    return ( int )node.Tag;
            }

            return -1;	//user IDs are always positive
        }

        private bool initSoundSystem()
        {
            int iInputDeviceCount = m_voipclient.GetInputDeviceCount( kiSoundsystem );
            int iInputDeviceID = m_voipclient.GetInputDeviceID( kiSoundsystem, iInputDeviceCount - 1 );
            if ( m_voipclient.LaunchSoundSystem( kiSoundsystem, iInputDeviceID, kiOutputDeviceId, 32000, 5 ) )
//            if (m_voipclient.LaunchSoundSystem(1, -1, -1, 32000, 5))
            {
                logDebugOnly( "Sound system launched\n" );
                updateButtonStates();
                m_timer1.Start();
                return true;
            }
            else
            {
                logDebugOnly("Sound system failed to launched\n");
                return false;
            }
        }

        public void JoinedNewChannel( int nChannelID )
        {
            TreeNode node = GetChannelNode( m_iCurChannelID );
            if ( node != null )
            {
                TreeNodeCollection nodes = node.Nodes;
                for ( int i = 0; i < nodes.Count; )
                    if ( ( nodes[i].ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) ||
                        ( nodes[i].ImageIndex == ( int )ImageIndex.USER_TALKING ) )
                        nodes.RemoveAt( i );
                    else
                        i++;
            }
            m_iCurChannelID = nChannelID;
        }

        public void LeftChannel()
        {
            TreeNode node = GetChannelNode( m_iCurChannelID );
            if ( node != null )
            {
                TreeNodeCollection nodes = node.Nodes;
                for ( int i = 0; i < nodes.Count; )
                    if ( ( nodes[i].ImageIndex == ( int )ImageIndex.USER_NOT_TALKING ) ||
                        ( nodes[i].ImageIndex == ( int )ImageIndex.USER_TALKING ) )
                        nodes.RemoveAt( i );
                    else
                        i++;
            }
            m_iCurChannelID = -1;
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
                    AddChannel( 0 );
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
            updateButtonStates();
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

        void updateButtonStates()
        {
            joinButton.Enabled = m_bLoggedIn;
            joinButton.Text = m_voipclient.GetMyChannelID() > 0 ? "Leave" : "Join";
        }

        public void AddUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                TreeNode node = GetChannelNode( m_voipclient.GetUserChannelID( nUserID ) );
                if ( node != null )
                {
                    int iImageIndex = ( int )ImageIndex.USER_NOT_TALKING;
                    TreeNode newnode = new TreeNode( m_voipclient.GetUserNickName( nUserID ), iImageIndex, iImageIndex, new TreeNode[0] );
                    newnode.Tag = nUserID;
                    node.Nodes.Add( newnode );
                    node.Expand();
                }
            }
        }

        public void UpdateUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                TreeNode node = GetUserNode( nUserID );
                if ( node != null )
                    node.Text = m_voipclient.GetUserNickName( nUserID );
            }
        }

        private void UpdateUserIcon( int nUserID, bool bTalking )
        {
            if ( m_iUserId == nUserID )
            {
                TreeNode node = GetUserNode( nUserID );
                if ( node != null )
                {
                    // Only update if you've got the wrong icon
                    if ( ( bTalking && node.ImageIndex == ( int )ImageIndex.USER_NOT_TALKING )
                      || ( !bTalking && node.ImageIndex == ( int )ImageIndex.USER_TALKING ) )
                    {
                        TreeNode channelNode = node.Parent;
                        if ( channelNode != null )
                        {
                            int iImageIndex = node.ImageIndex == ( int )ImageIndex.USER_TALKING ? ( int )ImageIndex.USER_NOT_TALKING : ( int )ImageIndex.USER_TALKING;
                            channelNode.Nodes.Remove( node );
                            TreeNode newnode = new TreeNode( m_voipclient.GetUserNickName( nUserID ), iImageIndex, iImageIndex, new TreeNode[0] );
                            newnode.Tag = nUserID;
                            channelNode.Nodes.Add( newnode );
                        }
                    }
                }
            }
        }

        public void RemoveUser( int nUserID, int nChannelID )
        {
            if ( m_iUserId == nUserID )
            {
                TreeNode node = GetUserNode( nUserID );
                if ( node != null )
                    node.Parent.Nodes.Remove( node );
            }
        }

        #endregion Helper Functions

        // ----------------------------------------------------------------------------

        #region Forms Event Handlers

        private void VoIPClientControl_Load( object sender, EventArgs e )
        {
            bool soundInited = initSoundSystem();
            if ( soundInited && Connect() )
            {
                // Register push-to-talk key
                bHotKeyRegistered = m_voipclient.RegisterHotKey(
                    kiPUSH_TO_TALK_ID,
                    false,  // control
                    false,  // alt
                    false,  // shift
                    false,  // windows key
                    kcPushToTalkKey);

                this.m_timer1.Tick += new System.EventHandler( this.timer1_Tick );

                // Give it a little time or you won't get the nodes when you log in
                // I found it works at 1/2 second but not less  -- shorvitz 3/10/08
                System.Threading.Thread.Sleep( 500 );
                login();
            }
        }

        private void timer1_Tick( object sender, System.EventArgs e )
        {
            if ( m_voipclient.IsSoundSystemInitialized() )
            {
                int nValue = m_voipclient.GetCurrentInputLevel();
                vumeterProgressBar.Value = nValue > 20 ? 20 : nValue;
            }
        }


        private void channelsTreeView_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            if ( ( e.Node.ImageIndex == ( int )ImageIndex.CHANNEL ) && ( m_iCurChannelID == -1 ) )
            {
                if ( m_iDDDChannelID == ( int )e.Node.Tag )
                {
                    joinButton.Enabled = false;
                    joinchannelTextBox.Text = "";
                }
                else
                {
                    joinButton.Enabled = true;
                    joinchannelTextBox.Text = m_voipclient.GetChannelName( ( int )e.Node.Tag );
                }
            }
        }

        private void joinButton_Click( object sender, EventArgs e )
        {
            if (( -1 == m_iCurChannelID )&& ( joinchannelTextBox.Text != null ))
            {
                int iChannelId = _aChannels[joinchannelTextBox.Text];
                sendDDDRequestJoinVoiceChannelEvent( joinchannelTextBox.Text, iChannelId );
            }
            else
            {
                sendDDDRequestLeaveVoiceChannelEvent();
            }
        }

        private void mastervolTrackBar_ValueChanged( object sender, EventArgs e )
        {
            m_voipclient.SetMasterVolume(mastervolTrackBar.Value);
        }

        private void voiceactTrackBar_ValueChanged( object sender, EventArgs e )
        {
            m_voipclient.SetVoiceActivationLevel( voiceactTrackBar.Value );
        }

        private void pushToTalkRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (pushToTalkRadio.Checked)
            {
                // Register push-to-talk key
                bHotKeyRegistered = m_voipclient.RegisterHotKey(
                    kiPUSH_TO_TALK_ID,
                    false,  // control
                    false,  // alt
                    false,  // shift
                    false,  // windows key
                    kcPushToTalkKey);
                richTextBoxLog.AppendText(String.Format("Push To Talk activated.\n"));
            }
            else
            {
                // UnRegister push-to-talk key
                m_voipclient.UnregisterHotKey(kiPUSH_TO_TALK_ID);
                bHotKeyRegistered = false;
            }
        }

        private void voiceActRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            m_voipclient.SetVoiceActivated(voiceActRadio.Checked);
            if (voiceActRadio.Checked)
            {
                richTextBoxLog.AppendText(String.Format("Voice activated talking activated\n"));
            }

        }

        private void closeButton_Click( object sender, EventArgs e )
        {
            if ( m_iCurChannelID != -1 )
            {
                sendDDDRequestLeaveVoiceChannelEvent();
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
                updateButtonStates();
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

        private void sendDDDRequestLeaveVoiceChannelEvent()
        {
            //"RemoveFromVoiceChannel"
            String strChannelName = m_voipclient.GetChannelName( m_iCurChannelID );
            if ( null == strChannelName )
            {
                foreach ( string strKey in _aChannels.Keys )
                {
                    if ( m_iCurChannelID == _aChannels[strKey] )
                    {
                        strChannelName = strKey;
                        break;
                    }
                }
            }
            if ( null != strChannelName )
            {
                richTextBoxLog.AppendText( String.Format( "DDD Request Leave Voice Channel {0} Event\n", strChannelName ) );
                m_eventCommunicator.sendRequestLeaveVoiceChannelEvent( strChannelName );
            }
        }

        private void sendDDDPushToTalkEvent()
        {
            //"PushToTalk"
            richTextBoxLog.AppendText( "DDD Push to Talk Event\n" );
            m_eventCommunicator.sendPushToTalkVoiceChannelEvent(
                m_voipclient.GetChannelName( m_iCurChannelID ) );
        }

        private void sendDDDStoppedTalkingEvent()
        {
            //"StoppedTalking"
            richTextBoxLog.AppendText( "DDD Stopped Talking Event\n" );
            m_eventCommunicator.sendStoppedTalkingVoiceChannelEvent(
                m_voipclient.GetChannelName( m_iCurChannelID ) );
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
                m_voipclient.DoJoinChannel( m_voipclient.GetChannelPath( iChannelId ), strChannelPassword, strTopic, strOpPassword );
            }
            else
            {
                throw new Exception( String.Format( "{1} is not authorized to join {1}", _strDM, strChannelName ) );
            }
        }

        public void notifyLeaveVoiceChannel( string strChannelName )
        {
            m_voipclient.DoLeaveChannel();
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

    }
}