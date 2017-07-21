// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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

        [Fact]
        public void Infer_Existing()
        {
            var existing = new string[] { "App" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Infer("App", validators);

            Assert.Equal("App1", result);
        }


        [Fact]
        public void Infer_Reserved()
        {
            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Infer("Page", validators);

            Assert.Equal("Page1", result);
        }

        [Fact]
        public void Infer_Default()
        {
            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Infer("LiveTile", validators);

            Assert.Equal("LiveTile1", result);
        }

        [Fact]
        public void Infer_Clean()
        {
            var existing = new string[] { };
            var result = Naming.Infer("Blank$Page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Infer_Clean2()
        {
            var existing = new string[] { };
            var result = Naming.Infer("ÑäöÜ!Page", new List<Validator>());

            Assert.Equal("ÑäöÜPage", result);
        }

        [Fact]
        public void Infer_TitleCase()
        {
            var existing = new string[] { };
            var result = Naming.Infer("blank page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Validate()
        {
            var result = Naming.Validate("Blank1", new List<Validator>());

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_Empty()
        {
            var result = Naming.Validate("", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Fact]
        public void Validate_Existing()
        {
            var existing = new string[] { "Blank" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Validate("Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Fact]
        public void Validate_Default()
        {
            var validators = new List<Validator>()
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Validate("LiveTile", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_Reserved()
        {
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Validate("Page", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_InvalidChars()
        {
            var existing = new string[] { };
            var result = Naming.Validate("Blank;", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_StartWithNumber()
        {            
            var result = Naming.Validate("1Blank", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
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
    }

}
