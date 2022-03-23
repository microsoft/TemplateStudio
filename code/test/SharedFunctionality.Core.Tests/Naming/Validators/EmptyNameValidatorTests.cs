// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Trait("Group", "Minimum")]
    [Trait("Type", "Naming")]
    public class EmptyNameValidatorTests
    {
        [Fact]
        public void Validate_RecognizesEmptyStringAsInvalid()
        {
            var validator = new EmptyNameValidator();

            var result = validator.Validate(string.Empty);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.EmptyName, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal(nameof(EmptyNameValidator), result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Fact]
        public void Validate_RecognizesNonEmptyStringAsValid()
        {
            var validator = new EmptyNameValidator();

            var result = validator.Validate("Test");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
