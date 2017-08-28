// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("OneByOneGenerationMVVMBasic")]
    public class SingleItemGenerationMVVMBasicTests : SingleItemGenerationBase
    {
        //[Theory]
        //[MemberData("GetPageAndFeatureTemplates", "MVVMBasic")]
        //[Trait("Type", "OneByOneItemGeneration")]
        //public async void GenMVVMBasicWithIsolatedItems(string itemName, string projectType, string framework, string itemId, string language)
        //{
        //    await ExecuteOneByOneGenerations(itemName, projectType, framework, itemId, language);
        //}

        [Fact]
        public void Test()
        {
            GetPageAndFeatureTemplates("MVVMLight");
            GetPageAndFeatureTemplates("MVVMBasic");
            GetPageAndFeatureTemplates("CodeBehind");
        }
    }
    //[Collection("OneByOneGenerationMVVMLight")]
    //public class SingleItemGenerationMVVMLightTests : SingleItemGenerationBase
    //{
    //    [Theory]
    //    [MemberData("GetPageAndFeatureTemplates", "MVVMLight")]
    //    [Trait("Type", "OneByOneItemGeneration")]
    //    public async void GenMVVMLightWithIsolatedItems(string itemName, string projectType, string framework, string itemId, string language)
    //    {
    //        await ExecuteOneByOneGenerations(itemName, projectType, framework, itemId, language);
    //    }
    //}
    //[Collection("OneByOneGenerationCodeBehind")]
    //public class SingleItemGenerationCodeBehindTests : SingleItemGenerationBase
    //{
    //    [Theory]
    //    [MemberData("GetPageAndFeatureTemplates", "CodeBehind")]
    //    [Trait("Type", "OneByOneItemGeneration")]
    //    public async void GenCodeBehindWithIsolatedItems(string itemName, string projectType, string framework, string itemId, string language)
    //    {
    //        await ExecuteOneByOneGenerations(itemName, projectType, framework, itemId, language);
    //    }
    //}

    public class SingleItemGenerationBase : BaseTestContextProvider
    {
        protected GenerationFixture _fixture;

        protected void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language, this);
        }

        protected async Task ExecuteOneByOneGenerations(string itemName, string projectType, string framework, string itemId, string language)
        {
            SetUpFixtureForTesting(language);

            var projectTemplate = GenerationFixture.Templates.FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));
            var itemTemplate = GenerationFixture.Templates.FirstOrDefault(t => t.Identity == itemId);
            var finalName = itemTemplate.GetDefaultName();
            var validators = new List<Validator>
            {
                new ReservedNamesValidator(),
            };
            if (itemTemplate.GetItemNameEditable())
            {
                validators.Add(new DefaultNamesValidator());
            }

            finalName = Naming.Infer(finalName, validators);

            var projectName = $"{projectType}{framework}{finalName}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework, language);

            GenerationFixture.AddItem(userSelection, itemTemplate, GenerationFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            // Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            // Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            Directory.Delete(outputPath, true);
        }
        public static IEnumerable<object[]> GetPageAndFeatureTemplates(string framework)
        {
            return GenerationFixture.GetPageAndFeatureTemplates(framework);
        }
    }
}
