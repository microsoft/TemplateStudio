// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.UI.Launcher;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.Utilities.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickActions : IContextProvider
    {
        private readonly IEnumerable<RightClickAvailability> _availableOptions = new List<RightClickAvailability>()
        {
            new RightClickAvailability(Platforms.Uwp, ProgrammingLanguages.CSharp) { TemplateTypes = new List<TemplateType>() { TemplateType.Page, TemplateType.Feature, TemplateType.Service, TemplateType.Testing } },
            new RightClickAvailability(Platforms.Uwp, ProgrammingLanguages.VisualBasic) { TemplateTypes = new List<TemplateType>() { TemplateType.Page, TemplateType.Feature, TemplateType.Service, TemplateType.Testing } },
            new RightClickAvailability(Platforms.Wpf, ProgrammingLanguages.CSharp) { TemplateTypes = new List<TemplateType>() { TemplateType.Page, TemplateType.Feature, TemplateType.Testing } },
            new RightClickAvailability(Platforms.WinUI, ProgrammingLanguages.CSharp, AppModels.Desktop) { TemplateTypes = new List<TemplateType>() { TemplateType.Page, TemplateType.Feature } },
            new RightClickAvailability(Platforms.WinUI, ProgrammingLanguages.CSharp, AppModels.Uwp) { TemplateTypes = new List<TemplateType>() { TemplateType.Page } },
            new RightClickAvailability(Platforms.WinUI, ProgrammingLanguages.Cpp, AppModels.Desktop) { TemplateTypes = new List<TemplateType>() { TemplateType.Page } },
            new RightClickAvailability(Platforms.WinUI, ProgrammingLanguages.Cpp, AppModels.Uwp) { TemplateTypes = new List<TemplateType>() { TemplateType.Page } },
        };

        private readonly GenerationService _generationService = GenerationService.Instance;

        private static VsGenShell _shell;

        public string ProjectName { get; private set; }

        public string SafeProjectName { get; private set; }

        public string GenerationOutputPath { get; private set; }

        public string DestinationPath { get; private set; }

        public ProjectInfo ProjectInfo { get; private set; }

        public List<string> FilesToOpen { get; private set; }

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; private set; }

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; private set; }

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; private set; }

        public RightClickActions()
        {
            _shell = new VsGenShell();
        }

        public void AddNewPage()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                SetContext();
                var userSelection = WizardLauncher.Instance.StartAddTemplate(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider(), TemplateType.Page, WizardTypeEnum.AddPage);
                var statusBarMessage = string.Format(StringRes.StatusBarNewItemAddPageSuccess, userSelection.Pages[0].Name);
                FinishGeneration(userSelection, statusBarMessage);
            }
            catch (WizardBackoutException)
            {
                _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddPageCancelled);
            }
        }

        public void AddNewFeature()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                SetContext();
                var userSelection = WizardLauncher.Instance.StartAddTemplate(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider(), TemplateType.Feature, WizardTypeEnum.AddFeature);
                var statusBarMessage = string.Format(StringRes.StatusBarNewItemAddFeatureSuccess, userSelection.Features[0].Name);
                FinishGeneration(userSelection, statusBarMessage);
            }
            catch (WizardBackoutException)
            {
                _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddFeatureCancelled);
            }
        }

        public void AddNewService()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                SetContext();
                var userSelection = WizardLauncher.Instance.StartAddTemplate(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider(), TemplateType.Service, WizardTypeEnum.AddService);
                var statusBarMessage = string.Format(StringRes.StatusBarNewItemAddServiceSuccess, userSelection.Services[0].Name);
                FinishGeneration(userSelection, statusBarMessage);
            }
            catch (WizardBackoutException)
            {
                _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddServiceCancelled);
            }
        }

        public void AddNewTesting()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                SetContext();
                var userSelection = WizardLauncher.Instance.StartAddTemplate(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider(), TemplateType.Testing, WizardTypeEnum.AddTesting);
                var statusBarMessage = string.Format(StringRes.StatusBarNewItemAddTestingSuccess, userSelection.Testing[0].Name);
                FinishGeneration(userSelection, statusBarMessage);
            }
            catch (WizardBackoutException)
            {
                _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddTestingCancelled);
            }
        }

        public bool Visible(TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!_shell.GetActiveProjectIsWts())
            {
                return false;
            }

            var projectConfigInfoService = new ProjectConfigInfoService(_shell);
            var configInfo = projectConfigInfoService.ReadProjectConfiguration();

            var rightClickOptions = _availableOptions.FirstOrDefault(o =>
                                        o.Platform == configInfo.Platform &&
                                        o.Language == projectConfigInfoService.GetProgrammingLanguage() &&
                                        o.AppModel == configInfo.AppModel);

            return rightClickOptions != null ? rightClickOptions.TemplateTypes.Contains(templateType) : false;
        }

        public bool Visible()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return _shell.GetActiveProjectIsWts();
        }

        public bool Enabled()
        {
            return !_shell.IsDebuggerEnabled() && !_shell.IsBuildInProgress();
        }

        public void OpenTempFolder()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var tempPath = GetTempGenerationFolder();
            if (HasContent(tempPath))
            {
                System.Diagnostics.Process.Start(tempPath);
            }
        }

        public bool TempFolderAvailable()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return HasContent(GetTempGenerationFolder());
        }

        private void SetContext()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            EnsureGenContextInitialized();
            var language = _shell.GetActiveProjectLanguage();
            if (GenContext.CurrentLanguage == language)
            {
                GenContext.Current = this;

                DestinationPath = GenContext.ToolBox.Shell.GetActiveProjectPath();
                ProjectName = GenContext.ToolBox.Shell.GetActiveProjectName();
                SafeProjectName = GenContext.ToolBox.Shell.GetActiveProjectNamespace();

                GenerationOutputPath = GenContext.GetTempGenerationPath(ProjectName);
                ProjectInfo = new ProjectInfo();
                FilesToOpen = new List<string>();
                FailedMergePostActions = new List<FailedMergePostActionInfo>();
                MergeFilesFromProject = new Dictionary<string, List<MergeInfo>>();
                ProjectMetrics = new Dictionary<ProjectMetricsEnum, double>();
            }
        }

        protected void FinishGeneration(UserSelection userSelection, string statusBarMessage)
        {
            SafeThreading.JoinableTaskFactory.Run(
            async () =>
            {
                await FinishGenerationAsync(userSelection, statusBarMessage);
            },
            JoinableTaskCreationOptions.LongRunning);
        }

        protected async System.Threading.Tasks.Task FinishGenerationAsync(UserSelection userSelection, string statusBarMessage)
        {
            if (userSelection is null)
            {
                return;
            }

            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            _generationService.FinishGeneration(userSelection);
            await _shell.ShowStatusBarMessageAsync(statusBarMessage);
        }

        private bool EnsureGenContextInitialized()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var projectLanguage = _shell.GetActiveProjectLanguage();
            var projectPlatform = ProjectMetadataService.GetProjectMetadata(_shell.GetActiveProjectPath()).Platform;

            if (!string.IsNullOrEmpty(projectLanguage) && !string.IsNullOrEmpty(projectPlatform))
            {
                if (GenContext.CurrentLanguage != projectLanguage || GenContext.CurrentPlatform != projectPlatform)
                {
#if DEBUG
                    GenContext.Bootstrap(new LocalTemplatesSource(string.Empty), _shell, projectPlatform, projectLanguage);
#else
                    var mstxFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");
                    GenContext.Bootstrap(new RemoteTemplatesSource(projectPlatform, projectLanguage, mstxFilePath, new DigitalSignatureService()), _shell, projectPlatform, projectLanguage);
#endif
                }

                return true;
            }

            return false;
        }

        private static string GetTempGenerationFolder()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string projectGuid = _shell.GetProjectGuidByName(_shell.GetActiveProjectName()).ToString();
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, projectGuid);
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }
    }
}
