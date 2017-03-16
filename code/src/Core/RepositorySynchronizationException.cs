using Microsoft.Templates.Core.Locations;
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

        public RepositorySynchronizationException(string message, Exception innerException = null) : base(message, innerException)
        {
        }

        protected RepositorySynchronizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RepositorySynchronizationException(SyncStatus status, Exception innerException = null) : base($"Error syncing templates. Status: '{status}'", innerException)
        {
        }
    }
}