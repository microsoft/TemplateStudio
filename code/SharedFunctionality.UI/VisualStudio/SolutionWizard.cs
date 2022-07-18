// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Launcher;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.VisualStudio.GenShell;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class SolutionWizard : IWizard, IContextProvider
    {
        private readonly GenerationService _generationService = GenerationService.Instance;
        private string _platform;
        private string _appModel;
        private string _language;

        private UserSelection _userSelection;
        private Dictionary<string, string> _replacementsDictionary;

        public string SafeProjectName => _replacementsDictionary["$safeprojectname$"];

        public string ProjectName => _replacementsDictionary["$projectname$"];

        public string DestinationPath => new DirectoryInfo(_replacementsDictionary["$destinationdirectory$"]).FullName;

        public string GenerationOutputPath => DestinationPath;

        public ProjectInfo ProjectInfo { get; } = new ProjectInfo();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; private set; } = new Dictionary<ProjectMetricsEnum, double>();

        protected void Initialize()
        {
            _platform = _replacementsDictionary.SafeGet("$ts.platform$");
            _appModel = _replacementsDictionary.SafeGet("$ts.appmodel$");
            _language = _replacementsDictionary.SafeGet("$ts.language$");

            TelemetryService.Current.WizardVersion = GenContext.GetWizardVersionFromAssembly().ToString();

            if (GenContext.CurrentLanguage != _language || GenContext.CurrentPlatform != _platform)
            {
                GenContext.Bootstrap(new VsixTemplatesSource(string.Empty, platform: _platform), new VsGenShell(), _platform, _language);
            }
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            System.Diagnostics.Debug.WriteLine("Project finished generating");
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            SafeThreading.JoinableTaskFactory.Run(
                async () =>
                {
                    await RunFinishedAsync();
                },
                JoinableTaskCreationOptions.LongRunning);
        }

        public async Task RunFinishedAsync()
        {
            try
            {
                AppHealth.Current.Info.TrackAsync(string.Format(Resources.StatusBarCreatingProject, TemplateStudioProject.AppName)).FireAndForget();

                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                await _generationService.GenerateProjectAsync(_userSelection);
                GenContext.Current.FilesToOpen.Add(Path.Combine(GenContext.Current.GenerationOutputPath, "README.md"));
                PostGenerationActions();

                AppHealth.Current.Info.TrackAsync(Resources.StatusBarGenerationFinished).FireAndForget();
            }
            catch (System.Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
                System.Diagnostics.Debugger.Break();
                AppHealth.Current.Exception.TrackAsync(exc, Resources.ErrorFailedToGenerateProjectFromTemplates).FireAndForget();
            }
        }

        private static void PostGenerationActions()
        {
            GenContext.ToolBox.Shell.Solution.CollapseSolutionItems();
            GenContext.ToolBox.Shell.UI.OpenProjectOverview();
            GenContext.ToolBox.Shell.UI.OpenItems(GenContext.Current.FilesToOpen.ToArray());
            GenContext.ToolBox.Shell.UI.ShowTaskList();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                if (runKind == WizardRunKind.AsNewProject || runKind == WizardRunKind.AsMultiProject)
                {
                    _replacementsDictionary = replacementsDictionary;

                    Initialize();

                    GenContext.Current = this;

                    var context = new UserSelectionContext(_language, _platform);
                    if (!string.IsNullOrEmpty(_appModel))
                    {
                        context.AddAppModel(_appModel);
                    }

                    var requiredVersion = _replacementsDictionary.SafeGet("$ts.requiredversion$");
                    var requiredworkloads = _replacementsDictionary.SafeGet("$ts.requiredworkloads$");

                    _userSelection = WizardLauncher.Instance.StartNewProject(context, requiredVersion, requiredworkloads, new VSStyleValuesProvider());
                }
            }
            catch (WizardBackoutException)
            {
                if (GenContext.ToolBox.Repo.SyncInProgress)
                {
                    GenContext.ToolBox.Repo.CancelSynchronization();
                }

                CleanupDirectories(DestinationPath);

                throw;
            }
            catch (System.Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
                System.Diagnostics.Debugger.Break();

                throw;
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private static void CleanupDirectories(string projectDirectory)
        {
            var parentDir = new DirectoryInfo(projectDirectory).Parent.FullName;
            Fs.SafeDeleteDirectory(projectDirectory);

            if (Directory.Exists(parentDir)
                && !Directory.EnumerateDirectories(parentDir).Any()
                && !Directory.EnumerateFiles(parentDir).Any())
            {
                Fs.SafeDeleteDirectory(parentDir);
            }
        }

        public SolutionWizard()
        {
        }
    }
}
