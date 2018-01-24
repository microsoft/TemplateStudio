// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class ItemsGroupViewModel<T> : Observable
        where T : class
    {
        private string _title;
        private T _selected;
        private Action<ItemsGroupViewModel<T>> _onItemChanged;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public T Selected
        {
            get => _selected;
            set
            {
                SetProperty(ref _selected, value);
                _onItemChanged?.Invoke(this);
            }
        }

        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        public ItemsGroupViewModel(string title, IEnumerable<T> items, Action<ItemsGroupViewModel<T>> onItemChanged)
        {
            Title = title;
            Items.AddRange(items);
            _onItemChanged = onItemChanged;
        }

        public void SelectFirstItem()
        {
            if (Items.Any())
            {
                Selected = Items.First();
            }
        }

        internal void CleanSelected()
        {
            if (_selected != default(T))
            {
                _selected = null;
                OnPropertyChanged("Selected");
            }
        }
    }
}
