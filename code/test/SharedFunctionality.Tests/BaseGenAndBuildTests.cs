// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI.Services;
using Xunit;


namespace Microsoft.Templates.Test
{
    public class BaseGenAndBuildTests
    {
        protected BaseGenAndBuildFixture _fixture;
        protected const string All = "all";

        protected List<string> excludedTemplates_Uwp_Group1 = new List<string>() { "ts.Service.IdentityOptionalLogin", "ts.Feat.MultiInstanceAdvanced", "ts.Feat.MultiInstance" };
        protected List<string> excludedTemplates_Uwp_Group2 = new List<string>() { "ts.Service.IdentityForcedLogin", "ts.Feat.BackgroundTask" }; // Add multiinstance templates on this group if possible
        protected List<string> excludedTemplates_Wpf_Group1 = new List<string>() { "ts.WPF.Service.IdentityOptionalLogin" };
        protected List<string> excludedTemplates_Wpf_Group2 = new List<string>() { "ts.WPF.Service.IdentityForcedLogin" }; // Add multiinstance templates on this group if possible
        protected List<string> excludedTemplatesGroup1VB = new List<string>() { "ts.Service.IdentityOptionalLogin.VB", "ts.Feat.MultiInstanceAdvanced.VB", "ts.Feat.MultiInstance.VB" };
        protected List<string> excludedTemplatesGroup2VB = new List<string>() { "ts.Service.IdentityForcedLogin.VB", "ts.Feat.BackgroundTask.VB" }; // Add multiinstance templates on this group if possible

        public BaseGenAndBuildTests(BaseGenAndBuildFixture fixture, IContextProvider contextProvider = null, string framework = "")
        {
            _fixture = fixture;
            _fixture.InitializeFixture(contextProvider ?? new FakeContextProvider(), framework);
        }

        protected static string ShortLanguageName(string language)
        {
            switch (language)
            {
                case ProgrammingLanguages.VisualBasic:
                    return "VB";
                case ProgrammingLanguages.Cpp:
                    return "CPP";
                case ProgrammingLanguages.CSharp:
                default:
                    return "CS";
            }
        }

        // Used to create names that include a number of characters that are valid in project names but have the potential to cause issues
        protected static string CharactersThatMayCauseProjectNameIssues()
        {
            // $ is technically valid in a project name but cannot be used with WTS as it is used as an identifier in global post action file names.
            // ^ is technically valid in project names but Visual Studio cannot open files with this in the path
            // ' is technically valid in project names but breaks test projects if used in the name so don't test for it
            // , is technically valid in project names but breaks vb test projects in VS 2019 if used in the name so don't test for it
            // £ is not valid in C++ projects
            return " -_.@! ()+=";
        }

        protected static string ShortProjectType(string projectType)
        {
            switch (projectType)
            {
                case ProjectTypes.Blank:
                    return "B";
                case ProjectTypes.SplitView:
                    return "SV";
                case ProjectTypes.TabbedNav:
                    return "TN";
                case ProjectTypes.MenuBar:
                    return "MB";
                case ProjectTypes.Ribbon:
                    return "RB";
                default:
                    return projectType;
            }
        }

        protected static string GetProjectExtension(string language)
        {
            switch (language)
            {
                case ProgrammingLanguages.CSharp:
                    return "csproj";
                case ProgrammingLanguages.VisualBasic:
                    return "vbproj";
                case ProgrammingLanguages.Cpp:
                    return "vcxproj";
                default:
                    return string.Empty;
            }
        }

        protected async Task<(string projectName, string projectPath)> GenerateEmptyProjectAsync(UserSelectionContext context)
        {
            var projectName = $"{ShortProjectType(context.ProjectType)}Empty{ShortLanguageName(context.Language)}";

            var projectPath = await AssertGenerateProjectAsync(projectName, context, null, null);

            return (projectName, projectPath);
        }

