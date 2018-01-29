// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Extensions;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class SavedTemplateViewModel : Observable
    {
        private string _name;
        private string _icon;
        private bool _itemNameEditable;
        private bool _isHidden;
        private bool _hasErrors;
        private bool _isHome;
        private bool _isReorderEnabled;
        private bool _isDragging;
        private ICommand _textChangedCommand;
        private ICommand _lostKeyboardFocusCommand;
        private RelayCommand _deleteCommand;

        public ITemplateInfo Template { get; }

        public string Identity { get; }

        public TemplateType TemplateType { get; }

        public int GenGroup { get; }

        public IEnumerable<BasicInfoViewModel> Dependencies { get; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public bool ItemNameEditable
        {
            get => _itemNameEditable;
            set => SetProperty(ref _itemNameEditable, value);
        }

        public bool IsHidden
        {
            get => _isHidden;
            set => SetProperty(ref _isHidden, value);
        }

        public bool HasErrors
        {
            get => _hasErrors;
            set => SetProperty(ref _hasErrors, value);
        }

        public bool IsHome
        {
            get => _isHome;
            private set
            {
                SetProperty(ref _isHome, value);
                DeleteCommand.OnCanExecuteChanged();
            }
        }

        public bool IsDragging
        {
            get => _isDragging;
            set => SetProperty(ref _isDragging, value);
        }

        public bool IsReorderEnabled
        {
            get => _isReorderEnabled;
            set => SetProperty(ref _isReorderEnabled, value);
        }

        public TemplateOrigin TemplateOrigin { get; }

        public ICommand TextChangedCommand => _textChangedCommand ?? (_textChangedCommand = new RelayCommand<TextChangedEventArgs>(OnTextChanged));

        public ICommand LostKeyboardFocusCommand => _lostKeyboardFocusCommand ?? (_lostKeyboardFocusCommand = new RelayCommand<KeyboardFocusChangedEventArgs>(OnLostKeyboardFocus));

        public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(OnDelete, () => !IsHome));

        public SavedTemplateViewModel(TemplateInfoViewModel template, TemplateOrigin templateOrigin)
        {
            Template = template.Template;
            Identity = template.Identity;
            TemplateType = template.TemplateType;
            GenGroup = template.GenGroup;
            Dependencies = template.Dependencies;
            Icon = template.Icon;
            ItemNameEditable = template.ItemNameEditable;
            IsHidden = template.IsHidden;
            TemplateOrigin = templateOrigin;
            IsReorderEnabled = template.TemplateType == TemplateType.Page;
        }

        public void Focus()
        {
            EventService.Instance.RaiseOnSavedTemplateFocused(this);
        }

        private void OnTextChanged(TextChangedEventArgs args)
        {
            var textBox = args.Source as TextBox;
            if (textBox != null)
            {
                if (ItemNameEditable)
                {
                    var validationResult = ValidationService.ValidateTemplateName(textBox.Text, ItemNameEditable, true);
                    HasErrors = !validationResult.IsValid;
                    MainViewModel.Instance.WizardStatus.HasValidationErrors = !validationResult.IsValid;
                    if (validationResult.IsValid)
                    {
                        NotificationsControl.Instance.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation).FireAndForget();
                    }
                    else
                    {
                        NotificationsControl.Instance.AddNotificationAsync(validationResult.GetNotification()).FireAndForget();
                    }

                    Name = textBox.Text;
                }
            }
        }

        private void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (HasErrors)
            {
                var textBox = args.Source as TextBox;
                textBox.Focus();
            }
        }

        public void OnDelete()
        {
            if (!IsHome)
            {
                EventService.Instance.RaiseOnDeleteTemplateClicked(this);
            }
        }

        public override bool Equals(object obj)
        {
            var result = false;
            if (obj is SavedTemplateViewModel savedTemplate)
            {
                result = Identity.Equals(savedTemplate.Identity) && Name.Equals(savedTemplate.Name);
            }
            else if (obj is TemplateInfoViewModel templateInfo)
            {
                result = Identity.Equals(templateInfo.Identity);
            }

            return result;
        }

        public override int GetHashCode() => base.GetHashCode();

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
        public (string name, ITemplateInfo template) GetUserSelection() => (Name, Template);
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

        public void SetHome(bool isHome) => IsHome = isHome;
    }
}
