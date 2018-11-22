// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickActions : IContextProvider
    {
        private static VsGenShell _shell;

        private GenerationService _generationService = GenerationService.Instance;

        public string ProjectName { get; private set; }

        public string GenerationOutputPath { get; private set; }

        public string DestinationPath { get; private set; }

        public List<ProjectInfo> Projects { get; private set; }

        public List<NugetReference> NugetReferences { get; private set; }

        public List<SdkReference> SdkReferences { get; private set; }

        public Dictionary<string, List<string>> ProjectReferences { get; private set; }

        public List<string> ProjectItems { get; private set; }

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
            if (_shell.GetActiveProjectIsWts())
            {
                SetContext();

                try
                {
                    var userSelection = NewItemController.Instance.GetUserSelectionNewPage(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider());

                    if (userSelection != null)
                    {
                        SafeThreading.JoinableTaskFactory.Run(
                        async () =>
                        {
                            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                            await _generationService.FinishGenerationAsync(userSelection);
                        },
                        JoinableTaskCreationOptions.LongRunning);

                        _shell.ShowStatusBarMessage(string.Format(StringRes.StatusBarNewItemAddPageSuccess, userSelection.Pages[0].name));
                    }
                }
                catch (WizardBackoutException)
                {
                    _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddPageCancelled);
                }
            }
        }

        public void AddNewFeature()
        {
            if (_shell.GetActiveProjectIsWts())
            {
                SetContext();
                try
                {
                    var userSelection = NewItemController.Instance.GetUserSelectionNewFeature(_shell.GetActiveProjectLanguage(), new VSStyleValuesProvider());

                    if (userSelection != null)
                    {
                        SafeThreading.JoinableTaskFactory.Run(
                        async () =>
                        {
                            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                            await _generationService.FinishGenerationAsync(userSelection);
                        },
                        JoinableTaskCreationOptions.LongRunning);

                        _shell.ShowStatusBarMessage(string.Format(StringRes.StatusBarNewItemAddFeatureSuccess, userSelection.Features[0].name));
                    }
                }
                catch (WizardBackoutException)
                {
                    _shell.ShowStatusBarMessage(StringRes.StatusBarNewItemAddFeatureCancelled);
                }
            }
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
                var projectConfig = ProjectConfigInfo.ReadProjectConfiguration();
                if (projectConfig.Platform == Platforms.Uwp)
                {
                    DestinationPath = GenContext.ToolBox.Shell.GetActiveProjectPath();
                    ProjectName = GenContext.ToolBox.Shell.GetActiveProjectName();
                }

                GenerationOutputPath = GenContext.GetTempGenerationPath(ProjectName);
                Projects = new List<ProjectInfo>();
                NugetReferences = new List<NugetReference>();
                SdkReferences = new List<SdkReference>();
                ProjectReferences = new Dictionary<string, List<string>>();
                ProjectItems = new List<string>();
                FilesToOpen = new List<string>();
                FailedMergePostActions = new List<FailedMergePostActionInfo>();
                MergeFilesFromProject = new Dictionary<string, List<MergeInfo>>();
                ProjectMetrics = new Dictionary<ProjectMetricsEnum, double>();

                GenContext.Current = this;
            }
        }

        private void EnsureGenContextInitialized()
        {
            if (GenContext.CurrentLanguage != _shell.GetActiveProjectLanguage())
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(), _shell, Platforms.Uwp, _shell.GetActiveProjectLanguage());
#else
                GenContext.Bootstrap(new RemoteTemplatesSource(Platforms.Uwp, _shell.GetActiveProjectLanguage()), _shell,  Platforms.Uwp, _shell.GetActiveProjectLanguage());
#endif
            }

            if (GenContext.CurrentLanguage != _shell.GetActiveProjectLanguage())
            {
                GenContext.SetCurrentLanguage(_shell.GetActiveProjectLanguage());
            }
        }

        private static string GetTempGenerationFolder()
        {
            string projectGuid = _shell.GetVsProjectId().ToString();
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, projectGuid);
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }
    }
}
