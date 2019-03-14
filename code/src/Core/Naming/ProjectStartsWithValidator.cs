// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Microsoft.Templates.Core
{
    public class ProjectStartsWithValidator : Validator
    {
        private static readonly string[] StartsWith = new string[]
        {
            "$",
        };

        public override ValidationResult Validate(string suggestedName)
        {
            if (StartsWith.Any(r => suggestedName.StartsWith(r)))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.ProjectStartsWith,
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
