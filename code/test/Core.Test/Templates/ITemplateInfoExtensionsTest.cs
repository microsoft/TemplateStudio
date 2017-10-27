// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("Type", "ProjectGeneration")]
    public class ITemplateInfoExtensionsTest
    {
        private readonly TemplatesFixture _fixture;

        public ITemplateInfoExtensionsTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetTemplateType_project(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");
            var result = target.GetTemplateType();

            Assert.Equal(TemplateType.Project, result);
        }

        [Fact]
        public void GetTemplateType_page()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Page, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetTemplateType_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Unspecified, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetIcon(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var folderName = "ProjectTemplate";

            if (language == ProgrammingLanguages.VisualBasic)
            {
                folderName += "VB";
            }

            var result = target.GetIcon();
            var expected = Path.Combine(_fixture.Repository.CurrentContentFolder, folderName, ".template.config", "icon.png");
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetIcon_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetIcon();
            Assert.Null(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetRichDescription(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetRichDescription();
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetRichDescription_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetRichDescription();
            Assert.Null(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetFramework(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result, e1 => e1.Equals("fx1"));
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetFramework_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetFrameworkList();
            Assert.Collection(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetVersion(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetVersion();
            Assert.Equal("1.0.0", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetVersion_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetVersion();
            Assert.Null(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetGroup(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetGroup();
            Assert.Equal("group1", result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetGroup_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetGroup();
            Assert.Null(result);
        }

        [Fact]
        [Trait("Type", "ProjectGeneration")]
        public void GetDisplayOrder()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            var result = target.GetDisplayOrder();
            Assert.Equal(1, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetDisplayOrder_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetDisplayOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void GetCompositionOrder()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("CompositionTemplate");

            var result = target.GetCompositionOrder();
            Assert.Equal(1, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetCompositionOrder_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetCompositionOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetLicenses(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetLicenses().ToList();
            Assert.NotNull(result);

            Assert.Collection(
                result,
                e1 =>
                {
                    Assert.Equal("text1", e1.Text);
                    Assert.Equal("url1", e1.Url);
                },
                e2 =>
                {
                    Assert.Equal("text2", e2.Text);
                    Assert.Equal("url2", e2.Url);
                });
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetLicenses_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLicenses().ToList();
            Assert.Collection(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetLayout(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetLayout().ToList();
            Assert.Collection(
                result,
                e1 =>
                {
                    Assert.Equal("Item1", e1.Name);
                    Assert.Equal("Microsoft.UWPTemplates.Test.ProjectTemplate", e1.TemplateGroupIdentity);
                    Assert.Equal(true, e1.Readonly);
                },
                e2 =>
                {
                    Assert.Equal("Item2", e2.Name);
                    Assert.Equal("Microsoft.UWPTemplates.Test.PageTemplate", e2.TemplateGroupIdentity);
                    Assert.Equal(false, e2.Readonly);
                });
        }

        [Theory]
        [MemberData("GetAllLanguages")]
        public void GetLayout_NoLayout(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLayout().ToList();
            Assert.Collection(result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
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
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("RightClickTemplate");
            var result = target.GetRightClickEnabled();

            Assert.Equal(true, result);
        }

        [Fact]
        public void GetRightClickEnabledFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");
            var result = target.GetRightClickEnabled();

            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData("GetAllLanguages")]
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
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                yield return new object[] { language };
            }
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
