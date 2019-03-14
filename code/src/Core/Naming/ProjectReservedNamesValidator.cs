// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Microsoft.Templates.Core
{
    public class ProjectReservedNamesValidator : Validator
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
                    ErrorType = ValidationErrorType.ProjectReservedName,
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
