// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Casing;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    [Trait("Type", "ProjectGeneration")]
    public class ITemplateInfoExtensionsTest
    {
        private readonly TemplatesFixture _fixture;

        public ITemplateInfoExtensionsTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_project(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");
            var result = target.GetTemplateType();

            Assert.Equal(TemplateType.Project, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_page(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("PageTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Page, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_feature(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("FeatureTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Feature, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_service(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ServiceTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Service, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_testing(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("TestingTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Testing, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_composition(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("CompositionTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Composition, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateType_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Unspecified, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateOutputType_project(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetTemplateOutputType();
            Assert.Equal(TemplateOutputType.Project, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateOutputType_item(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("PageTemplate");

            var result = target.GetTemplateOutputType();
            Assert.Equal(TemplateOutputType.Item, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetTemplateOutputType_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetTemplateOutputType();
            Assert.Equal(TemplateOutputType.Unspecified, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetLanguage(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetLanguage();
            Assert.Equal(language, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetCompositionFilter(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("CompositionTemplate");

            var result = target.GetCompositionFilter();
            Assert.Equal("groupidentity == Microsoft.Templates.Test.PageTemplate", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetPlatform(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetPlatform();
            Assert.Equal("test", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetIcon(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var folderName = "ProjectTemplate";

            if (language == ProgrammingLanguages.VisualBasic)
            {
                folderName += "VB";
            }
            else if (language == ProgrammingLanguages.Any)
            {
                folderName += ProgrammingLanguages.Any;
            }

            var result = target.GetIcon();

            // Use GetFullPath to resolve any relative paths within the CurrentContentFolder
            var expected = Path.GetFullPath(Path.Combine(_fixture.Repository.CurrentContentFolder, folderName, ".template.config", "icon.png"));
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetIcon_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetIcon();
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetRichDescription(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetRichDescription();
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetRichDescription_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetRichDescription();
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetFramework(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetFrontEndFrameworkList();
            Assert.Collection(result, e1 => e1.Equals("fx1", StringComparison.Ordinal), e2 => e2.Equals("TestFramework", StringComparison.Ordinal));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetFrontEndFramework_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetFrontEndFrameworkList();
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetProjectType(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetProjectTypeList();
            Assert.Collection(result, e1 => e1.Equals("pt1", StringComparison.Ordinal), e2 => e2.Equals("pt2", StringComparison.Ordinal), e3 => e3.Equals("TestProjectType", StringComparison.Ordinal));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetProjectType_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetProjectTypeList();
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetDependencyList(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("DependenciesTemplate");

            var result = target.GetDependencyList();
            Assert.Collection(result, e1 => e1.Equals("dp1", StringComparison.Ordinal), e2 => e2.Equals("dp2", StringComparison.Ordinal));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetRequirementList(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("RequirementsTemplate");

            var result = target.GetRequirementsList();
            Assert.Collection(result, e1 => e1.Equals("r1", StringComparison.Ordinal), e2 => e2.Equals("r2", StringComparison.Ordinal));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetExclusionsList(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ExclusionsTemplate");

            var result = target.GetExclusionsList();
            Assert.Collection(result, e1 => e1.Equals("e1", StringComparison.Ordinal), e2 => e2.Equals("e2", StringComparison.Ordinal));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetExports(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("PageTemplate");

            var result = target.GetExports();
            Assert.Collection(result, e1 => e1.Equals(("baseclass", "ViewModelBase")), e2 => e2.Equals(("setter", "Set")));
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetVersion(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetVersion();
            Assert.Equal("1.0.0", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetVersion_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetVersion();
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetGroup(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetGroup();
            Assert.Equal("group1", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetGenGroup(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("PageTemplate");

            var result = target.GetGenGroup();
            Assert.Equal(1, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetGenGroup_default(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("FeatureTemplate");

            var result = target.GetGenGroup();
            Assert.Equal(0, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetGroup_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetGroup();
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetIsGroupExclusiveSelection_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetIsGroupExclusiveSelection();
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetIsGroupExclusiveSelection_true(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("FeatureTemplate");

            var result = target.GetIsGroupExclusiveSelection();
            Assert.True(result);
        }

        [Fact]
        [Trait("Type", "ProjectGeneration")]
        public void GetDisplayOrder()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.Templates.Test.PageTemplate.CSharp");

            var result = target.GetDisplayOrder();
            Assert.Equal(1, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
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
        [MemberData(nameof(GetAllLanguages))]
        public void GetCompositionOrder_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetCompositionOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
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
        [MemberData(nameof(GetAllLanguages))]
        public void GetRequiredVersions(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("ProjectTemplate");

            var result = target.GetRequiredVersions().ToList();
            Assert.NotNull(result);

            Assert.Collection(
                result,
                e1 =>
                {
                    Assert.Equal("sdk1", e1);
                },
                e2 =>
                {
                    Assert.Equal("sdk2", e2);
                });
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetLicenses_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLicenses().ToList();
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
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
                    Assert.True(e1.Readonly);
                },
                e2 =>
                {
                    Assert.Equal("Item2", e2.Name);
                    Assert.Equal("Microsoft.UWPTemplates.Test.PageTemplate", e2.TemplateGroupIdentity);
                    Assert.False(e2.Readonly);
                });
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetLayout_NoLayout(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");

            var result = target.GetLayout().ToList();
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
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

            Assert.True(result);
        }

        [Fact]
        public void GetRightClickEnabledFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.Templates.Test.FeatureTemplate.CSharp");
            var result = target.GetRightClickEnabled();

            Assert.False(result);
        }

        [Fact]
        public void GetIsHidden()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("HiddenTemplate");
            var result = target.GetIsHidden();

            Assert.True(result);
        }

        [Fact]
        public void GetIsHiddenFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByIdentity("Microsoft.Templates.Test.FeatureTemplate.CSharp");
            var result = target.GetIsHidden();

            Assert.False(result);
        }

        [Fact]
        public void GetIsMultipleInstance()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("PageTemplate");
            var result = target.GetMultipleInstance();

            Assert.True(result);
        }

        [Fact]
        public void GetIsMultipleInstanceFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("FeatureTemplate");
            var result = target.GetMultipleInstance();

            Assert.False(result);
        }

        [Fact]
        public void GetItemNameEditable()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("PageTemplate");
            var result = target.GetItemNameEditable();

            Assert.True(result);
        }

        [Fact]
        public void GetItemNameEditableFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("FeatureTemplate");
            var result = target.GetItemNameEditable();

            Assert.False(result);
        }

        [Fact]
        public void GetOutputToParent()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("OutputToParentTemplate");
            var result = target.GetOutputToParent();

            Assert.True(result);
        }

        [Fact]
        public void GetOutputToParentFalse()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("PageTemplate");
            var result = target.GetOutputToParent();

            Assert.False(result);
        }

        [Fact]
        public void GetTelemetryName_unspecified()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("UnspecifiedTemplate");
            var result = target.GetTelemetryName();

            Assert.Equal("UnspecifiedTemplate", result);
        }

        [Fact]
        public void GetTelemetryName()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var target = GetTargetByName("TelemetryNameTemplate");
            var result = target.GetTelemetryName();

            Assert.Equal("TelemName", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetDefaultName_unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");
            var result = target.GetDefaultName();

            Assert.Equal("UnspecifiedTemplate", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetCasingServices(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("PageTemplate");
            var result = target.GetTextCasings();

            Assert.Equal(4, result.Count);
            Assert.Contains(result, r => r.Type == CasingType.Kebab);
            Assert.Contains(result, r => r.Type == CasingType.Snake);
            Assert.Contains(result, r => r.Type == CasingType.Camel);
            Assert.Contains(result, r => r.Type == CasingType.Pascal);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void GetCasingServices_Unspecified(string language)
        {
            SetUpFixtureForTesting(language);

            var target = GetTargetByName("UnspecifiedTemplate");
            var result = target.GetTextCasings();

            Assert.Empty(result);
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
