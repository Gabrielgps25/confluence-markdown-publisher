using System;
using System.Runtime.Serialization;

namespace ConfluencePublisher.Exceptions
{
    [Serializable]
    internal class InvalidParentPageException : Exception
    {
        public InvalidParentPageException()
        {
        }

        public InvalidParentPageException(string message) : base(message)
        {
        }

        public InvalidParentPageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidParentPageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}