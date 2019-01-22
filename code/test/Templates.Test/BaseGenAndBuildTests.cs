// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.Core.Helpers;

namespace Microsoft.Templates.Test
{
    public class BaseGenAndBuildTests
    {
        protected BaseGenAndBuildFixture _fixture;

        public BaseGenAndBuildTests(BaseGenAndBuildFixture fixture, IContextProvider contextProvider = null, string framework = "")
        {
            _fixture = fixture;
            _fixture.InitializeFixture(contextProvider ?? new FakeContextProvider(), framework);
        }

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
                case "TabbedNav":
                    return "TN";
                default:
                    return projectType;
            }
        }

        protected static string GetProjectExtension(string language)
        {
            return language == ProgrammingLanguages.CSharp ? "csproj" : "vbproj";
        }

        protected async Task<(string projectName, string projectPath)> GenerateEmptyProjectAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                     && t.GetProjectTypeList().Contains(projectType)
                     && t.GetFrameworkList().Contains(framework)
                     && !t.GetIsHidden()
                     && t.GetLanguage() == language;

            var projectName = $"{ShortProjectType(projectType)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, null, null, false);

            return (projectName, projectPath);
        }

        protected async Task<string> AssertGenerateProjectAsync(Func<ITemplateInfo, bool> projectTemplateSelector, string projectName, string projectType, string framework, string platform, string language, Func<ITemplateInfo, bool> itemTemplatesSelector = null, Func<ITemplateInfo, string> getName = null, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            var targetProjectTemplate = _fixture.Templates().FirstOrDefault(projectTemplateSelector);
            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

            var userSelection = _fixture.SetupProject(projectType, framework, platform, language);

            if (getName != null && itemTemplatesSelector != null)
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

        protected void EnsureCanInferConfigInfo(string projectType, string framework, string platform, string projectPath)
        {
            RemoveProjectConfigInfoFromProject();

            AssertCorrectProjectConfigInfo(projectType, framework, platform);
            AssertProjectConfigInfoRecreated(projectType, framework, platform);

            Fs.SafeDeleteDirectory(projectPath);
        }

        protected void RemoveProjectConfigInfoFromProject()
        {
            var manifest = Path.Combine(GenContext.Current.DestinationPath, "Package.appxmanifest");
            var lines = File.ReadAllLines(manifest);
            var sb = new StringBuilder();
            var fx = $"genTemplate:Item Name=\"framework\"";
            var pt = $"genTemplate:Item Name=\"projectType\"";
            foreach (var line in lines)
            {
                if (!line.Contains(fx) && !line.Contains(pt))
                {
                    sb.AppendLine(line);
                }
            }

            File.Delete(manifest);
            File.WriteAllText(manifest, sb.ToString());
        }

        protected void AssertCorrectProjectConfigInfo(string expectedProjectType, string expectedFramework, string expectedPlatform)
        {
            var info = ProjectConfigInfo.ReadProjectConfiguration();

            Assert.Equal(expectedProjectType, info.ProjectType);
            Assert.Equal(expectedFramework, info.Framework);
            Assert.Equal(expectedPlatform, info.Platform);
        }

        protected void AssertProjectConfigInfoRecreated(string projectType, string framework, string platform)
        {
            var content = File.ReadAllText(Path.Combine(GenContext.Current.DestinationPath, "Package.appxmanifest"));
            var expectedFxText = $"Name=\"framework\" Value=\"{framework}\"";
            var expectedPtText = $"Name=\"projectType\" Value=\"{projectType}\"";
            var expectedPfText = $"Name=\"platform\" Value=\"{platform}\"";

            Assert.Contains(expectedFxText, content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedPtText, content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedPfText, content, StringComparison.OrdinalIgnoreCase);
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
            var path = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = path,
                GenerationOutputPath = path,
            };

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

            await AddRightClickTemplatesAsync(path, rightClickTemplates, projectName, projectType, framework, platform, language);

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

        protected async Task AddRightClickTemplatesAsync(string destinationPath, IEnumerable<ITemplateInfo> rightClickTemplates, string projectName, string projectType, string framework, string platform, string language)
        {
            // Add new items
            foreach (var item in rightClickTemplates)
            {
                GenContext.Current = new FakeContextProvider
                {
                    ProjectName = projectName,
                    DestinationPath = destinationPath,
                    GenerationOutputPath = GenContext.GetTempGenerationPath(projectName),
                };

                var newUserSelection = new UserSelection(projectType, framework, platform, language)
                {
                    HomeName = string.Empty,
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge,
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
            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

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
            return GenerationFixture.GetProjectTemplates();
        }

        public static IEnumerable<object[]> GetCSharpUwpProjectTemplatesForGenerationAsync()
        {
            var result = GenerationFixture.GetProjectTemplates();

            foreach (var item in result)
            {
                if (item[2].ToString() == Platforms.Uwp && item[3].ToString() == ProgrammingLanguages.CSharp)
                {
                    yield return item;
                }
            }
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpComparisonProjectAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false, bool includeMultipleInstances = true)
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

            var projectName = $"{projectType}{framework}{singlePageName}{ShortLanguageName(language)}";
            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

            var userSelection = _fixture.SetupProject(projectType, framework, Platforms.Uwp, language);

            foreach (var identity in genIdentitiesList)
            {
                ITemplateInfo itemTemplate = _fixture.Templates()
                                                     .FirstOrDefault(t => (t.Identity.StartsWith($"{identity}.") || t.Identity.Equals(identity))
                                                                        && t.GetFrameworkList().Contains(framework));

                _fixture.AddItem(userSelection, itemTemplate, BaseGenAndBuildFixture.GetDefaultName);

                // Add multiple pages if supported to check these are handled the same
                if (includeMultipleInstances && itemTemplate.GetMultipleInstance())
                {
                    _fixture.AddItem(userSelection, itemTemplate, BaseGenAndBuildFixture.GetDefaultName);
                }
            }

            if (lastPageIsHome)
            {
                // Useful if creating a blank project type and want to change the start page
                userSelection.HomeName = userSelection.Pages.Last().name;

                if (projectType == "TabbedNav")
                {
                    userSelection.Pages.Reverse();
                }
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, GenContext.Current.ProjectName);

            return (resultPath, GenContext.Current.ProjectName);
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGeneration(string framework)
        {
            var result = GenerationFixture.GetPageAndFeatureTemplatesForGeneration(framework);
            return result;
        }

        private const string NavigationPanel = "SplitView";
        private const string Blank = "Blank";
        private const string TabbedNav = "TabbedNav";
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
            yield return new object[] { TabbedNav, CodeBehind };
            yield return new object[] { TabbedNav, MvvmBasic };
            yield return new object[] { TabbedNav, MvvmLight };
        }

        // Gets a list of partial identities for page and feature templates supported by C# and VB
        protected static IEnumerable<string> GetPagesAndFeaturesForMultiLanguageProjects()
        {
            return new[]
            {
                "wts.Page.Blank", "wts.Page.Settings", "wts.Page.Chart",
                "wts.Page.Grid", "wts.Page.WebView", "wts.Page.MediaPlayer",
                "wts.Page.TabbedPivot", "wts.Page.Map", "wts.Page.Camera",
                "wts.Page.ImageGallery", "wts.Page.MasterDetail",
                "wts.Page.InkDraw", "wts.Page.InkDrawPicture", "wts.Page.InkSmartCanvas",
                "wts.Page.ContentGrid", "wts.Page.DataGrid",
                "wts.Feat.SettingsStorage", "wts.Feat.SuspendAndResume", "wts.Feat.LiveTile",
                "wts.Feat.DeepLinking", "wts.Feat.FirstRunPrompt", "wts.Feat.WhatsNewPrompt",
                "wts.Feat.ToastNotifications", "wts.Feat.BackgroundTask", "wts.Feat.HubNotifications",
                "wts.Feat.StoreNotifications", "wts.Feat.FeedbackHub", "wts.Feat.MultiView",
                "wts.Feat.ShareSource", "wts.Feat.ShareTarget", "wts.Feat.WebToAppLink", "wts.Feat.DragAndDrop",
            };
        }

        // Need overload with different number of params to work with XUnit.MemberData
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
