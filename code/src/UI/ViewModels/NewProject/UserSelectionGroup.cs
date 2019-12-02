// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class UserSelectionGroup : Observable
    {
        private readonly bool _allowsOrdering;
        private string _header;
        private ICommand _moveUpCommand;
        private ICommand _moveDownCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private SavedTemplateViewModel _selectedItem;
        private OrderingService _orderingService;

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        public SavedTemplateViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public TemplateType TemplateType { get; }

        public ObservableCollection<SavedTemplateViewModel> Items { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ICommand MoveUpCommand => _moveUpCommand ?? (_moveUpCommand = new RelayCommand(OnMoveUp));

        public ICommand MoveDownCommand => _moveDownCommand ?? (_moveDownCommand = new RelayCommand(OnMoveDown));

        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand<SavedTemplateViewModel>(OnEdit, CanEdit));

        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand<SavedTemplateViewModel>(OnDelete, CanDelete));

        public UserSelectionGroup(TemplateType templateType, string header, bool allowsOrdering = false)
        {
            TemplateType = templateType;
            Header = header;
            _allowsOrdering = allowsOrdering;
        }

        public IEnumerable<string> GetNames() => Items.Select(i => i.Name);

        public IEnumerable<string> GetNames(Func<SavedTemplateViewModel, bool> predicate) => Items.Where(predicate).Select(i => i.Name);

        public void EnableOrdering(ListView listView)
        {
            if (_allowsOrdering)
            {
                _orderingService = new OrderingService(listView);
            }
        }

        public void UnsubscribeEventHandlers()
        {
            _orderingService?.UnsubscribeEventHandlers();
        }

        public void Remove(SavedTemplateViewModel savedTemplate)
        {
            Items.Remove(savedTemplate);
            OnPropertyChanged(nameof(Items));
        }

        public void Insert(int index, SavedTemplateViewModel savedTemplate)
        {
            Items.Insert(index, savedTemplate);
            OnPropertyChanged(nameof(Items));
        }

        public void Clear()
        {
            Items.Clear();
            OnPropertyChanged(nameof(Items));
        }

        private void OnMoveUp() => _orderingService?.MoveUp(SelectedItem);

        private void OnMoveDown() => _orderingService?.MoveDown(SelectedItem);

        private bool CanEdit(SavedTemplateViewModel item) => item != null;

        private void OnEdit(SavedTemplateViewModel item) => item.IsTextSelected = true;

        private bool CanDelete(SavedTemplateViewModel item) => item != null && item.DeleteCommand.CanExecute(null);

        private void OnDelete(SavedTemplateViewModel item) => item.DeleteCommand.Execute(null);
    }
}
