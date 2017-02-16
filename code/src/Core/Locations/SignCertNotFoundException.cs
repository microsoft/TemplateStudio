using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Locations
{
    [Serializable]
    public class SignCertNotFoundException : Exception
    {
        public SignCertNotFoundException()
        {
        }

        public SignCertNotFoundException(string message) : base(message)
        {
        }

        public SignCertNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SignCertNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}