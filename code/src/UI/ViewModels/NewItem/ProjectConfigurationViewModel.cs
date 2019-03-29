// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ProjectConfigurationViewModel : BaseDialogViewModel
    {
        public ObservableCollection<string> Platforms { get; } = new ObservableCollection<string>();

        public ObservableCollection<MetadataInfo> ProjectTypes { get; } = new ObservableCollection<MetadataInfo>();

        public ObservableCollection<MetadataInfo> Frameworks { get; } = new ObservableCollection<MetadataInfo>();

        private MetadataInfo _selectedProjectType;

        public MetadataInfo SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                SetProperty(ref _selectedProjectType, value);
                if (value != null)
                {
                    LoadFrameworks();
                }
            }
        }

        private MetadataInfo _selectedFramework;

        public MetadataInfo SelectedFramework
        {
            get => _selectedFramework;
            set => SetProperty(ref _selectedFramework, value);
        }

        private string _selectedPlatform;

        public string SelectedPlatform
        {
            get => _selectedPlatform;
            set
            {
                SetProperty(ref _selectedPlatform, value);
                if (_selectedPlatform != null)
                {
                    LoadProjectTypes();
                }
            }
        }

        private void LoadProjectTypes()
        {
            var targetProjectTypes = GenContext.ToolBox.Repo.GetProjectTypes(SelectedPlatform).ToList();
            ProjectTypes.Clear();
            Frameworks.Clear();
            ProjectTypes.AddRange(targetProjectTypes);
            SelectedProjectType = ProjectTypes.FirstOrDefault();
        }

        public ProjectConfigurationViewModel()
        {
            Title = StringRes.ProjectConfigurationTitleText;
            Description = StringRes.ProjectConfigurationDescriptionText;
        }

        public void Initialize()
        {
            Platforms.AddRange(Core.Platforms.GetAllPlatforms());
            SelectedPlatform = Platforms.FirstOrDefault();
        }

        private void LoadFrameworks()
        {
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(SelectedPlatform, SelectedProjectType.Name).ToList();

            Frameworks.Clear();
            Frameworks.AddRange(targetFrameworks);
            if (SelectedFramework == null)
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }
        }
    }
}
