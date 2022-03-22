// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core.Naming
{
    public class FileNameValidator : Validator<string>
    {
        public FileNameValidator(string config)
            : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            var existing = Directory.EnumerateFiles(Config)
                                            .Select(f => Path.GetFileNameWithoutExtension(f))
                                            .ToList();

            if (existing.Contains(suggestedName, StringComparer.OrdinalIgnoreCase))
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.AlreadyExists,
                    ValidatorName = nameof(FileNameValidator),
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
