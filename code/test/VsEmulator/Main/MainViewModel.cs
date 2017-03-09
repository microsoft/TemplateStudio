using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Error;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;


namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable
    {
        private readonly Window _host;

        public MainViewModel(Window host)
        {
            _host = host;

            GenContext.Bootstrap(new TemplatesRepository(new LocalTemplatesLocation()), new FakeGenShell(msg => SetStateAsync(msg), _host));
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

        public string SolutionPath { get; set; }

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
                    var outputPath = Path.Combine(newProjectInfo.location, newProjectInfo.name, newProjectInfo.name);
                    using (var context = GenContext.CreateNew(newProjectInfo.name, outputPath))
                    {
                        var userSelection = GenController.GetUserSelection(WizardSteps.Project);
                        if (userSelection != null)
                        {
                            SolutionName = null;

                            GenController.Generate(userSelection);

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


        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start(SolutionPath);
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
