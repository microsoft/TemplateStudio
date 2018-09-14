using System;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Helpers
{
    public static class PivotItemExtensions
    {
        public static T GetPage<T>(this PivotItem pivotItem)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content is T element)
                {
                    return element;
                }
            }

            return default(T);
        }

        public static bool IsOfViewModelName(this PivotItem pivotItem, string viewModelName)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content is Page page)
                {
                    if (page.DataContext.GetType().FullName == viewModelName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}