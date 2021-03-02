using System;
using System.Runtime.Serialization;

namespace challenge.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException()
        {
        }

        public EmployeeNotFoundException(string message) : base(message)
        {
        }

        public EmployeeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmployeeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}