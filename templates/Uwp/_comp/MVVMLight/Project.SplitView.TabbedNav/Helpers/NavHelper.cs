using System;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.Helpers
{
    public class NavHelper
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
            DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavHelper), new PropertyMetadata(null));
    }
}
