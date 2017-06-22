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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Views;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels
{
    public class TemplateInfoViewModel : CommonInfoViewModel
    {
        #region TemplateProperties
        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
        }

        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private int _order;
        public int Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
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
        #endregion

        #region UIProperties
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
                    _validateTemplateName?.Invoke(this);
                }
            }
        }

        private bool _isValidName;
        public bool IsValidName
        {
            get => _isValidName;
            set => SetProperty(ref _isValidName, value);
        }

        private Brush _titleForeground = new SolidColorBrush(Colors.Black);
        public Brush TitleForeground
        {
            get => _titleForeground;
            set => SetProperty(ref _titleForeground, value);
        }

        public RelayCommand<TemplateInfoViewModel> AddItemCommand { get; private set; }
        public RelayCommand<TemplateInfoViewModel> SaveItemCommand { get; private set; }

        private ICommand _closeEditionCommand;
        public ICommand CloseEditionCommand => _closeEditionCommand ?? (_closeEditionCommand = new RelayCommand(CloseEdition));

        private ICommand _showItemInfoCommand;
        public ICommand ShowItemInfoCommand => _showItemInfoCommand ?? (_showItemInfoCommand = new RelayCommand(ShowItemInfo));

        private Action<TemplateInfoViewModel> _validateTemplateName;
        #endregion

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies, RelayCommand<TemplateInfoViewModel> addItemCommand, RelayCommand<TemplateInfoViewModel> saveItemCommand, Action<TemplateInfoViewModel> validateTemplateName)
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
            Order = template.GetOrder();
            Summary = template.Description;
            TemplateType = template.GetTemplateType();
            Template = template;
            Version = template.GetVersion();

            AddItemCommand = addItemCommand;
            SaveItemCommand = saveItemCommand;
            _validateTemplateName = validateTemplateName;

            if (dependencies != null && dependencies.Any())
            {
                DependencyItems.AddRange(dependencies.Select(d => new DependencyInfoViewModel(new TemplateInfoViewModel(d, GenComposer.GetAllDependencies(d, MainViewModel.Current.ProjectSetup.SelectedFramework.Name), AddItemCommand, SaveItemCommand, validateTemplateName))));

                Dependencies = string.Join(",", dependencies.Select(d => d.Name));
            }
        }

        public void CloseEdition()
        {
            if (IsEditionEnabled)
            {
                IsEditionEnabled = false;
                MainViewModel.Current.CleanStatus(true);
            }
        }

        public void UpdateTemplateAvailability(bool isAlreadyDefined)
        {
            if (MultipleInstances == false && isAlreadyDefined)
            {
                AddingVisibility = Visibility.Collapsed;
                TitleForeground = MainViewModel.Current.MainView.FindResource("UIMiddleLightGray") as SolidColorBrush;
            }
            else
            {
                AddingVisibility = Visibility.Visible;
                TitleForeground = MainViewModel.Current.MainView.FindResource("UIBlack") as SolidColorBrush;
            }
        }

        private void ShowItemInfo()
        {
            MainViewModel.Current.InfoShapeVisibility = Visibility.Visible;
            var infoView = new InformationWindow(this, MainViewModel.Current.MainView);

            infoView.ShowDialog();
            MainViewModel.Current.InfoShapeVisibility = Visibility.Collapsed;
        }
    }
}
