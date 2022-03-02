// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Naming
{
    public class DefaultNamesValidator : Validator
    {
        private static readonly Lazy<string[]> _defaultNames = new Lazy<string[]>(() => GetDefaultNames());

        public static string[] DefaultNames => _defaultNames.Value;

        private static string[] GetDefaultNames()
        {
            return GenContext.ToolBox.Repo.Get(t => !t.GetItemNameEditable()).Select(n => n.GetDefaultName()).ToArray();
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            if (DefaultNames.Contains(suggestedName, StringComparer.OrdinalIgnoreCase))
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.ReservedName,
                    ValidatorName = nameof(DefaultNamesValidator),
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
