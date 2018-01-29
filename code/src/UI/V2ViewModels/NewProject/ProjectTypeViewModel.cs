// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class ProjectTypeViewModel : Observable
    {
        private MetadataInfoViewModel _selected;

        public MetadataInfoViewModel Selected
        {
            get => _selected;
        }

        public ObservableCollection<MetadataInfoViewModel> Items { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public ProjectTypeViewModel()
        {
        }

        public void LoadData()
        {
            _selected = null;
            if (DataService.LoadProjectTypes(Items))
            {
                BaseMainViewModel.BaseInstance.ProcessItem(Items.First());
            }
        }

        public bool Select(MetadataInfoViewModel value)
        {
            if (value != null)
            {
                if (value != _selected)
                {
                    _selected = value;
                    foreach (var item in Items)
                    {
                        item.IsSelected = false;
                    }

                    _selected.IsSelected = true;
                    OnPropertyChanged(nameof(Selected));
                    return true;
                }
            }

            return false;
        }
    }
}
