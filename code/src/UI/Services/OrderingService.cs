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
        public Style ListViewStyle { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        public Func<ObservableCollection<ObservableCollection<SavedTemplateViewModel>>> GetData { get; set; }
        public Action<SavedTemplateViewModel> SetHomePage { get; set; }

        private SavedTemplateViewModel _currentDragginTemplate;
        private SavedTemplateViewModel _dropTargetTemplate;

        public OrderingService()
        {
        }

        public void DefineDragAndDrop(ObservableCollection<SavedTemplateViewModel> items, bool allowDragAndDrop)
        {
            var listView = new ListView()
            {
                ItemsSource = items,
                Style = ListViewStyle,
                Tag = "AllowRename",
                Focusable = false,
                ItemTemplate = ItemTemplate
            };
            if (allowDragAndDrop)
            {
                var service = new DragAndDropService<SavedTemplateViewModel>(listView);
                service.ProcessDrop += DropTemplate;
            }
            Panel.Children.Add(listView);
        }

        public void SavedTemplateGotFocus(SavedTemplateViewModel savedTemplate)
        {
            _dropTargetTemplate = savedTemplate;
        }

        public bool SavedTemplateSetDrag(SavedTemplateViewModel savedTemplate)
        {
            if (_currentDragginTemplate == null)
            {
                _currentDragginTemplate = savedTemplate;
                return true;
            }
            return false;
        }

        public bool SavedTemplateSetDrop(SavedTemplateViewModel savedTemplate)
        {
            if (_currentDragginTemplate != null && _dropTargetTemplate != null && _currentDragginTemplate.ItemName != _dropTargetTemplate.ItemName)
            {
                var savedPages = GetData();
                var newIndex = savedPages.First().IndexOf(_dropTargetTemplate);
                var oldIndex = savedPages.First().IndexOf(_currentDragginTemplate);
                DropTemplate(null, new DragAndDropEventArgs<SavedTemplateViewModel>(null, _dropTargetTemplate, oldIndex, newIndex));
                _currentDragginTemplate = null;
                _dropTargetTemplate = null;
            }
            return false;
        }

        public bool ClearCurrentDragginTemplate()
        {
            if (_currentDragginTemplate != null)
            {
                _currentDragginTemplate = null;
                _dropTargetTemplate = null;
                return true;
            }
            return false;
        }

        public void DropTemplate(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            var savedPages = GetData();
            if (savedPages.Count > 0 && savedPages.Count >= e.ItemData.GenGroup + 1)
            {
                var items = savedPages[e.ItemData.GenGroup];
                if (items.Count > 1)
                {
                    if (e.NewIndex == 0)
                    {
                        SetHomePage(e.ItemData);
                    }
                    if (e.OldIndex > -1)
                    {
                        savedPages[e.ItemData.GenGroup].Move(e.OldIndex, e.NewIndex);
                    }
                    SetHomePage(items.First());
                }
            }
        }
    }
}
