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
        private DispatcherTimer _resetSelectionTimer;

        public MetadataInfoViewModel Selected
        {
            get => _selected;
            set
            {
                _origValue = _selected;
                if (value != _selected)
                {
                    _selected = value;
                    if (_isSelectionEnabled())
                    {
                        OnPropertyChanged("Selected");
                        EventService.Instance.RaiseOnFrameworkChanged(value);
                    }
                    else
                    {
                        _resetSelectionTimer.Start();
                    }
                }
            }
        }

        public ObservableCollection<MetadataInfoViewModel> Items { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public FrameworkViewModel(Func<bool> isSelectionEnabled)
        {
            _isSelectionEnabled = isSelectionEnabled;
            _resetSelectionTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Application.Current.Dispatcher);
            _resetSelectionTimer.Interval = TimeSpan.FromMilliseconds(1);
            _resetSelectionTimer.Tick += OnResetSelection;
        }

        public void LoadData(string projectTypeName)
        {
            if (DataService.LoadFrameworks(Items, projectTypeName))
            {
                Selected = Items.First();
            }
        }

        private void OnResetSelection(object sender, EventArgs e)
        {
            _selected = _origValue;
            OnPropertyChanged("Selected");
            _resetSelectionTimer.Stop();
        }
    }
}
