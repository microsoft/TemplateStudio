// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class TemplateInfoViewModel : BasicInfoViewModel
    {
        public string Group { get; }

        public TemplateType TemplateType { get; }

        public bool ItemNameEditable { get; }

        public ICommand ItemClickCommand { get; }

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            // BasicInfo properties
            Name = template.Name;
            Title = template.Name;
            Description = template.Description;
            Author = template.Author;
            Icon = template.GetIcon();
            Order = template.GetDisplayOrder();

            // ITemplateInfo properties
            Group = template.GetGroup();
            TemplateType = template.GetTemplateType();
            ItemNameEditable = template.GetItemNameEditable();
            ItemClickCommand = new RelayCommand(OnItemClick);
        }

        private void OnItemClick() => EventService.Instance.RaiseOnTemplateClicked(this);

        public void UpdateSelection(SavedTemplateViewModel newTemplate)
        {
            // TODO mvegaca: Add visual efects on new template
        }
    }
}
