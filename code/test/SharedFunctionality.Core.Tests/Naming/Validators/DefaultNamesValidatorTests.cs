// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test.Naming.Validators
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    public class DefaultNamesValidatorTests
    {
        private TemplatesFixture _fixture;

        public DefaultNamesValidatorTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_SuccessfullyIdentifiesDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validator = new DefaultNamesValidator();

            var result = validator.Validate("LiveTile");

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count == 1);
            Assert.Equal(ValidationErrorType.ReservedName, result.Errors.FirstOrDefault()?.ErrorType);
            Assert.Equal(nameof(DefaultNamesValidator), result.Errors.FirstOrDefault()?.ValidatorName);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_SuccessfullyValidatesNonDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validator = new DefaultNamesValidator();

            var result = validator.Validate("MyOwnName");

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count == 0);
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture("test", language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                if (language != ProgrammingLanguages.Any)
                {
                    yield return new object[] { language };
                }
            }
        }
    }
}
