// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.Naming
{
    public class RegExValidator : Validator<RegExConfig>
    {
        public RegExValidator(RegExConfig config)
           : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            var regex = new Regex(Config.Pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(suggestedName))
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.Regex,
                    ValidatorName = Config.Name,
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
