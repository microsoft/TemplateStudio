using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core
{
    [Serializable]
    public class RepositorySynchronizationException : Exception
    {
        public RepositorySynchronizationException()
        {
        }

        public RepositorySynchronizationException(string message) : base(message)
        {
        }

        public RepositorySynchronizationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RepositorySynchronizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}