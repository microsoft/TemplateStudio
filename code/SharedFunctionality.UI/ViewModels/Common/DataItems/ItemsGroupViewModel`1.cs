// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class ItemsGroupViewModel<T> : Observable
        where T : Selectable
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        public ItemsGroupViewModel(string title, IEnumerable<T> items)
        {
            Title = title;
            Items.AddRange(items);
        }

        internal void CleanSelected()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
        }
    }
}
