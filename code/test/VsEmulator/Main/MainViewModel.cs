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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.VsEmulator.TemplatesContent;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable
    {
        private readonly MainView _host;

        public MainViewModel(MainView host)
        {
            _host = host;
            _newProjectLabel = "New Project...";
            _isWizardVersionReconfigurable = true;
            UseWizardVersion = "0.0.0.0";
            UseTemplatesVersion = "0.0.0.0";
        }

        public RelayCommand NewProjectCommand => new RelayCommand(NewProject);

        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);
        public RelayCommand OpenInVsCodeCommand => new RelayCommand(OpenInVsCode);
        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);
        public RelayCommand TemplatesContentCommand => new RelayCommand(TemplatesContent);
        public RelayCommand SetVersionCommand => new RelayCommand(ConfigureGenContext);


        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private string _log;
        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }

        private Visibility _isProjectLoaded;
        public Visibility IsProjectLoaded
        {
            get { return _isProjectLoaded; }
            set { SetProperty(ref _isProjectLoaded, value); }
        }


        private String _useWizardVersion;
        public String UseWizardVersion
        {
            get { return _useWizardVersion; }
            set { SetProperty(ref _useWizardVersion, value); }
        }

        private String _useTemplatesVersion;
        public String UseTemplatesVersion
        {
            get { return _useTemplatesVersion; }
            set { SetProperty(ref _useTemplatesVersion, value); }
        }

        private String _newProjectLabel;
        public String NewProjectLabel
        {
            get { return _newProjectLabel; }
            set { SetProperty(ref _newProjectLabel, value); }
        }

        private bool _isWizardVersionReconfigurable;
        public bool IsWizardVersionReconfigurable
        {
            get { return _isWizardVersionReconfigurable; }
            set { SetProperty(ref _isWizardVersionReconfigurable, value); }
        }

        private string _solutionName;
        public string SolutionName
        {
            get { return _solutionName; }
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
            ConfigureGenContext();

            try
            {
                var newProjectInfo = ShowNewProjectDialog();
                if (!string.IsNullOrEmpty(newProjectInfo.name))
                {
                    var outputPath = Path.Combine(newProjectInfo.location, newProjectInfo.name, newProjectInfo.name);
                    using (var context = GenContext.CreateNew(newProjectInfo.name, outputPath))
                    {
                        var userSelection = GenController.GetUserSelection();
                        if (userSelection != null)
                        {
                            SolutionName = null;

                            await GenController.GenerateAsync(userSelection);

                            GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");

                            SolutionName = newProjectInfo.name;
                            SolutionPath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
                        }
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

        private void TemplatesContent()
        {
            var dialog = new TemplatesContentView(UseWizardVersion)
            {
                Owner = _host
            };
            dialog.Show();
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

        private void ConfigureGenContext()
        {
            GenContext.Bootstrap(new LocalTemplatesSource(UseTemplatesVersion)
                , new FakeGenShell(msg => SetState(msg), l => AddLog(l), _host)
                , new Version(UseWizardVersion));

            NewProjectLabel = $"New Project (w:{UseWizardVersion}; t:{UseTemplatesVersion})...";
            IsWizardVersionReconfigurable = false;
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
