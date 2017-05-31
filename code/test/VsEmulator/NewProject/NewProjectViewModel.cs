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
using System.Linq;
using System.Windows;
using System.Windows.Forms;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System.Collections.Generic;

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
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private string _solutionName;
        public string SolutionName
        {
            get => _solutionName;
            set => SetProperty(ref _solutionName, value);
        }

        private bool _createDirectory;
        public bool CreateDirectory
        {
            get => _createDirectory;
            set => SetProperty(ref _createDirectory, value);
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
            
            var validator = new List<Validator>()
            {
                new DirectoryExistsValidator(path)
            };

            return Naming.Infer(DefaultName, validator);
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
