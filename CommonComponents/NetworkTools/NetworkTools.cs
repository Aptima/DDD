using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.CommonComponents.NetworkTools
{
    enum NetMessageType {NONE,
                      REGISTER,
                      REGISTER_RESPONSE,
                      SUBSCRIBE,
                      EVENT,
        DISCONNECT,
        PING
    };
    class NetMessage
    {
        public NetMessageType type;
        public Int32 clientID;
        private String m_terminalID;

        public String TerminalID
        {
            get { return m_terminalID; }
        }

        //private Int32 bytes;
        public string msg;
        //private Byte[] recBuffer;
        
        public NetMessage()
        {
            type = NetMessageType.NONE;
            m_terminalID = "";
            clientID = -1;
            //bytes = 0;
            msg = String.Empty;
            //recBuffer = new Byte[256];
        }

        private void SendType(ref NetworkStream stream)
        {
            //Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
            Byte t = (Byte) type;
            //Int32 i = 0;
            //sizeof(Int32);
            stream.WriteByte(t);
        }
        private void ReceiveType(ref NetworkStream stream)
        {
            Byte[] t = new Byte[1];
            stream.Read(t, 0, 1);
            type = (NetMessageType)t[0];
        }
        private void SendInt32(ref NetworkStream stream, Int32 i)
        {
            Byte[] data;
            data = BitConverter.GetBytes(i);
            stream.Write(data, 0, data.Length);

        }

        private Int32 ReceiveInt32(ref NetworkStream stream)
        {
            Int32 i = 0;
            Byte[] data = new Byte[sizeof(Int32)];
            Int32 msgSize = data.Length;

            Int32 receivedBytes = 0;
            Int32 chunk = 0;
            while (receivedBytes < msgSize)
            {
                chunk = stream.Read(data, receivedBytes, msgSize - receivedBytes);
                receivedBytes += chunk;
            }

            i = BitConverter.ToInt32(data, 0);
            return i;
        }

        private void SendString(ref NetworkStream stream, string s)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
            SendInt32(ref stream,data.Length);

            stream.Write(data, 0, data.Length);
        }

        private string ReceiveString(ref NetworkStream stream)
        {
            string s = string.Empty;
            Int32 bytes = ReceiveInt32(ref stream);
            Byte[] data = new Byte[bytes];


            Int32 receivedBytes = 0;
            Int32 chunk = 0;

            while (receivedBytes < bytes)
            {
                chunk = stream.Read(data, receivedBytes, bytes - receivedBytes);
                receivedBytes += chunk;
            }

            s = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            
            return s;
        }
        public void Send(ref NetworkStream stream,String terminalID)
        {


            SendType(ref stream);
            switch (type)
            {
                case NetMessageType.NONE:
                    break;
                case NetMessageType.PING:
                    break;
                case NetMessageType.REGISTER:
                    SendInt32(ref stream, clientID);
                    SendString(ref stream, terminalID);
                    break;
                case NetMessageType.REGISTER_RESPONSE:
                    SendInt32(ref stream, clientID);
                    break;
                case NetMessageType.SUBSCRIBE:
                    SendInt32(ref stream, clientID);
                    SendString(ref stream, msg);
                    break;
                case NetMessageType.EVENT:
                    SendInt32(ref stream, clientID);
                    SendString(ref stream, msg);
                    break;
            }

            stream.Flush();

        }

        public void Receive(ref NetworkStream stream)
        {

            ReceiveType(ref stream);
            switch (type)
            {
                case NetMessageType.NONE:
                    break;
                case NetMessageType.REGISTER:
                    clientID = ReceiveInt32(ref stream);
                    m_terminalID = ReceiveString(ref stream);
                    break;
                case NetMessageType.REGISTER_RESPONSE:
                    clientID = ReceiveInt32(ref stream);
                    break;
                case NetMessageType.SUBSCRIBE:
                    clientID = ReceiveInt32(ref stream);
                    msg = ReceiveString(ref stream);
                    break;
                case NetMessageType.EVENT:
                    clientID = ReceiveInt32(ref stream);
                    msg = ReceiveString(ref stream);
                    break;
            }

        }
    }    
}


