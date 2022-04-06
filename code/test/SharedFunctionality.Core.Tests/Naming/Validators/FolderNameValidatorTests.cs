// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Trait("Group", "Minimum")]
    public class FolderNameValidatorTests
    {
        [Fact]
        public void Validate_RecognizesExistingFolderAsInvalid()
        {
            var tempDir = Path.GetFullPath(@".\temp\");
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "temp\\testfolder"));

            var validator = new FolderNameValidator(tempDir);

            var result = validator.Validate("testfolder");

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal(nameof(FolderNameValidator), result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Fact]
        public void Validate_RecognizesNotExistingFolderAsValid()
        {
            var tempDir = Path.GetFullPath(@".\temp\");
            Directory.CreateDirectory(tempDir);

            var validator = new FileNameValidator(tempDir);

            var result = validator.Validate("newfolder");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
