// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Core.Test;
using Microsoft.Templates.Test.Artifacts;
using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;

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
