using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client
{
    /// <summary>
    /// This interface is to send events from the voice client control
    /// to the DDD server
    /// </summary>
    public interface IVoiceClientEventCommunicator
    {
        void sendRequestJoinVoiceChannelEvent( string strChannelName );
        void sendRequestLeaveVoiceChannelEvent( string strChannelName );
        void sendRequestStartedTalkingVoiceChannelEvent( string strChannelName );
        void sendRequestStoppedTalkingVoiceChannelEvent( string strChannelName );
        void sendRequestMuteUserEvent(string strChannelName);
        void sendRequestUnmuteUserEvent(string strChannelName);
    }
}
