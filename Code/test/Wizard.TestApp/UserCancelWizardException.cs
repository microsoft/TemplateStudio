using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Wizard.TestApp
{
    [Serializable]
    internal class UserCancelWizardException : Exception
    {
        public UserCancelWizardException()
        {
        }

        public UserCancelWizardException(string message) : base(message)
        {
        }

        public UserCancelWizardException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserCancelWizardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}