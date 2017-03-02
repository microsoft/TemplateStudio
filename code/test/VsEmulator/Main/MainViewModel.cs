using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.VsEmulator.Mvvm;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable
    {
        private GenController _gen;
        private FakeGenShell _shell;

        private readonly Window _host;

        public MainViewModel(Window host)
        {
            _host = host;
        }

        public RelayCommand NewProjectCommand => new RelayCommand(NewProject);

        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);
        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private Visibility _isProjectLoaded;
        public Visibility IsProjectLoaded
        {
            get { return _isProjectLoaded; }
            set { SetProperty(ref _isProjectLoaded, value); }
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

        public void Initialize()
        {
            SolutionName = null;
        }

        private void NewProject()
        {
            try
            {
                var newProjectInfo = ShowNewProjectDialog();
                if (!string.IsNullOrEmpty(newProjectInfo.name))
                {
                    _shell = new FakeGenShell(newProjectInfo.name, newProjectInfo.location, newProjectInfo.solutionName, msg => SetStateAsync(msg), _host);
                    _gen = new GenController(_shell, new TemplatesRepository(new LocalTemplatesLocation()));

                    var userSelection = _gen.GetUserSelection(WizardSteps.Project);
                    if (userSelection != null)
                    {
                        _gen.Generate(userSelection);

                        _shell.ShowStatusBarMessage("Project created!!!");

                        SolutionName = newProjectInfo.name;
                    }
                }
            }
            catch (WizardBackoutException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Exception: {ex.ToString()}", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(_shell?.SolutionPath))
            {
                System.Diagnostics.Process.Start(_shell.SolutionPath);
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(_shell?.SolutionPath))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(_shell.SolutionPath));
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

        private void SetStateAsync(string message)
        {
            State = message;
            DoEvents();
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
