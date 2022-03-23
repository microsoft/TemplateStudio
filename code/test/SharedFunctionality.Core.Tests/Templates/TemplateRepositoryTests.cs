// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;
using Xunit;

namespace Microsoft.Templates.Core.Test.Templates
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    public class TemplateRepositoryTests
    {
        private readonly TemplatesFixture _fixture;
        private TemplatesRepository _repo;
        private const string TestPlatform = "test";
        private UserSelectionContext _context;

        public TemplateRepositoryTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(TestPlatform, language);
            _repo = GenContext.ToolBox.Repo;
            _context = new UserSelectionContext(language, TestPlatform);
        }

        [Fact]
        public void GetSupportedProjectTypes()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = _repo.GetProjectTypes(_context);

            Assert.Collection(projectTemplates, e1 => e1.Equals("ProjectType"));
        }

        [Fact]
        public void GetFrontendFrameworks()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            var frameworks = _repo.GetFrontEndFrameworks(_context);

            Assert.Collection(frameworks, e1 =>
            {
                Assert.Equal("TestFramework", e1.Name);
                Assert.Equal(FrameworkTypes.FrontEnd.ToString().ToUpperInvariant(), e1.Tags["type"].ToString().ToUpperInvariant());
            });
        }

        [Fact]
        public void GetPages_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx1";

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, _context);

            Assert.Collection(
                pages,
                p1 =>
                {
                    Assert.Equal("PageTemplate", p1.Name);
                });
        }

        [Fact]
        public void GetPages_BackendAndFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx1";
            _context.BackEndFramework = "fx3";

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, _context);

            Assert.Collection(
                pages,
                p1 =>
                {
                    Assert.Equal("PageTemplate", p1.Name);
                });
        }

        [Fact]
        public void GetPages_PropertyBagFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx4";
            _context.PropertyBag.Add("pb1", "value1");
            _context.PropertyBag.Add("pb2", "value2");

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, _context);

            Assert.Collection(
                pages,
                p1 =>
                {
                    Assert.Equal("PageTemplatePropertyBag", p1.Name);
                });
        }

        [Fact]
        public void GetPages_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx5";
            ////_context.BackEndFramework = "fx4";

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, _context);

            Assert.Empty(pages);
        }

        [Fact]
        public void GetFeatures_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx1";

            var features = _repo.GetTemplatesInfo(TemplateType.Feature, _context);

            Assert.Contains(features, f => f.Name == "FeatureTemplate");
        }

#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Fact(Skip = "There are templates that use an all check for FrontEndFramework and so without a backend check this always finds something")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
        public void GetFeatures_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt9";
            _context.FrontEndFramework = "fx5";
            ////_context.BackEndFramework = "fx4";

            var features = _repo.GetTemplatesInfo(TemplateType.Feature, _context);

            Assert.Empty(features);
        }

        [Fact]
        public void GetServices_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx1";

            var services = _repo.GetTemplatesInfo(TemplateType.Service, _context);

            Assert.Collection(
                services,
                f1 =>
                {
                    Assert.Equal("ServiceTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetServices_BackendAndFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt3";
            _context.FrontEndFramework = "fx1";
            _context.BackEndFramework = "fx3";

            var services = _repo.GetTemplatesInfo(TemplateType.Service, _context);

            Assert.Collection(
                services,
                f1 =>
                {
                    Assert.Equal("ServiceTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetServices_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx5";
            ////_context.BackEndFramework = "fx4";

            var services = _repo.GetTemplatesInfo(TemplateType.Service, _context);

            Assert.Empty(services);
        }

        [Fact]
        public void GetTestings_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx1";

            var testing = _repo.GetTemplatesInfo(TemplateType.Testing, _context);

            Assert.Collection(
                testing,
                f1 =>
                {
                    Assert.Equal("TestingTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetTestings_BackendAndFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt3";
            _context.FrontEndFramework = "fx1";
            _context.BackEndFramework = "fx3";

            var testing = _repo.GetTemplatesInfo(TemplateType.Testing, _context);

            Assert.Collection(
                testing,
                f1 =>
                {
                    Assert.Equal("TestingTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetTestings_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            _context.ProjectType = "pt1";
            _context.FrontEndFramework = "fx5";
            ////_context.BackEndFramework = "fx4";

            var testing = _repo.GetTemplatesInfo(TemplateType.Testing, _context);

            Assert.Empty(testing);
        }
    }
}
