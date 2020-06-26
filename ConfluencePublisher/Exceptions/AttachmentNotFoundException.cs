using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConfluencePublisher.Exceptions
{
    [Serializable]
    class AttachmentNotFoundException : Exception
    {
        public AttachmentNotFoundException()
        {
        }

        public AttachmentNotFoundException(string message) : base(message)
        {
        }

        public AttachmentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AttachmentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
