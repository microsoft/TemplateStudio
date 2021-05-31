// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class TemplateGroupViewModel : Observable
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string AccessibleName
        {
            get => !string.IsNullOrEmpty(_name) ? _name : "templateview";
        }

        public ObservableCollection<TemplateInfoViewModel> Items { get; }

        public TemplateGroupViewModel(IGrouping<string, TemplateInfoViewModel> templateGroup)
        {
            Name = GetName(templateGroup.Key);
            Items = new ObservableCollection<TemplateInfoViewModel>(templateGroup);
        }

        private string GetName(string groupName) => StringRes.ResourceManager.GetString($"TemplateGroup_{groupName}", CultureInfo.CurrentUICulture);

        public TemplateInfoViewModel GetTemplate(TemplateInfo templateInfo)
        {
            return Items.FirstOrDefault(t => t.Name == templateInfo.Name);
        }

        public void ClearIsSelected()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
        }
    }
}
