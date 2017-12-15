// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class TemplateSelectionViewModel : Observable
    {
        public ObservableCollection<SavedTemplateViewModel> Pages { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<SavedTemplateViewModel> Features { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public TemplateSelectionViewModel()
        {
        }

        public IEnumerable<string> GetNames() => Pages.Select(t => t.Name)
                                                    .Concat(Features.Select(f => f.Name));

        public SavedTemplateViewModel Add(TemplateInfoViewModel template)
        {
            var items = template.TemplateType == Core.TemplateType.Page ? Pages : Features;

            var newItem = new SavedTemplateViewModel()
            {
                Icon = template.Icon,
                Name = ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, true)
            };
            items.Add(newItem);
            return newItem;
        }
    }
}
