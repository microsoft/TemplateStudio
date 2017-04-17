using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Composition
{
    [Serializable]
    public class InvalidCompositionQueryException : Exception
    {
        public InvalidCompositionQueryException()
        {
        }

        public InvalidCompositionQueryException(string message) : base(message)
        {
        }

        public InvalidCompositionQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCompositionQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}