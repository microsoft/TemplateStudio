// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.Launcher;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.VsEmulator.LoadProject;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.VsEmulator.TemplatesContent;
using Microsoft.VisualStudio.TemplateWizard;
using static System.Environment;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable, IContextProvider
    {
        private readonly MainView _host;

        private string _platform;
        private bool _canRefreshTemplateCache;
        private string _selectedTheme;

        private GenerationService _generationService = GenerationService.Instance;

        private RelayCommand _refreshTemplateCacheCommand;

        public MainViewModel(MainView host)
        {
            _host = host;
            _wizardVersion = "0.0.0.0";
            _templatesVersion = "0.0.0.0";
            Themes.Add("Light");
            Themes.Add("Dark");
            SystemService = new SystemService();
        }

        public string ProjectName { get; private set; }

        public string SafeProjectName => ProjectName.MakeSafeProjectName();

        public string GenerationOutputPath { get;  set; }

        public string DestinationPath { get; private set; }

        public SystemService SystemService { get; }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
                var themeName = SystemService.IsHighContrast ? "HighContrast" : value;
                Services.FakeStyleValuesProvider.Instance.LoadResources(themeName);
            }
        }

        public ObservableCollection<string> Themes { get; } = new ObservableCollection<string>();

        public ProjectInfo ProjectInfo { get; } = new ProjectInfo();

        public List<SdkReference> SdkReferences { get; } = new List<SdkReference>();

        public List<NugetReference> NugetReferences { get; } = new List<NugetReference>();

        public Dictionary<string, List<string>> ProjectReferences { get; } = new Dictionary<string, List<string>>();

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();

        public RelayCommand NewUwpCSharpProjectCommand => new RelayCommand(NewUwpCSharpProject);

        public RelayCommand AnalyzeCSharpSelectionCommand => new RelayCommand(AnalyzeCSharpSelection);

        public RelayCommand AnalyzeVisualBasicSelectionCommand => new RelayCommand(AnalyzeVisualBasicSelection);

        public RelayCommand NewUwpVisualBasicProjectCommand => new RelayCommand(NewUwpVisualBasicProject);

        public RelayCommand LoadProjectCommand => new RelayCommand(LoadProject);

        public RelayCommand RefreshTemplateCacheCommand => _refreshTemplateCacheCommand ?? (_refreshTemplateCacheCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await RefreshTemplateCacheAsync()), () => _canRefreshTemplateCache));

        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);

        public RelayCommand OpenInVsCodeCommand => new RelayCommand(OpenInVsCode);

        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);

        public RelayCommand OpenTempInExplorerCommand => new RelayCommand(OpenTempInExplorer);

        public RelayCommand ConfigureVersionsCommand => new RelayCommand(ConfigureVersions);

        public RelayCommand AddNewFeatureCommand => new RelayCommand(() => AddNewItem(TemplateType.Feature));

        public RelayCommand AddNewPageCommand => new RelayCommand(() => AddNewItem(TemplateType.Page));

        private string _state;

        public string State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private string _log;

        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        private Visibility _isProjectLoaded;

        public Visibility IsProjectLoaded
        {
            get => _isProjectLoaded;
            set => SetProperty(ref _isProjectLoaded, value);
        }

        private Visibility _isWtsProject;

        public Visibility IsWtsProject
        {
            get => _isWtsProject;
            set => SetProperty(ref _isWtsProject, value);
        }

        public Visibility TempFolderAvailable
        {
            get
            {
                return HasContent(GetTempGenerationFolder()) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private string _wizardVersion;

        public string WizardVersion
        {
            get => _wizardVersion;
            set => SetProperty(ref _wizardVersion, value);
        }

        private string _templatesVersion;

        public string TemplatesVersion
        {
            get => _templatesVersion;
            set => SetProperty(ref _templatesVersion, value);
        }

        private string _solutionName;

        public string SolutionName
        {
            get => _solutionName;
            set
            {
                SetProperty(ref _solutionName, value);

                if (string.IsNullOrEmpty(value))
                {
                    IsProjectLoaded = Visibility.Hidden;
                }
                else
                {
                    IsProjectLoaded = Visibility.Visible;
                }
            }
        }

        public string SolutionFilePath { get; set; }

        public async Task InitializeAsync()
        {
            SystemService.Initialize();
            SelectedTheme = Themes.First();
            SolutionName = null;
            await ConfigureGenContextAsync();
        }

        private void NewUwpCSharpProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                await NewProjectAsync(Platforms.Uwp, ProgrammingLanguages.CSharp);
            });
        }

        private void FinishGeneration(UserSelection userSelection)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _generationService.FinishGeneration(userSelection);
            });
        }

        private void AnalyzeCSharpSelection()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                AnalyzeNewProject(Platforms.Uwp, ProgrammingLanguages.CSharp);
            });
        }

        private void AnalyzeVisualBasicSelection()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                AnalyzeNewProject(Platforms.Uwp, ProgrammingLanguages.VisualBasic);
            });
        }

        private void NewUwpVisualBasicProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                await NewProjectAsync(Platforms.Uwp, ProgrammingLanguages.VisualBasic);
            });
        }

        private async Task NewProjectAsync(string platform, string language)
        {
            _platform = platform;
            SetCurrentLanguage(language);
            SetCurrentPlatform(platform);

            try
            {
                var newProjectInfo = ShowNewProjectDialog();

                if (!string.IsNullOrEmpty(newProjectInfo.name))
                {
                    var destinationPath = Path.Combine(newProjectInfo.location, newProjectInfo.name, newProjectInfo.name);

                    GenContext.Current = this;
                    ProjectName = newProjectInfo.name;
                    DestinationPath = destinationPath;
                    GenerationOutputPath = destinationPath;
                    SolutionName = null;

                    var userSelection = WizardLauncher.Instance.StartNewProject(platform, language, Services.FakeStyleValuesProvider.Instance);

                    if (userSelection != null)
                    {
                        ClearContext();

                        await _generationService.GenerateProjectAsync(userSelection);

                        GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");

                        SolutionName = newProjectInfo.name;
                        SolutionFilePath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
                        IsWtsProject = GenContext.ToolBox.Shell.GetActiveProjectIsWts() ? Visibility.Visible : Visibility.Collapsed;

                        OnPropertyChanged(nameof(TempFolderAvailable));
                    }
                }
            }
            catch (WizardBackoutException)
            {
                CleanUp();
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void AnalyzeNewProject(string platform, string language)
        {
            SetCurrentLanguage(language);
            try
            {
                var newProjectName = "AnalyzeSelection" + Path.GetFileNameWithoutExtension(Path.GetTempFileName());
                var newProjectLocation = Path.GetTempPath();

                var destinationPath = Path.Combine(newProjectLocation, newProjectName, newProjectName);

                GenContext.Current = this;
                ProjectName = newProjectName;
                DestinationPath = destinationPath;
                GenerationOutputPath = destinationPath;

                var userSelection = WizardLauncher.Instance.StartNewProject(platform, language, Services.FakeStyleValuesProvider.Instance);

                if (userSelection != null)
                {
                    ClearContext();

                    AnalyzeSelectionOutput(userSelection);
                    AddLog("See debug window for analysis");
                }
            }
            catch (WizardBackoutException)
            {
                CleanUp();
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void AnalyzeSelectionOutput(UserSelection userSelection)
        {
            var generatedFileList = new Dictionary<string, List<string>>();

            var genItems = GenComposer.Compose(userSelection).ToList();

            Debug.WriteLine("Template output");
            Debug.WriteLine("===============");

            foreach (var genItem in genItems)
            {
                var configLoc = genItem.Template.ConfigPlace.Replace("/.template.config/template.json", string.Empty);
                Debug.WriteLine($"{genItem.Template.Identity} ({configLoc})");

                var fullConfigLoc = $"{GenContext.ToolBox.Repo.CurrentContentFolder}{configLoc.Replace("/", "\\")}";
                var files = Directory.EnumerateFiles(fullConfigLoc, "*.*", SearchOption.AllDirectories)
                                     .Where(f => !f.Contains(".template.config"))
                                     .ToList();

                foreach (var file in files)
                {
                    var shortFilePath = file.Replace(fullConfigLoc, string.Empty);

                    Debug.WriteLine($" - {shortFilePath}");

                    var configuredName = shortFilePath.Replace("wts.ItemName", genItem.Name).Replace("_postaction", string.Empty);

                    if (!generatedFileList.ContainsKey(configuredName))
                    {
                        generatedFileList.Add(configuredName, new List<string>());
                    }

                    if (file.Contains("_postaction"))
                    {
                        generatedFileList[configuredName].Add($"{shortFilePath} ({configLoc})");
                    }
                }

                Debug.WriteLine(string.Empty);
            }

            Debug.WriteLine(string.Empty);

            Debug.WriteLine("File output");
            Debug.WriteLine("===========");

            foreach (var genFile in generatedFileList.OrderBy(g => g.Key.Substring(1).Contains("\\")).ThenBy(f => f.Key))
            {
                Debug.WriteLine(genFile.Key);

                foreach (var extraFile in genFile.Value)
                {
                    Debug.WriteLine($" + {extraFile}");
                }

                Debug.WriteLine(string.Empty);
            }

            Debug.WriteLine(string.Empty);
        }

        private void CleanUp()
        {
            if (GenContext.ToolBox.Repo.SyncInProgress)
            {
                GenContext.ToolBox.Repo.CancelSynchronization();
            }
        }

        private void ClearContext()
        {
            ProjectInfo.Projects.Clear();
            ProjectInfo.ProjectReferences.Clear();
            ProjectInfo.NugetReferences.Clear();
            ProjectInfo.SdkReferences.Clear();
            ProjectInfo.ProjectItems.Clear();
            MergeFilesFromProject.Clear();
            FailedMergePostActions.Clear();
            FilesToOpen.Clear();
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

        private void AddNewItem(TemplateType templateType)
        {
            GenerationOutputPath = GenContext.GetTempGenerationPath(GenContext.Current.ProjectName);
            ClearContext();
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
                CleanUp();
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void LoadProject()
        {
            var loadProjectInfo = ShowLoadProjectDialog();

            if (!string.IsNullOrEmpty(loadProjectInfo))
            {
                SolutionFilePath = loadProjectInfo;
                SolutionName = Path.GetFileNameWithoutExtension(SolutionFilePath);

                var destinationParent = Path.GetDirectoryName(SolutionFilePath);

                var projFile = Directory.EnumerateFiles(destinationParent, "*.csproj", SearchOption.AllDirectories)
                        .Union(Directory.EnumerateFiles(destinationParent, "*.vbproj", SearchOption.AllDirectories)).FirstOrDefault();

                var language = Path.GetExtension(projFile) == ".vbproj" ? ProgrammingLanguages.VisualBasic : ProgrammingLanguages.CSharp;

                ProjectName = Path.GetFileNameWithoutExtension(projFile);
                DestinationPath = Path.GetDirectoryName(projFile);
                GenerationOutputPath = DestinationPath;
                GenContext.Current = this;

                var platform = ProjectConfigInfo.ReadProjectConfiguration().Platform;
                SetCurrentLanguage(language);
                SetCurrentPlatform(platform);

                IsWtsProject = GenContext.ToolBox.Shell.GetActiveProjectIsWts() ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(TempFolderAvailable));
                ClearContext();
            }
        }

        private async Task RefreshTemplateCacheAsync()
        {
            UpdateCanRefreshTemplateCache(false);

            await GenContext.ToolBox.Repo.RefreshAsync(true);
            AddLog($"{DateTime.Now.FormatAsTime()} - Template cache refreshed");

            UpdateCanRefreshTemplateCache(true);
        }

        private void ConfigureVersions()
        {
            var dialog = new TemplatesContentView(WizardVersion, TemplatesVersion);
            dialog.Owner = _host;

            var dialogRes = dialog.ShowDialog();

            if (dialogRes.HasValue && dialogRes.Value)
            {
                WizardVersion = dialog.ViewModel.Result.WizardVersion;
                TemplatesVersion = dialog.ViewModel.Result.TemplatesVersion;
                ConfigureGenContextAsync().FireAndForget();
            }
        }

        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(SolutionFilePath))
            {
                System.Diagnostics.Process.Start(SolutionFilePath);
            }
        }

        private void OpenInVsCode()
        {
            if (!string.IsNullOrEmpty(SolutionFilePath))
            {
                System.Diagnostics.Process.Start("code", $@"--new-window ""{Path.GetDirectoryName(SolutionFilePath)}""");
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(DestinationPath))
            {
                var destinationParentPath = Directory.GetParent(DestinationPath).FullName;
                System.Diagnostics.Process.Start(destinationParentPath);
            }
        }

        private void OpenTempInExplorer()
        {
            var tempGenerationPath = GetTempGenerationFolder();
            if (HasContent(tempGenerationPath))
            {
                System.Diagnostics.Process.Start(tempGenerationPath);
            }
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

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Any();
        }

        private (string name, string solutionName, string location) ShowNewProjectDialog()
        {
            var dialog = new NewProjectView();
            dialog.Owner = _host;

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return (dialog.ViewModel.Name, dialog.ViewModel.SolutionName, dialog.ViewModel.Location);
            }

            return (null, null, null);
        }

        private string ShowLoadProjectDialog()
        {
            var dialog = new LoadProjectView(SolutionFilePath);
            dialog.Owner = _host;

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return dialog.ViewModel.SolutionPath;
            }

            return string.Empty;
        }

        private void SetState(string message)
        {
            State = message;
            DoEvents();
        }

        private void AddLog(string message)
        {
            Log = message + Environment.NewLine + Log;
        }

        private void SetCurrentLanguage(string language)
        {
            GenContext.SetCurrentLanguage(language);
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentLanguage(language);
        }

        private void SetCurrentPlatform(string platform)
        {
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentPlatform(platform);
        }

        private async Task ConfigureGenContextAsync()
        {
            GenContext.Bootstrap(
                new LocalTemplatesSource(string.Empty, TemplatesVersion, string.Empty),
                new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp, msg => SetState(msg), l => AddLog(l), _host),
                new Version(WizardVersion),
                Platforms.Uwp,
                ProgrammingLanguages.CSharp);

            await GenContext.ToolBox.Repo.SynchronizeAsync();

            UpdateCanRefreshTemplateCache(true);
        }

        private void UpdateCanRefreshTemplateCache(bool canRefreshTemplateCache)
        {
            _canRefreshTemplateCache = canRefreshTemplateCache;
            RefreshTemplateCacheCommand.RaiseCanExecuteChanged();
        }

        [SuppressMessage(
            "Usage",
            "VSTHRD001:Use Await JoinableTaskFactory.SwitchToMainThreadAsync() to switch to the UI thread",
            Justification = "Not applying this rule to this method as it was specifically desgned as is.")]
        public void DoEvents()
        {
            var frame = new DispatcherFrame(true);

            SendOrPostCallback method = (arg) =>
            {
                var f = arg as DispatcherFrame;
                f.Continue = false;
            };

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, method, frame);
            Dispatcher.PushFrame(frame);
        }

        private void CleanUpNotUsedContentVersions()
        {
            if (_wizardVersion == "0.0.0.0" && _templatesVersion == "0.0.0.0")
            {
                var templatesFolder = GetTemplatesFolder();
                if (Directory.Exists(templatesFolder))
                {
                    var dirs = Directory.EnumerateDirectories(templatesFolder);
                    foreach (var dir in dirs)
                    {
                        if (!dir.EndsWith("0.0.0.0", StringComparison.OrdinalIgnoreCase))
                        {
                            Fs.SafeDeleteDirectory(dir);
                        }
                    }
                }
            }
        }

        private string GetTemplatesFolder()
        {
            return Path.Combine(SpecialFolder.LocalApplicationData.ToString(), @"\WinTS\Templates\LocalEnv");
        }
    }
}
