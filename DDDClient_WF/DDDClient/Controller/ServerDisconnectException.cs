using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client.Controller
{
    class ServerDisconnectException : ApplicationException
    {
        public ServerDisconnectException() { }
        public ServerDisconnectException(string message) : base(message) { }
        public ServerDisconnectException(string message, Exception inner_exception) : base(message, inner_exception) { }
        public ServerDisconnectException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
