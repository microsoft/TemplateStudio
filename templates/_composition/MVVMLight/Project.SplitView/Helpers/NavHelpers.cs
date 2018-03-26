using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Helpers
{
    public class NavHelpers
    {
        public static string GetNavigateTo(NavigationViewItem item)
        {
            return (string)item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(NavigationViewItem item, string value)
        {
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
          DependencyProperty.RegisterAttached("NavigateTo", typeof(string),
            typeof(NavHelpers), new PropertyMetadata(null));
    }
}
