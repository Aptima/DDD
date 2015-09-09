using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib
{
    public partial class VoIPClientDialog : Form
    {
        public VoIPClientDialog(
            IVoiceClientEventCommunicator eventCommunicator,
            String strDM,
            String strVoiceServerHostname, 
            int iVoiceServerPort, 
            String strConaitoServerPasswd )
        {
            InitializeComponent();
            voIPClientControl1.initialize( eventCommunicator, strDM, strVoiceServerHostname, iVoiceServerPort, strConaitoServerPasswd );
        }

        public VoIPClientDialog()
        {
            InitializeComponent();
        }

        #region DDD events the dialog is listening to
        public void notifyVoiceChannelCreated( string strChannelName, List<string> astrMembershipList )
        {
            voIPClientControl1.notifyVoiceChannelCreated( strChannelName, astrMembershipList );
        }

        public void notifyVoiceChannelClosed( string strChannelName )
        {
            voIPClientControl1.notifyVoiceChannelClosed( strChannelName );
        }

        public void notifyJoinVoiceChannel( string strChannelName )
        {
            voIPClientControl1.notifyJoinVoiceChannel( strChannelName );
        }

        public void notifyLeaveVoiceChannel( string strChannelName )
        {
            voIPClientControl1.notifyLeaveVoiceChannel( strChannelName );
        }
        #endregion DDD events the dialog is listening to

        private void voIPClientControl1_Load(object sender, EventArgs e)
        {

        }
    }
}