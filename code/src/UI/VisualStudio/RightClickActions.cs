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
using Microsoft.Templates.UI.Launcher;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.Utilities.Services;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickActions : IContextProvider
    {
        private static VsGenShell _shell;

        private GenerationService _generationService = GenerationService.Instance;

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
            if (!_shell.GetActiveProjectIsWts())
            {
                return;
            }

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
            if (!_shell.GetActiveProjectIsWts())
            {
                return;
            }

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
            if (!_shell.GetActiveProjectIsWts())
            {
                return;
            }

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
            if (!_shell.GetActiveProjectIsWts())
            {
                return;
            }

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
            return _shell.GetActiveProjectIsWts() && GenContext.ToolBox.Repo.GetAll().Any(t => t.GetTemplateType() == templateType);
        }

        public bool Visible()
        {
            return _shell.GetActiveProjectIsWts();
        }

        public bool Enabled()
        {
            return !_shell.IsDebuggerEnabled() && !_shell.IsBuildInProgress();
        }

        public void OpenTempFolder()
        {
            var tempPath = GetTempGenerationFolder();
            if (HasContent(tempPath))
            {
                System.Diagnostics.Process.Start(tempPath);
            }
        }

        public bool TempFolderAvailable()
        {
            return HasContent(GetTempGenerationFolder());
        }

        private void SetContext()
        {
            EnsureGenContextInitialized();
            if (GenContext.CurrentLanguage == _shell.GetActiveProjectLanguage())
            {
                var projectConfig = ProjectConfigInfoService.ReadProjectConfiguration();
                DestinationPath = GenContext.ToolBox.Shell.GetActiveProjectPath();
                ProjectName = GenContext.ToolBox.Shell.GetActiveProjectName();
                SafeProjectName = GenContext.ToolBox.Shell.GetActiveProjectNamespace();

                GenerationOutputPath = GenContext.GetTempGenerationPath(ProjectName);
                ProjectInfo = new ProjectInfo();
                FilesToOpen = new List<string>();
                FailedMergePostActions = new List<FailedMergePostActionInfo>();
                MergeFilesFromProject = new Dictionary<string, List<MergeInfo>>();
                ProjectMetrics = new Dictionary<ProjectMetricsEnum, double>();

                GenContext.Current = this;
            }
        }

        private void FinishGeneration(UserSelection userSelection, string statusBarMessage)
        {
            if (userSelection is null)
            {
                return;
            }

            SafeThreading.JoinableTaskFactory.Run(
            async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _generationService.FinishGeneration(userSelection);
                _shell.ShowStatusBarMessage(statusBarMessage);
            },
            JoinableTaskCreationOptions.LongRunning);
        }

        private void EnsureGenContextInitialized()
        {
            var projectLanguage = _shell.GetActiveProjectLanguage();
            if (GenContext.CurrentLanguage != projectLanguage)
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(string.Empty), _shell, Platforms.Uwp, projectLanguage);
#else
                var mstxFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");
                GenContext.Bootstrap(new RemoteTemplatesSource(Platforms.Uwp, projectLanguage, mstxFilePath, new DigitalSignatureService()), _shell,  Platforms.Uwp, _shell.GetActiveProjectLanguage());
#endif
            }

            if (GenContext.CurrentLanguage != projectLanguage)
            {
                GenContext.SetCurrentLanguage(projectLanguage);
            }
        }

        private static string GetTempGenerationFolder()
        {
            string projectGuid = _shell.GetProjectGuidByName(GenContext.Current.ProjectName).ToString();
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, projectGuid);
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }
    }
}
