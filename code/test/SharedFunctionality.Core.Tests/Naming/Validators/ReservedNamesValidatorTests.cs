// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Trait("Group", "Minimum")]
    public class ReservedNamesValidatorTests
    {
        [Fact]
        public void Validate_SuccessfullyIdentifiesNameReservedNames()
        {
            var validator = new ReservedNamesValidator(new string[] { "Reserved" });

            var result = validator.Validate("Reserved");

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.ReservedName, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal(nameof(ReservedNamesValidator), result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifiesNameNoReservedNames()
        {
            var validator = new ReservedNamesValidator(new string[] { "Reserved" });

            var result = validator.Validate("Blank1");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }
    }
}
