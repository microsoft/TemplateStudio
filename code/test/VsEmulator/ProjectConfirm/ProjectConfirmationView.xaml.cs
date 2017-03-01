using Microsoft.Templates.VsEmulator.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Templates.VsEmulator.ProjectConfirm
{
    /// <summary>
    /// Interaction logic for ProjectConfirmationView.xaml
    /// </summary>
    public partial class ProjectConfirmationView : Window
    {
        public ProjectConfirmationViewModel ViewModel { get; set; }

        public ProjectConfirmationView(string name, string path)
        {
            ViewModel = new ProjectConfirmationViewModel(this, name, path);

            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize();
            };
        }
    }

    public class ProjectConfirmationViewModel : Observable
    {
        private readonly Window _host;
        private readonly string _name;
        private readonly string _path;

        public ProjectConfirmationViewModel(Window host, string name, string path)
        {
            _host = host;
            _name = name;
            _path = path;
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);
        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);

        private string _solutionName;
        public string SolutionName
        {
            get { return _solutionName; }
            set { SetProperty(ref _solutionName, value); }
        }

        public void Initialize()
        {
            SolutionName = _name;
        }

        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(_path))
            {
                System.Diagnostics.Process.Start(_path); 
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(_path))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(_path)); 
            }
        }
    }
}
