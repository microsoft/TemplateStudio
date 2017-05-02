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

using System;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    public class ITemplateInfoExtensionsTest : IClassFixture<TemplatesFixture>
    {
        private readonly TemplatesFixture _fixture;

        public ITemplateInfoExtensionsTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetTemplateType_project()
        {
            var target = GetTarget("ProjectTemplate");
            var result = target.GetTemplateType();

            Assert.Equal(TemplateType.Project, result);
        }

        [Fact]
        public void GetTemplateType_page()
        {
            var target = GetTarget("PageTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Page, result);
        }

        [Fact]
        public void GetTemplateType_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Unspecified, result);
        }

        [Fact]
        public void GetIcon()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetIcon();
            var expected = Path.Combine(_fixture.Repository.CurrentContentFolder, @"ProjectTemplate", ".template.config",
                "icon.png");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetIcon_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetIcon();
            Assert.Null(result);
        }

        [Fact]
        public void GetRichDescription()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetRichDescription();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetRichDescription_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetRichDescription();
            Assert.Null(result);
        }

        [Fact]
        public void GetFramework()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result,
                e1 =>
                {
                    e1.Equals("fx1");
                });
        }

        [Fact]
        public void GetFramework_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result);
        }

        [Fact]
        public void GetVersion()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetVersion();
            Assert.Equal("1.0.0", result);
        }

        [Fact]
        public void GetVersion_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetVersion();
            Assert.Null(result);
        }

        [Fact]
        public void GetGroup()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetGroup();
            Assert.Equal("group1", result);
        }

        [Fact]
        public void GetGroup_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetGroup();
            Assert.Null(result);
        }

        [Fact]
        public void GetOrder()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetOrder();
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetOrder_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void GetLicenses()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetLicenses().ToList();
            Assert.NotNull(result);

            Assert.Collection(result,
                e1 =>
                {
                    Assert.Equal("text1", e1.Text);
                    Assert.Equal("url1", e1.Url);
                },
                e2 =>
                {
                    Assert.Equal("text2", e2.Text);
                    Assert.Equal("url2", e2.Url);
                }
                );
        }

        [Fact]
        public void GetLicenses_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetLicenses().ToList();
            Assert.Collection(result);
        }

        [Fact]
        public void GetLayout()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetLayout().ToList();
            Assert.Collection(result,
                e1 =>
                {
                    Assert.Equal("Item1", e1.name);
                    Assert.Equal("Microsoft.UWPTemplates.Test.ProjectTemplate", e1.templateGroupIdentity);
                    Assert.Equal(true, e1.@readonly);
                },
                e2 =>
                {
                    Assert.Equal("Item2", e2.name);
                    Assert.Equal("Microsoft.UWPTemplates.Test.PageTemplate", e2.templateGroupIdentity);
                    Assert.Equal(false, e2.@readonly);
                }
                );
        }

        [Fact]
        public void GetLayout_NoLayout()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetLayout().ToList();
            Assert.Collection(result);
        }

        [Fact]
        public void GetDefaultName()
        {
            var target = GetTarget("ProjectTemplate");
            var result = target.GetDefaultName();

            Assert.Equal("DefaultName", result);
        }

        [Fact]
        public void GetDefaultName_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");
            var result = target.GetDefaultName();

            Assert.Equal("UnspecifiedTemplate", result);
        }

        private ITemplateInfo GetTarget(string templateName)
        {
            var allTemplates = _fixture.Repository.GetAll();
            var target = allTemplates.FirstOrDefault(t => t.Name == templateName);
            if (target == null)
            {
                throw new ArgumentException($"There is no template with name '{templateName}'. Number of templates: '{allTemplates.Count()}'");
            }
            return target;
        }
    }
}