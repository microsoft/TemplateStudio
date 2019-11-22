// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateSelectionViewModel : Observable
    {
        private string _name;
        private bool _nameEditable;
        private bool _hasErrors;
        private bool _isFocused;
        private bool _isTextSelected;
        private ICommand _setFocusCommand;
        private ICommand _lostKeyboardFocusCommand;
        private string _emptyBackendFramework = string.Empty;

        public string Name
        {
            get => _name;
            set => SetName(value);
        }

        public bool NameEditable
        {
            get => _nameEditable;
            set => SetProperty(ref _nameEditable, value);
        }

        public bool HasErrors
        {
            get => _hasErrors;
            set => SetProperty(ref _hasErrors, value);
        }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                {
                    SetProperty(ref _isFocused, false);
                }

                SetProperty(ref _isFocused, value);
            }
        }

        public bool IsTextSelected
        {
            get => _isTextSelected;
            set
            {
                if (_isTextSelected == value)
                {
                    SetProperty(ref _isTextSelected, false);
                }

                SetProperty(ref _isTextSelected, value);
            }
        }

        public TemplateInfo Template { get; private set; }

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ObservableCollection<BasicInfoViewModel> Dependencies { get; } = new ObservableCollection<BasicInfoViewModel>();

        public ICommand SetFocusCommand => _setFocusCommand ?? (_setFocusCommand = new RelayCommand(() => IsFocused = true));

        public ICommand LostKeyboardFocusCommand => _lostKeyboardFocusCommand ?? (_lostKeyboardFocusCommand = new RelayCommand<KeyboardFocusChangedEventArgs>(OnLostKeyboardFocus));

        public TemplateSelectionViewModel()
        {
        }

        public void Focus()
        {
            IsTextSelected = true;
        }

        public void LoadData(TemplateType templateType, string platform, string projectTypeName, string frameworkName)
        {
            DataService.LoadTemplatesGroups(Groups, templateType, platform, projectTypeName, frameworkName, true);

            var group = Groups.FirstOrDefault();
            if (group != null)
            {
                SelectTemplate(group.Items.FirstOrDefault());
            }
        }

        public void SelectTemplate(TemplateInfoViewModel template)
        {
            if (template != null)
            {
                foreach (var group in Groups)
                {
                    group.ClearIsSelected();
                }

                template.IsSelected = true;
                NameEditable = template.ItemNameEditable;
                if (template.ItemNameEditable)
                {
                    Name = ValidationService.InferTemplateName(template.Name);
                }
                else
                {
                    Name = template.Template.DefaultName;
                }

                HasErrors = false;
                Template = template.Template;
                var licenses = GenContext.ToolBox.Repo.GetAllLicences(template.Template.TemplateId, MainViewModel.Instance.ConfigPlatform, MainViewModel.Instance.ConfigProjectType, MainViewModel.Instance.ConfigFramework, _emptyBackendFramework);
                LicensesService.SyncLicenses(licenses, Licenses);
                Dependencies.Clear();
                foreach (var dependency in template.Dependencies)
                {
                    Dependencies.Add(dependency);
                }

                OnPropertyChanged("Licenses");
                OnPropertyChanged("Dependencies");
                NotificationsControl.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation).FireAndForget();
                WizardStatus.Current.HasValidationErrors = false;
                if (NameEditable)
                {
                    Focus();
                }
            }
        }

        private void SetName(string newName)
        {
            if (NameEditable)
            {
                var validationResult = ValidationService.ValidateTemplateName(newName);
                HasErrors = !validationResult.IsValid;
                MainViewModel.Instance.WizardStatus.HasValidationErrors = !validationResult.IsValid;
                if (validationResult.IsValid)
                {
                    NotificationsControl.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation).FireAndForget();
                }
                else
                {
                    NotificationsControl.AddNotificationAsync(validationResult.GetNotification()).FireAndForget();
                }
            }

            SetProperty(ref _name, newName, nameof(Name));
        }

        private void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (HasErrors)
            {
                var textBox = args.Source as TextBox;
                textBox.Focus();
            }
        }
    }
}
