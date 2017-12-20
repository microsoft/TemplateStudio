// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class FrameworkViewModel : Observable
    {
        private MetadataInfoViewModel _selected;
        private Func<bool> _isSelectionEnabled;

        public MetadataInfoViewModel Selected
        {
            get => _selected;
            set
            {
                var origValue = _selected;
                if (value == _selected)
                {
                    return;
                }

                _selected = value;

                if (!_isSelectionEnabled())
                {
                    SafeThreading.JoinableTaskFactory.Run(async () =>
                    {
                        await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            _selected = origValue;
                            OnPropertyChanged("Selected");
                        }), DispatcherPriority.ContextIdle, null);
                    });
                }
                else
                {
                    OnPropertyChanged("Selected");
                    EventService.Instance.RaiseOnFrameworkChanged(value);
                }
            }
        }

        public ObservableCollection<MetadataInfoViewModel> Items { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public FrameworkViewModel(Func<bool> isSelectionEnabled)
        {
            _isSelectionEnabled = isSelectionEnabled;
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
