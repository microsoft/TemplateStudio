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
using Microsoft.Templates.UI;
using Microsoft.VisualStudio.Threading;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;
using Microsoft.Templates.UI.Generation;

namespace Microsoft.Templates.Test
{
    public class BaseGenAndBuildTests : BaseTestContextProvider
    {
        protected BaseGenAndBuildFixture _fixture;

        protected static string ShortLanguageName(string language)
        {
            return language == ProgrammingLanguages.CSharp ? "CS" : "VB";
        }

        protected static string ShortProjectType(string projectType)
        {
            switch (projectType)
            {
                case "Blank":
                    return "B";
                case "SplitView":
                    return "SV";
                case "TabbedPivot":
                    return "TP";
                default:
                    return projectType;
            }
        }

        protected static string GetProjectExtension(string language)
        {
            return language == ProgrammingLanguages.CSharp ? "csproj" : "vbproj";
        }

        protected async Task<string> AssertGenerateProjectAsync(Func<ITemplateInfo, bool> projectTemplateSelector, string projectName, string projectType, string framework, string platform, string language, Func<ITemplateInfo, bool> itemTemplatesSelector = null, Func<ITemplateInfo, string> getName = null, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            var targetProjectTemplate = _fixture.Templates().FirstOrDefault(projectTemplateSelector);

            ProjectName = projectName;

            DestinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            DestinationParentPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            var userSelection = _fixture.SetupProject(projectType, framework, platform, language);

            if (getName != null || itemTemplatesSelector == null)
            {
                var itemTemplates = _fixture.Templates().Where(itemTemplatesSelector);
                _fixture.AddItems(userSelection, itemTemplates, getName);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Count() > 2);

            if (platform == Platforms.Uwp)
            {
                AssertCorrectProjectConfigInfo(projectType, framework, platform);
            }

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(resultPath);
            }

            return resultPath;
        }

        protected void AssertCorrectProjectConfigInfo(string expectedProjectType, string expectedFramework, string expectedPlatform)
        {
            var info = ProjectConfigInfo.ReadProjectConfiguration();

            Assert.Equal(expectedProjectType, info.ProjectType);
            Assert.Equal(expectedFramework, info.Framework);
            Assert.Equal(expectedPlatform, info.Platform);
        }

        protected void AssertBuildProjectAsync(string projectPath, string projectName, string platform)
        {
            // Build solution
            var result = _fixture.BuildSolution(projectName, projectPath, platform);

            // Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            Fs.SafeDeleteDirectory(projectPath);
        }

        protected async Task<string> AssertGenerateRightClickAsync(string projectName, string projectType, string framework, string platform, string language, bool emptyProject, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            ProjectName = projectName;
            DestinationPath = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);
            DestinationParentPath = Path.Combine(_fixture.TestNewItemPath, projectName);

            var userSelection = _fixture.SetupProject(projectType, framework, platform, language);

            if (!emptyProject)
            {
                _fixture.AddItems(userSelection, _fixture.GetTemplates(framework, platform), BaseGenAndBuildFixture.GetDefaultName);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var project = Path.Combine(_fixture.TestNewItemPath, projectName);

            // Assert on project
            Assert.True(Directory.Exists(project));

            int emptyProjecFileCount = Directory.GetFiles(project, "*.*", SearchOption.AllDirectories).Count();
            Assert.True(emptyProjecFileCount > 2);

            var rightClickTemplates = _fixture.Templates().Where(
                                           t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                             && t.GetFrameworkList().Contains(framework)
                                             && t.GetPlatform() == platform
                                             && !t.GetIsHidden()
                                             && t.GetRightClickEnabled());

            await AddRightClickTemplatesAsync(rightClickTemplates, projectName, projectType, framework, platform, language);

            var finalProjectPath = Path.Combine(_fixture.TestNewItemPath, projectName);
            int finalProjectFileCount = Directory.GetFiles(finalProjectPath, "*.*", SearchOption.AllDirectories).Count();

            if (emptyProject)
            {
                Assert.True(finalProjectFileCount > emptyProjecFileCount);
            }
            else
            {
                Assert.True(finalProjectFileCount == emptyProjecFileCount);
            }

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(finalProjectPath);
            }

