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

namespace Microsoft.Templates.Core.Test
{
    public class NamingTest : IClassFixture<NamingFixture>
    {

        private readonly NamingFixture _fixture;

        public NamingTest(NamingFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Infer_Existing()
        {
            var existing = new string[] { "App" };
            var result = Naming.Infer(existing, "App", InferMode.ExcludeExisting);

            Assert.Equal("App1", result);
        }

        [Fact]
        public void ResolveTemplateName_Existing()
        {
            var existing = new string[] { "PageTemplate" };
            var result = Naming.ResolveTemplateName(existing, GetTarget("PageTemplate"));

            Assert.Equal("PageTemplate1", result);
        }

        [Fact]
        public void ResolveTemplateName_Reserved()
        {
            var existing = new string[] { };
            var result = Naming.ResolveTemplateName(existing, GetTarget("Page"));

            Assert.Equal("Page1", result);
        }

        [Fact]
        public void ResolveTemplateName_Default()
        {
            var existing = new string[] { };
            var result = Naming.ResolveTemplateName(existing, GetTarget("LiveTile"));

            Assert.Equal("LiveTile", result);
        }

        [Fact]
        public void ResolveTemplateName_Clean()
        {
            var existing = new string[] { };
            var result = Naming.ResolveTemplateName(existing, GetTarget("Blank$Page"));

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void ResolveTemplateName_TitleCase()
        {
            var existing = new string[] { };
            var result = Naming.ResolveTemplateName(existing, GetTarget("blank page"));

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Validate()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "Blank1", true);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_Empty()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "", true);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Fact]
        public void Validate_Existing()
        {
            var existing = new string[] { "Blank" };
            var result = Naming.Validate(existing, "Blank", true);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Fact]
        public void Validate_Default()
        {
            var existing = new string[] { "Blank" };
            var result = Naming.Validate(existing, "LiveTile", false);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_Reserved()
        {
            var existing = new string[] { "Blank" };
            var result = Naming.Validate(existing, "Page", false);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_InvalidChars()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "Blank;", true);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_StartWithNumber()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "1Blank", true);

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
