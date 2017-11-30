// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewProject;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class TemplateInfoViewModel : CommonInfoViewModel
    {
        private string _templateName;

        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
        }

        private bool _multipleInstances;

        public bool MultipleInstances
        {
            get => _multipleInstances;
            set => SetProperty(ref _multipleInstances, value);
        }

        private int _genGroup;

        public int GenGroup
        {
            get => _genGroup;
            set => SetProperty(ref _genGroup, value);
        }

        private string _group;

        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        private string _dependencies;

        public string Dependencies
        {
            get => _dependencies;
            set => SetProperty(ref _dependencies, value);
        }

        private TemplateType _templateType;

        public TemplateType TemplateType
        {
            get => _templateType;
            set => SetProperty(ref _templateType, value);
        }

        public ObservableCollection<DependencyInfoViewModel> DependencyItems { get; } = new ObservableCollection<DependencyInfoViewModel>();

        public ITemplateInfo Template { get; set; }

        private bool _isEditionEnabled;

        public bool IsEditionEnabled
        {
            get => _isEditionEnabled;
            set
            {
                EditingContentVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                NoEditingContentVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                SetProperty(ref _isEditionEnabled, value);
            }
        }

        private bool _canChooseItemName;

        public bool CanChooseItemName
        {
            get => _canChooseItemName;
            set => SetProperty(ref _canChooseItemName, value);
        }

        private Visibility _noEditingContentVisibility = Visibility.Visible;

        public Visibility NoEditingContentVisibility
        {
            get => _noEditingContentVisibility;
            private set => SetProperty(ref _noEditingContentVisibility, value);
        }

        private Visibility _editingContentVisibility = Visibility.Collapsed;

        public Visibility EditingContentVisibility
        {
            get => _editingContentVisibility;
            private set => SetProperty(ref _editingContentVisibility, value);
        }

        private Visibility _addingVisibility = Visibility.Visible;

        public Visibility AddingVisibility
        {
            get => _addingVisibility;
            set => SetProperty(ref _addingVisibility, value);
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private string _newTemplateName;

        public string NewTemplateName
        {
            get => _newTemplateName;
            set
            {
                SetProperty(ref _newTemplateName, value);
                if (CanChooseItemName)
                {
                    var validationResult = ValidationService.ValidateTemplateName(NewTemplateName, CanChooseItemName, true);
                    IsValidName = validationResult.IsValid;
                    ErrorMessage = string.Empty;
                    if (!IsValidName)
                    {
                        ErrorMessage = validationResult.ErrorType.GetResourceString();
                        MainViewModel.Current.SetValidationErrors(ErrorMessage);
                        throw new Exception(ErrorMessage);
                    }

                    MainViewModel.Current.CleanStatus(true);
                }
            }
        }

        private bool _isValidName;

        public bool IsValidName
        {
            get => _isValidName;
            set => SetProperty(ref _isValidName, value);
        }

        private Brush _titleForeground;

        public Brush TitleForeground
        {
            get => _titleForeground;
            set => SetProperty(ref _titleForeground, value);
        }

        private ICommand _addItemCommand;

        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new RelayCommand(OnAddItem));

        private ICommand _saveItemCommand;

        public ICommand SaveItemCommand => _saveItemCommand ?? (_saveItemCommand = new RelayCommand(OnSaveItem));

        private ICommand _closeEditionCommand;

        public ICommand CloseEditionCommand => _closeEditionCommand ?? (_closeEditionCommand = new RelayCommand(() => CloseEdition()));

        private ICommand _showItemInfoCommand;

        public ICommand ShowItemInfoCommand => _showItemInfoCommand ?? (_showItemInfoCommand = new RelayCommand(ShowItemInfo));

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Author = template.Author;
            CanChooseItemName = template.GetItemNameEditable();
            Description = template.GetRichDescription();
            Group = template.GetGroup();
            Icon = template.GetIcon();
            LicenseTerms = template.GetLicenses();
            MultipleInstances = template.GetMultipleInstance();
            GenGroup = template.GetGenGroup();
            Name = template.Name;
            Order = template.GetDisplayOrder();
            Summary = template.Description;
            TemplateType = template.GetTemplateType();
            Template = template;
            Version = template.GetVersion();

            TitleForeground = GetTitleForeground(true);

            if (dependencies != null && dependencies.Any())
            {
                DependencyItems.AddRange(dependencies.Select(d => new DependencyInfoViewModel(new TemplateInfoViewModel(d, GenComposer.GetAllDependencies(d, MainViewModel.Current.ProjectSetup.SelectedFramework.Name)))));

                Dependencies = string.Join(",", dependencies.Select(d => d.Name));
            }
        }

        public bool CloseEdition()
        {
            if (IsEditionEnabled)
            {
                IsEditionEnabled = false;
                MainViewModel.Current.CleanStatus(true);
                return true;
            }

            return false;
        }

        public void UpdateTemplateAvailability(bool isAlreadyDefined)
        {
            if (MultipleInstances == false && isAlreadyDefined)
            {
                AddingVisibility = Visibility.Collapsed;
                TitleForeground = GetTitleForeground(false);
            }
            else
            {
                AddingVisibility = Visibility.Visible;
                TitleForeground = GetTitleForeground(true);
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Summary}";
        }

        private void ShowItemInfo()
        {
            MainViewModel.Current.WizardStatus.InfoShapeVisibility = Visibility.Visible;
            var infoView = new InformationWindow(this, MainViewModel.Current.MainView);

            infoView.ShowDialog();
            MainViewModel.Current.WizardStatus.InfoShapeVisibility = Visibility.Collapsed;
        }

        private SolidColorBrush GetTitleForeground(bool isEnabled)
        {
            if (SystemService.Instance.IsHighContrast)
            {
                if (isEnabled)
                {
                    return SystemColors.ControlTextBrush;
                }
                else
                {
                    return SystemColors.ControlLightBrush;
                }
            }
            else
            {
                if (isEnabled)
                {
                    return ResourceService.FindResource<SolidColorBrush>("UIBlack");
                }
                else
                {
                    return ResourceService.FindResource<SolidColorBrush>("UIMiddleLightGray");
                }
            }
        }

        private void OnAddItem()
        {
            NewTemplateName = ValidationService.InferTemplateName(Template.GetDefaultName(), CanChooseItemName, CanChooseItemName);
            if (CanChooseItemName)
            {
                MainViewModel.Current.ProjectTemplates.CloseAllEditions();
                IsEditionEnabled = true;
            }
            else
            {
                SaveItem();
                UpdateTemplateAvailability(MainViewModel.Current.ProjectTemplates.IsTemplateAlreadyDefined(Template.Identity));
            }
        }

        private void OnSaveItem()
        {
            if (IsValidName)
            {
                SaveItem();
                CloseEdition();
                UpdateTemplateAvailability(MainViewModel.Current.ProjectTemplates.IsTemplateAlreadyDefined(Template.Identity));
            }
        }

        private void SaveItem()
        {
            UserSelectionService.AddTemplateAndDependencies((NewTemplateName, Template), MainViewModel.Current.ProjectTemplates.ContextFramework.Name, false);
            MainViewModel.Current.RebuildLicenses();
            MainViewModel.Current.ProjectTemplates.UpdateTemplatesAvailability();
            MainViewModel.Current.ProjectTemplates.UpdateSummaryTemplates();
            MainViewModel.Current.ProjectTemplates.UpdateHasPagesAndHasFeatures();
        }
    }
}
