// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Trait("Group", "Minimum")]
    public class RegExValidatorTests
    {
        [Fact]
        public void Validate_SuccessfullyIdentifiesNameThatDoesntMatchRegex()
        {
            var validator = new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" });

            var result = validator.Validate("Blank;");

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.Regex, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal("badFormat", result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifiesNameThatMatchesRegex()
        {
            var validator = new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" });

            var result = validator.Validate("Blank1");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
