// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    public class ItemNameServiceTests
    {
        private TemplatesFixture _fixture;

        public ItemNameServiceTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForReservedNames()
        {
            var config = new ItemNameValidationConfig()
            {
                ReservedNames = new string[]
                {
                    "Page",
                },
            };

            var validationService = new ItemNameService(config, null);
            var result = validationService.Infer("Page");

            Assert.Equal("Page1", result);
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForExistingNames()
        {
            Func<IEnumerable<string>> getExistingNames = () => { return new string[] { "Page1" }; };

            var config = new ItemNameValidationConfig()
            {
                ReservedNames = new string[]
                {
                    "Page",
                },
                ValidateExistingNames = true,
            };

            var validationService = new ItemNameService(config, getExistingNames);
            var result = validationService.Infer("Page");

            Assert.Equal("Page2", result);
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForExistingNamesWithNameUpdate()
        {
            var existingNames = new List<string>() { "Page1" };

            Func<IEnumerable<string>> getExistingNames = () => { return existingNames; };

            var config = new ItemNameValidationConfig()
            {
                ReservedNames = new string[]
                {
                    "Page",
                },
                ValidateExistingNames = true,
            };

            var validationService = new ItemNameService(config, getExistingNames);
            var result = validationService.Infer("Page");

            Assert.Equal("Page2", result);

            existingNames.Add("Page2");
            var result2 = validationService.Infer("Page");

            Assert.Equal("Page3", result2);
        }

        [Fact]
        public void Validate_SuccessfullyAccountsForRegex()
        {
            var config = new ItemNameValidationConfig()
            {
                Regexs = new RegExConfig[]
                {
                    new RegExConfig()
                    {
                        Name = "itemEndsWithPage",
                        Pattern = ".*(?<!page)$",
                    },
                },
            };

            var validationService = new ItemNameService(config, null);
            var result = validationService.Validate("Testpage");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorType == ValidationErrorType.Regex);
            Assert.Contains(result.Errors, e => e.ValidatorName == "itemEndsWithPage");
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_SuccessfullyAccountsForDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var config = new ItemNameValidationConfig()
            {
                ValidateDefaultNames = true,
            };

            var validationService = new ItemNameService(config, null);
            var result = validationService.Validate("LiveTile");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorType == ValidationErrorType.ReservedName);
            Assert.Contains(result.Errors, e => e.ValidatorName == "DefaultNamesValidator");
        }

        [Fact]
        public void Validate_SuccessfullyAccountsForEmptyNames()
        {
            var config = new ItemNameValidationConfig()
            {
                ValidateEmptyNames = true,
            };

            var validationService = new ItemNameService(config, null);
            var result = validationService.Validate(string.Empty);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorType == ValidationErrorType.EmptyName);
            Assert.Contains(result.Errors, e => e.ValidatorName == "EmptyNameValidator");
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture("test", language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                if (language != ProgrammingLanguages.Any && language != ProgrammingLanguages.Cpp)
                {
                    yield return new object[] { language };
                }
            }
        }
    }
}
