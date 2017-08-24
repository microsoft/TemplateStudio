// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    public class NamingTest
    {
        private TemplatesFixture _fixture;

        public NamingTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        [Trait("Type", "ProjectGeneration")]
        public void Infer_Existing(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { "App" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Infer("App", validators);

            Assert.Equal("App1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_Reserved(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Infer("Page", validators);

            Assert.Equal("Page1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_Default(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Infer("LiveTile", validators);

            Assert.Equal("LiveTile1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_Clean(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("Blank$Page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_Clean2(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("ÑäöÜ!Page", new List<Validator>());

            Assert.Equal("ÑäöÜPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Infer_TitleCase(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("blank page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("Blank1", new List<Validator>());

            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_Empty(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_Existing(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { "Blank" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Validate("Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_Default(string language)
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
        public void Validate_Reserved(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Validate("Page", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_BadFormat_InvalidChars(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Validate("Blank;", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void Validate_BadFormat_StartWithNumber(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("1Blank", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Fact]
        public void IsValid_DirectoryExistsValidator_NonExistantDirectory()
        {
            var nonExistantFolderDirectoryValidator = new DirectoryExistsValidator(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()));

            // invalid dir
            var nonExistantFolderDirectory = nonExistantFolderDirectoryValidator.Validate(Guid.NewGuid().ToString());

            Assert.Equal(nonExistantFolderDirectory.IsValid, true);
        }

        [Fact]
        public void IsValid_DirectoryExistsValidator_ValidDirectory()
        {
            var directoryValidator = new DirectoryExistsValidator(Environment.CurrentDirectory);
            var randomValidFolderDirectory = directoryValidator.Validate(Guid.NewGuid().ToString());

            Assert.Equal(randomValidFolderDirectory.IsValid, true);
        }

        [Fact]
        public void IsNotValid_DirectoryExistsValidator_CollisionDirectory()
        {
            var directoryValidator = new DirectoryExistsValidator(Environment.CurrentDirectory);
            var existingDir = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()));

            var existsDirectory = directoryValidator.Validate(existingDir.Name);

            Assert.Equal(existsDirectory.IsValid, false);

            // Clean
            existingDir.Delete();
        }

        private ITemplateInfo GetTarget(string templateName)
        {
            var allTemplates = GenContext.ToolBox.Repo.GetAll();
            var target = allTemplates.FirstOrDefault(t => t.Name == templateName);
            if (target == null)
            {
                throw new ArgumentException($"There is no template with name '{templateName}'. Number of templates: '{allTemplates.Count()}'");
            }
            return target;
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
