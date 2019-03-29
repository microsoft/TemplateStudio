// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class SuggestedDirectoryNameValidatorTests
    {
        [Fact]
        public void Anything_InANonExistentConfigDirectory_IsValid()
        {
            var nonExistentDirectory = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()); // Use new GUID as a name for a folder that won't already exist

            var sut = new SuggestedDirectoryNameValidator(nonExistentDirectory);

            var result = sut.Validate(Guid.NewGuid().ToString());

            Assert.True(result.IsValid);
        }

        [Fact]
        public void NewDirectory_InExistingConfigDirectory_IsValid()
        {
            var sut = new SuggestedDirectoryNameValidator(Environment.CurrentDirectory);
            var result = sut.Validate(Guid.NewGuid().ToString()); // Use new GUID as a name for a folder that won't already exist

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ExistingDirectory_InAnExistingConfigDirectory_IsNotValid()
        {
            // Create directory so can be sure it exists
            var existingDir = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()));

            try
            {
                var sut = new SuggestedDirectoryNameValidator(Environment.CurrentDirectory);

                var result = sut.Validate(existingDir.Name);

                Assert.False(result.IsValid);
            }
            finally
            {
                existingDir.Delete();
            }
        }
    }
}
