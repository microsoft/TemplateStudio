using Microsoft.Templates.UI.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
                if (this._indexToSelect < 0)
                {
                    return false;
                }

                var listViewItem = _listView.GetListViewItem(_indexToSelect);
                var bounds = VisualTreeHelper.GetDescendantBounds(listViewItem);
                var ptInItem = this._listView.TranslatePoint(_mouseDownPosition, listViewItem);

                double topOffset = Math.Abs(ptInItem.Y);
                double btmOffset = Math.Abs(bounds.Height - ptInItem.Y);
                double vertOffset = Math.Min(topOffset, btmOffset);

                double width = SystemParameters.MinimumHorizontalDragDistance * 2;
                double height = Math.Min(SystemParameters.MinimumVerticalDragDistance, vertOffset) * 2;
                Size szThreshold = new Size(width, height);

                Rect rect = new Rect(this._mouseDownPosition, szThreshold);
                rect.Offset(szThreshold.Width / -2, szThreshold.Height / -2);
                Point ptInListView = MouseUtilities.GetMousePosition(this._listView);
                return !rect.Contains(ptInListView);
            }
        }

        private bool IsMouseOverScrollbar
        {
            get
            {
                var mousePosition = MouseUtilities.GetMousePosition(this._listView);
                var result = VisualTreeHelper.HitTest(this._listView, mousePosition);
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
                if (this._itemUnderDragCursor == value)
                {
                    return;
                }

                // 1º Previous item under the cursor.
                // 2º the new one.
                for (int step = 0; step < 2; ++step)
                {
                    if (step == 1)
                    {
                        this._itemUnderDragCursor = value;
                    }

                    if (this._itemUnderDragCursor != null)
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
