using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Locations
{
    [Serializable]
    public class InvalidSignatureException : Exception
    {
        public InvalidSignatureException()
        {
        }

        public InvalidSignatureException(string message) : base(message)
        {
        }

        public InvalidSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSignatureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}