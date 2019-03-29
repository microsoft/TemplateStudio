// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public class FileExistsValidator : Validator<string>
    {
        public FileExistsValidator(string config)
            : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var existing = Directory.EnumerateFiles(Config)
                                            .Select(f => Path.GetFileNameWithoutExtension(f))
                                            .ToList();

            if (existing.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists,
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
