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

        protected static string GetProjectExtension(string language)
        {
            return language == ProgrammingLanguages.CSharp ? "csproj" : "vbproj";
        }

        protected async Task<string> AssertGenerateProjectAsync(Func<ITemplateInfo, bool> projectTemplateSelector, string projectName, string projectType, string framework, string language, Func<ITemplateInfo, string> getName = null, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);

            var targetProjectTemplate = _fixture.Templates().FirstOrDefault(projectTemplateSelector);

            ProjectName = projectName;

            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = _fixture.SetupProject(projectType, framework, language, getName);

            if (getName != null)
            {
                _fixture.AddItems(userSelection, _fixture.GetTemplates(framework), getName);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Count() > 2);
            AssertCorrectProjectConfigInfo(projectType, framework);

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(resultPath);
            }

            return resultPath;
        }

        protected void AssertCorrectProjectConfigInfo(string expectedProjectType, string expectedFramework)
        {
            var info = ProjectConfigInfo.ReadProjectConfiguration();

            Assert.Equal(expectedProjectType, info.ProjectType);
            Assert.Equal(expectedFramework, info.Framework);
        }

        protected void AssertBuildProjectAsync(string projectPath, string projectName)
        {
            // Build solution
            var result = _fixture.BuildSolution(projectName, projectPath);

            // Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            Fs.SafeDeleteDirectory(projectPath);
        }

        protected async Task<string> AssertGenerateRightClickAsync(string projectName, string projectType, string framework, string language, bool emptyProject, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = _fixture.SetupProject(projectType, framework, language);

            if (!emptyProject)
            {
                _fixture.AddItems(userSelection, _fixture.GetTemplates(framework), BaseGenAndBuildFixture.GetDefaultName);
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
                                             && !t.GetIsHidden()
                                             && t.GetRightClickEnabled());

            await AddRightClickTemplatesAsync(rightClickTemplates, projectName, projectType, framework, language);

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

        protected async Task AddRightClickTemplatesAsync(IEnumerable<ITemplateInfo> rightClickTemplates, string projectName, string projectType, string framework, string language)
        {
            // Add new items
            foreach (var item in rightClickTemplates)
            {
                OutputPath = GenContext.GetTempGenerationPath(projectName);

                var newUserSelection = new UserSelection(projectType, framework, language)
                {
                    HomeName = string.Empty,
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge
                };

                _fixture.AddItem(newUserSelection, item, BaseGenAndBuildFixture.GetDefaultName);

                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(item.GetTemplateType(), newUserSelection);

                NewItemGenController.Instance.UnsafeFinishGeneration(newUserSelection);
            }
        }

        protected async Task<(string ProjectPath, string ProjecName)> AssertGenerationOneByOneAsync(string itemName, string projectType, string framework, string itemId, string language, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);

            var projectTemplate = _fixture.Templates().FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));
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

            var projectName = $"{projectType}{finalName}{ShortLanguageName(language)}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = _fixture.SetupProject(projectType, framework, language);

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
            JoinableTaskContext context = new JoinableTaskContext();
            JoinableTaskCollection tasks = context.CreateCollection();
            context.CreateFactory(tasks);
            var result = context.Factory.Run(() => GenerationFixture.GetProjectTemplatesAsync());

            return result;
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpComparisonProjectAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);

            var projectTemplate = _fixture.Templates().FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));

            var singlePageName = string.Empty;

            var genIdentitiesList = genIdentities.ToList();

            if (genIdentitiesList.Count == 1)
            {
                singlePageName = genIdentitiesList.Last().Split('.').Last();
            }

            ProjectName = $"{projectType}{framework}Compare{singlePageName}{ShortLanguageName(language)}";
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, ProjectName, ProjectName);
            OutputPath = ProjectPath;

            var userSelection = _fixture.SetupProject(projectType, framework, language);

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

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGenerationAsync(string framework)
        {
            JoinableTaskContext context = new JoinableTaskContext();
            JoinableTaskCollection tasks = context.CreateCollection();
            context.CreateFactory(tasks);
            var result = context.Factory.Run(() => GenerationFixture.GetPageAndFeatureTemplatesAsync(framework));
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
        public static IEnumerable<object[]> GetProjectTemplatesForBuildAsync(string framework)
        {
            return GetProjectTemplatesForBuildAsync(framework, string.Empty);
        }

        // Set a single programming language to stop the fixture using all languages available to it
        public static IEnumerable<object[]> GetProjectTemplatesForBuildAsync(string framework, string programmingLanguage)
        {
            JoinableTaskContext context = new JoinableTaskContext();
            JoinableTaskCollection tasks = context.CreateCollection();
            context.CreateFactory(tasks);
            IEnumerable<object[]> result = new List<object[]>();

            switch (framework)
            {
                case "CodeBehind":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetProjectTemplatesAsync(framework, programmingLanguage));
                    break;

                case "MVVMBasic":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetProjectTemplatesAsync(framework, programmingLanguage));
                    break;

                case "MVVMLight":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetProjectTemplatesAsync(framework, programmingLanguage));
                    break;

                case "CaliburnMicro":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetProjectTemplatesAsync(framework, programmingLanguage));
                    break;

                case "LegacyFrameworks":
                    result = context.Factory.Run(() => BuildRightClickWithLegacyFixture.GetProjectTemplatesAsync());
                    break;
                case "Prism":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetProjectTemplatesAsync(framework, programmingLanguage));
                    break;
                default:
                    result = context.Factory.Run(() => BuildFixture.GetProjectTemplatesAsync());
                    break;
            }

            return result;
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForBuildAsync(string framework)
        {
            JoinableTaskContext context = new JoinableTaskContext();
            JoinableTaskCollection tasks = context.CreateCollection();
            context.CreateFactory(tasks);
            IEnumerable<object[]> result = new List<object[]>();

            switch (framework)
            {
                case "CodeBehind":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetPageAndFeatureTemplatesAsync(framework));
                    break;

                case "MVVMBasic":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetPageAndFeatureTemplatesAsync(framework));
                    break;

                case "MVVMLight":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetPageAndFeatureTemplatesAsync(framework));
                    break;

                case "CaliburnMicro":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetPageAndFeatureTemplatesAsync(framework));
                    break;

                case "Prism":
                    result = context.Factory.Run(() => BuildTemplatesTestFixture.GetPageAndFeatureTemplatesAsync(framework));
                    break;
            }

            return result;
        }
    }
}
