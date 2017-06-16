using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Services
{
    public static class ListViewItemDragState
    {
        public static readonly DependencyProperty IsBeingDraggedProperty = DependencyProperty.RegisterAttached("IsBeingDragged", typeof(bool), typeof(ListViewItemDragState), new UIPropertyMetadata(false));
        public static bool GetIsBeingDragged(ListViewItem item) => (bool)item.GetValue(IsBeingDraggedProperty);
        public static void SetIsBeingDragged(ListViewItem item, bool value) => item.SetValue(IsBeingDraggedProperty, value);

        public static bool GetIsUnderDragCursor(ListViewItem item) => (bool)item.GetValue(IsUnderDragCursorProperty);
        public static void SetIsUnderDragCursor(ListViewItem item, bool value) => item.SetValue(IsUnderDragCursorProperty, value);
        public static readonly DependencyProperty IsUnderDragCursorProperty = DependencyProperty.RegisterAttached("IsUnderDragCursor", typeof(bool), typeof(ListViewItemDragState), new UIPropertyMetadata(false));
    }
}
