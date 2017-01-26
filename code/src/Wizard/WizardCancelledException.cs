using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Wizard
{
    [Serializable]
    internal class WizardCancelledException : Exception
    {
        public WizardCancelledException()
        {
        }

        public WizardCancelledException(string message) : base(message)
        {
        }

        public WizardCancelledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WizardCancelledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}