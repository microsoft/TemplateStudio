// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Extensions;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class TemplateSelectionViewModel : Observable
    {
        private string _name;
        private bool _nameEditable;
        private bool _hasErrors;
        private ICommand _textChangedCommand;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ObservableCollection<BasicInfoViewModel> Dependencies { get; } = new ObservableCollection<BasicInfoViewModel>();

        public ICommand TextChangedCommand => _textChangedCommand ?? (_textChangedCommand = new RelayCommand<TextChangedEventArgs>(OnTextChanged));

        public TemplateSelectionViewModel()
        {
            EventService.Instance.OnTemplateSelected += OnTemplateSelected;
        }

        public void LoadData(TemplateType templateType, string framework)
        {
            DataService.LoadTemplateGroups(Groups, templateType, framework);
            var group = Groups.FirstOrDefault();
            if (group != null)
            {
                group.Selected = group.Items.FirstOrDefault();
            }
        }

        private void OnTemplateSelected(object sender, TemplateInfoViewModel template)
        {
            if (template != null)
            {
                foreach (var group in Groups)
                {
                    if (!group.Items.Any(g => g == template))
                    {
                        group.Selected = null;
                    }
                }

                NameEditable = template.ItemNameEditable;
                Name = ValidationService.InferTemplateName(template.Name, false, template.ItemNameEditable);
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

        private void OnTextChanged(TextChangedEventArgs args)
        {
            var textBox = args.Source as TextBox;
            if (textBox != null)
            {
                if (NameEditable)
                {
                    var validationResult = ValidationService.ValidateTemplateName(textBox.Text, NameEditable, false);
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
    }
}