            return finalProjectPath;
        }

        protected async Task AddRightClickTemplatesAsync(IEnumerable<ITemplateInfo> rightClickTemplates, string projectName, string projectType, string framework, string platform, string language)
        {
            // Add new items
            foreach (var item in rightClickTemplates)
            {
                TempGenerationPath = GenContext.GetTempGenerationPath(projectName);

                var newUserSelection = new UserSelection(projectType, framework, platform, language)
                {
                    HomeName = string.Empty,
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge
                };

                _fixture.AddItem(newUserSelection, item, BaseGenAndBuildFixture.GetDefaultName);

                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(item.GetTemplateType(), newUserSelection);

                NewItemGenController.Instance.UnsafeFinishGeneration(newUserSelection);
            }
        }

        protected async Task<(string ProjectPath, string ProjecName)> AssertGenerationOneByOneAsync(string itemName, string projectType, string framework, string platform, string itemId, string language, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            var projectTemplate = _fixture.Templates().FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework) && t.GetPlatform() == platform);
            var itemTemplate = _fixture.Templates().FirstOrDefault(t => t.Identity == itemId);
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

            var projectName = $"{ShortProjectType(projectType)}{finalName}{ShortLanguageName(language)}";

            ProjectName = projectName;
            DestinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            DestinationParentPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            OutputPath = DestinationPath;

            var userSelection = _fixture.SetupProject(projectType, framework, platform, language);

            _fixture.AddItem(userSelection, itemTemplate, BaseGenAndBuildFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Count() > 2);

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(resultPath);
            }

            return (resultPath, projectName);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForGenerationAsync()
        {
            var result = GenerationFixture.GetProjectTemplates();

            return result;
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpComparisonProjectAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(Platforms.Uwp);

            var projectTemplate = _fixture.Templates().FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));

            var singlePageName = string.Empty;

            var genIdentitiesList = genIdentities.ToList();

            if (genIdentitiesList.Count == 1)
            {
                singlePageName = genIdentitiesList.Last().Split('.').Last();
            }

            ProjectName = $"{projectType}{framework}Compare{singlePageName}{ShortLanguageName(language)}";
            DestinationPath = Path.Combine(_fixture.TestProjectsPath, ProjectName, ProjectName);
            OutputPath = DestinationPath;

            var userSelection = _fixture.SetupProject(projectType, framework, Platforms.Uwp, language);

            foreach (var identity in genIdentitiesList)
            {
                var itemTemplate = _fixture.Templates().FirstOrDefault(t => t.Identity.Contains(identity)
                                                                         && t.GetFrameworkList().Contains(framework));
                _fixture.AddItem(userSelection, itemTemplate, BaseGenAndBuildFixture.GetDefaultName);

                // Add multiple pages if supported to check these are handled the same
                if (itemTemplate.GetMultipleInstance())
                {
                    _fixture.AddItem(userSelection, itemTemplate, BaseGenAndBuildFixture.GetDefaultName);
                }
            }

            if (lastPageIsHome)
            {
                // Useful if creating a blank project type and want to change the start page
                userSelection.HomeName = userSelection.Pages.Last().name;

                if (projectType == "TabbedPivot")
                {
                    userSelection.Pages.Reverse();
                }
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, ProjectName);

            return (resultPath, ProjectName);
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGeneration(string framework)
        {
            var result = GenerationFixture.GetPageAndFeatureTemplatesForGeneration(framework);
            return result;
        }

        private const string NavigationPanel = "SplitView";
        private const string Blank = "Blank";
        private const string TabsAndPivot = "TabbedPivot";
        private const string MvvmBasic = "MVVMBasic";
        private const string MvvmLight = "MVVMLight";
        private const string CodeBehind = "CodeBehind";

        // This returns a list of project types and frameworks supported by BOTH C# and VB
        public static IEnumerable<object[]> GetMultiLanguageProjectsAndFrameworks()
        {
            yield return new object[] { NavigationPanel, CodeBehind };
            yield return new object[] { NavigationPanel, MvvmBasic };
            yield return new object[] { NavigationPanel, MvvmLight };
            yield return new object[] { Blank, CodeBehind };
            yield return new object[] { Blank, MvvmBasic };
            yield return new object[] { Blank, MvvmLight };
            yield return new object[] { TabsAndPivot, CodeBehind };
            yield return new object[] { TabsAndPivot, MvvmBasic };
            yield return new object[] { TabsAndPivot, MvvmLight };
        }

        // Gets a list of partial identities for page and feature templates supported by C# and VB
#pragma warning disable RECS0154 // Parameter is never used - projectType can be used when all options aren't supported on all platforms
        protected static IEnumerable<string> GetPagesAndFeaturesForMultiLanguageProjectsAndFrameworks(string projectType, string framework)
