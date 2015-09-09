using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client
{
    /// <summary>
    /// This interface is for a DDD voice server event listener
    /// </summary>
    public interface IVoiceClientController
    {
        void NotifyCreatedVoiceChannel( string strChannelName, List<string> astrMembershipList );
        void NotifyClosedVoiceChannel( string strChannelName );
        void NotifyJoinVoiceChannel( string strChannelName );
        void NotifyLeaveVoiceChannel( string strChannelName );
        void NotifyStartedTalking(string strUsername, string strChannelName);
        void NotifyStoppedTalking(string strUsername, string strChannelName);
        void NotifyMuteUser(string strUsername, string strChannelName);
        void NotifyUnmuteUser(string strUsername, string strChannelName);
    }
}
