using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Helpers
{
    public class NavigationViewItemExtensions
    {
        public static Type GetPageType(NavigationViewItem obj)
        {
            return (Type)obj.GetValue(PageTypeProperty);
        }

        public static void SetPageType(NavigationViewItem obj, Type value)
        {
            obj.SetValue(PageTypeProperty, value);
        }

        public static readonly DependencyProperty PageTypeProperty =
          DependencyProperty.RegisterAttached("PageType", typeof(Type),
            typeof(NavigationViewItemExtensions), new PropertyMetadata(null));
    }
}
