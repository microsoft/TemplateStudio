// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Test.TestFakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.Gen
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    public class GenComposerTests
    {
        private readonly TemplatesFixture _fixture;

        public GenComposerTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture("test", ProgrammingLanguages.CSharp);
        }

        [Fact]
        public void Compose_BasicGeneration()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx1",
            };

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "Main", TemplateId = "Microsoft.Templates.Test.PageTemplate.CSharp" }, TemplateType.Page);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);
        }

        [Fact]
        public void Compose_GenerationWithDependency()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx1",
            };

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "Main", TemplateId = "Microsoft.Templates.Test.PageTemplate.CSharp" }, TemplateType.Page);
            userSelection.Add(new UserSelectionItem() { Name = "Feature1", TemplateId = "Microsoft.Templates.Test.DependenciesTemplate.CSharp" }, TemplateType.Feature);
            userSelection.Add(new UserSelectionItem() { Name = "d1", TemplateId = "dp1" }, TemplateType.Feature);
            userSelection.Add(new UserSelectionItem() { Name = "d2", TemplateId = "dp2" }, TemplateType.Feature);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);

            var dp1 = new KeyValuePair<string, string>("dp1", "d1");
            var dp2 = new KeyValuePair<string, string>("dp2", "d2");

            Assert.True(genQueue.Where(g => g.Name == "Feature1").All(g => g.Parameters.Contains(dp1)));
            Assert.True(genQueue.Where(g => g.Name == "Feature1").All(g => g.Parameters.Contains(dp2)));
        }

        [Fact]
        public void Compose_GenerationWithRequirement()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx1",
            };

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "Main", TemplateId = "Microsoft.Templates.Test.PageTemplate.CSharp" }, TemplateType.Page);
            userSelection.Add(new UserSelectionItem() { Name = "r1", TemplateId = "r1" }, TemplateType.Feature);
            userSelection.Add(new UserSelectionItem() { Name = "Feature1", TemplateId = "Microsoft.Templates.Test.RequirementsTemplate.CSharp" }, TemplateType.Feature);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);
        }

        [Fact]
        public void Compose_GenerationWithExportParams()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx1",
            };

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "Main", TemplateId = "Microsoft.Templates.Test.PageTemplate.CSharp" }, TemplateType.Page);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);

            var export = new KeyValuePair<string, string>("testKey", "testValue");

            Assert.True(genQueue.Where(g => g.Template.Name == "Microsoft.Templates.Test.PageTemplate.CSharp").All(g => g.Parameters.Contains(export)));
        }

        [Fact]
        public void Compose_GenerationWithCasingParams()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx1",
            };

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "MainPage", TemplateId = "Microsoft.Templates.Test.PageTemplate.CSharp" }, TemplateType.Page);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);

            var kebabCase = new KeyValuePair<string, string>("ts.sourceName.casing.kebab", "main-page");
            var snakeCase = new KeyValuePair<string, string>("ts.sourceName.casing.snake", "main_page");
            var camelCase = new KeyValuePair<string, string>("ts.sourceName.casing.camel", "mainPage");
            var pascalCase = new KeyValuePair<string, string>("ts.sourceName.casing.pascal", "MainPage");

            Assert.True(genQueue.Where(g => g.Name == "MainPage").All(g => g.Parameters.Contains(kebabCase)));
            Assert.True(genQueue.Where(g => g.Name == "MainPage").All(g => g.Parameters.Contains(snakeCase)));
            Assert.True(genQueue.Where(g => g.Name == "MainPage").All(g => g.Parameters.Contains(camelCase)));
            Assert.True(genQueue.Where(g => g.Name == "MainPage").All(g => g.Parameters.Contains(pascalCase)));
        }

        [Fact]
        public void Compose_GenerationWithPropertyBag()
        {
            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = string.Empty,
                ProjectName = "TestProject",
            };

            var context = new UserSelectionContext(ProgrammingLanguages.CSharp, "test")
            {
                ProjectType = "pt1",
                FrontEndFramework = "fx4",
            };

            context.PropertyBag.Add("pb1", "value1");

            var userSelection = new UserSelection(context)
            {
                HomeName = "TestHome",
            };
            userSelection.Add(new UserSelectionItem() { Name = "MainPage", TemplateId = "Microsoft.Templates.Test.PageTemplatePropertyBag.CSharp" }, TemplateType.Page);

            var genQueue = GenComposer.Compose(userSelection);

            AssertBasicParameters(genQueue, context);

            var property = new KeyValuePair<string, string>("ts.generation.pb1", "value1");

            Assert.True(genQueue.Where(g => g.Name == "ProjectTemplatePropertyBag").All(g => g.Parameters.Contains(property)));
        }

        private void AssertBasicParameters(IEnumerable<GenInfo> genQueue, UserSelectionContext context)
        {
            // Check general params
            var homePageName = new KeyValuePair<string, string>(GenParams.HomePageName, "TestHome");
            var projectName = new KeyValuePair<string, string>(GenParams.ProjectName, GenContext.Current.ProjectName);
            var rootNamespace = new KeyValuePair<string, string>(GenParams.RootNamespace, GenContext.Current.SafeProjectName);

            Assert.True(genQueue.All(g => g.Parameters.Contains<KeyValuePair<string, string>>(homePageName)));
            Assert.True(genQueue.All(g => g.Parameters.Contains<KeyValuePair<string, string>>(projectName)));
            Assert.True(genQueue.All(g => g.Parameters.Contains<KeyValuePair<string, string>>(rootNamespace)));

            // Check project params
            var userName = new KeyValuePair<string, string>(GenParams.Username, Environment.UserName);
            var wizardVersion = new KeyValuePair<string, string>(GenParams.WizardVersion, string.Concat("v", GenContext.ToolBox.WizardVersion));
            var templatesVersion = new KeyValuePair<string, string>(GenParams.TemplatesVersion, string.Concat("v", GenContext.ToolBox.TemplatesVersion));
            var projectType = new KeyValuePair<string, string>(GenParams.ProjectType, context.ProjectType);
            var frontentFramework = new KeyValuePair<string, string>(GenParams.FrontEndFramework, context.FrontEndFramework);
            var platform = new KeyValuePair<string, string>(GenParams.Platform, context.Platform);

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(userName)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(userName)));

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(wizardVersion)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(wizardVersion)));

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(templatesVersion)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(templatesVersion)));

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(projectType)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(projectType)));

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(frontentFramework)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(frontentFramework)));

            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Project).All(g => g.Parameters.Contains(platform)));
            Assert.True(genQueue.Where(g => g.Template.GetTemplateOutputType() == TemplateOutputType.Item).All(g => !g.Parameters.Contains(platform)));
        }
    }
}
