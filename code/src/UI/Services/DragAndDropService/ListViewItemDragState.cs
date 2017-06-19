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
