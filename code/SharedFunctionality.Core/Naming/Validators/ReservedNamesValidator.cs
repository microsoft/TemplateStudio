// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.Naming
{
    public class ReservedNamesValidator : Validator<string[]>
    {
        public ReservedNamesValidator(string[] config)
           : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            if (Config.Contains(suggestedName, StringComparer.OrdinalIgnoreCase))
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.ReservedName,
                    ValidatorName = nameof(ReservedNamesValidator),
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
