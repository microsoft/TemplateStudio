// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Trait("Group", "Minimum")]
    [Trait("Type", "Naming")]
    public class ExistingNamesValidatorTests
    {
        [Fact]
        public void ExistingNamesValidator_NullConfig()
        {
            var validator = new ExistingNamesValidator(null);

            Assert.Throws<ArgumentNullException>(() => validator.Validate("Blank"));
        }

        [Fact]
        public void Validate_SuccessfullyIdentifiesExistingNames()
        {
            Func<IEnumerable<string>> getExistingNames = () => { return new string[] { "Blank" }; };

            var validator = new ExistingNamesValidator(getExistingNames);

            var result = validator.Validate("Blank");

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal(nameof(ExistingNamesValidator), result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyValidatesNotExistingNames()
        {
            Func<IEnumerable<string>> getExistingNames = () => { return new string[] { "Blank" }; };

            var validator = new ExistingNamesValidator(getExistingNames);

            var result = validator.Validate("Blank1");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
