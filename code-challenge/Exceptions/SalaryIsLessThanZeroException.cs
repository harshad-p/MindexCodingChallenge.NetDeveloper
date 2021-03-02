using System;
using System.Runtime.Serialization;

namespace challenge.Exceptions
{
    public class SalaryIsLessThanZeroException : Exception
    {
        public SalaryIsLessThanZeroException()
        {
        }

        public SalaryIsLessThanZeroException(string message) : base(message)
        {
        }

        public SalaryIsLessThanZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SalaryIsLessThanZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}