// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System;
using System.Linq;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class ItemsGroupViewModel<T> : Observable where T : class
    {
        private Action<ItemsGroupViewModel<T>> _onItemChanged;

        private bool _hasItems;
        public bool HasItems
        {
            get => _hasItems;
            set => SetProperty(ref _hasItems, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<T> Templates { get; } = new ObservableCollection<T>();

        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                _onItemChanged?.Invoke(this);
            }
        }

        public ItemsGroupViewModel(string name, IEnumerable<T> templates, Action<ItemsGroupViewModel<T>> onItemChanged = null)
        {
            Name = name;
            Title = GetTitle(name);
            Templates.AddRange<T>(templates);
            HasItems = templates != null && templates.Any();
            _onItemChanged = onItemChanged;
        }

        private string GetTitle(string name) => StringRes.ResourceManager.GetString($"TemplateGroup_{name}");

        internal void CleanSelected()
        {
            if (_selectedItem != default(T))
            {
                _selectedItem = null;
                OnPropertyChanged("SelectedItem");
            }
        }
    }
}
