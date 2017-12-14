// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class FrameworkViewModel : Observable
    {
        private MetadataInfoViewModel _selected;

        public MetadataInfoViewModel Selected
        {
            get => _selected;
            set
            {
                SetProperty(ref _selected, value);
                SelectionChanged?.Invoke(this, value);
            }
        }

        public ObservableCollection<MetadataInfoViewModel> Items { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public event EventHandler<MetadataInfoViewModel> SelectionChanged;

        public FrameworkViewModel()
        {
        }

        public void LoadData(string projectTypeName)
        {
            if (DataService.LoadFrameworks(Items, projectTypeName))
            {
                Selected = Items.First();
            }
        }
    }
}
