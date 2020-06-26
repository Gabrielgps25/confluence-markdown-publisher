using System;
using System.Runtime.Serialization;

namespace ConfluencePublisher.Exceptions
{
    [Serializable]
    internal class InvalidJsonSchemaException : Exception
    {
        public InvalidJsonSchemaException()
        {
        }

        public InvalidJsonSchemaException(string message) : base(message)
        {
        }

        public InvalidJsonSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidJsonSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}