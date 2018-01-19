// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class TemplateGroupViewModel : Observable
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private TemplateInfoViewModel _selected;

        public TemplateInfoViewModel Selected
        {
            get => _selected;
            set
            {
                SetProperty(ref _selected, value);
                EventService.Instance.RaiseOnTemplateSelected(value);
            }
        }

        public ObservableCollection<TemplateInfoViewModel> Items { get; }

        public TemplateGroupViewModel(IGrouping<string, TemplateInfoViewModel> templateGroup)
        {
            Name = templateGroup.Key;
            Items = new ObservableCollection<TemplateInfoViewModel>(templateGroup);
        }

        public TemplateInfoViewModel GetTemplate(ITemplateInfo templateInfo)
        {
            return Items.FirstOrDefault(t => t.Name == templateInfo.Name);
        }
    }
}
