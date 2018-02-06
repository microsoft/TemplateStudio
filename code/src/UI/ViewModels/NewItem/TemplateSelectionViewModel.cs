// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateSelectionViewModel : Observable
    {
        private string _name;
        private bool _nameEditable;
        private bool _hasErrors;

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

        public ITemplateInfo Template { get; private set; }

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ObservableCollection<BasicInfoViewModel> Dependencies { get; } = new ObservableCollection<BasicInfoViewModel>();

        public TemplateSelectionViewModel()
        {
        }

        public void LoadData(TemplateType templateType, string framework)
        {
            DataService.LoadTemplateGroups(Groups, templateType, framework);
            var group = Groups.FirstOrDefault();
            if (group != null)
            {
                AddTemplate(group.Items.FirstOrDefault());
            }
        }

        public void AddTemplate(TemplateInfoViewModel template)
        {
            if (template != null)
            {
                foreach (var group in Groups)
                {
                    group.ClearIsSelected();
                }

                template.IsSelected = true;
                NameEditable = template.ItemNameEditable;
                Name = ValidationService.InferTemplateName(template.Name, false, template.ItemNameEditable);
                Template = template.Template;
                var licenses = GenComposer.GetAllLicences(template.Template, MainViewModel.Instance.ConfigFramework);
                LicensesService.SyncLicenses(licenses, Licenses);
                Dependencies.Clear();
                foreach (var dependency in template.Dependencies)
                {
                    Dependencies.Add(dependency);
                }

                OnPropertyChanged("Licenses");
                OnPropertyChanged("Dependencies");
            }
        }

        private void SetName(string newName)
        {
            if (NameEditable)
            {
                var validationResult = ValidationService.ValidateTemplateName(newName, NameEditable, false);
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
            }

            SetProperty(ref _name, newName, nameof(Name));
        }
    }
}
