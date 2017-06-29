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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Generation;
using Microsoft.Templates.UI.Views.NewItem;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ProjectConfigurationViewModel : Observable
    {
        private ProjectConfigurationWindow _window;
        public ICommand OkCommand => new RelayCommand(OnOkCommand);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public ObservableCollection<string> ProjectTypes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Frameworks { get; } = new ObservableCollection<string>();

        private string _selectedProjectType;
        public string SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                SetProperty(ref _selectedProjectType, value);
                LoadFrameworks();
            }
        }

        private string _selectedFramework;
        public string SelectedFramework
        {
            get => _selectedFramework;
            set => SetProperty(ref _selectedFramework, value);
        }

        public ProjectConfigurationViewModel(ProjectConfigurationWindow window)
        {
            _window = window;
        }

        public void Initialize()
        {
            ProjectTypes.AddRange(GenContext.ToolBox.Repo.GetProjectTypes().Select(f => f.DisplayName));
            SelectedProjectType = ProjectTypes.FirstOrDefault();
        }

        private void LoadFrameworks()
        {
            var projectFrameworks = GenComposer.GetSupportedFx(SelectedProjectType);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                                .Select(f => f.DisplayName)
                                                                .ToList();
            Frameworks.Clear();
            Frameworks.AddRange(targetFrameworks);
            if (SelectedFramework == null)
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }
        }

        private void OnOkCommand()
        {
            ProjectConfigInfo.SaveProjectConfiguration(SelectedProjectType, SelectedFramework);
            _window.DialogResult = true;
            _window.Close();
        }
        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}
