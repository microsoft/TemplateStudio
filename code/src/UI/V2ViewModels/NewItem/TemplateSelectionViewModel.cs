// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class TemplateSelectionViewModel : Observable
    {
        private string _name;
        private bool _nameEditable;

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

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

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
                Name = ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, template.ItemNameEditable);
                LicensesService.SyncLicenses(template.Template.GetLicenses(), Licenses);
                OnPropertyChanged("Licenses");
            }
        }
    }
}
