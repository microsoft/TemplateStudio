// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Naming
{
    public class EmptyNameValidator : Validator
    {
        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(suggestedName))
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.EmptyName,
                    ValidatorName = nameof(EmptyNameValidator),
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
