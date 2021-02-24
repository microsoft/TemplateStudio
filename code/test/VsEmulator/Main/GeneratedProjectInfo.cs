using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
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
        private string _platform;
        private string _language;
        private string _appModel;
        private string _time;
        private bool _useStyleCop;
        private GenerationService _generationService = GenerationService.Instance;
        private Visibility _isAddNewPageCommandVisible;
        private Visibility _isAddNewFeatureCommandVisible;
        private Visibility _isAddNewServiceCommandVisible;
        private Visibility _isAddNewTestingCommandVisible;

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

        public RelayCommand AddNewServiceCommand { get; }

        public RelayCommand AddNewTestingCommand { get; }

        public RelayCommand OpenTempInExplorerCommand { get; }

        public Visibility IsAddNewPageCommandVisible
        {
            get => _isAddNewPageCommandVisible;
            set => SetProperty(ref _isAddNewPageCommandVisible, value);
        }

        public Visibility IsAddNewFeatureCommandVisible
        {
            get => _isAddNewFeatureCommandVisible;
            set => SetProperty(ref _isAddNewFeatureCommandVisible, value);
        }


        public Visibility IsAddNewServiceCommandVisible
        {
            get => _isAddNewServiceCommandVisible;
            set => SetProperty(ref _isAddNewServiceCommandVisible, value);
        }


        public Visibility IsAddNewTestingCommandVisible
        {
            get => _isAddNewTestingCommandVisible;
            set => SetProperty(ref _isAddNewTestingCommandVisible, value);
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

        public string Platform
        {
            get => _platform;
            set => SetProperty(ref _platform, value);
        }

        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public string AppModel
        {
            get => _appModel;
            set => SetProperty(ref _appModel, value);
        }

        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public bool UseStyleCop
        {
            get => _useStyleCop;
            set => SetProperty(ref _useStyleCop, value);
        }

        public GeneratedProjectInfo()
        {
            OpenInVsCommand = new RelayCommand(OpenInVs);
            OpenInVsCodeCommand = new RelayCommand(OpenInVsCode);
            OpenInExplorerCommand = new RelayCommand(OpenInExplorer);
            AddNewPageCommand = new RelayCommand(() => AddNewItem(TemplateType.Page));
            AddNewFeatureCommand = new RelayCommand(() => AddNewItem(TemplateType.Feature));
            AddNewServiceCommand = new RelayCommand(() => AddNewItem(TemplateType.Service));
            AddNewTestingCommand = new RelayCommand(() => AddNewItem(TemplateType.Testing));
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

        public void SetProjectData(UserSelectionContext context, bool useStyleCop)
        {
            ProjectType = context.ProjectType;
            Framework = context.FrontEndFramework;
            Platform = context.Platform;
            Language = context.Language;
            context.PropertyBag.TryGetValue("appmodel", out var appModel);
            AppModel = appModel;
            UseStyleCop = useStyleCop;
            Time = DateTime.Now.ToShortTimeString();
        }

        public void SetContextInfo()
        {
            SolutionFilePath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
            IsAddNewPageCommandVisible = HasTemplates(TemplateType.Page) ? Visibility.Visible : Visibility.Collapsed;
            IsAddNewFeatureCommandVisible = HasTemplates(TemplateType.Feature) ? Visibility.Visible : Visibility.Collapsed;
            IsAddNewServiceCommandVisible = HasTemplates(TemplateType.Service) ? Visibility.Visible : Visibility.Collapsed;
            IsAddNewTestingCommandVisible = HasTemplates(TemplateType.Testing) ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool HasTemplates(TemplateType templateType)
            => GenContext.ToolBox.Shell.GetActiveProjectIsWts() && GenContext.ToolBox.Repo.GetAll().Any(t => t.GetRightClickEnabled() && t.GetTemplateType() == templateType);

        public void SetContext()
        {
            GenContext.Current = this;
            if (!string.IsNullOrEmpty(Language) && GenContext.CurrentLanguage != Language)
            {
                GenContext.SetCurrentLanguage(Language);
            }

            if (!string.IsNullOrEmpty(Platform) && GenContext.CurrentPlatform != Platform)
            {
                GenContext.SetCurrentPlatform(Platform);
            }
        }

        private void OpenInVs()
        {
            Process.Start(SolutionFilePath);
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
            if (templateType.IsItemTemplate())
            {
                return WizardLauncher.Instance.StartAddTemplate(GenContext.CurrentLanguage, Services.FakeStyleValuesProvider.Instance, templateType, templateType.GetWizardType().Value);
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

            Guid guid = GenContext.ToolBox.Shell.GetProjectGuidByName(GenContext.Current.ProjectName);
            return Path.Combine(path, guid.ToString());
        }
    }
}
