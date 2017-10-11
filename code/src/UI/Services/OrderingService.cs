// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Services
{
    public static class OrderingService
    {
        public static StackPanel Panel { get; set; }

        private static Func<ObservableCollection<ObservableCollection<SavedTemplateViewModel>>> _getSavedPages;
        private static Action<SavedTemplateViewModel> _setHome;

        private static SavedTemplateViewModel _dragginItem;
        private static SavedTemplateViewModel _dropTarget;

        public static void Initialize(Func<ObservableCollection<ObservableCollection<SavedTemplateViewModel>>> getSavedPages, Action<SavedTemplateViewModel> setHome)
        {
            _getSavedPages = getSavedPages;
            _setHome = setHome;
        }

        public static void AddList(ObservableCollection<SavedTemplateViewModel> items, bool allowDragAndDrop)
        {
            if (Panel != null)
            {
                var listView = new ListView()
                {
                    ItemsSource = items,
                    Style = ResourceService.FindResource<Style>("SummaryListViewStyle"),
                    Tag = "AllowRename",
                    Focusable = false,
                    ItemTemplate = ResourceService.FindResource<DataTemplate>("ProjectTemplatesSummaryItemTemplate")
                };
                if (allowDragAndDrop)
                {
                    var service = new DragAndDropService<SavedTemplateViewModel>(listView);
                    service.ProcessDrop += Drop;
                }

                Panel.Children.Add(listView);
            }
        }

        public static void SetDropTarget(SavedTemplateViewModel savedTemplate) => _dropTarget = savedTemplate;

        public static bool SetDrag(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem == null)
            {
                _dragginItem = savedTemplate;
                return true;
            }

            return false;
        }

        public static bool SetDrop(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem != null && _dropTarget != null && _dragginItem.ItemName != _dropTarget.ItemName)
            {
                var newIndex = _getSavedPages().First().IndexOf(_dropTarget);
                var oldIndex = _getSavedPages().First().IndexOf(_dragginItem);
                Drop(null, new DragAndDropEventArgs<SavedTemplateViewModel>(null, _dropTarget, oldIndex, newIndex));
                _dragginItem = null;
                _dropTarget = null;
            }

            return false;
        }

        public static bool ClearDraggin()
        {
            if (_dragginItem != null)
            {
                _dragginItem = null;
                _dropTarget = null;
                return true;
            }

            return false;
        }

        private static void Drop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            if (_getSavedPages().Count > 0 && _getSavedPages().Count >= e.ItemData.GenGroup + 1)
            {
                var items = _getSavedPages()[e.ItemData.GenGroup];
                if (items.Count > 1)
                {
                    if (e.NewIndex == 0)
                    {
                        _setHome(e.ItemData);
                    }

                    if (e.OldIndex > -1)
                    {
                        _getSavedPages()[e.ItemData.GenGroup].Move(e.OldIndex, e.NewIndex);
                    }

                    _setHome(items.First());
                }
            }
        }
    }
}
