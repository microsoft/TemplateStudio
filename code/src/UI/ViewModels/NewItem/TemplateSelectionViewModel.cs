// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateSelectionViewModel : Observable
    {
        private readonly string _emptyBackendFramework = string.Empty;

        private string _name;
        private bool _nameEditable;
        private bool _hasErrors;
        private bool _isFocused;
        private bool _isTextSelected;
        private ICommand _setFocusCommand;
        private ICommand _lostKeyboardFocusCommand;

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

        public ObservableCollection<string> RequiredSdks { get; protected set; } = new ObservableCollection<string>();

        public ICommand SetFocusCommand => _setFocusCommand ?? (_setFocusCommand = new RelayCommand(() => IsFocused = true));

        public ICommand LostKeyboardFocusCommand => _lostKeyboardFocusCommand ?? (_lostKeyboardFocusCommand = new RelayCommand<KeyboardFocusChangedEventArgs>(OnLostKeyboardFocus));

        public TemplateSelectionViewModel()
        {
        }

        public void Focus()
        {
            IsTextSelected = true;
        }

        public void LoadData(TemplateType templateType, UserSelectionContext context)
        {
            DataService.LoadTemplatesGroups(Groups, templateType, context, true);

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
                var licenses = GenContext.ToolBox.Repo.GetAllLicences(template.Template.TemplateId, MainViewModel.Instance.ConfigContext);
                LicensesService.SyncLicenses(licenses, Licenses);
                Dependencies.Clear();
                foreach (var dependency in template.Dependencies)
                {
                    Dependencies.Add(dependency);
                }

                RequiredSdks.Clear();
                foreach (var requiredSdk in template.RequiredSdks)
                {
                    RequiredSdks.Add(requiredSdk);
                }

                OnPropertyChanged("Licenses");
                OnPropertyChanged("Dependencies");
                OnPropertyChanged("RequiredSdks");
                CheckForMissingSdks();

                NotificationsControl.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation).FireAndForget();
                WizardStatus.Current.HasValidationErrors = false;
                if (NameEditable)
                {
                    Focus();
                }
            }
        }

        private void CheckForMissingSdks()
        {
            var missingVersions = new List<RequiredVersionInfo>();

            foreach (var requiredVersion in Template.RequiredVersions)
            {
                var requirementInfo = RequiredVersionService.GetVersionInfo(requiredVersion);
                var isInstalled = RequiredVersionService.Instance.IsVersionInstalled(requirementInfo);
                if (!isInstalled)
                {
                    missingVersions.Add(requirementInfo);
                }
            }

            if (missingVersions.Any())
            {
                var missingSdkVersions = missingVersions.Select(v => RequiredVersionService.GetRequirementDisplayName(v));

                var notification = Notification.Warning(string.Format(StringRes.NotificationMissingVersions, missingSdkVersions.Aggregate((i, j) => $"{i}, {j}")), Category.MissingVersion, TimerType.None);
                NotificationsControl.AddNotificationAsync(notification).FireAndForget();
            }
            else
            {
                NotificationsControl.CleanCategoryNotificationsAsync(Category.MissingVersion).FireAndForget();
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
                    NotificationsControl.AddNotificationAsync(validationResult.Errors.FirstOrDefault()?.GetNotification()).FireAndForget();
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
