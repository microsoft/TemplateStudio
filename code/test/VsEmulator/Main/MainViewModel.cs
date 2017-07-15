// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.VsEmulator.TemplatesContent;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Templates.UI;
using Microsoft.Templates.VsEmulator.LoadProject;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using System.Diagnostics;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable, IContextProvider
    {
        private readonly MainView _host;
        private readonly string _language;

        public MainViewModel(MainView host, string language)
        {
            _host = host;
            _language = language;
            _wizardVersion = "0.0.0.0";
            _templatesVersion = "0.0.0.0";
        }

        public string ProjectName { get; private set; }
        public string OutputPath { get; private set; }
        public string ProjectPath { get; private set; }

        private bool _forceLocalTemplatesRefresh = true;
        public bool ForceLocalTemplatesRefresh
        {
            get => _forceLocalTemplatesRefresh;
            set => SetProperty(ref _forceLocalTemplatesRefresh, value);
        }

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions { get; } = new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public RelayCommand NewProjectCommand => new RelayCommand(NewProject);
        public RelayCommand LoadProjectCommand => new RelayCommand(LoadProject);
        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);
        public RelayCommand OpenInVsCodeCommand => new RelayCommand(OpenInVsCode);
        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);
        public RelayCommand OpenTempInExplorerCommand => new RelayCommand(OpenTempInExplorer);
        public RelayCommand ConfigureVersionsCommand => new RelayCommand(ConfigureVersions);
        public RelayCommand AddNewFeatureCommand => new RelayCommand(AddNewFeature);

        public RelayCommand AddNewPageCommand => new RelayCommand(AddNewPage);
       

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

        public string SolutionPath { get; set; }

        

        public void Initialize()
        {
            SolutionName = null;
        }

        private async void NewProject()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            try
            {
                var newProjectInfo = ShowNewProjectDialog();

                if (!string.IsNullOrEmpty(newProjectInfo.name))
                {
                    var projectPath = Path.Combine(newProjectInfo.location, newProjectInfo.name, newProjectInfo.name);
                    
                    GenContext.Current = this;

                    var userSelection = NewProjectGenController.Instance.GetUserSelection(_language);

                    if (userSelection != null)
                    {
                        ProjectName = newProjectInfo.name;
                        ProjectPath = projectPath;
                        OutputPath = projectPath;

                        ClearContext();
                        SolutionName = null;

                        userSelection.Language = _language;

                        await NewProjectGenController.Instance.GenerateProjectAsync(userSelection);

                        GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");

                        SolutionName = newProjectInfo.name;
                        SolutionPath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
                    }
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

        private void AddNewFeature()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            OutputPath = GetTempGenerationPath();
            ClearContext();

            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewFeature();

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
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

        private static string GetTempGenerationPath()
        {
            var tempGenerationPath = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);
            Fs.EnsureFolder(tempGenerationPath);

            var tempGenerationName = $"{GenContext.Current.ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            var inferredName = Naming.Infer(tempGenerationName, new List<Validator>() { new DirectoryExistsValidator(tempGenerationPath) }, "_");

            return Path.Combine(tempGenerationPath, inferredName);
        }

        private void ClearContext()
        {
            ProjectItems.Clear();
            MergeFilesFromProject.Clear();
            FailedMergePostActions.Clear();
            FilesToOpen.Clear();
        }

        private void AddNewPage()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            OutputPath = GetTempGenerationPath();
            ClearContext();
            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewPage();

                if (userSelection != null)
                {

                    NewItemGenController.Instance.FinishGeneration(userSelection);
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

        private void LoadProject()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);
            var loadProjectInfo = ShowLoadProjectDialog();

            if (!string.IsNullOrEmpty(loadProjectInfo))
            {
                SolutionPath = loadProjectInfo;
                SolutionName = Path.GetFileNameWithoutExtension(SolutionPath);

                // TODO: [ML] need to handle vbproj here too
                var projFile = Directory.EnumerateFiles(Path.GetDirectoryName(SolutionPath), "*.csproj", SearchOption.AllDirectories).FirstOrDefault();

                GenContext.Current = this;

                ProjectName = Path.GetFileNameWithoutExtension(projFile);
                ProjectPath = Path.GetDirectoryName(projFile);
                OutputPath = ProjectPath;
                IsWtsProject = GenContext.ToolBox.Shell.GetActiveProjectIsWts() ? Visibility.Visible : Visibility.Collapsed;
                ClearContext();
            }
        }

        private void ConfigureVersions()
        {
            var dialog = new TemplatesContentView(WizardVersion, TemplatesVersion)
            {
                Owner = _host
            };

            var dialogRes = dialog.ShowDialog();

            if (dialogRes.HasValue && dialogRes.Value)
            {
                WizardVersion = dialog.ViewModel.Result.WizardVersion;
                TemplatesVersion = dialog.ViewModel.Result.TemplatesVersion;
                ConfigureGenContext(ForceLocalTemplatesRefresh);
            }
        }


        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start(SolutionPath);
            }
        }

        private void OpenInVsCode()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start("code", $@"--new-window ""{Path.GetDirectoryName(SolutionPath)}""");
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(SolutionPath));
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
            return Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Count() > 0;
        }

        private (string name, string solutionName, string location) ShowNewProjectDialog()
        {
            var dialog = new NewProjectView()
            {
                Owner = _host
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return (dialog.ViewModel.Name, dialog.ViewModel.SolutionName, dialog.ViewModel.Location);
            }

            return (null, null, null);
        }

        private string ShowLoadProjectDialog()
        {
            var dialog = new LoadProjectView(SolutionPath)
            {
                Owner = _host
            };

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
            Log += message + Environment.NewLine;

            _host.Dispatcher.Invoke(() =>
            {
                _host.logScroll.ScrollToEnd();
            });
        }

        private void ConfigureGenContext(bool forceLocalTemplatesRefresh)
        {
            GenContext.Bootstrap(new LocalTemplatesSource(WizardVersion, TemplatesVersion, forceLocalTemplatesRefresh)
                , new FakeGenShell(msg => SetState(msg), l => AddLog(l), _host)
                , new Version(WizardVersion)
                , _language);

            CleanUpNotUsedContentVersions();
        }

        public void DoEvents()
        {
            var frame = new DispatcherFrame(true);

            var method = (SendOrPostCallback)delegate (object arg)
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
                        if (!dir.EndsWith("0.0.0.0"))
                        {
                            Fs.SafeDeleteDirectory(dir);
                        }
                    }
                }
            }
        }
        private string GetTemplatesFolder()
        {
            var _templatesSource = new LocalTemplatesSource(_wizardVersion, _templatesVersion);
            var _templatesSync = new TemplatesSynchronization(_templatesSource, new Version(_wizardVersion));
            string currentTemplatesFolder = _templatesSync.CurrentTemplatesFolder;

            return currentTemplatesFolder;
        }

    }
}
