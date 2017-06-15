using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Services
{
    public static class ListViewItemDragState
    {
        static public readonly DependencyProperty IsBeingDraggedProperty = DependencyProperty.RegisterAttached("IsBeingDragged", typeof(bool), typeof(ListViewItemDragState), new UIPropertyMetadata(false));
        static public bool GetIsBeingDragged(ListViewItem item) => (bool)item.GetValue(IsBeingDraggedProperty);
        static public void SetIsBeingDragged(ListViewItem item, bool value) => item.SetValue(IsBeingDraggedProperty, value);

        static public bool GetIsUnderDragCursor(ListViewItem item) => (bool)item.GetValue(IsUnderDragCursorProperty);
        static public void SetIsUnderDragCursor(ListViewItem item, bool value) => item.SetValue(IsUnderDragCursorProperty, value);
        static public readonly DependencyProperty IsUnderDragCursorProperty = DependencyProperty.RegisterAttached("IsUnderDragCursor", typeof(bool), typeof(ListViewItemDragState), new UIPropertyMetadata(false));        
    }
}
