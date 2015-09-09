using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.VoIPClientApp
{
    public partial class Form1 : Form
    {
        #region Attributes
        //If you get an error here's is because the COM control is not registered.
        //Use "regsvr32.exe EvoVoIP.dll" to register the COM control.
        //Check References in the Solution Explorer and add the COM control named
        //"EvoVoIP 2.1 Type Library"
        private ConaitoLib.EvoVoIP voipclient;

        private System.Windows.Forms.Timer timer1;
        private bool loggedin = false;
        private bool bHotKeyRegistered = false;
        private int curChannelID;

        private enum ImageIndex
        {
            USER_TALKING = 0,
            USER_NOT_TALKING = 1,
            CHANNEL = 2
        };

        // We'll want to set these in a configuration file;
        // for now, they're hard-coded
        private int kiSoundsystem = 2;
        private int kiInputDeviceId = 4;
        private int kiOutputDeviceId = 6;

        // We'll want to get these from the DDD server eventually
        // but for developing this, let's hard-code it at first
        const string kstrUserName = "myUser";
        const string kstrConaitoServerPasswd = "VoIP";
        const string kstrHost = "localhost";
        const int kiTCPPort = 10300;
        const int kiUDPPort = 10301;

        private const int PUSH_TO_TALK_ID = 1;

        #endregion Attributes

        #region ctor
        public Form1()
        {
            InitializeComponent();

            // Instantiate the client control
            voipclient = new ConaitoLib.EvoVoIPClass();
            if ( voipclient.InitVoIP( true ) )
            {
                textBoxLog.Text += "Initialized VoIP Evo voice client\n";
            }
            else
            {
                textBoxLog.Text += "Failed to initialize VoIP Evo voice client\n";
            }

            //connected successfully to server. Result of voipclient.Connect(..)
            voipclient.OnConnectSuccess += new ConaitoLib.IEvoVoIPEvents_OnConnectSuccessEventHandler( this.voipclient_OnConnectSuccess );
            //failed to connect to server. Result of voipclient.Connect(..)
            voipclient.OnConnectFailed += new ConaitoLib.IEvoVoIPEvents_OnConnectFailedEventHandler( this.voipclient_OnConnectFailed );
            //server dropped connection
            voipclient.OnConnectionLost += new ConaitoLib.IEvoVoIPEvents_OnConnectionLostEventHandler( this.voipclient_OnConnectionLost );
            //server accepted login. Result of voipclient.DoLogin(..)
            voipclient.OnAccepted += new ConaitoLib.IEvoVoIPEvents_OnAcceptedEventHandler( this.voipclient_OnAccepted );
            //logged out of server. Result of voipclient.DoLogout()
            //            voipclient.OnLoggedOut += new ConaitoLib.IEvoVoIPEvents_OnLoggedOutEventHandler( this.voipclient_OnLoggedOut );
            //a new user entered your channel
            voipclient.OnAddUser += new ConaitoLib.IEvoVoIPEvents_OnAddUserEventHandler( this.voipclient_OnAddUser );
            //a user in your channel updated his information, e.g. nickname, status, etc.
            voipclient.OnUpdateUser += new ConaitoLib.IEvoVoIPEvents_OnUpdateUserEventHandler( this.voipclient_OnUpdateUser );
            //a user left your channel
            voipclient.OnRemoveUser += new ConaitoLib.IEvoVoIPEvents_OnRemoveUserEventHandler( this.voipclient_OnRemoveUser );
            //a user in your channel is talking
            voipclient.OnUserTalking += new ConaitoLib.IEvoVoIPEvents_OnUserTalkingEventHandler( this.voipclient_OnUserTalking );
            //a user in your channel stopped talking
            voipclient.OnUserStoppedTalking += new ConaitoLib.IEvoVoIPEvents_OnUserStoppedTalkingEventHandler( this.voipclient_OnUserStoppedTalking );
            //a new channel was created on the server
            voipclient.OnAddChannel += new ConaitoLib.IEvoVoIPEvents_OnAddChannelEventHandler( this.voipclient_OnAddChannel );
            //a channel updated its information, e.g. its number of users
            voipclient.OnUpdateChannel += new ConaitoLib.IEvoVoIPEvents_OnUpdateChannelEventHandler( this.voipclient_OnUpdateChannel );
            //a channel was deleted from the server
            voipclient.OnRemoveChannel += new ConaitoLib.IEvoVoIPEvents_OnRemoveChannelEventHandler( this.voipclient_OnRemoveChannel );
            //you have joined a new channel. Result of voipclient.DoJoinChannel
            voipclient.OnJoinedChannel += new ConaitoLib.IEvoVoIPEvents_OnJoinedChannelEventHandler( this.voipclient_OnJoinedChannel );
            //you have left the channel. Result of voipclient.DoLeaveChannel
            voipclient.OnLeftChannel += new ConaitoLib.IEvoVoIPEvents_OnLeftChannelEventHandler( this.voipclient_OnLeftChannel );
            //a hotkey has become active
            voipclient.OnHotKeyToggle += new ConaitoLib.IEvoVoIPEvents_OnHotKeyToggleEventHandler( this.voipclient_OnHotKeyToggle );
            //notification containing the raw audio (PCM data) which was played when a user was talking.
            voipclient.OnUserAudioData += new ConaitoLib.IEvoVoIPEvents_OnUserAudioDataEventHandler( this.voipclient_OnUserAudioData );

            this.timer1 = new System.Windows.Forms.Timer( this.components );

            //Current mike input level
            vumeterProgressBar.Maximum = 20;
            vumeterProgressBar.Minimum = 0;

            this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
        }
        #endregion ctor

        // ----------------------------------------------------------------------------

        #region Conaito Event Handlers

        private void voipclient_OnAccepted( int nUserID )
        {
            textBoxLog.Text += "Connected to Conaito server\n";
            loggedin = true;
            updateButtonStates();
        }

        private void voipclient_OnConnectSuccess()
        {
            updateButtonStates();
        }

        private void voipclient_OnConnectFailed()
        {
            //call disconnect to clear out any state information
            voipclient.Disconnect();
            textBoxLog.Text += "Failed to connect to Conaito server\n";
            updateButtonStates();
        }

        private void voipclient_OnConnectionLost()
        {
            //call disconnect to clear out any state information
            voipclient.Disconnect();
            textBoxLog.Text += "Connection dropped!\n";
            updateButtonStates();
        }

        private void voipclient_OnAddChannel( int nChannelID )
        {
            //new channel has been created
            AddChannel( nChannelID );
        }

        private void voipclient_OnJoinedChannel( int nChannelID )
        {
            // you have joined a channel
            TreeNode node = GetChannelNode( curChannelID );
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

            curChannelID = nChannelID;

            //            filesListView.Items.Clear();
            updateButtonStates();
        }

        private void voipclient_OnLeftChannel( int nChannelID )
        {
            //            filesListView.Items.Clear();
            TreeNode node = GetChannelNode( curChannelID );
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
            updateButtonStates();
        }

        private void voipclient_OnUpdateChannel( int nChannelID )
        {
            //channel has been updated
            UpdateChannel( nChannelID );
        }

        private void voipclient_OnRemoveChannel( int nChannelID )
        {
            //a channel has been removed
            RemoveChannel( nChannelID );
        }

        private void voipclient_OnAddUser( int nUserID, int nChannelID )
        {
            //new user has arrived in channel
            AddUser( nUserID, nChannelID );
        }

        private void voipclient_OnUpdateUser( int nUserID, int nChannelID )
        {
            //user has been updated
            UpdateUser( nUserID, nChannelID );
        }

        private void voipclient_OnRemoveUser( int nUserID, int nChannelID )
        {
            //user has been removed from channel
            RemoveUser( nUserID, nChannelID );
        }

        private void voipclient_OnHotKeyToggle( int nHotKeyID, bool bActive )
        {
            //a registered hotkey is active
            if ( nHotKeyID == PUSH_TO_TALK_ID )
            {
                if ( bActive )
                {
                    voipclient.StartTransmitting();
                    UpdateUserIcon( voipclient.GetMyUserID(), true );
                }
                else
                {
                    voipclient.StopTransmitting();
                    UpdateUserIcon( voipclient.GetMyUserID(), false );
                }
            }
        }

        private void voipclient_OnUserTalking( int nUserID )
        {
            //switch to talking icon
            UpdateUserIcon( nUserID, true );
        }

        private void voipclient_OnUserStoppedTalking( int nUserID )
        {
            //switch to non-talking icon
            UpdateUserIcon( nUserID, false );
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
            string path = voipclient.GetChannelPath( nChannelID );
            if ( null == path )
            {
                path = "/";
            }
            string[] tokens = TokenizeChannelPath( path );
            int i = 0;
            path = "/";
            while ( i < tokens.Length - 1 )//strip the new channel
                path += tokens[i++] + "/";

            if ( tokens.Length == 0 )//add root channel
            {
                channelsTreeView.Nodes.Clear();
                //note that the root channel does not have a name
                TreeNode node = new TreeNode( voipclient.GetServerName(), ( int )ImageIndex.CHANNEL, ( int )ImageIndex.CHANNEL, new TreeNode[0] );
                node.Tag = nChannelID;
                channelsTreeView.Nodes.Add( node );
            }
            else
            {
                //add new sub channel
                TreeNode node = new TreeNode( tokens[tokens.Length - 1], ( int )ImageIndex.CHANNEL, ( int )ImageIndex.CHANNEL, new TreeNode[0] );
                node.Tag = nChannelID;
                TreeNode parent = GetChannelNode( voipclient.GetChannelParentID( nChannelID ) );

                parent.Nodes.Add( node );
            }
        }

        public void UpdateChannel( int nChannelID )
        {
        }

        public void RemoveChannel( int nChannelID )
        {
            TreeNode node = GetChannelNode( nChannelID );
            if ( node != null )
                node.Parent.Nodes.Remove( node );
        }

        private bool Connect()
        {
            if ( voipclient.Connect( kstrHost, kiTCPPort, kiUDPPort, 0, 0 ) )
            {
                // wait for OnConnectSuccess or OnConnectFailed event
                updateButtonStates();
                return true;
            }
            else
            {
                this.textBoxLog.Text += "Failed to connect to Conaito server\n";
                return false;
            }
        }

        private bool Disconnect()
        {
            if ( voipclient.IsConnected() || voipclient.IsConnecting() )
                return voipclient.Disconnect();
            else
                return false;
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
            Queue queue = new Queue();
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

        public int GetSelectedChannel()
        {
            TreeNode node = channelsTreeView.SelectedNode;
            if ( node != null )
            {
                if ( node.ImageIndex == ( int )ImageIndex.CHANNEL )
                    return ( int )node.Tag;
            }

            return -1;	//user IDs are always positive
        }

        private bool initSoundSystem()
        {
            if ( voipclient.LaunchSoundSystem( kiSoundsystem, kiInputDeviceId, kiOutputDeviceId, 32000, 5 ) )
            {
                textBoxLog.Text += "Sound system launched\n";
                updateButtonStates();
                timer1.Start();
                return true;
            }
            else
            {
                textBoxLog.Text += "Failed to initialize sound system.\n";
                return false;
            }
        }

        public void JoinedNewChannel( int nChannelID )
        {
            TreeNode node = GetChannelNode( curChannelID );
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
            curChannelID = nChannelID;
        }

        private bool login()
        {
            bool bStatus = true;
            if ( loggedin )
            {
                textBoxLog.Text += "Logged into Conaito server\n";
            }
            else
            {
                if ( voipclient.DoLogin( kstrUserName, kstrConaitoServerPasswd ) )
                {
                    loggedin = true;
                    textBoxLog.Text += "Logged into Conaito server\n";
                    // Get the root channel
                    AddChannel( 0 );
                }
                else
                {
                    textBoxLog.Text += "Could not log into Conaito server\n";
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
            joinButton.Enabled = loggedin;
            joinButton.Text = voipclient.GetMyChannelID() > 0 ? "Leave" : "Join";
        }

        public void AddUser( int nUserID, int nChannelID )
        {
            TreeNode node = GetChannelNode( voipclient.GetUserChannelID( nUserID ) );
            if ( node != null )
            {
                int iImageIndex = voiceactCheckBox.Checked ? ( int )ImageIndex.USER_TALKING : ( int )ImageIndex.USER_NOT_TALKING;
                TreeNode newnode = new TreeNode( voipclient.GetUserNickName( nUserID ), iImageIndex, iImageIndex, new TreeNode[0] );
                newnode.Tag = nUserID;
                node.Nodes.Add( newnode );
            }
        }

        public void UpdateUser( int nUserID, int nChannelID )
        {
            TreeNode node = GetUserNode( nUserID );
            if ( node != null )
                node.Text = voipclient.GetUserNickName( nUserID );
        }

        private void UpdateUserIcon( int nUserID, bool bTalking )
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
                        TreeNode newnode = new TreeNode( voipclient.GetUserNickName( nUserID ), iImageIndex, iImageIndex, new TreeNode[0] );
                        newnode.Tag = nUserID;
                        channelNode.Nodes.Add( newnode );
                    }
                }
            }
        }

        public void RemoveUser( int nUserID, int nChannelID )
        {
            TreeNode node = GetUserNode( nUserID );
            if ( node != null )
                node.Parent.Nodes.Remove( node );
        }

        #endregion Helper Functions

        // ----------------------------------------------------------------------------

        #region Forms Event Handlers

        private void Form1_Load( object sender, EventArgs e )
        {
            if ( initSoundSystem() && Connect() )
            {
                // Give it a little time or you won't get the nodes when you log in
                // I found it works at 1/2 second but not less  -- shorvitz 3/10/08
                System.Threading.Thread.Sleep( 500 );
                login();
            }
        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e )
        {
            if ( this.loggedin )
            {
                voipclient.DoLogout();
            }

            if ( voipclient.IsConnected() )
            {
                voipclient.Disconnect();
            }

            if ( voipclient.ShutdownSoundSystem() )
            {
                updateButtonStates();
                timer1.Stop();
            }
            voipclient.CloseVoIP();
        }

        private void timer1_Tick( object sender, System.EventArgs e )
        {
            if ( voipclient.IsSoundSystemInitialized() )
            {
                int nValue = voipclient.GetCurrentInputLevel();
                vumeterProgressBar.Value = nValue > 20 ? 20 : nValue;
            }
        }

        private void mastervolTrackBar_ValueChanged( object sender, EventArgs e )
        {
            voipclient.SetMasterVolume( mastervolTrackBar.Value );
        }

        private void joinButton_Click( object sender, EventArgs e )
        {
            if ( voipclient.GetMyChannelID() <= 0 )
            {
                if ( voipclient.DoJoinChannel( joinchannelTextBox.Text, "", "", "" ) )
                {
                    this.textBoxLog.Text += "Joined channel " + joinchannelTextBox.Text + "\n";
                }
                else
                {
                    this.textBoxLog.Text += "Failed to join channel " + joinchannelTextBox.Text + "\n";
                }
            }
            else
            {
                if ( voipclient.DoLeaveChannel() )
                {
                    this.textBoxLog.Text += "Left channel " + joinchannelTextBox.Text + "\n";
                }
                else
                {
                    this.textBoxLog.Text += "Failed to leave channel " + joinchannelTextBox.Text + "\n";
                }
            }
        }

        private void channelsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
        {
/*
            int id = GetSelectedUser();
            if ( id != -1 )
            {
                uservolTrackBar.Value = voipclient.GetUserVolume( id );
                muteuserCheckBox.Checked = voipclient.GetUserMute( id );
                if ( voipclient.GetUserStatusMode( id ) == 0 )
                    label11.Text = "User is available";
                else if ( voipclient.GetUserStatusMode( id ) == 1 )
                    label11.Text = "User is away";

                textBox7.Text = voipclient.GetUserStatusMsg( id );

                float x = 0.0f, y = 0.0f, z = 0.0f;
                voipclient.GetUserPosition( id, ref x, ref y, ref z );
                posxTrackBar.Value = ( int )( x * 10.0f );
                posyTrackBar.Value = ( int )( y * 10.0f );
                poszTrackBar.Value = ( int )( z * 10.0f );
            }
            else
            {
                uservolTrackBar.Value = 0;
                posxTrackBar.Value = posxTrackBar.Minimum;
                posyTrackBar.Value = posxTrackBar.Minimum;
                poszTrackBar.Value = posxTrackBar.Minimum;

                muteuserCheckBox.Checked = false;
                label11.Text = "";
                textBox7.Text = "";
            }
*/
            int chanid = GetSelectedChannel();
            if ( chanid > 0 )
            {
                joinchannelTextBox.Text = voipclient.GetChannelPath( chanid );
            }
        }

        private void registerButton_Click( object sender, EventArgs e )
        {
            if ( bHotKeyRegistered )
            {
                bHotKeyRegistered = !voipclient.UnregisterHotKey( PUSH_TO_TALK_ID );
                registerButton.Text = "Register hotkey";
                ctrlCheckBox.Checked = false;
                altCheckBox.Checked = false;
                shiftCheckBox.Checked = false;
                winCheckBox.Checked = false;
                vkTextBox.Text = "";
            }
            else
            {
                if ( vkTextBox.Text.Length == 1 )
                {
                    voipclient.UnregisterHotKey( PUSH_TO_TALK_ID );
//                    int iASCII = Convert.ToInt32( vkTextBox.Text.ToCharArray().GetValue(0) );
                    char cKey = (char)vkTextBox.Text.ToCharArray().GetValue(0);
                    bHotKeyRegistered = voipclient.RegisterHotKey(
                        PUSH_TO_TALK_ID,
                        ctrlCheckBox.Checked,
                        altCheckBox.Checked,
                        shiftCheckBox.Checked,
                        winCheckBox.Checked,
                        //                        0 );
                        //                        iASCII );
                        cKey );
                    registerButton.Text = "Clear hotkey";
                }
                else
                {
                    MessageBox.Show( "Please enter a virtual key" );
                }
            }
        }

        private void voiceactCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            voipclient.SetVoiceActivated( voiceactCheckBox.Checked );

            // Can't get rid of the talking icon otherwise
            if ( !voiceactCheckBox.Checked )
            {
                TreeNode channelNode = GetChannelNode( curChannelID );
                if ( channelNode != null )
                {
                    for ( int i = channelNode.Nodes.Count - 1; i >= 0; i-- )
                    {
                        TreeNode node = channelNode.Nodes[i];

                        // Changing the ImageIndex or the SelectedImageIndex and
                        // invalidating is not doing the trick.  Just remove the
                        // node and remake it.
                        if ( node.ImageIndex == (int)ImageIndex.USER_TALKING )
                        {
                            int nUserID = (int)node.Tag;
                            channelNode.Nodes.Remove( node );
                            TreeNode newnode = new TreeNode( voipclient.GetUserNickName( nUserID ), ( int )ImageIndex.USER_NOT_TALKING, ( int )ImageIndex.USER_NOT_TALKING, new TreeNode[0] );
                            newnode.Tag = nUserID;
                            channelNode.Nodes.Add( newnode );
                        }
                    }
                }
            }
        }

        private void voiceactTrackBar_ValueChanged( object sender, EventArgs e )
        {
            voipclient.SetVoiceActivationLevel( voiceactTrackBar.Value );
        }

        private void closeButton_Click( object sender, EventArgs e )
        {
            this.Close();
        }
        #endregion Forms Event Handlers
    }
}