// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Services
{
    public partial class DragAndDropService<T>
        where T : class
    {
        public DragAndDropService(ListView listView, Func<T, T, bool> canDrop)
        {
            _canDrop = canDrop;
            _canInitiateDrag = false;
            _dragAdornerLayerOpacity = 0.7;
            _indexToSelect = -1;
            _showDragAdornerLayer = true;
            _listView = listView;

            InitializeListView();
        }

        private void InitializeListView()
        {
            if (!_listView.AllowDrop)
            {
                _listView.AllowDrop = true;
            }

            _listView.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            _listView.PreviewMouseMove += OnPreviewMouseMove;
            _listView.DragOver += OnDragOver;
            _listView.DragLeave += OnDragLeave;
            _listView.DragEnter += OnDragEnter;
            _listView.Drop += OnDrop;
        }

        public void UnsubscribeEventHandlers()
        {
            _listView.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            _listView.PreviewMouseMove -= OnPreviewMouseMove;
            _listView.DragOver -= OnDragOver;
            _listView.DragLeave -= OnDragLeave;
            _listView.DragEnter -= OnDragEnter;
            _listView.Drop -= OnDrop;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseOverScrollbar)
            {
                _canInitiateDrag = false;
            }
            else
            {
                int index = GetIndexUnderDragCursor();
                _canInitiateDrag = index > -1;

                if (_canInitiateDrag)
                {
                    _mouseDownPosition = MouseUtilities.GetMousePosition(_listView);
                    _indexToSelect = index;

                    ConfigureAllowDropToItems(index);
                }
                else
                {
                    _mouseDownPosition = new Point(-10000, -10000);
                    _indexToSelect = -1;
                }
            }
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (CanStartDragOperation)
            {
                if (_listView.SelectedIndex != _indexToSelect)
                {
                    _listView.SelectedIndex = _indexToSelect;
                }

                if (_listView.SelectedItem != null)
                {
                    var itemToDrag = _listView.GetCurrentListViewItem();
                    if (itemToDrag != null)
                    {
                        var adornerLayer = ShowDragAdornerLayerResolved ? InitializeAdornerLayer(itemToDrag) : null;

                        InitializeDragOperation(itemToDrag);
                        PerformDragOperation();
                        FinishDragOperation(itemToDrag, adornerLayer);
                    }
                }
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

            if (ShowDragAdornerLayerResolved)
            {
                UpdateDragAdornerLocation();
            }

            int index = GetIndexUnderDragCursor();
            ItemUnderDragCursor = index < 0 ? null : _listView.Items[index] as T;
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if (!IsMouseOver(_listView))
            {
                if (ItemUnderDragCursor != null)
                {
                    ItemUnderDragCursor = null;
                }

                if (_dragAdornerLayer != null)
                {
                    _dragAdornerLayer.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (_dragAdornerLayer != null && _dragAdornerLayer.Visibility != Visibility.Visible)
            {
                UpdateDragAdornerLocation();
                _dragAdornerLayer.Visibility = Visibility.Visible;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (ItemUnderDragCursor != null)
            {
                ItemUnderDragCursor = null;
            }

            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(typeof(T)))
            {
                // Get the data dropped object.
                if (e.Data.GetData(typeof(T)) is T data)
                {
                    if (_listView.ItemsSource is ObservableCollection<T> itemsSource)
                    {
                        int oldIndex = itemsSource.IndexOf(data);
                        int newIndex = GetIndexUnderDragCursor();

                        if (newIndex < 0)
                        {
                            if (itemsSource.Count == 0)
                            {
                                newIndex = 0;
                            }
                            else if (oldIndex < 0)
                            {
                                newIndex = itemsSource.Count;
                            }
                            else
                            {
                                return;
                            }
                        }

                        if (oldIndex != newIndex)
                        {
                            if (ProcessDrop != null)
                            {
                                // Let the wizard process the drop.
                                DragAndDropEventArgs<T> args = new DragAndDropEventArgs<T>(itemsSource, data, oldIndex, newIndex, e.AllowedEffects);
                                ProcessDrop(this, args);
                                e.Effects = args.Effects;
                            }
                            else
                            {
                                if (oldIndex > -1)
                                {
                                    itemsSource.Move(oldIndex, newIndex);
                                }
                                else
                                {
                                    itemsSource.Insert(newIndex, data);
                                }

                                e.Effects = DragDropEffects.Move;
                            }
                        }
                    }
                }
            }
        }

        private void FinishDragOperation(ListViewItem draggedItem, AdornerLayer adornerLayer)
        {
            ListViewItemDragState.SetIsBeingDragged(draggedItem, false);

            if (ItemUnderDragCursor != null)
            {
                ItemUnderDragCursor = null;
            }

            if (adornerLayer != null)
            {
                adornerLayer.Remove(_dragAdornerLayer);
                _dragAdornerLayer = null;
            }
        }

        private int GetIndexUnderDragCursor()
        {
            int index = -1;
            for (int i = 0; i < _listView.Items.Count; ++i)
            {
                var listViewItem = _listView.GetListViewItem(i);
                if (IsMouseOver(listViewItem))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private ListViewItem GetListViewItem(T dataItem)
        {
            if (_listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return null;
            }

            return _listView.ItemContainerGenerator.ContainerFromItem(dataItem) as ListViewItem;
        }

        private AdornerLayer InitializeAdornerLayer(ListViewItem itemToDrag)
        {
            var brush = new VisualBrush(itemToDrag);
            _dragAdornerLayer = new DragAdornerLayer(_listView, itemToDrag.RenderSize, brush)
            {
                Opacity = _dragAdornerLayerOpacity,
            };

            var adornerLayer = AdornerLayer.GetAdornerLayer(_listView);
            adornerLayer.Add(_dragAdornerLayer);
            _mouseDownPosition = MouseUtilities.GetMousePosition(_listView);

            return adornerLayer;
        }

        private void InitializeDragOperation(ListViewItem itemToDrag)
        {
            _canInitiateDrag = false;
            ListViewItemDragState.SetIsBeingDragged(itemToDrag, true);
        }

        private bool IsMouseOver(Visual target)
        {
            if (target == null)
            {
                return false;
            }

            var descendantBounds = VisualTreeHelper.GetDescendantBounds(target);
            var mousePosition = MouseUtilities.GetMousePosition(target);
            return descendantBounds.Contains(mousePosition);
        }

        private void PerformDragOperation()
        {
            var selectedItem = _listView.SelectedItem as T;
            var allowedEffects = DragDropEffects.Move | DragDropEffects.Move | DragDropEffects.Link;
            if (DragDrop.DoDragDrop(_listView, selectedItem, allowedEffects) != DragDropEffects.None)
            {
                // selectedItem was dropped into a new location, now make it the new selected item.
                _listView.SelectedItem = selectedItem;
            }
        }

        private void UpdateDragAdornerLocation()
        {
            if (_dragAdornerLayer != null)
            {
                var mousePosition = MouseUtilities.GetMousePosition(_listView);

                double left = mousePosition.X - _mouseDownPosition.X;

                var itemBeingDragged = _listView.GetListViewItem(_indexToSelect);
                var itemLoc = itemBeingDragged.TranslatePoint(new Point(0, 0), _listView);
                double top = itemLoc.Y + mousePosition.Y - _mouseDownPosition.Y;

                _dragAdornerLayer.SetOffsets(left, top);
            }
        }

        private void ConfigureAllowDropToItems(int dragItemIndex)
        {
            var dragItem = _listView.Items[dragItemIndex] as T;

            foreach (T dropItem in _listView.Items)
            {
                var listViewItem = GetListViewItem(dropItem);
                listViewItem.AllowDrop = _canDrop(dragItem, dropItem);
            }
        }
    }
}
