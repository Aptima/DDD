using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    class CanvasException: ApplicationException
    {
        public CanvasException() {}
        public CanvasException(string message) : base(message) { }
        public CanvasException(string message, Exception inner_exception) : base(message, inner_exception) { }
        public CanvasException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        
    }
}
