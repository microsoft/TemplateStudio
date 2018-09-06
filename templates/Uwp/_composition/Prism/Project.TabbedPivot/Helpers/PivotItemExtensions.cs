using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Helpers
{
    public static class PivotItemExtensions
    {
        public static T GetPage<T>(this PivotItem pivotItem)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.DataContext is T element)
                    {
                        return element;
                    }
                }
            }

            return default(T);
        }

        public static bool IsOfPageType(this PivotItem pivotItem, string pageToken)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content.GetType().Name == $"{pageToken}Page")
                {
                    return true;
                }
            }
            return false;
        }
    }
}