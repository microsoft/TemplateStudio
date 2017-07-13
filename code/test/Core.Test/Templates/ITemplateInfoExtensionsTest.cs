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
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Test.Artifacts;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    public class ITemplateInfoExtensionsTest 
    {
        private readonly TemplatesFixture _fixture;

        public ITemplateInfoExtensionsTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetTemplateType_project(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");
            var result = target.GetTemplateType();

            Assert.Equal(TemplateType.Project, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetTemplateType_page(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByIdentity("Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Page, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetTemplateType_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Unspecified, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetIcon(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var folderName = "ProjectTemplate";

            if (language == "VisualBasic")
            {
                folderName += "VB";
            }

            var result = target.GetIcon();
            var expected = Path.Combine(_fixture.Repository.CurrentContentFolder, folderName, ".template.config",
                "icon.png");
            Assert.Equal(expected, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetIcon_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetIcon();
            Assert.Null(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetRichDescription(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetRichDescription();
            Assert.NotNull(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetRichDescription_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetRichDescription();
            Assert.Null(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetFramework(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result,
                e1 =>
                {
                    e1.Equals("fx1");
                });
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetFramework_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetVersion(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetVersion();
            Assert.Equal("1.0.0", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetVersion_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetVersion();
            Assert.Null(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetGroup(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetGroup();
            Assert.Equal("group1", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetGroup_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetGroup();
            Assert.Null(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetOrder(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetOrder();
            Assert.Equal(1, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetOrder_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetLicenses(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

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

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetLicenses_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLicenses().ToList();
            Assert.Collection(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetLayout(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

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

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetLayout_NoLayout(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLayout().ToList();
            Assert.Collection(result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetDefaultName(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetDefaultName();

            Assert.Equal("DefaultName", result);
        }

        [Fact]
        public void GetRightClickEnabled()
        {
            var target = GetTargetByName("RightClickTemplate");
            var result = target.GetRightClickEnabled();

            Assert.Equal(true, result);
        }

        [Fact]
        public void GetRightClickEnabledFalse()
        {
            var target = GetTargetByIdentity("Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");
            var result = target.GetRightClickEnabled();

            Assert.Equal(false, result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void GetDefaultName_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");
            var result = target.GetDefaultName();

            Assert.Equal("UnspecifiedTemplate", result);
        }

        private ITemplateInfo GetTargetByName(string templateName)
        {
            var allTemplates = _fixture.Repository.GetAll();
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
            GenContext.Bootstrap(new UnitTestsTemplatesSource(), new FakeGenShell(), language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            yield return new object[] { "C#" };
            yield return new object[] { "VisualBasic" };
        }

        private ITemplateInfo GetTargetByIdentity(string templateIdentity)
        {
            var allTemplates = _fixture.Repository.GetAll();
            var target = allTemplates.FirstOrDefault(t => t.Identity == templateIdentity);
            if (target == null)
            {
                throw new ArgumentException($"There is no template with identity '{templateIdentity}'. Number of templates: '{allTemplates.Count()}'");
            }
            return target;
        }
    }
}
