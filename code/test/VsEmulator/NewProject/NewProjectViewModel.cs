using Microsoft.Templates.Core;
using Microsoft.Templates.VsEmulator.Mvvm;
using Microsoft.Templates.Wizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Templates.VsEmulator.NewProject
{
    public class NewProjectViewModel : Observable
    {
        private const string DefaultName = "App";

        private readonly Window _host;

        public NewProjectViewModel(Window host)
        {
            _host = host;
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand OkCommand => new RelayCommand(SetSelection, CanSelect);
        public RelayCommand BrowseCommand => new RelayCommand(ShowFileDialog);

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

        private bool CanSelect() => true;

        private void SetSelection()
        {
            _host.DialogResult = true;
            _host.Close();
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
    }
}
