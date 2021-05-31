// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public ObservableCollection<string> AppModels { get; } = new ObservableCollection<string>();

        public ObservableCollection<MetadataInfo> ProjectTypes { get; } = new ObservableCollection<MetadataInfo>();

        public ObservableCollection<MetadataInfo> Frameworks { get; } = new ObservableCollection<MetadataInfo>();

        private bool _isWinUISelected;

        public bool IsWinUISelected
        {
            get => _isWinUISelected;
            set
            {
                SetProperty(ref _isWinUISelected, value);
            }
        }

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
                    if (SelectedPlatform == Core.Platforms.WinUI)
                    {
                        IsWinUISelected = true;
                        LoadAppModels();
                    }
                    else
                    {
                        IsWinUISelected = false;
                        LoadProjectTypes();
                    }
                }
            }
        }

        private string _selectedAppModel;

        public string SelectedAppModel
        {
            get => _selectedAppModel;
            set
            {
                SetProperty(ref _selectedAppModel, value);
                if (_selectedAppModel != null)
                {
                    LoadProjectTypes();
                }
            }
        }

        public string Language { get; set; }

        private void LoadAppModels()
        {
            AppModels.Clear();
            ProjectTypes.Clear();
            Frameworks.Clear();
            AppModels.AddRange(UI.AppModels.GetAllAppModels());
            SelectedAppModel = AppModels.FirstOrDefault();
        }

        private void LoadProjectTypes()
        {
            var context = new UserSelectionContext(Language, SelectedPlatform);
            if (SelectedPlatform == Core.Platforms.WinUI)
            {
                context.AddAppModel(SelectedAppModel);
            }

            var targetProjectTypes = GenContext.ToolBox.Repo.GetProjectTypes(context).ToList();
            ProjectTypes.Clear();
            Frameworks.Clear();
            ProjectTypes.AddRange(targetProjectTypes);
            SelectedProjectType = ProjectTypes.FirstOrDefault();
        }

        public ProjectConfigurationViewModel(string language)
        {
            Title = StringRes.ProjectConfigurationTitleText;
            Description = StringRes.ProjectConfigurationDescriptionText;
            Language = language;
        }

        public void Initialize()
        {
            Platforms.AddRange(Core.Platforms.GetAllPlatforms());
            SelectedPlatform = Platforms.FirstOrDefault();
        }

        private void LoadFrameworks()
        {
            var context = new UserSelectionContext(Language, SelectedPlatform)
            {
                ProjectType = SelectedProjectType.Name,
            };
            if (SelectedPlatform == Core.Platforms.WinUI)
            {
                context.AddAppModel(SelectedAppModel);
            }

            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context).ToList();

            Frameworks.Clear();
            Frameworks.AddRange(targetFrameworks);
            if (SelectedFramework == null)
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }
        }
    }
}