        protected async Task<(string projectName, string projectPath)> GenerateAllPagesAndFeaturesAsync(UserSelectionContext context)
        {
            //Microsoft.TemplateEngine.Utils.TemplateInfoExtensions
            // get first item from each exclusive selection group
             IEnumerable<ITemplateInfo> foo = GenContext.ToolBox.Repo.GetAll();
            var exclusiveSelectionGroups = foo.Where(t =>
                t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(context.ProjectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(context.FrontEndFramework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == context.Platform
                && t.GetIsGroupExclusiveSelection()).GroupBy(t => t.GetGroup(), (key, g) => g.First());

            // this selector excludes templates with exclusions
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(context.ProjectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(context.FrontEndFramework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == context.Platform
                && t.GetExclusionsList().Count == 0
                && (!t.GetIsGroupExclusiveSelection() || (t.GetIsGroupExclusiveSelection() && exclusiveSelectionGroups.Contains(t)))
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(context.ProjectType)}All{ShortLanguageName(context.Language)}";

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            return (projectName, projectPath);
        }

        protected async Task<string> AssertGenerateProjectAsync(string projectName, UserSelectionContext context, Func<ITemplateInfo, bool> itemTemplatesSelector = null, Func<TemplateInfo, string> getName = null, bool includeMultipleInstances = false, IEnumerable<ITemplateInfo> additionalTemplates = null)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(context.Language);
            BaseGenAndBuildFixture.SetCurrentPlatform(context.Platform);

            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

            var userSelection = _fixture.SetupProject(context);

            if (getName != null && itemTemplatesSelector != null)
            {
                var itemTemplates = _fixture.Templates().Where(itemTemplatesSelector).ToList();

                if (additionalTemplates != null)
                {
                    itemTemplates.AddRange(additionalTemplates);
                }

                var itemsTemplateInfo = GenContext.ToolBox.Repo.GetTemplatesInfo(itemTemplates, context);
                _fixture.AddItems(userSelection, itemsTemplateInfo, getName, includeMultipleInstances);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Length > 2);

            if (context.Platform == Platforms.Uwp)
            {
                AssertCorrectProjectConfigInfo(context);
            }

            return resultPath;
        }

        protected IEnumerable<ITemplateInfo> GetAdditionalTemplates(string mountpoint)
        {
            // The following relies on the TemplateRepository having been initilized - but that should be done during fixture initialization.
            try
            {
                //// Doing this here rather than as a part of the downloading & checking for new templates flow
                var scanResult = CodeGen.Instance.Scanner.Scan(mountpoint);

                return scanResult.Templates;
                //var scanResult = Task.Run(async () => await CodeGen.Instance.Scanner.ScanAsync(mountpoint)).Result;

                //var list = new List<ITemplateInfo>();
                //foreach (var q in scanResult.Templates)
                //{
                //    var b = Microsoft.TemplateEngine.Utils.IScanTemplateInfoExtensions.ToITemplateInfo(q);
                //    list.Add(b);
                //}
                //return list;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw;
            }
        }

        protected void EnsureCanInferConfigInfo(UserSelectionContext context, string projectPath)
        {
            RemoveProjectConfigInfoFromProject();

            AssertCorrectProjectConfigInfo(context);
            AssertProjectConfigInfoRecreated(context);

            Fs.SafeDeleteDirectory(projectPath);
        }

        protected void RemoveProjectConfigInfoFromProject()
        {
            var metadataFileNames = new List<string>() { "Package.appxmanifest", "TemplateStudio.xml" };
            var metadataFile = metadataFileNames.FirstOrDefault(fileName => File.Exists(Path.Combine(GenContext.Current.DestinationPath, fileName)));
            var metadataFilePath = Path.Combine(GenContext.Current.DestinationPath, metadataFile);

            var lines = File.ReadAllLines(metadataFilePath);
            var sb = new StringBuilder();
            var pl = $"genTemplate:Item Name=\"platform\"";
            var fx = $"genTemplate:Item Name=\"framework\"";
            var pt = $"genTemplate:Item Name=\"projectType\"";
            var am = $"genTemplate:Item Name=\"appmodel\"";
            foreach (var line in lines)
            {
                if (!line.Contains(fx) && !line.Contains(pt) && !line.Contains(pl) && !line.Contains(am))
                {
                    sb.AppendLine(line);
                }
            }

            File.Delete(metadataFilePath);
            File.WriteAllText(metadataFilePath, sb.ToString(), Encoding.UTF8);
        }

        protected void AssertCorrectProjectConfigInfo(UserSelectionContext context)
        {
            var projectConfigInfoService = new ProjectConfigInfoService(GenContext.ToolBox.Shell);
            var info = projectConfigInfoService.ReadProjectConfiguration();

            Assert.Equal(context.ProjectType, info.ProjectType);
            Assert.Equal(context.FrontEndFramework, info.Framework);
            Assert.Equal(context.Platform, info.Platform);
            if (!string.IsNullOrEmpty(context.GetAppModel()))
            {
                Assert.Equal(context.GetAppModel(), info.AppModel);
            }
        }

        protected void AssertProjectConfigInfoRecreated(UserSelectionContext context)
        {
            var metadataFileNames = new List<string>() { "Package.appxmanifest", "TemplateStudio.xml" };
            var metadataFile = metadataFileNames.FirstOrDefault(fileName => File.Exists(Path.Combine(GenContext.Current.DestinationPath, fileName)));
            var metadataFilePath = Path.Combine(GenContext.Current.DestinationPath, metadataFile);

            var content = File.ReadAllText(metadataFilePath);
            var expectedFxText = $"Name=\"framework\" Value=\"{context.FrontEndFramework}\"";
            var expectedPtText = $"Name=\"projectType\" Value=\"{context.ProjectType}\"";
            var expectedPfText = $"Name=\"platform\" Value=\"{context.Platform}\"";

            Assert.Contains(expectedFxText, content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedPtText, content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedPfText, content, StringComparison.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(context.GetAppModel()))
            {
                var expectedAmText = $"Name=\"appmodel\" Value=\"{context.GetAppModel()}\"";
                Assert.Contains(expectedAmText, content, StringComparison.OrdinalIgnoreCase);
            }
        }

        protected void AssertBuildProject(string projectPath, string projectName, string platform, string language = "", bool deleteAfterBuild = true)
        {
            (int exitCode, string outputFile) result = (1, string.Empty);

            // Build solution
            switch (platform)
            {
                case Platforms.Uwp:
                    result = _fixture.BuildSolutionUwp(projectName, projectPath, platform);
                    break;
                case Platforms.WinUI:
                    result = _fixture.BuildSolutionWinUI(projectName, projectPath, platform, language);
                    break;
                case Platforms.Wpf:
                    result = _fixture.BuildSolutionWpf(projectName, projectPath, platform);
                    break;
            }

            // Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            if (deleteAfterBuild)
            {
                Fs.SafeDeleteDirectory(projectPath);
            }
        }

        protected void AssertBuildProjectWpfWithMsix(string projectPath, string projectName, string platform, bool deleteAfterBuild = true)
        {
            // Build solution
            var (exitCode, outputFile) = _fixture.BuildSolutionWpfWithMsix(projectName, projectPath, platform);

            // Assert
            Assert.True(exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(outputFile)} for more details.");

            // Clean
            if (deleteAfterBuild)
            {
                Fs.SafeDeleteDirectory(projectPath);
            }
        }

        protected void AssertBuildProjectThenRunTests(string projectPath, string projectName, string platform)
        {
            var (buildExitCode, buildOutputFile) = _fixture.BuildSolutionUwp(projectName, projectPath, platform);

            if (buildExitCode.Equals(0))
            {
                var (testExitCode, testOutputFile) = _fixture.RunTests(projectName, projectPath);

                var summary = _fixture.GetTestSummary(testOutputFile);

                Assert.True(
                    summary.Contains("Failed: 0.") || !summary.Contains("Failed"),
                    $"Tests failed. {Environment.NewLine}{summary}{Environment.NewLine}Please see {Path.GetFullPath(buildOutputFile)} for more details.");
            }
            else
            {
                Assert.True(buildExitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(buildOutputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(buildOutputFile)} for more details.");
            }

            // Tidy up if all tests passed
            Fs.SafeDeleteDirectory(projectPath);
        }

        protected async Task<string> AssertGenerateRightClickAsync(string projectName, UserSelectionContext context, bool emptyProject, List<string> excludedGroupIdentity = null)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(context.Language);
            BaseGenAndBuildFixture.SetCurrentPlatform(context.Platform);
            var path = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = path,
                GenerationOutputPath = path,
            };

            var userSelection = _fixture.SetupProject(context);
            var appModel = context.GetAppModel();

            if (!emptyProject)
            {
                var templates = _fixture.Templates().Where(
                    t => t.GetTemplateType().IsItemTemplate()
                    && (t.GetProjectTypeList().Contains(context.ProjectType) || t.GetProjectTypeList().Contains(All))
                    && (t.GetFrontEndFrameworkList().Contains(context.FrontEndFramework) || t.GetFrontEndFrameworkList().Contains(All))
                    && t.GetPlatform() == context.Platform
                    && (string.IsNullOrEmpty(appModel) || t.GetPropertyBagValuesList("appmodel").Contains(appModel) || t.GetPropertyBagValuesList("appmodel").Contains(All))
                    && (excludedGroupIdentity == null || (!excludedGroupIdentity.Contains(t.GroupIdentity)))
                    && !t.GetIsHidden());

                var templatesInfo = GenContext.ToolBox.Repo.GetTemplatesInfo(templates, context);

                _fixture.AddItems(userSelection, templatesInfo, BaseGenAndBuildFixture.GetDefaultName);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var project = Path.Combine(_fixture.TestNewItemPath, projectName);

            // Assert on project
            Assert.True(Directory.Exists(project));

            int emptyProjecFileCount = Directory.GetFiles(project, "*.*", SearchOption.AllDirectories).Length;
            Assert.True(emptyProjecFileCount > 2);

            var rightClickTemplates = _fixture.Templates().Where(
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(context.ProjectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(context.FrontEndFramework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == context.Platform
                && (string.IsNullOrEmpty(appModel) || t.GetPropertyBagValuesList("appmodel").Contains(appModel) || t.GetPropertyBagValuesList("appmodel").Contains(All))
                && !t.GetIsHidden()
                && (excludedGroupIdentity == null || (!excludedGroupIdentity.Contains(t.GroupIdentity)))
                && t.GetRightClickEnabled());

            await AddRightClickTemplatesAsync(path, rightClickTemplates, projectName, context.ProjectType, context.FrontEndFramework, context.Platform, context.Language);

            var finalProjectPath = Path.Combine(_fixture.TestNewItemPath, projectName);
            int finalProjectFileCount = Directory.GetFiles(finalProjectPath, "*.*", SearchOption.AllDirectories).Length;

            if (emptyProject)
            {
                Assert.True(finalProjectFileCount > emptyProjecFileCount);
            }
            else
            {
                Assert.True(finalProjectFileCount == emptyProjecFileCount);
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

                var context = new UserSelectionContext(language, platform)
                {
                    ProjectType = projectType,
                    FrontEndFramework = framework,
                };

                var newUserSelection = new UserSelection(context)
                {
                    HomeName = string.Empty,
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge,
                };

                var templateInfo = GenContext.ToolBox.Repo.GetTemplateInfo(item, context);

                _fixture.AddItem(newUserSelection, templateInfo, BaseGenAndBuildFixture.GetDefaultName);

                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(item.GetTemplateType(), newUserSelection);

                NewItemGenController.Instance.UnsafeFinishGeneration(newUserSelection);
            }
        }

        protected async Task<(string ProjectPath, string ProjecName)> AssertGenerationOneByOneAsync(string itemName, UserSelectionContext context, string itemId, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(context.Language);
            BaseGenAndBuildFixture.SetCurrentPlatform(context.Platform);

            var itemTemplate = _fixture.Templates().FirstOrDefault(t => t.Identity == itemId);
            var finalName = itemTemplate.GetDefaultName();

            if (itemTemplate.GetItemNameEditable())
            {
                var nameValidationService = new ItemNameService(GenContext.ToolBox.Repo.ItemNameValidationConfig, () => new string[] { });
                finalName = nameValidationService.Infer(finalName);
            }

            var projectName = $"{ShortProjectType(context.ProjectType)}{finalName}{ShortLanguageName(context.Language)}";
            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

            var userSelection = _fixture.SetupProject(context);
            var templateInfo = GenContext.ToolBox.Repo.GetTemplateInfo(itemTemplate, context);

            _fixture.AddItem(userSelection, templateInfo, BaseGenAndBuildFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Length > 2);

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(resultPath);
            }

            return (resultPath, projectName);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForGeneration()
        {
            return GenerationFixture.GetProjectTemplates();
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpComparisonProjectAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false, bool includeMultipleInstances = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(Platforms.Uwp);

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

            var context = new UserSelectionContext(language, Platforms.Uwp)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };

            var userSelection = _fixture.SetupProject(context);

            foreach (var identity in genIdentitiesList)
            {
                var itemTemplate = _fixture.Templates().FirstOrDefault(t
                    => (t.Identity.StartsWith($"{identity}.", StringComparison.Ordinal) || t.Identity.Equals(identity, StringComparison.Ordinal))
                    && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                    && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All)));

                var templateInfo = GenContext.ToolBox.Repo.GetTemplateInfo(itemTemplate, context);
                _fixture.AddItem(userSelection, templateInfo, BaseGenAndBuildFixture.GetDefaultName);

                // Add multiple pages if supported to check these are handled the same
                if (includeMultipleInstances && templateInfo.MultipleInstance)
                {
                    _fixture.AddItem(userSelection, templateInfo, BaseGenAndBuildFixture.GetDefaultName);
                }
            }

            if (lastPageIsHome)
            {
                // Useful if creating a blank project type and want to change the start page
                userSelection.HomeName = userSelection.Pages.Last().Name;

                if (projectType == ProjectTypes.TabbedNav)
                {
                    userSelection.Pages.Reverse();
                }
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, GenContext.Current.ProjectName);

            return (resultPath, GenContext.Current.ProjectName);
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpWpfComparisonProjectAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(Platforms.Wpf);

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

            var context = new UserSelectionContext(language, Platforms.Wpf)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };

            var userSelection = _fixture.SetupProject(context);

            foreach (var identity in genIdentitiesList)
            {
                var itemTemplate = _fixture.Templates().FirstOrDefault(t
                    => (t.Identity.StartsWith($"{identity}.", StringComparison.Ordinal) || t.Identity.Equals(identity, StringComparison.Ordinal))
                    && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                    && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All)));

                var templateInfo = GenContext.ToolBox.Repo.GetTemplateInfo(itemTemplate, context);
                _fixture.AddItem(userSelection, templateInfo, BaseGenAndBuildFixture.GetDefaultName);
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

        // Gets a list of partial identities for page and feature templates supported by C# and VB
        protected static IEnumerable<string> GetTemplatesThatDoNotSupportVB()
        {
            return new[]
            {
                "ts.Service.WebApi",
                "ts.Service.SecuredWebApi",
                "ts.Service.SecuredWebApi.CodeBehind",
            };
        }

        // Set a single programming language to stop the fixture using all languages available to it
        public static IEnumerable<object[]> GetProjectTemplatesForBuild(string framework, string programmingLanguage, string platform)
        {
            IEnumerable<object[]> result = new List<object[]>();

            // This returns nothing as expected to be overridden in descendents

            return result;
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForBuild(string framework, string language, string platform, string excludedItem = "")
        {
            IEnumerable<object[]> result = new List<object[]>();

            // This returns nothing as expected to be overridden in descendents

            return result;
        }
    }
}
