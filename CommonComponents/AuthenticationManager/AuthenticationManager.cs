using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.UserTools;

namespace Aptima.Asim.DDD.CommonComponents.AuthenticationManager
{
    public class AuthenticationManager
    {
        private class LoginInfo
        {
            public string userName;
            public string password;

            public LoginInfo(string userName, string password)
            {
                this.userName = userName;
                this.password = password;
            }
        }

        private static SimulationEventDistributorClient server;
        private static SimulationModelReader smr = new SimulationModelReader();
        private static SimulationModelInfo simModelInfo;
        private static string simModelName;
        private static int maxNumberOfUsers;
        private static int currentlyLoggedInUsers;

        private Dictionary<string, LoginInfo> terminalIdMapping;

        public AuthenticationManager(ref SimulationEventDistributorClient netServ, ref SimulationModelInfo simModel, int numberfOfSeats)
        {
            simModelInfo = simModel;
            server = netServ;
            currentlyLoggedInUsers = 0;
            maxNumberOfUsers = numberfOfSeats;
            terminalIdMapping = new Dictionary<string, LoginInfo>();
        }

        public void ResetAuthenticationManager()
        { 
            terminalIdMapping = new Dictionary<string, LoginInfo>();
            currentlyLoggedInUsers = 0;
        }

        public void AuthenticationRequest(SimulationEvent e)
        {
            Authenticator.LoadUserFile();
            string terminalID = ((StringValue)e["TerminalID"]).value;
            
            if (currentlyLoggedInUsers == maxNumberOfUsers)
            {
                SendAuthenticationResponse(terminalID, "The server has reached its limit for number of users attached to the DDD.", false);
                return;
            }

            string username = ((StringValue)e["Username"]).value;
            string password = ((StringValue)e["Password"]).value;

            if (Authenticator.Authenticate(username, password))
            {
                SendAuthenticationResponse(terminalID, "Authentication successful!", true);
            }
            else
            {
                SendAuthenticationResponse(terminalID, "Invalid username and/or password", false);
            }
        }

        private void SendAuthenticationResponse(string termID, string message, bool success)
        {
            SimulationEvent e = SimulationEventFactory.BuildEvent(ref simModelInfo, "AuthenticationResponse");
            ((StringValue)e["TerminalID"]).value = termID;
            ((StringValue)e["Message"]).value = message;
            ((BooleanValue)e["Success"]).value = success;

            server.PutEvent(e);
        }
        public void IncrementUsers()
        {
            currentlyLoggedInUsers += 1;
        }

        public void DecrementUsers()
        {
            currentlyLoggedInUsers -= 1;
        }

        public void ResetUserCount()
        {
            currentlyLoggedInUsers = 0;
        }

    }
}
