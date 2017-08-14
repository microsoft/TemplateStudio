// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Windows;
using System.Windows.Forms;

using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.VsEmulator.LoadProject
{
    public class LoadProjectViewModel : Observable
    {
        private readonly Window _host;

        public LoadProjectViewModel(Window host)
        {
            _host = host;
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand OkCommand => new RelayCommand(SetSelection, CanSelect);
        public RelayCommand BrowseCommand => new RelayCommand(ShowFileDialog);

        private string _solutionpath;
        public string SolutionPath
        {
            get => _solutionpath;
            set => SetProperty(ref _solutionpath, value);
        }

        public void Initialize(string solutionPath)
        {
            if (!string.IsNullOrEmpty(solutionPath))
            {
                SolutionPath = solutionPath;
            }
        }

        private bool CanSelect() => true;

        private void SetSelection()
        {
            _host.DialogResult = true;
            _host.Close();
        }

        private void ShowFileDialog()
        {
            using (var fd = new OpenFileDialog())
            {
                fd.InitialDirectory = Path.GetDirectoryName(SolutionPath);
                fd.Filter = "Solution Files (*.sln)|*.sln";
                var result = fd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fd.FileName))
                {
                    SolutionPath = fd.FileName;
                }
            }
        }
    }
}
