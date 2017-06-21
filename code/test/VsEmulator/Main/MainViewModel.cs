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

        public MainViewModel(MainView host)
        {
            _host = host;
            _wizardVersion = "0.0.0.0";
            _templatesVersion = "0.0.0.0";
        }

        public string ProjectName { get; private set; }
        public string OutputPath { get; private set; }
        public string ProjectPath { get; private set; }
        public bool ForceLocalTemplatesRefresh { get; set; } = true;

        public List<string> ProjectItems { get; } = new List<string>();

        public List<GenerationWarning> GenerationWarnings { get; } = new List<GenerationWarning>();

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

        private Visibility _tempFolderAvailable;
        public Visibility TempFolderAvailable
        {
            get => _tempFolderAvailable;
            set => SetProperty(ref _tempFolderAvailable, value);
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
                    TempFolderAvailable = Visibility.Hidden;
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

                    var userSelection = NewProjectGenController.Instance.GetUserSelection();
                    if (userSelection != null)
                    {
                        ProjectName = newProjectInfo.name;
                        ProjectPath = projectPath;
                        OutputPath = projectPath;

                        ClearContext();
                        SolutionName = null;

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
            OutputPath = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, Path.GetRandomFileName());
            ClearContext();

            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewItem(TemplateType.Feature);

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    TempFolderAvailable = Visibility.Visible;
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

        private void ClearContext()
        {
            ProjectItems.Clear();
            MergeFilesFromProject.Clear();
            GenerationWarnings.Clear();
            FilesToOpen.Clear();
        }

        private void AddNewPage()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);
            OutputPath = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath, Path.GetRandomFileName());
            ClearContext();
            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewItem(TemplateType.Page);

                if (userSelection != null)
                {

                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    TempFolderAvailable = Visibility.Visible;
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
            var loadProjectInfo = ShowLoadProjectDialog();

            if (!string.IsNullOrEmpty(loadProjectInfo))
            {
                SolutionPath = loadProjectInfo;
                SolutionName = Path.GetFileNameWithoutExtension(SolutionPath);

                var projFile = Directory.EnumerateFiles(Path.GetDirectoryName(SolutionPath), "*.csproj", SearchOption.AllDirectories).FirstOrDefault();

                GenContext.Current = this;

                ProjectName = Path.GetFileNameWithoutExtension(projFile);
                ProjectPath = Path.GetDirectoryName(projFile);
                OutputPath = ProjectPath;
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
            if (!string.IsNullOrEmpty(GenContext.Current.OutputPath))
            {
                System.Diagnostics.Process.Start(GenContext.Current.OutputPath);
            }
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
            GenContext.Bootstrap(new LocalTemplatesSource(WizardVersion, TemplatesVersion)
                , new FakeGenShell(msg => SetState(msg), l => AddLog(l), _host)
                , new Version(WizardVersion));
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
    }
}
