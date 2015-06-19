using System;
using System.Runtime.Serialization;

namespace Nerdle.AutoConfig.Exceptions
{
    public abstract class AutoConfigException : Exception
    {
        protected AutoConfigException(string message) 
            : base(message)
        {
        }

        protected AutoConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AutoConfigException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}