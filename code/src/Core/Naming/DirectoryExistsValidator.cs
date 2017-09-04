// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

using Microsoft.Templates.Core;

namespace Microsoft.Templates.Core
{
    public class DirectoryExistsValidator : Validator<string>
    {
        public DirectoryExistsValidator(string config) : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var hasExistingItem = Directory.Exists(_config);

            if (hasExistingItem)
            {
                var existing = Directory.EnumerateDirectories(_config)
                                            .Select(d => new DirectoryInfo(d).Name)
                                            .ToList();

                hasExistingItem = existing.Contains(suggestedName);
            }

            if (hasExistingItem)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
                };
            }
            else
            {
                return new ValidationResult()
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None
                };
            }
        }
    }
}
