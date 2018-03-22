using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Helpers
{
    public class NavigationViewItemExtensions
    {
        public static string GetPageKey(NavigationViewItem obj)
        {
            return (string)obj.GetValue(PageKeyProperty);
        }

        public static void SetPageKey(NavigationViewItem obj, string value)
        {
            obj.SetValue(PageKeyProperty, value);
        }

        public static readonly DependencyProperty PageKeyProperty =
          DependencyProperty.RegisterAttached("PageKey", typeof(string),
            typeof(NavigationViewItemExtensions), new PropertyMetadata(null));
    }
}
