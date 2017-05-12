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

namespace Microsoft.Templates.UI.ViewModels
{
    public class SummaryItemViewModel : Observable
    {
        public static string SettingsButton = Char.ConvertFromUtf32(0xE713);
        public static string CloseButton = Char.ConvertFromUtf32(0xE013);

        #region TemplatesProperties
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

        public string DisplayText => CanChooseItemName ? ItemName : $"{ItemName} [{TemplateName}]";

        public ICommand OpenCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand SetHomeCommand { get; set; }
        
        public ICommand EditCommand { get; set; }

        public Action MouseLeaveAction => TryClose;

        public Action<SummaryItemViewModel> ValidateTemplateName;
        #endregion                               

        private DispatcherTimer dt;
        public SummaryItemViewModel(TemplateSelection item, bool isRemoveEnabled, ICommand removeTemplateCommand, ICommand summaryItemOpenCommand, ICommand summaryItemSetHomeCommand, ICommand editSummaryItemCommand, Action<SummaryItemViewModel> validateCurrentTemplateName)
        {
            ItemName = item.Name;
            Author = item.Template.Author;
            TemplateType = item.Template.GetTemplateType();
            CanChooseItemName = !item.Template.GetItemNameEditable();
            Identity = item.Template.Identity;
            TemplateName = item.Template.Name;
            IsRemoveEnabled = isRemoveEnabled;            
            RemoveCommand = removeTemplateCommand;
            OpenCommand = summaryItemOpenCommand;
            SetHomeCommand = summaryItemSetHomeCommand;
            EditCommand = editSummaryItemCommand;
            ValidateTemplateName = validateCurrentTemplateName;            
            dt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            dt.Tick += OnTimerTick;

            dt.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            ItemForeground = MainViewModel.Current.MainView.FindResource("UIBlack") as SolidColorBrush;
            AuthorForeground = MainViewModel.Current.MainView.FindResource("UIGray") as SolidColorBrush;

            dt.Stop();
        }

        internal void TryClose()
        {
            if (_isOpen)
            {
                IsOpen = false;
            }
        }

        public void CloseEdition()
        {
            if (IsEditionEnabled)
            {
                IsEditionEnabled = false;
            }
        }

        internal void TryReleaseHome()
        {
            if (_isHome)
            {
                IsHome = false;
            }
        }        
    }
}
