// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ProjectConfigurationViewModel : Observable
    {
        private ProjectConfigurationWindow _window;
        public ICommand OkCommand => new RelayCommand(OnOkCommand);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public ObservableCollection<MetadataInfo> ProjectTypes { get; } = new ObservableCollection<MetadataInfo>();
        public ObservableCollection<MetadataInfo> Frameworks { get; } = new ObservableCollection<MetadataInfo>();

        private MetadataInfo _selectedProjectType;
        public MetadataInfo SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                SetProperty(ref _selectedProjectType, value);
                LoadFrameworks();
            }
        }

        private MetadataInfo _selectedFramework;
        public MetadataInfo SelectedFramework
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
            ProjectTypes.AddRange(GenContext.ToolBox.Repo.GetProjectTypes());
            SelectedProjectType = ProjectTypes.FirstOrDefault();
        }

        private void LoadFrameworks()
        {
            var projectFrameworks = GenComposer.GetSupportedFx(SelectedProjectType.Name);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(tf => projectFrameworks.Contains(tf.Name))
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
            ProjectConfigInfo.SaveProjectConfiguration(SelectedProjectType.Name, SelectedFramework.Name);
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
