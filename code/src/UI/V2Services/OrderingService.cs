// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2ViewModels.NewProject;

namespace Microsoft.Templates.UI.V2Services
{
    public static class OrderingService
    {
        private static bool _isFocused;
        private static SavedTemplateViewModel _selected;
        private static SavedTemplateViewModel _dragginItem;
        private static SavedTemplateViewModel _dropTarget;

        private static ObservableCollection<SavedTemplateViewModel> _pages
        {
            get => MainViewModel.Instance.UserSelection.Pages;
        }

        public static void Initialize(ListView listView)
        {
            var service = new DragAndDropService<SavedTemplateViewModel>(listView, AreCompatibleItems);
            service.ProcessDrop += OnDrop;
            listView.SelectionChanged += OnSelectionChanged;
            listView.GotFocus += OnGotFocus;
            listView.LostFocus += OnLostFocus;
        }

        public static bool SetDrag(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem == null)
            {
                _dragginItem = savedTemplate;
                savedTemplate.IsDragging = true;
                return true;
            }

            return false;
        }

        public static bool SetDrop(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem != null && _dropTarget != null && !_dragginItem.Equals(_dropTarget))
            {
                var newIndex = _pages.IndexOf(_dropTarget);
                var oldIndex = _pages.IndexOf(_dragginItem);
                OnDrop(null, new DragAndDropEventArgs<SavedTemplateViewModel>(_pages, _dropTarget, oldIndex, newIndex));
                _dragginItem = null;
                _dropTarget = null;
                return true;
            }

            return false;
        }

        private static bool AreCompatibleItems(SavedTemplateViewModel startItem, SavedTemplateViewModel endItem)
        {
            return startItem.GenGroup == endItem.GenGroup;
        }

        private static void OnDrop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            if (e.OldIndex > -1)
            {
                e.Items.Move(e.OldIndex, e.NewIndex);
                EventService.Instance.RaiseOnReorderTemplate();
            }
        }

        //private static void OnKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter && _isFocused && _selected != null)
        //    {
        //        if (SetDrag(_selected))
        //        {
        //            _selected = null;
        //        }
        //        else
        //        {
        //            _dropTarget = _selected;
        //            if (SetDrop(_selected))
        //            {
        //                _selected = null;
        //            }
        //        }
        //    }
        //}

        private static void OnLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _isFocused = false;
        }

        private static void OnGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _isFocused = true;
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                _selected = listView.SelectedItem as SavedTemplateViewModel;
            }
        }
    }
}
