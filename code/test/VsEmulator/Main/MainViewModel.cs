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
using Microsoft.Internal.VisualStudio.Shell.TableControl;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
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
    public class MainViewModel : Observable
    {
        private readonly MainView _host;
        private readonly GenerationService _generationService = GenerationService.Instance;
        private bool _canRefreshTemplateCache;
        private bool _canRecreateUwpProject;
        private bool _canRecreateWpfProject;
        private bool _canRecreateWinUIProject;
        private string _selectedTheme;
        private bool _useStyleCop;
        private UserSelection _userSelectionUwp;
        private UserSelection _userSelectionWpf;
        private UserSelection _userSelectionWinUI;
        private (string name, string solutionName, string location) _projectLocation;

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

        public bool UseStyleCop
        {
            get => _useStyleCop;
            set => SetProperty(ref _useStyleCop, value);
        }

        public ObservableCollection<string> Themes { get; } = new ObservableCollection<string>();

        public ObservableCollection<GeneratedProjectInfo> Projects { get; } = new ObservableCollection<GeneratedProjectInfo>();

        public RelayCommand<string> NewProjectCommand => new RelayCommand<string>(NewProject);

        public RelayCommand<string> AnalyzeSelectionCommand => new RelayCommand<string>(AnalyzeSelection);

        public RelayCommand LoadProjectCommand => new RelayCommand(LoadProject);

        public RelayCommand RecreateUwpProjectCommand => new RelayCommand(RecreateUwpProject, () => _canRecreateUwpProject);

        public RelayCommand RecreateWpfProjectCommand => new RelayCommand(RecreateWpfProject, () => _canRecreateWpfProject);

        public RelayCommand RecreateWinUIProjectCommand => new RelayCommand(RecreateWinUIProject, () => _canRecreateWinUIProject);

        public RelayCommand RefreshTemplateCacheCommand => _refreshTemplateCacheCommand ?? (_refreshTemplateCacheCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await RefreshTemplateCacheAsync()), () => _canRefreshTemplateCache));

        public RelayCommand ConfigureVersionsCommand => new RelayCommand(ConfigureVersions);

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

        public async Task InitializeAsync()
        {
            SystemService.Initialize();
            SelectedTheme = Themes.First();
            await ConfigureGenContextAsync();
        }

        private void NewProject(string parameter)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var parameters = parameter.Split(',');
                var appModel = parameters.Length == 3 ? parameters[2] : string.Empty;
                var userSelection = await NewProjectAsync(parameters[0], parameters[1], appModel);
                if (parameters[0] == Platforms.Uwp)
                {
                    _userSelectionUwp = userSelection;
                }
                else if (parameters[0] == Platforms.Wpf)
                {
                    _userSelectionWpf = userSelection;
                }
                else if (parameters[0] == Platforms.WinUI)
                {
                    _userSelectionWinUI = userSelection;
                }
            });
        }

        private void RecreateUwpProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _projectLocation = NewProjectViewModel.GetNewProjectInfo();
                _userSelectionUwp = await RecreateProjectAsync(_userSelectionUwp);
            });
        }

        private void RecreateWpfProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _projectLocation = NewProjectViewModel.GetNewProjectInfo();
                _userSelectionWpf = await RecreateProjectAsync(_userSelectionWpf);
            });
        }

        private void RecreateWinUIProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _projectLocation = NewProjectViewModel.GetNewProjectInfo();
                _userSelectionWinUI = await RecreateProjectAsync(_userSelectionWinUI);
            });
        }

        private void AnalyzeSelection(string parameter)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var parameters = parameter.Split(',');
                AnalyzeNewProject(parameters[0], parameters[1]);
            });
        }

        private async Task<UserSelection> NewProjectAsync(string platform, string language, string appmodel = null)
        {

            SetCurrentLanguage(language);
            SetCurrentPlatform(platform);

            if (!string.IsNullOrEmpty(appmodel))
            {
                SetCurrentAppModel(appmodel);
            }
            

            try
            {
                _projectLocation = ShowNewProjectDialog();

                if (!string.IsNullOrEmpty(_projectLocation.name))
                {
                    var newProject = new GeneratedProjectInfo();
                    newProject.SetBasicInfo(_projectLocation);
                    newProject.SetContext();

                    var context = new UserSelectionContext(language, platform);
                    if (!string.IsNullOrEmpty(appmodel))
                    {
                        context.PropertyBag.Add("appmodel", appmodel);
                    }
                    
                    var userSelection = WizardLauncher.Instance.StartNewProject(context, string.Empty, Services.FakeStyleValuesProvider.Instance);
                    switch (platform)
                    {
                        case Platforms.Uwp:
                            _canRecreateUwpProject = true;
                            break;
                        case Platforms.Wpf:
                            _canRecreateWpfProject = true;
                            break;
                        case Platforms.WinUI:
                            _canRecreateWinUIProject = true;
                            break;
                    }

                    if (userSelection != null)
                    {
                        if (UseStyleCop)
                        {
                            AddStyleCop(userSelection, platform, language);
                        }
                        await _generationService.GenerateProjectAsync(userSelection);
                        GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");
                        newProject.SetProjectData(userSelection.Context, UseStyleCop);
                        newProject.SetContextInfo();
                        Projects.Insert(0, newProject);
                    }

                    return userSelection;
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

            return null;
        }

        private async Task<UserSelection> RecreateProjectAsync(UserSelection userSelection)
        {
            try
            {
                if (userSelection != null && !string.IsNullOrEmpty(_projectLocation.name))
                {
                    var newProject = new GeneratedProjectInfo();
                    newProject.SetBasicInfo(_projectLocation);
                    newProject.SetContext();
                    SetCurrentLanguage(userSelection.Context.Language);
                    SetCurrentPlatform(userSelection.Context.Platform);

                    if (userSelection.Context.PropertyBag.ContainsKey("appmodel"))
                    {
                        SetCurrentAppModel(userSelection.Context.PropertyBag["appmodel"]);
                    }

                    if (UseStyleCop)
                    {
                        AddStyleCop(userSelection, userSelection.Context.Platform, userSelection.Context.Language);
                    }
                    await _generationService.GenerateProjectAsync(userSelection);
                    GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");
                    newProject.SetProjectData(userSelection.Context, UseStyleCop);
                    newProject.SetContextInfo();
                    Projects.Insert(0, newProject);
                    return userSelection;
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

            return null;
        }

        private void AddStyleCop(UserSelection userSelection, string platform, string language)
        {
            var styleCopTemplate = string.Empty;
            switch (platform)
            {
                case Platforms.Uwp:
                    switch (language)
                    {
                        case "C#":
                            styleCopTemplate = "wts.Feat.StyleCop";
                            break;
                        case "VisualBasic":
                            styleCopTemplate = "wts.Feat.VBStyleAnalysis";
                            break;
                        default:
                            return;
                    }
                    break;
                case Platforms.Wpf:
                    styleCopTemplate = "wts.Wpf.Feat.StyleCop";
                    break;
                case Platforms.WinUI:
                    styleCopTemplate = "wts.WinUI.Feat.StyleCop";
                    break;
            }


            var testingFeature = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == styleCopTemplate);
            if (testingFeature != null)
            {
                var userSelectionItem = new UserSelectionItem()
                {
                    Name = styleCopTemplate,
                    TemplateId = styleCopTemplate,
                };

                userSelection.Add(userSelectionItem, testingFeature.GetTemplateType());
            }
        }

        private void AnalyzeNewProject(string platform, string language)
        {
            SetCurrentLanguage(language);
            SetCurrentPlatform(platform);
            try
            {
                var newProjectName = "AnalyzeSelection" + Path.GetFileNameWithoutExtension(Path.GetTempFileName());
                var newProjectLocation = Path.GetTempPath();
                var newProject = new GeneratedProjectInfo();
                newProject.SetBasicInfo((newProjectName, newProjectName, newProjectLocation));
                newProject.SetContext();

                var context = new UserSelectionContext(language, platform);

                var userSelection = WizardLauncher.Instance.StartNewProject(context, string.Empty, Services.FakeStyleValuesProvider.Instance);

                if (userSelection != null)
                {
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

        private void LoadProject()
        {
            var newProject = new GeneratedProjectInfo();
            newProject.SetContext();
            var loadProjectInfo = ShowLoadProjectDialog();

            if (!string.IsNullOrEmpty(loadProjectInfo))
            {
                var solutionFilePath = loadProjectInfo;
                var solutionName = Path.GetFileNameWithoutExtension(solutionFilePath);
                var destinationParent = Path.GetDirectoryName(solutionFilePath);
                var projFile = Directory.EnumerateFiles(destinationParent, "*.csproj", SearchOption.AllDirectories)
                        .Union(Directory.EnumerateFiles(destinationParent, "*.vbproj", SearchOption.AllDirectories))
                        .Union(Directory.EnumerateFiles(destinationParent, "*.vcxproj", SearchOption.AllDirectories)).FirstOrDefault();

                string language = String.Empty;
                switch (Path.GetExtension(projFile))
                {
                    case ".vbproj":
                        language = ProgrammingLanguages.VisualBasic;
                        break;
                    case ".csproj":
                        language = ProgrammingLanguages.CSharp;
                        break;
                    case ".vcxproj":
                        language = ProgrammingLanguages.Cpp;
                        break;
                }
                var projectName = Path.GetFileNameWithoutExtension(projFile);
                var destinationPath = Path.GetDirectoryName(projFile);
                newProject.SetBasicInfo(projectName, destinationPath);
                var config = ProjectConfigInfoService.ReadProjectConfiguration();
                SetCurrentLanguage(language);
                SetCurrentPlatform(config.Platform);

                if (!string.IsNullOrEmpty(config.AppModel))
                {
                    SetCurrentPlatform(config.AppModel);
                }

                var context = new UserSelectionContext(language, config.Platform)
                {
                    FrontEndFramework = config.Framework,
                    ProjectType = config.ProjectType
                };

                context.PropertyBag.Add("appmodel", config.AppModel);
                newProject.SetProjectData(context, false);
                newProject.SetContextInfo();
                Projects.Insert(0, newProject);
            }
        }

        private async Task RefreshTemplateCacheAsync()
        {
            UpdateCanRefreshTemplateCache(false);
            GenContext.ToolBox.Shell.ShowStatusBarMessage("Refreshing template cache ...");

            await GenContext.ToolBox.Repo.RefreshAsync(true);
            AddLog($"{DateTime.Now.FormatAsTime()} - Template cache refreshed");

            UpdateCanRefreshTemplateCache(true);
            GenContext.ToolBox.Shell.ShowStatusBarMessage("Template cache refresh completed");
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
            var dialog = new LoadProjectView(null);
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
            GenContext.SetCurrentPlatform(platform);
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentPlatform(platform);
        }

        private void SetCurrentAppModel(string appModel)
        {
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentAppModel(appModel);
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
            return Path.Combine(SpecialFolder.LocalApplicationData.ToString(), @"\WinTS\Templates\LocalEnvWinTS");
        }
    }
}
