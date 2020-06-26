using System;
using System.Runtime.Serialization;

namespace ConfluencePublisher.Exceptions
{
    [Serializable]
    internal class ConfluenceApiCallException : Exception
    {
        public ConfluenceApiCallException()
        {
        }

        public ConfluenceApiCallException(string message) : base(message)
        {
        }

        public ConfluenceApiCallException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfluenceApiCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}