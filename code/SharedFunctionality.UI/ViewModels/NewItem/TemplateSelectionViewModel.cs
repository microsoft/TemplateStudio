﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.VisualStudio.TextManager.Interop;

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

        public ObservableCollection<UserSelectionGroup> userSelectionGroups = new ObservableCollection<UserSelectionGroup>();

        public TemplateSelectionViewModel()
        {
            userSelectionGroups.Add(new UserSelectionGroup(TemplateType.Page, Resources.ProjectDetailsPagesSectionTitle, true));
            userSelectionGroups.Add(new UserSelectionGroup(TemplateType.Feature, Resources.ProjectDetailsFeaturesSectionTitle));
            userSelectionGroups.Add(new UserSelectionGroup(TemplateType.Service, Resources.ProjectDetailsServicesSectionTitle));
            userSelectionGroups.Add(new UserSelectionGroup(TemplateType.Testing, Resources.ProjectDetailsTestingSectionTitle));
        }

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ObservableCollection<BasicInfoViewModel> Dependencies { get; } = new ObservableCollection<BasicInfoViewModel>();

        public ObservableCollection<string> RequiredSdks { get; protected set; } = new ObservableCollection<string>();

        public ICommand SetFocusCommand => _setFocusCommand ?? (_setFocusCommand = new RelayCommand(() => IsFocused = true));

        public ICommand LostKeyboardFocusCommand => _lostKeyboardFocusCommand ?? (_lostKeyboardFocusCommand = new RelayCommand<KeyboardFocusChangedEventArgs>(OnLostKeyboardFocus));


        /*public TemplateSelectionViewModel()
        {
        }*/

        public IEnumerable<string> GetPageNames()
            => userSelectionGroups.First(g => g.TemplateType == TemplateType.Page).GetNames(p => p.ItemNameEditable);

        public IEnumerable<string> GetNames()
        {
            var names = new List<string>();
            userSelectionGroups.ToList().ForEach(g => names.AddRange(g.GetNames()));
            return names;
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

        //Add Async called from MainViewModel
        public async Task AddAsync(TemplateOrigin templateOrigin, TemplateInfoViewModel template, string layoutName = null, bool isReadOnly = false)
        {
            //check for exclusions, requirements, and exclusivities
            // get dependencies
            // increase selection (template.IncreaseSelection)
            template.IncreaseSelection();
            var savedTemplate = new SavedTemplateViewModel(template, templateOrigin, isReadOnly);

            // name setting
            if (template.MultipleInstance) // add condition for OR not already added
            {
                if (!string.IsNullOrEmpty(layoutName))
                {
                    savedTemplate.SetName(layoutName, true);
                }
                else
                {
                    if (template.ItemNameEditable)
                    {
                        savedTemplate.SetName(ValidationService.InferTemplateName(template.Name)); // set temp name for template
                        // focus = true ?
                    }
                    else
                    {
                        savedTemplate.SetName(template.Template.DefaultName); // set unchangeable default name
                    }
                }
            }

            // add template to corresponding group (page, feature, or testing
            AddToGroup(template.TemplateType, savedTemplate);
            // UpdateHasItemsAddedByUser();
            // BuildLicenses();
            // CheckForMissingVersions();
            /*if (focus)
                {
                    savedTemplate.IsTextSelected = true;
                }
            }*/
        }

        public UserSelectionGroup GetGroup(TemplateType templateType) => userSelectionGroups.First(t => t.TemplateType == templateType);


        private void AddToGroup(TemplateType templateType, SavedTemplateViewModel savedTemplate)
        {
            bool GenGroupEqual(SavedTemplateViewModel st) => st.GenGroup == savedTemplate.GenGroup;
            bool GenGroupPrevious(SavedTemplateViewModel st) => st.GenGroup < savedTemplate.GenGroup;

            int index = 0;
            var group = GetGroup(templateType);
            if (group.Items.Any(GenGroupEqual))
            {
                index = group.Items.IndexOf(group.Items.Last(GenGroupEqual)) + 1;
            }
            else if (group.Items.Any())
            {
                index = group.Items.IndexOf(group.Items.Last(GenGroupPrevious)) + 1;
            }

            group.Insert(index, savedTemplate);
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
                var licenses = GenContext.ToolBox.Repo.GetAllLicences(template.Template.TemplateId, MainViewModel.Instance.Context);
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

                OnPropertyChanged(nameof(Licenses));
                OnPropertyChanged(nameof(Dependencies));
                OnPropertyChanged(nameof(RequiredSdks));
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

                var notification = Notification.Warning(string.Format(Resources.NotificationMissingVersions, missingSdkVersions.Aggregate((i, j) => $"{i}, {j}")), Category.MissingVersion, TimerType.None);
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
