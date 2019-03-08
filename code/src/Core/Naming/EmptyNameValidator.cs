// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Microsoft.Templates.Core
{
    public class EmptyNameValidator : Validator
    {
        public override ValidationResult Validate(string suggestedName)
        {
            if (string.IsNullOrEmpty(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.Empty,
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
