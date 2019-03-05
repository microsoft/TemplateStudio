// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public class ExistingNamesValidator : Validator<IEnumerable<string>>
    {
        public ExistingNamesValidator(IEnumerable<string> config)
            : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            if (Config.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists,
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                ErrorType = ValidationErrorType.None,
            };
        }
    }
}
