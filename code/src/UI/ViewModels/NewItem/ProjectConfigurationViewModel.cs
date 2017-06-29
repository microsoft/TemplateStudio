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

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ProjectConfigurationViewModel : Observable
    {
        public ICommand OkCommand => new RelayCommand(SetProjectConfig);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public ObservableCollection<string> ProjectTypes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Frameworks { get; } = new ObservableCollection<string>();

        private string _projectType;
        public string ProjectType
        {
            get => _projectType;
            set => SetProperty(ref _projectType, value);
        }

        private string _framework;
        public string Framework
        {
            get => _framework;
            set => SetProperty(ref _framework, value);
        }

        public ProjectConfigurationViewModel()
        {
        }

        public void Initialize()
        {
            ProjectTypes.AddRange(GenContext.ToolBox.Repo.GetProjectTypes().Select(f => f.DisplayName));
        }

        private void LoadFrameworks()
        {
            var projectFrameworks = GenComposer.GetSupportedFx(_projectType);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                                .Select(f => f.DisplayName)
                                                                .ToList();
            Frameworks.Clear();
            Frameworks.AddRange(targetFrameworks);
            if (Framework == null)
            {
                Framework = Frameworks.FirstOrDefault();
            }
        }

        private void SetProjectConfig()
        {
            ProjectConfigInfo.SaveProjectConfiguration(ProjectType, Framework);
        }
        private void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
