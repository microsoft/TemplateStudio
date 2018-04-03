// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Validators;

namespace Microsoft.Templates.UI.Services
{
    public static class BreakingChangesValidatorService
    {
        // add breaking changes validators
        private static List<IValidator> _validators = new List<IValidator>
        {
            new HamburgerMenuValidator()
        };

        public static ValidationResult Validate()
        {
            var validations = _validators.Select(v => v.Validate()).ToList();

            return new ValidationResult
            {
                IsValid = validations.All(v => v.IsValid),
                ErrorMessages = validations.SelectMany(v => v.ErrorMessages).ToList()
            };
        }
    }
}
