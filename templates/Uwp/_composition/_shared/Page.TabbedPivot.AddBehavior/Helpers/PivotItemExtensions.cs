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

        public static bool IsOfPageType(this PivotItem pivotItem, Type pageType)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content.GetType() == pageType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}