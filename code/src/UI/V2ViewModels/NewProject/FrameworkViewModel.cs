// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class FrameworkViewModel : Observable
    {
        private MetadataInfoViewModel _selected;
        private MetadataInfoViewModel _origValue;
        private Func<bool> _isSelectionEnabled;

        public MetadataInfoViewModel Selected
        {
            get => _selected;
        }

        public ObservableCollection<MetadataInfoViewModel> Items { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public FrameworkViewModel(Func<bool> isSelectionEnabled)
        {
            _isSelectionEnabled = isSelectionEnabled;
        }

        public void LoadData(string projectTypeName)
        {
            _selected = null;
            if (DataService.LoadFrameworks(Items, projectTypeName))
            {
                BaseMainViewModel.BaseInstance.ProcessItem(Items.First());
            }
        }

        public bool Select(MetadataInfoViewModel value)
        {
            if (value != null)
            {
                _origValue = _selected;
                if (value != _selected)
                {
                    _selected = value;
                    if (_isSelectionEnabled())
                    {
                        foreach (var item in Items)
                        {
                            item.IsSelected = false;
                        }

                        _selected.IsSelected = true;
                        OnPropertyChanged("Selected");
                        return true;
                    }
                    else
                    {
                        _selected = _origValue;
                        OnPropertyChanged("Selected");
                    }
                }
            }

            return false;
        }
    }
}
