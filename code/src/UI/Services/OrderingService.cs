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
    public class OrderingService
    {
        public StackPanel Panel { get; set; }

        private SavedTemplateViewModel _dragginItem;
        private SavedTemplateViewModel _dropTarget;

        public void AddList(ObservableCollection<SavedTemplateViewModel> items, bool allowDragAndDrop)
        {
            if (Panel != null)
            {
                var listView = new ListView()
                {
                    ItemsSource = items,
                    Style = MainViewModel.Current.FindResource<Style>("SummaryListViewStyle"),
                    Tag = "AllowRename",
                    Focusable = false,
                    ItemTemplate = MainViewModel.Current.FindResource<DataTemplate>("ProjectTemplatesSummaryItemTemplate")
                };
                if (allowDragAndDrop)
                {
                    var service = new DragAndDropService<SavedTemplateViewModel>(listView);
                    service.ProcessDrop += Drop;
                }
                Panel.Children.Add(listView);
            }
        }

        public void SetDropTarget(SavedTemplateViewModel savedTemplate) => _dropTarget = savedTemplate;

        public bool SetDrag(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem == null)
            {
                _dragginItem = savedTemplate;
                return true;
            }
            return false;
        }

        public bool SetDrop(SavedTemplateViewModel savedTemplate)
        {
            if (_dragginItem != null && _dropTarget != null && _dragginItem.ItemName != _dropTarget.ItemName)
            {
                var savedPages = MainViewModel.Current.ProjectTemplates.SavedPages;
                var newIndex = savedPages.First().IndexOf(_dropTarget);
                var oldIndex = savedPages.First().IndexOf(_dragginItem);
                Drop(null, new DragAndDropEventArgs<SavedTemplateViewModel>(null, _dropTarget, oldIndex, newIndex));
                _dragginItem = null;
                _dropTarget = null;
            }
            return false;
        }

        public bool ClearDraggin()
        {
            if (_dragginItem != null)
            {
                _dragginItem = null;
                _dropTarget = null;
                return true;
            }
            return false;
        }

        private void Drop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            var savedPages = MainViewModel.Current.ProjectTemplates.SavedPages;
            if (savedPages.Count > 0 && savedPages.Count >= e.ItemData.GenGroup + 1)
            {
                var items = savedPages[e.ItemData.GenGroup];
                if (items.Count > 1)
                {
                    if (e.NewIndex == 0)
                    {
                        MainViewModel.Current.ProjectTemplates.SetHomePage(e.ItemData);
                    }
                    if (e.OldIndex > -1)
                    {
                        savedPages[e.ItemData.GenGroup].Move(e.OldIndex, e.NewIndex);
                    }
                    MainViewModel.Current.ProjectTemplates.SetHomePage(items.First());
                }
            }
        }
    }
}
