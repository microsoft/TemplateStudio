// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core
{
    public class PageSuffixValidator : Validator
    {
        private const string PageSuffix = "page";

        public override ValidationResult Validate(string suggestedName)
        {
            if (suggestedName.EndsWith(PageSuffix, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.EndsWithPageSuffix,
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
