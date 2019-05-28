using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI.Launcher;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class GeneratedProjectInfo : Observable, IContextProvider
    {
        private string _projectType;
        private string _framework;
        private GenerationService _generationService = GenerationService.Instance;
        private Visibility _isWtsProject;

        public string ProjectName { get; private set; }

        public string SafeProjectName => ProjectName.MakeSafeProjectName();

        public string GenerationOutputPath { get; private set; }

        public string DestinationPath { get; private set; }

        public string SolutionFilePath { get; set; }

        public ProjectInfo ProjectInfo { get; } = new ProjectInfo();

        public List<string> FilesToOpen { get; } = new List<string>();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();

        public RelayCommand OpenInVsCommand { get; }

        public RelayCommand OpenInVsCodeCommand { get; }

        public RelayCommand OpenInExplorerCommand { get; }

        public RelayCommand AddNewPageCommand { get; }

        public RelayCommand AddNewFeatureCommand { get; }

        public RelayCommand OpenTempInExplorerCommand { get; }

        public Visibility IsWtsProject
        {
            get => _isWtsProject;
            set => SetProperty(ref _isWtsProject, value);
        }

        public Visibility TempFolderAvailable
        {
            get => HasContent(GetTempGenerationFolder()) ? Visibility.Visible : Visibility.Hidden;
        }

        public string ProjectType
        {
            get => _projectType;
            set => SetProperty(ref _projectType, value);
        }

        public string Framework
        {
            get => _framework;
            set => SetProperty(ref _framework, value);
        }

        public GeneratedProjectInfo()
        {
            OpenInVsCommand = new RelayCommand(OpenInVs);
            OpenInVsCodeCommand = new RelayCommand(OpenInVsCode);
            OpenInExplorerCommand = new RelayCommand(OpenInExplorer);
            AddNewPageCommand = new RelayCommand(() => AddNewItem(TemplateType.Page));
            AddNewFeatureCommand = new RelayCommand(() => AddNewItem(TemplateType.Feature));
            OpenTempInExplorerCommand = new RelayCommand(OpenTempInExplorer);
        }

        public void SetBasicInfo((string name, string solutionName, string location) projectInfo)
        {
            var destinationPath = Path.Combine(projectInfo.location, projectInfo.solutionName, projectInfo.name);
            ProjectName = projectInfo.name;
            DestinationPath = destinationPath;
            GenerationOutputPath = destinationPath;
        }

        public void SetBasicInfo(string projectName, string destinationPath)
        {
            ProjectName = projectName;
            DestinationPath = destinationPath;
            GenerationOutputPath = destinationPath;
        }

        public void SetProjectData(string projectType, string framework)
        {
            ProjectType = projectType;
            Framework = framework;
        }

        public void SetContextInfo()
        {
            SolutionFilePath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
            IsWtsProject = GenContext.ToolBox.Shell.GetActiveProjectIsWts() ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetContext()
        {
            GenContext.Current = this;
        }

        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(SolutionFilePath))
            {
                Process.Start(SolutionFilePath);
            }
        }

        private void OpenInVsCode()
        {
            if (!string.IsNullOrEmpty(SolutionFilePath))
            {
                Process.Start("code", $@"--new-window ""{Path.GetDirectoryName(SolutionFilePath)}""");
            }
        }

        private void OpenTempInExplorer()
        {
            var tempGenerationPath = GetTempGenerationFolder();
            if (HasContent(tempGenerationPath))
            {
                Process.Start(tempGenerationPath);
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(DestinationPath))
            {
                var destinationParentPath = Directory.GetParent(DestinationPath).FullName;
                Process.Start(destinationParentPath);
            }
        }

        private void AddNewItem(TemplateType templateType)
        {
            SetContext();
            GenerationOutputPath = GenContext.GetTempGenerationPath(GenContext.Current.ProjectName);
            try
            {
                var userSelection = GetUserSelectionByType(templateType);

                if (userSelection != null)
                {
                    FinishGeneration(userSelection);
                    OnPropertyChanged(nameof(TempFolderAvailable));
                    GenContext.ToolBox.Shell.ShowStatusBarMessage("Item created!!!");
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private UserSelection GetUserSelectionByType(TemplateType templateType)
        {
            if (templateType == TemplateType.Page)
            {
                return WizardLauncher.Instance.StartAddPage(GenContext.CurrentLanguage, Services.FakeStyleValuesProvider.Instance);
            }

            if (templateType == TemplateType.Feature)
            {
                return WizardLauncher.Instance.StartAddFeature(GenContext.CurrentLanguage, Services.FakeStyleValuesProvider.Instance);
            }

            return null;
        }

        private void FinishGeneration(UserSelection userSelection)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _generationService.FinishGeneration(userSelection);
            });
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Any();
        }

        private static string GetTempGenerationFolder()
        {
            if (GenContext.ToolBox == null || GenContext.ToolBox.Shell == null)
            {
                return string.Empty;
            }

            var path = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);

            Guid guid = GenContext.ToolBox.Shell.GetVsProjectId();
            return Path.Combine(path, guid.ToString());
        }
    }
}
