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
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using Microsoft.Templates.Core.Mvvm;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;
using System.Collections.Generic;

namespace Microsoft.Templates.UI.ViewModels
{
    public class SavedTemplateViewModel : Observable
    {
        public static string SettingsButton = Char.ConvertFromUtf32(0xE713);
        public static string CloseButton = Char.ConvertFromUtf32(0xE013);

        #region TemplatesProperties

        private ITemplateInfo _template;
        public (string name, ITemplateInfo template) UserSelection => (ItemName, _template);

        private string _identity;
        public string Identity
        {
            get => _identity;
            set => SetProperty(ref _identity, value);
        }

        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set
            {
                SetProperty(ref _itemName, value);
                OnPropertyChanged("DisplayText");
                ItemForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
                AuthorForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
                colorTimer.Start();
            }
        }
        private string _newItemName;
        public string NewItemName
        {
            get
            {
                if (String.IsNullOrEmpty(_newItemName))
                {
                    _newItemName = ItemName;
                }
                return _newItemName;
            }
            set
            {
                SetProperty(ref _newItemName, value);
                if (CanChooseItemName)
                {
                    ValidateTemplateName?.Invoke(this);
                }
            }
        }

        private bool _canChooseItemName;
        public bool CanChooseItemName
        {
            get => _canChooseItemName;
            set => SetProperty(ref _canChooseItemName, value);
        }

        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
        }

        private List<string> _dependencyList;
        public List<string> DependencyList
        {
            get => _dependencyList;
            set => SetProperty(ref _dependencyList, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private TemplateType _templateType;
        public TemplateType TemplateType
        {
            get => _templateType;
            set => SetProperty(ref _templateType, value);
        }

        private bool _isRemoveEnabled;
        public bool IsRemoveEnabled
        {
            get => _isRemoveEnabled;
            set => SetProperty(ref _isRemoveEnabled, value);
        }       

        private bool _isHome;
        public bool IsHome
        {
            get => _isHome;
            set
            {
                SetProperty(ref _isHome, value);
                ItemFontWeight = value ? FontWeights.Bold : FontWeights.Normal;
                OnPropertyChanged("CanSetHome");
                MainViewModel.Current.CreateCommand.OnCanExecuteChanged();
            }
        }

        public bool CanSetHome
        {
            get
            {
                return !IsHome;
            }
        }
        #endregion

        #region UISummaryProperties
        private DispatcherTimer colorTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };

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

        private bool _isValidName;
        public bool IsValidName
        {
            get => _isValidName;
            set => SetProperty(ref _isValidName, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private FontWeight _itemFontWeight = FontWeights.Normal;
        public FontWeight ItemFontWeight
        {
            get => _itemFontWeight;
            set => SetProperty(ref _itemFontWeight, value);
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                SetProperty(ref _isOpen, value);
                OpenIcon = value ? CloseButton : SettingsButton;
            }
        }

        private Brush _itemForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
        public Brush ItemForeground
        {
            get => _itemForeground;
            set => SetProperty(ref _itemForeground, value);
        }

        private Brush _authorForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
        public Brush AuthorForeground
        {
            get => _authorForeground;
            set => SetProperty(ref _authorForeground, value);
        }

        private string _openIcon = SettingsButton;
        public string OpenIcon
        {
            get => _openIcon;
            private set => SetProperty(ref _openIcon, value);
        }

        private bool _canMoveUp;
        public bool CanMoveUp
        {
            get => _canMoveUp;
            set => SetProperty(ref _canMoveUp, value);
        }

        private bool _canMoveDown;
        public bool CanMoveDown
        {
            get => _canMoveDown;
            set => SetProperty(ref _canMoveDown, value);
        }

        public string DisplayText => CanChooseItemName ? ItemName : $"{ItemName} [{TemplateName}]";        
        
        public ICommand OpenCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand SetHomeCommand { get; set; }
        
        public ICommand RenameCommand { get; set; }

        public ICommand ConfirmRenameCommand { get; set; }

        public ICommand MoveUpCommand { get; set; }

        public ICommand MoveDownCommand { get; set; }        

        public Action<SavedTemplateViewModel> ValidateTemplateName;        

        public ICommand _cancelRenameCommand;
        public ICommand CancelRenameCommand => _cancelRenameCommand ?? (_cancelRenameCommand = new RelayCommand(CancelRenameAction));

        public Action CancelRenameAction => OnCancelRename;        
        #endregion

        public SavedTemplateViewModel((string name, ITemplateInfo template) item, bool isRemoveEnabled, ICommand openCommand, ICommand removeTemplateCommand, ICommand summaryItemSetHomeCommand, ICommand renameItemCommand, ICommand confirmRenameCommand, ICommand moveUpCommand, ICommand moveDownCommand, Action<SavedTemplateViewModel> validateCurrentTemplateName)
        {
            _template = item.template;
            colorTimer.Tick += OnColorTimerTick;
            ItemName = item.name;
            Author = item.template.Author;
            TemplateType = item.template.GetTemplateType();
            CanChooseItemName = item.template.GetItemNameEditable();
            Identity = item.template.Identity;
            TemplateName = item.template.Name;
            DependencyList = item.template.GetDependencyList();
            IsRemoveEnabled = isRemoveEnabled;
            OpenCommand = openCommand;
            RemoveCommand = removeTemplateCommand;
            SetHomeCommand = summaryItemSetHomeCommand;
            RenameCommand = renameItemCommand;
            ConfirmRenameCommand = confirmRenameCommand;
            MoveUpCommand = moveUpCommand;
            MoveDownCommand = moveDownCommand;
            ValidateTemplateName = validateCurrentTemplateName;        
        }

        private void OnColorTimerTick(object sender, EventArgs e)
        {
            ItemForeground = MainViewModel.Current.MainView.FindResource("UIBlack") as SolidColorBrush;
            AuthorForeground = MainViewModel.Current.MainView.FindResource("UIGray") as SolidColorBrush;

            colorTimer.Stop();
        }
        
        internal void TryClose(bool force = false)
        {
            if (IsOpen)
            {
                 IsOpen = false;
            }            
        }

        public void TryReleaseHome()
        {
            if (_isHome)
            {
                IsHome = false;
            }
        }

        public void OnCancelRename()
        {
            if (IsEditionEnabled)
            {
                IsEditionEnabled = false;
                _newItemName = string.Empty;
                OnPropertyChanged("NewItemName");
                MainViewModel.Current.CleanStatus(true);
            }            
        }        
    }
}
