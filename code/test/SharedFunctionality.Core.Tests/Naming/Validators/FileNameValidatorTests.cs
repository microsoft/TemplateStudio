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
    public class FileNameValidatorTests
    {
        [Fact]
        public void Validate_RecognizesExistingFileAsInvalid()
        {
            var tempDir = Path.GetFullPath(@".\temp\");
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "temp"));

            using (File.Create(Path.Combine(Environment.CurrentDirectory, "temp\\testfile.txt")))
            {
                var validator = new FileNameValidator(tempDir);

                var result = validator.Validate("testfile");

                Assert.False(result.IsValid);
                Assert.True(result.Errors.Count == 1);
                Assert.Equal(ValidationErrorType.AlreadyExists, result.Errors.FirstOrDefault()?.ErrorType);
                Assert.Equal(nameof(FileNameValidator), result.Errors.FirstOrDefault()?.ValidatorName);
            }

            Directory.Delete(tempDir, true);
        }

        [Fact]
        public void Validate_RecognizesNotExistingFileAsValid()
        {
            var tempDir = Path.GetFullPath(@".\temp\");

            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "temp"));

            var validator = new FileNameValidator(tempDir);

            var result = validator.Validate("newfile");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
