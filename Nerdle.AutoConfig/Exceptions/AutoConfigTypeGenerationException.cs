using System;
using System.Runtime.Serialization;

namespace Nerdle.AutoConfig.Exceptions
{
    [Serializable]
    public class AutoConfigTypeGenerationException : Exception
    {
        public AutoConfigTypeGenerationException(string message) 
            : base(message)
        {
        }

        public AutoConfigTypeGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AutoConfigTypeGenerationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
