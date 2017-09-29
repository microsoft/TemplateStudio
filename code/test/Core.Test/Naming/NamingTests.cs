// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]

    public class NamingTests
    {
        private TemplatesFixture _fixture;

        public NamingTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        [Trait("Type", "ProjectGeneration")]
        public void Infer_SuccessfullyAccountsForExistingNames(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new[] { "App" };
            var validators = new List<Validator>
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Infer("App", validators);

            Assert.Equal("App1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_SuccessfullyAccountsForReservedNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Infer("Page", validators);

            Assert.Equal("Page1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_SuccessfullyAccountsForDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Infer("LiveTile", validators);

            Assert.Equal("LiveTile1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_RemovesInvalidCharacters(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Infer("Blank$Page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_DoesNotRemoveNonAsciiCharacters(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Infer("ÑäöÜ!Page", new List<Validator>());

            Assert.Equal("ÑäöÜPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_SuccessfullyHandlesSpacesAndConversionToTitleCase(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Infer("blank page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_RecognizesValidNameAsValid(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("Blank1", new List<Validator>());

            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_RecognizesEmptyStringAsInvalid(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_SuccessfullyIdentifiesExistingNames(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new[] { "Blank" };
            var validators = new List<Validator>
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Validate("Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_SuccessfullyIdentifiesDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Validate("LiveTile", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_SuccessfullyIdentifiesReservedNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Validate("Page", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_SuccessfullyIdentifies_InvalidChars(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("Blank;", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_SuccessfullyIdentifies_NamesThatStartWithNumbers(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("1Blank", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                yield return new object[] { language };
            }
        }
    }
}
