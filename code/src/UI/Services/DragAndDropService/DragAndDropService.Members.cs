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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Services
{
    public partial class DragAndDropService<T>
    {
        private bool _canInitiateDrag;
        private bool _showDragAdornerLayer;
        private int _indexToSelect;
        private double _dragAdornerLayerOpacity;

        private T _itemUnderDragCursor;
        private ListView _listView;
        private Point _mouseDownPosition;
        private DragAdornerLayer _dragAdornerLayer;

        public event EventHandler<DragAndDropEventArgs<T>> ProcessDrop;

        private bool CanStartDragOperation
        {
            get
            {
                if (Mouse.LeftButton != MouseButtonState.Pressed
                    || !_canInitiateDrag
                    || _indexToSelect == -1
                    || !HasCursorLeftDragThreshold)
                {
                    return false;
                }

                return true;
            }
        }

        private bool HasCursorLeftDragThreshold
        {
            get
            {
                if (_indexToSelect < 0)
                {
                    return false;
                }

                var listViewItem = _listView.GetListViewItem(_indexToSelect);
                var bounds = VisualTreeHelper.GetDescendantBounds(listViewItem);
                var ptInItem = this._listView.TranslatePoint(_mouseDownPosition, listViewItem);

                int distanceScale = 3;

                double topOffset = Math.Abs(ptInItem.Y);
                double btmOffset = Math.Abs(bounds.Height - ptInItem.Y);
                double vertOffset = Math.Min(topOffset, btmOffset);
                double width = SystemParameters.MinimumHorizontalDragDistance * distanceScale;
                double height = Math.Min(SystemParameters.MinimumVerticalDragDistance, vertOffset) * distanceScale;
                Size szThreshold = new Size(width, height);

                Rect rect = new Rect(this._mouseDownPosition, szThreshold);
                rect.Offset(szThreshold.Width / -2, szThreshold.Height / -2);

                Point ptInListView = MouseUtilities.GetMousePosition(_listView);

                return !rect.Contains(ptInListView);
            }
        }

        private bool IsMouseOverScrollbar
        {
            get
            {
                var mousePosition = MouseUtilities.GetMousePosition(_listView);
                var result = VisualTreeHelper.HitTest(_listView, mousePosition);

                if (result == null)
                {
                    return false;
                }

                var dependencyObject = result.VisualHit;

                while (dependencyObject != null)
                {
                    if (dependencyObject is ScrollBar)
                    {
                        return true;
                    }

                    if (dependencyObject is Visual || dependencyObject is Visual3D)
                    {
                        dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                    }
                    else
                    {
                        dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
                    }
                }

                return false;
            }
        }

        private T ItemUnderDragCursor
        {
            get => _itemUnderDragCursor;
            set
            {
                if (_itemUnderDragCursor == value)
                {
                    return;
                }

                // 1º Previous item under the cursor.
                // 2º the new one.
                for (int step = 0; step < 2; ++step)
                {
                    if (step == 1)
                    {
                        _itemUnderDragCursor = value;
                    }

                    if (_itemUnderDragCursor != null)
                    {
                        var listViewItem = GetListViewItem(_itemUnderDragCursor);

                        if (listViewItem != null)
                        {
                            ListViewItemDragState.SetIsUnderDragCursor(listViewItem, step == 1);
                        }
                    }
                }
            }
        }

        bool ShowDragAdornerLayerResolved => _showDragAdornerLayer && _dragAdornerLayerOpacity > 0.0;
    }
}
