using System;
using System.Runtime.Serialization;

namespace challenge.Exceptions
{
    public class EffectiveDateCouldNotBeParsedException : Exception
    {
        public EffectiveDateCouldNotBeParsedException()
        {
        }

        public EffectiveDateCouldNotBeParsedException(string message) : base(message)
        {
        }

        public EffectiveDateCouldNotBeParsedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EffectiveDateCouldNotBeParsedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}