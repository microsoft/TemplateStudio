// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core.Naming
{
    public class FolderNameValidator : Validator<string>
    {
        // config should be the path of an existing folder
        public FolderNameValidator(string config)
            : base(config)
        {
        }

        // Can a folder with the suggested name be created in the "config" folder
        public override ValidationResult Validate(string suggestedName)
        {
            var result = new ValidationResult();

            // if the config directory doesn't exist then there's definitely not anything already in it with the suggested name
            var suggestedDirectoryExists = Directory.Exists(Config);

            if (suggestedDirectoryExists)
            {
                // Does a subdirectory with the suggested name already exist?
                var existingSubdirectories = Directory.EnumerateDirectories(Config)
                                                      .Select(d => new DirectoryInfo(d).Name)
                                                      .ToList();

                suggestedDirectoryExists = existingSubdirectories.Contains(suggestedName, StringComparer.OrdinalIgnoreCase);
            }

            if (suggestedDirectoryExists)
            {
                var error = new ValidationError()
                {
                    ErrorType = ValidationErrorType.AlreadyExists,
                    ValidatorName = nameof(FolderNameValidator),
                };

                result.IsValid = false;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