#pragma warning restore RECS0154 // Parameter is never used
        {
            if (framework == CodeBehind)
            {
                return new[]
                {
                    "wts.Page.Blank.CodeBehind", "wts.Page.Settings.CodeBehind", "wts.Page.Chart.CodeBehind",
                    "wts.Page.Grid.CodeBehind", "wts.Page.WebView.CodeBehind", "wts.Page.MediaPlayer.CodeBehind",
                    "wts.Page.TabbedPivot.CodeBehind", "wts.Page.Map.CodeBehind", "wts.Page.Camera.CodeBehind",
                    "wts.Page.ImageGallery.CodeBehind", "wts.Page.MasterDetail.CodeBehind",
                    "wts.Feat.SettingsStorage", "wts.Feat.SuspendAndResume", "wts.Feat.LiveTile",
                    "wts.Feat.UriScheme", "wts.Feat.FirstRunPrompt", "wts.Feat.WhatsNewPrompt",
                    "wts.Feat.ToastNotifications", "wts.Feat.BackgroundTask", "wts.Feat.HubNotifications",
                    "wts.Feat.StoreNotifications", "wts.Feat.FeedbackHub.CodeBehind", "wts.Feat.MultiView",
                    "wts.Feat.ShareSource", "wts.Feat.ShareTarget", "wts.Feat.UriScheme", "wts.Feat.WebToAppLink",
                    "wts.Feat.DragAndDrop.CodeBehind"
                };
            }
            else
            {
                return new[]
                {
                    "wts.Page.Blank", "wts.Page.Settings", "wts.Page.Chart",
                    "wts.Page.Grid", "wts.Page.WebView", "wts.Page.MediaPlayer",
                    "wts.Page.TabbedPivot", "wts.Page.Map", "wts.Page.Camera",
                    "wts.Page.ImageGallery", "wts.Page.MasterDetail",
                    "wts.Feat.SettingsStorage", "wts.Feat.SuspendAndResume", "wts.Feat.LiveTile",
                    "wts.Feat.UriScheme", "wts.Feat.FirstRunPrompt", "wts.Feat.WhatsNewPrompt",
                    "wts.Feat.ToastNotifications", "wts.Feat.BackgroundTask", "wts.Feat.HubNotifications",
                    "wts.Feat.StoreNotifications", "wts.Feat.FeedbackHub", "wts.Feat.MultiView",
                    "wts.Feat.ShareSource", "wts.Feat.ShareTarget", "wts.Feat.UriScheme", "wts.Feat.WebToAppLink",
                    "wts.Feat.DragAndDrop"
                };
            }
        }

        // Need overload with different number of params to work with XUnit.MemeberData
        public static IEnumerable<object[]> GetProjectTemplatesForBuild(string framework)
        {
            return GetProjectTemplatesForBuild(framework, string.Empty, string.Empty);
        }

        // Set a single programming language to stop the fixture using all languages available to it
        public static IEnumerable<object[]> GetProjectTemplatesForBuild(string framework, string programmingLanguage, string platform)
        {
            IEnumerable<object[]> result = new List<object[]>();

            switch (framework)
            {
                case "CodeBehind":
                    result = BuildTemplatesTestFixture.GetProjectTemplates(framework, programmingLanguage, platform);
                    break;

                case "MVVMBasic":
                    result = BuildTemplatesTestFixture.GetProjectTemplates(framework, programmingLanguage, platform);
                    break;

                case "MVVMLight":
                    result = BuildTemplatesTestFixture.GetProjectTemplates(framework, programmingLanguage, platform);
                    break;

                case "CaliburnMicro":
                    result = BuildTemplatesTestFixture.GetProjectTemplates(framework, programmingLanguage, platform);
                    break;

                case "LegacyFrameworks":
                    result = BuildRightClickWithLegacyFixture.GetProjectTemplates();
                    break;

                case "Prism":
                    result = BuildTemplatesTestFixture.GetProjectTemplates(framework, programmingLanguage, platform);
                    break;

                default:
                    result = BuildFixture.GetProjectTemplates();
                    break;
            }

            return result;
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForBuild(string framework)
        {
            IEnumerable<object[]> result = new List<object[]>();

            switch (framework)
            {
                case "CodeBehind":
                    result = BuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework);
                    break;

                case "MVVMBasic":
                    result = BuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework);
                    break;

                case "MVVMLight":
                    result = BuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework);
                    break;

                case "CaliburnMicro":
                    result = BuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework);
                    break;

                case "Prism":
                    result = BuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework);
                    break;
            }

            return result;
        }
    }
}
