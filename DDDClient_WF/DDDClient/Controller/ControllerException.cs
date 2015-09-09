using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client.Controller
{
    class ControllerException : ApplicationException
    {
        public ControllerException() { }
        public ControllerException(string message) : base(message) { }
        public ControllerException(string message, Exception inner_exception) : base(message, inner_exception) { }
        public ControllerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
