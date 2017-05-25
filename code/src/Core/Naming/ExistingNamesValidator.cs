using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class ExistingNamesValidator : Validator<IEnumerable<string>>
    {
        public ExistingNamesValidator(IEnumerable<string> config) : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
           
            if (_config.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
                };
            }
            else
            {
                return new ValidationResult()
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None
                };
            }
        }
    }
}
