// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public class SuggestedDirectoryNameValidator : Validator<string>
    {
        // config should be the path of an existing folder
        public SuggestedDirectoryNameValidator(string config) : base(config)
        {
        }

        // Can a folder with the suggested name be created in the "config" folder
        public override ValidationResult Validate(string suggestedName)
        {
            // if the config directory doesn't exist then there's definitely not anything already in it with the suggested name
            var suggestedDirectoryExists = Directory.Exists(_config);

            if (suggestedDirectoryExists)
            {
                // Does a subdirectory with the suggested name already exist?
                var existingSubdirectories = Directory.EnumerateDirectories(_config)
                                                      .Select(d => new DirectoryInfo(d).Name)
                                                      .ToList();

                suggestedDirectoryExists = existingSubdirectories.Contains(suggestedName);
            }

            if (suggestedDirectoryExists)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
                };
            }
            else
            {
                return new ValidationResult
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None
                };
            }
        }
    }
}
