using System;
using System.Runtime.Serialization;

namespace Nerdle.AutoConfig.Exceptions
{
    [Serializable]
    public class AutoConfigMappingException : AutoConfigException
    {
        public AutoConfigMappingException(string message) 
            : base(message)
        {
        }

        public AutoConfigMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AutoConfigMappingException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}