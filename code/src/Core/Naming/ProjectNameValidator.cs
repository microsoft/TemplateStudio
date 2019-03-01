using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class ProjectNameValidator : Validator
    {
        private static readonly string[] ReservedNames = new string[]
        {
            "Prism",
            "CaliburnMicro",
            "MVVMLight",
        };

        public override ValidationResult Validate(string suggestedName)
        {
            if (ReservedNames.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.ReservedName,
                };
            }
            else
            {
                return new ValidationResult()
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None,
                };
            }
        }
    }
}
