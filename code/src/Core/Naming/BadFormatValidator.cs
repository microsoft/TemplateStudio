// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core
{
    public class BadFormatValidator : Validator
    {
        private const string ValidationPattern = @"^((?!\d)\w+)$";

        public override ValidationResult Validate(string suggestedName)
        {
            var m = Regex.Match(suggestedName, ValidationPattern);
            if (!m.Success)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.BadFormat,
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
