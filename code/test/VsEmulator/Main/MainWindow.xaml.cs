using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.VsEmulator.Mvvm;
using Microsoft.Templates.VsEmulator.ProjectConfirm;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Microsoft.Templates.VsEmulator.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            ViewModel = new MainViewModel(this);
            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize();
            };
        }
    }

    public class MainViewModel : Observable
    {
        private const string DefaultName = "App";

        private GenController _gen;
        private FakeGenShell _shell;

        private readonly Window _host;

        public MainViewModel(Window host)
        {
            _host = host;
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand CreateCommand => new RelayCommand(Create, CanCreate);
        public RelayCommand BrowseCommand => new RelayCommand(ShowFileDialog);

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        private string _solutionName;
        public string SolutionName
        {
            get { return _solutionName; }
            set { SetProperty(ref _solutionName, value); }
        }

        private bool _createDirectory;
        public bool CreateDirectory
        {
            get { return _createDirectory; }
            set { SetProperty(ref _createDirectory, value); }
        }

        public void Initialize()
        {
            Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Visual Studio 2017", "Projects");

            SetName();

            CreateDirectory = true;
        }

        private void SetName()
        {
            Name = GetSuggestedSolution(Location);
            SolutionName = Name;
        }

        private bool CanCreate() => true;

        private void Create()
        {
            try
            {

                _shell = new FakeGenShell(_name, _location, _solutionName, msg => SetStateAsync(msg), _host);
                _gen = new GenController(_shell, new TemplatesRepository(new LocalTemplatesLocation()));

                var userSelection = _gen.GetUserSelection(WizardSteps.Project);
                if (userSelection != null)
                {
                    _gen.Generate(userSelection);

                    _shell.ShowStatusBarMessage("Project created!!!");

                    ShowConfirmation();
                    SetName();
                }
            }
            catch (WizardBackoutException)
            {
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Unexpected Exception: {ex.ToString()}", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowConfirmation()
        {
            var confirmation = new ProjectConfirmationView(Name, _shell?.SolutionPath)
            {
                Owner = _host
            };
            confirmation.ShowDialog();
        }

        private static string GetSuggestedSolution(string path)
        {
            var existing = Directory.EnumerateDirectories(path)
                                            .Select(d => new DirectoryInfo(d).Name)
                                            .ToList();

            return Naming.Infer(existing, DefaultName);
        }

        private void ShowFileDialog()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Location = fbd.SelectedPath;
                    SetName();
                }
            }
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
