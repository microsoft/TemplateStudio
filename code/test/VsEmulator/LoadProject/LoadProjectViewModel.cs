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

using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Templates.VsEmulator.LoadProject
{
    public class LoadProjectViewModel: Observable
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
            if (!String.IsNullOrEmpty(solutionPath))
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
