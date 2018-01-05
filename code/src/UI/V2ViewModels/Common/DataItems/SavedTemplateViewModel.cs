// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class SavedTemplateViewModel : Observable
    {
        private string _name;
        private string _icon;
        private bool _itemNameEditable;
        private bool _isFocused;
        private ICommand _textChangedCommand;

        public string Name
        {
            get => _name;
            set
            {
                if (ItemNameEditable)
                {
                    var validationResult = ValidationService.ValidateTemplateName(value, ItemNameEditable, true);
                    if (validationResult.IsValid)
                    {
                        SetProperty(ref _name, value);
                        NotificationsControl.Instance.CleanNotificationsAsync(Category.NamingValidation).FireAndForget();
                    }
                    else
                    {
                        var notification = Notification.Error("Quieto cobarde!!!!", Category.NamingValidation);
                        NotificationsControl.Instance.AddNotificationAsync(notification).FireAndForget();
                    }
                }
            }
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

        public bool IsFocused
        {
            get => _isFocused;
            set => SetProperty(ref _isFocused, value);
        }

        public ICommand TextChangedCommand => _textChangedCommand ?? (_textChangedCommand = new RelayCommand<string>(OnTextChanged));

        public SavedTemplateViewModel(TemplateInfoViewModel template)
        {
            Icon = template.Icon;
            ItemNameEditable = template.ItemNameEditable;
        }

        public void UpdateSelection()
        {
            IsFocused = true;
        }

        private void OnTextChanged(string text)
        {
            Name = text;
        }
    }
}
